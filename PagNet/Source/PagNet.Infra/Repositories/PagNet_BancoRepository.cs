using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_BancoRepository : RepositoryBase<PAGNET_BANCO>, IPagNet_BancoRepository
    {
        public PagNet_BancoRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

