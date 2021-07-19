using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Bordero_PagamentoRepository : IRepositoryBase<PAGNET_BORDERO_PAGAMENTO>
    {
        int GetMaxKey();
    }
}
