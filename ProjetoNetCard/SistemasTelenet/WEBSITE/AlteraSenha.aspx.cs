using System;
using System.Configuration;
using System.Web.UI;
using TELENET.SIL.BL;

public partial class AlteraSenha : TelenetPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MsgErroAutenticacao"] != null)
        {
            edtLogin.Text = (string)Session["Login"];
            edtSenha.Text = (string)Session["Senha"];
            lblErroAut.Text = (string)Session["MsgErroAutenticacao"];
        }
        else
            lblErroAut.Text = string.Empty;
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        var blOperador = new blOperador();
        var MsgErro = string.Empty;        

        if (edtNovaSenha.Text != edtConfNovaSenha.Text)
        {
            MsgErro = "Os campos Nova Senha e Confirma Nova Senha devem ser iguais.";
        }
        else
        {
            MsgErro = blOperador.AlterarSenha(edtLogin.Text.ToLower(), edtSenha.Text, edtNovaSenha.Text);
            Session["MsgErroAutenticacao"] = MsgErro;

            var ControleAcessoBL = new blControleAcessoVA(blOperador.Operador);
            if (MsgErro == string.Empty)
            {
                blOperador.Operador.HABAGRUPAMENTO = ControleAcessoBL.HabilitaAgrupamento();
                if (blOperador.Operador.HABAGRUPAMENTO)
                    blOperador.Operador.CODAG = ControleAcessoBL.ConsultaAgrupamento(blOperador.Operador.IDPERFIL);

                blOperador.Operador.NUMCONSREGTRANS = ControleAcessoBL.NumRegConsultaTransacao();
            }

            if (Session["Operador"] != null)
                Session.Remove("Operador");
            Session["Operador"] = blOperador.Operador;
            if (MsgErro == string.Empty)
            {
                Session["ColecaoControleForm"] = new blControleAcesso(blOperador.Operador).ColecaoControleAcesso(blOperador.Operador.IDPERFIL, "S");
            }
        }
        Redirect(MsgErro == string.Empty ? "~/Default.aspx" : "~/AlteraSenha.aspx");
    }   

}