using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CADFAVORECIDORepository : RepositoryBase<PAGNET_CADFAVORECIDO>, IPAGNET_CADFAVORECIDORepository
    {
        public PAGNET_CADFAVORECIDORepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            var codigoRetorno = DbNetCard.PAGNET_CADFAVORECIDO.Select(p => p.CODFAVORECIDO).DefaultIfEmpty(0).Max();
            if (codigoRetorno < 1000000)
            {
                codigoRetorno = 1000000;
            }

            return codigoRetorno + 1;
        }
    }
}
