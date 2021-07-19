using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_MenuRepository : RepositoryBase<PAGNET_MENU>, IPagNet_MenuRepository
    {
        public PagNet_MenuRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

