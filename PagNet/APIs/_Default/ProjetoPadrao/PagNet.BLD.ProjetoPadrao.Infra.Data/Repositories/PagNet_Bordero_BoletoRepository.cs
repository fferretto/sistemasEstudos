using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Bordero_BoletoRepository : RepositoryBase<PAGNET_BORDERO_BOLETO>, IPagNet_Bordero_BoletoRepository
    {
        public PagNet_Bordero_BoletoRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_BORDERO_BOLETO.Select(p => p.CODBORDERO).DefaultIfEmpty(0).Max();
        }
    }
}

