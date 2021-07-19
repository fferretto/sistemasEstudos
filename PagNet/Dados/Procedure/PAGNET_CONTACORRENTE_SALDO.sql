
IF EXISTS (SELECT 1 
				 FROM INFORMATION_SCHEMA.TABLES 
				 WHERE TABLE_SCHEMA = 'DBO' 
				 AND  TABLE_NAME = 'PAGNET_CONTACORRENTE_SALDO')
BEGIN
		DROP TABLE PAGNET_CONTACORRENTE_SALDO
END
ELSE
BEGIN
	CREATE TABLE PAGNET_CONTACORRENTE_SALDO
	(
		 CODSALDO           INT             NOT NULL    PRIMARY KEY
		,CODEMPRESA         INT             NOT NULL    FOREIGN KEY REFERENCES PAGNET_CADEMPRESA(CODEMPRESA) 
		,CODCONTACORRENTE   INT             NOT NULL    FOREIGN KEY REFERENCES PAGNET_CONTACORRENTE(CODCONTACORRENTE) 
		,DATLANCAMENTO      DATETIME        NOT NULL
		,SALDO              DECIMAL(18,2)   NOT NULL
	)
END
