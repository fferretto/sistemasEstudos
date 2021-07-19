using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_BANCOService : IServiceBase<PAGNET_BANCO>
    {
        object[][] GetBancoAtivos();
        Task<PAGNET_BANCO> getBancoByID(string codBanco);
    }
}
