using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Titulos_PagosRepository : RepositoryBase<PAGNET_TITULOS_PAGOS>, IPagNet_Titulos_PagosRepository
    {
        public PagNet_Titulos_PagosRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TITULOS_PAGOS.Select(p => p.CODTITULOPAGO).DefaultIfEmpty(0).Max();
        }
    }
}

