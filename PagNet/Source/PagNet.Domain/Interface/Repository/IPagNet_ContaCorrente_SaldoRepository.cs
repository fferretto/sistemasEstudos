using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_ContaCorrente_SaldoRepository : IRepositoryBase<PAGNET_CONTACORRENTE_SALDO>
    {
        int GetMaxKey();
    }
}
