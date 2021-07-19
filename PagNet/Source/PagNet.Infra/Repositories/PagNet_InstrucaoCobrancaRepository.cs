using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_InstrucaoCobrancaRepository : RepositoryBase<PAGNET_INSTRUCAOCOBRANCA>, IPagNet_InstrucaoCobrancaRepository
    {
        public PagNet_InstrucaoCobrancaRepository(ContextNetCard context)
            : base(context)
        { }
    }
}

