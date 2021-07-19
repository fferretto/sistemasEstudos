/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_IND_PAG_ANO PARA O PAGNET Versao 1.0                           */         
/* CRIAÇÃO : Luiz Felipe - JANEIRO/2019                                       */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  
CREATE PROCEDURE [dbo].[PROC_PAGNET_IND_PAG_ANO]
                         @codSubRede         INT       
                        ,@codBanco           VARCHAR(4)  = ''
                        ,@codCredenciado     INT         = 0
AS

BEGIN

----------------------TESTE
--        DECLARE @codSubRede         INT    
--               ,@codBanco           VARCHAR(4)  
--               ,@codCredenciado     INT        


--        SET @codSubRede     = 1
--        SET @codBanco       = ''
--        SET @codCredenciado = 0
----------------------------

    DECLARE @dtInicio as datetime
           ,@dtFim as datetime
           ,@data as datetime


    set @data = getdate()


    set @dtInicio = DATEADD(MONTH, -13, @data)
    SET @dtInicio = DATEADD(DAY, - DAY(@dtInicio) + 1, @dtInicio)
    
    SET @dtFim = @data
    SET @dtFim = DATEADD(DAY, - DAY(@dtFim), @dtFim)
       

  DECLARE @TABTEMP TABLE 
    (
        CODCRE              INT
       ,DTREALPAGAMENTO     DATETIME
       ,DATPGTO             DATETIME
       ,VALOR               DECIMAL(13,2)
       ,VALTAXA             DECIMAL(13,2)
    )
    
    
    DECLARE @TABRETORNO TABLE 
    (
        DATRET              VARCHAR(6)
       ,VALDISPESA          DECIMAL(13,2)
       ,VALTAXARETIDA       DECIMAL(13,2)
    )

    ----------------------------------------CALCULO PARA VALOR QUE FOI PAGO NOS ULTIMOS 12 MESES - SEM CONTAR O MÊS ATUAL------------------------------

        INSERT INTO @TABTEMP
        SELECT 
             PG.CODCEN
            ,PG.DTREALPAGAMENTO
            ,PG.DTPAGAMENTO
            ,PG.VALOR
            ,(SUM(T.VALTRA - CONVERT(NUMERIC(11,2), T.VALTRA - CAST((T.VALTRA*FC.TAXSER)/100.0 AS NUMERIC(11,2))))) AS VALTAXA
        FROM PAGNET_PAGAMENTO               PG 
            ,PAGNET_CONTACORRENTE           CC  
            ,FECHCRED         FC
            ,TRANSACAO      T with (index(IX_TRANSACAO_CODCRE_NUMFECCRE))
        WHERE CC.CODCONTACORRENTE   = PG.CODCONTACORRENTE
          AND PG.DTREALPAGAMENTO    >= @dtInicio
          AND PG.DTREALPAGAMENTO    <= @dtFim
          AND PG.CODSUBREDE = @codSubRede
          AND ((@codBanco = '') OR CC.CODBANCO = @codBanco)
          AND ((@codCredenciado = 0) OR PG.CODCEN = @codCredenciado)
          AND PG.STATUS IN ('BAIXADO_MANUALMENTE', 'BAIXADO')
          AND FC.CODCEN         = PG.CODCEN
          AND FC.DATPGTO        = PG.DTPAGAMENTO
          AND T.CODCRE       = FC.CODCRE 
          AND T.NUMFECCRE    = FC.NUMFECCRE 
          AND T.CODRTA       = 'V' 
          AND T.TIPTRA       < 80000 
          AND T.CODSUBREDE = @codSubRede
        GROUP BY 
             PG.CODCEN
            ,PG.DTREALPAGAMENTO
            ,PG.DTPAGAMENTO
            ,PG.VALOR

    ----------------------------------------RETORNO DA PROCEDURE------------------------------
    INSERT INTO @TABRETORNO
    SELECT   LEFT(CONVERT(varchar, CRED.DTREALPAGAMENTO,112),6)         AS  [MESREF]
            ,SUM(CRED.VALOR)                                            AS  [DESPESA]  
            ,SUM(CRED.VALTAXA)                                          AS  [TAXARETIDA]    
     FROM @TABTEMP  CRED
    GROUP BY CRED.DTREALPAGAMENTO

             
              
    SELECT   SUBSTRING(TAB.DATRET, 5,2) + '/' + SUBSTRING(TAB.DATRET,0,5) AS MESREF
            ,SUM(TAB.VALDISPESA)            AS  [VLPAGO]   
            ,SUM(TAB.VALTAXARETIDA)         AS  [VLTAXA]   
     FROM @TABRETORNO TAB
    GROUP BY TAB.DATRET
    ORDER BY TAB.DATRET asc
  

END