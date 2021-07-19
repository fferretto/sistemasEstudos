
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

CREATE PROCEDURE [dbo].[PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC]
                        @CPF            NVARCHAR(11)
                       ,@SISTEMA        INT 
                       ,@CODEMPRESA     INT 
AS

BEGIN

    -------------TESTE

    --DECLARE  @CPF        NVARCHAR(20)
    --        ,@SISTEMA    INT
    --        ,@CODEMPRESA     INT  

    --SET @CPF = '01292015616'
    --SET @SISTEMA = 0
    --SET @CODEMPRESA = 1

    ------------------
    

    IF NOT EXISTS(SELECT 1 FROM PAGNET_CADFAVORECIDO WHERE CPFCNPJ = @CPF)
    BEGIN
        DECLARE @MAXCODFAVORECIDO INT
        SELECT @MAXCODFAVORECIDO = ISNULL(MAX(CODFAVORECIDO),0) FROM PAGNET_CADFAVORECIDO

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
               AND STA = '00'
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
                    ENDUSUCOM,
                    ENDCPL,
                    BAIRRO,
                    LOCALIDADE,
                    UF    
              FROM VUSUARIO
             WHERE CPF = @CPF 
               AND STA = '00'
        END

        if (@MAXCODFAVORECIDO < 1000000) 
        BEGIN
            set @MAXCODFAVORECIDO = 1000000
        END

        INSERT INTO PAGNET_CADFAVORECIDO
         SELECT @MAXCODFAVORECIDO + 1 AS CODFAVORECIDO,
                NOMEUSUARIO AS NMFAVORECIDO,
                CPF,
                NULL AS CODCEN,
                BANCO,
                AGENCIA,
                DVAGENCIA,
                OPE,
                CONTACORRENTE,
                DVCONTACORRENTE,
                CEP,
                LOGRADOURO,
                NROLOGRADOURO,
                COMPLEMENTO,
                BAIRRO,
                CIDADE,
                UF,              
                'S' AS ATIVO,
                @CODEMPRESA AS CODEMPRESA
           FROM @TABTEMP

    END

    SELECT '0' AS RETORNO

    SET NOCOUNT OFF

END
