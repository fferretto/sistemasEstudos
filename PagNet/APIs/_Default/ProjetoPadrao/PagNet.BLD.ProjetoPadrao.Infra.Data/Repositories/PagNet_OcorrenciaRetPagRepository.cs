using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_OcorrenciaRetPagRepository : RepositoryBase<PAGNET_OCORRENCIARETPAG>, IPagNet_OcorrenciaRetPagRepository
    {
        public PagNet_OcorrenciaRetPagRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

