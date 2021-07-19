using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CONTACORRENTERepository : RepositoryBase<PAGNET_CONTACORRENTE>, IPAGNET_CONTACORRENTERepository
    {
        public PAGNET_CONTACORRENTERepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PagNet_ContaCorrente.Select(p => p.CODCONTACORRENTE).DefaultIfEmpty(0).Max();
        }
    }
}

