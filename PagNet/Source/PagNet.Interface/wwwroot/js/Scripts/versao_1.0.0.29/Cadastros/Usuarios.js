function carregaPagina() {
    //alert($("#Administrador").val());

    $('#spanLogin').text($('#PerfilOperadora').val());

    if ($("#acessoAdmin").val() == 'True') {
        $("#btnDesativar").show();
        $("#btnNovo").show();
        $('#btnSim').show();
        $("#btnLocalizarUsuario").show();
        $("#FiltroEmpresa").show();
        if ($('#CodUsuario').val() != '' && $('#CodUsuario').val() != '0') {
            $(".CODEMPRESA").prop('disabled', true);
        }
    }
    else {
        $(".CODEMPRESA").prop('disabled', true);
    }

    if ($('#CodUsuario').val() != '' && $('#CodUsuario').val() != '0') {
        $('#Email').prop('disabled', true);
        $('#Cpf').prop('disabled', true);
        $('#Login').prop('disabled', true);  
        if ($("#acessoAdmin").val() == 'True') {
            if ($('#Administrador').val() == "True") {
                $("#btnSim").click();
                $("#liNao").removeClass('active');
                $("#liSim").addClass('active');
            }
            else if ($('#Administrador').val() == "False") {
                $("#btnNao").click();
                $("#liSim").removeClass('active');
                $("#liNao").addClass('active');
            }
        }
    }
    else {
        $("#btnDesativar").attr('disabled', 'disabled');
    }

}

$(window).load(carregaPagina);


$("#btnSim").click(function () {
    $("#Administrador").val('True');
})
$("#btnNao").click(function () {
    $("#Administrador").val('False');
})

$(".CODEMPRESA").change(function () {
    $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());

    $("#nmUsuario").val('');
    $("#Email").val('');
    $("#Cpf").val('');
    $("#Login").val('');
    $("#Password").val('');
    $("#ConfirmPassword").val('');

    $("#btnNao").click();
    $("#liSim").removeClass('active');
    $("#liNao").addClass('active');

    $('#Email').prop('disabled', false);
    $('#Cpf').prop('disabled', false);
    $('#Login').prop('disabled', false);  

    //var url = montaUrl('/ContasPagar/GeraBordero/BuscaBanco/?filtro=' + filtro);
    var url = montaUrl('/Cadastros/Usuarios/BuscaFlagLogin/?codEmpresa=' + $("#CODEMPRESA").val());

    $.ajax({
        type: 'get',
        url: url,
        dataType: 'json',
        success: function (data) {
            if (data.length > 0) {
                $('#spanLogin').text(data);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            addErros("FiltroNmBanco", "Erro interno. Tente novamente mais tarde.");
        }
    });
});

function confirmaSalvar() {
    var validado = true;

    $('#Email').prop('disabled', false);
    $('#Cpf').prop('disabled', false);
    $('#Login').prop('disabled', false);  

    if ($('.CODEMPRESA option:selected').val() > "0") {
        $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());
    }

    if ($("#CODEMPRESA").val() < "1") {
        validado = false;
        msgAviso("Sub Rede não Informada!")
    }

    var senha = $('#Password').val();
    var ConfirmPassword = $('#ConfirmPassword').val();

    if ($("#CodUsuario").val() == "0") {
        if (senha == "") {
            validado = false;
            addErros("Password", "Obrigatório informar a senha!")
        }
        if (ConfirmPassword == "") {
            validado = false;
            addErros("ConfirmPassword", "Obrigatório confirmar a senha!")
        }
    }

    if (senha != ConfirmPassword) {
        validado = false;
        $('#ConfirmPassword').addClass("borderError");
        addErros("ConfirmPassword", "Senhas não conferem!")
        $('#Password').addClass("borderError");
    }

    if (!validarCPF($('#Cpf').val())) {
        validado = false;
        $('#Cpf').addClass("borderError");
        addErros("Cpf", "CPF Inválido!")
    }


    if (validado) {
        $("#Login").prop('disabled', false);
        $(".CODEMPRESA").prop('disabled', false);
        var form = $("form", "#page-inner");

        var url = montaUrl('/Cadastros/Usuarios/Salvar')

        form.attr('action', url);
        form.submit();
    }

}

function confirmaDesativar() {

    var form = $("form", "#page-inner");

    var url = montaUrl('/Cadastros/Usuarios/Desativar')

    form.attr('action', url);
    form.submit();
}

function CarregaGridUsuario() {

    $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());
    var url = montaUrl("/Cadastros/Usuarios/ConsultaUsuarios/");
    url += "&codEmpresa=" + $("#CODEMPRESA").val();

    $("#modalUsuario").load(url);
    $("#LocalizarUsuario").modal();
}

$('#Cpf').focusout(function () {

    var CPF = $('#Cpf').val();

    if (CPF != "") {
        if (!validarCPF(CPF)) {
            $('#Cpf').addClass("borderError");
            addErros("Cpf", "CPF Inválido!")
        }
        else {
            $('#Cpf').val(cpf_mask(CPF))
            $('#Cpf').removeClass("borderError");
            $("span[data-valmsg-for='Cpf']").hide();
        }
    }
})
$('#ConfirmPassword').focusout(function () {

    var senha = $('#Password').val();
    var ConfirmPassword = $('#ConfirmPassword').val();

    if (senha != "****************" && ConfirmPassword != "****************") {
        if (senha != ConfirmPassword) {
            $('#ConfirmPassword').addClass("borderError");
            addErros("ConfirmPassword", "Senhas não conferem!")
            $('#Password').addClass("borderError");
        }
        else {
            $('#ConfirmPassword').removeClass("borderError");
            $('#Password').removeClass("borderError");
            $("span[data-valmsg-for='ConfirmPassword']").hide();
        }
    }
})

$('#Password').focusout(function () {

    var senha = $('#Password').val();
    var quantidade = senha.length;

    if (senha != "****************") {
        if (quantidade > 0 && quantidade < 6) {
            $('#Password').addClass("borderError");
            addErros("Password", "A senha deve conter no mínimo 6 caracteres!")
        }
        else {
            $('#Password').removeClass("borderError");
            $("span[data-valmsg-for='Password']").hide();
        }
    }
})
