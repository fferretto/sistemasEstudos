using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Infra.Data.ContextDados;
using PagNet.Bld.AntecipPGTO.Infra.Data.Repositories.Common;
using System.Linq;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.Repositories
{
    public class PagNet_Emissao_TitulosRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS>, IPagNet_Emissao_TitulosRepository
    {
        public PagNet_Emissao_TitulosRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbPagNet.PAGNET_EMISSAO_TITULOS.Select(p => p.CODTITULO).DefaultIfEmpty(0).Max();
        }
    }
    public class PagNet_Emissao_Titulos_LogRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>, IPagNet_Emissao_Titulos_LogRepository
    {
        public PagNet_Emissao_Titulos_LogRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbPagNet.PAGNET_EMISSAO_TITULOS_LOG.Select(p => p.CODTITULO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}
