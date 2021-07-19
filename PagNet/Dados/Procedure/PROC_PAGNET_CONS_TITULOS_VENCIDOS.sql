CREATE PROCEDURE [dbo].[PROC_PAGNET_CONS_TITULOS_VENCIDOS]
                        @codEmpresa INT        
                       ,@dtInicio   DATETIME          
                       ,@dtFim      DATETIME           
AS

BEGIN


/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_CONS_FECHAMENTO_CRED  PARA O NETCARD Versao 1.0                */         
/* CRIA��O : Luiz Felipe - MAR�O/2019                                         */         
/* DESCRI��O : Retorna todos os t�tulos que n�o foram baixados e que n�o      */
/*             foram e que est�o vencidos                                     */
/*             ELES PARA UMA TABELA DO SISTEMA PAGNET                         */
/* VARI�VEIS :  @codEmpresa = C�DIGO DA Empresa                               */
/*              @CARTAOPRE  = RETORNA OS T�TULOS DE CART�O PR�                */
/*              @CARTAOPOS  = RETORNA OS T�TULOS DE CART�O P�S                */
/* REVIS�O :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  



--------------------------------TESTE
--DECLARE  @codEmpresa INT        
--        ,@dtInicio   DATETIME          
--        ,@dtFim      DATETIME     


--SELECT @codEmpresa  = 1
--      ,@dtInicio    = '20190409'
--      ,@dtFim       = '20190905'
--------------------------------------------------------------------------------



    DECLARE @TABTEMP TABLE 
    (
         CODTITULO          INT
        ,CODFAVORECIDO             INT
        ,NMFAVORECIDO       VARCHAR(200)
        ,CPFCNPJ            VARCHAR(14)
        ,DATREALPGTO        DATETIME
        ,VALTOTAL           DECIMAL(13,2)
        ,BANCO              NVARCHAR(4)
        ,AGENCIA            NVARCHAR(10)
        ,CONTACORRENTE      NVARCHAR(20)
        ,TIPOTITULO         VARCHAR(20)
    )

        INSERT INTO @TABTEMP
        SELECT   TIT.CODTITULO
                ,FAV.CODCEN
                ,FAV.NMFAVORECIDO
                ,FAV.CPFCNPJ
                ,TIT.DATREALPGTO
                ,TIT.VALTOTAL 
                ,FAV.BANCO
                ,FAV.AGENCIA + '-' + FAV.DVAGENCIA
                ,FAV.CONTACORRENTE + '-' + FAV.DVCONTACORRENTE
                ,TIT.TIPOTITULO
            FROM PAGNET_EMISSAO_TITULOS         TIT 
                ,PAGNET_CADFAVORECIDO           FAV   
            WHERE FAV.CODFAVORECIDO   = TIT.CODFAVORECIDO
                AND TIT.DATREALPGTO     >= @dtInicio
                AND TIT.DATREALPGTO     <= @dtFim
                AND TIT.CODEMPRESA     = @codEmpresa
                AND TIT.STATUS         IN ('EM_ABERTO', 'RECUSADO')



        SELECT  CODTITULO      
               ,CODFAVORECIDO         
               ,NMFAVORECIDO     
               ,CPFCNPJ        
               ,DATREALPGTO    
               ,VALTOTAL       
               ,BANCO          
               ,AGENCIA        
               ,CONTACORRENTE  
               ,TIPOTITULO               
         FROM @TABTEMP      
     ORDER BY NMFAVORECIDO  desc

   
END
