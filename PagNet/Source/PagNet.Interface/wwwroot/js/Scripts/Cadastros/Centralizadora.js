function carregaPagina() {

    if ($("#codigoFavorecido").val() == '0') {
        $("#btnLocalizaLog").attr('disabled', 'disabled');
    }

    if ($('#codigoFavorecido').val() > 0) {
        $('#btnSalvar').show();
        $('#btnDesativar').show();
    }


    if ($('#regraDiferenciada').val() == "True") {
        $("#btnPagSim").click();
        $("#liPagNao").removeClass('active');
        $("#liPagSim").addClass('active');

    }

    if ($('#contaPagamentoPadrao').val() == "True") {
        $("#btnContaCorrenteSim").click();
        $("#liContaCorrenteNao").removeClass('active');
        $("#liContaCorrenteSim").addClass('active');
    }


    $('#LocalizandoEndereco').hide();


}
$(window).load(carregaPagina);

$("#btnPagSim").click(function () {
    $("#regraDiferenciada").val('True');
})
$("#btnPagNao").click(function () {
    $("#regraDiferenciada").val('False');

    $("#btnTaxaTEDNao").click();
    $("#liTaxaTEDSim").removeClass('active');
    $("#liTaxaTEDNao").addClass('active');
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

    var _url = montaUrl("/Cadastros/Centralizadora/BuscaEndereco");
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

            }
            else {

                $('#Logradouro').val("");
                $('#Bairro').val("");
                $('#cidade').val("");
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

    var url = montaUrl("/Cadastros/Centralizadora/ConsultaCentralizadora");

    $("#modal").load(url);
    $("#Localizar").modal();
}

function ConsultaLog() {
    var url = montaUrl("/Cadastros/Centralizadora/LocalizaLog")
    url += "&codigoFavorecido=" + $("#codigoFavorecido").val()

    $("#modalLog").load(url);
    $("#ConsultLog").modal();
}

function ConfirmaSalvar() {

    var form = $('form', '#page-inner');


    $('#Logradouro').prop('disabled', false);
    $('#Bairro').prop('disabled', false);
    $('#cidade').prop('disabled', false);
    $('#UF').prop('disabled', false);


    var url = montaUrl("/Cadastros/Centralizadora/Salvar/");

    form.attr('action', url);
    form.submit();

}