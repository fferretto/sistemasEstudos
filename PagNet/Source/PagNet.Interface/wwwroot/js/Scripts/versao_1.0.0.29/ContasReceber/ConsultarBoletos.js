function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    $("#nomeCliente").prop('disabled', true);

    CarregaListaBoletos();
}

$(window).load(carregaPagina());

$("#dtInicio").change(function () {
    var dtInicio = $("#dtInicio").val().split("/");
    var dtFim = $("#dtFim").val().split("/");
    var dataIni = new Date(dtInicio[2], dtInicio[1] - 1, dtInicio[0]);
    var dataFim = new Date(dtFim[2], dtFim[1] - 1, dtFim[0]);
    alert(1)
    if (dataFim < dataIni) {
        $("#dtFim").val($("#dtInicio").val())
    }
});

$("#dtFim").change(function () {
    var dtInicio = $("#dtInicio").val().split("/");
    var dtFim = $("#dtFim").val().split("/");
    var dataIni = new Date(dtInicio[2], dtInicio[1] - 1, dtInicio[0]);
    var dataFim = new Date(dtFim[2], dtFim[1] - 1, dtFim[0]);
    alert(2)
    if (dataFim < dataIni) {
        $("#dtInicio").val($("#dtFim").val());
    }
});

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});

$(".codStatus").change(function () {
    $("#codStatus").val($('.codStatus option:selected').val());
});
$('#filtroCliente').focusout(function () {
    var filtro = $('#filtroCliente').val();
    if (filtro != "") {

        $.ajax({
            type: 'get',
            url: montaUrl('/ContasReceber/ConsultarBoletos/BuscaCliente/?filtro=' + filtro),
            dataType: 'json',
            success: function (data) {
                if (data.codcliente != null && data.codcliente > 0) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);

                } else {
                    $("#nomeCliente").val("Cliente não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                msgErro("Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtroCliente").val('');
        $("#nomeCliente").val('');
        $("#CodigoCliente").val(0)
    }
})


function CarregaListaBoletos() {

    $('#modal').empty();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#codStatus").val($('.codStatus option:selected').val());


    var url = montaUrl("/ContasReceber/ConsultarBoletos/CarregaListaBoletos/?codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codStatus=" + $("#codStatus").val()
        + "&dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodigoCliente=" + $("#CodigoCliente").val())


    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaBordero").show();

};

function AtualizaSolicitacaoCarga() {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/AtualizaSolicitacaoCarga/");

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $.ajax({
        type: "Post",
        url: url,
        success: function (data) {
            $.unblockUI();

            if (data.success) {
                CarregaListaBoletos();
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

};

function EnviarEmail(codEmissaoBoleto) {
    $.confirm({
        title: "Confirmar",
        icon: "glyphicon glyphicon-alert",
        content: "Confirma o envio do boleto para o email cadastrado na conta do cliente?",
        type: 'green',
        typeAnimated: true,
        buttons: {
            formSubmit: {
                text: 'Sim',
                btnClass: 'btn-blue',
                action: function () {
                    var url = montaUrl("/ContasReceber/ConsultarBoletos/EnviarBoletoEmail/");

                    url += "&codEmissaoBoleto=" + codEmissaoBoleto

                    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

                    $.ajax({
                        type: "Post",
                        url: url,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {
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
                btnClass: 'btn-red',
                action: function () { }
            },
            tryAgain: {
                text: 'Enviar para outro email',
                btnClass: 'btn-default',
                action: function () {
                    $.confirm({
                        title: 'Email Para Envio!',
                        content: '' +
                            '<form action="" class="formName">' +
                            '<div class="form-group">' +
                            '<label>Informe o email que irá receber o boleto</label>' +
                            '<input type="text" placeholder="email@provedor.com.br" class="email form-control" required />' +
                            '</div>' +
                            '</form>',
                        buttons: {
                            formSubmit: {
                                text: 'Enviar',
                                btnClass: 'btn-blue',
                                action: function () {
                                    var email = this.$content.find('.email').val();
                                    if (!email) {
                                        $.alert('Email não informado!');
                                        return false;
                                    }

                                    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

                                    var _url = montaUrl("/ContasReceber/ConsultarBoletos/EnviarBoletoOutroEmail/");

                                    _url += "&codEmissaoBoleto=" + codEmissaoBoleto
                                        + "&email=" + email


                                    $.ajax({
                                        type: "Post",
                                        url: _url,
                                        success: function (data) {
                                            $.unblockUI();

                                            if (data.success) {

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
                            cancel: function () {
                                //close
                            },
                        },
                        onContentReady: function () {
                            // bind to events
                            var jc = this;
                            this.$content.find('form').on('submit', function (e) {
                                // if the user submits the form by pressing enter in the field.
                                e.preventDefault();
                                jc.$$formSubmit.trigger('click'); // reference the button and click it
                            });
                        }
                    });
                },

            }
        }
    });
}

function VisualizarLog(CodFatura) {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/VisualizarLog/");

    url += "&CodFatura=" + CodFatura

    $("#modalLog").load(url);
    $("#VisualizarLog").modal();
}
function AbrirJanelaAcoes(codEmissaoBoleto, Status) {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/RealizaAcoes/");

    url += "&CodFatura=" + codEmissaoBoleto
    url += "&status=" + Status

    $("#modalJanelaAcoes").load(url);
    $("#JanelaAcoes").modal();

}
function EditarFaturamento(codEmissaoBoleto) {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/EditarPedidoFaturamento/");

    url += "&CodFatura=" + codEmissaoBoleto
    url += "&codEmpresa=" + $('.codigoEmpresa option:selected').val()

    $("#modalJanelaAcoes").load(url);
    $("#JanelaAcoes").modal();

}

function JustificarCancelamentoPedidoFaturamento(CodFatura) {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/JustificarCancelamentoPedidoFaturamento")

    $("#modalJanelaAcoes").load(url);
    $("#JanelaAcoes").modal();


}
function CancelarPedidoFaturamento() {


    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    var DescJustOutros = $('#DescJustOutros').val();

    if (codJustificativa == "-1" || codJustificativa == "" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da liquidação manual.");
        return false;
    }

    var table = $('#gridConsultaFaturamento').DataTable();
    var trs = $(table.cells().nodes()).parent()
    var codigoFaturamento = 0;
    $(trs).each(function (index, value) {
        //console.log(value)
        if ($(value).hasClass("selected")) {
            codigoFaturamento = $(value).find('.codEmissaoBoleto').find('input').val();
        }
    });

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $("#ValorRecebido").prop('disabled', false);

    var data = "codigoEmissaoBoleto=" + codigoFaturamento
        + "&codJustificativa=" + codJustificativa
        + "&DescJustOutros=" + DescJustOutros
        + "&descJustificativa=" + descJustificativa


    var _url = montaUrl("/ContasReceber/ConsultarBoletos/CancelarFaturamento/");

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {


                var TableTitulos = $('#gridConsultaFaturamento').DataTable()

                var trs = $(TableTitulos.cells().nodes()).parent();
                $(trs).each(function (index, value) {

                    if (codigoFaturamento == $(value).find('.codEmissaoBoleto').find('input').val()) {

                        var linha = $(value).closest('tr');
                        linha.hide();
                    }

                });

                table.rows('.selected').remove().draw(false);
                $("#JanelaAcoes .close").click();
                msgSucesso(data.responseText)

            }
            else {
                msgErro(data.responseText)
            }
        }
    });

}

function AbrirJanelaLiquidacaoManual(CodFaturamento) {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/LiquidarManualmente/");

    url += "&CodFatura=" + CodFaturamento
    url += "&codEmpresa=" + $('.codigoEmpresa option:selected').val()

    $("#modalJanelaAcoes").load(url);
    $("#JanelaAcoes").modal();

}

function LiquidacaoManual(CodFatura) {

    var codigoContaCorrente = $('.codigoContaCorrente option:selected').val()
    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    var DescJustOutros = $('#DescJustOutros').val();

    if (codJustificativa == "-1" || codJustificativa == "" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da liquidação manual.");
        return false;
    }
    else if (codigoContaCorrente == '' || codigoContaCorrente == '-1' || codigoContaCorrente == "null" || codigoContaCorrente == null) {
        valido = false;
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
        return false;
    }

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $("#ValorRecebido").prop('disabled', false);

    var data = "codigoEmissaoBoleto=" + CodFatura
        + "&ValorDescontoConcedido=" + $("#ValorDescontoConcedido").val()
        + "&JurosCobrado=" + $("#JurosCobrado").val()
        + "&MultaCobrada=" + $("#MultaCobrada").val()
        + "&ValorRecebido=" + $("#ValorRecebido").val()
        + "&codigoContaCorrente=" + codigoContaCorrente
        + "&codigoEmpresa=" + $('.codigoEmpresa option:selected').val()
        + "&codJustificativa=" + codJustificativa
        + "&DescJustOutros=" + DescJustOutros
        + "&descJustificativa=" + descJustificativa


    var _url = montaUrl("/ContasReceber/ConsultarBoletos/LiquidacaoManual/");

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {


                var TableTitulos = $('#gridConsultaFaturamento').DataTable()

                var trs = $(TableTitulos.cells().nodes()).parent();
                $(trs).each(function (index, value) {

                    if ($("#codigoEmissaoBoleto").val() == $(value).find('.codEmissaoBoleto').find('input').val()) {

                        $(value).find('.inpValor')[0].innerHTML = $("#ValorPago").val();
                        $(value).find('.inpStatus')[0].innerHTML = "LIQUIDADO MANUALMENTE";
                    }

                });

                $("#JanelaAcoes .close").click();
                msgSucesso(data.responseText)

            }
            else {
                msgErro(data.responseText)
            }
        }
    });

}
function EnviaDescritivoValoresCobrados(codEmissaoBoleto, EmailCliente) {
    var data = new Date();
    var nomeArquivo = codEmissaoBoleto + data.getDate() + data.getMonth();

    var url = montaUrl("/ContasReceber/ConsultarBoletos/CriaDetalhamentoFaturaReembolso/");
    url += "&codEmissaoBoleto=" + codEmissaoBoleto
    url += "&nomeArquivo=" + nomeArquivo

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });
    //Processo de criação do arquivo PDF
    $.ajax({
        type: "Post",
        url: url,
        success: function (data) {
            $.unblockUI();
            $.confirm({
                title: "Confirmar",
                icon: "glyphicon glyphicon-alert",
                content: "Confirma o envio do detalhamento da cobranca para o email cadastrado na conta do cliente?",
                type: 'green',
                typeAnimated: true,
                buttons: {
                    sim: {
                        text: 'Sim',
                        btnClass: 'btn-blue',
                        action: function () {

                            $.blockUI({ message: '<div class="ModalCarregando"></div>' });

                            var url = montaUrl("/ContasReceber/ConsultarBoletos/EnviarDetalhamentoFaturaEmail/");
                            url += "&nomeArquivo=" + nomeArquivo
                            url += "&codEmissaoBoleto=" + codEmissaoBoleto
                            url += "&email=" + EmailCliente

                            //Processo para Enviar o Arquivo por email
                            $.ajax({
                                type: "Post",
                                url: url,
                                success: function (data) {
                                    $.unblockUI();
                                    if (data.success) {
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
                    outroEmail: {
                        text: 'Enviar para outro email',
                        btnClass: 'btn-default',
                        action: function () {
                            $.confirm({
                                title: 'Email Para Envio!',
                                content: '' +
                                    '<form action="" class="formName">' +
                                    '<div class="form-group">' +
                                    '<label>Informe o email que irá receber o detalhamento da cobranca</label>' +
                                    '<input type="text" placeholder="email@provedor.com.br" class="email form-control" required />' +
                                    '</div>' +
                                    '</form>',
                                buttons: {
                                    formSubmit: {
                                        text: 'Enviar',
                                        btnClass: 'btn-blue',
                                        action: function () {
                                            var email = this.$content.find('.email').val();
                                            if (!email) {
                                                $.alert('Email não informado!');
                                                return false;
                                            }

                                            $.blockUI({ message: '<div class="ModalCarregando"></div>' });
                                            //Processo de criação do arquivo PDF

                                            var url = montaUrl("/ContasReceber/ConsultarBoletos/EnviarDetalhamentoFaturaEmail/");
                                            url += "&nomeArquivo=" + nomeArquivo
                                            url += "&codEmissaoBoleto=" + codEmissaoBoleto
                                            url += "&email=" + email

                                            //Processo para Enviar o Arquivo por email
                                            $.ajax({
                                                type: "Post",
                                                url: url,
                                                success: function (data) {
                                                    $.unblockUI();
                                                    if (data.success) {
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
                                    cancel: function () {
                                        //close
                                    },
                                },
                                onContentReady: function () {
                                    // bind to events
                                    var jc = this;
                                    this.$content.find('form').on('submit', function (e) {
                                        // if the user submits the form by pressing enter in the field.
                                        e.preventDefault();
                                        jc.$$formSubmit.trigger('click'); // reference the button and click it
                                    });
                                }
                            });
                        },

                    },
                    download: {
                        text: 'download pdf!', // With spaces and symbols
                        btnClass: 'btn-green',
                        action: function () {
                            window.location.href = (montaUrl("/ContasReceber/ConsultarBoletos/DownloadDetalhamentoFatura/?nomeArquivo=" + nomeArquivo));
                        }
                    },
                    nao: {
                        text: 'Cancelar',
                        btnClass: 'btn-red',
                        action: function () { }
                    }

                }
            });


        },
        error: function (data) {
            $.unblockUI();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });


}
function ParcelarFatura(codEmissaoBoleto) {

    var url = montaUrl("/ContasReceber/ConsultarBoletos/ParcelarFatura/");

    url += "&CodFatura=" + codEmissaoBoleto
    url += "&codEmpresa=" + $('.codigoEmpresa option:selected').val()

    $("#modalJanelaAcoes").load(url);
    $("#JanelaAcoes").modal();

}


function CalcularParcelamentoFatura() {

    $('#modalListaParcelas').empty();

    var dt = $("#Parcela_dataPrimeiraParcela").val().replace("/", "_");


    var url = montaUrl("/ContasReceber/ConsultarBoletos/CalculaParcelasFaturamentos/?valor=" + $("#Parcela_ValorOriginal").val()
        + "&qtParcela=" + $("#Parcela_qtParcelas").val()
        + "&primeiraParcela=" + dt
        + "&taxaMes=" + $("#Parcela_TaxaMensal").val())


    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modalListaParcelas").load(url);
    $("#ListaParcelas").show();

};
function SalvarParcelamentoFatura() {

    var codigoEmpresa = $('.codigoEmpresa').find("option:selected").val();

    var table = $('#gridListaParcelasFatura').DataTable();
    var meuData = '';
    var i = 0;
    var trs = $(table.cells().nodes()).parent()
    $(trs).each(function (index, value) {
        meuData += '&ListaParcelas%5B' + i + '%5D.NroParcela=' + $(value).find('.NroParcela').find('input').val();
        meuData += '&ListaParcelas%5B' + i + '%5D.Juros=' + $(value).find('.Juros').find('input').val();
        meuData += '&ListaParcelas%5B' + i + '%5D.ValorParcela=' + $(value).find('.ValorParcela').find('input').val();
        meuData += '&ListaParcelas%5B' + i + '%5D.VencimentoParcela=' + $(value).find('.VencimentoParcela').find('input').val();
        i = i + 1;

    });

    var data = "&Parcela_codEmpresa=" + codigoEmpresa
        + "&Parcela_codFaturamento=" + $("#Parcela_codFaturamento").val()
        + "&Parcela_qtParcelas=" + $("#Parcela_qtParcelas").val()
        + "&Parcela_TaxaMensal=" + $("#Parcela_TaxaMensal").val()
        + meuData

    $("#JanelaAcoes .close").click();

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });


    var _url = montaUrl("/ContasReceber/ConsultarBoletos/SalvarParcelamentoFatura/");

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText)

            }
            else {
                msgErro(data.responseText)
            }
        }
    });
}