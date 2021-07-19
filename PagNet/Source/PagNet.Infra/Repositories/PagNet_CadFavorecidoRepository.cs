using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_CadFavorecidoRepository : RepositoryBase<PAGNET_CADFAVORECIDO>, IPagNet_CadFavorecidoRepository
    {
        public PagNet_CadFavorecidoRepository(ContextNetCard context)
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
