using System;
using System.Collections.Generic;
using System.Drawing;
using SIL.BLL;
using TELENET.SIL.PO;
using TELENET.SIL;
using TELENET.SIL.BL;
using DevExpress.Web.ASPxClasses;
using System.IO;
using System.Text;
using System.Globalization;
using MenuItem = DevExpress.Web.ASPxMenu.MenuItem;
using System.Linq;

public partial class TransfUsuario : TelenetPage
{
    protected OPERADORA GetOperador()
    {
        return (OPERADORA)Session["Operador"];

    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
        if (GetOperador() == null) return;

        Page.ClientScript.RegisterClientScriptInclude("TransfUsuario", Page.ResolveVersionScript("TransferenciaUsuario.js"));
        txtCodCliOrigem.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        txtCodCliDestino.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";

        if (!Page.IsPostBack)
            txtCpfUsuarioOrigem.ClientEnabled = false;
    }

    protected void clbClienteOrigem_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;


        imgStatusClienteOrigem.Visible = true;
        hdClienteOrigemOk.Value = "n";

        if (e.Parameter != "")
        {
            var cliente = e.Parameter.Split('-');
            var codClientePopUp = Convert.ToInt32(cliente[0]);
            var sistemaPopUp = Convert.ToInt32(0);

            var listClientes = (List<VRESUMOCLI>)Session["ListClientesOrigem"];

            var clientePopUp = listClientes.Where(x => x.CODCLI == codClientePopUp && x.SISTEMA == sistemaPopUp).SingleOrDefault();

            if (clientePopUp != null)
            {
                Session["ListClientesOrigem"] = null;
                Session["ListClientesOrigem"] = clientePopUp;

                if (clientePopUp.STA != "00")
                {
                    hdClienteOrigemOk.Value = "n";

                    txtnomCliOrigem.Text = "CLIENTE COM STATUS DIFERENTE DE ATIVO";
                    txtnomCliOrigem.Border.BorderColor = Color.Red;
                    txtnomCliOrigem.BackColor = Color.Salmon;

                }
                else
                {
                    hdClienteOrigemOk.Value = "s";

                    txtnomCliOrigem.Text = clientePopUp.NOMCLI;
                    //txtCodCliOrigem.Enabled = false;
                    imgStatusClienteOrigem.ImageUrl = "~/Images/Small/Apply.gif";
                }
            }
            else
            {
                hdClienteOrigemOk.Value = "n";

                txtnomCliOrigem.Text = "CLIENTE PÓS PAGO NÃO ENCONTRADO";
                txtnomCliOrigem.Border.BorderColor = Color.Red;
                txtnomCliOrigem.BackColor = Color.Salmon;
            }

        }
        else
        {
            if (txtCodCliOrigem.Text != string.Empty)
            {
                int codCli;
                int.TryParse(txtCodCliOrigem.Text, out codCli);

                Session["ListClientesOrigem"] = null;
                
                IFilter Parametro1 = null;
                IFilter filtro = null;
                        
                Parametro1 = new Filter("codcli", SqlOperators.Equal, codCli.ToString(CultureInfo.InvariantCulture));
                filtro = new ANDFilter(Parametro1, new Filter("SISTEMA", SqlOperators.Equal, "0"));

                var clientes = new blCLIENTENovo(GetOperador()).ColecaoClientes(filtro);

                if (clientes.Count == 1)
                {
                    Session["ListClientesOrigem"] = clientes.SingleOrDefault();
                }
                else
                {
                    Session["ListClientesOrigem"] = clientes;
                }

                var qtdClientes = clientes.Count;
                switch (qtdClientes)
                {
                    case 1:
                        {
                            if (clientes[0].STA != "00")
                            {
                                hdClienteOrigemOk.Value = "n";

                                txtnomCliOrigem.Text = "CLIENTE COM STATUS DIFERENTE DE ATIVO";
                                txtnomCliOrigem.Border.BorderColor = Color.Red;
                                txtnomCliOrigem.BackColor = Color.Salmon;

                            }
                            else
                            {
                                hdClienteOrigemOk.Value = "s";
                                txtnomCliOrigem.Text = clientes[0].NOMCLI;
                                //txtCodCliOrigem.Enabled = false;
                                imgStatusClienteOrigem.ImageUrl = "~/Images/Small/Apply.gif";
                            }

                        }
                        break;
                    case 2:
                        {
                            PopUpClientesOrigem(clientes);
                        }
                        break;
                    default:
                        {
                            hdClienteOrigemOk.Value = "n";

                            txtnomCliOrigem.Text = "CLIENTE PÓS PAGO NÃO ENCONTRADO";
                            txtnomCliOrigem.Border.BorderColor = Color.Red;
                            txtnomCliOrigem.BackColor = Color.Salmon;
                        }
                        break;
                }
            }
            else
            {
                txtnomCliOrigem.Text = string.Empty;
            }
        }

