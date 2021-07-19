BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'DBO' 
					 AND  TABLE_NAME = 'PAGNET_TRANSMISSAOARQUIVO')
	BEGIN
			DROP TABLE PAGNET_TRANSMISSAOARQUIVO
	END
END
BEGIN
CREATE TABLE PAGNET_TRANSMISSAOARQUIVO
    (
         CODTRANSMISSAOARQUIVO  INT                 NOT NULL PRIMARY KEY
        ,CODCONTACORRENTE       INT                 NOT NULL FOREIGN KEY REFERENCES PAGNET_CONTACORRENTE(CODCONTACORRENTE)
		,TIPOARQUIVO			VARCHAR(30)         NOT NULL
		,FORMATRANSMISSAO       VARCHAR(30)         NOT NULL
		,LOGINTRANSMISSAO       VARCHAR(150)            NULL
		,SENHATRANSMISSAO  VARCHAR(80)             NULL
        ,CAMINHOREM             VARCHAR(300)            NULL 
        ,CAMINHORET             VARCHAR(300)			NULL
        ,CAMINHOAUX				VARCHAR(300)			NULL
    )

END

