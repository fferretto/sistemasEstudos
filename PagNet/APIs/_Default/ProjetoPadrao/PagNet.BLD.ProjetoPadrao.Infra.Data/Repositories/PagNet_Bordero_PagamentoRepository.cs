using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Bordero_PagamentoRepository : RepositoryBase<PAGNET_BORDERO_PAGAMENTO>, IPagNet_Bordero_PagamentoRepository
    {
        public PagNet_Bordero_PagamentoRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_BORDERO_PAGAMENTO.Select(p => p.CODBORDERO).DefaultIfEmpty(0).Max();
        }

    }
}

