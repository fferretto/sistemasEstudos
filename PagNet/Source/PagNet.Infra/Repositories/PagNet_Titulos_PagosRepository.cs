using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Titulos_PagosRepository : RepositoryBase<PAGNET_TITULOS_PAGOS>, IPagNet_Titulos_PagosRepository
    {
        public PagNet_Titulos_PagosRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TITULOS_PAGOS.Select(p => p.CODTITULOPAGO).DefaultIfEmpty(0).Max();
        }
    }
}

