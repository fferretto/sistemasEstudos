
--/*-------------------------------------------------------------------------------------------------------------------------------------------------------------------*/    
--/*                                                                                                                                                                   */         
--/* PN_REL_LIST_PAG_RECUSADO PARA O NETCARD Versao 1.0                                                                                                             */
--/* DESCRICÃO:  Relatório de Lista de Transações de pagamentos realizados e seus status                                                                               */
--/* CRIAÇÃO : Luiz Felipe - Abril/2019                                                                                                                                */
--/* REVISÃO :                                                                                                                                                         */
--/*                                                                                                                                                                   */   
--/*              ''''                                                                                                                                                 */    
--/*-------------------------------------------------------------------------------------------------------------------------------------------------------------------*/  

CREATE PROCEDURE [dbo].[PN_REL_LIST_PAG_RECUSADO] 
                        @FORMATO    SMALLINT    = 0 
                       ,@DATA_INI   DATETIME
                       ,@DATA_FIM   DATETIME
                       ,@CODBANCO   VARCHAR(3)  = ''  
                       ,@CODFAVORECIDO     INT         = 0 
                       ,@CODEMPRESA INT    
                       ,@UF         VARCHAR(2)  = ''    
                       ,@CIDADE     VARCHAR(11) = ''    
AS

BEGIN      
	SET NOCOUNT ON;   
    DECLARE @AUX_SEG INT

/* PARA TESTAR RETIRE OS COMENTARIOS ABAIXO*/

--DECLARE  @DATA_INI          DATETIME
--        ,@DATA_FIM          DATETIME
--        ,@CODEMPRESA        INT        
--        ,@CODFAVORECIDO     INT        
--        ,@CODBANCO          VARCHAR(3)      
--        ,@FORMATO           SMALLINT  

--SELECT @DATA_INI            = '20190523'
--      ,@DATA_FIM            = '20190830'
--      ,@CODEMPRESA          = 1
--      ,@CODFAVORECIDO       = 0
--      ,@CODBANCO            = ''
--      ,@FORMATO             = 1


/*-----------------------------------------*/
    DECLARE @ERRO INT
	SET @ERRO = 0
	

	IF @ERRO = 0 
	BEGIN
		
        DECLARE @TABTEMP TABLE 
        (
            DTPAGAMENTO             DATETIME
           ,CODFAVORECIDO           NVARCHAR(20)
           ,NMFAVORECIDO            VARCHAR(200)
           ,CNPJ                    VARCHAR(20)
           ,VALOR                   VARCHAR(15)
           ,OCORRENCIARETORNO       VARCHAR(8000)
        )

        INSERT INTO @TABTEMP
        SELECT P.DTREALPAGAMENTO 
              ,CONVERT(VARCHAR(20),FAV.CODFAVORECIDO)
              ,FAV.NMFAVORECIDO
              ,dbo.formatarCNPJCPF(FAV.CPFCNPJ)
              ,dbo.converter_moeda(P.VALOR, 'pt-br')
              ,CASE WHEN LEN(P.OCORRENCIARETORNO) <= 10 THEN DBO.PagNet_RetOcorrenciaRetorno(P.OCORRENCIARETORNO) ELSE P.OCORRENCIARETORNO END AS OCORRENCIARETORNO
        FROM PAGNET_TITULOS_PAGOS       P
            ,PAGNET_CADFAVORECIDO       FAV 
            ,PAGNET_CONTACORRENTE       CC -- WITH(INDEX(IX_TRANSACAO_CODCRE_NUMFECCRE))
       WHERE P.DTREALPAGAMENTO      >= @DATA_INI
         AND P.DTREALPAGAMENTO      <= @DATA_FIM
         AND P.CODEMPRESA           = @CODEMPRESA
         AND P.CODFAVORECIDO        = FAV.CODFAVORECIDO
         AND ((@CODFAVORECIDO      = 0)    OR P.CODFAVORECIDO         = @CODFAVORECIDO)
         AND ((@CODBANCO    = '')   OR CC.CODBANCO      = @CODBANCO)
         AND P.STATUS               IN ('RECUSADO') 
         AND CC.CODCONTACORRENTE    = P.CODCONTACORRENTE 

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

			SELECT @OPERADORA = VAL FROM CONFIG_JOBS WHERE ID0 = 'OPERADORA'

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
		                  
			SET @CABEC4    = convert(char(13),'DATA PGTO') + convert(char(38), 'FAVORECIDO') + convert(char(20),'CNPJ') + CONVERT(char(15),'VALOR PAGO')    +  CONVERT(char(32), 'MENSAGEM DE RETORNO')  
 			SET @TRACO     =  REPLICATE(CHAR(151),118) 
            		             
			/*-------------------------------*/            
			DECLARE  @LINREL  TABLE ( [LINHAIMP]  varchar(148),      
		 							  [TIP]       char(1) 
									 )  
	    
			/* IMPRIME O CABECALHO DA PAGINA */      
			INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC1,   0      
			INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC2,   0 
			INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC3,   0      
			INSERT @LINREL(LINHAIMP, TIP) SELECT SPACE(10), 0  
			INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO,    0      
			INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC4,   0

         
			INSERT @LINREL(LINHAIMP, TIP)
			SELECT CONVERT(VARCHAR, DTPAGAMENTO, 103) + '   ' +
				   convert(char(37),CODFAVORECIDO + '-' + NMFAVORECIDO) + ' ' +
				   convert(char(19),CNPJ) + ' ' +
				   CONVERT(char(15),VALOR)   + ' ' +
				   CONVERT(char(32),OCORRENCIARETORNO)
                   AS [LINHAIMP] 
                  ,2 AS [TIP] 
			FROM @TABTEMP 
        ORDER BY DTPAGAMENTO ASC
        
			SELECT LINHAIMP, TIP FROM @LINREL
		END

		/* EMISSÃO NO FORMATO EXCELL */
		IF (@FORMATO = 1)  --EXCELL SEM CABECALHO
		BEGIN
			SELECT convert(varchar,DTPAGAMENTO,103)              AS [Data de Pagamento]
                  ,CODFAVORECIDO            AS [Código do Favorecido/Centralizadora]
                  ,NMFAVORECIDO             AS [Nome do Favorecido/Centralizadora]
			      ,CNPJ                     AS [CNPJ]
			      ,VALOR                    AS [Valor Pago]
			      ,OCORRENCIARETORNO        AS [Mensagem de Retorno do Banco]
			 FROM @TABTEMP	             	
            ORDER BY DTPAGAMENTO ASC
		END
	END
END

SET ANSI_NULLS ON






