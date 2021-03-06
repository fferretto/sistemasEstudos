function carregaPagina() {
    console.log($("#acessoAdmin").val())
    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    $("#nomeCliente").prop('disabled', true);

}

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});
$(".codigoFatura").change(function () {
    $("#codigoFatura").val($('.codigoFatura option:selected').val());
});

$("#btnProcessarArquivo").click(function () {
    if ($("#VisualizarFaturasNaoDescontadas").val() == 'True') {

        $('#VisualizarFaturasNaoDescontadas').val("False");
        $('#modalListaPedidos').empty();
        $("#ListaPedidos").hide();
    }
});
$("#btnUsuarioNaoDescontados").click(function () {
    if ($("#VisualizarFaturasNaoDescontadas").val() == 'False') {
        $('#VisualizarFaturasNaoDescontadas').val("True");
        $('#modalListaPedidos').empty();
        $("#ListaPedidos").hide();
    }
});


$('#filtroCliente').focusout(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    var filtro = $('#filtroCliente').val();

    if (filtro != "") {
        var _url = montaUrl('/ContasReceber/DescontoFolha/BuscarCliente/?filtro=' + filtro)
        _url += "&codempresa=" + $("#codigoEmpresa").val();

        $.ajax({
            type: 'get',
            url: _url,
            dataType: 'json',
            success: function (data) {
                if (data.codcliente != null && data.codcliente > 0) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);
                    $('.codigoFatura').prop('disabled', false);

                } else {
                    $("#nomeCliente").val("Cliente não encontrado");
                    $('.codigoFatura').prop('disabled', true);
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
        $("#CodigoCliente").val(0)
    }
})

function LocalizaCliente() {

    var filtro = '?codempresa=' + $('.codigoEmpresa option:selected').val()
    var url = montaUrl("/ContasReceber/DescontoFolha/ConsultaTodosClientes/" + filtro)

    $("#modalCliente").load(url);
    $("#LocalizaCliente").modal();
}
function SelecionaCliente(codCliente, codigoEmpresa) {

    if (codCliente != "") {
        var filtro = '?filtro=' + codCliente + '&codempresa=' + codigoEmpresa

        var url = montaUrl('/ContasReceber/DescontoFolha/BuscarCliente/' + filtro);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#LocalizaCliente .close").click();
                if (data != null) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);
                    $('.codigoFatura').prop('disabled', false);

                } else {
                    $("#nomeCliente").val("Cliente não encontrado");
                    $('.codigoFatura').prop('disabled', true);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("filtroCliente", "Erro interno. Tente novamente mais tarde.");
                $("#DadosCliente").hide();
            }
        });
    }
    else {
        $("#filtroCliente").val('');
        $("#CodigoCliente").val('');
        $("#nomeCliente").val('');
    }

}


