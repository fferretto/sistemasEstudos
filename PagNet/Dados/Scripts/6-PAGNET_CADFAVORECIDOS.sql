
IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_CADFAVORECIDO')
BEGIN
        DROP TABLE PAGNET_CADFAVORECIDO_CONFIG
<<<<<<< HEAD
=======
        DROP TABLE PAGNET_CADFAVORECIDO_LOG
>>>>>>> remotes/origin/master
        DROP TABLE PAGNET_CADFAVORECIDO
        DROP TABLE PAGNET_CADFAVORECIDO_LOG
END
BEGIN
    CREATE TABLE PAGNET_CADFAVORECIDO
    (
         CODFAVORECIDO      INT             NOT NULL PRIMARY KEY
        ,NMFAVORECIDO         VARCHAR(100)  NULL
        ,CPFCNPJ            VARCHAR(14)     NOT NULL
        ,CODCEN             INT             NULL
        ,BANCO              CHAR(3)
        ,AGENCIA            CHAR(5)
        ,DVAGENCIA          CHAR(1)
        ,OPE                VARCHAR(3)
        ,CONTACORRENTE      VARCHAR(12)
        ,DVCONTACORRENTE    CHAR(1)
        ,CEP                CHAR(9)         
        ,LOGRADOURO         VARCHAR(200)
        ,NROLOGRADOURO      VARCHAR(20)
        ,COMPLEMENTO        VARCHAR(60)
        ,BAIRRO             VARCHAR(100)
        ,CIDADE             VARCHAR(100)
        ,UF                 CHAR(2)
        ,ATIVO              CHAR(1)         NOT NULL
<<<<<<< HEAD
        ,CODEMPRESA         INT             NULL     
        ,DTALTERACAO        DATETIME        NULL   
    )
END

BEGIN
    CREATE TABLE PAGNET_CADFAVORECIDO_LOG
    (   
         CODFAVORECIDO_LOG  INT NOT NULL PRIMARY KEY
        ,CODFAVORECIDO      INT NOT NULL FOREIGN KEY REFERENCES PAGNET_CADFAVORECIDO(CODFAVORECIDO)      
        ,NMFAVORECIDO       VARCHAR(100)     
        ,CPFCNPJ            VARCHAR(14)     
        ,CODCEN             INT           
        ,BANCO              CHAR(3)
        ,AGENCIA            CHAR(5)
        ,DVAGENCIA          CHAR(1)
        ,OPE                VARCHAR(3)
        ,CONTACORRENTE      VARCHAR(12)
        ,DVCONTACORRENTE    CHAR(1)
        ,CEP                CHAR(9)         
        ,LOGRADOURO         VARCHAR(200)
        ,NROLOGRADOURO      VARCHAR(20)
        ,COMPLEMENTO        VARCHAR(60)
        ,BAIRRO             VARCHAR(100)
        ,CIDADE             VARCHAR(100)
        ,UF                 CHAR(2)
        ,ATIVO              CHAR(1)         NOT NULL
        ,CODUSUARIO         INT             NOT NULL    
        ,DATINCLOG          DATETIME        NOT NULL  
        ,DESCLOG            VARCHAR(500)    NOT NULL 
        ,CODEMPRESA         INT                 NULL  
        ,DTALTERACAO        DATETIME            NULL

    )
END

BEGIN
    CREATE TABLE PAGNET_CADFAVORECIDO_CONFIG
    (
        CODFAVORECIDOCONFIG     INT                 NOT NULL PRIMARY KEY,
        CODFAVORECIDO           INT                 NOT NULL FOREIGN KEY REFERENCES PAGNET_CADFAVORECIDO(CODFAVORECIDO),
        CODEMPRESA              int                 NOT NULL,
        REGRADIFERENCIADA       CHAR(1)             NOT NULL,
        VALTED                  DECIMAL(15,2)       NOT NULL,
        VALMINIMOTED            DECIMAL(15,2)       NOT NULL,
        VALMINIMOCC             DECIMAL(15,2)       NOT NULL,
        CODCONTACORRENTE        INT
=======
        ,CODEMPRESA         INT             NULL   
        ,DTINCLUSAO         DATETIME
        ,DTALTERACAO        DATETIME
>>>>>>> remotes/origin/master
    )
END
BEGIN
    CREATE TABLE PAGNET_CADFAVORECIDO_LOG
    (   
         CODFAVORECIDO_LOG  INT NOT NULL PRIMARY KEY
        ,CODFAVORECIDO      INT             
        ,NMFAVORECIDO       VARCHAR(100)     
        ,CPFCNPJ            VARCHAR(14)     
        ,CODCEN             INT           
        ,BANCO              CHAR(3)
        ,AGENCIA            CHAR(5)
        ,DVAGENCIA          CHAR(1)
        ,OPE                VARCHAR(3)
        ,CONTACORRENTE      VARCHAR(12)
        ,DVCONTACORRENTE    CHAR(1)
        ,CEP                CHAR(9)         
        ,LOGRADOURO         VARCHAR(200)
        ,NROLOGRADOURO      VARCHAR(20)
        ,COMPLEMENTO        VARCHAR(60)
        ,BAIRRO             VARCHAR(100)
        ,CIDADE             VARCHAR(100)
        ,UF                 CHAR(2)
        ,ATIVO              CHAR(1)        
        ,CODUSUARIO         INT                 
        ,DATINCLOG          DATETIME          
        ,DESCLOG            VARCHAR(500)     
        ,CODEMPRESA         INT             NULL    
        ,DTINCLUSAO         DATETIME
        ,DTALTERACAO        DATETIME

    )
END
BEGIN
    CREATE TABLE PAGNET_CADFAVORECIDO_CONFIG
    (
        CODFAVORECIDOCONFIG     INT                 NOT NULL PRIMARY KEY,
        CODFAVORECIDO           INT                 NOT NULL FOREIGN KEY REFERENCES PAGNET_CADFAVORECIDO(CODFAVORECIDO),
        CODEMPRESA              INT                 NOT NULL,        
        REGRADIFERENCIADA       CHAR(1)             NOT NULL,
        VALTED                  DECIMAL(15,2)       NOT NULL,
        VALMINIMOTED            DECIMAL(15,2)       NOT NULL,
        VALMINIMOCC             DECIMAL(15,2)       NOT NULL,
        CODCONTACORRENTE        INT                 NULL
    )
END
