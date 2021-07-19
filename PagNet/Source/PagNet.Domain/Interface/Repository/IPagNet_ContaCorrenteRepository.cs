using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_ContaCorrenteRepository : IRepositoryBase<PAGNET_CONTACORRENTE>
    {
        int GetMaxKey();
    }
}
