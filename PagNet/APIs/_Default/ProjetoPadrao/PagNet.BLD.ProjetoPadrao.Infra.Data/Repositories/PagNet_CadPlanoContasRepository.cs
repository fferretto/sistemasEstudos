using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_CadPlanoContasRepository : RepositoryBase<PAGNET_CADPLANOCONTAS>, IPagNet_CadPlanoContasRepository
    {
        public PagNet_CadPlanoContasRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADPLANOCONTAS.Select(p => p.CODPLANOCONTAS).DefaultIfEmpty(0).Max();
        }
    }
}

