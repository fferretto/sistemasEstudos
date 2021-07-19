using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Taxas_TitulosRepository : RepositoryBase<PAGNET_TAXAS_TITULOS>, IPagNet_Taxas_TitulosRepository
    {
        public PagNet_Taxas_TitulosRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_TAXAS_TITULOS.Select(p => p.CODTAXATITULO).DefaultIfEmpty(0).Max();
        }
    }
}
