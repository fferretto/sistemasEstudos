using System;
using TELENET.SIL.PO;
using TELENET.SIL.BL;

public partial class TrocarSenha : TelenetPage
{
    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        var blOperador = new blOperador();
        var MsgErro = string.Empty;

        if (edtNovaSenha.Text != edtConfNovaSenha.Text)
        {
            MsgErro = "Os campos Nova Senha e Confirma Nova Senha devem ser iguais.";
        }
        else
        {
            MsgErro = blOperador.AlterarSenha(GetOperador().LOGIN, edtSenhaAtual.Text, edtNovaSenha.Text);
            Session["MsgErroAutenticacao"] = MsgErro;
            if (Session["Operador"] != null)
                Session.Remove("Operador");
            Session["Operador"] = blOperador.Operador;
            if (MsgErro == string.Empty)
            {
                var ControleAcessoBL = new blControleAcessoVA(blOperador.Operador);
                Session["ColecaoControleForm"] = ControleAcessoBL.ColecaoControleAcessoVA(blOperador.Operador.IDPERFIL, "S");
            }
        }
        Redirect(MsgErro == string.Empty ? "~/Default.aspx" : "~/TrocarSenha.aspx");


        //blVopervaws VopervawsBL = new blVopervaws(GetOperador());
        //OPERADORA Operador = new OPERADORA();
        //Operador = (OPERADORA)Session["Operador"];

        //if ((edtSenhaAtual.Text == String.Empty) || (edtNovaSenha.Text == String.Empty) || (edtConfNovaSenha.Text == String.Empty))
        //    Master.ExibeMensagem("Informe todos os campos.");
        //else if (edtNovaSenha.Text.ToLower() != edtConfNovaSenha.Text.ToLower())
        //    Master.ExibeMensagem("Nova Senha nao confere com a Confirmacao de Senha.");
        //else if (Operador.SENHA.ToLower() != edtSenhaAtual.Text.ToLower())
        //    Master.ExibeMensagem("Senha atual nao confere.");
        //else
        //{
        //    VopervawsBL.AlteraSenha(edtNovaSenha.Text);

        //    Operador.SENHA = edtNovaSenha.Text;
        //    Session["Operador"] = Operador;

        //    edtSenhaAtual.Text = string.Empty;
        //    edtNovaSenha.Text = string.Empty;
        //    edtConfNovaSenha.Text = string.Empty;
        //    Master.ExibeMensagem("Senha alterada com sucesso.");

    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Redirect("~/Default.aspx");
    }
}