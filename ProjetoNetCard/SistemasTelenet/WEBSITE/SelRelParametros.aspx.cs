using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using SIL.BLL;
using TELENET.SIL;
using TELENET.SIL.PO;
using System.Web.UI.WebControls;
using TextBox = System.Web.UI.WebControls.TextBox;
using System.Linq;
using System.Text;
using System.Configuration;
using SIL.APIs.Model;
using System.Web.Services;
using System.Web.Script.Services;
using System.Net;
using Newtonsoft.Json;

public partial class SelRelParametros : TelenetPage
{
    protected string[,] matrizDropDownList = new string[10, 2];
    string valorData;
    protected string _BaseAdress = "https://localhost:44310/";
    //protected string _BaseAdress = "Relatorio/";
    ApiClientBase _api;
    private static object cep;

    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];
    }

    protected override void Page_Init(object sender, EventArgs e)
    {
        Session.RegisterCustomSerializer<ASPxComboBox, DevexComboSerializer>();

        if (GetOperador() == null) return;
        if (!IsPostBack) for (int i = 0; i < 10; i++) { Session["DadosddlObj" + i] = null; }
        Page.ClientScript.RegisterClientScriptInclude("ClienteVA", Page.ResolveVersionScript("Consultas.js"));
        Page.ClientScript.RegisterClientScriptInclude("Relatorio", Page.ResolveVersionScript("Relatorio.js"));
        _api = new ApiClientBase(_BaseAdress, GetOperador().SERVIDORNC);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        ListarRelatorios();
    }

    protected void ListarRelatorios()
    {
        //if (Page.IsPostBack) return;
        if (GetOperador() == null) return;
        var idRel = Convert.ToInt32(Request.QueryString["IdRel"]);
        var dados = BuscaParametrosRel(idRel);
        var relvisto = Convert.ToInt32(Session["relvisto"]);

        if (Convert.ToInt32(relvisto) == 0)
        {
            if (idRel == 0)
            {
                idRel = Convert.ToInt32(Session["idRelD"]);
                dados = BuscaParametrosRel(idRel);
            }
            var ListaCampoFormato = dados.Where(x => x.NOMPAR != "FORMATO").ToList();
            dados= ListaCampoFormato;

            var daosAux = dados.OrderByDescending(o => o.ORDEM_TELA).ToList();
            var listParFunc = new List<string>();
            foreach (var par in dados)
            {
                if (!string.IsNullOrEmpty(par.NOM_FUNCTION))
                {
                    if (!string.IsNullOrEmpty(par.PARAM_FUNCTION))
                    {
                        listParFunc = new List<string>(par.PARAM_FUNCTION.Split(','));

                        foreach (var parF in listParFunc)
                        {
                            var ppp = new PARAMETRO();
                            ppp = dados.Find(x => x.NOMPAR == parF);
                            dados.Find(x => x.NOMPAR == parF).CHAMADA_FUNC_JS += ppp.LABEL == "TextBox" ?
                                parF.Substring(1) + par.ORDEM_TELA + "(this.value);" :
                                parF.Substring(1) + par.ORDEM_TELA + "(s.GetSelectedItem().value);";
                        }
                    }
                }
            }
            var dadosRel = (APIRelatorioModel)Session["DadosRel"];
            idRel = dadosRel.idRelatorio;
            Session["idRelD"] = dadosRel.idRelatorio;
            Session["Parametro"] = dados;
            grvParametros.DataSource = dados;
            grvParametros.DataBind();
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:AplicarMaskData(); ", true);
            ppc.HeaderText = dadosRel.nmTela;
            ExecutaViaJob.Value = dadosRel.ExecutaViaJob;
            idRelatorio.Value = dadosRel.idRelatorio.ToString();
            TipoRelatorio.Value = Convert.ToString(dadosRel.TipoRelatorio);
            statusGeracao.Value = dadosRel.statusGeracao;
            msgRetorno.Value = dadosRel.msgRetorno;
            pathArquivo.Value = dadosRel.pathArquivo;
            urlRelatorio.Value = dadosRel.urlRelatorio;
        }
        else if (Convert.ToInt32(relvisto) == 1)
        {
            idRel = Convert.ToInt32(Session["idRel"]);
            dados = (List<PARAMETRO>)Session["Parametro"];
            //this.ParametrosRetorno(idRel, dados);
            grvParametros.DataSource = dados;
            grvParametros.DataBind();
            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:AplicarMaskData(); ", true);
            ppc.HeaderText = dados.Find(p => p.IDREL == idRel).NOMREL;
        }
        Session.Remove("relvisto");
        Session["idRelD"] = Convert.ToInt32(Session["idRel"]);
    }
    protected void cplParametro_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        var filtros = e.Parameter.ToString();
        var row = filtros.Substring(0, filtros.IndexOf(':'));
        var nomFunc = e.Parameter.Substring(filtros.IndexOf(':') + 1, e.Parameter.IndexOf('@') - 2);
        var valor = e.Parameter.Substring(e.Parameter.IndexOf('@')).Replace(" ", "");

        if (GetOperador() == null) return;
        var listParam = new List<PARAM>();
        var itens = valor.Split(',');
        foreach (var item in itens)
        {
            var param = new PARAM();
            param.ID0 = item.Split('|')[0];
            param.VAL = item.Split('|')[1];
            listParam.Add(param);
        }

        var dados = new blModRel(GetOperador()).FuncParametros(nomFunc, listParam);
        var ddlObj = (ASPxComboBox)Session["ddlObj" + (Convert.ToInt16(row) + 1)];
        ddlObj.Items.Clear();
        foreach (var detpar in dados)
        {
            var itemDropDown = new[]
            {
                new ListEditItem(detpar.VAL, detpar.ID0)
            };
            ddlObj.Items.AddRange(itemDropDown);
        }
        Session["DadosddlObj" + (Convert.ToInt16(row) + 1)] = dados;
        grvParametros.Rows[Convert.ToInt16(row)].Cells[9].Controls.Clear();
        grvParametros.Rows[Convert.ToInt16(row)].Cells[9].Controls.Add(ddlObj);

        var rows = grvParametros.Rows.Count;
        for (int i = Convert.ToInt16(row) + 1; i < rows; i++)
        {
            var drop = (ASPxComboBox)grvParametros.Rows[i].Cells[9].FindControl("ddlObj" + (i + 1));
            if (drop != null)
            {
                drop.Items.Clear();
                grvParametros.Rows[i].Cells[9].Controls.Clear();
                grvParametros.Rows[i].Cells[9].Controls.Add(drop);
            }
        }
    }
    protected void cplAction_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (e.Parameter[0] == '1')//1 - visualizar relatorio
        {
            VisualizaRel(0);
        }
        else if (e.Parameter[0] == '2')//2 - exporta para csv
        {
            VisualizaRel(1);
        }
        else if (e.Parameter[0] == '3')//3 - Gera Relatorio via Job
        {
            var retorno = GeraRelatorioViaJob();
        }
        else if (e.Parameter[0] == '4')//4 - retorno do Job
        {
            //RetornoRelatornoViaJob();
        }
        
    }

    private APIRelatorioModel MontraParametrosRel()
    {
        var modelRel = (APIRelatorioModel)Session["DadosRel"];
        string ValorCampo = "";
        int idParam = 0;

        foreach (GridViewRow row in grvParametros.Rows)
        {
            ValorCampo = "";
            if (grvParametros.DataKeys.Count <= row.RowIndex) continue;

            var dataKey = grvParametros.DataKeys[row.RowIndex];
            idParam = Convert.ToInt32(dataKey["IDPAR"]);

            var parametro = modelRel.listaCampos.Find(tp => tp.ID_PAR == idParam);
            var componente = parametro.LABEL;

            switch (componente)
            {
                case "DropDownList":
                    if (((ASPxComboBox)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("ddlObj" + parametro.ORDEM_TELA)).Value != null)
                        ValorCampo = ((ASPxComboBox)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("ddlObj" + parametro.ORDEM_TELA)).SelectedItem.Value.ToString();
                    break;

                case "DateEdit":
                    if (!string.IsNullOrEmpty(((ASPxDateEdit)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("dteObj" + parametro.ORDEM_TELA)).Text))
                    {
                        ValorCampo = ((ASPxDateEdit)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("dteObj" + parametro.ORDEM_TELA)).Text;
                        //valorPar = Convert.ToDateTime(valorPar).ToString("yyyyMMdd");
                    }

                    break;

                case "TextBox":
                    if (!string.IsNullOrEmpty(((TextBox)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("txtObj" + parametro.ORDEM_TELA)).Text))
                        ValorCampo = ((TextBox)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("txtObj" + parametro.ORDEM_TELA)).Text;


                    break;

                case "CheckBox":
                    var valor =
                        ((ASPxCheckBox)grvParametros.Rows[row.RowIndex].Cells[9].FindControl("chkObj" + parametro.ORDEM_TELA)).Value.ToString();
                    ValorCampo = valor == "True" ? "1" : "0";
                    break;

            }
            modelRel.listaCampos.Find(tp => tp.ID_PAR == idParam).VALCAMPO = ValorCampo;


        }

        return modelRel;

    }

    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    [WebMethod()]
    public static bool VerificaConclusaoRelatorio2(string idRel)
    {
        var parameters = new Dictionary<string, object>();
        parameters.Add("codigoCliente", 0);
        parameters.Add("sistema", 0);
        parameters.Add("codigoRelatorio", idRel);

        var client = new WebClient();
        var Caminho = Path.Combine("https://localhost:44310/", "VerificaTerminoGeracaoRelatorio");

        client.Headers.Add("Authorization", "Bearer " + ApiHelper.GetToken());
        client.Headers[HttpRequestHeader.ContentType] = "application/json";
        client.Encoding = Encoding.UTF8;
        var response = client.UploadString(Caminho, "POST", JsonConvert.SerializeObject(parameters));
        var resultado = JsonConvert.DeserializeObject<RawApiResult>(response);

        return (bool)resultado.results;
    }
    protected void VisualizaRel(int formato)
    {
        if(formato == 0)
        {
            MontaRelPDF();
            Redirect("~/PreviewModRel.aspx");
        }
        else
        {
            ExportarCSV();
        }       
    }
    public void MontaRelPDF()
    {
        var ParametrosRel = MontraParametrosRel();

        var retornoRel = _api.ExecutaPost<APIModRelPDF, APIRelatorioModel>("RelatorioPDF", ParametrosRel).Result;
        retornoRel.nmRelatorio = "";

        Session["ResultRel"] = null;
        Session["ResultRel"] = retornoRel;

    }
    private void ExportarCSV()
    {
        var ParametrosRel = MontraParametrosRel();

        var dadosRetorno = _api.ExecutaPost<APIRetornoModel, APIRelatorioModel>("ExportaExcel", ParametrosRel).Result;

        if (!dadosRetorno.Sucesso) return;

        var path = Path.Combine(RetornaCaminhoArquivo(), dadosRetorno.msgResultado);

        byte[] buffer;
        using (var myFileStream = new FileStream(path, FileMode.Open))
        {
            var size = myFileStream.Length;
            buffer = new byte[size];
            myFileStream.Read(buffer, 0, int.Parse(myFileStream.Length.ToString(CultureInfo.InvariantCulture)));
        }
        Response.ContentType = "text/txt";
        Response.AddHeader("content-disposition", "attachment; filename=" + UtilSIL.RemoverAcentos(dadosRetorno.msgResultado.Trim()));
        Response.BinaryWrite(buffer);
        Response.End();
    }
    private string RetornaCaminhoArquivo()
    {
        var CaminhoPadrao = @"\\192.168.70.7\c$\ARQ_NETCARD\";

        CaminhoPadrao = Path.Combine(CaminhoPadrao, GetOperador().NOMEOPERADORA);
        CaminhoPadrao = Path.Combine(CaminhoPadrao, "RELATORIO_OPERADORA");      

        if (!Directory.Exists(CaminhoPadrao))
        {
            Directory.CreateDirectory(CaminhoPadrao);
        }

        return CaminhoPadrao;
    }

    protected void grvParametros_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (GetOperador() == null) return;
        try
        {
            if (e.Row.RowIndex != -1)
            {
                if (e.Row.RowType.ToString().Equals("Header"))
                    e.Row.Cells[9].Text = "Valor OK";
                else
                {
                    var item = (PARAMETRO)e.Row.DataItem;
                    var idepar = item.IDPAR.ToString();
                    var listParNomFunc = !string.IsNullOrEmpty(item.NOM_FUNCTION) ? item.PARAM_FUNCTION.Split(',') : null;
                    //var pararametros = (List<PARAMETRO>)Session["Parametro"];

                    int count = 10;
                    switch (item.LABEL)
                    {
                        case "TextBox":
                            {
                                var txtObj = new TextBox();
                                txtObj.ID = "txtObj" + item.ORDEM_TELA;
                                txtObj.Text = string.IsNullOrEmpty(item.DEFAULT) ? string.Empty : item.DEFAULT;
                                txtObj.Width = 192;
                                if (!string.IsNullOrEmpty(item.CHAMADA_FUNC_JS))
                                    txtObj.Attributes.Add("onchange", "javascript:" + item.CHAMADA_FUNC_JS);
                                e.Row.Cells[9].Controls.Add(txtObj);
                            }
                            break;
                        case "DateEdit":
                            {
                                var dteObj = new ASPxDateEdit();
                                dteObj.ID = "dteObj" + item.ORDEM_TELA;

                                valorData = item.DEFAULT;
                                dteObj.Date = !string.IsNullOrEmpty(valorData)
                                                  ? Convert.ToDateTime(valorData)
                                                  : DateTime.Now;
                                dteObj.Width = 200;
                                dteObj.Attributes.Add("data-mask-data", "true");
                                if (!string.IsNullOrEmpty(item.CHAMADA_FUNC_JS))
                                    dteObj.Attributes.Add("onchange", "javascript:" + item.CHAMADA_FUNC_JS);
                                e.Row.Cells[9].Controls.Add(dteObj);
                            }
                            break;
                        case "DropDownList":
                            {

                                var filtroDDL = new Dictionary<string, object>();
                                filtroDDL.Add("ParametroWhere", "");
                                filtroDDL.Add("codigoParametro", item.IDPAR);
                                var retornoRel = _api.ExecutaPost<List<APIRetornoDDLModel>, Dictionary<string, object>>("CarregaDDL", filtroDDL).Result;

                                //var detPar = new blModRel(GetOperador()).DetalheParametro(item.IDPAR);
                                var ddlObj = new ASPxComboBox();
                                ddlObj.ID = "ddlObj" + item.ORDEM_TELA;
                                ddlObj.Width = 200;
                                ddlObj.Native = true;
                                ddlObj.CssClass = "TextBox";

                                foreach (var itemDrop in retornoRel)
                                {
                                    //var itemDropDown = new ListEditItem(itemDrop.VAL, itemDrop.ID0);
                                    var itemDropDown = new[]
                                            {
                                                        new ListEditItem(itemDrop.Descricao, itemDrop.Valor)
                                                    };

                                    ddlObj.Items.AddRange(itemDropDown);
                                }

                                ddlObj.SelectedIndex = 0;

                                Session[ddlObj.ID] = ddlObj;


                                e.Row.Cells[9].Controls.Add(ddlObj);


                                grvParametros.DataSource.ToString();
                            }
                            break;
                        case "CheckBox":
                            {
                                var chkObj = new ASPxCheckBox();
                                chkObj.ID = "chkObj" + item.ORDEM_TELA;
                                chkObj.Width = 200;
                                if (!string.IsNullOrEmpty(item.CHAMADA_FUNC_JS))
                                    chkObj.Attributes.Add("onchange", "javascript:" + item.CHAMADA_FUNC_JS);
                                chkObj.Checked = false;
                                e.Row.Cells[9].Controls.Add(chkObj);

                                //Preenchimento dos filtros memorizados de checkbox
                                if (matrizDropDownList != null && matrizDropDownList[0, 0] != null)
                                {
                                    for (int i = 0; i < count; i++)
                                        for (int j = 0; j < 2; j++)
                                            if (matrizDropDownList[i, j].ToString() == idepar)
                                            {
                                                if (matrizDropDownList[i, 1] == "1")
                                                    chkObj.Checked = true;
                                                else
                                                    chkObj.Checked = false;
                                                i = 10;
                                                break;
                                            }
                                }
                            }
                            break;
                    }
                }
            }
        }
        catch (Exception f)
        {
            Response.Write("Error: " + f);
        }
    }
    private List<PARAMETRO> BuscaParametrosRel(int idRel)
    {
        if (idRel == 0) return new List<PARAMETRO>();

        var parameters = new Dictionary<string, object>();
        parameters.Add("codigoCliente", 0);
        parameters.Add("sistema", 0);
        parameters.Add("codigoRelatorio", idRel);

        var DadosModel = _api.ExecutaPost<APIRelatorioModel, Dictionary<string, object>>("BuscaParametrosRelatorio", parameters).Result;
        Session["DadosRel"] = DadosModel;

        var dados = new List<PARAMETRO>();
        foreach (var idr in DadosModel.listaCampos)
        {
            var parametro = new PARAMETRO
            {
                IDREL = Convert.ToInt32(idr.ID_REL),
                NOMPROC = Convert.ToString(DadosModel.nmProc),
                //DISPOSICAO = Convert.ToString(DadosModel),
                IDPAR = Convert.ToInt32(idr.ID_PAR),
                DESPAR = Convert.ToString(idr.DESPAR),
                NOMPAR = Convert.ToString(idr.NOMPAR),
                LABEL = Convert.ToString(idr.LABEL),
                TIPO = Convert.ToString(idr.TIPO),
                TAMANHO = Convert.ToInt16(idr.TAMANHO),
                HASDETAIL = Convert.ToString(idr._DEFAULT),
                NOMREL = Convert.ToString(DadosModel.nmTela),
                EXECUTAVIAJOB = Convert.ToString(DadosModel.ExecutaViaJob),
                DEFAULT = Convert.ToString(idr._DEFAULT),
                ORDEM_PROC = Convert.ToInt16(idr.ORDEM_PROC),
                //SAIDA_ARQ_DIRETO = Convert.ToString(idr.SAIDA_ARQ_DIRETO),
                ORDEM_TELA = Convert.ToInt16(idr.ORDEM_TELA),
                PARAMETRODINAMICO = false
            };

            if (!string.IsNullOrEmpty(idr.NOM_FUNCTION))
            {
                var nomFunction = Convert.ToString(idr.NOM_FUNCTION);

                if (nomFunction.Contains("@"))
                {
                    parametro.NOM_FUNCTION = nomFunction.Substring(0, nomFunction.IndexOf('@') - 1);
                    parametro.PARAM_FUNCTION = nomFunction.Substring(nomFunction.IndexOf('@')).Replace(" ", "");
                }
                else
                {
                    parametro.NOM_FUNCTION = nomFunction;
                    parametro.PARAM_FUNCTION = "";
                }
            }

            dados.Add(parametro);
        }
        return dados;
    }
    public APIRetornoModel GeraRelatorioViaJob()
    {
        try
        {
            var ParametrosRel = MontraParametrosRel();

            var retornoGeracaoRelatorio = _api.ExecutaPost<APIRetornoModel, APIRelatorioModel>("GeraRelatorioViaJob", ParametrosRel).Result;

            return retornoGeracaoRelatorio;
        }
        catch (ApplicationException ex)
        {
            throw ex;
        }
    }
    protected void btnVisualizar_Click(object sender, EventArgs e)
    {

        var ParametrosRel = MontraParametrosRel();
        ParametrosRel.pathArquivo = RetornaCaminhoArquivo();
        try
        {
            var retornoRel = _api.ExecutaPost<APIModRelPDF, APIRelatorioModel>("RetornoRelatornoViaJob", ParametrosRel).Result;
            retornoRel.nmRelatorio = "";


            //relatório PDF
            if (retornoRel.TipoRel == 0)
            {
                Session["ResultRel"] = null;
                Session["ResultRel"] = retornoRel;

                Redirect("~/PreviewModRel.aspx");

            }
            else
            {
                var path = Path.Combine(RetornaCaminhoArquivo(), retornoRel.caminhoArquivo);

                byte[] buffer;
                using (var myFileStream = new FileStream(path, FileMode.Open))
                {
                    var size = myFileStream.Length;
                    buffer = new byte[size];
                    myFileStream.Read(buffer, 0, int.Parse(myFileStream.Length.ToString(CultureInfo.InvariantCulture)));
                }
                Response.ContentType = "text/txt";
                Response.AddHeader("content-disposition", "attachment; filename=" + UtilSIL.RemoverAcentos(retornoRel.nmRelatorio.Trim()));
                Response.BinaryWrite(buffer);
                Response.End();
            }
        }
        catch (ApplicationException ex)
        {
            throw ex;
        }
    }
    public void RetornoRelatornoViaJob(object sender, EventArgs e)
    {
        var ParametrosRel = MontraParametrosRel();
        ParametrosRel.pathArquivo = RetornaCaminhoArquivo();
        try
        {
            var retornoRel = _api.ExecutaPost<APIModRelPDF, APIRelatorioModel>("RetornoRelatornoViaJob", ParametrosRel).Result;
            retornoRel.nmRelatorio = "";


            //relatório PDF
            if (retornoRel.TipoRel == 0)
            {
                Session["ResultRel"] = null;
                Session["ResultRel"] = retornoRel;


                Redirect("~/PreviewModRel.aspx");

            }
            else
            {
                var path = Path.Combine(RetornaCaminhoArquivo(), retornoRel.caminhoArquivo);

                byte[] buffer;
                using (var myFileStream = new FileStream(path, FileMode.Open))
                {
                    var size = myFileStream.Length;
                    buffer = new byte[size];
                    myFileStream.Read(buffer, 0, int.Parse(myFileStream.Length.ToString(CultureInfo.InvariantCulture)));
                }
                Response.ContentType = "text/txt";
                Response.AddHeader("content-disposition", "attachment; filename=" + UtilSIL.RemoverAcentos(retornoRel.nmRelatorio.Trim()));
                Response.BinaryWrite(buffer);
                Response.End();
            }
        }
        catch (ApplicationException ex)
        {
            throw ex;
        }

    }
    
    internal class RawApiResult
    {
        public int Status { get; set; }
        public object results { get; set; }
    }
}