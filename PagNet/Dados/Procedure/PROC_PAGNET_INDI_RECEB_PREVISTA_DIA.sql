--/*----------------------------------------------------------------------------*/    
--/*                                                                            */         
--/* PROC_PAGNET_INDICADOR_RECEBIMENTO_DIA PARA O NETCARD Versao 1.0            */         
--/* CRIAÇÃO : Luiz Felipe - JANEIRO/2019                                       */         
--/* REVISÃO :                                                                  */
--/*                                                                            */   
--/*                                                                            */    
/*----------------------------------------------------------------------------*/  
CREATE PROCEDURE [dbo].[PROC_PAGNET_INDI_RECEB_PREVISTA_DIA]
                        @dtInicio   DATETIME
                       ,@dtFim      DATETIME
                       ,@codSubRede INT         = 0
AS

BEGIN

----------------------TESTE
--DECLARE                 @dtInicio   DATETIME
--                       ,@dtFim      DATETIME
--                       ,@codSubRede INT         = 0


--SELECT @dtInicio   = '20190501'
--      ,@dtFim      = '20190520'
--      ,@codSubRede = 1
-----------------------

    DECLARE @TABELA TABLE
    (
         DATPGTO     DATETIME
        ,VALOR       DECIMAL(13,2)
    )

    INSERT INTO @TABELA
      SELECT FC.DATPGTO
            ,SUM(FC.TOTAL)     AS  VLRECEBER        
        FROM FECHCLIENTE FC 
       WHERE fc.DATPGTO      >= @dtInicio
         AND fc.DATPGTO    <= @dtFim
         AND fc.TOTAL      > 0     
         AND FC.DATFECLOT  IS NOT NULL 
         AND ((@codSubRede = 0) OR FC.CODSUBREDE = @codSubRede)
    GROUP BY FC.DATPGTO     

    INSERT INTO @TABELA
   SELECT  DATEADD(dd, DATEDIFF(dd, 0, C.DTAUTORIZ), 0)  AS DATA
         ,SUM(C.VALOR)                          AS VALOR
     FROM CARGAC C
    WHERE C.DTAUTORIZ      >= @dtInicio
      AND C.DTAUTORIZ      <= @dtFim
      AND C.DTCARGA         > '19000101'
      AND ((@codSubRede = 0) OR C.CODSUBREDE = @codSubRede)
 GROUP BY DATEADD(dd, DATEDIFF(dd, 0, C.DTAUTORIZ), 0)   



         SELECT DATPGTO
               ,SUM(VALOR)  AS  VLRECEBER 
         FROM @TABELA
     GROUP BY DATPGTO
     ORDER BY DATPGTO DESC
END

