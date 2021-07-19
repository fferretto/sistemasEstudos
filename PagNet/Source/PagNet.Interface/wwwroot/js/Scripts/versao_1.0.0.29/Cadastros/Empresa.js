function carregaPagina() {

    if ($('#UTILIZANETCARD').val() == "True") {
        $("#btnSim").click();
        $("#liNao").removeClass('active');
        $("#liSim").addClass('active');
    }
    var codEmpresa = parseInt($('#CODEMPRESA').val());
    if (codEmpresa > 0) {
        $('#NMLOGIN').prop('disabled', true);
        $('#btnSim').prop('disabled', true);
        $('#btnNao').prop('disabled', true);
        $('.CODSUBREDE').prop('disabled', true);
        
    }

    $('#LocalizandoEndereco').hide();
    $('#LOGRADOURO').prop('disabled', true);
    $('#BAIRRO').prop('disabled', true);
    $('#CIDADE').prop('disabled', true);
    $('#UF').prop('disabled', true);  

}
$(window).load(carregaPagina);

$("#btnSim").click(function () {
    $("#UTILIZANETCARD").val('True');
})
$("#btnNao").click(function () {
    $("#UTILIZANETCARD").val('False');
})

$('#CNPJ').focusout(function () {

    var CNPJ = $('#CNPJ').val();

    if (CNPJ != "") {
        CNPJ = CNPJ.replace('/', '').replace('.', '').replace('-', '')
        if (!validarCNPJ(CNPJ)) {
            $('#CNPJ').addClass("borderError");
            addErros("CNPJ", "CNPJ Inválido!")
        }
        else {
            //$('#CpfCnpj').val(cpf_mask(CNPJ))
            $('#CNPJ').removeClass("borderError");
            $("span[data-valmsg-for='CNPJ']").hide();
        }
    }
})


$(".CEP").change(function () {

    var _url = montaUrl("/Cadastros/Empresa/BuscaEndereco");
    var data = "cpf=" + $("#CEP").val();
    $('#LocalizandoEndereco').show();
    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            if (data.endereco != null) {

                $('#LOGRADOURO').val(data.endereco);
                $('#BAIRRO').val(data.localidadeBairroDescricao);
                $('#CIDADE').val(data.localidadeMunicipioDescricao);
                $('#UF').val(data.localidadeUfDescricao);  
                $('#LocalizandoEndereco').hide();

                $('#LOGRADOURO').prop('disabled', true);
                $('#BAIRRO').prop('disabled', true);
                $('#CIDADE').prop('disabled', true);
                $('#UF').prop('disabled', true);
            }
            else {

                $('#LOGRADOURO').val("");
                $('#BAIRRO').val("");
                $('#CIDADE').val("");
                $('#UF').val("");  

                $('#LOGRADOURO').prop('disabled', false);
                $('#BAIRRO').prop('disabled', false);
                $('#CIDADE').prop('disabled', false);
                $('#UF').prop('disabled', false);

                $('#LocalizandoEndereco').hide();
            }
        },
        error: function (data) {
            $('#LocalizandoEndereco').hide();
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');

            $('#LOGRADOURO').val("");
            $('#BAIRRO').val("");
            $('#CIDADE').val("");
            $('#UF').val("");

            $('#LOGRADOURO').prop('disabled', false);
            $('#BAIRRO').prop('disabled', false);
            $('#CIDADE').prop('disabled', false);
            $('#UF').prop('disabled', false);
        }
    });
});

$(".CODSUBREDE").change(function () {
    $("#CODSUBREDE").val($('.CODSUBREDE option:selected').val());

    var _url = montaUrl("/Cadastros/Empresa/BuscaDadosSubRede");
    var data = "id=" + $("#CODSUBREDE").val();

    $.ajax({
        type: "Post",
        url: _url,
        data: data,
        success: function (data) {
            if (data != null) {

                $('#RAZAOSOCIAL').val(data.razaosocial);
                $('#NMFANTASIA').val(data.nomsubrede);
                $('#CNPJ').val(data.cnpj);
                $('#CEP').val(data.cep);
                $('#LOGRADOURO').val(data.logradouro);
                $('#NROLOGRADOURO').val(data.nrologradouro);
                $('#COMPLEMENTO').val(data.complemento);
                $('#BAIRRO').val(data.bairro);
                $('#CIDADE').val(data.cidade);
                $('#UF').val(data.uf);

            }
        },
        error: function (data) {
            msgErro('Ocorreu uma falha durante o processo. favor contactar o suporte técnico!');
        }
    });
   
});

function LocalizaEmpresa() {

    var url = montaUrl("/Cadastros/Empresa/ConsultaEmpresas")

    $("#modal").load(url);
    $("#Localizar").modal();
}

function ConfirmaSalvar() {

    var form = $('form', '#page-inner');

    $("#CODSUBREDE").val($('.CODSUBREDE option:selected').val());
    $('#LOGRADOURO').prop('disabled', false);
    $('#BAIRRO').prop('disabled', false);
    $('#CIDADE').prop('disabled', false);
    $('#UF').prop('disabled', false);
    $('#NMLOGIN').prop('disabled', false);

    var url = montaUrl("/Cadastros/Empresa/Salvar/");

        form.attr('action', url);
        form.submit();

}