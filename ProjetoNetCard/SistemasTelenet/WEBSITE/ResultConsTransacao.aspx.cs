using System;
using System.Globalization;
using System.Web.UI.WebControls;
using TELENET.SIL.PO;
using TELENET.SIL;
using SIL.BLL;
using SIL.BL;
using System.IO;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using System.Collections.Generic;

public partial class ResultConsTransacao : TelenetPage
{
    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }

    protected void CriarInstancia(object sender, ObjectDataSourceEventArgs e)
    {
        if (GetOperador() == null) return;
        e.ObjectInstance = new blTransacao(GetOperador());
    }

    protected override void Page_Init(object sender, EventArgs e)
    {
        Session.LoadSession();
        var consulta = Session[UtilSIL.DADOSPARACONSULTA] as CONSULTA_VA;
        var numMaxConsTrans = GetOperador().NUMCONSREGTRANS;

        if (new blTransacao(GetOperador()).ObterNumeroTransacoes(consulta) > numMaxConsTrans)
        {
            ExibeMensagem("A pesquisa retornou uma quantidade muito grande de registros e nao pode ser exibida. Favor modificar os filtros da consulta.");
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "contadorConsulta", "ExibeMensagem(); window.btnVoltarFiltros.DoClick();", true);
        }

        #region Regiao

        var filtros = (CONSULTA_VA)Session["DADOSPARACONSULTA"];
        var resultado = (List<CTTRANSVA>)Session["RESULCONSTRANSACAO"];

        if (resultado == null)
        {
            resultado = new blTransacao(GetOperador()).GerarConsultaTransacao(filtros);
            Session["RESULCONSTRANSACAO"] = resultado;
        }        

        grvResultado.DataSource = resultado;    //obd;
        grvResultado.DataBind();
        #endregion
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        Title = GetOperador().NOMEOPERADORA + Constantes.tituloConsultaTrans;
        Page.ClientScript.RegisterClientScriptInclude("Consultas", Page.ResolveVersionScript("Consultas.js"));
        if (!IsPostBack) GenerateColluns();
    }

    private void GenerateColluns()
    {
        var filtro = Session[UtilSIL.DADOSPARACONSULTA] as CONSULTA_VA;

        if (filtro == null)
        {
            return;
        }

        var bllCons = new blConsultaVA(GetOperador(), filtro.SISTEMA);
        var colunasVisualizadas = bllCons.ColunasVisualizadas(filtro.LISTA_COL);
        var listaColunas = bllCons.GeraColunas(colunasVisualizadas);
        if (listaColunas == null || listaColunas.Count == 0)
        {
            grvResultado.Visible = false;
            return;
        }

        // Tem de renomear as colunas no PJ no resultSet, pq elas vêem com o nome do label da coluna
        foreach (var colum in listaColunas) grvResultado.Columns.Add(colum);
        grvResultado.KeyFieldName = "DATTRA";
        grvResultado.Columns["NSUAUT"].Visible = false;
        grvResultado.Columns["NSUHOS"].Visible = false;
        grvResultado.Columns["DATTRA"].Visible = false;
        grvResultado.Columns["CODRTA"].Visible = false;
        grvResultado.Columns["DATFECCRE"].Visible = false;
        grvResultado.Columns["TIPTRA"].Visible = false;
        grvResultado.Columns["FLAG_AUT"].Visible = false;
        grvResultado.Columns["DAD_JUST"].Visible = false;

        

        if (colunasVisualizadas != null)
            foreach (var item in colunasVisualizadas)
            {
                switch (item.Value)
                {
                    case "DATTRA": grvResultado.Columns["DATTRA"].Visible = true; break;
                    case "NSUAUT": grvResultado.Columns["NSUAUT"].Visible = true; break;
                    case "NSUHOS": grvResultado.Columns["NSUHOS"].Visible = true; break;
                    case "CODRTA": grvResultado.Columns["CODRTA"].Visible = true; break;
                    case "DATFECCRE": grvResultado.Columns["DATFECCRE"].Visible = true; break;
                    case "TIPTRA": grvResultado.Columns["TIPTRA"].Visible = true; break;
                }
            }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        var fileName = string.Empty;
        var pathArquivo = Server.MapPath(@"~/App_Data/Upload/");
        if (Session[UtilSIL.DADOSPARACONSULTA] != null)
        {
            var consulta = (CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA];
            var result = (List<CTTRANSVA>)Session["RESULCONSTRANSACAO"];

            if (result == null)
            {
                result = new blTransacao(GetOperador()).GerarConsultaTransacao(consulta);
                Session["RESULCONSTRANSACAO"] = result;
            }
            fileName = new blConsultaVA(GetOperador(), consulta.SISTEMA).GeraExcel((CONSULTA_VA)Session[UtilSIL.DADOSPARACONSULTA], result, pathArquivo);
        }
        if (string.IsNullOrEmpty(fileName)) return;
        byte[] buffer;
        using (var myFileStream = new FileStream(pathArquivo + fileName, FileMode.Open))
        {
            var size = myFileStream.Length;
            buffer = new byte[size];
            myFileStream.Read(buffer, 0, int.Parse(myFileStream.Length.ToString(CultureInfo.InvariantCulture)));
        }
        Response.ContentType = "text/xls";
        Response.AddHeader("content-disposition", "attachment; filename=ListagemTransacoes.xls");
        Response.BinaryWrite(buffer);
        Response.Flush();
        Response.End();
    }

    protected void btnVoltarFiltros_Click(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        Session[UtilSIL.ISRETORNORESULTCONSULTA] = true;
        Master.Transfer("SelConsTransacao.aspx", true);
    }

    #region Tratamento botoes para transacoes

    protected void clbBotoes_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {

        var filtro = Session[UtilSIL.DADOSPARACONSULTA] as CONSULTA_VA;
        if (GetOperador() == null) return;
        if (e.Parameter == "Listar")
            GenerateColluns();
        else
        {
            if (grvResultado.FocusedRowIndex < 0)
                return;

            if (grvResultado.VisibleRowCount > 0)
            {
                string datFecCli = "";

                var datFecCre = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATFECCRE").ToString();
                if (filtro.SISTEMA == 0) grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATFECCLI").ToString();
                var tipTra = Convert.ToInt32(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "TIPTRA"));
                var codRta = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "CODRTA").ToString();
                var flgAut = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "FLAG_AUT").ToString() == "1" ? "S" : "N";

                int codcli;
                int.TryParse(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "CODCLI").ToString(), out codcli);
                var numDep = Convert.ToInt32(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NUMDEP"));

                var acoesTrans = new blConsultaVA(GetOperador()).ConsultaAcoesTrans(tipTra, codRta, datFecCli, datFecCre, flgAut, filtro.SISTEMA, codcli, numDep);
                btnConfirmarTransacao.ClientEnabled = acoesTrans.CONFIRMAR;
                btnAlterarTransacao.ClientEnabled = acoesTrans.ALTERAR;
                btnCancelarTransacao.ClientEnabled = acoesTrans.CANCELAR;
                btnAltValorTrans.ClientEnabled = acoesTrans.ALTVALOR;
                hdValorAltTrans.Value = Convert.ToString(acoesTrans.VALOR);
            }
        }
    }

    protected void clbGrid_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (grvResultado.FocusedRowIndex < 0) return;
        var result = string.Empty;

        var filtro = Session[UtilSIL.DADOSPARACONSULTA] as CONSULTA_VA;

        switch (e.Parameter)
        {
            case "0":
                {
                    var datFecCre = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATFECCRE").ToString();
                    var datFecCli = "";
                    var dattra = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATTRA").ToString();
                    var numHost = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUHOS").ToString();
                    var nsuAut = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUAUT").ToString();
                    var tipTra = Convert.ToInt32(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "TIPTRA"));
                    var codRta = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "CODRTA").ToString();
                    var flgAut = Convert.ToInt32(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "FLAG_AUT")) == 0 ? "N" : "S";
                    var codCli = Convert.ToInt32(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "CODCLI"));
                    var numDep = Convert.ToInt32(grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NUMDEP"));

                    var valor = Convert.ToDecimal(hdValorAltTrans.Value);                    
                    result = new blTransacao(GetOperador()).AlterarValTrans(filtro.SISTEMA, dattra, numHost, nsuAut, valor);
                    var acoesTrans = new blConsultaVA(GetOperador()).ConsultaAcoesTrans(tipTra, codRta, datFecCli, datFecCre, flgAut, filtro.SISTEMA, codCli, numDep);
                    hdValorAltTrans.Value = Convert.ToString(acoesTrans.VALOR);

                    var resultado = (List<CTTRANSVA>)Session["RESULCONSTRANSACAO"];

                    foreach (var item in resultado)
                    {
                        if (item.DATTRA == dattra && item.NSUAUT == nsuAut && item.CODCLI == Convert.ToString(codCli))
                        {
                            item.VALTRA = Convert.ToString(valor);
                            item.TVALOR = Convert.ToString(valor);
                        }

                    }
                    grvResultado.DataSource = resultado;

                }
                break;
            case "1":
                {
                    var dattra = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATTRA").ToString();
                    var numHost = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUHOS").ToString();
                    var nsuAut = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUAUT").ToString();
                    var justific = edtJustific.Text;
                    result = new blTransacao(GetOperador()).CancelarTrans(filtro.SISTEMA, dattra, numHost, nsuAut, justific);
                }
                break;
            case "2":
                {
                    var dattra = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATTRA").ToString();
                    var numHost = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUHOS").ToString();
                    var nsuAut = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUAUT").ToString();
                    result = new blTransacao(GetOperador()).AlteraTrans(filtro.SISTEMA, dattra, numHost, nsuAut);
                }
                break;
            case "3":
                {
                    var dattra = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATTRA").ToString();
                    var numHost = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUHOS").ToString();
                    var nsuAut = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUAUT").ToString();
                    var justific = edtJustific.Text;
                    result = new blTransacao(GetOperador()).ConfirmaTrans(filtro.SISTEMA, dattra, numHost, nsuAut, justific);
                }
                break;
        }

        if (result == "OK")
        {
            var acao = string.Empty;
            if (e.Parameter == "3") acao = "confirmada";
            if (e.Parameter == "1") acao = "cancelada";
            if (e.Parameter == "2" || e.Parameter == "0") acao = "alterada";

            hdnErroConsulta.Value = "Transacao " + acao + " com sucesso.";

            var filtros = (CONSULTA_VA)Session["DADOSPARACONSULTA"];
            var resultado = new blTransacao(GetOperador()).GerarConsultaTransacao(filtros);
            Session["RESULCONSTRANSACAO"] = resultado;
            grvResultado.DataSource = resultado;
            grvResultado.DataBind();
        }
        else
        {
            hdnErroConsulta.Value = result;
            grvResultado.DataBind();
        }
    }

    protected void cplInfoJustific_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;

        var justific = string.Empty;

        var datTra = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DATTRA").ToString();
        var nsuHos = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUHOS").ToString();
        var nsuAut = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "NSUAUT").ToString();
        var tipTra = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "TIPTRA").ToString();
        var DAD = grvResultado.GetRowValues(grvResultado.FocusedRowIndex, "DAD_JUST").ToString();

        if (tipTra == "999007")
        {
            justific = new blConsultaVA(GetOperador()).ConsultaJustificSegViaCard(DAD);
        }
        else
        {
            justific = new blConsultaVA(GetOperador()).ConsultaJustific(datTra, nsuHos, nsuAut, tipTra);
        }

        if (string.IsNullOrEmpty(justific))
        {
            cplInfoJustific.JSProperties["cpWasSuccessful"] = true;
            return;
        }
        lblJustific.Text = justific;
    }

    #endregion

    protected void grvResultado_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
    {
        if (e.RowType != GridViewRowType.Data)
        {
            return;
        }

        var tipoAcesso = Convert.ToString(e.GetValue("FLAG_AUT"));

        if (tipoAcesso == "1")
        {
            e.Row.BackColor = System.Drawing.Color.LightGray;
        }
    }
}
