BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'DBO' 
					 AND  TABLE_NAME = 'PAGNET_CONTACORRENTE')
	BEGIN
			DROP TABLE PAGNET_CONTACORRENTE
	END
END
BEGIN
CREATE TABLE PAGNET_CONTACORRENTE
    (
         CODCONTACORRENTE       INT                 NOT NULL PRIMARY KEY
		,NMCONTACORRENTE        VARCHAR(100)        NOT NULL
		,NMEMPRESA              VARCHAR(150)        NOT NULL
		,CPFCNPJ                VARCHAR(14)         NOT NULL
        ,CODBANCO               VARCHAR(3)          NOT NULL 
        ,CODEMPRESA             INT 
        ,CODOPERACAOCC          VARCHAR(3)
        ,NROCONTACORRENTE       VARCHAR(12)         NOT NULL 
        ,DIGITOCC	            CHAR(1)             NOT NULL 
        ,CONTAMOVIEMNTO         VARCHAR(10)         NOT NULL 
        ,AGENCIA                VARCHAR(6)          NOT NULL 
		,DIGITOAGENCIA		    VARCHAR(1)          NOT NULL 
        ,QTPOSICAOARQPGTO       INT
        ,QTPOSICAOARQBOL        INT
        ,CODCONVENIOBOL         VARCHAR(20)
        ,CODCONVENIOPAG         VARCHAR(20)
        ,CODTRANSMISSAO         VARCHAR(20)
        ,CARTEIRAREMESSA        VARCHAR(40) 
        ,CARTEIRABOL   		    VARCHAR(40)        
        ,VALTED                 DECIMAL(13,2)           NULL
        ,VALMINIMOTED           DECIMAL(13,2)
        ,VALMINIMOCC            DECIMAL(13,2)
        ,COBRAJUROS             CHAR(1)         
        ,VLJUROSDIAATRASO       DECIMAL(15,2)   
        ,PERCJUROS              DECIMAL(5,2) 
        ,COBRAMULTA             CHAR(1)         
        ,VLMULTADIAATRASO       DECIMAL(15,2)
        ,PERCMULTA              DECIMAL(5,2)
        ,CODPRIMEIRAINSTCOBRA   INT             
        ,CODSEGUNDAINSTCOBRA    INT             
        ,TAXAEMISSAOBOLETO      DECIMAL(13,2)
        ,AGRUPARFATURAMENTOSDIA CHAR(1)         
        ,CODCEDENTE             VARCHAR(20)
        ,DVCODCEDENTE           VARCHAR(5)  
        ,ATIVO                  CHAR(1)             NOT NULL
    )

END

