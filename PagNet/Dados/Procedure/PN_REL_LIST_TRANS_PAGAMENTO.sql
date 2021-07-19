
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------------*/    
/*                                                                                                                                                                   */         
/* PN_REL_LIST_TRANS_PAGAMENTO PARA O NETCARD Versao 1.0                                                                                                             */
/* DESCRICÃO:  Relatório de Lista de Transações de pagamentos realizados e seus status                                                                               */
/* CRIAÇÃO : Luiz Felipe - Abril/2019                                                                                                                                */
/* REVISÃO :                                                                                                                                                         */
/*                                                                                                                                                                   */   
/*              ''''                                                                                                                                                 */    
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------------*/  

CREATE PROCEDURE [dbo].[PN_REL_LIST_TRANS_PAGAMENTO] 
                        @FORMATO            SMALLINT    = 0 
                       ,@DATA_INI           DATETIME
                       ,@DATA_FIM           DATETIME
                       ,@CODBANCO           VARCHAR(3)  = ''  
                       ,@CODFAVORECIDO      INT         = 0 
                       ,@CODEMPRESA         INT    
                       ,@UF                 VARCHAR(2)  = ''    
                       ,@CIDADE             VARCHAR(11) = ''       
                       ,@COD_STATUS_REL     VARCHAR(100)      
                       ,@ERRO               INT             OUTPUT   
                       ,@MSG_ERRO           VARCHAR(800)    OUTPUT
AS


