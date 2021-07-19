<%@ Page Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true"
    CodeFile="SelConsTransacao.aspx.cs" Inherits="SelConsTransacao" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxLoadingPanel" TagPrefix="dxlp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ MasterType VirtualPath="~/Cadastro.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphCadastro" runat="Server">
    <dxlp:ASPxLoadingPanel ID="loadingPanel" runat="server" Text="Carregando...Aguarde"
        ClientInstanceName="loadingPanel" HorizontalAlign="Center" Modal="true">
        <LoadingDivStyle Opacity="70">
        </LoadingDivStyle>
    </dxlp:ASPxLoadingPanel>

    <script type="text/javascript">
        function configurarElementosCartao() {
            $('#selecaoTipoCartao').css('display', 'block');
            $('#configuracaoFiltros').css('display', 'block');

            var posPago = $('#ctl00_cphCadastro_cplMain_rplMainTrasacoes_posPago').attr('checked');
            var prePago = $('#ctl00_cphCadastro_cplMain_rplMainTrasacoes_prePago').attr('checked');
            var showFiltro = posPago || prePago;

            $('#configuracaoFiltros').css('display', showFiltro ? 'block' : 'none');

            if (!showFiltro) {
                return;
            }

            var tipoCartao = posPago ? 0 : 1;

            if (tabDadosTrans.activeTabIndex == 1) {
                $('#fechamentoCliente').css('display', tipoCartao == 0 ? 'block' : 'none');
            }
            else if (tabDadosTrans.activeTabIndex == 2) {
                if (window.clbColunas != undefined) {
                    window.clbColunas.PerformCallback();
                }
            }
        }

        $(document).ready(function () {
            if ($('#configuracaoFiltros').css('display') == 'none') {
                $('#ctl00_cphCadastro_cplMain_rplMainTrasacoes_posPago').attr('checked', false);
                $('#ctl00_cphCadastro_cplMain_rplMainTrasacoes_prePago').attr('checked', false);
            };

            $('#ctl00_cphCadastro_cplMain_rplMainTrasacoes_posPago').live('click', function () {
                cplMain.PerformCallback(2);
            });

            $('#ctl00_cphCadastro_cplMain_rplMainTrasacoes_prePago').live('click', function () {
                cplMain.PerformCallback(2);
            });
        });

        loadingPanel.Show();

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(prm_InitializeRequest);
        prm.add_endRequest(prm_EndRequest);

        function prm_InitializeRequest() {
            window.loadingPanel.Show();
        }
        function prm_EndRequest() {
            window.loadingPanel.Hide();
        }

    </script>

    <%--BOTOES DE ACAO--%>
    <div>
        <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnNovaConsulta" runat="server" AutoPostBack="False" ClientInstanceName="btnNovaConsulta"
                CssClass="toolButton" ToolTip="Nova Consulta" EnableViewState="false">
                <Image Url="~/Images/Small/file_edit.png" UrlDisabled="~/Images/Small/Novo_Desab.png" />
                <ClientSideEvents Click="function(s, e) {NovaConsulta(); }"></ClientSideEvents>
            </dxe:ASPxButton>
        </div>
        <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnVoltar" runat="server" CssClass="toolButton" AutoPostBack="false"
                ToolTip="Voltar" ClientEnabled="false" ClientInstanceName="btnVoltar">
                <Image Url="~/Images/Small/Editar.png" UrlDisabled="~/Images/Small/Editar_Desab.png" />
                <ClientSideEvents Click="function(s, e) {  BotaoVoltar();  }" />
            </dxe:ASPxButton>
        </div>
        <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnExcluir" runat="server" CssClass="toolButton" AutoPostBack="false"
                ToolTip="Excluir Consulta" ClientInstanceName="btnExcluir" Height="31px" Width="45px">
                <ClientSideEvents Click="function(s, e) {  Excluir();  }" />
                <Image Url="~/Images/Small/Lixeira.png" />
            </dxe:ASPxButton>
        </div>
        <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnSalvaConsulta" runat="server" CssClass="toolButton" ClientInstanceName="btnSalvaConsulta"
                ToolTip="Salvar Consulta gerada" AutoPostBack="False" ClientEnabled="false" Height="33px"
                Width="45px">
                <Image Url="~/Images/Small/SalvaCons.png" UrlDisabled="~/Images/Small/SalvaConsPB.png" />
                <ClientSideEvents Click="function(s, e) {  SalvaConsulta();  }" />
            </dxe:ASPxButton>
        </div>
        <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnResultado" runat="server" CssClass="toolButton" AutoPostBack="false"
                ToolTip="Gerar pesquisa de Transacoes para os filtros informados" ClientInstanceName="btnResultado"
                ClientEnabled="False" Height="33px" Width="45px"
                OnClick="btnResultado_Click">
                <Image Url="~/Images/Small/42.png" UrlDisabled="~/Images/Small/filtrarPB.png" />
            </dxe:ASPxButton>
        </div>
        <%-- <div class="BotoesAcaoConsulta">
            <dxe:ASPxButton ID="btnSair" runat="server" CssClass="toolButton" AutoPostBack="false"
                ToolTip="Sair" OnClick="btnSair_Click" ClientInstanceName="btnSair">
                <Image Url="~/Images/Small/Cancelar.png" />
            </dxe:ASPxButton>
        </div>--%>
        <div>
            <asp:HiddenField ID="hdAcao" runat="server" />
        </div>
    </div>
    <%--PESQUISA--%>
    <div style="width: 100%; float: right;">
        <dxp:ASPxPanel ID="pnlPesquisaTrans" runat="server" Width="100%" ClientInstanceName="pnlPesquisaTrans"
            DefaultButton="btnPesquisar">
            <PanelCollection>
                <dxp:PanelContent ID="PanelContent2" runat="server">
                    <%--FILTROS PARA PESQUISA--%>
                    <div class="divsPrincipaisConsultaTrans">
                        <div style="float: left; width: 30%">
                            <dxe:ASPxComboBox runat="server" EnableCallbackMode="True" ValueType="System.String"
                                ID="cbxCampoFiltro" SelectedIndex="0" Width="98%" CssClass="ComboBox" Native="true"
                                ClientInstanceName="cbxCampoFiltro">
                                <Items>
                                    <dxe:ListEditItem Text="Nome Consulta" Value="1" />
                                    <dxe:ListEditItem Text="Codigo Consulta" Value="2" />
                                </Items>
                            </dxe:ASPxComboBox>
                        </div>
                        <div style="float: left; width: 55%;">
                            <dxe:ASPxTextBox ID="txtPesquisaTransacao" runat="server" Width="98%" CssClass="TextBox">
                                <ClientSideEvents KeyPress="function(s, e) {
    if (e.htmlEvent.which || e.htmlEvent.keyCode) {
        if ((e.htmlEvent.which == 13) || (e.htmlEvent.keyCode == 13)) {
            btnPesquisar.Focus();
            Pesquisar();
        }
    }
}"></ClientSideEvents>
                            </dxe:ASPxTextBox>
                        </div>
                        <div style="float: left; width: 14%">
                            <dxe:ASPxButton runat="server" AutoPostBack="False" Text="Pesquisar" ID="btnPesquisar"
                                EnableViewState="False" ClientInstanceName="btnPesquisar" Width="100%">
                                <ClientSideEvents Click="function(s, e) {Pesquisar(); }"></ClientSideEvents>
                            </dxe:ASPxButton>
                        </div>
                    </div>
                    <%--GRID PARA PESQUISA--%>
                    <div class="divsPrincipaisConsultaTrans">
                        <dxcp:ASPxCallbackPanel ID="pnlPesquisa" runat="server" Width="100%" ClientInstanceName="pnlPesquisa"
                            EnableCallbackCompression="True" EnableViewState="False" OnCallback="pnlPesquisa_Callback" LoadingPanelText="Pesquisando....">
                            <PanelCollection>
                                <dxp:PanelContent ID="PanelContent0" runat="server">
                                    <dxwgv:ASPxGridView ID="grvListaConsultas" runat="server" AutoGenerateColumns="False"
                                        Width="100%" KeyFieldName="CODCONS" EnableCallbackCompression="True" OnBeforeColumnSortingGrouping="grvListaConsultas_BeforeColumnSortingGrouping"
                                        OnPageIndexChanged="grvListaConsultas_PageIndexChanged" ClientInstanceName="grvListaConsultas"
                                        EnableViewState="False">
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn Caption="Codigo" FieldName="CODCONS" ReadOnly="True"
                                                VisibleIndex="0" Width="15%">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_CONSULTA" ReadOnly="True"
                                                VisibleIndex="1" Width="55%">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Tipo da Consulta" FieldName="DESCRICAO" ReadOnly="True"
                                                VisibleIndex="2" Width="15%">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="Tipo de Cartão" FieldName="TIPOCARTAO" ReadOnly="True"
                                                VisibleIndex="3" Width="15%">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AllowFocusedRow="True" />
                                        <SettingsPager PageSize="10">
                                            <AllButton Text="All">
                                            </AllButton>
                                            <NextPageButton Text="Next &gt;">
                                            </NextPageButton>
                                            <PrevPageButton Text="&lt; Prev">
                                            </PrevPageButton>
                                        </SettingsPager>
                                        <SettingsText EmptyDataRow="Nenhum registro encontrado" />
                                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                                        <SettingsPager>
                                            <AllButton Text="All">
                                            </AllButton>
                                            <NextPageButton Text="Next &gt;">
                                            </NextPageButton>
                                            <PrevPageButton Text="&lt; Prev">
                                            </PrevPageButton>
                                        </SettingsPager>
                                        <ClientSideEvents EndCallback="function(s, e) {FocusGrid();}" RowDblClick="function(s, e) {DoubleClick();}"
                                            CallbackError="function(s,e){MensagemErroGeral();}" FocusedRowChanged="function(s,e) { SelecaoConsultaVA();}" />
                                    </dxwgv:ASPxGridView>
                                    <asp:HiddenField ID="hdGerouPesquisa" runat="server" />
                                </dxp:PanelContent>
                            </PanelCollection>
                            <ClientSideEvents EndCallback="function (s,e) {MensagemOKGeral();}" />
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </dxp:PanelContent>
            </PanelCollection>
        </dxp:ASPxPanel>
    </div>
    <%-- MAIN --%>
    <div style="width: 100%; float: right;">
        <dxcp:ASPxCallbackPanel ID="cplMain" runat="server" Width="100%" ClientInstanceName="cplMain"
            OnCallback="cplMain_Callback" LoadingPanelText="Carregando...">
            <ClientSideEvents EndCallback="function (s, e) { configurarElementosCartao(); }" />
            <PanelCollection>
                <dxp:PanelContent ID="PanelContent1" runat="server">
                    <dxrp:ASPxRoundPanel ID="rplMainTrasacoes" runat="server" Width="100%" ClientInstanceName="rplMainTrasacoes"
                        ShowHeader="False">
                        <PanelCollection>
                            <dxp:PanelContent ID="PanelContent3" runat="server">
                                <div id="selecaoTipoCartao" style="display: none;">
                                    <table width="100%" class="tableFiltros" style="margin-bottom: 8px;">
                                        <tr class="linha">
                                            <td>
                                                <asp:RadioButton ID="posPago" runat="server" GroupName="tipoCartao" Font-Bold="true" Font-Size="12pt" Text="Pós Pago" EnableViewState="false" Style="margin-right: 10px;"></asp:RadioButton>
                                                <asp:RadioButton ID="prePago" runat="server" GroupName="tipoCartao" Font-Bold="true" Font-Size="12pt" Text="Pré Pago" EnableViewState="false"></asp:RadioButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="configuracaoFiltros">
                                    <dxtc:ASPxPageControl ID="tabDadosTrans" runat="server" ActiveTabIndex="0" ClientInstanceName="tabDadosTrans"
                                        Width="100%" OnActiveTabChanged="tabDadosTrans_ActiveTabChanged" EnableCallBacks="True"
                                        EnableViewState="False" EnableCallbackCompression="True"
                                        LoadingPanelText="Aguarde...">
                                        <ClientSideEvents ActiveTabChanged="function (s, e) { configurarElementosCartao(); }" />
                                        <TabPages>
                                            <dxtc:TabPage Text="Filtros Gerais" Name="Dados para Consulta">
                                                <ContentCollection>
                                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                                        <div class="DivAbas">
                                                            <table width="100%" cellspacing="10">
                                                                <tr>
                                                                    <td width="50%">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbPeriodo" runat="server" Font-Bold="true" Font-Size="12pt" Text="Periodo"
                                                                                        EnableViewState="false"></asp:CheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td width="33%">
                                                                                                <dxe:ASPxTextBox ID="txtPeriodoIni" runat="server" Width="100%" ClientInstanceName="txtPeriodoIni"
                                                                                                    ClientEnabled="False" EnableViewState="false" MaxLength="10">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td width="7%">
                                                                                                <dxe:ASPxTextBox ID="txtPeriodoHoraIni" runat="server" Width="100%" ClientInstanceName="txtPeriodoHoraIni"
                                                                                                    ClientEnabled="False" MaxLength="2">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td width="1%">:
                                                                                            </td>
                                                                                            <td width="7%">
                                                                                                <dxe:ASPxTextBox ID="txtPeriodoMinIni" runat="server" Width="100%" ClientInstanceName="txtPeriodoMinIni"
                                                                                                    ClientEnabled="False" MaxLength="2">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td width="2%" style="text-align: center;">a
                                                                                            </td>
                                                                                            <td width="33%">
                                                                                                <dxe:ASPxTextBox ID="txtPeriodoFim" runat="server" Width="100%" ClientInstanceName="txtPeriodoFim"
                                                                                                    ClientEnabled="False" MaxLength="10">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td width="7%">
                                                                                                <dxe:ASPxTextBox ID="txtPeriodoHoraFim" runat="server" Width="100%" ClientInstanceName="txtPeriodoHoraFim"
                                                                                                    ClientEnabled="False" MaxLength="2">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td width="1%">:
                                                                                            </td>
                                                                                            <td width="7%">
                                                                                                <dxe:ASPxTextBox ID="txtPeriodoMinFim" runat="server" Width="100%" ClientInstanceName="txtPeriodoMinFim"
                                                                                                    ClientEnabled="False" MaxLength="2">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbNumHost" runat="server" Font-Bold="true" Font-Size="12pt" Text="Nº Host:" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td width="48%">
                                                                                                <dxe:ASPxTextBox ID="txtHostIni" runat="server" Width="100%" ClientInstanceName="txtHostIni"
                                                                                                    ClientEnabled="False" MaxLength="8">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td style="text-align: center;">a
                                                                                            </td>
                                                                                            <td width="48%">
                                                                                                <dxe:ASPxTextBox ID="txtHostFim" runat="server" Width="100%" ClientInstanceName="txtHostFim"
                                                                                                    ClientEnabled="False" MaxLength="8">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbNumAutorizacao" runat="server" Font-Bold="true" Font-Size="12pt"
                                                                                        Text="Nº Autorizacao:" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td width="48%">
                                                                                                <dxe:ASPxTextBox ID="txtAutIni" runat="server" Width="100%" ClientEnabled="false"
                                                                                                    ClientInstanceName="txtAutIni" MaxLength="8">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                            <td style="text-align: center;">a
                                                                                            </td>
                                                                                            <td width="48%">
                                                                                                <dxe:ASPxTextBox ID="txtAutFim" runat="server" Width="100%" ClientEnabled="false"
                                                                                                    ClientInstanceName="txtAutFim" MaxLength="8">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbTipoUsuario" runat="server" Font-Bold="true" Font-Size="12pt"
                                                                                        Text="Usuario:" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxRadioButton ID="rdbUsuarioNome" runat="server" Text="Nome" ClientInstanceName="rdbUsuarioNome"
                                                                                                    ClientEnabled="false" GroupName="gnUsuario">
                                                                                                </dxe:ASPxRadioButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxTextBox ID="txtUsuarioNome" runat="server" Width="100%" ClientInstanceName="txtUsuarioNome"
                                                                                                    ClientEnabled="false">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxRadioButton ID="rdbUsuarioCPF" runat="server" Text="CPF" ClientInstanceName="rdbUsuarioCPF"
                                                                                                    ClientEnabled="false" GroupName="gnUsuario">
                                                                                                </dxe:ASPxRadioButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxTextBox ID="txtUsuarioCPF" runat="server" Width="100%" ClientInstanceName="txtUsuarioCPF"
                                                                                                    ClientEnabled="false">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxRadioButton ID="rdbUsuarioMatricula" runat="server" Text="Matricula" ClientInstanceName="rdbUsuarioMatricula"
                                                                                                    ClientEnabled="false" GroupName="gnUsuario">
                                                                                                </dxe:ASPxRadioButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxTextBox ID="txtUsuarioMatricula" runat="server" Width="100%" ClientInstanceName="txtUsuarioMatricula"
                                                                                                    ClientEnabled="false">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <div style="padding-left: 10px; width: 100%;">
                                                                <div style="float: left; width: 49%; border: 1px solid LightGray;">
                                                                    <div id="divCartao" runat="server">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr>
                                                                                <td class="linha">
                                                                                    <asp:CheckBox ID="chbCartao" runat="server" Font-Bold="true" Font-Size="12pt"
                                                                                        Text="Cartao:" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <dxe:ASPxTextBox ID="txtCartao" runat="server" Width="100%" ClientInstanceName="txtCartao"
                                                                                        ClientEnabled="false">
                                                                                    </dxe:ASPxTextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <div style="width: 49%; height: 56px">
                                                                </div>
                                                            </div>
                                                            <div style="padding-left: 10px; width: 100%;">
                                                                <div id="divFiltroSubrede" runat="server" style="float: left; width: 49%; border: 1px solid LightGray;">
                                                                    <asp:UpdatePanel ID="upd0" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr class="linha">
                                                                                    <td>
                                                                                        <dxe:ASPxCheckBox ID="chbSubRede" runat="server" Font-Bold="True" Text="Subrede"
                                                                                            Font-Size="12pt" AutoPostBack="true" ClientInstanceName="chbSubRede" OnCheckedChanged="chbSubRede_CheckedChanged">
                                                                                            <ClientSideEvents CheckedChanged="function (s,e) { Checked(); }" />
                                                                                        </dxe:ASPxCheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div class="divScrollTrans">
                                                                                            <dxe:ASPxCheckBox ID="chbTodasSubRedes" runat="server" Font-Bold="True" Text="Selecionar todas"
                                                                                                Font-Size="10pt" AutoPostBack="true" ClientInstanceName="chbTodasSubRedes" OnCheckedChanged="chbSubRede_CheckedChanged">
                                                                                                <ClientSideEvents CheckedChanged="function (s,e) { CheckedTodasSubRedes(); }" />
                                                                                            </dxe:ASPxCheckBox>
                                                                                            <dxwgv:ASPxGridView ID="chbListaSubRede" ClientInstanceName="chbListaSubRede"
                                                                                                runat="server" AutoGeneratheColumns="False" Width="95%" SettingsBehavior-ColumnResizeMode="Control"
                                                                                                Settings-ShowColumnHeaders="False" Settings-GridLines="None" EnableViewState="False"
                                                                                                KeyFieldName="CODSUBREDE" AutoGenerateColumns="False" OnDataBinding="chbListaSubRede_DataBinding">
                                                                                                <Border BorderColor="White" BorderStyle="None" />
                                                                                                <SettingsBehavior ColumnResizeMode="Control" />
                                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                                    <Summary Text="({2} items)" />
                                                                                                </SettingsPager>
                                                                                                <SettingsText EmptyDataRow="Marque a caixa Subrede para listar as opcoes" />
                                                                                                <Columns>
                                                                                                    <dxwgv:GridViewCommandColumn Name="Checkbox" ShowSelectCheckbox="true" Width="7%" VisibleIndex="0">
                                                                                                    </dxwgv:GridViewCommandColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="CODSUBREDE" Width="15%" VisibleIndex="1" Visible="false">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="NOMSUBREDE" Width="65%" VisibleIndex="2">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                </Columns>
                                                                                                <Settings GridLines="None" ShowColumnHeaders="False" />
                                                                                            </dxwgv:ASPxGridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="chbSubRede" EventName="CheckedChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div id="divRedeCaptura" runat="server" style="float: left; width: 49%; border: 1px solid LightGray;">
                                                                    <asp:UpdatePanel ID="upd10" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr class="linha">
                                                                                    <td>
                                                                                        <dxe:ASPxCheckBox ID="chbRedeCaptura" runat="server" Font-Bold="True" Text="Rede de Captura"
                                                                                            Font-Size="12pt" AutoPostBack="true" ClientInstanceName="chbRedeCaptura" OnCheckedChanged="chbRedeCaptura_CheckedChanged">
                                                                                            <ClientSideEvents CheckedChanged="function (s,e) { Checked(); }" />
                                                                                        </dxe:ASPxCheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div class="divScrollTrans">
                                                                                            <dxe:ASPxCheckBox ID="chbTodasRedeCaptura" runat="server" Font-Bold="True" Text="Selecionar todas"
                                                                                                Font-Size="10pt" AutoPostBack="true" ClientInstanceName="chbTodasRedeCaptura" OnCheckedChanged="chbRedeCaptura_CheckedChanged">
                                                                                                <ClientSideEvents CheckedChanged="function (s,e) { CheckedTodasRedeCaptura(); }" />
                                                                                            </dxe:ASPxCheckBox>
                                                                                            <dxwgv:ASPxGridView ID="chbListaRedeCaptura" ClientInstanceName="chbListaRedeCaptura"
                                                                                                runat="server" AutoGeneratheColumns="False" Width="95%" SettingsBehavior-ColumnResizeMode="Control"
                                                                                                Settings-ShowColumnHeaders="False" Settings-GridLines="None" EnableViewState="False"
                                                                                                KeyFieldName="REDE" AutoGenerateColumns="False" OnDataBinding="chbListaRedeCaptura_DataBinding">
                                                                                                <Border BorderColor="White" BorderStyle="None" />
                                                                                                <SettingsBehavior ColumnResizeMode="Control" />
                                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                                    <Summary Text="({2} items)" />
                                                                                                </SettingsPager>
                                                                                                <SettingsText EmptyDataRow="Marque a caixa Rede de Captura para listar as opcoes" />
                                                                                                <Columns>
                                                                                                    <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" Width="7%" VisibleIndex="0">
                                                                                                    </dxwgv:GridViewCommandColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="REDE" Width="15%" VisibleIndex="1" Visible="false">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="NOME" Width="65%" VisibleIndex="2">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                </Columns>
                                                                                                <Settings GridLines="None" ShowColumnHeaders="False" />
                                                                                            </dxwgv:ASPxGridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="chbSubRede" EventName="CheckedChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                            <div style="padding-left: 10px; width: 100%;">
                                                                <div style="float: left; width: 49%; border: 1px solid LightGray;">
                                                                    <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr class="linha">
                                                                                    <td>
                                                                                        <dxe:ASPxCheckBox ID="chbTipoTrans" runat="server" Font-Bold="True" Text="Tipo Transacao"
                                                                                            Font-Size="12pt" AutoPostBack="true" ClientInstanceName="chbTipoTrans" OnCheckedChanged="chbTipoTrans_CheckedChanged">
                                                                                            <ClientSideEvents CheckedChanged="function (s,e) { Checked(); }" />
                                                                                        </dxe:ASPxCheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div class="divScrollTrans">
                                                                                            <dxe:ASPxCheckBox ID="chbTodasTrans" runat="server" Font-Bold="True" Text="Selecionar todas"
                                                                                                Font-Size="10pt" AutoPostBack="true" ClientInstanceName="chbTodasTrans" OnCheckedChanged="chbTipoTrans_CheckedChanged">
                                                                                                <ClientSideEvents CheckedChanged="function (s,e) { CheckedTodasTrans(); }" />
                                                                                            </dxe:ASPxCheckBox>
                                                                                            <dxwgv:ASPxGridView ID="chbListaTipoTrans" ClientInstanceName="chbListaTipoTrans"
                                                                                                runat="server" AutoGeneratheColumns="False" Width="95%" SettingsBehavior-ColumnResizeMode="Control"
                                                                                                Settings-ShowColumnHeaders="False" Settings-GridLines="None" EnableViewState="False"
                                                                                                KeyFieldName="TIPTRA" AutoGenerateColumns="False" OnDataBinding="chbListaTipoTrans_DataBinding">
                                                                                                <Border BorderColor="White" BorderStyle="None" />
                                                                                                <SettingsBehavior ColumnResizeMode="Control" />
                                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                                    <Summary Text="({2} items)" />
                                                                                                </SettingsPager>
                                                                                                <SettingsText EmptyDataRow="Marque a caixa Tipo de Transacoes para listar as opcoes" />
                                                                                                <Columns>
                                                                                                    <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" Width="7%" VisibleIndex="0">
                                                                                                    </dxwgv:GridViewCommandColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="TIPTRA" Width="15%" VisibleIndex="1">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="DESTIPTRA" Width="65%" VisibleIndex="2">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                </Columns>
                                                                                                <Settings GridLines="None" ShowColumnHeaders="false" />
                                                                                            </dxwgv:ASPxGridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="chbTipoTrans" EventName="CheckedChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <div style="float: left; width: 49%; border: 1px solid LightGray;">
                                                                    <asp:UpdatePanel ID="upd2" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table width="100%">
                                                                                <tr class="linha">
                                                                                    <td>
                                                                                        <dxe:ASPxCheckBox ID="chbTipoResposta" runat="server" Font-Bold="True" Font-Size="12pt"
                                                                                            Text="Tipo Resposta" ClientInstanceName="chbTipoResposta" AutoPostBack="true"
                                                                                            OnCheckedChanged="chbTipoResposta_CheckedChanged">
                                                                                            <ClientSideEvents CheckedChanged="function (s,e) { Checked(); }" />
                                                                                        </dxe:ASPxCheckBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div class="divScrollTrans">
                                                                                            <dxe:ASPxCheckBox ID="chbTodosTipResp" runat="server" Font-Bold="True" Text="Selecionar todas"
                                                                                                Font-Size="10pt" AutoPostBack="true" ClientInstanceName="chbTodosTipResp" OnCheckedChanged="chbTipoResposta_CheckedChanged">
                                                                                                <ClientSideEvents CheckedChanged="function (s,e) { CheckedTodasTipResp(); }" />
                                                                                            </dxe:ASPxCheckBox>
                                                                                            <dxwgv:ASPxGridView ID="chbListTipoResp" ClientInstanceName="chbListTipoResp" runat="server"
                                                                                                AutoGeneratheColumns="False" Width="95%" SettingsBehavior-ColumnResizeMode="Control"
                                                                                                Settings-ShowColumnHeaders="False" Settings-GridLines="None" EnableViewState="False"
                                                                                                KeyFieldName="VALUE" AutoGenerateColumns="False" OnDataBinding="chbListTipoResp_DataBinding">
                                                                                                <Border BorderColor="White" BorderStyle="None" />
                                                                                                <SettingsBehavior ColumnResizeMode="Control" />
                                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                                    <Summary Text="({2} items)" />
                                                                                                </SettingsPager>
                                                                                                <SettingsText EmptyDataRow="Marque a caixa Tipo de Resposta para listar as opcoes" />
                                                                                                <Columns>
                                                                                                    <dxwgv:GridViewCommandColumn ShowSelectCheckbox="true" Width="7%" VisibleIndex="0">
                                                                                                    </dxwgv:GridViewCommandColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="VALUE" Width="15%" VisibleIndex="1" Visible="false">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn FieldName="DESCR" Width="65%" VisibleIndex="2">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                </Columns>
                                                                                                <Settings GridLines="None" ShowColumnHeaders="False" />
                                                                                            </dxwgv:ASPxGridView>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="chbTipoResposta" EventName="CheckedChanged" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </dxw:ContentControl>
                                                </ContentCollection>
                                            </dxtc:TabPage>
                                            <dxtc:TabPage Name="Clientes e Credenciados" Text="Filtros para Clientes e Credenciados">
                                                <ContentCollection>
                                                    <dxw:ContentControl ID="ContentControl2" runat="server">
                                                        <div class="divFiltroFechamentoEIntervalos">
                                                            <%--FILTRO FECHAMENTOS--%>
                                                            <table width="98%" class="tableFiltros" style="margin-left: 11px;">
                                                                <tr style="padding-top: 5px">
                                                                    <td class="linha">
                                                                        <asp:CheckBox ID="chbTipoFechCred" runat="server" Font-Bold="true" Font-Size="12pt"
                                                                            Text="Fechamento Credenciado:" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div style="width: 100%;">
                                                                            <div style="width: 14%; float: left; text-align: right; padding-right: 5px">
                                                                                <dxe:ASPxRadioButton ID="rdbEspecicarNum" runat="server" Text="Especificar nº:" ClientInstanceName="rdbEspecicarNum"
                                                                                    ClientEnabled="false" TextAlign="Right" GroupName="gnFechamentoCred">
                                                                                </dxe:ASPxRadioButton>
                                                                            </div>
                                                                            <div style="width: 14%; float: left">
                                                                                <dxe:ASPxTextBox ID="txtEspecNumIni" runat="server" Width="100%" ClientInstanceName="txtEspecNumIni"
                                                                                    ClientEnabled="false" MaxLength="3">
                                                                                </dxe:ASPxTextBox>
                                                                            </div>
                                                                            <div style="width: 5%; float: left; text-align: center">
                                                                                a
                                                                            </div>
                                                                            <div style="width: 15%; float: left">
                                                                                <dxe:ASPxTextBox ID="txtEspecNumFim" runat="server" Width="92%" ClientInstanceName="txtEspecNumFim"
                                                                                    ClientEnabled="false" MaxLength="3">
                                                                                </dxe:ASPxTextBox>
                                                                            </div>
                                                                            <div style="width: 15%; float: left; text-align: right; padding-right: 5px">
                                                                                <dxe:ASPxRadioButton ID="rdbEspecicarData" runat="server" Text="Especificar data:"
                                                                                    ClientInstanceName="rdbEspecicarData" ClientEnabled="false"
                                                                                    TextAlign="Right" GroupName="gnFechamentoCred">
                                                                                </dxe:ASPxRadioButton>
                                                                            </div>
                                                                            <div style="width: 15%; float: left">
                                                                                <dxe:ASPxTextBox ID="txtEspecDataIni" runat="server" Width="98%" ClientEnabled="false"
                                                                                    ClientInstanceName="txtEspecDataIni" MaxLength="10">
                                                                                </dxe:ASPxTextBox>
                                                                            </div>
                                                                            <div style="width: 5%; float: left; text-align: center">
                                                                                a
                                                                            </div>
                                                                            <div style="width: 15%; float: left">
                                                                                <dxe:ASPxTextBox ID="txtEspecDataFim" runat="server" Width="95%" ClientInstanceName="txtEspecDataFim"
                                                                                    ClientEnabled="false" MaxLength="10">
                                                                                </dxe:ASPxTextBox>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                            <div id="fechamentoCliente">
                                                                <table width="98%" class="tableFiltros" style="margin-left: 11px; margin-bottom: 10px; padding-bottom: 5px; margin-top: 20px;">
                                                                    <tr style="padding-top: 5px">
                                                                        <td class="linha">
                                                                            <asp:CheckBox ID="chbTipoFechCliente" runat="server" Font-Bold="true" Font-Size="12pt" Text="Fechamento Cliente:" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <div style="width: 100%;">
                                                                                <div style="width: 14%; float: left; text-align: right; padding-right: 5px">
                                                                                    <dxe:ASPxRadioButton ID="rdbEspecicarNumCliente" runat="server" Text="Especificar nº:" ClientInstanceName="rdbEspecicarNumCliente"
                                                                                        ClientEnabled="false" TextAlign="Right" GroupName="gnFechamentoCliente">
                                                                                    </dxe:ASPxRadioButton>
                                                                                </div>
                                                                                <div style="width: 14%; float: left">
                                                                                    <dxe:ASPxTextBox ID="txtEspecNumIniCliente" runat="server" Width="100%" ClientInstanceName="txtEspecNumIniCliente"
                                                                                        ClientEnabled="false" MaxLength="3">
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>
                                                                                <div style="width: 5%; float: left; text-align: center">
                                                                                    a
                                                                                </div>
                                                                                <div style="width: 15%; float: left">
                                                                                    <dxe:ASPxTextBox ID="txtEspecNumFimCliente" runat="server" Width="92%" ClientInstanceName="txtEspecNumFimCliente"
                                                                                        ClientEnabled="false" MaxLength="3">
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>
                                                                                <div style="width: 15%; float: left; text-align: right; padding-right: 5px">
                                                                                    <dxe:ASPxRadioButton ID="rdbEspecicarDataCliente" runat="server" Text="Especificar data:"
                                                                                        ClientInstanceName="rdbEspecicarDataCliente" ClientEnabled="false" TextAlign="Right" GroupName="gnFechamentoCliente">
                                                                                    </dxe:ASPxRadioButton>
                                                                                </div>
                                                                                <div style="width: 15%; float: left">
                                                                                    <dxe:ASPxTextBox ID="txtEspecDataIniCliente" runat="server" Width="98%" ClientEnabled="false"
                                                                                        ClientInstanceName="txtEspecDataIniCliente" MaxLength="10">
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>
                                                                                <div style="width: 5%; float: left; text-align: center">
                                                                                    a
                                                                                </div>
                                                                                <div style="width: 15%; float: left">
                                                                                    <dxe:ASPxTextBox ID="txtEspecDataFimCliente" runat="server" Width="95%" ClientInstanceName="txtEspecDataFimCliente"
                                                                                        ClientEnabled="false" MaxLength="10">
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div class="divFiltroFechamentoEIntervalos">
                                                            <%--FILTRO INTERVALO CLIENTES--%>
                                                            <table width="100%" cellspacing="10">
                                                                <tr>
                                                                    <td width="33%">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbIntervaloCli" runat="server" Text="Intervalo de Clientes:" Font-Bold="True"
                                                                                        Font-Size="12pt"></asp:CheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="divDivisao">
                                                                                        <div style="width: 29%; float: left; padding-right: 7px; text-align: right">
                                                                                            Cod.Cliente:
                                                                                        </div>
                                                                                        <div style="width: 30%; float: left">
                                                                                            <dxe:ASPxTextBox ID="txtInterCliIni" runat="server" Width="95%" ClientEnabled="False"
                                                                                                ClientInstanceName="txtInterCliIni">
                                                                                            </dxe:ASPxTextBox>
                                                                                        </div>
                                                                                        <div style="width: 9%; float: left; text-align: center">
                                                                                            a
                                                                                        </div>
                                                                                        <div style="width: 28%; float: left">
                                                                                            <dxe:ASPxTextBox ID="txtInterCliFim" runat="server" Width="98%" ClientEnabled="False"
                                                                                                ClientInstanceName="txtInterCliFim">
                                                                                            </dxe:ASPxTextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="33%">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbIntervaloCentralizadora" runat="server" Text="Fechamento Centralizadora:" Font-Bold="True"
                                                                                        Font-Size="12pt"></asp:CheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="divDivisao">
                                                                                        <div style="width: 29%; float: left; padding-right: 7px; text-align: right">
                                                                                            Cod.Credenciado:
                                                                                        </div>
                                                                                        <div style="width: 30%; float: left">
                                                                                            <dxe:ASPxTextBox ID="txtInterCentralizadora" runat="server" Width="95%" ClientEnabled="False"
                                                                                                ClientInstanceName="txtInterCentralizadora">
                                                                                            </dxe:ASPxTextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td width="33%">
                                                                        <table width="100%" class="tableFiltros">
                                                                            <tr class="linha">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chbIntervaloCre" runat="server" Text="Intervalo de Credenciados:"
                                                                                        Font-Bold="True" Font-Size="12pt"></asp:CheckBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div class="divDivisao">
                                                                                        <div style="width: 26%; float: left; padding-right: 5px; text-align: right">
                                                                                            Cod.Credenciado:
                                                                                        </div>
                                                                                        <div style="width: 30%; float: left">
                                                                                            <dxe:ASPxTextBox ID="txtInterCredIni" runat="server" Width="98%" ClientInstanceName="txtInterCredIni"
                                                                                                ClientEnabled="false">
                                                                                            </dxe:ASPxTextBox>
                                                                                        </div>
                                                                                        <div style="width: 11%; float: left; text-align: center">
                                                                                            a
                                                                                        </div>
                                                                                        <div style="width: 29%; float: left">
                                                                                            <dxe:ASPxTextBox ID="txtInterCredFim" runat="server" Width="98%" ClientInstanceName="txtInterCredFim"
                                                                                                ClientEnabled="false">
                                                                                            </dxe:ASPxTextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="height: 360px">
                                                            <table width="100%" cellspacing="10">
                                                                <tr>
                                                                    <%--FILTRO LISTA CLIENTES--%>
                                                                    <td width="50%" class="tableFiltros">
                                                                        <div class="divClientesConsulta">
                                                                            <div class="styleCheckBoxFitros">
                                                                                <dxe:ASPxCheckBox ID="chbTipoCliente" runat="server" Text="Lista Clientes:" Font-Bold="True"
                                                                                    Font-Size="12pt" AutoPostBack="false" ClientInstanceName="chbTipoCliente">
                                                                                    <ClientSideEvents CheckedChanged="function(s, e) { habilitaListaCliente(); }" />
                                                                                </dxe:ASPxCheckBox>
                                                                            </div>
                                                                            <div style="width: 100%; margin-bottom: 10px; margin-top: 5px; padding-left: 5px; font-weight: bold">
                                                                                Informe o Cliente pelo codigo:
                                                                            </div>
                                                                            <div class="divsPrincipaisConsultaTrans divFiltrosListaCliCred">
                                                                                <div style="width: 8%; text-align: right; float: left; padding-right: 5px;">
                                                                                    Cod.:
                                                                                </div>
                                                                                <div style="width: 17%; text-align: right; float: left">
                                                                                    <dxe:ASPxTextBox ID="txtCodCliFiltro" runat="server" Width="100%" CssClass="TextBox"
                                                                                        MaxLength="6" ClientInstanceName="txtCodCliFiltro">
                                                                                        <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                        </ReadOnlyStyle>
                                                                                        <ClientSideEvents LostFocus="function(s, e) { clbNomeCli.PerformCallback(); }" />
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>
                                                                                <div style="width: 15%; text-align: right; float: left; padding-right: 5px;">
                                                                                    Nome:
                                                                                </div>
                                                                                <div style="width: 49%; text-align: right; float: left">
                                                                                    <dxcp:ASPxCallbackPanel ID="clbNomeCli" runat="server" Width="100%" ClientInstanceName="clbNomeCli"
                                                                                        OnCallback="clbNomeCli_Callback">
                                                                                        <PanelCollection>
                                                                                            <dxp:PanelContent ID="PanelContent4" runat="server">
                                                                                                <dxe:ASPxTextBox ID="txtNomeClienteFiltro" runat="server" Width="100%" ReadOnly="true"
                                                                                                    ClientInstanceName="txtNomeClienteFiltro">
                                                                                                    <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                                    </ReadOnlyStyle>
                                                                                                </dxe:ASPxTextBox>
                                                                                            </dxp:PanelContent>
                                                                                        </PanelCollection>
                                                                                    </dxcp:ASPxCallbackPanel>
                                                                                </div>
                                                                                <div style="width: 3%; text-align: center; float: left; padding-left: 3px">
                                                                                    <dxe:ASPxButton ID="btnIncluirCliente" runat="server" AutoPostBack="False" Height="15px"
                                                                                        Width="100%">
                                                                                        <ClientSideEvents Click="function(s, e) { 
                                                                                                clbGridClientes.PerformCallback('Incluir');
                                                                                                txtNomeClienteFiltro.SetText('');
                                                                                                txtCodCliFiltro.SetText(''); }" />
                                                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="Images/Small/Fall.gif" Repeat="NoRepeat"
                                                                                            VerticalPosition="center" />
                                                                                    </dxe:ASPxButton>
                                                                                </div>
                                                                            </div>
                                                                            <div class="divsPrincipaisConsultaTrans divScroll">
                                                                                <dxcp:ASPxCallbackPanel ID="clbGridClientes" runat="server" Width="100%" ClientInstanceName="clbGridClientes"
                                                                                    OnCallback="clbGridClientes_Callback">
                                                                                    <PanelCollection>
                                                                                        <dxp:PanelContent ID="PanelContent5" runat="server">
                                                                                            <dxwgv:ASPxGridView ID="grvListaFiltroClientes" runat="server" AutoGenerateColumns="False"
                                                                                                Width="98%">
                                                                                                <SettingsPager PageSize="5">
                                                                                                </SettingsPager>
                                                                                                <SettingsText EmptyDataRow="Para incluir na lista, acione a seta vermelha apos selecionar um cliente valido" />
                                                                                                <SettingsBehavior AllowFocusedRow="false" AllowSort="false" />
                                                                                                <Settings GridLines="None" ShowColumnHeaders="false" />
                                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                                </SettingsPager>
                                                                                                <Border BorderColor="White" />
                                                                                                <Columns>
                                                                                                    <dxwgv:GridViewDataTextColumn Caption="Codigo" VisibleIndex="0" Width="15%" FieldName="CODCLI">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn Caption="Nome" VisibleIndex="1" Width="80%" FieldName="NOMCLI">
                                                                                                        <CellStyle Wrap="True">
                                                                                                        </CellStyle>
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewCommandColumn VisibleIndex="3" Width="5%" Caption="Remover" ButtonType="Image">
                                                                                                        <CustomButtons>
                                                                                                            <dxwgv:GridViewCommandColumnCustomButton Visibility="AllDataRows">
                                                                                                                <Image Url="~/Images/Small/cancelar_16.png" />
                                                                                                            </dxwgv:GridViewCommandColumnCustomButton>
                                                                                                        </CustomButtons>
                                                                                                        <CellStyle Cursor="pointer">
                                                                                                        </CellStyle>
                                                                                                    </dxwgv:GridViewCommandColumn>
                                                                                                </Columns>
                                                                                                <ClientSideEvents CustomButtonClick="function(s, e) { clbGridClientes.PerformCallback(e.visibleIndex.toString()); }" />
                                                                                            </dxwgv:ASPxGridView>
                                                                                        </dxp:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dxcp:ASPxCallbackPanel>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                    <td width="50%" class="tableFiltros">
                                                                        <%--FILTRO LISTA CREDENCIADO--%>
                                                                        <div class="divClientesConsulta">
                                                                            <div class="styleCheckBoxFitros" style="width: 100%;">
                                                                                <dxe:ASPxCheckBox ID="chbTipoCredenciado" runat="server" Text="Lista Credenciados:"
                                                                                    Font-Bold="True" Font-Size="12pt" AutoPostBack="false" ClientInstanceName="chbTipoCredenciado">
                                                                                    <ClientSideEvents CheckedChanged="function(s, e) { habilitaListaCredenciado(); }" />
                                                                                </dxe:ASPxCheckBox>
                                                                            </div>
                                                                            <div style="width: 100%; margin-bottom: 10px; margin-top: 5px; padding-left: 5px; font-weight: bold">
                                                                                Informe o Credenciado pelo codigo:
                                                                            </div>
                                                                            <div class="divsPrincipaisConsultaTrans divFiltrosListaCliCred">
                                                                                <div style="width: 8%; text-align: right; float: left; padding-right: 5px;">
                                                                                    Cod.:
                                                                                </div>
                                                                                <div style="width: 17%; text-align: right; float: left;">
                                                                                    <dxe:ASPxTextBox ID="txtCodCreFiltro" runat="server" Width="100%" CssClass="TextBox"
                                                                                        MaxLength="6" ClientInstanceName="txtCodCreFiltro">
                                                                                        <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                        </ReadOnlyStyle>
                                                                                        <ClientSideEvents LostFocus="function(s, e) { clbtxtRazSocial.PerformCallback(); }" />
                                                                                    </dxe:ASPxTextBox>
                                                                                </div>
                                                                                <div style="width: 15%; text-align: right; float: left; padding-right: 5px;">
                                                                                    Raz.Social:
                                                                                </div>
                                                                                <div style="width: 49%; text-align: right; float: left;">
                                                                                    <dxcp:ASPxCallbackPanel ID="clbtxtRazSocial" runat="server" Width="100%" ClientInstanceName="clbtxtRazSocial"
                                                                                        OnCallback="clbtxtRazSocial_Callback">
                                                                                        <PanelCollection>
                                                                                            <dxp:PanelContent ID="PanelContent6" runat="server">
                                                                                                <dxe:ASPxTextBox ID="txtRazSoc" runat="server" Width="100%" ReadOnly="true" ClientInstanceName="txtRazSoc">
                                                                                                    <ReadOnlyStyle CssClass="EditDesabilitado">
                                                                                                    </ReadOnlyStyle>
                                                                                                </dxe:ASPxTextBox>
                                                                                            </dxp:PanelContent>
                                                                                        </PanelCollection>
                                                                                    </dxcp:ASPxCallbackPanel>
                                                                                </div>
                                                                                <div style="width: 3%; text-align: center; float: left; padding-left: 3px;">
                                                                                    <dxe:ASPxButton ID="btnIncluirForm" runat="server" Height="15px" Width="100%" AutoPostBack="False">
                                                                                        <BackgroundImage Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center"
                                                                                            ImageUrl="Images/Small/Fall.gif" />
                                                                                        <ClientSideEvents Click="function(s, e) { 
                                                                                                clbGridCredenciados.PerformCallback('Incluir');
                                                                                                txtCodCreFiltro.SetText('');
                                                                                                txtRazSoc.SetText(''); }" />
                                                                                    </dxe:ASPxButton>
                                                                                </div>
                                                                            </div>
                                                                            <div class="divsPrincipaisConsultaTrans divScroll">
                                                                                <dxcp:ASPxCallbackPanel ID="clbGridCredenciados" runat="server" Width="100%" ClientInstanceName="clbGridCredenciados"
                                                                                    OnCallback="clbGridCredenciados_Callback">
                                                                                    <PanelCollection>
                                                                                        <dxp:PanelContent ID="PanelContent7" runat="server">
                                                                                            <dxwgv:ASPxGridView ID="grvListaFiltroCredenciados" runat="server" AutoGenerateColumns="False"
                                                                                                Width="98%">
                                                                                                <SettingsText EmptyDataRow="Para incluir na lista, acione a seta vermelha apos selecionar um credenciado valido" />
                                                                                                <SettingsBehavior AllowFocusedRow="false" AllowSort="false" />
                                                                                                <Settings GridLines="None" ShowColumnHeaders="false" />
                                                                                                <SettingsPager Mode="ShowAllRecords">
                                                                                                </SettingsPager>
                                                                                                <Border BorderColor="White" />
                                                                                                <Columns>
                                                                                                    <dxwgv:GridViewDataTextColumn Caption="Codigo" VisibleIndex="0" Width="15%" FieldName="CODCRE">
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewDataTextColumn Caption="Razao Social" VisibleIndex="1" FieldName="RAZSOC"
                                                                                                        Width="80%">
                                                                                                        <CellStyle Wrap="False">
                                                                                                        </CellStyle>
                                                                                                    </dxwgv:GridViewDataTextColumn>
                                                                                                    <dxwgv:GridViewCommandColumn VisibleIndex="3" Width="5%" Caption="Remover" ButtonType="Image">
                                                                                                        <CustomButtons>
                                                                                                            <dxwgv:GridViewCommandColumnCustomButton Visibility="AllDataRows">
                                                                                                                <Image Url="~/Images/Small/cancelar_16.png" />
                                                                                                            </dxwgv:GridViewCommandColumnCustomButton>
                                                                                                        </CustomButtons>
                                                                                                        <CellStyle Cursor="pointer">
                                                                                                        </CellStyle>
                                                                                                    </dxwgv:GridViewCommandColumn>
                                                                                                </Columns>
                                                                                                <ClientSideEvents CustomButtonClick="function(s, e) { clbGridCredenciados.PerformCallback(e.visibleIndex.toString()); }" />
                                                                                            </dxwgv:ASPxGridView>
                                                                                        </dxp:PanelContent>
                                                                                    </PanelCollection>
                                                                                </dxcp:ASPxCallbackPanel>
                                                                            </div>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </dxw:ContentControl>
                                                </ContentCollection>
                                            </dxtc:TabPage>
                                            <dxtc:TabPage Name="Colunas para Visualizacao" Text="Colunas para Visualizacao">
                                                <ContentCollection>
                                                    <dxw:ContentControl ID="ContentControl3" runat="server">
                                                        <div style="width: 100%; padding-top: 20px; padding-bottom: 20px;">
                                                            <table width="100%" class="tableFiltros">
                                                                <tr class="linha">
                                                                    <td style="font-size: 12pt; font-weight: bold;">Controle de colunas para visualizacao na listagem
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <dxcp:ASPxCallbackPanel ID="clbColunas" runat="server" Width="100%" ClientInstanceName="clbColunas"
                                                            EnableCallbackCompression="True" AutoGenerateColumns="False" EnableViewState="false"
                                                            LoadingPanelText="Carregando..." OnCallback="clbColunas_Callback">
                                                            <PanelCollection>
                                                                <dxp:PanelContent ID="PanelContent8" runat="server">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td style="width: 40%; vertical-align: top;">
                                                                                <dxwgv:ASPxGridView ID="grvColDisponiveis" runat="server" AutoGenerateColumns="false"
                                                                                    EnableViewState="false" KeyFieldName="Value" EnableCallbackCompression="True"
                                                                                    DataSourceID="dtsColVisualizados" Width="100%"
                                                                                    ClientInstanceName="grvColDisponiveis">
                                                                                    <Columns>
                                                                                        <dxwgv:GridViewDataTextColumn Caption="COLUNAS APRESENTADAS NA LISTAGEM" FieldName="Text"
                                                                                            VisibleIndex="0" Width="98%">
                                                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true">
                                                                                                <HoverStyle ForeColor="White" BackColor="White">
                                                                                                </HoverStyle>
                                                                                            </HeaderStyle>
                                                                                        </dxwgv:GridViewDataTextColumn>
                                                                                        <dxwgv:GridViewDataTextColumn FieldName="Value" Visible="false" VisibleIndex="1">
                                                                                        </dxwgv:GridViewDataTextColumn>
                                                                                    </Columns>
                                                                                    <SettingsBehavior AllowFocusedRow="True" />
                                                                                    <SettingsPager NumericButtonCount="1" PageSize="30" ShowNumericButtons="False">
                                                                                        <AllButton Text="All">
                                                                                        </AllButton>
                                                                                        <FirstPageButton Visible="True">
                                                                                        </FirstPageButton>
                                                                                        <LastPageButton Visible="True">
                                                                                        </LastPageButton>
                                                                                        <Summary AllPagesText="Paginas: {0} - {1}" Text="Pagina {0} de {1}" />
                                                                                    </SettingsPager>
                                                                                    <Settings ShowFilterRow="false" />
                                                                                    <SettingsText EmptyDataRow="Colunas disponiveis" />
                                                                                    <ClientSideEvents EndCallback="function(s, e) { validaGrid(s, e) }" />
                                                                                    <Styles EnableDefaultAppearance="False">
                                                                                    </Styles>
                                                                                    <StylesPager CssFilePath="App_Themes/divColunasConsTrans.css">
                                                                                    </StylesPager>
                                                                                </dxwgv:ASPxGridView>
                                                                            </td>
                                                                            <td style="width: 10%;">
                                                                                <div style="margin-left: 40px; margin-right: auto;">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxButton ID="btnRetirarColuna" runat="server" AutoPostBack="False" EnableViewState="False"
                                                                                                    Width="100%" ToolTip="Retirar coluna da listagem">
                                                                                                    <ClientSideEvents Click="function(s, e) { clbColunas.PerformCallback(1);
                                                                                                            validaGrid();
                                                                                                        }" />
                                                                                                    <Image Url="~/Images/Small/SetaDirVer.png" />
                                                                                                </dxe:ASPxButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxButton ID="btnRetirarTodasColunas" runat="server" AutoPostBack="False" Width="100%"
                                                                                                    ToolTip="Retirar todas as colunas da listagem" EnableViewState="False">
                                                                                                    <ClientSideEvents Click="function(s, e) {
                                                                                                            clbColunas.PerformCallback(2);
                                                                                                            btnResultado.SetEnabled(false);}" />
                                                                                                    <Image Url="~/Images/Small/SetaDirDuplaVer.PNG" />
                                                                                                </dxe:ASPxButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxButton ID="btnIncluirTodasColuna" runat="server" AutoPostBack="False" ToolTip="Incluir todas colunas na listagem"
                                                                                                    EnableViewState="False" Width="100%">
                                                                                                    <ClientSideEvents Click="function(s, e) {
                                                                                                            clbColunas.PerformCallback(3);
                                                                                                            btnResultado.SetEnabled(true);}" />
                                                                                                    <Image Url="~/Images/Small/SetaEsqDuplaVerde.PNG" />
                                                                                                </dxe:ASPxButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <dxe:ASPxButton ID="btnIncluirColuna" runat="server" AutoPostBack="False" Width="100%"
                                                                                                    EnableViewState="False" ToolTip="Incluir Coluna na listagem">
                                                                                                    <ClientSideEvents Click="function(s, e) {
                                                                                                            clbColunas.PerformCallback(4);
                                                                                                            btnResultado.SetEnabled(true);}" />
                                                                                                    <Image Url="~/Images/Small/SetaEsqVerde.png" />
                                                                                                </dxe:ASPxButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                            <td style="width: 35%; vertical-align: top;">
                                                                                <dxwgv:ASPxGridView ID="grvColRetiradas" runat="server" AutoGenerateColumns="False"
                                                                                    EnableViewState="false" Width="100%" KeyFieldName="Value" EnableCallbackCompression="True"
                                                                                    DataSourceID="dtsColunasNaoVisualizadas">
                                                                                    <Columns>
                                                                                        <dxwgv:GridViewDataTextColumn Caption="COLUNAS RETIRADAS DA LISTAGEM" FieldName="Text"
                                                                                            VisibleIndex="0" Width="98%">
                                                                                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                                                                                        </dxwgv:GridViewDataTextColumn>
                                                                                        <dxwgv:GridViewDataTextColumn FieldName="Value" Visible="False" VisibleIndex="1">
                                                                                        </dxwgv:GridViewDataTextColumn>
                                                                                    </Columns>
                                                                                    <SettingsBehavior AllowFocusedRow="True" />
                                                                                    <SettingsPager PageSize="30" ShowNumericButtons="False">
                                                                                        <AllButton Text="All">
                                                                                        </AllButton>
                                                                                        <FirstPageButton Visible="True">
                                                                                        </FirstPageButton>
                                                                                        <LastPageButton Visible="True">
                                                                                        </LastPageButton>
                                                                                        <Summary AllPagesText="Paginas: {0} - {1}" Text="Pagina {0} de {1} " />
                                                                                    </SettingsPager>
                                                                                    <Settings ShowFooter="false" />
                                                                                    <SettingsText EmptyDataRow="Adicionar Colunas para nao visualizacao na listagem" />
                                                                                    <SettingsLoadingPanel Text="Carregando..." />
                                                                                    <ClientSideEvents EndCallback="function(s, e) { FocusGrid() ;}" />
                                                                                </dxwgv:ASPxGridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </dxp:PanelContent>
                                                            </PanelCollection>
                                                        </dxcp:ASPxCallbackPanel>
                                                    </dxw:ContentControl>
                                                </ContentCollection>
                                            </dxtc:TabPage>
                                        </TabPages>
                                    </dxtc:ASPxPageControl>
                                </div>
                                <asp:HiddenField ID="hdAlteraConsulta" runat="server" />
                            </dxp:PanelContent>
                        </PanelCollection>
                    </dxrp:ASPxRoundPanel>
                </dxp:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
    <div style="width: 98%;">
        <dxpc:ASPxPopupControl ID="ppcNomeConsulta" runat="server" ClientInstanceName="ppcNomeConsulta"
            PopupVerticalAlign="WindowCenter" PopupAction="None" PopupHorizontalAlign="WindowCenter"
            ShowCloseButton="true" ShowHeader="true" HeaderText="" Width="300px">
            <ContentCollection>
                <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                </dxpc:PopupControlContentControl>
            </ContentCollection>
            <Windows>
                <dxpc:PopupWindow Name="SalvarConsulta" HeaderText="Digite o nome para a consulta"
                    ShowHeader="True">
                    <ContentCollection>
                        <dxpc:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        <br />
                                        <dxe:ASPxTextBox ID="txtNomeConsulta" runat="server" Width="100%" Height="25px" ClientInstanceName="txtNomeConsulta"
                                            MaxLength="50">
                                        </dxe:ASPxTextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50%;">
                                        <dxe:ASPxButton ID="btnSalvarConsulta" runat="server" ClientInstanceName="btnSalvarConsulta"
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" Text="Salvar"
                                            Width="98%" EnableViewState="False" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) { ValidaBotaoSalvaConsulta();}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td style="width: 50%;">
                                        <dxe:ASPxButton ID="btnCancelarConsulta" runat="server" AutoPostBack="False" ClientInstanceName="btnCancelarConsulta"
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" Text="Cancelar"
                                            Width="98%">
                                            <ClientSideEvents Click="function(s, e) {ppcNomeConsulta.HideWindow(ppcNomeConsulta.GetWindowByName('SalvarConsulta'));}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dxpc:PopupControlContentControl>
                    </ContentCollection>
                </dxpc:PopupWindow>
            </Windows>
        </dxpc:ASPxPopupControl>
    </div>
    <div>
        <asp:ObjectDataSource ID="dtsColVisualizados" runat="server" OnObjectCreating="CriarInstanciaConsulta"
            SelectMethod="ColunasVisualizadas" TypeName="SIL.BL.blConsultaVA">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="-1" Name="ColunasNaoVisualizadas" SessionField="ColunasNaoVisualizadas"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="dtsColunasNaoVisualizadas" runat="server" OnObjectCreating="CriarInstanciaConsulta"
            SelectMethod="ColunasNaoVisualizadas" TypeName="SIL.BL.blConsultaVA">
            <SelectParameters>
                <asp:SessionParameter DefaultValue="-1" Name="ColunasNaoVisualizadas" SessionField="ColunasNaoVisualizadas"
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>

    <script type="text/javascript">
        loadingPanel.Hide();
    </script>

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
        </Windows>
    </dxpc:ASPxPopupControl>
</asp:Content>
