function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var url = montaUrl("/ContasPagar/ConsultarArquivoRemessa/CarregaGrig/?dtInicio=" + $("#dtInicio").val()
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
    
    $('#modal').empty();
    var url = montaUrl("/ContasPagar/ConsultarArquivoRemessa/CarregaGrig/?dtInicio=" + $("#dtInicio").val()
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
        //addErros("dtFim", "Data Inválida!");
    }
});


function ValidaArquivoRemessa() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var myfile = document.getElementById("uploadFile");
    var formData = new FormData();

    if (myfile.files.length > 0) {
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }
    }
    var _url = montaUrl("/ContasPagar/ConsultarArquivoRemessa/UploadArquivoValidacaoRemessa/");

    $.ajax({
        url: _url,
        type: "POST",
        dataType: "json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success) {
                $('#modalListaArquivos').empty();
                $('#ListaArquivosRemessa').hide();

                var url = montaUrl("/ContasPagar/ConsultarArquivoRemessa/ValidaArquivo/?caminho=" + data.responseText)

                $("#modalListaArquivos").load(url);
                $("#ListaArquivosRemessa").show();
            }
            else {
                $.unblockUI();
                msgErro(data.responseText)
            }
        },
        error: function (data) {
            $.unblockUI();
            msgErro("Falha ao tentar ler o arquivo. Verifique se o mesmo está no formato correto.")
        }
    })

};

//-----------------------------------CarregaGrid--------------------------

function VisualizarPagamentos(codArquivo, dtArquivo) {

    var url = montaUrl("/ContasPagar/ConsultarArquivoRemessa/ConsultaPagamentosArquivo/");

    url += "&codArquivo=" + codArquivo
        + "&dtArquivo=" + dtArquivo


    $("#modalTransacoes").load(url);
    $("#VisualizarTransacoes").modal();
}
function CancelaArquivoRemessa(CodArquivo) {

    $.confirm({
        title: 'CUIDADO!',
        icon: "glyphicon glyphicon-alert",
        content: ' Tenha certeza de não ter enviado este arquivo para o banco, caso contrário poderá ocasionar ' +
                'duplicidade de pagamento.Confirma o cancelamento deste arquivo ? ',
        type: 'blue',
        buttons: {
            confirm: {
                text: 'Sim',
                btnClass: 'btn-blue',
                action: function () {

                    var url = montaUrl('/ContasPagar/ConsultarArquivoRemessa/CancelaArquivoRemessa')
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

                                var table = $('#gridArquivosGerados').DataTable();

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
            },
            cancel: {
                text: 'Não',
                keys: ['enter']
            }
        }
    });

}



$(".btnDownload").click(function () {
    var data = this.href;
    
    $.ajax({
        type: 'get',
        url: montaUrl('/ContasPagar/ConsultarArquivoRemessa/UpdateStatusArquivo/?caminhoArquivo=' + data),
        dataType: 'json',
        success: function (data) {
        }
    });


});