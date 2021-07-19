  
/*----------------------------------------------------------------------------*/        
/* REL_HISTORICO_TRANSACOES                                 VERSAO 5          */             
/* LISTA TRANSACOES DE UM DETERMINADO CPF                                     */        
/* é importante inserir CNPJ do cliente e dos credenciados                    */  
/* PARAMETROS:                                                                */          
/*                                                                            */        
/* @SISTEMA ->  Sistema 0 = PJ (Pós-pago)                                     */          
/*                      1 = VA (Pré-pago) --> NAO UTILIZADO                   */    
/*                                                                            */  
/* @CODCLI                                                                    */  
/* @CPF     -> CPF para a listagem                                            */        
/* @DATAINI -> AAAAMMDD                                                       */           
/* @DATAFIM -> AAAAMMDD                                                       */          
/* @FORMATO -> Tipo do Relatório 0 = Texto                                    */        
/*                               1 = Excel                                    */  
/* @TIPO    -> 0 Mostra Nome Fantasia do Credenciado                          */  
/*             1 Mostra Razao Social                                          */           
/*                                                                            */        
/*  UTILIZA A FUNCAO RESUME_DESCRICAO                                         */    
/* REV03 JUNCAO TIPTRANS                                                      */        
/* REV 5 Corrigido leitura do saldo para pré-pago                             */  
/*----------------------------------------------------------------------------*/         
ALTER PROCEDURE [dbo].[REL_HISTORICO_TRANSACOES_COM_SALDO] 
                            @SISTEMA            INT = 0, 
                            @CODCLI             INT = 0,   
                            @CPF                CHAR(11) = NULL, 
                            @DATAINI            CHAR(08), 
                            @DATAFIM            CHAR(08),  
                            @TIPO               INT = 0,  
                            @FORMATO            SMALLINT = NULL,   
                            @COD_STATUS_REL     VARCHAR(100) ,  
                            @ERRO               INT             OUTPUT ,    
                            @MSG_ERRO           VARCHAR(800)    OUTPUT  
        
