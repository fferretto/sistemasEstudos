using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_BORDERO_BOLETORepository : RepositoryBase<PAGNET_BORDERO_BOLETO>, IPAGNET_BORDERO_BOLETORepository
    {
        public PAGNET_BORDERO_BOLETORepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_BORDERO_BOLETO.Select(p => p.CODBORDERO).DefaultIfEmpty(0).Max();
        }
    }
}

