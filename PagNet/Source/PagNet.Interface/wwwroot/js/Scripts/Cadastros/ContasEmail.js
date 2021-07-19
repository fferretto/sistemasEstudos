
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    if ($("#CODCONTAEMAIL").val() == "" || $("#CODCONTAEMAIL").val() == "0") {
        $("#EMAILPRINCIPAL").val('True');
        $("#btnEmailPrincipalSim").click();
        $("#liEmailPrincipalNao").removeClass('active');
        $("#liEmailPrincipalSim").addClass('active');
    }
    else {
        if ($('#EMAILPRINCIPAL').val() == "False") {
            $("#btnEmailPrincipalNao").click();
            $("#liEmailPrincipalNao").addClass('active');
            $("#liEmailPrincipalSim").removeClass('active');
        }
    }
}
$(window).load(carregaPagina);

$(".CRIPTOGRAFIA").change(function () {
    $("#CRIPTOGRAFIA").val($('.CRIPTOGRAFIA option:selected').val());
});

$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
});

function LocalizarContas() {

    var url = montaUrl("/Cadastros/ContasEmail/ConsultaEmailsCadastrados/");
    url += "&codEmpresa=" + $("#codEmpresa").val();

    $("#modal").load(url);
    $("#Localizar").modal();
}

$("#btnEmailPrincipalSim").click(function () {
    $("#EMAILPRINCIPAL").val('True');
})
$("#btnEmailPrincipalNao").click(function () {
    $("#EMAILPRINCIPAL").val('False');
})

function TestarConfiguracoes() {

    $.confirm({
        title: 'Email Para Envio!',
        content: '' +
            '<form action="" class="formName">' +
            '<div class="form-group">' +
            '<label>Informe um email de destino para realizar o teste</label>' +
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
                    $("#EMAILPARA").val(email);
                    // Testando o email

                    var _url = montaUrl("/Cadastros/ContasEmail/TestarConfiguracoes/");
                    var form = $('form', '#page-inner');
                    var data = form.serialize();


                    $.ajax({
                        type: "Post",
                        url: _url,
                        data: data,
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
}

function ConfirmaSalvar() {
    var validado = true;

    var form = $('form', '#page-inner');

    var url = montaUrl("/Cadastros/ContasEmail/Salvar/");

    if (validado) {
        form.attr('action', url);
        form.submit();
    }
}

function ConfirmaDesativar() {

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    var form = $('form', '#page-inner');

    var url = montaUrl("/Cadastros/ContasEmail/Desativar/");

    form.attr('action', url);
    form.submit();
}
