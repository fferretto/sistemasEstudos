function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
    if ($('#codigoEmissaoBoleto').val() > 0 && $('#Status').val() != 'EM ABERTO') {
        if ($('#codigoFormaFaturamento').val() == 1 && ($('#Status').val() == 'REGISTRADO' || $('#Status').val() == 'PENDENTE REGISTRO')) {
            $('#edicaoboleto').show();
        }

        $('#filtroCliente').prop('disabled', true);
        $('#btnLocalizarCliente').hide();

    }

    if ($('#codigoEmissaoBoleto').val() > 0) {
        if ($('#Origem').val() == 'PAGNET') {
            $('#btnExcluir').show();
        }
        $('#Valor').prop('disabled', true);
    }

    if ($("#codigoFormaFaturamento").val() == '1') {
        $("#TipoBoleto").show();
    }
    else {
        $("#TipoBoleto").hide();
    }

    if ($('#CodigoCliente').val() != null && $('#CodigoCliente').val() > 0) {

        $("#DadosCliente").show();
    }

    $('#nomeCliente').prop('disabled', true);

}
$(window).load(carregaPagina);


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    $("#filtroCliente").val();
    $("#CodigoCliente").val();
    $("#nomeCliente").val();

});

$(".codigoFormaFaturamento").change(function () {
    $("#codigoFormaFaturamento").val($('.codigoFormaFaturamento option:selected').val());

    if ($("#codigoFormaFaturamento").val() == '1') {
        $("#TipoBoleto").show();
    }
    else {
        $("#TipoBoleto").hide();
    }
});

$(".codigoOcorrencia").change(function () {
    $("#codigoOcorrencia").val($('.codigoOcorrencia option:selected').val());
});
$(".CodigoPlanoContas").change(function () {
    $("#CodigoPlanoContas").val($('.CodigoPlanoContas option:selected').val());
});

$('#filtroCliente').focusout(function () {

    if ($('#filtroCliente').val() != "") {
        var filtro = '?filtro=' + $('#filtroCliente').val() + '&codempresa=' + $('.codigoEmpresa option:selected').val()

        var url = montaUrl('/ContasReceber/IncluirNovoBoleto/BuscarCliente/' + filtro);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                if (data.codcliente != null && data.codcliente > 0) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);

                    $("#DadosCliente").show();
                    $("#TaxaEmissaoBoleto").val("R$ " + data.taxaemissaoboleto);
                    $("#CPFCNPJCliente").val(data.cpfcnpj);
                    $("#EmailCliente").val(data.email);
                    $("#CobraJuros").val(((data.cobrajuros) ? "Sim" : "Não"));
                    $("#ValorJuros").val("R$ " + data.vljurosdiaatraso);
                    $("#PercentualJuros").val(data.percjuros + "%");
                    $("#CobraMulta").val(((data.cobramulta) ? "Sim" : "Não"));
                    $("#ValorMulta").val("R$ " + data.vlmultadiaatraso);
                    $("#PercentualMulta").val(data.percmulta + "%");



                } else {
                    $("#nomeCliente").val("Cliente não encontrado");
                    //addErros("filtroCliente", "Cliente não encontrado");
                    $("#DadosCliente").hide();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("filtroCliente", "Erro interno. Tente novamente mais tarde.");
                $("#DadosCliente").hide();
            }
        });
    }
    else {
        $("#filtroCliente").val('');
        $("#CodigoCliente").val('');
        $("#nomeCliente").val('');
        $("#DadosCliente").hide();
    }
});

function LocalizaBoletosCriados() {

    var filtro = '?codempresa=' + $('.codigoEmpresa option:selected').val()
    var url = montaUrl("/ContasReceber/IncluirNovoBoleto/BustaTodosBoletosParaEdicao/" + filtro)

    $("#modalLocalizaBoletos").load(url);
    $("#LocalizaBoletos").modal();
}
function LocalizaCliente() {

    var filtro = '?codempresa=' + $('.codigoEmpresa option:selected').val()
    var url = montaUrl("/ContasReceber/IncluirNovoBoleto/ConsultaTodosClientes/" + filtro)

    $("#modalCliente").load(url);
    $("#LocalizaCliente").modal();
}

