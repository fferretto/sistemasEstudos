using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_InstrucaoCobrancaRepository : RepositoryBase<PAGNET_INSTRUCAOCOBRANCA>, IPagNet_InstrucaoCobrancaRepository
    {
        public PagNet_InstrucaoCobrancaRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

