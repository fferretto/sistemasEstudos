/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_CONSCARGA_PGTO          Versao 1.0                                    */         
/* CRIAÇÃO : ALEX 05/07/2019                                                  */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/* CONSULTA AS CARGAS A PAGAR PARA GERAR BOLETO                               */    
/* PARAMETROS:                                                                */     
/*                                                                            */    
/*                                                                            */      
/*----------------------------------------------------------------------------*/  

CREATE PROCEDURE [dbo].[PROC_PAGNET_CONSCARGA_PGTO]
                        @CODCLI         INT         = 0
                       ,@CODSUBREDE     INT 
                       ,@DATINI         DATETIME 
                       ,@DATFIM         DATETIME 
                       ,@NUMCARGA       INT
AS

BEGIN

-------------TESTE

--DECLARE  @CODCLI        INT
--        ,@CODSUBREDE    INT 
--        ,@DATINI        DATETIME
--        ,@DATFIM        DATETIME
--        ,@NUMCARGA      INT

--SET @CODCLI = 0
--SET @DATINI = '20190525'
--SET @DATFIM = '20190602'
--SET @CODSUBREDE = 1
--SET @NUMCARGA = 0


------------------

DECLARE @NOVACARGAATIVA CHAR(1)

    SELECT @NOVACARGAATIVA = VAL FROM PARAMVA WHERE ID0 = 'NOVACARGAATIVA'

        DECLARE @TABTEMP TABLE 
        (
            CODCLI         INT,
            CNPJ           CHAR(14),
            NMCLIENTE      NVARCHAR(255),
            NUMCARGA       INT ,
            DTAUTORIZ      DATETIME,
            DTCARGA        DATETIME,
            QUANT          INT,
            VALCARGA       NUMERIC(15, 2),
            VAL2AVIA       NUMERIC(15, 2),
            VALTAXA        NUMERIC(15, 2),
            VALTAXACARREG  NUMERIC(15, 2),
            TOTAL          NUMERIC(15, 2),
            SALDOCONTAUTIL NUMERIC(15,2)
        )


    DECLARE @TABCARGASEQ TABLE 
    (
      CODCLI    INT,
      NUMCARG_1 INT
    )


    SET NOCOUNT ON

    --Seleciona as cargas aguardando pagamento

    IF @NOVACARGAATIVA = 'S' 
    BEGIN
        INSERT INTO @TABTEMP (CODCLI, CNPJ,NMCLIENTE, NUMCARGA, DTAUTORIZ, DTCARGA, QUANT, VALCARGA, VAL2AVIA, VALTAXA, VALTAXACARREG, TOTAL, SALDOCONTAUTIL)
        SELECT C.CODCLI
              ,C.CGC
              ,C.NOMCLI
              ,G.NUMCARG_VA
              ,G.DTAUTORIZ
              ,NULL
              ,ISNULL(COUNT(CS.CPF), 0) AS QUANTUSU
              ,ISNULL(SUM(CS.VCARGAUTO), 0) AS VALOR
              ,(SELECT ISNULL(SUM(T.VALTRA), 0.00) 
                  FROM TRANSACVA T with (nolock) 
                 WHERE T.CODCLI = C.CODCLI  
                   AND T.CODRTA = 'V' 
                   AND T.TIPTRA = 999007 
                   AND ((T.NUMCARG_VA = G.NUMCARG_VA) OR (T.NUMCARG_VA IS NULL AND T.TIPTRA = 999007 AND T.DATTRA <= G.DTAUTORIZ)) 
                   AND T.DTCARGA is null 
                   AND (select CLIENTEVA.COB2AV FROM CLIENTEVA with (nolock) WHERE CLIENTEVA.CODCLI = C.CODCLI) = 1
               ) AS VAL2AVIA
              ,CASE C.TIPOTAXSER WHEN 'P' THEN CAST((SUM(CS.VCARGAUTO) * C.TAXSER_VA) / 100.00 AS NUMERIC(15,2)) ELSE COUNT(CS.CPF) * C.TAXSER_VA END AS VALTAXASER
              ,(COUNT(CS.CPF) * C.TAXADM_VA) AS VALTAXACARREG
              ,NULL AS TOTAL
              ,ISNULL(G.SALDOCONTABLOQ, 0.00) AS SALDOCONTABLOQ
         FROM CLIENTEVA C with (nolock)
        INNER 
         JOIN CARGAC G with (nolock) ON C.CODCLI = G.CODCLI
       --LEFT JOIN USUARIOVA CS with (nolock) ON CS.CODCLI = G.CODCLI AND CS.ULTCARGVA = G.NUMCARG_VA
         LEFT 
         JOIN CARGA_SOLICITADA CS with (nolock) ON CS.CODCLI = G.CODCLI AND CS.NUMCARG_VA = G.NUMCARG_VA
        WHERE C.NSUCARGA IS NOT NULL 
          AND C.NSUCARGA <> 0 
          AND G.DTCARGA IS NULL 
          AND ((@NUMCARGA = 0) OR G.NUMCARG_VA = @NUMCARGA)
          AND C.PGTOANTECIPADO = 'S' 
          AND G.DTPGTO IS NULL 
          AND ((@CODCLI = 0) OR (C.CODCLI = @CODCLI))
          AND C.CODSUBREDE = @CODSUBREDE
          AND CONVERT(DATETIME,ROUND(CONVERT(FLOAT,G.DTAUTORIZ),0,1)) >= @DATINI
          AND CONVERT(DATETIME,ROUND(CONVERT(FLOAT,G.DTAUTORIZ),0,1)) <= @DATFIM
     GROUP BY C.CODCLI
             ,C.CGC
             ,C.NOMCLI
             ,G.NUMCARG_VA
             ,G.DTAUTORIZ
             ,G.DTPROG
             ,C.TIPOTAXSER
             ,C.TAXSER_VA
             ,C.TAXADM_VA
             ,G.SALDOCONTABLOQ
     ORDER BY C.CODCLI
             ,G.NUMCARG_VA
    END
    ELSE
    BEGIN
        INSERT INTO @TABTEMP (CODCLI, CNPJ, NMCLIENTE, NUMCARGA, DTAUTORIZ, DTCARGA, QUANT, VALCARGA, VAL2AVIA, VALTAXA, VALTAXACARREG, TOTAL, SALDOCONTAUTIL)
        SELECT C.CODCLI
              ,C.CGC
              ,C.NOMCLI
              ,G.NUMCARG_VA
              ,G.DTAUTORIZ
              ,NULL
              ,ISNULL(COUNT(CS.CPF), 0) AS QUANTUSU
              ,ISNULL(SUM(CS.VCARGAUTO), 0) AS VALOR
              ,(SELECT ISNULL(SUM(T.VALTRA), 0.00) 
                  FROM TRANSACVA T WITH (NOLOCK) 
                 WHERE T.CODCLI = C.CODCLI  
                   AND T.CODRTA = 'V' 
                   AND T.TIPTRA = 999007 
                   AND ((T.NUMCARG_VA = G.NUMCARG_VA) OR (T.NUMCARG_VA IS NULL AND T.TIPTRA = 999007 AND T.DATTRA <= G.DTAUTORIZ)) 
                   AND T.DTCARGA IS NULL 
                   AND (SELECT CLIENTEVA.COB2AV FROM CLIENTEVA WITH (NOLOCK) WHERE CLIENTEVA.CODCLI = C.CODCLI) = 1
               ) AS VAL2AVIA
              ,CASE C.TIPOTAXSER WHEN 'P' THEN CAST((SUM(CS.VCARGAUTO) * C.TAXSER_VA) / 100.00 AS NUMERIC(15,2)) ELSE COUNT(CS.CPF) * C.TAXSER_VA END AS VALTAXASER
              ,(COUNT(CS.CPF) * C.TAXADM_VA) AS VALTAXACARREG
              ,NULL AS TOTAL
              ,ISNULL(G.SALDOCONTABLOQ, 0.00) AS SALDOCONTABLOQ
          FROM CLIENTEVA C WITH (NOLOCK)
    INNER JOIN CARGAC G WITH (NOLOCK) ON C.CODCLI = G.CODCLI
     LEFT JOIN USUARIOVA CS WITH (NOLOCK) ON CS.CODCLI = G.CODCLI AND CS.ULTCARGVA = G.NUMCARG_VA
        --LEFT JOIN CARGA_SOLICITADA CS WITH (NOLOCK) ON CS.CODCLI = G.CODCLI AND CS.NUMCARG_VA = G.NUMCARG_VA
        WHERE C.NSUCARGA IS NOT NULL 
          AND C.NSUCARGA <> 0 
          AND G.DTCARGA IS NULL 
          AND C.PGTOANTECIPADO = 'S' 
          AND G.DTPGTO IS NULL 
          AND ((@NUMCARGA = 0) OR G.NUMCARG_VA = @NUMCARGA)
          AND ((@CODCLI = 0) OR (C.CODCLI = @CODCLI))
          AND C.CODSUBREDE = @CODSUBREDE
          AND CONVERT(DATETIME,ROUND(CONVERT(FLOAT,G.DTAUTORIZ),0,1)) >= @DATINI
          AND CONVERT(DATETIME,ROUND(CONVERT(FLOAT,G.DTAUTORIZ),0,1)) <= @DATFIM
     GROUP BY C.CODCLI
             ,C.CGC
             ,C.NOMCLI
             ,G.NUMCARG_VA
             ,G.DTAUTORIZ
             ,G.DTPROG
             ,C.TIPOTAXSER
             ,C.TAXSER_VA
             ,C.TAXADM_VA
             ,G.SALDOCONTABLOQ
     ORDER BY C.CODCLI
             ,G.NUMCARG_VA
    END

    --ZER TAXAS 2A VIA REPEDITAS PARA CARGAS SEQUENCIAIS
    INSERT INTO @TABCARGASEQ (NUMCARG_1, CODCLI)
    SELECT MIN(NUMCARGA)
         ,CODCLI 
     FROM @TABTEMP
 GROUP BY CODCLI
   HAVING COUNT(CODCLI) > 1

    UPDATE T 
       SET T.VAL2AVIA = 0 
      FROM @TABTEMP T
    INNER JOIN @TABCARGASEQ S ON S.CODCLI = T.CODCLI AND T.NUMCARGA > S.NUMCARG_1 

    --Acerta o total
    UPDATE @TABTEMP SET TOTAL = VALCARGA + VALTAXA + VALTAXACARREG


    --Seleciona as cargas efetuadas
    INSERT INTO @TABTEMP (CODCLI, CNPJ, NMCLIENTE, NUMCARGA, DTAUTORIZ, DTCARGA, QUANT, VALCARGA, VAL2AVIA, VALTAXA, VALTAXACARREG, TOTAL, SALDOCONTAUTIL)
    SELECT C.CODCLI
          ,C.CGC
          ,C.NOMCLI
          ,G.NUMCARG_VA
          ,G.DTAUTORIZ
          ,G.DTCARGA
          ,G.QUANT_CARREGADOS
          ,(G.VALOR - G.VAL2AVIA), G.VAL2AVIA, G.TAXSER, G.TAXADM, (G.VALOR + G.TAXSER + G.TAXADM)
          ,G.SALDOCONTAUTIL /*G.PRAPAG, G.DTCARGA + G.PRAPAG,*/    
      FROM CARGAC G
