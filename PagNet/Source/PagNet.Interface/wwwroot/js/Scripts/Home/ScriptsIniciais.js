
/*---------------------------scripts para visualizar itens do alerta no header do sistema------------------------------------------*/

function FechaJanelaModal() {
    $("#JanelaModal").hide();
}

function FuncTotalTitulosVencido() {
    $('#modalStatusItens').empty();
    var filtro = "?codEmpresa=0";
    filtro += "&status=VENCIDO";

    var url = montaUrl("/Home/ListaTitulosNaoLiquidado/" + filtro)

    $("#modalStatusItens").load(url);
    $("#JanelaModal").show();
}
function FuncTotalTitulosPendBaixa() {
    $('#modalStatusItens').empty();

    var filtro = "?codEmpresa=0";
    filtro += "&status=PENDENTERETORNO";

    var url = montaUrl("/Home/ListaTitulosNaoLiquidado/" + filtro)

    $("#modalStatusItens").load(url);
    $("#JanelaModal").show();
}


function FuncTotalFaturamentoPendRegistro() {
    $('#modalStatusItens').empty();

    var filtro = "?codEmpresa=0";
    filtro += "&status=PENDENTEREGISTRO";

    var url = montaUrl("/Home/ListaFaturamentoNaoLiquidado/" + filtro)

    $("#modalStatusItens").load(url);
    $("#JanelaModal").show();
}


function FuncTotalFaturamentoPendBaixa() {
    $('#modalStatusItens').empty();

    var filtro = "?codEmpresa=0";
    filtro += "&status=REGISTRADO";

    var url = montaUrl("/Home/ListaFaturamentoNaoLiquidado/" + filtro)

    $("#modalStatusItens").load(url);
    $("#JanelaModal").show();
}

function FuncTotalFaturamentoVencido() {
    $('#modalStatusItens').empty();

    var filtro = "?codEmpresa=0";
    filtro += "&status=VENCIDO";

    var url = montaUrl("/Home/ListaFaturamentoNaoLiquidado/" + filtro)

    $("#modalStatusItens").load(url);
    $("#JanelaModal").show();
}