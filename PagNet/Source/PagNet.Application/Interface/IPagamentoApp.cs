using PagNet.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IPagamentoApp
    {

        Task<string> SalvaBordero(FiltroBorderoPagVM model);
        Task<IDictionary<bool, string>> IncluirNovoTituloPGTO(EmissaoTituloAvulsto vm);
        Task<IDictionary<string, string>> SalvaEdicaoTitulo(DadosTituloVm model);
        Task<IDictionary<bool, string>> SalvarAjusteValorTitulo(AjustarValorTitulo model);
        Task<string> PagamentoManualAsync(FiltroBorderoPagVM model);
        Task<string> BaixaManualByID(FiltroBorderoPagVM model);
        Task<string> DesvinculaTitulo(FiltroBorderoPagVM model);
        Task<IDictionary<bool, string>> CancelarTitulo(FiltroBorderoPagVM model);
        Task<IDictionary<string, string>> AlteraDataPGTOEmMassa(GridTitulosVM vm);
        GridTitulosVM GetAllTitulosVencidos(FiltroTitulosPagamentoVM vm);
        FiltroBorderoPagVM CarregaGridTitulos(FiltroTitulosPagamentoVM model);
        Task<List<TaxasCobradasPGTOVm>> ListarTaxasTitulo(int codTitulo);
        FiltroTitulosPagamentoVM FindBancoByID(int id);
        Task<IDictionary<string, string>> ProcessaBaixaPagamento(List<BaixaPagamentoVM> model, int codUsuario);
        Task<List<ListaTitulosPagVM>> ConsultaTransacaoPagamento(ConsultaFechamentoCredVm model);
        Task<List<ListaTitulosPGTOVM>> ConsultaTitulosByFavorecidoDatPGTO(int codEmpresa, int codFavorecido, DateTime datPGTO);
        Task<List<LogTituloVM>> ConsultaLog(int CODIGOTITULO);
        Task<List<ConstulaPagNetFechCredVM>> GetAllPagamentosBordero(int CodBordero);
        Task<List<ConstulaPagNetFechCredVM>> GetAllPagamentosArquivo(int CodArquivo, string dtArquivo);
        ConsultaFechamentoCredVm RetornaDadosInicioConsultaCred(string dados, int codSubRede);
        FiltroTitulosPagamentoVM RetornaDadosInicio(string dados, int codSubRede);
        FiltroDownloadArquivoVm RetornaDadosInicioDownloadCred(int codSubRede);
        IndicaroresVM RetornaDadosInicioIndicadoresCred(int codSubRede);
        FiltroConsultaBorderoPagVM RetornaDadosInicioConstulaBordero(string dados, int codSubRede);
        Task<List<ListaEmissaoTitulosVM>> ListaTitulosNaoLiquidado(int codEmpresa);
        Task<List<ListaTitulosInclusaoMassa>> CarregaArquivoTituloMassaNC(string CaminhoArquivo, int codEmpresa, int CodUsuario);
        Task<List<ListaTitulosInclusaoMassa>> CarregaArquivoTituloMassa(string CaminhoArquivo, int codEmpresa, int CodUsuario);
        object[][] DDLListaTitulosPendentes(int codFavorecido);
        BorderoPagVM ConsultaBordero(FiltroConsultaBorderoPagVM model);
        void CancelaBordero(int codBordero, int codUsuario);
        Task<DadosTituloVm> RetornaDadosTitulo(int codFechCred);
        Task<List<BaixaPagamentoVM>> CarregaDadosArquivoRemessa(string CaminhoArquivo);

        Task<List<ConsultaArquivosRemessaVM>> CarregaGridArquivo(FiltroDownloadArquivoVm model);
        Task<IDictionary<string, string>> AtualizaStatusArquivoByName(string nmArquivo, string status);
        Task<IDictionary<string, string>> AtualizaStatusArquivoByID(int codCarquivo, string status);
        Task<IDictionary<string, string>> CancelaArquivoRemessaByID(int codCarquivo, int codUsuario);

    }
}
