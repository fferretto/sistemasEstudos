
function carregaPagina() {

    $("#nmFavorecido").prop("disabled", "disabled")
    $("#FiltroNmBanco").prop("disabled", "disabled")

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
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
        }
    });
}

$(window).load(carregaPagina());


$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

});

$(".codContaCorrente").change(function () {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());

});



$('#filtroCodBanco').focusout(function () {
    var filtro = $('#filtroCodBanco').val();
    if (filtro != "") {
        $("#codBanco").val(filtro)

        //var url = montaUrl('/ContasPagar/GeraBordero/BuscaBanco/?filtro=' + filtro);
        var url = montaUrl('/Generico/CadastrosDiversos/BuscaBanco/?filtro=' + filtro);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                if (data.length > 0) {
                    PreencheBanco(data);
                } else {
                    addErros("FiltroNmBanco", "Banco não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("FiltroNmBanco", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtroCodBanco").val('');
        $("#FiltroNmBanco").val('');
        $("#codBanco").val('');
    }
});

$('#filtro').focusout(function () {
    var filtro = $('#filtro').val();
    if (filtro != "") {
        var url = montaUrl('/ContasPagar/GeraBordero/BuscaFavorecido/?filtro=' + filtro);
        url += '&codEmpresa=' + $("#codEmpresa").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                console.log(data.length)
                console.log(data)
                if (data.length > 0) {
                    PreencheFavorito(data);
                } else {
                    addErros("nmFavorecido", "Favorito não encontrado");
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
    var url = montaUrl("/ContasPagar/GeraBordero/ConsultaFavorecido")
    url += "&codEmpresa=" + $("#codEmpresa").val();

    $("#modalFavorecido").load(url);
    $("#LocalizarFavorecido").modal();
}

function SelecionaFavorecido(CODFAVORECIDO) {

    if (CODFAVORECIDO != "") {

        var url = montaUrl('/ContasPagar/GeraBordero/BuscaFavorecido/?filtro=' + CODFAVORECIDO);
        url += '&codEmpresa=' + $("#codEmpresa").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#LocalizarFavorecido .close").click();
                console.log(data.length)
                console.log(data)
                if (data.length > 0) {
                    PreencheFavorito(data);

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
function ConsultarTitulos() {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    if (isValidaBusca()) {
        ConsultaFechCredBordero();
    }

};

function ConsultaFechCredBordero() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $('#modal').empty();

    var url = montaUrl("/ContasPagar/GeraBordero/ListaTitulos/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codFavorecido=" + $("#codFavorecido").val()
        + "&codEmpresa=" + $("#codEmpresa").val()
        + "&codContaCorrente=" + $("#codContaCorrente").val()
        + "&codBanco=" + $("#codBanco").val());

    $("#modal").load(url);
    $("#ListFechCred").show();
};

function isValidaBusca() {
    var valido = true;

    var data = new Date();
    var dtInicio = $("#dtInicio").val().split("/");
    var dtFim = $("#dtFim").val().split("/");
    var dataIni = new Date(`${dtInicio[2]},${dtInicio[1]},${dtInicio[0]}`);
    var dataFim = new Date(`${dtFim[2]},${dtFim[1]},${dtFim[0]}`);
    var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)

    //dataIni.setDate(dataIni.getDate + 7)

    //var diferenca = dataFim - dataIni;

    var contacorrente = $("#codContaCorrente").val();

    if (dataIni < dataAtual) {
        valido = false;
        msgAviso("A data de início não pode ser inferior a data atual.");
    }
    else if (dataIni.setDate(dataIni.getDate() + 30) < dataFim) {
        valido = false;
        msgAviso("O intervalo entre as datas não pode ser superior a 30 dias.");
    }
    else if (contacorrente == '' || contacorrente == '-1' || contacorrente == "null" || contacorrente == null) {
        valido = false;
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
    }

    return valido;

}



function PreencheFavorito(data) {
    var arr = data.split("/");

    $("#filtro").val(arr[0]);
    $("#nmFavorecido").val(arr[1]);
    $("#codFavorecido").val(arr[2])

};

function PreencheBanco(data) {
    var arr = data.split("/");

    $("#filtroCodBanco").val(arr[0]);
    $("#FiltroNmBanco").val(arr[1]);
    $("#codBanco").val(arr[2])

};

//---------------------------------IListFechCreNovo-------------------------

function VisualizarFechamentos(CODIGOTITULO) {

    var url = montaUrl("/ContasPagar/GeraBordero/ConsultaTransacoes/");

    url += "?CODIGOTITULO=" + CODIGOTITULO

    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}

function SalvaBordero() {

    var table = $('#gridListaTitulos').DataTable();
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {

        meuData += '&ListaFechamento%5B' + index + '%5D.VALLIQ=' + $(value).find('.VALLIQ').find('input').val();
        meuData += '&ListaFechamento%5B' + index + '%5D.CODFAVORECIDO=' + $(value).find('.CODFAVORECIDO').find('input').val();
        meuData += '&ListaFechamento%5B' + index + '%5D.DATPGTO=' + $(value).find('.DATPGTO').find('input').val();
        meuData += '&ListaFechamento%5B' + index + '%5D.LINHADIGITAVEL=' + $(value).find('.LINHADIGITAVEL').find('input').val();
        
    });

    var data = "codigoFormaPGTO=" + $("#codigoFormaPGTO").val()
        + "&codigoBanco=" + $("#codigoBanco").val()
        + "&codEmpresa=" + $("#codEmpresa").val()
        + "&codigoContaCorrente=" + $("#codContaCorrente").val()
        + meuData;

    var _url = montaUrl("/ContasPagar/GeraBordero/SalvaBordero")


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var ocultar = $('input[type="checkbox"]:checked', table.cells().nodes());
                $(ocultar).each((i, e) => $(e).closest('tr').addClass('selected'));
                table.rows('.selected').remove().draw(false);

                $("#qtTitulosSelecionados").val('0');
                $("#ValorBordero").val(formatReal('0'));
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

function JustificativaBaixaManual() {

    var table = $('#gridListaTitulos').DataTable();
    var ExisteItensBaixa = false;

    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        ExisteItensBaixa = true;
    });

    if (!ExisteItensBaixa) {
        msgAviso("obrigatório selecionar ao menos título para realizar a baixa.");
    }
    else {
        var url = montaUrl("/ContasPagar/GeraBordero/JustificativaBaixaManual")

        $("#modalJustBaixaManual").load(url);
        $("#JustBaixaManual").modal();


    }
}
function BaixaManual() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da baixa.");
        return false;
    }

    var table = $('#gridListaTitulos').DataTable();

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        meuData += '&ListaFechamento%5B' + index + '%5D.VALLIQ=' + $(value).find('.VALLIQ').find('input').val();
        meuData += '&ListaFechamento%5B' + index + '%5D.CODFAVORECIDO=' + $(value).find('.CODFAVORECIDO').find('input').val();
        meuData += '&ListaFechamento%5B' + index + '%5D.DATPGTO=' + $(value).find('.DATPGTO').find('input').val();
    });

    var data = "codigoFormaPGTO=" + $("#codigoFormaPGTO").val()
        + "&codigoBanco=" + $("#codigoBanco").val()
        + "&codEmpresa=" + $("#codEmpresa").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()
        + meuData;

    var url = montaUrl("/ContasPagar/GeraBordero/BaixaManual");
    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var ocultar = $('input[type="checkbox"]:checked', table.cells().nodes());
                $(ocultar).each((i, e) => $(e).closest('tr').addClass('selected'));
                table.rows('.selected').remove().draw(false);


                $("#JustBaixaManual .close").click();


                $("#qtTitulosSelecionados").val('0');
                $("#ValorBordero").val(formatReal('0'));
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
function EditarTitulo(codFechCred) {

    var url = montaUrl("/ContasPagar/GeraBordero/EdicaoTitulo/")
    url += ("&codFechCred=" + codFechCred)

    $("#modalEditarTitulo").load(url);
    $("#EditarTitulo").modal();
}


