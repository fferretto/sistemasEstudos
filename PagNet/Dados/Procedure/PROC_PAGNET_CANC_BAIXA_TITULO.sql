
/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_CANC_BAIXA_TITULO  PARA O NETCARD Versao 1.0                   */         
/* CRIAÇÃO : Luiz Felipe - dezembro/2019                                      */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_CANC_BAIXA_TITULO]
                        @codTitulo         INT
                       ,@codUsuario        INT
                       ,@Justificativa     NVARCHAR(250)
AS

BEGIN

-----------------------TESTE
    --DECLARE  @codTitulo          INT
    --        ,@codUsuario         INT
    --        ,@Justificativa      nvarchar(250)


    --SELECT @codTitulo        = 12
    --      ,@codUsuario       = 9999
    --      ,@Justificativa    = 'CANCELAMENTO DE BAIXA A PEDIDO DO USUÁRIO EDERSON'
-------------------------------

DECLARE @MAXCODLOG INT
DECLARE @SEUNUMERO NVARCHAR(50)
    
    SELECT @MAXCODLOG = MAX(CODTITULO_LOG) FROM PAGNET_EMISSAO_TITULOS_LOG
    SELECT @SEUNUMERO = SEUNUMERO FROM PAGNET_EMISSAO_TITULOS WHERE CODTITULO = @codTitulo


    INSERT INTO PAGNET_EMISSAO_TITULOS_LOG
    SELECT @MAXCODLOG + 1
          ,CODTITULO
          ,CODTITULOPAI
          ,'EM_ABERTO'          AS STATUS
          ,CODEMPRESA
          ,CODFAVORECIDO
          ,DATEMISSAO
          ,DATPGTO
          ,DATREALPGTO
          ,VALBRUTO
          ,VALLIQ
          ,VALTOTAL
          ,TIPOTITULO
          ,ORIGEM
          ,SISTEMA
          ,LINHADIGITAVEL
          ,CODBORDERO
          ,SEUNUMERO
          ,@codUsuario              AS CODUSUARIO
          ,GETDATE()                AS DATINCLOG
          ,@Justificativa           AS DESCLOG
          ,CODCONTACORRENTE
          ,CODPLANOCONTAS
      FROM PAGNET_EMISSAO_TITULOS
      WHERE CODTITULO = @codTitulo


      UPDATE PAGNET_EMISSAO_TITULOS
         SET STATUS = 'EM_ABERTO'
       WHERE CODTITULO = @codTitulo
              

      UPDATE PAGNET_TITULOS_PAGOS
         SET STATUS = 'CANCELADO'
       WHERE SEUNUMERO = @SEUNUMERO
END

