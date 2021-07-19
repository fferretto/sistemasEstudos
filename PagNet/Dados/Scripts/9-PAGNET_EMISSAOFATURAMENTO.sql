BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_EMISSAOFATURAMENTO')
	BEGIN
			DROP TABLE PAGNET_EMISSAOFATURAMENTO
	END
END
BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_EMISSAOFATURAMENTO_LOG')
	BEGIN
			DROP TABLE PAGNET_EMISSAOFATURAMENTO_LOG
	END
END
BEGIN
    CREATE TABLE PAGNET_EMISSAOFATURAMENTO
    (
         CODEMISSAOFATURAMENTO      INT             NOT NULL  PRIMARY KEY
        ,[STATUS]                   NVARCHAR(50) 
        ,CODCLIENTE       	        INT             NULL  FOREIGN KEY REFERENCES PAGNET_CADCLIENTE(CODCLIENTE)
        ,CODBORDERO       	        INT             NULL      
        ,CODEMPRESA                 INT             NOT NULL  FOREIGN KEY REFERENCES PAGNET_CADEMPRESA(CODEMPRESA)
        ,CODFORMAFATURAMENTO        INT             NOT NULL  FOREIGN KEY REFERENCES PAGNET_FORMAS_FATURAMENTO(CODFORMAFATURAMENTO)
        ,DATVENCIMENTO              DATETIME        NOT NULL 
        ,DATPGTO                    DATETIME        NULL 
        ,VLPGTO                     DECIMAL(15,2)   NULL 
        ,ORIGEM                     NVARCHAR(90)    NOT NULL
        ,TIPOFATURAMENTO            NVARCHAR(50)    NOT NULL
        ,NROREF_NETCARD             NVARCHAR(50)    NULL
        ,SEUNUMERO                  CHAR(15)
        ,VALOR                      DECIMAL(15,2)   NOT NULL 
        ,DATSOLICITACAO             DATETIME        NOT NULL
        ,DATSEGUNDODESCONTO         DATETIME
        ,VLDESCONTO                 DECIMAL(15,2)
        ,VLSEGUNDODESCONTO          DECIMAL(15,2)
        ,MENSAGEMARQUIVOREMESSA     NVARCHAR(100) 
        ,MENSAGEMINSTRUCOESCAIXA    NVARCHAR(100) 
        ,NRODOCUMENTO               NVARCHAR(100)    
        ,VLDESCONTOCONCEDIDO        DECIMAL(15,2)
        ,JUROSCOBRADO               DECIMAL(15,2)
        ,MULTACOBRADA               DECIMAL(15,2)    
        ,CODEMISSAOFATURAMENTOPAI   INT   
        ,PARCELA                    INT   
        ,TOTALPARCELA               INT   
        ,VALORPARCELA               DECIMAL(15,2)   
        ,CODCONTACORRENTE           INT             NULL FOREIGN KEY REFERENCES PAGNET_CONTACORRENTE(CODCONTACORRENTE)   
        ,CODPLANOCONTAS             INT
        ,CODMOVIMENTACAO            INT
    )

END
BEGIN
  CREATE TABLE PAGNET_EMISSAOFATURAMENTO_LOG
    (
         CODEMISSAOFATURAMENTO_LOG  INT             NOT NULL  PRIMARY KEY
        ,CODEMISSAOFATURAMENTO      INT             NOT NULL  
        ,[STATUS]                   NVARCHAR(50) 
        ,CODCLIENTE       	        INT             NULL 
        ,CODBORDERO       	        INT             NULL     
        ,CODEMPRESA                 INT             NOT NULL 
        ,CODFORMAFATURAMENTO        INT             NOT NULL 
        ,DATVENCIMENTO              DATETIME        NOT NULL 
        ,DATPGTO                    DATETIME        NULL 
        ,VLPGTO                     DECIMAL(15,2)   NULL 
        ,ORIGEM                     NVARCHAR(90)    NOT NULL
        ,TIPOFATURAMENTO            NVARCHAR(50)    NOT NULL
        ,NROREF_NETCARD             NVARCHAR(50)    NULL
        ,SEUNUMERO                  CHAR(15)
        ,VALOR                      DECIMAL(15,2)   NOT NULL 
        ,DATSOLICITACAO             DATETIME        NOT NULL
        ,DATSEGUNDODESCONTO         DATETIME
        ,VLDESCONTO                 DECIMAL(15,2)
        ,VLSEGUNDODESCONTO          DECIMAL(15,2)
        ,MENSAGEMARQUIVOREMESSA     NVARCHAR(100) 
        ,MENSAGEMINSTRUCOESCAIXA    NVARCHAR(100) 
        ,CODUSUARIO                 INT             NOT NULL 
        ,DATINCLOG                  DATETIME        NOT NULL 
        ,DESCLOG                    VARCHAR(8000)   NOT NULL
        ,NRODOCUMENTO               NVARCHAR(100)               
        ,VLDESCONTOCONCEDIDO        DECIMAL(15,2)
        ,JUROSCOBRADO               DECIMAL(15,2)
        ,MULTACOBRADA               DECIMAL(15,2)     
        ,CODEMISSAOFATURAMENTOPAI   INT   
        ,PARCELA                    INT   
        ,TOTALPARCELA               INT   
        ,VALORPARCELA               DECIMAL(15,2)        
        ,CODCONTACORRENTE           INT             NULL 
        ,CODPLANOCONTAS             INT
        ,CODMOVIMENTACAO            INT
    )
	
END	
