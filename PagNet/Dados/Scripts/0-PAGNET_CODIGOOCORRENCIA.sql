BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'dbo' 
					 AND  TABLE_NAME = 'PAGNET_CODIGOOCORRENCIA')
	BEGIN
			DROP TABLE PAGNET_CODIGOOCORRENCIA
	END
END
BEGIN
    CREATE TABLE PAGNET_CODIGOOCORRENCIA
    (
         CODOCORRENCIA   INT NOT NULL PRIMARY KEY
        ,NMOCORRENCIA    CHAR(100)
        ,ATIVO           CHAR(1)
    )
    
END
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(01 , 'ENTRADA DE T?TULO', 'N')   
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(02 , 'BAIXA DE T?TULO', 'S')   
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(04 , 'CONCESS?O DE ABATIMENTO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(05 , 'CANCELAMENTO ABATIMENTO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(06 , 'ALTERA??O DE VENCIMENTO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(07 , 'ALT. N?MERO CONT.BENEFICI?RIO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(08 , 'ALTERA??O DO SEU N?MERO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(09 , 'PROTESTAR', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(CODOCORRENCIA, NMOCORRENCIA, ATIVO)
    VALUES(10 , 'CONCESS?O DE DESCONTO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(CODOCORRENCIA, NMOCORRENCIA, ATIVO)
    VALUES(11 , 'CANCELAMENTO DE DESCONTO', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(18 , 'SUSTAR PROTESTO (Ap?s in?cio do ciclo de protesto)', 'S')    
 
INSERT INTO PAGNET_CODIGOOCORRENCIA(codOcorrencia, nmOcorrencia, Ativo)
    VALUES(98 , 'N?O PROTESTAR (Antes do in?cio do ciclo de protesto)', 'S')
 
