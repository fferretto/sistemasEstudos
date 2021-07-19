using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Emissao_Titulos_LogRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>, IPagNet_Emissao_Titulos_LogRepository
    {
        public PagNet_Emissao_Titulos_LogRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAO_TITULOS_LOG.Select(p => p.CODTITULO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}


