using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_Bol_LogRepository : RepositoryBase<PAGNET_CONFIG_REGRA_BOL_LOG>, IPagNet_Config_Regra_Bol_LogRepository
    {
        public PagNet_Config_Regra_Bol_LogRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_BOL_LOG.Select(p => p.CODREGRA_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

