using System;
using System.Collections.Generic;
using SIL.BLL;
using TELENET.SIL.PO;
using System.Text;
using System.IO;
using SIL.APIs.Model;

public partial class Reports_PreviewModRelPaisagem : TelenetPage
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
            itemMR.LINHAIMP = item.LINHAIMP;
            itemMR.TIP = item.TIP;
            dadosMR.Add(itemMR);
        }
        foreach (var item in DadosRel.Conteudo)
        {
            itemMR = new MODREL();
            itemMR.LINHAIMP = item.LINHAIMP;
            itemMR.TIP = item.TIP;
            dadosMR.Add(itemMR);
        }
        
        if (dadosMR != null)
            rtvModRelPaisagem.Report = new RelModeloTextPaisagem(dadosMR);
        

        #endregion

    }
    protected void ASPxButton1_Click(object sender, EventArgs e)
    {
        Session["relvisto"] = 1;
        var idRel = ((List<PARAMETRO>)Session["Parametro"])[0].IDREL;
        Session["idRel"] = idRel;
        Redirect("SelRelParametros.aspx");
    }
    protected void btnFecharRel_Click(object sender, EventArgs e)
    {
        Redirect("~/Default.aspx");
    }
}