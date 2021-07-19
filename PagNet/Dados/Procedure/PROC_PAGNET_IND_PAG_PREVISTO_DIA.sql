/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_VAL_TOTAL_PAGAMENTO PARA O NETCARD Versao 1.0                  */         
/* CRIAÇÃO : Luiz Felipe - JANEIRO/2019                                       */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  

CREATE PROCEDURE [dbo].[PROC_PAGNET_IND_PAG_PREVISTO_DIA]
                        @dtInicio   DATETIME
                       ,@dtFim      DATETIME
                       ,@codSubRede INT         = 0
AS

BEGIN
--------------------TESTE
--DECLARE                 @dtInicio   DATETIME
--                       ,@dtFim      DATETIME
--                       ,@codSubRede INT         = 0


--SELECT @dtInicio   = '20190420'
--      ,@dtFim      = '20190520'
--      ,@codSubRede = 2
--------------------------


  DECLARE @FECHAMENTOS TABLE 
    (
        CODCRE          int
       ,DATPGTO         DATETIME
       ,VALLIQ          DECIMAL(13,2)
       ,VALTAXA         DECIMAL(13,2)
    )

    BEGIN
    
    --POS-PAGO
    INSERT INTO @FECHAMENTOS
        SELECT 
             fc.CODCRE
            ,fc.DATPGTO
            ,fc.VALLIQ
            ,FC.VALTAXA
        FROM FECHCRED FC 
            ,VCREDENCIADO   C  
            ,TRANSACAO      T  WITH(INDEX(IX_TRANSACAO_CODCRE_NUMFECCRE))
        WHERE fc.DATPGTO    >= @dtInicio
          AND fc.DATPGTO    <= @dtFim
          AND fc.VALLIQ      > 0     
          AND FC.CODCRE      = C.CODCRE 
          AND T.CODCRE       = FC.CODCRE 
          AND T.NUMFECCRE    = FC.NUMFECCRE 
          AND T.CODRTA      = 'V' 
          AND T.TIPTRA       < 80000 
          AND FC.DATFECLOT  IS NOT NULL 
          AND ((@codSubRede = 0) OR T.CODSUBREDE = @codSubRede)
        GROUP BY 
                fc.CODCRE
               ,fc.DATPGTO
               ,fc.VALLIQ
               ,FC.VALTAXA

 END
  
    BEGIN

    --PRE-PAGO
    INSERT INTO @FECHAMENTOS
        SELECT 
             fc.CODCRE
            ,fc.DATPGTO
            ,fc.VALLIQ
            ,FC.VALTAXA
    FROM FECHCREDVA fc
        ,VCREDENCIADO    C 
        ,TRANSACVA       T  WITH(INDEX(IX_TRANSACVA_CODCRE_NUMFECCRE))
    WHERE fc.DATPGTO    >= @dtInicio
          AND fc.DATPGTO    <= @dtFim
          AND fc.VALLIQ      > 0     
          AND FC.CODCRE      = C.CODCRE 
          AND T.CODCRE       = FC.CODCRE 
          AND T.NUMFECCRE    = FC.NUMFECCRE 
          AND T.CODRTA      = 'V' 
          AND T.TIPTRA       < 80000 
          AND FC.DATFECLOT  IS NOT NULL 
          AND ((@codSubRede = 0) OR T.CODSUBREDE = @codSubRede)
        GROUP BY 
                 fc.CODCRE
                ,fc.DATPGTO
                ,fc.VALLIQ
                ,FC.VALTAXA
END

    SELECT DATPGTO     
          ,SUM(VALLIQ) VLPAGAR  
          ,SUM(VALTAXA) VALTAXA  
     FROM @FECHAMENTOS
    GROUP BY DATPGTO
    ORDER BY DATPGTO DESC
    
END