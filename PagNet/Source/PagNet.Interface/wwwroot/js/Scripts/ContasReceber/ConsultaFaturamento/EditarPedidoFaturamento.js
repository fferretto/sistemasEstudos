

$(document).ready(function () {
    inicia();

    $("#CodigoClienteConsulta").prop('disabled', true);
    $("#nomeClienteConsulta").prop('disabled', true);
    $("#nomeEmpresa").prop('disabled', true);
    $("#Valor").prop('disabled', true);
    $("#ValorRecebido").prop('disabled', true);

    if ($('.codigoFormaFaturamento option:selected').val() == '1') {
        $("#TipoBoleto").show();
    }
    else {
        $("#TipoBoleto").hide();
    }

    $(".DescJustOutros").hide();
    $(".codJustificativa").change(function () {
        $("#codJustificativa").val($('.codJustificativa option:selected').val());
        if ($("#codJustificativa").val() == "OUTROS") {
            $(".DescJustOutros").show();
        }
        else {
            $(".DescJustOutros").hide();
            $("#DescJustOutros").val("");
        }
    });



    $("#dataVencimento").change(function () {

        var data = new Date();
        var dtInicio = $("#dataVencimento").val().split("/");
        var dataIni = new Date(`${dtInicio[2]},${dtInicio[1]},${dtInicio[0]}`);
        var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)

        if (dataIni < dataAtual) {
            valido = false;
            msgAviso("A data de vencimento não pode ser inferior a data atual.");
        }

    });
    $(".CodigoPlanoContas").change(function () {
        $("#CodigoPlanoContas").val($('.CodigoPlanoContas option:selected').val());
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

    $('.umadata input').datepicker({
        format: "dd/mm/yyyy",
        language: "pt-BR",
        orientation: "auto",
        autoclose: true
    });


});

function CalculaValorTotalRecebido() {

    var ValorPrevisto = ConvertDecimal($("#valorPrevistoRecebimento").val())
    var ValorDesconto = ConvertDecimal($("#ValorDescontoConcedido").val())
    var ValorMulta = ConvertDecimal($("#JurosCobrado").val())
    var ValorJuros = ConvertDecimal($("#MultaCobrada").val())

    var valorTotal = (ValorPrevisto + ValorMulta + ValorJuros) - (ValorDesconto)

    $("#ValorRecebido").val(FormataMoedaReal(valorTotal));
}


$(".umValorMonetario").on('click', function () {
    if ($(this).val() == "0,00") {
        $(this).val("")
    }
});

$('.umValorMonetario').focusout(function () {
    if ($(this).val() == "") {
        $(this).val("0,00")
    }

    CalculaValorTotalRecebido();
});

function SalvarEdicao() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    var DescJustOutros = $('#DescJustOutros').val();
    $("#CodigoPlanoContas").val($('.CodigoPlanoContas option:selected').val());

    if (codJustificativa == "-1" || codJustificativa == "" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar da edição deste pedido faturamento.");
        return false;
    }

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    var data = "codigoEmissaoBoleto=" + $("#codigoEmissaoBoleto").val()
        + "&dataVencimento=" + $("#dataVencimento").val()
        + "&Valor=" + $("#Valor").val()
        + "&nroDocumento=" + $("#nroDocumento").val()
        + "&valorDesconto=" + $("#valorDesconto").val()
        + "&dataSegundoDesconto=" + $("#dataSegundoDesconto").val()
        + "&valorSegundoDesconto=" + $("#valorSegundoDesconto").val()
        + "&MensagemArquivoRemessa=" + $("#MensagemArquivoRemessa").val()
        + "&MensagemInstrucoesCaixa=" + $("#MensagemInstrucoesCaixa").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + DescJustOutros
        + "&codigoEmpresa=" + $('.codigoEmpresa option:selected').val()
        + "&codigoFormaFaturamento=" + $('.codigoFormaFaturamento option:selected').val()
        + "&JurosCobrado=" + $("#JurosCobrado").val()
        + "&MultaCobrada=" + $("#MultaCobrada").val()
        + "&ValorRecebido=" + $("#ValorRecebido").val()
        + "&CodigoPlanoContas=" + $("#CodigoPlanoContas").val()

    var _url = montaUrl("/ContasReceber/ConsultarBoletos/SalvarAlteracao/");

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

                        $(value).find('.inpdtVencimento')[0].innerHTML = $("#dataVencimento").val()
                        $(value).find('.inpValor')[0].innerHTML = $("#Valor").val()
                        $(value).find('.inpStatus')[0].innerHTML = "A Faturar"
                        $(value).find('.inpnroDocumento')[0].innerHTML = $("#nroDocumento").val()
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
};
