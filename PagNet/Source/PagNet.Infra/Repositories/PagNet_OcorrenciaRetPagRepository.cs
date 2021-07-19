using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_OcorrenciaRetPagRepository : RepositoryBase<PAGNET_OCORRENCIARETPAG>, IPagNet_OcorrenciaRetPagRepository
    {
        public PagNet_OcorrenciaRetPagRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

