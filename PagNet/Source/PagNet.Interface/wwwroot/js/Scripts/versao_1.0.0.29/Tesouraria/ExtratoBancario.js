function carregaPagina() {

    if ($('#acessoAdmin').val() == "False") {
        $('.codigoEmpresa').prop('disabled', true);
    }
}

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});
$(".codContaCorrente").change(function () {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
});

function ConsultarExtrato() {

    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    var contacorrente = $("#codContaCorrente").val();

    if (contacorrente == '' || contacorrente == '-1' || contacorrente == "null" || contacorrente == null) {
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
        return false;
    }
    ListaMaioresReceitas();
    ListaMaioresDespesas();
    DadosContaCorrente();
    ListaExtratoBancario();
}
function ListaMaioresReceitas() {

    $('#modalListaMaioresReceitas').empty();
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var contacorrente = $("#codContaCorrente").val();

    var url = montaUrl("/Tesouraria/ExtratoBancario/ListaMaioresReceitas/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codContaCorrente=" + contacorrente)


    $("#modalListaMaioresReceitas").load(url);
    $("#ListaMaioresReceitas").show();
};
function ListaMaioresDespesas() {

    $('#modalListaMaioresDespesas').empty();
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var contacorrente = $("#codContaCorrente").val();

    var url = montaUrl("/Tesouraria/ExtratoBancario/ListaMaioresDespesas/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codContaCorrente=" + contacorrente)


    $("#modalListaMaioresDespesas").load(url);
    $("#ListaMaioresDespesas").show();
};
function DadosContaCorrente() {

    $('#modalDadosContaCorrente').empty();
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var contacorrente = $("#codContaCorrente").val();

    var url = montaUrl("/Tesouraria/ExtratoBancario/BuscaSaldoContaCorrente/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codContaCorrente=" + contacorrente)


    $("#modalDadosContaCorrente").load(url);
    $("#DadosContaCorrente").show();
};
function ListaExtratoBancario() {

    $('#modalListaExtratoBancario').empty();
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    var contacorrente = $("#codContaCorrente").val();

    var url = montaUrl("/Tesouraria/ExtratoBancario/ListaExtratoBancario/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codContaCorrente=" + contacorrente)


    $("#modalListaExtratoBancario").load(url);
    $("#ListaExtratoBancario").show();
};