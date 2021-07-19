
function readCookie(name) {
    var nameEQ = name + "=";
    var vcookie = document.cookie;

    if (vcookie.indexOf(";") >= 0) {
        var vcookie2 = vcookie.split(';');
        vcookie = vcookie2[1];
    }
    if (vcookie.indexOf("DadosLogin=") >= 0) {
        vcookie = vcookie.replace("DadosLogin=", "");
    }
    var ca = vcookie.split('&');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function ConvertDecimal(str) {
    if (str != "undefined")
        return parseInt(str.replace(/[\D]+/g, ''));
    else
        return 0
}
function FormataMoedaReal(int) {
    var tmp = int + '';
    tmp = tmp.replace(/([0-9]{2})$/g, ",$1");
    if (tmp.length > 6)
        tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

    return tmp;
}

function addErros(name, data) {
    var str = "<ul style='width:98%; margin-top:27px;'>";
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
    $("span[data-valmsg-for='" + name + "']").show();
}

function msgSucesso(mensagem) {

    return new $.confirm({
        title: "Sucesso",
        icon: "glyphicon glyphicon-ok",
        content: mensagem,
        type: 'green',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'OK',
                btnClass: 'btn-green',
                action: function () {
                }
            }
        }
    });
}
function msgSucessoComReload(mensagem) {

    return new $.confirm({
        title: "Sucesso",
        icon: "glyphicon glyphicon-ok",
        content: mensagem,
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
function msgSucessoWithLoad(mensagem) {

    return new $.confirm({
        title: "Sucesso",
        icon: "glyphicon glyphicon-ok",
        content: mensagem,
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


function msgErro(mensagem) {

    return new $.confirm({
        title: "Erro",
        icon: "glyphicon glyphicon-remove",
        content: mensagem,
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'OK',
                btnClass: 'btn-red',
                action: function () {
                }
            }
        }
    });
}

function msgAviso(mensagem) {

    return new $.confirm({
        title: "Aviso",
        icon: "glyphicon glyphicon-alert",
        content: mensagem,
        type: 'blue',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'OK',
                btnClass: 'btn-blue',
                action: function () {
                }
            }
        }
    });
}

$refresh = [];
var ddlPaiAlterado = 0;

function limpaDDL(element, urlFunc, pai) {
    $refresh[urlFunc] = 1;
}
function refreshDDLInView(element, urlFunc) {

    var urlFunc = montaUrl(urlFunc);

    var Inner = element.innerHTML;
    var sPlitInner = Inner.split('"');

    var valorAtual = sPlitInner[1];

    var length = $(element)[0].childNodes[1];


    if (valorAtual == -10) {
        $refresh[urlFunc] = 4;
    }
    if (valorAtual == -1) {
        $refresh[urlFunc] = 9;
    }
    if (typeof ($refresh[urlFunc]) === "undefined") {
        $refresh[urlFunc] = 1;
    }
    if (length =! "undefined") {
        $refresh[urlFunc] = 1;
    }

    if ($refresh[urlFunc] != 7) {
        $refresh[urlFunc] = 2;
        $.ajax({
            type: 'GET',
            url: urlFunc,
            dataType: 'json',
            cache: false,
            async: false,
            success: function (data) {
                $refresh[urlFunc] = 7;
                var items = element.innerHTML
                items += '<div class="dropdownInView-content">';
                $.each(data, function (i, retorno) {
                    items += " <a style='cursor:Pointer' onclick=" + retorno[0] + "('" + retorno[1]+ "')>" + retorno[2] + "</a>"                                      
                });

                element.innerHTML = items;
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $refresh[urlFunc] = 3;
                alert("Erro detectado: " + thrownError);
            }
        });
    }
    return;
}
function changeDDLInView(element) {

    var valorAtual = $(element).val();
    var descracaoAtual = $(element).find(":selected").text();

    $(element).next().next().val(descracaoAtual);
    if ($(element).next().val() == valorAtual) {
        return;
    }
    $(element).next().val(valorAtual);

    return;
}
function refreshDDL(element, urlFunc, pai) {

    var urlFunc = montaUrl(urlFunc);

    var hiddenFor = $(element).next();
    var Inner = element.innerHTML;
    var sPlitInner = Inner.split('"');

    var valorAtual = sPlitInner[1];    
    var length = $(element).children('option').length;

    if (pai != "nulo") {
        var id = $("select." + pai).next().val();
        if (typeof (id) === "undefined") {
            id = $("#" + pai).val();
        }
        urlFunc = urlFunc + '&id=' + id;
    }


    if (valorAtual == -10) {
        $refresh[urlFunc] = 4;
    }
    if (valorAtual == -1) {
        $refresh[urlFunc] = 9;
    }
    if (typeof ($refresh[urlFunc]) === "undefined") {
        $refresh[urlFunc] = 1;
    }
    if (ddlPaiAlterado == 1) {
        ddlPaiAlterado = 0;
        $refresh[urlFunc] = 1;
    }

    if (length <= 1 && $refresh[urlFunc] == 7) {
        $refresh[urlFunc] = 1;
    }

    if ($refresh[urlFunc] != 7) {
        $refresh[urlFunc] = 2;
        $.ajax({
            type: 'GET',
            url: urlFunc,
            dataType: 'json',
            cache: false,
            async: false,
            success: function (data) {
                $refresh[urlFunc] = 7;
                var items = ' ';
                $.each(data, function (i, retorno) {
                    if (retorno[2] == "undefined") retorno[2] = "";

                    if (valorAtual == retorno[0]) {
                        items += "<option value='" + retorno[0] + "' title='" + retorno[1] + "' selected>" + retorno[1] + "</option>";
                    } else {
                        items += "<option value='" + retorno[0] + "' title='" + retorno[1] + "' >" + retorno[1] + "</option>";
                    }
                });

                element.innerHTML = items;
                changeDDL(element, $(element).attr("PagNet-ddl-filho"))
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $refresh[urlFunc] = 3;
                alert("Erro detectado: " + thrownError);
            }
        });
    }
    return;
}

function changeDDL(element, dependentes) {

    var valorAtual = $(element).val();
    var descracaoAtual = $(element).find(":selected").text();

    $(element).next().next().val(descracaoAtual);
    if ($(element).next().val() == valorAtual) {
        return;
    }
    $(element).next().val(valorAtual);


    if (dependentes == "nulo") {
        return;
    }

    ddlPaiAlterado = 0;

    var selPedente = $("select." + dependentes)
    $(selPedente).html("<option value='-10'>  </option>");
    $(selPedente).prop("disabled", false);

    changeDDL(selPedente, $(selPedente).attr("PagNet-ddl-filho"));
    return;
}

function inicia() {
    var ddls = $("select.PagNet-ddl");

    var ddlInView = $("div.dropdownInView");

    $.each(ddls, function (key, value) {
        //var NewUrl = montaUrl();

        $(value).hover(function () { refreshDDL(value, $(value).attr("PagNet-ddl-url"), $(value).attr("PagNet-ddl-pai")) });
        $(value).click(function () { refreshDDL(value, $(value).attr("PagNet-ddl-url"), $(value).attr("PagNet-ddl-pai")) });
        $(value).change(function () { changeDDL(value, $(value).attr("PagNet-ddl-filho")) });
    });

    $.each(ddlInView, function (key, value) {
        $(value).hover(function () { refreshDDLInView(value, $(value).attr("PagNet-ddl-url")) });

    });

    //$('#main-menu').metisMenu();
}


$('.tooltip2').jBox('Tooltip', { pointer: 'center:-100' });

$('.umadata input').datepicker({
    format: "dd/mm/yyyy",
    language: "pt-BR",
    orientation: "auto",
    autoclose: true
});

$('.ummes input').datepicker({
    format: "mm/yyyy",
    minViewMode: 1,
    language: "pt-BR",
    orientation: "top auto",
    autoclose: true
});

new jBox('Confirm', {

    confirmButton: 'Confirmar',
    cancelButton: 'Cancelar'
});
function MascaraTelefone(o, f) {
    v_obj = o
    v_fun = f
    setTimeout("execmascaraTel()", 1)
}
function execmascaraTel() {
    v_obj.value = v_fun(v_obj.value)
}
function mtel(v) {
    v = v.replace(/\D/g, "");             //Remove tudo o que não é dígito
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2"); //Coloca parênteses em volta dos dois primeiros dígitos
    v = v.replace(/(\d)(\d{4})$/, "$1-$2");    //Coloca hífen entre o quarto e o quinto dígitos
    return v;
}
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

function validarCNPJ(cnpj) {

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;

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

//------------------------------menu principal-------------------------


$("#close-sidebar").click(function () {
    $(".page-wrapper").removeClass("toggled");
});
$("#show-sidebar").click(function () {
    $(".page-wrapper").addClass("toggled");
});


$('[data-toggle=popover]').on('dblclick', function (e) {
    $('[data-toggle=popover]').popover('hide');
});

/*---------------------------------------evento ao digitar valores montários--------------------------------*/
function FormataMoeda(a, e, r, t) {
    let n = ""
        , h = j = 0
        , u = tamanho2 = 0
        , l = ajd2 = ""
        , o = window.Event ? t.which : t.keyCode;
    if (13 == o || 8 == o)
        return !0;
    if (n = String.fromCharCode(o),
        -1 == "0123456789".indexOf(n))
        return !1;
    for (u = a.value.length,
        h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
        ;
    for (l = ""; h < u; h++)
        -1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
    if (l += n,
        0 == (u = l.length) && (a.value = ""),
        1 == u && (a.value = "0" + r + "0" + l),
        2 == u && (a.value = "0" + r + l),
        u > 2) {
        for (ajd2 = "",
            j = 0,
            h = u - 3; h >= 0; h--)
            3 == j && (ajd2 += e,
                j = 0),
                ajd2 += l.charAt(h),
                j++;
        for (a.value = "",
            tamanho2 = ajd2.length,
            h = tamanho2 - 1; h >= 0; h--)
            a.value += ajd2.charAt(h);
        a.value += r + l.substr(u - 2, u)
    }
    return !1
}
//$(".umValorMonetario").on('click', function () {
//    if ($(this).val() == "0,00") {
//        $(this).val("")
//    }
//});

//$('.umValorMonetario').focusout(function () {
//    if ($(this).val() == "") {
//        $(this).val("0,00")
//    }
//});
