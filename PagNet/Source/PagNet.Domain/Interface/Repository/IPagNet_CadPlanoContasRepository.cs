using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_CadPlanoContasRepository : IRepositoryBase<PAGNET_CADPLANOCONTAS>
    {
        int GetMaxKey();
    }
}
