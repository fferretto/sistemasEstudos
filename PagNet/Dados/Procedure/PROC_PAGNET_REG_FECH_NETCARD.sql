ALTER PROCEDURE [dbo].[PROC_PAGNET_REG_FECH_NETCARD]
                         @DATREF DATETIME = NULL,
                         @DATFIM DATETIME = NULL
AS

BEGIN

/* ---------------------- Parametros para testar ---------------------*/

    --declare @DATREF DATETIME,
    --        @DATFIM DATETIME

    --SET @DATREF = CONVERT(DATETIME,'20210101')
    --SET @DATFIM = CONVERT(DATETIME,'20210305')
/*------------------------------------------------------------------------*/


/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* JOB_PAGNET_TITULOS  PARA O NETCARD Versao 1.0                              */         
/* CRIAÇÃO : Luiz Felipe - AGOSTO/2019                                        */
/* DESCRIÇÃO : PROCEDURE UTILIZADA PARA LER OS FECHAMENTOS DO DIA E COPIAR    */
/*             ELES PARA UMA TABELA DO SISTEMA PAGNET                         */
/* VARIÁVEIS :  @DTFECHLOT = caso vier preenchido o sistema irá assumir esta  */
/*                           data como referência para buscar os fechamentos  */
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  

/*----------------------------------------------------------------------------------------------------------------------------------------------*/
/*-------------------------------------------------------TABELAS TEMPORÁRIAS UTILIZADAS---------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/

 DECLARE @FECHAMENTOS_PRE TABLE 
    (
         CODCRE             INT
        ,LOTE               INT
        ,CNPJ               VARCHAR(14)
        ,DTINI              DATE
        ,DTFIM              DATE
        ,QUANT              INT
        ,VALLIQ             DECIMAL(13,2)
        ,TAXA_PERC          DECIMAL(13,2)
        ,TAXAS              DECIMAL(13,2)
        ,TOTAL              DECIMAL(13,2)
        ,DTFECHTO           DATETIME
        ,DTPAGTO            DATETIME
        ,PRAZO              INT
        ,CODCEN             INT
        ,CODSUBREDE         INT
        ,SUBREDE            VARCHAR(100)
    )

    DECLARE @FECHAMENTOS_POS TABLE 
    (
         CODCRE             INT
        ,LOTE               INT
        ,CNPJ               VARCHAR(14)
        ,DTINI              DATETIME
        ,DTFIM              DATETIME
        ,QUANT              INT
        ,VALLIQ             DECIMAL(13,2)
        ,TAXA_PERC          DECIMAL(13,2)
        ,TAXAS              DECIMAL(13,2)
        ,TOTAL              DECIMAL(13,2)
        ,DTFECHTO           DATETIME
        ,DTPAGTO            DATETIME
        ,PRAZO              INT
        ,CODCEN             INT
        ,CODSUBREDE         INT
        ,SUBREDE            VARCHAR(100)
    )


    DECLARE @TABTEMP TABLE
    (
         ID             INT NOT NULL IDENTITY(1,1)
        ,CODTITULOAUX   INT 
        ,STATUS         NVARCHAR(50)
        ,CODCEN         INT
        ,DTFECHTO       DATETIME
        ,DATPGTO        DATETIME
        ,BANCO          VARCHAR(4)
        ,AGENCIA        VARCHAR(10)
        ,CONTA          VARCHAR(15)
        ,VALLIQ         DECIMAL(13,2)
        ,VALBRUTO       DECIMAL(13,2)
        ,CODSUBREDE     INT
        ,SISTEMA        INT
        ,CODEMPRESA     INT
    ) 

    DECLARE @TABTEMPFAVORITOS TABLE
    (
         ID             INT NOT NULL IDENTITY(1,1)
        ,CODCEN         INT
        ,NMFAVORITO     NVARCHAR(100)
        ,CPFCNPJ        NVARCHAR(14)
        ,BANCO          NVARCHAR(4)
        ,AGENCIA        NVARCHAR(5)
        ,DVAGENCIA      NVARCHAR(1)
        ,OPE            NVARCHAR(3)
        ,CONTA          NVARCHAR(15)
        ,DVCONTA        NVARCHAR(1)
        ,CEP            NVARCHAR(9)
        ,LOGRADOURO     NVARCHAR(100)
        ,NROLOGRADOURO  NVARCHAR(20)
        ,COMPLEMENTO    NVARCHAR(100)
        ,BAIRRO         NVARCHAR(100)
        ,CIDADE         NVARCHAR(100)
        ,UF             NVARCHAR(2)
        ,CODEMPRESA     INT
    )
    
    DECLARE @TAXASFECHAMENTO_PRE TABLE
    (
        CODCRE          INT
       ,LOTE            INT
       ,CODCEN          INT
       ,DTFECHTO        DATETIME
       ,DATPGTO         DATETIME    
       ,VALTAXA         DECIMAL(13,2)
       ,TAXA            VARCHAR(100)
       ,CODSUBREDE      INT
       ,SUBREDE         VARCHAR(50)
    )
    DECLARE @TAXASFECHAMENTO_POS TABLE
    (
        CODCRE          INT
       ,LOTE            INT
       ,CODCEN          INT
       ,DTFECHTO        DATETIME
       ,DATPGTO         DATETIME    
       ,VALTAXA         DECIMAL(13,2)
       ,TAXA            VARCHAR(100)
       ,CODSUBREDE      INT
       ,SUBREDE         VARCHAR(50)
    )
    DECLARE @TAXASFECHAMENTO TABLE
    (
        ID              INT NOT NULL IDENTITY(1,1)
       ,CODTAXA         INT
       ,CODCEN          INT
       ,DTFECHTO        DATETIME
       ,DATPGTO         DATETIME    
       ,VALTAXA         DECIMAL(13,2)
       ,TAXA            VARCHAR(100)
       ,CODSUBREDE      INT
       ,SISTEMA         INT
    )
    
/*----------------------------------------------------------------------------------------------------------------------------------------------*/
/*-------------------------------------------------------CARREGANDO TABELAS TEMPORÁRIAS---------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/

    --VARIÁVEL UTILIZADA PARA RETORNAR O ULTIMO ID DA TABELA DE FECHAMENTO, COM ISSO O SISTEMA IRÁ GARANTIR QUE O SEQUENCIAL FIQUE CORRETO.
    DECLARE @MAXCOD INT
    DECLARE @MAXCODLOG INT

    --DATA UTILIZADA COMO BASE PARA PESQUISAR A DATA DE FECHAMENTO NO NETCARD

    IF(@DATREF IS NULL)
    BEGIN
        SET @DATREF = CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111))
    END
    
    IF(@DATFIM IS NULL)
    BEGIN
        SET @DATFIM = CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111))
    END

    --SET @DATREF = CONVERT(DATETIME,CONVERT(VARCHAR,'20200901',111))
    --SET @DATFIM = CONVERT(DATETIME,CONVERT(VARCHAR,'20200901',111))
    
    ----Consulta Taxas do fechamento de credenciado POS
    INSERT INTO @TAXASFECHAMENTO_POS
    EXEC SRV_CONSULTA_TAXAS_FECHCRED @SISTEMA = 0, @SELECAO = 0, @DATINI = @DATREF, @DATFIM = @DATFIM
    
    --Consulta Taxas do fechamento de credenciado Pre
    INSERT INTO @TAXASFECHAMENTO_PRE
    EXEC SRV_CONSULTA_TAXAS_FECHCRED @SISTEMA = 1, @SELECAO = 0, @DATINI = @DATREF, @DATFIM = @DATFIM


     INSERT INTO @TAXASFECHAMENTO
     SELECT 0
           ,CODCEN         
           ,DTFECHTO       
           ,DATPGTO        
           ,SUM(VALTAXA)
           ,TAXA           
           ,CODSUBREDE     
           ,0 AS SISTEMA   
       FROM @TAXASFECHAMENTO_POS  
       GROUP BY  CODCEN         
                ,DTFECHTO       
                ,DATPGTO        
                ,TAXA           
                ,CODSUBREDE  

     INSERT INTO @TAXASFECHAMENTO
     SELECT 0
           ,CODCEN         
           ,DTFECHTO       
           ,DATPGTO        
           ,SUM(VALTAXA)
           ,TAXA           
           ,CODSUBREDE     
           ,1 AS SISTEMA   
       FROM @TAXASFECHAMENTO_PRE  
       GROUP BY  CODCEN         
                ,DTFECHTO       
                ,DATPGTO        
                ,TAXA           
                ,CODSUBREDE  
        
        --POS-PAGO
    INSERT INTO @FECHAMENTOS_POS
    EXEC SRV_CONSULTA_FECHCRED @SISTEMA = 0, @SELECAO = 0,  @DATINI = @DATREF, @DATFIM = @DATFIM

    --PRE-PAGO
    INSERT INTO @FECHAMENTOS_PRE
    EXEC SRV_CONSULTA_FECHCRED @SISTEMA = 1, @SELECAO = 0,  @DATINI = @DATREF, @DATFIM = @DATFIM

    
    INSERT INTO @TABTEMP
        SELECT 0                                                            AS 'CODTITULOAUX'
              ,'EM_ABERTO'                                                  AS 'STATUS'
              ,FPOS.CODCEN                                                  AS 'CODCEN'
              ,FPOS.DTFECHTO                                                AS 'DTFECHTO'
              ,FPOS.DTPAGTO                                                 AS 'DATPGTO'
              ,SUBSTRING(REPLACE(C.CTABCO,'-',''), 1, 4)                    AS 'BANCO'
              ,SUBSTRING(REPLACE(C.CTABCO,'-',''), 5, 6)                    AS 'AGENCIA'
              ,SUBSTRING(REPLACE(C.CTABCO,'-',''), 11, LEN(C.CTABCO))       AS 'CONTA'
              ,SUM(FPOS.VALLIQ)                                             AS 'VALTRA'
              ,SUM(FPOS.TOTAL)                                              AS 'TOTAL'
              ,FPOS.CODSUBREDE                                              AS 'CODSUBREDE'
              ,0                                                            AS 'SISTEMA'
              ,E.CODEMPRESA                                                 AS 'CODEMPRESA'
        FROM @FECHAMENTOS_POS   FPOS
            ,VCREDENCIADO       C
            ,PAGNET_CADEMPRESA  E 
        WHERE C.CODCRE          = FPOS.CODCEN  
          AND E.CODSUBREDE      = FPOS.CODSUBREDE
      GROUP BY FPOS.CODCEN                                                
              ,FPOS.DTPAGTO                                               
              ,FPOS.DTFECHTO                                              
              ,SUBSTRING(REPLACE(C.CTABCO,'-',''), 1, 4)                                  
              ,SUBSTRING(REPLACE(C.CTABCO,'-',''), 5, 6)                                  
              ,SUBSTRING(REPLACE(C.CTABCO,'-',''), 11, LEN(C.CTABCO))                
              ,FPOS.CODSUBREDE 
              ,E.CODEMPRESA



    INSERT INTO @TABTEMP
       SELECT 0                                                             AS 'CODTITULOAUX'
              ,'EM_ABERTO'                                                  AS 'STATUS'
              ,FPRE.CODCEN                                                  AS 'CODCEN'
              ,FPRE.DTFECHTO                                                AS 'DTFECHTO'
              ,FPRE.DTPAGTO                                                 AS 'DATPGTO'
              ,SUBSTRING(REPLACE(C.CTABCO_VA,'-',''), 1, 4)                 AS 'BANCO'
              ,SUBSTRING(REPLACE(C.CTABCO_VA,'-',''), 5, 6)                 AS 'AGENCIA'
              ,SUBSTRING(REPLACE(C.CTABCO_VA,'-',''), 11, LEN(C.CTABCO_VA)) AS 'CONTA'
              ,SUM(FPRE.VALLIQ)                                             AS 'VALTRA'
              ,SUM(FPRE.TOTAL)                                              AS 'TOTAL'
              ,FPRE.CODSUBREDE                                              AS 'CODSUBREDE'
              ,1                                                            AS 'SISTEMA'
              ,E.CODEMPRESA                                                 AS 'CODEMPRESA'
        FROM @FECHAMENTOS_PRE   FPRE
            ,VCREDENCIADO       C
            ,PAGNET_CADEMPRESA  E 
        WHERE C.CODCRE          = FPRE.CODCEN 
          AND E.CODSUBREDE      = FPRE.CODSUBREDE
      GROUP BY FPRE.CODCEN                                                
              ,FPRE.DTPAGTO                                               
              ,FPRE.DTFECHTO                                              
              ,SUBSTRING(REPLACE(C.CTABCO_VA,'-',''), 1, 4)                                  
              ,SUBSTRING(REPLACE(C.CTABCO_VA,'-',''), 5, 6)                                  
              ,SUBSTRING(REPLACE(C.CTABCO_VA,'-',''), 11, LEN(C.CTABCO_VA))                
              ,FPRE.CODSUBREDE 
              ,E.CODEMPRESA 
              
      
      --REMOVE OS TÍTULOS QUE JÁ FORAM MIGRADOS PARA O PAGNET      
    DELETE TEMP
    FROM @TABTEMP TEMP
        ,PAGNET_EMISSAO_TITULOS ET
    WHERE ET.CODFAVORECIDO  = TEMP.CODCEN
      AND ET.CODEMPRESA     = TEMP.CODEMPRESA
      AND ET.DATPGTO        = TEMP.DATPGTO
      AND ET.VALBRUTO       = TEMP.VALBRUTO


/*----------------------------------------------------------------------------------------------------------------------------------------------*/
/*--------------------------------------------------INCLUI CREDENCIADO NA TABELA DE FAVORITOS---------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/

   INSERT INTO @TABTEMPFAVORITOS
   SELECT  DISTINCT
           T.CODCEN        
          ,CRE.RAZSOC    
          ,CRE.CNPJ        
          ,T.BANCO  
          ,CASE WHEN LEN(T.AGENCIA) = 6 THEN SUBSTRING(T.AGENCIA, 1, 5) ELSE T.AGENCIA END AGENCIA
          ,CASE WHEN LEN(T.AGENCIA) = 6 THEN SUBSTRING(T.AGENCIA, 6, 1) ELSE '0' END AS DVAGENCIA
          ,'0' 
          ,CASE WHEN LEN(T.CONTA) > 0 THEN SUBSTRING(T.CONTA, 1, LEN(T.CONTA) - 1) ELSE T.CONTA END CONTA  
          ,CASE WHEN LEN(T.CONTA) > 0 THEN SUBSTRING(T.CONTA, LEN(T.CONTA), 1) ELSE '0' END DVCONTA  
          ,CRE.CEP           
          ,CRE.ENDCRE    
          ,'' 
          ,CRE.ENDCPL   
          ,B.NOMBAI        
          ,LOC.NOMLOC        
          ,CRE.SIGUF0    
          ,T.CODEMPRESA
     FROM @TABTEMP      T
     JOIN VCREDENCIADO  CRE ON CRE.CODCRE = T.CODCEN
LEFT JOIN BAIRRO        B   ON B.CODBAI = CRE.CODBAI
LEFT JOIN LOCALIDADE    LOC ON LOC.CODLOC = CRE.CODLOC

    --REMOVE OS FAVORITOS DUPLICADOS. ISSO ACONTECE QUANDO OS DADOS BANCÁRIOS DE PRÉ E POS SÃO DIFERENTES PARA A MESMA CENTRALIZADORA
    DELETE T1 
     FROM @TABTEMPFAVORITOS AS T1
         ,@TABTEMPFAVORITOS AS T2
    WHERE T1.CODCEN=T2.CODCEN AND T1.ID < T2.ID


    --ATUALIZA O CÓDIGO DA OPERAÇÃO CASO O BANCO FOR DA CAIXA
    UPDATE @TABTEMPFAVORITOS
    SET OPE =  CASE WHEN SUBSTRING(CONTA, 1, 3) = '003' THEN SUBSTRING(CONTA, 1, 3) ELSE '0' END 
        ,CONTA = CASE WHEN SUBSTRING(CONTA, 1, 3) = '003' THEN SUBSTRING(CONTA, 4, LEN(CONTA) - 3) ELSE '0' END
    WHERE BANCO = '0104' 
    
    INSERT INTO PAGNET_CADFAVORECIDO
    SELECT T.CODCEN                 AS CODFAVORECIDO
        ,T.NMFAVORITO               AS NMFAVORITO
        ,T.CPFCNPJ                  AS CPFCNPJ
        ,T.CODCEN                   AS CODCEN
        ,SUBSTRING(T.BANCO,2,3)     AS BANCO
        ,T.AGENCIA
        ,T.DVAGENCIA
        ,T.OPE
        ,REPLACE(T.CONTA, '-','')   AS CONTACORRENTE
        ,T.DVCONTA                  AS DVCONTACORRENTE
        ,T.CEP
        ,T.LOGRADOURO
        ,T.NROLOGRADOURO
        ,T.COMPLEMENTO
        ,T.BAIRRO
        ,T.CIDADE
        ,T.UF
        ,'S'                        AS ATIVO
        ,NULL                       AS CODEMPRESA
        ,GETDATE()                  AS DTINCLUSAO
        ,GETDATE()                  AS DTALTERACAO
    FROM @TABTEMPFAVORITOS T
   WHERE NOT EXISTS (SELECT 1 
                       FROM PAGNET_CADFAVORECIDO FAV 
                      WHERE FAV.CODCEN = T.CODCEN)

    SELECT @MAXCODLOG = ISNULL((SELECT MAX(CODFAVORECIDO_LOG) FROM PAGNET_CADFAVORECIDO_LOG),0)              

    --GRAVA LOG DE INCLUSÃO DE FAVORITOS
    INSERT INTO PAGNET_CADFAVORECIDO_LOG
    SELECT @MAXCODLOG + T.ID        AS CODFAVORECIDO_LOG
        ,T.CODCEN                   AS CODFAVORECIDO
        ,T.NMFAVORITO               AS NMFAVORITO
        ,T.CPFCNPJ                  AS CPFCNPJ
        ,T.CODCEN                   AS CODCEN
        ,SUBSTRING(T.BANCO,2,3)     AS BANCO
        ,T.AGENCIA
        ,T.DVAGENCIA
        ,T.OPE
        ,REPLACE(T.CONTA, '-','')   AS CONTACORRENTE
        ,T.DVCONTA                  AS DVCONTACORRENTE
        ,T.CEP
        ,T.LOGRADOURO
        ,T.NROLOGRADOURO
        ,T.COMPLEMENTO
        ,T.BAIRRO
        ,T.CIDADE
        ,T.UF
        ,'S'                        AS ATIVO
        ,9999
        ,GETDATE()
        ,'Cadastro de favoritos via Sistema NetCard'
        ,NULL                       AS CODEMPRESA
        ,GETDATE()                  AS DTINCLUSAO
        ,GETDATE()                  AS DTALTERACAO
    FROM @TABTEMPFAVORITOS T
   WHERE NOT EXISTS (SELECT 1 
                       FROM PAGNET_CADFAVORECIDO_LOG FAV 
                      WHERE FAV.CPFCNPJ = T.CPFCNPJ)


    SET @MAXCOD = 0
   --*********************************************************************************************************--
   
/*----------------------------------------------------------------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------INSERI OS TÍTULOS NO PAGNET-----------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/

    DECLARE @CODIGOTITULOPGTODEFAULT int
    
    SELECT @MAXCOD = ISNULL((SELECT MAX(CODTITULO) FROM PAGNET_EMISSAO_TITULOS),0)
    SELECT @MAXCODLOG = ISNULL((SELECT MAX(CODTITULO_LOG) FROM PAGNET_EMISSAO_TITULOS_LOG),0)       

    UPDATE @TABTEMP
       SET STATUS = 'BAIXADO'
     WHERE VALLIQ = 0

     UPDATE @TABTEMP
     SET CODTITULOAUX = @MAXCOD + ID

     SELECT @CODIGOTITULOPGTODEFAULT = CODPLANOCONTAS FROM PAGNET_CADPLANOCONTAS WHERE PLANOCONTASDEFAULT = 'S' AND DEFAULTPAGAMENTO = 'S'
          

    BEGIN TRY
        --set @DefaultPicture = CONVERT(varbinary(max),@DefaultPicture)
        BEGIN TRANSACTION     

            /*GRAVA OS DADOS NA TABELA DE TÍTULOS DO PAGNET*/
            INSERT INTO PAGNET_EMISSAO_TITULOS
            SELECT  CODTITULOAUX   
                   ,CODTITULOAUX   AS CODTITULOPAI
                   ,STATUS
                   ,CODEMPRESA 
                   ,TEMP.CODCEN     AS CODFAVORECIDO 
                   ,TEMP.DTFECHTO   AS DATEMISSAO
                   ,TEMP.DATPGTO
                   ,dbo.AJUSTA_FERIADO(TEMP.DATPGTO)    AS DATREALPGTO
                   ,TEMP.VALBRUTO
                   ,TEMP.VALLIQ
                   ,TEMP.VALLIQ     AS VALTOTAL
                   ,'TEDDOC'        AS TIPOTITULO
                   ,'NC'            AS ORIGEM
                   ,TEMP.SISTEMA
                   ,''              AS LINHADIGITAVEL
                   ,NULL            AS CODBORDERO
                   ,''              AS SEUNUMERO  
                   ,NULL            AS CODCONTACORRENTE  
                   ,ISNULL((SELECT PC.CODPLANOCONTAS FROM PAGNET_CADPLANOCONTAS PC WHERE PC.PLANOCONTASDEFAULT = 'S' AND PC.DEFAULTPAGAMENTO = 'S' AND PC.CODEMPRESA = TEMP.CODEMPRESA), @CODIGOTITULOPGTODEFAULT)         
              FROM @TABTEMP TEMP

        
           ---- --GRAVA OS O LOG DE INCLUSÃO DE DADOS NA TABELA DE FECHAMENTO DO PAGNET
            INSERT INTO PAGNET_EMISSAO_TITULOS_LOG
            SELECT @MAXCODLOG + ID
                  ,CODTITULOAUX   
                  ,CODTITULOAUX   AS CODTITULOPAI
                  ,STATUS
                  ,(SELECT F.CODEMPRESA FROM PAGNET_CADEMPRESA F WHERE F.CODSUBREDE = TEMP.CODSUBREDE ) AS CODEMPRESA 
                  ,TEMP.CODCEN     AS CODFAVORECIDO 
                  ,TEMP.DTFECHTO   AS DATEMISSAO
                  ,TEMP.DATPGTO
                  ,TEMP.DATPGTO    AS DATREALPGTO
                  ,TEMP.VALBRUTO
                  ,TEMP.VALLIQ
                  ,TEMP.VALLIQ     AS VALTOTAL
                  ,'TEDDOC'        AS TIPOTITULO
                  ,'NC'            AS ORIGEM
                  ,TEMP.SISTEMA
                  ,''              AS LINHADIGITAVEL
                  ,NULL            AS CODBORDERO
                  ,''              AS SEUNUMERO         
                 ,9999
                 ,GETDATE()
                 ,'Importação de fechamento do NetCard'
                 ,NULL            AS CODCONTACORRENTE 
                 ,ISNULL((SELECT PC.CODPLANOCONTAS FROM PAGNET_CADPLANOCONTAS PC WHERE PC.PLANOCONTASDEFAULT = 'S' AND PC.DEFAULTPAGAMENTO = 'S' AND PC.CODEMPRESA = TEMP.CODEMPRESA), @CODIGOTITULOPGTODEFAULT)         
              FROM @TABTEMP TEMP


