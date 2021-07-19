ALTER PROCEDURE [dbo].[REL_FECHCRED] 
                    @SISTEMA smallint = null,        
                    @CLASSIF smallint = null, 
                    @SELECAO smallint = null,  
                    @TIPO smallint = null,        
                    @DATA_INI varchar(10) = null, 
                    @DATA_FIM varchar(10) = null,        
                    @PARAM_INI varchar(50) = null, 
                    @PARAM_FIM varchar(50) = null, 
                    @CENTRALIZ smallint = null, 
                    @FORMATO smallint = null,
                    @COD_STATUS_REL     VARCHAR(100) ,  
                    @ERRO               INT             OUTPUT ,    
                    @MSG_ERRO           VARCHAR(800)    OUTPUT 
AS  
  
   
SET NOCOUNT ON        
      
/* PARA TESTAR RETIRE OS COMENTARIOS */        
    --DECLARE @CENTRALIZ  VARCHAR(1)        
    --DECLARE @CLASSIF    smallint        
    --DECLARE @SELECAO    smallint        
    --DECLARE @TIPO       smallint             
    --DECLARE @DATA_INI   varchar(10)         
    --DECLARE @DATA_FIM   varchar(10)         
    --DECLARE @PARAM_INI  varchar(50)         
    --DECLARE @PARAM_FIM  varchar(50)         
    --DECLARE @FORMATO    SMALLINT        
    --DECLARE @SISTEMA    SMALLINT        
    --DECLARE @COD_STATUS_REL     VARCHAR(100) 
    --DECLARE @ERRO               INT             
    --DECLARE @MSG_ERRO           VARCHAR(800)    
  
    --SET @ERRO = 0  
    --SET @MSG_ERRO  = ''  
    --SET @SISTEMA   = 0       
    --SET @TIPO      = 1   /* 0 Sintético - Analitico  */    
    --SET @CLASSIF   = 0        
    --SET @SELECAO   = 0   /* 0- por data de fechamento 1- por data de pagamento */        
    --SET @DATA_INI  = '20191210'        
    --SET @DATA_FIM  = '20191210'        
    --SET @PARAM_INI = '200887'  --  580700 'SESI_PR' -- 586935 'SESI_ACRE'  -- 586718 'SESI_GO' -- 588162 'SESI_RN'        
    --SET @PARAM_FIM = '200903'         
    --SET @CENTRALIZ  = 0        
    --SET @FORMATO   =  3  
/*----------------------------------*/        
        
--DECLARE @QUANTSUBREDE INT        
    DECLARE @AUX            INT        
    DECLARE @AUX2           INT        
    DECLARE @TOT_REG        INT        
    DECLARE @CONTADOR       INT        
    DECLARE @NOME           VARCHAR(50)        
    DECLARE @CGC            VARCHAR(15)        
    DECLARE @INIFEC         CHAR(10)        
    DECLARE @FIMFEC         CHAR(10)        
    DECLARE @PRAZO          VARCHAR(3)        
    DECLARE @DTPGTO         CHAR(10)        
    DECLARE @NUMFEC         smallint        
    DECLARE @AUXFEC         smallint        
    DECLARE @DTFEC          CHAR(10)        
    DECLARE @VALTRA         numeric(13,2)        
    DECLARE @VTAXA          numeric(13,2)        
    DECLARE @PARC           VARCHAR(6)        
    DECLARE @VTOTAL         numeric(13,2)        
    DECLARE @VTOTAL1        numeric(13,2)        
    DECLARE @VTAXAS1        numeric(13,2)        
    DECLARE @QUANT          int        
    DECLARE @QUANT1         int        
    DECLARE @VTOTAL_GERAL   numeric(13,2)        
    DECLARE @QUANT_GERAL    int        
    DECLARE @VTAXA_GERAL    numeric(13,2)        
    DECLARE @VLIQUIDO1      numeric(13,2)        
    DECLARE @VLIQUIDO_GERAL numeric (13,2)        
    DECLARE @NOMREDE        varchar(25)        
    DECLARE @ORIGEM         varchar(20)        
    DECLARE @NSUHOS         INT        
    DECLARE @NSUAUT         INT        
    DECLARE @CODCLI         varchar(5)        
    DECLARE @AUXCLI         varchar(5)        
    DECLARE @VTOTALCLI      numeric(15,2)     /* soma valtrans por cliente */        
    DECLARE @QUANTCLI       INT               /* conta quant transacoes por cliente */        
    DECLARE @TEM_SUB_REDE   varchar(1)     /* para verificar se há ou não sub-rede */        
    DECLARE @SUBREDE        INT        
    DECLARE @SUBREDE2       INT      
    DECLARE @NOM_SUBREDE    CHAR(26)    
    DECLARE @NOM_SUBREDE2   CHAR(26)  
    --DECLARE @AUX_TIPTRA     INT     
    
    
WAITFOR DELAY '00:05:00'
  
DECLARE @CPF     varchar(11)  
DECLARE @VALOR_TAXA_ADM numeric(9,2)   
DECLARE @DATA_TRANS CHAR(10)  
DECLARE @AUX_TIPTRA INT            
    DECLARE @LINIMP         VARCHAR(118)        
    DECLARE @CABEC1         VARCHAR(118)        
    DECLARE @CABEC2         VARCHAR(118)        
    DECLARE @CABEC3         VARCHAR(118)        
    DECLARE @CABEC4         VARCHAR(118)        
    DECLARE @CABEC5         VARCHAR(118)        
    DECLARE @CABEC6         VARCHAR(118)        
    DECLARE @TRACO          VARCHAR(118)        
    DECLARE @JAPASSOU       INT        
    DECLARE @LINHAS         INT        
    DECLARE @MAXLINHAS      INT        
    DECLARE @NOMOPE         VARCHAR(50)     
    DECLARE @OPERADORA      CHAR(30)  
    DECLARE @QT_ESPACO1     INT  
    DECLARE @QT_ESPACO2     INT     
        
    /* esta parte é para identificar se há ou não subrede, se houver já encaminha para a opção correta */        
            
    IF @SISTEMA = 0  /* pj */        
       BEGIN        
         SELECT @TEM_SUB_REDE = VAL FROM PARAM WHERE ID0 ='SUBREDE'        
         SELECT @NOMOPE = VAL FROM PARAM WHERE ID0 = 'NOMOPENET'    
       END        
    ELSE        
       BEGIN        
         SELECT @TEM_SUB_REDE = VAL FROM PARAMVA WHERE ID0 ='SUBREDE'        
         SELECT @NOMOPE = VAL FROM PARAMVA WHERE (ID0 = 'NOMOPENET' ) or (ID0 = 'NOMOPNET')        
       END        
            
    /*--------------------------------------------------------------*/        
    IF @CLASSIF = 0 AND (@PARAM_INI IS NULL) /* define param_INI e param_FIM se não estiverem definidos */        
       BEGIN        
       SET @PARAM_INI = '000001'        
       SET @PARAM_FIM = '999999'        
       END        
    ELSE        
    IF @CLASSIF = 0 AND ISNUMERIC(@PARAM_INI) = 0 /* se for por código e param_ini não for numero */        
       BEGIN        
       RAISERROR('PARAMETRO INVALIDO: Classificação por código, param_ini não pode ter Letras:  %s', 16, 1, @PARAM_INI)        
       RETURN        
       END        
    ELSE        
    IF @PARAM_INI IS NULL         
    BEGIN        
       SET @PARAM_INI = 'A'        
       SET @PARAM_FIM = 'Z'        
    END        
    /*-------------------------------------------------------------*/        
            
    DECLARE  @QUERY     Nvarchar(2096)         
    DECLARE  @CONDICAO  varchar(1024)        
    DECLARE  @ORDENACAO varchar(1024)        
        
