/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_INFO_CONTA_CORRENTE  Versao 1.0                                */         
/* CRIAÇÃO : Luiz Felipe - JAN/2020                                           */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_INFO_CONTA_CORRENTE]
                        @DTINICIO         DATETIME
                       ,@DTFIM            DATETIME
                       ,@CODCONTACORRENTE INT             
                       ,@CODEMPRESA       INT            
AS

BEGIN

-----------------------TESTE
    --DECLARE  @DTINICIO          DATETIME
    --        ,@DTFIM            DATETIME
    --        ,@CODCONTACORRENTE INT             
    --        ,@CODEMPRESA       INT   


    --SELECT @dtInicio            = '20191202'
    --      ,@dtFim               = '20200113'
    --      ,@CODCONTACORRENTE    = 2
    --      ,@CODEMPRESA          = 1
-------------------------------

    DECLARE @TABTEMP TABLE 
    (
         VALOR              DECIMAL(15,2)   
        ,TIPOTRANSACAO      NVARCHAR(60)     
    )
    DECLARE @SALDOANTERIOR DECIMAL(15,2)
    DECLARE @TOTALRECEITA DECIMAL(15,2)
    DECLARE @TOTALDESPESA DECIMAL(15,2)
    DECLARE @SALDOCONTA DECIMAL(15,2)

    INSERT INTO @TABTEMP
    SELECT 
        SUM(ET.VALTOTAL),
        'DESPESA' AS TIPOTRANSACAO
    FROM PAGNET_EMISSAO_TITULOS ET
    WHERE ET.DATREALPGTO >=  @DTINICIO
      AND ET.DATREALPGTO <= @DTFIM
      AND ET.STATUS IN ('BAIXADO', 'BAIXADO_MANUALMENTE', 'CONSOLIDADO')
      AND ET.CODCONTACORRENTE = @CODCONTACORRENTE
      AND ET.CODEMPRESA = @CODEMPRESA
    GROUP BY ET.TIPOTITULO


    INSERT INTO @TABTEMP
    SELECT SUM(ABS(TT.VALOR)) AS VALOR,
           CASE WHEN SUM(TT.VALOR) < 0 THEN 'RECEITA' ELSE 'DESPESA' END  AS TIPOTRANSACAO
     FROM PAGNET_EMISSAO_TITULOS ET
     JOIN PAGNET_TAXAS_TITULOS TT ON ET.CODTITULO = TT.CODTITULO
    WHERE ET.DATREALPGTO >=  @DTINICIO
      AND ET.DATREALPGTO <= @DTFIM
      AND ET.STATUS IN ('BAIXADO', 'BAIXADO_MANUALMENTE')
      AND ET.CODCONTACORRENTE = @CODCONTACORRENTE
      AND ET.CODEMPRESA = @CODEMPRESA
    GROUP BY TT.DESCRICAO

   SELECT TOP 1 
          @SALDOANTERIOR = SALDO
     FROM PAGNET_CONTACORRENTE_SALDO 
    WHERE DATLANCAMENTO < @dtInicio
      AND CODEMPRESA = @CODEMPRESA
      AND CODCONTACORRENTE = @CODCONTACORRENTE
    ORDER BY CODSALDO DESC

    INSERT INTO @TABTEMP
    SELECT 
        SUM(EF.VLPGTO),
        'RECEITA' AS TIPOTRANSACAO 
     FROM PAGNET_EMISSAOFATURAMENTO EF
     WHERE EF.DATVENCIMENTO >=  @DTINICIO
       AND EF.DATVENCIMENTO <= @DTFIM
       AND EF.STATUS IN ('LIQUIDADO', 'LIQUIDADO_MANUALMENTE', 'CONSOLIDADO')
       AND EF.CODCONTACORRENTE = @CODCONTACORRENTE
       AND EF.CODEMPRESA = @CODEMPRESA
    GROUP BY EF.TIPOFATURAMENTO

   SELECT @TOTALRECEITA= SUM(VALOR)          
      FROM @TABTEMP
     WHERE TIPOTRANSACAO = 'RECEITA'
     
   SELECT @TOTALDESPESA= SUM(VALOR)          
      FROM @TABTEMP
     WHERE TIPOTRANSACAO = 'DESPESA'

   SELECT TOP 1 
          @SALDOCONTA = SALDO
     FROM PAGNET_CONTACORRENTE_SALDO 
    WHERE CODEMPRESA = @CODEMPRESA
      AND CODCONTACORRENTE = @CODCONTACORRENTE
    ORDER BY CODSALDO DESC

     SELECT ISNULL(@SALDOANTERIOR, 0)  AS SALDOANTERIOR,
            ISNULL(@SALDOCONTA, 0)     AS SALDOCONTA,
            ISNULL(@TOTALRECEITA, 0)   AS TOTALRECEITA,
            ISNULL(@TOTALDESPESA, 0)   AS TOTALDESPESA
END
