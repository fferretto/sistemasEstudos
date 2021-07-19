/* LISTAGEM DAS TRANSAÇÕES DO CLIENTE, ACUMULADADAS PELO SEGMENTO            */    
/* DO CREDENCIADO                                                            */    
/*                                                                           */    
/* ESTE RELATORIO OCORRE DUAS VEZES NA TABELA, ELE TEM UMA  CONFIGURACAO     */    
/* PARA USO NO MODULO DE CLIENTES E OUTRA  PARA USO NO NETCARD               */      
/*                                                                           */    
/* 12/02/2015  Marco Avellar                                  REV 02         */    
/*---------------------------------------------------------------------------*/    
ALTER PROCEDURE [dbo].[MW_REL_LST_TRANS_CLIENTE_POR_SEGMENTO]   
                    @SISTEMA int = Null,   
                    @CODCLI varchar(5) = NULL,     
                    @DATA_INI CHAR(08),   
                    @DATA_FIM CHAR(08),   
                    @FORMATO smallint = null,      
                    @COD_STATUS_REL     VARCHAR(100),  
                    @ERRO               INT             OUTPUT ,    
                    @MSG_ERRO           VARCHAR(800)    OUTPUT          
AS    
    
BEGIN          
 SET NOCOUNT ON;       
    DECLARE @AUX_SEG INT   
    
/* PARA TESTAR RETIRE OS COMENTARIOS ABAIXO*/    
    
 --DECLARE @SISTEMA INT    
 --DECLARE @FORMATO SMALLINT     
 --DECLARE @CODCLI  varchar(5)     
 --DECLARE @DATA_INI CHAR(08)    
 --DECLARE @DATA_FIM CHAR(08)    
 --SET @SISTEMA = 0    
 --SET @CODCLI  = null --'1500'    
 --SET @DATA_INI  = '20190101'    
 --SET @DATA_FIM  = '20190131'    
 --SET @FORMATO = 0    
    
