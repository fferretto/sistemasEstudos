using System;
using TELENET.SIL;
using TELENET.SIL.BL;
using TELENET.SIL.PO;
using Sil.Criptografia;
using System.Web;

public partial class SelCredenciadoGrupo : System.Web.UI.Page
{
    private const string CACHECREDENCIADOS = "ListaCredenciadosForaGrupo";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        Page.ClientScript.RegisterClientScriptInclude("GrupoCred", Page.ResolveVersionScript("GrupoCredenciados.js"));
        Page.ClientScript.RegisterClientScriptInclude("Utils", Page.ResolveVersionScript("Utils.js"));
        if (!IsPostBack)
        {
            var fOperador = GetOperador();
            if (fOperador != null)
            {
                var bdtelenet = string.Format(ConstantesSIL.BDTELENET, fOperador.SERVIDORNC, fOperador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
                string key = "aifargotpirC";
                var crip = new Criptografia(CryptProvider.TripleDES);
                crip.Key = key;
                hd.Value = System.Web.HttpContext.Current.Server.UrlEncode(crip.Encrypt(bdtelenet));              
            }
            hdGrupo.Value = Request.QueryString["grupo"];
            lblCabecalho.Text = "NETCARD VA :: Inclusao de Credenciados para o Grupo " + Request.QueryString["nome"];
            ddlUf.SelectedIndex = -1;
        }
    }

    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }    

    private void AplicarFiltro()
    {
        if (GetOperador() == null) return;
        if (string.IsNullOrEmpty(hdGrupo.Value)) return;
        var bll = new blCredenciadoVA(GetOperador());
        Session[CACHECREDENCIADOS] = bll.ColecaoCredenciadosForaDoGrupo(hdGrupo.Value, txtBairro.Text, txtCodigo.Text,
                                                                        txtLocalidade.Text, txtSegmento.Text, ddlUf.Text);
        grvCredenciados.DataSource = Session[CACHECREDENCIADOS];
        grvCredenciados.DataBind();
        grvCredenciados.FocusedRowIndex = -1;
    }
    
    protected void grvCredenciados_CustomCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs e)
    {
        if (GetOperador() == null) return;
        if (e.Parameters == "cadCredGrupo")
            CadastraCredenciadosGrupo();
        else
            AplicarFiltro();
    }

    protected void grvCredenciados_PageIndexChanged(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        if (Session[CACHECREDENCIADOS] != null)
        {
            grvCredenciados.DataSource = Session[CACHECREDENCIADOS];
            grvCredenciados.DataBind();
            grvCredenciados.FocusedRowIndex = -1;
        }
    }

    protected void btnSalvarConsulta_Click(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        CadastraCredenciadosGrupo();
        AplicarFiltro();
    }   

    private void CadastraCredenciadosGrupo()
    {
        if (GetOperador() == null) return;
        var listaCredenciados = grvCredenciados.GetSelectedFieldValues("CODCRE");
        grvCredenciados.DataBind();
        grvCredenciados.Selection.UnselectAll();
        if (listaCredenciados == null || listaCredenciados.Count < 1 || hdGrupo.Value == string.Empty) return;
        var bll = new blCredenciadoVANovo(GetOperador());

        string retMensagem;
        bll.IncluirCredenciadosGrupo(hdGrupo.Value, listaCredenciados, out retMensagem);

        ExibeMensagem(retMensagem);
    }
    protected new void ExibeMensagem(string texto)
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
}
