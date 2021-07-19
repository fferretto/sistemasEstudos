using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_LogEmailEnviadoRepository : IRepositoryBase<PAGNET_LOGEMAILENVIADO>
    {
        int GetMaxKey();
    }
}
