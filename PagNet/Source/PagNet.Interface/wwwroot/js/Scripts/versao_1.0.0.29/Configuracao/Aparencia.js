function carregaPagina() {
    $("#btnCabecaho").style.backgroundColor = $("#Cabecaho").val();
    $("#btnLinhaCabecalho").style.backgroundColor = $("#LinhaCabecalho").val();
    $("#btncorTexto1").style.backgroundColor = $("#corTexto1").val();
    $("#btncorTexto2").style.backgroundColor = $("#corTexto2").val();
    $("#btncorTexto3").style.backgroundColor = $("#corTexto3").val();
    $("#btncorTexto4").style.backgroundColor = $("#corTexto4").val();


    $("#btnPainelSuperior").style.backgroundColor = $("#PainelSuperior").val();
    $("#btnLinhaPainel").style.backgroundColor = $("#LinhaPainel").val();

    $("#botaoSucesso").style.backgroundColor = $("#btnSucesso").val();
    $("#botaoDanger").style.backgroundColor = $("#btnDanger").val();
    $("#botaoDefault").style.backgroundColor = $("#btnDefault").val();

}
$(window).load(carregaPagina);



$("#imgLogo").change(function () {

    var myfile = document.getElementById("imgLogo");
    var formData = new FormData();

    if (myfile.files.length > 0) {
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }
    }
    var _url = montaUrl("/Configuracao/Aparencia/UploadImgLogo/");

    $.ajax({
        url: _url,
        type: "POST",
        dataType: "json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            var imagem = document.getElementById("LogoImage");

            imagem.src = montaUrl("/Configuracao/Aparencia/GetImagemLogo/") + `?timestamp=${new Date().getTime().toString()}`
        },
        error: function (data) {
            msgErro("Ocorreu uma falha inesperada durante o processo de upload. Favor contactar o suporte.")
        }
    })
})
$("#ImgIco").change(function () {

    var myfile = document.getElementById("ImgIco");
    var formData = new FormData();

    if (myfile.files.length > 0) {
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }
    }    
    var _url = montaUrl("/Configuracao/Aparencia/UploadImgIco/");

    $.ajax({
        url: _url,
        type: "POST",
        dataType: "json",
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            var imagem = document.getElementById("icoImage");
            
            imagem.src = montaUrl("/Configuracao/Aparencia/GetImagemIco/") + `?timestamp=${new Date().getTime().toString()}`
        },
        error: function (data) {
            msgErro("Ocorreu uma falha inesperada durante o processo de upload. Favor contactar o suporte.")
        }
    })
})
function SalvaAlteracaoLayout() {

    var form = $('form', '#page-inner');

    var url = montaUrl("/Configuracao/Aparencia/Salvar/");

    form.attr('action', url);
    form.submit();
}

function ResetarAparencia() {

    var form = $('form', '#page-inner');

    var url = montaUrl("/Configuracao/Aparencia/ResetarAparencia/");

    form.attr('action', url);
    form.submit();
}

function setColorCabecalho(picker) {
    for (var i = 0; i < document.getElementsByClassName("Color-Header-menu").length; i++) {
        document.getElementsByClassName("Color-Header-menu")[i].style.backgroundColor = '#' + picker.toString();
    }
}

function setColorLinhaCabecalho(picker) {

    document.getElementsByClassName("page-header")[0].style.borderColor = '#' + picker.toString();
    document.getElementsByClassName("nav-side-menu")[0].style.borderTopColor = '#' + picker.toString();


}

function setColorTexto1(picker) {

    document.getElementsByClassName("text-header")[0].style.color = '#' + picker.toString();
}

function setColorTexto2(picker) {
    document.getElementsByClassName("info-user")[0].style.color = '#' + picker.toString();
}

function setColorTexto3(picker) {

    document.getElementsByClassName("mnuAlteraSenha")[0].style.color = '#' + picker.toString();
}

function setColorTexto4(picker) {
    for (var i = 0; i < document.getElementsByClassName("menu")[0].children.length; i++) {
        document.getElementsByClassName('menu')[0].children[i].children[0].style.color = '#' + picker.toString();
    }
}

function setColorPainelSuperior(picker) {
    for (var i = 0; i < document.getElementsByClassName("panel-heading").length; i++) {
        document.getElementsByClassName("panel-heading")[i].style.backgroundColor = '#' + picker.toString();
    }
}
function setColorLinhaPainel(picker) {
    for (var i = 0; i < document.getElementsByClassName("panel-heading").length; i++) {
        document.getElementsByClassName("panel-heading")[i].style.borderColor = '#' + picker.toString();
    }
}


function setColorbtnSucesso(picker) {
    for (var i = 0; i < document.getElementsByClassName("btn-success").length; i++) {
        document.getElementsByClassName("btn-success")[i].style.backgroundColor = '#' + picker.toString();
    }
}
function setColorbtnDanger(picker) {
    for (var i = 0; i < document.getElementsByClassName("btn-danger").length; i++) {
        document.getElementsByClassName("btn-danger")[i].style.backgroundColor = '#' + picker.toString();
    }
}

function setColorbtnDefault(picker) {
    for (var i = 0; i < document.getElementsByClassName("btn-default").length; i++) {
        document.getElementsByClassName("btn-default")[i].style.backgroundColor = '#' + picker.toString();
    }
}
