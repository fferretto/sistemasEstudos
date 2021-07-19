function carregaPagina() {
    
    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }

    $('#NMFORNECEDOR').prop('disabled', true);


}
$(window).load(carregaPagina);

$("#btnTituloMassa").click(function () {

    $('#modalMostraTituloMassa').empty();
    var url = montaUrl("/ContasPagar/EmitirTitulo/VisualizaTituloMassa/");

    $("#modalMostraTituloMassa").load(url);
    $("#MostraTituloMassa").modal();
});

$(".TIPOTITULO").change(function () {
    $("#TIPOTITULO").val($('.TIPOTITULO option:selected').val());
    if ($("#TIPOTITULO").val() == 'TEDDOC') {
        $("#BOLETO").hide();
        $("#TEDDOC").show();
    }
    else if ($("#TIPOTITULO").val() == 'BOLETO') {
        $("#BOLETO").show();
        $("#TEDDOC").hide();
    }
    else {
        $("#BOLETO").hide();
        $("#TEDDOC").hide();
    }
});

$(".CODEMPRESA").change(function () {
    $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());
});
$(".CodigoPlanoContas").change(function () {
    $("#CodigoPlanoContas").val($('.CodigoPlanoContas option:selected').val());
});



$('#CODFORNECEDOR').focusout(function () {
    var filtro = $('#CODFORNECEDOR').val();
    if (filtro != "") {
        $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());
        var url = montaUrl('/ContasPagar/EmitirTitulo/BuscaFavorecido/?filtro=' + filtro);
        url += '&codEmpresa=' + $("#CODEMPRESA").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                if (data.length > 0) {

                    var arr = data.split("/");
                    $("#CODFORNECEDOR").val(arr[0]);
                    $("#NMFORNECEDOR").val(arr[1]);

                } else {
                    addErros("CODFORNECEDOR", "Favorecido não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("CODFORNECEDOR", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#CODFORNECEDOR").val('');
        $("#NMFORNECEDOR").val('');
    }
});

function SalvarTitulo() {
    var Valido = true;

    if ($("#TIPOTITULO").val() == 'TEDDOC') {

        var data = new Date();
        var dtInicio = $("#DATREALPGTO").val().split("/");
        var dataIni = new Date(`${dtInicio[2]},${dtInicio[1]},${dtInicio[0]}`);
        var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)

        if ($("#CODFORNECEDOR").val() == "") {
            msgAviso("Obrigatório informar um favorecido.")
            Valido = false;
        }
        else if ($("#DATREALPGTO").val() == "") {
            msgAviso("Obrigatório informar a data de pagamento.")
            Valido = false;
        }
        else if ($("#VALOR").val() == "") {
            msgAviso("Obrigatório inforamr o valor para pagamento.")
            Valido = false;
        }
        else if (dataIni < dataAtual) {
            msgAviso("A data de pagameto não pode ser inferior a data atual.");
            valido = false;
        }
    }
    else if ($("#TIPOTITULO").val() == 'BOLETO') {

        var LinhaDigitavel = $("#LINHADIGITAVELCOBRANCA1").val() +
            $("#LINHADIGITAVELCOBRANCA2").val() +
            $("#LINHADIGITAVELCOBRANCA3").val() +
            $("#LINHADIGITAVELCOBRANCA4").val() +
            $("#LINHADIGITAVELCOBRANCA5").val() +
            $("#LINHADIGITAVELCOBRANCA6").val() +
            $("#LINHADIGITAVELCOBRANCA7").val() +
            $("#LINHADIGITAVELCOBRANCA8").val();

        $("#LINHADIGITAVEL").val(LinhaDigitavel)

        if (LinhaDigitavel.length < 47 && LinhaDigitavel.length > 48) {
            msgAviso("Linha digitável inválida.")
            Valido = false;
        }
        else if ($("#DATREALPGTOBOLETO").val() == "" && $("#DATVENCIMENTOBOLETO").val() == "") {
            msgAviso("Obrigatório informar a data de pagamento e a de vencimento.")
            Valido = false;
        }

        else if ($("#DATREALPGTOBOLETO").val() == "") {
            msgAviso("Obrigatório informar a data de pagamento.")
            Valido = false;
        }
        else if ($("#DATVENCIMENTOBOLETO").val() == "") {
            msgAviso("Obrigatório informar a data de vencimento.")
            Valido = false;
        }
        else {
            var data = new Date();
            var dataAtual = new Date(`${data.getFullYear()},${data.getMonth() + 1},${data.getDate()}`)
            var dtPGTOAux = $("#DATREALPGTOBOLETO").val().split("/");
            var datPGTO = new Date(`${dtPGTOAux[2]},${dtPGTOAux[1]},${dtPGTOAux[0]}`);
            var dtVencimentoAux = $("#DATVENCIMENTOBOLETO").val().split("/");
            var datVencimento = new Date(`${dtVencimentoAux[2]},${dtVencimentoAux[1]},${dtVencimentoAux[0]}`);
            
            if (datVencimento < datPGTO) {
                msgAviso("A data de vencimento não pode ser inferior a data de pagamento.");
                valido = false;
            }
            else if (datPGTO < dataAtual) {
                msgAviso("A data de pagamento não pode ser inferior a data de atual.");
                valido = false;
            }
        }

    }
    else {
        msgAviso("Obrigatório informar ó tipo de título que será gerado.")
        Valido = false;
    }

    if (Valido) {
        var form = $('form', '#page-inner');

        var url = montaUrl("/ContasPagar/EmitirTitulo/Salvar/");

        form.attr('action', url);
        form.submit();
    }

}

