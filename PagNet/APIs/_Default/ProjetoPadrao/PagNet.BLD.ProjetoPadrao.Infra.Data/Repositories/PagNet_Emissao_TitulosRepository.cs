using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Emissao_TitulosRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS>, IPagNet_Emissao_TitulosRepository
    {
        public PagNet_Emissao_TitulosRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAO_TITULOS.Select(p => p.CODTITULO).DefaultIfEmpty(0).Max();
        }
    }
}

