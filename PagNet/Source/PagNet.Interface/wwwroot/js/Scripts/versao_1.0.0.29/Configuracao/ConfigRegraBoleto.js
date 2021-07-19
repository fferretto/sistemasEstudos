
function carregaPagina() {

    if ($("#CODREGRA").val() == '0') {
        $("#DesativarRegra").prop('disabled', true);
    }

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    if ($('#teraMulta').val() == "True") {
        $("#btnMultaSim").click();

        if ($('#valorMulta').val() != " 0,00") {
            $("#btnMultaValor").click();
            $("#liMultaValor").addClass('active');
        }
        else if ($('#PercMulta').val() != " 0,00"){
            $("#btnMultaPerc").click();
            $("#liMultaPerc").addClass('active');
        }
    }

    if ($('#AgruparFaturamentosDia').val() == "True") {
        $("#btnAgruparCobrancaSim").click();
        $("#liAgruparCobrancaNao").removeClass('active');
        $("#liAgruparCobrancaSim").addClass('active');
    }


    if ($('#teraJuros').val() == "True") {
        $("#btnJurosSim").click();

        if ($('#ValorJuros').val() != " 0,00") {
            $("#btnJurosValor").click();
            $("#liJurosValor").addClass('active');
        }
        else if ($('#PercJuros').val() != " 0,00") {
            $("#btnJurosPerc").click();
            $("#liJurosPerc").addClass('active');
        }
    }

}

$(window).load(carregaPagina());


$("#btnAgruparCobrancaNao").click(function () {
    $("#AgruparFaturamentosDia").val('false');
})

$("#btnAgruparCobrancaSim").click(function () {
    $("#AgruparFaturamentosDia").val('true');
})

$("#btnMultaNao").click(function () {
    $("#valorMulta").val('');
    $("#PercMulta").val('');
    $("#teraMulta").val('false');
})

$("#btnMultaSim").click(function () {
    $("#teraMulta").val('true');
})
$("#btnMultaValor").click(function () {
    $("#PercMulta").val('');
    $("#liMultaValor").removeClass('active');
    $("#liMultaPerc").removeClass('active');
    $("#liMultaValor").addClass('active');
})

$("#btnMultaPerc").click(function () {
    $("#valorMulta").val('');
})

$("#btnJurosNao").click(function () {
    $("#ValorJuros").val('');
    $("#PercJuros").val('');
    $("#teraJuros").val('false');
})

$("#btnJurosSim").click(function () {
    $("#teraJuros").val('true');
})


$("#btnJurosValor").click(function () {
    $("#PercJuros").val('');
})

$("#btnJurosPerc").click(function () {
    $("#ValorJuros").val('');
})

$(".codigoPrimeiraInscricaoCobraca").change(function () {
    $("#codigoPrimeiraInscricaoCobraca").val($('.codigoPrimeiraInscricaoCobraca option:selected').val());

});

$(".codigoSegundaInscricaoCobraca").change(function () {
    $("#codigoSegundaInscricaoCobraca").val($('.codigoSegundaInscricaoCobraca option:selected').val());

});

function JustificativaDesativarRegra() {
    
    var url = montaUrl("/Configuracao/ConfigRegraBoleto/Justificativa")

    $("#modalJustificativa").load(url);
    $("#Justificativa").modal();

}

function SalvarRegra() {
    var Valido = true;


    if (Valido) {
        var form = $('form', '#page-inner');

        var url = montaUrl("/Configuracao/ConfigRegraBoleto/SalvaRegra/");

        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

        form.attr('action', url);
        form.submit();
    }

}
function ConfirmaDesativarRegra() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo.");
        return false;
    }


    var data = "CODREGRA=" + $("#CODREGRA").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()
    
    var url = montaUrl("/Configuracao/ConfigRegraBoleto/DesativarRegra");

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                $("#Justificativa .close").click();
                msgSucessoWithLoad(data.responseText);

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
