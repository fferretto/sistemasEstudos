function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    if ($('#bBoleto').val() == "True") {
        $("#btnBolSim").click();
        $("#liBolNao").removeClass('active');
        $("#liBolSim").addClass('active');
    }
    if ($('#bPagamento').val() == "True") {
        $("#btnPagSim").click();
        $("#liPagNao").removeClass('active');
        $("#liPagSim").addClass('active');
    }

    if ($('#cnab240pgto').val() == "False") {
        $("#btnCNAB400PGTO").click();
        $("#liCNAB240PGTO").removeClass('active');
        $("#liCNAB400PGTO").addClass('active');
    }
    if ($('#cnab240boleto').val() == "False") {
        $("#btnCNAB400BOL").click();
        $("#liCNAB240BOL").removeClass('active');
        $("#liCNAB400BOL").addClass('active');
    }
    if ($('#AgruparFaturamentosDia').val() == "False") {
        $("#btnNaoAgruparFaturamentos").click();
        $("#liSimAgruparFaturamentos").removeClass('active');
        $("#liNaoAgruparFaturamentos").addClass('active');
    }

    if ($('#acessoAdmin').val() == "False") {
        $('.codEmpresa').prop('disabled', true);
    }
    
    if ($('#codContaCorrente').val() == '' || $('#codContaCorrente').val() == '0') {
        $("#btnDesativar").attr('disabled', 'disabled');
        $("#btnHomologacao").attr('disabled', 'disabled');
        
    }

    if ($('#formaTransmissaoPG').val() == "MANUAL") {
        $("#btnManualPGTO").click();
        $("#liAPIPGTO").removeClass('active');
        $("#liVanBancariaPGTO").removeClass('active');
        $("#liManualPGTO").addClass('active');
    }
    if ($('#formaTransmissaoPG').val() == "API") {
        $("#btnAPIPGTO").click();
        $("#liVanBancariaPGTO").removeClass('active');
        $("#liManualPGTO").removeClass('active');
        $("#liAPIPGTO").addClass('active');
    }
    if ($('#formaTransmissaoPG').val() == "VAN") {
        $("#btnVanBancariaPGTO").click();
        $("#liAPIPGTO").removeClass('active');
        $("#liManualPGTO").removeClass('active');
        $("#liVanBancariaPGTO").addClass('active');
    }


    if ($('#formaTransmissaoBol').val() == "MANUAL") {
        $("#btnManualBOL").click();
        $("#liAPIBOL").removeClass('active');
        $("#liVanBancariaBOL").removeClass('active');
        $("#liManualBOL").addClass('active');
    }
    if ($('#formaTransmissaoBol').val() == "API") {
        $("#btnAPIBOL").click();
        $("#liVanBancariaBOL").removeClass('active');
        $("#liManualBOL").removeClass('active');
        $("#liAPIBOL").addClass('active');
    }
    if ($('#formaTransmissaoBol').val() == "VAN") {
        $("#btnVanBancariaBOL").click();
        $("#liAPIBOL").removeClass('active');
        $("#liManualBOL").removeClass('active');
        $("#liVanBancariaBOL").addClass('active');
    }


    if ($('#teraMulta').val() == "True") {
        $("#btnMultaSim").click();
        $("#liMultaNao").removeClass('active');
        $("#liMultaSim").addClass('active');

        if ($('#valorMulta').val() != " 0,00") {
            $("#btnMultaValor").click();
            $("#liMultaValor").addClass('active');
        }
        else if ($('#PercMulta').val() != " 0,00") {
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
        $("#liJurosNao").removeClass('active');
        $("#liJurosSim").addClass('active');

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
$(window).load(carregaPagina);


$('#CpfCnpj').focusout(function () {

    var CNPJ = $('#CpfCnpj').val();

    if (CNPJ != "") {
        CNPJ = CNPJ.replace('/', '').replace('.', '').replace('-', '')
        if (!validarCNPJ(CNPJ)) {
            $('#CpfCnpj').addClass("borderError");
            addErros("CpfCnpj", "CNPJ Inválido!")
        }
        else {
            //$('#CpfCnpj').val(cpf_mask(CNPJ))
            $('#CpfCnpj').removeClass("borderError");
            $("span[data-valmsg-for='CpfCnpj']").hide();
        }
    }
})


$(".CodBanco").change(function () {
    $("#CodBanco").val($('.CodBanco option:selected').val());

    if ($('#CodBanco').val() == "104") {
        $('#ParamTransCaixa').show();
    }
    else {
        $('#ParamTransCaixa').val('');
        $('#ParamTransCaixa').hide();
    }
});

$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

});

