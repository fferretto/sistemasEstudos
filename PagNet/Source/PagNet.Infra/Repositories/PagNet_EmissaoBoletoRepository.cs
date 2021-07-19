using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_EmissaoBoletoRepository : RepositoryBase<PAGNET_EMISSAOBOLETO>, IPagNet_EmissaoBoletoRepository
    {
        public PagNet_EmissaoBoletoRepository(ContextNetCard context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAOBOLETO.Select(p => p.codEmissaoBoleto).DefaultIfEmpty(0).Max();
        }
    }
}
