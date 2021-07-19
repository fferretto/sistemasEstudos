
IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_CADCLIENTE')
BEGIN
        DROP TABLE PagNet_CadCliente
END
BEGIN
    IF EXISTS (SELECT 1 
                     FROM INFORMATION_SCHEMA.TABLES 
                     WHERE TABLE_SCHEMA = 'DBO' 
                     AND  TABLE_NAME = 'PAGNET_CADCLIENTE_LOG')
    BEGIN
            DROP TABLE PagNet_CadCliente_Log
    END
END
BEGIN
    CREATE TABLE PAGNET_CADCLIENTE
    (
         CODCLIENTE             INT             NOT NULL  PRIMARY KEY
        ,NMCLIENTE              VARCHAR(100)    NOT NULL    
        ,CPFCNPJ                VARCHAR(14)     NOT NULL   
        ,CODEMPRESA             INT             NOT NULL FOREIGN KEY REFERENCES PAGNET_CADEMPRESA(CODEMPRESA)
        ,CODFORMAFATURAMENTO    INT             NOT NULL FOREIGN KEY REFERENCES PAGNET_FORMAS_FATURAMENTO(CODFORMAFATURAMENTO)
        ,CEP                    VARCHAR(9)      NOT NULL    
        ,LOGRADOURO             VARCHAR(200)    
        ,NROLOGRADOURO          VARCHAR(20)
        ,COMPLEMENTO            VARCHAR(60)
        ,BAIRRO                 VARCHAR(100)
        ,CIDADE                 VARCHAR(100)
        ,UF                     CHAR(2)
        ,COBRANCADIFERENCIADA   CHAR(1)         NOT NULL
        ,COBRAJUROS             CHAR(1)         NOT NULL
        ,VLJUROSDIAATRASO       DECIMAL(15,2)     
        ,PERCJUROS              DECIMAL(5,2) 
        ,COBRAMULTA             CHAR(1)         NOT NULL
        ,VLMULTADIAATRASO       DECIMAL(15,2)
        ,PERCMULTA              DECIMAL(5,2)
        ,EMAIL                  NVARCHAR(200)
        ,CODPRIMEIRAINSTCOBRA   INT             
        ,CODSEGUNDAINSTCOBRA    INT             
        ,TAXAEMISSAOBOLETO      DECIMAL(13,2)
        ,AGRUPARFATURAMENTOSDIA CHAR(1)         NOT NULL
        ,ATIVO                  CHAR(1)         NOT NULL
        ,TIPOCLIENTE            CHAR(1)         

    )
END
BEGIN
    CREATE TABLE PAGNET_CADCLIENTE_LOG
    (    
         CODCLIENTE_LOG         INT             NOT NULL  PRIMARY KEY
        ,CODCLIENTE             INT            
        ,NMCLIENTE              VARCHAR(100)       
        ,CPFCNPJ                VARCHAR(14)     
        ,CODEMPRESA             INT 
        ,CODFORMAFATURAMENTO    INT           
        ,CEP                    VARCHAR(9)         
        ,LOGRADOURO             VARCHAR(200)
        ,NROLOGRADOURO          VARCHAR(20)
        ,COMPLEMENTO            VARCHAR(60)
        ,BAIRRO                 VARCHAR(100)
        ,CIDADE                 VARCHAR(100)
        ,UF                     CHAR(2)
        ,COBRANCADIFERENCIADA   CHAR(1)
        ,COBRAJUROS             CHAR(1)  
        ,VLJUROSDIAATRASO       DECIMAL(15,2)     
        ,PERCJUROS              DECIMAL(5,2) 
        ,COBRAMULTA             CHAR(1)        
        ,VLMULTADIAATRASO       DECIMAL(15,2)
        ,PERCMULTA              DECIMAL(5,2)
        ,EMAIL                  NVARCHAR(200)
        ,CODPRIMEIRAINSTCOBRA   INT             
        ,CODSEGUNDAINSTCOBRA    INT             
        ,TAXAEMISSAOBOLETO      DECIMAL(13,2)
        ,AGRUPARFATURAMENTOSDIA CHAR(1)         NOT NULL
        ,ATIVO                  CHAR(1)   
        ,CODUSUARIO             INT                 
        ,DATINCLOG              DATETIME          
        ,DESCLOG                VARCHAR(500)   
        ,TIPOCLIENTE            CHAR(1)          

    )
END