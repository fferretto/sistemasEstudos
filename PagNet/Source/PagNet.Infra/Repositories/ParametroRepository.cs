using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Parametro_RelRepository : RepositoryBase<PAGNET_PARAMETRO_REL>, IPagNet_Parametro_RelRepository
    {
        public PagNet_Parametro_RelRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

