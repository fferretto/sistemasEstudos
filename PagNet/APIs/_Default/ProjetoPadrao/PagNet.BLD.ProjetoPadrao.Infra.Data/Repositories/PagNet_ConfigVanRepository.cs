using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_ConfigVanRepository : RepositoryBase<PAGNET_CONFIGVAN>, IPagNet_ConfigVanRepository
    {
        public PagNet_ConfigVanRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIGVAN.Select(p => p.CODCONFIGVAN).DefaultIfEmpty(0).Max();
        }
    }
}

