BEGIN
	IF EXISTS (SELECT 1 
					 FROM INFORMATION_SCHEMA.TABLES 
					 WHERE TABLE_SCHEMA = 'DBO' 
					 AND  TABLE_NAME = 'PAGNET_USUARIO')
	BEGIN
			delete from PAGNET_USUARIO
	END
	ELSE
	BEGIN
		CREATE TABLE PAGNET_USUARIO
		(
			 CODUSUARIO         INT             NOT NULL PRIMARY KEY
			,NMUSUARIO          VARCHAR(100)        NULL
			,LOGIN              VARCHAR(100)    NOT NULL
			,SENHA              VARCHAR(200)    NOT NULL
			,CPF                VARCHAR(14)
			,CODEMPRESA         INT
			,EMAIL              CHAR(200)      
			,ADMINISTRADOR      CHAR(1)         NOT NULL DEFAULT 'S'
			,VISIVEL            CHAR(1)         NOT NULL DEFAULT 'S'
			,ATIVO              CHAR(1)         NOT NULL DEFAULT 'S'
		)
	END
END
BEGIN

	INSERT INTO CONCENTRADOR..PAGNET_USUARIO
	SELECT 'Administrador Telenet'
		  ,'tln@008CARD'
		  ,'b096200eeecb770a1962e743b3ddd0a35e2997405e8a6169a1c1d2f4b18e0b65'
		  ,'41922550000145'
		  ,'suporte@tln.com.br'
		  ,16
		  ,'S'
		  ,'S'
		  ,'N'
		  ,1

END
BEGIN

	Insert into PAGNET_USUARIO
	 SELECT  PG.CODUSUARIO    
			,PG.NMUSUARIO     
			,PG.LOGIN         
			,PG.SENHA         
			,PG.CPF           
			,PG.CODEMPRESA    
			,PG.EMAIL           
			,PG.ADMINISTRADOR 
			,PG.VISIVEL       
			,PG.ATIVO         
	   from CONCENTRADOR..PAGNET_USUARIO PG 
	  where PG.CODOPE = 16
  
END
