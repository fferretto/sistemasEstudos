using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_ContaCorrenteRepository : RepositoryBase<PAGNET_CONTACORRENTE>, IPagNet_ContaCorrenteRepository
    {
        public PagNet_ContaCorrenteRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PagNet_ContaCorrente.Select(p => p.CODCONTACORRENTE).DefaultIfEmpty(0).Max();
        }
    }
}

