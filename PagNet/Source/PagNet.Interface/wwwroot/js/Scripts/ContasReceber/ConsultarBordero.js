

function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    ConsultarBordero();
}

$(window).load(carregaPagina());


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});

$(".codStatus").change(function () {
    $("#codStatus").val($('.codStatus option:selected').val());
});


function ConsultarBordero() {

    $('#modal').empty();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());


    var url = montaUrl("/ContasReceber/ConsultarBordero/ConsultaBordero/?codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codStatus=" + $("#codStatus").val()
        + "&codBordero=" + $("#codBordero").val())

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaBordero").show();

};

//------------------------------IListaConsultaBorderoBol-------------------

function VisualizarBoletos(CODBORDERO) {

    var url = montaUrl("/ContasReceber/ConsultarBordero/ConsultaBoletosBordero/");

    url += "&CODBORDERO=" + CODBORDERO


    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}

function CancelaBordero(CODBORDERO) {

    var url = montaUrl("/ContasReceber/ConsultarBordero/CancelaBordero/");

    var data = "codBordero=" + CODBORDERO

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                var table = $('#gridBordero').DataTable();

                var trs = $(table.cells().nodes()).parent();
                $(trs).each(function (i, e) {

                    if ($(e).find('.CODBORDERO').find('input').val() == CODBORDERO) {

                        var linha = $(e).closest('tr');
                        linha.hide();

                    }
                });
                table.rows('.selected').remove().draw(false);

                msgSucesso(data.responseText);

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
