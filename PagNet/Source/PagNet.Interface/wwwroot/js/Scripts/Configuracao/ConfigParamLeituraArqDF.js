function carregaPagina() {
    
    if ($("#extensaoArquivo").val() != 'TXT') {
        document.getElementsByClassName('posicaoInicialMatricula')[0].children[0].innerText = 'Posição da Coluna'
        document.getElementsByClassName('posicaoInicialCPF')[0].children[0].innerText = 'Posição da Coluna'
        document.getElementsByClassName('posicaoInicialValor')[0].children[0].innerText = 'Posição da Coluna Referente ao Valor'
        $('.posicaoFinalCPF').hide();
        $('.posicaoFinalMatricula').hide();
        $('.posicaoFinalValor').hide();

    }
    else {
        document.getElementsByClassName('posicaoInicialMatricula')[0].children[0].innerText = 'Posição inicial da Matrícula'
        document.getElementsByClassName('posicaoInicialCPF')[0].children[0].innerText = 'Posição inicial do CPF'
        document.getElementsByClassName('posicaoInicialValor')[0].children[0].innerText = 'Posição inicial do Valor'
        $('.posicaoFinalCPF').show();
        $('.posicaoFinalMatricula').show();
        $('.posicaoFinalValor').show();
    }

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
    $("#nomeCliente").prop('disabled', true);
}

$(window).load(carregaPagina());

$("#btnCPF").click(function () {
    $("#IsCPF").val('true');
})

$("#btnMatricula").click(function () {
    $("#IsCPF").val('false');
})

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});

$(".extensaoArquivo").change(function () {
    $("#extensaoArquivo").val($('.extensaoArquivo option:selected').val());
    if ($("#extensaoArquivo").val() != 'TXT') {
        document.getElementsByClassName('posicaoInicialMatricula')[0].children[0].innerText = 'Posição da Coluna'
        document.getElementsByClassName('posicaoInicialCPF')[0].children[0].innerText = 'Posição da Coluna'
        document.getElementsByClassName('posicaoInicialValor')[0].children[0].innerText = 'Posição da Coluna Referente ao Valor'
        $('.posicaoFinalCPF').hide();
        $('.posicaoFinalMatricula').hide();
        $('.posicaoFinalValor').hide();

    }
    else {
        document.getElementsByClassName('posicaoInicialMatricula')[0].children[0].innerText = 'Posição inicial da Matrícula'
        document.getElementsByClassName('posicaoInicialCPF')[0].children[0].innerText = 'Posição inicial do CPF'
        document.getElementsByClassName('posicaoInicialValor')[0].children[0].innerText = 'Posição inicial do Valor'
        $('.posicaoFinalCPF').show();
        $('.posicaoFinalMatricula').show();
        $('.posicaoFinalValor').show();
    }
});
$(".codigoFormaVerificacaoArq").change(function () {
    $("#codigoFormaVerificacaoArq").val($('.codigoFormaVerificacaoArq option:selected').val());
});
$('#filtroCliente').focusout(function () {
    CarregaCliente($('#filtroCliente').val());

})

function LocalizaCliente() {

    var filtro = '?codempresa=' + $("#codigoEmpresa").val()
    var url = montaUrl("/Configuracao/ConfigParamLeituraArqDF/ConsultaTodosClientes/" + filtro)

    $("#modalCliente").load(url);
    $("#LocalizaCliente").modal();
}

