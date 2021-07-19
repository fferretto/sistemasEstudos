using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_EMISSAOFATURAMENTORepository : RepositoryBase<PAGNET_EMISSAOFATURAMENTO>, IPAGNET_EMISSAOFATURAMENTORepository
    {
        public PAGNET_EMISSAOFATURAMENTORepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOFATURAMENTO.Select(p => p.CODEMISSAOFATURAMENTO).DefaultIfEmpty(0).Max();
        }
    }

    public class PAGNET_EMISSAOFATURAMENTO_LOGRepository : RepositoryBase<PAGNET_EMISSAOFATURAMENTO_LOG>, IPAGNET_EMISSAOFATURAMENTO_LOGRepository
    {
        public PAGNET_EMISSAOFATURAMENTO_LOGRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOFATURAMENTO_LOG.Select(p => p.CODEMISSAOFATURAMENTO_LOG).DefaultIfEmpty(0).Max();
        }
    }    
}
