

function carregaPagina() {

    $("#FiltroNmBanco").prop("disabled", "disabled")

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

}

$(window).load(carregaPagina());

$(".codContaCorrente").change(function () {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
});


$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
});

$(".codStatus").change(function () {
    $("#codStatus").val($('.codStatus option:selected').val());
});

$('#filtroCodBanco').focusout(function () {
    var filtro = $('#filtroCodBanco').val();
    if (filtro != "") {

        var url = montaUrl('/ContasPagar/ConsultaBordero/BuscaBanco/?filtro=' + filtro);

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

function ConsultarFechCred() {


    $('#modal').empty();
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    $("#codStatus").val($('.codStatus option:selected').val());


    var url = montaUrl("/ContasPagar/ConsultaBordero/ConsultaBordero/?codEmpresa=" + $("#codEmpresa").val()
        + "&codBanco=" + $("#codBanco").val()
        + "&codStatus=" + $("#codStatus").val()
        + "&codBordero=" + $("#codBordero").val()
        + "&codContaCorrente=" + $("#codContaCorrente").val())

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaBordero").show();


};

function PreencheBanco(data) {
    var arr = data.split("/");

    $("#filtroCodBanco").val(arr[0]);
    $("#FiltroNmBanco").val(arr[1]);
    $("#codBanco").val(arr[2])

};

//-----------------------IListConsultaBorderoPag----------------------


function VisualizarFechamentos(CODBORDERO) {

    var url = montaUrl("/ContasPagar/ConsultaBordero/ConsultaFechamentosBordero/");
    url += "&CODBORDERO=" + CODBORDERO


    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}

function CancelaBordero(CODBORDERO) {

    var url = montaUrl("/ContasPagar/ConsultaBordero/CancelaBordero/");

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
