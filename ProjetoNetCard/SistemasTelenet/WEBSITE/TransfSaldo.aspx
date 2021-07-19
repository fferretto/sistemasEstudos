<%@ Page Title="" Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true"
    CodeFile="TransfSaldo.aspx.cs" Inherits="TransfSaldo" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ MasterType VirtualPath="~/Cadastro.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title id="ViaSistema">Via Sistema</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCadastro" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('.valor').priceFormat({
                prefix: '',
                centsSeparator: ',',
                thousandsSeparator: '',
                limit: 10
            });
        });


        function AcionarMascara() {
            AplicarMascarasJuncao();
            $('#ctl00_cphCadastro_clbProcessa_txtSaldoTransf').priceFormat({ prefix: '', centsSeparator: ',', thousandsSeparator: '' });
        }
    </script>

    <%-- ToolBar --%>
    <div style="margin-bottom: 5px">
        <dxp:ASPxPanel ID="pnlToolBar" runat="server" Width="100%">
            <PanelCollection>
                <dxp:PanelContent ID="PanelContent5" runat="server">
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="btnProcessar" runat="server" AutoPostBack="False" ClientInstanceName="btnProcessar"
                                    CssClass="toolButton" ToolTip="Processar" Height="33px"
                                    ClientEnabled="False">
                                    <ClientSideEvents Click="function(s, e) { ProcessarTransfSaldo(); } " />
                                    <Image Url="Images/Small/42.png" UrlDisabled="~/Images/Small/42_Desabilitado.png" />
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnLimpar" runat="server" Text="" CssClass="toolButton" ToolTip="Limpar Tela"
                                    ClientInstanceName="btnLimpar" AutoPostBack="false" Height="33px">
                                    <Image Url="~/Images/Small/Clear.png" Width="25px" UrlDisabled="~/Images/Small/Cancelar_Desab.png" />
                                    <ClientSideEvents Click="function(s, e) { LimparTelaTransf(2); txtCodCli.Focus(); } " />
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dxp:PanelContent>
            </PanelCollection>
        </dxp:ASPxPanel>
    </div>
    
    <dxe:ASPxLabel ID="lblTitulo" runat="server" CssClass="TituloPanel" Text="TRANSFERENCIA DE SALDO" Width="100%">
    </dxe:ASPxLabel>    
    <asp:Panel ID="pnlSistema" runat="server" Width="99%" BorderStyle="Solid" BorderColor="LightGray" BorderWidth="1px" Height="450px">
        <dxcp:ASPxCallbackPanel ID="clbProcessa" runat="server" Width="100%" ClientInstanceName="clbProcessa" OnCallback="clbProcessa_Callback">
            <PanelCollection>
                <dxp:PanelContent ID="PanelContent4" runat="server">
                    <div style="padding-left: 16px; height: 470px;">
                        <div class="Label" style="margin-top: 10px">                            
                            <div style="width: 12.5%; float: left;">
                                Código Cliente
                            </div>
                            <div style="width: 37.5%; float: left;">
                                Razão Social
                            </div>
                            <div style="width: 25%; float: left;">
                                Status
                            </div>
                            <div id="saldo" runat="server" style="width: 25%; float: left;">
                                Saldo
                            </div>
                        </div>
                        <div style="float: left; width: 100%; margin-bottom: 10px; float: left">
                            <dxcp:ASPxCallbackPanel ID="clbCliente" runat="server"
                                ClientInstanceName="clbCliente" OnCallback="clbCliente_Callback" Width="100%">
                                <PanelCollection>
                                    <dxp:PanelContent ID="PanelContent2" runat="server">
                                        <div>                                            
                                            <div style="width: 12.5%; float: left">
                                                <div style="width: 80%; float: left">
                                                    <dxe:ASPxTextBox ID="txtCodCli" runat="server" ClientInstanceName="txtCodCli" Font-Names="Tahoma" 
                                                        Font-Size="Small" MaxLength="5" Width="92%" CssClass="TextBox" Native="true">
                                                        <ClientSideEvents ValueChanged="function(s,e){ SelecionaCliente(); }" />
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <div style="width: 20%; float: left">
                                                    <dxe:ASPxImage ID="imgStatusCliente" runat="server"
                                                        ClientInstanceName="imgStatusCliente" Visible="false">
                                                    </dxe:ASPxImage>
                                                </div>
                                            </div>
                                            <div style="float: left; width: 37.5%;">
                                                <dxe:ASPxTextBox ID="txtnomCli" runat="server" ClientInstanceName="txtnomCli" CssClass="TextBox" Native="true"
                                                    Font-Bold="true" ReadOnly="true" Width="92%">
                                                    <ReadOnlyStyle CssClass="EditDesabilitado"></ReadOnlyStyle>
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div style="float: left; width: 25%;">
                                                <dxe:ASPxTextBox ID="txtStatus" runat="server" ClientInstanceName="txtStatus" CssClass="TextBox" Native="true"
                                                    Font-Bold="true" ReadOnly="true" Width="90%">
                                                    <ReadOnlyStyle CssClass="EditDesabilitado">
                                                    </ReadOnlyStyle>
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div style="float: left; width: 25%;">
                                                <dxe:ASPxTextBox ID="txtSaldoCli" runat="server" CssClass="TextBox" Native="true"
                                                    ClientInstanceName="txtSaldoCli" Font-Bold="true" ReadOnly="true" Width="90%">
                                                    <ReadOnlyStyle CssClass="EditDesabilitado">
                                                    </ReadOnlyStyle>
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="hdClienteLiberado" runat="server" />
                                    </dxp:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s, e){ trataSelecaoCliente(); AplicarMascarasJuncao(); }" />
                            </dxcp:ASPxCallbackPanel>
                        </div>
                        <dxp:ASPxPanel ID="pnlOrigem" runat="server" Width="100%"
                            ClientInstanceName="pnlOrigem" ClientVisible="False">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent6" runat="server">
                                    <div style="width: 100%; margin-bottom: 5px; margin-top: 5px; font-weight: bold; padding-left: 2px; float: left">
                                        Informe o CPF de origem a debitar :
                                        <br />
                                        <div class="Label" style="margin-top: 10px">
                                            <div style="width: 12.5%; float: left;">
                                                CPF Origem
                                            </div>
                                            <div style="width: 37.5%; float: left;">
                                                Nome
                                            </div>
                                            <div style="width: 25%; float: left;">
                                                Nº Cartao
                                            </div>
                                            <div style="width: 12.5%; float: left;">
                                                Status
                                            </div>
                                            <div style="width: 12.5%; float: left;">
                                                Saldo
                                            </div>
                                        </div>
                                        <div style="width: 100%; margin-bottom: 10px; float: left">
                                            <dxcp:ASPxCallbackPanel ID="clbDadosUsuOri" runat="server" Width="100%" ClientInstanceName="clbDadosUsuOri"
                                                OnCallback="clbDadosUsuOri_Callback">
                                                <PanelCollection>
                                                    <dxp:PanelContent ID="PanelContent3" runat="server">                                                        
                                                        <div style="width: 12.5%; float: left">
                                                            <div style="width: 80%; float: left">
                                                                <dxe:ASPxTextBox ID="txtCpfUsuarioOri" runat="server" CssClass="TextBox MaskCPFTemp" Native="true"
                                                                    ClientInstanceName="txtCpfUsuarioOri" Font-Names="Tahoma" Font-Size="small" MaxLength="11" Width="92%">
                                                                    <ClientSideEvents ValueChanged="function(s,e){ SelecionaCartaoTranfOri(this); }" TextChanged="function(s,e){  txtCpfUsuarioDest.Focus(); }" />
                                                                </dxe:ASPxTextBox>
                                                            </div>
                                                            <div style="width: 20%; float: left">
                                                                <dxe:ASPxImage ID="imgIncluirCartaoOri" runat="server"
                                                                    ClientInstanceName="imgIncluirCartaoOri" Visible="false">
                                                                </dxe:ASPxImage>
                                                            </div>
                                                        </div>
                                                        <div style="text-align: left; width: 37.5%; float: left">
                                                            <dxe:ASPxTextBox ID="txtnomUsuOri" runat="server"
                                                                ClientInstanceName="txtnomUsuOri" Font-Bold="true" ReadOnly="true" Width="92%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <div style="width: 25%; float: left">
                                                            <dxe:ASPxTextBox ID="txtNumCartaoOri" runat="server"
                                                                ClientInstanceName="txtNumCartaoOri" Font-Bold="true" ReadOnly="true"
                                                                Width="92%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <div style="text-align: left; width: 12.5%; float: left">
                                                            <dxe:ASPxTextBox ID="txtStatusUsuarioOri" runat="server" ClientInstanceName="txtStatusUsuarioOri" Font-Bold="true"
                                                                ReadOnly="true" Width="80%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <div style="text-align: left; width: 12.5%; float: left">
                                                            <dxe:ASPxTextBox ID="txtSaldoIniOri" runat="server" ClientInstanceName="txtSaldoIniOri" Font-Bold="true" ReadOnly="true" Width="80%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <asp:HiddenField ID="hdUsuarioLiberadoOri" runat="server" />
                                                    </dxp:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="function(s, e) { exibePainelOri();  AplicarMascarasJuncao(); }" />
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                    </div>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                        <dxp:ASPxPanel ID="pnlDestino" runat="server" Width="100%"
                            ClientInstanceName="pnlDestino" ClientVisible="False">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent7" runat="server">
                                    <div style="width: 100%; margin-bottom: 5px; margin-top: 5px; font-weight: bold; padding-left: 2px; float: left">
                                        Informe o CPF de destino a creditar :
                                    <br />
                                        <div class="Label" style="margin-top: 10px">
                                            <div style="width: 12.5%; float: left;">
                                                CPF Destino
                                            </div>
                                            <div style="width: 37.5%; float: left;">
                                                Nome
                                            </div>
                                            <div style="width: 25%; float: left;">
                                                NºCartao
                                            </div>
                                            <div style="width: 12.5%; float: left;">
                                                Status
                                            </div>
                                            <div style="width: 12.5%; float: left;">
                                                Saldo
                                            </div>
                                        </div>
                                        <div style="width: 100%; margin-bottom: 10px; float: left">
                                            <dxcp:ASPxCallbackPanel ID="clbDadosUsuDestino" runat="server" Width="100%" ClientInstanceName="clbDadosUsuDestino"
                                                OnCallback="clbDadosUsuDestino_Callback">
                                                <PanelCollection>
                                                    <dxp:PanelContent ID="PanelContent1" runat="server">                                                        
                                                        <div style="width: 12.5%; float: left">
                                                            <div style="width: 80%;float: left">
                                                                <dxe:ASPxTextBox ID="txtCpfUsuarioDest" runat="server" CssClass="TextBox MaskCPFTemp" Native="true"
                                                                    ClientInstanceName="txtCpfUsuarioDest" MaxLength="11" Width="92%">
                                                                    <ClientSideEvents ValueChanged="function(s,e){ SelecionaCartaoTranfDest(); }" />
                                                                </dxe:ASPxTextBox>
                                                            </div>
                                                            <div style="width: 20%; float: left">
                                                                <dxe:ASPxImage ID="imgIncluirCartaoDest" runat="server"
                                                                    ClientInstanceName="imgIncluirCartaoDest" Visible="false">
                                                                </dxe:ASPxImage>
                                                            </div>
                                                        </div>
                                                        <div style="width: 37.5%; float: left">
                                                            <dxe:ASPxTextBox ID="txtnomUsuDest" runat="server" CssClass="TextBox" Native="true"
                                                                ClientInstanceName="txtnomUsuDest" Font-Bold="true" ReadOnly="true" Width="92%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <div style="width: 25%; float: left">
                                                            <dxe:ASPxTextBox ID="txtNumCartaoDest" runat="server" CssClass="TextBox"
                                                                ClientInstanceName="txtNumCartaoDest" Font-Bold="true" ReadOnly="true" Width="92%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <div style="width: 12.5%; float: left">
                                                            <dxe:ASPxTextBox ID="txtStatusUsuarioDest" runat="server" CssClass="TextBox" Native="true"
                                                                ClientInstanceName="txtStatusUsuarioDest" Font-Bold="true" ReadOnly="true" Width="80%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <div style="width: 12.5%; float: left">
                                                            <dxe:ASPxTextBox ID="txtSaldoIniDest" runat="server" CssClass="TextBox" Native="true"
                                                                ClientInstanceName="txtSaldoIniDest" Font-Bold="true" ReadOnly="true" Width="80%">
                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                </ReadOnlyStyle>
                                                            </dxe:ASPxTextBox>
                                                        </div>
                                                        <asp:HiddenField ID="hdUsuarioLiberadoDest" runat="server" />
                                                    </dxp:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="function(s,e){ exibePainelDest();  AplicarMascarasJuncao(); }" />
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                    </div>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                        <dxp:ASPxPanel ID="pnlSaldoTransf" runat="server" Width="100%" ClientInstanceName="pnlSaldoTransf" ClientVisible="False">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent8" runat="server">
                                    <div style="width: 100%; margin-bottom: 5px; margin-top: 5px; font-weight: bold; padding-left: 2px; float: left">
                                        Informe o valor a ser transferido:
                                        <br />
                                        <div class="Label" style="margin-top: 10px">
                                            <div style="width: 100%; float: left;">
                                                Valor:
                                            </div>
                                        </div>
                                        <div style="width: 100%; float: left;">
                                            <asp:TextBox ID="txtSaldoTransf" runat="server" class="TextBox MaskMoeda" Width="15%" Text="0,00" MaxLength="10">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxp:ASPxPanel>
                    </div>
                </dxp:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="AcionarMascara" />
        </dxcp:ASPxCallbackPanel>
    </asp:Panel>    
</asp:Content>
