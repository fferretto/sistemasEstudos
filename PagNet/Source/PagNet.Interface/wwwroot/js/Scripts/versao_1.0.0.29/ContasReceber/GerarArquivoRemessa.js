

function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
}

$(window).load(carregaPagina());


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});
$(".codContaCorrente").change(function () {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
});


function ConsultarBordero() {


    $('#modal').empty();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());


    var url = montaUrl("/ContasReceber/GerarArquivoRemessa/ConsultaBordero/?codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codBordero=" + $("#codBordero").val());

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaBordero").show();


};

function VisualizarFechamentos(CODBORDERO) {

    var url = montaUrl("/ContasReceber/GerarArquivoRemessa/ConsultaBoletosBordero/");

    url += "&CODBORDERO=" + CODBORDERO


    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}

function GeraArquivoRemessa() {

    var table = $('#gridBorderoBol').DataTable();


    var contacorrente = $('.codContaCorrente option:selected').val();
    var codigoEmpresa = $('.codigoEmpresa option:selected').val();

    if (contacorrente == '' || contacorrente == '-1' || contacorrente == "null" || contacorrente == null) {
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
        return false;
    }

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {

        meuData += '&ListaBordero%5B' + index + '%5D.codigoBordero=' + $(value).find('.CODBORDERO').find('input').val();

    });

    var data = "codContaCorrente=" + contacorrente
        + "&codigoEmpresa=" + codigoEmpresa
             + meuData;


    var _url = montaUrl("/ContasReceber/GerarArquivoRemessa/GeraArquivo")


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {
                $("#CaminhoArquivo").val(data);

                $('#btnDownload').attr({
                    href: montaUrl("/ContasReceber/GerarArquivoRemessa/DownloadArquivoRemessa/" + data.responseText)
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
                                var caminho = montaUrl("/ContasReceber/GerarArquivoRemessa/DownloadArquivoRemessa/?url=" + data.responseText);

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
function ValidaBoletosTituloVencidoLoad() {

    var table = $('#gridBorderoBol').DataTable();

    var TITVENCIDO;
    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (index, value) {

        TITVENCIDO = $(value).find('.POSSUIBOLETOVENCIDO').find('input').val();

        if (TITVENCIDO == "S") {
            msgAviso("Existem boletos vencidos em um ou mais borderôs. E por este motivo não será possível incluir os mesmos no arquivo de remessa.")
            return false;
        }

    });
};
function SelectAllgridBordero(Checado) {
    var table = $('#gridBorderoBol').DataTable();
    var TITVENCIDO;

    var trsBordero = $(table.cells().nodes()).parent();
    $(trsBordero).each(function (index, value) {

        TITVENCIDO = $(value).find('.POSSUIBOLETOVENCIDO').find('input').val();

        if (TITVENCIDO == "N") {
            $(value).find('.inpchkBordero').prop('checked', Checado);
        }

    });
};

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
function AtualizaValorArquivo() {
    var qtSelecionados = 0;
    var ValorBordero = 0.00;

    var TableTitulos = $('#gridBorderoBol').DataTable();

    var trs = $(TableTitulos.cells().nodes()).parent();
    $(trs).each(function (index, value) {
        if ($('input[type="checkbox"]:checked', $(value).find('.CheckBox-Grid')[0]).val() == 'on') {
            qtSelecionados = qtSelecionados + 1;
            ValorBordero = ValorBordero + getMoney($(value).find('.inpVLBORDERO')[0].innerText)
        }

    });

    $("#qtBorderosSelecionados").val(qtSelecionados);
    $("#ValorTotalArquivo").val(formatReal(ValorBordero));

}