/*----------------------------------------------------------------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------INSERI AS TAXAS NO PAGNET-------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/
    DECLARE @CODTAXATITULO INT
    SELECT @CODTAXATITULO = isnull(MAX(CODTAXATITULO),0) FROM PAGNET_TAXAS_TITULOS
    
    UPDATE @TAXASFECHAMENTO
    SET CODTAXA = ID + @CODTAXATITULO 
    
    INSERT INTO PAGNET_TAXAS_TITULOS
    SELECT  CODTAXA                         AS CODTAXATITULO
           ,TEMP.CODTITULOAUX               AS CODTITULO
           ,TX.TAXA                         AS DESCRICAO
           ,TX.VALTAXA - (TX.VALTAXA * 2)   AS VALOR
           ,GETDATE()                       AS DTINCLUSAO
           ,'NC'                            AS ORIGEM
           ,9999
    FROM @TAXASFECHAMENTO TX
        ,@TABTEMP TEMP
    WHERE TEMP.CODCEN       = TX.CODCEN
      AND TEMP.DATPGTO      = TX.DATPGTO
      AND TEMP.CODSUBREDE   = TX.CODSUBREDE
      AND TEMP.SISTEMA      = TX.SISTEMA

      
/*----------------------------------------------------------------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------FIM DO PROCESSO DA DIÁRIA-------------------------------------------------------*/
/*----------------------------------------------------------------------------------------------------------------------------------------------*/

        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        DECLARE @ErrorSeverity INT,
                @ErrorNumber   INT,
                @ErrorMessage nvarchar(4000),
                @ErrorState INT,
                @ErrorLine  INT,
                @ErrorProc nvarchar(200)
                -- Grab error information from SQL functions
        SET @ErrorSeverity = ERROR_SEVERITY()
        SET @ErrorNumber   = ERROR_NUMBER()
        SET @ErrorMessage  = ERROR_MESSAGE()
        SET @ErrorState    = ERROR_STATE()
        SET @ErrorLine     = ERROR_LINE()
        SET @ErrorProc     = ERROR_PROCEDURE()
        SELECT 'Problem updating person''s information.' + CHAR(13) + 'SQL Server Error Message is: ' + CAST(@ErrorNumber AS VARCHAR(10)) + ' in procedure: ' + @ErrorProc + ' Line: ' + CAST(@ErrorLine AS VARCHAR(10)) + ' Error text: ' + @ErrorMessage
        -- Not all errors generate an error state, to set to 1 if it's zero
        IF @ErrorState  = 0
        SET @ErrorState = 1
        ---- If the error renders the transaction as uncommittable or we have open transactions, we may want to rollback
        IF @@TRANCOUNT > 0
        BEGIN
            select @ErrorMessage as ErrorMessage
                --print 'Rollback transaction'
                ROLLBACK TRANSACTION
        END
        --RAISERROR (@ErrorMessage , @ErrorSeverity, @ErrorState, @ErrorNumber)
    END CATCH
    --RETURN @@ERROR
END


