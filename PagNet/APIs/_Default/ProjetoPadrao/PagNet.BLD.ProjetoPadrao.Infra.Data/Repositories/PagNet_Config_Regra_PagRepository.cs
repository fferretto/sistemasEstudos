using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_PagRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG>, IPagNet_Config_Regra_PagRepository
    {
        public PagNet_Config_Regra_PagRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_PAG.Select(p => p.CODREGRA).DefaultIfEmpty(0).Max();
        }
    }
    public class PagNet_Config_Regra_Pag_LogRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG_LOG>, IPagNet_Config_Regra_Pag_LogRepository
    {
        public PagNet_Config_Regra_Pag_LogRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_PAG_LOG.Select(p => p.CODREGRA_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

