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
--		VALUES('00','Crédito ou Débito Efetivado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('01','Insuficiência de Fundos - Débito Não Efetuado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('02','Crédito ou Débito Cancelado pelo Pagador/Credor ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('03','Débito Autorizado pela Agência - Efetuado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AA','Controle Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AB','Tipo de Operação Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AC','Tipo de Serviço Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AE','Tipo/Número de Inscrição Inválido (gerado na crítica ou para informar rejeição)* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AF','Códi de Convênio Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AG','Agência/Conta Corrente/DV Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AH','Número Seqüencial do Registro no Lote Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AI','Códi de Segmento de Detalhe Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AJ','Tipo de Movimento Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AK','Códi da Câmara de Compensação do Banco do Favorecido/Depositário Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AL','Códi do Banco do Favorecido ou Depositário Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AM','Agência Mantenedora da Conta Corrente do Favorecido Inválida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AN','Conta Corrente/DV do Favorecido Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AO','Nome do Favorecido não Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AP','Data Lançamento Inválida/Vencimento Inválido/Data de Pagamento não permitda. ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AQ','Tipo/Quantidade da Moeda Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AR','Valor do Lançamento Inválido/Divergente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AS','Aviso ao Favorecido - Identificação Inválida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AT','Tipo/Número de Inscrição do Favorecido/Contribuinte Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AU','Logradouro do Favorecido não Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AV','Número do Local do Favorecido não Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AW','Cidade do Favorecido não Informada ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AX','CEP/Complemento do Favorecido Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AY','Sigla do Estado do Favorecido Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('AZ','Códi/Nome do Banco Depositário Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BA','Códi/Nome da Agência Depositário não Informado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BB','Número do Documento Inválido(Seu Número) ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BC','Nosso Número Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BD','Inclusão Efetuada com Sucesso ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BE','Alteração Efetuada com Sucesso ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BF','Exclusão Efetuada com Sucesso ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('BG','Agência/Conta Impedida Legalmente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B1','Bloqueado Pendente de Autorização ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B3','Bloqueado pelo cliente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B4','Bloqueado pela captura de titulo da cobrança ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('B8','Bloqueado pela Validação de Tributos ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CA','Códi de barras - Códi do Banco Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CB','Códi de barras - Códi da Moeda Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CC','Códi de barras - Dígito Verificador Geral Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CD','Códi de barras - Valor do Título Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CE','Códi de barras  - Campo Livre Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CF','Valor do Documento/Principal/menor que o minimo Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CH','Valor do Desconto Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CI','Valor de Mora Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CJ','Valor da Multa Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CK','Valor do IR Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CL','Valor do ISS Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CG','Valor do Abatimento inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CM','Valor do IOF Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CN','Valor de Outras Deduções Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('CO','Valor de Outros Acréscimos Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HA','Lote Não Aceito ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HB','Inscrição da Empresa Inválida para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HC','Convênio com a Empresa Inexistente/Inválido para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HD','Agência/Conta Corrente da Empresa Inexistente/Inválida para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HE','Tipo de Serviço Inválido para o Contrato ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HF','Conta Corrente da Empresa com Saldo Insuficiente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HG','Lote de Serviço fora de Seqüência ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HH','Lote de Serviço Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HI','Arquivo não aceito ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HJ','Tipo de Registro Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HL','Versão de Layout Inválida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('HU','IA Hora de Envio Inválida Pagamento exclusive em Cartório. ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IJ','Competência ou Período de Referencia ou Numero da Parcela invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IL','Codi Pagamento / Receita não numérico ou com zeros ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IM','Município Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IN','Numero Declaração Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IO','Numero Etiqueta invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IP','Numero Notificação invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IQ','Inscrição Estadual invalida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IR','Divida Ativa Invalida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IS','Valor Honorários ou Outros Acréscimos invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IT','Período Apuração invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IU','Valor ou Percentual da Receita invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('IV','Numero Referencia invalida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('SC','Validação parcial ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('TA','Lote não Aceito - Totais do Lote com Diferença ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('XB','Número de Inscrição do Contribuinte Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('XC','Códi do Pagamento ou Competência ou Número de Inscrição Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('XF','Códi do Pagamento ou Competência não Numérico ou igual á zeros ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YA','Título não Encontrado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YB','Identificação Registro Opcional Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YC','Códi Padrão Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YD','Códi de Ocorrência Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YE','Complemento de Ocorrência Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('YF','Alegação já Informada ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZA','Transferencia  Devolvida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZB','Transferencia mesma titularidade não permitida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZC','Códi pagamento Tributo inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZD','Competência Inválida ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZE','Título Bloqueado na base ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZF','Sistema em Contingência – Titulo com valor maior que referência ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZG','Sistema em Contingência – Título vencido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZH','Sistema em contingência -  Título indexado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZI','Beneficiário divergente ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZJ','Limite de pagamentos parciais excedido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZK','Título já liquidado ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZT','Valor outras entidades inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZU','Sistema Origem Inválido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZW','Banco Destino não recebe DOC ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZX','Banco Destino inoperante para DOC ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZY','Códi do Histórico de Credito Invalido ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('ZV','Autorização iniciada no Internet Banking ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z0','Conta com bloqueio* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z1','Conta fechada. É necessário ativar a conta* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z2','Conta com movimento controlado* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z3','Conta cancelada* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z4','Registro inconsistente (Título)* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z5','Apresentação indevida (Título)* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z6','Dados do destinatário inválidos* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z7','Agência ou conta destinatária do crédito inválida* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z8','Divergência na titularidade* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('Z9','Conta destinatária do crédito encerrada* ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C1','COMPROR – Devolvido por outros bancos** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C2','COMPROR – Recusado** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C3','COMPROR – Rejeitado por sistema** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C4','COMPROR – Rejeitado por horário** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C6','COMPROR – Aprovado** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('C7','COMPROR – Compromisso Inválido**')
		
--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('F1','CONFIRMING – Compromisso Liquidado***   ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('F2','CONFIRMING – Compromisso em Neciação*** ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O1','Códi da  OCT invalido ****  ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O2','Descrição do remetente inválida ****  ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O3','Descrição da finalidade inválida ****  ')

--	INSERT INTO PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao)
--		VALUES('O4','Códi Convenio cobrança Inválido **** ')
        
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) 
--     values ('AD', 'Forma de Lançamento Inválida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BH', 'Empresa não pau salário')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BI', 'Falecimento do mutuário ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BJ', 'Empresa não enviou remessa do mutuário ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BK', 'Empresa não enviou remessa no vencimento ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BL', 'Valor da parcela inválida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BM', 'Identificação do contrato inválida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BN', 'Operação de Consignação Incluída com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BO', 'Operação de Consignação Alterada com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BP', 'Operação de Consignação Excluída com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values   
--	('BQ', 'Operação de Consignação Liquidada com Sucesso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('CP', 'Valor do INSS Inválido')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HK', 'Códi Remessa / Retorno Inválido')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HM', 'Mutuário não identificado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HN', 'Tipo do benefício não permite empréstimo ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HO', 'Benefício cessado/suspenso ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HP', 'Benefício possui representante legal ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HQ', 'Benefício é do tipo PA (Pensão alimentícia) ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HR', 'Quantidade de contratos permitida excedida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HS', 'Benefício não pertence ao Banco informado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HT', 'Início do desconto informado já ultrapassado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HV', 'Quantidade de parcela inválida ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HW', 'Margem consignável excedida para o mutuário dentro do prazo do contrato ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HX', 'Empréstimo já cadastrado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HY', 'Empréstimo inexistente ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('HZ', 'Empréstimo já encerrado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H1', 'Arquivo sem trailer')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H2', 'Mutuário sem crédito na competência ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H3', 'Não descontado – outros motivos ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H4', 'Retorno de Crédito não pa Estorno de pagamento quando os dados do favorecido esta incorreto ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H5', 'Cancelamento de empréstimo retroativo ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H6', 'Outros Motivos de Glosa ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H7', 'Margem consignável excedida para o mutuário acima do prazo do contrato ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('H8', 'Mutuário desligado do empregador ‘H9’ = Mutuário afastado por licença ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('IA', 'Primeiro nome do mutuário diferente do primeiro nome do movimento do censo ou diferente da base de Titular do Benefício ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5A', 'Agendado sob lista de debito Pagamento agendado que faz parte de uma lista com um número para autorização. ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5B', 'Pagamento não autoriza sob lista de debito Pagamento da lista não foi autorizado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5C', 'Lista com mais de uma modalidade Lista de pagamento não permite mais de uma modalidade ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5D', 'Lista com mais de uma data de pagamento Lista de pagamento não permite mais de uma data de pagamento ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5E', 'Número de lista duplicado Número da lista enviado pelo cliente já foi utilizado ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5F', 'Lista de debito vencida e não autorizada Pagamentos que pertence a uma determinada lista estão vencidos e não foram autorizados  ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5I', 'Ordem de Pagamento emitida  Pagamento realizado ao favorecido nesta data ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5M', 'Número de lista de debito invalida Número de lista inválido (deve ser numérico) ')
	 
--	 insert into PAGNET_OCORRENCIARETPAG (codOcorrenciaRetPag, Descricao) values  
--	('5T', 'Pagamento realizado em contrato na condição de TESTE  ')
--END