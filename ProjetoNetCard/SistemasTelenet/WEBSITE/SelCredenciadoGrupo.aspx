<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelCredenciadoGrupo.aspx.cs"
    Inherits="SelCredenciadoGrupo" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v8.3" %>
<%@ Register TagPrefix="dxwgv" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v8.3" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="App_Themes/Estilos.css" rel="stylesheet" type="text/css" />
    <title></title>

    <script type="text/javascript" src="ILL/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="ILL/jquery-latest.js"></script>

    <script type="text/javascript" src="ILL/jquery.autocomplete.js"></script>

    <script>

    </script>
</head>
<body onblur="this.focus()">
    <form id="form1" runat="server" autocomplete="off" novalidate>
        <div style="width: 99%;">
            <div style="width: 97%; padding-top: 20px; padding-bottom: 10px; padding-left: 15px">
                <table width="100%" class="tableFiltros">
                    <tr class="linha">
                        <td style="font-size: 12pt; font-weight: bold;">
                            <dxe:ASPxLabel ID="lblCabecalho" runat="server" Text="  NetcardVA :: Inclusao de Credenciados para o Grupo ">
                            </dxe:ASPxLabel>
                        </td>
                    </tr>
                </table>
            </div>
            <table style="margin-left: 5px">
                <tr>
                    <td style="padding-left: 16px">
                        <dxe:ASPxLabel ID="lblAbrev" runat="server" Text="Código" />
                    </td>
                    <td style="padding-left: 15px">
                        <dxe:ASPxLabel ID="Segmento" runat="server" Text="Segmento" />
                    </td>
                    <td style="padding-left: 15px">
                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="UF" />
                    </td>
                    <td style="padding-left: 15px">
                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Localidade" />
                    </td>
                    <td style="padding-left: 15px">
                        <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Bairro" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 16px">
                        <asp:TextBox ID="txtCodigo" runat="server" Width="80px" CssClass="TextBox" MaxLength="6"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px">
                        <asp:TextBox ID="txtSegmento" runat="server" Width="180px" CssClass="TextBox"></asp:TextBox>
                    </td>
                    <td style="padding-left: 5px">
                        <dxe:ASPxComboBox ID="ddlUf" runat="server" SelectedIndex="0" ValueType="System.String"
                            Native="True" CssClass="ComboBox">
                            <Items>
                                <dxe:ListEditItem Text="AC" Value="AC" />
                                <dxe:ListEditItem Text="AL" Value="AL" />
                                <dxe:ListEditItem Text="AP" Value="AP" />
                                <dxe:ListEditItem Text="AM" Value="AM" />
                                <dxe:ListEditItem Text="BA" Value="BA" />
                                <dxe:ListEditItem Text="CE" Value="CE" />
                                <dxe:ListEditItem Text="DF" Value="DF" />
                                <dxe:ListEditItem Text="ES" Value="ES" />
                                <dxe:ListEditItem Text="GO" Value="GO" />
                                <dxe:ListEditItem Text="MA" Value="MA" />
                                <dxe:ListEditItem Text="MT" Value="MT" />
                                <dxe:ListEditItem Text="MS" Value="MS" />
                                <dxe:ListEditItem Text="MG" Value="MG" />
                                <dxe:ListEditItem Text="PA" Value="PA" />
                                <dxe:ListEditItem Text="PB" Value="PB" />
                                <dxe:ListEditItem Text="PR" Value="PR" />
                                <dxe:ListEditItem Text="PE" Value="PE" />
                                <dxe:ListEditItem Text="PI" Value="PI" />
                                <dxe:ListEditItem Text="RJ" Value="RJ" />
                                <dxe:ListEditItem Text="RN" Value="RN" />
                                <dxe:ListEditItem Text="RS" Value="RS" />
                                <dxe:ListEditItem Text="RO" Value="RO" />
                                <dxe:ListEditItem Text="RR" Value="RR" />
                                <dxe:ListEditItem Text="SC" Value="SC" />
                                <dxe:ListEditItem Text="SP" Value="SP" />
                                <dxe:ListEditItem Text="SE" Value="SE" />
                                <dxe:ListEditItem Text="TO" Value="TO" />
                            </Items>
                        </dxe:ASPxComboBox>
                    </td>
                    <td style="padding-left: 15px">
                        <asp:TextBox ID="txtLocalidade" runat="server" Width="180px" CssClass="TextBox"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px">
                        <asp:TextBox ID="txtBairro" runat="server" CssClass="TextBox" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hd" runat="server" />
                        <asp:HiddenField ID="hdGrupo" runat="server" />
                        <asp:HiddenField ID="hdPesquisa" runat="server" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-left: 15px">
                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" ClientInstanceName="btnCancelar"
                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" Text="Listar Credenciados"
                            Width="50%">
                            <ClientSideEvents Click="function (s,e) {grvCredenciados.PerformCallback() }"></ClientSideEvents>
                        </dxe:ASPxButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="padding-left: 15px">
                        <dxwgv:ASPxGridView ID="grvCredenciados" runat="server" Width="100%" AutoGenerateColumns="False"
                            OnCustomCallback="grvCredenciados_CustomCallback" KeyFieldName="CODCRE" EnableViewState="false"
                            ClientInstanceName="grvCredenciados" Enabled="True" OnPageIndexChanged="grvCredenciados_PageIndexChanged">
                            <Columns>
                                <dxwgv:GridViewDataTextColumn VisibleIndex="0" FieldName="CODCRE" Caption="Código"
                                    Width="20%">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn VisibleIndex="2" FieldName="RAZSOC" Caption="Razao Social">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn VisibleIndex="1" FieldName="NOMFAN" Caption="Nome Fantasia">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <ClientSideEvents EndCallback="function (s,e) { FecharListaCredenciadosForaGrupo(); ExibeMensagem();}"></ClientSideEvents>
                            <SettingsBehavior AllowFocusedRow="True" AllowMultiSelection="True" />
                            <SettingsPager PageSize="12">
                                <AllButton Text="All">
                                </AllButton>
                                <NextPageButton Text="Próximo">
                                </NextPageButton>
                                <PrevPageButton Text="Anterior">
                                </PrevPageButton>
                            </SettingsPager>
                            <SettingsText EmptyDataRow="Nenhum registro encontrado" />
                        </dxwgv:ASPxGridView>
                    </td>
                </tr>
            </table>
            <div style="position: absolute; bottom: 0; margin-bottom: 15px; width: 50%; margin-left: 30%">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dxe:ASPxButton ID="btnSalvarConsulta" runat="server" ClientInstanceName="btnSalvarConsulta"
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" Text="OK"
                                Width="70%" EnableViewState="False" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) { CadCredGrupo(); }" />
                            </dxe:ASPxButton>
                        </td>
                        <td>
                            <dxe:ASPxButton ID="btnCancelar" runat="server" AutoPostBack="False" ClientInstanceName="btnCancelar"
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" Text="Sair"
                                Width="70%" PostBackUrl="~/CadGrupoCredenciado.aspx">
                                <ClientSideEvents Click="function (s,e){ window.close();}"></ClientSideEvents>
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <dxpc:aspxpopupcontrol id="ppcMensagem" runat="server" clientinstancename="ppcMensagem"
                    headertext="Mensagens" popuphorizontalalign="WindowCenter" popupverticalalign="WindowCenter"
                    width="600px" popupaction="None" modal="true" closeaction="None">
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
                                                window.ppcMensagem.HideWindow(window.ppcMensagem.GetWindowByName('Mensagem'));
                                                FechaTelaCadCredGrupo();
                                            }" />
                                        </dxe:ASPxButton>
                                    </div>
                                </dxpc:PopupControlContentControl>
                            </ContentCollection>
                        </dxpc:PopupWindow>

                        <dxpc:PopupWindow Name="Aguarde" Modal="true">
                            <ContentCollection>
                                <dxpc:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                                    <div style="padding-bottom: 10px">
                                        <dxe:ASPxLabel ID="lblMensagemAguarde" ClientInstanceName="lblMensagemAguarde" runat="server" Font-Bold="True" Text="Aguarde...">
                                        </dxe:ASPxLabel>
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
                                        <div style="float:left;width:50%;">
                                            <dxe:ASPxButton ID="btnSim" runat="server" AutoPostBack="False" ClientInstanceName="btnSim"
                                                Text="Sim" Width="80px">
                                                <ClientSideEvents Click="function(s, e) { 
                                                    excuteSimNaoHandler($(window.ppcMensagem).data('fnOk'));
                                                }" />
                                            </dxe:ASPxButton>
                                        </div>
                                        <div style="float:left;width:50%;">
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
                </dxpc:aspxpopupcontrol>
            </div>
        </div>


    </form>

    <script>

        var hidden = document.getElementById("hd").value;
        $("#txtBairro").attr("autocomplete", "none");
        $("#txtBairro").autocomplete("Autocompleter.ashx?r=bairro&bede=" + hidden);
        $("#txtSegmento").attr("autocomplete", "none");
        $("#txtSegmento").autocomplete("Autocompleter.ashx?r=seg&bede=" + hidden);
        $("#txtLocalidade").attr("autocomplete", "none");
        $("#txtLocalidade").autocomplete("Autocompleter.ashx?r=loc&bede=" + hidden);

    </script>

</body>

</html>
