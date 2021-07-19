

CREATE PROCEDURE [dbo].[PROC_PAGNET_CONS_FATURAS_BORDERO]
                       @dtInicio            DATETIME
                      ,@dtFim               DATETIME
                      ,@codCliente          INT    
                      ,@codEmpresa          INT    
                      ,@codContaCorrente    INT
AS
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
--DECLARE   @dtInicio             DATETIME
--         ,@dtFim                DATETIME
--         ,@codCliente           INT    
--         ,@codEmpresa           INT    
--         ,@codContaCorrente     INT         

--SELECT @dtInicio            = '20210601'
--      ,@dtFim               = '20210630'
--      ,@codCliente          = 0
--      ,@codEmpresa          = 1
--      ,@codContaCorrente    = 1
---------------------------------


    DECLARE  @AGRUPARFATURAMENTOSDIA CHAR(1) 

    DECLARE @LISTABOLETOS TABLE 
    (   
         CODFATURA                  INT
        ,CODCLIENTE                 INT
        ,NMCLIENTE                  NVARCHAR(250)
        ,CPFCNPJ                    NVARCHAR(20)
        ,ORIGEM                     NVARCHAR(250)
        ,DATVENCIMENTO              DATETIME
        ,VALOR                      DECIMAL(15,2)   
        ,AGRUPARFATURAMENTOSDIA     char(1)
    )

    DECLARE @LISTARETORNO TABLE 
    (
         CODFATURA                  INT
        ,CODCLIENTE                 INT
        ,NMCLIENTE                  NVARCHAR(250)
        ,CPFCNPJ                    NVARCHAR(20)
        ,ORIGEM                     NVARCHAR(250)
        ,DATVENCIMENTO              DATETIME
        ,QTFATURAMENTO              INT
        ,VALOR                      DECIMAL(15,2)   
    )    

    SELECT @AGRUPARFATURAMENTOSDIA = AGRUPARFATURAMENTOSDIA 
     FROM PAGNET_CONTACORRENTE 
    WHERE CODCONTACORRENTE = @codContaCorrente
          
    INSERT INTO @LISTABOLETOS
    SELECT   BOL.CODEMISSAOFATURAMENTO
            ,BOL.CODCLIENTE
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
        SELECT 0 AS CODFATURA
              ,TMP.CODCLIENTE
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
        SELECT TMP.CODFATURA
              ,TMP.CODCLIENTE
              ,TMP.NMCLIENTE
              ,TMP.CPFCNPJ
              ,TMP.ORIGEM
              ,TMP.DATVENCIMENTO
              ,1 AS QTFATURAMENTO
              ,TMP.VALOR
          FROM @LISTABOLETOS TMP
         WHERE TMP.AGRUPARFATURAMENTOSDIA = 'N'


         SELECT  CODFATURA
                ,CODCLIENTE               
                ,NMCLIENTE                
                ,CPFCNPJ                  
                ,DATVENCIMENTO            
                ,QTFATURAMENTO            
                ,VALOR                    
           FROM @LISTARETORNO

END
