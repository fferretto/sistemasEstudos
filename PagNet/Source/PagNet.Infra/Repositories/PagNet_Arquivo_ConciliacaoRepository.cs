using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;


namespace PagNet.Infra.Data.Repositories
{
    public class PAGNET_ARQUIVO_DESCONTOFOLHARepository : RepositoryBase<PAGNET_ARQUIVO_DESCONTOFOLHA>, IPAGNET_ARQUIVO_DESCONTOFOLHARepository
    {
        public PAGNET_ARQUIVO_DESCONTOFOLHARepository(ContextNetCard context)
            : base(context)
        { }
    }
    public class PAGNET_FORMA_VERIFICACAO_DFRepository : RepositoryBase<PAGNET_FORMA_VERIFICACAO_DF>, IPAGNET_FORMA_VERIFICACAO_DFRepository
    {
        public PAGNET_FORMA_VERIFICACAO_DFRepository(ContextNetCard context)
            : base(context)
        { }
    }
    public class PAGNET_PARAM_ARQUIVO_DESCONTOFOLHARepository : RepositoryBase<PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA>, IPAGNET_PARAM_ARQUIVO_DESCONTOFOLHARepository
    {
        public PAGNET_PARAM_ARQUIVO_DESCONTOFOLHARepository(ContextNetCard context)
            : base(context)
        { }
    }
}