function LocalizaFavorecido() {
    $("#CODEMPRESA").val($('.CODEMPRESA option:selected').val());
    var url = montaUrl("/ContasPagar/EmitirTitulo/ConsultaFavorecido")
    url += "&codEmpresa=" + $("#CODEMPRESA").val();

    $("#modalFavorecido").load(url);
    $("#LocalizarFavorecido").modal();
}
function SelecionaFavorecido(CODFAVORECIDO) {

    if (CODFAVORECIDO != "") {

        var url = montaUrl('/ContasPagar/EmitirTitulo/BuscaFavorecido/?filtro=' + CODFAVORECIDO);
        url += '&codEmpresa=' + $("#CODEMPRESA").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#LocalizarFavorecido .close").click();
                if (data.length > 0) {

                    var arr = data.split("/");
                    $("#CODFORNECEDOR").val(arr[0]);
                    $("#NMFORNECEDOR").val(arr[1]);


                } else {
                    addErros("CODFORNECEDOR", "Favorecido não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("CODFORNECEDOR", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#CODFORNECEDOR").val('');
        $("#NMFORNECEDOR").val('');
    }

}

////////EMISSÃO DE BOLETOS ---------------------------

$("#LINHADIGITAVEL").on("paste", function () {

    setTimeout(function () {
        var value = $("#LINHADIGITAVEL").val();
        var url = montaUrl('/ContasPagar/EmitirTitulo/ValidaLinhaDigitavel/?linhadigitavel=' + value);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                var value = $("#LINHADIGITAVEL").val();
                var TipoBoleto = value.substring(0, 1);
                if (parseInt(TipoBoleto) != 8) {
                    $("#LINHADIGITAVELCOBRANCA1").val($("#LINHADIGITAVEL").val().substring(0, 5));
                    $("#LINHADIGITAVELCOBRANCA2").val($("#LINHADIGITAVEL").val().substring(5, 10));
                    $("#LINHADIGITAVELCOBRANCA3").val($("#LINHADIGITAVEL").val().substring(10, 15));
                    $("#LINHADIGITAVELCOBRANCA4").val($("#LINHADIGITAVEL").val().substring(15, 21));
                    $("#LINHADIGITAVELCOBRANCA5").val($("#LINHADIGITAVEL").val().substring(21, 26));
                    $("#LINHADIGITAVELCOBRANCA6").val($("#LINHADIGITAVEL").val().substring(26, 32));
                    $("#LINHADIGITAVELCOBRANCA7").val($("#LINHADIGITAVEL").val().substring(32, 33));
                    $("#LINHADIGITAVELCOBRANCA8").val($("#LINHADIGITAVEL").val().substring(33, 47));


                    $("#LINHADIGITAVELCOBRANCA1").css("width", "75%");
                    $("#LINHADIGITAVELCOBRANCA2").css("width", "75%");
                    $("#LINHADIGITAVELCOBRANCA3").css("width", "75%");
                    $("#LINHADIGITAVELCOBRANCA4").css("width", "75%");
                    $("#LINHADIGITAVELCOBRANCA5").css("width", "75%");
                    $("#LINHADIGITAVELCOBRANCA6").css("width", "75%");
                    $("#LINHADIGITAVELCOBRANCA7").css("width", "27%");
                    $("#LINHADIGITAVELCOBRANCA8").css("width", "40%");

                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA2")[0].style.marginLeft = "-3%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA3")[0].style.marginLeft = "-3%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA4")[0].style.marginLeft = "-3%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA5")[0].style.marginLeft = "-3%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA6")[0].style.marginLeft = "-3%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA7")[0].style.marginLeft = "-3%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA8")[0].style.marginLeft = "-6%"


                    $("#LINHADIGITAVELCOBRANCA1").attr('maxlength', '5');
                    $("#LINHADIGITAVELCOBRANCA2").attr('maxlength', '5');
                    $("#LINHADIGITAVELCOBRANCA3").attr('maxlength', '5');
                    $("#LINHADIGITAVELCOBRANCA4").attr('maxlength', '6');
                    $("#LINHADIGITAVELCOBRANCA5").attr('maxlength', '5');
                    $("#LINHADIGITAVELCOBRANCA6").attr('maxlength', '6');
                    $("#LINHADIGITAVELCOBRANCA7").attr('maxlength', '1');
                    $("#LINHADIGITAVELCOBRANCA8").attr('maxlength', '14');

                    $(".LINHADIGITAVEL").hide();
                    $("#BOLETOCOBRANCA").show();
                    $("#LINHADIGITAVELCOBRANCA8").focus();

                    $("#VALORBOLETO").val(data.valorboleto);
                    $("#DATVENCIMENTOBOLETO").val(data.datvencimentoboleto);
                    $("#DATREALPGTOBOLETO").val(data.datrealpgtoboleto);
                }
                else {
                    $("#LINHADIGITAVELCOBRANCA1").val($("#LINHADIGITAVEL").val().substring(0, 11));
                    $("#LINHADIGITAVELCOBRANCA2").val($("#LINHADIGITAVEL").val().substring(11, 12));
                    $("#LINHADIGITAVELCOBRANCA3").val($("#LINHADIGITAVEL").val().substring(12, 23));
                    $("#LINHADIGITAVELCOBRANCA4").val($("#LINHADIGITAVEL").val().substring(23, 24));
                    $("#LINHADIGITAVELCOBRANCA5").val($("#LINHADIGITAVEL").val().substring(24, 35));
                    $("#LINHADIGITAVELCOBRANCA6").val($("#LINHADIGITAVEL").val().substring(35, 36));
                    $("#LINHADIGITAVELCOBRANCA7").val($("#LINHADIGITAVEL").val().substring(36, 47));
                    $("#LINHADIGITAVELCOBRANCA8").val($("#LINHADIGITAVEL").val().substring(47, 48));

                    $("#LINHADIGITAVELCOBRANCA1").attr('maxlength', '11');
                    $("#LINHADIGITAVELCOBRANCA2").attr('maxlength', '1');
                    $("#LINHADIGITAVELCOBRANCA3").attr('maxlength', '11');
                    $("#LINHADIGITAVELCOBRANCA4").attr('maxlength', '1');
                    $("#LINHADIGITAVELCOBRANCA5").attr('maxlength', '11');
                    $("#LINHADIGITAVELCOBRANCA6").attr('maxlength', '1');
                    $("#LINHADIGITAVELCOBRANCA7").attr('maxlength', '11');
                    $("#LINHADIGITAVELCOBRANCA8").attr('maxlength', '1');

                    $("#LINHADIGITAVELCOBRANCA1").css("width", "121%");
                    $("#LINHADIGITAVELCOBRANCA2").css("width", "26%");
                    $("#LINHADIGITAVELCOBRANCA3").css("width", "121%");
                    $("#LINHADIGITAVELCOBRANCA4").css("width", "26%");
                    $("#LINHADIGITAVELCOBRANCA5").css("width", "121%");
                    $("#LINHADIGITAVELCOBRANCA6").css("width", "26%");
                    $("#LINHADIGITAVELCOBRANCA7").css("width", "121%");
                    $("#LINHADIGITAVELCOBRANCA8").css("width", "7%");

                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA2")[0].style.marginLeft = "0%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA3")[0].style.marginLeft = "-6%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA4")[0].style.marginLeft = "0%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA5")[0].style.marginLeft = "-6%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA6")[0].style.marginLeft = "0%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA7")[0].style.marginLeft = "-6%"
                    document.getElementsByClassName("LINHADIGITAVELCOBRANCA8")[0].style.marginLeft = "0%"


                    //82630000000-5 46190361201-2 91018000003 - 6 09080920191 - 7

                    $(".LINHADIGITAVEL").hide();
                    $("#BOLETOCOBRANCA").show();
                    $("#LINHADIGITAVELCOBRANCA8").focus();


                    $("#VALORBOLETO").val(data.valorboleto);
                    $("#DATVENCIMENTOBOLETO").val(data.datvencimentoboleto);
                    $("#DATREALPGTOBOLETO").val(data.datrealpgtoboleto);

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }, 500);
});



$("#LINHADIGITAVEL").focusout(function () {

    $("#LINHADIGITAVEL").attr('maxlength', '90');
    $("#LINHADIGITAVEL").val().replace(/[^\d]+/g, '');

});

$("#LINHADIGITAVEL").on("keypress", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    var ctrl = window.event.ctrlKey;
    var tecla = window.event.keyCode;
    if (!(ctrl && tecla == 86)) {
        var value = $("#LINHADIGITAVEL").val();
        $("#LINHADIGITAVEL").attr('maxlength', '90');
        $("#LINHADIGITAVEL").val().replace(/[^\d]+/g, '');

        if (value.length >= 1) {
            var TipoBoleto = value.substring(0, 1);
            if (parseInt(TipoBoleto) == 8) {
                $("#LINHADIGITAVELCOBRANCA1").val($("#LINHADIGITAVEL").val());

                $("#LINHADIGITAVELCOBRANCA1").attr('maxlength', '11');
                $("#LINHADIGITAVELCOBRANCA2").attr('maxlength', '1');
                $("#LINHADIGITAVELCOBRANCA3").attr('maxlength', '11');
                $("#LINHADIGITAVELCOBRANCA4").attr('maxlength', '1');
                $("#LINHADIGITAVELCOBRANCA5").attr('maxlength', '11');
                $("#LINHADIGITAVELCOBRANCA6").attr('maxlength', '1');
                $("#LINHADIGITAVELCOBRANCA7").attr('maxlength', '11');
                $("#LINHADIGITAVELCOBRANCA8").attr('maxlength', '1');

                $("#LINHADIGITAVELCOBRANCA1").css("width", "121%");
                $("#LINHADIGITAVELCOBRANCA2").css("width", "26%");
                $("#LINHADIGITAVELCOBRANCA3").css("width", "121%");
                $("#LINHADIGITAVELCOBRANCA4").css("width", "26%");
                $("#LINHADIGITAVELCOBRANCA5").css("width", "121%");
                $("#LINHADIGITAVELCOBRANCA6").css("width", "26%");
                $("#LINHADIGITAVELCOBRANCA7").css("width", "121%");
                $("#LINHADIGITAVELCOBRANCA8").css("width", "7%");

                document.getElementsByClassName("LINHADIGITAVELCOBRANCA2")[0].style.marginLeft = "0%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA3")[0].style.marginLeft = "-6%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA4")[0].style.marginLeft = "0%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA5")[0].style.marginLeft = "-6%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA6")[0].style.marginLeft = "0%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA7")[0].style.marginLeft = "-6%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA8")[0].style.marginLeft = "0%"

                $(".LINHADIGITAVEL").hide();
                $("#BOLETOCOBRANCA").show();
                $("#LINHADIGITAVELCOBRANCA1").focus();
            }
            else {
                $("#LINHADIGITAVELCOBRANCA1").val($("#LINHADIGITAVEL").val());

                $("#LINHADIGITAVELCOBRANCA1").css("width", "75%");
                $("#LINHADIGITAVELCOBRANCA2").css("width", "75%");
                $("#LINHADIGITAVELCOBRANCA3").css("width", "75%");
                $("#LINHADIGITAVELCOBRANCA4").css("width", "75%");
                $("#LINHADIGITAVELCOBRANCA5").css("width", "75%");
                $("#LINHADIGITAVELCOBRANCA6").css("width", "75%");
                $("#LINHADIGITAVELCOBRANCA7").css("width", "27%");
                $("#LINHADIGITAVELCOBRANCA8").css("width", "40%");

                document.getElementsByClassName("LINHADIGITAVELCOBRANCA2")[0].style.marginLeft = "-3%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA3")[0].style.marginLeft = "-3%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA4")[0].style.marginLeft = "-3%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA5")[0].style.marginLeft = "-3%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA6")[0].style.marginLeft = "-3%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA7")[0].style.marginLeft = "-3%"
                document.getElementsByClassName("LINHADIGITAVELCOBRANCA8")[0].style.marginLeft = "-6%"

                $("#LINHADIGITAVELCOBRANCA1").attr('maxlength', '5');
                $("#LINHADIGITAVELCOBRANCA2").attr('maxlength', '5');
                $("#LINHADIGITAVELCOBRANCA3").attr('maxlength', '5');
                $("#LINHADIGITAVELCOBRANCA4").attr('maxlength', '6');
                $("#LINHADIGITAVELCOBRANCA5").attr('maxlength', '5');
                $("#LINHADIGITAVELCOBRANCA6").attr('maxlength', '6');
                $("#LINHADIGITAVELCOBRANCA7").attr('maxlength', '1');
                $("#LINHADIGITAVELCOBRANCA8").attr('maxlength', '14');

                $(".LINHADIGITAVEL").hide();
                $("#BOLETOCOBRANCA").show();
                $("#LINHADIGITAVELCOBRANCA1").focus();



            }
        }
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA2").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA2").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA1").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA3").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA3").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA2").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA4").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA4").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA3").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA5").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA5").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA4").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA6").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA6").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA5").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA7").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA7").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA6").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA8").on("keydown", function () {

    evt = window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 8 && $("#LINHADIGITAVELCOBRANCA8").val().length == 0) {
        $("#LINHADIGITAVELCOBRANCA7").focus();
    }
    return true;
});
$("#LINHADIGITAVELCOBRANCA1").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA1").val().length == 5) {
                $("#LINHADIGITAVELCOBRANCA2").focus();
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA1").val().length == 11) {
                $("#LINHADIGITAVELCOBRANCA2").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA2").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA2").val().length == 5) {
                $("#LINHADIGITAVELCOBRANCA3").focus();
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA2").val().length == 1) {
                $("#LINHADIGITAVELCOBRANCA3").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA3").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA3").val().length == 5) {
                $("#LINHADIGITAVELCOBRANCA4").focus();
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA3").val().length == 11) {
                $("#LINHADIGITAVELCOBRANCA4").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA4").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA4").val().length == 6) {
                $("#LINHADIGITAVELCOBRANCA5").focus();
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA4").val().length == 1) {
                $("#LINHADIGITAVELCOBRANCA5").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA5").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA5").val().length == 5) {
                $("#LINHADIGITAVELCOBRANCA6").focus();
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA5").val().length == 11) {
                $("#LINHADIGITAVELCOBRANCA6").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA6").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA6").val().length == 6) {
                $("#LINHADIGITAVELCOBRANCA7").focus();
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA6").val().length == 1) {
                $("#LINHADIGITAVELCOBRANCA7").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA7").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA7").val().length == 1) {
                $("#LINHADIGITAVELCOBRANCA8").focus();
                $("#LINHADIGITAVELCOBRANCA8").attr('maxlength', '14');
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA7").val().length == 11) {
                $("#LINHADIGITAVELCOBRANCA8").focus();
            }
        }
    }
});
$("#LINHADIGITAVELCOBRANCA8").on("keyup", function () {

    if (ValidaPossuiDados()) {
        var LinhaDigitavel = $("#LINHADIGITAVELCOBRANCA1").val() +
            $("#LINHADIGITAVELCOBRANCA2").val() +
            $("#LINHADIGITAVELCOBRANCA3").val() +
            $("#LINHADIGITAVELCOBRANCA4").val() +
            $("#LINHADIGITAVELCOBRANCA5").val() +
            $("#LINHADIGITAVELCOBRANCA6").val() +
            $("#LINHADIGITAVELCOBRANCA7").val() +
            $("#LINHADIGITAVELCOBRANCA8").val();


        var value = $("#LINHADIGITAVEL").val();
        var TipoBoleto = value.substring(0, 1);
        if (parseInt(TipoBoleto) != 8) {
            if ($("#LINHADIGITAVELCOBRANCA8").val().length == 14) {
                ValidaLinhaDigitavel(LinhaDigitavel);
            }
        }
        else {
            if ($("#LINHADIGITAVELCOBRANCA8").val().length == 1) {
                ValidaLinhaDigitavel(LinhaDigitavel);
            }
        }
    }
});
function ValidaPossuiDados() {
    var qtCaracteres = 0;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA1").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA2").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA3").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA4").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA5").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA6").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA7").val().length;
    qtCaracteres += $("#LINHADIGITAVELCOBRANCA8").val().length;

    if (qtCaracteres == 0) {
        $("#BOLETOCOBRANCA").hide();
        $("#LINHADIGITAVEL").val("");
        $(".LINHADIGITAVEL").show();
        $("#LINHADIGITAVEL").focus();
        return false;
    }
    return true;
};

