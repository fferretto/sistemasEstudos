using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_ContaCorrente_SaldoRepository : RepositoryBase<PAGNET_CONTACORRENTE_SALDO>, IPagNet_ContaCorrente_SaldoRepository
    {
        public PagNet_ContaCorrente_SaldoRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PagNet_ContaCorrente_Saldo.Select(p => p.CODSALDO).DefaultIfEmpty(0).Max();
        }
    }
}

