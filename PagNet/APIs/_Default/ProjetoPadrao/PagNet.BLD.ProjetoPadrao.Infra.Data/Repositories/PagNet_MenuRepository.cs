using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_MenuRepository : RepositoryBase<PAGNET_MENU>, IPagNet_MenuRepository
    {
        public PagNet_MenuRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

