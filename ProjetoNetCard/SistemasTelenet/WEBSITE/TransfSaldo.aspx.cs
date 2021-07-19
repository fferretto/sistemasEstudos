using System;
using System.Collections.Generic;
using System.Drawing;
using SIL.BLL;
using TELENET.SIL.PO;
using TELENET.SIL;
using TELENET.SIL.BL;
using DevExpress.Web.ASPxClasses;
using System.Configuration;

public partial class TransfSaldo : System.Web.UI.Page
{
    private string tipoTransf = string.Empty;

    protected OPERADORA GetOperador()
    {
        return (OPERADORA) Session["Operador"];
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (GetOperador() == null) return;
        #region Scripts

        tipoTransf = Request.QueryString["tela"];
        switch (tipoTransf)
        {
            case "0":
                lblTitulo.Text += " ENTRE CARTOES - (Exceto VOUCHER)";
                saldo.Visible = false;
                txtSaldoCli.Visible = false;
                break;
            case "1":
                lblTitulo.Text += " P/ CONTA CLIENTE";
                saldo.Visible = true;
                txtSaldoCli.Visible = true;
                break;
        }

        Page.ClientScript.RegisterClientScriptInclude("SolicitaCargas", Page.ResolveVersionScript("TransfereciaSaldo.js"));
        txtCodCli.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        //txtCpfUsuarioOri.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";
        //txtCpfUsuarioDest.ClientSideEvents.KeyPress = "function(s, e) {SomenteNumeros(s, e.htmlEvent);}";

        #endregion
    }

    #region Via Sistema

    protected void clbCliente_Callback(object source, CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (e.Parameter != string.Empty) return;
        imgStatusCliente.Visible = true;
        var clientes = new List<CLIENTEVA_PREPAGO>();
        IFilter filter = new Filter("codcli", SqlOperators.Equal, txtCodCli.Text);
        try
        {
            clientes = new blCLIENTEVA(GetOperador()).ColecaoClientesVA(filter);            

            if (clientes.Count == 1)
            {
                var tipoProd = new blTabelas(GetOperador()).GetProdutoByName(clientes[0].PRODUTO).TIPOPROD;
                var contaDigitalHabilitada = new blCLIENTENovo(GetOperador()).ContaDigitalHabilitada(tipoProd);

                if (contaDigitalHabilitada)
                {
                    txtnomCli.Text = "CLIENTE DO TIPO " + clientes[0].TIPOPRODDESCRICAO + ", NÃO PERMITIDO TRANSFERÊNCIA DE SALDO";
                    txtStatus.Text = string.Empty;
                    txtnomCli.Border.BorderColor = Color.Red;
                    txtnomCli.BackColor = Color.Salmon;
                    txtCodCli.Focus();
                    return;
                }

                limparTelaOrigemDestino();
                txtnomCli.Text = clientes[0].NOMCLI;
                txtStatus.Text = clientes[0].DESTA;
                txtSaldoCli.Text = Convert.ToDecimal(clientes[0].SALDO) < 0 ? "0,00" : clientes[0].SALDO;                
                
                //var listCodProd = new List<int>() { 2, 3, 6 }; // 2: ALIMENTACAO - 3: REFEICAO - 6: CULTURA
                var listTipoProd = ConfigurationManager.AppSettings["listaTipProdBlocTransSal"];

                if (!txtSaldoCli.Visible)
                {
                    if (listTipoProd.Contains(clientes[0].TIPOPROD.ToString()))
                    {
                        txtnomCli.Text = "CLIENTE DO TIPO " + clientes[0].TIPOPRODDESCRICAO + ", NÃO PERMITIDO TRANSFERÊNCIA DE SALDO";
                        txtStatus.Text = string.Empty;
                        txtnomCli.Border.BorderColor = Color.Red;
                        txtnomCli.BackColor = Color.Salmon;
                        txtCodCli.Focus();
                        return;
                    }
                }

                if (clientes[0].STA != "00")
                    TratarVisualTransfNaoLiberada(false);
                else
                {
                    imgStatusCliente.ImageUrl = "~/Images/Small/Apply.gif";
                    imgStatusCliente.ToolTip = "Cliente liberado para transferencia de saldo ";
                    hdClienteLiberado.Value = "1";
                }
            }
            else
                TratarVisualTransfNaoLiberada(true);
        }
        catch
        {
            TratarVisualTransfNaoLiberada(true);
        }
    }