function ProcessarArquivoCliente() {

    $("#codigoFatura").val($('.codigoFatura option:selected').val());
    var codFatura = $("#codigoFatura").val()

    if (codFatura == '' || codFatura == '-1' || codFatura == "null" || codFatura == null) {
        msgAviso("Obrigatório informar a fatura.");
        $("#ValidacodigoFatura").show();
        return false;
    }

    if ($("#VisualizarFaturasNaoDescontadas").val() == 'True') {
        $('#modalListaPedidos').empty();

        $("#codigoFatura").val($('.codigoFatura option:selected').val());
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

        var url = montaUrl("/ContasReceber/DescontoFolha/ConsolidaArquivoRetornoCliente/")
        url += "&codFatura=" + $("#codigoFatura").val();
        url += "&ValidaArquivo=False";
        


        $("#modalListaPedidos").load(url);
        $("#ListaPedidos").show();
    }
    else {

        var myfile = document.getElementById("uploadFileCliente");
        var files = myfile.files;
        var formData = new FormData();


        if (files.length > 0) {
            $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

            formData.append('file', $('#uploadFileCliente')[0].files[0]); // myFile is the input type="file" control

            var _url = montaUrl("/ContasReceber/DescontoFolha/UploadArquivoValidacaoRetorno/");

            $.ajax({
                url: _url,
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    $.unblockUI();
                    if (data.success) {
                        $('#modalListaPedidos').empty();
                        
                        $("#codigoFatura").val($('.codigoFatura option:selected').val());
                        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                        var url = montaUrl("/ContasReceber/DescontoFolha/ConsolidaArquivoRetornoCliente/")
                        url += "&CaminhoArquivo=" + data.responseText;
                        url += "&codFatura=" + $("#codigoFatura").val();
                        url += "&ValidaArquivo=True";
                        
                        $("#modalListaPedidos").load(url);
                        $("#ListaPedidos").show();
                    }
                    else {
                        msgErro(data.responseText)
                    }
                },
                error: function (data) {
                    $.unblockUI();
                    console.log(data)
                    msgErro("Falha ao tentar ler o arquivo. Verifique se o mesmo está no formato correto.")

                }
            })
        }
        else {
            msgAviso("Favor selecionar um arquivo antes de continuar.")
        }
    }
};
function SelectGeraFaturaUsuario(matricula) {

    var table = $('#GridListaClienteUsuario').DataTable();
    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if ($(e).find('.Matricula').find('input').val() == matricula.toString()) {

            //$(e).find('.inpchkProximaFatura').prop('checked', false);
            $(e).find('.inpchkBaixarFatura').prop('checked', false);
            return false;
        }
    });
    RecalculaItensPendentes();
};
function SelectProximaFatura(matricula) {

    var table = $('#GridListaClienteUsuario').DataTable();
    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if ($(e).find('.Matricula').find('input').val() == matricula.toString()) {

            $(e).find('.inpchkGeraFaturaUsuario').prop('checked', false);
            $(e).find('.inpchkBaixarFatura').prop('checked', false);
            return false;
        }
    });
    RecalculaItensPendentes();
};
function SelectBaixarFatura(matricula) {

    var table = $('#GridListaClienteUsuario').DataTable();
    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if ($(e).find('.Matricula').find('input').val() == matricula.toString()) {

            //$(e).find('.inpchkProximaFatura').prop('checked', false);
            $(e).find('.inpchkGeraFaturaUsuario').prop('checked', false);
            return false;
        }
    });
    RecalculaItensPendentes();
};
function AtualizaitensDesvinculados() {

    var table = $('#GridListaClienteUsuario').DataTable();
    var ValorTotal = 0.00;
    var qtTotal = 0;

    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if (!$(e).find('.inpchkGeraFaturaUsuario')[0].checked && !$(e).find('.inpchkBaixarFatura')[0].checked) {
            qtTotal += 1;
            ValorTotal += ConvertDecimal($(e).find('.Valor').find('input').val());
        }
    });

    var valorFormatado = "R$ " + formatReal(ValorTotal);
    $('#qtNaoDefinidos').val(qtTotal)
    $('#vlNaoDefinido').val(valorFormatado)
};
function AtualizaItensNovaFatura() {

    var table = $('#GridListaClienteUsuario').DataTable();
    var ValorTotal = 0.00;
    var qtTotal = 0;

    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if ($(e).find('.inpchkGeraFaturaUsuario')[0].checked) {
            qtTotal += 1;
            ValorTotal += ConvertDecimal($(e).find('.Valor').find('input').val());
        }
    });

    var valorFormatado = "R$ " + formatReal(ValorTotal);
    $('#qtGeraFatura').val(qtTotal)
    $('#vlGeraFatura').val(valorFormatado)
};
function AtualizaItensACancelar() {

    var table = $('#GridListaClienteUsuario').DataTable();
    var ValorTotal = 0.00;
    var qtTotal = 0;

    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if ($(e).find('.inpchkBaixarFatura')[0].checked) {
            qtTotal += 1;
            ValorTotal += ConvertDecimal($(e).find('.Valor').find('input').val());
        }
    });

    var valorFormatado = "R$ " + formatReal(ValorTotal);
    $('#qtBaixaAutomatica').val(qtTotal)
    $('#vlBaixaAutomatica').val(valorFormatado)
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

function RecalculaItensPendentes() {
    AtualizaitensDesvinculados();
    AtualizaItensNovaFatura();
    AtualizaItensACancelar();
}
function ProximaFatura(index) {
    var table = $('#gridValidaViaArquivo').DataTable();

    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if (i == index) {
            $(e).find('.Acao').find('input').val('PROXIMAFATURA')
        }
    });
};
function DescontoDireto(index) {
    var table = $('#gridValidaViaArquivo').DataTable();

    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if (i == index) {
            $(e).find('.Acao').find('input').val('COBRANCADIRETA')
        }
    });
};
function PerdoarDivida(index) {
    var table = $('#gridValidaViaArquivo').DataTable();

    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {
        if (i == index) {
            $(e).find('.Acao').find('input').val('PERDOARDIVIDA')
        }
    });
};
function GerarFaturamentosByArquivoCliente() {

    var table = $('#gridValidaViaArquivo').DataTable();
    var ListaD = '';


    var trs = $(table.cells().nodes()).parent();
    $(trs).each(function (i, e) {

        if ($(e).find('.Acao').find('input').val() == "") {
            msgAviso("Existe um ou mais usuários que não foram definida a ação a ser tomada.")
            return false;
        }
        ListaD += '&ListaUsuarios%5B' + i + '%5D.CPF=' + $(e).find('.CPF').find('input').val();
        ListaD += '&ListaUsuarios%5B' + i + '%5D.Acao=' + $(e).find('.Acao').find('input').val();
    });

    var meuData = 'codigoCliente=' + $("#CodigoCliente").val();
    meuData += '&codigoFatura=' + $("#codigoFatura").val();
    meuData += ListaD;

    var _url = montaUrl("/ContasReceber/DescontoFolha/ProcessaDescontoFolha")

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: meuData,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var ocultar = $(table.cells().nodes()).parent();
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
