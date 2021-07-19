

function carregaPagina() {

    $("#nmCredenciado").prop("disabled", "disabled")
    $("#FiltroNmBanco").prop("disabled", "disabled")
    $("#ValorArquivo").prop('disabled', true);
    
    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

}

$(window).load(carregaPagina());


$(".CodFormaPagamento").change(function () {
    $("#CodFormaPagamento").val($('.CodFormaPagamento option:selected').val());
});

$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
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
function PreencheBanco(data) {
    var arr = data.split("/");

    $("#filtroCodBanco").val(arr[0]);
    $("#FiltroNmBanco").val(arr[1]);
    $("#codBanco").val(arr[2])

};

function ConsultarFechCred() {

    var codContaCorrente = $('.codContaCorrente option:selected').val()
    var nmContaCorrente = $('.codContaCorrente').find("option:selected").text();


    if (codContaCorrente == '' || codContaCorrente == '-1' || codContaCorrente == "null" || codContaCorrente == null) {
        msgAviso("Obrigatório informar uma conta corrente.")
        $("#ValidaContaCorrente").show();
        $.unblockUI();
        return false;
    }


    $('#modal').empty();
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
    $("#CodFormaPagamento").val($('.CodFormaPagamento option:selected').val());


    var url = montaUrl("/ContasPagar/GerarArquivoRemessa/ConsultaBordero/?codEmpresa=" + $("#codEmpresa").val()
        + "&codBanco=" + $("#codBanco").val()
        + "&codContaCorrente=" + $("#codContaCorrente").val())

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaBordero").show();


};


//----------------------------ConsultaBordero--------------------

$('input[type=radio][name=FormaPagamento]').change(function () {
    $('#codFormaPagamento').val(this.value);
});

$('.umadataPartialView input').datepicker({
    format: "dd/mm/yyyy",
    language: "pt-BR",
    orientation: "auto",
    autoclose: true
});


$(".codContaCorrente").change(function () {

    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    
});

//-----------------------------IListaBorderoPag----------------------------------------

function ValidaTituloVencidoLoad() {

    var table = $('#gridBordero').DataTable();

    var TITVENCIDO;
    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (index, value) {

        TITVENCIDO = $(value).find('.TITVENCIDO').find('input').val();

        if (TITVENCIDO == "S") {
            msgAviso("Existem títulos vencidos em um ou mais borderôs. E por este motivo não será possível incluir os mesmos no arquivo de remessa.")
            return false;
        }

    });
}
function SelectAllgridBordero(Checado) {
    var table = $('#gridBordero').DataTable();
    var TITVENCIDO;
    
    var trsBordero = $(table.cells().nodes()).parent();
    $(trsBordero).each(function (index, value) {

        TITVENCIDO = $(value).find('.TITVENCIDO').find('input').val();

        if (TITVENCIDO == "N") {
            $(value).find('.inpchkFechCredPai').prop('checked', Checado);
        }

    });
    //$('input', table.cells().nodes()).prop('checked', this.checked);
};

function AltercaoContaCorrente(CODBANCO) {

    CODBANCO = "0" + CODBANCO;

    var table = $('#gridBordero').DataTable();

    var trs = $(table.cells().nodes()).parent().parent();
    $(trs).each(function (i, e) {

        if ($(e).find('.FORMAPGTO').find('input').val() == "CRÉDITO EM CONTA") {
            if ($(e).find('.CODBANCO').find('input').val() != CODBANCO) {

                $(e).find('input[type="checkbox"]:checked').val('unchecked')
                var linha = $(e).closest('tr');
                linha.hide();
            }
        }
    });
    table.rows('.selected').remove().draw(false);

}

function VisualizarFechamentos(CODBORDERO) {

    var url = montaUrl("/ContasPagar/GerarArquivoRemessa/ConsultaTitulosBordero/");

    url += "&CODBORDERO=" + CODBORDERO


    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}

