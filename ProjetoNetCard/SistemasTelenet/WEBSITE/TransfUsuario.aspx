<%@ Page Title="" Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true" CodeFile="TransfUsuario.aspx.cs" Inherits="TransfUsuario" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ MasterType VirtualPath="~/Cadastro.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dxuc" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxwm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register TagPrefix="dxp" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v8.3" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v8.3" %>

<%@ Register TagPrefix="dxtc" Namespace="DevExpress.Web.ASPxTabControl" Assembly="DevExpress.Web.v8.3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title id="TranfUsuarioCliente">Tranferência Usuário</title>
    <%--<script type="text/javascript" src="ILL/AutorizaCargas.js"></script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphCadastro" runat="Server">

    <asp:HiddenField ID="hddSituacaoClienteOrigemOk" runat="server" />
    <asp:HiddenField ID="hddSituacaoClienteDestinoOk" runat="server" />
    <asp:HiddenField ID="hddSituacaoCpfOrigemOk" runat="server" />

    <%-- ToolBar --%>
    <div style="margin-bottom: 5px">
        <dxp:ASPxPanel ID="pnlToolBar" runat="server" Width="100%" Visible="true">
            <PanelCollection>
                <dxp:PanelContent ID="PanelContent5" runat="server">
                    <table>
                        <tr>
                            <td>
                                <dxe:ASPxButton ID="btnProcessar" runat="server" AutoPostBack="False" Visible="false" ClientInstanceName="btnProcessar"
                                    CssClass="toolButton" ToolTip="Processar" Height="33px"
                                    ClientEnabled="false">
                                    <ClientSideEvents Click="function(s, e) { ProcessarTransfUsuario(); } " />
                                    <Image Url="Images/Small/42.png" UrlDisabled="~/Images/Small/42_Desabilitado.png" />
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnLimpar" runat="server" Text="" CssClass="toolButton" ToolTip="Limpar Tela"
                                    ClientInstanceName="btnLimpar" AutoPostBack="false" Height="33px">
                                    <Image Url="~/Images/Small/Clear.png" Width="25px" UrlDisabled="~/Images/Small/Cancelar_Desab.png" />
                                    <ClientSideEvents Click="function(s, e) { LimparTelaTransfUsuario(); txtCodCliOrigem.Focus(); } " />
                                </dxe:ASPxButton>
                            </td>
                            <td>
                                <dxe:ASPxButton ID="btnLog" runat="server" AutoPostBack="true" ClientInstanceName="btnLog"
                                    CssClass="toolButton" ToolTip="Arquivo de Log" Text="LOG" Height="33px" Visible="false"
                                    Font-Bold="true" OnClick="btnLog_Click">
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dxp:PanelContent>
            </PanelCollection>
        </dxp:ASPxPanel>
    </div>

    <div style="width: 100%; background-color: #F4F4F4; height: 20px; border: solid 1px LightGray; font-weight: bold; margin-top: 5px; margin-left: auto; margin-right: auto;">
        <dxe:ASPxLabel ID="lblTitulo" runat="server" margin-top="1%" Text="TRANSFERÊNCIA DE USUÁRIO PÓS PAGO"></dxe:ASPxLabel>
    </div>

    <asp:Panel ID="pnlSistema" runat="server" Width="100%" BorderStyle="Solid" BorderColor="LightGray" BorderWidth="1px" Height="650px">

        <br />

        <dxtc:ASPxPageControl ID="pgcUsuario" runat="server" ActiveTabIndex="0" ClientInstanceName="pgcUsuario"
            Width="100%" EnableCallbackCompression="True" EnableCallBacks="True" ClientVisible="true"
            EnableViewState="False" LoadingPanelText="Carregando&amp;hellip;"
            Paddings-PaddingBottom="15px">
            <TabPages>
                <dxtc:TabPage Text="Manual" Name="tpeManual">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <dxcp:ASPxCallbackPanel ID="clbProcessa" runat="server" Width="100%" ClientInstanceName="clbProcessa" OnCallback="clbProcessa_Callback">
                                <PanelCollection>
                                    <dxp:PanelContent ID="PanelContent4" runat="server">
                                        <div style="width: 100%; margin-bottom: 7px; margin-top: 15px; padding-left: 5px; font-weight: bold">
                                            Informe o Codigo do Cliente Pós Pago de Origem:
                                        </div>
                                        <div>
                                            <div style="width: 10%; float: left; font-size: x-small; font-weight: bold">
                                                &nbsp;Código Cliente Pós Pago
                                            </div>
                                            <div style="width: 54%; float: left; font-size: x-small; font-weight: bold">
                                                Razão Social
                                            </div>
                                        </div>
                                        <div style="float: left; width: 100%; margin-bottom: 10px; float: left">
                                            <dxcp:ASPxCallbackPanel ID="clbClienteOrigem" runat="server" ClientInstanceName="clbClienteOrigem" Width="100%" OnCallback="clbClienteOrigem_Callback">
                                                <PanelCollection>
                                                    <dxp:PanelContent ID="PanelContent2" runat="server">
                                                        <div>
                                                            <div style="width: 5%; float: left;"></div>
                                                            <div style="width: 10%; float: left">
                                                                <div style="width: 62%; padding-left: 2px; margin-bottom: 20px; float: left">
                                                                    <dxe:ASPxTextBox ID="txtCodCliOrigem" runat="server" CssClass="MaskNum"
                                                                        ClientInstanceName="txtCodCliOrigem" Font-Names="Tahoma" Font-Size="Small"
                                                                        MaxLength="5" Width="100px">
                                                                        <ClientSideEvents ValueChanged="function(s,e){ SelecionaClienteOrigemTransferenciaUsuario(); 
                                                                            }" />
                                                                    </dxe:ASPxTextBox>
                                                                </div>
                                                                <div style="float: left; width: 10%; margin-bottom: 0px;">
                                                                    <dxe:ASPxImage ID="imgStatusClienteOrigem" runat="server"
                                                                        ClientInstanceName="imgStatusClienteOrigem" Visible="false">
                                                                    </dxe:ASPxImage>
                                                                </div>
                                                            </div>
                                                            <div style="float: left; width: 54%;">
                                                                <dxe:ASPxTextBox ID="txtnomCliOrigem" runat="server" Native="true" ClientInstanceName="txtnomCliOrigem"
                                                                    Font-Bold="true" ReadOnly="true" Width="90%">
                                                                    <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                    </ReadOnlyStyle>
                                                                </dxe:ASPxTextBox>
                                                                <dxwm:ASPxPopupMenu ID="menuSelClienteOrigem" ClientInstanceName="menuSelClienteOrigem" runat="server">
                                                                    <ClientSideEvents ItemClick="function(s, e){ PopUpClienteOrigemClick(s, e); }" />
                                                                </dxwm:ASPxPopupMenu>
                                                            </div>
                                                        </div>
                                                        <asp:HiddenField ID="hdClienteOrigemOk" runat="server" />
                                                    </dxp:PanelContent>
                                                </PanelCollection>
                                                <ClientSideEvents EndCallback="function(s, e){ 
                                                    MaskCPFTemp();
                                                    MaskNumeros();
                                                    AbrirMenuClienteOrigem();
                                                    HabilitaDesabilitaUsuario();
                                                }" />
                                            </dxcp:ASPxCallbackPanel>
                                        </div>
                                        <dxp:ASPxPanel ID="pnlOrigem" runat="server" Width="100%" ClientInstanceName="pnlOrigem">
                                            <PanelCollection>
                                                <dxp:PanelContent ID="PanelContent6" runat="server">
                                                    <div style="width: 100%; margin-bottom: 7px; font-weight: bold; padding-left: 2px; float: left">
                                                        Informe o Codigo do Cliente Pós Pago de Destino:                                
                                                    </div>
                                                    <div>
                                                        <div style="width: 5%; float: left;"></div>
                                                        <div style="width: 10%; float: left; font-size: x-small; font-weight: bold">
                                                            &nbsp;Código Cliente Pós Pago
                                                        </div>
                                                        <div style="width: 20%; float: left; font-size: x-small; font-weight: bold">
                                                            Razão Social
                                                        </div>

                                                    </div>
                                                    <div style="width: 100%; margin-bottom: 10px; float: left">
                                                        <dxcp:ASPxCallbackPanel ID="clbClienteDestino" runat="server" Width="100%" ClientInstanceName="clbClienteDestino" OnCallback="clbClienteDestino_Callback">
                                                            <PanelCollection>
                                                                <dxp:PanelContent ID="PanelContent3" runat="server">
                                                                    <div>
                                                                        <div style="width: 5%; float: left;"></div>
                                                                        <div style="width: 10%; float: left">
                                                                            <div style="width: 62%; padding-left: 2px; margin-bottom: 20px; float: left">
                                                                                <dxe:ASPxTextBox ID="txtCodCliDestino" runat="server" CssClass="MaskNum"
                                                                                    ClientInstanceName="txtCodCliDestino" Font-Names="Tahoma" Font-Size="small" MaxLength="5" Width="100px">
                                                                                    <ClientSideEvents ValueChanged="function(s,e){ SelecionaClienteDestinoTransferenciaUsuario(); }" />
                                                                                </dxe:ASPxTextBox>
                                                                            </div>
                                                                            <div style="width: 10%; float: left">
                                                                                <dxe:ASPxImage ID="imgStatusClienteDestino" runat="server"
                                                                                    ClientInstanceName="imgStatusClienteDestino" Visible="false">
                                                                                </dxe:ASPxImage>
                                                                            </div>
                                                                        </div>
                                                                        <div style="text-align: left; width: 54%; float: left">
                                                                            <dxe:ASPxTextBox ID="txtnomCliDestino" runat="server" Native="true"
                                                                                ClientInstanceName="txtnomCliDestino" Font-Bold="true" ReadOnly="true"
                                                                                Width="90%">
                                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                </ReadOnlyStyle>
                                                                            </dxe:ASPxTextBox>
                                                                            <dxwm:ASPxPopupMenu ID="menuSelClienteDestino" ClientInstanceName="menuSelClienteDestino" runat="server">
                                                                                <ClientSideEvents ItemClick="function(s, e){ PopUpClienteDestinoClick(s, e); }" />
                                                                            </dxwm:ASPxPopupMenu>
                                                                        </div>
                                                                    </div>
                                                                    <asp:HiddenField ID="hdClienteDestinoOk" runat="server" />

                                                                </dxp:PanelContent>
                                                            </PanelCollection>
                                                            <ClientSideEvents EndCallback="function(s, e){
                                                                MaskCPFTemp();
                                                                MaskNumeros();
                                                                AbrirMenuClienteDestino();
                                                                HabilitaDesabilitaUsuario();
                                                            }" />
                                                        </dxcp:ASPxCallbackPanel>
                                                    </div>
                                                </dxp:PanelContent>
                                            </PanelCollection>
                                        </dxp:ASPxPanel>
                                        <dxp:ASPxPanel ID="pnlDestino" runat="server" Width="100%" ClientInstanceName="pnlDestino">
                                            <PanelCollection>
                                                <dxp:PanelContent ID="PanelContent7" runat="server">
                                                    <div style="width: 100%; margin-bottom: 7px; font-weight: bold; padding-left: 2px; float: left">
                                                        Informe o CPF de Origem a ser transferido:
                                                    </div>
                                                    <div>
                                                        <div style="width: 5%; float: left;"></div>
                                                        <div style="width: 10%; float: left; font-size: x-small; font-weight: bold">
                                                            &nbsp;CPF Usuario
                                                        </div>
                                                        <div style="width: 55%; float: left; font-size: x-small; font-weight: bold">
                                                            Nome
                                                        </div>

                                                    </div>
                                                    <div style="width: 100%; margin-bottom: 10px; float: left">
                                                        <dxcp:ASPxCallbackPanel ID="clbCpfUsuarioOrigem" runat="server" Width="100%" ClientInstanceName="clbCpfUsuarioOrigem" OnCallback="clbCpfUsuarioOrigem_Callback">
                                                            <PanelCollection>
                                                                <dxp:PanelContent ID="PanelContent1" runat="server">
                                                                    <div>
                                                                        <div style="width: 5%; float: left;"></div>
                                                                        <div style="width: 10%; float: left">
                                                                            <div style="width: 72%; padding-left: 2px; margin-bottom: 20px; float: left">
                                                                                <dxe:ASPxTextBox ID="txtCpfUsuarioOrigem" runat="server" Native="true" CssClass="TextBox MaskCPFTemp"
                                                                                    ClientInstanceName="txtCpfUsuarioOrigem" Font-Names="Tahoma" Font-Size="small"
                                                                                    MaxLength="11" Width="100px">
                                                                                    <%--     <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                        </ReadOnlyStyle>--%>
                                                                                    <ClientSideEvents ValueChanged="function(s,e){ SelecionaCartaoTranfUsuarioOrigem(); }" />
                                                                                </dxe:ASPxTextBox>
                                                                            </div>
                                                                            <div style="width: 10%; float: left">
                                                                                <dxe:ASPxImage ID="imgIncluirCartaoOrigem" runat="server"
                                                                                    ClientInstanceName="imgIncluirCartaoOrigem" Visible="false">
                                                                                </dxe:ASPxImage>
                                                                            </div>
                                                                        </div>
                                                                        <div style="text-align: left; width: 55%; float: left">
                                                                            <dxe:ASPxTextBox ID="txtnomUsuOrigem" runat="server" Native="true"
                                                                                ClientInstanceName="txtnomUsuOrigem" Font-Bold="true" ReadOnly="true" Width="90%">
                                                                                <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                </ReadOnlyStyle>
                                                                            </dxe:ASPxTextBox>
                                                                        </div>
                                                                        <div>
                                                                            <dxe:ASPxButton ID="btnProcessarManual" runat="server" Text="Processar" ClientEnabled="false" ClientInstanceName="btnProcessarManual">
                                                                                <ClientSideEvents Click="function(s, e) { ProcessarTransfUsuario(); } " />
                                                                            </dxe:ASPxButton>
                                                                        </div>
                                                                    </div>
                                                                    <asp:HiddenField ID="hdCpfOrigemOk" runat="server" />
                                                                </dxp:PanelContent>
                                                            </PanelCollection>

                                                        </dxcp:ASPxCallbackPanel>
                                                    </div>
                                                </dxp:PanelContent>
                                            </PanelCollection>
                                        </dxp:ASPxPanel>
                                        <dxp:ASPxPanel ID="pnlResultado" runat="server" Width="100%" ClientInstanceName="pnlResultado" Visible="false">
                                            <PanelCollection>
                                                <dxp:PanelContent ID="PanelContent8" runat="server">
                                                    <div style="width: 100%; margin-bottom: 5px; margin-top: 5px; font-weight: bold; padding-left: 2px; float: left">
                                                        <div style="width: 100%; margin-bottom: 10px; float: left">
                                                            <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%">
                                                                <PanelCollection>
                                                                    <dxp:PanelContent ID="PanelContent9" runat="server">
                                                                        <div align="center">
                                                                            <div style="width: 25%; float: left;"></div>
                                                                            <div style="width: 10%; float: left">
                                                                                <div style="width: 62%; padding-left: 2px; margin-bottom: 20px; float: left">
                                                                                    <dxe:ASPxTextBox ID="txtResultado" runat="server"
                                                                                        ClientInstanceName="txtResultado" Font-Names="Tahoma" Font-Size="small"
                                                                                        MaxLength="11" Width="700px">
                                                                                        <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                        </ReadOnlyStyle>
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>

                                                                            </div>

                                                                        </div>
                                                                    </dxp:PanelContent>
                                                                </PanelCollection>

                                                            </dxcp:ASPxCallbackPanel>
                                                        </div>
                                                    </div>
                                                </dxp:PanelContent>
                                            </PanelCollection>
                                        </dxp:ASPxPanel>
                                        <dxp:ASPxPanel ID="panelProcessarManual" runat="server" Width="100%" ClientInstanceName="panelProcessarManual">
                                            <PanelCollection>
                                                <dxp:PanelContent ID="PanelContent10" runat="server">
                                                    <div style="width: 100%; margin-bottom: 5px; margin-top: 5px; font-weight: bold; padding-left: 2px;">
                                                        <div style="width: 100%; margin-bottom: 10px; float: left">
                                                            <dxcp:ASPxCallbackPanel ID="ASPxCallbackPanel2" runat="server" Width="100%">
                                                                <PanelCollection>
                                                                    <dxp:PanelContent ID="PanelContent11" runat="server">
                                                                        <div>
                                                                        </div>
                                                                    </dxp:PanelContent>
                                                                </PanelCollection>

                                                            </dxcp:ASPxCallbackPanel>
                                                        </div>
                                                    </div>
                                                </dxp:PanelContent>
                                            </PanelCollection>
                                        </dxp:ASPxPanel>
                                    </dxp:PanelContent>
                                </PanelCollection>
                                <%--<ClientSideEvents BeginCallback="OnInitCallback" />--%>
                            </dxcp:ASPxCallbackPanel>


                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>

                <dxtc:TabPage Text="Via Arquivo" Name="tpeArquivo" Visible="true">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">                            
                            <asp:Panel ID="pnlArquivo" runat="server" Width="100%" BorderStyle="Solid" BorderColor="LightGray"
                                BorderWidth="1px" Height="100px">
                                <div style="width: 100%; margin-top: 30px; margin-bottom: 10px; padding-left: 10px; vertical-align: middle">
                                    <dxuc:ASPxUploadControl ID="uplArquivo" runat="server" Size="500" ClientInstanceName="uplArquivo" Width="600px"
                                        OnFileUploadComplete="uplArquivo_FileUploadComplete">
                                    </dxuc:ASPxUploadControl>
                                </div>
                            </asp:Panel>
                            <dxp:ASPxPanel ID="pnlProcessarArquivo" runat="server" Width="100%" ClientInstanceName="pnlProcessarArquivo">
                                <PanelCollection>
                                    <dxp:PanelContent ID="PanelContent12" runat="server">
                                        <div style="width: 100%; margin-bottom: 5px; margin-top: 5px; font-weight: bold; padding-left: 2px;">
                                            <div style="width: 100%; margin-bottom: 10px; float: left">
                                                <div>
                                                    <dxe:ASPxButton ID="btnProcessarArquivo" ClientInstanceName="btnProcessarArquivo" 
                                                        runat="server" Text="Processar" OnClick="Page_Load">                                                                    
                                                    </dxe:ASPxButton>
                                                </div>
                                            </div>
                                            <div style="width: 100%; margin-bottom: 10px; float: left">
                                                <dxe:ASPxLabel ID="lblMensagemArquivo" ClientInstanceName="lblMensagemArquivo" 
                                                    runat="server" Font-Bold="True">
                                                </dxe:ASPxLabel>
                                            </div>
                                        </div>
                                    </dxp:PanelContent>
                                </PanelCollection>
                            </dxp:ASPxPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
            <Paddings PaddingBottom="15px"></Paddings>
            <ActiveTabStyle VerticalAlign="Top">
            </ActiveTabStyle>
            <ContentStyle VerticalAlign="Top">
            </ContentStyle>
        </dxtc:ASPxPageControl>
    </asp:Panel>

    <%--Área de mensagens do sistema--%>
    <dxpc:ASPxPopupControl ID="ppcMensagem" runat="server" ClientInstanceName="ppcMensagem"
        HeaderText="Mensagens" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Width="600px" PopupAction="None" Modal="true" CloseAction="None">
        <ClientSideEvents Shown="function(s, e){ window.btnCancel.SetFocus(); }" CloseButtonClick="function(s, e) { try { window.ppcMensagem.SetVisible(false); } catch (erro) { } }" />
        <HeaderStyle Font-Bold="True" />
        <Windows>
            <dxpc:PopupWindow Name="Mensagem" Modal="true">
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <div style="padding-bottom: 10px">
                            <dxe:ASPxLabel ID="lblMensagem" ClientInstanceName="lblMensagem" runat="server" Font-Bold="True">
                            </dxe:ASPxLabel>
                        </div>
                        <div style="padding-left: 43%;">
                            <dxe:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel"
                                Text="Ok" Width="80px">
                                <ClientSideEvents Click="function(s, e) { 
                                    window.ppcMensagem.HideWindow(window.ppcMensagem.GetWindowByName('Mensagem'));
                                    document.cookie = 'MsgErro' + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                                    document.cookie = 'MsgRetorno' + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                                    excuteCloseFn();
                                }" />
                            </dxe:ASPxButton>
                        </div>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:PopupWindow>

            <dxpc:PopupWindow Name="ConfirmaSimNao" Modal="true">
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
                        <div style="padding-bottom: 10px">
                            <dxe:ASPxLabel ID="lblMensagemSimNao" ClientInstanceName="lblMensagemSimNao" runat="server" Font-Bold="True">
                            </dxe:ASPxLabel>
                        </div>
                        <div style="padding-left: 43%;">
                            <div style="padding-left: 43%;">
                                <div style="float: left; width: 50%;">
                                    <dxe:ASPxButton ID="btnSim" runat="server" AutoPostBack="False" ClientInstanceName="btnSim"
                                        Text="Sim" Width="80px">
                                        <ClientSideEvents Click="function(s, e) { 
                                        excuteSimNaoHandler($(window.ppcMensagem).data('fnOk'));
                                    }" />
                                    </dxe:ASPxButton>
                                </div>
                                <div style="float: left; width: 50%;">
                                    <dxe:ASPxButton ID="btnNao" runat="server" AutoPostBack="False" ClientInstanceName="btnNao"
                                        Text="Não" Width="80px">
                                        <ClientSideEvents Click="function(s, e) { 
                                        excuteSimNaoHandler($(window.ppcMensagem).data('fnCancel'));
                                    }" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:PopupWindow>
        </Windows>
    </dxpc:ASPxPopupControl>
</asp:Content>

