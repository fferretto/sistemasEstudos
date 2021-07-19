using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Bordero_BoletoRepository : RepositoryBase<PAGNET_BORDERO_BOLETO>, IPagNet_Bordero_BoletoRepository
    {
        public PagNet_Bordero_BoletoRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_BORDERO_BOLETO.Select(p => p.CODBORDERO).DefaultIfEmpty(0).Max();
        }
    }
}

