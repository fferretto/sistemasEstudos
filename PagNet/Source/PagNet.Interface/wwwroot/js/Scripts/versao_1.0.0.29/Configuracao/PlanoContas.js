
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
    CarregaPlanosContas();
}

$(window).load(carregaPagina());


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    CarregaPlanosContas();
});

function CarregaPlanosContas() {
    $('#modalListaPlanoContas').empty();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var url = montaUrl("/Configuracao/PlanoContas/CarregaListaPlanoContas/?codigoEmpresa=" + $("#codigoEmpresa").val())

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modalListaPlanoContas").load(url);
    $("#ListaPlanoContas").show();
}
function DesativaPlanoConta(codigo) {

    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $.confirm({
        title: 'Confirma!',
        icon: "glyphicon glyphicon-alert",
        content: 'O sistema irá remover o Plano de Contas e todas as categorias que dependam dele. Confirma a remoção?',
        type: 'blue',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                keys: ['enter'],
                action: function () {
                    var _url = montaUrl("/Configuracao/PlanoContas/RemovePlanoContas/");

                    var data = "codigoPlanoContas=" + codigo;
                    data += "&codigoEmpresa=" + $("#codigoEmpresa").val();
                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: _url,
                        data: data,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                msgSucesso(data.responseText);
                                CarregaPlanosContas();

                            }
                            else {
                                msgErro(data.responseText)
                            }
                        },
                        error: function (data) {
                            $.unblockUI();
                            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
                        }
                    });
                }
            },
            cancel: {
                text: 'Não'
            }
        }
    });   
}
function IncluiNovoPlanoContas() {

    var url = montaUrl("/Configuracao/PlanoContas/NovoPlanoContas/?codigoEmpresa=" + $("#codigoEmpresa").val());

    $("#modal").load(url);
    $("#Localizar").modal();
}
function EditarPlanoContas(codigoPlanoContas){

    var url = montaUrl("/Configuracao/PlanoContas/AlteraPlanoContas/");
    url += "&codigoPlanoContas=" + codigoPlanoContas;

    $("#modal").load(url);
    $("#Localizar").modal();
}

//-------------------------------------INCLUIR NOVO PLANO DE CONTAS ---------------------------------------

function SalvarNovoPlanoContas() {

    $("#CodigoTipoConta").val($('.CodigoTipoConta option:selected').val());
    $("#CodigoNatureza").val($('.CodigoNatureza option:selected').val());

    $.confirm({
        title: 'Confirma!',
        icon: "glyphicon glyphicon-alert",
        content: 'Confirma a inclusão/edição do plano de contas?',
        type: 'blue',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                keys: ['enter'],
                action: function () {
                    var _url = montaUrl("/Configuracao/PlanoContas/SalvarNovoPlanoContas/");

                    var data = "TipoDespesa=" + $("#TipoDespesa").val();
                    data += "&codigoEmpresaPlanoContas=" + $("#codigoEmpresa").val();
                    data += "&PagamentoCentralizadora=" + $("#PagamentoCentralizadora").val();
                    data += "&RecebimentoClienteNetCard=" + $("#RecebimentoClienteNetCard").val();
                    data += "&CODPLANOCONTAS=" + $("#CODPLANOCONTAS").val();
                    data += "&CODPLANOCONTAS_PAI=" + $("#codigoRaiz").val();                    
                    data += "&Classificacao=" + $("#Classificacao").val();
                    data += "&CodigoTipoConta=" + $("#CodigoTipoConta").val();
                    data += "&CodigoNatureza=" + $("#CodigoNatureza").val();
                    data += "&Descricao=" + $("#Descricao").val();
                    data += "&codigoRaiz=" + $("#codigoRaiz").val();
                                                         
                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: _url,
                        data: data,
                        success: function (data) {
                            $.unblockUI();
                            $("#Localizar .close").click();
                            if (data.success) {

                                msgSucesso(data.responseText);
                                CarregaPlanosContas();

                            }
                            else {
                                msgErro(data.responseText)
                            }
                        },
                        error: function (data) {
                            $.unblockUI();
                            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
                        }
                    });
                }
            },
            cancel: {
                text: 'Não'
            }
        }
    });
}
