using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_OcorrenciaRetBolRepository : RepositoryBase<PAGNET_OCORRENCIARETBOL>, IPagNet_OcorrenciaRetBolRepository
    {
        public PagNet_OcorrenciaRetBolRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

