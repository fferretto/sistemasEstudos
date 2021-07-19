using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_Bol_LogRepository : RepositoryBase<PAGNET_CONFIG_REGRA_BOL_LOG>, IPagNet_Config_Regra_Bol_LogRepository
    {
        public PagNet_Config_Regra_Bol_LogRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_BOL_LOG.Select(p => p.CODREGRA_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

