function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
    
    $('#nomeCliente').prop('disabled', true);
    $('.codigoFatura').prop('disabled', true);

}
$(window).load(carregaPagina);



$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());

    $("#filtroCliente").val('');
    $("#CodigoCliente").val('');
    $("#nomeCliente").val('');

});
$(".codigoFatura").change(function () {
    $("#codigoFatura").val($('.codigoFatura option:selected').val());    
});

function LocalizaCliente() {

    var filtro = '?codempresa=' + $('.codigoEmpresa option:selected').val()
    var url = montaUrl("/ContasReceber/ArquivoRetorno/ConsultaClientesEmpresa/" + filtro)

    $("#modalCliente").load(url);
    $("#LocalizaCliente").modal();
}

$('#filtroCliente').focusout(function () {

    if ($('#filtroCliente').val() != "") {
        var filtro = '?filtro=' + $('#filtroCliente').val() + '&codempresa=' + $('.codigoEmpresa option:selected').val()

        var url = montaUrl('/ContasReceber/ArquivoRetorno/BuscarCliente/' + filtro);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                if (data.codcliente != null && data.codcliente > 0) {

                    $("#filtroCliente").val(data.codcliente);
                    $("#CodigoCliente").val(data.codcliente);
                    $("#nomeCliente").val(data.nmcliente);

                    $('.codigoFatura').prop('disabled', false);

                } else {
                    $("#nomeCliente").val("Cliente não encontrado");
                    $('.codigoFatura').prop('disabled', true);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("filtroCliente", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtroCliente").val('');
        $("#CodigoCliente").val('');
        $("#nomeCliente").val('');
    }
});
function SelecionaCliente(codCliente, codigoEmpresa) {

    if (codCliente != "") {
        var filtro = '?filtro=' + codCliente + '&codempresa=' + codigoEmpresa

        var url = montaUrl('/ContasReceber/ArquivoRetorno/BuscarCliente/' + filtro);

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

                    $('.codigoFatura').prop('disabled', false);

                } else {
                    addErros("filtroCliente", "Cliente não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("filtroCliente", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtroCliente").val('');
        $("#CodigoCliente").val('');
        $("#nomeCliente").val('');
    }

}


function ProcessarArquivo() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var myfile = document.getElementById("uploadFile");
    var formData = new FormData();

    if (myfile.files.length > 0) {
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }
    }
    var _url = montaUrl("/ContasReceber/ArquivoRetorno/UploadArquivoValidacaoRemessa/");


    $.ajax({
        url: _url,
        type: "POST",
        dataType: "json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data.success) {
                $('#modal').empty();
                $('#ResultadoBaixa').hide();


                var url = montaUrl("/ContasReceber/ArquivoRetorno/ValidaArquivo/?CaminhoArquivo=" + data.responseText)

                $("#modal").load(url);
                $("#ListaPagamentos").show();
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


function ExibeMensagemRetorno(e) {
    alert(e);
    if (e != "") {
        msgErro(e);
    }
}