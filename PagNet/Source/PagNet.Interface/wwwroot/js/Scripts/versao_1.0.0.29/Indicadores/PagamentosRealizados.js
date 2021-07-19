
function carregaPagina() {

    $("#nmCredenciado").prop("disabled", "disabled")
    $("#FiltroNmBanco").prop("disabled", "disabled")

    if ($("#acessoAdmin").val() == 'False') {
        $(".codigoEmpresa").prop('disabled', true);
    }

}

$(window).load(carregaPagina());


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});


function ConsultarTransacaoo() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var urlListDespesasPeriodo = montaUrl("/Indicadores/PagamentosRealizados/GraficoPagamentosPeriodo/");
    var urlListTransacaoPag = montaUrl("/Indicadores/PagamentosRealizados/GraficoPagamentoMensal/");

    urlParametros = "&dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&codCredenciado=" + $("#codCredenciado").val()
        + "&CodSubRede=" + $("#CodSubRede").val()

    $("#modalListDespesasPeriodo").load(urlListDespesasPeriodo + urlParametros);
    $("#ListDespesasPeriodo").show({ backdrop: true, keyboard: false, show: false });

    $("#modalListTransacaoPag").load(urlListTransacaoPag + urlParametros);
    $("#ListTransacaoPag").show({ backdrop: true, keyboard: false, show: false });


};

$('#filtroCodBanco').focusout(function () {
    var filtro = $('#filtroCodBanco').val();
    if (filtro != "") {

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

$('#filtroCredenciado').focusout(function () {
    var filtro = $('#filtro').val();
    if (filtro != "") {
        var url = montaUrl('/Generico/CadastrosDiversos/BuscaCredenciado/?filtro=' + filtro);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                if (data.length > 0) {
                    PreencheCredenciado(data);
                } else {
                    addErros("nmCredenciado", "Credenciado não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                msgErro("Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtroCredenciado").val('');
        $("#nmCredenciado").val('');
        $("#codCredenciado").val(0)
    }
})


function PreencheCredenciado(data) {
    var arr = data.split("/");

    $("#filtroCredenciado").val(arr[0]);
    $("#nmCredenciado").val(arr[1]);
    $("#codCredenciado").val(arr[2])
};

function numberParaReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}
//-----------------------------------------------------------------------------------------------------------//
//-------------------------------------------GraficoPagamentosPeriodo----------------------------------------//
//-----------------------------------------------------------------------------------------------------------//
function GraficoDespesaPeriodo() {

    var url = montaUrl("/Indicadores/PagamentosRealizados/IndicadorPagamentoPeriodo/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&codCredenciado=" + $("#codCredenciado").val()
        + "&CodSubRede=" + $("#CodSubRede").val())

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                ChartValorPagamentoDia(data);
                return false;
            }
            else {
                msgErro(data.result)
            }
        }
    })
    return false;
}


function ChartValorPagamentoDia(data) {
    $.unblockUI();

    var chartdata = new google.visualization.DataTable();
    chartdata.addColumn('string', 'Data de Pagamento');
    chartdata.addColumn('number', 'Valor Pago');
    chartdata.addColumn('number', 'Taxa Retida');

    $.each(data, function (i, item) {
        var vlDespesa = numberParaReal(item.vlDespesas);
        var vlTaxa = numberParaReal(item.vlTaxas);
        chartdata.addRows([[item.dtReferencia, { v: item.vlDespesas, f: vlDespesa }, { v: item.vlTaxas, f: vlTaxa }]]);
    });

     var options = {
        title: 'Valores Pagos no Período',
        height: 300,
        chartArea: {
            width: '70%'
        },
        //colors: ['#b0120a', '#2f7fc3'],
        hAxis: {
            title: 'Data de Pagamento',
            viewWindow: {
                min: [7, 30, 0],
                max: [17, 30, 0]
            },
            minValue: 0,
            direction: -1,
            slantedText: true,
            slantedTextAngle: 45,

        },
        vAxis: {
            title: 'Valores'
        }
    };

    var chart1_chart = new google.charts.Bar(document.getElementById('Chart_pag_periodo'));
    chart1_chart.draw(chartdata, options);
    return false;
}

//-----------------------------------------------------------------------------------------------------------//
//-------------------------------------------GraficoPagamentoMensal------------------------------------------//
//-----------------------------------------------------------------------------------------------------------//
function GraficoDespesaAno() {

    var url = montaUrl("/Indicadores/PagamentosRealizados/IndicadorPagamentoMensal/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&codCredenciado=" + $("#codCredenciado").val()
        + "&CodSubRede=" + $("#CodSubRede").val());

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            if (data != null) {
                ChartValorPagamentoAno(data);
                return false;
            }
            else {
                msgErro(data.result)
            }
        }
    })
    return false;
}


function ChartValorPagamentoAno(data) {
    $.unblockUI();

    var chartdata = new google.visualization.DataTable();
    chartdata.addColumn('string', 'Mês Referencia');
    chartdata.addColumn('number', 'Valor Pago');
    chartdata.addColumn('number', 'Taxa Retida');

    $.each(data, function (i, item) {
        var vlDespesa = numberParaReal(item.vlDespesas);
        var vlTaxa = numberParaReal(item.vlTaxas);
   
        chartdata.addRows([[item.mesRef, { v: item.vlDespesas, f: vlDespesa }, { v: item.vlTaxas, f: vlTaxa }]]);
    });

    var options = {
        title: 'Valores Pagos nos últimos 12 Meses',
        height: 300,
        chartArea: {
            width: '70%'
        },
        //colors: ['#b0120a', '#2f7fc3'],
        hAxis: {
            title: 'Mês Referencia',
            viewWindow: {
                min: [7, 30, 0],
                max: [17, 30, 0]
            },
            minValue: 0,
            direction: -1,
            slantedText: true,
            slantedTextAngle: 45,

        },
        vAxis: {
            title: 'Valores'
        }
    };

    var chart1_chart = new google.charts.Bar(document.getElementById('Chart_pag_Mensal'));
    chart1_chart.draw(chartdata, options);
    return false;
}