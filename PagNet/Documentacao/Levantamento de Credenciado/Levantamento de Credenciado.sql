   
       DECLARE @TABTEMP TABLE
       (
           CODCRE         INT
          ,CNPJ           VARCHAR(20)
          ,RAZSOC         VARCHAR(250)
          ,CTABCO         VARCHAR(60)
          ,BANCO          VARCHAR(60)
          ,AGENCIA        VARCHAR(60)
          ,CONTACORRENTE  VARCHAR(60)
          ,CGCCTABCO      VARCHAR(60)
          ,RAZSOC_CTABCO  VARCHAR(60)
       )
   

      INSERT 
        INTO @TABTEMP
      select C.CODCRE 
            ,C.CNPJ
            ,C.RAZSOC
            ,C.CTABCO
            ,SUBSTRING(C.CTABCO, 1, 4)                as Banco
            ,SUBSTRING(C.CTABCO, 6, 6)                as AGENCIA
            ,SUBSTRING(C.CTABCO, 13, LEN(C.CTABCO))   as [conta corrente]
            ,C.CGCCTABCO
            ,C.RAZSOC_CTABCO
        from FECHCRED     FC
            ,VCREDENCIADO C
            --,TRANSACAO    TRA
       WHERE FC.CODCEN      = C.CODCRE
         --AND TRA.CODCRE     = FC.CODCRE
         --AND TRA.NUMFECCRE  = FC.NUMFECCRE
         --AND TRA.CODSUBREDE = 2
         AND FC.DATPGTO     >= DATEADD(MONTH, -2, GETDATE())
         AND C.STA          != '02'
    GROUP BY C.CODCRE 
            ,C.CNPJ
            ,C.RAZSOC
            ,C.CTABCO
            ,C.CGCCTABCO
            ,C.RAZSOC_CTABCO

       INSERT 
         INTO @TABTEMP
      SELECT C.CODCRE 
            ,C.CNPJ
            ,C.RAZSOC
            ,C.CTABCO_VA
            ,SUBSTRING(C.CTABCO_VA, 1, 4)                  as Banco
            ,SUBSTRING(C.CTABCO_VA, 6, 6)                   as AGENCIA
            ,SUBSTRING(C.CTABCO_VA, 13, LEN(C.CTABCO_VA))   as [conta corrente]
            ,C.CGCCTABCO_VA
            ,C.RAZSOC_CTABCO_VA
        FROM FECHCREDVA     FC
            ,VCREDENCIADO   C
            --,TRANSACVA      TRA
       WHERE FC.CODCEN      = C.CODCRE
         --AND TRA.CODCRE     = FC.CODCRE
         --AND TRA.NUMFECCRE  = FC.NUMFECCRE
         --AND TRA.CODSUBREDE = 2
         AND FC.DATPGTO     >= DATEADD(MONTH, -2, GETDATE())
         AND C.STA          != '02'
    GROUP BY C.CODCRE 
            ,C.CNPJ
            ,C.RAZSOC
            ,C.CTABCO_VA
            ,C.CGCCTABCO_VA
            ,C.RAZSOC_CTABCO_VA


            
    update @TABTEMP
    set AGENCIA = REPLACE(AGENCIA, '-', '')
       ,CONTACORRENTE = REPLACE(CONTACORRENTE, '-', '')
       ,BANCO = REPLACE(BANCO, '-', '')

       --SELECT * FROM @TABTEMP

       SELECT DISTINCT
              CODCRE                                                            AS [Código do Credenciado]
             ,dbo.formatarCNPJCPF(CNPJ)                                         AS [CPF/CNPJ]
             ,RAZSOC                                                            AS [Razão Social]
             ,CASE WHEN LTRIM(RTRIM((BANCO))) != '' 
                    THEN SUBSTRING(BANCO, 1, 4)
                    ELSE '0'
              END                                                               AS [Banco]
             ,CASE WHEN LTRIM(RTRIM((AGENCIA))) != '' 
                    THEN SUBSTRING(AGENCIA, 1, LEN(AGENCIA) - 1)
                    ELSE '0'
              END                                                               AS [Agencia]
             ,CASE WHEN LTRIM(RTRIM((AGENCIA))) != '' 
                    THEN SUBSTRING(AGENCIA, 6, 1)
                    ELSE '0'
              END                                                               AS [DG Agencia]
             ,CASE WHEN LTRIM(RTRIM((CONTACORRENTE))) != '' 
                    THEN SUBSTRING(CONTACORRENTE, 1, LEN(CONTACORRENTE) - 1)
                    ELSE '0'
              END                                                               AS [Conta Corrente]
             ,CASE WHEN LTRIM(RTRIM((CONTACORRENTE))) != '' 
                    THEN SUBSTRING(CONTACORRENTE, LEN(CONTACORRENTE), 1)
                    ELSE '0'
              END                                                               AS [DG Conta Corrente] 
             ,CASE WHEN CGCCTABCO IS NULL THEN '' ELSE CGCCTABCO END            AS [CPF/CNPJ da Conta] 
             ,CASE WHEN RAZSOC_CTABCO IS NULL THEN '' ELSE RAZSOC_CTABCO END    AS [Razão Social do Credenciado Dententor da Conta]            
        FROM @TABTEMP
    ORDER BY BANCO
    