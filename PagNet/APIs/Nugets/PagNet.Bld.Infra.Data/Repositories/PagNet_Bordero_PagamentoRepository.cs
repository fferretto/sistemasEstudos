using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_BORDERO_PAGAMENTORepository : RepositoryBase<PAGNET_BORDERO_PAGAMENTO>, IPAGNET_BORDERO_PAGAMENTORepository
    {
        public PAGNET_BORDERO_PAGAMENTORepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_BORDERO_PAGAMENTO.Select(p => p.CODBORDERO).DefaultIfEmpty(0).Max();
        }

    }
}

