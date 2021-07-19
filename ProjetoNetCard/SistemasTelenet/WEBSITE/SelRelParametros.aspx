<%@ Page Title="" Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true"
    CodeFile="SelRelParametros.aspx.cs" Inherits="SelRelParametros" %>

<%@ MasterType VirtualPath="~/Cadastro.master" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCadastro" runat="Server">

    <script type="text/javascript" src="ILL/jquery-1.4.2.min.js"></script>
    <script type="text/javascript" src="ILL/jquery.maskedinput-1.3.js"></script>
    <script type="text/javascript" src="ILL/jquery.blockUI.js"></script>
    <script type="text/javascript">function AplicarMaskData() { $('[data-mask-data="true"]').children().children().children().children().mask('99/99/9999'); }</script>


    <script type="text/javascript">        
        $(document).ready(function () {
            console.log($("#ctl00_cphCadastro_msgRetorno").val())
            console.log($("#ctl00_cphCadastro_statusGeracao").val())
            console.log($("#ctl00_cphCadastro_TipoRelatorio").val())
            if ($("#ctl00_cphCadastro_msgRetorno").val() != "") {
                alert($("#ctl00_cphCadastro_msgRetorno").val());
            }
            if ($("#ctl00_cphCadastro_statusGeracao").val() == "EM_ANDAMENTO") {
                $("#btnAcao").hide();
                $("#dvDownloadRel_Tip0").hide();
                $("#dvDownloadRel_Tip1").hide();
                $("#ctl00_cphCadastro_ppc_cplAction_dvEmGeracao").show();
                $("#ctl00_cphCadastro_ppc_cplParametro_grvParametros").find('input, textarea, select').attr('disabled', 'disabled');
                setTimeout("VerificaConclusaoRelatorio()", 5000);
            }
            else if ($("#ctl00_cphCadastro_statusGeracao").val() == "FINALIZADO") {
                $("#btnAcao").hide();
                $("#ctl00_cphCadastro_ppc_cplAction_dvEmGeracao").hide();
                $("#ctl00_cphCadastro_ppc_cplParametro_grvParametros").find('input, textarea, select').attr('disabled', 'disabled');
                if ($("#ctl00_cphCadastro_TipoRelatorio").val() == 0) {
                    $("#dvDownloadRel_Tip0").show();
                }
                else if ($("#ctl00_cphCadastro_TipoRelatorio").val() == 1) {
                    $("#dvDownloadRel_Tip1").show();
                }

            }
        })
