function carregaPagina() {
    $.unblockUI();
    if ($('#acessoAdmin').val() == "False") {
        $('.codigoEmpresa').prop('disabled', true);
    }
}

$(window).load(carregaPagina());

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});
$(".codContaCorrente").change(function () {
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
});

function ProcessarArquivo() {
    
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    var contacorrente = $("#codContaCorrente").val();

    if (contacorrente == '' || contacorrente == '-1' || contacorrente == "null" || contacorrente == null) {
        msgAviso("Obrigatório informar a conta corrente.");
        $("#ValidaContaCorrente").show();
        return false;
    }

    var myfile = document.getElementById("uploadFile");
    var formData = new FormData();
    if (myfile.files.length > 0) {
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }

        var _url = montaUrl("/Tesouraria/ConciliacaoBancaria/UploadHomeReport/");

        $.ajax({
            url: _url,
            type: "POST",
            dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                $.unblockUI();
                if (data.success) {
                    $('#modalListaConciliacao').empty();
                    $('#ResultadoBaixa').hide();

                    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

                    var url = montaUrl("/Tesouraria/ConciliacaoBancaria/ListarTransacoes/")
                    url += "&caminhoArquivo=" + data.responseText;
                    url += "&codContaCorrente=" + contacorrente;
                    url += "&codigoEmpresa=" + $("#codigoEmpresa").val();

                    $("#modalListaConciliacao").load(url);
                    $("#ListaConciliacao").show();
                }
                else {
                    msgErro(data.responseText)
                }
            },
            error: function (data) {
                $.unblockUI();
                msgErro("Falha ao tentar ler o arquivo. Verifique se o mesmo está no formato correto.")

            }
        })
    }
    else {
        msgAviso("Favor selecionar um arquivo antes de continuar.")
    }

};
function IncluirTransacao() {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#codContaCorrente").val($('.codContaCorrente option:selected').val());
    var contacorrente = $("#codContaCorrente").val();

    var table = $('#gridConciliacaoBancaria').DataTable();
    var trs = $(table.cells().nodes()).parent()
    var TipoConciliacao = "";
    var TransacaoEncontrada = "";
    var DataConciliacao = "";
    var DescricaoConciliacao = "";
    var ValorTransacao = "";
    $(trs).each(function (index, value) {
        if ($(value).hasClass("selected")) {
            TipoConciliacao = $(value).find('.TipoConciliacao').find('input').val();
            TransacaoEncontrada = $(value).find('.TransacaoEncontrada').find('input').val();
            DataConciliacao = $(value).find('.DataConciliacao').find('input').val();
            DescricaoConciliacao = $(value).find('.DescricaoConciliacao').find('input').val();
            ValorTransacao = $(value).find('.ValorTransacao').find('input').val();
        }
    });


    var _url = montaUrl("/Tesouraria/ConciliacaoBancaria/IncluirTransacao/");
    var data = "TipoIncTransacao=" + TipoConciliacao 
        + "&dtIncTransacao=" + DataConciliacao 
        + "&DescricaoIncTransacao=" + DescricaoConciliacao 
        + "&ValorIncTransacao=" + ValorTransacao 
        + "&codContaCorrenteIncTransacao=" + contacorrente 
        + "&codigoEmpresaIncTransacao=" + $("#codigoEmpresa").val() 
        + "&ParcelaInicioIncTransacao=1" +
        + "&ParcelaTerminoIncTransacao=1" 
        + "&ValorTotalIncTransacao=" + ValorTransacao

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $.ajax({
        url: _url,
        type: "POST",
        dataType: "json",
        data: data,
        success: function (data) {
            $.unblockUI();
            if (data.success) {

                msgSucesso(data.responseText);

                var table = $('#gridConciliacaoBancaria').DataTable();
                var trs = $(table.cells().nodes()).parent()
                $(trs).each(function (index, value) {
                    if ($(value).hasClass("selected")) {
                        $(value).find('.inpStatusPN')[0].innerHTML = "CONCILIADA";
                        $(value).find('#imgExclamation').hide();
                    }
                });
                table.$('tr.selected').css('background-color', '#ffffff');
                table.$('tr.selected').removeClass('selected');

            }
            else {
                msgErro(data.responseText)
            }
        },
        error: function (data) {
            $.unblockUI();
            msgErro("Falha ao tentar ler o arquivo. Verifique se o mesmo está no formato correto.")

        }
    })


};