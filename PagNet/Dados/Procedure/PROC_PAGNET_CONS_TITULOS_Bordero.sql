
ALTER PROCEDURE [dbo].[PROC_PAGNET_CONS_TITULOS_BORDERO]
                       @dtInicio      DATETIME
                      ,@dtFim        DATETIME
                      ,@codfavorito  INT    
                      ,@codEmpresa   INT        
                      ,@codContac    INT
                      ,@Banco        varchar(5)
AS
/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_CONS_TITULOS  PARA O NETCARD Versao 1.0                        */         
/* CRIAÇÃO : Luiz Felipe - MARÇO/2019                                         */  
/* DESCRIÇÃO: Procedure utilizada para retornar os títulos em aberto para     */ 
/*            pagamento                                                       */         
/* REVISÃO : 1.1 (Cardoso - 05/07/2021)                                       */
/* DESCRIÇÃO: -> Alteração da rotina para incluir a mensagem do tipo de       */
/*               conta igual a CC e alteração da condição da @LISTATITULOS    */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  

BEGIN

------------------------------TESTE
--DECLARE   @dtInicio      DATETIME
--         ,@dtFim        DATETIME
--         ,@codfavorito  INT    
--         ,@codEmpresa   INT        
--         ,@codContac    INT
--         ,@Banco        varchar(5)

