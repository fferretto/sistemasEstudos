/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_CONSULTA_TITULOS  PARA O NETCARD Versao 1.0                    */         
/* CRIAÇÃO : Luiz Felipe - MARÇO/2019                                         */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_CONSULTA_TITULOS]
                        @dtInicio         DATETIME
                       ,@dtFim            DATETIME
                       ,@codFavorecido    INT             = 0
                       ,@codEmpresa       INT             = 0
                       ,@status           VARCHAR(50)     = ''
                       ,@codigoTitulo     INT             = 0
AS

BEGIN

-----------------------TESTE
    --DECLARE  @dtInicio          DATETIME
    --         ,@dtFim            DATETIME
    --         ,@codFavorecido    INT        
    --         ,@codEmpresa       INT        
    --         ,@status           VARCHAR(50)    
    --         ,@codigoTitulo     INT    


    --SELECT @dtInicio        = '20191202'
    --      ,@dtFim           = '20200113'
    --      ,@codFavorecido   = 0
    --      ,@codEmpresa      = 1
    --      ,@status          = ''
    --      ,@codigoTitulo    = 6088
-------------------------------

    DECLARE @TABTEMP TABLE 
    (
         CODTITULO          INT
        ,CODFAVORECIDO      INT  
        ,NMFAVORECIDO       VARCHAR(200)
        ,STATUS             VARCHAR(200)
        ,CPFCNPJ            VARCHAR(14)
        ,DATEMISSAO         DATETIME
        ,DATPGTO            DATETIME
        ,DATREALPGTO        DATETIME
        ,BANCO              VARCHAR(40)
        ,AGENCIA            VARCHAR(40)
        ,CONTACORRENTE      VARCHAR(40)
        ,VALLIQ             DECIMAL(13,2)
        ,VALTOTAL           DECIMAL(13,2)
        ,VALBRUTO           DECIMAL(13,2)
        ,VALTAXA            DECIMAL(13,2)
        ,TIPCARTAO          VARCHAR(40)
        ,TIPOTITULO         VARCHAR(40)
        ,SEUNUMERO          VARCHAR(40)
    )

    if @codigoTitulo > 0
    BEGIN
        INSERT INTO @TABTEMP
        SELECT TIT.CODTITULO
                ,TIT.CODFAVORECIDO
                ,FAV.NMFAVORECIDO
                ,TIT.STATUS
                ,FAV.CPFCNPJ
                ,TIT.DATEMISSAO
                ,TIT.DATPGTO
                ,TIT.DATREALPGTO
                ,FAV.BANCO
                ,FAV.AGENCIA + '-' + FAV.DVAGENCIA 
                ,FAV.CONTACORRENTE + '-' + FAV.DVCONTACORRENTE
                ,TIT.VALLIQ
                ,TIT.VALTOTAL
                ,ISNULL(TIT.VALBRUTO,0)
                ,(ISNULL(TIT.VALBRUTO,0) - TIT.VALTOTAL) AS VALTAXA
                ,CASE WHEN TIT.ORIGEM = 'NC' THEN (CASE WHEN TIT.SISTEMA = 0 THEN 'Pós Pago' ELSE 'Pré Pago' END) ELSE '' END AS TIPCARTAO
                ,TIT.TIPOTITULO
                ,TIT.SEUNUMERO   
        FROM PAGNET_EMISSAO_TITULOS     TIT 
            ,PAGNET_CADFAVORECIDO       FAV   
        WHERE TIT.CODFAVORECIDO          = FAV.CODFAVORECIDO
            AND TIT.CODTITULO            = @codigoTitulo
            AND TIT.CODEMPRESA           = @codEmpresa
    END
    ELSE
    BEGIN
        INSERT INTO @TABTEMP
        SELECT TIT.CODTITULO
                ,TIT.CODFAVORECIDO
                ,FAV.NMFAVORECIDO
                ,TIT.STATUS
                ,FAV.CPFCNPJ
                ,TIT.DATEMISSAO
                ,TIT.DATPGTO
                ,TIT.DATREALPGTO
                ,FAV.BANCO
                ,FAV.AGENCIA + '-' + FAV.DVAGENCIA 
                ,FAV.CONTACORRENTE + '-' + FAV.DVCONTACORRENTE
                ,TIT.VALLIQ
                ,TIT.VALTOTAL
                ,ISNULL(TIT.VALBRUTO,0)
                ,(ISNULL(TIT.VALBRUTO,0) - TIT.VALTOTAL) AS VALTAXA
                ,CASE WHEN TIT.ORIGEM = 'NC' THEN (CASE WHEN TIT.SISTEMA = 0 THEN 'Pós Pago' ELSE 'Pré Pago' END) ELSE '' END AS TIPCARTAO
                ,TIT.TIPOTITULO
                ,TIT.SEUNUMERO   
        FROM PAGNET_EMISSAO_TITULOS     TIT 
            ,PAGNET_CADFAVORECIDO       FAV   
        WHERE TIT.CODFAVORECIDO          = FAV.CODFAVORECIDO
            AND TIT.DATREALPGTO           >= @dtInicio
            AND TIT.DATREALPGTO           <= @dtFim
            AND ((@codFavorecido          = 0) OR TIT.CODFAVORECIDO = @codFavorecido)
            AND ((@codigoTitulo           = 0) OR TIT.CODTITULO = @codigoTitulo)
            AND TIT.CODEMPRESA            = @codEmpresa
    END

        


        SELECT  TEMP.CODTITULO         
               ,TEMP.CODFAVORECIDO     
               ,TEMP.NMFAVORECIDO        
               ,CASE  WHEN PAG.STATUS IS NULL THEN TEMP.STATUS 
                      WHEN TEMP.STATUS = 'BAIXADO_MANUALMENTE' THEN  TEMP.STATUS  
                      WHEN PAG.STATUS = 'CANCELADO' THEN  TEMP.STATUS               
                    ELSE PAG.STATUS END STATUS
               ,TEMP.CPFCNPJ           
               ,TEMP.DATEMISSAO        
               ,TEMP.DATPGTO           
               ,TEMP.DATREALPGTO       
               ,TEMP.BANCO             
               ,TEMP.AGENCIA           
               ,TEMP.CONTACORRENTE     
               ,TEMP.VALLIQ           
               ,TEMP.VALTOTAL
               ,TEMP.VALBRUTO          
               ,TEMP.VALTAXA           
               ,TEMP.TIPCARTAO         
               ,TEMP.TIPOTITULO
               ,LTRIM(RTRIM(ISNULL(PAG.OCORRENCIARETORNO,''))) AS MSGRETORNO
          FROM @TABTEMP                 TEMP
     LEFT JOIN PAGNET_TITULOS_PAGOS     PAG ON PAG.SEUNUMERO = TEMP.SEUNUMERO   
         WHERE ((@status                 = '') OR CASE WHEN PAG.STATUS IS NULL THEN TEMP.STATUS  ELSE PAG.STATUS END = @status)
    ORDER BY TEMP.NMFAVORECIDO, TEMP.CPFCNPJ

END