    protected void clbDadosUsuDestino_Callback(object source, CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (e.Parameter == string.Empty || hdClienteLiberado.Value != "1") return;
        imgIncluirCartaoDest.Visible = true;
        IFilter filter = new Filter("u.cpf", SqlOperators.Equal, UtilSIL.RetirarCaracteres(".-/", txtCpfUsuarioDest.Text));
        var and = new ANDFilter(filter, new Filter("u.codcli", SqlOperators.Equal, e.Parameter));
        var andTwo = new ANDFilter(and, new Filter("u.numdep", SqlOperators.Equal, "0"));

        try
        {
            var usuario = new blUsuarioVA(GetOperador()).ColecaoUsuarioVA(andTwo)[0];
            if (usuario != null)
            {
                txtnomUsuDest.Text = usuario.NOMUSU;
                txtStatusUsuarioDest.Text = usuario.STASTR;
                txtNumCartaoDest.Text = usuario.CODCRTMASK;
                Session["CODCRTDEST"] = usuario.CODCRT;

                var blUsuario = new blUsuarioVA(GetOperador());
                CTCARTVA CartaoVA = blUsuario.CartaoVA(usuario.CODCRT);

                txtSaldoIniDest.Text = CartaoVA.SALDOVA;

                if (usuario.STA == "00")
                {
                    imgIncluirCartaoDest.ImageUrl = "~/Images/Small/Apply.gif";
                    imgIncluirCartaoDest.ToolTip = "Usuario liberado para transferencia de saldo ";
                    hdUsuarioLiberadoDest.Value = "1";
                }
                else
                    TrataVisualUsuarioDest(false);
            }
            else
                TrataVisualUsuarioDest(true);
        }
        catch
        {
            TrataVisualUsuarioDest(true);
        }
    }

    protected void clbDadosUsuOri_Callback(object source, CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        if (e.Parameter == string.Empty || hdClienteLiberado.Value != "1") return;
        imgIncluirCartaoOri.Visible = true;
        IFilter filter = new Filter("u.cpf", SqlOperators.Equal, UtilSIL.RetirarCaracteres(".-/", txtCpfUsuarioOri.Text));
        var and = new ANDFilter(filter, new Filter("u.codcli", SqlOperators.Equal, e.Parameter));
        var andTwo = new ANDFilter(and, new Filter("u.numdep", SqlOperators.Equal, "0"));

        try
        {
            var usuario = new blUsuarioVA(GetOperador()).ColecaoUsuarioVA(andTwo)[0];
            if (usuario != null)
            {
                txtnomUsuOri.Text = usuario.NOMUSU;
                txtStatusUsuarioOri.Text = usuario.STASTR;
                txtNumCartaoOri.Text = usuario.CODCRTMASK;
                Session["CODCRTORIG"] = usuario.CODCRT;

                var codCli = Convert.ToInt32(txtCodCli.Text);
                var blUsuario = new blUsuarioVA(GetOperador());
                CTCARTVA CartaoVA = blUsuario.CartaoVA(usuario.CODCRT);

                txtSaldoIniOri.Text = CartaoVA.SALDOVA;

                if (usuario.STA == "00")
                {
                    imgIncluirCartaoOri.ImageUrl = "~/Images/Small/Apply.gif";
                    imgIncluirCartaoOri.ToolTip = "Usuario liberado para transferencia de saldo";
                    hdUsuarioLiberadoOri.Value = "1";
                }
                else
                    TrataVisualUsuarioOri(false);
            }
            else
                TrataVisualUsuarioOri(true);
        }
        catch
        {
            TrataVisualUsuarioOri(true);
        }
    }