--SELECT @dtInicio        = '20210612'
--      ,@dtFim           = '20210618'
--      ,@codfavorito     = 0
--      ,@codEmpresa      = 1
--      ,@codContac       = 1
--      ,@Banco           = ''
-------------------------------

    DECLARE @CODBANCO NVARCHAR(4)
    DECLARE @VALTED DECIMAL(15,2)
    DECLARE @VALMINIMOCC DECIMAL(15,2)
    DECLARE @VALMINIMOTED DECIMAL(15,2)

    SELECT @CODBANCO        = CODBANCO ,
           @VALTED          = ISNULL(C.VALTED,0),
           @VALMINIMOCC     = ISNULL(C.VALMINIMOCC,0),
           @VALMINIMOTED    = ISNULL(C.VALMINIMOTED,0)
      FROM PAGNET_CONTACORRENTE C 
     WHERE C.CODCONTACORRENTE = @codContac

    DECLARE @LISTATITULOS TABLE 
    (
         CODFAVORECIDO      INT
        ,NMFAVORECIDO       VARCHAR(200)
        ,CPFCNPJ            VARCHAR(14)
        ,DATREALPGTO        DATETIME
        ,QTTITULOS          INT
        ,VALTOTAL           DECIMAL(13,2)
        ,REGRADIFERENCIADA  CHAR(1)
        ,VALMINIMOTED       DECIMAL(13,2)
        ,VALMINIMOCC        DECIMAL(13,2)
        ,VALTED             DECIMAL(13,2)
        ,BANCO              NVARCHAR(4)
        ,AGENCIA            NVARCHAR(10)
        ,CONTACORRENTE      NVARCHAR(20)
        ,TIPOTITULO         VARCHAR(20)
        ,DESFORPAG          VARCHAR(50)
        ,LINHADIGITAVEL     NVARCHAR(65)
    )
          
    INSERT INTO @LISTATITULOS
    SELECT   ISNULL(FAV.CODFAVORECIDO, 0)
            ,CASE WHEN TIT.TIPOTITULO = 'BOLETO' THEN 'BOLETO BANCÁRIO' ELSE FAV.NMFAVORECIDO END AS 'FAVORECIDO'
            ,ISNULL(FAV.CPFCNPJ, 0)
            ,TIT.DATREALPGTO
            ,COUNT(TIT.CODTITULO)
            ,SUM(TIT.VALTOTAL)  AS VALTOTAL 
            ,ISNULL(CONFIG.REGRADIFERENCIADA,'N')
            ,ISNULL(CONFIG.VALMINIMOTED, 0)
            ,ISNULL(CONFIG.VALMINIMOCC, 0)
            ,ISNULL(CONFIG.VALTED,0)
            ,FAV.BANCO
            ,LTRIM(RTRIM(FAV.AGENCIA)) + '-' + FAV.DVAGENCIA
            ,LTRIM(RTRIM(FAV.CONTACORRENTE)) + '-' + FAV.DVCONTACORRENTE
            ,TIT.TIPOTITULO
            ,CASE TIT.TIPOTITULO 
                WHEN 'TEDDOC' THEN (CASE WHEN FAV.BANCO = @CODBANCO THEN 'CREDITO EM CONTA' ELSE 'TED' END) 
                WHEN 'TED' THEN (CASE WHEN FAV.BANCO = @CODBANCO THEN 'CREDITO EM CONTA' ELSE 'TED' END) 
                WHEN 'CC' THEN (CASE WHEN FAV.BANCO = @CODBANCO THEN 'CREDITO EM CONTA' ELSE 'TED' END) 
                WHEN 'BOLETO' THEN 'PAGAMENTO DE BOLETO'
                ELSE TIT.TIPOTITULO END  AS DESFORPAG
           ,ISNULL(TIT.LINHADIGITAVEL,'')
        FROM PAGNET_EMISSAO_TITULOS         TIT 
   LEFT JOIN PAGNET_CADFAVORECIDO           FAV ON  FAV.CODFAVORECIDO   = TIT.CODFAVORECIDO
   LEFT JOIN PAGNET_CADFAVORECIDO_CONFIG    CONFIG ON  CONFIG.CODFAVORECIDO   = FAV.CODFAVORECIDO AND CONFIG.CODEMPRESA = @codEmpresa
        WHERE TIT.DATREALPGTO     >= @dtInicio
            AND TIT.DATREALPGTO   <= @dtFim
            AND ((@codfavorito    = 0) OR TIT.CODFAVORECIDO = @codfavorito)
            AND ((@Banco          = '') OR FAV.BANCO = @Banco)
            AND TIT.CODEMPRESA    = @codEmpresa
            AND TIT.STATUS         in ('EM_ABERTO', 'RECUSADO')
            AND (CONFIG.CODCONTACORRENTE IS NULL OR CONFIG.CODCONTACORRENTE = @codContac)
       GROUP BY FAV.CODFAVORECIDO
               ,FAV.NMFAVORECIDO
               ,FAV.CPFCNPJ
               ,TIT.DATREALPGTO
               ,CONFIG.REGRADIFERENCIADA
               ,CONFIG.VALMINIMOTED
               ,CONFIG.VALMINIMOCC
               ,CONFIG.VALTED
               ,FAV.BANCO
               ,LTRIM(RTRIM(FAV.AGENCIA)) + '-' + FAV.DVAGENCIA
               ,LTRIM(RTRIM(FAV.CONTACORRENTE)) + '-' + FAV.DVCONTACORRENTE
               ,TIT.TIPOTITULO
               ,TIT.LINHADIGITAVEL
                             
               UPDATE @LISTATITULOS
                  SET VALTED = @VALTED,
                      VALMINIMOCC = @VALMINIMOCC,
                      VALMINIMOTED = @VALMINIMOTED
                WHERE REGRADIFERENCIADA = 'N'


       SELECT FC.CODFAVORECIDO             
             ,FC.NMFAVORECIDO         
             ,FC.CPFCNPJ            
             ,FC.DATREALPGTO  
             ,FC.QTTITULOS
             ,FC.VALTOTAL  
             ,CASE WHEN DESFORPAG = 'TED' THEN VALTED ELSE 0 END AS TAXATRANSFERENCIA
             ,CASE WHEN DESFORPAG = 'TED' THEN VALTOTAL - VALTED ELSE VALTOTAL END AS VALORPREVISTOPAGAMENTO
             ,FC.BANCO              
             ,FC.AGENCIA            
             ,FC.CONTACORRENTE         
             ,FC.TIPOTITULO         
             ,FC.DESFORPAG   
             ,FC.LINHADIGITAVEL   
         FROM @LISTATITULOS  FC    
         WHERE (FC.DESFORPAG = 'TED' AND (FC.VALTOTAL - FC.VALTED) > FC.VALMINIMOTED)
				OR (FC.DESFORPAG = 'TED' AND FC.VALTOTAL > FC.VALMINIMOTED)
				OR (FC.DESFORPAG = 'CREDITO EM CONTA' AND FC.VALTOTAL > FC.VALMINIMOCC)
				OR FC.DESFORPAG = 'PAGAMENTO DE BOLETO'
		            
    ORDER BY FC.NMFAVORECIDO, FC.DATREALPGTO  desc

   
END
