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

CREATE PROCEDURE [dbo].[PROC_PAGNET_BUSCA_USUARIO_NC]
                        @MATRICULA      NVARCHAR(30)
                       ,@SISTEMA        INT 
                       ,@CODCLIENTE     INT 
AS

BEGIN



    -------------TESTE

    --DECLARE  @MATRICULA     NVARCHAR(20)
    --        ,@SISTEMA       INT
    --        ,@CODCLIENTE    INT  

    --SET @MATRICULA = '500418'
    --SET @SISTEMA = 0
    --SET @CODCLIENTE = 5
    
    ------------------
    
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
              FROM VUSUARIOVA V
             WHERE V.MAT  = @MATRICULA 
               AND V.CODCLI = @CODCLIENTE
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
              FROM VUSUARIO V
             WHERE V.MAT  = @MATRICULA 
               AND V.CODCLI = @CODCLIENTE
        END

         SELECT  NOMEUSUARIO,      
                 CPF,              
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
                 UF
           FROM @TABTEMP


    SET NOCOUNT OFF

END
