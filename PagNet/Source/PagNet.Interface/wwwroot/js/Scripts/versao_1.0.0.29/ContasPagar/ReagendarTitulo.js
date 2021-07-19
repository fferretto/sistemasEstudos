
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
    CarregaGridTitulosVencidos();
}

$(window).load(carregaPagina());


$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());

});


function CarregaGridTitulosVencidos() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $('#modal').empty();

    var url = montaUrl("/ContasPagar/ReagendarTitulo/CarregaGridTitulosVencidos/?codEmpresa=" + $("#codEmpresa").val()
        + "&dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val())

    $("#modal").load(url);
    $("#ListTitulosVencidos").show();
};

function JustificarTitulosEmMassa() {

    var dtTransferencia = $("#dtTransferencia").val().split("/");
    var dataAtual = new Date();
    dataAtual.setDate(dataAtual.getDate() - 1);
    var newData = new Date(`${dtTransferencia[2]},${dtTransferencia[1]},${dtTransferencia[0]}`);

    if (newData <= dataAtual) {
        msgAviso("A nova data de pagamento não pode ser menor que a data atual.");
        return false;
    }

    var table = $('#GridTitulosVencidos').DataTable();
    var ExisteItensBaixa = false;

    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        ExisteItensBaixa = true;
    });

    if (!ExisteItensBaixa) {
        msgAviso("obrigatório selecionar ao menos título para realizar a alteração.");
    }
    else {
        var url = montaUrl("/ContasPagar/ReagendarTitulo/JustificarTitulosEmMassa")

        $("#modalJustEdicaoTitulo").load(url);
        $("#JustEdicaoTitulo").modal();


    }
}
function AlteraTituloEmMassa() {

    var codJustificativa = $('.codJustificativa option:selected').val()
    var descJustificativa = $('.codJustificativa').find("option:selected").text();
    var DescJustOutros = $('#DescJustOutros').val();

    if (codJustificativa == "-1" || codJustificativa == "NAOINFORMADO") {
        msgAviso("Obrigatório justificar o motivo da baixa.");
        return false;
    }

    if ($.trim($("#dtTransferencia").val()) == "") {
        msgAviso("Nova Data de Agendamento não Informada");
        return false;
    }

    var table = $('#GridTitulosVencidos').DataTable();

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
    $("#JustEdicaoTitulo .close").click();

    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        meuData += '&ListaFechamento%5B' + index + '%5D.CODTITULO=' + $(value).find('.CODTITULO').find('input').val();

    });

    var data = "dtTransferencia=" + $("#dtTransferencia").val()
        + "&Justificativa=" + descJustificativa
        + "&JustificativaOutros=" + DescJustOutros
        + meuData;

    var url = montaUrl("/ContasPagar/ReagendarTitulo/AlteraTituloEmMassa");
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


                $("#JustEdicaoTitulo .close").click();
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
