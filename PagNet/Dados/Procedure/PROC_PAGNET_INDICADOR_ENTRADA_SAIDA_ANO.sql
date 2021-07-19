/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_VAL_TOTAL_PAGAMENTO PARA O NETCARD Versao 1.0                  */         
/* CRIAÇÃO : Luiz Felipe - JANEIRO/2019                                       */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  
CREATE PROCEDURE [dbo].[PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO]
                            @codSubRede INT         = 0
AS

BEGIN

--------------------TESTE
--DECLARE @codSubRede INT    
--SET @codSubRede = 0
--------------------------

    DECLARE @dtInicio as datetime
           ,@dtFim as datetime
           ,@data as datetime


    set @data = getdate()


    set @dtInicio = DATEADD(MONTH, -12, @data)
    SET @dtInicio = DATEADD(DAY, - DAY(@dtInicio) + 1, @dtInicio)
    
    SET @dtFim = DATEADD(MONTH, 1, @data)
    SET @dtFim = DATEADD(DAY, - DAY(@dtFim), @dtFim)
    

  DECLARE @FECHAMENTOSCREDAUX TABLE 
    (
        TIPPAG          VARCHAR(50)
       ,DAT             DATETIME
       ,NUMFECCRE       INT
       ,CODCRE          INT       
       ,VALLIQ          DECIMAL(13,2)
    )
    
    DECLARE @FECHAMENTOSCLI TABLE 
    (
        DAT             DATETIME
       ,VALOR           DECIMAL(13,2)
    )

    DECLARE @FECHAMENTOSCRED TABLE 
    (
        TIPPAG          varchar(50)
       ,DAT             DATETIME
       ,VALLIQ          DECIMAL(13,2)
    )

    
    DECLARE @TABRETORNO TABLE 
    (
        DATRET              VARCHAR(6)
       ,VALRECEITA          DECIMAL(13,2)
       ,VALDISPESA          DECIMAL(13,2)
    )

    ----------------------------------------CALCULO PARA VALOR A PAGAR------------------------------

    BEGIN    
        --POS-PAGO
        INSERT INTO @FECHAMENTOSCREDAUX
            SELECT 'CREDPOS'         AS TIPPAG
                  ,FC.DATPGTO
                  ,FC.NUMFECCRE
                  ,FC.CODCRE
                  ,FC.VALLIQ          
            FROM FECHCRED FC 
                ,TRANSACAO      T  WITH(INDEX(IX_TRANSACAO_CODCRE_NUMFECCRE))
           WHERE fc.DATPGTO      >= @dtInicio
             AND fc.DATPGTO    <= @dtFim
             AND fc.VALLIQ      > 0     
             AND FC.DATFECLOT  IS NOT NULL 
             AND T.CODCRE       = FC.CODCRE 
             AND T.NUMFECCRE    = FC.NUMFECCRE 
             AND T.CODRTA      = 'V' 
             AND T.TIPTRA       < 80000 
             AND ((@codSubRede = 0) OR T.CODSUBREDE = @codSubRede)
        GROUP BY FC.DATPGTO
                ,FC.NUMFECCRE
                ,FC.CODCRE
                ,FC.VALLIQ   


    END 
    
     BEGIN

        --PRE-PAGO
         INSERT INTO @FECHAMENTOSCREDAUX
            SELECT 'CREDPRE'         AS TIPPAG
                  ,FC.DATPGTO
                  ,FC.NUMFECCRE
                  ,FC.CODCRE
                  ,FC.VALLIQ          
             FROM FECHCREDVA      FC 
                 ,TRANSACVA       T  WITH(INDEX(IX_TRANSACVA_CODCRE_NUMFECCRE))
            WHERE fc.DATPGTO      >= @dtInicio
              AND fc.DATPGTO    <= @dtFim
              AND fc.VALLIQ      > 0     
              AND FC.DATFECLOT  IS NOT NULL 
              AND T.CODCRE       = FC.CODCRE 
              AND T.NUMFECCRE    = FC.NUMFECCRE 
              AND T.CODRTA      = 'V' 
              AND T.TIPTRA       < 80000 
              AND ((@codSubRede = 0) OR T.CODSUBREDE = @codSubRede)
        GROUP BY FC.DATPGTO
                ,FC.NUMFECCRE
                ,FC.CODCRE
                ,FC.VALLIQ  

   END

    INSERT INTO @FECHAMENTOSCRED
            SELECT  'CRED'                                          AS  TIPPAG   
                   ,DAT      
                   ,SUM(VALLIQ)                                     AS VALLIQ
             FROM  @FECHAMENTOSCREDAUX
             GROUP BY DAT
             
    
    ----------------------------------------CALCULO PARA VALOR A RECEBER------------------------------
    
        BEGIN    

              INSERT INTO @FECHAMENTOSCLI
                  SELECT FC.DATPGTO
                        ,SUM(FC.TOTAL)     AS  VLRECEBER        
                    FROM FECHCLIENTE FC 
                   WHERE fc.DATPGTO      >= @dtInicio
                     AND fc.DATPGTO    <= @dtFim
                     AND fc.TOTAL      > 0     
                     AND FC.DATFECLOT  IS NOT NULL 
                     AND ((@codSubRede = 0) OR FC.CODSUBREDE = @codSubRede)
                GROUP BY FC.DATPGTO     
        END 
    
        BEGIN  
                INSERT INTO @FECHAMENTOSCLI
               SELECT  DATEADD(dd, DATEDIFF(dd, 0, C.DTAUTORIZ), 0)  AS DATA
                     ,SUM(C.VALOR)                          AS VALOR
                 FROM CARGAC C
                WHERE C.DTAUTORIZ      >= @dtInicio
                  AND C.DTAUTORIZ      <= @dtFim
                  AND C.DTCARGA         > '19000101'
                  AND ((@codSubRede = 0) OR C.CODSUBREDE = @codSubRede)
             GROUP BY DATEADD(dd, DATEDIFF(dd, 0, C.DTAUTORIZ), 0)   

        END 
 
 
    ----------------------------------------RETORNO DA PROCEDURE------------------------------
    INSERT INTO @TABRETORNO
    SELECT   LEFT(CONVERT(varchar, CRED.DAT,112),6)                          AS  MESREF
            ,0                                                          AS  [RECEITA]   
            ,SUM(CRED.VALLIQ)                                           AS  [DESPESA]   
     FROM @FECHAMENTOSCRED  CRED
    GROUP BY CRED.DAT

    UNION     

    SELECT   LEFT(CONVERT(varchar, CLI.DAT,112),6)                   AS  MESREF
            ,SUM(CLI.VALOR)                                             AS  [RECEITA]   
            ,0                                                          AS  [DESPESA]   
     FROM @FECHAMENTOSCLI   CLI
    GROUP BY CLI.DAT
             
              
    SELECT   SUBSTRING(TAB.DATRET, 5,2) + '/' + SUBSTRING(TAB.DATRET,0,5) AS MESREF
            ,SUM(TAB.VALRECEITA)      AS  [RECEITA]   
            ,SUM(TAB.VALDISPESA)    AS  [DESPESA]   
     FROM @TABRETORNO TAB
    GROUP BY TAB.DATRET
    ORDER BY TAB.DATRET DESC
  

END