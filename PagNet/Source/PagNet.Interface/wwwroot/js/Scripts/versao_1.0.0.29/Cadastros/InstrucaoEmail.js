
function carregaPagina() {

    if ($("#acessoAdmin").val() == 'True') {
        $("#FiltroEmpresa").show();
    }
}
$(window).load(carregaPagina);

$(".codEmpresa").change(function () {
    $("#codEmpresa").val($('.codEmpresa option:selected').val());
});

function ConfirmaSalvar() {
    var validado = true;

    var form = $('form', '#page-inner');

    var url = montaUrl("/Cadastros/InstrucaoEmail/Salvar/");

    if (validado) {
        form.attr('action', url);
        form.submit();
    }
}
