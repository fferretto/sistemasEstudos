function carregaPagina() {

    $("#nomeCliente").prop("disabled", true)

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
}

$(window).load(carregaPagina());

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});
$(".codigoContaCorrente").change(function () {
    $("#codigoContaCorrente").val($('.codigoContaCorrente option:selected').val());
});


function LocalizaCliente() {

    var filtro = '?codempresa=' + $('.codigoEmpresa option:selected').val()
    var url = montaUrl("/ContasReceber/GerarBordero/ConsultaTodosClientes/" + filtro)

    $("#modalCliente").load(url);
    $("#LocalizaCliente").modal();
}

function SelecionaCliente(codCliente, codigoEmpresa) {

    if (codCliente != "") {
        var filtro = '?filtro=' + codCliente + '&codempresa=' + codigoEmpresa

        var url = montaUrl('/ContasReceber/GerarBordero/BuscaCliente/') + filtro;

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

                } else {
                    addErros("filtroCliente", "Cliente não encontrado");
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

$('#filtroCliente').focusout(function () {
    var filtroCliente = $('#filtroCliente').val()

    if (filtroCliente != "") {

        var filtro = '?filtro=' + filtroCliente
            + '&codempresa=' + $('.codigoEmpresa option:selected').val()

        $.ajax({
            type: 'get',
            url: montaUrl('/ContasReceber/GerarBordero/BuscaCliente/' + filtro),
            dataType: 'json',
            success: function (data) {
                if (data.codcliente != null && data.codcliente > 0) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);

                } else {
                    $("#nomeCliente").val("Cliente não encontrado");
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

function FuncConsultaSolicitacaoBoletos() {


    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#codigoContaCorrente").val($('.codigoContaCorrente option:selected').val());

    $('#modalSolicitacaoBoleto').empty();

    var url = montaUrl("/ContasReceber/GerarBordero/ConsultaSolicitacaoBoleto/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodigoCliente=" + $("#CodigoCliente").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codigoContaCorrente=" + $("#codigoContaCorrente").val());


    if (isValidaBusca()) {
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
        $("#modalSolicitacaoBoleto").load(url);
        $("#SolicitacaoBoleto").show();
    }

};
function isValidaBusca() {
    var valido = true;

    var data = new Date();
    var dtInicio = $("#dtInicio").val().split("/");
    var dtFim = $("#dtFim").val().split("/");
    var dataIni = new Date(`${dtInicio[2]},${dtInicio[1]},${dtInicio[0]}`);
    var dataFim = new Date(`${dtFim[2]},${dtFim[1]},${dtFim[0]}`);
    var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)


    if (dataIni < dataAtual) {
        valido = false;
        msgAviso("A data de início não pode ser inferior a data atual.");
    }

    return valido;

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

//-------------------------------------------IListaSolicitacaoBoleto---------------------


$(".codigoContaCorrente").change(function () {
    $("#codigoContaCorrente").val($('.codigoContaCorrente option:selected').val());
});
function SalvaBordero() {

    var table = $('#gridSolicitacaoBoleto').DataTable();

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var codigoEmpresa = $('#codigoEmpresa').val();

    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {

        meuData += '&ListaBoletos%5B' + index + '%5D.codEmissaoBoleto=' + $(value).find('.codEmissaoBoleto').find('input').val();
        meuData += '&ListaBoletos%5B' + index + '%5D.Valor=' + $(value).find('.Valor').find('input').val();
    });

    var data = 'codEmpresa=' + codigoEmpresa
        + meuData;

    var url = montaUrl("/ContasReceber/GerarBordero/SalvaBordero/");

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                var ocultar = $('input[type="checkbox"]:checked', table.cells().nodes());
                $(ocultar).each((i, e) => $(e).closest('tr').addClass('selected'));
                table.rows('.selected').remove().draw(false);

                msgSucesso(data.responseText);

            }
            else {
                msgErro(data.responseText)
            }
        }
    });

    // Prevent form submission
    return false;
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
function AtualizaValorBorderoBoleto() {
    var qtSelecionados = 0;
    var ValorBordero = 0.00;

    var TableTitulos = $('#gridSolicitacaoBoleto').DataTable();

    var trs = $(TableTitulos.cells().nodes()).parent();
    $(trs).each(function (index, value) {
        if ($('input[type="checkbox"]:checked', $(value).find('.CheckBox-Grid')[0]).val() == 'on') {
            qtSelecionados = qtSelecionados + 1;
            ValorBordero = ValorBordero + getMoney($(value).find('.INPVALOR')[0].innerText)
        }

    });

    $("#qtFaturasSelecionados").val(qtSelecionados);
    $("#ValorBordero").val(formatReal(ValorBordero));

}