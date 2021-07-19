using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Bordero_PagamentoRepository : RepositoryBase<PAGNET_BORDERO_PAGAMENTO>, IPagNet_Bordero_PagamentoRepository
    {
        public PagNet_Bordero_PagamentoRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_BORDERO_PAGAMENTO.Select(p => p.CODBORDERO).DefaultIfEmpty(0).Max();
        }

    }
}

