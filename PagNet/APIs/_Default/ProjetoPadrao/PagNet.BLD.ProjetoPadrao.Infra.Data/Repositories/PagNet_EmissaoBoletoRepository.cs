using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_EmissaoBoletoRepository : RepositoryBase<PAGNET_EMISSAOBOLETO>, IPagNet_EmissaoBoletoRepository
    {
        public PagNet_EmissaoBoletoRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOBOLETO.Select(p => p.codEmissaoBoleto).DefaultIfEmpty(0).Max();
        }
    }
}
