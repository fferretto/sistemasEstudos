using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxMenu;
using SIL.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TELENET.SIL.BL;
using TELENET.SIL.PO;
using MenuItem = DevExpress.Web.ASPxMenu.MenuItem;

public partial class Relatorio : System.Web.UI.MasterPage
{
    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Operador"] == null)
        {
            if (Page.IsCallback)
                ASPxWebControl.RedirectOnCallback("~/Login.aspx");
            else
                Response.Redirect("~/Login.aspx"); // Forca Autenticacao
            return;
        }

        BuildMenuRelatorios(mnuTop);
        AplicaPermissoes();
        AplicarPermissoesMenu();        
    }

    private static MenuItem CreateMenuItem(RELATORIO relatorio)
    {
        var menu = new MenuItem { Name = relatorio.TIPREL, Text = relatorio.NOMREL, ToolTip = relatorio.DESCRICAO };
        if (string.IsNullOrEmpty(relatorio.PROCREL))
            menu.NavigateUrl = relatorio.NAVIGATEURL;
        else
            menu.NavigateUrl = "~/SelRelParametros.aspx?IdRel=" + relatorio.IDREL;
        return menu;
    }

    protected void BuildMenuRelatorios(ASPxMenu menuPrincipal)
    {
        if (menuPrincipal == null) return;
        var menuPrincipalRelatorios = menuPrincipal.Items[4];
        if (menuPrincipalRelatorios == null) return;
        var listaRelatorios = (GetOperador().HABAGRUPAMENTO && GetOperador().CODAG > 0) ?
            new blModRel((OPERADORA)Session["Operador"]).ListaRelatorio(GetOperador().CODAG) :
            new blModRel((OPERADORA)Session["Operador"]).ListaRelatorio();

        if (listaRelatorios.Count <= 0) return;
        var criouMenu = false;
        MenuItem menuGrupo = null;
        var grupo = listaRelatorios[0].TIPREL;

        foreach (var relatorio in listaRelatorios)
        {
            if (relatorio.TIPREL == string.Empty)
            {
                CreateMenuItem(relatorio);
                break;
            }
            var grupoAux = relatorio.TIPREL;
            if (grupoAux == grupo)
            {
                if (!criouMenu)
                {
                    menuGrupo = new MenuItem { Name = grupoAux, Text = grupoAux, ToolTip = grupoAux };
                    menuPrincipalRelatorios.Items.Add(menuGrupo);
                    criouMenu = true;
                }
                menuGrupo.Items.Add(CreateMenuItem(relatorio));
            }
            else
            {
                grupo = relatorio.TIPREL;
                menuGrupo = new MenuItem { Name = grupo, Text = grupo, ToolTip = grupo };
                menuPrincipalRelatorios.Items.Add(menuGrupo);
                menuGrupo.Items.Add(CreateMenuItem(relatorio));
            }
        }
    }

    #region Mensagens

    public void ExibeMensagem(string Mensagem)
    {
        var ScriptMsg = new StringBuilder();
        ScriptMsg.Append("alert('" + Mensagem + "');");
        ScriptManager.RegisterStartupScript(Page, GetType(), "alert", ScriptMsg.ToString(), true);
    }

    #endregion

    #region AplicaRestricoes

    private string[] _enabledControls = new string[] 
    {
        "navMenu",
        "ppcEncerrar",
        "ppcMensagem",
        "btnCancel",
        "btnSim",
        "btnNao",
        "lblMensagemAguarde",
        "lblMensagemSimNao",
        "lblMensagem",
        "loadingPanel",
        "ppcAcaoAux"
    };

    private string[] _enabledMenus = new string[] 
    {
        "navMenu",
        "EncerrarSessao",
        "Inicio",
        "Operadores",
        "Cadastros",
        "Tarefas",
        "Consultas",
        "Modulo_Relatorios",
        "Logoff"        
    };

    private bool EControleEdicao(WebControl control)
    {
        if (control == null)
            return false;

        return (new Type[] {
            typeof(ASPxTextBox),
            typeof(ASPxComboBox),
            typeof(ASPxCheckBox),
            typeof(ASPxRadioButton),
            typeof(ASPxButton),
            typeof(TextBox),
            typeof(ASPxGridView)
        }).Contains(control.GetType());
    }

    private void DisableControls(ControlCollection controls)
    {
        foreach (Control control in controls)
        {
            var webControl = control as WebControl;

            if (EControleEdicao(webControl) &&
                (((System.Web.UI.Control)(webControl)).TemplateControl).ToString() != "ASP.selrelparametros_aspx" &&
                (((System.Web.UI.Control)(webControl)).TemplateControl).ToString() != "ASP.previewmodrel_aspx" &&
                (((System.Web.UI.Control)(webControl)).TemplateControl).ToString() != "ASP.previewmodrelpaisagem_aspx" &&
                (((System.Web.UI.Control)(webControl)).TemplateControl).ToString() != "ASP.resultconstransacao_aspx"
                )
            {
                webControl.Enabled = _enabledControls.Any(c => c == webControl.ID);                
            }
            DisableControls(control.Controls);
        }
    }

    private void DisabledMenuItems()
    {
        foreach (MenuItem itemMenu in mnuTop.Items)
        {
            itemMenu.Enabled = _enabledMenus.Any(c => c == itemMenu.Name);

            foreach (MenuItem itemMenuFilho1 in itemMenu.Items)
            {
                itemMenuFilho1.Enabled = _enabledMenus.Any(c => c == itemMenuFilho1.Name);
                if (itemMenuFilho1.Items != null)
                {
                    foreach (MenuItem itemMenuFilho2 in itemMenuFilho1.Items)
                    {
                        itemMenuFilho2.Enabled = _enabledMenus.Any(c => c == itemMenuFilho2.Name);
                        if (itemMenuFilho2.Items != null)
                        {
                            foreach (MenuItem itemMenuFilho3 in itemMenuFilho2.Items)
                            {
                                itemMenuFilho3.Enabled = _enabledMenus.Any(c => c == itemMenuFilho3.Name);
                                itemMenuFilho3.NavigateUrl = InsereSessionId(itemMenuFilho3.NavigateUrl);
                            }
                        }

                        itemMenuFilho2.NavigateUrl = InsereSessionId(itemMenuFilho2.NavigateUrl);
                    }
                }

                itemMenuFilho1.NavigateUrl = InsereSessionId(itemMenuFilho1.NavigateUrl);
            }

            if (itemMenu.NavigateUrl.IndexOf("Default.aspx") > -1)
                itemMenu.NavigateUrl = InsereSessionId(itemMenu.NavigateUrl);
        }
    }

    private string InsereSessionId(string url)
    {
        if (string.IsNullOrEmpty(url) || url.IndexOf("sid") > -1)
            return url;

        if (url.IndexOf('?') > -1)
        {
            return url = url + "&sid=" + Request.QueryString["sid"];
        }

        return url = url + "?sid=" + Request.QueryString["sid"];
    }

    public void AplicaPermissoes()
    {        
        var strPaginaAtual = Request.Path;
        strPaginaAtual = strPaginaAtual.Remove(0, strPaginaAtual.LastIndexOf("/", StringComparison.Ordinal) + 1);
        strPaginaAtual = strPaginaAtual.Substring(0, strPaginaAtual.Length - 5);
        var paginaAtual = Page.ToString().Substring(4).Replace("_aspx", "").ToUpper();
        DisableControls(Controls);
        DisabledMenuItems();
        var colecaoPermissao = new blControleAcesso((OPERADORA)Session["Operador"]).ColecaoControleAcesso("N", paginaAtual);
        foreach (var permissao in colecaoPermissao)
            AplicarPermissoesControle(Page, permissao.DESCRICAO, permissao.TIPOCONTROLE);
    }

    public void AplicarPermissoesMenu()
    {
        mnuTop.Items[1].Items[3].Visible = new blCredenciadoVA((OPERADORA)Session["Operador"]).ExibeModuloTaxaCre();
        var colecaoControleForm = (List<PERMISSAOACESSO>)Session["ColecaoControleForm"];

        //ATENÇÃO !!!!!! GAMBIARRA - Retira dos perfis abaixo o módulo de relatórios na BRASIL CONVÊNCIOS. 
        if (GetOperador().NOMEOPERADORA.Contains("BRASIL"))
        {
            if (GetOperador().IDPERFIL == 19 || GetOperador().IDPERFIL == 20)
            {
                foreach (MenuItem mn in mnuTop.Items[4].Items)
                    mn.ClientEnabled = true;
            }
        }

        //Aplica permissoes ao menu do topo da tela
        foreach (MenuItem itemMenu in mnuTop.Items)
        {
            if (colecaoControleForm.Exists(c => c.DESCRICAO == itemMenu.Name))
                itemMenu.Enabled = true;

            foreach (MenuItem itemMenuFilho in itemMenu.Items)
            {
                if (colecaoControleForm.Exists(c => c.DESCRICAO == itemMenuFilho.Name))
                    itemMenuFilho.Enabled = true;

                foreach (MenuItem itemMenuNeto in itemMenuFilho.Items)
                {
                    if (colecaoControleForm.Exists(c => c.DESCRICAO == itemMenuNeto.Name))
                        itemMenuNeto.Enabled = true;
                }
            }
        }
    }

    void AplicarPermissoesControle(Control Controle, string NomeControle, string tipoControle)
    {
        if (Controle is WebControl)
        {
            if ((Controle as WebControl).ID == NomeControle)
            {
                switch (tipoControle)
                {
                    case "Habilitado":
                        {
                            (Controle as WebControl).Enabled = true;
                        }
                        break;
                    case "Visivel":
                        {
                            (Controle as WebControl).Visible = true;
                        }
                        break;
                    default:
                        {
                            (Controle as WebControl).Enabled = false;
                            (Controle as WebControl).CssClass = "EditDesabilitado";
                        }
                        break;
                }
            }
        }

        // Childs do Controle
        foreach (Control controleChild in Controle.Controls)
            AplicarPermissoesControle(controleChild, NomeControle, tipoControle);
    }


    #endregion
}
