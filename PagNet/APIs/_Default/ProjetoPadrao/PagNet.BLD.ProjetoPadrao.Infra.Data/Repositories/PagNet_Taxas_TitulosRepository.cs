using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_Taxas_TitulosRepository : RepositoryBase<PAGNET_TAXAS_TITULOS>, IPagNet_Taxas_TitulosRepository
    {
        public PagNet_Taxas_TitulosRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TAXAS_TITULOS.Select(p => p.CODTAXATITULO).DefaultIfEmpty(0).Max();
        }
    }
}