function CarregaGrid() {

    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    var url = montaUrl("/Cadastros/ContaCorrente/ConsultaContaCorrente/")
    url += ("&codEmpresa=" + $("#codEmpresa").val());

    $("#modal").load(url);
    $("#Localizar").modal();
}

function GeraArquivoHomologacao() {

    var url = montaUrl("/Cadastros/ContaCorrente/ProcessoHomologacaoConta/")
    url += "&codigoContaCorrente=" + $("#codContaCorrente").val();

    $("#modal").load(url);
    $("#Localizar").modal();

}
function formaTransmissaoPG(tipo) {
    if (tipo == 'MANUAL') {
        $("#formaTransmissaoPG").val('MANUAL');
        $("#loginTransmissaoPG").val('');
        $("#senhaTransmissaoPG").val('');
        $("#caminhoRemessaPG").val('');
        $("#caminhoRetornoPG").val('');
    }
    else if (tipo == 'API') {
        $("#formaTransmissaoPG").val('API');
        $("#caminhoRemessaPG").val('');
        $("#caminhoRetornoPG").val('');
    }
    else if (tipo == 'VAN') {
        $("#formaTransmissaoPG").val('VAN');
        $("#loginTransmissaoPG").val('');
        $("#senhaTransmissaoPG").val('');
    }
};

function formaTransmissaoBOL(tipo) {
    if (tipo == 'MANUAL') {
        $("#formaTransmissaoBOL").val('MANUAL');
        $("#loginTransmissaoBOL").val('');
        $("#senhaTransmissaoBOL").val('');
        $("#caminhoRemessaBOL").val('');
        $("#caminhoRetornoBOL").val('');
    }
    else if (tipo == 'API') {
        $("#formaTransmissaoBOL").val('API');
        $("#caminhoRemessaBOL").val('');
        $("#caminhoRetornoBOL").val('');
    }
    else if (tipo == 'VAN') {
        $("#formaTransmissaoBOL").val('VAN');
        $("#loginTransmissaoBOL").val('');
        $("#senhaTransmissaoBOL").val('');
    }
};


$("#btnCNAB400PGTO").click(function () {
    $("#cnab240pgto").val('False');
    $("#qtPosicaoArqPGTO").val("400")
})
$("#btnCNAB240PGTO").click(function () {
    $("#cnab240pgto").val('True');
    $("#qtPosicaoArqPGTO").val("240")
})
$("#btnCNAB400BOL").click(function () {
    $("#cnab240boleto").val('False');
    $("#qtPosicaoArqBoleto").val("400")
})
$("#btnCNAB240BOL").click(function () {
    $("#cnab240boleto").val('True');
    $("#qtPosicaoArqBoleto").val("240")
})
$("#btnNaoAgruparFaturamentos").click(function () {
    $("#AgruparFaturamentosDia").val('False');
})
$("#btnSimAgruparFaturamentos").click(function () {
    $("#AgruparFaturamentosDia").val('True');
})


$("#btnPagSim").click(function () {
    $("#bPagamento").val('True');
    formaTransmissaoPG('MANUAL');
})
$("#btnPagNao").click(function () {
    $("#bPagamento").val('False');
    $("#CodConvenioPag").val('');
    $("#ParametroTransmissaoPag").val('');
    $("#valTED").val('');
    $("#ValMinPGTO").val('');
    $("#ValMinTED").val('');
})
$("#btnBolSim").click(function () {
    formaTransmissaoBOL('MANUAL');
    $("#bBoleto").val('True');
})
$("#btnBolNao").click(function () {
    $("#bBoleto").val('False');
    $("#codigoCedente").val('');
    $("#digitoCodigoCedente").val('');
    $("#CarteiraRemessa").val('');
    $("#VariacaoCarteira").val('');
    $("#CodTransmissao").val('');
    $("#TaxaEmissaoBoleto").val('');
})
$("#btnManualPGTO").click(function () {
    formaTransmissaoPG('MANUAL');
});
$("#btnAPIPGTO").click(function () {
    formaTransmissaoPG('API');
});
$("#btnVanBancariaPGTO").click(function () {
    formaTransmissaoPG('VAN');
});

$("#btnManualBOL").click(function () {
    formaTransmissaoBOL('MANUAL');
});
$("#btnAPIBOL").click(function () {
    formaTransmissaoBOL('API');
});
$("#btnVanBancariaBOL").click(function () {
    formaTransmissaoBOL('VAN');
});

$("#btnAgruparCobrancaNao").click(function () {
    $("#AgruparFaturamentosDia").val('False');
})

