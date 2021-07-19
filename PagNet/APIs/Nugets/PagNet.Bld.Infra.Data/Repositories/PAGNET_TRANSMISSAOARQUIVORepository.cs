using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_TRANSMISSAOARQUIVORepository : RepositoryBase<PAGNET_TRANSMISSAOARQUIVO>, IPAGNET_TRANSMISSAOARQUIVORepository
    {
        public PAGNET_TRANSMISSAOARQUIVORepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TRANSMISSAOARQUIVO.Select(p => p.CODTRANSMISSAOARQUIVO).DefaultIfEmpty(0).Max();
        }
    }
}

