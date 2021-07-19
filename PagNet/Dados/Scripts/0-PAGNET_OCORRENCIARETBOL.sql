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
		VALUES('01','código do banco invalido')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('02','código do registro detalhe inválido')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('03','código do segmento invalido')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES( '04','código do movimento não permitido para carteira ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('05','código de movimento invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('06','tipo/numero de inscrição do Beneficiário inválidos ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('07','agencia/conta/DV invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('08','nosso numero invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('09','nosso numero duplicado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('10','carteira invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('11','forma de cadastramento do titulo invalida Se desconto, titulo rejeitado - operação de desconto / horário limite. ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('12','tipo de documento invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('13','identificação da emissão do Boleto invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('14','identificação da distribuição do Boleto invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('15','características da cobrança incompatíveis ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('16','data de vencimento invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('17','data de vencimento anterior a data de emissão ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('18','vencimento fora do prazo de operação ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('19','titulo a cargo de bancos correspondentes com vencimento inferior a xx dias ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('20','valor do título invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('21','espécie do titulo invalida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('22','espécie não permitida para a carteira ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('23','aceite invalido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('24','Data de emissão inválida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('25','Data de emissão posterior a data de entrada ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('26','Código de juros de mora inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('27','Valor/Taxa de juros de mora inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('28','Código de desconto inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('29','Valor do desconto maior ou igual ao valor do título ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('30','Desconto a conceder não confere  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('31','Concessão de desconto - já existe desconto anterior ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('32','Valor do IOF  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('33','Valor do abatimento inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('34','Valor do abatimento maior ou igual ao valor do título ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('35','Abatimento a conceder não confere ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('36','Concessão de abatimento - já existe abatimento anterior ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('37','Código para protesto inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('38','Prazo para protesto inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('39','Pedido de protesto não permitido para o título ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('40','Título com ordem de protesto emitida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('41','Pedido de cancelamento/sustação para títulos sem instrução de protesto ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('42','Código para baixa/devolução inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('43','Prazo para baixa/devolução inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('44','Código de moeda inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('45','Nome do Pagador não informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('46','Tipo /Número de inscrição do Pagador inválidos ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('47','Endereço do Pagador não informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('48','CEP inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('49','CEP sem praça de cobrança (não localizado)  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('50','CEP referente a um Banco Correspondente ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('51','CEP incompatível com a unidade de federação ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('52','Unidade de federação inválida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('53','Tipo/Número de inscrição do sacador/avalista inválidos ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('54','Sacador/Avalista não informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('55','Nosso número no Banco Correspondente não informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('56','Código do Banco Correspondente não informado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('57','Código da multa inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('58','Data da multa inválida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('59','Valor/Percentual da multa inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('60','Movimento para título não cadastrado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('61','Alteração de agência cobradora/dv inválida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('62','Tipo de impressão inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('63','Entrada para título já cadastrado ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('64','Número da linha inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('65','A espécie de título não permite a instrução')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('72','Entrada de título Sem Registro ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('90','Identificador/Quantidade de Parcelas de carnê invalido  ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('91','Título Descontado, instrução não permititda ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('92','Data de Desconto Inválida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('93','Número do lote remessa inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B2','Valor Nominal do Título Conflitante ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B3','Tipo de Pagamento Inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B4','Valor Máximo ou Percentual Máximo Inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('B5','Valor Mínimo ou Percentual Mínimo Inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z1','Quantidade de Pagamento Possíveis Inválido ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z5','TÍtulo com reserva, instrução não permitida ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z6','Segmento Inválido para o tipo de Carteira de Cobrança ')

	INSERT INTO PAGNET_OCORRENCIARETBOL (codOcorrenciaRetBol, Descricao)
		VALUES('Z7','Instrução exige segmento Y53')
END