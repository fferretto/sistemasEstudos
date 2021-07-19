using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_CadFavorecidoRepository : RepositoryBase<PAGNET_CADFAVORECIDO>, IPagNet_CadFavorecidoRepository
    {
        public PagNet_CadFavorecidoRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            var codigoRetorno = DbNetCard.PAGNET_CADFAVORECIDO.Select(p => p.CODFAVORECIDO).DefaultIfEmpty(0).Max();
            if (codigoRetorno < 1000000)
            {
                codigoRetorno = 1000000;
            }

            return codigoRetorno + 1;
        }
    }
}
