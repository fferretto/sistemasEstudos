using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_EMISSAOBOLETORepository : RepositoryBase<PAGNET_EMISSAOBOLETO>, IPAGNET_EMISSAOBOLETORepository
    {
        public PAGNET_EMISSAOBOLETORepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOBOLETO.Select(p => p.codEmissaoBoleto).DefaultIfEmpty(0).Max();
        }
    }
}