    private void TratarVisualTransfNaoLiberada(bool naoEncontrado)
    {
        if (GetOperador() == null) return;
        if (naoEncontrado)
        {
            txtnomCli.Text = "CLIENTE NAO ENCONTRADO";
            txtStatus.Text = string.Empty;
            txtnomCli.Border.BorderColor = Color.Red;
            txtnomCli.BackColor = Color.Salmon;
            txtCodCli.Focus();
        }
        else
        {
            txtStatus.Border.BorderColor = Color.Red;
            txtStatus.BackColor = Color.Salmon;
        }

        txtCodCli.Text = string.Empty;
        imgStatusCliente.ImageUrl = "~/Images/Small/cancelar_16.png";
        imgStatusCliente.ToolTip = "Cliente nao liberado para transferencia de saldo ";
        hdClienteLiberado.Value = "0";
        txtCodCli.Focus();
    }

    private void TrataVisualUsuarioOri(bool naoEncontrado)
    {
        if (GetOperador() == null) return;
        if (naoEncontrado)
        {
            txtnomUsuOri.Text = "USUARIO NAO ENCONTRADO";
            txtStatusUsuarioOri.Text = string.Empty;
            txtnomUsuOri.Border.BorderColor = Color.Red;
            txtnomUsuOri.BackColor = Color.Salmon;
            txtCpfUsuarioOri.Focus();
            txtCpfUsuarioOri.Text = string.Empty;
            txtSaldoIniOri.Text = string.Empty;
            txtNumCartaoOri.Text = string.Empty;
            imgIncluirCartaoOri.ImageUrl = "~/Images/Small/cancelar_16.png";
            imgIncluirCartaoOri.ToolTip = "Usuario nao encontrado com o cliente e o cpf informado";
            hdUsuarioLiberadoOri.Value = "0";
            txtCpfUsuarioOri.Focus();
        }
        else
        {
            txtStatusUsuarioOri.Border.BorderColor = Color.Red;
            txtStatusUsuarioOri.BackColor = Color.Salmon;
            imgIncluirCartaoOri.ImageUrl = "~/Images/Small/Apply.gif";
        }
    }

    private void TrataVisualUsuarioDest(bool naoEncontrado)
    {
        if (GetOperador() == null) return;
        if (naoEncontrado)
        {
            txtnomUsuDest.Text = "USUARIO NAO ENCONTRADO";
            txtStatusUsuarioDest.Text = string.Empty;
            txtnomUsuDest.Border.BorderColor = Color.Red;
            txtnomUsuDest.BackColor = Color.Salmon; 
            txtCpfUsuarioDest.Focus();
        }
        else
        {
            txtStatusUsuarioDest.Border.BorderColor = Color.Red;
            txtStatusUsuarioDest.BackColor = Color.Salmon;
        }

        txtCpfUsuarioDest.Text = string.Empty;
        txtSaldoIniDest.Text = string.Empty;
        txtNumCartaoDest.Text = string.Empty;
        imgIncluirCartaoDest.ImageUrl = "~/Images/Small/cancelar_16.png";
        imgIncluirCartaoDest.ToolTip = "Usuario nao encontrado com o cliente e o cpf informado";
        hdUsuarioLiberadoDest.Value = "0";
        txtCpfUsuarioOri.Focus();
    }

    #endregion

    #region Processar

