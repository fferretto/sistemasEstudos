using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface ITesourariaApp
    {
        Task<FiltroTransacaoVM> CarregaDadosInicio(int codEmpresa);
        Task<IDictionary<bool, string>> IncluirNovaTransacaoENTRADA(IncluiTransacaoVM vm);
        Task<IDictionary<bool, string>> IncluirNovaTransacaoSAIDA(IncluiTransacaoVM vm);
        Task<IDictionary<bool, string>> IncluirNovaTransacaoJaConciliadaSAIDA(IncluiTransacaoVM vm);
        Task<IDictionary<bool, string>> IncluirNovaTransacaoJaConciliadaENTRADA(IncluiTransacaoVM vm);

        Task<IDictionary<bool, string>> ConsolidaTransacao(EditarTransacaoVM vm);
        Task<IDictionary<bool, string>> CancelaTransacao(EditarTransacaoVM vm);
        Task<IDictionary<bool, string>> AtualizaTransacaoENTRADA(EditarTransacaoVM vm);
        Task<IDictionary<bool, string>> AtualizaTransacaoSAIDA(EditarTransacaoVM vm);
        Task<EditarTransacaoVM> ConsultaTransacao(int CodTransacao, string TipoTransacao);
        Task<List<ListaTransacoesAConsolidarVm>> ListaTransacoesAConsolidar(FiltroTransacaoVM vm);

        Task<bool> VerificaParcelasFuturas(int CodTransacao, string TipoTransacao);

        Task<List<TesourariaExtratoBancarioVM>> ListaExtratoBancario(FiltroExtratoBancarioVm vm);
        Task<List<TesourariaMaioresDespesasVM>> ListaMaioresSAIDAs(FiltroExtratoBancarioVm vm);
        Task<List<TesourariaMaioresReceitasVM>> ListaMaioresENTRADAs(FiltroExtratoBancarioVm vm);
        Task<TesourariaInformacaoCCVM> BuscaSaldoContaCorrente(FiltroExtratoBancarioVm vm);

        Task<List<ListaConciliacaoVM>> ProcessaArquivoConciliacaoBancaria(FiltroConciliacaoBancariaVm model);

    }
}
