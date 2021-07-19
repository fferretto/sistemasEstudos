using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_CodigoOcorrenciaRepository : RepositoryBase<PAGNET_CODIGOOCORRENCIA>, IPagNet_CodigoOcorrenciaRepository
    {
        public PagNet_CodigoOcorrenciaRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

