
function carregaPagina() {
    
    if ($("#CODREGRA").val() == '0') {
        $("#DesativarRegra").prop('disabled', true);
    }
    
    if ($('#COBRATAXAANTECIPACAO').val() == "True") {
        $("#btnAntecipaSim").click();

        if ($('#VLTAXAANTECIPACAO').val() != " 0,00") {
            $("#btnAntecipaValor").click();
            $("#liAntecipaValor").addClass('active');
        }
        else if ($('#PORCENTAGEMTAXAANTECIPACAO').val() != " 0,00") {
            $("#btnAntecipaPerc").click();
            $("#liAntecipaPerc").addClass('active');
        }
    }
    if ($('#TIPOFORMACOMPENSACAO').val() == "M") {
        $("#btnCalculoMensal").click();
        $("#liCalculoMensal").addClass('active');
    }
    else if ($('#TIPOFORMACOMPENSACAO').val() == "D") {
        $("#btnCalculoDiario").click();
        $("#liCalculoDiario").addClass('active');
    }
    else if ($('#TIPOFORMACOMPENSACAO').val() == "F") {
        $("#btnCalculoFixo").click();
        $("#liCalculoFixo").addClass('active');
    }
}

$(window).load(carregaPagina());

$("#btnCalculoFixo").click(function () {
    $("#TIPOFORMACOMPENSACAO").val('F');
    $("#liCalculoDiario").removeClass('active');
    $("#liCalculoMensal").removeClass('active');
    $("#liCalculoFixo").addClass('active');
})
$("#btnCalculoMensal").click(function () {
    $("#TIPOFORMACOMPENSACAO").val('M');
    $("#liCalculoDiario").removeClass('active');
    $("#liCalculoFixo").removeClass('active');
    $("#liCalculoMensal").addClass('active');
})
$("#btnCalculoDiario").click(function () {
    $("#TIPOFORMACOMPENSACAO").val('D');
    $("#liCalculoMensal").removeClass('active');
    $("#liCalculoFixo").removeClass('active');
    $("#liCalculoDiario").addClass('active');
})

$("#btnAntecipaNao").click(function () {
    $("#VLTAXAANTECIPACAO").val('');
    $("#PORCENTAGEMTAXAANTECIPACAO").val('');
    $("#COBRATAXAANTECIPACAO").val('false');
})

$("#btnAntecipaSim").click(function () {
    $("#COBRATAXAANTECIPACAO").val('true');
})
$("#btnAntecipaValor").click(function () {
    $("#PORCENTAGEMTAXAANTECIPACAO").val('');
    $("#liAntecipaValor").removeClass('active');
    $("#liAntecipaPerc").removeClass('active');
    $("#liAntecipaValor").addClass('active');
})

$("#btnAntecipaPerc").click(function () {
    $("#VLTAXAANTECIPACAO").val('');
})

function JustificativaDesativarRegra() {

    var url = montaUrl("/Configuracao/ConfigRegraPagamento/Justificativa")

    $("#modalJustificativa").load(url);
    $("#Justificativa").modal();

}

function SalvarRegra() {
    var Valido = true;

    if ($("#TIPOFORMACOMPENSACAO").val() == '') {
        msgAviso('Obrigatório infomrar uma forma de calcular o valor')
        Valido = false;
    }

    if (Valido) {
        var form = $('form', '#page-inner');

        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

        var url = montaUrl("/Configuracao/ConfigRegraPagamento/SalvaRegra/");

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


    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var url = montaUrl("/Configuracao/ConfigRegraPagamento/DesativarRegra");
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
