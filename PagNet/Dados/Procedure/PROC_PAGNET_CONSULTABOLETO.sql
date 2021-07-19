CREATE PROCEDURE [dbo].[PROC_PAGNET_CONSULTABOLETO]
                       @dtInicio            DATETIME
                      ,@dtFim               DATETIME
                      ,@codCliente          INT             = 0
                      ,@codEmpresa          INT    
                      ,@status              NVARCHAR(100)   = ''    
                      ,@ERRO                INT             = 0   OUTPUT  
                      ,@MSG_ERRO            VARCHAR(500)    = ''   OUTPUT
AS
/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_CONSULTABOLETO               Versao 1.0                        */         
/* CRIAÇÃO : Luiz Felipe - SETEMBRO/2019                                      */  
/* DESCRIÇÃO: Procedure utilizada para retornar os boletos par a tela de      */ 
/*            consultar situação boletos                                      */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  

BEGIN

------------------------------TESTE
--DECLARE   @dtInicio             DATETIME
--         ,@dtFim                DATETIME
--         ,@codCliente           INT    
--         ,@codEmpresa           INT   
--         ,@status               NVARCHAR(100)
--         ,@ERRO                 INT             
--         ,@MSG_ERRO             VARCHAR(500)    

--SELECT @dtInicio            = '20190923'
--      ,@dtFim               = '20190930'
--      ,@codCliente          = 0
--      ,@codEmpresa          = 1
--      ,@status              = ''
--      ,@ERRO                = 0
--      ,@MSG_ERRO            = ''
---------------------------------

    DECLARE  @COBRAJUROS             CHAR(1)        
            ,@VLJUROSDIAATRASO       DECIMAL(15,2)     
            ,@PERCJUROS              DECIMAL(5,2) 
            ,@COBRAMULTA             CHAR(1)        
            ,@VLMULTADIAATRASO       DECIMAL(15,2)
            ,@PERCMULTA              DECIMAL(5,2)
            ,@NOMEPRIMEIRAINSTCOBRA  VARCHAR(200)            
            ,@NOMESEGUNDAINSTCOBRA   VARCHAR(200)            
            ,@TAXAEMISSAOBOLETO      DECIMAL(13,2)

    DECLARE @LISTABOLETOS TABLE 
    (
         CODEMISSAOBOLETO           INT
        ,STATUS                     NVARCHAR(100)
        ,CODCLIENTE                 INT
        ,NMCLIENTE                  NVARCHAR(250)
        ,CPFCNPJ                    NVARCHAR(20)
        ,EMAIL                      NVARCHAR(100)
        ,COBRANCADIFERENCIADA       CHAR(1)
        ,COBRAJUROS                 CHAR(1)
        ,VLJUROSDIAATRASO           DECIMAL(15,2)
        ,PERCJUROS                  DECIMAL(15,2)
        ,COBRAMULTA                 CHAR(1)
        ,VLMULTADIAATRASO           DECIMAL(15,2)
        ,PERCMULTA                  DECIMAL(15,2)
        ,NOMEPRIMEIRAINSTCOBRA      NVARCHAR(250)  
        ,NOMESEGUNDAINSTCOBRA       NVARCHAR(250)  
        ,TAXAEMISSAOBOLETO          DECIMAL(15,2)
        ,DATVENCIMENTO              DATETIME
        ,NOSSONUMERO                NVARCHAR(250)
        ,CODOCORRENCIA              NVARCHAR(250)
        ,SEUNUMERO                  NVARCHAR(250)
        ,VALOR                      DECIMAL(15,2)
        ,DATSOLICITACAO             DATETIME
        ,DATREFERENCIA              DATETIME
        ,DATSEGUNDODESCONTO         DATETIME
        ,VLDESCONTO                 DECIMAL(15,2)
        ,VLSEGUNDODESCONTO          DECIMAL(15,2)
        ,MENSAGEMARQUIVOREMESSA     NVARCHAR(250)
        ,MENSAGEMINSTRUCOESCAIXA    NVARCHAR(250)
        ,NUMCONTROLE                NVARCHAR(250)
        ,OCORRENCIARETBOL           NVARCHAR(250)
        ,NMBOLETOGERADO             NVARCHAR(250)
        ,DESCRICAOOCORRENCIARETBOL  NVARCHAR(250)
        ,BOLETORECUSADO             CHAR(1)
        ,MSGRECUSA                  VARCHAR(8000)       
    )
          
    
        BEGIN TRY

        INSERT INTO @LISTABOLETOS
        SELECT   BOL.CODEMISSAOBOLETO
                ,BOL.STATUS
                ,BOL.CODCLIENTE
                ,CLI.NMCLIENTE
                ,CLI.CPFCNPJ
                ,CLI.EMAIL
                ,CLI.COBRANCADIFERENCIADA
                ,CLI.COBRAJUROS
                ,CLI.VLJUROSDIAATRASO
                ,CLI.PERCJUROS
                ,CLI.COBRAMULTA
                ,CLI.VLMULTADIAATRASO
                ,CLI.PERCMULTA
                ,PRI.nmInstrucaoCobranca
                ,SEC.nmInstrucaoCobranca
                ,CLI.TAXAEMISSAOBOLETO
                ,BOL.DATVENCIMENTO
                ,BOL.NOSSONUMERO
                ,BOL.CODOCORRENCIA
                ,BOL.SEUNUMERO
                ,BOL.VALOR
                ,BOL.DATSOLICITACAO
                ,BOL.DATREFERENCIA
                ,BOL.DATSEGUNDODESCONTO
                ,BOL.VLDESCONTO
                ,BOL.VLSEGUNDODESCONTO
                ,BOL.MENSAGEMARQUIVOREMESSA
                ,BOL.MENSAGEMINSTRUCOESCAIXA
                ,BOL.NUMCONTROLE
                ,BOL.OCORRENCIARETBOL
                ,BOL.NMBOLETOGERADO
                ,BOL.DESCRICAOOCORRENCIARETBOL
                ,'N'
                ,''
            FROM PAGNET_EMISSAOBOLETO        BOL 
                ,PAGNET_CADCLIENTE           CLI 
                ,PAGNET_INSTRUCAOCOBRANCA    PRI   
                ,PAGNET_INSTRUCAOCOBRANCA    SEC   
            WHERE CLI.CODCLIENTE            = BOL.CODCLIENTE
                AND PRI.codInstrucaoCobranca  = CLI.CODPRIMEIRAINSTCOBRA
                AND SEC.codInstrucaoCobranca  = CLI.CODSEGUNDAINSTCOBRA
                AND BOL.DATVENCIMENTO       >= @dtInicio
                AND BOL.DATVENCIMENTO       <= @dtFim
                AND ((@codCliente           = 0) OR BOL.CODCLIENTE = @codCliente)
                AND BOL.CODEMPRESA          = @codEmpresa
                AND ((@status           = 0) OR BOL.STATUS = @status) 


              SELECT  @COBRAJUROS             = REG.COBRAJUROS            
                     ,@VLJUROSDIAATRASO       = REG.VLJUROSDIAATRASO    
                     ,@PERCJUROS              = REG.PERCJUROS    
                     ,@COBRAMULTA             = REG.COBRAMULTA    
                     ,@VLMULTADIAATRASO       = REG.VLMULTADIAATRASO    
                     ,@PERCMULTA              = REG.PERCMULTA    
                     ,@NOMEPRIMEIRAINSTCOBRA  = PRI.nmInstrucaoCobranca  
                     ,@NOMESEGUNDAINSTCOBRA   = SEC.nmInstrucaoCobranca 
                     ,@TAXAEMISSAOBOLETO      = REG.TAXAEMISSAOBOLETO
                FROM PAGNET_CONFIG_REGRA_BOL     REG
                    ,PAGNET_INSTRUCAOCOBRANCA    PRI   
                    ,PAGNET_INSTRUCAOCOBRANCA    SEC 
               WHERE PRI.codInstrucaoCobranca  = REG.CODPRIMEIRAINSTCOBRA
                 AND SEC.codInstrucaoCobranca  = REG.CODSEGUNDAINSTCOBRA
                 AND REG.ATIVO = 'S'
                 AND REG.CODEMPRESA = @codEmpresa

                --inseri valores das taxas
                UPDATE TABTEMP
                  SET TABTEMP.COBRAJUROS                = @COBRAJUROS   
                     ,TABTEMP.VLJUROSDIAATRASO          = @VLJUROSDIAATRASO   
                     ,TABTEMP.PERCJUROS                 = @PERCJUROS  
                     ,TABTEMP.COBRAMULTA                = @COBRAMULTA  
                     ,TABTEMP.VLMULTADIAATRASO          = @VLMULTADIAATRASO  
                     ,TABTEMP.PERCMULTA                 = @PERCMULTA  
                     ,TABTEMP.NOMEPRIMEIRAINSTCOBRA     = @NOMEPRIMEIRAINSTCOBRA  
                     ,TABTEMP.NOMESEGUNDAINSTCOBRA      = @NOMESEGUNDAINSTCOBRA  
                     ,TABTEMP.TAXAEMISSAOBOLETO         = @TAXAEMISSAOBOLETO  
                FROM @LISTABOLETOS TABTEMP
               WHERE TABTEMP.COBRANCADIFERENCIADA = 'N'

               --informa se o boleto já foi recusado alguma vez pelo banco
               UPDATE TABTEMP
                  SET TABTEMP.BOLETORECUSADO = 'S'
                     ,TABTEMP.MSGRECUSA = LOG.DESCLOG
                FROM @LISTABOLETOS TABTEMP
                   JOIN PAGNET_EMISSAOBOLETO_LOG [LOG] on LOG.CODEMISSAOBOLETO = TABTEMP.CODEMISSAOBOLETO and LOG.STATUS = 'RECUSADO'
               WHERE TABTEMP.STATUS NOT IN ('LIQUIDADO', 'LIQUIDADO_MANUALMENTE')

           SELECT  CODEMISSAOBOLETO          
                  ,STATUS                    
                  ,CODCLIENTE                
                  ,NMCLIENTE                 
                  ,CPFCNPJ                   
                  ,EMAIL                     
                  ,COBRANCADIFERENCIADA      
                  ,COBRAJUROS                
                  ,VLJUROSDIAATRASO          
                  ,PERCJUROS                 
                  ,COBRAMULTA                
                  ,VLMULTADIAATRASO          
                  ,PERCMULTA                 
                  ,NOMEPRIMEIRAINSTCOBRA     
                  ,NOMESEGUNDAINSTCOBRA      
                  ,TAXAEMISSAOBOLETO                
                  ,DATVENCIMENTO             
                  ,NOSSONUMERO               
                  ,CODOCORRENCIA             
                  ,SEUNUMERO                 
                  ,VALOR                     
                  ,DATSOLICITACAO            
                  ,DATREFERENCIA             
                  ,DATSEGUNDODESCONTO        
                  ,VLDESCONTO                
                  ,VLSEGUNDODESCONTO         
                  ,MENSAGEMARQUIVOREMESSA    
                  ,MENSAGEMINSTRUCOESCAIXA   
                  ,NUMCONTROLE               
                  ,OCORRENCIARETBOL          
                  ,NMBOLETOGERADO            
                  ,DESCRICAOOCORRENCIARETBOL
                  ,BOLETORECUSADO 
                  ,MSGRECUSA
             FROM @LISTABOLETOS  FC    

             set @ERRO = 0
             set @MSG_ERRO = 'Sucesso'
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

        set @ERRO = @ErrorNumber
        set @MSG_ERRO = @ErrorMessage
    END CATCH
   
END
