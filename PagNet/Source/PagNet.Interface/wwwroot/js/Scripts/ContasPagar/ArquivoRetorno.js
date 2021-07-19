function carregaPagina() {
}

$(window).load(carregaPagina());

$('#txtUploadFile').on('change', function (e) {
    var files = e.target.files;
   
    //var myID = 3; //uncomment this to make sure the ajax URL works
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
            }
            var _url = montaUrl("/ContasPagar/ArquivoRetorno/UploadHomeReport/");

            $.ajax({
                type: "POST",
                url: _url,
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                }
            });
        } else {
            msgAviso("Navegador não homologado pelo sistema!");
        }
    }
});

function ProcessarArquivo() {

    var myfile = document.getElementById("uploadFile");
    var formData = new FormData();

    if (myfile.files.length > 0) {
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }

        var _url = montaUrl("/ContasPagar/ArquivoRetorno/UploadHomeReport/");

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

                    var url = montaUrl("/ContasPagar/ArquivoRetorno/ValidaArquivo/")
                    url += "&caminho=" + data.responseText;

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
    }
    else {
        msgAviso("Favor selecionar um arquivo antes de continuar.")
    }

};


//-------------------------Valida Arquivo-----------------------
function ExibeMensagemRetorno(e) {
    alert(e);
    if (e != "") {
        msgErro(e);
    }
}


function confirmaBaixa() {

    var table = $('#example').dataTable();

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var data = table.$("input, select").serialize();

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: montaUrl("ContasPagar/ArquivoRetorno/ProcessaBaixaPagamento"),
        data: data,
        success: function (data) {
            ResultadoBaixa(data);
            $.unblockUI();
        }
    });
}

function ResultadoBaixa(e) {

    var resultado = e.split("/");

    $('#ResultadoBaixa').show();

    if (resultado[0] == "sucesso") {
        msgSucesso(resultado[1])
    }
    else {
        msgErro(resultado[1]);
    }
};