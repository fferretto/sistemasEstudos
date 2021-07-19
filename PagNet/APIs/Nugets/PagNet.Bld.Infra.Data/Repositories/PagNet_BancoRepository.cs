using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_BANCORepository : RepositoryBase<PAGNET_BANCO>, IPAGNET_BANCORepository
    {
        public PAGNET_BANCORepository(ContextPagNet context)
            : base(context)
        { }
    }
}

