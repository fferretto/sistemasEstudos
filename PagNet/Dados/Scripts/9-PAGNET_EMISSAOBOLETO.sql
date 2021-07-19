BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_EMISSAOBOLETO')
	BEGIN
			DROP TABLE PagNet_EmissaoBoleto
	END
END
BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_EMISSAOBOLETO_LOG')
	BEGIN
			DROP TABLE PagNet_EmissaoBoleto_Log
	END

END
BEGIN
    CREATE TABLE PAGNET_EMISSAOBOLETO
    (
         CODEMISSAOBOLETO           INT             NOT NULL  PRIMARY KEY
        ,[STATUS]                   NVARCHAR(50) 
        ,CODCLIENTE       	        INT             NOT NULL  FOREIGN KEY REFERENCES PAGNET_CADCLIENTE(CODCLIENTE)
        ,CODEMPRESA                 INT             NOT NULL  FOREIGN KEY REFERENCES PAGNET_CADEMPRESA(CODEMPRESA)
        ,CODCONTACORRENTE           INT             NOT NULL  FOREIGN KEY REFERENCES PAGNET_CONTACORRENTE(CODCONTACORRENTE)
        ,DATVENCIMENTO              DATETIME        NOT NULL 
        ,DATPGTO                    DATETIME        NULL 
        ,VLPGTO                     DECIMAL(15,2)   NULL 
        ,NOSSONUMERO                NVARCHAR(15)
        ,CODOCORRENCIA              INT
        ,SEUNUMERO                  CHAR(15)
        ,VALOR                      DECIMAL(15,2)   NOT NULL 
        ,DATSOLICITACAO             DATETIME        NOT NULL
        ,DATREFERENCIA              DATETIME        NOT NULL
        ,DATSEGUNDODESCONTO         DATETIME
        ,VLDESCONTO                 DECIMAL(15,2)
        ,VLSEGUNDODESCONTO          DECIMAL(15,2)
        ,MENSAGEMARQUIVOREMESSA     NVARCHAR(100) 
        ,MENSAGEMINSTRUCOESCAIXA    NVARCHAR(100) 
        ,NUMCONTROLE                NVARCHAR(25)
        ,OCORRENCIARETBOL           NVARCHAR(250) 
        ,NMBOLETOGERADO             nvarchar(200)
        ,DESCRICAOOCORRENCIARETBOL  VARCHAR(8000) 
        ,CODARQUIVO                 INT
        ,BOLETOENVIADO              CHAR(1)
    )

END