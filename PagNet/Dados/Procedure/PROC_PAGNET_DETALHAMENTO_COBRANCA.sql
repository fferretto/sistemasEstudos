/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_DETALHAMENTO_COBRANCA  PARA O NETCARD Versao 1.0               */         
/* CRIAÇÃO : Luiz Felipe - MARÇO/2019                                         */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_DETALHAMENTO_COBRANCA]
                        @CodEmissaoFaturamento         INT
AS

BEGIN

-----------------------TESTE
    --DECLARE  @CodEmissaoFaturamento          INT
    --SELECT @CodEmissaoFaturamento            = 5
-------------------------------

    DECLARE @TIPOFATURAMENTO NVARCHAR(30)
    DECLARE @CODCLIENTE INT
    DECLARE @NROREF_NETCARD INT
    DECLARE @DATVENCIMENTO  DATETIME
    DECLARE @VALORTOTAL  DECIMAL(13,2)

    SELECT @TIPOFATURAMENTO     = FAT.TIPOFATURAMENTO
           ,@CODCLIENTE         = FAT.CODCLIENTE
           ,@NROREF_NETCARD     = FAT.NROREF_NETCARD
           ,@DATVENCIMENTO      = FAT.DATVENCIMENTO
           ,@VALORTOTAL         = FAT.VALOR
    FROM PAGNET_EMISSAOFATURAMENTO FAT
   WHERE CODEMISSAOFATURAMENTO = @CodEmissaoFaturamento

    DECLARE @TABTEMP TABLE 
    (
         DESCRICAO          VARCHAR(200)
        ,VALOR              DECIMAL(13,2)  
    )

    DECLARE @TABTEMPTAXAS TABLE
    (
        CODCLI      INT,
        LOTE        INT,
        CNPJ        NVARCHAR(20),
        DTFECHTO    DATETIME,
        DTPAGTO     DATETIME,
        VALTAXA     DECIMAL(13,2),
        DESCTAXA    NVARCHAR(200),
        CODSUBREDE  INT,
        DESCSUBREDE NVARCHAR(200)
    )
    INSERT INTO @TABTEMPTAXAS
    EXEC SRV_CONSULTA_TAXAS_FECHCLI  @SISTEMA = 0, @SELECAO = 1, @DATINI = @DATVENCIMENTO, @DATFIM = @DATVENCIMENTO
    
    IF @TIPOFATURAMENTO = 'POSPAGO'
    BEGIN      
    
        --TAXAS REGISTRADAS
        INSERT INTO @TABTEMP
        SELECT TEMP.DESCTAXA
              ,TEMP.VALTAXA
          FROM @TABTEMPTAXAS TEMP
         WHERE TEMP.CODCLI  = @CODCLIENTE 
           AND TEMP.LOTE    = @NROREF_NETCARD
           
        --COMPRAS
        INSERT INTO @TABTEMP
        SELECT 'COMPRAS'
              ,@VALORTOTAL - ISNULL(SUM(TEMP.VALTAXA), 0)
          FROM @TABTEMPTAXAS TEMP
         WHERE TEMP.CODCLI  = @CODCLIENTE 
           AND TEMP.LOTE    = @NROREF_NETCARD


    END
    ELSE IF @TIPOFATURAMENTO = 'CARGA'
    BEGIN
    
            --VALOR DA CARGA
      INSERT INTO @TABTEMP
           SELECT 'VALOR DA CARGA'
                 ,C.VALOR_A_CARREGAR
             FROM CARGAC C
            WHERE C.CODCLI = @CODCLIENTE 
              AND C.NUMCARG_VA = @NROREF_NETCARD
          
            --TAXA DE SERVIÇO
      INSERT INTO @TABTEMP
           SELECT 'TAXA DE SERVIÇO'
                 ,C.TAXSER
             FROM CARGAC C
            WHERE C.CODCLI = @CODCLIENTE 
              AND C.NUMCARG_VA = @NROREF_NETCARD
    END

    SELECT DESCRICAO
          ,VALOR
     FROM  @TABTEMP

END


