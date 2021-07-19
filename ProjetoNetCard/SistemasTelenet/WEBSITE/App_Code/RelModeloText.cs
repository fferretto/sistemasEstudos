using System.Text;
using DevExpress.XtraReports.UI;
using TELENET.SIL.PO;
using System.Collections.Generic;

/// <summary>
/// Summary description for RelCredenciadosFechadosSintetico
/// </summary>
public class RelModeloText : XtraReport
{
    private DetailBand Detail;
    private XRLabel lblRetorno;
    private FormattingRule formattingRule1;
    private PageHeaderBand PageHeader;
    private XRLabel lblCabecalho;
    private XRPageInfo lblPage;
    private XRPageInfo lblDataHora;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public RelModeloText()
    {
        InitializeComponent();
       
    }

    public RelModeloText(List<MODREL> dados)
    {
        if(dados == null) return;
        InitializeComponent();
        Landscape = true;
        var dadosCabecalho = dados.FindAll(d => d.TIP == 0);
        if (dadosCabecalho.Count > 0)
        {
            var cabecalho = new StringBuilder();
            foreach (var fechcredVA in dadosCabecalho)
                cabecalho.AppendLine(fechcredVA.LINHAIMP);
            lblCabecalho.Text = cabecalho.ToString();
        }
        else
        {
            lblCabecalho.Visible = false;
            lblDataHora.Visible = false;
            lblPage.Visible = false;
        }
        DataSource = dados.FindAll(d => d.TIP > 0);
        lblRetorno.DataBindings.Add(new XRBinding("Text", null, "LINHAIMP"));
        //lblRetorno.Text = "999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.99999999";
        //lblCabecalho.Text = "999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.999999999.99999999";
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.Detail = new DevExpress.XtraReports.UI.DetailBand();
        this.lblRetorno = new DevExpress.XtraReports.UI.XRLabel();
        this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
        this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
        this.lblDataHora = new DevExpress.XtraReports.UI.XRPageInfo();
        this.lblPage = new DevExpress.XtraReports.UI.XRPageInfo();
        this.lblCabecalho = new DevExpress.XtraReports.UI.XRLabel();
        ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
        // 
        // Detail
        // 
        this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblRetorno});
        this.Detail.Height = 18;
        this.Detail.MultiColumn.ColumnSpacing = 10F;
        this.Detail.MultiColumn.ColumnWidth = 355F;
        this.Detail.Name = "Detail";
        this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.Detail.RepeatCountOnEmptyDataSource = 12;
        this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // lblRetorno
        // 
        this.lblRetorno.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblRetorno.Location = new System.Drawing.Point(8, 0);
        this.lblRetorno.Name = "lblRetorno";
        this.lblRetorno.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.lblRetorno.Size = new System.Drawing.Size(892, 17);
        this.lblRetorno.StylePriority.UseFont = false;
        this.lblRetorno.StylePriority.UseTextAlignment = false;        
        this.lblRetorno.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // formattingRule1
        // 
        this.formattingRule1.Name = "formattingRule1";
        // 
        // PageHeader
        // 
        this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] { this.lblDataHora, this.lblPage, this.lblCabecalho});
        this.PageHeader.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.PageHeader.Height = 34;
        this.PageHeader.Name = "PageHeader";
        this.PageHeader.StylePriority.UseFont = false;
        // 
        // lblDataHora
        // 
        this.lblDataHora.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblDataHora.Format = "{0:dd/MM/yyyy HH:mm:ss}";
        this.lblDataHora.Location = new System.Drawing.Point(8, 0);
        this.lblDataHora.Name = "lblDataHora";
        this.lblDataHora.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.lblDataHora.PageInfo = DevExpress.XtraPrinting.PageInfo.DateTime;
        this.lblDataHora.Size = new System.Drawing.Size(154, 15);
        this.lblDataHora.StylePriority.UseFont = false;
        // 
        // lblPage
        // 
        this.lblPage.Font = new System.Drawing.Font("Courier New", 9F);
        this.lblPage.Format = "PAG. {0}";
        this.lblPage.Location = new System.Drawing.Point(845, 0);
        this.lblPage.Name = "lblPage";
        this.lblPage.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
        this.lblPage.Size = new System.Drawing.Size(50, 15);
        this.lblPage.StylePriority.UseFont = false;
        this.lblPage.StylePriority.UseTextAlignment = false;
        this.lblPage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
        // 
        // lblCabecalho
        // 
        this.lblCabecalho.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblCabecalho.Location = new System.Drawing.Point(8, 17);
        this.lblCabecalho.Multiline = true;
        this.lblCabecalho.Name = "lblCabecalho";
        this.lblCabecalho.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
        this.lblCabecalho.Size = new System.Drawing.Size(892, 17);
        this.lblCabecalho.StylePriority.UseFont = false;
        this.lblCabecalho.StylePriority.UseTextAlignment = false;
        this.lblCabecalho.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
        // 
        // RelModeloText
        // 
        this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {this.Detail, this.PageHeader});
        this.DefaultPrinterSettingsUsing.UseLandscape = true;
        this.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] { this.formattingRule1});
        this.GridSize = new System.Drawing.Size(4, 4);
        this.Margins = new System.Drawing.Printing.Margins(10, 0, 20, 0);
        this.PageHeight = 1268;
        this.PageWidth = 929;
        this.PaperKind = System.Drawing.Printing.PaperKind.A4Extra;
        this.Version = "8.3";
        ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}
