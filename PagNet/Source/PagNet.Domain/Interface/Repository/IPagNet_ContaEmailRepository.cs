using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_ContaEmailRepository : IRepositoryBase<PAGNET_CONTAEMAIL>
    {
        int GetMaxKey();
    }
}
