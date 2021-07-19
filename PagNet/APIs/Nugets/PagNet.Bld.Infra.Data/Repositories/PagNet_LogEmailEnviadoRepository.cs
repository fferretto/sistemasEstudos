using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_LOGEMAILENVIADORepository : RepositoryBase<PAGNET_LOGEMAILENVIADO>, IPAGNET_LOGEMAILENVIADORepository
    {
        public PAGNET_LOGEMAILENVIADORepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_LOGEMAILENVIADO.Select(p => p.CODLOGEMAILENVIADO).DefaultIfEmpty(0).Max();
        }
    }
}

