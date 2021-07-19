
IF EXISTS (SELECT 1 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'DBO' 
                 AND  TABLE_NAME = 'PAGNET_USUARIO')
BEGIN
        DROP TABLE PAGNET_USUARIO
END
GO
    CREATE TABLE PAGNET_USUARIO
    (
         CODUSUARIO         INT             NOT NULL IDENTITY(1,1) PRIMARY KEY
        ,NMUSUARIO          VARCHAR(100)        NULL
        ,LOGIN              VARCHAR(100)    NOT NULL
        ,SENHA              VARCHAR(200)    NOT NULL
        ,CPF                VARCHAR(14)
        ,CODSUBREDE         INT             NOT NULL 
        ,EMAIL              VARCHAR(200)
        ,CODOPE             SMALLINT        NOT NULL FOREIGN KEY REFERENCES OPERADORA(CODOPE)
        ,GERABOLETO         CHAR(1)         NOT NULL DEFAULT 'S'
        ,FAZPAGAMENTO       CHAR(1)         NOT NULL DEFAULT 'S'
        ,ADMINISTRADOR      CHAR(1)         NOT NULL DEFAULT 'S'
        ,VISIVEL            CHAR(1)         NOT NULL DEFAULT 'S'
        ,ATIVO              CHAR(1)         NOT NULL DEFAULT 'S'
    )
GO

INSERT INTO PAGNET_USUARIO
 SELECT nmUsuario = 'Administrador Telenet'
       ,'admin@tln'
       ,'ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f'
       ,'000000000000'
       ,1
       ,'suporte@tln.com.br'
       ,999
       ,'S'
       ,'S'
       ,'S'
       ,'N'
       ,'S'

GO

select * from PAGNET_USUARIO