$("#btnAgruparCobrancaSim").click(function () {
    $("#AgruparFaturamentosDia").val('True');
})

$("#btnMultaNao").click(function () {
    $("#valorMulta").val('');
    $("#PercMulta").val('');
    $("#teraMulta").val('False');
})

$("#btnMultaSim").click(function () {
    $("#teraMulta").val('True');
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
    $("#teraJuros").val('False');
})

$("#btnJurosSim").click(function () {
    $("#teraJuros").val('True');
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

function ConfirmaSalvar() {
    var validado = true;

    if ($("#CodBanco").val() == "" || $("#CodBanco").val() == "0") {
        validado = false;
        msgAviso("Banco não Informado!")
    }

    var CNPJ = $('#CpfCnpj').val();
    if (!validarCNPJ(CNPJ)) {
        validado = false;
        $('#CpfCnpj').addClass("borderError");
        addErros("CpfCnpj", "CNPJ Inválido!")
    }


    var form = $('form', '#page-inner');

    var url = montaUrl("/Cadastros/ContaCorrente/Salvar/");

    if (validado) {
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
        form.attr('action', url);
        form.submit();
    }
}
function NovaContaCorrente() {

    var form = $('form', '#page-inner');

    var url = montaUrl("/Cadastros/ContaCorrente/Index/");

    form.attr('action', url);
    form.submit();
}
function ConfirmaDesativar() {
    var form = $('form', '#page-inner');

    var url = montaUrl("/Cadastros/ContaCorrente/Desativar/");
    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    form.attr('action', url);
    form.submit();
}

//--------------------------Geração de arquivos para homologação----------------------
function LocalizaFavorecido() {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
    var url = montaUrl("/Cadastros/ContaCorrente/ConsultaFavorecido")
    url += "&codigoEmpresa=" + $("#codEmpresa").val();


    $("#modal2").load(url);
    $("#Localizar2").modal();
}
function SelecionaFavorecido(CODFAVORECIDO) {

    if (CODFAVORECIDO != "") {

        var url = montaUrl('/Cadastros/ContaCorrente/BuscaFavorecido/?filtroFavorecido=' + CODFAVORECIDO);
        url += '&codigoEmpresa=' + $("#codEmpresa").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#Localizar2 .close").click();
                if (data.length > 0) {
                    PreencheFavorecido(data);

                } else {
                    addErros("nomeFavorecido", "Favorecido não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("nomeFavorecido", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtroFavorecido").val('');
        $("#nomeFavorecido").val('');
    }

}
function PreencheFavorecido(data) {
    var arr = data.split("/");

    $("#filtroFavorecido").val(arr[0]);
    $("#nomeFavorecido").val(arr[1]);
    $("#codigoFavorecido").val(arr[0])
};
function GeraArquivoRemessa(TipoArquivo) {

    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    var data = "codigoEmpresa=" + $("#codEmpresa").val()
        + "&CodigoContaCorrente=" + $("#codContaCorrente").val()
        + "&codigoFavorecido=" + + $("#codigoFavorecido").val()
        + "&TipoArquivo=" + TipoArquivo;


    var _url = montaUrl("/Cadastros/ContaCorrente/GeraArquivo")
    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {
                var caminho = montaUrl("/Cadastros/ContaCorrente/DownloadArquivoRemessa/");
                caminho += "&id=" + data.responseText;
                caminho += "&codigoEmpresa=" + $("#codEmpresa").val();
                caminho += "&TipoArquivo=" + TipoArquivo;
                var TransmissaoManual = false;
                console.log($('#formaTransmissaoPG').val());
                if (TipoArquivo == 'PAG') {
                    if ($('#formaTransmissaoPG').val() == "MANUAL")
                        TransmissaoManual = true;
                    else
                        TransmissaoManual = false;
                }
                else {
                    if ($('#formaTransmissaoBol').val() == "MANUAL")
                        TransmissaoManual = true;
                    else
                        TransmissaoManual = false;
                }


                $("#btnDownloadBolPDF").attr('disabled', false);
                if (TransmissaoManual) {
                    window.location.href = caminho;
                }
                else {
                    msgSucesso("Arquivo gerado e enviado para o banco com sucesso!")
                }
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
function DownloadBoleto() {

    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    var _url = montaUrl("/Cadastros/ContaCorrente/DownloadBoleto");
    _url += "&codigoEmpresa=" + $("#codEmpresa").val();
    _url += "&CodigoContaCorrente=" + $("#codContaCorrente").val();

    window.location.href = _url;

};
