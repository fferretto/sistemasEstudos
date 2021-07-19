

--CREATE PROCEDURE [dbo].[PROC_PAGNET_SOLICITACAOBOLETO]
--                       @dtInicio            DATETIME
--                      ,@dtFim               DATETIME
--                      ,@codCliente          INT    
--                      ,@codEmpresa          INT    
--                      ,@ERRO                INT             OUTPUT  
--                      ,@MSG_ERRO            VARCHAR(500)    OUTPUT
--AS
--/*----------------------------------------------------------------------------*/    
--/*                                                                            */         
--/* PROC_PAGNET_SOLICITACAOBOLETO            Versao 1.0                        */         
--/* CRIAÇÃO : Luiz Felipe - SETEMBRO/2019                                      */  
--/* DESCRIÇÃO: Procedure utilizada para retornar as solicitações de emissão de */ 
--/*            boletos                                                         */         
--/* REVISÃO :                                                                  */
--/*                                                                            */   
--/*                                                                            */    
--/*----------------------------------------------------------------------------*/  

BEGIN


------------------------------TESTE
DECLARE   @dtInicio             DATETIME
         ,@dtFim                DATETIME
         ,@codCliente           INT    
         ,@codEmpresa           INT    
         ,@codContaCorrente     INT       
         ,@ERRO                 INT             
         ,@MSG_ERRO             VARCHAR(500)    

SELECT @dtInicio            = '20210611'
      ,@dtFim               = '20210621'
      ,@codCliente          = 0
      ,@codEmpresa          = 1
      ,@codContaCorrente    = 1
      ,@ERRO                = 0
      ,@MSG_ERRO            = ''
---------------------------------

    SET  @ERRO = 0
    SET @MSG_ERRO   = 'SUCESSO'

    DECLARE  @AGRUPARFATURAMENTOSDIA CHAR(1) 

    SELECT @AGRUPARFATURAMENTOSDIA = AGRUPARFATURAMENTOSDIA 
     FROM PAGNET_CONTACORRENTE 
    WHERE CODCONTACORRENTE = @codContaCorrente

    DECLARE @LISTABOLETOS TABLE 
    (
         CODCLIENTE                 INT
        ,NMCLIENTE                  NVARCHAR(250)
        ,CPFCNPJ                    NVARCHAR(20)
        ,ORIGEM                     NVARCHAR(250)
        ,DATVENCIMENTO              DATETIME
        ,VALOR                      DECIMAL(15,2)   
        ,AGRUPARFATURAMENTOSDIA     char(1)
    )

    DECLARE @LISTARETORNO TABLE 
    (
         CODCLIENTE                 INT
        ,NMCLIENTE                  NVARCHAR(250)
        ,CPFCNPJ                    NVARCHAR(20)
        ,ORIGEM                     NVARCHAR(250)
        ,DATVENCIMENTO              DATETIME
        ,QTFATURAMENTO              INT
        ,VALOR                      DECIMAL(15,2)   
    )
          
    INSERT INTO @LISTABOLETOS
    SELECT   BOL.CODCLIENTE
            ,CLI.NMCLIENTE
            ,CLI.CPFCNPJ
            ,BOL.ORIGEM
            ,BOL.DATVENCIMENTO
            ,BOL.VALOR
            ,CASE WHEN CLI.COBRANCADIFERENCIADA = 'S' THEN CLI.AGRUPARFATURAMENTOSDIA ELSE @AGRUPARFATURAMENTOSDIA END AS AGRUPARFATURAMENTOSDIA
        FROM PAGNET_EMISSAOFATURAMENTO   BOL 
            ,PAGNET_CADCLIENTE           CLI  
        WHERE CLI.CODCLIENTE            = BOL.CODCLIENTE
            AND BOL.DATVENCIMENTO       >= @dtInicio
            AND BOL.DATVENCIMENTO       <= @dtFim
            AND ((@codCliente           = 0) OR BOL.CODCLIENTE = @codCliente)
            AND BOL.CODFORMAFATURAMENTO = 1 --BOLETO 
            AND BOL.CODEMPRESA          = @codEmpresa
            AND BOL.STATUS              in ('EM_ABERTO', 'RECUSADO')

        INSERT INTO @LISTARETORNO
        SELECT TMP.CODCLIENTE
              ,TMP.NMCLIENTE
              ,TMP.CPFCNPJ
              ,TMP.ORIGEM
              ,TMP.DATVENCIMENTO
              ,COUNT(1) AS QTFATURAMENTO
              ,SUM(TMP.VALOR)
          FROM @LISTABOLETOS TMP
         WHERE TMP.AGRUPARFATURAMENTOSDIA = 'S'
         GROUP BY TMP.CODCLIENTE
                 ,TMP.NMCLIENTE
                 ,TMP.CPFCNPJ
                 ,TMP.ORIGEM
                 ,TMP.DATVENCIMENTO

        INSERT INTO @LISTARETORNO
        SELECT TMP.CODCLIENTE
              ,TMP.NMCLIENTE
              ,TMP.CPFCNPJ
              ,TMP.ORIGEM
              ,TMP.DATVENCIMENTO
              ,1 AS QTFATURAMENTO
              ,TMP.VALOR
          FROM @LISTABOLETOS TMP
         WHERE TMP.AGRUPARFATURAMENTOSDIA = 'N'


         SELECT  CODCLIENTE               
                ,NMCLIENTE                
                ,CPFCNPJ                  
                ,ORIGEM                   
                ,DATVENCIMENTO            
                ,QTFATURAMENTO            
                ,VALOR                    
           FROM @LISTARETORNO

         
END
