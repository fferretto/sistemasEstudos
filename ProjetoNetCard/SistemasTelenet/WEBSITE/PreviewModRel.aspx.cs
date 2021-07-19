using System;
using System.Collections.Generic;
using SIL.APIs.Model;
using SIL.BLL;
using TELENET.SIL.PO;

public partial class PreviewModRel : TelenetPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Relatorio

        if (Session["ResultRel"] == null) return;

        var DadosRel = (APIModRelPDF)Session["ResultRel"];

        List<MODREL> dadosMR = new List<MODREL>();
        var itemMR = new MODREL();
        foreach (var item in DadosRel.Cabecalho)
        {
            itemMR = new MODREL();
            itemMR.LINHAIMP = item.LINHAIMP.Replace("&nbsp"," ");
            itemMR.TIP = item.TIP;
            dadosMR.Add(itemMR);
        }
        foreach (var item in DadosRel.Conteudo)
        {
            itemMR = new MODREL();
            itemMR.LINHAIMP = item.LINHAIMP.Replace("&nbsp", " ");
            itemMR.TIP = item.TIP;
            dadosMR.Add(itemMR);
        }

        var erro = string.Empty;

        if (erro == string.Empty && dadosMR != null)
        {
            if (dadosMR.Count > 0)
            {
                string primeiraLinha = dadosMR[0].LINHAIMP;

                if (primeiraLinha.Length > 118)
                {
                    Redirect("~/PreviewModRelPaisagem.aspx");
                }
                rtvModRel.Report = new RelModeloText(dadosMR);
            }
        }
        else
        {
            erro = string.IsNullOrEmpty(erro) ? "Ocorreu um erro durante o processo, favor entrar em contato com o suporte." : erro;
            ASPxButton2.Visible = false;
            ASPxButton3.Visible = false;
            ReportToolbar1.Visible = false;
            Master.ExibeMensagem(erro);
        }

        #endregion

    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        Session["relvisto"] = 1;
        var idRel = ((List<PARAMETRO>)Session["Parametro"])[0].IDREL;
        Session["idRel"] = idRel;

        if (idRel == 0)
        {
            Redirect("SelRelParametros.aspx");
        }
        else
        {
            Redirect(string.Format("SelRelParametros.aspx?IdRel={0}", idRel));
        }
    }
    protected void btnFecharRel_Click(object sender, EventArgs e)
    {
        Redirect("~/Default.aspx");
    }
}