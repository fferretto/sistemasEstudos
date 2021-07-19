/*------------------------------------------------------------------------------*/    
/*                                                                              */         
/* PROC_PAGNET_IND_PAG_REALIZADO_DIA PARA O PAGNET Versao 1.0                   */         
/* CRIAÇÃO : Luiz Felipe - JULHO/2019                                           */         
/* REVISÃO :                                                                    */
/*                                                                              */   
/*                                                                              */    
/*------------------------------------------------------------------------------*/  
CREATE PROCEDURE [dbo].[PROC_PAGNET_IND_PAG_REALIZADO_DIA]
                        @dtInicio           DATETIME
                       ,@dtFim              DATETIME
                       ,@codSubRede         INT         
                       ,@codBanco           varchar(4)  = ''
                       ,@codCredenciado     INT         = 0
AS

BEGIN
----------------------TESTE
--DECLARE                 @dtInicio           DATETIME
--                       ,@dtFim              DATETIME
--                       ,@codSubRede         INT          
--                       ,@codBanco           varchar(4)  = ''
--                       ,@codCredenciado     INT         = 0


--SELECT @dtInicio            = '20190613'
--      ,@dtFim               = '20190723'
--      ,@codSubRede          = 1
--      ,@codBanco            = ''
--      ,@codCredenciado      = 0
----------------------------


  DECLARE @TABTEMP TABLE 
    (
        CODCRE              INT
       ,DTREALPAGAMENTO     DATETIME
       ,DATPGTO             DATETIME
       ,VALOR               DECIMAL(13,2)
    )

    DECLARE @PAGAMENTOS TABLE 
    (
        CODCRE              INT
       ,DTREALPAGAMENTO     DATETIME
       ,DATPGTO             DATETIME
       ,VALOR               DECIMAL(13,2)
       ,VALTAXA             DECIMAL(13,2)
    )

    
    INSERT INTO @TABTEMP
        SELECT 
             PG.CODCEN
            ,PG.DTREALPAGAMENTO
            ,PG.DTPAGAMENTO
            ,PG.VALOR
        FROM PAGNET_PAGAMENTO               PG 
            ,PAGNET_CONTACORRENTE           CC  
        WHERE CC.CODCONTACORRENTE   = PG.CODCONTACORRENTE
          AND PG.DTREALPAGAMENTO    >= @dtInicio
          AND PG.DTREALPAGAMENTO    <= @dtFim
          AND PG.CODSUBREDE = @codSubRede
          AND ((@codBanco = '') OR CC.CODBANCO = @codBanco)
          AND ((@codCredenciado = 0) OR PG.CODCEN = @codCredenciado)
          AND PG.STATUS IN ('BAIXADO_MANUALMENTE', 'BAIXADO')
        GROUP BY 
             PG.CODCEN
            ,PG.DTREALPAGAMENTO
            ,PG.DTPAGAMENTO
            ,PG.VALOR

   INSERT INTO @PAGAMENTOS
    SELECT TEMP.CODCRE           
          ,TEMP.DTREALPAGAMENTO  
          ,TEMP.DATPGTO          
          ,TEMP.VALOR  
          ,(SUM(T.VALTRA - CONVERT(NUMERIC(11,2), T.VALTRA - CAST((T.VALTRA*FC.TAXSER)/100.0 AS NUMERIC(11,2))))) AS VALTAXA
     FROM @TABTEMP          TEMP
          ,FECHCRED         FC
          ,TRANSACAO      T with (index(IX_TRANSACAO_CODCRE_NUMFECCRE))
    WHERE FC.CODCEN         = TEMP.CODCRE
      AND FC.DATPGTO        = TEMP.DATPGTO
        AND T.CODCRE       = FC.CODCRE 
        AND T.NUMFECCRE    = FC.NUMFECCRE 
        AND T.CODRTA       = 'V' 
        AND T.TIPTRA       < 80000 
        AND T.CODSUBREDE = @codSubRede
  group by TEMP.CODCRE           
          ,TEMP.DTREALPAGAMENTO  
          ,TEMP.DATPGTO          
          ,TEMP.VALOR  


    SELECT DTREALPAGAMENTO  as DATPGTO
          ,SUM(VALOR)       as VLPAGAR  
          ,SUM(VALTAXA)     as VALTAXA  
     FROM @PAGAMENTOS
    GROUP BY DTREALPAGAMENTO
    ORDER BY DTREALPAGAMENTO asc
    
END