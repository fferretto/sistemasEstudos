using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_CodigoOcorrenciaRepository : RepositoryBase<PAGNET_CODIGOOCORRENCIA>, IPagNet_CodigoOcorrenciaRepository
    {
        public PagNet_CodigoOcorrenciaRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

