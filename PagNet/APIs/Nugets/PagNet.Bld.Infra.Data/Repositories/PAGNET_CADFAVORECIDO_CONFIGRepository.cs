using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;
namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CADFAVORECIDO_CONFIGRepository : RepositoryBase<PAGNET_CADFAVORECIDO_CONFIG>, IPAGNET_CADFAVORECIDO_CONFIGRepository
    {
        public PAGNET_CADFAVORECIDO_CONFIGRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADFAVORECIDO_CONFIG.Select(p => p.CODFAVORECIDOCONFIG).DefaultIfEmpty(0).Max();
        }
    }
}

