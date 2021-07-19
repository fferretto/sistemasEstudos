IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_ARQUIVO')
BEGIN
        DROP TABLE PAGNET_ARQUIVO
END
BEGIN
    CREATE TABLE PAGNET_ARQUIVO
    (
         CODARQUIVO           INT               NOT NULL PRIMARY KEY
        ,NMARQUIVO            NVARCHAR(100)     NOT NULL  --(NOME DO ARQUIVO)
        ,CODBANCO             CHAR(3)           NOT NULL FOREIGN KEY REFERENCES PAGNET_BANCO(CODBANCO)
		,STATUS               NVARCHAR(50)      NOT NULL 
        ,TIPARQUIVO           CHAR(3)           NOT NULL  --(REM = REMESSA; RET = RETORNO; LIQ = LIQUIDA��O)
		,NROSEQARQUIVO		  INT               NOT NULL
		,DTARQUIVO			  DATETIME			NOT NULL
		,VLTOTAL			  DECIMAL(13,2) 	NOT NULL
        ,QTREGISTRO           INT               NOT NULL
        ,CAMINHOARQUIVO       VARCHAR(1000)     NOT NULL 
    )
END