        HabilitaDesabilitaCodUsuario();
    }

    protected void clbClienteDestino_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;


        imgStatusClienteDestino.Visible = true;
        hdClienteDestinoOk.Value = "n";

        if (e.Parameter != "")
        {
            var cliente = e.Parameter.Split('-');
            var codClientePopUp = Convert.ToInt32(cliente[0]);
            var sistemaPopUp = 0;

            var listClientes = (List<VRESUMOCLI>)Session["ListClientesDestino"];

            var clientePopUp = listClientes.Where(x => x.CODCLI == codClientePopUp && x.SISTEMA == sistemaPopUp).SingleOrDefault();


            if (clientePopUp != null)
            {
                Session["ListClientesDestino"] = null;
                Session["ListClientesDestino"] = clientePopUp;

                if (clientePopUp.STA != "00")
                {
                    hdClienteDestinoOk.Value = "n";

                    txtnomCliDestino.Text = "CLIENTE COM STATUS DIFERENTE DE ATIVO";
                    txtnomCliDestino.Border.BorderColor = Color.Red;
                    txtnomCliDestino.BackColor = Color.Salmon;

                }
                else
                {
                    hdClienteDestinoOk.Value = "s";
                    txtnomCliDestino.Text = clientePopUp.NOMCLI;
                    //txtCodCliDestino.Enabled = false;

                    imgStatusClienteDestino.ImageUrl = "~/Images/Small/Apply.gif";
                    imgStatusClienteDestino.ToolTip = "Cliente liberado para transferencia de usuário ";
                }
            }
            else
            {
                hdClienteDestinoOk.Value = "n";

                txtnomCliDestino.Text = "CLIENTE PÓS PAGO NÃO ENCONTRADO";
                txtnomCliDestino.Border.BorderColor = Color.Red;
                txtnomCliDestino.BackColor = Color.Salmon;
            }

        }
        else
        {
            if (txtCodCliDestino.Text != string.Empty)
            {
                int codCli;
                int.TryParse(txtCodCliDestino.Text, out codCli);

                Session["ListClientesDestino"] = null;

                IFilter Parametro1 = null;
                IFilter filtro = null;

                Parametro1 = new Filter("codcli", SqlOperators.Equal, codCli.ToString(CultureInfo.InvariantCulture));
                filtro = new ANDFilter(Parametro1, new Filter("SISTEMA", SqlOperators.Equal, "0"));                  


                var clientes = new blCLIENTENovo(GetOperador()).ColecaoClientes(filtro);

                if (clientes.Count == 1)
                {
                    Session["ListClientesDestino"] = clientes.SingleOrDefault();
                }
                else
                {
                    Session["ListClientesDestino"] = clientes;
                }
                var qtdClientes = clientes.Count;
                switch (qtdClientes)
                {
                    case 1:
                        {
                            if (clientes[0].STA != "00")
                            {
                                hdClienteDestinoOk.Value = "n";

                                txtnomCliDestino.Text = "CLIENTE COM STATUS DIFERENTE DE ATIVO";
                                txtnomCliDestino.Border.BorderColor = Color.Red;
                                txtnomCliDestino.BackColor = Color.Salmon;

                            }
                            else
                            {
                                hdClienteDestinoOk.Value = "s";

                                txtnomCliDestino.Text = clientes[0].NOMCLI;
                                //txtCodCliDestino.Enabled = false;

                                imgStatusClienteDestino.ImageUrl = "~/Images/Small/Apply.gif";
                                imgStatusClienteDestino.ToolTip = "Cliente liberado para transferencia de usuário ";
                            }

                        }
                        break;
                    case 2:
                        {
                            PopUpClientesDestino(clientes);
                        }
                        break;
                    default:
                        {
                            hdClienteDestinoOk.Value = "n";

                            txtnomCliDestino.Text = "CLIENTE PÓSMPAGO NÃO ENCONTRADO";
                            txtnomCliDestino.Border.BorderColor = Color.Red;
                            txtnomCliDestino.BackColor = Color.Salmon;
                        }
                        break;
                }
            }
            else
            {
                txtnomCliDestino.Text = string.Empty;
            }
        }
        HabilitaDesabilitaCodUsuario();

    }
    protected void PopUpClientesOrigem(List<VRESUMOCLI> clientesUsu)
    {
        MenuItem menuGrupo = null;
        foreach (var cli in clientesUsu)
        {
            menuGrupo = new MenuItem();
            menuGrupo.Text = "(" + Convert.ToString(cli.CODCLI) + ") " + cli.NOMCLI + " - " + cli.PRODUTO + " - " + (cli.SISTEMA == 0 ? ConstantesSIL.PosPago : ConstantesSIL.PrePago);
            menuGrupo.Name = Convert.ToString(cli.CODCLI + "-" + cli.SISTEMA);
            menuSelClienteOrigem.Items.Add(menuGrupo);
        }
    }
    protected void PopUpClientesDestino(List<VRESUMOCLI> clientesUsu)
    {
        MenuItem menuGrupo = null;
        foreach (var cli in clientesUsu)
        {
            menuGrupo = new MenuItem();
            menuGrupo.Text = "(" + Convert.ToString(cli.CODCLI) + ") " + cli.NOMCLI + " - " + cli.PRODUTO + " - " + (cli.SISTEMA == 0 ? ConstantesSIL.PosPago : ConstantesSIL.PrePago);
            menuGrupo.Name = Convert.ToString(cli.CODCLI + "-" + cli.SISTEMA);
            menuSelClienteDestino.Items.Add(menuGrupo);
        }
    }

    protected void clbCpfUsuarioOrigem_Callback(object source, CallbackEventArgsBase e)
    {
        imgIncluirCartaoOrigem.Visible = true;

        var CPF = e.Parameter;

        IFilter Parametro1 = new Filter("cpf", SqlOperators.Equal, UtilSIL.RetirarCaracteres("./-", CPF));
        var Parametro2 = new ANDFilter(Parametro1, new Filter("codcli", SqlOperators.Equal, txtCodCliOrigem.Text));
        var Parametro3 = new ANDFilter(Parametro2, new Filter("numdep", SqlOperators.Equal, "0"));



        IFilter filter = Parametro3;

            filter = new ANDFilter(Parametro3, new Filter("sistema", SqlOperators.Equal, "0"));
      

        try
        {
            string filtro = filter.FilterString;
            var usuario = new blUsuarioVANovo(GetOperador()).ColecaoUsuario(filtro);
            txtCpfUsuarioOrigem.Text = CPF;
            if (usuario.Count == 1)
            {
                hdCpfOrigemOk.Value = "s";
                //txtCpfUsuarioOrigem.Enabled = false;
                txtnomUsuOrigem.Text = usuario[0].NOMUSU;
                imgIncluirCartaoOrigem.ImageUrl = "~/Images/Small/Apply.gif";

                btnProcessarManual.ClientEnabled = true;
            }
            else
            {
                hdCpfOrigemOk.Value = "n";

                txtnomUsuOrigem.Text = "USUARIO NAO LOCALIZADO";
                txtnomUsuOrigem.Border.BorderColor = Color.Red;
                txtnomUsuOrigem.BackColor = Color.Salmon;
            }
        }
        catch
        {
            //TrataVisualUsuarioDest(true);
        }
    }

    private void HabilitaDesabilitaCodUsuario()
    {
        txtCpfUsuarioOrigem.Enabled = false;

        if (hdClienteDestinoOk.Value == "s" || hdClienteOrigemOk.Value == "s")
        {
            txtCpfUsuarioOrigem.Enabled = true;
        }
    }

    protected void clbProcessa_Callback(object source, CallbackEventArgsBase e)
    {
        var CPF = UtilSIL.RetirarCaracteres("/.-", e.Parameter);
        string retorno = "";
        string mensagem = "";

        var bll = new blSolicitaCarga(GetOperador());
        bll.TransferenciaUsuario(Convert.ToInt32(txtCodCliOrigem.Text), Convert.ToInt32(txtCodCliDestino.Text), CPF, out retorno, out mensagem);
        btnProcessar.Enabled = false;
        btnProcessarArquivo.Enabled = true;

        if (retorno == "0")
        {
            txtResultado.Border.BorderColor = Color.Green;
            txtResultado.BackColor = Color.LightGreen;
        }
        else
        {
            txtResultado.Border.BorderColor = Color.Red;
            txtResultado.BackColor = Color.Salmon;
        }

        mensagem = msgTransferenciaUsuario(Convert.ToInt32(retorno));
        pnlResultado.Visible = true;
        txtCodCliOrigem.ClientEnabled = false;
        txtCodCliDestino.ClientEnabled = false;
        txtCpfUsuarioOrigem.ClientEnabled = false;
        txtResultado.Text = mensagem.ToUpper();
    }
    private string msgTransferenciaUsuario(int codTransferencia)
    {
        string msg = "";

        switch (codTransferencia)
        {
            case 1:
                msg = "Usuario nao existe na origem";
                break;
            case 2:
                msg = "Cancelado na origem";
                break;
            case 3:
                msg = "Erro";
                break;
            case 4: 
                msg = "Cliente destino não existe";
                break;
            case 5: 
                msg = "Cliende destino cancelado";
                break;
            case 6: 
                msg = "Usuário já existe no destino";
                break;
            case 7: 
                msg = "Limite de crédito do cliente insuficiente";
                break;
            case 8: 
                msg = "Cliente destino possui configuração de senha diferente do cliente origem";
                break;
            case 9: 
                msg = "Erro na conexao com o banco autorizador";
                break;             
        }

        if (codTransferencia == 0)
            msg = "Transferencia realizada com sucesso!";

        if (codTransferencia == 0 && msg == "")
            msg = "Falha no processo de transferencia de usuário.";

        return msg;
    }

    protected void uplArquivo_FileUploadComplete(object sender, DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs e)
    {
        try
        {
            string retorno, mensagem;

            if (e.IsValid)
            {
                var bc = e.UploadedFile.FileContent.CanRead;
                var extension = Path.GetExtension(e.UploadedFile.FileName);
                var fileExtension = extension.ToLower();
                var folder = GetOperador().BANCONC;
                folder = folder.Substring(0, folder.IndexOf('_'));
                var filePath = Path.Combine(Server.MapPath(@"~/App_Data/Upload/" + folder + "/"), "TRANSFERENCIA_USUARIO_" + GetOperador().LOGIN.ToUpper() + "DATA_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".csv");

                StreamReader arquivo = null;
                if (!Directory.Exists(Server.MapPath(@"~/App_Data/Upload/" + folder + "/")))
                    Directory.CreateDirectory(Server.MapPath(@"~/App_Data/Upload/" + folder + "/"));

                e.UploadedFile.SaveAs(filePath);
                e.CallbackData = e.UploadedFile.FileName;

                if (File.Exists(filePath))
                {
                    arquivo = new StreamReader(filePath);
                    var linhas = new List<string>();
                    var listaCpf = new List<string>();
                    var listaCpfLayout7 = new List<string>();

                    bool leuCabecalho = false;
                    var log = new LOG();

                    using (arquivo)
                    {
                        string linha;
                        var bll = new blSolicitaCarga(GetOperador());
                        while ((linha = arquivo.ReadLine()) != null)
                        {
                            if (leuCabecalho)
                            {                                
                                string[] corte = linha.Split(';');

                                int codCliOri;
                                int codcliDest;
                                int.TryParse(corte[0], out codCliOri);
                                int.TryParse(corte[2], out codcliDest);
                                var cpf = corte[1].PadLeft(11, '0');
                                

                                var linhaValida = (codCliOri > 0) &&
                                                  (codcliDest > 0);

                                if (linhaValida)
                                {
                                    bll.TransferenciaUsuario(codCliOri, codcliDest, cpf, out retorno, out mensagem);
                                    mensagem = msgTransferenciaUsuario(Convert.ToInt32(retorno));
                                    log.AddLog(new LOG("TRANSFERENCIA USUARIO", codCliOri + "/" + cpf + "/" + codcliDest, mensagem));
                                }
                                else
                                {
                                    log.AddLog(new LOG("TRANSFERENCIA USUARIO", codCliOri + "/" + cpf + "/" + codcliDest, "Registro com erro na linha - " + linha));
                                }
                            }

                            if (!leuCabecalho)
                                leuCabecalho = true;
                        }
                    }
                    arquivo.Close();                    
                    ExibeMensagem("Processo finalizado, clique no botão de Log para obter o resultado.");
                    //Para criar o log
                    if (Session["LOG"] != null) Session.Remove("DADOSLOG");
                    Session["LOG"] = log;
                    btnLog.Visible = true;

                    lblMensagemArquivo.Text = "Processo finalizado, clique no botão de Log para obter o resultado.";
                    btnProcessarArquivo.ClientSideEvents.LostFocus = "function(s, e) { ExibeMensagem();}";
                }
            }
        }
        catch (Exception) { ExibeMensagem("Ocorreu um erro ao processar a solicitação de transferência"); }
    }

    protected void btnLog_Click(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        if (Session["LOG"] == null) return;
        var pathArquivo = Server.MapPath(@"~/App_Data/Upload/Arquivo_LOG_" + GetOperador().NOMEOPERADORA + "_" + GetOperador().LOGIN + ".txt");
        bool gerouArquivo;
        try
        {
            var log = (LOG)Session["LOG"];
            log.removeRecordOBS();
            gerouArquivo = UtilSIL.GerarArquivoLog(pathArquivo, log);
        }
        catch
        {
            throw new Exception("Erro ao gerar o arquivo de log");
        }

        if (!gerouArquivo) return;
        var Arquivo = new FileInfo(pathArquivo);
        Response.Clear();
        Response.AddHeader("Content-Disposition", "attachment; filename=ArquivoTransferencia_LOG.txt");
        Response.AddHeader("Content-Length", Arquivo.Length.ToString());
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(Arquivo.FullName);
        Response.End();
    }
}