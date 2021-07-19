
function IncluirArquivo() {
    var myfile = document.getElementById("uploadFile");
    var formData = new FormData();

    if (myfile.files.length > 0) {
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }

        var _url = montaUrl("/Ajuda/Contato/UploadHomeReport/");


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
                    console.log(data.nomeArquivo)
                    console.log(data.novoNomeArquivo)
                    IncluiArquivoGrid(data.nomeArquivo, data.novoNomeArquivo);
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

}
function ExcluiLinha(CodigoLinha) {
    var table = $('#gridArquivosAnexados').DataTable();

    var trs = $(table.cells().nodes()).parent().parent();
    $(trs).each(function (i, e) {
        console.log($(e));

        if ($(e).find('.inpCodigoLinha').find('input').val() == CodigoLinha) {
            var linha = $(e).closest('tr');
            linha.hide();
        }
    });

}

function EnviarEmail() {


    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var meuData = ''
    //habilitar este processo quando voltar a desenvolver a inclusão de index. O processo não está concluído pois falta validar o botão de 
    //exclusão do anexo e validar se está conseguindo ler o conteudo dentro do grid.
    //var table = $('#gridArquivosAnexados').DataTable();
    //var trs = $(table.cells().nodes()).parent();
    //$(trs).each(function (index, value) {

    //    meuData += '&Anexo%5B' + index + '%5D.nomeArquivo=' + $(value)[index].childNodes[0].innerHTML;
    //    meuData += '&Anexo%5B' + index + '%5D.NovoNomeArquivo=' + $(value)[index].childNodes[2].innerHTML;

    //});

    var data = "Mensagem=" + $("#Mensagem").val()
        + "&Assunto=" + $("#Assunto").val()
        + "&TelefoneSolicitante=" + $("#TelefoneSolicitante").val()
        + "&EmailSolicitente=" + $("#EmailSolicitente").val()
        + "&nmEmpresaSolicitante=" + $("#nmEmpresaSolicitante").val()
        + "&nmSolicitante=" + $("#nmSolicitante").val()
        + meuData;


    var _url = montaUrl("/Ajuda/Contato/EnviarEmail")


    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                $("#JanelaModal .close").click();
                $.confirm({
                    title: "Sucesso",
                    icon: "glyphicon glyphicon-ok",
                    content: data.mensagem,
                    type: 'green',
                    typeAnimated: true,
                    buttons: {
                        tryAgain: {
                            text: 'OK',
                            btnClass: 'btn-green',
                            action: function () {
                                location.reload();
                            }
                        }
                    }
                });


            }
            else {
                msgErro(data.mensagem)
            }
        },
        error: function (data) {
            $.unblockUI();
            msgErro(data.mensagem);
        }
    });

};
