CREATE PROCEDURE [dbo].[PROC_PAGNET_REG_FECH_CLI_NETCARD]
                        @ERRO                  INT             = 0   OUTPUT  
                       ,@MSG_ERRO              VARCHAR(500)    = ''   OUTPUT
AS

BEGIN

/*-----------------PARAMETROS PARA TESTES-----------------------*/

    -----------------------DATA UTILIZADA COMO BASE PARA PESQUISAR A DATA DE FECHAMENTO NO NETCARD

    --DECLARE @ERRO       INT    
    --DECLARE @MSG_ERRO   VARCHAR(500)   

/*-----------------FIM PARAMETROS PARA TESTES-------------------*/


/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_REG_FECH_CLI_NETCARD  PARA O NETCARD Versao 1.0                */         
/* CRIA��O : Luiz Felipe - AGOSTO/2019                                        */
/* DESCRI��O : PROCEDURE UTILIZADA PARA LER OS FECHAMENTOS DE CLIENTES DO DIA */
/*             E COPIAR ELES PARA UMA TABELA DO SISTEMA PAGNET                */
/* VARI�VEIS :                                                                */
/* REVIS�O :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


    DECLARE @TABTEMP TABLE
    (
         ID             INT NOT NULL IDENTITY(1,1)
        ,STATUS         NVARCHAR(50)
        ,CODCLI         INT
        ,CODEMPRESA     INT
        ,DTFECHTO       DATETIME
        ,DATPGTO        DATETIME
        ,VALOR          DECIMAL(13,2)
        ,CODSUBREDE     INT
        ,ORIGEM         NVARCHAR(40)
        ,NROREF_NETCARD NVARCHAR(50)
        ,NRODOCUMENTO   NVARCHAR(50)
    ) 

    DECLARE @FECHAMENTOS_CLI TABLE
    (
	    CODCLI      INT,
	    LOTE        INT,
	    CNPJ        CHAR(14),
	    DTINI       DATETIME,
	    DTFIM       DATETIME,
	    DTFECHTO    DATETIME,
	    TOTAL       NUMERIC(15,2),
	    COMPREC     NUMERIC(15,2),
	    TAXA        NUMERIC(12,2),
	    SEGVIA      NUMERIC(12,2),
	    COMPRAS     NUMERIC(15,2),
	    PREMIO      NUMERIC(15,2),
	    DESCONTO    NUMERIC(15,2),
	    PRAZO       INT,
	    DTPAGTO     DATETIME,
	    BENEFICIO   NUMERIC(12,2),
	    CODSUBREDE  INT,
	    SUBREDE     VARCHAR(20)
    )

    DECLARE @TABTEMPCLIENTE TABLE
    (
         ID                     INT NOT NULL IDENTITY(1,1)
        ,CODCLIENTE	            INT
        ,NMCLIENTE	            VARCHAR(200)
        ,CPFCNPJ	            VARCHAR(200)
        ,CODEMPRESA	            INT
        ,CEP	                VARCHAR(200)
        ,LOGRADOURO	            VARCHAR(200)
        ,NROLOGRADOURO	        VARCHAR(200)
        ,COMPLEMENTO	        VARCHAR(200)
        ,BAIRRO	                VARCHAR(200)
        ,CIDADE	                VARCHAR(200)
        ,UF	                    NVARCHAR(200)
        ,COBRANCADIFERENCIADA	NVARCHAR(200)
        ,COBRAJUROS	            NVARCHAR(200)
        ,VLJUROSDIAATRASO	    DECIMAL(15,2)
        ,PERCJUROS	            DECIMAL(15,2)
        ,COBRAMULTA	            NVARCHAR(200)
        ,VLMULTADIAATRASO	    DECIMAL(15,2)
        ,PERCMULTA	            DECIMAL(15,2)
        ,CODPRIMEIRAINSTCOBRA	INT
        ,CODSEGUNDAINSTCOBRA	INT
        ,TAXAEMISSAOBOLETO	    DECIMAL(15,2)
        ,EMAIL	                NVARCHAR(200)
    )
    

    --VARI�VEL UTILIZADA PARA RETORNAR O ULTIMO ID DA TABELA DE FECHAMENTO, COM ISSO O SISTEMA IR� GARANTIR QUE O SEQUENCIAL FIQUE CORRETO.
    DECLARE @MAXCOD INT
    DECLARE @MAXCODLOG INT
    DECLARE @CODIGOFORMAFATURAMENTO INT

    
    --DATA UTILIZADA COMO BASE PARA PESQUISAR A DATA DE FECHAMENTO NO NETCARD
    DECLARE @DATREF DATETIME

    SET @DATREF = CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111))

    ---------------------------------------------------------------------------------------------------------------------------
    ---------------------------------lISTA OS FECHAMENTOS DO DIA---------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------

    INSERT INTO @FECHAMENTOS_CLI
    EXEC SRV_CONSULTA_FECHCLI @SISTEMA = 0, @SELECAO = 0, @DATINI = @DATREF, @DATFIM = @DATREF  

    
    ---------------------------------------------------------------------------------------------------------------------------
    ---------------------------------lISTA OS FECHAMENTOS DO DIA---------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------


    INSERT INTO @TABTEMP
        SELECT 'EM_ABERTO'                                                      AS 'STATUS'
              ,FPOS.CODCLI                                                      AS 'CODCLI'
              ,E.CODEMPRESA                                                     AS 'CODEMPRESA'
              ,FPOS.DTFECHTO                                                    AS 'DTFECHTO'
              ,FPOS.DTPAGTO                                                     AS 'DATPGTO'
              ,FPOS.TOTAL                                                       AS 'TOTAL'
              ,FPOS.CODSUBREDE                                                  AS 'CODSUBREDE'
              ,'POS PAGO NETCARD'                                               AS 'ORIGEM'
              ,CONVERT(VARCHAR,FPOS.LOTE)                                       AS 'NROREF_NETCARD'
              ,CONVERT(VARCHAR,FPOS.CODCLI) + CONVERT(VARCHAR,FPOS.LOTE)        AS 'NRODOCUMENTO'
        FROM @FECHAMENTOS_CLI   FPOS  
            ,PAGNET_CADEMPRESA  E  WITH(NOLOCK)         
        WHERE FPOS.CODSUBREDE   = E.CODSUBREDE 
		  AND FPOS.TOTAL        > 0
          AND NOT EXISTS (SELECT 1 
                           FROM PAGNET_EMISSAOFATURAMENTO PGF WITH(NOLOCK)
                          WHERE PGF.CODCLIENTE          = FPOS.CODCLI
                            AND PGF.DATVENCIMENTO       = FPOS.DTPAGTO
                            AND PGF.CODEMPRESA          = (SELECT E.CODEMPRESA FROM PAGNET_CADEMPRESA E WITH(NOLOCK)  WHERE E.CODSUBREDE = FPOS.CODSUBREDE )
                            AND PGF.VALOR               = FPOS.TOTAL
                            AND PGF.TIPOFATURAMENTO     = 'POSPAGO'
                            )   
                                      
   --**********************************INCLUI CLINETE NA TABELA DE PAGNET_CADCLIENTE******************************--
   
       INSERT INTO @TABTEMPCLIENTE
       SELECT  DISTINCT
              T.CODCLI AS CODCLIENTE
             ,CLI.NOMCLI AS NMCLIENTE
             ,CGC AS CPFCNPJ
             ,T.CODEMPRESA AS CODEMPRESA
             ,CLI.CEP AS CEP
             ,CLI.ENDEDC AS LOGRADOURO
             ,0 AS NROLOGRADOURO
             ,CLI.ENDCPLEDC AS COMPLEMENTO
             ,B.NOMBAI AS BAIRRO
             ,LOC.NOMLOC AS CIDADE
             ,CLI.SIGUF0EDC AS UF
             ,'N' AS COBRANCADIFERENCIADA
             ,'N' AS COBRAJUROS
             ,NULL AS VLJUROSDIAATRASO
             ,NULL AS PERCJUROS
             ,'N' AS COBRAMULTA
             ,NULL AS VLMULTADIAATRASO
             ,NULL AS PERCMULTA
             ,0 AS CODPRIMEIRAINSTCOBRA
             ,0 AS CODSEGUNDAINSTCOBRA
             ,0 AS TAXAEMISSAOBOLETO
             ,CLI.EMA AS EMAIL  
         FROM @TABTEMP      T
         JOIN VCLIENTE      CLI WITH(NOLOCK) ON CLI.CODCLI = T.CODCLI
         JOIN PRODUTO       PROD WITH(NOLOCK) ON CLI.CODPROD = PROD.CODPROD
         JOIN TIPOPRODUTO   TPROD WITH(NOLOCK) ON TPROD.TIPOPROD = PROD.TIPOPROD AND TPROD.EXPORTAFEC = 'S'
    LEFT JOIN BAIRRO        B   ON B.CODBAI = CLI.CODBAI
    LEFT JOIN LOCALIDADE    LOC ON LOC.CODLOC = CLI.CODLOC
   WHERE NOT EXISTS (SELECT 1 
                       FROM PAGNET_CADCLIENTE cliente WITH(NOLOCK)
                      WHERE cliente.CODCLIENTE = T.CODCLI)

    
    BEGIN TRY

        BEGIN TRANSACTION   
                INSERT INTO PAGNET_CADCLIENTE
                SELECT 
                     CODCLIENTE	            
                    ,NMCLIENTE	            
                    ,CPFCNPJ	            
                    ,CODEMPRESA	            
                    ,1 AS CODFORMAFATURAMENTO
                    ,CEP	                
                    ,LOGRADOURO	            
                    ,NROLOGRADOURO	        
                    ,COMPLEMENTO	        
                    ,BAIRRO	                
                    ,CIDADE	                
                    ,UF	                    
                    ,COBRANCADIFERENCIADA	
                    ,COBRAJUROS	            
                    ,VLJUROSDIAATRASO	    
                    ,PERCJUROS	            
                    ,COBRAMULTA	            
                    ,VLMULTADIAATRASO	    
                    ,PERCMULTA	 
                    ,EMAIL	                  
                    ,CODPRIMEIRAINSTCOBRA	
                    ,CODSEGUNDAINSTCOBRA	
                    ,TAXAEMISSAOBOLETO
                    ,'S'	 
                    ,'S'         
                    ,'J'       
                FROM @TABTEMPCLIENTE T
                WHERE NOT EXISTS (SELECT 1 
                                    FROM PAGNET_CADCLIENTE CLI WITH(NOLOCK)
                                    WHERE CLI.CODCLIENTE = T.CODCLIENTE)

                SELECT @MAXCODLOG = ISNULL((SELECT MAX(CODCLIENTE_LOG) FROM PAGNET_CADCLIENTE_LOG WITH(NOLOCK)),0)              

                --GRAVA LOG DE INCLUS�O DE CLIENTE
                INSERT INTO PAGNET_CADCLIENTE_LOG
                SELECT @MAXCODLOG + T.ID   AS CODCLIENTE_LOG
                        ,CODCLIENTE	            
                        ,NMCLIENTE	            
                        ,CPFCNPJ	            
                        ,CODEMPRESA	            
                        ,1 AS CODFORMAFATURAMENTO
                        ,CEP	                
                        ,LOGRADOURO	            
                        ,NROLOGRADOURO	        
                        ,COMPLEMENTO	        
                        ,BAIRRO	                
                        ,CIDADE	                
                        ,UF	                    
                        ,COBRANCADIFERENCIADA	
                        ,COBRAJUROS	            
                        ,VLJUROSDIAATRASO	    
                        ,PERCJUROS	            
                        ,COBRAMULTA	            
                        ,VLMULTADIAATRASO	    
                        ,PERCMULTA	 
                        ,EMAIL	                  
                        ,CODPRIMEIRAINSTCOBRA	
                        ,CODSEGUNDAINSTCOBRA	
                        ,TAXAEMISSAOBOLETO
                        ,'S'	 
                        ,'S'      
                        ,9999
                        ,GETDATE()
                        ,'Cadastro de CLIENTE via Sistema NetCard'  
                        ,'J'  
                FROM @TABTEMPCLIENTE T
                WHERE NOT EXISTS (SELECT 1 
                                    FROM PAGNET_CADCLIENTE_LOG LOG WITH(NOLOCK)
                                    WHERE LOG.CODCLIENTE = T.CODCLIENTE)

                 COMMIT TRANSACTION
        END TRY
        BEGIN CATCH
            DECLARE @ErrorSeverityCli INT,
                    @ErrorNumberCli   INT,
                    @ErrorMessageCli nvarchar(4000),
                    @ErrorStateCli INT,
                    @ErrorLineCli  INT,
                    @ErrorProcCli nvarchar(200)
                    -- Grab error information from SQL functions
            SET @ErrorSeverityCli = ERROR_SEVERITY()
            SET @ErrorNumberCli   = ERROR_NUMBER()
            SET @ErrorMessageCli  = ERROR_MESSAGE()
            SET @ErrorStateCli    = ERROR_STATE()
            SET @ErrorLineCli     = ERROR_LINE()
            SET @ErrorProcCli     = ERROR_PROCEDURE()
            SELECT 'Problem updating person''s information.' + CHAR(13) + 'SQL Server Error Message is: ' + CAST(@ErrorNumberCli AS VARCHAR(10)) + ' in procedure: ' + @ErrorProcCli + ' Line: ' + CAST(@ErrorLineCli AS VARCHAR(10)) + ' Error text: ' + @ErrorMessageCli
            -- Not all errors generate an error state, to set to 1 if it's zero
            IF @ErrorStateCli  = 0
            SET @ErrorStateCli = 1
            ---- If the error renders the transaction as uncommittable or we have open transactions, we may want to rollback
            IF @@TRANCOUNT > 0
            BEGIN
            
                    set @ERRO = @ErrorNumberCli
                    set @MSG_ERRO = @ErrorMessageCli
                    --print 'Rollback transaction'
                    ROLLBACK TRANSACTION
            END
            --RAISERROR (@ErrorMessage , @ErrorSeverity, @ErrorState, @ErrorNumber)
        END CATCH

    SET @MAXCOD = 0
   --*********************************************************************************************************--
   
    DECLARE @CODIGOTITULOPGTODEFAULT INT

    SELECT @MAXCOD = ISNULL((SELECT MAX(CODEMISSAOFATURAMENTO) FROM PAGNET_EMISSAOFATURAMENTO),0)
    SELECT @MAXCODLOG = ISNULL((SELECT MAX(CODEMISSAOFATURAMENTO_LOG) FROM PAGNET_EMISSAOFATURAMENTO_LOG),0)       
    
    SELECT @CODIGOTITULOPGTODEFAULT = CODPLANOCONTAS FROM PAGNET_CADPLANOCONTAS WHERE PLANOCONTASDEFAULT = 'S' AND DEFAULTPAGAMENTO = 'S'

    BEGIN TRY
        BEGIN TRANSACTION   
        

            --GRAVA OS DADOS NA TABELA DE FATURAMENTO DO PAGNET
            INSERT INTO PAGNET_EMISSAOFATURAMENTO
            SELECT @MAXCOD + TEMP.ID    AS CODEMISSAOFATURAMENTO
                  ,TEMP.STATUS
                  ,TEMP.CODCLI
                  ,NULL                 AS CODBORDERO
                  ,TEMP.CODEMPRESA
                  ,cli.CODFORMAFATURAMENTO
                  ,TEMP.DATPGTO         AS DATVENCIMENTO
                  ,NULL                 AS DATPGTO
                  ,NULL                 AS VLPGTO
                  ,TEMP.ORIGEM          AS ORIGEM
                  ,'POSPAGO'            AS TIPOFATURAMENTO
                  ,TEMP.NROREF_NETCARD  AS NROREF_NETCARD
                  ,NULL                 AS SEUNUMERO
                  ,TEMP.VALOR           AS VALOR
                  ,TEMP.DTFECHTO        AS DATSOLICITACAO
                  ,NULL                 AS DATSEGUNDODESCONTO
                  ,NULL                 AS VLDESCONTO
                  ,NULL                 AS VLSEGUNDODESCONTO
                  ,NULL                 AS MENSAGEMARQUIVOREMESSA
                  ,NULL                 AS MENSAGEMINSTRUCOESCAIXA
                  ,TEMP.NRODOCUMENTO    AS NRODOCUMENTO
                  ,NULL                 AS VLDESCONTOCONCEDIDO
                  ,NULL                 AS JUROSCOBRADO
                  ,NULL                 AS MULTACOBRADA  
                  ,@MAXCOD + TEMP.ID    AS CODEMISSAOFATURAMENTOPAI  
                  ,1                    AS PARCELA  
                  ,1                    AS TOTALPARCELA  
                  ,TEMP.VALOR           AS VALORPARCELA 
                  ,NULL                 AS CODCONTACORRENTE 
                  ,(SELECT ISNULL(PC.CODPLANOCONTAS, @CODIGOTITULOPGTODEFAULT) FROM PAGNET_CADPLANOCONTAS PC WITH(NOLOCK) WHERE PC.PLANOCONTASDEFAULT = 'S' AND PC.DEFAULTPAGAMENTO = 'S' AND PC.CODEMPRESA = TEMP.CODEMPRESA)        
              FROM @TABTEMP TEMP
              JOIN PAGNET_CADCLIENTE cli ON cli.CODCLIENTE = TEMP.CODCLI
        
           -- --/*GRAVA OS O LOG DE INCLUS�O DE DADOS NA TABELA DE FATURAMENTO DO PAGNET*/
            INSERT INTO PAGNET_EMISSAOFATURAMENTO_LOG
            SELECT @MAXCODLOG + TEMP.ID AS CODEMISSAOFATURAMENTO_LOG
                  ,@MAXCOD + TEMP.ID    AS CODEMISSAOFATURAMENTO
                  ,TEMP.STATUS
                  ,TEMP.CODCLI
                  ,NULL                 AS CODBORDERO
                  ,TEMP.CODEMPRESA
                  ,cli.CODFORMAFATURAMENTO
                  ,TEMP.DATPGTO         AS DATVENCIMENTO
                  ,NULL                 AS DATPGTO
                  ,NULL                 AS VLPGTO
                  ,TEMP.ORIGEM          AS ORIGEM
                  ,'POSPAGO'            AS TIPOFATURAMENTO
                  ,TEMP.NROREF_NETCARD  AS NROREF_NETCARD
                  ,NULL                 AS SEUNUMERO
                  ,TEMP.VALOR           AS VALOR
                  ,TEMP.DTFECHTO        AS DATSOLICITACAO
                  ,NULL                 AS DATSEGUNDODESCONTO
                  ,NULL                 AS VLDESCONTO
                  ,NULL                 AS VLSEGUNDODESCONTO
                  ,NULL                 AS MENSAGEMARQUIVOREMESSA
                  ,NULL                 AS MENSAGEMINSTRUCOESCAIXA       
                  ,9999
                  ,GETDATE()
                  ,'Importa��o de fechamento do NetCard'
                  ,TEMP.NRODOCUMENTO    AS NRODOCUMENTO
                  ,NULL                 AS VLDESCONTOCONCEDIDO
                  ,NULL                 AS JUROSCOBRADO
                  ,NULL                 AS MULTACOBRADA 
                  ,@MAXCOD + TEMP.ID    AS CODEMISSAOFATURAMENTOPAI  
                  ,1                    AS PARCELA  
                  ,1                    AS TOTALPARCELA  
                  ,TEMP.VALOR           AS VALORPARCELA  
                  ,NULL                 AS CODCONTACORRENTE 
                  ,(SELECT ISNULL(PC.CODPLANOCONTAS, @CODIGOTITULOPGTODEFAULT) FROM PAGNET_CADPLANOCONTAS PC WITH(NOLOCK) WHERE PC.PLANOCONTASDEFAULT = 'S' AND PC.DEFAULTPAGAMENTO = 'S' AND PC.CODEMPRESA = TEMP.CODEMPRESA)        
              FROM @TABTEMP TEMP
              JOIN PAGNET_CADCLIENTE cli ON cli.CODCLIENTE = TEMP.CODCLI

              
             set @ERRO = 0
             set @MSG_ERRO = 'Sucesso'

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
        
                set @ERRO = @ErrorNumber
                set @MSG_ERRO = @ErrorMessage
                --print 'Rollback transaction'
                ROLLBACK TRANSACTION
        END
        --RAISERROR (@ErrorMessage , @ErrorSeverity, @ErrorState, @ErrorNumber)
    END CATCH
    --SELECT @@ERROR
END