function ValidaLinhaDigitavel(linhaDigitavel) {
    setTimeout(function () {

        var url = montaUrl('/ContasPagar/EmitirTitulo/ValidaLinhaDigitavel/?linhadigitavel=' + linhaDigitavel);

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#VALORBOLETO").val(data.valorboleto);
                $("#DATVENCIMENTOBOLETO").val(data.datvencimentoboleto);
                $("#DATREALPGTOBOLETO").val(data.datrealpgtoboleto);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }, 500);

};
function ProcessarArquivo(TipoArquivo) {

    if (TipoArquivo == 'NC') {
        var myfile = document.getElementById("ArquivoTituloNC");
    }
    else {
        var myfile = document.getElementById("ArquivoTituloMassa");
    }
    var formData = new FormData();

    if (myfile.files.length > 0) {
        $.blockUI({ message: '<div class="ModalCarregando"></dic>' });
        for (var i = 0; i < myfile.files.length; i++) {
            formData.append('file-' + i, myfile.files[i]);
        }

        var _url = montaUrl("/ContasPagar/EmitirTitulo/uploadArquivo/");
        _url += "&tipoArquivo=" + TipoArquivo;
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

                    if (!data.arquivovalido) {
                        msgErro(data.descricao)
                    }
                    else {
                        var url = montaUrl("/ContasPagar/EmitirTitulo/LogProcessoInclusaoMassa/")
                        url += "&caminho=" + data.responseText;
                        url += "&tipoArquivo=" + TipoArquivo;


                        $("#modalLogProcesso").load(url);
                        $("#LogProcesso").modal();
                    }
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
function GerarTituloEmMassa() {
    var table = $('#gridDadosUsuarioNC').DataTable();
    console.log($("#CODEMPRESA").val())

    var trs = $(table.cells().nodes()).parent();
    trs.each(function (index, value) {

        var codFavorecidoNC = $(value).find('.codFavorecidoNC').find('input').val();
        var DataPGTO = $(value).find('.DataPGTO').find('input').val();
        var vlLiquido = $(value).find('.vlLiquido').find('input').val();


        $(value).find('.inpStatusProcessamento')[0].innerHTML = "Enviando...";

        var data = "codFavorecidoNC=" + codFavorecidoNC;
        data += "&DataPGTO=" + DataPGTO;
        data += "&vlLiquido=" + vlLiquido;
        data += "&codEmpresa=" + $("#CODEMPRESA").val();
        

        var _url = montaUrl("/ContasPagar/EmitirTitulo/IncluirTituloEmMassa")

        ////Submit form data via ajax
        $.ajax({
            type: "Post",
            url: _url,
            data: data,
            success: function (data) {
                $.unblockUI();
                if (data.success) {
                    $(value).find('.inpStatusProcessamento')[0].innerHTML = "Enviado";
                    $(value).find('.inpLogProcessamento')[0].innerHTML = data.responseText;

                }
                else {
                    $(value).find('.inpStatusProcessamento')[0].innerHTML = "Falha";
                    $(value).find('.inpLogProcessamento')[0].innerHTML = data.responseText;
                }
                $("#btnGerarTituloEmMassa").prop('disabled', true)
                msgSucesso("Processo Finalizado");
            },
            error: function (data) {
                $.unblockUI();
                msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
            }
        });
    });

};
function FechaJanelaLogProcesso() {
    $("#LogProcesso .close").click();
}