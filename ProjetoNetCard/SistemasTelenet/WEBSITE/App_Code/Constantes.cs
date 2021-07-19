public static class Constantes
{

    #region Acoes Cadastro
    
    public enum AcaoCadastro
    {acPESQUISANDO, acINSERINDO, acALTERANDO, acSALVANDO, acCANCELANDO, acEXCLUINDO, acVISUALIZANDO, acREPESQUISANDO, acIMPRIMINDO, acGERARESULT};
    public static string[] DescAcaoCadastro = {"Pesquisando", "Inserindo", "Alterando", "Salvando", "Cancelando", "Excluindo", "Visualizando", "Repesquisando", "Imprimindo"};
    public static int IdxAcaoPesquisar = 0;
    public static int IdxAcaoInserir = 1;
    public static int IdxAcaoAlterar = 2;
    public static int IdxAcaoSalvar = 3;
    public static int IdxAcaoCancelar = 4;
    public static int IdxAcaoExcluir = 5;
    public static int IdxAcaoVisualizar = 6;
    public static int IdxAcaoRepesquisar = 7;
    public static int IdxAcaoImprimir = 8;
    public static int IdxAcaoNavgPrimeiro = 9;
    public static int IdxAcaoNavAnterior = 10;
    public static int IdxAcaoNavegProximo = 11;
    public static int IdxAcaoNavegUltimo = 12;
    public static int IdxAcaoImpAfiliacao = 13;
    public static int IdxAcaoRepesquisarImpTaxaPJ = 14;
    public static int IdxAcaoAbaEspecialidades = 15;
    public static int IdxAcaoImpPrincipal = 16;
    public static int IdxAcaoInserirProd = 17;
    public static int IdxAcaoSelProd = 18;
    public static int IdxPainelUsuario = 19;
    public static int IdxAcaoInserirMesmoProd = 20;

    #endregion

    #region Scripts
    public static string FuncaoJSFormataMaiusculo = "Maiusculo(s)";
    public static string FuncaoJSFormataCNPJ = "MascaraCNPJ(s, e.htmlEvent);";
    public static string FuncaoJSFormataCPF = "MascaraCPF(s, e.htmlEvent);";
    public static string FuncaoJSFormataCEP = "MascaraCEP(s, e.htmlEvent);";
    public static string FuncaoJSFormataCel = "MascaraCel(s, e.htmlEvent)";
    public static string FuncaoJSFormataFone = "MascaraFone(s, e.htmlEvent)";
    public static string FuncaoJSFormataMoeda = "MascaraMoeda(s, e.htmlEvent)";
    public static string FuncaoJSFormataMoedaTamMax = "MascaraMoeda(s, e.htmlEvent, {0})";
    public static string FuncaoJSFormataNumero = "SomenteNumeros(s, e.htmlEvent)";
    public static string FuncaoJSFormataNumeroLetras = "SomenteLetrasNumeros(e.htmlEvent)";
    public static string FuncaoJSFormataLetras = "SomenteLetras(e.htmlEvent)";
    public static string FuncaoJSFormataData = "MascaraData(s, e.htmlEvent)";
    public static string FuncaoJSConfirmacao = "Confirmacao()";
    public static string FuncaoJSNotificarAlteracao = string.Format("try {{ window.btnSalvar.SetEnabled(true); }} catch (erro) {{ }} ;try {{ window.btnCancelar.SetEnabled(true); }} catch (erro) {{ }}");
    public static string FuncaoJSDesabilitarAlteracao = string.Format("try {{ window.btnSalvar.SetEnabled(false); }} catch (erro) {{ }} ;try {{ window.btnCancelar.SetEnabled(false); }} catch (erro) {{ }}");

    #endregion

    #region Mensagens
    public static string MsgCampoRequerido = "Favor informar campo";
    public static string MsgItemExcluido = "Item excluido com sucesso";
    public static string MsgSucessoAlteracao = "Dados alterados com sucesso.";
    
    #endregion

    #region ClienteVA
    public static char TipoTaxaPercentual = 'P';
    public static char TipoTaxaValor = 'V';
    public static string TituloPaginaCli = " :: Clientes";

    #endregion
    
    #region Outros
    
    public static string FormatacaoDecimal = "f2";
    public static string FormatacaoData = "dd/MM/yyyy";
    public static string FormatacaoDataHora = "dd/MM/yyyy hh:mm:ss";
    public static string FormatacaoPercentual = "P";
    public static int CodigoFinal = 999999;
    public static string[] DiasSemana = { "Domingo", "Segunda", "Terca", "Quarta", "Quinta", "Sexta", "Sabado" };
    public static string TrataDataNula = "01/01/0001";
    public static string ValorZero = "0";
    public static string Porcento = "%";
    public static string Separador = " - ";

    #endregion

    #region Credenciado

    public static string MatrizNaoEncontrado = "CREDENCIADO MATRIZ NAO ENCONTRADO";
    public static string CentralizadoraNaoEncontrado = "CREDENCIADO CENTRALIZADORA NAO ENCONTRADO";
    public static string PrincipalNaoEncontrado = "CREDENCIADO PRINCIPAL NÃO ENCONTRADO";
    public static string SegmentoDiferentePrincipal = "SEGTO. DEVE SER DIFERENTE DO PRINCIPAL";
    public static string TituloPaginaCre = " :: Credenciados";

    #endregion

    #region Usuario

    public static string TituloPaginaUsu = " :: Usuario";
    public static string FiltroTitDepen = "Tit/Depen";
    public static string FiltroNoDep = "No. Dep";
    public static string FiltroCliente = "Cliente";
    public static string FiltroCodCliente = "Cod. Cliente";
    public static string FiltroStatus = "Status";
    public static string ClienteNaoEncontrado = "CLIENTE NAO ENCONTRADO";
    public static string ClienteExistNoCliente = "CARTÃO EXISTENTE NO CLIENTE INFORMADO.";
    public static string DadosDependente = "DADOS DO DEPENDENTE";
    public static string Dependente = " DEPENDENTE  -  ";
    public static string Titular = " TITULAR  -  ";
    
    #endregion

    #region Relatorios
    public static string DataInicialRel = "01/01/2009";
    public static string DataFinalRel = "31/12/2099";
    #endregion

    #region Upload/Download
    public static string PathDownLoad = "~/App_Data/DownLoad";
    public static string PathUpLoad = "~/App_Data/UpLoad";
    public static string PathBin = "~/Bin";
    #endregion

    #region Títulos

    public static string tituloControleAcesso = " :: Controle Acesso";
    public static string tituloOperador = " :: Operador";
    public static string tituloConsultaTrans = " :: Consultas";
    public static string tituloProcessoImportacao = " :: Processo Importação";

    #endregion

    #region Tipo Transacao

    public static string TipoTransacao = "1";
    public static string TipoCliente = "2";
    public static string TipoCredenciado = "3";
    public static string TipoUsuarios = "4";
    public static string TipoListagens = "5";

    #endregion

}
