--BEGIN
--	IF EXISTS ( SELECT  1
--				FROM    sys.objects
--				WHERE   object_id = OBJECT_ID(N'PROC_PAGNET_CONS_BORDERO')
--						AND type IN ( N'P', N'PC' ) ) 
--	BEGIN
--		DROP PROCEDURE PROC_PAGNET_CONS_BORDERO
--	END
--END

/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_CONS_FECHAMENTO_CRED  PARA O NETCARD Versao 1.0                */         
/* CRIAÇÃO : Luiz Felipe - MARÇO/2019                                         */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_CONS_BORDERO]
                         @codEmpresa            INT             
                        ,@codBordero            INT           = 0         
                        ,@codContaCorrente      INT           = 0  
                        ,@Status                VARCHAR(60)   = ''
AS

BEGIN

--------------------------------TESTE
--DECLARE  @codEmpresa            INT             
--        ,@codBordero            INT             
--        ,@codContaCorrente      INT            
--        ,@Status                VARCHAR(60)


--SELECT @codEmpresa          = 1
--      ,@codBordero          = 0
--      ,@codContaCorrente    = 0
--      ,@Status              = 'PENDENTE_BAIXA'

---------------------------------


    DECLARE @CodBancoContaCorrente nvarchar(4)
    DECLARE @DATREF DATETIME

    DECLARE @BORDEROS TABLE 
    (
         CODBORDERO                 INT
        ,VLBORDERO                  DECIMAL(13,2)
        ,DTBORDERO                  DATETIME
        ,CODEMPRESA                 INT
        ,CODUSUARIO                 INT
        ,STATUS                     VARCHAR(60)
        ,TITULOVENCIDO              CHAR(1)
    )
    
    
    SET @DATREF = CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111))

    INSERT INTO @BORDEROS
    SELECT PBP.CODBORDERO
          ,PBP.VLBORDERO
          ,PBP.DTBORDERO
          ,PBP.CODEMPRESA
          ,PBP.CODUSUARIO
          ,PBP.STATUS
          ,'N'
     FROM PAGNET_BORDERO_PAGAMENTO  PBP
    WHERE PBP.CODEMPRESA        = @codEmpresa
      AND ((@codContaCorrente = 0) OR PBP.CODCONTACORRENTE = @codContaCorrente)
      AND ((@codBordero = 0) OR PBP.CODBORDERO = @codBordero)
      AND ((@Status = '') OR PBP.STATUS = @Status)

    

      --RETORNO DOS DADOS
      SELECT CODBORDERO
            ,VLBORDERO
            ,DTBORDERO
            ,CODEMPRESA
            ,CODUSUARIO
            ,STATUS
            ,TITULOVENCIDO
       FROM @BORDEROS

   
END

