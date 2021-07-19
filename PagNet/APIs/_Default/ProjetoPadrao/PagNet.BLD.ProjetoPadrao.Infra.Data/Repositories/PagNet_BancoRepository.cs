using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_BancoRepository : RepositoryBase<PAGNET_BANCO>, IPagNet_BancoRepository
    {
        public PagNet_BancoRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

