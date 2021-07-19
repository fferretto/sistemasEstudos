IF EXISTS ( SELECT  1
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'PROC_PAGNET_INSERE_FECHECRE')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
    DROP PROCEDURE PROC_PAGNET_INSERE_FECHECRE
END

GO

/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_INSERE_FECHECRE      PARA O NETCARD Versao 1.0                 */         
/* CRIAÇÃO : Luiz Felipe - MARÇO/2019                                         */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  

CREATE PROCEDURE [dbo].[PROC_PAGNET_INSERE_FECHECRE]
                        @dtInicio       DATETIME
                       ,@dtFim          DATETIME
                       ,@codPagamento   INT  
                       ,@codcen         INT  
                       ,@cartaoPre      BIT         = 0
                       ,@cartaoPos      BIT         = 0  
AS

BEGIN

--------TESTE
--DECLARE    @dtInicio       DATETIME
--          ,@dtFim          DATETIME
--          ,@codPagamento   INT  
--          ,@codcen         INT  
--          ,@cartaoPre      BIT        
--          ,@cartaoPos      BIT         


--SELECT @dtInicio        = '20190505'
--      ,@dtFim           = '20190515'
--      ,@codPagamento    = 8
--      ,@codcen          = 437642
--      ,@cartaoPre       = 1
--      ,@cartaoPos       = 1
-----------------------


    DECLARE @TABTEMP TABLE
    (
             CODTRANSACAOTRANSFERENCIA      INT NOT NULL  
            ,CODCRE                         INT NOT NULL
            ,CODCEN                         INT NOT NULL
            ,DATPGTO                        DATETIME NOT NULL
            ,NUMFECCRE                      INT NOT NULL
            ,VALLIQ                         DECIMAL(13,2) NOT NULL
            ,SISTEMA                        BIT NOT NULL  
    )

    IF @cartaoPos = 1
    BEGIN

       --POS-PAGO
       INSERT INTO @TABTEMP
            SELECT @codPagamento
                  ,FC.CODCRE
                  ,FC.CODCEN
                  ,FC.DATPGTO
                  ,FC.NUMFECCRE
                  ,FC.VALLIQ
                  ,0
            FROM FECHCRED FC 
            WHERE FC.DATPGTO    >= @dtInicio
              AND FC.DATPGTO    <= @dtFim
              AND FC.CODCEN      = @codCen 
              AND FC.VALLIQ      > 0                    
         GROUP BY FC.CODCRE
                 ,FC.CODCEN
                 ,FC.DATPGTO
                 ,FC.NUMFECCRE
                 ,FC.VALLIQ

        END
        
    IF @cartaoPre = 1
    BEGIN

        --PRE-PAGO
        INSERT INTO @TABTEMP
            SELECT @codPagamento
                  ,FC.CODCRE
                  ,FC.CODCEN
                  ,FC.DATPGTO
                  ,FC.NUMFECCRE
                  ,FC.VALLIQ
                  ,1
            FROM FECHCREDVA FC 
            WHERE FC.DATPGTO    >= @dtInicio
              AND FC.DATPGTO    <= @dtFim
              AND FC.CODCEN      = @codCen  
              AND FC.VALLIQ      > 0        
         GROUP BY FC.CODCRE
                 ,FC.CODCEN
                 ,FC.DATPGTO
                 ,FC.NUMFECCRE
                 ,FC.VALLIQ
        
        END
                       

     INSERT INTO PAGNET_FECHECRED
          SELECT CODTRANSACAOTRANSFERENCIA   
                ,CODCRE                      
                ,CODCEN                      
                ,DATPGTO                     
                ,NUMFECCRE                   
                ,VALLIQ                      
                ,SISTEMA        
            FROM @TABTEMP 
        ORDER BY DATPGTO
    
    select 'OK' as resultado

END
