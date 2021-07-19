using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CADPLANOCONTASRepository : RepositoryBase<PAGNET_CADPLANOCONTAS>, IPAGNET_CADPLANOCONTASRepository
    {
        public PAGNET_CADPLANOCONTASRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADPLANOCONTAS.Select(p => p.CODPLANOCONTAS).DefaultIfEmpty(0).Max();
        }
    }
}