AS        
  
  
BEGIN  
 SET NOCOUNT ON  
  
    /*------------------------------------------------------------*/  
 /* Retire os comentários abaixo para fazer testes */  
 --DECLARE @CODCLI   int   
 --DECLARE @CPF      char(11)  
 --DECLARE @SISTEMA   INT   
 --DECLARE @DATAINI   CHAR(08)  
 --DECLARE @DATAFIM   CHAR(08)  
 --DECLARE @FORMATO  INT  
 --DECLARE @TIPO     INT  
  
 --SET @SISTEMA  = 0  
 --SET @CODCLI   = 10000   
 --SET @CPF      = '00893888958'   
 --SET @DATAINI  = '20200901'   
 --SET @DATAFIM  = '20200915'   
 --SET @FORMATO  = 0  
 --SET @TIPO     = 0   
 /*---------------------------------------------*/      
    SET @ERRO = 0  
    SET @MSG_ERRO  = ''    
    DECLARE @NOMUSU       VARCHAR(50)  
    DECLARE @NOMCLI       VARCHAR(50)  
    DECLARE @OPERADORA    VARCHAR(30)  
  
    DECLARE @BANCO_AUT    varchar(100)  
    DECLARE @QUERY        varchar(1000)  
    DECLARE @CNPJCLI      CHAR(14)  
    DECLARE @CNPJOPER     CHAR(14)  
    DECLARE @BRANCOS      VARCHAR(118)  
    DECLARE @LINIMP       VARCHAR(118)  
    DECLARE @CABEC1       VARCHAR(118)   
    DECLARE @CABEC2       VARCHAR(118)  
    DECLARE @CABEC3       VARCHAR(118)  
    DECLARE @CABEC4       VARCHAR(118)  
    DECLARE @CABEC5       VARCHAR(118)  
    DECLARE @CABEC6       VARCHAR(118)  
    DECLARE @TRACO        VARCHAR(118)  
    DECLARE @CONTADOR     INT   
    DECLARE @TOTAL        NUMERIC(11,2)  
    DECLARE @QT_ESPACO1   INT  
    DECLARE @QT_ESPACO2   INT  
    DECLARE @RZ_OPERADORA CHAR(50)  
    DECLARE @SALDO        NUMERIC(11,2)  
    DECLARE @DATASALDO    DATETIME  
    DECLARE @TABSALDO TABLE ( SALDOPOS NUMERIC(11,2), SALDOPRE NUMERIC(11,2), VALCOMP NUMERIC(11,2), GASTOATUAL NUMERIC(11,2) )  
  
 INSERT @TABSALDO  
    EXEC PROC_CONSULTA_SALDO  @SISTEMA, @CODCLI, @CPF  
  
 IF @SISTEMA = 0  
   SELECT @SALDO = SALDOPOS, @DATASALDO = GETDATE() FROM @TABSALDO  
 ELSE  
   SELECT @SALDO = SALDOPRE, @DATASALDO = GETDATE() FROM @TABSALDO    
  
    
 SELECT @OPERADORA = VAL FROM CONFIG_JOBS WHERE ID0 = 'OPERADORA'  
 SELECT @RZ_OPERADORA = VAL FROM PARAM WHERE ID0 = 'RAZSC_OPERADORA'  
  
 SET    @OPERADORA = LTRIM(RTRIM(@OPERADORA)) + REPLICATE(' ', 30-LEN(LTRIM(RTRIM(@OPERADORA))))  
 SET    @CNPJOPER  = ( SELECT VAL FROM PARAM WHERE ID0 = 'CNPJOPERADORA' )  
 SET    @BANCO_AUT = DBO.BancoAutorizador()  
  
 IF @SISTEMA = 0   
 BEGIN  
  SELECT @NOMCLI = NOMCLI, @CNPJCLI = CGC FROM CLIENTE WHERE CODCLI = @CODCLI   
  SELECT @NOMUSU = NOMUSU   
  FROM USUARIO   
  WHERE CODCLI = @CODCLI AND CPF = @CPF AND NUMDEP = 0   
 END  
  
 IF @SISTEMA <> 0  
 BEGIN  
  SELECT @NOMCLI = NOMCLI, @CNPJCLI = CGC FROM CLIENTEVA WHERE CODCLI = @CODCLI   
  SELECT @NOMUSU = NOMUSU   
  FROM USUARIOVA   
  WHERE CODCLI = @CODCLI AND CPF = @CPF AND NUMDEP = 0   
 END  
  
    /* CORRECAO PARA NAO FICAR APARECENDO NULL NO MEIO DA LISTAGEM QUANDO NAO ENCONTRA O NOME DO USUARIO */  
 SET @NOMCLI       = ISNULL(@NOMCLI, 'NAO LOCALIZADO')    
    SET @NOMUSU       = ISNULL(@NOMUSU, 'NAO LOCALIZADO')  
    SET @RZ_OPERADORA = ISNULL(@RZ_OPERADORA, 'NAO LOCALIZADO' )  
      
 CREATE TABLE #TRANSABT (  
      CODCLI    int           ,   
      CODCRT    CHAR(06)      ,   
      TD        CHAR(1)       ,  
      CPF       CHAR(11)      ,   
      DATTRA    DATETIME      ,   
      NSUAUT    int           ,   
      NSUHOS    INT           ,  
      VALTRA    numeric(15,2) ,   
      TIPTRA    int           ,   
      DESCRICAO CHAR(20)      ,  
      PARC      CHAR(05)      ,  
      CODCRE    int           ,   
      CNPJ_CRE  CHAR(14)      ,  
      NOMCRE    CHAR(50)      ,  
      ID        INT  IDENTITY     )  
  
  
 SELECT @QUERY =   
 'INSERT INTO #TRANSABT '                                                           + CHAR(13) +   
 'SELECT  T.CODCLI, '                                                               + CHAR(13) +   
 'SUBSTRING(dbo.MASCARACARTAO(T.CODCRT, 12), 7,6), '                                + CHAR(13) +   
 'CASE T.NUMDEP WHEN 0 THEN ''T'' ELSE ''D'' END AS TD,'                            + CHAR(13) +   
 'T.CPF,'                                                                           + CHAR(13) +  
 'T.DATTRA, '                                                                       + CHAR(13) +   
 'T.NSUAUT, T.NSUHOS, T.VALTRA, '                                                   + CHAR(13) +   
 'T.TIPTRA, '                                                                       + CHAR(13) +   
 'dbo.RESUME_DESCRICAO(T2.DESTIPTRA) AS DESCRICAO, '                                + CHAR(13) +   
    'CASE WHEN T.TIPTRA < 80000 '                + CHAR(13) +   
    'THEN STR(ISNULL(ASCII(SUBSTRING(T.DAD, 30,1))-32,1),2) + ''/'' + REPLACE(STR(ISNULL(ASCII(SUBSTRING(T.DAD, 31,1))-32,1),2),'' '','''') ELSE SPACE(5) END AS PARC,' + CHAR(13) +  
 'T.CODCRE, ISNULL(C.CGC, SPACE(14)) , '                                             + CHAR(13) +   
 CASE WHEN @TIPO = 0   
      THEN 'ISNULL(SUBSTRING(C.NOMFAN,1,40), SPACE(1)) '   
      ELSE 'ISNULL(SUBSTRING(C.RAZSOC,1,40), SPACE(1)) '   
 END   + ' AS NOMCRE '                                                               + CHAR(13) +   
 'FROM ' + CASE WHEN @SISTEMA = 0 THEN 'TRANSACAO T ' ELSE 'TRANSACVA T ' END        + CHAR(13) +   
 'LEFT JOIN CREDENCIADO C ON C.CODCRE = T.CODCRE '                                   + CHAR(13) +   
 'LEFT JOIN TIPTRANS T2 ON (T.TIPTRA = T2.TIPTRA) '                                  + CHAR(13) +  
 'WHERE T.CPF = ' + CHAR(39) + @CPF + CHAR(39)                                       + CHAR(13) +   
 'AND T.CODCLI = ' + CAST(@CODCLI AS VARCHAR)                                        + CHAR(13) +   
 'and T.CODRTA in (''V'', ''A'', ''P'') '                                            + CHAR(13) +  
 'AND CONVERT(CHAR(08), T.DATTRA,112) >= ' + CHAR(39) + @DATAINI + CHAR(39)          + CHAR(13) +   
 'AND CONVERT(CHAR(08), T.DATTRA,112) <= ' + CHAR(39) + @DATAFIM + CHAR(39)          + CHAR(13) +   
 --'AND T.TIPTRA NOT IN (999012) ' + CHAR(13) + /*exclui trans.referencia de saldo ou alteracao de premio */   
 'AND (T.TIPTRA < 80000 OR T.TIPTRA > 89999) ' + --Para excluir as consultas  
 'AND T.TIPTRA NOT IN (999012) ' +  
 --'AND (T.TIPTRA < 80000 OR T.TIPTRA = 999010) ' +  
 'order by T.DATTRA'  
  
 EXEC(@QUERY)  
  
 --AUTORIZADOR  
 SELECT @QUERY =   
 'INSERT INTO #TRANSABT '                                                            + CHAR(13) +   
 'SELECT T.CODCLI, '                                                                 + CHAR(13) +   
 'SUBSTRING(dbo.MASCARACARTAO(T.CODCRT, 12), 7,6), '                                 + CHAR(13) +   
 'CASE T.NUMDEP WHEN 0 THEN ''T'' ELSE ''D'' END AS TD,'                             + CHAR(13) +   
 'T.CPF, '                                                                           + CHAR(13) +   
 'T.DATTRA,'                                                                         + CHAR(13) +   
 'T.NSUAUT, T.NSUHOS, T.VALTRA, '                                                    + CHAR(13) +   
 'T.TIPTRA, '                                                                        + CHAR(13) +   
 'dbo.RESUME_DESCRICAO(T2.DESTIPTRA) AS DESCRICAO, '                                 + CHAR(13) +   
 'STR(ISNULL(ASCII(SUBSTRING(T.DAD, 30,1))-32,1),2) + ''/'' + REPLACE(STR(ISNULL(ASCII(SUBSTRING(T.DAD, 31,1))-32,1),2),'' '','''') AS PARC,' + CHAR(13) +  
  'CAST(T.CODPS AS INT) AS CODCRE, '                                                  + CHAR(13) +   
  'ISNULL(C.CGC,SPACE(14)), '                                                         + CHAR(13) +   
 CASE WHEN @TIPO = 0   
      THEN 'ISNULL(C.NOMFAN, SPACE(1)) '   
      ELSE 'ISNULL(C.RAZSOC, SPACE(1)) '   
 END   + ' AS NOMCRE '                                                               + CHAR(13) +   
  
 'FROM ' + @BANCO_AUT +   
 CASE WHEN @SISTEMA = 0 THEN '.CTTRANS  T ' ELSE '.CTTRANSVA  T '  END               + CHAR(13) +            
 'LEFT JOIN CREDENCIADO C ON C.CODCRE = T.CODPS '                                    + CHAR(13) +   
 'LEFT JOIN TIPTRANS T2 ON (T.TIPTRA = T2.TIPTRA) '                                  + CHAR(13) +  
 'WHERE T.CPF = ' + CHAR(39) +  @CPF + CHAR(39)                                      + CHAR(13) +        
 'AND T.PROCESSADA = ''N'' '                                                         + CHAR(13) +   
 'and T.CODCLI = ' + CAST(@CODCLI AS VARCHAR)                                        + CHAR(13) +   
 'and T.CODRTA IN (''V'', ''A'', ''P'') '                                            + CHAR(13) +  
 'AND CONVERT(CHAR(08), T.DATTRA,112) >= ' + CHAR(39) + @DATAINI + CHAR(39)          + CHAR(13) +   
 'AND CONVERT(CHAR(08), T.DATTRA,112) <= ' + CHAR(39) + @DATAFIM + CHAR(39)          + CHAR(13) +   
 'ORDER BY T.DATTRA '   
 EXEC(@QUERY)  
  
    IF @FORMATO = 0    
 BEGIN  /* formata o relatório */  
  CREATE TABLE #LINREL ( LINHAIMP  varchar(118),        /* tabela temporaria que vai receber o relatório */  
          TIP       char(1) )  
  
  /* PARA CENTRALIZAR O CABECALHO COM O NOME DA OPERADORA E O TEXTO SISTEMA NETCARD */  
  SET @QT_ESPACO1 = ((118-LEN(LTRIM(RTRIM(@OPERADORA))))/2)  
  SET @QT_ESPACO2 = ((118-(@QT_ESPACO1 + LEN(@OPERADORA)))/4)+2       
  
  SET @LINIMP   = ''       
  SET @CABEC1   = REPLICATE(' ', @QT_ESPACO1 ) + @OPERADORA +    
         SPACE(@QT_ESPACO2) + 'SISTEMA NETCARD ' + CASE @SISTEMA WHEN 0 THEN 'PJ' ELSE  'PP' END   
  
  SET @CABEC2   = REPLICATE(' ' , 25) +    
                  '  HISTORICO DAS TRANSACOES NO PERIODO DE ' +  
      SUBSTRING(@DATAINI, 7,2) + '/' + SUBSTRING(@DATAINI, 5, 2) + '/' + SUBSTRING(@DATAINI, 1, 4)  +  
      ' ATE ' +  
      SUBSTRING(@DATAFIM, 7,2) + '/' + SUBSTRING(@DATAFIM, 5, 2) + '/' + SUBSTRING(@DATAFIM, 1, 4)    
    
  SET @TRACO    = REPLICATE(CHAR(151),118)    
                                           
        SET @CABEC4   = '  DATA   HORA         CARTAO     NSU     AUT   DESCRICAO                    VALOR  ESTABELECIMENTO '  
        SET @CABEC5   = ''  
        SET @CABEC6   = ''   
        SET @BRANCOS  = REPLICATE(' ',57)  
        SET @CONTADOR = ( SELECT COUNT(*) FROM #TRANSABT)  
  
  /* IMPRIME O CABECALHO DA PAGINA */  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @CABEC1, 0  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @CABEC2, 0  
        INSERT #LINREL(LINHAIMP, TIP) SELECT @CABEC6, 0  
  /*-------------------------------*/  
  
        SET @LINIMP = '  OPERADORA- ' + @RZ_OPERADORA  +  
                      SPACE(8)  +   
                      'CNPJ '   +   
                      ISNULL(@CNPJOPER , SPACE(14) )  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
  
        SET @LINIMP = '  CLIENTE  - ' +   
                      STR(@CODCLI,5) + ' - ' +   
                      LTRIM(RTRIM(@NOMCLI)) + REPLICATE(' ', 50-LEN(RTRIM(LTRIM(@NOMCLI)))) +   
                      'CNPJ ' + ISNULL(@CNPJCLI, SPACE(14))  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
  
  /*  
  SELECT @NOMUSU, @CPF  
  SELECT '  USUARIO  - ' +  
                      @NOMUSU + SPACE(18) + 'CPF  ' + @CPF   
  SELECT '  USUARIO  - ' +  
                      LTRIM(RTRIM(@NOMUSU)) + REPLICATE(' ', 40-LEN(RTRIM(LTRIM(@NOMUSU)))) +  
                      SPACE(18) + 'CPF  ' + @CPF   
  */  
  
  /*SET @LINIMP = '  USUARIO  - ' +   
                      LTRIM(RTRIM(@NOMUSU)) + REPLICATE(' ', 40-LEN(RTRIM(LTRIM(@NOMUSU)))) +  
                      SPACE(18) + 'CPF  ' + @CPF         
  */  
  
  INSERT #LINREL  
  SELECT '  USUARIO  - ' +  
               @NOMUSU +   
      SPACE(18) +   
      'CPF  ' +   
      @CPF  ,  
      2  
  
        INSERT #LINREL   
  SELECT '  SALDO ATUAL ' +   
      REPLACE( STR( @SALDO, 11, 2), '.',',') +   
      SPACE(03) +  
      ' EM ' +   
      CONVERT(CHAR(10), @DATASALDO, 103) +  
      SPACE(01) +  
      CONVERT(CHAR(08), @DATASALDO, 114), 2   
        
  --SET @LINIMP = '  USUARIO  - ' +  
  --                    @NOMUSU + SPACE(18) + 'CPF  ' + @CPF   
  -- INSERT #LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @CABEC6, 2         
  INSERT #LINREL(LINHAIMP, TIP) SELECT @CABEC4, 2  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @TRACO,  1  
  
        IF @CONTADOR <> 0  /* INSERE AS LINHAS FORMATADAS EM LINREL */  
        BEGIN  
   INSERT #LINREL    
   SELECT CONVERT(CHAR(08), DATTRA, 3) + ' ' + CONVERT(CHAR(08), DATTRA, 114)  +   
          --'   ' +  
          '  ' +  TD +   
          '  ' +  
          CODCRT +   
          '  ' +   
       STR(NSUAUT,7) + ' ' + STR(NSUHOS, 7) +   
       '  ' +   
          SUBSTRING(DESCRICAO,1,17) +   
          '  ' +   
          ISNULL(PARC,SPACE(5)) +   
          '  ' +  
       REPLACE(STR(VALTRA, 08, 2), '.', ',') +  
       '  ' +   
       SUBSTRING(NOMCRE,1,35), 2  
   FROM #TRANSABT      
   ORDER BY DATTRA  
        END                      
  
        /* CALCULA E INSERE O TOTAL */  
        SET @TOTAL = (SELECT ISNULL(SUM(VALTRA),0) FROM #TRANSABT WHERE ((TIPTRA < 80000) OR (TIPTRA = 999011) OR (TIPTRA >= 999100 AND TIPTRA <= 999199)))  
        SET @LINIMP = REPLICATE(' ', 37)       +   
                      'TOTAL         '         +  
                      SPACE(19)                +   
                      REPLACE(STR(@TOTAL,11,2),'.',',')   
        INSERT #LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
  
  INSERT #LINREL(LINHAIMP, TIP) SELECT @BRANCOS, 2 
  
    INSERT INTO RELATORIO_RESULTADO 
    SELECT @COD_STATUS_REL, LINHAIMP, TIP 
      FROM #LINREL    
  
       DROP TABLE #LINREL   
 END  
  
    IF @FORMATO = 1  /* SAIDA PARA EXCEL  */  
    BEGIN  
        INSERT INTO RELATORIO_RESULTADO  
        SELECT @COD_STATUS_REL, 'CLIENTE;CNPJ_CLIENTE;USUARIO;CPF;DATA_HORA;CARTAO;AUT;NSU;DESCRICAO;VALOR;CREDENCIADO', ''  
        
        INSERT INTO RELATORIO_RESULTADO  
        SELECT @COD_STATUS_REL,
                @NOMCLI  + ';' +  
                @CNPJCLI  + ';' + 
                @NOMUSU  + ';' +
                @CPF    + ';' +
                CONVERT(CHAR(08), DATTRA, 3) + ' ' + CONVERT(CHAR(08), DATTRA, 114)  + ';' + 
                CODCRT     + ';' +
                STR(NSUAUT,7)   + ';' +   
                STR(NSUHOS, 7)  + ';' + 
                SUBSTRING(DESCRICAO,1,17)  + ISNULL(PARC,SPACE(5))  + ';' +
                REPLACE(STR(VALTRA, 08, 2), '.', ',')  + ';' + 
                NOMCRE   + ';', ''
          FROM #TRANSABT      
       ORDER BY DATTRA  
    END  
  
END  
  
DROP TABLE #TRANSABT  
  
SET NOCOUNT OFF  
  
    update RELATORIO 
  SET EXECUTARVIAJOB = 'S'
  WHERE NOMPROC = 'REL_HISTORICO_TRANSACOES_COM_SALDO'
  
/* PROCEDIMENTO PARA CRIAR O RELATORIO */  
/*  
DECLARE @ID_REL INT  
DECLARE @ID_PAR INT  
DECLARE @ID_DET INT  
  
SET @ID_REL = 0   
SET @ID_PAR = 0   
  
select @ID_REL = ID_REL  from relatorio where NOMPROC = 'REL_HISTORICO_TRANSACOES_COM_SALDO'  
DELETE FROM DETALHES_PARAMETROS WHERE ID_PARAMETRO IN ( SELECT ID_PAR FROM PARAMETRO WHERE ID_REL = @ID_REL)  
DELETE FROM PARAMETRO WHERE ID_REL = @ID_REL  
DELETE FROM RELATORIO WHERE ID_REL = @ID_REL  
  
SET @ID_REL = ( SELECT MAX(ID_REL) FROM RELATORIO) +1  
SET @ID_PAR = ( SELECT MAX(ID_PAR)  FROM PARAMETRO)+1  
SET @ID_DET = ( SELECT MAX(ID)     FROM DETALHES_PARAMETROS)   
  
INSERT RELATORIO VALUES ( @ID_REL, 'Historico das Transacoes por CPF', 'Historico Transacoes por CPF c/Saldo', 'Transações', 'REL_HISTORICO_TRANSACOES_COM_SALDO', 'N', NULL, '*', 'S', 'CLIENTE', '*', 'N', NULL)  
  
INSERT PARAMETRO VALUES( @ID_PAR  , @ID_REL,'TIPO CARTAO'        ,'@SISTEMA'  ,'DropDownList','Byte'  ,     1,    0, 'N' ,'S', 1, 0, NULL )  
INSERT PARAMETRO VALUES( @ID_PAR+1, @ID_REL,'CLIENTE'            ,'@CODCLI'   ,'TextBox'  ,'String',     5, NULL, 'S' ,'S', 2, 1, NULL )   
INSERT PARAMETRO VALUES( @ID_PAR+2, @ID_REL,'CPF'                ,'@CPF'   ,'TextBox'  ,'String',  11, NULL, 'N' ,'N', 3, 2, NULL )  
INSERT PARAMETRO VALUES( @ID_PAR+3, @ID_REL,'DATA INICIO'        ,'@DATAINI'  ,'DateEdit'  ,'String',  10, NULL, 'N' ,'N', 4, 3, NULL )  
INSERT PARAMETRO VALUES( @ID_PAR+4, @ID_REL,'DATA FIM'           ,'@DATAFIM'  ,'DateEdit'  ,'String',     10, NULL, 'N' ,'N', 5, 4, NULL )  
INSERT PARAMETRO VALUES( @ID_PAR+5, @ID_REL,'DADOS CREDENCIADO'  ,'@TIPO'   ,'DropDownList','Byte'  ,      1,    0, 'S' ,'S', 6, 5, NULL )  
INSERT PARAMETRO VALUES( @ID_PAR+6, @ID_REL,'SAÍDA PARA EXCEL'   ,'@FORMATO'  ,'CheckBox'  ,'Byte'  ,      1, NULL, 'S' ,'N', 7, 6, NULL )  
  
select * FROM RELATORIO WHERE NOMREL LIKE '%Historico%'  
  
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR,    'POS PAGO',  0,     'Numerico', @ID_DET + 1)  
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR,    'PRE PAGO',  1,     'Numerico', @ID_DET + 2)  
  
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR+5, 'NOME FANTASIA', 0, 'AlfaNumerico', @ID_DET + 3)  
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR+5, 'RAZAO SOCIAL ', 1, 'AlfaNumerico', @ID_DET + 4)  
  
  
  
  
  
-- PARA O MODULO WEB  
DECLARE @IDREL INT  
DECLARE @IDPAR INT  
SET @IDREL = 1940  
INSERT RELATORIO  
VALUES ( @IDREL, 'Historico das Transacoes por CPF', 'Historico das Transacoes por CPF', 'Historico Transacoes', 'REL_HISTORICO_TRANSACOES', 'N', NULL, '*', 'S', NULL, 'MW', 'N')  
  
INSERT PARAMETRO  
VALUES( @IDREL  , @IDREL, 'CLIENTE'          , '@CODCLI'     , 'TextBox'      , 'String',  5, NULL,  'S', 'S', 1, 1)  
INSERT PARAMETRO  
VALUES( @IDREL+1, @IDREL, 'CPF    '          , '@CPF'        , 'TextBox'      , 'String', 11, NULL,  'N', 'N', 2, 2)  
INSERT PARAMETRO  
VALUES( @IDREL+2, @IDREL, 'DATA INICIO   '   , '@DATAINI'    , 'DateEdit'     , 'String', 10, NULL,  'N', 'N', 3, 3)  
INSERT PARAMETRO  
VALUES( @IDREL+3, @IDREL, 'DATA FIM   '      , '@DATAFIM'    , 'DateEdit'     , 'String', 10, NULL,  'N', 'N', 4, 4)  
INSERT PARAMETRO  
VALUES( @IDREL+4, @IDREL, 'DADOS CREDENCIADO', '@TIPO'       , 'DropDownList' , 'Byte'  ,  1, 0   ,  'S', 'S', 5, 5)  
INSERT PARAMETRO  
VALUES( @IDREL+5, @IDREL, 'SAÍDA PARA EXCEL ', '@FORMATO'    , 'CheckBox'     , 'Byte'  ,  1, NULL,  'S', 'N', 6, 6)  
  
SET @IDPAR = (SELECT MAX(ID) FROM DETALHES_PARAMETROS)  
INSERT DETALHES_PARAMETROS  
VALUES (@IDREL+4, 'NOME FANTASIA', 0, 'AlfaNumerico', @IDPAR+1)  
INSERT DETALHES_PARAMETROS  
VALUES (@IDREL+4, 'RAZAO SOCIAL ', 1, 'AlfaNumerico', @IDPAR+2)  
*/  
  
  