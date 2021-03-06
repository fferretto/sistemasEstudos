
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT 1
                 FROM sys.all_objects 
                 WHERE NAME = 'VALIDA_LOGIN_PAGNET')
BEGIN
        DROP PROCEDURE VALIDA_LOGIN_PAGNET
END

GO

CREATE PROCEDURE [dbo].[VALIDA_LOGIN_PAGNET]
                        @LOGIN VARCHAR(100)
                       ,@SENHA VARCHAR(200) 
AS

SET NOCOUNT ON

BEGIN

    SELECT USU.NMUSUARIO
          ,USU.CODUSUARIO
          ,USU.LOGIN
          ,USU.CODSUBREDE
          ,USU.GERABOLETO
          ,USU.FAZPAGAMENTO
          ,USU.ADMINISTRADOR
          ,OPE.NOMOPERAFIL
          ,OPE.CODOPE
          ,OPE.BD_AUT
          ,OPE.BD_NC
          ,OPE.SERVIDOR
          ,OPE.SERVIDOR_AUT
     FROM PAGNET_USUARIO    USU
         ,OPERADORA         OPE
    WHERE USU.CODOPE = OPE.CODOPE
      AND LTRIM(RTRIM(USU.Login)) = @LOGIN
      AND LTRIM(RTRIM(USU.SENHA)) = @SENHA
      AND USU.Ativo              = 'S' 

SET NOCOUNT OFF

END

