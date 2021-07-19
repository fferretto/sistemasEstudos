using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_ContaCorrente_SaldoRepository : IRepositoryBase<PAGNET_CONTACORRENTE_SALDO>
    {
        int GetMaxKey();
    }
}
