<%@ Page Title="" Language="C#" MasterPageFile="~/Relatorio.master" AutoEventWireup="true"
    CodeFile="PreviewModRel.aspx.cs" Inherits="PreviewModRel" %>

<%@ MasterType VirtualPath="~/Relatorio.master" %>
<%@ Register TagPrefix="dxe" Namespace="DevExpress.Web.ASPxEditors" Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register Assembly="DevExpress.XtraReports.v8.3.Web, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraReports.Web" TagPrefix="dxxr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphRelatorio" runat="Server">
    <script type="text/javascript" src="ILL/jquery-1.4.2.min.js"></script>    
    <script type="text/javascript">
        $(document).ready(function () {            
            $(".csFD641621").css({ "padding-right": "35px" });
        });

    </script>

    <div style="width: 100%; margin-top: 20px">
        <div style="width: 75%; float: left; margin-left: auto; margin-right: auto; text-align: center;
            height: 40px;">
            <dxxr:ReportToolbar ID="rtbCredenciadosFechSintetico" runat='server' ShowDefaultButtons='False'
                Width="100%" ReportViewer="<%# rtvModRel %>" Height="36px">
                <SeparatorBackgroundImage HorizontalPosition="center" />
                <Items>
                    <dxxr:ReportToolbarButton Enabled="False" ItemKind="FirstPage" ToolTip="Primeira" />
                    <dxxr:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" ToolTip="Anterior" />
                    <dxxr:ReportToolbarLabel Text="Pag." />
                    <dxxr:ReportToolbarComboBox ItemKind="PageNumber" Width="50px">
                    </dxxr:ReportToolbarComboBox>
                    <dxxr:ReportToolbarLabel Text="de" />
                    <dxxr:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                    <dxxr:ReportToolbarButton ItemKind="NextPage" ToolTip="Proxima" />
                    <dxxr:ReportToolbarButton ItemKind="LastPage" ToolTip="Ultima" />
                    <dxxr:ReportToolbarLabel Text="Formato:" />
                    <dxxr:ReportToolbarComboBox ItemKind="SaveFormat" Width="50px">
                        <Elements>
                            <dxxr:ListElement Text="Pdf" Value="pdf" />
                            <dxxr:ListElement Text="Xls" Value="xls" />
                            <dxxr:ListElement Text="Rtf" Value="rtf" />
                            <dxxr:ListElement Text="Mht" Value="mht" />
                            <dxxr:ListElement Text="Texto" Value="txt" />
                            <dxxr:ListElement Text="Csv" Value="csv" />
                            <dxxr:ListElement Text="Imagem" Value="png" />
                        </Elements>
                    </dxxr:ReportToolbarComboBox>
                    <dxxr:ReportToolbarButton ItemKind="SaveToWindow" ToolTip="Exportar em Nova Janela"
                        Text="Imprimir" />
                    <dxxr:ReportToolbarButton ItemKind="SaveToDisk" ToolTip="Exportar e Salvar em Disco"
                        Text="Salvar" />
                </Items>
            </dxxr:ReportToolbar>
        </div>
        <div style="width: 12%; float: left; padding-left: 5px;">
            <dxe:ASPxButton ID="ASPxButton1" runat="server" Width="98%" Text="Voltar Filtros"
                Height="36px" OnClick="ASPxButton1_Click">
                <Image Url="~/Images/Small/previous.png" />
            </dxe:ASPxButton>
        </div>
        <div style="width: 12%; float: left; padding-left: 3px;">
            <dxe:ASPxButton ID="btnFecharRel" runat="server" Width="98%" Text="Fechar Relatorio"
                Height="36px" OnClick="btnFecharRel_Click">
                <Image Url="~/Images/Small/No.png" />
            </dxe:ASPxButton>
        </div>
    </div>
    <div>
        <br />
        <br />
        <br />
        <br />
    </div>
    <div id="divRel" runat="server" align="center">
        <div>
            <dxxr:ReportViewer ID="rtvModRel" runat="server" Height="100%"
                Width="100%" AutoSize="true" LoadingPanelText="Carregando">
            </dxxr:ReportViewer>
        </div>
    </div>
      <div>
        <br />
        <br />
        <br />
        <br />
    </div>  
    <div style="width: 100%; margin-bottom : 20px ">
        <div style="width: 75%; float: left; margin-left: auto; margin-right: auto; text-align: center;
            height: 40px;">
            <dxxr:ReportToolbar ID="ReportToolbar1" runat='server' ShowDefaultButtons='False'
                Width="100%" ReportViewer="<%# rtvModRel %>" Height="36px">
                <SeparatorBackgroundImage HorizontalPosition="center" />
                <Items>
                    <dxxr:ReportToolbarButton Enabled="False" ItemKind="FirstPage" ToolTip="Primeira" />
                    <dxxr:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" ToolTip="Anterior" />
                    <dxxr:ReportToolbarLabel Text="Pag." />
                    <dxxr:ReportToolbarComboBox ItemKind="PageNumber" Width="50px">
                    </dxxr:ReportToolbarComboBox>
                    <dxxr:ReportToolbarLabel Text="de" />
                    <dxxr:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                    <dxxr:ReportToolbarButton ItemKind="NextPage" ToolTip="Proxima" />
                    <dxxr:ReportToolbarButton ItemKind="LastPage" ToolTip="Ultima" />
                    <dxxr:ReportToolbarLabel Text="Formato:" />
                    <dxxr:ReportToolbarComboBox ItemKind="SaveFormat" Width="50px">
                        <Elements>
                            <dxxr:ListElement Text="Pdf" Value="pdf" />
                            <dxxr:ListElement Text="Xls" Value="xls" />
                            <dxxr:ListElement Text="Rtf" Value="rtf" />
                            <dxxr:ListElement Text="Mht" Value="mht" />
                            <dxxr:ListElement Text="Texto" Value="txt" />
                            <dxxr:ListElement Text="Csv" Value="csv" />
                            <dxxr:ListElement Text="Imagem" Value="png" />
                        </Elements>
                    </dxxr:ReportToolbarComboBox>
                    <dxxr:ReportToolbarButton ItemKind="SaveToWindow" ToolTip="Exportar em Nova Janela"
                        Text="Imprimir" />
                    <dxxr:ReportToolbarButton ItemKind="SaveToDisk" ToolTip="Exportar e Salvar em Disco"
                        Text="Salvar" />
                </Items>
            </dxxr:ReportToolbar>
        </div>
        <div style="width: 12%; float: left; padding-left: 5px;">
            <dxe:ASPxButton ID="ASPxButton2" runat="server" Width="98%" Text="Voltar Filtros"
                Height="36px" OnClick="ASPxButton1_Click">
                <Image Url="~/Images/Small/previous.png" />
            </dxe:ASPxButton>
        </div>
        <div style="width: 12%; float: left; padding-left: 3px;">
            <dxe:ASPxButton ID="ASPxButton3" runat="server" Width="98%" Text="Fechar Relatorio"
                Height="36px" OnClick="btnFecharRel_Click">
                <Image Url="~/Images/Small/No.png" />
            </dxe:ASPxButton>
        </div>
    </div>  <%-- coloquei aqui o teste--%>
</asp:Content>