using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Infra.Data.Repositories.Common;
using PagNet.Bld.AntecipPGTO.Infra.Data.ContextDados;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.Repositories
{
    public class PagNet_CadFavorecidoRepository : RepositoryBase<PAGNET_CADFAVORECIDO>, IPagNet_CadFavorecidoRepository
    {
        public PagNet_CadFavorecidoRepository(ContextPagNet context)
            : base(context)
        { }

    }
}
