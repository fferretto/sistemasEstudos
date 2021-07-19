using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Titulos_PagosRepository : IRepositoryBase<PAGNET_TITULOS_PAGOS>
    {
        int GetMaxKey();
    }
}
