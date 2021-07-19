using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_ConfigVanRepository : IRepositoryBase<PAGNET_CONFIGVAN>
    {
        int GetMaxKey();
    }
}
