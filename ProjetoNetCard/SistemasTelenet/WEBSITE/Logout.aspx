<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logout.aspx.cs" Inherits="Logout" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <dxrp:ASPxRoundPanel ID="pnlLogout" runat="server" HeaderText="Encerrando Sessao"
        Width="100%" ClientVisible="True" ClientInstanceName="pnlLogout">
        <HeaderStyle Font-Bold="True" />
        <PanelCollection>
            <dxp:PanelContent runat="server">
                Obrigado por utilizar nossos servicos.<br />
                <br />
                <br />
                <dxe:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl="~/Login.aspx"
                    Text="Se desejar entrar novamente utilize este Link.">
                </dxe:ASPxHyperLink>
            </dxp:PanelContent>
        </PanelCollection>
    </dxrp:ASPxRoundPanel>
    </form>
</body>
</html>
