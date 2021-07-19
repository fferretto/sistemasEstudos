using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Bordero_PagamentoRepository : IRepositoryBase<PAGNET_BORDERO_PAGAMENTO>
    {
        int GetMaxKey();
    }
}
