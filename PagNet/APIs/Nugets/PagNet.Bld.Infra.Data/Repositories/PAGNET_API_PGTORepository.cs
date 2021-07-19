using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;
using PagNet.Bld.Domain.Interface.Repository;
using System.Linq;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_API_PGTORepository : RepositoryBase<PAGNET_API_PGTO>, IPAGNET_API_PGTORepository
    {
        public PAGNET_API_PGTORepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_API_PGTO.Select(p => p.CODIGOAPI).DefaultIfEmpty(0).Max();
        }
    }
}
