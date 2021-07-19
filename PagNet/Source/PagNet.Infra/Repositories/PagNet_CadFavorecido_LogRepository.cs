using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_CadFavorecido_LogRepository : RepositoryBase<PAGNET_CADFAVORECIDO_LOG>, IPagNet_CadFavorecido_LogRepository
    {
        public PagNet_CadFavorecido_LogRepository(ContextNetCard context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADFAVORECIDO_LOG.Select(p => p.CODFAVORECIDO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

