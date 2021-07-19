BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_OCORRENCIARETPAG')
	BEGIN
			DROP TABLE PAGNET_OCORRENCIARETPAG
	END
END
BEGIN
    CREATE TABLE PAGNET_OCORRENCIARETPAG
    (
         codOcorrenciaRetPag   CHAR(2) NOT NULL PRIMARY KEY
        ,Descricao             CHAR(8000)
    )
    
END

INSERT INTO PAGNET_OCORRENCIARETPAG
SELECT CODOCORRENCIARETPAG,
       DESCRICAO
FROM FACESP_NETCARDPJ..PAGNET_OCORRENCIARETPAG

--BEGIN
--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('00','Cr�dito ou D�bito Efetivado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('01','Insufici�ncia de Fundos - D�bito N�o Efetuado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('02','Cr�dito ou D�bito Cancelado pelo Pagador/Credor ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('03','D�bito Autorizado pela Ag�ncia - Efetuado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AA','Controle Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AB','Tipo de Opera��o Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AC','Tipo de Servi�o Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AE','Tipo/N�mero de Inscri��o Inv�lido (gerado na cr�tica ou para informar rejei��o)* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AF','C�di de Conv�nio Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AG','Ag�ncia/Conta Corrente/DV Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AH','N�mero Seq�encial do Registro no Lote Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AI','C�di de Segmento de Detalhe Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AJ','Tipo de Movimento Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AK','C�di da C�mara de Compensa��o do Banco do Favorecido/Deposit�rio Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AL','C�di do Banco do Favorecido ou Deposit�rio Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AM','Ag�ncia Mantenedora da Conta Corrente do Favorecido Inv�lida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AN','Conta Corrente/DV do Favorecido Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AO','Nome do Favorecido n�o Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AP','Data Lan�amento Inv�lida/Vencimento Inv�lido/Data de Pagamento n�o permitda. ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AQ','Tipo/Quantidade da Moeda Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AR','Valor do Lan�amento Inv�lido/Divergente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AS','Aviso ao Favorecido - Identifica��o Inv�lida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AT','Tipo/N�mero de Inscri��o do Favorecido/Contribuinte Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AU','Logradouro do Favorecido n�o Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AV','N�mero do Local do Favorecido n�o Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AW','Cidade do Favorecido n�o Informada ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AX','CEP/Complemento do Favorecido Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AY','Sigla do Estado do Favorecido Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AZ','C�di/Nome do Banco Deposit�rio Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BA','C�di/Nome da Ag�ncia Deposit�rio n�o Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BB','N�mero do Documento Inv�lido(Seu N�mero) ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BC','Nosso N�mero Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BD','Inclus�o Efetuada com Sucesso ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BE','Altera��o Efetuada com Sucesso ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BF','Exclus�o Efetuada com Sucesso ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BG','Ag�ncia/Conta Impedida Legalmente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B1','Bloqueado Pendente de Autoriza��o ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B3','Bloqueado pelo cliente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B4','Bloqueado pela captura de titulo da cobran�a ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B8','Bloqueado pela Valida��o de Tributos ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CA','C�di de barras - C�di do Banco Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CB','C�di de barras - C�di da Moeda Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CC','C�di de barras - D�gito Verificador Geral Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CD','C�di de barras - Valor do T�tulo Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CE','C�di de barras  - Campo Livre Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CF','Valor do Documento/Principal/menor que o minimo Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CH','Valor do Desconto Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CI','Valor de Mora Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CJ','Valor da Multa Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CK','Valor do IR Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CL','Valor do ISS Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CG','Valor do Abatimento inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CM','Valor do IOF Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CN','Valor de Outras Dedu��es Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CO','Valor de Outros Acr�scimos Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HA','Lote N�o Aceito ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HB','Inscri��o da Empresa Inv�lida para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HC','Conv�nio com a Empresa Inexistente/Inv�lido para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HD','Ag�ncia/Conta Corrente da Empresa Inexistente/Inv�lida para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HE','Tipo de Servi�o Inv�lido para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HF','Conta Corrente da Empresa com Saldo Insuficiente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HG','Lote de Servi�o fora de Seq��ncia ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HH','Lote de Servi�o Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HI','Arquivo n�o aceito ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HJ','Tipo de Registro Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HL','Vers�o de Layout Inv�lida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HU','IA Hora de Envio Inv�lida Pagamento exclusive em Cart�rio. ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IJ','Compet�ncia ou Per�odo de Referencia ou Numero da Parcela invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IL','Codi Pagamento / Receita n�o num�rico ou com zeros ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IM','Munic�pio Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IN','Numero Declara��o Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IO','Numero Etiqueta invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IP','Numero Notifica��o invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IQ','Inscri��o Estadual invalida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IR','Divida Ativa Invalida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IS','Valor Honor�rios ou Outros Acr�scimos invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IT','Per�odo Apura��o invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IU','Valor ou Percentual da Receita invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IV','Numero Referencia invalida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('SC','Valida��o parcial ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('TA','Lote n�o Aceito - Totais do Lote com Diferen�a ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('XB','N�mero de Inscri��o do Contribuinte Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('XC','C�di do Pagamento ou Compet�ncia ou N�mero de Inscri��o Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('XF','C�di do Pagamento ou Compet�ncia n�o Num�rico ou igual � zeros ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YA','T�tulo n�o Encontrado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YB','Identifica��o Registro Opcional Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YC','C�di Padr�o Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YD','C�di de Ocorr�ncia Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YE','Complemento de Ocorr�ncia Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YF','Alega��o j� Informada ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZA','Transferencia  Devolvida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZB','Transferencia mesma titularidade n�o permitida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZC','C�di pagamento Tributo inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZD','Compet�ncia Inv�lida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZE','T�tulo Bloqueado na base ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZF','Sistema em Conting�ncia � Titulo com valor maior que refer�ncia ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZG','Sistema em Conting�ncia � T�tulo vencido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZH','Sistema em conting�ncia -  T�tulo indexado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZI','Benefici�rio divergente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZJ','Limite de pagamentos parciais excedido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZK','T�tulo j� liquidado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZT','Valor outras entidades inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZU','Sistema Origem Inv�lido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZW','Banco Destino n�o recebe DOC ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZX','Banco Destino inoperante para DOC ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZY','C�di do Hist�rico de Credito Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZV','Autoriza��o iniciada no Internet Banking ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z0','Conta com bloqueio* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z1','Conta fechada. � necess�rio ativar a conta* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z2','Conta com movimento controlado* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z3','Conta cancelada* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z4','Registro inconsistente (T�tulo)* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z5','Apresenta��o indevida (T�tulo)* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z6','Dados do destinat�rio inv�lidos* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z7','Ag�ncia ou conta destinat�ria do cr�dito inv�lida* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z8','Diverg�ncia na titularidade* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z9','Conta destinat�ria do cr�dito encerrada* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C1','COMPROR � Devolvido por outros bancos** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C2','COMPROR � Recusado** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C3','COMPROR � Rejeitado por sistema** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C4','COMPROR � Rejeitado por hor�rio** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C6','COMPROR � Aprovado** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C7','COMPROR � Compromisso Inv�lido**')
		
--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('F1','CONFIRMING � Compromisso Liquidado***   ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('F2','CONFIRMING � Compromisso em Necia��o*** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O1','C�di da  OCT invalido ****  ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O2','Descri��o do remetente inv�lida ****  ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O3','Descri��o da finalidade inv�lida ****  ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O4','C�di Convenio cobran�a Inv�lido **** ')
        
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) 
--     values ('AD', 'Forma de Lan�amento Inv�lida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BH', 'Empresa n�o pau sal�rio')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BI', 'Falecimento do mutu�rio ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BJ', 'Empresa n�o enviou remessa do mutu�rio ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BK', 'Empresa n�o enviou remessa no vencimento ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BL', 'Valor da parcela inv�lida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BM', 'Identifica��o do contrato inv�lida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BN', 'Opera��o de Consigna��o Inclu�da com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BO', 'Opera��o de Consigna��o Alterada com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BP', 'Opera��o de Consigna��o Exclu�da com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BQ', 'Opera��o de Consigna��o Liquidada com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('CP', 'Valor do INSS Inv�lido')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HK', 'C�di Remessa / Retorno Inv�lido')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HM', 'Mutu�rio n�o identificado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HN', 'Tipo do benef�cio n�o permite empr�stimo ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HO', 'Benef�cio cessado/suspenso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HP', 'Benef�cio possui representante legal ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HQ', 'Benef�cio � do tipo PA (Pens�o aliment�cia) ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HR', 'Quantidade de contratos permitida excedida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HS', 'Benef�cio n�o pertence ao Banco informado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HT', 'In�cio do desconto informado j� ultrapassado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HV', 'Quantidade de parcela inv�lida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HW', 'Margem consign�vel excedida para o mutu�rio dentro do prazo do contrato ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HX', 'Empr�stimo j� cadastrado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HY', 'Empr�stimo inexistente ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HZ', 'Empr�stimo j� encerrado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H1', 'Arquivo sem trailer')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H2', 'Mutu�rio sem cr�dito na compet�ncia ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H3', 'N�o descontado � outros motivos ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H4', 'Retorno de Cr�dito n�o pa Estorno de pagamento quando os dados do favorecido esta incorreto ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H5', 'Cancelamento de empr�stimo retroativo ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H6', 'Outros Motivos de Glosa ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H7', 'Margem consign�vel excedida para o mutu�rio acima do prazo do contrato ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H8', 'Mutu�rio desligado do empregador �H9� = Mutu�rio afastado por licen�a ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('IA', 'Primeiro nome do mutu�rio diferente do primeiro nome do movimento do censo ou diferente da base de Titular do Benef�cio ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5A', 'Agendado sob lista de debito Pagamento agendado que faz parte de uma lista com um n�mero para autoriza��o. ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5B', 'Pagamento n�o autoriza sob lista de debito Pagamento da lista n�o foi autorizado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5C', 'Lista com mais de uma modalidade Lista de pagamento n�o permite mais de uma modalidade ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5D', 'Lista com mais de uma data de pagamento Lista de pagamento n�o permite mais de uma data de pagamento ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5E', 'N�mero de lista duplicado N�mero da lista enviado pelo cliente j� foi utilizado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5F', 'Lista de debito vencida e n�o autorizada Pagamentos que pertence a uma determinada lista est�o vencidos e n�o foram autorizados  ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5I', 'Ordem de Pagamento emitida  Pagamento realizado ao favorecido nesta data ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5M', 'N�mero de lista de debito invalida N�mero de lista inv�lido (deve ser num�rico) ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5T', 'Pagamento realizado em contrato na condi��o de TESTE  ')
--END