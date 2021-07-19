using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Bordero_BoletoRepository : IRepositoryBase<PAGNET_BORDERO_BOLETO>
    {
        int GetMaxKey();
    }
}
