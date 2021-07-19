using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class SUBREDERepository : RepositoryBase<SUBREDE>, ISUBREDERepository
    {
        public SUBREDERepository(ContextPagNet context)
            : base(context)
        { }
    }
}

