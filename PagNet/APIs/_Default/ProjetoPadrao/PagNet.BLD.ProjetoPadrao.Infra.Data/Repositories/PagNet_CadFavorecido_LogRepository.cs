using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_CadFavorecido_LogRepository : RepositoryBase<PAGNET_CADFAVORECIDO_LOG>, IPagNet_CadFavorecido_LogRepository
    {
        public PagNet_CadFavorecido_LogRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADFAVORECIDO_LOG.Select(p => p.CODFAVORECIDO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

