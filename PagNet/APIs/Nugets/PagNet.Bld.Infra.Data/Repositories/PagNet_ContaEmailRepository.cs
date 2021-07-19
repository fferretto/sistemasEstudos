using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CONTAEMAILRepository : RepositoryBase<PAGNET_CONTAEMAIL>, IPAGNET_CONTAEMAILRepository
    {
        public PAGNET_CONTAEMAILRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONTAEMAIL.Select(p => p.CODCONTAEMAIL).DefaultIfEmpty(0).Max();
        }
    }
}

