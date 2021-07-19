
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

}
$(window).load(carregaPagina);


$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});

$(".codStatus").change(function () {
    $("#codStatus").val($('.codStatus option:selected').val());
});
$(".CodApenasBoletosEntreges").change(function () {
    $("#CodApenasBoletosEntreges").val($('.CodApenasBoletosEntreges option:selected').val());
});

$('#filtro').focusout(function () {
    var filtro = $('#filtro').val();
    if (filtro != "") {

        $.ajax({
            type: 'get',
            url: montaUrl('/ContasReceber/EmitirBoletosGerados/BuscaCliente/?filtro=' + filtro),
            dataType: 'json',
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
})

function PreencheCliente(data) {
    var arr = data.split("/");

    $("#filtro").val(arr[0]);
    $("#nmCliente").val(arr[1]);
    $("#codCli").val(arr[2])
};
function ConsultarBoletos() {


    $('#modal').empty();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#CodApenasBoletosEntreges").val($('.CodApenasBoletosEntreges option:selected').val());


    var url = montaUrl("/ContasReceber/EmitirBoletosGerados/CarregaListaBoletos/?codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codStatus=" + $("#codStatus").val()
        + "&dtInicio=" + $("#dtInicio").val()
        + "&CodApenasBoletosEntreges=" + $("#CodApenasBoletosEntreges").val()
        + "&dtFim=" + $("#dtFim").val())

    

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaBordero").show();

};

//--------------------------------------CarregaListaBoletos----------------------

$("#select_all").click(function () {
    $('input', table.cells().nodes()).prop('checked', this.checked);
});

function EnviarEmailEmMassa() {
    var table = $('#gridBoletosGerados').DataTable();

    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    var meuData = '';
    var trs = $(table.cells().nodes()).parent();
    trs.each(function (index, value) {
        meuData += '&ListaBoleto%5B' + index + '%5D.codEmissaoBoleto=' + $(value).find('.codEmissaoBoleto').find('input').val();
    });

    var data = 'codigoEmpresa=' + $("#codigoEmpresa").val()
    data += meuData;

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
    var _url = montaUrl("/ContasReceber/EmitirBoletosGerados/EnviarBoletoEmailEmMassa")

    ////Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();
            msgSucesso("Processo de envio finalizado!");
            ConsultarBoletos();
        },
        error: function (data) {
            $.unblockUI();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });
    //var url = montaUrl("/ContasReceber/EmitirBoletosGerados/EnvioEmailEmMassa/");

    //$("#modalLogProcessoEnvioEmail").load(url);
    //$("#LogProcessoEnvioEmail").modal();
}


function EnviarEmail(codEmissaoBoleto) {
    $.confirm({
        title: "Confirmar",
        icon: "glyphicon glyphicon-alert",
        content: "Confirma o envio do boleto para o email cadastrado na conta do cliente?",
        type: 'green',
        typeAnimated: true,
        buttons: {
            formSubmit: {
                text: 'Sim',
                btnClass: 'btn-blue',
                action: function () {
                    var url = montaUrl("/ContasReceber/EmitirBoletosGerados/EnviarBoletoEmail/");

                    url += "&codEmissaoBoleto=" + codEmissaoBoleto

                    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

                    $.ajax({
                        type: "Post",
                        url: url,
                        success: function (data) {
                            $.unblockUI();

                            if (data.success) {

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
                btnClass: 'btn-red',
                action: function () { }
            },
            tryAgain: {
                text: 'Enviar para outro email',
                btnClass: 'btn-default',
                action: function () {
                    $.confirm({
                        title: 'Email Para Envio!',
                        content: '' +
                            '<form action="" class="formName">' +
                            '<div class="form-group">' +
                            '<label>Informe o email que irá receber o boleto</label>' +
                            '<input type="text" placeholder="email@provedor.com.br" class="email form-control" required />' +
                            '</div>' +
                            '</form>',
                        buttons: {
                            formSubmit: {
                                text: 'Enviar',
                                btnClass: 'btn-blue',
                                action: function () {
                                    var email = this.$content.find('.email').val();
                                    if (!email) {
                                        $.alert('Email não informado!');
                                        return false;
                                    }

                                    $.blockUI({ message: '<div class="ModalCarregando"></div>' });

                                    var _url = montaUrl("/ContasReceber/EmitirBoletosGerados/EnviarBoletoOutroEmail/");

                                    _url += "&codEmissaoBoleto=" + codEmissaoBoleto
                                        + "&email=" + email


                                    $.ajax({
                                        type: "Post",
                                        url: _url,
                                        success: function (data) {
                                            $.unblockUI();

                                            if (data.success) {

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
                },

            }
        }
    });
}

//-------------------------------EnvioEmailEmMassa---------------------

function EnviarEmailSelecionados() {
    var table = $('#gridBoletosGerados').DataTable();

    var meuData = '';
    var trs = $(table.cells().nodes()).parent();
    trs.each(function (index, value) {
        meuData += '&codEmissaoBoleto%5B' + index + '%5D=' + $(value).find('.codEmissaoBoleto').find('input').val();

    });

    var _url = montaUrl("/ContasReceber/EmitirBoletosGerados/EnviarBoletoEmail")

    ////Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: meuData,
        success: function (data) {
            $.unblockUI();
            msgSucesso("Processo de envio finalizado!");
            ConsultarBoletos();
        },
        error: function (data) {
            $.unblockUI();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });
};
