/*----------------------------------------------------------------------------*/    
/*                                                                            */         
/* PROC_PAGNET_MAIORES_RECEITAS  Versao 1.0                                   */         
/* CRIAÇÃO : Luiz Felipe - JAN/2020                                           */         
/* REVISÃO :                                                                  */
/*                                                                            */   
/*                                                                            */    
/*----------------------------------------------------------------------------*/  


CREATE PROCEDURE [dbo].[PROC_PAGNET_MAIORES_RECEITAS]
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
         NOME               NVARCHAR(200)
        ,ORIGEM             NVARCHAR(100)             
        ,VALOR              DECIMAL(15,2)   
    )
    
    INSERT INTO @TABTEMP
    SELECT
           FAV.NMFAVORECIDO,
          'TAXAS CENTRALIZADORA' AS ORIGEM, 
          SUM(ABS(TT.VALOR)) AS VALOR
     FROM PAGNET_EMISSAO_TITULOS ET
     JOIN PAGNET_TAXAS_TITULOS TT ON ET.CODTITULO = TT.CODTITULO
     JOIN PAGNET_CADFAVORECIDO FAV ON FAV.CODFAVORECIDO = ET.CODFAVORECIDO
    WHERE ET.DATREALPGTO >=  @DTINICIO
      AND ET.DATREALPGTO <= @DTFIM
      AND ET.STATUS IN ('BAIXADO', 'BAIXADO_MANUALMENTE')
      AND ET.CODCONTACORRENTE = @CODCONTACORRENTE
      AND ET.CODEMPRESA = @CODEMPRESA
      AND TT.VALOR < 0
    GROUP BY FAV.NMFAVORECIDO



    INSERT INTO @TABTEMP
    SELECT 
           CLI.NMCLIENTE,
          'CLIENTE NETCARD' AS ORIGEM, 
           SUM(EF.VLPGTO)
      FROM PAGNET_EMISSAOFATURAMENTO EF
      JOIN PAGNET_CADCLIENTE CLI ON CLI.CODCLIENTE = EF.CODCLIENTE
     WHERE EF.DATVENCIMENTO >=  @DTINICIO
       AND EF.DATVENCIMENTO <= @DTFIM
       AND EF.STATUS IN ('LIQUIDADO', 'LIQUIDADO_MANUALMENTE', 'CONSOLIDADO')
       AND EF.CODCONTACORRENTE = @CODCONTACORRENTE
       AND EF.CODEMPRESA = @CODEMPRESA
    GROUP BY CLI.NMCLIENTE

    SELECT TOP 8 
            NOME,
            ORIGEM,
            VALOR     
      FROM @TABTEMP
      ORDER BY VALOR DESC
END
