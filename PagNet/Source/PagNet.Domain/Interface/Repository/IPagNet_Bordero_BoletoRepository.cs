using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Bordero_BoletoRepository : IRepositoryBase<PAGNET_BORDERO_BOLETO>
    {
        int GetMaxKey();
    }
}
