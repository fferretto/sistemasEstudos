using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CONFIG_REGRA_PAGRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG>, IPAGNET_CONFIG_REGRA_PAGRepository
    {
        public PAGNET_CONFIG_REGRA_PAGRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_PAG.Select(p => p.CODREGRA).DefaultIfEmpty(0).Max();
        }
    }
    public class PAGNET_CONFIG_REGRA_PAG_LOGRepository : RepositoryBase<PAGNET_CONFIG_REGRA_PAG_LOG>, IPAGNET_CONFIG_REGRA_PAG_LOGRepository
    {
        public PAGNET_CONFIG_REGRA_PAG_LOGRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIG_REGRA_PAG_LOG.Select(p => p.CODREGRA_LOG).DefaultIfEmpty(0).Max();
        }
    }
}