/*-----------------------------------------*/     
SET @ERRO = 0  
SET @MSG_ERRO  = ''  
 IF ISDATE(@DATA_INI) = 0     
 BEGIN    
     SET @ERRO = 1    
     raiserror ('Erro: Data Inicial inválida' ,16,1)    
 END    
    
 IF ISDATE(@DATA_FIM) = 0     
 BEGIN    
     SET @ERRO = 1    
     raiserror ('Erro: Data Final inválida' ,16,1)    
 END    
 /*TRATAMENTO PARA NÃO EXIBIR INFORMAÇÕES COM MAIS DE 90 DIAS*/    
 IF @ERRO = 0     
 BEGIN    
  IF DATEDIFF(DAY, @DATA_INI, GETDATE()) > 90    
  BEGIN    
    SET @DATA_INI = CONVERT( CHAR(08), GETDATE() - 90, 112)   /* AAAAMMDD */    
  END    
    
  DECLARE @NOMCLI   VARCHAR(40)    
  DECLARE @COMANDO  VARCHAR(2048)    
  DECLARE @QUANT_TOTAL INT    
  DECLARE @VALOR_TOTAL NUMERIC(11,2)    
  SET @QUANT_TOTAL = 0     
  SET @VALOR_TOTAL = 0     
    
   DECLARE @TABELA TABLE (  SEGMENTO      CHAR(40),     
          QT_TRANSACOES INT,     
          VALOR         NUMERIC(15,2),     
          QT_PERC       NUMERIC(15,2),     
          VALOR_PERC    NUMERIC(15,2),     
          QUANT_TOTAL   INT,     
          VALOR_TOTAL   NUMERIC(15,2) )    
   IF @SISTEMA = 0    
   BEGIN    
   /* Pega o nome do cliente */    
    SET @NOMCLI = ISNULL((SELECT NOMCLI FROM CLIENTE WHERE CODCLI = @CODCLI), 'NAO ENCONTRADO')    
         
         
    INSERT @TABELA (SEGMENTO, QT_TRANSACOES, VALOR )    
    SELECT  dbo.F_ACENTO(S.NOMSEG) AS SEGMENTO, COUNT(S.NOMSEG) AS QT_TRANSACOES, SUM(T.VALTRA) AS VALOR     
    FROM TRANSACAO T     
    INNER JOIN CREDENCIADO C ON C.CODCRE = T.CODCRE     
    INNER JOIN SEGMENTO S ON (S.CODSEG = C.CODSEG)     
    WHERE ( @CODCLI IS NULL OR ( @CODCLI  IS NOT NULL AND T.CODCLI =  @CODCLI) )      
    AND T.TIPTRA < 80000 AND T.CODRTA IN ('V','A')     
    AND ISNULL(ASCII(SUBSTRING(T.DAD,30,1))-32,1) = 1    
    AND (T.DATGER >= @DATA_INI AND T.DATGER <= @DATA_FIM)    
    GROUP BY  S.NOMSEG    
   END    
         
   IF @SISTEMA <> 0     
   BEGIN    
         
   /* Pega o nome do cliente */    
    SET @NOMCLI = ISNULL((SELECT NOMCLI FROM CLIENTEVA WHERE CODCLI = @CODCLI), 'NAO ENCONTRADO')    
         
        INSERT @TABELA (SEGMENTO, QT_TRANSACOES, VALOR )    
    SELECT  dbo.F_ACENTO(S.NOMSEG) AS SEGMENTO, COUNT(S.NOMSEG) AS QT_TRANSACOES, SUM(T.VALTRA) AS VALOR     
    FROM TRANSACVA T     
    INNER JOIN CREDENCIADO C ON C.CODCRE = T.CODCRE     
    INNER JOIN SEGMENTO S ON (S.CODSEG = C.CODSEG)     
    WHERE ( @CODCLI IS NULL OR ( @CODCLI IS NOT NULL AND T.CODCLI =  @CODCLI ) )     
    AND T.TIPTRA < 80000 AND T.CODRTA IN ('V','A')     
     AND (T.DATTRA >= @DATA_INI AND T.DATTRA <= @DATA_FIM)    
    GROUP BY  S.NOMSEG    
   END    
         
   SELECT @QUANT_TOTAL = SUM(QT_TRANSACOES), @VALOR_TOTAL = SUM(VALOR) FROM @TABELA    
    
   UPDATE @TABELA SET QUANT_TOTAL = @QUANT_TOTAL,     
         QT_PERC = CONVERT(NUMERIC(6,2), STR(CONVERT(NUMERIC(15,4),QT_TRANSACOES*100)/@QUANT_TOTAL,6,1)),    
         VALOR_TOTAL = @VALOR_TOTAL,     
         VALOR_PERC = CONVERT(NUMERIC(6,2), STR(CONVERT(NUMERIC(15,4),VALOR*100)/@VALOR_TOTAL,6,1))    
  /* EMISSAO NO FORMATO TEXTO */    
  IF @FORMATO = 0     
  BEGIN    
   DECLARE @CABEC1      VARCHAR(121)          
   DECLARE @CABEC2      VARCHAR(121)          
   DECLARE @CABEC3      VARCHAR(121)          
   DECLARE @CABEC4      VARCHAR(121)          
   DECLARE @CABEC5      VARCHAR(121)          
   DECLARE @CABEC6      VARCHAR(121)          
   DECLARE @TRACO       VARCHAR(121)      
   DECLARE @OPERADORA   CHAR(30)    
   DECLARE @QT_ESPACO1  INT    
   DECLARE @QT_ESPACO2  INT    
   SELECT @OPERADORA = VAL FROM CONFIG_JOBS WHERE ID0 = 'OPERADORA'    
    
   /* PARA CENTRALIZAR O CABECALHO COM O NOME DA OPERADORA E O TEXTO SISTEMA NETCARD */    
   SET @QT_ESPACO1 = ((118-LEN(LTRIM(RTRIM(@OPERADORA))))/2)    
   SET @QT_ESPACO2 = ((118-(@QT_ESPACO1 + LEN(@OPERADORA)))/4)+2            
   SET @CABEC1    = REPLICATE(' ', @QT_ESPACO1 ) + @OPERADORA + SPACE(118-@QT_ESPACO1+LEN(@OPERADORA) )    
   SET @CABEC1    = STUFF(@CABEC1, 118-18, 18, 'SISTEMA NETCARD ' + CASE @SISTEMA WHEN 0 THEN 'PJ' ELSE  'PP' END )          
   SET @CABEC2    = REPLICATE(' ' , 30) + '  LISTAGEM DO LOTE DISTRIBUINDO OS VALORES POR SEGMENTO '     
   SET @CABEC3    = SPACE(40) + 'NO PERIODO DE ' +       
        SUBSTRING(@DATA_INI, 7,2) +  '/' + SUBSTRING(@DATA_INI, 5, 2) +  '/'+ SUBSTRING(@DATA_INI,1,4) +     
        ' ATE ' +     
        SUBSTRING(@DATA_FIM, 7,2) +  '/' + SUBSTRING(@DATA_FIM, 5, 2) +  '/'+ SUBSTRING(@DATA_FIM,1,4)    
                        
   SET @CABEC4    = 'CLIENTE  ' + ISNULL(@CODCLI, SPACE(5))  + ' - ' + CASE WHEN @CODCLI IS NULL THEN 'TODOS ' ELSE @NOMCLI END      
   SET @CABEC5    = 'SEGMENTO                           QT.TRANSACOES         VALOR   PERC QT    PERC VAL'    
    SET @TRACO     =  REPLICATE(CHAR(151),118)      
                   
   /*-------------------------------*/                
   DECLARE  @LINREL  TABLE ( [LINHAIMP]  varchar(148),          
            [TIP]       char(1)     
          )      
         
   /* IMPRIME O CABECALHO DA PAGINA */          
   INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC1,   0          
   INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC2,   0     
   INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC3,   0          
   INSERT @LINREL(LINHAIMP, TIP) SELECT SPACE(10), 0      
   INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC4,   0    
   INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO,    0          
   INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC5,   0    
             
   INSERT @LINREL(LINHAIMP, TIP)    
   SELECT SEGMENTO + ' ' +    
       STR(QT_TRANSACOES, 7) +  '   ' +     
       REPLACE(STR(VALOR,11,2)     , '.',',') + '    ' +     
       REPLACE(STR(QT_PERC, 5,2)   , '.',',') + '%' + '      ' +     
       REPLACE(STR(ISNULL(VALOR_PERC,0), 5,2), '.',',') + '%' + ' ', 2     
   FROM @TABELA ORDER BY QT_TRANSACOES DESC    
            
  
    INSERT INTO RELATORIO_RESULTADO    
    SELECT @COD_STATUS_REL,     
            LINHAIMP, TIP     
        FROM @LINREL   
  
  END    
    
  /* EMISSÃO NO FORMATO EXCELL */    
  IF (@FORMATO = 1)  --EXCELL SEM CABECALHO    
  BEGIN    
    
        INSERT INTO RELATORIO_RESULTADO    
        SELECT @COD_STATUS_REL, 'SEGMENTO;QT_TRANSACOES;VALOR;% PERC QT;% PER VAL;', ''    
          
        INSERT INTO RELATORIO_RESULTADO    
        SELECT @COD_STATUS_REL,  
               SEGMENTO + ';' +  
               QT_TRANSACOES  + ';' +   
               REPLACE(STR(VALOR,11,2)     , '.',',')  + ';' +  
               REPLACE(STR(QT_PERC, 5,2)   , '.',',') + '%'  + ';' +  
               REPLACE(STR(VALOR_PERC, 5,2), '.',',') + '%'  + ';', ''  
        FROM @TABELA ORDER BY QT_TRANSACOES DESC     
  END    
 END    
