using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_LogEmailEnviadoRepository : IRepositoryBase<PAGNET_LOGEMAILENVIADO>
    {
        int GetMaxKey();
    }
}
