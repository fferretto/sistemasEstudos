
    IF EXISTS (SELECT 1 
                     FROM INFORMATION_SCHEMA.TABLES 
                     WHERE TABLE_SCHEMA = 'DBO' 
                     AND  TABLE_NAME = 'PAGNET_MENU')
    BEGIN
            DELETE FROM PAGNET_MENU
            
    END
    ELSE
    BEGIN
        CREATE TABLE PAGNET_MENU
        (
             CODMENU        INT             NOT NULL PRIMARY KEY
            ,NOME           VARCHAR(50)     NOT NULL
            ,DESCRICAO      VARCHAR(50)     NOT NULL
            ,CODMENUPAI     INT             NULL     --FOREIGN KEY REFERENCES PAGNET_MENU(CODMENU)
            ,AREA           VARCHAR(50)     NULL
            ,CONTROLLER     VARCHAR(50)     NULL 
            ,ACTION         VARCHAR(50)     NULL
            ,NIVEL          INT             NOT NULL
            ,ORDEM          INT             NOT NULL
            ,favIcon        varchar(30)     NULL
            ,ATIVO          CHAR(1)         NOT NULL DEFAULT 'S'  
            ,EXCLUSIVOUSUNC CHAR(1)         NULL
        )
        
    END
    ---------------------------------------------------------------------------------------------------------------------------------------------
    --------------------------------------------------------MENU DE CONFIGURAÇÃO-----------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    BEGIN
        DECLARE @CODMENU INT, @CODMENUPAI INT
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConfiguracao', 'Configuração', null, null, null, null, 1, 1, 'fa fa-cog', 'S','N'
    END
     --------------------------------------------CONFIRGURAÇÃO DE REGRAS DEFAULT DE PAGAMENTO E EMISSÃO DE BOLETOS-------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuConfiguracao'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuRegrasNegocio', 'Regras do Sistema', @CODMENUPAI, null, null, null, 2, 1, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRegrasNegocio'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuRegraEmissaoBoleto', 'Emissão de Boleto', @CODMENUPAI, 'Configuracao', 'ConfigRegraBoleto', 'Index', 3, 1, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRegrasNegocio'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConfigDescontoFolha', 'Desconto em Folha', @CODMENUPAI, 'Configuracao', 'ConfigParamLeituraArqDF', 'Index', 3, 2, NULL, 'N','N'
    END
    
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRegrasNegocio'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuRegraPagamento', 'Antecipação PGTO', @CODMENUPAI, 'Configuracao', 'ConfigRegraPagamento', 'Index', 3, 3, NULL, 'S','N'
    END
    
     --------------------------------------------CONFIGURAÇÃO DE CONTROLE DE ACESSO-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuConfiguracao'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuPerfilAcesso', 'Perfil de Acesso', @CODMENUPAI, 'Configuracao', 'PerfisAcesso', 'Index', 2, 2, NULL, 'N','N'
    END
    
     --------------------------------------------CONFIGURACAO DE PLANO DE CONTAS-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuConfiguracao'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuPlanoContas', 'Plano de Contas', @CODMENUPAI, 'Configuracao', 'PlanoContas', 'Index', 2, 3, NULL, 'S','N'
    END
     --------------------------------------------CONFIGURAÇÃO DE APARENCIA DO SISTEMA-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuConfiguracao'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuAparencia', 'Definições de Aparência', @CODMENUPAI, 'Configuracao', 'Aparencia', 'Index', 2, 4, NULL, 'N','N'
    END
    
    ---------------------------------------------------------------------------------------------------------------------------------------------
    --------------------------------------------------------MENU DE CADASTRO-----------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuCadastro', 'Cadastros', null, null, null, null, 1, 2, 'fa fa-cog', 'S','N'
    END
    
     --------------------------------------------CADASTRO DE EMPRESAS-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuEmpresa', 'Empresa', @CODMENUPAI, 'Cadastros', 'Empresa', 'Index', 2, 1, NULL, 'S','N'
    END
    
     --------------------------------------------CADASTRO DE USUÁRIOS DO SISTEMA-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuUsuario', 'Usuário', @CODMENUPAI, 'Cadastros', 'Usuarios', 'Index', 2, 2, NULL, 'S','N'
    END
    
     --------------------------------------------CADASTRO DE FORNECEDOR-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuFornecedor', 'Favorecido', @CODMENUPAI, 'Cadastros', 'Fornecedor', 'Index', 2, 3, NULL, 'S','N'
    END
    
     --------------------------------------------CADASTRO DE CENTRALIZADORA-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuCentralizadora', 'Centralizadora', @CODMENUPAI, 'Cadastros', 'Centralizadora', 'Index', 2, 4, NULL, 'S','S'
    END
    
     --------------------------------------------CADASTRO DE CLIENTES EMPRESAS-------------------------------------------
   BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'nmuCadastroCliente', 'Cliente', @CODMENUPAI, null, null, null, 2, 6, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'nmuCadastroCliente'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuCadClienteEmpresa', 'Pessoa Jurídica', @CODMENUPAI, 'Cadastros', 'Cliente', 'Index', 3, 1, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'nmuCadastroCliente'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuCadClienteUsuario', 'Pessoa Física', @CODMENUPAI, 'Cadastros', 'ClienteUsuario', 'Index', 3, 2, NULL, 'S','N'
    END
    
     --------------------------------------------CADASTRO DE CONTAS CORRENTES-------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuContaCorrente', 'Conta Corrente', @CODMENUPAI, 'Cadastros', 'ContaCorrente', 'Index', 2, 7, NULL, 'S','N'
    END
    
     --------------------------------------------CADASTRO DE CONTAS DE EMAIL-------------------------------------------    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuContaEmail', 'Contas Email', @CODMENUPAI, 'Cadastros', 'ContasEmail', 'Index', 2, 8, NULL, 'S','N'
    END
    
	     --------------------------------------------CADASTRO DE CONTAS DE EMAIL-------------------------------------------    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuCadastro'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuInstrucaoEmail', 'Instrução Email', @CODMENUPAI, 'Cadastros', 'InstrucaoEmail', 'Index', 2, 9, NULL, 'S','N'
    END
    ---------------------------------------------------------------------------------------------------------------------------------------------
    ------------------------------------------------------MENU DE PAGAMENTOS---------------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------    
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuPagamentos', 'Pagamento', null, null, null, null, 1, 3, 'fas fa-dollar-sign', 'S','N'
    END
    ------------------------------------------------------CONSULTA------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuPagamentos'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConsultarCredenciados', 'Consultar Titulos', @CODMENUPAI, 'ContasPagar', 'ConsultarTitulos', 'Index', 2, 1, NULL, 'S','N'
    END
    ------------------------------------------------------EMITIR TÍTULO AVULSTO---------------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuPagamentos'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'nmuEmissaoTitulo', 'Emissão de Título Avulso', @CODMENUPAI, 'ContasPagar', 'EmitirTitulo', 'Index', 2, 2, NULL, 'S','N'
    END
    ------------------------------------------------------BORDERO---------------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuPagamentos'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuBorderoPagamento', 'Borderô', @CODMENUPAI, null, null, null, 2, 4, 'fas fa-dollar-sign', 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuBorderoPagamento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuGeraBorderoPag', 'Gerar', @CODMENUPAI, 'ContasPagar', 'GeraBordero', 'Index', 3, 1, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuBorderoPagamento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConsultaBorderoPag', 'Consultar', @CODMENUPAI, 'ContasPagar', 'ConsultaBordero', 'Index', 3, 2, NULL, 'S','N'
    END
    ------------------------------------------------------REMESSA---------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuPagamentos'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuRemessa', 'Arquivo de Remessa', @CODMENUPAI, null, null, null, 2, 5, 'fas fa-dollar-sign', 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRemessa'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuGerarArquivoRemessa', 'Gerar', @CODMENUPAI, 'ContasPagar', 'GerarArquivoRemessa', 'Index', 3, 1, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRemessa'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConsultarArquivoRemessa', 'Consultar', @CODMENUPAI, 'ContasPagar', 'ConsultarArquivoRemessa', 'Index', 3, 2, NULL, 'S','N'
    END
    ------------------------------------------------------ARQUIVO RETORNO---------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuPagamentos'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuArquivoRetorno', 'Baixa via Arquivo', @CODMENUPAI, 'ContasPagar', 'ArquivoRetorno', 'Index', 2, 6, NULL, 'S','N'
    END
    ------------------------------------------------------REAGENDAR PAGAMENTO---------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuPagamentos'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'nmuReagendarPGTO', 'Alterar Vencimento Título', @CODMENUPAI, 'ContasPagar', 'ReagendarTitulo', 'Index', 2, 7, NULL, 'S','N'
    END
    
    ---------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------MENU DE RECEBIMENTOS---------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuRecebimento', 'Recebimento', null, null, null, null, 1, 4, 'fas fa-donate', 'S','N'
    END
    ------------------------------------------------------------Consultar Situação dos Boletos-----------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuBoletos', 'Consutar Faturamentos', @CODMENUPAI, 'ContasReceber', 'ConsultarBoletos', 'Index', 2, 1, NULL, 'S','N'
    END
    ------------------------------------------------------EMITIR BOLETO AVULSTO----------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'nmuEmissaoBoleto', 'Emissão/Edição de Faturamento', @CODMENUPAI, 'ContasReceber', 'IncluirNovoBoleto', 'Index', 2, 2, NULL, 'S','N'
    END
    ------------------------------------------------------------VALIDAR PEDIDOS DE FATURAMENTO-----------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuFaturamento', 'Validar Pedidos de Faturamento', @CODMENUPAI, 'ContasReceber', 'ValidaFaturamento', 'Index', 2, 3, NULL, 'S','N'
    END
    
    -------------------------------------------------------------BORDERO----------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuBorderoRec', 'Borderô', @CODMENUPAI, null, null, null, 2, 3, NULL, 'S','N'
    END
     
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuBorderoRec'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuGeraBordero', 'Gerar', @CODMENUPAI, 'ContasReceber', 'GerarBordero', 'Index', 3, 1, NULL, 'S','N'
    END
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuBorderoRec'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConsultaBordero', 'Consultar', @CODMENUPAI, 'ContasReceber', 'ConsultarBordero', 'Index', 3, 2, NULL, 'S','N'
    END
    
    ------------------------------------------------------------------ARQUIVO REMESSA-----------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuArqRemessaRec', 'Arquivo de Remessa', @CODMENUPAI, null, null, null, 2, 4, NULL, 'S','N'
    END
     
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuArqRemessaRec'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuArqGerar', 'Gerar', @CODMENUPAI, 'ContasReceber', 'GerarArquivoRemessa', 'Index', 3, 1, NULL, 'S','N'
    END
     
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuArqRemessaRec'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuArqConsultar', 'Consultar', @CODMENUPAI, 'ContasReceber', 'ConsultarArquivoRemessa', 'Index', 3, 2, NULL, 'S','N'
    END
    
    ------------------------------------------------------------------Geração de BOLETOS---------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuBoletos', 'Gerar Boletos', @CODMENUPAI, 'ContasReceber', 'EmitirBoletosGerados', 'Index', 2, 5, NULL, 'S','N'
    END
    
        ---------------------------------------------------------------------ARQUIVO DE RETORNO---------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuRecebimento'

    --
        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuArquivoRetorno', 'Baixa via Arquivo', @CODMENUPAI, 'ContasReceber', 'ArquivoRetorno', 'Index', 2, 6, NULL, 'S','N'
    END
    ---------------------------------------------------------------------------------------------------------------------------------------------
    ---------------------------------------------------------------MENU TESOURARIA---------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnTesouraria', 'Tesouraria', null, null, null, null, 1, 5, 'fa fa-balance-scale', 'S','N'
    END
    
    ------------------------------------------------------------LANCAMENTO E CONSOLIDAÇÃO DE TRANSACAO-------------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnTesouraria'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuTransacao', 'Transação', @CODMENUPAI, 'Tesouraria', 'Transacao', 'Index', 2, 1, NULL, 'S','N'
    END
    
    ------------------------------------------------------------CONCILIACAO BANCARIA-------------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnTesouraria'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuConciliacaoBancaria', 'Conciliação Bancária', @CODMENUPAI, 'Tesouraria', 'ConciliacaoBancaria', 'Index', 2, 2, NULL, 'S','N'
    END
        
    ------------------------------------------------------------EXTRATO BANCÁRIO-------------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnTesouraria'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuExtratoBancario', 'Extrato Bancario', @CODMENUPAI, 'Tesouraria', 'ExtratoBancario', 'Index', 2, 3, NULL, 'S','N'
    END
    
    ---------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------MENU DE RELATÓRIOS-----------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuRelatorio', 'Relatórios', null, null, null, null, 1, 6, 'fa fa-newspaper-o', 'S','N'
    END
    ---------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------MENU DE AJUDA-----------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuAjuda', 'Ajuda', null, null, null, null, 1, 7, 'fa fa-search', 'S','N'
    END
    ------------------------------------------------------------TREINAMENTO-------------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuAjuda'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuTreinamento', 'Treinamento', @CODMENUPAI, 'Ajuda', 'Treinamento', 'Index', 2, 1, NULL, 'S','N'
    END
    ------------------------------------------------------------CONTATO-------------------------------------------------
    BEGIN
        SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
        SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuAjuda'

        INSERT INTO PAGNET_MENU
            SELECT @CODMENU, 'mnuContato', 'Contato', @CODMENUPAI, 'Ajuda', 'Contato', 'Index', 2, 2, NULL, 'S','N'
    END
     
    ---------------------------------------------------------------------------------------------------------------------------------------------
    ----------------------------------------------------------MENU DE INDICADORES----------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------
    --
    --BEGINE
    --    DECLARE @CODMENU INT, @CODMENUPAI INT
    --    SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENUE

    ----
    --    INSERT INTO PAGNET_MENU
    --        SELECT @CODMENU, 'mnuIndicador', 'Indicadores', null, null, null, null, 1, 5, 'fa fa-bar-chart-o', 'N','N'
    --END
    --
    --BEGIN
    --    DECLARE @CODMENU INT, @CODMENUPAI INT
    --    SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
    --    SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuIndicador'

    ----
    --    INSERT INTO PAGNET_MENUE
    --        SELECT @CODMENU, 'mnuIndicPagRealizados', 'Pagamentos Realizados', @CODMENUPAI, 'Indicadores', 'PagamentosRealizados', 'Index', 2, 1, null, 'S','N'
    --END
    --
    --BEGIN
    --    DECLARE @CODMENU INT, @CODMENUPAI INT
    --    SELECT @CODMENU = ISNULL(MAX(CODMENU), 0) + 1 FROM PAGNET_MENU
    --    SELECT @CODMENUPAI = CODMENU FROM PAGNET_MENU WHERE NOME = 'mnuIndicador'

    ----
    --    INSERT INTO PAGNET_MENU
    --        SELECT @CODMENU, 'mnuIndicReceitaDespesa', 'Previsão Receitas X Despesas', @CODMENUPAI, 'Indicadores', 'ReceitaDespesa', 'Index', 2, 2, null, 'S','N'
    --END
    --

