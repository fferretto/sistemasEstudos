/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_CONSCARGA_PGTO          Versao 1.0                                    */         
/* CRIAÇÃO : ALEX 05/07/2019                                                  */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/* CONSULTA AS CARGAS A PAGAR PARA GERAR BOLETO                               */    
/* PARAMETROS:                                                                */     
/*                                                                            */    
/*                                                                            */      
/*----------------------------------------------------------------------------*/  

CREATE PROCEDURE [dbo].[PROC_PAGNET_INC_CLIENTE_USUARIO_NC]
                        @CPF              NVARCHAR(11)
                       ,@SISTEMA          INT 
                       ,@CODEMPRESA       INT 
                       ,@CODIGOUSUARIO_PN INT 
AS

BEGIN

    -------------TESTE

    --DECLARE  @CPF        NVARCHAR(20)
    --        ,@SISTEMA    INT
    --        ,@CODEMPRESA     INT 
    --        ,@CODIGOUSUARIO_PN INT 
	
    --SET @CPF = '08547535675'
    --SET @SISTEMA = 0
    --SET @CODEMPRESA = 1
    --SET @CODIGOUSUARIO_PN = 24

    ------------------

    DECLARE @NOVOCODCLIENTE INT
        
    BEGIN TRY

        DECLARE @TABTEMP TABLE
        (
             NOMEUSUARIO        NVARCHAR(200)
            ,CPF                NVARCHAR(20)
            ,BANCO              NVARCHAR(4)
            ,AGENCIA            NVARCHAR(20)
            ,DVAGENCIA          NVARCHAR(1)
            ,OPE                NVARCHAR(3)
            ,CONTACORRENTE      NVARCHAR(20)
            ,DVCONTACORRENTE    NVARCHAR(5)
            ,CEP                NVARCHAR(20)
            ,LOGRADOURO         NVARCHAR(200)
            ,NROLOGRADOURO      NVARCHAR(200)
            ,COMPLEMENTO        NVARCHAR(200)
            ,BAIRRO             NVARCHAR(200)
            ,CIDADE             NVARCHAR(200)
            ,UF                 NVARCHAR(2)
        )
        if @SISTEMA = 1
        BEGIN

            INSERT INTO @TABTEMP
            SELECT  NOMUSU,
                    CPF,
                    '' AS BANCO,
                    '' AS AGENCIA,
                    '' AS DVAGENCIA,
                    '' AS OPE,
                    '' AS CONTA,
                    '' AS DVCONTACORRENTE,
                    CEP,
                    ENDUSU,
                    ENDUSUCOM,
                    ENDCPL,
                    BAIRRO,
                    LOCALIDADE,
                    UF    
              FROM VUSUARIOVA
             WHERE CPF = @CPF 
               AND NUMDEP = 0
        END
        ELSE --SISTEMA 0
        BEGIN
    
            INSERT INTO @TABTEMP
            SELECT  NOMUSU,
                    CPF,
                    BANCO,
                    CASE WHEN CHARINDEX('-', AGENCIA) > 0 THEN SUBSTRING(AGENCIA, 1,CHARINDEX('-', AGENCIA) - 1) ELSE AGENCIA END AS AGENCIA,
                    CASE WHEN CHARINDEX('-', AGENCIA) > 0 THEN SUBSTRING(AGENCIA, CHARINDEX('-', AGENCIA) +1, 2) ELSE '0' END AS DVAGENCIA,
                    '0' AS OPE,
                    CASE WHEN CHARINDEX('-', CONTA) > 0 THEN SUBSTRING(CONTA, 1,CHARINDEX('-', CONTA) - 1) ELSE CONTA END AS CONTA,
                    CASE WHEN CHARINDEX('-', CONTA) > 0 THEN SUBSTRING(CONTA, CHARINDEX('-', CONTA) +1, 2) ELSE '0' END AS DVCONTACORRENTE,
                    CEP,
                    ENDUSU,
                    ENDNUMUSU,
                    ENDCPL,
                    BAIRRO,
                    LOCALIDADE,
                    UF    
              FROM VUSUARIO
             WHERE CPF = @CPF 
        END

        
        DECLARE @MAXCODCLIENTE_LOG INT
        SELECT @MAXCODCLIENTE_LOG = ISNULL(MAX(CODCLIENTE_LOG),0) FROM PAGNET_CADCLIENTE_LOG
        SET @MAXCODCLIENTE_LOG = @MAXCODCLIENTE_LOG + 1
        

    IF NOT EXISTS(SELECT 1 FROM PAGNET_CADCLIENTE WHERE CPFCNPJ = @CPF)
    BEGIN
        DECLARE @MAXCODCLIENTE INT
        SELECT @MAXCODCLIENTE = ISNULL(MAX(CODCLIENTE),0) FROM PAGNET_CADCLIENTE

        if (@MAXCODCLIENTE < 100000) 
        BEGIN
            set @MAXCODCLIENTE = 100000
        END
                
        SET @NOVOCODCLIENTE = (@MAXCODCLIENTE + 1)
        BEGIN TRANSACTION


         INSERT INTO PAGNET_CADCLIENTE
         SELECT @NOVOCODCLIENTE         AS CODCLIENTE,  
                t.NOMEUSUARIO           AS NMCLIENTE,
                T.CPF                   AS CPFCNPJ,
                @CODEMPRESA             AS CODEMPRESA,
                1                       AS CODFORMAFATURAMENTO,
                T.CEP                   AS CEP,
                T.LOGRADOURO            AS LOGRADOURO,
                T.NROLOGRADOURO         AS NROLOGRADOURO,
                T.COMPLEMENTO           AS COMPLEMENTO,
                T.BAIRRO                AS BAIRRO,
                T.CIDADE                AS CIDADE,
                T.UF                    AS UF,
                'N'                     AS COBRANCADIFERENCIADA,
                'N'                     AS COBRAJUROS,
                NULL                    AS VLJUROSDIAATRASO,
                NULL                    AS PERCJUROS,
                'N'                     AS COBRAMULTA,
                NULL                    AS VLMULTADIAATRASO,
                NULL                    AS PERCMULTA,
                ''                      AS EMAIL,
                0                       AS CODPRIMEIRAINSTCOBRA,
                0                       AS CODSEGUNDAINSTCOBRA,
                0                       AS TAXAEMISSAOBOLETO,
                'S'                     AS AGRUPARFATURAMENTOSDIA,
                'S'                     AS ATIVO,
                'F'                     AS TIPOCLIENTE
           FROM @TABTEMP T

         INSERT INTO PAGNET_CADCLIENTE_LOG
         SELECT @MAXCODCLIENTE_LOG      AS CODCLIENTE_LOG,
                @NOVOCODCLIENTE         AS CODCLIENTE,  
                t.NOMEUSUARIO           AS NMCLIENTE,
                T.CPF                   AS CPFCNPJ,
                @CODEMPRESA             AS CODEMPRESA,
                1                       AS CODFORMAFATURAMENTO,
                T.CEP                   AS CEP,
                T.LOGRADOURO            AS LOGRADOURO,
                T.NROLOGRADOURO         AS NROLOGRADOURO,
                T.COMPLEMENTO           AS COMPLEMENTO,
                T.BAIRRO                AS BAIRRO,
                T.CIDADE                AS CIDADE,
                T.UF                    AS UF,
                'N'                     AS COBRANCADIFERENCIADA,
                'N'                     AS COBRAJUROS,
                NULL                    AS VLJUROSDIAATRASO,
                NULL                    AS PERCJUROS,
                'N'                     AS COBRAMULTA,
                NULL                    AS VLMULTADIAATRASO,
                NULL                    AS PERCMULTA,
                ''                      AS EMAIL,
                0                       AS CODPRIMEIRAINSTCOBRA,
                0                       AS CODSEGUNDAINSTCOBRA,
                0                       AS TAXAEMISSAOBOLETO,
                'S'                     AS AGRUPARFATURAMENTOSDIA,
                'S'                     AS ATIVO,
                @CODIGOUSUARIO_PN       AS CODUSUARIO,
                GETDATE()               AS DATINCLOG,
                'Migração de usuário do netcard para o pagnet na forma de cliente, para emitir boletos.',
                'F'                     AS TIPOCLIENTE
           FROM @TABTEMP T

           COMMIT TRANSACTION
    END
    ELSE    --USUÁRIO JÁ INCLUÍDO NO PAGNET, NO CASO IREMOS ATUALIZAR O ENDEREÇO
    BEGIN
        BEGIN TRANSACTION

        UPDATE CLI
          SET CLI.CEP                   = T.CEP,
              CLI.LOGRADOURO            = T.LOGRADOURO,
              CLI.NROLOGRADOURO         = T.NROLOGRADOURO,
              CLI.COMPLEMENTO           = T.COMPLEMENTO,
              CLI.BAIRRO                = T.BAIRRO,
              CLI.CIDADE                = T.CIDADE,
              CLI.UF                    = T.UF
        FROM PAGNET_CADCLIENTE CLI
        JOIN @TABTEMP T ON T.CPF = CLI.CPFCNPJ

        SELECT @NOVOCODCLIENTE = CODCLIENTE FROM PAGNET_CADCLIENTE WHERE CPFCNPJ = @CPF

         INSERT INTO PAGNET_CADCLIENTE_LOG
         SELECT @MAXCODCLIENTE_LOG      AS CODCLIENTE_LOG,
                @NOVOCODCLIENTE         AS CODCLIENTE, 
                CLI.NMCLIENTE,
                CLI.CPFCNPJ,
                CLI.CODEMPRESA,
                CLI.CODFORMAFATURAMENTO,
                CLI.CEP,
                CLI.LOGRADOURO,
                CLI.NROLOGRADOURO,
                CLI.COMPLEMENTO,
                CLI.BAIRRO,
                CLI.CIDADE,
                CLI.UF,
                CLI.COBRANCADIFERENCIADA,
                CLI.COBRAJUROS,
                CLI.VLJUROSDIAATRASO,
                CLI.PERCJUROS,
                CLI.COBRAMULTA,
                CLI.VLMULTADIAATRASO,
                CLI.PERCMULTA,
                CLI.EMAIL,
                CLI.CODPRIMEIRAINSTCOBRA,
                CLI.CODSEGUNDAINSTCOBRA,
                CLI.TAXAEMISSAOBOLETO,
                CLI.AGRUPARFATURAMENTOSDIA,
                CLI.ATIVO,
                @CODIGOUSUARIO_PN       AS CODUSUARIO,
                GETDATE()               AS DATINCLOG,
                'Atualização dos dados de endereço do cliente com base no que está cadastrado no NetCard.',
                'F'                     AS TIPOCLIENTE
           FROM PAGNET_CADCLIENTE CLI
           WHERE CLI.CODCLIENTE = @NOVOCODCLIENTE

           COMMIT TRANSACTION

    END

    END TRY

    BEGIN CATCH
        ROLLBACK TRANSACTION
        SELECT ERROR_MESSAGE()
        SET @NOVOCODCLIENTE = 0
    END CATCH
    
    SELECT @NOVOCODCLIENTE AS CODCLIENTE

    SET NOCOUNT OFF

END