    protected void clbProcessa_Callback(object source, CallbackEventArgsBase e)
    {
        if (GetOperador() == null) return;
        switch (e.Parameter)
        {
            //case "0": //Exibe div origem e destino
            //    {
            //        if ( txtCodCli.Text != string.Empty && txtnomCli.Text != string.Empty)
            //        {
            //            if (!destino.Visible)
            //            {
            //                if (!origem.Visible)
            //                    origem.Visible = true;
            //                else
            //                {
            //                    if (txtCpfUsuarioOri.Text != string.Empty && txtnomUsuOri.Text != string.Empty)
            //                        destino.Visible = true;
            //                }
            //            }
                        
            //        }
            //    }
            //    break;
            case "1":
                {
                    var codCli = Convert.ToInt32(txtCodCli.Text);
                    var cpfOrig = UtilSIL.RetirarCaracteres(".-/",txtCpfUsuarioOri.Text);
                    var cpfDest = UtilSIL.RetirarCaracteres(".-/", txtCpfUsuarioDest.Text);
                    var crtOrig = (string) Session["CODCRTORIG"];
                    var crtDest = (string) Session["CODCRTDEST"];
                    var valor = Convert.ToDecimal(txtSaldoTransf.Text);
                    new blSolicitaCarga(GetOperador()).TransferenciaSaldo(Convert.ToInt32(tipoTransf), codCli,
                                                                          cpfOrig,
                                                                          cpfDest, crtOrig, crtDest, valor);
                    txtCodCli.ClientEnabled = false;
                    pnlOrigem.ClientVisible = true;
                    pnlDestino.ClientVisible = tipoTransf == "0";
                    txtSaldoCli.ClientVisible = tipoTransf == "1";
                    txtCpfUsuarioOri.ClientEnabled = false;
                    txtCpfUsuarioDest.ClientEnabled = false;

                    txtSaldoIniOri.Text =
                        Convert.ToString(Convert.ToDecimal(txtSaldoIniOri.Text) - Convert.ToDecimal(txtSaldoTransf.Text));
                    if (tipoTransf == "0" && txtSaldoIniDest.Text != string.Empty)
                        txtSaldoIniDest.Text =
                            Convert.ToString(Convert.ToDecimal(txtSaldoIniDest.Text) +
                                             Convert.ToDecimal(txtSaldoTransf.Text));
                    else if (txtSaldoCli.Text != string.Empty)
                        txtSaldoCli.Text = Convert.ToString(Convert.ToDecimal(txtSaldoCli.Text) +
                                                            Convert.ToDecimal(txtSaldoTransf.Text));
                }
                break;
            case "2":
                {
                    limparTela();
                    txtCodCli.ClientEnabled = true;
                    txtCpfUsuarioOri.ClientEnabled = true;
                    txtCpfUsuarioDest.ClientEnabled = true;
                }
                break;
        }
    }

    protected void limparTela()
    {
        if (GetOperador() == null) return;
        txtCodCli.Text = string.Empty;
        txtnomCli.Text = string.Empty;
        txtSaldoCli.Text = string.Empty;
    
        txtStatus.Text = string.Empty;
        txtCpfUsuarioOri.Text = string.Empty;
        txtNumCartaoOri.Text = string.Empty;
        txtnomUsuOri.Text = string.Empty;
        txtStatusUsuarioOri.Text = string.Empty;
        txtSaldoIniOri.Text = string.Empty;

        txtCpfUsuarioDest.Text = string.Empty;
        txtNumCartaoDest.Text = string.Empty;
        txtnomUsuDest.Text = string.Empty;
        txtStatusUsuarioDest.Text = string.Empty;
        txtSaldoIniDest.Text = string.Empty;

        txtSaldoTransf.Text = string.Empty;
    }

    protected void limparTelaOrigemDestino()
    {
        if (GetOperador() == null) return;
        txtCpfUsuarioOri.Text = string.Empty;
        txtNumCartaoOri.Text = string.Empty;
        txtnomUsuOri.Text = string.Empty;
        txtStatusUsuarioOri.Text = string.Empty;
        txtSaldoIniOri.Text = string.Empty;

        txtCpfUsuarioDest.Text = string.Empty;
        txtNumCartaoDest.Text = string.Empty;
        txtnomUsuDest.Text = string.Empty;
        txtStatusUsuarioDest.Text = string.Empty;
        txtSaldoIniDest.Text = string.Empty;

        txtSaldoTransf.Text = string.Empty;
    }

    #endregion
}
