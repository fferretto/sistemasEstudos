using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_TITULOS_PAGOSRepository : RepositoryBase<PAGNET_TITULOS_PAGOS>, IPAGNET_TITULOS_PAGOSRepository
    {
        public PAGNET_TITULOS_PAGOSRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TITULOS_PAGOS.Select(p => p.CODTITULOPAGO).DefaultIfEmpty(0).Max();
        }
    }
}

