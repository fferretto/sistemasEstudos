using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_CadPlanoContasRepository : IRepositoryBase<PAGNET_CADPLANOCONTAS>
    {
        int GetMaxKey();
    }
}
