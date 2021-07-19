using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_EspecieDocRepository : RepositoryBase<PAGNET_ESPECIEDOC>, IPagNet_EspecieDocRepository
    {
        public PagNet_EspecieDocRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

