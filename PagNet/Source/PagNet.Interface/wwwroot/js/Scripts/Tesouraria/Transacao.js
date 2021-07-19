function carregaPagina() {

    if ($('#acessoAdmin').val() == "False") {
        $('.codigoEmpresa').prop('disabled', true);
    }

}

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});
$(".codContaCorrente").change(function () {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
});

function ConsultarTransacoes() {
    $('#modal').empty();
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var contacorrente = $("#codContaCorrente").val();

    if (contacorrente == '' || contacorrente == '-1' || contacorrente == "null" || contacorrente == null) {
        valido = false;
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
    }

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });


    var url = montaUrl("/Tesouraria/Transacao/ListagemTransacao/?MesRef=" + $("#MesRef").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codContaCorrente=" + contacorrente)


    $("#modal").load(url);
    $("#ListTransacao").show();
};
function IncluirTransacao(Codigo, TipoTransacao) {

    var url = montaUrl("/Tesouraria/Transacao/IncluirTransacao/")
    url += ("&codigoTransacao=" + Codigo)
    url += ("&TipoTransacao=" + TipoTransacao)

    $("#modalAbreTelaModal").load(url);
    $("#AbreTelaModal").modal();
};
function EditarTransacao(Codigo, TipoTransacao) {

    var url = montaUrl("/Tesouraria/Transacao/EdicaoTransacao/")
    url += ("&CodTransacao=" + Codigo)
    url += ("&TipoTransacao=" + TipoTransacao)

    $("#modalAbreTelaModal").load(url);
    $("#AbreTelaModal").modal();
};
function Transacao() {

    $.confirm({
        title: 'Confirmar Consolidação!',
        icon: "glyphicon glyphicon-alert",
        content: ' Confirma a consolidação desta transação? ',
        type: 'green',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                action: function () {
                    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
                    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

                    var table = $('#gridListaTransacao').DataTable();
                    var trs = $(table.cells().nodes()).parent()
                    var Meudata = "";
                    $(trs).each(function (index, value) {
                        if ($(value).hasClass("selected")) {
                            Meudata = "&ListaTransacao%5B0%5D.codigo=" + $(value).find('.codigo').find('input').val();
                            Meudata += "&ListaTransacao%5B0%5D.Descricao=" + $(value).find('.Descricao').find('input').val();
                            Meudata += "&ListaTransacao%5B0%5D.dataSolicitacao=" + $(value).find('.dataSolicitacao').find('input').val();
                            Meudata += "&ListaTransacao%5B0%5D.dataPGTO=" + $(value).find('.dataPGTO').find('input').val();
                            Meudata += "&ListaTransacao%5B0%5D.ValorTransacao=" + $(value).find('.ValorTransacao').find('input').val();
                            Meudata += "&ListaTransacao%5B0%5D.TipoTransacao=" + $(value).find('.TipoTransacao').find('input').val();
                        }
                    });

                    var data = "codContaCorrente=" + $("#codigoEmpresa").val()
                        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
                        + Meudata;

                    var url = montaUrl("/Tesouraria/Transacao/ConsolidaTransacao/")
                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: data,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                var table = $('#gridListaTransacao').DataTable();
                                var trs = $(table.cells().nodes()).parent()
                                $(trs).each(function (index, value) {
                                    if ($(value).hasClass("selected")) {
                                        var linha = $(value).closest('tr');
                                        linha.hide();
                                    }
                                });

                                table.rows('.selected').remove().draw(false);

                                msgSucesso(data.responseText);

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
                text: 'Não',
                keys: ['enter']
            }
        }
    });

};
function CancelarTransacao() {


    var table = $('#gridListaTransacao').DataTable();
    var trs = $(table.cells().nodes()).parent()
    var Meudata = "";
    $(trs).each(function (index, value) {
        if ($(value).hasClass("selected")) {
            Meudata = "&codigoTransacao=" + $(value).find('.codigo').find('input').val();
            Meudata += "&TipoTransacao=" + $(value).find('.TipoTransacao').find('input').val();
        }
    });

    var url = montaUrl("/Tesouraria/Transacao/VerificaParcelasFuturas/")
    url += Meudata;

    // Submit form data via ajax
    $.ajax({
        type: "Get",
        url: url,
        success: function (data) {
            $.unblockUI();

            if (data.resultado) {
                CancelaEstaTransacoesEFuturas();
            }
            else {
                CancelaApenasUmaTransacao();
            }
        },
        error: function (data) {
            $.unblockUI();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });


};
function CancelaApenasUmaTransacao() {
    $.confirm({
        title: 'Confirmar Cancelamento!',
        icon: "glyphicon glyphicon-alert",
        content: ' Confirma o cancelamento desta transação? ',
        type: 'red',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                action: function () {
                    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
                    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

                    var table = $('#gridListaTransacao').DataTable();
                    var trs = $(table.cells().nodes()).parent()
                    var Meudata = "";

                    $(trs).each(function (index, value) {
                        if ($(value).hasClass("selected")) {
                            Meudata = "codigoTransacao=" + $(value).find('.codigo').find('input').val();
                            Meudata += "&ParcelaTermino=1";
                            Meudata += "&TipoTransacao=" + $(value).find('.TipoTransacao').find('input').val();
                        }
                    });

                    var url = montaUrl("/Tesouraria/Transacao/CancelarTransacao/")

                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: Meudata,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                var table = $('#gridListaTransacao').DataTable();
                                var trs = $(table.cells().nodes()).parent()
                                $(trs).each(function (index, value) {
                                    if ($(value).hasClass("selected")) {
                                        var linha = $(value).closest('tr');
                                        linha.hide();
                                    }
                                });

                                table.rows('.selected').remove().draw(false);

                                msgSucesso(data.responseText);

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
                text: 'Não',
                keys: ['enter']
            }
        }
    });

};
function CancelaEstaTransacoesEFuturas() {

    var table = $('#gridListaTransacao').DataTable();
    var trs = $(table.cells().nodes()).parent()
    var Meudata = "";

    $(trs).each(function (index, value) {
        if ($(value).hasClass("selected")) {
            Meudata = "codigoTransacao=" + $(value).find('.codigo').find('input').val();
            Meudata += "&TipoTransacao=" + $(value).find('.TipoTransacao').find('input').val();
        }
    });

    var url = montaUrl("/Tesouraria/Transacao/CancelarTransacao/")

    $.confirm({
        title: 'Confirmar Cancelamento!',
        icon: "glyphicon glyphicon-alert",
        content: 'Cancelar as transações futuras?',
        type: 'red',
        buttons: {
            MaisTransacao: {
                text: 'Sim',
                btnClass: 'btn-red',
                action: function () {
                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    Meudata += "&ParcelaTermino=2";
                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: Meudata,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                var table = $('#gridListaTransacao').DataTable();
                                var trs = $(table.cells().nodes()).parent()
                                $(trs).each(function (index, value) {
                                    if ($(value).hasClass("selected")) {
                                        var linha = $(value).closest('tr');
                                        linha.hide();
                                    }
                                });

                                table.rows('.selected').remove().draw(false);

                                msgSucesso(data.responseText);

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
            UmaTransacao: {
                text: 'Não, Apenas Esta',
                btnClass: 'btn-green',
                action: function () {

                    Meudata += "&ParcelaTermino=1";


                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: Meudata,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                var table = $('#gridListaTransacao').DataTable();
                                var trs = $(table.cells().nodes()).parent()
                                $(trs).each(function (index, value) {
                                    if ($(value).hasClass("selected")) {
                                        var linha = $(value).closest('tr');
                                        linha.hide();
                                    }
                                });

                                table.rows('.selected').remove().draw(false);

                                msgSucesso(data.responseText);

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
                text: 'Cancelar',
                keys: ['enter']
            }
        }
    });

};
function AlterarTituloPGTO() {

    $.confirm({
        title: 'Confirmar Alteração!',
        icon: "glyphicon glyphicon-alert",
        content: ' Confirma a alteração desta transação? ',
        type: 'green',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                action: function () {

                    var data = "";

                    $("#codContaCorrenteTransacao").val($('.codContaCorrenteTransacao option:selected').val());

                    data = 'DescricaoTransacao=' + $("#DescricaoTransacao").val()
                        + '&dtTransacao=' + $("#dtTransacao").val()
                        + '&ValorTransacao=' + $("#ValorTransacao").val()
                        + '&codigoTransacao=' + $("#codigoTransacao").val()
                        + '&TipoTransacao=' + $("#TipoTransacao").val()
                        + "&codigoEmpresaTransacao=" + $("#codigoEmpresa").val()
                        + '&codContaCorrenteTransacao=' + $("#codContaCorrenteTransacao").val();


                    var url = montaUrl("/Tesouraria/Transacao/SalvaEdicaoTransacao/")

                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: data,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                msgSucesso(data.responseText);

                                //ConsultarTransacoes();

                                var table = $('#gridListaTransacao').DataTable();
                                var trs = $(table.cells().nodes()).parent()
                                $(trs).each(function (index, value) {
                                    if ($(value).hasClass("selected")) {
                                        $(value).find('.inpDescricao')[0].innerHTML = $("#DescricaoTransacao").val();
                                        $(value).find('.inpdataPGTO')[0].innerHTML = $("#dtTransacao").val();
                                        $(value).find('.inpValorTransacao')[0].innerHTML = $("#ValorTransacao").val();
                                    }
                                });

                                $("#AbreTelaModal .close").click();
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
                text: 'Não',
                keys: ['enter']
            }
        }
    });
};
function IncluiTransacao() {

    if ($("#TipoIncTransacao").val() == "") {
        msgAviso("Obrigatório informar o tipo da transação!");
        return false;
    }

    $.confirm({
        title: 'Confirmar Alteração!',
        icon: "glyphicon glyphicon-alert",
        content: ' Confirma a alteração desta transação? ',
        type: 'green',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                action: function () {

                    var data = "";

                    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
                    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

                    data = 'DescricaoIncTransacao=' + $("#DescricaoIncTransacao").val()
                        + '&dtIncTransacao=' + $("#dtIncTransacao").val()
                        + '&RepetirIncTransacao=' + $("#RepetirIncTransacao").val()
                        + '&ValorIncTransacao=' + $("#ValorIncTransacao").val()
                        + '&TipoIncTransacao=' + $("#TipoIncTransacao").val()
                        + "&ParcelaInicioIncTransacao=" + $("#ParcelaInicioIncTransacao").val()
                        + "&ParcelaTerminoIncTransacao=" + $("#ParcelaTerminoIncTransacao").val()
                        + "&codigoEmpresaIncTransacao=" + $("#codigoEmpresa").val()
                        + '&codContaCorrenteIncTransacao=' + $("#codContaCorrente").val();


                    var url = montaUrl("/Tesouraria/Transacao/IncluirTransacao/")

                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: data,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                msgSucesso(data.responseText);

                                $("#AbreTelaModal .close").click();
                                //ConsultarTransacoes();
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
                text: 'Não',
                keys: ['enter']
            }
        }
    });
};
function ConsolidarTransacao() {
    $.confirm({
        title: 'Confirmar Cosnolidação!',
        icon: "glyphicon glyphicon-alert",
        content: ' Confirma a consolidação desta transação? ',
        type: 'green',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-green',
                action: function () {


                    var table = $('#gridListaTransacao').DataTable();
                    var trs = $(table.cells().nodes()).parent()
                    var Meudata = "";
                    $(trs).each(function (index, value) {
                        if ($(value).hasClass("selected")) {
                            Meudata = "codigoTransacao=" + $(value).find('.codigo').find('input').val();
                            Meudata += "&TipoTransacao=" + $(value).find('.TipoTransacao').find('input').val();
                        }
                    });

                    var url = montaUrl("/Tesouraria/Transacao/ConsolidaTransacao/")
                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: url,
                        data: Meudata,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                var table = $('#gridListaTransacao').DataTable();
                                var trs = $(table.cells().nodes()).parent()
                                $(trs).each(function (index, value) {
                                    if ($(value).hasClass("selected")) {
                                        var linha = $(value).closest('tr');
                                        linha.hide();
                                    }
                                });

                                table.rows('.selected').remove().draw(false);

                                msgSucesso(data.responseText);

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
                text: 'Não',
                keys: ['enter']
            }
        }
    });

};

