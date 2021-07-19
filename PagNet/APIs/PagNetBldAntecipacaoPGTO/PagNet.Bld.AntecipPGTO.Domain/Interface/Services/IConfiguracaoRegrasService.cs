using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Services.Common;
using System.Threading.Tasks;

namespace PagNet.Bld.AntecipPGTO.Domain.Interface.Services
{
    public interface IConfiguracaoRegrasService : IServiceBase<PAGNET_CONFIG_REGRA_PAG>
    {
        PAGNET_CONFIG_REGRA_PAG BuscaRegraPagByID(int codRegra);
        PAGNET_CONFIG_REGRA_PAG BuscaRegraAtivaPag();

    }
}
