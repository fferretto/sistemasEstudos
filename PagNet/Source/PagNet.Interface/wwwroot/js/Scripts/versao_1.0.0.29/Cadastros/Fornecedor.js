function carregaPagina() {
    if ($("#codigoFavorecido").val() == '0') {
        $("#btnLocalizaLog").attr('disabled', 'disabled');
    }

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    if ($('#regraDiferenciada').val() == "True") {
        $("#btnRegraPadraoSim").click();
        $("#liRegraPadraoNao").removeClass('active');
        $("#liRegraPadraoSim").addClass('active');
    }

    if ($('#contaPagamentoPadrao').val() == "True") {
        $("#btnContaCorrenteSim").click();
        $("#liContaCorrenteNao").removeClass('active');
        $("#liContaCorrenteSim").addClass('active');
    }


    $('#LocalizandoEndereco').hide();    

}
$(window).load(carregaPagina);


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});

$("#btnRegraPadraoSim").click(function () {
    $("#regraDiferenciada").val('True');
})
$("#btnRegraPadraoNao").click(function () {
    $("#regraDiferenciada").val('False');
})
$("#btnContaCorrenteSim").click(function () {
    $("#contaPagamentoPadrao").val('True');
})
$("#btnContaCorrenteNao").click(function () {
    $("#contaPagamentoPadrao").val('False');
})
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


$(".CEP").change(function () {

    var _url = montaUrl("/Cadastros/Fornecedor/BuscaEndereco");
    var data = "cpf=" + $("#CEP").val();
    $('#LocalizandoEndereco').show();
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            if (data.endereco != null) {

                $('#Logradouro').val(data.endereco);
                $('#Bairro').val(data.localidadeBairroDescricao);
                $('#cidade').val(data.localidadeMunicipioDescricao);
                $('#UF').val(data.localidadeUfDescricao);
                $('#LocalizandoEndereco').hide();

                //$('#LOGRADOURO').prop('disabled', true);
                //$('#BAIRRO').prop('disabled', true);
                //$('#CIDADE').prop('disabled', true);
                //$('#UF').prop('disabled', true);
            }
            else {

                $('#Logradouro').val("");
                $('#Bairro').val("");
                $('#cidade').val("");
                $('#UF').val("");

                //$('#LOGRADOURO').prop('disabled', false);
                //$('#BAIRRO').prop('disabled', false);
                //$('#CIDADE').prop('disabled', false);
                //$('#UF').prop('disabled', false);

                $('#LocalizandoEndereco').hide();
            }
        },
        error: function (data) {
            $('#LocalizandoEndereco').hide();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });
});

$(".Banco").change(function () {

    var codBanco = parseInt($("#Banco").val());
    if (codBanco == 104) {
        $('#Operacao').prop('disabled', false);
    }
    else {
        $('#Operacao').prop('disabled', true);
    }

});

function LocalizaFavorito() {

    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var url = montaUrl("/Cadastros/Fornecedor/ConsultaFavorecidos")
    url += "&codigoEmpresa=" + $("#codigoEmpresa").val()

    $("#modal").load(url);
    $("#Localizar").modal();
}

function LocalizaLog() {
    var url = montaUrl("/Cadastros/Fornecedor/LocalizaLog")
    url += "&codigoFavorecido=" + $("#codigoFavorecido").val()

    $("#modalLog").load(url);
    $("#LocalizarLog").modal();
}

function ConfirmaSalvar() {

    var form = $('form', '#page-inner');

    $('#Logradouro').prop('disabled', false);
    $('#Bairro').prop('disabled', false);
    $('#cidade').prop('disabled', false);
    $('#UF').prop('disabled', false);

    var url = montaUrl("/Cadastros/Fornecedor/Salvar/");

    form.attr('action', url);
    form.submit();

}