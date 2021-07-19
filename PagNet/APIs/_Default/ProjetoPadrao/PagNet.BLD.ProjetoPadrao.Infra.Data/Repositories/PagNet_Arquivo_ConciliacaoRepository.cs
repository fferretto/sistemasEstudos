using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Arquivo_ConciliacaoRepository : RepositoryBase<PAGNET_ARQUIVO_CONCILIACAO>, IPagNet_Arquivo_ConciliacaoRepository
    {
        public PagNet_Arquivo_ConciliacaoRepository(ContextPagNet context)
            : base(context)
        { }
    }
    public class PagNet_Forma_Verificacao_ArquivoRepository : RepositoryBase<PAGNET_FORMA_VERIFICACAO_ARQUIVO>, IPagNet_Forma_Verificacao_ArquivoRepository
    {
        public PagNet_Forma_Verificacao_ArquivoRepository(ContextPagNet context)
            : base(context)
        { }
    }
    public class PagNet_Param_Arquivo_ConciliacaoRepository : RepositoryBase<PAGNET_PARAM_ARQUIVO_CONCILIACAO>, IPagNet_Param_Arquivo_ConciliacaoRepository
    {
        public PagNet_Param_Arquivo_ConciliacaoRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

