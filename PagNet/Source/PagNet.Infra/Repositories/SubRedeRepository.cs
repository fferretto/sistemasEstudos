using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class SubRedeRepository : RepositoryBase<SUBREDE>, ISubRedeRepository
    {
        public SubRedeRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

