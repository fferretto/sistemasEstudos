<%@ Page Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true" CodeFile="TrocarSenha.aspx.cs" Inherits="TrocarSenha"%>

<%@ MasterType VirtualPath="~/Cadastro.master" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>    

<asp:Content ID="Content1" ContentPlaceHolderID="cphCadastro" runat="Server">
    <dxpc:ASPxPopupControl ID="ppcTrocarSenha" runat="server" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" FooterText="" ClientInstanceName="ppcTrocarSenha"
                            ShowOnPageLoad="True" ShowCloseButton="False" CloseAction="None" HeaderText="Troca de Senha">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="lblSenhaAtual" runat="server" Text="Senha Atual" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxTextBox ID="edtSenhaAtual" ClientInstanceName="edtSenhaAtual" runat="server" Width="200px" MaxLength="15" Font-Bold="true" Password="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="lblNovaSenha" runat="server" Text="Nova Senha (Mínimo de 6 e máximo de 8)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxTextBox ID="edtNovaSenha" ClientInstanceName="edtNovaSenha" runat="server" Width="200px" MaxLength="8" Font-Bold="true" Password="True" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxLabel ID="lblConfNovaSenha" runat="server" Text="Confirma Nova Senha" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dxe:ASPxTextBox ID="edtConfNovaSenha" ClientInstanceName="edtConfNovaSenha" runat="server" Width="200px" MaxLength="8" Font-Bold="true" Password="True" />
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <dxe:ASPxButton ID="btnOK" runat="server" Text="OK" Width="100%" OnClick="btnOK_Click"></dxe:ASPxButton>
                        </td>
                        <td style="width: 50%">
                            <dxe:ASPxButton ID="btnCancelar" runat="server" Text="Voltar" Width="100%" 
                                OnClick="btnCancelar_Click"></dxe:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle Font-Bold="True" />
    </dxpc:ASPxPopupControl>
</asp:Content>