--DROP TABLE RELATORIO_RESULTADO
--DROP TABLE RELATORIO_PARAM_UTILIZADO
--DROP TABLE RELATORIO_STATUS

CREATE TABLE RELATORIO_STATUS
(
    COD_STATUS_REL	VARCHAR(400)    NOT NULL PRIMARY KEY,
    ID_REL	        INT             NOT NULL FOREIGN KEY REFERENCES RELATORIO(ID_REL),
    CHAVEACESSO     VARCHAR(400)    NOT NULL,
    STATUS	        VARCHAR(150)    NOT NULL,
    TIPORETORNO	    INT             NOT NULL,
    DATEMISSAO	    DATETIME        NOT NULL,
    ERRO	        INT,
    MSG_ERRO	    VARCHAR(600)
)
GO
CREATE TABLE RELATORIO_PARAM_UTILIZADO
(
    COD_PARAM_UTILIZADO	INT                 NOT NULL PRIMARY KEY      IDENTITY(1,1),
    COD_STATUS_REL	    VARCHAR(400)        NOT NULL FOREIGN KEY REFERENCES RELATORIO_STATUS(COD_STATUS_REL),
    NOMPAR	            NVARCHAR(50)        NOT NULL,
    LABEL               NVARCHAR(50)        NOT NULL,
    CONTEUDO	        VARCHAR(8000)       NOT NULL
)
GO
CREATE TABLE RELATORIO_RESULTADO
(
    COD_RESULTADO	INT             NOT NULL    PRIMARY KEY IDENTITY(1,1),
    COD_STATUS_REL	VARCHAR(400)    NOT NULL    FOREIGN KEY REFERENCES RELATORIO_STATUS(COD_STATUS_REL),
    LINHAIMP	    VARCHAR(8000)   NOT NULL,
    TIP	            CHAR(1)         NOT NULL
)
GO
ALTER TABLE PARAMETRO ADD MASCARA NVARCHAR(100)
GO
ALTER TABLE PARAMETRO ADD TEXTOAJUDA NVARCHAR(200)
GO
ALTER TABLE RELATORIO ADD EXECUTARVIAJOB CHAR(1)
GO
UPDATE RELATORIO
SET EXECUTARVIAJOB = 'N'
GO
