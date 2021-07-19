
BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'DBO' 
					 AND  TABLE_NAME = 'PAGNET_EMISSAO_TITULOS_LOG')
	BEGIN
			DROP TABLE PAGNET_EMISSAO_TITULOS_LOG
	END
END
BEGIN
    CREATE TABLE PAGNET_EMISSAO_TITULOS_LOG
    (
        CODTITULO_LOG                   INT             NOT NULL PRIMARY KEY
        ,CODTITULO                      INT             NOT NULL
        ,CODTITULOPAI                   INT
        ,[STATUS]                       NVARCHAR(60)   
        ,CODEMPRESA                     INT            
        ,CODFAVORECIDO                  INT            
        ,DATEMISSAO                     DATETIME       
        ,DATPGTO                        DATETIME       
        ,DATREALPGTO                    DATETIME       
        ,VALBRUTO                       DECIMAL(13,2)  
        ,VALLIQ                         DECIMAL(13,2)    
        ,VALTOTAL                       DECIMAL(13,2)  
        ,TIPOTITULO                     NVARCHAR(60)  
        ,ORIGEM                         NVARCHAR(20)
        ,SISTEMA                        INT             
        ,LINHADIGITAVEL                 NVARCHAR(60)   
        ,CODBORDERO                     INT            
        ,SEUNUMERO                      NVARCHAR(40)            
        ,CODUSUARIO                     INT                 NOT NULL 
        ,DATINCLOG                      DATETIME            NOT NULL 
        ,DESCLOG                        VARCHAR(500)        NOT NULL
        ,CODCONTACORRENTE               INT 
        ,CODPLANOCONTAS                 INT
    )
END
