using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_CadFavorecido_LogRepository : IRepositoryBase<PAGNET_CADFAVORECIDO_LOG>
    {
        int GetMaxKey();
    }
}
