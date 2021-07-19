using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CADEMPRESARepository : RepositoryBase<PAGNET_CADEMPRESA>, IPAGNET_CADEMPRESARepository
    {
        public PAGNET_CADEMPRESARepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADEMPRESA.Select(p => p.CODEMPRESA).DefaultIfEmpty(0).Max();
        }
    }
}

