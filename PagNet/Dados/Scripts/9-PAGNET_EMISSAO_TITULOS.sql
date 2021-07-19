BEGIN
	IF EXISTS (SELECT 1 
				 FROM INFORMATION_SCHEMA.TABLES 
				 WHERE TABLE_SCHEMA = 'DBO' 
				   AND TABLE_NAME = 'PAGNET_EMISSAO_TITULOS')
	BEGIN
			DROP TABLE PAGNET_EMISSAO_TITULOS
	END
END
BEGIN
    CREATE TABLE PAGNET_EMISSAO_TITULOS
    (
         CODTITULO                      INT             NOT NULL    PRIMARY KEY
        ,CODTITULOPAI                   INT
        ,[STATUS]                       NVARCHAR(60)    NOT NULL
        ,CODEMPRESA                     INT             NOT NULL    FOREIGN KEY REFERENCES PAGNET_CADEMPRESA(CODEMPRESA)
        ,CODFAVORECIDO                  INT             NULL    FOREIGN KEY REFERENCES PAGNET_CADFAVORECIDO(CODFAVORECIDO)
        ,DATEMISSAO                     DATETIME        NOT NULL
        ,DATPGTO                        DATETIME        NOT NULL
        ,DATREALPGTO                    DATETIME        NOT NULL
        ,VALBRUTO                       DECIMAL(13,2)   NOT NULL
        ,VALLIQ                         DECIMAL(13,2)   NOT NULL      
        ,VALTOTAL                       DECIMAL(13,2)   NOT NULL  
        ,TIPOTITULO                     NVARCHAR(60)  
        ,ORIGEM                         NVARCHAR(20)
        ,SISTEMA                        INT             NULL    
        ,LINHADIGITAVEL                 NVARCHAR(60)    NULL     
        ,CODBORDERO                     INT             NULL  
        ,SEUNUMERO                      NVARCHAR(40)
        ,CODCONTACORRENTE               INT             NULL FOREIGN KEY REFERENCES PAGNET_CONTACORRENTE(CODCONTACORRENTE)
        ,CODPLANOCONTAS                 INT
    )
END