function funcRetornoRelatornoViaJob2() {
    alert("entroua qui");
    //window.cplAction.PerformCallback(4);
    //$("#dvDownloadRel").hide();
    //$("#btnAcao").show();
    //$("#ctl00_cphCadastro_dvNovoRelatorio").show();
    //$("#ctl00_cphCadastro_ppc_cplParametro_grvParametros").find('input, textarea, select').removeAttr("disabled");

}
    </script>

    <asp:HiddenField ID="idRelatorio" runat="server" Value="" />
    <asp:HiddenField ID="nmProc" runat="server" Value="" />
    <asp:HiddenField ID="ExecutaViaJob" runat="server" Value="" />
    <asp:HiddenField ID="statusGeracao" runat="server" Value="" />
    <asp:HiddenField ID="TipoRelatorio" runat="server" Value="" />
    <asp:HiddenField ID="PossuiOutroRelatorioSendoGerado" runat="server" Value="" />
    <asp:HiddenField ID="codigoRelatorioSendoGerado" runat="server" Value="" />
    <asp:HiddenField ID="msgRetorno" runat="server" Value="" />
    <asp:HiddenField ID="urlRelatorio" runat="server" Value="" />
    <asp:HiddenField ID="pathArquivo" runat="server" Value="" />


    <dxpc:ASPxPopupControl ID="ppc" runat="server" HeaderText="" Modal="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowOnPageLoad="True" ShowCloseButton="false"
        CloseAction="None" ClientInstanceName="ppc" Width="500px" BackColor="White">
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div id="divParametros" runat="server">
                    <dxcp:ASPxCallbackPanel ID="cplParametro" runat="server" ClientInstanceName="cplParametro"
                        EnableCallbackCompression="True" EnableViewState="False" OnCallback="cplParametro_Callback"
                        Width="100%" LoadingPanelText="Carregando&amp;hellip;">
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent3" runat="server">
                                <asp:GridView ID="grvParametros" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" AutoGenerateColumns="False" DataKeyNames="IDREL,IDPAR" OnRowDataBound="grvParametros_RowDataBound"
                                    Width="100%" ShowHeader="False">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="IDREL" Visible="False" HeaderText="IDREL" InsertVisible="False" ReadOnly="True" SortExpression="IDREL" />
                                        <asp:BoundField DataField="IDPAR" HeaderText="IDPAR" InsertVisible="False" ReadOnly="True" SortExpression="IDPAR" Visible="False" />
                                        <asp:BoundField DataField="DESPAR" HeaderText="DESPAR" SortExpression="DESPAR" />
                                        <asp:BoundField DataField="NOMPAR" HeaderText="NOMPAR" SortExpression="NOMPAR" Visible="False" />
                                        <asp:BoundField DataField="LABEL" HeaderText="LABEL" SortExpression="LABEL" Visible="False" />
                                        <asp:BoundField DataField="TIPO" HeaderText="TIPO" SortExpression="TIPO" Visible="False" />
                                        <asp:BoundField DataField="TAMANHO" HeaderText="TAMANHO" SortExpression="TAMANHO" Visible="False" />
                                        <asp:BoundField DataField="DEFAULT" HeaderText="DEFAULT" SortExpression="DEFAULT" Visible="False" />
                                        <asp:BoundField DataField="REQUERIDO" HeaderText="REQUERIDO" SortExpression="REQUERIDO" Visible="False" />
                                        <asp:BoundField HeaderText="VALOR FILTRO" />
                                        <asp:BoundField DataField="HASDETAIL" HeaderText="HASDETAIL" SortExpression="HASDETAIL" Visible="False" />
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                </asp:GridView>
                            </dxp:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
                <br />
                <dxcp:ASPxCallbackPanel ID="cplAction" runat="server" ClientInstanceName="cplAction"
                    EnableCallbackCompression="True" EnableViewState="False" OnCallback="cplAction_Callback"
                    Width="100%" LoadingPanelText="Carregando&amp;hellip;">
                    <PanelCollection>
                        <dxp:PanelContent ID="PanelContent1" runat="server">
                            <div id="btnAcao">
                                <table>
                                    <tr>
                                        <td>
                                            <dxe:ASPxButton ID="btnVisualizar" runat="server" Font-Bold="False" Text="Visualizar"
                                                ClientInstanceName="btnVisualizar" Style="text-align: center;" CausesValidation="false">
                                                <ClientSideEvents Click="function(s, e){ VisualizaRel(); }" />
                                            </dxe:ASPxButton>
                                        </td>
                                        <td>
                                            <dxe:ASPxButton ID="btnExportarCSV" runat="server" Font-Bold="False" AutoPostBack="False" Text="ExportaCSV"
                                                ClientInstanceName="ExportaCSV" Style="text-align: center;" CausesValidation="false">
                                                <ClientSideEvents Click="function(s, e){ ExportaRel(); }"  />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="dvEmGeracao" style="display: none" runat="server">
                                <div class="divFiltros" style="width: 96%; text-align: center; background-color: gainsboro; font-size: 13px; border-color: green; color: green; font-weight: bold;">
                                    <img src="Images/Medium/processando.gif" alt="" style="width: 30px" />
                                    Seu relatório está sendo gerado. Aguarde a conclusão.
                                </div>
                            </div>

                            <div id="dvDownloadRel_Tip1" style="display: none">
                                <div class="divFiltros" style="width: 96%; text-align: center; background-color: gainsboro; font-size: 13px; border-color: green; color: green; font-weight: bold;">
                                    <b>Relatório concluído.</b> <a onclick="RetornoRelatornoViaJob" target ='_blank'; style="cursor: pointer; color: firebrick;">Clique aqui</a> <b>para realizar o download</b>
                                </div>
                            </div>
                            <div id="dvDownloadRel_Tip0"  style="display: none">
                                <div class="divFiltros" style="width: 96%; text-align: center; background-color: gainsboro; font-size: 13px; border-color: green; color: green; font-weight: bold;">
                                    <b>Relatório concluído.</b>
                                    <%--<a id="idRetRelViaJob" onclick="funcRetornoRelatornoViaJob2" style="cursor: pointer; color: firebrick;">Clique aqui</a>--%>
                                    <dxe:ASPxButton ID="VisualizaRelPDF" runat="server" Font-Bold="False" Text="Clique aqui" CssClass="Label"
                                        Style="text-align: center;" CausesValidation="false" OnClick="RetornoRelatornoViaJob" target ='_blank';>
                                    </dxe:ASPxButton>
                                    <b>para visualizar</b>
                                </div>
                            </div>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
<%--                <table>
                    <tr>
                        <td>
                            <dxe:ASPxButton ID="btnVisualizar2" runat="server" Font-Bold="False" Text="Visualizar"
                                Style="text-align: center;" CausesValidation="false" OnClick="RetornoRelatornoViaJob">
                            </dxe:ASPxButton>
                        </td>
                    </tr>
                </table>--%>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle Font-Bold="True" />
    </dxpc:ASPxPopupControl>
</asp:Content>

