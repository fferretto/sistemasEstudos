using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CONFIG_REGRAService : IServiceBase<PAGNET_CONFIG_REGRA_PAG>
    {
        //----------------------CONFIGURAÇÃO DE REGRAS DE PAGAMENTO---------------------------------

        void IncluiRegraPag(PAGNET_CONFIG_REGRA_PAG regra);
        void IncluiLogRegraPag(PAGNET_CONFIG_REGRA_PAG regra, int codUsuario, string msgLog);
        void AtualizaRegraPag(PAGNET_CONFIG_REGRA_PAG regra);
        void DesativaRegrPag(int codRegra);

        Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraPagByID(int codRegra);
        Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraAtivaPag();
    }
}
