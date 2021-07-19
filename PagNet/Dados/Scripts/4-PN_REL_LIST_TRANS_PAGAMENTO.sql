
BEGIN
	IF EXISTS (SELECT 1 FROM PAGNET_RELATORIO WHERE NOMPROC = 'PN_REL_LIST_TRANS_PAGAMENTO')
	BEGIN
		DECLARE @ID_REL INT

		SELECT @ID_REL = ID_REL FROM PAGNET_RELATORIO WHERE NOMPROC = 'PN_REL_LIST_TRANS_PAGAMENTO'

		DELETE FROM PAGNET_PARAMETRO_REL WHERE ID_REL = @ID_REL
		DELETE FROM PAGNET_RELATORIO  WHERE ID_REL = @ID_REL
	END

END

IF NOT EXISTS (SELECT 1 FROM PAGNET_RELATORIO WHERE NOMPROC = 'PN_REL_LIST_TRANS_PAGAMENTO')
BEGIN
    DECLARE @IDREL_1 INT   
    SET @IDREL_1 = (SELECT MAX(ISNULL(ID_REL, 0)) + 1 FROM PAGNET_RELATORIO)

    IF @IDREL_1 IS NULL
    BEGIN 
        SET @IDREL_1 = 1
    END

    INSERT INTO PAGNET_RELATORIO
    SELECT 
            @IDREL_1                                AS ID_REL
           ,'LISTAGEM DOS PAGAMENTOS REALIZADOS'    AS DESCRICAO
           ,'Pagamentos Realizados'                 AS NOMREL
           ,'PagNet'                                AS TIPREL
           ,'PN_REL_LIST_TRANS_PAGAMENTO'           AS NOMPROC
           ,'S'                                     AS EXECUTARVIAJOB


        
    DECLARE @IDREL INT   
    SET @IDREL =  (SELECT ID_REL FROM PAGNET_RELATORIO WHERE NOMPROC = 'PN_REL_LIST_TRANS_PAGAMENTO')

    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END  AS ID_PAR
            ,@IDREL                                  AS ID_REL
            ,'Data Inicio'                           AS DESPAR
            ,'@DATA_INI'                             AS NOMPAR
            ,'DateEdit'                              AS LABEL
            ,'String'                                AS TIPO
            ,'10'                                    AS TAMANHO
            ,NULL                                    AS _DEFAULT
            ,NULL                                    AS REQUERIDO
            ,1                                       AS ORDEM_TELA
            ,1                                       AS ORDEM_PROC
            ,null                                    AS NOM_FUNCTION
            ,'99/99/9999'                            AS MASCARA
            ,'Filtro de inicio da data de pagamento' AS TEXTOAJUDA

    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END  AS ID_PAR
            ,@IDREL                                  AS ID_REL
            ,'Data Fim'                              AS DESPAR
            ,'@DATA_FIM'                             AS NOMPAR
            ,'DateEdit'                              AS LABEL
            ,'String'                                AS TIPO
            ,'10'                                    AS TAMANHO
            ,NULL                                    AS _DEFAULT
            ,NULL                                    AS REQUERIDO
            ,2                                       AS ORDEM_TELA
            ,2                                       AS ORDEM_PROC
            ,null                                    AS NOM_FUNCTION
            ,'99/99/9999'                            AS MASCARA
            ,'Filtro de termino da data de pagamento' AS TEXTOAJUDA
        

    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END     AS ID_PAR
            ,@IDREL                                     AS ID_REL
            ,'Código do Banco'                          AS DESPAR
            ,'@CODBANCO'                                AS NOMPAR
            ,'TextBox'                                  AS LABEL
            ,'String'                                   AS TIPO
            ,'3'                                        AS TAMANHO
            ,NULL                                       AS _DEFAULT
            ,NULL                                       AS REQUERIDO
            ,3                                          AS ORDEM_TELA
            ,3                                          AS ORDEM_PROC
            ,NULL                                       AS NOM_FUNCTION
            ,''                                         AS MASCARA
            ,'Código do banco que efetuou o pagamento'  AS TEXTOAJUDA
                   
    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END     AS ID_PAR
            ,@IDREL                                     AS ID_REL
            ,'Código do Favorecido'                     AS DESPAR
            ,'@CODFAVORECIDO'                           AS NOMPAR
            ,'TextBox'                                  AS LABEL
            ,'String'                                   AS TIPO
            ,'9'                                        AS TAMANHO
            ,NULL                                       AS _DEFAULT
            ,NULL                                       AS REQUERIDO
            ,4                                          AS ORDEM_TELA
            ,4                                          AS ORDEM_PROC
            ,NULL                                       AS NOM_FUNCTION
            ,''                                         AS MASCARA
            ,'Código do favorecido que recebeu o pagamento' AS TEXTOAJUDA
            
    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END     AS ID_PAR
            ,@IDREL                                     AS ID_REL
            ,'Empresa'                                  AS DESPAR
            ,'@CODEMPRESA'                              AS NOMPAR
            ,'DropDownList'                             AS LABEL
            ,'int'                                      AS TIPO
            ,'10'                                       AS TAMANHO
            ,NULL                                       AS _DEFAULT
            ,NULL                                       AS REQUERIDO
            ,5                                          AS ORDEM_TELA
            ,5                                          AS ORDEM_PROC
            ,'/Generico/CadastrosDiversos/GetEmpresa/' AS NOM_FUNCTION
            ,''                                         AS MASCARA
            ,'Filtrar relatorio por Empresa'            AS TEXTOAJUDA

    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END     AS ID_PAR
            ,@IDREL                                     AS ID_REL
            ,'UF'                                       AS DESPAR
            ,'@UF'                                      AS NOMPAR
            ,'TextBox'                                  AS LABEL
            ,'String'                                   AS TIPO
            ,'2'                                        AS TAMANHO
            ,NULL                                       AS _DEFAULT
            ,NULL                                       AS REQUERIDO
            ,6                                          AS ORDEM_TELA
            ,6                                          AS ORDEM_PROC
            ,NULL                                       AS NOM_FUNCTION
            ,''                                         AS MASCARA
            ,'Informe o Estado com 2 Caracteres' AS TEXTOAJUDA

    INSERT INTO PAGNET_PARAMETRO_REL
    SELECT
            CASE WHEN (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) IS NULL THEN 1 ELSE (SELECT MAX(ID_PAR) + 1 FROM PAGNET_PARAMETRO_REL) END     AS ID_PAR
            ,@IDREL                                     AS ID_REL
            ,'Cidade'                                   AS DESPAR
            ,'@CIDADE'                                  AS NOMPAR
            ,'TextBox'                                  AS LABEL
            ,'String'                                   AS TIPO
            ,'100'                                      AS TAMANHO
            ,NULL                                       AS _DEFAULT
            ,NULL                                       AS REQUERIDO
            ,7                                          AS ORDEM_TELA
            ,7                                          AS ORDEM_PROC
            ,NULL                                       AS NOM_FUNCTION
            ,''                                         AS MASCARA
            ,'Informe o Município'                      AS TEXTOAJUDA

END

