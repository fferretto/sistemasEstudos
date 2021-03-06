BEGIN
	IF EXISTS ( SELECT  1
				FROM    sys.objects
				WHERE   object_id = OBJECT_ID(N'PAGNET_PARAMETRO_REL')
						AND type IN ( N'P', N'PC' ) ) 
	BEGIN
		DROP PROCEDURE PAGNET_PARAMETRO_REL
	END
END
BEGIN
	IF EXISTS ( SELECT  1
				FROM    sys.objects
				WHERE   object_id = OBJECT_ID(N'PAGNET_RELATORIO')
						AND type IN ( N'P', N'PC' ) ) 
	BEGIN
		DROP PROCEDURE PAGNET_RELATORIO
	END

END
BEGIN
	CREATE TABLE PAGNET_RELATORIO
	  (
		ID_REL              INT             NOT NULL PRIMARY KEY,
		DESCRICAO           VARCHAR(150)    NOT NULL,
		NOMREL              VARCHAR(150)    NOT NULL,
		TIPREL              VARCHAR(50)     NOT NULL,
		NOMPROC             VARCHAR(50)     NOT NULL,
		EXECUTARVIAJOB      CHAR(1)         NOT NULL	
	  ) 

END
BEGIN

	CREATE TABLE PAGNET_PARAMETRO_REL
	 (
		ID_PAR          INT             NOT NULL PRIMARY KEY,
		ID_REL          INT             NOT NULL FOREIGN KEY REFERENCES PAGNET_RELATORIO(ID_REL),
		DESPAR          NCHAR(50)       NULL,
		NOMPAR          VARCHAR(50)     NULL,
		LABEL           VARCHAR(50)     NULL,
		TIPO            VARCHAR(50)     NULL,
		TAMANHO         INT             NULL,
		_DEFAULT        VARCHAR(50)     NULL,
		REQUERIDO       CHAR(1)         NULL,
		ORDEM_TELA      SMALLINT        NULL,
		ORDEM_PROC      SMALLINT        NULL,
		NOM_FUNCTION    VARCHAR(250)    NULL,
		MASCARA         VARCHAR(30)     NULL,
		TEXTOAJUDA      VARCHAR(100)    NULL,
	) 
END

