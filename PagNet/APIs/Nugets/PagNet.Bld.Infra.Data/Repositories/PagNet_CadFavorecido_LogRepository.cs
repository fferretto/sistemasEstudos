using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CADFAVORECIDO_LOGRepository : RepositoryBase<PAGNET_CADFAVORECIDO_LOG>, IPAGNET_CADFAVORECIDO_LOGRepository
    {
        public PAGNET_CADFAVORECIDO_LOGRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADFAVORECIDO_LOG.Select(p => p.CODFAVORECIDO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

