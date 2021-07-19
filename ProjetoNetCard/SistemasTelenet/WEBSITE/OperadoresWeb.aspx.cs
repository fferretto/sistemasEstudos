using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using TELENET.SIL;
using TELENET.SIL.BL;
using TELENET.SIL.PO;
using MenuItem = DevExpress.Web.ASPxMenu.MenuItem;

public partial class OperadoresWeb : System.Web.UI.Page
{
    public void Page_Init(object sender, EventArgs e)
    {
        if (Session["Operador"] == null)
        {
            if (Page.IsCallback)
                ASPxWebControl.RedirectOnCallback("~/Login.aspx");
            else
                Response.Redirect("~/Login.aspx"); // Forca Autenticacao
            return;
        }

        InicializarObjetosDts();
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (Session["Operador"] == null)
        {
            if (Page.IsCallback)
                ASPxWebControl.RedirectOnCallback("~/Login.aspx");
            else
                Response.Redirect("~/Login.aspx"); // Forca Autenticacao
            return;
        }

        var HabilitarCRUD = (bool)Session["HabilitarCRUD"];
        Page.ClientScript.RegisterClientScriptInclude("OperadorWeb", Page.ResolveVersionScript("OperadorWeb.js"));
        Page.ClientScript.RegisterClientScriptInclude("Formatacao", Page.ResolveVersionScript("Formatacao.js"));
        Page.ClientScript.RegisterClientScriptInclude("Utils", Page.ResolveVersionScript("Utils.js"));

        AplicaRestricoesPermissoes();
    }

