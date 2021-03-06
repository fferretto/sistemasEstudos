IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_INSTRUCAOEMAIL')
BEGIN
        DROP TABLE PAGNET_INSTRUCAOEMAIL
END
BEGIN
    CREATE TABLE PAGNET_INSTRUCAOEMAIL
    (
         CODINSTRUCAOEMAIL      INT                 NOT NULL PRIMARY KEY
        ,CODEMPRESA             INT                 NOT NULL FOREIGN KEY REFERENCES PAGNET_CADEMPRESA(CODEMPRESA)
		,ASSUNTO                VARCHAR(100)        NOT NULL 
		,MENSAGEM               VARCHAR(1000)       NOT NULL
    )
END
