function carregaPagina() {

    $("#nomeFavorecido").prop('disabled', true);
    
    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
  
}

$(window).load(carregaPagina());

$(".codigoEmpresa").change(function () {
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
});

$(".codigoTitulo").change(function () {
    $("#codigoTitulo").val($('.codigoTitulo option:selected').val());
});


function CalcularTaxas() {

    $('#modal').empty();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    $("#codigoTitulo").val($('.codigoTitulo option:selected').val());


    var url = montaUrl("/ContasPagar/AntecipacaoPGTO/ListaTitulosAntecipacao/?codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&codigoFavorecido=" + $("#codigoFavorecido").val()
        + "&codigoTitulo=" + $("#codigoTitulo").val()
        + "&dtAntecipacao=" + $("#dtAntecipacao").val())

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    $("#modal").load(url);
    $("#ListaTitulos").show();

};
function LocalizaFavorecido() {

    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
    var url = montaUrl("/ContasPagar/AntecipacaoPGTO/ConsultaFavorecido")
    url += '&codEmpresa=' + $("#codigoEmpresa").val();

    $("#modalFavorecido").load(url);
    $("#LocalizarFavorecido").modal();
}
function SelecionaFavorecido(CODFAVORECIDO) {

    if (CODFAVORECIDO != "") {

        $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
        var url = montaUrl('/ContasPagar/AntecipacaoPGTO/BuscaFavorecido/?filtro=' + CODFAVORECIDO);
        url += '&codEmpresa=' + $("#codigoEmpresa").val();

        $.ajax({
            type: 'get',
            url: url,
            dataType: 'json',
            success: function (data) {
                $("#LocalizarFavorecido .close").click();
                if (data.length > 0) {

                    var arr = data.split("/");
                    $("#codigoFavorecido").val(arr[0]);
                    $("#nomeFavorecido").val(arr[1]);


                } else {
                    addErros("codigoFavorecido", "Favorecido não encontrado");
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                addErros("codigoFavorecido", "Erro interno. Tente novamente mais tarde.");
            }
        });
    }
    else {
        $("#codigoFavorecido").val('');
        $("#nomeFavorecido").val('');
    }
}
    $('#codigoFavorecido').focusout(function () {
        var filtro = $('#codigoFavorecido').val();
        if (filtro != "") {

            $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());
            var url = montaUrl('/ContasPagar/AntecipacaoPGTO/BuscaFavorecido/?filtro=' + filtro);
            url += '&codEmpresa=' + $("#codigoEmpresa").val();

            $.ajax({
                type: 'get',
                url: url,
                dataType: 'json',
                success: function (data) {
                   
                    if (data.length > 0) {

                        var arr = data.split("/");
                        $("#codigoFavorecido").val(arr[0]);
                        $("#nomeFavorecido").val(arr[1]);


                    } else {
                        addErros("codigoFavorecido", "Favorecido não encontrado");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    addErros("codigoFavorecido", "Erro interno. Tente novamente mais tarde.");
                }
            });
        }
        else {
            $("#codigoFavorecido").val('');
            $("#nomeFavorecido").val('');
        }
    });

function SalvarAntecipacaoPagamento() {

    var table = $('#gridListaAntecip').DataTable();
    $("#codigoEmpresa").val($('.codigoEmpresa option:selected').val());


    var meuData = '';
    var trs = $('input[type="checkbox"]:checked', table.cells().nodes()).parent().parent();
    $(trs).each(function (index, value) {
        meuData += '&ListaTitulos%5B' + index + '%5D.Codigo=' + $(value).find('.Codigo').find('input').val();
        meuData += '&ListaTitulos%5B' + index + '%5D.VALATUAL=' + $(value).find('.VALATUAL').find('input').val();
        meuData += '&ListaTitulos%5B' + index + '%5D.VALTOTAL=' + $(value).find('.VALTOTAL').find('input').val();
        
    });

    var data = "codigoEmpresa=" + $("#codigoEmpresa").val()
        + "&NovaDataPGTO=" + $("#dtAntecipacao").val()
        + meuData;

    var _url = montaUrl("/ContasPagar/AntecipacaoPGTO/SalvarAntecipacao")

    $.blockUI({ message: '<div class="ModalCarregando"></dic>' });

    // Submit form data via ajax
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            $.unblockUI();

            if (data.success) {

                msgSucesso(data.responseText);

                var table = $('#gridListaAntecip').DataTable();
                var ocultar = $('input[type="checkbox"]:checked', table.cells().nodes());
                $(ocultar).each((i, e) => $(e).closest('tr').addClass('selected'));
                table.rows('.selected').remove().draw(false);

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

};
