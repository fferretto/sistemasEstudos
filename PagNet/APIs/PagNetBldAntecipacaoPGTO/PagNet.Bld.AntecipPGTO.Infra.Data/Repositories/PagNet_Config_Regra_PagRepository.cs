using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Infra.Data.Repositories.Common;
using PagNet.Bld.AntecipPGTO.Infra.Data.ContextDados;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.Repositories
{
    public class PagNet_Config_Regra_PagRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG>, IPagNet_Config_Regra_PagRepository
    {
        public PagNet_Config_Regra_PagRepository(ContextPagNet context)
            : base(context)
        { }
    }
}