function GeraArquivoRemessa() {

    var table = $('#gridBordero').DataTable();

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });


    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        meuData += '&ListaBorderosPGTO%5B' + index + '%5D.codigoBordero=' + $(value).find('.CODBORDERO').find('input').val();
    });

    var data = "codigoEmpresa=" + $("#codEmpresa").val()
        + "&codigoContaCorrente=" + $("#codContaCorrente").val()
        + "&DadosContaCorrente=" + $('.codContaCorrente option:selected').text()
        + meuData;

    var _url = montaUrl("/ContasPagar/GerarArquivoRemessa/GeraArquivo")


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {
                $("#CaminhoArquivo").val(data.responseText);
                var urlDownload = montaUrl("/ContasPagar/GerarArquivoRemessa/DownloadArquivoRemessa/")
                urlDownload += "&id=" + data.responseText
                urlDownload += "&codigoEmpresa=" + $("#codEmpresa").val()

                $('#btnDownload').attr({
                    href: urlDownload
                });

                $('#btnDownload').show();
                $('#btnDownload').removeAttr('disabled');
                $("#btnDownload").removeClass('btn-default');
                $("#btnDownload").addClass('btn-success');

                $.confirm({
                    title: 'Sucesso!',
                    icon: "glyphicon glyphicon-ok",
                    content: 'Arquivo gerado com sucesso. Deseja realizar o download?',
                    type: 'green',
                    buttons: {
                        confirm: {
                            text: 'Sim',
                            btnClass: 'btn-green',
                            keys: ['enter'],
                            action: function () {

                                var caminho = urlDownload;

                                window.location.href = caminho;
                            }
                        },
                        cancel: {
                            text: 'Não'
                        }
                    }
                });


                var ocultar = $('input[type="checkbox"]:checked', table.cells().nodes());
                $(ocultar).each((i, e) => $(e).closest('tr').addClass('selected'));
                table.rows('.selected').remove().draw(false);

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

function EditarTitulo(codFechCred) {

    var url = montaUrl("/ContasPagar/GerarArquivoRemessa/EdicaoTitulo/")
    url += ("&codFechCred=" + codFechCred)

    $("#modalEditarTitulo").load(url);
    $("#EditarTitulo").modal();
}
function AlterarTituloPGTO() {

    var data = new Date();
    var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)
    var datGrid = $("#DATPGTO").val().split('/');
    var DataFormatadaGrid = new Date(`${datGrid[2]},${datGrid[1]},${datGrid[0]}`);

    var url = montaUrl("/ContasPagar/GerarArquivoRemessa/SalvaEdicaoTitulo/");

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


    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);


                var TableTitulos = $('#GridTransacao').DataTable()
                var dataTitulo = "";
                var ExisteTitVencido = false;

                var trs = $(TableTitulos.cells().nodes()).parent();
                $(trs).each(function (index, value) {

                    if ($("#CODIGOTITULO").val() == $(value).find('.Codigo').find('input').val()) {
                        $(value).find('.dtAgendamento')[0].innerText = $("#DATPGTO").val();

                            $(value).find('.dtAgendamento')[0].style.backgroundColor = "#ffffff"
                            $(value).find('.btnEdit')[0].style.backgroundColor = "#ffffff"
                            $(value).find('.CODFAVORECIDO')[0].style.backgroundColor = "#ffffff"
                            $(value).find('.NMFAVORECIDO')[0].style.backgroundColor = "#ffffff"
                            $(value).find('.VALLIQ')[0].style.backgroundColor = "#ffffff"
                            $(value).find('.CODBANCO')[0].style.backgroundColor = "#ffffff"
                    }
                    else {
                        var datGrid = $(value).find('.dtAgendamento')[0].innerText.split('/');
                        var DataFormatadaGrid = new Date(`${datGrid[2]},${datGrid[1]},${datGrid[0]}`);
                        if (DataFormatadaGrid < dataAtual) {
                            ExisteTitVencido = true;
                        }
                    }

                });

                if (!ExisteTitVencido) {
                    var tableBordero = $('#gridBordero').DataTable();

                    var trsBordero = $(tableBordero.cells().nodes()).parent();
                    $(trsBordero).each(function (index, value) {
                        $(value).find('.inpchkFechCredPai').prop('disabled', false);
                        $(value).find('.TITVENCIDO').find('input').val('N');

                        $(value).find('.bntVisualizaFech')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.chkFechCred')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.inpCODBORDERO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.inpFORMAPGTO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.inpVLBORDERO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.inpCODBANCO')[0].style.backgroundColor = "#ffffff"
                        $(value).find('.inpDTBORDERO')[0].style.backgroundColor = "#ffffff"


                    });
                }



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

function AtualizaValorArquivo() {
    var ValorArquivo = 0.00;

    var TableTitulos = $('#gridBordero').DataTable();

    var trs = $(TableTitulos.cells().nodes()).parent();
    $(trs).each(function (index, value) {
        if ($('input[type="checkbox"]:checked', $(value).find('.CheckBox-Grid')[0]).val() == 'on') {
            ValorArquivo = ValorArquivo + getMoney($(value).find('.INPVALLIQ')[0].innerText)
        }

    });

    $("#ValorArquivo").val(formatReal(ValorArquivo));

}
