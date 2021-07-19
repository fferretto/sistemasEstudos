using System;
using System.Web;
using System.Web.UI;

/// <summary>
/// Implementa a página ancestral das página da Telenet.
/// </summary>
public class TelenetPage : System.Web.UI.Page
{
    public TelenetPage()
    {
        SessionId = HttpContext.Current.Request.QueryString["sid"];
    }

    protected string SessionId { get; private set; }

    /// <summary>
    /// Exibe uma mensagem na tela.
    /// </summary>
    protected void ExibeMensagem(string texto)
    {
        HttpCookie cookie = Request.Cookies["MsgRetorno"];
        if (cookie == null)
        {
            cookie = new HttpCookie("MsgRetorno");
            cookie["Mensagem"] = texto;
        }
        else
        {
            var valCk = cookie["Mensagem"] + "<br />" + texto;
            cookie["Mensagem"] = valCk;
        }
        cookie.Expires = DateTime.Now.AddSeconds(1);
        Response.Cookies.Add(cookie);
    }

    /// <summary>
    /// Exibe uma mensagem de erro na tela.
    /// </summary>
    protected void ExibeMensagem(Exception ex)
    {
        HttpCookie cookie = Request.Cookies["MsgErro"];
        if (cookie == null)
        {
            cookie = new HttpCookie("MsgErro");
            cookie["Mensagem"] = ex.Message;
        }
        else
        {
            var valCk = cookie["Mensagem"] + "<br />" + ex.Message;
            cookie["Mensagem"] = valCk;
        }
        cookie.Expires = DateTime.Now.AddSeconds(1);
        Response.Cookies.Add(cookie);
    }

    /// <summary>
    /// Executa as operações de pré inicialização da página atual.
    /// </summary>
    protected virtual void Page_Init(object sender, EventArgs e)
    {
        Session.LoadSession();
        ScriptManager.RegisterStartupScript(this, GetType(), "AplicarMascarasJuncao", "$(document).ready(function () { AplicarMascarasJuncao(); });", true);
    }

    /// <summary>
    /// Exibe uma mensagem de erro na tela.
    /// </summary>
    protected virtual void ShowErrorMessage(string message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "showErro", string.Format("ExibeMensagemNovo('{0}');", message), true);
    }

    /// <summary>
    /// Executa as operações de pré descarregamento da página atual.
    /// </summary>
    protected virtual void Page_Unload(object sender, EventArgs e)
    { }

    /// <summary>
    /// Efetua um redirect inserindo o sid na querystring.
    /// </summary>
    protected void Redirect(string url)
    {
        if (!string.IsNullOrEmpty(SessionId) && !url.Contains("sid"))
        {
            // Acrescentamos o sid de acordo com a url atual.
            url = url.Contains(".aspx?")
                ? url = url + "&sid=" + SessionId
                : url = url + "?sid=" + SessionId;
        }

        Response.Redirect(url);
    }

    /// <summary>
    /// Aplica as restrições customizadas de acesso do usuário logado para a página atual.
    /// </summary>
    public virtual void AplicarRestricoesCustomizadasControle(Control control)
    { }

    public override void Dispose()
    {
        Session.SaveSession();
        base.Dispose();
    }
}