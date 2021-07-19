<%@ Page Title="" Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true"
    CodeFile="ResultConsTransacao.aspx.cs" Inherits="ResultConsTransacao" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ MasterType VirtualPath="~/Cadastro.master" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxGridView" Assembly="DevExpress.Web.ASPxGridView.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Register TagPrefix="dxp" Namespace="DevExpress.Web.ASPxPanel" Assembly="DevExpress.Web.v8.3" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCadastro" runat="Server">

    <script type="text/javascript">
        function memo_OnKeyDown(s, e) {
            if (s.GetText().length >= 200)
                if (e.htmlEvent.keyCode > 47 && e.htmlEvent.keyCode < 90)
                    ASPxClientUtils.PreventEventAndBubble(e.htmlEvent);
            // Ctrl + V
            if (e.htmlEvent.keyCode === 86 && e.htmlEvent.ctrlKey)
                CorrectTextWithDelay();
        }
        function memo_OnInit(s, e) {
            var input = edtJustific.GetInputElement();
            if (ASPxClientUtils.opera)
                input.oncontextmenu = function () { return false; };
            else
                input.onpaste = CorrectTextWithDelay;
        }
        function CorrectTextWithDelay() {
            var maxLength = 200;
            setTimeout(function () { edtJustific.SetText(edtJustific.GetText().substr(0, maxLength)); }, 0);
        }
    </script>

    <%--BOTOES DE ACAO--%>
    <div>
        <div class="BotoesAcaoConsulta" style="padding-right: 8px">
            <dxe:ASPxButton ID="btnVoltarFiltros" runat="server" CssClass="toolButton" AutoPostBack="true"
                ToolTip="Voltar para os filtros" ClientInstanceName="btnVoltarFiltros" Height="33px"
                OnClick="btnVoltarFiltros_Click">
                <Image Url="~/Images/Small/Editar.png" UrlDisabled="~/Images/Small/Editar_Desab.png" />
            </dxe:ASPxButton>
        </div>
        <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnExportar" runat="server" CssClass="toolButton" AutoPostBack="true"
                ToolTip="Exportar para Excel" ClientInstanceName="btnExportar" Height="33px"
                Width="45px" OnClick="btnExportar_Click">
                <Image Url="~/Images/Small/Excel-16.gif" UrlDisabled="~/Images/Small/Excel-16PB.png" />
            </dxe:ASPxButton>
        </div>
        <%--<div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnSair" runat="server" CssClass="toolButton" AutoPostBack="false"
                ToolTip="Sair" OnClick="btnSair_Click" ClientInstanceName="btnSair">
                <Image Url="~/Images/Small/downGreen.png" Width="25px" />
            </dxe:ASPxButton>
        </div>--%>
        <div style="float: left">
            <dxcp:ASPxCallbackPanel ID="clbBotoes" runat="server" ClientInstanceName="clbBotoes"
                EnableViewState="False" EnableCallbackCompression="True" OnCallback="clbBotoes_Callback"
                HideContentOnCallback="False" LoadingPanelText="" ShowLoadingPanel="False" ShowLoadingPanelImage="False">
                <PanelCollection>
                    <dxp:PanelContent ID="PanelContent1" runat="server">
                        <div class="BotoesAcaoConsulta">
                            <dxe:ASPxButton ID="btnConfirmarTransacao" runat="server" CssClass="toolButton" AutoPostBack="false"
                                ToolTip="Confirmar Transacao" Height="33px" ClientVisible="true" ClientEnabled="false" Width="45px"
                                ClientInstanceName="btnConfirmarTransacao">
                                <Image Url="~/Images/Small/93.gif" UrlDisabled="~/Images/Small/93PB.gif" />
                                <ClientSideEvents Click="function(s,e) { JanelaCancConf('ConfJustifica'); }" />
                            </dxe:ASPxButton>
                        </div>
                        <div class="BotoesAcaoConsulta">
                            <dxe:ASPxButton ID="btnAlterarTransacao" runat="server" CssClass="toolButton" AutoPostBack="false"
                                ToolTip="Alterar transacao efetuada via URA" ClientVisible="false" ClientEnabled="false" Height="33px"
                                ClientInstanceName="btnAlterarTransacao" Width="45px">
                                <Image Url="~/Images/Small/63.gif" UrlDisabled="~/Images/Small/63PB.gif" />
                                <ClientSideEvents Click="function(s,e) { AlteraTrans(); }" />
                            </dxe:ASPxButton>
                        </div>
                        <div class="BotoesAcaoConsulta">
                            <dxe:ASPxButton ID="btnCancelarTransacao" runat="server" ClientEnabled="false" CssClass="toolButton"
                                AutoPostBack="false" ToolTip="Cancelar transacao" Height="33px" Width="45px"
                                ClientInstanceName="btnCancelarTransacao">
                                <Image Url="~/Images/Small/No.png" UrlDisabled="~/Images/Small/NoPB.gif" />
                                <ClientSideEvents Click="function(s,e) { JanelaCancConf('CancJustifica'); }" />
                            </dxe:ASPxButton>
                        </div>
                        <div class="BotoesAcaoConsulta">
                            <dxe:ASPxButton ID="btnAltValorTrans" runat="server" ClientEnabled="false" CssClass="toolButton"
                                AutoPostBack="false" ToolTip="Alterar Valor" Height="33px" Width="45px"
                                ClientInstanceName="btnAltValorTrans">
                                <Image Url="~/Images/Small/editar_16.png" UrlDisabled="~/Images/Small/editar_16_desab.png" />
                                <ClientSideEvents Click="function(s,e) { JanelaAltValor(); }" />
                            </dxe:ASPxButton>
                            <asp:HiddenField ID="hdValorAltTrans" runat="server" />
                        </div>
                    </dxp:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
    <div>
        <p>
        </p>
    </div>
    <div style="width: 100%; height: 100%; overflow: scroll;">
        <dxcp:ASPxCallbackPanel ID="clbGrid" runat="server" Width="100%" EnableCallbackCompression="True"
            ClientInstanceName="clbGrid" OnCallback="clbGrid_Callback">
            <PanelCollection>
                <dxp:PanelContent runat="server">
                    <dxwgv:ASPxGridView ID="grvResultado" runat="server" EnableCallbackCompression="True"
                        SettingsBehavior-AllowFocusedRow="True" AutoGenerateColumns="false" Width="100%"
                        ClientInstanceName="grvResultado" OnHtmlRowPrepared="grvResultado_HtmlRowPrepared">
                        <SettingsBehavior AllowFocusedRow="True" />
                        <Styles Cell-Wrap="False">
                            <Cell Wrap="False">
                            </Cell>
                        </Styles>
                        <SettingsPager PageSize="20">
                        </SettingsPager>
                        <Settings ShowFooter="True" />
                        <SettingsText EmptyDataRow="Nenhum dado retornado para esta consulta" />
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="DATTRA" SummaryType="Count" DisplayFormat="Transacoes = {0}" />
                            <dx:ASPxSummaryItem FieldName="VALTRA" SummaryType="Sum" DisplayFormat="{0:c2}" />
                        </TotalSummary>
                        <ClientSideEvents RowDblClick="function(s, e) { InfoJustific(); }"
                            EndCallback="function(s,e){ window.btnVoltarFiltros.Focus(); }"
                            FocusedRowChanged="function(s,e){ SelecaoRowTransacao(); }" />
                    </dxwgv:ASPxGridView>
                    <input id="hdnErroConsulta" type="hidden" runat="server" />
                    <br />
                </dxp:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="function(s, e) {                
                ppcAcaoAux.HideWindow(ppcAcaoAux.GetWindow(0));
                alert(document.getElementById('ctl00_cphCadastro_clbGrid_hdnErroConsulta').value);
}" />
        </dxcp:ASPxCallbackPanel>
    </div>

    <dxpc:ASPxPopupControl ID="ppcAcaoAux" runat="server" ClientInstanceName="ppcAcaoAux"
        Font-Names="tahoma" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        PopupAction="None" CloseAction="CloseButton">
        <ContentCollection>
            <dxpc:PopupControlContentControl runat="server">
            </dxpc:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle Font-Bold="True" />
        <Windows>

            <%-- Justificativa --%>
            <dxpc:PopupWindow HeaderText="Cancelar / Confirmar Transacao" Name="Justifica">
                <HeaderStyle Font-Bold="True" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
                        <dxcp:ASPxCallbackPanel ID="cplConfJustifica" runat="server" Width="100%" ClientInstanceName="cplConfJustifica">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent13" runat="server">
                                    <div style="width: 420px">
                                        <dxe:ASPxLabel ID="lblAcao" runat="server" CssClass="TituloPanel" Text="" ClientInstanceName="lblAcao"
                                            Width="100%">
                                        </dxe:ASPxLabel>
                                        <br />
                                        <br />
                                        <div>
                                            <div style="width: 33%; float: left">Data:</div>
                                            <div style="width: 33%; float: left">N° Autorização:</div>
                                            <div style="width: 33%; float: left">Valor transação:</div>
                                        </div>
                                        <br />
                                        <div>
                                            <div style="width: 33%; float: left">
                                                <dxe:ASPxTextBox ID="edtDatTra" runat="server" EnableViewState="false" Width="97%" ClientInstanceName="edtDatTra"
                                                    MaxLength="16" ReadOnly="true" CssClass="EditDesabilitado">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div style="width: 33%; float: left">
                                                <dxe:ASPxTextBox ID="edtNsuAut" runat="server" EnableViewState="false" Width="97%" ClientInstanceName="edtNsuAut"
                                                    MaxLength="16" ReadOnly="true" CssClass="EditDesabilitado">
                                                </dxe:ASPxTextBox>
                                            </div>
                                            <div style="width: 33%; float: left">
                                                <dxe:ASPxTextBox ID="edtValor" runat="server" EnableViewState="false" Width="97%" ClientInstanceName="edtValor"
                                                    MaxLength="16" ReadOnly="true" CssClass="EditDesabilitado">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                        <div>
                                            <div style="width: 100%; float: left">
                                                Informe uma justificativa:
                                            </div>
                                        </div>
                                        <br />
                                        <div>
                                            <div style="width: 101%; height: 100px; float: left;">
                                                <dxe:ASPxMemo ID="edtJustific" runat="server" EnableViewState="false" Width="97%" Rows="6" ClientInstanceName="edtJustific">
                                                    <ClientSideEvents KeyDown="memo_OnKeyDown" Init="memo_OnInit" />
                                                </dxe:ASPxMemo>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                        <div>
                                            <div style="width: 50%; float: left">
                                                <dxe:ASPxButton ID="btnConfirma" runat="server" Text="Confirma" AutoPostBack="false" Width="97%">
                                                    <ClientSideEvents Click="function(s, e) { ConfirmaAcao(); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                            <div style="width: 50%; float: left">
                                                <dxe:ASPxButton ID="btnNaoConfirma" runat="server" Text="Não Confirma" AutoPostBack="false" Width="97%">
                                                    <ClientSideEvents Click="function(s, e) { ppcAcaoAux.HideWindow(ppcAcaoAux.GetWindow(0)); }" />
                                                </dxe:ASPxButton>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                        <div id="InfoSenha" runat="server" visible="false">
                                            <br />
                                            <dxe:ASPxLabel ID="lblObs" Text="Senha expirada." ForeColor="Red" Font-Bold="true" runat="server" EnableViewState="false">
                                            </dxe:ASPxLabel>
                                        </div>
                                    </div>
                                </dxp:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:PopupWindow>

            <%-- Informação Credenciado --%>
            <dxpc:PopupWindow HeaderText="Justificativa de cancelamento / confirmação de transações" Name="InfoJustific">
                <HeaderStyle Font-Bold="True" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="PopupControlContentControl3" runat="server" Width="300px">
                        <dxcp:ASPxCallbackPanel ID="cplInfoJustific" runat="server" Width="100%" EnableCallbackCompression="True"
                            LoadingPanelText="Carregando&amp;hellip;" EnableViewState="False" ClientInstanceName="cplInfoJustific"
                            OnCallback="cplInfoJustific_Callback">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent2" runat="server">
                                    <div style="font-weight: bold">Justificativa: </div>
                                    <br />
                                    <table width="100%">
                                        <tr style="width: 100%;">
                                            <td>
                                                <dxe:ASPxLabel ID="lblJustific" runat="server" Text="" ClientInstanceName="lblJustific"></dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <dxe:ASPxButton ID="btnOk" runat="server" Text="OK" AutoPostBack="false">
                                                    <ClientSideEvents Click="function(s, e) 
                                                            {   ppcAcaoAux.HideWindow(ppcAcaoAux.GetWindowByName('InfoJustific'));  }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dxp:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="EndCallback" />
                        </dxcp:ASPxCallbackPanel>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:PopupWindow>

        </Windows>
    </dxpc:ASPxPopupControl>
<%--    <div>
        <asp:ObjectDataSource ID="obd" runat="server" OnObjectCreating="CriarInstancia" SelectMethod="GerarConsultaTransacao"
            TypeName="SIL.BLL.blTransacao">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="null" Name="filtros" SessionField="DADOSPARACONSULTA"
                    Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>--%>
</asp:Content>
