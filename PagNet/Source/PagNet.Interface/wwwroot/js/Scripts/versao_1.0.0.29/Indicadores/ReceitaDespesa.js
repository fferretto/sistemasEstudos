
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'False') {
        $(".CodSubRede").prop('disabled', true);
    }
    else {
        $("#chkSubRede").show();
    }

}

$(window).load(carregaPagina());


$(".CodSubRede").change(function () {
    $("#CodSubRede").val($('.CodSubRede option:selected').val());
});


function ConsultarTransacaoo() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var urlReceitaDespesa = montaUrl("/Indicadores/ReceitaDespesa/GraficoReceitaDespesa/");
    var urlDespesa = montaUrl("/Indicadores/ReceitaDespesa/GraficoPagamentoPeriodo/");
    var urlReceita = montaUrl("/Indicadores/ReceitaDespesa/GraficoReceitaPeriodo/");

    urlParametros = "&dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&cartaoPos=" + $("#cartaoPos").val()
        + "&cartaoPre=" + $("#cartaoPre").val();

    $("#modalReceitaDespesa").load(urlReceitaDespesa + urlParametros);
    $("#ListReceitaDespesa").show({ backdrop: true, keyboard: false, show: false });

    $("#modalPag").load(urlDespesa + urlParametros);
    $("#ListTransacaoPag").show({ backdrop: true, keyboard: false, show: false });

    $("#modalReceita").load(urlReceita + urlParametros);
    $("#ListTransacaoReceita").show({ backdrop: true, keyboard: false, show: false });

};

function numberParaReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}
//-----------------------------------------------------------------------------------------------------------//
//-------------------------------------------GraficoPagamentoPeriodo-----------------------------------------//
//-----------------------------------------------------------------------------------------------------------//


function GraficoDespesaPeriodo() {

    var url = montaUrl("/Indicadores/ReceitaDespesa/IndicadorPagamentoPeriodo/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&chkTodasSubRedes=" + $('#chkTodasSubRedes').prop('checked'));

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            ChartValorPagamentoDia(data);
            return false;
        }
    })
    return false;
}


function ChartValorPagamentoDia(data) {
    $.unblockUI();
    var dataArray = [
        ['Data Fechamento', 'Valor Pago', 'Taxa Retida']
    ];
    $.each(data, function (i, item) {
        dataArray.push([item.dtPagameno, item.vlPago, item.vlTaxa]);
    });
    var data = google.visualization.arrayToDataTable(dataArray);
    var options = {
        title: 'Valores Pagos por Data de Fechamento no Período',
        //height: 300,
        //chartArea: {
        //    width: '70%'
        //},
        //colors: ['#b0120a', '#2f7fc3'],
        hAxis: {
            title: 'Data de Fechamento',
            minValue: 0,
            direction: -1,
            slantedText: true,
            slantedTextAngle: 45,

        },
        vAxis: {
            title: 'Valores'
        }
    };

    var chart1_chart = new google.visualization.LineChart(document.getElementById('Chart_pag_periodo'));
    chart1_chart.draw(data, options);
    return false;
}

//-----------------------------------------------------------------------------------------------------------//
//-------------------------------------------GraficoReceitaDespesa-------------------------------------------//
//-----------------------------------------------------------------------------------------------------------//

function GraficoReceitaDespesa() {

    var url = montaUrl("/Indicadores/ReceitaDespesa/IndicadorReceitaDespesa/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&chkTodasSubRedes=" + $('#chkTodasSubRedes').prop('checked'));

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            ChartReceitaDespesaAno(data);
            return false;
        }
    })
    return false;
}

function ChartReceitaDespesaAno(data) {
    $.unblockUI();

    var dataReceitaDespesa = new google.visualization.DataTable();
    dataReceitaDespesa.addColumn('string', 'Mes Referencia');
    dataReceitaDespesa.addColumn('number', 'Valor Recebido');
    dataReceitaDespesa.addColumn('number', 'Valor Pago');

    $.each(data, function (i, item) {
        dataReceitaDespesa.addRow([item.mesref, item.receita, item.despesa]);
    });

    var optionsReceitaDespesa = {
        title: 'Comparativo de Receita e Despesa nos últimos 12 Meses',
        chartArea: {
            width: '65%'
        },
        //colors: ['#b0120a', '#2f7fc3'],
        hAxis: {
            title: 'Mês Referência',
            minValue: 0,
            direction: -1,
            slantedText: true,
            slantedTextAngle: 45,
        },
        vAxis: {
            title: 'Valores'
        }
    };
    var chartReceitaDespesa = new google.visualization.LineChart(document.getElementById('Chart_ReceitaDespesa'));
    chartReceitaDespesa.draw(dataReceitaDespesa, optionsReceitaDespesa);
    return false;
}

//-----------------------------------------------------------------------------------------------------------//
//-------------------------------------------GraficoReceitaPeriodo-------------------------------------------//
//-----------------------------------------------------------------------------------------------------------//


function GraficoReceita() {

    var url = montaUrl("/Indicadores/ReceitaDespesa/IndicadorReceitaPeriodo/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&CodSubRede=" + $("#CodSubRede").val()
        + "&chkTodasSubRedes=" + $('#chkTodasSubRedes').prop('checked'));

    $.ajax({
        type: "GET",
        url: url,
        contentType: "application/json",
        dataType: "json",
        success: function (data) {
            ChartValorRecebimentoDia(data);
            return false;
        }
    })
    return false;
}
function ChartValorRecebimentoDia(data) {
    $.unblockUI();
    var dataArray = [
        ['Data Fechamento', 'Valor Recebido']
    ];
    $.each(data, function (i, item) {
        dataArray.push([item.dtReceita, item.vlRecebido]);
    });
    var dataReceita = google.visualization.arrayToDataTable(dataArray);
    var optionsReceita = {
        title: 'Valores Recebidos por Data de Fechamento no Período',
        chartArea: {
            width: '65%'
        },
        //colors: ['#b0120a', '#2f7fc3'],
        hAxis: {
            title: 'Data de Fechamento',
            minValue: 0,
            direction: -1,
            slantedText: true,
            slantedTextAngle: 45,
        },
        vAxis: {
            title: 'Valores'
        }
    };
    var chartReceita = new google.visualization.LineChart(document.getElementById('Chart_Receita'));
    chartReceita.draw(dataReceita, optionsReceita);
    return false;
}