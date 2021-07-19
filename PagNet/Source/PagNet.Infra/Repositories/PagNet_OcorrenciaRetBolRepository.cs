using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_OcorrenciaRetBolRepository : RepositoryBase<PAGNET_OCORRENCIARETBOL>, IPagNet_OcorrenciaRetBolRepository
    {
        public PagNet_OcorrenciaRetBolRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

