/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_EXTRATO_BANCARIO  Versao 1.0                                   */         
/* CRIAÇÃO : Luiz Felipe - JAN/2020                                           */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_EXTRATO_BANCARIO]
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
    --      ,@dtFim               = '20200116'
    --      ,@CODCONTACORRENTE    = 2
    --      ,@CODEMPRESA          = 1
-------------------------------

    DECLARE @SALDOANTERIOR DECIMAL(18,2)
    DECLARE @TOTALENTRADA DECIMAL(18,2)
    DECLARE @TOTALSAIDA DECIMAL(18,2)
    DECLARE @SALDOFINAL DECIMAL(18,2)

    DECLARE @TABTEMP TABLE 
    (
         DESCRICAO          NVARCHAR(200)
        ,DATA               DATETIME
        ,VALOR              DECIMAL(15,2)   
        ,TIPOTRANSACAO      NVARCHAR(60)  
        ,SALDOANTERIOR      DECIMAL(18,2)  
        ,TOTALENTRADA       DECIMAL(18,2)  
        ,TOTALSAIDA         DECIMAL(18,2)  
        ,SALDOFINAL         DECIMAL(18,2)    
    )
    
    /*RETORNA O ANTERIOR A DATA DE INÍCIO*/
    SELECT TOP 1 
          @SALDOANTERIOR = SALDO
     FROM PAGNET_CONTACORRENTE_SALDO 
    WHERE DATLANCAMENTO < @dtInicio
      AND CODEMPRESA = @CODEMPRESA
      AND CODCONTACORRENTE = @CODCONTACORRENTE
    ORDER BY CODSALDO DESC


    INSERT INTO @TABTEMP
    SELECT 
        CASE ET.TIPOTITULO 
            WHEN 'TED'    THEN 'PAGAMENTOS VIA TED' 
            WHEN 'CC'     THEN 'PAGAMENTOS VIA CRÉDITO EM CONTA' 
            WHEN 'DOC'    THEN 'PAGAMENTOS VIA DOC' 
            WHEN 'BOLETO' THEN 'PAGAMENTOS VIA BOLETO BANCÁRIO' 
            ELSE ET.TIPOTITULO 
        END AS DESCRICAO,
        ET.DATREALPGTO,
        SUM(ET.VALTOTAL),
        'SAIDA' AS TIPOTRANSACAO,
        0,
        0,
        0,
        0
    FROM PAGNET_EMISSAO_TITULOS ET
    WHERE ET.DATREALPGTO >=  @DTINICIO
      AND ET.DATREALPGTO <= @DTFIM
      AND ET.STATUS IN ('BAIXADO', 'BAIXADO_MANUALMENTE', 'CONSOLIDADO')
      AND ET.CODCONTACORRENTE = @CODCONTACORRENTE
      AND ET.CODEMPRESA = @CODEMPRESA
    GROUP BY ET.TIPOTITULO,
             ET.DATREALPGTO


    INSERT INTO @TABTEMP
    SELECT UPPER(TT.DESCRICAO),
           ET.DATREALPGTO,
           SUM(ABS(TT.VALOR)) AS VALOR,
           CASE WHEN SUM(TT.VALOR) < 0 THEN 'ENTRADA' ELSE 'SAIDA' END  AS TIPOTRANSACAO,
            0,
            0,
            0,
            0
     FROM PAGNET_EMISSAO_TITULOS ET
     JOIN PAGNET_TAXAS_TITULOS TT ON ET.CODTITULO = TT.CODTITULO
    WHERE ET.DATREALPGTO >=  @DTINICIO
      AND ET.DATREALPGTO <= @DTFIM
      AND ET.STATUS IN ('BAIXADO', 'BAIXADO_MANUALMENTE')
      AND ET.CODCONTACORRENTE = @CODCONTACORRENTE
      AND ET.CODEMPRESA = @CODEMPRESA
    GROUP BY TT.DESCRICAO,
             ET.DATREALPGTO



    INSERT INTO @TABTEMP
    SELECT 
       CASE EF.TIPOFATURAMENTO 
            WHEN 'CARGA'    THEN 'CARGA DE CARTÃO' 
            WHEN 'POSPAGO'  THEN 'CARTÃO PÓS PAGO' 
            ELSE EF.TIPOFATURAMENTO 
        END AS DESCRICAO,
        EF.DATVENCIMENTO,
        SUM(EF.VLPGTO),
        'ENTRADA' AS TIPOTRANSACAO ,
        0,
        0,
        0,
        0
     FROM PAGNET_EMISSAOFATURAMENTO EF
     WHERE EF.DATVENCIMENTO >=  @DTINICIO
       AND EF.DATVENCIMENTO <= @DTFIM
       AND EF.STATUS IN ('LIQUIDADO', 'LIQUIDADO_MANUALMENTE', 'CONSOLIDADO')
       AND EF.CODCONTACORRENTE = @CODCONTACORRENTE
       AND EF.CODEMPRESA = @CODEMPRESA
    GROUP BY EF.TIPOFATURAMENTO,
             EF.DATVENCIMENTO

    /*BUSCA O TOTAL DE ENTRADA*/
       SELECT @TOTALENTRADA = SUM(VALOR)  
         FROM @TABTEMP
        WHERE TIPOTRANSACAO = 'ENTRADA'
        
    /*BUSCA O TOTAL DE SAIDA*/
       SELECT @TOTALSAIDA = SUM(VALOR)  
         FROM @TABTEMP
        WHERE TIPOTRANSACAO = 'SAIDA'

    
        SET @SALDOFINAL = (@TOTALENTRADA + @SALDOANTERIOR) - @TOTALSAIDA

        UPDATE @TABTEMP
        SET  SALDOANTERIOR   = @SALDOANTERIOR
            ,TOTALENTRADA    = @TOTALENTRADA
            ,TOTALSAIDA      = @TOTALSAIDA
            ,SALDOFINAL      = @SALDOFINAL

   SELECT  UPPER(DESCRICAO) AS DESCRICAO    
          ,DATA             
          ,VALOR            
          ,TIPOTRANSACAO     
          ,ISNULL(SALDOANTERIOR, 0) SALDOANTERIOR
          ,TOTALENTRADA    
          ,TOTALSAIDA    
          ,ISNULL(SALDOFINAL, 0)  SALDOFINAL 
      FROM @TABTEMP
      WHERE VALOR <> 0
   ORDER BY DATA
END
