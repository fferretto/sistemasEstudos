using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PAGNET_EMISSAOFATURAMENTORepository : RepositoryBase<PAGNET_EMISSAOFATURAMENTO>, IPAGNET_EMISSAOFATURAMENTORepository
    {
        public PAGNET_EMISSAOFATURAMENTORepository(ContextNetCard context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOFATURAMENTO.Select(p => p.CODEMISSAOFATURAMENTO).DefaultIfEmpty(0).Max();
        }
    }

    public class PAGNET_EMISSAOFATURAMENTO_LOGRepository : RepositoryBase<PAGNET_EMISSAOFATURAMENTO_LOG>, IPAGNET_EMISSAOFATURAMENTO_LOGRepository
    {
        public PAGNET_EMISSAOFATURAMENTO_LOGRepository(ContextNetCard context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOFATURAMENTO_LOG.Select(p => p.CODEMISSAOFATURAMENTO_LOG).DefaultIfEmpty(0).Max();
        }
    }    
}
