using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_BolRepository : RepositoryBase<PAGNET_CONFIG_REGRA_BOL>, IPagNet_Config_Regra_BolRepository
    {
        public PagNet_Config_Regra_BolRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_BOL.Select(p => p.CODREGRA).DefaultIfEmpty(0).Max();
        }
    }
}

