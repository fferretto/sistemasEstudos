using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_CadFavorecidoRepository : IRepositoryBase<PAGNET_CADFAVORECIDO>
    {
        int GetMaxKey();
    }
}
