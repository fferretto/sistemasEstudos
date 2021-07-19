using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
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
