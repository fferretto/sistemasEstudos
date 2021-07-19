
/*----------------------------------------------------------------------------*/      
/* SRV_CONSULTA_TAXAS_FECHCLI POS                                             */           
/* CONSULTA TAXAS DOS FECHAMENTOS DE CLIENTES POR DATA DE                     */
/*   FECHAMENTO OU PAGAMENTO                                                  */
/*                                                               VERSÃO 1     */
/* AUTOR: ALEX FABIANO MODESTO                                                */
/*                                                                            */
/* PARAMETROS:                                                                */      
/*                                                                            */        
/*@SISTEMA ->  Sistema 0 = PJ (Pós-pago), mantido só para compatibilidade     */        
/*             só vale para o siste pós-pago                                  */
/*                                                                            */        
/*                                                                            */   
/* @SELECAO -> Seleção 0 = Por Data de fechamento                             */      
/*                     1 = Por Data de pagamento                              */
/*                                                                            */        
/*@DATA_INI -> Data inicial do período                                        */      
/*@DATA_FIM -> Data Final do período                                          */
/*----------------------------------------------------------------------------*/       

CREATE PROCEDURE [dbo].[SRV_CONSULTA_TAXAS_FECHCLI]
	@SISTEMA smallint = null, @SELECAO smallint = 0, 
	@DATINI datetime = null, @DATFIM datetime = null
	--,@ERRO int output, @MSG_ERRO varchar(255) output
AS
BEGIN

SET NOCOUNT ON

--Pega somente a data
SET @DATINI = convert(datetime,round(convert(float,@DATINI),0,1))
SET @DATFIM = convert(datetime,round(convert(float,@DATFIM),0,1))


DECLARE @TABFECHTEMP TABLE
(
	[CODCLI] [int],
	[LOTE] [int],
	[CNPJ] [char](14),
	[DTFECHTO] [datetime],
	[DTPAGTO] [datetime],
	[VALTAXA] [numeric](12,2),
	[TAXA] [varchar](50),
	[CODSUBREDE] [int],
	[SUBREDE] [varchar](20)
)

IF @SISTEMA = 0
BEGIN
	--POS PAGO
	INSERT INTO @TABFECHTEMP
	SELECT T.CODCLI, T.NUMFECCLI, C.CGC, F.DATFECLOT, F.DATPGTO, SUM(T.VALTRA) AS VALTAXA, UPPER(TT.DESTIPTRA) AS TAXA, S.CODSUBREDE, S.NOMSUBREDE 
	FROM TRANSACAO T 
	JOIN FECHCLIENTE F ON  F.CODCLI = T.CODCLI AND F.NUMFECCLI = T.NUMFECCLI AND F.DATFECLOT = T.DATFECCLI
	JOIN VCLIENTE C ON C.CODCLI = T.CODCLI
	JOIN SUBREDE S ON S.CODSUBREDE = C.CODSUBREDE
	JOIN TIPTRANS TT ON TT.TIPTRA = T.TIPTRA
	WHERE 
	((@SELECAO = 0 AND F.DATFECLOT >= @DATINI AND F.DATFECLOT <= @DATFIM) OR
	 (@SELECAO = 1 AND F.DATPGTO >= @DATINI AND F.DATPGTO <= @DATFIM))
	 AND F.DATFECLOT IS NOT NULL AND
	((T.TIPTRA >= 999100 AND T.TIPTRA <= 999199) OR T.TIPTRA = 999007)
	AND T.VALTRA > 0
	GROUP BY 
	T.CODCLI, T.NUMFECCLI, C.CGC, F.DATFECLOT, F.DATPGTO, TT.DESTIPTRA, S.CODSUBREDE, S.NOMSUBREDE 
END

SELECT * FROM  @TABFECHTEMP

END
SET NOCOUNT OFF

