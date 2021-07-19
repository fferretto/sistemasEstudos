using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_ContaCorrenteRepository : RepositoryBase<PAGNET_CONTACORRENTE>, IPagNet_ContaCorrenteRepository
    {
        public PagNet_ContaCorrenteRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PagNet_ContaCorrente.Select(p => p.CODCONTACORRENTE).DefaultIfEmpty(0).Max();
        }
    }
}