function SelecionaCliente(codCliente, codigoEmpresa) {

    if (codCliente != "") {
        var filtro = '?filtro=' + codCliente + '&codempresa=' + codigoEmpresa

        var url = montaUrl('/ContasReceber/IncluirNovoBoleto/BuscarCliente/' + filtro);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#LocalizaCliente .close").click();
                if (data != null) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);

                    $("#DadosCliente").show();

                    $("#CPFCNPJCliente").val(data.cpfcnpj);
                    $("#EmailCliente").val(data.email);
                    $("#CobraJuros").val(data.cobrajuros);
                    $("#ValorJuros").val(data.vljurosdiaatraso);
                    $("#PercentualJuros").val(data.percjuros);
                    $("#CobraMulta").val(data.cobramulta);
                    $("#ValorMulta").val(data.vlmultadiaatraso);
                    $("#PercentualMulta").val(data.percmulta);



                } else {
                    addErros("filtroCliente", "Cliente não encontrado");
                    $("#DadosCliente").hide();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("filtroCliente", "Erro interno. Tente novamente mais tarde.");
                $("#DadosCliente").hide();
            }
        });
    }
    else {
        $("#filtroCliente").val('');
        $("#CodigoCliente").val('');
        $("#nomeCliente").val('');
        $("#DadosCliente").hide();
    }

}
$(document).on("input", "#MensagemArquivoRemessa", function () {
    var limite = 40;
    var caracteresDigitados = $(this).val().length;
    var caracteresRestantes = limite - caracteresDigitados;

    $(".caracteresRemessa").text(caracteresRestantes);
});

$(document).on("input", "#MensagemInstrucoesCaixa", function () {
    var limite = 40;
    var caracteresDigitados = $(this).val().length;
    var caracteresRestantes = limite - caracteresDigitados;

    $(".caracteresCaixa").text(caracteresRestantes);
});

function SalvarBoleto() {
    var Valido = true;

    $('#filtroCliente').prop('disabled', false);
    $('#Valor').prop('disabled', false);

    if ($("#dataVencimento").val() != "") {
        var data = new Date();
        var dtInicio = $("#dataVencimento").val().split("/");
        var dataIni = new Date(`${dtInicio[2]},${dtInicio[1]},${dtInicio[0]}`);
        var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)

        if (dataIni < dataAtual) {
            addErros("dataVencimento", "A data de vencimento não pode ser inferior a data atual.");
            return false;
            valido = false;
        }
    }

    var codigoFormaFaturamento = $('.codigoFormaFaturamento option:selected').val()
    
    if (codigoFormaFaturamento == "-1" || codigoFormaFaturamento == "" || codigoFormaFaturamento == "null") {
        msgAviso("Obrigatório informar a forma que será faturado este pedido.");
        valido = false;
        return false;
    }

    var CodigoPlanoContas = $('.CodigoPlanoContas option:selected').val()

    if (CodigoPlanoContas == "-1" || CodigoPlanoContas == "" || CodigoPlanoContas == "null") {
        msgAviso("Obrigatório informar o Plano de contas que será utilizado.");
        valido = false;
        return false;
    }
    

    if (Valido) {

        $.blockUI({ message: '<div class="ModalCarregando"></div>' });

        var form = $('form', '#page-inner');

        var url = montaUrl("/ContasReceber/IncluirNovoBoleto/Salvar/");

        form.attr('action', url);
        form.submit();
    }

}
function SalvarEdicao(codJustificativa, descJustificativa, DescJustOutros) {

    $('#codJustificativa').val(codJustificativa)
    $('#descJustificativa').val(descJustificativa);
    $('#DescJustOutros').val(DescJustOutros);


    $("#JanelaModal .close").click();
       
    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    var form = $('form', '#page-inner');

    var url = montaUrl("/ContasReceber/IncluirNovoBoleto/Salvar/");

    form.attr('action', url);
    form.submit();
}

function JustificarCancelamentoPedidoFaturamento() {

    var url = montaUrl("/ContasReceber/IncluirNovoBoleto/JustificarCancelamentoPedidoFaturamento")

    $("#modalJanelaModal").load(url);
    $("#JanelaModal").modal();


}
function CancelarPedidoFaturamento(codJustificativa, descJustificativa, DescJustOutros) {

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    var data = "codigoEmissaoBoleto=" + $('#codigoEmissaoBoleto').val()
        + "&codJustificativa=" + codJustificativa
        + "&DescJustOutros=" + DescJustOutros
        + "&descJustificativa=" + descJustificativa


    var _url = montaUrl("/ContasReceber/IncluirNovoBoleto/CancelarFatura/");

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                $.confirm({
                    title: "Sucesso",
                    icon: "glyphicon glyphicon-ok",
                    content: data.responseText,
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        tryAgain: {
                            text: 'OK',
                            btnClass: 'btn-green',
                            action: function () {
                                location.reload();
                            }
                        }
                    }
                });
                
            }
            else {
                msgErro(data.responseText)
            }
        }
    });
       
}
function LiquidacaoManual() {

    var form = $('form', '#page-inner');

    var url = montaUrl("/ContasReceber/IncluirNovoBoleto/LiquidacaoManual/");

    form.attr('action', url);
    form.submit();


}