function SelecionaCliente(codCliente) {

    CarregaCliente(codCliente);

}
function CarregaCliente(codigoCli) {

    if (codigoCli != "") {

        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

        $.ajax({
            type: 'Post',
            url: montaUrl('/Configuracao/ConfigParamLeituraArqDF/BuscaCliente/?codigoCliente=' + codigoCli),
            dataType: 'json',
            success: function (data) {
                $.unblockUI();
                $("#LocalizaCliente .close").click();

                $("#filtroCliente").val(data.codigoCliente);
                $("#codigoCliente").val(data.codigoCliente);
                $("#nomeCliente").val(data.nomeCliente);

                LimparCampos()

                if (data.codigoArquivoDescontoFolha != null && data.codigoArquivoDescontoFolha > 0) {

                    document.getElementById("codigoFormaVerificacaoArq").selectedIndex = (data.codigoFormaVerificacao - 1)

                    document.getElementById("extensaoArquivo").value = data.extensaoArquivoRET.trim();


                    $("#IsCPF").val(data.isCPF);
                    $("#linhaInicial").val(data.linhaInicial);
                    $("#posicaoInicialCPF").val(data.posicaoInicialCPF);
                    $("#posicaoFinalCPF").val(data.posicaoFinalCPF);
                    $("#posicaoInicialMatricula").val(data.posicaoInicialMatricula);
                    $("#posicaoFinalMatricula").val(data.posicaoFinalMatricula);
                    $("#posicaoInicialValor").val(data.posicaoInicialValor);
                    $("#posicaoFinalValor").val(data.posicaoFinalValor);
                    $("#codigoArquivoDescontoFolha").val(data.codigoArquivoDescontoFolha);
                    $("#codigoFormaVerificacao").val(data.codigoFormaVerificacao);
                    $("#codigoParamUsuario").val(data.codigoParamUsuario);
                    $("#codigoParamValor").val(data.codigoParamValor);
                    $("#extensaoArquivoREM").val(data.extensaoArquivoREM);
                    $("#extensaoArquivoRET").val(data.extensaoArquivoRET);




                    if (data.isCPF) {
                        $("#btnCPF").click();
                        $("#liMatricula").removeClass('active');
                        $("#liCPF").addClass('active');
                    }
                    else {
                        $("#btnMatricula").click();
                        $("#liCPF").removeClass('active');
                        $("#liMatricula").addClass('active');
                    }

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
        $("#codigoCliente").val(0)
    }

}
function SalvarConfiguracao() {

    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#extensaoArquivo").val($('.extensaoArquivo option:selected').val());
    $("#codigoFormaVerificacaoArq").val($('.codigoFormaVerificacaoArq option:selected').val());

    var data = "codigoCliente=" + $("#codigoCliente").val()
        + "&IsCPF=" + $("#IsCPF").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codigoFormaVerificacaoArq=" + $("#codigoFormaVerificacaoArq").val()
        + "&extensaoArquivo=" + $("#extensaoArquivo").val()
        + "&linhaInicial=" + $("#linhaInicial").val()
        + "&posicaoInicialCPF=" + $("#posicaoInicialCPF").val()
        + "&posicaoFinalCPF=" + $("#posicaoFinalCPF").val()
        + "&posicaoInicialMatricula=" + $("#posicaoInicialMatricula").val()
        + "&posicaoFinalMatricula=" + $("#posicaoFinalMatricula").val()
        + "&posicaoInicialValor=" + $("#posicaoInicialValor").val()
        + "&posicaoFinalValor=" + $("#posicaoFinalValor").val()
        + "&codigoArquivoDescontoFolha=" + $("#codigoArquivoDescontoFolha").val()
        + "&codigoFormaVerificacao=" + $("#codigoFormaVerificacao").val()
        + "&codigoParamUsuario=" + $("#codigoParamUsuario").val()
        + "&codigoParamValor=" + $("#codigoParamValor").val()
        + "&extensaoArquivoREM=" + $("#extensaoArquivoREM").val()
        + "&extensaoArquivoRET=" + $("#extensaoArquivoRET").val();


    var url = montaUrl("/Configuracao/ConfigParamLeituraArqDF/SalvarParamLeituraArquivo");

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
                CarregaCliente($("#codigoCliente").val());
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
function LimparCampos() {

    document.getElementById("codigoFormaVerificacaoArq").selectedIndex = -1;
    document.getElementById("extensaoArquivo").selectedIndex = -1;

    $("#IsCPF").val(false);
    $("#linhaInicial").val('');
    $("#posicaoInicialCPF").val('');
    $("#posicaoFinalCPF").val('');
    $("#posicaoInicialMatricula").val('');
    $("#posicaoFinalMatricula").val('');
    $("#posicaoInicialValor").val('');
    $("#posicaoFinalValor").val('');
    $("#codigoArquivoDescontoFolha").val('');
    $("#codigoFormaVerificacao").val('');
    $("#codigoParamUsuario").val('');
    $("#codigoParamValor").val('');
    $("#extensaoArquivoRET").val('');

    $("#liMatricula").removeClass('active');
    $("#liCPF").removeClass('active');

}
