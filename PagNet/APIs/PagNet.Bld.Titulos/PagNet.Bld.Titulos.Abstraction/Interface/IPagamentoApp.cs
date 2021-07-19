using PagNet.Bld.Titulos.Abstraction.Interface.Model;
using PagNet.Bld.Titulos.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Titulos.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        APIRetornoModal IncluirNovoTituloPGTO(IAPIDadosTituloModel vm);
        APIRetornoModal SalvaEdicaoTitulo(IAPIDadosTituloModel model);
        APIRetornoModal SalvarAjusteValorTitulo(IAPIDadosTituloModel model);
        List<TaxasCobradasPGTOModel> ListarTaxasTitulo(int codigoTitulo);

        APIRetornoModal PagamentoManual(IAPIFiltroBorderoModel model);
        APIRetornoModal BaixaManualByID(IAPIFiltroBorderoModel model);
        APIRetornoModal DesvinculaTitulo(IAPIFiltroBorderoModel model);
        APIRetornoModal CancelarTitulo(IAPIFiltroBorderoModel model);

        APIRetornoModal AlteraDataPGTOEmMassa(IGridTitulosModel vm);
        GridTitulosModel GetAllTitulosVencidos(IAPIFiltroTitulosPagamentoModel vm);
        APIFiltroBorderoModel CarregaGridTitulos(IAPIFiltrotituloModel model);

        //List<APIDadosTituloModel> ConsultaTransacaoPagamento(APIDadosTituloModel model);
        //List<APIDadosTituloModel> ConsultaTitulosByFavorecidoDatPGTO(int codEmpresa, int codFavorecido, DateTime datPGTO);
        //List<LogTituloModel> ConsultaLog(int CODIGOTITULO);
        //List<APIDadosTituloModel> GetAllPagamentosBordero(int CodBordero);
        //List<APIDadosTituloModel> GetAllPagamentosArquivo(int CodArquivo, string dtArquivo);


        //APIDadosTituloModel RetornaDadosTitulo(int codFechCred);
        //APIFiltroTitulosPagamentoModel FindBancoByID(int id);

        //List<APIDadosTituloModel> ListaTitulosNaoLiquidado(int codEmpresa);

        //List<APIDadosTituloModel> CarregaArquivoTituloMassaNC(string CaminhoArquivo, int codEmpresa, int CodUsuario);
        //List<APIDadosTituloModel> CarregaArquivoTituloMassa(string CaminhoArquivo, int codEmpresa, int CodUsuario);

        //APIRetornoDDLModel DDLListaTitulosPendentes(int codFavorecido);

        ////Borderô Pagamento
        //APIRetornoModal SalvaBordero(IAPIFiltroBorderoModel model);
        //BorderoPagamentoModel ConsultaBordero(IAPIFiltroBorderoModel model);
        //void CancelaBordero(int codBordero, int codUsuario);

    }
}
