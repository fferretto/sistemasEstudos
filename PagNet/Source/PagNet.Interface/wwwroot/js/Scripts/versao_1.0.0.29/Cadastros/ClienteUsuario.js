
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();

        if ($("#CODCLIENTE").val() == '0') {
            $("#DesativarCliente").prop('disabled', true);
        }

        if ($('#COBRANCADIFERENCIADA').val() == "True") {

            $("#btnRegraDefaultNao").click();
            $("#liRegraDefaultSim").removeClass('active');
            $("#liRegraDefaultNao").addClass('active');

            if ($('#COBRAMULTA').val() == "True") {
                $("#btnMultaSim").click();

                if ($('#VLMULTADIAATRASO').val() != " 0,00") {
                    console.log($('#VLMULTADIAATRASO').val())
                    $("#btnMultaValor").click();
                    $("#liMultaValor").addClass('active');
                }
                else if ($('#PERCMULTA').val() != " 0,00") {
                    $("#btnMultaPerc").click();
                    $("#liMultaPerc").addClass('active');
                }
            }

            if ($('#COBRAJUROS').val() == "True") {
                $("#btnJurosSim").click();

                if ($('#VLJUROSDIAATRASO').val() != " 0,00") {
                    $("#btnJurosValor").click();
                    $("#liJurosValor").addClass('active');
                }
                else if ($('#PERCJUROS').val() != " 0,00") {
                    $("#btnJurosPerc").click();
                    $("#liJurosPerc").addClass('active');
                }
            }
            if ($('#AgruparCobranca').val() == "True") {
                $("#btnAgruparCobrancaSim").click();
                $("#liAgruparCobrancaNao").removeClass('active');
                $("#liAgruparCobrancaSim").addClass('active');
            }
        }

    }
}

$(window).load(carregaPagina());


$("#btnAgruparCobrancaNao").click(function () {
    $("#AgruparCobranca").val('false');
})

$("#btnAgruparCobrancaSim").click(function () {
    $("#AgruparCobranca").val('true');
})

$("#btnRegraDefaultNao").click(function () {
    $("#COBRANCADIFERENCIADA").val('true');
})

$("#btnRegraDefaultSim").click(function () {
    $("#COBRANCADIFERENCIADA").val('false');
    $("#VLMULTADIAATRASO").val('');
    $("#PERCMULTA").val('');
    $("#COBRAMULTA").val('false');

    $("#VLJUROSDIAATRASO").val('');
    $("#PERCJUROS").val('');
    $("#COBRAJUROS").val('false');

    $("#liJurosValor").removeClass('active');
    $("#liJurosPerc").removeClass('active');

    $("#liMultaValor").removeClass('active');
    $("#liMultaPerc").removeClass('active');
})


$("#btnMultaNao").click(function () {
    $("#VLMULTADIAATRASO").val('');
    $("#PERCMULTA").val('');
    $("#COBRAMULTA").val('false');
})

$("#btnMultaSim").click(function () {
    $("#COBRAMULTA").val('true');
})
$("#btnMultaValor").click(function () {
    $("#PERCMULTA").val('');
})

$("#btnMultaPerc").click(function () {
    $("#VLMULTADIAATRASO").val('');
})

$("#btnJurosNao").click(function () {
    $("#VLJUROSDIAATRASO").val('');
    $("#PERCJUROS").val('');
    $("#COBRAJUROS").val('false');
})

$("#btnJurosSim").click(function () {
    $("#COBRAJUROS").val('true');
})


$("#btnJurosValor").click(function () {
    $("#PERCJUROS").val('');
})

$("#btnJurosPerc").click(function () {
    $("#VLJUROSDIAATRASO").val('');
})

$(".CODEMPRESA").change(function () {
    $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());
});

$(".CODPRIMEIRAINSTCOBRA").change(function () {
    $("#CODPRIMEIRAINSTCOBRA").val($('.CODPRIMEIRAINSTCOBRA option:selected').val());

});

$(".CODSEGUNDAINSTCOBRA").change(function () {
    $("#CODSEGUNDAINSTCOBRA").val($('.CODSEGUNDAINSTCOBRA option:selected').val());

});

$(".codigoFormaLiquidacao").change(function () {
    $("#codigoFormaLiquidacao").val($('.codigoFormaLiquidacao option:selected').val());
});
$(".codigoFormaFaturamento").change(function () {
    $("#codigoFormaFaturamento").val($('.codigoFormaFaturamento option:selected').val());
});

