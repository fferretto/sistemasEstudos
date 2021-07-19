<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlteraSenha.aspx.cs" Inherits="AlteraSenha" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v8.3" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dxpc" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v8.3" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1 {
            width: 100%;
        }

        .style3 {
            width: 50px;
            height: 19px;
        }

        .style4 {
            height: 19px;
        }

        .style5 {
            height: 28px;
        }
    </style>


    <script type="text/javascript" src="https://code.jquery.com/jquery-3.1.1.min.js">

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <dxpc:ASPxPopupControl ID="ppcAlteraSenha" runat="server" HeaderText="NetCardWeb ::  Alterar Senha"
            Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            ClientInstanceName="ppcLogin" ShowOnPageLoad="True" ShowCloseButton="False" CloseAction="None" Width="344px">
            <ClientSideEvents Init="function(s, e) {edtLoginAS.Focus();}" />
            <ContentCollection>
                <dxpc:PopupControlContentControl runat="server">
                    <table align="center" class="style1">
                        <tr>
                            <td class="style3"></td>
                            <td class="style4">&nbsp;
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <dxe:ASPxImage ID="ASPxImage4" runat="server" ImageUrl="~/Images/Medium/Cadeado.gif">
                                </dxe:ASPxImage>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td style="text-align: right">Login:
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="edtLogin" runat="server" Width="120px" ClientInstanceName="edtLogin">
                                                <ValidationSettings ErrorDisplayMode="Text" ErrorText="*" RequiredField-IsRequired="true">
                                                    <RequiredField IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Senha:
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="edtSenha" runat="server" Width="120px" Password="True">
                                                <ValidationSettings ErrorDisplayMode="Text" ErrorText="*" RequiredField-IsRequired="true">
                                                    <RequiredField IsRequired="True"></RequiredField>
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Nova Senha (Mínimo de 6 e máximo de 8)</td>
                                        <td>
                                            <dxe:ASPxTextBox ID="edtNovaSenha" runat="server" Password="True" Width="120px" MaxLength="8">
                                                <ValidationSettings ErrorDisplayMode="Text" ErrorText="*">
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Confirma Nova Senha</td>
                                        <td>
                                            <dxe:ASPxTextBox ID="edtConfNovaSenha" runat="server" Password="True" Width="120px" MaxLength="8">
                                                <ValidationSettings ErrorDisplayMode="Text" ErrorText="*">
                                                    <RequiredField IsRequired="True" />
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style5" colspan="3" nowrap="nowrap">
                                <dxe:ASPxLabel ID="lblErroAut" runat="server" ForeColor="Maroon">
                                </dxe:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="btnAlterarSenha" Text="Alterar Senha" runat="server" OnClick="SubmitButton_Click"
                                    Height="24px" Width="100px" CssClass="dxbButton"></asp:Button>
                            </td>
                            <td style="text-align: center;">
                                <asp:Button ID="btnSair" Text="Sair" runat="server" OnClick="SubmitButton_Click"
                                    Height="24px" Width="100px" PostBackUrl="~/Logout.aspx" CssClass="dxbButton" CausesValidation="False"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </dxpc:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle Font-Bold="True" />
        </dxpc:ASPxPopupControl>
        <br />
    </form>

    <script type="text/javascript">

        function ExcluirTelaFirefox() {
            var x = document.getElementById("ppcLogin_DXPWMB-1");
            x.style.display = "none";


            var z = document.getElementById("ppcLogin_TCFix-1");
            z.style.backgroundColor = "#777777";

            navigator.sayswho = (function () {
                var ua = navigator.userAgent, tem,
                M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*([\d\.]+)/i) || [];
                if (/trident/i.test(M[1])) {
                    tem = /\brv[ :]+(\d+(\.\d+)?)/g.exec(ua) || [];
                    return 'IE ' + (tem[1] || '');
                }
                M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
                if ((tem = ua.match(/version\/([\.\d]+)/i)) != null) M[2] = tem[1];
                alert( M.join(' '));
            });
        }

        setTimeout('ExcluirTelaFirefox()', 100);

    </script>

    <script type="text/javascript">
        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
            var ieversion = new Number(RegExp.$1)

            alert(ieversion);

            if (ieversion >= 8)
                // Para IE8
                document.getElementsByTagName('html')[0].className += 'ie8';
            else if (ieversion >= 7)
                // Para IE7
                document.getElementsByTagName('html')[0].className += 'ie7';
            else if (ieversion >= 6)
                // Para IE6
                document.getElementsByTagName('html')[0].className += 'ie6';
        }
</script>
</body>
</html>
