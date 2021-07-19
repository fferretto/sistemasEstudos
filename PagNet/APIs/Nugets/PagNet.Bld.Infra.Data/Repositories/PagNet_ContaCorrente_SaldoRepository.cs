using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CONTACORRENTE_SALDORepository : RepositoryBase<PAGNET_CONTACORRENTE_SALDO>, IPAGNET_CONTACORRENTE_SALDORepository
    {
        public PAGNET_CONTACORRENTE_SALDORepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PagNet_ContaCorrente_Saldo.Select(p => p.CODSALDO).DefaultIfEmpty(0).Max();
        }
    }
}

