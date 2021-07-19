IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_LOGEMAILENVIADO')
BEGIN
        DROP TABLE PAGNET_LOGEMAILENVIADO
END
BEGIN
IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_CONTAEMAIL')
BEGIN
        DROP TABLE PAGNET_CONTAEMAIL
END
END
BEGIN
    CREATE TABLE PAGNET_CONTAEMAIL
    (
         CODCONTAEMAIL          INT                 NOT NULL PRIMARY KEY
        ,CODEMPRESA             INT
		,NMCONTAEMAIL           VARCHAR(100)        NOT NULL
		,EMAIL                  VARCHAR(100)        NOT NULL
		,SENHA                  VARCHAR(100)        NOT NULL
		,SERVIDOR               VARCHAR(50)         NOT NULL
        ,ENDERECOSMTP           VARCHAR(100)        NOT NULL 
        ,PORTA                  VARCHAR(20)         NOT NULL 
        ,CRIPTOGRAFIA           VARCHAR(20)         NOT NULL
        ,EMAILPRINCIPAL         CHAR(1)             NOT NULL
        ,ATIVO                  CHAR(1)             NOT NULL
    )
END
BEGIN
    CREATE TABLE PAGNET_LOGEMAILENVIADO
    (
         CODLOGEMAILENVIADO     INT                 NOT NULL  PRIMARY KEY
        ,CODUSUARIO             INT                 NOT NULL  FOREIGN KEY REFERENCES PAGNET_USUARIO(CODUSUARIO)
        ,CODCONTAEMAIL          INT                 NOT NULL  FOREIGN KEY REFERENCES PAGNET_CONTAEMAIL(CODCONTAEMAIL)
        ,CODEMISSAOBOLETO       INT                 NOT NULL  FOREIGN KEY REFERENCES PAGNET_EMISSAOBOLETO(CODEMISSAOBOLETO)
        ,EMAILDESTINO           VARCHAR(250)        NOT NULL
		,DTENVIO                DATETIME            NOT NULL
		,STATUS                 VARCHAR(60)         NOT NULL
		,MENSAGEM               VARCHAR(250)        NOT NULL
    )
END


