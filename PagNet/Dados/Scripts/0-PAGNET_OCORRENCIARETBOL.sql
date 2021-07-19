BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_OCORRENCIARETBOL')
	BEGIN
			DROP TABLE PAGNET_OCORRENCIARETBOL
	END
END
BEGIN
    CREATE TABLE PAGNET_OCORRENCIARETBOL
    (
         codOcorrenciaRetBol   CHAR(2) NOT NULL PRIMARY KEY
        ,Descricao             CHAR(250)
    )

END
BEGIN
	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('01','c�digo do banco invalido')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('02','c�digo do registro detalhe inv�lido')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('03','c�digo do segmento invalido')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES( '04','c�digo do movimento n�o permitido para carteira ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('05','c�digo de movimento invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('06','tipo/numero de inscri��o do Benefici�rio inv�lidos ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('07','agencia/conta/DV invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('08','nosso numero invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('09','nosso numero duplicado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('10','carteira invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('11','forma de cadastramento do titulo invalida Se desconto, titulo rejeitado - opera��o de desconto / hor�rio limite. ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('12','tipo de documento invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('13','identifica��o da emiss�o do Boleto invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('14','identifica��o da distribui��o do Boleto invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('15','caracter�sticas da cobran�a incompat�veis ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('16','data de vencimento invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('17','data de vencimento anterior a data de emiss�o ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('18','vencimento fora do prazo de opera��o ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('19','titulo a cargo de bancos correspondentes com vencimento inferior a xx dias ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('20','valor do t�tulo invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('21','esp�cie do titulo invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('22','esp�cie n�o permitida para a carteira ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('23','aceite invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('24','Data de emiss�o inv�lida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('25','Data de emiss�o posterior a data de entrada ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('26','C�digo de juros de mora inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('27','Valor/Taxa de juros de mora inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('28','C�digo de desconto inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('29','Valor do desconto maior ou igual ao valor do t�tulo ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('30','Desconto a conceder n�o confere  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('31','Concess�o de desconto - j� existe desconto anterior ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('32','Valor do IOF  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('33','Valor do abatimento inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('34','Valor do abatimento maior ou igual ao valor do t�tulo ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('35','Abatimento a conceder n�o confere ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('36','Concess�o de abatimento - j� existe abatimento anterior ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('37','C�digo para protesto inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('38','Prazo para protesto inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('39','Pedido de protesto n�o permitido para o t�tulo ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('40','T�tulo com ordem de protesto emitida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('41','Pedido de cancelamento/susta��o para t�tulos sem instru��o de protesto ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('42','C�digo para baixa/devolu��o inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('43','Prazo para baixa/devolu��o inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('44','C�digo de moeda inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('45','Nome do Pagador n�o informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('46','Tipo /N�mero de inscri��o do Pagador inv�lidos ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('47','Endere�o do Pagador n�o informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('48','CEP inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('49','CEP sem pra�a de cobran�a (n�o localizado)  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('50','CEP referente a um Banco Correspondente ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('51','CEP incompat�vel com a unidade de federa��o ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('52','Unidade de federa��o inv�lida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('53','Tipo/N�mero de inscri��o do sacador/avalista inv�lidos ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('54','Sacador/Avalista n�o informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('55','Nosso n�mero no Banco Correspondente n�o informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('56','C�digo do Banco Correspondente n�o informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('57','C�digo da multa inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('58','Data da multa inv�lida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('59','Valor/Percentual da multa inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('60','Movimento para t�tulo n�o cadastrado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('61','Altera��o de ag�ncia cobradora/dv inv�lida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('62','Tipo de impress�o inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('63','Entrada para t�tulo j� cadastrado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('64','N�mero da linha inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('65','A esp�cie de t�tulo n�o permite a instru��o')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('72','Entrada de t�tulo Sem Registro ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('90','Identificador/Quantidade de Parcelas de carn� invalido  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('91','T�tulo Descontado, instru��o n�o permititda ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('92','Data de Desconto Inv�lida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('93','N�mero do lote remessa inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B2','Valor Nominal do T�tulo Conflitante ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B3','Tipo de Pagamento Inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B4','Valor M�ximo ou Percentual M�ximo Inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B5','Valor M�nimo ou Percentual M�nimo Inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z1','Quantidade de Pagamento Poss�veis Inv�lido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z5','T�tulo com reserva, instru��o n�o permitida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z6','Segmento Inv�lido para o tipo de Carteira de Cobran�a ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z7','Instru��o exige segmento Y53')
END