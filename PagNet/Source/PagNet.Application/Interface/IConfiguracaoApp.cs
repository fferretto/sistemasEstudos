using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IConfiguracaoApp
    {
        Task<IDictionary<bool, string>> SalvaRegraBoleto(RegraNegocioBoletoVm regra);
        Task<IDictionary<bool, string>> DesativaRegraBol(int codRegra, int codUsuario, string Justificativa);

        Task<RegraNegocioBoletoVm> BuscaRegraByID(int codRegra);
        Task<RegraNegocioBoletoVm> BuscaRegraAtiva(int codEmpresa);

        Task<IDictionary<bool, string>> SalvaRegraPagamento(RegraNegocioPagamentoVm regra);
        Task<IDictionary<bool, string>> DesativaRegraPagamento(int codRegra, int codUsuario, string Justificativa);

        Task<RegraNegocioPagamentoVm> BuscaRegraPagamentoByID(int codRegra);
        Task<RegraNegocioPagamentoVm> BuscaRegraAtivaPagamento();

        Task<FiltroPlanoContasVm> CarregaPlanosContas(int codEmpresa);
        Task<VisualizaListaPlanoContasVM> BuscaListaPlanosContas(int codEmpresa);

        Task<IDictionary<bool, string>> RemovePlanoContas(int codigoPlanoContas, int codigoEmpresa);
        Task<PlanoContasVm> CarregaPlanoContas(int codPlanoContas);
        Task<IDictionary<bool, string>> BuscaNovaClassificacaoPlanoContas(int CodigoPlanoContas, int codigoEmpresa);
        Task<IDictionary<bool, string>> SalvarNovoPlanoContas(PlanoContasVm model);
    }
}
