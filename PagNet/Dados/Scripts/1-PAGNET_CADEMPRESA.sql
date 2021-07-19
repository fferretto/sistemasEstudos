BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'DBO' 
					 AND  TABLE_NAME = 'PAGNET_CADEMPRESA')
	BEGIN
			DROP TABLE PAGNET_CADEMPRESA
	END
END
BEGIN
    CREATE TABLE PAGNET_CADEMPRESA
    (
         CODEMPRESA         INT             NOT NULL PRIMARY KEY
        ,RAZAOSOCIAL        VARCHAR(100)        NULL
        ,NMFANTASIA         VARCHAR(100)        NULL
        ,CNPJ               CHAR(14)        NOT NULL        
        ,CEP                CHAR(9)         NOT NULL 
        ,LOGRADOURO         VARCHAR(200)    NOT NULL
        ,NROLOGRADOURO      VARCHAR(20)     NOT NULL
        ,COMPLEMENTO        VARCHAR(60)     NOT NULL
        ,BAIRRO             VARCHAR(100)    NOT NULL
        ,CIDADE             VARCHAR(100)    NOT NULL    
        ,UF                 CHAR(2)         NOT NULL
        ,UTILIZANETCARD     CHAR(1)         NOT NULL
        ,CODSUBREDE         INT
        ,NMLOGIN            NVARCHAR(20)    NOT NULL
    )
END
