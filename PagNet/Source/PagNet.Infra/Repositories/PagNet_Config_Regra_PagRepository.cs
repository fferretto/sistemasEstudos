using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_PagRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG>, IPagNet_Config_Regra_PagRepository
    {
        public PagNet_Config_Regra_PagRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_PAG.Select(p => p.CODREGRA).DefaultIfEmpty(0).Max();
        }
    }
    public class PagNet_Config_Regra_Pag_LogRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG_LOG>, IPagNet_Config_Regra_Pag_LogRepository
    {
        public PagNet_Config_Regra_Pag_LogRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_PAG_LOG.Select(p => p.CODREGRA_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

