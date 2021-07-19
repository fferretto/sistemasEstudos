using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Infra.Data.Repositories.Common;
using PagNet.Bld.AntecipPGTO.Infra.Data.ContextDados;
using System.Linq;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.Repositories
{
    public class PagNet_Taxas_TitulosRepository : RepositoryBase<PAGNET_TAXAS_TITULOS>, IPagNet_Taxas_TitulosRepository
    {
        public PagNet_Taxas_TitulosRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbPagNet.PAGNET_TAXAS_TITULOS.Select(p => p.CODTAXATITULO).DefaultIfEmpty(0).Max();
        }
    }
}
