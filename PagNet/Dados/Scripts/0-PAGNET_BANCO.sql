BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'DBO' 
					 AND  TABLE_NAME = 'PAGNET_BANCO')
	BEGIN
			DROP TABLE PAGNET_BANCO
	END
END
BEGIN
    CREATE TABLE PAGNET_BANCO
    (
         CODBANCO   CHAR(3) NOT NULL PRIMARY KEY
        ,NMBANCO    NVARCHAR(200)
        ,POSSUIVAN  CHAR(1)
        ,ATIVO      CHAR(1)
    )

END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('654','BANCO A.J.RENNER S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('246','BANCO ABC BRASIL S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('025','BANCO ALFA S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('641','BANCO ALVORADA S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('213','BANCO ARBI S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('019','BANCO AZTECA DO BRASIL S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('029','BANCO BANERJ S.A.','N','N') 
END
BEGIN
	INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('000','BANCO BANKPAR S.A.','N','N') 
END
BEGIN
    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('740','BANCO BARCLAYS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('107','BANCO BBM S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('031','BANCO BEG S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('739','BANCO BGN S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('096','BANCO BM&F DE SERVI�OS DE LIQUIDA��O E CUST�DIA S.A','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('318','BANCO BMG S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('752','BANCO BNP PARIBAS BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('248','BANCO BOAVISTA INTERATL�NTICO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('218','BANCO BONSUCESSO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('065','BANCO BRACCE S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('036','BANCO BRADESCO BBI S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('204','BANCO BRADESCO CART�ES S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('394','BANCO BRADESCO FINANCIAMENTOS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('237','BANCO BRADESCO S.A.', 'N','S') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('225','BANCO BRASCAN S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M15','BANCO BRJ S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('208','BANCO BTG PACTUAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('044','BANCO BVA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('263','BANCO CACIQUE S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('473','BANCO CAIXA GERAL - BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('412','BANCO CAPITAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('040','BANCO CARGILL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('745','BANCO CITIBANK S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M08','BANCO CITICARD S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('241','BANCO CL�SSICO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M19','BANCO CNH CAPITAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('215','BANCO COMERCIAL E DE INVESTIMENTO SUDAMERIS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('756','BANCO COOPERATIVO DO BRASIL S.A. - BANCOOB','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('748','BANCO COOPERATIVO SICREDI S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('075','BANCO CR2 S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('721','BANCO CREDIBEL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('222','BANCO CREDIT AGRICOLE BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('505','BANCO CREDIT SUISSE (BRASIL) S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('229','BANCO CRUZEIRO DO SUL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('266','BANCO C�DULA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('003','BANCO DA AMAZ�NIA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M21','BANCO DAIMLERCHRYSLER S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('707','BANCO DAYCOVAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('300','BANCO DE LA NACION ARGENTINA','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('495','BANCO DE LA PROVINCIA DE BUENOS AIRES','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('494','BANCO DE LA REPUBLICA ORIENTAL DEL URUGUAY','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M06','BANCO DE LAGE LANDEN BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('024','BANCO DE PERNAMBUCO S.A. - BANDEPE','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('456','BANCO DE TOKYO-MITSUBISHI UFJ BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('214','BANCO DIBENS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('001','BANCO DO BRASIL S.A.', 'N','S') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('047','BANCO DO ESTADO DE SERGIPE S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('037','BANCO DO ESTADO DO PAR� S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('039','BANCO DO ESTADO DO PIAU� S.A. - BEP','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('041','BANCO DO ESTADO DO RIO GRANDE DO SUL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('004','BANCO DO NORDESTE DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('265','BANCO FATOR S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M03','BANCO FIAT S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('224','BANCO FIBRA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('626','BANCO FICSA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M18','BANCO FORD S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('233','BANCO GE CAPITAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('734','BANCO GERDAU S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M07','BANCO GMAC S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('612','BANCO GUANABARA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M22','BANCO HONDA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('063','BANCO IBI S.A. BANCO M�LTIPLO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M11','BANCO IBM S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('604','BANCO INDUSTRIAL DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('320','BANCO INDUSTRIAL E COMERCIAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('653','BANCO INDUSVAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('630','BANCO INTERCAP S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('249','BANCO INVESTCRED UNIBANCO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M09','BANCO ITAUCRED FINANCIAMENTOS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('184','BANCO ITA� BBA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('479','BANCO ITA�BANK S.A','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('376','BANCO J. P. MORGAN S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('074','BANCO J. SAFRA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('217','BANCO JOHN DEERE S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('076','BANCO KDB S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('757','BANCO KEB DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('600','BANCO LUSO BRASILEIRO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('212','BANCO MATONE S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M12','BANCO MAXINVEST S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('389','BANCO MERCANTIL DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('746','BANCO MODAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M10','BANCO MONEO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('738','BANCO MORADA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('066','BANCO MORGAN STANLEY S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('243','BANCO M�XIMA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('045','BANCO OPPORTUNITY S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M17','BANCO OURINVEST S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('623','BANCO PANAMERICANO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('611','BANCO PAULISTA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('613','BANCO PEC�NIA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('643','BANCO PINE S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('724','BANCO PORTO SEGURO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('735','BANCO POTTENCIAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('638','BANCO PROSPER S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M24','BANCO PSA FINANCE BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('747','BANCO RABOBANK INTERNATIONAL BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('356','BANCO REAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('633','BANCO RENDIMENTO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('741','BANCO RIBEIR�O PRETO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M16','BANCO RODOBENS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('072','BANCO RURAL MAIS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('453','BANCO RURAL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('422','BANCO SAFRA S.A.','N','S') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('033','BANCO SANTANDER (BRASIL) S.A.', 'N','S') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('250','BANCO SCHAHIN S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('743','BANCO SEMEAR S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('749','BANCO SIMPLES S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('366','BANCO SOCI�T� G�N�RALE BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('637','BANCO SOFISA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('012','BANCO STANDARD DE INVESTIMENTOS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('464','BANCO SUMITOMO MITSUI BRASILEIRO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M20','BANCO TOYOTA DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M13','BANCO TRICURY S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('634','BANCO TRI�NGULO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M14','BANCO VOLKSWAGEN S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('M23','BANCO VOLVO (BRASIL) S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('655','BANCO VOTORANTIM S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('610','BANCO VR S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('370','BANCO WESTLB DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('021','BANESTES S.A. BANCO DO ESTADO DO ESP�RITO SANTO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('719','BANIF-BANCO INTERNACIONAL DO FUNCHAL (BRASIL)S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('755','BANK OF AMERICA MERRILL LYNCH BANCO M�LTIPLO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('744','BANKBOSTON N.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('073','BB BANCO POPULAR DO BRASIL S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('078','BES INVESTIMENTO DO BRASIL S.A.-BANCO DE INVESTIMENTO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('069','BPN BRASIL BANCO M�LTIPLO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('070','BRB - BANCO DE BRAS�LIA S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('104','CAIXA ECON�MICA FEDERAL', 'N','S') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('477','CITIBANK N.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('487','DEUTSCHE BANK S.A. - BANCO ALEM�O','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('751','DRESDNER BANK BRASIL S.A. - BANCO M�LTIPLO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('064','LDMAN SACHS DO BRASIL BANCO M�LTIPLO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('062','HIPERCARD BANCO M�LTIPLO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('399','HSBC BANK BRASIL S.A. - BANCO M�LTIPLO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('168','HSBC FINANCE (BRASIL) S.A. - BANCO M�LTIPLO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('492','ING BANK N.V.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('652','ITA� UNIBANCO HOLDING S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('341','ITA� UNIBANCO S.A.', 'N','S') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('079','JBS BANCO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('488','JPMORGAN CHASE BANK','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('014','NATIXIS BRASIL S.A. BANCO M�LTIPLO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('753','NBC BANK BRASIL S.A. - BANCO M�LTIPLO','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('254','PARAN� BANCO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('409','UNIBANCO - UNI�O DE BANCOS BRASILEIROS S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('230','UNICARD BANCO M�LTIPLO S.A.','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('084','UNICRED NORTE DO PARAN�','N','N') 

    INSERT INTO PAGNET_BANCO(CODBANCO, NMBANCO, POSSUIVAN, ATIVO) VALUES ('077', 'BANCO INTERMEDIUM', 'N', 'N') 
END

select * from PAGNET_BANCO where ativo = 'S'