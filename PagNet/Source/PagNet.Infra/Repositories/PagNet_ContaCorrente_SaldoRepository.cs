using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_ContaCorrente_SaldoRepository : RepositoryBase<PAGNET_CONTACORRENTE_SALDO>, IPagNet_ContaCorrente_SaldoRepository
    {
        public PagNet_ContaCorrente_SaldoRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PagNet_ContaCorrente_Saldo.Select(p => p.CODSALDO).DefaultIfEmpty(0).Max();
        }
    }
}

