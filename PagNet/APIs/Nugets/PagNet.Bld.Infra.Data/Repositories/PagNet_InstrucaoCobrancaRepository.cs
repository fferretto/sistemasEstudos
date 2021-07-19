using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_INSTRUCAOCOBRANCARepository : RepositoryBase<PAGNET_INSTRUCAOCOBRANCA>, IPAGNET_INSTRUCAOCOBRANCARepository
    {
        public PAGNET_INSTRUCAOCOBRANCARepository(ContextPagNet context)
            : base(context)
        { }
    }
}

