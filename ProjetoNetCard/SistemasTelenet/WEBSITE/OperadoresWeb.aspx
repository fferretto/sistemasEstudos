<%@ Page Language="C#" MasterPageFile="~/Cadastro.master" AutoEventWireup="true" CodeFile="OperadoresWeb.aspx.cs" Inherits="OperadoresWeb"%>
<%@ MasterType VirtualPath="~/Cadastro.master" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxCallbackPanel" tagprefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v8.3, Version=8.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxwm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphCadastro" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <dxe:ASPxButton ID="btnIncOpWeb" runat="server" EnableViewState="false" AutoPostBack="false"
                        ToolTip="Novo">
                        <Image Url="Images/Small/Novo_16.png" />
                        <ClientSideEvents Click="function (s, e){window.cplDadosOpWebParc.PerformCallback('Novo');} " />
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnCancOpWeb" runat="server" EnableViewState="false" AutoPostBack="false"
                        ToolTip="Cancelar">
                        <Image Url="Images/Small/Cancelar_16.png" />
                        <ClientSideEvents Click="function (s, e){window.cplDadosOpWebParc.PerformCallback('Cancelar');} " />
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnEditarOpWeb" runat="server" EnableViewState="false" AutoPostBack="false"
                        ToolTip="Editar">
                        <Image Url="Images/Small/Editar_16.png" />
                        <ClientSideEvents Click="function (s, e){window.cplDadosOpWebParc.PerformCallback('Editar');}" />
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnSalvarOpWeb" runat="server" EnableViewState="false" AutoPostBack="false"
                        ToolTip="Salvar">
                        <Image Url="Images/Small/Salvar_16.png" />
                        <ClientSideEvents Click="function (s, e){ AcaoSalvarOp(); }" />
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnExcOpWeb" runat="server" EnableViewState="false" AutoPostBack="false"
                        ToolTip="Excluir">
                        <Image Url="Images/Small/Deletar_16.png" />
                        <ClientSideEvents Click="function (s, e){ValidarExclusaoOperador();}" />
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnRenovarAcesso" runat="server" EnableViewState="false" AutoPostBack="false"
                        ToolTip="Renovar Acesso">
                        <Image Url="Images/Small/Chaves.png" />
                        <ClientSideEvents Click="function (s, e){ ValidarRenovacaoAcessoUsuario(); }" />
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
        <div style="padding-left: 5px; height: 470px;">
            <div style="margin-bottom: 5px; margin-top: 5px;">
                <dxe:ASPxLabel ID="ASPxLabel1" runat="server" CssClass="TituloPanel" Text="OPERADOR ACESSO MÚLTIPLO" Width="100%"></dxe:ASPxLabel>
            </div>
            <dxwgv:ASPxGridView ID="grvOperadorParc" runat="server" AutoGenerateColumns="False" EnableCallbackCompression="True" OnHtmlDataCellPrepared="grvOperadorParc_HtmlDataCellPrepared"
				KeyFieldName="ID" EnableViewState="false" Width="99.5%" Enabled="True" ClientInstanceName="grvOperadorParc">
                <Columns>
                    <dxwgv:GridViewDataTextColumn Caption="Tipo Seleção" FieldName="TIPOACESSO" VisibleIndex="0" Width="15%"></dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Grupo" FieldName="NOMGRUPO" VisibleIndex="1" Width="25%"></dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME" VisibleIndex="2" Width="25%"></dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Login" FieldName="LOGIN" VisibleIndex="3" Width="25%"></dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Validade Senha" FieldName="DTEXPSENHA" VisibleIndex="4" Width="10%"></dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="ID" Visible="false"></dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="CODPARCERIA" Visible="false"></dxwgv:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior AllowFocusedRow="true" />
                <SettingsPager PageSize="5">
                    <AllButton Text="All">
                    </AllButton>
                    <NextPageButton Text="Next &gt;">
                    </NextPageButton>
                    <PrevPageButton Text="&lt; Prev">
                    </PrevPageButton>
                </SettingsPager>
                <SettingsText EmptyDataRow="Favor clicar no botao 'Novo' para incluir operador" PopupEditFormCaption="Favor informar os dados" />
                <ClientSideEvents
                    Init="function (s, e) { window.cplDadosOpWebParc.PerformCallback('Pesquisar'); }"
                    FocusedRowChanged="function (s, e) { window.cplDadosOpWebParc.PerformCallback('Pesquisar'); }"
                    RowDblClick="function (s, e) { window.cplDadosOpWebParc.PerformCallback('Editar'); }" />
                <SettingsLoadingPanel Mode="Disabled" ShowImage="false" />
                <Styles>
                    <SelectedRow ForeColor="#CCFFFF">
                    </SelectedRow>
                </Styles>
            </dxwgv:ASPxGridView>
                    
            <div id="DadosOperWeb" style="margin-top: 10px;">
                <dxcp:ASPxCallbackPanel ID="cplDadosOpWebParc" runat="server" ClientInstanceName="cplDadosOpWebParc"
                    EnableCallbackCompression="true" EnableViewState="false" OnCallback="cplDadosOpWebParc_Callback"
                    LoadingPanelText="Carregando&amp;hellip;">
                    <ClientSideEvents EndCallback="function(s, e) {grvOperadorParc.Refresh(); PreparaTipoSelecao(); MaskNumeros(); }" />
                    <PanelCollection>
                        <dxp:PanelContent>
                            <div id="InfoSenha" runat="server" visible="false">
                                <dxe:ASPxLabel ID="lblObs" Text="Senha expirada." ForeColor="Red" Font-Bold="true" runat="server" EnableViewState="false">
                                </dxe:ASPxLabel>
                            </div>
                            <dxcp:ASPxCallbackPanel ID="clbCliente" runat="server" ClientInstanceName="clbCliente"
                                EnableCallbackCompression="true" EnableViewState="False" OnCallback="clbCliente_Callback">
                                <PanelCollection>
                                    <dxp:PanelContent ID="PanelContent8" runat="server">
                                        <asp:HiddenField id="hdTipoAcesso" runat="server"/>
                                        <div style="width: 33%; float: left;">
                                            <dxe:ASPxRadioButtonList ID="rblTipoOperador" runat="server" ClientInstanceName="rblTipoOperador"
                                                EnableDefaultAppearance="False" Border-BorderStyle="None" ItemSpacing="20px" SelectedIndex="0"
                                                CssClass="TextBox TrueOrFalse" RepeatDirection="Horizontal" Width="90%" Font-Size="Small">
                                                <Items>
                                                    <dxe:ListEditItem Text="Parceria" Value="0" />
                                                    <dxe:ListEditItem Text="Seleção Clientes" Value="1" />
                                                </Items>
                                                <Border BorderStyle="None"></Border>
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { AlterarTipoOperador(); }" />
                                            </dxe:ASPxRadioButtonList>
                                        </div>
                                        <div style="float: left; width: 67%;padding-bottom: 5px;height:25px;">
                                            <div id="divSelecaoCliente" runat="server" style="display:none;">
                                                <div style="float: left; width: 95%;">
                                                    <dxe:ASPxTextBox ID="txtClienteAgrupamento" runat="server" ClientInstanceName="txtClienteAgrupamento" EnableViewState="false"
                                                        ReadOnly="true" Native="true" Width="98%" Font-Bold="true" CssClass="TextBox">
                                                        <ReadOnlyStyle CssClass="EditDesabilitado">
                                                        </ReadOnlyStyle>
                                                    </dxe:ASPxTextBox>
                                                    <dxwm:ASPxPopupMenu ID="menuSelCliente" ClientInstanceName="menuSelCliente" runat="server">
                                                        <ClientSideEvents ItemClick="function(s, e){ PopUpClienteClick(s, e); }" />
                                                    </dxwm:ASPxPopupMenu>
                                                </div>                                                    
                                                <div style="float: left; width:5%;">
                                                    <dxe:ASPxButton ID="btnIncluirCliente" runat="server" Height="15px" Width="10px" 
                                                        AutoPostBack="False" SkinID="Clique para permitir">
                                                        <BackgroundImage Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center"
                                                            ImageUrl="Images/Small/RaiseJ.gif" />
                                                        <ClientSideEvents Click="function(s, e) {
                                                            clbCliente.PerformCallback('I');    
                                                        }" />
                                                    </dxe:ASPxButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div style="float: left;width:33%;padding-top: 5px;">
                                            <dxe:ASPxLabel ID="lblSelecao" ClientInstanceName="lblSelecao" Font-Bold="true" Text="Selecione a Parceria" runat="server" EnableViewState="false">
                                            </dxe:ASPxLabel>
                                        </div>
                                        <div style="float:left; width:67%;padding-top:5px;">
                                            <dxe:ASPxLabel ID="lblInfo" Font-Bold="true" Text="Operador com acesso a todos clientes desta parceria." runat="server" EnableViewState="false">
                                            </dxe:ASPxLabel>
                                        </div>
                                        <%--<div style="padding-bottom:33px;">--%>
                                            <div style="width: 33%; float: left;">
                                                <div id="divParceria" runat="server">
                                                    <dxe:ASPxComboBox runat="server" EnableCallbackMode="True" ValueType="System.String" ClientInstanceName="cbxParceria"
                                                        ID="cbxParceria" DropDownStyle="DropDown" Width="90%" Native="true" CssClass="ComboBox" ReadOnly="true"
                                                        Font-Size="Small" TextField="NOMPARCERIA" ValueField="CODPARCERIA">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e){ cblAgrupamento.PerformCallback(); } " />
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                <div id="divCodCliente" runat="server" style="display:none">
                                                    <div style="width: 50%; float: left;">
                                                        <dxe:ASPxTextBox ID="txtNomGrupo" runat="server" EnableViewState="False" MaxLength="20" Width="90%">
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                    <div style="width: 50%; float: left;">
                                                        <dxe:ASPxTextBox ID="txtCodClienteAgrupamento" runat="server" ClientInstanceName="txtCodClienteAgrupamento" MaxLength="5"
                                                            EnableViewState="false" CssClass="TextBox MaskNum" Width="90%" Native="true">
                                                            <ClientSideEvents ValueChanged="function(s, e) { clbCliente.PerformCallback(); }" />
                                                        </dxe:ASPxTextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div style="width: 67%; float: left;padding-bottom: 10px;">
                                                <div style="float: left; width: 95%;">
                                                    <dxcp:ASPxCallbackPanel ID="cplAgrupamento" runat="server" ClientInstanceName="cblAgrupamento"
                                                        EnableCallbackCompression="true" EnableViewState="false" OnCallback="cplAgrupamento_Callback"
                                                        LoadingPanelText="Carregando&amp;hellip;">
                                                        <ClientSideEvents EndCallback="function(s, e) { MaskNumeros(); }" />
                                                        <PanelCollection>
                                                            <dxp:PanelContent>                                                                
                                                                <dxe:ASPxComboBox runat="server" EnableCallbackMode="True" ValueType="System.String" SelectedIndex="0"
                                                                    ID="cbxAgrupamentoCliente" ClientInstanceName="cbxAgrupamentoCliente" DropDownStyle="DropDown" 
                                                                    Width="99%" Native="true" CssClass="ComboBox" Font-Size="Small">
                                                                </dxe:ASPxComboBox>
                                                            </dxp:PanelContent>
                                                        </PanelCollection>
                                                    </dxcp:ASPxCallbackPanel>
                                                </div>
                                                <div style="float: left; width:5%;">
                                                    <dxe:ASPxButton ID="btnExcluirClienteAgrupamento" ClientInstanceName="btnExcluirClienteAgrupamento" runat="server" Height="15px" Width="10px" 
                                                        AutoPostBack="False" SkinID="Excluir cliente do acesso múltiplo.">
                                                        <BackgroundImage Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center"
                                                            ImageUrl="Images/Small/cancelar_16.png" />
                                                        <ClientSideEvents Click="function(s, e) {
                                                            clbCliente.PerformCallback('E:' + cbxAgrupamentoCliente.GetSelectedIndex());    
                                                        }" />
                                                    </dxe:ASPxButton>
                                                </div>
                                            </div>
                                        <%--</div>--%>
                                        <br />
                                    </dxp:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s, e){ AbrirMenuCliente(); MaskNumeros(); }" />
                            </dxcp:ASPxCallbackPanel>
                            <div>
                                <div style="width: 33%; float: left;">Nome</div>
                                <div style="width: 33%; float: left">Login</div>
                                <div style="width: 33%; float: left">Validade Senha</div>
                            </div>
                            <div>
                                <div style="width: 33%; float: left">
                                    <dxe:ASPxTextBox ID="edtNomeOpWeb" runat="server" EnableViewState="False" MaxLength="50" Width="90%">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div style="width: 33%; float: left">
                                    <dxe:ASPxTextBox ID="edtLoginOpWeb" runat="server" EnableViewState="false" Width="90%" MaxLength="16">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div style="width: 33%; float: left;padding-bottom:10px;">
                                    <dxe:ASPxTextBox ID="edtValidade" runat="server" EnableViewState="false" Width="90%" MaxLength="16" ReadOnly="true" CssClass="EditDesabilitado">
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="height: 76px">
                                <div class="Label" style="float: left; width: 33%">
                                    <div>
                                        <dxe:ASPxCheckBox ID="checkSelecionarTodos" runat="server" Text="Selecionar Todos" EnableViewState="false" Width="180px" CssClass="no-save">
                                            <ClientSideEvents CheckedChanged="function(s, e){ todos(s); }" />
                                        </dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div class="Label" style="float: left; width: 64%">
                                    <div>
                                        <dxe:ASPxCheckBox ID="ckxAcessoBloqueado" ClientInstanceName="ckxAcessoBloqueado" runat="server" Text="Bloquear Operador" EnableViewState="false" ForeColor="Red" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxIncCartao" ClientInstanceName="ckxIncCartao" runat="server" Text="Incluir Cartao" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxCancCartao" ClientInstanceName="ckxCancCartao" runat="server" Text="Cancelar Cartao" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 34%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxBloCartao" ClientInstanceName="ckxBloCartao" runat="server" Text="Bloquear Cartao" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxEmitir2Via" ClientInstanceName="ckxEmitir2Via" runat="server" Text="Emitir Segunda Via" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxListarCartao" ClientInstanceName="ckxListarCartao" runat="server" Text="Listar Cartoes" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 34%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxDesbCartao" ClientInstanceName="ckxDesbCartao" runat="server" Text="Desbloquear Cartao" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxLitarIncCartao" ClientInstanceName="ckxLitarIncCartao" runat="server" Text="Listar Inclusoes Cartoes" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxListarExtMov" ClientInstanceName="ckxListarExtMov" runat="server" Text="Listar Extrato Movimento" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 34%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxConsRede" ClientInstanceName="ckxConsRede" runat="server" Text="Consultar Rede Credenciada" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxAlterarLimite" ClientInstanceName="ckxAlterarLimite" runat="server" Text="Alterar Limite do Cartão" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxListTransAb" ClientInstanceName="ckxListTransAb" runat="server" Text="Listar Transações Abertas" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 34%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxSolicitarCarga" ClientInstanceName="ckxSolicitarCarga" runat="server" Text="Solicitar Carga" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                </div>
                                <div>
                                    <div style="width: 33%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxTransferirSaldo" ClientInstanceName="ckxTransferirSaldo" runat="server" Text="Transferir Saldo Cartão" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                    <div style="width: 64%; float: left">
                                        <dxe:ASPxCheckBox ID="ckxTransferirSaldoConta" ClientInstanceName="ckxTransferirSaldoConta" runat="server" Text="Transferir Saldo Cliente" EnableViewState="false" Width="180px" CssClass="no-save"></dxe:ASPxCheckBox>
                                    </div>
                                </div>
                            </div>
                        </dxp:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </div>
        </div>
    </div>
</asp:Content>