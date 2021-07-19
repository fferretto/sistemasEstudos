function carregaPagina() {

}
$(window).load(carregaPagina);

function CarregaGrid() {
    
    var url = montaUrl("/Configuracao/PerfisAcesso/AbrirJanelaModal/")

    $("#modal").load(url);
    $("#Localizar").modal();
}