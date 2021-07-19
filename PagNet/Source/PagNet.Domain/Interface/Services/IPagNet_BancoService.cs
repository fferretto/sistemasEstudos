using PagNet.Domain.Entities;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_BancoService : IServiceBase<PAGNET_BANCO>
    {
        object[][] GetBancoAtivos();
        Task<PAGNET_BANCO> getBancoByID(string codBanco);
    }
}
