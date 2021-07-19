using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_BancoService : IServiceBase<PAGNET_BANCO>
    {
        object[][] GetBancoAtivos();
        Task<PAGNET_BANCO> getBancoByID(string codBanco);
    }
}
