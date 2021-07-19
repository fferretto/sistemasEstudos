using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_EspecieDocRepository : RepositoryBase<PAGNET_ESPECIEDOC>, IPagNet_EspecieDocRepository
    {
        public PagNet_EspecieDocRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

