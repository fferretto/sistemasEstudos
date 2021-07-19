function carregaPagina() {

    $("#nmFavorecido").prop('disabled', true);
    
    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
}

$(window).load(carregaPagina());


$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
});
$(".CodigoPlanoContas").change(function () {
    $("#CodigoPlanoContas").val($('.CodigoPlanoContas option:selected').val());
});

$('#filtro').focusout(function () {
    var filtro = $('#filtro').val();
    if (filtro != "") {
        var url = montaUrl('/ContasPagar/ConsultarTitulos/BuscaFavorecido/');
        url += '&filtro=' + filtro
        url += '&codEmpresa=' + $("#codEmpresa").val();
            

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                if (data.length > 0) {
                    PreencheFavorecido(data);
                } else {
                    msgAviso("Favorecido não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                msgErro("Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtro").val('');
        $("#nmFavorecido").val('');
        $("#codFavorecido").val(0)
    }
})
function LocalizaFavorecido() {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
    var url = montaUrl("/ContasPagar/ConsultarTitulos/ConsultaFavorecido")
    url += "&codEmpresa=" + $("#codEmpresa").val();

    $("#modalFavorecido").load(url);
    $("#LocalizarFavorecido").modal();
}
function SelecionaFavorecido(CODFAVORECIDO) {

    if (CODFAVORECIDO != "") {

        var url = montaUrl('/ContasPagar/ConsultarTitulos/BuscaFavorecido/?filtro=' + CODFAVORECIDO);
        url += '&codEmpresa=' + $("#codEmpresa").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#LocalizarFavorecido .close").click();
                if (data.length > 0) {
                    PreencheFavorecido(data);

                } else {
                    addErros("CODFORNECEDOR", "Favorecido não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("CODFORNECEDOR", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#CODFORNECEDOR").val('');
        $("#NMFORNECEDOR").val('');
    }

}
function PreencheFavorecido(data) {
    var arr = data.split("/");

    $("#filtro").val(arr[0]);
    $("#nmFavorecido").val(arr[1]);
    $("#codFavorecido").val(arr[0])
};

function ConsultarFechCred() {

    $('#modal').empty();
    var codigoTitulo = "0";

    if ($("#CodigoTitulo").val() != "") {
        if (!isNumeric($("#CodigoTitulo").val())) {
            msgAviso("O código do título inválido!");
            return false;
        }
        else {
            codigoTitulo = $("#CodigoTitulo").val();
        }
    }
        

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });


    $("#codEmpresa").val($('.codEmpresa option:selected').val());
    $("#codStatus").val($('.codStatus option:selected').val());

    var url = montaUrl("/ContasPagar/ConsultarTitulos/CarregaGrigTitulos/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codFavorecido=" + $("#codFavorecido").val()
        + "&codEmpresa=" + $("#codEmpresa").val()
        + "&codStatus=" + $("#codStatus").val()
        + "&CodigoTitulo=" + codigoTitulo
        + "&chkTodasSubRedes=" + $('#chkTodasSubRedes').prop('checked'))

    

    $("#modal").load(url);
    $("#ListTransacaoPag").show();
};

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
$("#dtInicio").change(function () {
    var dtInicio = $("#dtInicio").val().split("/");
    var dtFim = $("#dtFim").val().split("/");
    var dataIni = new Date(dtInicio[2], dtInicio[1] - 1, dtInicio[0]);
    var dataFim = new Date(dtFim[2], dtFim[1] - 1, dtFim[0]);

    if (dataFim < dataIni) {
        $("#dtFim").val($("#dtInicio").val())
    }
});
$("#dtFim").change(function () {
    var dtInicio = $("#dtInicio").val().split("/");
    var dtFim = $("#dtFim").val().split("/");
    var dataIni = new Date(dtInicio[2], dtInicio[1] - 1, dtInicio[0]);
    var dataFim = new Date(dtFim[2], dtFim[1] - 1, dtFim[0]);

    if (dataFim < dataIni) {
        $("#dtInicio").val($("#dtFim").val());
        //addErros("dtFim", "Data Inválida!");
    }
});
function VisualizarFechamentos(CODTITULO) {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/ConsultaTransacoes/");

    url += "&CODTITULO=" + CODTITULO

    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}
function DetalharTaxas(codigo) {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/DetalharTaxas/");
    url += "&codtitulo=" + codigo

    $("#modalVisualizarTaxas").load(url);
    $("#VisualizarTaxas").modal();

}
function VisualizarLog(CODTITULO) {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/ConsultaLog/");

    url += "&CODTITULO=" + CODTITULO

    $("#modalLog").load(url);
    $("#VisualizarLog").modal();
}
function AnteciparPGTO(codigoTitulo) {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/AnteciparPGTO/");
    url += "&CODTITULO=" + codigoTitulo;

    $("#modalJanelaMdl").load(url);
    $("#JanelaMdl").modal();
}
function EditarTitulo(CODTITULO) {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/EdicaoTitulo/")
    url += ("&CODTITULO=" + CODTITULO)

    $("#modalEditarTitulo").load(url);
    $("#EditarTitulo").modal();
}
function AlterarTituloPGTO() {

    var dtAtual = new Date();
    var dataAtual = new Date(`${dtAtual.getFullYear()},${dtAtual.getMonth() + 1},${dtAtual.getDate()}`)
    var datGrid = $("#DATREALPGTO").val().split('/');
    var DataFormatadaGrid = new Date(`${datGrid[2]},${datGrid[1]},${datGrid[0]}`);

    $("#CodigoPlanoContas").val($('.CodigoPlanoContas option:selected').val());

    var url = montaUrl("/ContasPagar/ConsultarTitulos/SalvaEdicaoTitulo/");

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da alteração.");
        return false;
    }
    if (DataFormatadaGrid < dataAtual) {
        msgAviso("A data de pagamento não pode ser inferior a data atual");
        return false;
    }

    var data = "DATREALPGTO=" + $("#DATREALPGTO").val()
        + "&VALLIQ=" + $("#VALLIQ").val()
        + "&VALACRESCIMO=" + $("#VALACRESCIMO").val()
        + "&VALDESCONTO=" + $("#VALDESCONTO").val()
        + "&CODBANCO=" + $("#CODBANCO").val()
        + "&AGENCIA=" + $("#AGENCIA").val()
        + "&DVAGENCIA=" + $("#DVAGENCIA").val()
        + "&CONTACORRENTE=" + $("#CONTACORRENTE").val()
        + "&DVCONTACORRENTE=" + $("#DVCONTACORRENTE").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()
        + "&CODIGOTITULO=" + $("#CODIGOTITULO").val()
        + "&CodigoPlanoContas=" + $("#CodigoPlanoContas").val()

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var TableTitulos = $('#gridConsultaTitulos').DataTable()
                var dataTitulo = "";
                var ExisteTitVencido = false;

                var trs = $(TableTitulos.cells().nodes()).parent();
                $(trs).each(function (index, value) {

                    if ($("#CODIGOTITULO").val() == $(value).find('.CODIGOTITULO').find('input').val()) {

                        $(value).find('.TITVENCIDO').find('input').val('N');
                        $(value).find('.DATPGTO')[0].innerText = $("#DATPGTO").val();

                        $(value).find('.inpBola')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.inpEditarTitulo')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.TIPCARTAO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.NMFAVORECIDO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.CNPJ')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.DATPGTO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.VALOR')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.STATUS')[0].style.backgroundColor = "#ffffff"
                    }

                });

                $("#EditarTitulo .close").click();
                //ConsultarFechCred();

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
function ValidaTituloVencidoLoad() {

    var table = $('#gridConsultaTitulos').DataTable();
    var valor = 0.00;

    var TITVENCIDO;
    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (index, value) {

        TITVENCIDO = $(value).find('.TITVENCIDO').find('input').val();

        if (TITVENCIDO == "S") {
            msgAviso("Existem títulos vencidos. Favor ajustar para realizar o pagamento.")
            return false;
        }
    });

}
function JustificativaDesvincularTitulo() {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/JustificativaDesvincularTitulo")

    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();

}
function JustificativaBaixaManual() {
    
    var url = montaUrl("/ContasPagar/ConsultarTitulos/JustificativaBaixaManual")

    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();

}
function JustificativaCancelamentoTitulo() {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/JustificativaCancelarTitulo")

    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();

}
function BaixaManual() {
    
    var codContaCorrenteBaixaManual = $('.codContaCorrenteBaixaManual option:selected').val()
    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da baixa.");
        return false;
    }
    else if (codContaCorrenteBaixaManual == '' || codContaCorrenteBaixaManual == '-1' || codContaCorrenteBaixaManual == "null" || codContaCorrenteBaixaManual == null) {
        valido = false;
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
        return false;
    }
    var table = $('#gridConsultaTitulos').DataTable();


    var meuData = '';
    var i = 0;
    var trs = $(table.cells().nodes()).parent()
    $(trs).each(function (index, value) {
        //console.log(value)
        if ($(value).hasClass("selected")) {
            meuData += '&ListaFechamento%5B' + i + '%5D.CODTITULO=' + $(value).find('.CODTITULO').find('input').val();
            i = i + 1;
        }
    });
    
    var data = "codEmpresa=" + $("#codEmpresa").val()
            + "&codJustificativa=" + codJustificativa
            + "&descJustificativa=" + descJustificativa
            + "&codigoContaCorrente=" + codContaCorrenteBaixaManual
            + "&DescJustOutros=" + $("#DescJustOutros").val()
        + meuData;
    

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var url = montaUrl("/ContasPagar/ConsultarTitulos/BaixaManual");
    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var table = $('#gridConsultaTitulos').DataTable();
                var trs = $(table.cells().nodes()).parent()
                $(trs).each(function (index, value) {
                    if ($(value).hasClass("selected")) {
                        $(value).find('.inpSTATUS')[0].innerText = 'BAIXADO MANUALMENTE';
                    }
                });

                $("#VisualizarTransacoes .close").click();

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
function CancelarTitulo() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo do cancelamento.");
        return false;
    }

    var table = $('#gridConsultaTitulos').DataTable();


    var meuData = '';
    var i = 0;
    var trs = $(table.cells().nodes()).parent()
    $(trs).each(function (index, value) {
        if ($(value).hasClass("selected")) {
            meuData += '&ListaFechamento%5B' + i + '%5D.CODTITULO=' + $(value).find('.CODTITULO').find('input').val();
            i = i + 1;
        }
    });

    var data = "codEmpresa=" + $("#codEmpresa").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()
        + meuData;


    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var url = montaUrl("/ContasPagar/ConsultarTitulos/CancelarTitulo");
    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var table = $('#gridConsultaTitulos').DataTable();
                var trs = $(table.cells().nodes()).parent()
                $(trs).each(function (index, value) {
                    if ($(value).hasClass("selected")) {
                        $(value).find('.inpSTATUS')[0].innerText = 'CANCELADO';
                    }
                });

                $("#VisualizarTransacoes .close").click();

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
function DesvinculaTitulo() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da baixa.");
        return false;
    }
    var table = $('#gridConsultaTitulos').DataTable();


    var meuData = '';
    var i = 0;
    var trs = $(table.cells().nodes()).parent()
    $(trs).each(function (index, value) {
        //console.log(value)
        if ($(value).hasClass("selected")) {
            meuData += '&ListaFechamento%5B' + i + '%5D.CODTITULO=' + $(value).find('.CODTITULO').find('input').val();
            i = i + 1;
        }
    });

    var data = "codEmpresa=" + $("#codEmpresa").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()
        + meuData;


    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var url = montaUrl("/ContasPagar/ConsultarTitulos/DesvinculaTitulo");
    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var table = $('#gridConsultaTitulos').DataTable();
                var trs = $(table.cells().nodes()).parent()
                $(trs).each(function (index, value) {
                    if ($(value).hasClass("selected")) {
                        $(value).find('.inpSTATUS')[0].innerText = 'EM ABERTO';
                    }
                });

                $("#VisualizarTransacoes .close").click();

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
//----------------------------------------ANTECIPAÇÃO DE PAGAMENTO DE TÍTULOS ---------------------------------------
function CalcularTaxas() {

    $('#modalCalculoAntecipacao').empty();


    var dtAtual = new Date();
    var dataAtual = new Date(`${dtAtual.getFullYear()},${dtAtual.getMonth() + 1},${dtAtual.getDate()}`)
    var datGrid = $("#dtAntecipacao").val().split('/');
    var DataFormatadaGrid = new Date(`${datGrid[2]},${datGrid[1]},${datGrid[0]}`);

    if (DataFormatadaGrid < dataAtual) {
        msgAviso("A nova data de pagamento não pode ser inferior a data atual");
        return false;
    }

    var url = montaUrl("/ContasPagar/ConsultarTitulos/CalculaTaxaAntecipacaoPGTO");
    url += '&codigoTitulo=' + $("#codigoTitulo").val();
    url += '&dtAntecipacao=' + $("#dtAntecipacao").val();

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $("#modalCalculoAntecipacao").load(url);
    $("#CalculoAntecipacao").show();

};
function SalvarAntecipacaoPagamento() {

    var data = "dtAntecipacao=" + $("#dtAntecipacao").val()
        + "&codigoTitulo=" + $("#codigoTitulo").val();

    var _url = montaUrl("/ContasPagar/ConsultarTitulos/SalvarAntecipacaoPGTO")

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                $('#modalCalculoAntecipacao').empty();
                $("#JanelaMdl .close").click();
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

};
//----------------------------------------AJUSTAR VALOR DO TÍTULO ---------------------------------------

function AjustarValorTitulo(CODTITULO) {

    var url = montaUrl("/ContasPagar/ConsultarTitulos/AjustarValorTitulo/");

    url += "&CODTITULO=" + CODTITULO

    $("#modalJanelaMdl").load(url);
    $("#JanelaMdl").modal();
}
function SalvarAjusteValor() {

    var valor = getMoney($("#valorConcedido").val());
    if ($("#Descricao").val() == '') {
        msgAviso('Obrigatório informar a descrição.');
        return false;
    }
    if (valor <= 0) {
        msgAviso('Valor inválido.');
        return false;
    }
    
    var url = montaUrl("/ContasPagar/ConsultarTitulos/SalvarAjusteValor/");

    var data = "codigoTituloAjusteValor=" + $("#codigoTituloAjusteValor").val()
        + "&valorConcedido=" + $("#valorConcedido").val()
        + "&Descricao=" + $("#Descricao").val()
        + "&Desconto=" + $("#Desconto").val()

    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                $("#JanelaMdl .close").click();
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
                                ConsultarFechCred();
                            }
                        }
                    }
                });



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