SET  @CONDICAO = 'where C.FLAG_' + CASE @SISTEMA WHEN 0 THEN 'PJ' ELSE 'VA' END + ' = ''S'' AND '  +   
                   CASE @SELECAO WHEN 0 THEN 'DATFECLOT' ELSE 'DATPGTO' END +   
                   ' >= ''' + @DATA_INI + ''' and ' + CASE @SELECAO WHEN 0 THEN 'DATFECLOT' ELSE 'DATPGTO' END +   
                   ' <= ''' + @DATA_FIM + ''''    + CHAR(13) +  
                     
                   CASE @CLASSIF   
                   WHEN 0 THEN ' AND ((C.CODCRE >= ' + @PARAM_INI + ' AND C.CODCRE <= ' + @PARAM_FIM + ') ' +   
                               CASE @CENTRALIZ WHEN 1 THEN 'OR (C.CODCEN >= ' + @PARAM_INI + ' AND C.CODCEN <= ' + @PARAM_FIM + ')) ' ELSE ')' END       
                   WHEN 1 THEN ' AND C.NOMFAN LIKE ' + CHAR(39) + '%' + @PARAM_INI + '%' + CHAR(39) + ' AND C.NOMFAN LIKE ' + CHAR(39) + '%' + @PARAM_FIM + '%' + CHAR(39)  
                   WHEN 2 THEN ' AND C.RAZSOC LIKE ' + CHAR(39) + '%' + @PARAM_INI + '%' + CHAR(39) + ' AND C.RAZSOC LIKE ' + CHAR(39) + '%' + @PARAM_FIM + '%' + CHAR(39)   
                   END  
        
SET @ORDENACAO =   /*ordenando a querie de acordo com a Selecao */        
    CASE @CLASSIF         
    WHEN 0 THEN ' ORDER BY C.CODCRE'        
    WHEN 1 THEN ' ORDER BY C.NOMFAN, C.CODCRE'        
    WHEN 2 THEN ' ORDER BY C.RAZSOC, C.CODCRE'        
    END       +  
        
    CASE @SELECAO      /* completando ordenacao de acordo com a classificacao */         
    WHEN 0 THEN  ', DATFECLOT, F.NUMFECCRE '        
    WHEN 1 THEN  ', DATPGTO, F.NUMFECCRE '        
    END       +     
  
    ', T.CODSUBREDE, T.DATTRA'   
    --CASE @TIPO        /* completando a ordenacao de acordo com o tipo de relatorio */        
    --WHEN 0 THEN ', T.CODSUBREDE, T.DATTRA'        
    --WHEN 1 THEN ', T.CODSUBREDE, T.DATTRA'        
    --END        
  
  CREATE TABLE #TABRELTEMP2  ( NOMSUBREDE varchar(26),   
                               CODCRE     int,        
                               CGC        char(14),        
                               NOMFAN     CHAR(50),        
                               RAZSOC     varchar(50),        
                               NUMBOR     smallint,              
                               VALPAG     numeric(15,2),         
            QTETRA     int,                   
                               TAXA       numeric (9,2),        
                               VALTAXA    numeric (15,2),         
                               VALLIQ     numeric (15,2),         
                               DTINIFECH  CHAR(10),        
                               DTFIMFECH  CHAR(10),        
                               NUMFECH    smallint,        
                               DATFECLOT  CHAR(10),        
                               PRAZO      smallint,        
                               DATPGTO    CHAR(10),        
                               DATTRA     CHAR(10),            
                               NSUHOS     int,                
                               NSUAUT     int,                
                               CPF        char(11),  
                               NUMDEP     smallint,  
                CODRTA     char(1),  
                               TIPTRA     int,                
                               VALTRA     numeric(15,2),        
                               CODSUBREDE INT,        
                               CODCLI     CHAR(5),        
                               ORIGEM     CHAR(20),        
                               PARC       CHAR(5),        
                               VTAXA      numeric(15,2),  
                               BANCO      varchar (6),  
                               AGENCIA    varchar(10),  
                               CONTA      varchar(20),  
                               NOMESIS    varchar(50),  
                               ID         INT IDENTITY PRIMARY KEY        
                            )        
       
        
  SET @QUERY =         
    N'INSERT INTO #TABRELTEMP2 '                                                                                               + CHAR(13) +   
    'select ISNULL(S.NOMSUBREDE, (SELECT NOMSUBREDE FROM SUBREDE WHERE CODSUBREDE = 1)) AS NOMSUBREDE, '                       + CHAR(13) +   
    'C.CODCRE, C.CGC, NOMFAN, RAZSOC, NUMBOR, VALPAG AS VALPAG, '                                                              + CHAR(13) +   
    'QTETRAVAD AS QTETRA, ' +                                                                                                  + CHAR(13) +   
     CASE @SISTEMA WHEN 0 THEN 'F.TAXSER ' ELSE 'F.TAXADM ' END  + 'AS TAXA, '                                                 + CHAR(13) +   
    'F.VALTAXA AS VALTAXA, '                                                                                                   + CHAR(13) +   
    'F.VALPAG - F.VALTAXA AS VALLIQ, '                                                                                         + CHAR(13) +  
    'CONVERT(CHAR(10), DATINI,103), CONVERT(CHAR(10),DATFIN,103), '                                                            + CHAR(13) +   
    'F.NUMFECCRE, CONVERT(CHAR(10),DATFECLOT,103), F.PRAPAG, '                                                                 + CHAR(13) +   
    'CASE WHEN F.DATPGTO IS NULL THEN CONVERT(CHAR(10), F.DATFECLOT+F.PRAPAG,103) ELSE CONVERT(CHAR(10), F.DATPGTO,103) END, ' + CHAR(13) +    
    'CONVERT(CHAR(10), T.DATTRA,103), ISNULL(T.NSUHOS,0), ISNULL(T.NSUAUT,0), '                                                + CHAR(13) +   
    'T.CPF, T.NUMDEP, T.CODRTA, '                                                                                              + CHAR(13) +   
    'ISNULL(T.TIPTRA,0), ISNULL(T.VALTRA,0), ISNULL(T.CODSUBREDE,1), '                                                         + CHAR(13) +   
    'CASE WHEN CL.CODCLI IS NULL THEN ''ZZZZZ'' ELSE STR(CL.CODCLI, 5) END AS CODCLI, '                                        + CHAR(13) +     
    'CASE WHEN (T.TIPTRA  < 80000 ) '                                                                                          + CHAR(13) +   
    '     THEN dbo.MascaraCartao(T.CODCRT, 17) '                                                                               + CHAR(13) +    
    'ELSE SUBSTRING(TPX.DESTIPTRA, 1, 17)+ REPLICATE( CHAR(32), 17 - LEN(SUBSTRING(TPX.DESTIPTRA, 1, 17)))  END AS ORIGEM, '   + CHAR(13) +      
    'CASE WHEN SUBSTRING(T.DAD, 30, 1) = '' '' THEN ''1/1'' ELSE CAST(ASCII(SUBSTRING(T.DAD, 30, 1)) - 32 AS VARCHAR ) + ''/'' + CAST(ASCII(SUBSTRING(T.DAD, 31, 1)) - 32 AS VARCHAR) END AS PARC, ' +     CHAR(13) +    
    'CASE WHEN T.TIPTRA < 80000 THEN CAST(T.VALTRA * ( ' + CASE @SISTEMA WHEN 0 THEN 'F.TAXSER' ELSE 'F.TAXADM' END + ' / 100) AS NUMERIC(12,2)) ELSE 0.00 END AS VTAXA, ' + CHAR(13) +   
    'SUBSTRING(F.CTABCO, 1, 4) AS BANCO, SUBSTRING(F.CTABCO, 6, 6) AS AGENCIA, '                                               + CHAR(13) +   
 'CASE WHEN LEN(F.CTABCO) < 12 THEN '''' ELSE SUBSTRING(F.CTABCO, 13, LEN(F.CTABCO)-12) END AS CONTA, '                     + CHAR(13) +   
    CHAR(39) + @NOMOPE + CHAR(39) +' ' +                                                       + CHAR(13) +  
    'FROM ' + CASE @SISTEMA WHEN 0 THEN 'FECHCRED' ELSE 'FECHCREDVA' END + ' F '                                               + CHAR(13) +    
    'INNER JOIN CREDENCIADO C ON F.CODCRE = C.CODCRE '                                                                         + CHAR(13) +   
    'JOIN ' + CASE @SISTEMA WHEN 0 THEN 'TRANSACAO' ELSE 'TRANSACVA' END + ' T on T.CODCRE = C.CODCRE AND T.NUMFECCRE = F.NUMFECCRE ' + CHAR(13) +   
    'AND (TIPTRA < 80000 OR (TIPTRA >= 999300 AND TIPTRA <= 999399))  AND CODRTA = ''V'' '                                     + CHAR(13) +   
    'LEFT JOIN ' + CASE @SISTEMA WHEN 0 THEN 'CLIENTE' ELSE 'CLIENTEVA' END + ' CL  ON T.CODCLI  = CL.CODCLI '                 + CHAR(13) +     
    'LEFT JOIN SUBREDE S ON T.CODSUBREDE = S.CODSUBREDE '                                                                      + CHAR(13) +   
    'LEFT JOIN TIPTRANS TPX ON T.TIPTRA = TPX.TIPTRA '               + CHAR(13) +   
    @CONDICAO    + @ORDENACAO         
  
