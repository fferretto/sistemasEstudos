BEGIN
	IF EXISTS (SELECT 1 
                     FROM INFORMATION_SCHEMA.TABLES 
                     WHERE TABLE_SCHEMA = 'dbo' 
                     AND  TABLE_NAME = 'PAGNET_PARAM_ARQUIVO_CONCILIACAO')
    BEGIN
            DROP TABLE PagNet_Param_Arquivo_Conciliacao
    END
END
BEGIN
    IF EXISTS (SELECT 1 
                     FROM INFORMATION_SCHEMA.TABLES 
                     WHERE TABLE_SCHEMA = 'dbo' 
                     AND  TABLE_NAME = 'PAGNET_ARQUIVO_CONCILIACAO')
    BEGIN
            DROP TABLE PagNet_Arquivo_Conciliacao
    END
END
BEGIN
    IF EXISTS (SELECT 1 
                     FROM INFORMATION_SCHEMA.TABLES 
                     WHERE TABLE_SCHEMA = 'dbo' 
                     AND  TABLE_NAME = 'PAGNET_FORMA_VERIFICACAO_ARQUIVO')
    BEGIN
            DROP TABLE PagNet_Forma_Verificacao_Arquivo
    END
END
BEGIN
    CREATE TABLE PAGNET_FORMA_VERIFICACAO_ARQUIVO
    (
         CODFORMAVERIFICACAO    INT             NOT NULL PRIMARY KEY
        ,DESCRICAO              NVARCHAR(200)   NOT NULL
    )
END
BEGIN
    CREATE TABLE PAGNET_ARQUIVO_CONCILIACAO
    (
         CODARQUIVO_CONCILIACAO     INT             NOT NULL  PRIMARY KEY
        ,CODCLIENTE                 INT             NOT NULL FOREIGN KEY REFERENCES PAGNET_CADCLIENTE(CODCLIENTE)
        ,CODFORMAVERIFICACAO        INT             NOT NULL FOREIGN KEY REFERENCES PAGNET_FORMA_VERIFICACAO_ARQUIVO(CODFORMAVERIFICACAO)
        ,EXTENSAOARQUI_RET          CHAR(5)         NOT NULL
        ,EXTENSAOARQUI_REM          CHAR(5)         NOT NULL
        ,ATIVO                      CHAR(1)         NOT NULL
    )
END
BEGIN
    CREATE TABLE PAGNET_PARAM_ARQUIVO_CONCILIACAO
    (
         CODPARAM                   INT             NOT NULL PRIMARY KEY
        ,CODARQUIVO_CONCILIACAO     INT             NOT NULL FOREIGN KEY REFERENCES PAGNET_ARQUIVO_CONCILIACAO(CODARQUIVO_CONCILIACAO)
        ,TIPOARQUIVO                CHAR(3)         NOT NULL
        ,CAMPO                      NVARCHAR(50)    NOT NULL
        ,LINHAINICIO                INT             NOT NULL
        ,POSICAO_CSV                INT             NOT NULL 
        ,POSICAOINI_TXT             INT             NOT NULL 
        ,POSICAOFIM_TXT             INT             NOT NULL
    )
END

INSERT INTO PAGNET_FORMA_VERIFICACAO_ARQUIVO (CODFORMAVERIFICACAO,DESCRICAO) VALUES(1,'No Arquivo de retorno possui apenas usuários que NÃO CONSEGUIRAM descontar o valor')
INSERT INTO PAGNET_FORMA_VERIFICACAO_ARQUIVO (CODFORMAVERIFICACAO,DESCRICAO) VALUES(2,'No Arquivo de retorno possui apenas usuários que CONSEGUIRAM descontar o valor')
INSERT INTO PAGNET_FORMA_VERIFICACAO_ARQUIVO (CODFORMAVERIFICACAO,DESCRICAO) VALUES(3,'No Arquivo de retorno possui todos os usuários e os que eles conseguiram descontar o campo de valor está vazio')