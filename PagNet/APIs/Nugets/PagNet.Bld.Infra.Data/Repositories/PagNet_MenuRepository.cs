using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_MENURepository : RepositoryBase<PAGNET_MENU>, IPAGNET_MENURepository
    {
        public PAGNET_MENURepository(ContextPagNet context)
            : base(context)
        { }
    }
}