$(".codJustificativa").change(function () {
    $("#codJustificativa").val($('.codJustificativa option:selected').val());

});

function AlterarTituloPGTO() {

    var descFormaPagamento = $('.codFormaPagamento').find("option:selected").text();
    var CODBANCO_ORI = parseInt($("#CODBANCO_ORI").val());
    var CODBANCO = parseInt($("#CODBANCO").val());


    if (CODBANCO_ORI != CODBANCO && $.trim(descFormaPagamento) == 'CRÉDITO EM CONTA') {
        $.confirm({
            title: 'Confirma!',
            icon: "glyphicon glyphicon-ok",
            content: 'Com esta alteração, este título será removido da lista. Deseja continar?',
            type: 'green',
            buttons: {
                confirm: {
                    text: 'Sim',
                    btnClass: 'btn-green',
                    keys: ['enter'],
                    action: function () {
                        AtualizaTitulo(true);
                    }
                },
                cancel: {
                    action: function () {
                        msgAviso("Alteração cancelada!")
                    }
                }
            }
        });
    }
    else {
        AtualizaTitulo(false);
    }
}

function AtualizaTitulo(RemoveItemLista) {


    var data = new Date();
    var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)
    var datGrid = $("#DATREALPGTO").val().split('/');
    var DataFormatadaGrid = new Date(`${datGrid[2]},${datGrid[1]},${datGrid[0]}`);

    var url = montaUrl("/ContasPagar/GeraBordero/SalvaEdicaoTitulo/");

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();

    var codFormaPagamento = $('.codFormaPagamento option:selected').val()
    var descFormaPagamento = $('.codFormaPagamento').find("option:selected").text();

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da alteração.");
        return false;
    }
    if (DataFormatadaGrid < dataAtual) {
        msgAviso("A data de pagamento não pode ser inferior a data atual");
        return false;
    }

    var data = "DATPGTO=" + $("#DATPGTO").val()
        + "&VALLIQ=" + $("#VALLIQ").val()
        + "&CODBANCO=" + $("#CODBANCO").val()
        + "&AGENCIA=" + $("#AGENCIA").val()
        + "&DVAGENCIA=" + $("#DVAGENCIA").val()
        + "&CONTACORRENTE=" + $("#CONTACORRENTE").val()
        + "&DVCONTACORRENTE=" + $("#DVCONTACORRENTE").val()
        + "&codJustificativa=" + codJustificativa
        + "&descJustificativa=" + descJustificativa
        + "&DescJustOutros=" + $("#DescJustOutros").val()
        + "&CODIGOTITULO=" + $("#CODIGOTITULO").val()
        + "&codFormaPagamento=" + codFormaPagamento
        + "&descFormaPagamento=" + descFormaPagamento
        + "&VALACRESCIMO=" + $("#VALACRESCIMO").val()
        + "&VALDESCONTO=" + $("#VALDESCONTO").val()
        + "&DATREALPGTO=" + $("#DATREALPGTO").val()

   
    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var TableTitulos = $('#gridListaTitulos').DataTable();

                var trs = $(TableTitulos.cells().nodes()).parent();
                $(trs).each(function (index, value) {

                    if ($("#CODIGOTITULO").val() == $(value).find('.CODIGOTITULO').find('input').val()) {

                        $(value).find('.INPDATPGTO')[0].innerText = $("#DATREALPGTO").val();
                        $(value).find('.INPDESFORPAG')[0].innerText = descFormaPagamento;
                        $(value).find('.INPVALLIQ')[0].innerText = $("#VALLIQ").val();
                        $(value).find('.INPBANCO')[0].innerText = $("#CODBANCO").val();
                        $(value).find('.INPAGENCIA')[0].innerText = $("#AGENCIA").val() + "-" + $("#DVAGENCIA").val();
                        $(value).find('.INPCONTACORRENTE')[0].innerText = $("#CONTACORRENTE").val() + "-" + $("#DVCONTACORRENTE").val();

                        if (RemoveItemLista) {
                            $(value).addClass('selected');
                            TableTitulos.rows('.selected').remove().draw(false);
                        }
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

function getMoney(str) {
    return parseInt(str.replace(/[\D]+/g, ''));
}
function formatReal(int) {
    var tmp = int + '';
    tmp = tmp.replace(/([0-9]{2})$/g, ",$1");
    if (tmp.length > 6)
        tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

    return tmp;
}
function AtualizaValorBordero() {
    var qtSelecionados = 0;
    var ValorBordero = 0.00;

    var TableTitulos = $('#gridListaTitulos').DataTable();

    var trs = $(TableTitulos.cells().nodes()).parent();
    $(trs).each(function (index, value) {
        if ($('input[type="checkbox"]:checked', $(value).find('.CheckBox-Grid')[0]).val() == 'on') {
            qtSelecionados = qtSelecionados + 1;
            ValorBordero = ValorBordero + getMoney($(value).find('.INPVALLIQ')[0].innerText)
        }

    });

    $("#qtTitulosSelecionados").val(qtSelecionados);
    $("#ValorBordero").val(formatReal(ValorBordero));

}
function VisualizarTitulos(CODFAVORECIDO, DATPGTO) {


    var url = montaUrl("/ContasPagar/GeraBordero/ConsultaTitulosPGTO/");
    
    url += "&codEmpresa=" + $("#codEmpresa").val()
    url += "&codFavorecido=" + CODFAVORECIDO
    url += "&datPGTO=" + DATPGTO

    $("#modalListaTitulosFavorecido").load(url);
    $("#ListaTitulosFavorecido").modal();
}