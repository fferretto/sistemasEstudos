using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using SIL;
using SIL.BL;
using SIL.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using TELENET.SIL;
using TELENET.SIL.BL;
using TELENET.SIL.PO;

public partial class SelConsTransacao : TelenetPage
{
    #region Constantes e Globais

    private const int IdxTabDois = 1;
    private const string ListaCredenciado = "ListaCredenciado";
    private const string ListaColunasNaoVisualizar = "ColunasNaoVisualizadas";
    private const string ListaClientes = "ListaClientes";
    private const string ListaSubRede = "ListaSubRede";
    private const string ListaRedeCaptura = "ListaRedeCaptura";
    private const string ListaTipoTransacao = "ListaTipoTransacao";
    private const string PesquisaInicial = "PesquisaInicial";
    private const string CodigoConsulta = "CodigoConsulta";
    private const string NomeConsulta = "NomeConsulta";
    private const string IsretornoNovaconsulta = "IsRetornoNovaConsulta";
    private bool temSubRede;

    #endregion

    #region Properties

    protected short Sistema { get { return (short)(prePago.Checked ? 1 : 0); } }

    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }

    private bool FlgTabDoisVisivel
    {
        get { return (bool)Session["FlgTabDoisVisivel"]; }
        set { Session["FlgTabDoisVisivel"] = value; }
    }

    #endregion

    #region Auxiliares

    private static void TratarVisual(ASPxTextBox textBox)
    {
        textBox.Text = "NAO ENCONTRADO";
        textBox.Border.BorderColor = Color.Red;
        textBox.BackColor = Color.Salmon;
    }

    private void RemoveSessionsSimples()
    {
        Session.Remove(ListaCredenciado);
        Session.Remove(ListaClientes);
        Session.Remove(CodigoConsulta);
        if (temSubRede) Session.Remove(ListaSubRede);
        Session.Remove(ListaTipoTransacao);
        Session.Remove(UtilSIL.ISRETORNORESULTCONSULTA);
        Session.Remove(IsretornoNovaconsulta);
        Session.Remove(ListaColunasNaoVisualizar);
    }

    private void RemoveSessionConsultaVa()
    {
        if (GetOperador() == null) return;
        Session.Remove(UtilSIL.DADOSPARACONSULTA);
    }

    private void ClearCampos() //Limpa todos os campos da tela
    {
        #region Periodo

        chbPeriodo.Checked = false;
        txtPeriodoIni.Text = string.Empty;
        txtPeriodoFim.Text = string.Empty;
        txtPeriodoHoraIni.Text = string.Empty;
        txtPeriodoHoraFim.Text = string.Empty;
        txtPeriodoMinIni.Text = string.Empty;
        txtPeriodoMinFim.Text = string.Empty;

        #endregion

        #region Num Host

        chbNumHost.Checked = false;
        txtHostIni.Text = string.Empty;
        txtHostFim.Text = string.Empty;

        #endregion

        #region Num Autorizacao

        chbNumAutorizacao.Checked = false;
        txtAutIni.Text = string.Empty;
        txtAutFim.Text = string.Empty;

        #endregion

        #region Tipo Usuario

        chbTipoUsuario.Checked = false;
        rdbUsuarioNome.Checked = false;
        rdbUsuarioCPF.Checked = false;
        rdbUsuarioMatricula.Checked = false;
        txtUsuarioNome.Text = string.Empty;
        txtUsuarioCPF.Text = string.Empty;
        txtUsuarioMatricula.Text = string.Empty;

        #endregion

        #region SubRede

        if (temSubRede)
        {
            chbSubRede.Checked = false;
            chbTodasSubRedes.Enabled = false;
            chbListaSubRede.Selection.UnselectAll();
            chbListaSubRede.Enabled = false;
            Session.Remove(ListaSubRede);
        }

        #endregion

        #region Redes captura

        chbRedeCaptura.Checked = false;
        chbTodasRedeCaptura.Enabled = false;
        chbListaRedeCaptura.Selection.UnselectAll();
        chbListaRedeCaptura.Enabled = false;
        Session.Remove(ListaRedeCaptura);

        #endregion

        #region Tipo Transacao

        chbTipoTrans.Checked = false;
        chbTodasTrans.Enabled = false;
        chbListaTipoTrans.Selection.UnselectAll();
        chbListaTipoTrans.Enabled = false;
        Session.Remove(ListaTipoTransacao);

        #endregion

        #region Tipo Resposta

        chbTipoResposta.Checked = false;
        chbTodosTipResp.Enabled = false;
        chbListTipoResp.Selection.UnselectAll();
        chbListTipoResp.Enabled = false;

        #endregion

        #region Numero Cartao

        chbCartao.Checked = false;
        txtCartao.Text = string.Empty;

        #endregion

        #region Tipo Credenciado

        chbTipoCredenciado.Checked = false;
        grvListaFiltroCredenciados.DataSource = null;
        grvListaFiltroCredenciados.DataBind();
        chbIntervaloCre.Checked = false;
        txtInterCredIni.Text = string.Empty;
        txtInterCredFim.Text = string.Empty;
        txtCodCreFiltro.Text = string.Empty;
        txtCodCreFiltro.ClientEnabled = false;
        txtRazSoc.Text = string.Empty;

        #endregion

        #region Tipo Cliente

        chbTipoCliente.Checked = false;
        grvListaFiltroClientes.DataSource = null;
        grvListaFiltroClientes.DataBind();
        chbIntervaloCli.Checked = false;
        txtInterCliIni.Text = string.Empty;
        txtInterCliFim.Text = string.Empty;
        txtCodCliFiltro.Text = string.Empty;
        txtNomeClienteFiltro.Text = string.Empty;
        txtCodCliFiltro.ClientEnabled = false;

        #endregion

        #region Tipo Fechamento

        chbTipoFechCred.Checked = false;
        rdbEspecicarData.Checked = false;
        rdbEspecicarNum.Checked = false;
        txtEspecDataIni.Text = string.Empty;
        txtEspecDataFim.Text = string.Empty;
        txtEspecNumIni.Text = string.Empty;
        txtEspecNumFim.Text = string.Empty;

        chbTipoFechCliente.Checked = false;
        rdbEspecicarDataCliente.Checked = false;
        rdbEspecicarNumCliente.Checked = false;
        txtEspecDataIniCliente.Text = string.Empty;
        txtEspecDataFimCliente.Text = string.Empty;
        txtEspecNumIniCliente.Text = string.Empty;
        txtEspecNumFimCliente.Text = string.Empty;

        #endregion
    }

    #endregion

    #region Conversões para Junção.

    private bool ExibeSubrede()
    {
        return new blCLIENTENovo(GetOperador()).ExibeSubRede();
    }

    private IList<IDadosBasicosCliente> ColecaoClientes(IFilter filtro, int codAg)
    {
        if (codAg == 0)
        {
            return new blCLIENTENovo(GetOperador()).ColecaoClientes(new ANDFilter(filtro, new Filter("SISTEMA", SqlOperators.Equal, Sistema.ToString()))).OfType<IDadosBasicosCliente>().ToList();
        }

        return new blCLIENTENovo(GetOperador()).ColecaoClientes(filtro, codAg).OfType<IDadosBasicosCliente>().ToList();
    }

    private IList<IDadosBasicosCliente> ColecaoClientes(IFilter filtro)
    {
        return ColecaoClientes(filtro, 0);
    }

    #endregion

    #region Init

    protected void Page_Load(object sender, EventArgs e)
    {
        //Session.LoadSession();

        if (!IsPostBack)
        {
            prePago.Checked = posPago.Checked = false;
        }

        if (GetOperador() == null)
        {
            return;
        }

        temSubRede = ExibeSubrede();

        divFiltroSubrede.Visible = temSubRede;
        //divCartao.Attributes.Add("style", temSubRede 
        //    ? "float: left; width: 49%; border: 1px solid LightGray;" 
        //    : "float: left; width: 98%; border: 1px solid LightGray;");

        Title = GetOperador().NOMEOPERADORA + " :: Consultas";
        Page.ClientScript.RegisterClientScriptInclude("Consultas", Page.ResolveVersionScript("Consultas.js"));
        AplicarFuncoesScripts();

        if (Session[UtilSIL.ISRETORNORESULTCONSULTA] != null && (bool)Session[UtilSIL.ISRETORNORESULTCONSULTA])
        {
            TrataRetornoTelaResult();
        }
        else
        {
            rplMainTrasacoes.ClientVisible = false;

            if (!IsPostBack)
            {
                btnExcluir.ClientEnabled = false;
                RemoveSessionsSimples();
                RemoveSessionConsultaVa();
            }
        }
    }

    private void TrataRetornoTelaResult()
    {
        if (GetOperador() == null)
        {
            return;
        }

        rplMainTrasacoes.ClientVisible = true;
        pnlPesquisaTrans.ClientVisible = false;
        btnSalvaConsulta.ClientEnabled = true;
        btnVoltar.ClientEnabled = true;
        btnResultado.ClientEnabled = true;
        Session.Remove(UtilSIL.ISRETORNORESULTCONSULTA);

        if (Session[UtilSIL.DADOSPARACONSULTA] == null)
        {
            return;
        }

        tabDadosTrans.ActiveTabIndex = 0;

        var consulta = (CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA];

        if (consulta.CODCONS > 0)
        {
            Session[CodigoConsulta] = consulta.CODCONS;
        }
        else
        {
            Master.AcaoCadastro = Constantes.AcaoCadastro.acINSERINDO;
            Session[IsretornoNovaconsulta] = true;
        }

        Session[ListaColunasNaoVisualizar] = consulta.LISTA_COL;
        FlgTabDoisVisivel = false;
        InitDadosPreGravadosTabUm(consulta);
        PreencherTelaFiltro(consulta);

        System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "VoltarFiltro", "configurarElementosCartao();", true);
    }

    protected void AplicarFuncoesScripts()
    {
        #region Script

        txtPesquisaTransacao.ClientSideEvents.GotFocus = Util.IncluirFuncaoJS(txtPesquisaTransacao.ClientSideEvents.GotFocus, "nextfield = 'done';");

        //Scripts para inicializar controles da opcao USUARIO 
        chbTipoUsuario.Attributes.Add("onclick", string.Format(@"habilitaControlesUsu('{0}')", chbTipoUsuario.ClientID));
        rdbUsuarioNome.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoUsuNome(s);}";
        rdbUsuarioCPF.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoUsuCPF(s);}";
        rdbUsuarioMatricula.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoUsuMat(s);}";
        txtPeriodoIni.ClientSideEvents.KeyPress = Util.IncluirFuncaoJS(txtPeriodoIni.ClientSideEvents.KeyPress, Constantes.FuncaoJSFormataData);
        txtPeriodoFim.ClientSideEvents.KeyPress = Util.IncluirFuncaoJS(txtPeriodoFim.ClientSideEvents.KeyPress, Constantes.FuncaoJSFormataData);
        txtEspecDataIni.ClientSideEvents.KeyPress = Util.IncluirFuncaoJS(txtEspecDataIni.ClientSideEvents.KeyPress, Constantes.FuncaoJSFormataData);
        txtEspecDataFim.ClientSideEvents.KeyPress = Util.IncluirFuncaoJS(txtEspecDataFim.ClientSideEvents.KeyPress, Constantes.FuncaoJSFormataData);
        txtEspecDataIniCliente.ClientSideEvents.KeyPress = Util.IncluirFuncaoJS(txtEspecDataIniCliente.ClientSideEvents.KeyPress, Constantes.FuncaoJSFormataData);
        txtEspecDataFimCliente.ClientSideEvents.KeyPress = Util.IncluirFuncaoJS(txtEspecDataFimCliente.ClientSideEvents.KeyPress, Constantes.FuncaoJSFormataData);

        //Scripts para tratar estado dos controles para transacoes
        chbPeriodo.Attributes.Add("onclick", string.Format(@"habilitaControlesPer('{0}')", chbPeriodo.ClientID));
        chbNumHost.Attributes.Add("onclick", string.Format(@"habilitaControlesHost('{0}')", chbNumHost.ClientID));
        chbNumAutorizacao.Attributes.Add("onclick", string.Format(@"habilitaControlesAut('{0}')", chbNumAutorizacao.ClientID));
        chbCartao.Attributes.Add("onclick", string.Format(@"habilitaControlesCart('{0}')", chbCartao.ClientID));

        //Scripts para o tipo de fechamento credenciado
        chbTipoFechCred.Attributes.Add("onclick", string.Format(@"habilitaControlesFechCred('{0}')", chbTipoFechCred.ClientID));
        rdbEspecicarNum.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoFechCredNum(s);}";
        rdbEspecicarData.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoFechCredData(s);}";
        txtEspecNumIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtEspecNumFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";

        //Scripts para o tipo de fechamento cliente
        chbTipoFechCliente.Attributes.Add("onclick", string.Format(@"habilitaControlesFechCliente('{0}')", chbTipoFechCliente.ClientID));
        rdbEspecicarNumCliente.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoFechClienteNum(s);}";
        rdbEspecicarDataCliente.ClientSideEvents.CheckedChanged = "function(s, e) {habilitaControleInternoFechClienteData(s);}";
        txtEspecNumIniCliente.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtEspecNumFimCliente.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";

        txtPeriodoHoraFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNum(s, e.htmlEvent);}";
        txtPeriodoMinFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNum(s, e.htmlEvent);}";
        txtPeriodoHoraIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNum(s, e.htmlEvent);}";
        txtPeriodoMinIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNum(s, e.htmlEvent);}";

        txtPeriodoHoraFim.ClientSideEvents.LostFocus = "function(s, e) {ConsisteHora(s);}";
        txtPeriodoMinFim.ClientSideEvents.LostFocus = "function(s, e) {ConsisteHora(s);}";
        txtPeriodoHoraIni.ClientSideEvents.LostFocus = "function(s, e) {ConsisteHora(s);}";
        txtPeriodoMinIni.ClientSideEvents.LostFocus = "function(s, e) {ConsisteHora(s);}";
        txtHostIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtHostFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtAutIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtAutFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        chbIntervaloCre.Attributes.Add("onclick", string.Format(@"habilitaControlesIntervaloCre('{0}')", chbIntervaloCre.ClientID));
        chbIntervaloCentralizadora.Attributes.Add("onclick", string.Format(@"habilitaControlesIntervaloCentralizadora('{0}')", chbIntervaloCentralizadora.ClientID));
        chbIntervaloCli.Attributes.Add("onclick", string.Format(@"habilitaControlesIntervaloCli('{0}')", chbIntervaloCli.ClientID));
        txtInterCliIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtInterCliFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtInterCredIni.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtInterCredFim.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";

        txtPeriodoIni.ClientSideEvents.LostFocus = Util.IncluirFuncaoJS(txtPeriodoIni.ClientSideEvents.LostFocus, "ValidaData(s, e)");
        txtPeriodoFim.ClientSideEvents.LostFocus = "function(s, e) {ConsisteDataPeriodo(s, txtPeriodoIni);}";

        txtEspecDataIni.ClientSideEvents.LostFocus = Util.IncluirFuncaoJS(txtEspecDataIni.ClientSideEvents.LostFocus, "ValidaData(s, e)");
        txtEspecDataFim.ClientSideEvents.LostFocus = "function(s, e) {ConsisteDataPeriodo(s, txtEspecDataIni);}";

        #endregion
    }

    protected void CriarInstanciaConsulta(object sender, ObjectDataSourceEventArgs e)
    {
        if (GetOperador() == null) return;
        e.ObjectInstance = new blConsultaVA(GetOperador());
    }

    #endregion

    #region Acoes

    private CONSULTA_VA RecuperaConsulta(int codConsulta)
    {
        if (GetOperador() == null) return null;
        CONSULTA_VA cons = new blConsultaVA(GetOperador(), Sistema).RecuperarConsultaVA(codConsulta);
        return cons;
    }

    private void InitDadosPreGravadosTabUm(CONSULTA_VA consultaVA) //Inicia os dados da tab1 a partir do banco de dados
    {
        if (GetOperador() == null) return;
        if (consultaVA == null)
            return;

        #region Periodo

        //Consulta com periodo 
        if ((consultaVA.PERIODO_INI != DateTime.MinValue && consultaVA.PERIODO_FIM != DateTime.MaxValue) &&
             (consultaVA.PERIODO_INI != DateTime.MaxValue && consultaVA.PERIODO_FIM != DateTime.MinValue))
        {
            //Habilito todos controles de periodo e marco o check box Periodo em caso de periodo fornecido
            chbPeriodo.Checked = true;
            txtPeriodoIni.ClientEnabled = true;
            txtPeriodoHoraIni.ClientEnabled = true;
            txtPeriodoFim.ClientEnabled = true;
            txtPeriodoHoraFim.ClientEnabled = true;
            txtPeriodoMinFim.ClientEnabled = true;
            txtPeriodoMinIni.ClientEnabled = true;
        }

        // DateTime DataPeriodoIni, DataPeriodoFim;
        if (consultaVA.PERIODO_INI != DateTime.MinValue && consultaVA.PERIODO_INI != DateTime.MaxValue)
        {
            //Monto a Data  
            txtPeriodoIni.Text = consultaVA.PERIODO_INI.ToShortDateString();
        }

        //Monto o tempo caso exista
        if (!string.IsNullOrEmpty(consultaVA.HORA_PERIODO_INI))
        {
            txtPeriodoHoraIni.Text = consultaVA.HORA_PERIODO_INI.Substring(0, 2);
            txtPeriodoMinIni.Text = consultaVA.HORA_PERIODO_INI.Substring(3, 2);
        }


        if (consultaVA.PERIODO_FIM != DateTime.MinValue && consultaVA.PERIODO_FIM != DateTime.MaxValue)
        {
            txtPeriodoFim.Text = consultaVA.PERIODO_FIM.ToShortDateString();
        }

        //Monto o tempo caso exista
        if (!string.IsNullOrEmpty(consultaVA.HORA_PERIODO_FIM))
        {
            txtPeriodoHoraFim.Text = consultaVA.HORA_PERIODO_FIM.Substring(0, 2);
            txtPeriodoMinFim.Text = consultaVA.HORA_PERIODO_FIM.Substring(3, 2);
        }

        #endregion

        #region Num Host

        if (consultaVA.NUM_HOST_INI > 0 || consultaVA.NUM_HOST_FIM > 0)
        {
            chbNumHost.Checked = true;
            txtHostIni.ClientEnabled = true;
            txtHostFim.ClientEnabled = true;
        }

        txtHostIni.Text = (consultaVA.NUM_HOST_INI > 0) ? consultaVA.NUM_HOST_INI.ToString() : string.Empty;
        txtHostFim.Text = (consultaVA.NUM_HOST_FIM > 0) ? consultaVA.NUM_HOST_FIM.ToString() : string.Empty;

        #endregion

        #region Num Autorizacao

        if (consultaVA.NUM_AUT_INI > 0 || consultaVA.NUM_AUT_FIM > 0)
        {
            chbNumAutorizacao.Checked = true;
            txtAutIni.ClientEnabled = true;
            txtAutFim.ClientEnabled = true;
        }

        txtAutIni.Text = (consultaVA.NUM_AUT_INI > 0) ? consultaVA.NUM_AUT_INI.ToString() : string.Empty;
        txtAutFim.Text = (consultaVA.NUM_AUT_FIM > 0) ? consultaVA.NUM_AUT_FIM.ToString() : string.Empty;

        #endregion

        #region Usuario

        if (!string.IsNullOrEmpty(consultaVA.NOME_USUARIO) ||
            !string.IsNullOrEmpty(consultaVA.MAT_USUARIO) ||
            !string.IsNullOrEmpty(consultaVA.CPF_USUARIO))
        {
            chbTipoUsuario.Checked = true;
            rdbUsuarioNome.ClientEnabled = true;
            rdbUsuarioCPF.ClientEnabled = true;
            rdbUsuarioMatricula.ClientEnabled = true;
        }

        if (!string.IsNullOrEmpty(consultaVA.NOME_USUARIO))
        {
            rdbUsuarioNome.Checked = true;
            txtUsuarioNome.ClientEnabled = true;
            txtUsuarioNome.Text = consultaVA.NOME_USUARIO;
        }

        if (!string.IsNullOrEmpty(consultaVA.CPF_USUARIO))
        {
            rdbUsuarioCPF.Checked = true;
            txtUsuarioCPF.ClientEnabled = true;
            txtUsuarioCPF.Text = consultaVA.CPF_USUARIO;
        }


        if (!string.IsNullOrEmpty(consultaVA.MAT_USUARIO))
        {
            rdbUsuarioMatricula.Checked = true;
            txtUsuarioMatricula.ClientEnabled = true;
            txtUsuarioMatricula.Text = consultaVA.MAT_USUARIO;
        }

        #endregion

        #region SubRede

        if (temSubRede)
        {
            if (!string.IsNullOrEmpty(consultaVA.SUBREDE))
            {
                chbSubRede.Checked = true;
                chbListaSubRede.Enabled = true;
                chbListaSubRede.Selection.UnselectAll();

                if (Session[ListaSubRede] == null)
                {
                    Session[ListaSubRede] = new blTransacao(GetOperador()).ListaSubRede();
                    chbListaSubRede.DataSource = Session[ListaSubRede];
                    chbListaSubRede.DataBind();
                }
                else if (Session[ListaSubRede] != null)
                {
                    chbListaSubRede.Enabled = chbSubRede.Checked;
                    chbListaSubRede.DataSource = Session[ListaSubRede];
                    chbListaSubRede.DataBind();
                }

                string[] subRede = consultaVA.SUBREDE.Split(',');
                int subrede;
                foreach (string sRede in subRede)
                {
                    if (!string.IsNullOrEmpty(sRede))
                    {
                        subrede = Convert.ToInt32(sRede);
                        chbListaSubRede.Selection.SetSelectionByKey(subrede, true);
                    }
                }
            }
            else
            {
                chbListaSubRede.Enabled = false;
            }
        }

        #endregion

        #region Tipo Transacao

        if (!string.IsNullOrEmpty(consultaVA.TIPO_TRANSACAO))
        {
            chbTipoTrans.Checked = true;
            chbListaTipoTrans.Enabled = true;
            chbListaTipoTrans.Selection.UnselectAll();

            if (Session[ListaTipoTransacao] == null)
            {
                Session[ListaTipoTransacao] = new blTransacao(GetOperador()).ListaTipoTrans();
                chbListaTipoTrans.DataSource = Session[ListaTipoTransacao];
                chbListaTipoTrans.DataBind();
            }
            else
                if (Session[ListaTipoTransacao] != null)
            {
                chbListaTipoTrans.Enabled = chbTipoTrans.Checked;
                chbListaTipoTrans.DataSource = Session[ListaTipoTransacao];
                chbListaTipoTrans.DataBind();
            }

            string[] tipoTransacao = consultaVA.TIPO_TRANSACAO.Split(',');
            int transacao;
            foreach (string trans in tipoTransacao)
            {
                if (!string.IsNullOrEmpty(trans))
                {
                    transacao = Convert.ToInt32(trans);
                    chbListaTipoTrans.Selection.SetSelectionByKey(transacao, true);
                }
            }
        }
        else
        {
            chbListaTipoTrans.Enabled = false;
        }

        #endregion

        #region Tipo Resposta

        if (!string.IsNullOrEmpty(consultaVA.TIPO_RESPOSTA))
        {
            chbTipoResposta.Checked = true;
            chbListTipoResp.Enabled = true;
            chbListTipoResp.Selection.UnselectAll();
            chbListTipoResp.DataSource = new blConsultaVA(GetOperador()).InitColectionTipoResp();
            chbListTipoResp.DataBind();
            string[] tipoResposta = consultaVA.TIPO_RESPOSTA.Split(',');

            foreach (string resp in tipoResposta)
            {
                char[] ch = resp.ToCharArray(); // retorna um char

                if (resp == "'PP'")
                    chbListTipoResp.Selection.SetSelectionByKey('X', true);
                else
                {
                    if (!string.IsNullOrEmpty(resp))
                        chbListTipoResp.Selection.SetSelectionByKey(ch[1], true);
                }
            }
        }
        else
            chbListTipoResp.Enabled = false;

        #endregion

        #region Numero Cartao

        if (!string.IsNullOrEmpty(consultaVA.NUM_CARTAO))
        {
            chbCartao.Checked = true;
            txtCartao.ClientEnabled = true;
            txtCartao.Text = consultaVA.NUM_CARTAO;
        }

        #endregion

        hdAlteraConsulta.Value = consultaVA.NOME_CONSULTA;
    }

    private void InitDadosPreGravadosTabDois(CONSULTA_VA consulta) //Inicia os dados da tab2 a partir do banco de dados
    {
        if (GetOperador() == null) return;
        if (consulta == null)
            return;

        FlgTabDoisVisivel = true;

        #region Fechamento Credenciado

        if (consulta.DATA_FECH_CRED_INI != DateTime.MinValue || consulta.DATA_FECH_CRED_FIM != DateTime.MinValue)
        {
            chbTipoFechCred.Checked = true;
            rdbEspecicarData.Checked = true;
            rdbEspecicarData.ClientEnabled = true;
            txtEspecDataIni.ClientEnabled = true;
            txtEspecDataFim.ClientEnabled = true;
            txtEspecDataIni.Text = consulta.DATA_FECH_CRED_INI.ToShortDateString();
            txtEspecDataFim.Text = consulta.DATA_FECH_CRED_FIM.ToShortDateString();
        }

        if (consulta.NUM_FECH_CRED_INI > 0 || consulta.NUM_FECH_CRED_FIM > 0)
        {
            chbTipoFechCred.Checked = true;
            rdbEspecicarNum.Checked = true;
            rdbEspecicarNum.ClientEnabled = true;
            txtEspecNumIni.ClientEnabled = true;
            txtEspecNumFim.ClientEnabled = true;
            txtEspecNumIni.Text = consulta.NUM_FECH_CRED_INI > 0 ? consulta.NUM_FECH_CRED_INI.ToString() : "";
            txtEspecNumFim.Text = consulta.NUM_FECH_CRED_FIM > 0 ? consulta.NUM_FECH_CRED_FIM.ToString() : "";
        }

        #endregion

        #region Fechamento Cliente

        if (consulta.DATA_FECH_CLI_INI != DateTime.MinValue || consulta.DATA_FECH_CLI_FIM != DateTime.MinValue)
        {
            chbTipoFechCliente.Checked = true;
            rdbEspecicarDataCliente.Checked = true;
            rdbEspecicarDataCliente.ClientEnabled = true;
            txtEspecDataIniCliente.ClientEnabled = true;
            txtEspecDataFimCliente.ClientEnabled = true;
            txtEspecDataIniCliente.Text = consulta.DATA_FECH_CLI_INI.ToShortDateString();
            txtEspecDataFimCliente.Text = consulta.DATA_FECH_CLI_FIM.ToShortDateString();
        }

        if (consulta.NUM_FECH_CLI_INI > 0 || consulta.NUM_FECH_CLI_FIM > 0)
        {
            chbTipoFechCliente.Checked = true;
            rdbEspecicarNumCliente.Checked = true;
            rdbEspecicarNumCliente.ClientEnabled = true;
            txtEspecNumIniCliente.ClientEnabled = true;
            txtEspecNumFimCliente.ClientEnabled = true;
            txtEspecNumIniCliente.Text = consulta.NUM_FECH_CLI_INI > 0 ? consulta.NUM_FECH_CLI_INI.ToString() : "";
            txtEspecNumFimCliente.Text = consulta.NUM_FECH_CLI_FIM > 0 ? consulta.NUM_FECH_CLI_FIM.ToString() : "";
        }

        #endregion

        #region Intervalo Clientes

        if (!string.IsNullOrEmpty(consulta.INTERVALO_CLI_INI) && !string.IsNullOrEmpty(consulta.INTERVALO_CLI_FIM))
        {
            chbIntervaloCli.Checked = true;
            txtInterCliIni.Text = consulta.INTERVALO_CLI_INI;
            txtInterCliFim.Text = consulta.INTERVALO_CLI_FIM;
            txtInterCliIni.ClientEnabled = true;
            txtInterCliFim.ClientEnabled = true;
        }

        #endregion

        #region Intervalo de Credenciados

        if (!string.IsNullOrEmpty(consulta.INTERVALO_CRED_INI) && !string.IsNullOrEmpty(consulta.INTERVALO_CRED_FIM))
        {
            chbIntervaloCre.Checked = true;
            txtInterCredIni.Text = consulta.INTERVALO_CRED_INI;
            txtInterCredFim.Text = consulta.INTERVALO_CRED_FIM;
            txtInterCredIni.ClientEnabled = true;
            txtInterCredFim.ClientEnabled = true;
        }

        #endregion

        #region Lista Credenciado

        if (!string.IsNullOrEmpty(consulta.LISTA_CRED))
        {
            chbTipoCredenciado.Checked = true;
            grvListaFiltroCredenciados.PageIndex = 1;
            txtCodCreFiltro.ClientEnabled = true;

            System.Collections.Specialized.StringCollection colection = new System.Collections.Specialized.StringCollection();
            string[] credenciados = consulta.LISTA_CRED.Split(',');
            foreach (string cred in credenciados)
                if (!string.IsNullOrEmpty(cred))
                    colection.Add(cred);

            var filtro = new INFilter("CODCRE", colection);
            Session[ListaCredenciado] = new blCredenciadoVA(GetOperador()).ColecaoCredenciados(filtro);
            grvListaFiltroCredenciados.DataSource = Session[ListaCredenciado];
            grvListaFiltroCredenciados.DataBind();
        }
        else
        {
            Session.Remove(ListaCredenciado);
            txtCodCreFiltro.ClientEnabled = false;
        }

        #endregion

        #region Lista Cliente

        if (!string.IsNullOrEmpty(consulta.LISTA_CLI))
        {
            chbTipoCliente.Checked = true;
            grvListaFiltroClientes.PageIndex = 1;
            txtCodCliFiltro.ClientEnabled = true;

            System.Collections.Specialized.StringCollection colection = new System.Collections.Specialized.StringCollection();
            string[] clientes = consulta.LISTA_CLI.Split(',');
            foreach (string cli in clientes)
                if (!string.IsNullOrEmpty(cli))
                    colection.Add(cli);

            var filtro = new INFilter("CODCLI", colection);
            Session[ListaClientes] = ColecaoClientes(filtro);
            grvListaFiltroClientes.DataSource = Session[ListaClientes];
            grvListaFiltroClientes.DataBind();
        }
        else
        {
            txtCodCliFiltro.ClientEnabled = false;
            Session.Remove(ListaClientes);
        }

        #endregion
    }

    private void PreencherGridSelecao<TItem>(ASPxGridView grid, ASPxCheckBox checkSelecao, ASPxCheckBox checkTodas, string codigos, string sessionKey, Func<List<TItem>> getList)
    {
        //checkSelecao.ClientEnabled = checkTodas.Enabled = grid.Enabled = !string.IsNullOrEmpty(codigos);

        if (!string.IsNullOrEmpty(sessionKey))
        {
            if (Session[sessionKey] == null)
            {
                Session[sessionKey] = getList();
            }
        }

        grid.DataSource = string.IsNullOrEmpty(sessionKey) ? getList() : Session[sessionKey];
        grid.DataBind();
    }

    private void PreencherTelaFiltro(CONSULTA_VA consultaVA)
    {
        if (consultaVA == null)
        {
            return;
        }

        ClearCampos();

        posPago.Checked = consultaVA.SISTEMA == 0;
        prePago.Checked = consultaVA.SISTEMA == 1;

        txtPeriodoIni.Text = consultaVA.PERIODO_INI != DateTime.MinValue ? consultaVA.PERIODO_INI.ToString("dd/MM/yyyy") : null;
        txtPeriodoFim.Text = consultaVA.PERIODO_FIM != DateTime.MaxValue ? consultaVA.PERIODO_FIM.ToString("dd/MM/yyyy") : null;
        txtPeriodoHoraIni.Text = consultaVA.HORA_PERIODO_INI;
        txtPeriodoHoraFim.Text = consultaVA.HORA_PERIODO_FIM;
        chbPeriodo.Checked = consultaVA.PERIODO_INI != DateTime.MinValue || consultaVA.PERIODO_FIM != DateTime.MaxValue
            || !string.IsNullOrEmpty(consultaVA.HORA_PERIODO_INI) || !string.IsNullOrEmpty(consultaVA.HORA_PERIODO_FIM);
        txtPeriodoIni.ClientEnabled = txtPeriodoFim.ClientEnabled = txtPeriodoHoraIni.ClientEnabled = txtPeriodoHoraFim.ClientEnabled = chbPeriodo.Checked;

        txtHostIni.Text = consultaVA.NUM_HOST_INI > 0 ? consultaVA.NUM_HOST_INI.ToString() : null;
        txtHostFim.Text = consultaVA.NUM_HOST_FIM > 0 ? consultaVA.NUM_HOST_FIM.ToString() : null;
        chbNumHost.Checked = consultaVA.NUM_HOST_INI > 0 || consultaVA.NUM_HOST_FIM > 0;
        txtHostIni.ClientEnabled = txtHostFim.ClientEnabled = chbNumHost.Checked;

        txtAutIni.Text = consultaVA.NUM_AUT_INI > 0 ? consultaVA.NUM_AUT_INI.ToString() : null;
        txtAutFim.Text = consultaVA.NUM_AUT_FIM > 0 ? consultaVA.NUM_AUT_FIM.ToString() : null;
        chbNumAutorizacao.Checked = consultaVA.NUM_AUT_INI > 0 || consultaVA.NUM_AUT_FIM > 0;
        txtAutIni.ClientEnabled = txtAutFim.ClientEnabled = chbNumAutorizacao.Checked;

        txtUsuarioNome.Text = consultaVA.NOME_USUARIO;
        txtUsuarioCPF.Text = consultaVA.CPF_USUARIO;
        txtUsuarioMatricula.Text = consultaVA.MAT_USUARIO;
        chbTipoUsuario.Checked = !string.IsNullOrEmpty(consultaVA.NOME_USUARIO) || !string.IsNullOrEmpty(consultaVA.CPF_USUARIO) || !string.IsNullOrEmpty(consultaVA.MAT_USUARIO);
        txtUsuarioNome.ClientEnabled = txtUsuarioCPF.ClientEnabled = txtUsuarioMatricula.ClientEnabled = chbTipoUsuario.Checked;

        if (temSubRede)
        {
            PreencherGridSelecao<SUBREDE>(chbListaSubRede, chbSubRede, chbTodasSubRedes, consultaVA.SUBREDE, ListaSubRede, () => { return new blTransacao(GetOperador()).ListaSubRede(); });
        }

        PreencherGridSelecao<REDECAPTURA>(chbListaRedeCaptura, chbRedeCaptura, chbTodasRedeCaptura, consultaVA.REDE, ListaRedeCaptura, () => { return new blTransacao(GetOperador()).ListaRedeCaptura(); });
        PreencherGridSelecao<TIPTRANS>(chbListaTipoTrans, chbTipoTrans, chbTodasTrans, consultaVA.TIPO_TRANSACAO, ListaTipoTransacao, () => { return new blTransacao(GetOperador()).ListaTipoTrans(); });
        PreencherGridSelecao<TipoResposta>(chbListTipoResp, chbTipoResposta, chbTodosTipResp, consultaVA.TIPO_RESPOSTA, null, () => { return new blConsultaVA(GetOperador()).InitColectionTipoResp(); });

        txtCartao.Text = consultaVA.NUM_CARTAO;
        chbCartao.Checked = !string.IsNullOrEmpty(consultaVA.NUM_CARTAO);
        txtCartao.ClientEnabled = chbCartao.Checked;

        chbTipoCliente.Checked = !string.IsNullOrEmpty(consultaVA.LISTA_CLI);

        txtInterCliIni.Text = consultaVA.INTERVALO_CLI_INI;
        txtInterCliFim.Text = consultaVA.INTERVALO_CLI_FIM;
        chbIntervaloCli.Checked = !string.IsNullOrEmpty(txtInterCliIni.Text) || !string.IsNullOrEmpty(txtInterCliFim.Text);
        txtInterCliIni.ClientEnabled = txtInterCliFim.ClientEnabled = chbIntervaloCli.Checked;

        chbTipoCredenciado.Checked = !string.IsNullOrEmpty(consultaVA.LISTA_CRED);

        txtInterCredIni.Text = consultaVA.INTERVALO_CRED_INI;
        txtInterCredFim.Text = consultaVA.INTERVALO_CRED_FIM;
        chbIntervaloCre.Checked = !string.IsNullOrEmpty(txtInterCredIni.Text) || !string.IsNullOrEmpty(txtInterCredFim.Text);
        txtInterCredIni.ClientEnabled = txtInterCredFim.ClientEnabled = chbIntervaloCre.Checked;

        txtInterCentralizadora.Text = consultaVA.CODCEN > 0 ? consultaVA.CODCEN.ToString() : null;
        chbIntervaloCentralizadora.Checked = consultaVA.CODCEN > 0;
        txtInterCentralizadora.ClientEnabled = chbIntervaloCentralizadora.Checked;

        txtEspecNumIni.Text = consultaVA.NUM_FECH_CRED_INI > 0 ? consultaVA.NUM_FECH_CRED_INI.ToString() : null;
        txtEspecNumFim.Text = consultaVA.NUM_FECH_CRED_FIM > 0 ? consultaVA.NUM_FECH_CRED_FIM.ToString() : null;
        rdbEspecicarNum.Checked = consultaVA.NUM_FECH_CRED_INI > 0 || consultaVA.NUM_FECH_CRED_FIM > 0;
        txtEspecNumIni.ClientEnabled = txtEspecNumFim.ClientEnabled = rdbEspecicarNum.Checked;

        txtEspecDataIni.Text = consultaVA.DATA_FECH_CRED_INI != DateTime.MinValue ? consultaVA.DATA_FECH_CRED_INI.ToString("dd/MM/yyyy") : null;
        txtEspecDataFim.Text = consultaVA.DATA_FECH_CRED_FIM != DateTime.MaxValue ? consultaVA.DATA_FECH_CRED_FIM.ToString("dd/MM/yyyy") : null;
        rdbEspecicarData.Checked = consultaVA.DATA_FECH_CRED_INI != DateTime.MinValue || consultaVA.DATA_FECH_CRED_FIM != DateTime.MaxValue;
        txtEspecDataIni.ClientEnabled = txtEspecDataFim.ClientEnabled = rdbEspecicarData.Checked;

        chbTipoFechCred.Checked = rdbEspecicarNum.Checked || rdbEspecicarData.Checked;

        txtEspecNumIniCliente.Text = consultaVA.NUM_FECH_CLI_INI > 0 ? consultaVA.NUM_FECH_CLI_INI.ToString() : null;
        txtEspecNumFimCliente.Text = consultaVA.NUM_FECH_CLI_FIM > 0 ? consultaVA.NUM_FECH_CLI_FIM.ToString() : null;
        rdbEspecicarNumCliente.Checked = consultaVA.NUM_FECH_CRED_INI > 0 || consultaVA.NUM_FECH_CRED_FIM > 0;
        txtEspecNumIniCliente.ClientEnabled = txtEspecNumFimCliente.ClientEnabled = rdbEspecicarNumCliente.Checked;

        txtEspecDataIni.Text = consultaVA.DATA_FECH_CLI_INI != DateTime.MinValue ? consultaVA.DATA_FECH_CLI_INI.ToString("dd/MM/yyyy") : null;
        txtEspecDataFim.Text = consultaVA.DATA_FECH_CLI_FIM != DateTime.MaxValue ? consultaVA.DATA_FECH_CLI_FIM.ToString("dd/MM/yyyy") : null;
        rdbEspecicarDataCliente.Checked = consultaVA.DATA_FECH_CLI_INI != DateTime.MinValue || consultaVA.DATA_FECH_CLI_FIM != DateTime.MaxValue;
        txtEspecDataIni.ClientEnabled = txtEspecDataFim.ClientEnabled = rdbEspecicarDataCliente.Checked;

        chbTipoFechCliente.Checked = rdbEspecicarNumCliente.Checked || rdbEspecicarDataCliente.Checked;
    }

    private CONSULTA_VA MontaFiltroConsulta() //Monta o filtro da consulta a partir dos dados da tela
    {
        if (GetOperador() == null) return null;
        var consultaVA = new CONSULTA_VA();
        consultaVA.SISTEMA = Sistema;
        consultaVA.CODTIPOCONS = 1;
        consultaVA.OPERADOR = GetOperador().CODOPE;

        if (Master.AcaoCadastro == Constantes.AcaoCadastro.acALTERANDO)
        {
            if (Session[CodigoConsulta] != null)
            {
                consultaVA.CODCONS = (int)Session[CodigoConsulta];
                consultaVA.NOME_CONSULTA = (string)grvListaConsultas.GetRowValues(grvListaConsultas.FocusedRowIndex, "NOME_CONSULTA");
                if (!FlgTabDoisVisivel)
                    InitDadosPreGravadosTabDois(RecuperaConsulta(consultaVA.CODCONS));
            }
        }
        else
            if (Master.AcaoCadastro == Constantes.AcaoCadastro.acGERARESULT && Session[UtilSIL.DADOSPARACONSULTA] != null)
        {
            if (Session[CodigoConsulta] != null)
            {
                consultaVA.CODCONS = (int)Session[CodigoConsulta];
                consultaVA.NOME_CONSULTA = Session[NomeConsulta].ToString();
                if (!FlgTabDoisVisivel)
                    InitDadosPreGravadosTabDois((CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA]);
            }
        }
        else
                if (Session[IsretornoNovaconsulta] != null && (bool)Session[IsretornoNovaconsulta] && !FlgTabDoisVisivel)
            InitDadosPreGravadosTabDois((CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA]);

        #region Agrupamento

        if (GetOperador().HABAGRUPAMENTO)
        {
            if (GetOperador().CODAG > 0)
            {
                consultaVA.AGRUPAMENTO = GetOperador().CODAG;
            }
        }

        #endregion

        #region Periodo

        consultaVA.PERIODO_INI = DateTime.MinValue;
        consultaVA.PERIODO_FIM = DateTime.MaxValue;
        string horaIni = string.Empty, horaFim = string.Empty;

        if (chbPeriodo.Checked)
        {
            if (!string.IsNullOrEmpty(txtPeriodoIni.Text))
            {
                consultaVA.PERIODO_INI = Convert.ToDateTime(txtPeriodoIni.Text);

                if (!string.IsNullOrEmpty(txtPeriodoHoraIni.Text))
                {
                    if (blConsultaVA.IsNaturalNumber(txtPeriodoHoraIni.Text))
                    {
                        consultaVA.HORA_PERIODO_INI += (txtPeriodoHoraIni.Text.Length > 1) ? txtPeriodoHoraIni.Text : "0" + txtPeriodoHoraIni.Text;

                        if (!string.IsNullOrEmpty(txtPeriodoMinIni.Text) && blConsultaVA.IsNaturalNumber(txtPeriodoMinIni.Text))
                            consultaVA.HORA_PERIODO_INI += ":" + txtPeriodoMinIni.Text;
                        else
                            consultaVA.HORA_PERIODO_INI += ":00";
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtPeriodoFim.Text))
            {
                consultaVA.PERIODO_FIM = Convert.ToDateTime(txtPeriodoFim.Text);

                if (!string.IsNullOrEmpty(txtPeriodoHoraFim.Text))
                {
                    if (blConsultaVA.IsNaturalNumber(txtPeriodoHoraFim.Text))
                    {
                        consultaVA.HORA_PERIODO_FIM += (txtPeriodoHoraFim.Text.Length > 1) ? txtPeriodoHoraFim.Text : "0" + txtPeriodoHoraFim.Text;

                        if (!string.IsNullOrEmpty(txtPeriodoMinFim.Text) && blConsultaVA.IsNaturalNumber(txtPeriodoMinFim.Text))
                            consultaVA.HORA_PERIODO_FIM += ":" + txtPeriodoMinFim.Text;
                        else
                            consultaVA.HORA_PERIODO_FIM += ":00";
                    }
                }
            }
        }

        #endregion

        #region Numero Host

        if (chbNumHost.Checked)
        {
            if (!string.IsNullOrEmpty(txtHostIni.Text))
                consultaVA.NUM_HOST_INI = int.Parse(txtHostIni.Text);

            if (!string.IsNullOrEmpty(txtHostFim.Text))
                consultaVA.NUM_HOST_FIM = int.Parse(txtHostFim.Text);
        }

        #endregion

        #region Numero Autorizacao

        if (chbNumAutorizacao.Checked)
        {
            if (!string.IsNullOrEmpty(txtAutIni.Text))
                consultaVA.NUM_AUT_INI = int.Parse(txtAutIni.Text);

            if (!string.IsNullOrEmpty(txtAutFim.Text))
                consultaVA.NUM_AUT_FIM = int.Parse(txtAutFim.Text);
        }
        #endregion

        #region Tipo Usuario

        if (chbTipoUsuario.Checked)
        {
            consultaVA.NOME_USUARIO = (string.IsNullOrEmpty(txtUsuarioNome.Text) ? null : txtUsuarioNome.Text);
            consultaVA.CPF_USUARIO = (string.IsNullOrEmpty(txtUsuarioCPF.Text) ? null : txtUsuarioCPF.Text);
            consultaVA.MAT_USUARIO = (string.IsNullOrEmpty(txtUsuarioMatricula.Text) ? null : txtUsuarioMatricula.Text);
        }

        #endregion

        #region Subrede

        if (temSubRede)
        {
            if (chbSubRede.Checked)
            {
                StringBuilder lista = new StringBuilder();
                List<object> rows = chbListaSubRede.GetSelectedFieldValues("CODSUBREDE");
                for (int i = 0; i < rows.Count; i++)
                {
                    lista.Append(rows[i].ToString() + ",");
                }

                consultaVA.SUBREDE = lista.ToString();
            }
        }

        #endregion

        #region Rede de Captura

        if (chbRedeCaptura.Checked)
        {
            StringBuilder lista = new StringBuilder();
            List<object> rows = chbListaRedeCaptura.GetSelectedFieldValues("REDE");
            for (int i = 0; i < rows.Count; i++)
            {
                lista.Append(rows[i].ToString() + ",");
            }

            consultaVA.REDE = lista.ToString();
        }

        #endregion

        #region Tipo Transacao

        if (chbTipoTrans.Checked)
        {
            StringBuilder lista = new StringBuilder();
            List<object> rows = chbListaTipoTrans.GetSelectedFieldValues("TIPTRA");
            for (int i = 0; i < rows.Count; i++)
            {
                lista.Append(rows[i].ToString() + ",");
            }

            consultaVA.TIPO_TRANSACAO = lista.ToString();
        }

        #endregion

        #region Tipo Resposta

        if (chbTipoResposta.Checked)
        {
            var listaResp = new StringBuilder();
            var rows = chbListTipoResp.GetSelectedFieldValues("VALUE");
            foreach (var t in rows)
            {
                listaResp.Append("'" + t + "',");
            }
            consultaVA.TIPO_RESPOSTA = listaResp.ToString();
        }

        #endregion

        #region Numero Cartao

        if (chbCartao.Checked && !string.IsNullOrEmpty(txtCartao.Text))
            consultaVA.NUM_CARTAO = txtCartao.Text;

        #endregion

        #region Cliente

        if (chbTipoCliente.Checked)
        {
            if (Session[ListaClientes] != null)
            {
                var serializer = Session[ListaClientes] as ListaClientesSessionSerializer;
                List<IDadosBasicosCliente> lista = (List<IDadosBasicosCliente>)serializer.Instance;
                foreach (IDadosBasicosCliente cli in lista)
                    consultaVA.LISTA_CLI += cli.CODCLI + ",";
            }
        }
        else
        {
            if (chbIntervaloCli.Checked && txtInterCliIni.Text != string.Empty && txtInterCliFim.Text != string.Empty)
            {
                consultaVA.INTERVALO_CLI_INI = txtInterCliIni.Text;
                consultaVA.INTERVALO_CLI_FIM = txtInterCliFim.Text;
            }
        }

        #endregion

        #region Credenciado

        if (chbTipoCredenciado.Checked)
        {
            if (Session[ListaCredenciado] != null)
            {
                List<CREDENCIADO_VA> lista = (List<CREDENCIADO_VA>)Session[ListaCredenciado];
                foreach (CREDENCIADO_VA cre in lista)
                    consultaVA.LISTA_CRED += cre.CODCRE + ",";
            }
        }
        else
        {
            if (chbIntervaloCre.Checked && txtInterCredIni.Text != string.Empty && txtInterCredFim.Text != string.Empty)
            {
                consultaVA.INTERVALO_CRED_INI = txtInterCredIni.Text;
                consultaVA.INTERVALO_CRED_FIM = txtInterCredFim.Text;
            }
        }

        #endregion

        #region Centralizadora

        if (chbIntervaloCentralizadora.Checked)
        {
            consultaVA.CODCEN = Convert.ToInt32(txtInterCentralizadora.Text);
        }

        #endregion

        #region Fechamento Credenciado

        if (chbTipoFechCred.Checked)
        {
            if (rdbEspecicarNum.Checked)
            {
                consultaVA.NUM_FECH_CRED_INI = txtEspecNumIni.Text == "" ? 0 : Convert.ToInt32(txtEspecNumIni.Text);
                consultaVA.NUM_FECH_CRED_FIM = txtEspecNumFim.Text == "" ? 0 : Convert.ToInt32(txtEspecNumFim.Text);
            }

            if (rdbEspecicarData.Checked)
            {
                if (!string.IsNullOrEmpty(txtEspecDataIni.Text))
                    consultaVA.DATA_FECH_CRED_INI = Convert.ToDateTime(txtEspecDataIni.Text);

                if (!string.IsNullOrEmpty(txtEspecDataFim.Text))
                    consultaVA.DATA_FECH_CRED_FIM = Convert.ToDateTime(txtEspecDataFim.Text);
            }
        }

        #endregion

        #region Fechamento Cliente

        if (chbTipoFechCliente.Checked)
        {
            if (rdbEspecicarNumCliente.Checked)
            {
                consultaVA.NUM_FECH_CLI_INI = txtEspecNumIniCliente.Text == "" ? 0 : Convert.ToInt32(txtEspecNumIniCliente.Text);
                consultaVA.NUM_FECH_CLI_FIM = txtEspecNumFimCliente.Text == "" ? 0 : Convert.ToInt32(txtEspecNumFimCliente.Text);
            }

            if (rdbEspecicarDataCliente.Checked)
            {
                if (!string.IsNullOrEmpty(txtEspecDataIniCliente.Text))
                    consultaVA.DATA_FECH_CLI_INI = Convert.ToDateTime(txtEspecDataIniCliente.Text);

                if (!string.IsNullOrEmpty(txtEspecDataFimCliente.Text))
                    consultaVA.DATA_FECH_CLI_FIM = Convert.ToDateTime(txtEspecDataFimCliente.Text);
            }
        }

        #endregion

        if (Session[ListaColunasNaoVisualizar] != null)
            consultaVA.LISTA_COL = Session[ListaColunasNaoVisualizar].ToString();
        else
            consultaVA.LISTA_COL = string.Empty;

        return consultaVA;
    }

    private void Pesquisar() //Pesquisa inicial das consultas salvas em banco de dados
    {
        if (GetOperador() == null) return;
        RemoveSessionsSimples();
        StringBuilder Filtro = new StringBuilder();
        blConsultaVA consulta = new blConsultaVA(GetOperador());

        Filtro.Append(" WHERE C.OPERADOR = " + GetOperador().CODOPE + " AND C.CODTIPOCONS = " + 1);//Filtros. Depois eu penso em uma solucao mais elegante...

        if (cbxCampoFiltro.Value.ToString() == "2")
        {
            if (!string.IsNullOrEmpty(txtPesquisaTransacao.Text))
                Filtro.Append(" AND C.CODCONS = " + Convert.ToInt32(txtPesquisaTransacao.Text));
        }
        else
            if (cbxCampoFiltro.Value.ToString() == "1")
            Filtro.Append(" AND C.NOME_CONSULTA LIKE '" + txtPesquisaTransacao.Text + "%'");

        Session[PesquisaInicial] = consulta.ColecaoConsultas(Filtro.ToString());
        grvListaConsultas.DataSource = Session[PesquisaInicial];
        grvListaConsultas.DataBind();
    }

    private void SalvarConsulta()
    {
        if (GetOperador() == null) return;
        CONSULTA_VA consultaVA = null;
        if (Master.AcaoCadastro == Constantes.AcaoCadastro.acINSERINDO)
        {
            consultaVA = MontaFiltroConsulta();
            if (consultaVA != null)
            {
                consultaVA.NOME_CONSULTA = txtNomeConsulta.Text;
                new blConsultaVA(GetOperador()).Incluir(consultaVA);
            }
        }
        else
        {
            consultaVA = MontaFiltroConsulta();
            if (consultaVA != null)
                new blConsultaVA(GetOperador()).Alterar(consultaVA);
        }
        Pesquisar();
    }

    protected void chbSubRede_CheckedChanged(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        if (hdAcao.Value == "AcaoInicial")
            return;

        if (hdAcao.Value == "AcaoCheckedTodasSubRedes")
        {
            chbListaSubRede.Selection.SelectAll();
            return;
        }

        if (hdAcao.Value == "AcaoNoCheckedTodasSubRedes")
        {
            chbListaSubRede.Selection.UnselectAll();
            return;
        }

        chbListaSubRede.Enabled = chbSubRede.Checked;
        chbTodasSubRedes.Enabled = chbSubRede.Checked;

        if (!chbSubRede.Checked)
        {
            chbListaSubRede.Selection.UnselectAll();
            chbTodasSubRedes.Checked = false;
            return;
        }
        else
            if (Session[ListaSubRede] == null)
        {
            Session[ListaSubRede] = new blTransacao(GetOperador()).ListaSubRede();
            chbListaSubRede.DataSource = Session[ListaSubRede];
            chbListaSubRede.DataBind();
        }
        else
                if (Session[ListaSubRede] != null)
        {
            chbListaSubRede.Enabled = chbSubRede.Checked;
            chbListaSubRede.DataSource = Session[ListaSubRede];
            chbListaSubRede.DataBind();
        }
    }

    protected void chbRedeCaptura_CheckedChanged(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        if (hdAcao.Value == "AcaoInicial")
            return;

        if (hdAcao.Value == "AcaoCheckedTodasRedeCaptura")
        {
            chbListaRedeCaptura.Selection.SelectAll();
            return;
        }

        if (hdAcao.Value == "AcaoNoCheckedTodasRedeCaptura")
        {
            chbListaRedeCaptura.Selection.UnselectAll();
            return;
        }

        chbListaRedeCaptura.Enabled = chbRedeCaptura.Checked;
        chbTodasRedeCaptura.Enabled = chbRedeCaptura.Checked;

        if (!chbRedeCaptura.Checked)
        {
            chbListaRedeCaptura.Selection.UnselectAll();
            chbTodasRedeCaptura.Checked = false;
            return;
        }
        else
            if (Session[ListaRedeCaptura] == null)
        {
            Session[ListaRedeCaptura] = new blTransacao(GetOperador()).ListaRedeCaptura();
            chbListaRedeCaptura.DataSource = Session[ListaRedeCaptura];
            chbListaRedeCaptura.DataBind();
        }
        else
                if (Session[ListaRedeCaptura] != null)
        {
            chbListaRedeCaptura.Enabled = chbRedeCaptura.Checked;
            chbListaRedeCaptura.DataSource = Session[ListaRedeCaptura];
            chbListaRedeCaptura.DataBind();
        }
    }

    protected void chbTipoTrans_CheckedChanged(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        if (hdAcao.Value == "AcaoInicial")
            return;

        if (hdAcao.Value == "AcaoCheckedTodasTrans")
        {
            chbListaTipoTrans.Selection.SelectAll();
            return;
        }

        if (hdAcao.Value == "AcaoNoCheckedTodasTrans")
        {
            chbListaTipoTrans.Selection.UnselectAll();
            return;
        }

        chbListaTipoTrans.Enabled = chbTipoTrans.Checked;
        chbTodasTrans.Enabled = chbTipoTrans.Checked;

        if (!chbTipoTrans.Checked)
        {
            chbListaTipoTrans.Selection.UnselectAll();
            chbTodasTrans.Checked = false;
            return;
        }
        else
            if (Session[ListaTipoTransacao] == null)
        {
            Session[ListaTipoTransacao] = new blTransacao(GetOperador()).ListaTipoTrans();
            chbListaTipoTrans.DataSource = Session[ListaTipoTransacao];
            chbListaTipoTrans.DataBind();
        }
        else
                if (Session[ListaTipoTransacao] != null)
        {
            chbListaTipoTrans.Enabled = chbTipoTrans.Checked;
            chbListaTipoTrans.DataSource = Session[ListaTipoTransacao];
            chbListaTipoTrans.DataBind();
        }
    }

    protected void chbTipoResposta_CheckedChanged(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        if (hdAcao.Value == "AcaoInicial")
            return;

        if (hdAcao.Value == "AcaoCheckedTodosTipResp")
        {
            chbListTipoResp.Selection.SelectAll();
            return;
        }

        if (hdAcao.Value == "AcaoNoCheckedTodasTipResp")
        {
            chbListTipoResp.Selection.UnselectAll();
            return;
        }

        chbListTipoResp.Enabled = chbTipoResposta.Checked;
        chbTodosTipResp.Enabled = chbTipoResposta.Checked;

        if (!chbTipoResposta.Checked)
        {
            chbListTipoResp.Selection.UnselectAll();
            chbTodosTipResp.Checked = false;
            return;
        }
        else
        {
            chbListTipoResp.Enabled = chbTipoResposta.Checked;
            chbListTipoResp.DataSource = new blConsultaVA(GetOperador()).InitColectionTipoResp();
            chbListTipoResp.DataBind();
        }
    }

    protected void tabDadosTrans_ActiveTabChanged(object source, DevExpress.Web.ASPxTabControl.TabControlEventArgs e)
    {
        if (e.Tab.Index == IdxTabDois && !FlgTabDoisVisivel)
        {
            if (Master.AcaoCadastro == Constantes.AcaoCadastro.acALTERANDO && Session[CodigoConsulta] != null)
                InitDadosPreGravadosTabDois(RecuperaConsulta((int)Session[CodigoConsulta]));
            else
                if (Master.AcaoCadastro == Constantes.AcaoCadastro.acGERARESULT && Session[UtilSIL.DADOSPARACONSULTA] != null)
                InitDadosPreGravadosTabDois((CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA]);
            else
                    if (Master.AcaoCadastro == Constantes.AcaoCadastro.acINSERINDO && Session[IsretornoNovaconsulta] != null && (bool)Session[IsretornoNovaconsulta] && Session[UtilSIL.DADOSPARACONSULTA] != null)
            {
                InitDadosPreGravadosTabDois((CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA]);
                Session.Remove(IsretornoNovaconsulta);
            }
        }
    }

    #endregion

    #region Callbacks (Todos)

    protected void clbColunas_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        switch (e.Parameter)
        {
            case "1":
                {
                    if (grvColDisponiveis.FocusedRowIndex < 0)
                        return;
                    var value = grvColDisponiveis.GetRowValues(grvColDisponiveis.FocusedRowIndex, "Value").ToString();

                    if (Session[ListaColunasNaoVisualizar] != null)
                    {
                        var colunas = Session[ListaColunasNaoVisualizar].ToString();
                        colunas += value + ";";
                        Session[ListaColunasNaoVisualizar] = colunas;
                    }
                    else
                        Session[ListaColunasNaoVisualizar] = value + ";";
                }
                break;
            case "2":
                {
                    var todasColunas = new blConsultaVA(GetOperador()).MontarListaTodasColunas();
                    var colunas = new StringBuilder();
                    foreach (var item in todasColunas)
                        colunas.Append(item.Value + ";");

                    Session[ListaColunasNaoVisualizar] = colunas.ToString();
                }
                break;
            case "3":
                Session[ListaColunasNaoVisualizar] = string.Empty;
                break;
            case "4":
                {
                    if (grvColRetiradas.FocusedRowIndex < 0)
                        return;
                    var value = grvColRetiradas.GetRowValues(grvColRetiradas.FocusedRowIndex, "Value").ToString();
                    var newColuns = new StringBuilder();
                    if (Session[ListaColunasNaoVisualizar] != null)
                    {
                        var colunas = Session[ListaColunasNaoVisualizar].ToString().Split(';');
                        foreach (var col in colunas.Where(col => col != value && col != string.Empty))
                            newColuns.Append(col + ";");
                    }
                    Session[ListaColunasNaoVisualizar] = newColuns.ToString();
                }
                break;
        }

        grvColDisponiveis.DataBind();
        grvColRetiradas.DataBind();
    }

    protected void cplMain_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        ClearCampos();
        RemoveSessionsSimples();
        FlgTabDoisVisivel = false;
        tabDadosTrans.ActiveTabIndex = 0;

        switch (e.Parameter)
        {
            case "1":
                {
                    Master.AcaoCadastro = Constantes.AcaoCadastro.acALTERANDO;
                    if (grvListaConsultas.FocusedRowIndex != -1)
                    {
                        rplMainTrasacoes.ClientVisible = true;
                        int codigo;
                        int.TryParse(grvListaConsultas.GetRowValues(grvListaConsultas.FocusedRowIndex, "CODCONS").ToString(), out codigo);
                        Session[CodigoConsulta] = codigo;
                        Session[NomeConsulta] = grvListaConsultas.GetRowValues(grvListaConsultas.FocusedRowIndex, "NOME_CONSULTA");
                        var consulta = RecuperaConsulta(codigo);
                        if (consulta != null)
                            Session[ListaColunasNaoVisualizar] = consulta.LISTA_COL;
                        //InitDadosPreGravadosTabUm(consulta);
                        PreencherTelaFiltro(consulta);
                    }
                }
                break;
            case "2":
                rplMainTrasacoes.ClientVisible = true;
                hdAlteraConsulta.Value = string.Empty;
                Master.AcaoCadastro = Constantes.AcaoCadastro.acINSERINDO;
                RemoveSessionConsultaVa();

                if (prePago.Checked || posPago.Checked)
                {
                    Session[UtilSIL.DADOSPARACONSULTA] = new CONSULTA_VA { SISTEMA = Sistema };
                }

                break;
            case "3":
                rplMainTrasacoes.ClientVisible = true;
                hdAlteraConsulta.Value = string.Empty;
                Master.AcaoCadastro = Constantes.AcaoCadastro.acINSERINDO;
                TrataRetornoTelaResult();
                break;
        }
    }

    protected void clbGridCredenciados_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (e.Parameter == "Limpar")
        {
            Session.Remove(ListaCredenciado);
            grvListaFiltroCredenciados.DataBind();
        }
        else
        {
            CREDENCIADO_VA cred;
            var lista = new List<CREDENCIADO_VA>();
            if (Session[ListaCredenciado] != null)
                lista = (List<CREDENCIADO_VA>)Session[ListaCredenciado];

            if (e.Parameter == "Incluir")
            {
                if (txtRazSoc.Text != string.Empty && txtRazSoc.Text != "NAO ENCONTRADO")
                {
                    if (Session[ListaCredenciado] != null)
                        lista = (List<CREDENCIADO_VA>)Session[ListaCredenciado];
                    else
                        lista = new List<CREDENCIADO_VA>();

                    cred = new blConsultaVA(GetOperador()).CriaCredConsultaTrans(txtRazSoc.Text, txtCodCreFiltro.Text);
                    if (lista.Find(u => u.CODCRE == cred.CODCRE) == null)
                        lista.Add(cred);
                }
            }
            else
            {
                if (lista != null && lista.Count > 0)
                {
                    var codcre = grvListaFiltroCredenciados.GetRowValues(Convert.ToInt16(e.Parameter), "CODCRE").ToString();
                    cred = lista.Find(u => u.CODCRE == codcre);
                    lista.Remove(cred);
                }
            }

            if (lista != null && lista.Count > 0)
            {
                var sort = new Sort<CREDENCIADO_VA>("CODCRE");
                lista.Sort(sort);
            }

            Session[ListaCredenciado] = lista;
            grvListaFiltroCredenciados.DataSource = Session[ListaCredenciado];
            grvListaFiltroCredenciados.DataBind();
        }
    }

    protected void clbtxtRazSocial_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        IFilter filter = new Filter("codcre", SqlOperators.Equal, txtCodCreFiltro.Text);
        try
        {
            var credenciados = new blCredenciadoVA(GetOperador()).ColecaoCredenciados(filter);
            if (credenciados.Count == 1)
                txtRazSoc.Text = credenciados[0].RAZSOC;
            else
                TratarVisual(txtRazSoc);
        }
        catch
        {
            TratarVisual(txtRazSoc);
        }
    }

    protected void clbNomeCli_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        IFilter filter = new Filter("codcli", SqlOperators.Equal, txtCodCliFiltro.Text);
        IDadosBasicosCliente cliente = null;

        try
        {
            if (GetOperador().HABAGRUPAMENTO && GetOperador().CODAG > 0)
                cliente = ColecaoClientes(filter, GetOperador().CODAG).FirstOrDefault();
            else
                cliente = ColecaoClientes(filter).FirstOrDefault();

            if (cliente != null)
                txtNomeClienteFiltro.Text = cliente.NOMCLI;
            else
                TratarVisual(txtNomeClienteFiltro);
        }
        catch
        {
            TratarVisual(txtNomeClienteFiltro);
        }
    }

    protected void clbGridClientes_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (e.Parameter == "Limpar")
        {
            Session.Remove(ListaClientes);
            grvListaFiltroClientes.DataBind();
        }
        else
        {
            IDadosBasicosCliente cli;
            var lista = new List<IDadosBasicosCliente>();
            if (Session[ListaClientes] != null)
                lista = (List<IDadosBasicosCliente>)Session[ListaClientes];

            if (e.Parameter == "Incluir")
            {
                if (txtNomeClienteFiltro.Text != string.Empty && txtNomeClienteFiltro.Text != "NAO ENCONTRADO")
                {
                    int codcliaux;
                    int.TryParse(txtCodCliFiltro.Text, out codcliaux);
                    cli = new blConsultaVA(GetOperador()).CriaCliConsultaTrans(txtNomeClienteFiltro.Text, codcliaux);
                    if (lista.Find(u => u.CODCLI == cli.CODCLI) == null)
                        lista.Add(cli);
                }
            }
            else
            {
                if (lista != null && lista.Count > 0)
                {
                    var codcli = grvListaFiltroClientes.GetRowValues(Convert.ToInt16(e.Parameter), "CODCLI").ToString();
                    int codcliAux;
                    int.TryParse(codcli, out codcliAux);
                    cli = lista.Find(u => u.CODCLI == codcliAux);
                    lista.Remove(cli);
                }
            }

            if (lista != null && lista.Count > 0)
            {
                var sort = new Sort<IDadosBasicosCliente>("CODCLI");
                lista.Sort(sort);
            }

            var serializer = new ListaClientesSessionSerializer { Instance = lista };
            Session[ListaClientes] = serializer;
            Session.SaveSession();

            grvListaFiltroClientes.DataSource = serializer.Instance;
            grvListaFiltroClientes.DataBind();
        }
    }

    protected void pnlPesquisa_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        switch (e.Parameter)
        {
            case "Excluir":
                if (Session[CodigoConsulta] != null)
                    new blConsultaVA(GetOperador()).Excluir((int)Session[CodigoConsulta], 1);
                else
                {
                    if (grvListaConsultas.FocusedRowIndex != -1)
                    {
                        int codigo;
                        int.TryParse(grvListaConsultas.GetRowValues(grvListaConsultas.FocusedRowIndex, "CODCONS").ToString(), out codigo);
                        new blConsultaVA(GetOperador()).Excluir(codigo, 1);
                    }
                }
                break;
            case "Salvar":
                if (!string.IsNullOrEmpty(txtNomeConsulta.Text))
                    SalvarConsulta();
                break;
        }

        Pesquisar();
    }

    #endregion

    #region Page index e sort (Todas Grids)

    protected void grvListaConsultas_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
    {
        if (GetOperador() == null) return;
        grvListaConsultas.DataSource = Session[PesquisaInicial];
        grvListaConsultas.DataBind();
    }

    protected void grvListaConsultas_PageIndexChanged(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        grvListaConsultas.DataSource = Session[PesquisaInicial];
        grvListaConsultas.DataBind();
    }

    #endregion

    #region Eventos botoes Geral

    protected void btnResultado_Click(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        Session["RESULCONSTRANSACAO"] = null;

        var consulta = MontaFiltroConsulta();
        var mensagem = consulta.FILTROVAZIO
            ? "O filtro não foi informado ou contem somente valores vazios. Informe um filtro valido para efetuar a consulta."
            : new blTransacao(GetOperador()).ObterNumeroTransacoes(consulta) > GetOperador().NUMCONSREGTRANS
                ? "A consulta requisitada retorna uma quantidade muito grande de registros e nao pode ser concluida. Por favor, modifique o filtro da consulta e tente novamente."
                : string.Empty;

        Session[UtilSIL.DADOSPARACONSULTA] = consulta;

        if (!string.IsNullOrEmpty(mensagem))
        {
            ExibeMensagem(mensagem);

            if (!posPago.Checked && !prePago.Checked)
            {
                Session[UtilSIL.DADOSPARACONSULTA] = null;
            }

            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "mensagemFiltroVazio", "ExibeMensagem(null, EditarFiltroConsulta);", true);
            return;
        }

        Session.SaveSession();
        Master.AcaoCadastro = Constantes.AcaoCadastro.acGERARESULT;        
        Master.Transfer("ResultConsTransacao.aspx", true);
    }

    #endregion

    private void BindingGrid<TTipoChave>(ASPxGridView grid, ASPxCheckBox todas, Func<string, object> convert, Func<CONSULTA_VA, string> getCodigos)
    {
        var filtro = Session[UtilSIL.DADOSPARACONSULTA] as CONSULTA_VA;

        if (filtro == null)
        {
            return;
        }

        var codigos = getCodigos(filtro);

        if (string.IsNullOrEmpty(codigos))
        {
            return;
        }

        todas.Enabled = true;
        grid.Enabled = true;

        foreach (var codigo in codigos.Split(',').Where(c => !string.IsNullOrEmpty(c)).Select(c => convert(c)))
        {
            grid.Selection.SetSelectionByKey(codigo, true);
        }
    }

    protected void chbListaSubRede_DataBinding(object sender, EventArgs e)
    {
        BindingGrid<int>(chbListaSubRede, chbTodasSubRedes, (chave) => { return Convert.ToInt32(chave.Trim()); }, (filtro) => { return filtro.SUBREDE; });
    }

    protected void chbListaTipoTrans_DataBinding(object sender, EventArgs e)
    {
        BindingGrid<int>(chbListaTipoTrans, chbTodasTrans, (chave) => { return Convert.ToInt32(chave.Trim()); }, (filtro) => { return filtro.TIPO_TRANSACAO; });
    }

    protected void chbListaRedeCaptura_DataBinding(object sender, EventArgs e)
    {
        BindingGrid<string>(chbListaRedeCaptura, chbTodasRedeCaptura, (chave) => { return chave.Trim(); }, (filtro) => { return filtro.REDE; });
    }

    protected void chbListTipoResp_DataBinding(object sender, EventArgs e)
    {
        BindingGrid<char>(chbListTipoResp, chbTodosTipResp, (chave) => { return chave.Trim()[1]; }, (filtro) => { return filtro.TIPO_RESPOSTA; });
    }
}