INNER JOIN CLIENTEVA C ON C.CODCLI = G.CODCLI
     WHERE ((G.CODCLI = @CODCLI) OR (@CODCLI = 0))
       AND G.DTCARGA IS NOT NULL 
       AND ((@NUMCARGA = 0) OR G.NUMCARG_VA = @NUMCARGA)
       AND C.CODSUBREDE = @CODSUBREDE
       AND G.DTCARGA >= @DATINI 
       AND G.DTCARGA <= @DATFIM
    --and ((G.DTCARGA >= @DATINI and G.DTCARGA <= @DATFIM) or
    --(G.DTCARGA = 0 and G.DTAUTORIZ >= @DATINI and G.DTAUTORIZ <= @DATFIM))

    --REMOVE TODAS AS SOLICITAÇÕE DE CARGA QUE JÁ FORAM PROCESSADAS NO PAGNET
    --DELETE T
    --FROM @TABTEMP T
    --    ,PAGNET_EMISSAOFATURAMENTO EB
    --WHERE EB.CODCLIENTE             = T.CODCLI 
    --  AND EB.NUMCONTROLETAB     = T.NUMCARGA
    --  AND EB.TIPOFATURAMENTO            = 'CARGA'
    --  AND EB.STATUS             IN ('A_LIQUIDAR', 'EM_ABERTO', 'REGISTRADO', 'LIQUIDADO', 'BAIXADO')
    --  AND EB.DATVENCIMENTO       = t.DTCARGA
    --  AND PGF.CODEMPRESA      = (SELECT E.CODEMPRESA FROM PAGNET_CADEMPRESA E WHERE E.CODSUBREDE = T. )



      SELECT T.CODCLI
            ,T.CNPJ
            ,T.NMCLIENTE
            ,T.NUMCARGA
            ,T.DTAUTORIZ
            ,T.DTCARGA
            ,T.QUANT
            ,T.VALCARGA
            ,T.VAL2AVIA
            ,T.VALTAXA
            ,T.VALTAXACARREG
            ,T.TOTAL
            ,T.SALDOCONTAUTIL
            --,CASE WHEN EB.DESCRICAOOCORRENCIARETBOL IS NULL THEN '' ELSE EB.DESCRICAOOCORRENCIARETBOL END AS DESCRICAOOCORRENCIARETBOL
       FROM @TABTEMP T
  --LEFT JOIN PAGNET_EMISSAOBOLETO EB ON EB.CODCLI            = T.CODCLI 
  --                                 AND EB.NUMCONTROLETAB    = T.NUMCARGA 
  --                                 AND EB.SISTEMA           = 1 
  --                                 AND EB.DTREFERENCIA      = t.DTAUTORIZ




    SET NOCOUNT OFF

END