END    
    
SET ANSI_NULLS ON    
  
    
/* PROCEDIMENTO PARA CRIAR O RELATORIO */    
/*    
-----------------------  ATENCAO: ESTE RELATORIO OCORRE DUAS VEZES NA TABELA, ELE TEM UMA  CONFIGURACAO PARA USO NO MODULO DE CLIENTES E OUTRA    
-----------------------           PARA USO NO NETCARD    
    
DECLARE @ID_REL INT    
DECLARE @ID_PAR INT    
DECLARE @ID_DET INT    
    
-- APAGANDO O RELATORIO ANTERIOR DO NETCARD    
select @ID_REL = ID_REL from relatorio where nomproc = 'MW_REL_LST_TRANS_CLIENTE_POR_SEGMENTO' AND MODULO = 'NC'    
DELETE FROM DETALHES_PARAMETROS WHERE ID_PARAMETRO IN ( SELECT ID_PAR FROM PARAMETRO WHERE ID_REL = @ID_REL)    
DELETE PARAMETRO WHERE ID_REL = @ID_REL    
DELETE RELATORIO WHERE ID_REL = @ID_REL    
    
    
SET @ID_REL = ( SELECT MAX(ID_REL) FROM RELATORIO)    
SET @ID_PAR = ( SELECT MAX(ID_PAR)  FROM PARAMETRO)    
SET @ID_DET = ( SELECT MAX(ID)     FROM DETALHES_PARAMETROS)    
    
INSERT RELATORIO VALUES (@ID_REL+1, 'Transações por Segmento', 'Transações por Segmento', 'Clientes', 'MW_REL_LST_TRANS_CLIENTE_POR_SEGMENTO', 'N', NULL, '*',  'S', NULL, 'NC', 'N', NULL)    
    
INSERT PARAMETRO VALUES(@ID_PAR+1, @ID_REL+1  ,'TIPO CARTAO     ', '@SISTEMA' ,'DropDownList','Byte'  ,   1,    0,  'N', 'S', 1, 0, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+2, @ID_REL+1  ,'CODIGO CLIENTE  ', '@CODCLI'  ,'TextBox'     ,'String',  5, NULL, NULL, 'N', 2, 1, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+3, @ID_REL+1  ,'DATA INICIO     ', '@DATA_INI','DateEdit'    ,'String', 10, NULL, NULL, 'N', 3, 2, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+4, @ID_REL+1  ,'DATA FIM        ', '@DATA_FIM','DateEdit'    ,'String', 10, NULL, NULL, 'N', 4, 3, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+5, @ID_REL+1  ,'SAIDA PARA EXCEL', '@FORMATO' ,'CheckBox'    ,'byte'  ,  1, NULL, NULL, 'N', 5, 4, NULL)    
    
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR+1, 'POS PAGO', 0, NULL, @ID_DET+1 )    
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR+1, 'PRE PAGO', 1, NULL, @ID_DET+2)    
    
    
-- APAGANDO O RELATORIO ANTERIOR DO MODULO WEB     
select @ID_REL = ID_REL from relatorio where nomproc = 'MW_REL_LST_TRANS_CLIENTE_POR_SEGMENTO' AND MODULO = 'MW'    
DELETE FROM DETALHES_PARAMETROS WHERE ID_PARAMETRO IN ( SELECT ID_PAR FROM PARAMETRO WHERE ID_REL = @ID_REL)    
DELETE PARAMETRO WHERE ID_REL = @ID_REL    
DELETE RELATORIO WHERE ID_REL = @ID_REL    
    
    
SET @ID_REL = ( SELECT MAX(ID_REL) FROM RELATORIO)    
SET @ID_PAR = ( SELECT MAX(ID_PAR)  FROM PARAMETRO)    
SET @ID_DET = ( SELECT MAX(ID)     FROM DETALHES_PARAMETROS)    
    
INSERT RELATORIO VALUES (@ID_REL+1, 'Transações por Segmento', 'Transações por Segmento', 'Clientes', 'MW_REL_LST_TRANS_CLIENTE_POR_SEGMENTO', 'N', NULL, '*',  NULL, 'CLIENTE', 'MW', 'N', NULL)    
    
--INSERT PARAMETRO VALUES(@ID_PAR+1, @ID_REL+1  ,'TIPO CARTAO     ', '@SISTEMA' ,'DropDownList','Byte'  ,   1,    0,  'N', 'S', 1, 0, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+2, @ID_REL+1  ,'CODIGO CLIENTE  ', '@CODCLI'  ,'TextBox'     ,'String',  5,    0, NULL, 'N', 1, 1, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+3, @ID_REL+1  ,'DATA INICIO     ', '@DATA_INI','DateEdit'    ,'String', 10, NULL, NULL, 'N', 1, 2, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+4, @ID_REL+1  ,'DATA FIM        ', '@DATA_FIM','DateEdit'    ,'String', 10, NULL, NULL, 'N', 3, 3, NULL)    
INSERT PARAMETRO VALUES(@ID_PAR+5, @ID_REL+1  ,'SAIDA PARA EXCEL', '@FORMATO' ,'CheckBox'    ,'byte'  ,  1, NULL, NULL, 'N', 4, 4, NULL)    
    
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR+1, 'POS PAGO', 0, NULL, @ID_DET+1 )    
INSERT DETALHES_PARAMETROS VALUES (@ID_PAR+1, 'PRE PAGO', 1, NULL, @ID_DET+2)    
    
*/     
    