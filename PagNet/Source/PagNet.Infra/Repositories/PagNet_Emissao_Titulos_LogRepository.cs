using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Emissao_Titulos_LogRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>, IPagNet_Emissao_Titulos_LogRepository
    {
        public PagNet_Emissao_Titulos_LogRepository(ContextNetCard context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAO_TITULOS_LOG.Select(p => p.CODTITULO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}


