using System;
using System.Configuration;
using TELENET.SIL;
using TELENET.SIL.BL;
using System.Text.RegularExpressions;
using Telenet.Core.Authorization;
using Telenet.Core.DependencyInjection;

public partial class Login : TelenetPage
{
    private bool EBrowseValido()
    {
        var flag = ConfigurationManager.AppSettings["verificarCompatibilidadeBrowse"];
        var verificar = true;

        if (!string.IsNullOrEmpty(flag))
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings["verificarCompatibilidadeBrowse"], out verificar))
                verificar = true;
        }

        if (!verificar) return true;

        var navegador = Context.Request.Browser.Browser ?? "";
        var valido = true;

        if (!Context.Request.Browser.IsBrowser("Chrome"))
        {
            valido = false;
        }
        else
        {
            var match = Regex.Match(Context.Request.UserAgent, "(Edge|OPR|Opera)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                navegador = match.Value;
                valido = false;
            }
        }

        if (!valido)
        {
            form1.Visible = false;
            Response.ContentType = "text/html";
            Response.Write(string.Format(Resources.Resource.MensagemNavegadorNaoAutorizado, navegador));
            Response.Flush();
            Response.End();
        }

        return valido;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!EBrowseValido())
        {
            return;
        }
        var ret = this.Request.QueryString["ret"];
        if (!IsPostBack)
        {
            if (Session.Count > 0 && (string)Session["MsgErroAutenticacao"] == string.Empty)
            {
                var sessionId = Session.GetSessionId();

                if (string.IsNullOrEmpty(sessionId))
                {
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    Redirect("~/Default.aspx");
                }
            }
        }
        if (!string.IsNullOrEmpty(ret))
        {
            lblErroAut.Text = ret;
        }
        else
        {
            lblErroAut.Text = string.Empty;
        }
    }

    private void RedirectWithSessionId(string pageName, string sessionId)
    {
        Session.SaveSession();
        Response.Redirect(string.Format("~/{0}?sid={1}", pageName, sessionId));
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        UtilSIL.ValidaUserAgent(Request.UserAgent);

        var blOperador = new blOperador();
        var Acao = string.Empty;
        var MsgErro = blOperador.Autenticar2(edtLogin.Text.ToLower(), edtSenha.Text, out Acao);
        var ControleAcessoBL = new blControleAcessoVA(blOperador.Operador);

        if (Acao == "OK" || Acao == "Avisar")
        {
            blOperador.Operador.HABAGRUPAMENTO = ControleAcessoBL.HabilitaAgrupamento();
            if (blOperador.Operador.HABAGRUPAMENTO)
                blOperador.Operador.CODAG = ControleAcessoBL.ConsultaAgrupamento(blOperador.Operador.IDPERFIL);

            blOperador.Operador.NUMCONSREGTRANS = ControleAcessoBL.NumRegConsultaTransacao();

            try
            {
                var authorization = ServiceConfiguration
                    .ServiceProvider
                    .GetService<IUserAuthorization<NetCardAuthorizationContext>>();

                authorization.Authenticate(
                    string.Concat(edtLogin.Text.ToLower(), "@operador"),
                    edtSenha.Text);
            }
            catch (Exception exception)
            {
            }
        }

        if (Session["Operador"] != null)
            Session.Remove("Operador");
        Session["Operador"] = blOperador.Operador;

        var sessionId = Session.CreateSession();

        switch (Acao)
        {
            case "OK":
                {
                    Session["ColecaoControleForm"] = new blControleAcesso(blOperador.Operador).ColecaoControleAcesso(blOperador.Operador.IDPERFIL, "S");
                    Session["MsgErroAutenticacao"] = "Usuário autenticado com sucesso!";
                    RedirectWithSessionId("Default.aspx", sessionId);
                }
                break;
            case "PrimeiroAcesso":
                {
                    Session["MsgErroAutenticacao"] = "Primeiro acesso ao sistema, favor alterar a senha.";
                    Session["Login"] = edtLogin.Text;
                    Session["Senha"] = edtSenha.Text;
                    RedirectWithSessionId("AlteraSenha.aspx", sessionId);
                }
                break;
            case "Avisar":
                {
                    Session["MsgErroAutenticacao"] = "Senha a expirar, sugerimos que efetue a troca.";
                    Session["ColecaoControleForm"] = new blControleAcesso(blOperador.Operador).ColecaoControleAcesso(blOperador.Operador.IDPERFIL, "S");                    
                    RedirectWithSessionId("Default.aspx", sessionId);
                    //RedirectWithSessionId("AlteraSenha.aspx", sessionId);
                }
                break;
            case "Expirar":
                {
                    Session["MsgErroAutenticacao"] = "Senha expirada, favor entar em contato com o suporte.";
                    Response.Redirect("~/Login.aspx");
                }
                break;
            default:
                {
                    Session["MsgErroAutenticacao"] = MsgErro;
                    Response.Redirect("~/Login.aspx?ret=" + MsgErro);
                }
                break;
        }
    }
}