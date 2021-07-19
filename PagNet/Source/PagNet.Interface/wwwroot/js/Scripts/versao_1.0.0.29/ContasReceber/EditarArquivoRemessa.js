function carregaPagina() {

    $("#nmCliente").prop("disabled", "disabled")

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
}

$(window).load(carregaPagina());

$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
});


$('#filtro').focusout(function () {
    var filtro = $('#filtro').val();
    if (filtro != "") {

        $.ajax({
            type: 'get',
            url: montaUrl('/ContasReceber/EditarArquivoRemessa/BuscaCliente/?filtro=' + filtro),
            dataType: 'json',
            async: true,
            success: function (data) {
                if (data.length > 0) {
                    PreencheCliente(data);
                } else {
                    addErros("nmCliente", "Cliente não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                msgErro("Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#filtro").val('');
        $("#nmCliente").val('');
        $("#codCli").val(0)
    }
});

function PreencheCliente(data) {
    var arr = data.split("/");

    $("#filtro").val(arr[0]);
    $("#nmCliente").val(arr[1]);
    $("#codCli").val(arr[2])
};

function ConsultarBoletos() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#codEmpresa").val($('.codEmpresa option:selected').val());

    var codContaCorrente = $('.CodContaCorrente option:selected').val()

    if (codContaCorrente <= 0 || codContaCorrente == "null") {
        msgAviso("Obrigatório informar uma conta corrente.")
        $("#ValidaContaCorrente").show();
        $.unblockUI();
        return false;
    }


    $('#modal').empty();
    var url = montaUrl("/ContasReceber/EditarArquivoRemessa/BuscaBoletosGerados/?dtInicio=" + $("#dtInicio").val()
        + "&dtFim=" + $("#dtFim").val()
        + "&codEmpresa=" + $("#codEmpresa").val()
        + "&CodContaCorrente=" + $("#CodContaCorrente").val()
        + "&codCli=" + $("#codCli").val())

    $("#modalListBoletos").load(url);
    $("#ListBoletos").show();
};

function AlterarVencimento() {
    $.confirm({
        title: 'Nova data de vencimento!',
        content: '' +
            '<form action="" class="formName">' +
            '<div class="form-group">' +
            '<label>Informe a nova data de vencimento do boleto.</label>' +
            '<input type="text" placeholder="04/03/2019" class="email form-control" required />' +
            '</div>' +
            '</form>',
        buttons: {
            formSubmit: {
                text: 'Enviar',
                btnClass: 'btn-blue',
                action: function () {
                    var novaData = this.$content.find('.email').val();
                    if (!novaData) {
                        $.alert('Nova data de vencimento não informada!');
                        return false;
                    }

                    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

                    var table = $('#gridBoletosEmitidos').DataTable();

                    var meuData = '';
                    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
                    $(trs).each(function (index, value) {                        
                        meuData += '&ListaBoletos%5B' + index + '%5D.codBordero=' + $(value).find('.codBordero').find('input').val();
                        meuData += '&ListaBoletos%5B' + index + '%5D.codEmissaoBoleto=' + $(value).find('.codEmissaoBoleto').find('input').val();
                    });

                    var data = "NovaDataVencimento=" + novaData  
                        + "&CodContaCorrente=" + $("#CodContaCorrente").val()
                        + meuData;

                    var _url = montaUrl("/ContasReceber/EditarArquivoRemessa/AlterarVencimento/");
                
                    // Submit form data via ajax
                    $.ajax({
                        type: "Post",
                        url: _url,
                        data: data,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

                                $('#btnDownload').attr({
                                    href: montaUrl("/ContasReceber/GerarArquivoRemessa/DownloadArquivoRemessa/" + data.responseText)
                                });

                                $('#btnDownload').show();
                                $('#btnDownload').removeAttr('disabled');
                                $("#btnDownload").removeClass('btn-default');
                                $("#btnDownload").addClass('btn-success');

                                $.confirm({
                                    title: 'Sucesso!',
                                    icon: "glyphicon glyphicon-ok",
                                    content: 'Arquivo gerado com sucesso. Deseja realizar o download?',
                                    type: 'green',
                                    buttons: {
                                        confirm: {
                                            text: 'Sim',
                                            btnClass: 'btn-green',
                                            keys: ['enter'],
                                            action: function () {
                                                window.location.href = (montaUrl("/ContasReceber/EditarArquivoRemessa/DownloadArquivoRemessa/" + data.responseText));
                                            }
                                        },
                                        cancel: {
                                            text: 'Não'
                                        }
                                    }
                                });

                            }
                            else {
                                msgErro(data.responseText)
                            }
                        },
                        error: function () {
                            msgErro("Ocorreu uma falha inesperada. Favor contactar o suporte.")
                        }
                    });

                    // Prevent form submission
                    return false;
                }
            },
            cancel: function () {
                //close
            },
        },
        onContentReady: function () {
            // bind to events
            var jc = this;
            this.$content.find('form').on('submit', function (e) {
                // if the user submits the form by pressing enter in the field.
                e.preventDefault();
                jc.$$formSubmit.trigger('click'); // reference the button and click it
            });
        }
    });
};

function BaixarBoletos() {
    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

    var table = $('#gridBoletosEmitidos').DataTable();

    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        meuData += '&ListaBoletos%5B' + index + '%5D.codBordero=' + $(value).find('.codBordero').find('input').val();
        meuData += '&ListaBoletos%5B' + index + '%5D.codEmissaoBoleto=' + $(value).find('.codEmissaoBoleto').find('input').val();
    });

    var data = "NovaDataVencimento=" + ""
        + "&CodContaCorrente=" + $("#CodContaCorrente").val()
        + meuData;

    var url = montaUrl("/ContasReceber/EditarArquivoRemessa/BaixarBoletos/");

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                $('#btnDownload').attr({
                    href: montaUrl("/ContasReceber/GerarArquivoRemessa/DownloadArquivoRemessa/" + data.responseText)
                });

                $('#btnDownload').show();
                $('#btnDownload').removeAttr('disabled');
                $("#btnDownload").removeClass('btn-default');
                $("#btnDownload").addClass('btn-success');

                $.confirm({
                    title: 'Sucesso!',
                    icon: "glyphicon glyphicon-ok",
                    content: 'Arquivo gerado com sucesso. Deseja realizar o download?',
                    type: 'green',
                    buttons: {
                        confirm: {
                            text: 'Sim',
                            btnClass: 'btn-green',
                            keys: ['enter'],
                            action: function () {
                                window.location.href = (montaUrl("/ContasReceber/EditarArquivoRemessa/DownloadArquivoRemessa/" + data.responseText));
                            }
                        },
                        cancel: {
                            text: 'Não'
                        }
                    }
                });

            }
            else {
                msgErro(data.responseText)
            }
        }
    });
};