--PRINT @QUERY      
  
EXEC(@QUERY)        
        
        
IF @FORMATO = 0         
 BEGIN  /* formata o relatório  tipo 2 ou 3 ou 4*/  
  
  /* tabela temporaria que vai receber o relatório */  
  DECLARE @LINREL TABLE ( LINHAIMP  varchar(132),  
                         TIP       char(1)   
                        )  
                          
  
        SELECT  @OPERADORA = VAL FROM CONFIG_JOBS WHERE ID0 = 'OPERADORA'        
  
        /* PARA CENTRALIZAR O CABECALHO COM O NOME DA OPERADORA E O TEXTO SISTEMA NETCARD */  
        SET @QT_ESPACO1 = ((118-LEN(LTRIM(RTRIM(@OPERADORA))))/2)  
        SET @QT_ESPACO2 = ((118-(@QT_ESPACO1 + LEN(@OPERADORA)))/4)+2       
  
        SET @LINIMP = ''       
        SET @CABEC1 = REPLICATE(' ', @QT_ESPACO1 ) + @OPERADORA +    
                    SPACE(@QT_ESPACO2) + 'SISTEMA NETCARD ' + CASE @SISTEMA WHEN 0 THEN 'PJ' ELSE  'PP' END   
        
  
        SET @CABEC2 = REPLICATE(' ' , 23) + 'RELATORIO '        +         
                      CASE @TIPO         
                           WHEN 0 THEN 'SINTETICO '         
                           WHEN 1 THEN 'ANALITICO '        
                      END        
                      + 'DE LOTES DE CREDENCIADOS POR DATA DE ' +        
                      CASE @SELECAO        
                           WHEN 0 THEN 'FECHAMENTO'        
                           WHEN 1 THEN 'PAGAMENTO '        
                      END   
                                                                +   
                      CASE @CENTRALIZ  
                           WHEN 0 THEN ''  
                           ELSE ' P/CENTRALIZADORA'  
                      END       
  
        SET @TRACO = REPLICATE(CHAR(151),118)    
          
        SET @CABEC3 = '           PERIODO DE ' + SUBSTRING(@DATA_INI,7,2) + '/'+SUBSTRING(@DATA_INI,5,2)+'/'+SUBSTRING(@DATA_INI,1,4)  +   
                      ' A ' +  
                      SUBSTRING(@DATA_FIM, 7,2) + '/' + SUBSTRING(@DATA_FIM, 5,2) + '/' + SUBSTRING(@DATA_FIM,1,4) +         
                      ' ORDENADO ' +         
                      CASE @CLASSIF    
                           WHEN 0 THEN 'PELO CODIGO DE CREDENCIADO '          
                           WHEN 1 THEN 'PELA RAZAO SOCIAL '          
                           WHEN 2 THEN 'PELO NOME FANTASIA '         
                      END          +      
                      'COM SELECAO DE ' + @PARAM_INI + ' A ' + @PARAM_FIM        
  
        SET @CABEC4 = 'CREDENCIADO                          CNPJ                INICIO      FIM        LOTE       DT.FEC.    PRZO  DT.PAGTO'  
          
          
        SET @CABEC5 = SPACE(17) +   
                      CASE @TEM_SUB_REDE WHEN 'S' THEN 'SUBREDE' ELSE SPACE(7) END +  
                      SPACE(23) +   
                      'QUANT                        VALOR        TAXAS       V.LIQUIDO'            
  
        SET @CABEC6 = SPACE(08) + 'DATA       NSU     DOC     ' + CASE @SISTEMA WHEN 0 THEN 'PARCELA' ELSE '' END +  
                                '         ORIGEM                      VAL.TRANS     VAL.TAXA       V.LIQUIDO    '   
      
      
  SET @LINHAS = 7  
  SET @MAXLINHAS = 99999999  
  SET @JAPASSOU = 1  /* 1 imprime cabec6, 0 não imprime */  
  
  SET @TOT_REG = (SELECT COUNT(*) FROM #TABRELTEMP2)  
  
  SET @CONTADOR = 1  
  SET @QUANT_GERAL  = 0  
  SET @VTOTAL_GERAL = 0  
  SET @VTAXA_GERAL  = 0  
        SET @VLIQUIDO_GERAL = 0  
  
  /* IMPRIME O CABECALHO DA PAGINA */  
  INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC1,0  
  INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC2,0  
  INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC3,0  
  INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO, 0  
  INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC4,0  
  INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO, 0  
  /*-------------------------------*/  
  
        WHILE (@CONTADOR <= @TOT_REG)  
        BEGIN   /* lê o registro da tabela um por um */  
          SELECT @AUX = CODCRE, @NOME = RAZSOC, @CGC = CGC, @INIFEC = DTINIFECH,   
           @FIMFEC     = DTFIMFECH,   
           @PRAZO      = PRAZO,   
           @NUMFEC     = NUMFECH,   
           @SUBREDE    = CODSUBREDE,   
           @DTFEC      = DATFECLOT,   
           @DTPGTO     = DATPGTO,   
           @VALOR_TAXA_ADM = TAXA,   
           @CODCLI     = CODCLI,   
           @VALTRA     = VALTRA,  
           @ORIGEM     = ORIGEM,   
           @CPF        = CPF,   
           @DATA_TRANS = DATTRA, @NSUHOS = NSUHOS, @NSUAUT = NSUAUT,@AUX_TIPTRA = TIPTRA,  
                 @VTAXA      = VTAXA,   
                 @PARC       = PARC,  
                 @VTOTAL1    = VALPAG,   /* já pega os totais para imprimir no final */  
                 @VTAXAS1    = VALTAXA,   
                 @VLIQUIDO1  = VALLIQ,   
                 @QUANT1     = QTETRA                   
          FROM #TABRELTEMP2 where ID = @Contador  
/*===========================================================================================*/  
  
          /* insere a linha com os dados do credenciado */  
          SET @LINIMP = STR(@AUX,6) + '  ' +         
                        SUBSTRING(@NOME,1,26) + REPLICATE(' ',26-LEN(SUBSTRING(@NOME, 1, 26))) + '  ' +         
                        SUBSTRING(@CGC,1,2) +'.'+SUBSTRING(@CGC,3,3)+'.'+SUBSTRING(@CGC,6,3)+'/'+SUBSTRING(@CGC,9,4) +'-'+SUBSTRING(@CGC,13,2)+ '  ' +   
                        @INIFEC + '  ' +        
                        @FIMFEC + '  ' +        
                        STR(@NUMFEC, 4) + '      ' +        
                        @DTFEC + '  ' +         
                        STR(@PRAZO, 3)  + '  ' +         
                        @DTPGTO        
  
          INSERT @LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
  
          SET @LINHAS = @LINHAS + 1  
          SET @AUX2   = @AUX       /* guarda o codigo do credenciado para controle do cabecalho */  
          SET @AUXCLI = @CODCLI  
          SET @AUXFEC = @NUMFEC  /* guarda o numero do fechamento para controle */  
  
          SET @VTOTAL      = 0  
          SET @QUANT       = 0  
          SET @VTOTALCLI   = 0  /* soma  valtrans  por  cliente  usa quando tipo = 4 */  
          SET @QUANTCLI    = 0  /* conta quant trans por cliente usa quando tipo = 4*/  
          --SET @VALOR_TAXAS = 0   
          --SET @VLIQUIDO1   = 0   
          --SET @QUANT1      = 0         
          --SET @VTOTAL1     = 0  
          --SET @VTAXAS1     = 0                
          SET @LINHAS = @LINHAS + 1  
/*---------===================================--------------------================================*/           
          WHILE  (@CONTADOR <= @TOT_REG) AND (@AUX2 = @AUX) AND (@AUXFEC = @NUMFEC)  
          BEGIN  
                IF (@TIPO = 1) AND (@JAPASSOU = 1)   /* analitico/subrede */  
                BEGIN                         /* insere o cabecalho das transações */  
                   INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC6, 2  
                   SET @JAPASSOU = 0  
                END  
  
            
          SET @QUANTCLI  = @QUANTCLI  + 1        /* usa quando tipo = 4 */  
          IF (@AUX_TIPTRA  < 80000)  
                BEGIN  
                  SET @QUANT  = @QUANT  + 1  
               SET @VTOTAL = @VTOTAL + @VALTRA   
            SET @VTOTALCLI = @VTOTALCLI + @VALTRA  /* usa quando tipo = 4 */  
                END  
                  ---olhar se precisa deste @auxfec aqui  
          IF (@TIPO = 1)  AND (@AUXFEC = @NUMFEC)  /* se relatorio ANALITICO mostra dados da transações */  
                BEGIN  
                    /* inserir as taxas */  
                    IF (@AUX_TIPTRA  >= 999300 AND @AUX_TIPTRA <= 999399)   
                    BEGIN  
                       SET @LINIMP = REPLICATE( ' ', 08) +   
                                     CONVERT(VARCHAR(10), @DATA_TRANS, 103) + ' ' +   
                                     STR(@NSUHOS, 7)  + ' ' +   
                                     STR(@NSUAUT, 7)  + ' ' +   
                                     SPACE(09)        +  
                                     dbo.F_ACENTO(UPPER(@ORIGEM))   +    
                                     SPACE(18)        +   
                                     REPLACE(STR(@VALTRA, 12, 2),'.',',')  
                    END  
           ELSE  
                    BEGIN  
  
                       IF LEN(@PARC) < 5  
                       BEGIN  
                         SET @PARC = ' ' + @PARC  
                       END  
  
                       SET @ORIGEM = @ORIGEM + REPLICATE(' ',17- LEN(@ORIGEM))   /* corrige o tamanho no caso dos cartões antigos*/  
  
                       SET @LINIMP = REPLICATE( ' ', 08)           +   
                                     @DATA_TRANS                   +  
                                     ' '                           +   
                                     STR(@NSUHOS, 7)               +  
                                     ' '                           +   
                                     STR(@NSUAUT, 7)               +   
                                     ' '                           +   
                                     CASE WHEN @AUX_TIPTRA  < 80000   
                                          THEN REPLICATE(' ', 5- LEN(LTRIM(RTRIM(@PARC)))) + LTRIM(RTRIM(@PARC))   
                                          ELSE SPACE(5)   
                                     END                           +   
                                     '    '                        +  
                                     dbo.F_ACENTO(UPPER(@ORIGEM))  +      
                                     --SPACE(08)                     +       
                                     CASE WHEN @AUX_TIPTRA  < 80000   
                                          THEN '     '   
                                          ELSE  SPACE(18)   
                                     END                           +     
                                     ' '                           +      
                                     REPLACE(STR(@VALTRA, 11, 2),'.',',')    
                    END  
              INSERT @LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
             END    
  
          /* lê o proximo registro */  
             SET @CONTADOR = @CONTADOR + 1   
    SELECT @AUX2 = CODCRE, @SUBREDE2 = CODSUBREDE, @VALTRA = VALTRA,   
                    @ORIGEM = ORIGEM, @CPF = CPF, @DATA_TRANS = DATTRA,   
              @NSUHOS = NSUHOS, @NSUAUT = NSUAUT, @CODCLI = CODCLI, @AUXFEC = NUMFECH, @AUX_TIPTRA = TIPTRA,  
                       @VTAXA = VTAXA, @PARC = PARC  
             FROM #TABRELTEMP2 where ID = @Contador  
  
  
             IF (@SUBREDE <> @SUBREDE2)  OR (@AUX2 <> @AUX) OR (@CONTADOR > @TOT_REG)   OR (@AUXFEC <> @NUMFEC)  
             BEGIN /* se Subrede = 0 mostrar nome da operadora */  
             --     IF @TEM_SUB_REDE = 'S'  
             --     BEGIN  
             --      IF (@SUBREDE = 0) OR (@SUBREDE IS NULL)  
             --   BEGIN  
             --    SET @NOMREDE = SUBSTRING(@OPERADORA,1,30)  
             --   END  
             --ELSE  
             --   BEGIN  
             --     SELECT @NOMREDE = SUBSTRING(NOMSUBREDE,1,30) FROM SUBREDE WHERE CODSUBREDE = @SUBREDE  
             --   END  
  
             --SET @LINIMP = REPLICATE(' ', 14) +   
             --  @NOMREDE + REPLICATE(' ', 30-LEN(@NOMREDE)) +  
             --  ' ' +  
             --  STR(@QUANT, 6)  +   
             --  SPACE(17)       +   
             --  REPLACE(STR(@VTOTAL, 13, 2),'.',',') + ' '     
  
             --INSERT @LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
             --     END  
  
            IF @TIPO = 1  
            BEGIN  
            SET @LINIMP = REPLICATE(' ', 110)  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
            END  
  
  
            SET @JAPASSOU = 1   /* para imprimir CABEC6 novamente */  
            SET @LINHAS   = @LINHAS + 1  
            SET @SUBREDE  = @SUBREDE2  
               SET @QUANT    = 0  
            SET @VTOTAL   = 0  
            --SET @VTOTAL1 = @VTOTAL1 + @VTOTAL  
            --SET @QUANT1  = @QUANT1  + @QUANT  
                  --SET @VTAXAS1 = @VTAXAS1 + @VALOR_TAXAS  
                  --SET @VLIQUIDO1 = @VTOTAL1-@VALOR_TAXAS+(@VTOTAL1 * (@VALOR_TAXA_ADM /100))  /* pegar no banco de dados*/  
             END  
          END  
/*-----------------------------=====================================================-------------------------------*/  
          --IF @SISTEMA = 0 /* PJ */  
          --BEGIN  
          --  SELECT @VLIQUIDO1 = VALLIQ, @VTAXAS1 = VALTAXA, @VTOTAL1 = VALPAG FROM FECHCRED WHERE NUMFECCRE = @NUMFEC AND CODCRE = @AUX  
          --END  
          --ELSE  
          --BEGIN  
          --  SELECT @VLIQUIDO1 = VALLIQ, @VTAXAS1 = VALTAXA, @VTOTAL1 = VALPAG FROM FECHCREDVA WHERE NUMFECCRE = @NUMFEC AND CODCRE = @AUX  
          --END  
  
          /* BUSCA O OS TOTAIS PARA IMPRIMIR */   -- ASSIM DEMORA MAIS NAO SEI PORQUE  
          --SELECT TOP 1 @VTOTAL1   = VALPAG,   
          --             @VTAXAS1   = VALTAXA,   
          --             @VLIQUIDO1 = VALLIQ,   
          --             @QUANT1    = QTETRA  
          --FROM #TABRELTEMP2   
          --WHERE (CODCRE = @AUX AND NUMFECH = @NUMFEC)   
/*-----------------------------=====================================================-------------------------------*/  
  
--*****************************************  
          IF (@TIPO = 0  AND @TEM_SUB_REDE = 'S')   
          BEGIN  
              INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC5,2  
              INSERT @LINREL(LINHAIMP, TIP)   
              SELECT SPACE(17)                                +   
                     LTRIM(RTRIM(NOMSUBREDE))+REPLICATE(' ', 26-LEN(LTRIM(RTRIM(NOMSUBREDE)))) +   
                     SPACE(02)                               +   
                     STR(COUNT(NOMSUBREDE),6)                +   
                     SPACE(15)                               +   
                     REPLACE(STR(SUM(VALTRA),15,2),'.',',')  +  
                     SPACE(05)                               +   
                     REPLACE(STR(SUM(CAST( ((VALTRA*TAXA)/100.0) AS NUMERIC(15,2))),08,2),'.',',') +  
                     SPACE(01)                               +   
                     REPLACE(STR(SUM(VALTRA - CAST( ((VALTRA*TAXA)/100.0) AS NUMERIC(15,2))),15,2),'.',',')      
                     ,2   
              FROM #TABRELTEMP2      
              WHERE TIPTRA < 80000 AND VALTRA <> 0   
              AND CODCRE = @AUX   
              AND NUMFECH = @NUMFEC  
              GROUP BY NOMSUBREDE  
          END  
--*****************************************  
            
          SET @LINIMP = REPLICATE(' ',8)  +  
                     'TOTAL  ' + REPLICATE(' ',30) +   
                     STR(@QUANT1, 6) + '  ' +   
                     SPACE(13)       +  
                     REPLACE(STR(@VTOTAL1, 15, 2),'.',',') +  ' ' +   
                        REPLACE(STR(@VTAXAS1, 12, 2),'.',',') + ' ' +  
                     REPLACE(STR(@VLIQUIDO1, 15, 2),'.',',')    
  
  
          INSERT @LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  
            
          INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO, 1  
  
          SET @LINHAS = @LINHAS + 2  
          IF @LINHAS > @MAXLINHAS  
          BEGIN  
            SET @LINHAS = 1  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC1,0  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC2,0  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC3,0  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO, 1  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @CABEC4,0  
            INSERT @LINREL(LINHAIMP, TIP) SELECT @TRACO, 1  
          END  
  
          SET @VTOTAL_GERAL   = @VTOTAL_GERAL + @VTOTAL1  
          SET @QUANT_GERAL    = @QUANT_GERAL  + @QUANT1  
          SET @VTAXA_GERAL    = @VTAXA_GERAL  + @VTAXAS1    
          SET @VLIQUIDO_GERAL = @VLIQUIDO_GERAL + @VLIQUIDO1  
        END  
          
     IF (@TIPO = 0  AND @TEM_SUB_REDE = 'S')   
        BEGIN  
            INSERT @LINREL(LINHAIMP, TIP)   
            SELECT SPACE(17)                                +   
                  LTRIM(RTRIM(NOMSUBREDE))+REPLICATE(SPACE(1), 26-LEN(LTRIM(RTRIM(NOMSUBREDE)))) +   
                  SPACE(02)                               +   
                  STR(COUNT(NOMSUBREDE),6)                +   
                  SPACE(15)                               +   
                  REPLACE(STR(SUM(VALTRA),15,2),'.',',')  +  
                  SPACE(05)                               +   
                  REPLACE(STR(SUM(CAST( ((VALTRA*TAXA)/100.0) AS NUMERIC(15,2))),08,2),'.',',') +  
                  SPACE(01)                               +   
                  REPLACE(STR(SUM(VALTRA - CAST( ((VALTRA*TAXA)/100.0) AS NUMERIC(15,2))),15,2),'.',',')      
                  ,2   
            FROM #TABRELTEMP2    WHERE TIPTRA < 80000 AND VALTRA <> 0   
            GROUP BY NOMSUBREDE  
        END          
          
        SET @LINIMP = SPACE(08) +  
                  'TOTAL  GERAL'                             +  
                  REPLICATE(' ',25)                          +  
                  REPLACE(STR(@QUANT_GERAL, 6),'.',',')      +   
                  SPACE(15)                                  +   
                  REPLACE(STR(@VTOTAL_GERAL, 15, 2),'.',',') +    
                  ' '                                        +   
                  REPLACE(STR(@VTAXA_GERAL, 12, 2),'.',',')  +   
                  ' '                                        +   
                  REPLACE(STR(@VLIQUIDO_GERAL, 15, 2),'.',',')    
  
        INSERT @LINREL(LINHAIMP, TIP) SELECT @LINIMP, 2  

        INSERT INTO RELATORIO_RESULTADO 
        SELECT @COD_STATUS_REL, LINHAIMP, TIP 
        FROM @LINREL  
    END  
        
IF @FORMATO = 1  /* SAIDA PARA EXCELL */        
BEGIN  

   IF @TIPO = 0         
   BEGIN    
     IF @TEM_SUB_REDE = 'S'     
        BEGIN
        INSERT INTO RELATORIO_RESULTADO  
        SELECT @COD_STATUS_REL, 'SUBREDE;BANCO;AGENCIA;CONTA;COD.CREDENCIADO;CNPJ;NOME FANTASIA;RAZAO SOCIAL;VALOR;QTDE;TAXA;'+
        'VALOR TAXA;VALOR LIQUIDO;DATA INICIO FECH;DATA FIM FECH;NUM.FECHAMENTO;DATA FECH.LOTE;PRAZO;DATA PAGTO;SISTEMA', '' 

        --> VER DEPOIS PARA QUEM UTILIZA MODULO DE TAXAS...  
        SELECT  @COD_STATUS_REL,
        NOMSUBREDE                                                                                                + ';' +                     
        ISNULL(BANCO, ' ')                                                                                        + ';' +             
        ISNULL(AGENCIA, ' ')                                                                                      + ';' +             
        ISNULL(CONTA, ' ')                                                                                        + ';' +             
        CONVERT(VARCHAR,CODCRE)                                                                                   + ';' +             
        CGC                                                                                                       + ';' +             
        NOMFAN                                                                                                    + ';' +             
        RAZSOC                                                                                                    + ';' +             
        CONVERT(VARCHAR,REPLACE(STR(SUM(VALTRA),11,2),'.',','))                                                   + ';' + 
        CONVERT(VARCHAR,COUNT(DATTRA))                                                                            + ';' +                    
        CONVERT(VARCHAR,REPLACE(STR(TAXA, 7, 2),'.',','))                                                         + ';' +        
        CONVERT(VARCHAR,REPLACE(STR( (SUM(VALTRA) - (SUM(VALTRA) * (1 - TAXA / 100))),11,2),'.',','))             + ';' +        
        CONVERT(VARCHAR,REPLACE(STR( CAST((SUM(VALTRA) * (1 - TAXA / 100)) AS NUMERIC(15,2)),11,2),'.',','))      + ';' +     
        CONVERT(VARCHAR, DTINIFECH, 103)                                                                          + ';' + 
        CONVERT(VARCHAR, DTFIMFECH, 103)                                                                          + ';' + 
        CONVERT(VARCHAR,NUMFECH)                                                                                  + ';' + 
        CONVERT(VARCHAR, DATFECLOT, 103)                                                                          + ';' + 
        CONVERT(VARCHAR,PRAZO),                                                                                   + ';' + 
        CONVERT(VARCHAR, DATPGTO, 103)                                                                            + ';' + 
        NOMESIS                                 
        , '' 
        FROM #TABRELTEMP2            
        WHERE TIPTRA < 80000  
        GROUP BY NOMSUBREDE, ISNULL(BANCO, ' '), ISNULL(AGENCIA, ' '), ISNULL(CONTA, ' '), CODCRE, CGC,  
        NOMFAN, RAZSOC,   
        TAXA,   
        CONVERT(VARCHAR, DTINIFECH, 103),        
        CONVERT(VARCHAR, DTFIMFECH, 103), NUMFECH,   
        CONVERT(VARCHAR, DATFECLOT, 103), PRAZO,        
        CONVERT(VARCHAR, DATPGTO, 103), NOMESIS  
        ORDER BY CODCRE, NUMFECH, NOMSUBREDE  
    END
     ELSE 
    BEGIN
     
        INSERT INTO RELATORIO_RESULTADO  
        SELECT @COD_STATUS_REL, 'BANCO;AGENCIA;CONTA;COD.CREDENCIADO;CNPJ;NOME FANTASIA;RAZAO SOCIAL;VALOR;PARCELAS;QTDE;TAXA;'+
        'VALOR TAXA;VALOR LIQUIDO;DATA INICIO FECH;DATA FIM FECH;NUM.FECHAMENTO;DATA FECH.LOTE;PRAZO;DATA PAGTO;SISTEMA', '' 

        SELECT DISTINCT @COD_STATUS_REL,
        ISNULL(BANCO, ' ')                 + ';' +   
        ISNULL(AGENCIA, ' ')               + ';' +  
        ISNULL(CONTA, ' ')                 + ';' +  
        CONVERT(VARCHAR, CODCRE)           + ';' +    
        CGC                                + ';' +          
        NOMFAN                             + ';' +    
        RAZSOC                             + ';' +    
        REPLACE(STR(VALPAG, 11,2),'.',',') + ';' +   
        ISNULL(PARC, '1/1')                + ';' +   
        CONVERT(VARCHAR, QTETRA)           + ';' +         
        REPLACE(STR(TAXA   ,11,2),'.',',') + ';' +    
        REPLACE(STR(VALTAXA,11,2),'.',',') + ';' +    
        REPLACE(STR(VALLIQ, 11,2),'.',',') + ';' +         
        CONVERT(VARCHAR, DTINIFECH, 103)   + ';' +         
        CONVERT(VARCHAR, DTFIMFECH, 103)   + ';' +    
        CONVERT(VARCHAR, NUMFECH)          + ';' +    
        CONVERT(VARCHAR, DATFECLOT, 103)   + ';' +    
        CONVERT(VARCHAR, PRAZO)            + ';' +       
        CONVERT(VARCHAR, DATPGTO, 103)     + ';' +    
        NOMESIS                            ,
        ''
        FROM #TABRELTEMP2        
    END
   END  
   ELSE  
   BEGIN  
    IF @TEM_SUB_REDE = 'S'  
    BEGIN
        INSERT INTO RELATORIO_RESULTADO  
        SELECT @COD_STATUS_REL, 'SUBREDE;COD.CREDENCIADO;CNPJ;NOME FANTASIA;RAZAO SOCIAL;BANCO;AGENCIA;CONTA;VALOR;PARCELAS;QTDE;TAXA;'+
        'VALOR TAXA;VALOR LIQUIDO;TIPO TRANS;VALOR TRANSAÇÃO;DATA INICIO FECH;DATA FIM FECH;NUM.FECHAMENTO;DATA FECH.LOTE;PRAZO;DATA PAGTO;DATA TRANSAÇÃO;'+
        'NUM.HOST;NUM.AUTORIZAÇÃO;ORIGEM;CPF;TIPO;STATUS;COD.CLIENTE', '' 

        SELECT @COD_STATUS_REL,
        NOMSUBREDE                           + ';' +  
        CODCRE                               + ';' +  
        CGC                                  + ';' +        
        NOMFAN                               + ';' +  
        RAZSOC                               + ';' +  
        ISNULL(BANCO, ' ')                   + ';' + 
        ISNULL(AGENCIA, ' ')                 + ';' +
        ISNULL(CONTA, ' ')                   + ';' +
        REPLACE(STR(VALPAG, 11, 2), '.',',') + ';' +      
        ISNULL(PARC, '1/1')                  + ';' + 
        QTETRA                               + ';' +        
        REPLACE(STR(TAXA,   11, 2), '.',',') + ';' +  
        REPLACE(STR(VALTAXA,11, 2), '.',',') + ';' +  
        REPLACE(STR(VALLIQ, 11, 2), '.',',') + ';' +       
        TIPTRA                               + ';' +  
        REPLACE(STR(VALTRA, 11, 2), '.',',') + ';' +  
        CONVERT(VARCHAR, DTINIFECH, 103)     + ';' +       
        CONVERT(VARCHAR, DTFIMFECH, 103)     + ';' +  
        NUMFECH                              + ';' +  
        CONVERT(VARCHAR, DATFECLOT, 103)     + ';' +
        PRAZO                                + ';' +  
        CONVERT(VARCHAR, DATPGTO, 103)       + ';' + 
        (CONVERT(VARCHAR, DATTRA, 103) + ' ' + CONVERT(VARCHAR, DATTRA, 108)) + ';' +   
        NSUHOS                               + ';' +  
        NSUAUT                               + ';' +       
        dbo.F_ACENTO(UPPER(ORIGEM))          + ';' + 
        CPF                                  + ';' +  
        CASE WHEN NUMDEP = 0 THEN 'TITULAR' ELSE 'DEPENDENTE' END + ';' + 
        CODRTA                               + ';' +        
        CASE WHEN CODCLI = 'ZZZZZ' THEN '' ELSE CODCLI END        ,
        ''
        FROM #TABRELTEMP2    
    END
    ELSE  
    BEGIN
        
        INSERT INTO RELATORIO_RESULTADO  
        SELECT @COD_STATUS_REL, 'COD.CREDENCIADO;CNPJ;NOME FANTASIA;RAZAO SOCIAL;BANCO;AGENCIA;CONTA;VALOR;PARCELAS;QTDE;TAXA;'+
        'VALOR TAXA;VALOR LIQUIDO;TIPO TRANS;VALOR TRANSAÇÃO;DATA INICIO FECH;DATA FIM FECH;NUM.FECHAMENTO;DATA FECH.LOTE;PRAZO;DATA PAGTO;DATA TRANSAÇÃO;'+
        'NUM.HOST;NUM.AUTORIZAÇÃO;ORIGEM;CPF;TIPO;STATUS;COD.CLIENTE', '' 

        SELECT @COD_STATUS_REL,
        CONVERT(VARCHAR, CODCRE)                       + ';' +   
        CGC                                 + ';' +         
        NOMFAN                              + ';' +   
        RAZSOC                              + ';' +   
        ISNULL(BANCO, ' ')                  + ';' +  
        ISNULL(AGENCIA, ' ')                + ';' + 
        ISNULL(CONTA, ' ')                  + ';' + 
        REPLACE(STR(VALPAG, 11, 2),'.',',') + ';' +    
        ISNULL(PARC, '1/1')                 + ';' +  
        CONVERT(VARCHAR, QTETRA)            + ';' +         
        REPLACE(STR(TAXA,   11, 2),'.',',') + ';' +   
        REPLACE(STR(VALTAXA,11, 2),'.',',') + ';' +   
        REPLACE(STR(VALLIQ, 11, 2),'.',',') + ';' +        
        CONVERT(VARCHAR, TIPTRA)            + ';' +   
        REPLACE(STR(VALTRA, 11, 2),'.',',') + ';' +  
        CONVERT(VARCHAR, DTINIFECH, 103)    + ';' +        
        CONVERT(VARCHAR, DTFIMFECH, 103)    + ';' +   
        CONVERT(VARCHAR, NUMFECH)           + ';' +   
        CONVERT(VARCHAR, DATFECLOT, 103)    + ';' +   
        CONVERT(VARCHAR, PRAZO)             + ';' +        
        CONVERT(VARCHAR, DATPGTO, 103)      + ';' +  
        CONVERT(VARCHAR, DATTRA, 103) + ' ' + CONVERT(VARCHAR, DATTRA, 108) + ';' +  
        CONVERT(VARCHAR, NSUHOS)            + ';' + 
        CONVERT(VARCHAR, NSUAUT)            + ';' +        
        dbo.F_ACENTO(UPPER(ORIGEM))         + ';' +
        CPF                                 + ';' +
        CASE WHEN NUMDEP = 0 THEN 'TITULAR' ELSE 'DEPENDENTE' END + ';' +
        CODRTA                              + ';' +      
        CASE WHEN CODCLI = 'ZZZZZ' THEN '' ELSE CODCLI END,
        ''
        FROM #TABRELTEMP2  
   END
   END  
END        
  
  
IF @FORMATO = 3
BEGIN  
    INSERT INTO RELATORIO_RESULTADO 
    SELECT @COD_STATUS_REL, 'CODCRE'       +        
           ';'            +      
           'CGC'          +    
           ';'            +          
           'NOMFAN'       +    
           ';'            +          
           'RAZSOC'       +    
           ';'            +     
           'BANCO'        +  
           ';'            +      
           'AGENCIA'      +  
           ';'            +      
           'CONTA'        +  
           ';'            +                    
           'VALOR'        +    
           ';'            +           
           'QT_OPER'      +                   
           ';'            +      
           'TAXA'         +    
           ';'            +          
           'VALOR_TAXA'   +   
           ';'            +            
           'VALOR_LIQUIDO'+   
           ';'            +            
           --'TIPTRA'       +    
           --';'            +                  
           'INICIO_LOTE'  +    
           ';'            +          
           'FIM_LOTE'     +    
           ';'            +          
           'NUM_LOTE'     +    
           ';'            +     
           'FECHTO_LOTE'  +    
           ';'            +     
           'DT_PAGTO'     +    
           ';'            +      
           'DT_TRANS'     +    
           ';'            +          
           'NSUHOS'       +         
           ';'            +             
           'NSUAUT'       +         
           ';'            +     
           'VALOR_TRANS'  +    
           ';'            +                
           'ORIGEM'       +    
           ';'            +     
           'COD.CLIENTE'  +                       
           ';'            +               
           'CPF'          +  
           ';'            +      
           'T/D'          +  
           --';'            +      
           --'CODRTA'       +  
           ';'            AS LINHA, '' AS TIPO     
    UNION       
  
    SELECT STR(CODCRE,6)                      +        
           ';'                                +      
           CGC                                +    
           ';'                                +          
           NOMFAN                             +    
           ';'                                +          
           RAZSOC                             +    
           ';'                                +     
           REPLACE(BANCO, ' ', '0')           +  
           ';'                                +      
           REPLACE(AGENCIA,' ', '0')          +  
           ';'                                +      
           REPLACE(CONTA, ' ','0')            +  
           ';'                                +               
           REPLACE(STR(VALPAG,11,2),'.',',')  +    
           ';'                                +           
           STR(QTETRA,6)                      +                   
           ';'                                +      
           REPLACE(STR(TAXA,6,2),'.',',')     +    
           ';'                                +          
           REPLACE(STR(VALTAXA,11,2),'.',',') +   
           ';'                                +            
           REPLACE(STR(VALLIQ,11,2),'.',',')  +   
           ';'                                +     
           --STR(TIPTRA,6)                      +    
           --';'                                +    
           DTINIFECH                          +    
           ';'                                +          
           DTFIMFECH                          +    
           ';'                                +          
           STR(NUMFECH,3)                     +    
           ';'                                +          
           DATFECLOT                          +    
           ';'                                +          
           DATPGTO                            +    
           ';'                                +          
           DATTRA                             +    
           ';'                                +           
           STR(NSUHOS,8)                      +         
           ';'                                +             
           STR(NSUAUT,8)                      +         
           ';'                                +     
           REPLACE(STR(VALTRA,11,2),'.',',')  +    
           ';'                                +                 
           dbo.F_ACENTO(UPPER(ORIGEM))        +    
           ';'                                +       
           CASE WHEN CODCLI = 'ZZZZZ'   
                THEN '     '   
                ELSE CODCLI   
           END                                +             
           ';'                                +                         
           ISNULL(CPF, SPACE(11))             +    
           ';'                                +      
           CASE WHEN NUMDEP = 0 THEN 'T' ELSE 'D' END +  
           --';'                                +      
           --CODRTA                             +  
           ';'          AS LINHA, '' AS TIPO              
    FROM #TABRELTEMP2    
    ORDER BY TIPO  
   
END  
  
DROP TABLE #TABRELTEMP2        
        
SET NOCOUNT OFF        
  
  
/* INSERE LISTAGEM NO MODULO DE RELATORIOS */  
/*  
DECLARE @ID_REL     INT  
DECLARE @ID_PAR     INT  
DECLARE @ID_DETALHE INT  
  
-- deletando o relatorio anterior  
SELECT @ID_REL = ID_REL FROM RELATORIO WHERE NOMPROC = 'REL_FECHCRED'   
  
DELETE D FROM  DETALHES_PARAMETROS D  
JOIN PARAMETRO P ON ( D.ID_PARAMETRO = P.ID_PAR)  
WHERE P.ID_REL = @ID_REL  
  
DELETE FROM PARAMETRO WHERE ID_REL = @ID_REL  
DELETE FROM RELATORIO WHERE ID_REL = @ID_REL  
  
SET @ID_REL     = ( SELECT MAX(ID_REL) FROM RELATORIO)  
SET @ID_PAR     = ( SELECT MAX(ID_PAR)  FROM PARAMETRO)  
SET @ID_DETALHE = ( SELECT MAX(ID_PARAMETRO) FROM DETALHES_PARAMETROS)  
  
INSERT RELATORIO VALUES (@ID_REL+1,'Relatório de Lotes Fechados de Credenciados','Lotes Fechados', 'Credenciados','REL_FECHCRED','N','', '*','S',NULL,'NC','N', NULL)  
  
insert PARAMETRO VALUES (@ID_PAR+1, @ID_REL+1,'CLASSIFICAÇÃO'    , '@CLASSIF' ,'DropDownList', 'Byte' , 1,'0'     , NULL,'S',1, 1, NULL)  
insert PARAMETRO VALUES (@ID_PAR+2, @ID_REL+1,'POR DATA DE  '    , '@SELECAO' ,'DropDownList', 'Byte' , 1,'0'     , NULL,'S',2, 2, NULL)  
insert PARAMETRO VALUES (@ID_PAR+3, @ID_REL+1,'TIPO         '    , '@TIPO'     ,'DropDownList', 'Byte' , 1,'0'     , NULL,'S',3, 3, NULL)  
insert PARAMETRO VALUES (@ID_PAR+4, @ID_REL+1,'DATA INICIAL   ', '@DATA_INI' ,'DateEdit'    , 'String',10, NULL   , NULL,'N',4, 4, NULL)  
insert PARAMETRO VALUES (@ID_PAR+5, @ID_REL+1,'DATA FINAL        ', '@DATA_FIM' ,'DateEdit'    , 'String',10, NULL   , NULL,'N',5, 5, NULL)  
insert PARAMETRO VALUES (@ID_PAR+6, @ID_REL+1,'INTERVALO INICIAL ', '@PARAM_INI','TextBox'     , 'String',50,'1'     , NULL,'N',6, 6, NULL)  
insert PARAMETRO VALUES (@ID_PAR+7, @ID_REL+1,'INTERVALO FINAL   ', '@PARAM_FIM','TextBox'     , 'String',50,'999999', NULL,'N',7, 7, NULL)  
insert PARAMETRO VALUES (@ID_PAR+8, @ID_REL+1,'CENTRALIZADORA    ', '@CENTRALIZ','CheckBox'    , 'Byte' , 1,'N'     , NULL,'N',8, 8, NULL)  
insert PARAMETRO VALUES (@ID_PAR+9, @ID_REL+1,'SAÍDA PARA EXCEL  ', '@FORMATO' ,'CheckBox'    , 'Byte' , 1, NULL   , NULL,'S',9, 9, NULL)  
  
               
insert DETALHES_PARAMETROS VALUES (@ID_PAR+1,'CODIGO'       , 0,'Numerico'    , @ID_DETALHE+1 )  
insert DETALHES_PARAMETROS VALUES (@ID_PAR+1,'NOME FANTASIA', 1,'AlfaNumerico', @ID_DETALHE+2 )  
insert DETALHES_PARAMETROS VALUES (@ID_PAR+1,'RAZAO SOCIAL ', 2,'AlfaNumerico', @ID_DETALHE+3 )  
  
insert DETALHES_PARAMETROS VALUES (@ID_PAR+2,'FECHAMENTO'   , 0,'Numerico'    , @ID_DETALHE+4 )  
insert DETALHES_PARAMETROS VALUES (@ID_PAR+2,'PAGAMENTO'    ,  1,'Numerico'    , @ID_DETALHE+5 )  
  
insert DETALHES_PARAMETROS VALUES (@ID_PAR+3,'SINTETICO'    , 0, 'Numerico'   , @ID_DETALHE+6 )  
insert DETALHES_PARAMETROS VALUES (@ID_PAR+3,'ANALITICO'    , 1, 'Numerico'   , @ID_DETALHE+7 )  
  
--DECLARE @ID_REL INT  
SELECT @ID_REL = ID_REL FROM RELATORIO WHERE NOMPROC = 'REL_FECHCRED'   
SELECT * FROM RELATORIO WHERE ID_REL = @ID_REL   
SELECT * fROM PARAMETRO WHERE ID_REL = @ID_REL  
SELECT D.* FROM  DETALHES_PARAMETROS D  
JOIN PARAMETRO P ON ( D.ID_PARAMETRO = P.ID_PAR)  
WHERE P.ID_REL = @ID_REL  
  
  
*/  
  
  