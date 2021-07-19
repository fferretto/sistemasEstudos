
    IF NOT EXISTS(SELECT 1
              FROM   INFORMATION_SCHEMA.COLUMNS
              WHERE  TABLE_NAME = 'OPERADORA'
                     AND COLUMN_NAME = 'GERABOLETO') 
    BEGIN
        ALTER TABLE OPERADORA ADD GERABOLETO CHAR(1) NOT NULL DEFAULT 'N'
    END
GO
    IF NOT EXISTS(SELECT 1
              FROM   INFORMATION_SCHEMA.COLUMNS
              WHERE  TABLE_NAME = 'OPERADORA'
                     AND COLUMN_NAME = 'FAZPAGAMENTO') 
    BEGIN
        ALTER TABLE OPERADORA ADD FAZPAGAMENTO CHAR(1) NOT NULL DEFAULT 'N'
    END
GO
    IF NOT EXISTS(SELECT 1
              FROM   INFORMATION_SCHEMA.COLUMNS
              WHERE  TABLE_NAME = 'OPERADORA'
                     AND COLUMN_NAME = 'CAMINHOARQUIVO') 
    BEGIN
        ALTER TABLE OPERADORA ADD CAMINHOARQUIVO VARCHAR(200)
    END
GO
IF EXISTS (SELECT 1 FROM OPERADORA WHERE CODOPE = 999)
BEGIN
    DELETE FROM OPERADORA WHERE CODOPE = 999
END
    INSERT 
      INTO OPERADORA
        (
             CODOPE
            ,NOME
            ,BD_AUT
            ,BD_NC
            ,FLAG_VA
            ,FLAG_TESTE
            ,SERVIDOR
            ,DESC_PREFEITURA
            ,REAJUSTE
            ,DIA_VENCIMENTO
            ,MINIMO
            ,TIPO_CALC
            ,VALOR_UNITARIO
            ,VALOR_CNP
            ,DESCNPB
            ,DESC_CANC
            ,LICENCA
            ,SERVIDOR_AUT
            ,EXIBE_GRAFICO
            ,OPERADORA
            ,SERVIDOR_IIS
            ,NOMOPERAFIL
            ,CAMINHO_ARQ_FECH_CLI
            ,CAMINHO_ARQ_FECH_CRE
            ,GERABOLETO
            ,FAZPAGAMENTO
            ,CAMINHOARQUIVO
        )
       VALUES
       (
             999
            ,'Telenet'
            ,'GRAAL_AUTORIZADOR'
            ,'GRAAL_NETCARDPJ'
            ,'N'
            ,'N'
            ,'ZEUS'
            ,''
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,NULL
            ,'ZEUS'
            ,NULL
            ,NULL
            ,NULL
            ,'tln'
            ,NULL
            ,NULL
            ,'N'
            ,'N'
            ,'\\DESENV04\TesteGeracaoArquivo'
       )

GO

select top 1000 * from OPERADORA

