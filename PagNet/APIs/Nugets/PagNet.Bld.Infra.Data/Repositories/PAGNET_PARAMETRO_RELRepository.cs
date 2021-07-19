using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_PARAMETRO_RELRepository : RepositoryBase<PAGNET_PARAMETRO_REL>, IPAGNET_PARAMETRO_RELRepository
    {
        public PAGNET_PARAMETRO_RELRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

