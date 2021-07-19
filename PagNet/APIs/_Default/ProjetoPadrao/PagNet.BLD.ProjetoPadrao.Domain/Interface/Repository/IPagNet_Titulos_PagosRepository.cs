using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Titulos_PagosRepository : IRepositoryBase<PAGNET_TITULOS_PAGOS>
    {
        int GetMaxKey();
    }
}
