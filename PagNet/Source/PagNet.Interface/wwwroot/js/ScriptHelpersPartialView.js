

function addErros(name, data) {
    var str = "<ul>";
    if (typeof data === 'string') {
        //alert('é string: ' + data);
        data = [{ ErrorMessage: data }];
    }
    for (var i = 0; i < data.length; i++) {
        if (data[i].ErrorMessage != null) {
            str += "<li>" + data[i].ErrorMessage + "</li>";
        } else {
            str += "<li> Erro não identificado </li>";
        }
    }
    str += "</ul>";
    $("span[data-valmsg-for='" + name + "']").html(str);
}


$refresh = [];

new jBox('Confirm', {

    confirmButton: 'Confirmar',
    cancelButton: 'Cancelar'
});

function validarCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');
    if (cpf == '') return false;
    // Elimina CPFs invalidos conhecidos	
    if (cpf.length != 11 ||
        cpf == "00000000000" ||
        cpf == "11111111111" ||
        cpf == "22222222222" ||
        cpf == "33333333333" ||
        cpf == "44444444444" ||
        cpf == "55555555555" ||
        cpf == "66666666666" ||
        cpf == "77777777777" ||
        cpf == "88888888888" ||
        cpf == "99999999999")
        return false;
    // Valida 1o digito	
    add = 0;
    for (i = 0; i < 9; i++)
        add += parseInt(cpf.charAt(i)) * (10 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(9)))
        return false;
    // Valida 2o digito	
    add = 0;
    for (i = 0; i < 10; i++)
        add += parseInt(cpf.charAt(i)) * (11 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(10)))
        return false;
    return true;
}
function FormataCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');
    var novoCPF = '';
    if (cpf != '') {
        novoCPF = cpf_mask(cpf);
    }
    return novoCPF;
}
function cpf_mask(v) {
    v = v.replace(/\D/g, "") //Remove tudo o que não é dígito
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1.$2");
    v = v.replace(/(\d{3})(\d)/, "$1-$2");
    return v
}

$.desabilitaValidacao = function () {
    $("[data-val='true']").attr('data-val', 'false');

    //Reset validation message
    $('.field-validation-error')
        .removeClass('field-validation-error')
        .addClass('field-validation-valid');

    //Reset input, select and textarea style
    $('.input-validation-error')
        .removeClass('input-validation-error')
        .addClass('valid');

    //Reset validation summary
    $(".validation-summary-errors")
        .removeClass("validation-summary-errors")
        .addClass("validation-summary-valid");

    //To reenable lazy validation (no validation until you submit the form)
    $('form').removeData('unobtrusiveValidation');
    $('form').removeData('validator');
    $.validator.unobtrusive.parse($('form'));
}

$(window).load(inicia());