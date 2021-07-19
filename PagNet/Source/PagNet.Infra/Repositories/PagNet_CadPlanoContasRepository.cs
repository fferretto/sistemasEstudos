using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_CadPlanoContasRepository : RepositoryBase<PAGNET_CADPLANOCONTAS>, IPagNet_CadPlanoContasRepository
    {
        public PagNet_CadPlanoContasRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADPLANOCONTAS.Select(p => p.CODPLANOCONTAS).DefaultIfEmpty(0).Max();
        }
    }
}

