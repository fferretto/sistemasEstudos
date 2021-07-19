using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Parametro_RelRepository : RepositoryBase<PAGNET_PARAMETRO_REL>, IPagNet_Parametro_RelRepository
    {
        public PagNet_Parametro_RelRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

