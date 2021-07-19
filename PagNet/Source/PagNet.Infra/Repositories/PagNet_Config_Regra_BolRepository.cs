using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_BolRepository : RepositoryBase<PAGNET_CONFIG_REGRA_BOL>, IPagNet_Config_Regra_BolRepository
    {
        public PagNet_Config_Regra_BolRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_BOL.Select(p => p.CODREGRA).DefaultIfEmpty(0).Max();
        }
    }
}

