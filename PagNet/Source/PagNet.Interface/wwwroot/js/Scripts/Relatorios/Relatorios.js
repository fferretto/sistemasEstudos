
function carregaPagina() {
    var urlRel = montaUrl('/Generico/VisualizadorRelatorioPDF/RelPDF/')
    $("#urlRelatorio").val(urlRel);

    $("#dvNovoRelatorio").show();

    if ($("#PossuiOutroRelatorioSendoGerado").val() == "True") {
        $("#dvNovoRelatorio").hide();
        $("#dvOutroSendoGerado").show();
    }
    else if ($("#statusGeracao").val() == "EM_ANDAMENTO") {
        $("#dvNovoRelatorio").hide();
        $("#dvEmGeracao").show();
        setTimeout("VerificaConclusaoRelatorio()", 5000);
    }
    else if ($("#statusGeracao").val() == "FINALIZADO") {
        $("#dvNovoRelatorio").hide();
        $("#dvDownloadRel").show();
    }
}

$(window).load(carregaPagina());


function VisualizaRel() {

    if ($("#ExecutaViaJob").val() == "S") {
        $("#TipoRelatorio").val('0')
        GeraRelatorioViaJob();
    }
    else {

        for (var i = 0; i < document.getElementsByClassName('PagNet-ddl').length; i++) {
            var idCampo = document.getElementsByClassName("PagNet-ddl")[i].id;

            $(idCampo).val($('.' + idCampo + ' option:selected').val());
        }

        var form = $("form", "#page-inner");

        var url = montaUrl('/Relatorios/Relatorios/VisualizaRel')

        //submit via post
        form.attr('action', url);
        form.attr("target", "_blank");
        form.submit();
    }
}
function ExportaRel() {

    if ($("#ExecutaViaJob").val() == "S") {
        $("#TipoRelatorio").val('1')
        GeraRelatorioViaJob();
    }
    else {
        for (var i = 0; i < document.getElementsByClassName('PagNet-ddl').length; i++) {
            var idCampo = document.getElementsByClassName("PagNet-ddl")[i].id;

            $(idCampo).val($('.' + idCampo + ' option:selected').val());

        }

        var form = $("form", "#page-inner");
        var _url = montaUrl('/Relatorios/Relatorios/ExportaExcel')

        var data = form.serialize();

        // Submit form data via ajax
        $.ajax({
            type: "Post",
            url: _url,
            data: data,
            success: function (data) {
                $.unblockUI();
                if (data != 'Erro') {
                    if (data.caminhoArquivo == "") {
                        msgAviso("Nenhum registro encontrado!");

                    }
                    else {

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

                                        window.location.href = (montaUrl('/Relatorios/Relatorios/DownloadArquivo/?url=' + data.caminhoArquivo));
                                    }
                                },
                                cancel: {
                                    text: 'Não'
                                }
                            }
                        });     

                    }
                }
                else {
                    msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
                }

            },
            error: function (data) {
                $.unblockUI();
                msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
            }
        });

    }
}
function GeraRelatorioViaJob() {

    for (var i = 0; i < document.getElementsByClassName('PagNet-ddl').length; i++) {
        var idCampo = document.getElementsByClassName("PagNet-ddl")[i].id;

        $(idCampo).val($('.' + idCampo + ' option:selected').val());
    }

    var form = $("form", "#page-inner");

    var url = montaUrl('/Relatorios/Relatorios/GeraRelatorioViaJob');

    var data = form.serialize();
    $.ajax({
        type: "Post",
        url: url,
        data: data,
        success: function (data) {
            $.unblockUI();
            if (data != 'Erro') {
                $("#dvNovoRelatorio").hide();
                $("#dvEmGeracao").show();
                setTimeout("VerificaConclusaoRelatorio()", 1000);
            }
            else {
                msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
            }

        },
        error: function (data) {
            if (data.status && data.status == 409)
                msgErro(data.responseText);
            else
                msgErro("Ocorreu uma falha durante o processo.favor contactar o suporte técnico!");
        }
    });

}
function RetornoRelatornoViaJob() {

    $("#dvDownloadRel").hide();
    $("#dvNovoRelatorio").show();

    for (var i = 0; i < document.getElementsByClassName('PagNet-ddl').length; i++) {
        var idCampo = document.getElementsByClassName("PagNet-ddl")[i].id;

        $(idCampo).val($('.' + idCampo + ' option:selected').val());
    }

    var form = $("form", "#page-inner");

    var url = montaUrl('/Relatorios/Relatorios/RetornoRelatornoViaJob');

    var data = form.serialize();
    if ($("#TipoRelatorio").val() == 0) {

        form.attr('action', url);
        form.attr("target", "_blank");
        form.submit();

    }
    else {
        // Submit form data via ajax
        $.ajax({
            type: "Post",
            url: url,
            data: data,
            success: function (data) {
                window.location.href = (montaUrl('/Relatorios/Relatorios/DownloadArquivo/?url=' + data.caminhoArquivoRet));
            },
            error: function (data) {
                if (data.status && data.status == 409)
                    msgErro(data.responseText);
                else
                    msgErro("Ocorreu uma falha durante o processo.favor contactar o suporte técnico!");
            }
        });
    }    

}
function VerificaConclusaoRelatorio() {

    var _url = montaUrl('/Relatorios/Relatorios/VerificaConclusaoRelatorio')
    var data = "codigoRelatorio=" + $("#codRel").val()
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            console.log(data.concluido)
            if (data.concluido) {
                $("#dvEmGeracao").hide();
                $("#dvDownloadRel").show();
            }
            else {
                setTimeout("VerificaConclusaoRelatorio()", 7000);
            }

        },
        error: function (data) {
            if (data.status && data.status == 409)
                msgErro(data.responseText);
            else
                msgErro("Ocorreu uma falha durante o processo.favor contactar o suporte técnico!");
        }
    });

}