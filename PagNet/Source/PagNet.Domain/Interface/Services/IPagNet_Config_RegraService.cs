using PagNet.Domain.Entities;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_Config_RegraService : IServiceBase<PAGNET_CONFIG_REGRA_BOL>
    {
        //----------------------CONFIGURAÇÃO DE REGRAS DE BOLETO---------------------------------

        void IncluiRegraBol(PAGNET_CONFIG_REGRA_BOL regra);
        void IncluiLogRegraBol(PAGNET_CONFIG_REGRA_BOL regra, int codUsuario, string msgLog);
        void AtualizaRegraBol(PAGNET_CONFIG_REGRA_BOL regra);
        void DesativaRegraBol(int codRegra);

        Task<PAGNET_CONFIG_REGRA_BOL> BuscaRegraBolByID(int codRegra);
        Task<PAGNET_CONFIG_REGRA_BOL> BuscaRegraAtivaBol(int codEmpresa);


        //----------------------CONFIGURAÇÃO DE REGRAS DE PAGAMENTO---------------------------------

        void IncluiRegraPag(PAGNET_CONFIG_REGRA_PAG regra);
        void IncluiLogRegraPag(PAGNET_CONFIG_REGRA_PAG regra, int codUsuario, string msgLog);
        void AtualizaRegraPag(PAGNET_CONFIG_REGRA_PAG regra);
        void DesativaRegrPag(int codRegra);

        Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraPagByID(int codRegra);
        Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraAtivaPag();
    }
}