    public void AplicaRestricoesPermissoes()
    {
        if (GetOperador().HABAGRUPAMENTO && GetOperador().CODAG > 0)
        {
            btnIncOpWeb.Enabled = false;
            btnCancOpWeb.Enabled = false;
            btnEditarOpWeb.Enabled = false;
            btnSalvarOpWeb.Enabled = false;
            btnExcOpWeb.Enabled = false;
        }
    }

    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }

    protected void CriarInstanciaClienteBL(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new blCLIENTEVA(GetOperador());
    }

    private void InicializarObjetosDts()
    {
        #region OperadorWEB
        var dtsOperadorWEBParc = new ObjectDataSource();
        dtsOperadorWEBParc.TypeName = "TELENET.SIL.BL.blCLIENTEVA";
        dtsOperadorWEBParc.DataObjectTypeName = "TELENET.SIL.PO.OPERCLIWEB_VA";
        dtsOperadorWEBParc.SelectMethod = "OperadoresWEBParceria";
        dtsOperadorWEBParc.DeleteMethod = "ExcluirOperadorWEB";
        dtsOperadorWEBParc.InsertMethod = "InserirOperadorWEB";
        dtsOperadorWEBParc.UpdateMethod = "AlterarOperadorWEB";
        dtsOperadorWEBParc.ObjectCreating += new ObjectDataSourceObjectEventHandler(CriarInstanciaClienteBL);
        dtsOperadorWEBParc.DataBind();
        grvOperadorParc.DataSource = dtsOperadorWEBParc;
        grvOperadorParc.DataBind();
        #endregion

        #region Parceria
        var dtsParceria = new ObjectDataSource();
        dtsParceria.TypeName = "TELENET.SIL.BL.blTabelas";
        dtsParceria.DataObjectTypeName = "TELENET.SIL.PO.PARCERIA";
        dtsParceria.SelectMethod = "ColecaoParceriaLK";
        dtsParceria.ObjectCreating += new ObjectDataSourceObjectEventHandler(CriarInstanciaTabelaLKBL);
        dtsParceria.DataBind();
        cbxParceria.DataSource = dtsParceria;
        cbxParceria.DataBind();
        #endregion
    }

    protected void CriarInstanciaTabelaLKBL(object sender, ObjectDataSourceEventArgs e)
    {
        e.ObjectInstance = new blTabelas(GetOperador());
    }

    protected void cplDadosOpWebParc_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        try
        {
            if (GetOperador() == null) return;
            var blClienteVA = new blCLIENTEVA(GetOperador());
            var blTabelas = new blTabelas(GetOperador());
            var parceria = new List<PARCERIA>();

            if (Session["OperParceria"] != null)
                parceria = (List<PARCERIA>)Session["OperParceria"];
            else
            {
                parceria = blTabelas.ColecaoParceriaLK();
                Session["OperParceria"] = parceria;
            }

            ACESSOOPERADORMW operadorWeb = null;

            if (Session["AcaoCadOpWebParc"] == null)
                Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acVISUALIZANDO;

            Constantes.AcaoCadastro acaoCadOpWebParc;
            var idOperador = Convert.ToInt32(grvOperadorParc.GetRowValues(grvOperadorParc.FocusedRowIndex, "ID"));
            var codParceria = Convert.ToInt32(grvOperadorParc.GetRowValues(grvOperadorParc.FocusedRowIndex, "CODPARCERIA"));

            if (idOperador > 0)
            {
                var validade = new blCLIENTEVA(GetOperador()).ValidadeSenha(idOperador);
                if (validade != string.Empty)
                {
                    edtValidade.Text = validade;
                    InfoSenha.Visible = Convert.ToDateTime(validade) <= DateTime.Now;
                }
            }

            var retorno = string.Empty;

            switch (e.Parameter)
            {
                case "Novo":
                    cbxParceria.Text = string.Empty;
                    edtNomeOpWeb.Text = string.Empty;
                    edtLoginOpWeb.Text = string.Empty;
                    edtValidade.Text = string.Empty;
                    txtCodClienteAgrupamento.Text = string.Empty;
                    txtClienteAgrupamento.Text = string.Empty;
                    txtNomGrupo.Text = string.Empty;

                    Session["SelecaoCliente"] = null;
                    Session["ClienteSelecao"] = null;
                    Session["ListClientesSelecao"] = null;

                    checkSelecionarTodos.Checked = false;
                    ckxAcessoBloqueado.Checked = false;

                    ckxIncCartao.Checked = false;
                    ckxBloCartao.Checked = false;
                    ckxDesbCartao.Checked = false;
                    ckxCancCartao.Checked = false;
                    ckxEmitir2Via.Checked = false;
                    ckxListarExtMov.Checked = false;
                    ckxConsRede.Checked = false;
                    ckxListarCartao.Checked = false;
                    ckxLitarIncCartao.Checked = false;
                    ckxSolicitarCarga.Checked = false;
                    ckxTransferirSaldo.Checked = false;
                    ckxTransferirSaldoConta.Checked = false;
                    ckxAlterarLimite.Checked = false;
                    ckxListTransAb.Checked = false;
                    Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acINSERINDO;
                    break;
                case "Pesquisar":
                    if (true)//grvClienteVA.FocusedRowIndex != -1)
                    {
                        Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acPESQUISANDO;
                        operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                        hdTipoAcesso.Value = operadorWeb.TIPOACESSO;

                        if (operadorWeb != null)
                        {
                            if (operadorWeb.TIPOACESSO == "P")
                                cbxParceria.Text = parceria.Find(x => x.CODPARCERIA == operadorWeb.CODPARCERIA).NOMPARCERIA;

                            txtNomGrupo.Text = operadorWeb.NOMGRUPO;
                            edtNomeOpWeb.Text = operadorWeb.NOME;
                            edtLoginOpWeb.Text = operadorWeb.LOGIN;
                            edtValidade.Text = operadorWeb.DTEXPSENHA;

                            if (!operadorWeb.FINCCART || !operadorWeb.FINCCART || !operadorWeb.FBLOQCART || !operadorWeb.FDESBLOQCART || !operadorWeb.FCANCCART || !operadorWeb.FSEGVIACART ||
                                !operadorWeb.FEXTMOV || !operadorWeb.FCONSREDE || !operadorWeb.FLISTCART || !operadorWeb.FLISTINCCART || !operadorWeb.FALTLIMITE || !operadorWeb.FLISTTRANSAB ||
                                !operadorWeb.FCARGA || !operadorWeb.FTRANSFSALDO || !operadorWeb.FTRANSFSALDOCLI)
                                checkSelecionarTodos.Checked = false;
                            else
                                checkSelecionarTodos.Checked = true;

                            ckxAcessoBloqueado.Checked = operadorWeb.ACESSOBLOQUEADO;

                            // Flags
                            ckxIncCartao.Checked = operadorWeb.FINCCART;
                            ckxBloCartao.Checked = operadorWeb.FBLOQCART;
                            ckxDesbCartao.Checked = operadorWeb.FDESBLOQCART;
                            ckxCancCartao.Checked = operadorWeb.FCANCCART;
                            ckxEmitir2Via.Checked = operadorWeb.FSEGVIACART;
                            ckxListarExtMov.Checked = operadorWeb.FEXTMOV;
                            ckxConsRede.Checked = operadorWeb.FCONSREDE;
                            ckxListarCartao.Checked = operadorWeb.FLISTCART;
                            ckxLitarIncCartao.Checked = operadorWeb.FLISTINCCART;

                            ckxSolicitarCarga.Checked = operadorWeb.FCARGA;
                            ckxTransferirSaldo.Checked = operadorWeb.FTRANSFSALDO;
                            ckxTransferirSaldoConta.Checked = operadorWeb.FTRANSFSALDOCLI;

                            ckxAlterarLimite.Checked = operadorWeb.FALTLIMITE;
                            ckxListTransAb.Checked = operadorWeb.FLISTTRANSAB;

                            
                            lblInfo.Text = "Operador com acesso a todos clientes desta parceria.";
                            if (operadorWeb.TIPOACESSO == "D")
                            {
                                rblTipoOperador.SelectedIndex = 1;
                                var agrupamentoCliente = blClienteVA.OperadoresWEBParceriaAgrupamentoCliente(idOperador);
                                var agrupamentoClienteTexto = new List<string>();
                                foreach (var item in agrupamentoCliente)
                                {
                                    agrupamentoClienteTexto.Add(string.Format("{0} - COD. {1} - {2}", item.TIPOCARTAO, item.CODCLI, item.NOMCLI));
                                }
                                Session["SelecaoCliente"] = agrupamentoClienteTexto;
                                Session["ListClientesSelecao"] = agrupamentoCliente;
                                cbxAgrupamentoCliente.DataSource = agrupamentoClienteTexto;
                                cbxAgrupamentoCliente.DataBind();
                                btnExcluirClienteAgrupamento.ClientVisible = true;
                                btnExcluirClienteAgrupamento.ClientEnabled = false;
                                btnIncluirCliente.ClientEnabled = false;
                                lblInfo.Text = "Operador com acesso apenas aos clientes listados.";
                            }
                            if (operadorWeb.TIPOACESSO == "P")
                            {
                                rblTipoOperador.SelectedIndex = 0;
                                var clientesParceria = blClienteVA.OperadoresWEBParceriaClientes(codParceria);
                                btnExcluirClienteAgrupamento.ClientVisible = false;
                                cbxAgrupamentoCliente.DataSource = clientesParceria;
                                cbxAgrupamentoCliente.DataBind();
                            }
                        }
                    }
                    break;
                case "Editar":
                    if (true) //grvClienteVA.FocusedRowIndex != -1)
                    {
                        Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acALTERANDO;
                        operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                        hdTipoAcesso.Value = operadorWeb.TIPOACESSO;

                        if (operadorWeb != null)
                        {
                            if (operadorWeb.TIPOACESSO == "P")
                                cbxParceria.Text = parceria.Find(x => x.CODPARCERIA == operadorWeb.CODPARCERIA).NOMPARCERIA;

                            txtNomGrupo.Text = operadorWeb.NOMGRUPO;
                            txtNomGrupo.ClientEnabled = true;

                            edtNomeOpWeb.Text = operadorWeb.NOME;
                            edtLoginOpWeb.Text = operadorWeb.LOGIN;

                            if (!operadorWeb.FINCCART || !operadorWeb.FINCCART || !operadorWeb.FBLOQCART || !operadorWeb.FDESBLOQCART || !operadorWeb.FCANCCART || !operadorWeb.FSEGVIACART ||
                                !operadorWeb.FEXTMOV || !operadorWeb.FCONSREDE || !operadorWeb.FLISTCART || !operadorWeb.FLISTINCCART || !operadorWeb.FALTLIMITE || !operadorWeb.FLISTTRANSAB ||
                                !operadorWeb.FCARGA || !operadorWeb.FTRANSFSALDO || !operadorWeb.FTRANSFSALDOCLI)
                                checkSelecionarTodos.Checked = false;
                            else
                                checkSelecionarTodos.Checked = true;

                            ckxAcessoBloqueado.Checked = operadorWeb.ACESSOBLOQUEADO;

                            // Flags
                            ckxIncCartao.Checked = operadorWeb.FINCCART;
                            ckxBloCartao.Checked = operadorWeb.FBLOQCART;
                            ckxDesbCartao.Checked = operadorWeb.FDESBLOQCART;
                            ckxCancCartao.Checked = operadorWeb.FCANCCART;
                            ckxEmitir2Via.Checked = operadorWeb.FSEGVIACART;
                            ckxListarExtMov.Checked = operadorWeb.FEXTMOV;
                            ckxConsRede.Checked = operadorWeb.FCONSREDE;
                            ckxListarCartao.Checked = operadorWeb.FLISTCART;
                            ckxLitarIncCartao.Checked = operadorWeb.FLISTINCCART;

                            ckxSolicitarCarga.Checked = operadorWeb.FCARGA;
                            ckxTransferirSaldo.Checked = operadorWeb.FTRANSFSALDO;
                            ckxTransferirSaldoConta.Checked = operadorWeb.FTRANSFSALDOCLI;

                            ckxAlterarLimite.Checked = operadorWeb.FALTLIMITE;
                            ckxListTransAb.Checked = operadorWeb.FLISTTRANSAB;

                            btnEditarOpWeb.ClientEnabled = false;

                            lblInfo.Text = "Operador com acesso a todos clientes desta parceria.";
                            if (operadorWeb.TIPOACESSO == "D")
                            {
                                rblTipoOperador.SelectedIndex = 1;
                                var agrupamentoCliente = blClienteVA.OperadoresWEBParceriaAgrupamentoCliente(idOperador);
                                var agrupamentoClienteTexto = new List<string>();
                                foreach (var item in agrupamentoCliente)
                                {
                                    agrupamentoClienteTexto.Add(string.Format("{0} - COD. {1} - {2}", item.TIPOCARTAO, item.CODCLI, item.NOMCLI));
                                }
                                Session["SelecaoCliente"] = agrupamentoClienteTexto;
                                Session["ListClientesSelecao"] = agrupamentoCliente;
                                cbxAgrupamentoCliente.DataSource = agrupamentoClienteTexto;
                                cbxAgrupamentoCliente.DataBind();
                                lblInfo.Text = "Operador com acesso apenas aos clientes listados.";
                                btnExcluirClienteAgrupamento.ClientVisible = true;
                            }
                            if (operadorWeb.TIPOACESSO == "P")
                            {
                                rblTipoOperador.SelectedIndex = 0;
                                var clientesParceria = blClienteVA.OperadoresWEBParceriaClientes(codParceria);
                                cbxAgrupamentoCliente.DataSource = clientesParceria;
                                cbxAgrupamentoCliente.DataBind();
                                btnExcluirClienteAgrupamento.ClientVisible = false;
                            }
                        }
                    }
                    break;
                case "Salvar":
                    acaoCadOpWebParc = (Constantes.AcaoCadastro)Session["AcaoCadOpWebParc"];
                    switch (acaoCadOpWebParc)
                    {
                        case Constantes.AcaoCadastro.acINSERINDO:
                            operadorWeb = new ACESSOOPERADORMW {
                                CODPARCERIA = codParceria,
                                TIPOACESSO = rblTipoOperador.SelectedIndex == 0 ? "P" : "D"
                            };
                            break;
                        case Constantes.AcaoCadastro.acALTERANDO:
                            operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                            break;
                    }
                    if (operadorWeb != null)
                    {
                        if (operadorWeb.TIPOACESSO == "P")
                            operadorWeb.CODPARCERIA = cbxParceria.SelectedItem.Value != null ? Convert.ToInt16(cbxParceria.SelectedItem.Value) : -1;
                        if (operadorWeb.TIPOACESSO == "D")
                            operadorWeb.AGRUPAMENTO = (List<CLIENTEAGRUPAMENTO>)Session["ListClientesSelecao"];

                        operadorWeb.NOMGRUPO = txtNomGrupo.Text.ToUpper();
                        operadorWeb.NOME = edtNomeOpWeb.Text.ToUpper();
                        operadorWeb.LOGIN = edtLoginOpWeb.Text.ToUpper().Trim();

                        operadorWeb.ACESSOBLOQUEADO = ckxAcessoBloqueado.Checked;

                        operadorWeb.FINCCART = ckxIncCartao.Checked;
                        operadorWeb.FBLOQCART = ckxBloCartao.Checked;
                        operadorWeb.FDESBLOQCART = ckxDesbCartao.Checked;
                        operadorWeb.FCANCCART = ckxCancCartao.Checked;
                        operadorWeb.FSEGVIACART = ckxEmitir2Via.Checked;
                        operadorWeb.FEXTMOV = ckxListarExtMov.Checked;
                        operadorWeb.FCONSREDE = ckxConsRede.Checked;
                        operadorWeb.FLISTCART = ckxListarCartao.Checked;
                        operadorWeb.FLISTINCCART = ckxLitarIncCartao.Checked;

                        operadorWeb.FCARGA = ckxSolicitarCarga.Checked;
                        operadorWeb.FTRANSFSALDO = ckxTransferirSaldo.Checked;
                        operadorWeb.FTRANSFSALDOCLI = ckxTransferirSaldoConta.Checked;

                        operadorWeb.FALTLIMITE = ckxAlterarLimite.Checked;
                        operadorWeb.FLISTTRANSAB = ckxListTransAb.Checked;

                        // Inclusao
                        if ((Session["AcaoCadOpWebParc"] != null) && (acaoCadOpWebParc == Constantes.AcaoCadastro.acINSERINDO))
                        {
                            blClienteVA.InserirOperadorWEB(operadorWeb, out retorno);
                            InfoSenha.Visible = true;
                            lblObs.Text = retorno;
                        }

                            // Alteracao
                        else if ((Session["AcaoCadOpWebParc"] != null) && (acaoCadOpWebParc == Constantes.AcaoCadastro.acALTERANDO))
                        {
                            blClienteVA.InserirOperadorWEB(operadorWeb, out retorno);
                            lblObs.Text = retorno;
                        }

                        operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                        lblInfo.Text = "Operador com acesso a todos clientes desta parceria.";
                        if (operadorWeb.TIPOACESSO == "D")
                        {
                            rblTipoOperador.SelectedIndex = 1;
                            var agrupamentoCliente = blClienteVA.OperadoresWEBParceriaAgrupamentoCliente(idOperador);
                            var agrupamentoClienteTexto = new List<string>();
                            foreach (var item in agrupamentoCliente)
                            {
                                agrupamentoClienteTexto.Add(string.Format("{0} - COD. {1} - {2}", item.TIPOCARTAO, item.CODCLI, item.NOMCLI));
                            }
                            Session["SelecaoCliente"] = agrupamentoClienteTexto;
                            Session["ListClientesSelecao"] = agrupamentoCliente;
                            cbxAgrupamentoCliente.DataSource = agrupamentoClienteTexto;
                            cbxAgrupamentoCliente.DataBind();
                            lblInfo.Text = "Operador com acesso apenas aos clientes listados.";
                            btnExcluirClienteAgrupamento.ClientVisible = true;
                        }
                        if (operadorWeb.TIPOACESSO == "P")
                        {
                            rblTipoOperador.SelectedIndex = 0;
                            var clientesParceria = blClienteVA.OperadoresWEBParceriaClientes(codParceria);
                            cbxAgrupamentoCliente.DataSource = clientesParceria;
                            cbxAgrupamentoCliente.DataBind();
                            btnExcluirClienteAgrupamento.ClientVisible = false;
                        }

                        Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acVISUALIZANDO;
                        grvOperadorParc.DataBind();
                    }
                    break;
                case "Excluir":
                    operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                    if (operadorWeb != null)
                    {
                        blClienteVA.ExcluirOperadorWEB(operadorWeb);
                        Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acVISUALIZANDO;
                    }
                    cbxParceria.Text = string.Empty;
                    txtCodClienteAgrupamento.Text = string.Empty;
                    txtClienteAgrupamento.Text = string.Empty;
                    txtNomGrupo.Text = string.Empty;
                    edtNomeOpWeb.Text = string.Empty;
                    edtLoginOpWeb.Text = string.Empty;
                    edtValidade.Text = string.Empty;
                    if (operadorWeb != null)
                    {
                        operadorWeb.ACESSOBLOQUEADO = ckxAcessoBloqueado.Checked;

                        operadorWeb.FINCCART = ckxIncCartao.Checked;
                        operadorWeb.FBLOQCART = ckxBloCartao.Checked;
                        operadorWeb.FDESBLOQCART = ckxDesbCartao.Checked;
                        operadorWeb.FCANCCART = ckxCancCartao.Checked;
                        operadorWeb.FSEGVIACART = ckxEmitir2Via.Checked;
                        operadorWeb.FEXTMOV = ckxListarExtMov.Checked;
                        operadorWeb.FCONSREDE = ckxConsRede.Checked;
                        operadorWeb.FLISTCART = ckxListarCartao.Checked;
                        operadorWeb.FLISTINCCART = ckxLitarIncCartao.Checked;

                        operadorWeb.FCARGA = ckxSolicitarCarga.Checked;
                        operadorWeb.FTRANSFSALDO = ckxTransferirSaldo.Checked;
                        operadorWeb.FTRANSFSALDOCLI = ckxTransferirSaldoConta.Checked;

                        operadorWeb.FALTLIMITE = ckxAlterarLimite.Checked;
                        operadorWeb.FLISTTRANSAB = ckxListTransAb.Checked;
                    }
                    break;
                case "Cancelar":
                    operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                    acaoCadOpWebParc = (Constantes.AcaoCadastro)Session["AcaoCadOpWebParc"];
                    switch (acaoCadOpWebParc)
                    {
                        case Constantes.AcaoCadastro.acINSERINDO:
                            cbxParceria.Text = string.Empty;
                            txtCodClienteAgrupamento.Text = string.Empty;
                            txtClienteAgrupamento.Text = string.Empty;
                            edtNomeOpWeb.Text = string.Empty;
                            edtLoginOpWeb.Text = string.Empty;
                            edtValidade.Text = string.Empty;
                            txtNomGrupo.Text = string.Empty;

                            Session["SelecaoCliente"] = null;
                            Session["ClienteSelecao"] = null;
                            Session["ListClientesSelecao"] = null;

                            ckxAcessoBloqueado.Checked = false;
                            checkSelecionarTodos.Checked = false;

                            ckxIncCartao.Checked = false;
                            ckxBloCartao.Checked = false;
                            ckxDesbCartao.Checked = false;
                            ckxCancCartao.Checked = false;
                            ckxEmitir2Via.Checked = false;
                            ckxListarExtMov.Checked = false;
                            ckxConsRede.Checked = false;
                            ckxListarCartao.Checked = false;
                            ckxLitarIncCartao.Checked = false;

                            ckxSolicitarCarga.Checked = false;
                            ckxTransferirSaldo.Checked = false;
                            ckxTransferirSaldoConta.Checked = false;

                            ckxAlterarLimite.Checked = false;
                            ckxListTransAb.Checked = false;

                            break;
                        case Constantes.AcaoCadastro.acALTERANDO:
                            Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acPESQUISANDO;
                            operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                            hdTipoAcesso.Value = operadorWeb.TIPOACESSO;

                            if (operadorWeb != null)
                            {
                                if (operadorWeb.TIPOACESSO == "P")
                                    cbxParceria.Text = parceria.Find(x => x.CODPARCERIA == operadorWeb.CODPARCERIA).NOMPARCERIA;

                                txtNomGrupo.Text = operadorWeb.NOMGRUPO;
                                edtNomeOpWeb.Text = operadorWeb.NOME;
                                edtLoginOpWeb.Text = operadorWeb.LOGIN;
                                edtValidade.Text = operadorWeb.DTEXPSENHA;
                                txtCodClienteAgrupamento.Text = string.Empty;
                                txtClienteAgrupamento.Text = string.Empty;


                                if (!operadorWeb.FINCCART || !operadorWeb.FINCCART || !operadorWeb.FBLOQCART || !operadorWeb.FDESBLOQCART || !operadorWeb.FCANCCART || !operadorWeb.FSEGVIACART ||
                                    !operadorWeb.FEXTMOV || !operadorWeb.FCONSREDE || !operadorWeb.FLISTCART || !operadorWeb.FLISTINCCART || !operadorWeb.FALTLIMITE || !operadorWeb.FLISTTRANSAB ||
                                    !operadorWeb.FCARGA || !operadorWeb.FTRANSFSALDO || !operadorWeb.FTRANSFSALDOCLI)
                                    checkSelecionarTodos.Checked = false;
                                else
                                    checkSelecionarTodos.Checked = true;

                                ckxAcessoBloqueado.Checked = operadorWeb.ACESSOBLOQUEADO;

                                // Flags
                                ckxIncCartao.Checked = operadorWeb.FINCCART;
                                ckxBloCartao.Checked = operadorWeb.FBLOQCART;
                                ckxDesbCartao.Checked = operadorWeb.FDESBLOQCART;
                                ckxCancCartao.Checked = operadorWeb.FCANCCART;
                                ckxEmitir2Via.Checked = operadorWeb.FSEGVIACART;
                                ckxListarExtMov.Checked = operadorWeb.FEXTMOV;
                                ckxConsRede.Checked = operadorWeb.FCONSREDE;
                                ckxListarCartao.Checked = operadorWeb.FLISTCART;
                                ckxLitarIncCartao.Checked = operadorWeb.FLISTINCCART;

                                ckxSolicitarCarga.Checked = operadorWeb.FCARGA;
                                ckxTransferirSaldo.Checked = operadorWeb.FTRANSFSALDO;
                                ckxTransferirSaldoConta.Checked = operadorWeb.FTRANSFSALDOCLI;

                                ckxAlterarLimite.Checked = operadorWeb.FALTLIMITE;
                                ckxListTransAb.Checked = operadorWeb.FLISTTRANSAB;


                                lblInfo.Text = "Operador com acesso a todos clientes desta parceria.";
                                if (operadorWeb.TIPOACESSO == "D")
                                {
                                    rblTipoOperador.SelectedIndex = 1;
                                    var agrupamentoCliente = blClienteVA.OperadoresWEBParceriaAgrupamentoCliente(idOperador);
                                    var agrupamentoClienteTexto = new List<string>();
                                    foreach (var item in agrupamentoCliente)
                                    {
                                        agrupamentoClienteTexto.Add(string.Format("{0} - COD. {1} - {2}", item.TIPOCARTAO, item.CODCLI, item.NOMCLI));
                                    }
                                    Session["SelecaoCliente"] = agrupamentoClienteTexto;
                                    Session["ListClientesSelecao"] = agrupamentoCliente;
                                    cbxAgrupamentoCliente.DataSource = agrupamentoClienteTexto;
                                    cbxAgrupamentoCliente.DataBind();
                                    lblInfo.Text = "Operador com acesso apenas aos clientes listados.";
                                    btnExcluirClienteAgrupamento.ClientVisible = true;
                                }
                                if (operadorWeb.TIPOACESSO == "P")
                                {
                                    rblTipoOperador.SelectedIndex = 0;
                                    var clientesParceria = blClienteVA.OperadoresWEBParceriaClientes(codParceria);
                                    cbxAgrupamentoCliente.DataSource = clientesParceria;
                                    cbxAgrupamentoCliente.DataBind();
                                    btnExcluirClienteAgrupamento.ClientVisible = false;
                                }

                                Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acCANCELANDO;
                            }
                            break;
                    }
                    if (operadorWeb != null)
                    {
                        Session["AcaoCadOpWebParc"] = Constantes.AcaoCadastro.acVISUALIZANDO;
                        lblInfo.Text = "Operador com acesso a todos clientes desta parceria.";
                        if (operadorWeb.TIPOACESSO == "D")
                        {
                            rblTipoOperador.SelectedIndex = 1;
                            var agrupamentoCliente = blClienteVA.OperadoresWEBParceriaAgrupamentoCliente(idOperador);
                            var agrupamentoClienteTexto = new List<string>();
                            foreach (var item in agrupamentoCliente)
                            {
                                agrupamentoClienteTexto.Add(string.Format("{0} - COD. {1} - {2}", item.TIPOCARTAO, item.CODCLI, item.NOMCLI));
                            }
                            Session["SelecaoCliente"] = agrupamentoClienteTexto;
                            Session["ListClientesSelecao"] = agrupamentoCliente;
                            cbxAgrupamentoCliente.DataSource = agrupamentoClienteTexto;
                            cbxAgrupamentoCliente.DataBind();
                            lblInfo.Text = "Operador com acesso apenas aos clientes listados.";
                            btnExcluirClienteAgrupamento.ClientVisible = true;
                        }
                        if (operadorWeb.TIPOACESSO == "P")
                        {
                            rblTipoOperador.SelectedIndex = 0;
                            var clientesParceria = blClienteVA.OperadoresWEBParceriaClientes(codParceria);
                            cbxAgrupamentoCliente.DataSource = clientesParceria;
                            cbxAgrupamentoCliente.DataBind();
                            btnExcluirClienteAgrupamento.ClientVisible = false;
                        }
                    }
                    break;
                case "RenovarAcesso":
                    if (idOperador > 0)
                    {
                        operadorWeb = blClienteVA.OperadorWEBParceria(idOperador);
                        blClienteVA.RenovarAcessoOperadorWEB(operadorWeb);
                    }
                    break;
            }

            // Controla Estado Botoes
            acaoCadOpWebParc = (Constantes.AcaoCadastro)Session["AcaoCadOpWebParc"];

            btnIncOpWeb.ClientEnabled = (acaoCadOpWebParc == Constantes.AcaoCadastro.acVISUALIZANDO);
            btnSalvarOpWeb.ClientEnabled = ((acaoCadOpWebParc == Constantes.AcaoCadastro.acINSERINDO) ||
                (acaoCadOpWebParc == Constantes.AcaoCadastro.acALTERANDO));
            btnExcOpWeb.ClientEnabled = (acaoCadOpWebParc == Constantes.AcaoCadastro.acVISUALIZANDO);

            switch (acaoCadOpWebParc)
            {
                case Constantes.AcaoCadastro.acINSERINDO:
                    {
                        rblTipoOperador.ReadOnly = false;
                        cbxParceria.ClientEnabled = true;
                        txtNomGrupo.ClientEnabled = true;
                        edtNomeOpWeb.ClientEnabled = true;
                        edtLoginOpWeb.ClientEnabled = true;                        
                        txtCodClienteAgrupamento.ClientEnabled = true;

                        ckxAcessoBloqueado.ClientEnabled = true;
                        ckxIncCartao.ClientEnabled = true;
                        ckxBloCartao.ClientEnabled = true;
                        ckxDesbCartao.ClientEnabled = true;
                        ckxCancCartao.ClientEnabled = true;
                        ckxEmitir2Via.ClientEnabled = true;
                        ckxListarExtMov.ClientEnabled = true;
                        ckxConsRede.ClientEnabled = true;
                        ckxListarCartao.ClientEnabled = true;
                        ckxLitarIncCartao.ClientEnabled = true;

                        ckxSolicitarCarga.ClientEnabled = true;
                        ckxTransferirSaldo.ClientEnabled = true;
                        ckxTransferirSaldoConta.ClientEnabled = true;

                        ckxAlterarLimite.ClientEnabled = true;
                        ckxListTransAb.ClientEnabled = true;
                    }
                    break;
                case Constantes.AcaoCadastro.acALTERANDO:
                    {
                        rblTipoOperador.ReadOnly = true;
                        cbxParceria.ClientEnabled = false;
                        
                        edtNomeOpWeb.ClientEnabled = false;
                        edtLoginOpWeb.ClientEnabled = false;
                        txtCodClienteAgrupamento.ClientEnabled = true;

                        ckxAcessoBloqueado.ClientEnabled = true;

                        ckxIncCartao.ClientEnabled = true;
                        ckxBloCartao.ClientEnabled = true;
                        ckxDesbCartao.ClientEnabled = true;
                        ckxCancCartao.ClientEnabled = true;
                        ckxEmitir2Via.ClientEnabled = true;
                        ckxListarExtMov.ClientEnabled = true;
                        ckxConsRede.ClientEnabled = true;
                        ckxListarCartao.ClientEnabled = true;
                        ckxLitarIncCartao.ClientEnabled = true;

                        ckxSolicitarCarga.ClientEnabled = true;
                        ckxTransferirSaldo.ClientEnabled = true;
                        ckxTransferirSaldoConta.ClientEnabled = true;

                        ckxAlterarLimite.ClientEnabled = true;
                        ckxListTransAb.ClientEnabled = true;
                    }
                    break;
                default:
                    {
                        rblTipoOperador.ReadOnly = true;
                        cbxParceria.ClientEnabled = false;
                        txtNomGrupo.ClientEnabled = false;
                        edtNomeOpWeb.ClientEnabled = false;
                        edtLoginOpWeb.ClientEnabled = false;
                        txtCodClienteAgrupamento.ClientEnabled = false;

                        ckxAcessoBloqueado.ClientEnabled = false;
                        checkSelecionarTodos.ClientEnabled = false;

                        ckxIncCartao.ClientEnabled = false;
                        ckxBloCartao.ClientEnabled = false;
                        ckxDesbCartao.ClientEnabled = false;
                        ckxCancCartao.ClientEnabled = false;
                        ckxEmitir2Via.ClientEnabled = false;
                        ckxListarExtMov.ClientEnabled = false;
                        ckxConsRede.ClientEnabled = false;
                        ckxListarCartao.ClientEnabled = false;
                        ckxLitarIncCartao.ClientEnabled = false;

                        ckxSolicitarCarga.ClientEnabled = false;
                        ckxTransferirSaldo.ClientEnabled = false;
                        ckxTransferirSaldoConta.ClientEnabled = false;

                        ckxAlterarLimite.ClientEnabled = false;
                        ckxListTransAb.ClientEnabled = false;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            var aux = ex.Message;
        }
    }

    protected void clbCliente_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        var param = e.Parameter;
        var selecaoCliente = new List<string>();
        var listClienteSelecao = new List<CLIENTEAGRUPAMENTO>();
        if (Session["ListClientesSelecao"] != null)
            listClienteSelecao = (List<CLIENTEAGRUPAMENTO>)Session["ListClientesSelecao"];

        if (e.Parameter.Contains("E"))
        {
            var item = Convert.ToInt32(e.Parameter.Split(':')[1]);
            listClienteSelecao[item].TIPOATUALIZACAO = "E";
            Session["ListClientesSelecao"] = listClienteSelecao;
            selecaoCliente = (List<string>)Session["SelecaoCliente"];
            selecaoCliente.Remove(selecaoCliente[item]);
            Session["SelecaoCliente"] = selecaoCliente;
        }
        else if (e.Parameter.Contains("I"))
        {
            if (Session["ClienteSelecao"] != null)
            {
                var clientePopUp = (CLIENTEAGRUPAMENTO)Session["ClienteSelecao"];

                if (Session["SelecaoCliente"] != null)
                    selecaoCliente = (List<string>)Session["SelecaoCliente"];

                if (listClienteSelecao.Where(x => x.SISTEMA == clientePopUp.SISTEMA && x.CODCLI == clientePopUp.CODCLI).ToList().Count == 0)
                {
                    listClienteSelecao.Add(clientePopUp);
                    Session["ListClientesSelecao"] = listClienteSelecao;
                    selecaoCliente.Add(string.Format("{0} - COD. {1} - {2}", clientePopUp.TIPOCARTAO, clientePopUp.CODCLI, clientePopUp.NOMCLI));
                    Session["SelecaoCliente"] = selecaoCliente;
                    txtClienteAgrupamento.Text = string.Empty;
                }
                else
                {
                    txtClienteAgrupamento.Text = "CLIENTE JA FOI INSERIDO";
                    txtClienteAgrupamento.Border.BorderColor = Color.Red;
                    txtClienteAgrupamento.BackColor = Color.Salmon;
                }
                lblInfo.Text = "Operador com acesso apenas aos clientes listados.";
                txtCodClienteAgrupamento.Text = string.Empty;
                Session["ClienteSelecao"] = null;
            }
        }
        else
        {
            if (e.Parameter != "")
            {
                var cliente = e.Parameter.Split('-');
                var codClientePopUp = Convert.ToInt32(cliente[0]);
                var sistemaPopUp = Convert.ToInt32(cliente[1]);

                var listClientes = (List<CLIENTEAGRUPAMENTO>)Session["ClienteSelecao"];

                var clienteSelecao = listClientes.Where(x => x.CODCLI == codClientePopUp && x.SISTEMA == sistemaPopUp).SingleOrDefault();

                if (clienteSelecao != null)
                {
                    Session["ClienteSelecao"] = null;
                    Session["ClienteSelecao"] = clienteSelecao;

                    if (clienteSelecao.STA == "02")
                    {
                        txtClienteAgrupamento.Text = "CLIENTE COM STATUS CANCELADO";
                        txtClienteAgrupamento.Border.BorderColor = Color.Red;
                        txtClienteAgrupamento.BackColor = Color.Salmon;
                        btnIncluirCliente.ClientEnabled = false;
                    }
                    else
                    {
                        if (listClienteSelecao.Where(x => x.SISTEMA == clienteSelecao.SISTEMA && x.CODCLI == clienteSelecao.CODCLI).ToList().Count == 0)
                            txtClienteAgrupamento.Text = "(" + Convert.ToString(clienteSelecao.CODCLI) + ") " + clienteSelecao.NOMCLI + " - " + clienteSelecao.PRODUTO + " - " + (clienteSelecao.TIPOCARTAO);
                        else
                        {
                            txtClienteAgrupamento.Text = "CLIENTE JA FOI INSERIDO";
                            txtClienteAgrupamento.Border.BorderColor = Color.Red;
                            txtClienteAgrupamento.BackColor = Color.Salmon;
                            btnIncluirCliente.ClientEnabled = false;
                        }
                    }
                }
                else
                {
                    txtClienteAgrupamento.Text = "CLIENTE NÃO ENCONTRADO";
                    txtClienteAgrupamento.Border.BorderColor = Color.Red;
                    txtClienteAgrupamento.BackColor = Color.Salmon;
                    btnIncluirCliente.ClientEnabled = false;
                }

            }
            else
            {
                if (txtCodClienteAgrupamento.Text != string.Empty)
                {
                    int codCli;
                    int.TryParse(txtCodClienteAgrupamento.Text, out codCli);

                    IFilter filtro = null;
                    filtro = new Filter("codcli", SqlOperators.Equal, codCli.ToString(CultureInfo.InvariantCulture));
                    var clientes = new blCLIENTENovo(GetOperador()).ColecaoClientes(filtro);
                    var clienteSelecao = new List<CLIENTEAGRUPAMENTO>();

                    foreach (var item in clientes)
                    {
                        var cliente = new CLIENTEAGRUPAMENTO
                        {
                            SISTEMA = item.SISTEMA,
                            CODCLI = item.CODCLI,
                            NOMCLI = item.NOMCLI,
                            PRODUTO = item.PRODUTO,
                            STA = item.STA,
                            TIPOATUALIZACAO = "I"
                        };
                        clienteSelecao.Add(cliente);
                    }

                    if (clientes.Count == 1)
                    {
                        Session["ClienteSelecao"] = clienteSelecao.SingleOrDefault();
                    }
                    else
                    {
                        Session["ClienteSelecao"] = clienteSelecao;
                    }

                    var qtdClientes = clienteSelecao.Count;
                    switch (qtdClientes)
                    {
                        case 1:
                            {
                                if (clienteSelecao[0].STA == "02")
                                {
                                    txtClienteAgrupamento.Text = "CLIENTE COM STATUS CANCELADO";
                                    txtClienteAgrupamento.Border.BorderColor = Color.Red;
                                    txtClienteAgrupamento.BackColor = Color.Salmon;
                                    btnIncluirCliente.ClientEnabled = false;
                                }
                                else
                                {
                                    if (listClienteSelecao.Where(x => x.SISTEMA == clienteSelecao[0].SISTEMA && x.CODCLI == clienteSelecao[0].CODCLI).ToList().Count == 0)
                                    {
                                        var clientePopUp = new CLIENTEAGRUPAMENTO
                                        {
                                            SISTEMA = clienteSelecao[0].SISTEMA,
                                            CODCLI = clienteSelecao[0].CODCLI,
                                            NOMCLI = clienteSelecao[0].NOMCLI,
                                            PRODUTO = clienteSelecao[0].PRODUTO,
                                            STA = clienteSelecao[0].STA,
                                            TIPOATUALIZACAO = "I"
                                        };
                                        Session["ClienteSelecao"] = clientePopUp;
                                        txtClienteAgrupamento.Text = "(" + Convert.ToString(clientePopUp.CODCLI) + ") " + clientePopUp.NOMCLI + " - " + clientePopUp.PRODUTO + " - " + (clientePopUp.TIPOCARTAO);
                                    }
                                    else
                                    {
                                        txtClienteAgrupamento.Text = "CLIENTE JA FOI INSERIDO";
                                        txtClienteAgrupamento.Border.BorderColor = Color.Red;
                                        txtClienteAgrupamento.BackColor = Color.Salmon;
                                        btnIncluirCliente.ClientEnabled = false;
                                    }
                                }
                            }
                            break;
                        case 2:
                            {
                                PopUpClientes(clienteSelecao);
                            }
                            break;
                        default:
                            {
                                txtClienteAgrupamento.Text = "CLIENTE NÃO ENCONTRADO";
                                txtClienteAgrupamento.Border.BorderColor = Color.Red;
                                txtClienteAgrupamento.BackColor = Color.Salmon;
                                btnIncluirCliente.ClientEnabled = false;
                            }
                            break;
                    }
                }
                else
                {
                    txtClienteAgrupamento.Text = string.Empty;
                }
            }
        }

        if ((Constantes.AcaoCadastro)Session["AcaoCadOpWebParc"] != Constantes.AcaoCadastro.acINSERINDO)
            rblTipoOperador.ReadOnly = true;

        if (Session["SelecaoCliente"] != null)
            selecaoCliente = (List<string>)Session["SelecaoCliente"];

        cbxAgrupamentoCliente.DataSource = null;
        cbxAgrupamentoCliente.DataSource = selecaoCliente;
        cbxAgrupamentoCliente.DataBind();
    }

    protected void PopUpClientes(List<CLIENTEAGRUPAMENTO> clientesUsu)
    {
        MenuItem menuGrupo = null;
        foreach (var cli in clientesUsu)
        {
            menuGrupo = new MenuItem();
            menuGrupo.Text = "(" + Convert.ToString(cli.CODCLI) + ") " + cli.NOMCLI + " - " + cli.PRODUTO + " - " + (cli.TIPOCARTAO);
            menuGrupo.Name = Convert.ToString(cli.CODCLI + "-" + cli.SISTEMA);
            menuSelCliente.Items.Add(menuGrupo);
        }
    }

    protected void grvOperadorParc_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
    {
        if (e.DataColumn.FieldName == "TIPOACESSO")
        {
            if (e.CellValue != null)
            {
                e.Cell.Text = e.CellValue.ToString() == "P" ? "PARCERIA" : "MÚLTIPLO";
            }
        }
    }

    protected void cplAgrupamento_Callback(object source, CallbackEventArgsBase e)
    {        
        int codParceria;
        int.TryParse(cbxParceria.Value.ToString(), out codParceria);
        var clientesParceria = new blCLIENTEVA(GetOperador()).OperadoresWEBParceriaClientes(codParceria);
        btnExcluirClienteAgrupamento.ClientVisible = false;
        cbxAgrupamentoCliente.DataSource = clientesParceria;
        cbxAgrupamentoCliente.DataBind();
    }
}