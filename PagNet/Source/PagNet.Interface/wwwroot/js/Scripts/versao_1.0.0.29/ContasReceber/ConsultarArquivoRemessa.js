function carregaPagina() {


    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
   
    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var url = montaUrl("/ContasReceber/ConsultarArquivoRemessa/CarregaGridArquivosRemessa/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codEmpresa=" + $("#codEmpresa").val());
       
    $("#modal").load(url);
    $("#ListArqDownload").show();
}

$(window).load(carregaPagina());


$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
});

function ConsultarArquivo() {

    //$.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $('#modal').empty();
    var url = montaUrl("/ContasReceber/ConsultarArquivoRemessa/CarregaGridArquivosRemessa/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codEmpresa=" + $("#codEmpresa").val())
    
    $("#modal").load(url);
    $("#ListArqDownload").show();
};


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
    }
});

//-------------------------------CarregaGridArquivosRemessa-------------------------

function VisualizarBoletos(codArquivo) {

    var url = montaUrl("/ContasReceber/ConsultarArquivoRemessa/VisualizarBoletos/");

    url += "&codArquivo=" + codArquivo


    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}
function confirmaCancelarArquivo(CodArquivo) {
    var form = $("form", "#page-inner");
    var url = montaUrl('/ContasReceber/ConsultarArquivoRemessa/CancelaArquivoRemessa/?id=') + CodArquivo

    form.attr('action', url);
    form.submit();
    msgSucesso("Arquivo cancelado com sucesso!")
}
function CancelaArquivoRemessa(CodArquivo) {

    var url = montaUrl('/ContasReceber/ConsultarArquivoRemessa/CancelaArquivoRemessa')

    var data = "id=" + CodArquivo

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                var table = $('#example').DataTable();

                var trs = $(table.cells().nodes()).parent();
                $(trs).each(function (i, e) {

                    if ($(e).find('.codArquivo').find('input').val() == CodArquivo) {

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



$(".btnDownload").click(function () {
    var data = this.href;

    $.ajax({
        type: 'get',
        url: montaUrl('/ContasReceber/ConsultarArquivoRemessa/UpdateStatusArquivo/?caminhoArquivo=' + data),
        dataType: 'json',
        success: function (data) {
        }
    });


});