function JustificativaDesativarCliente() {

    var url = montaUrl("/Cadastros/ClienteUsuario/Justificativa")

    $("#modalJustificativa").load(url);
    $("#Justificativa").modal();

}

function ConfirmaSalvar() {
    var Valido = true;


    if (Valido) {
        var form = $('form', '#page-inner');

        var url = montaUrl("/Cadastros/ClienteUsuario/Salvar/");

        form.attr('action', url);
        form.submit();
    }

}
function JustificativaDesativarCliente() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da baixa.");
        return false;
    }


    var data = "CODCLIENTE=" + $("#CODCLIENTE").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()


    var url = montaUrl("/Cadastros/ClienteUsuario/DesativaCliente");
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

$("#CPFCNPJ").keydown(function () {
    try {
        $("#CPFCNPJ").unmask();
    } catch (e) { }

    $('#CPFCNPJ').val($('#CPFCNPJ').val().normalize('NFD').replace(/([\u0300-\u036f]|[^0-9a-zA-Z])/g, ''));
    var tamanho = $("#CPFCNPJ").val().length;

    if (tamanho > 0) {
        if (tamanho < 11) {
            $("#CPFCNPJ").mask("999.999.999-99");
        } else if (tamanho >= 11) {
            $("#CPFCNPJ").mask("99.999.999/9999-99");
        }
    }

    // ajustando foco
    var elem = this;
    setTimeout(function () {
        // mudo a posição do seletor
        elem.selectionStart = elem.selectionEnd = 10000;
    }, 0);
    // reaplico o valor para mudar o foco
    var currentValue = $(this).val();
    $(this).val('');
    $(this).val(currentValue);
});
$('#CPFCNPJ').focusout(function () {

    var CPFCNPJ = $('#CPFCNPJ').val();

    if (CPFCNPJ != "") {
        try {
            $("#CPFCNPJ").unmask();
        } catch (e) { }

        CPFCNPJ = CPFCNPJ.normalize('NFD').replace(/([\u0300-\u036f]|[^0-9a-zA-Z])/g, '');
        if (CPFCNPJ.length == 14) {
            if (!validarCNPJ(CPFCNPJ)) {
                $('#CPFCNPJ').addClass("borderError");
                addErros("CPFCNPJ", "CPF/CNPJ Inválido!")
            }
            else {
                $('#CPFCNPJ').val(CPFCNPJ);
                $("#CPFCNPJ").mask("99.999.999/9999-99");
                $('#CPFCNPJ').removeClass("borderError");
                $("span[data-valmsg-for='CPFCNPJ']").hide();
            }
        }
        else if (CPFCNPJ.length == 11) {
            if (!validarCPF(CPFCNPJ)) {
                $('#CPFCNPJ').addClass("borderError");
                addErros("CPFCNPJ", "CPF/CNPJ Inválido!")
            }
            else {
                $('#CPFCNPJ').val(FormataCPF(CPFCNPJ))
                $('#CPFCNPJ').removeClass("borderError");
                $("span[data-valmsg-for='CPFCNPJ']").hide();
            }
        }
        else {
            $('#CPFCNPJ').addClass("borderError");
            addErros("CPFCNPJ", "CPF/CNPJ Inválido!")
        }

    }
})


function LocalizaCliente() {

    var url = montaUrl("/Cadastros/ClienteUsuario/ConsultaCliente");
    url += "&codEmpresa=" + $("#CODEMPRESA").val();

    $("#modal").load(url);
    $("#Localizar").modal();
}


$(".CEP").change(function () {

    var _url = montaUrl("/Generico/ConsultasGenericas/BuscaEndereco");
    var data = "cpf=" + $("#CEP").val();
    $('#LocalizandoEndereco').show();
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            if (data.endereco != null) {

                $('#LOGRADOURO').val(data.endereco);
                $('#BAIRRO').val(data.localidadeBairroDescricao);
                $('#CIDADE').val(data.localidadeMunicipioDescricao);
                $('#UF').val(data.localidadeUfDescricao);
                $('#LocalizandoEndereco').hide();

            }
            else {

                $('#LOGRADOURO').val("");
                $('#BAIRRO').val("");
                $('#CIDADE').val("");
                $('#UF').val("");

                $('#LocalizandoEndereco').hide();
            }
        },
        error: function (data) {
            $('#LocalizandoEndereco').hide();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });
});