BEGIN      
    SET NOCOUNT ON;   

    /* PARA TESTAR RETIRE OS COMENTARIOS ABAIXO*/

    --DECLARE  @DATA_INI              DATETIME
    --        ,@DATA_FIM              DATETIME
    --        ,@CODEMPRESA            INT        
    --        ,@CODFAVORECIDO         INT        
    --        ,@CODBANCO              VARCHAR(3)      
    --        ,@FORMATO               SMALLINT  
    --        ,@UF                    VARCHAR(2)    
    --        ,@CIDADE                VARCHAR(11)   
    --        ,@COD_STATUS_REL        VARCHAR(100)      
    --        ,@ERRO                  INT                
    --        ,@MSG_ERRO              VARCHAR(800)    

    --SELECT @DATA_INI        = '20200909'
    --        ,@DATA_FIM        = '20200913'
    --        ,@CODEMPRESA      = 1
    --        ,@CODFAVORECIDO          = 0
    --        ,@CODBANCO        = ''
    --        ,@FORMATO         = 0
    --        ,@UF              = ''
    --        ,@CIDADE          = ''
    --        ,@COD_STATUS_REL = '99991111'


    /*-----------------------------------------*/
    DECLARE @MAXKEY INT
    SET @ERRO = 0
    SET @MSG_ERRO  = ''

    --WAITFOR DELAY '00:00:40';
    
    --SELECT @MAXKEY = ISNULL(MAX(COD_RESULTADO), 0) FROM PAGNET_RELATORIO_RESULTADO
    
    DECLARE @PAGAMENTOS TABLE   
    (  
         ID              INT NOT NULL IDENTITY(1,1)
        ,DTPAGAMENTO     DATETIME  
        ,NMCREDENCIADO   VARCHAR(200)  
        ,CNPJ            VARCHAR(20)  
        ,STATUS          VARCHAR(60)  
        ,VALOR           varchar(15)  
        ,VALORCONTA      decimal  
    )  
  
    INSERT INTO @PAGAMENTOS  
    SELECT P.DATREALPGTO   
            ,FAV.NMFAVORECIDO  
            ,dbo.formatarCNPJCPF(FAV.CPFCNPJ)  
            ,P.STATUS   
            ,dbo.converter_moeda(SUM(P.VALTOTAL), 'pt-br')   
            ,SUM(P.VALTOTAL)
        FROM PAGNET_EMISSAO_TITULOS     P  
            ,PAGNET_CADFAVORECIDO       FAV   
            ,PAGNET_CONTACORRENTE       CC -- WITH(INDEX(IX_TRANSACAO_CODCRE_NUMFECCRE))  
       WHERE P.DATREALPGTO      >= @DATA_INI  
        AND P.DATREALPGTO      <= @DATA_FIM  
        AND P.CODEMPRESA           = @CODEMPRESA  
        AND P.CODFAVORECIDO        = FAV.CODFAVORECIDO  
        AND ((@CODFAVORECIDO      = 0)    OR P.CODFAVORECIDO         = @CODFAVORECIDO)  
        AND ((@CODBANCO    = '')   OR CC.CODBANCO      = @CODBANCO)  
        AND ((@UF    = '') OR FAV.UF         = @UF)  
        AND ((@CIDADE    = '')     OR FAV.CIDADE       = @CIDADE)  
        AND P.STATUS               IN ('BAIXADO','BAIXADO_MANUALMENTE')   
        AND CC.CODCONTACORRENTE    = P.CODCONTACORRENTE 
        GROUP BY P.DATREALPGTO   
                ,FAV.NMFAVORECIDO  
                ,dbo.formatarCNPJCPF(FAV.CPFCNPJ)  
                ,P.STATUS   
        
               
  
    /* EMISSAO NO FORMATO TEXTO */  
    IF @FORMATO = 0   
    BEGIN  
        DECLARE @CABEC1      VARCHAR(121)        
        DECLARE @CABEC2      VARCHAR(121)        
        DECLARE @CABEC3      VARCHAR(121)        
        DECLARE @CABEC4      VARCHAR(121)        
        DECLARE @CABEC5      VARCHAR(121)        
        DECLARE @CABEC6      VARCHAR(121)        
        DECLARE @TRACO       VARCHAR(121)    
        DECLARE @OPERADORA   CHAR(30)  
        DECLARE @QT_ESPACO1  INT  
        DECLARE @QT_ESPACO2  INT  
  
        SELECT @OPERADORA = VAL 
          FROM CONFIG_JOBS 
         WHERE ID0 = 'OPERADORA'  
  
        /* PARA CENTRALIZAR O CABECALHO COM O NOME DA OPERADORA E O TEXTO SISTEMA NETCARD */  
        SET @QT_ESPACO1 = ((118-LEN(LTRIM(RTRIM(@OPERADORA))))/2)  
        SET @QT_ESPACO2 = ((118-(@QT_ESPACO1 + LEN(@OPERADORA)))/4)+2          
        SET @CABEC1    = REPLICATE(' ', @QT_ESPACO1 ) + @OPERADORA + SPACE(118-@QT_ESPACO1+LEN(@OPERADORA) )  
        SET @CABEC1    = STUFF(@CABEC1, 118-18, 18, 'SISTEMA PAGNET ')        
        SET @CABEC2    = SPACE(40) + '  LISTAGEM DO PAGAMENTOS REALIZADOS '   
        SET @CABEC3    = SPACE(40) + 'NO PERIODO DE ' +     
        CONVERT(VARCHAR, @DATA_INI, 103) +   
        ' ATE ' +   
        CONVERT(VARCHAR, @DATA_FIM, 103)  
                      
        SET @CABEC4    = convert(char(17),'DATA PAGAMENTO') + convert(char(38), 'CREDENCIADO') + convert(char(20),'CNPJ') +  CONVERT(char(28), 'STATUS') + CONVERT(char(15),'VALOR PAGO')      
        SET @TRACO     =  REPLICATE(CHAR(151),118)   
                             
        /*-------------------------------*/              
        DECLARE  @LINREL  TABLE 
        ( 
            ID          INT IDENTITY(1,1),
            [LINHAIMP]  VARCHAR(148),        
            [TIP]       CHAR(1)   
        )    
       
        /* IMPRIME O CABECALHO DA PAGINA */        
        INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC1,   0        
        INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC2,   0   
        INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC3,   0        
        INSERT @LINREL(LINHAIMP, TIP) SELECT SPACE(10), 0    
        INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO,    0        
        INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC4,   0  
  
           
        INSERT @LINREL(LINHAIMP, TIP)  
        SELECT CONVERT(VARCHAR, DTPAGAMENTO, 103) + '       ' +  
                CONVERT(CHAR(37),NMCREDENCIADO) + ' ' +  
                CONVERT(CHAR(19),CNPJ) + ' ' +  
                CONVERT(CHAR(27), STATUS) + ' ' +  
                CONVERT(CHAR(15),VALOR)  AS [LINHAIMP]   
                ,2 AS [TIP]   
          FROM @PAGAMENTOS   
       ORDER BY DTPAGAMENTO ASC  
              
        --INCLUSÃO TO TOTAL GERAL          
        INSERT @LINREL(LINHAIMP, TIP) 
        SELECT @TRACO,    2  
               
        INSERT @LINREL(LINHAIMP, TIP)  
        SELECT CONVERT(CHAR(84),' ') + 'Total Geral     ' +   
               CONVERT(CHAR(19),dbo.converter_moeda(SUM(VALORCONTA), 'pt-br') )  AS [LINHAIMP]   
              ,2 AS [TIP]   
         FROM @PAGAMENTOS   
     
   
        INSERT INTO PAGNET_RELATORIO_RESULTADO
        SELECT @COD_STATUS_REL, 
               LINHAIMP, TIP 
         FROM @LINREL  
    END  
  
    /* EMISSÃO NO FORMATO EXCELL */  
    IF (@FORMATO = 1)  --EXCELL SEM CABECALHO  
    BEGIN  
        SET @MAXKEY = @MAXKEY + 1

        INSERT INTO PAGNET_RELATORIO_RESULTADO
        SELECT @COD_STATUS_REL, 'Data de Pagamento;Credenciado;CNPJ;Status;Valor Pago;', ''

        INSERT INTO PAGNET_RELATORIO_RESULTADO
        SELECT @COD_STATUS_REL, CONVERT(VARCHAR,DTPAGAMENTO,103) + ';' + NMCREDENCIADO + ';' + CNPJ + ';' + STATUS + ';' + CONVERT(VARCHAR,VALOR) + ';' , ''         
        FROM @PAGAMENTOS                 
        ORDER BY DTPAGAMENTO ASC  
    END  
END   
  
SET ANSI_NULLS ON  
  
  
  
  
  