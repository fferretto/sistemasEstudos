using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;
using PagNet.Bld.Domain.Interface.Repository;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class NETCARD_USUARIOPOSRepository : RepositoryBase<NETCARD_USUARIOPOS>, INETCARD_USUARIOPOSRepository
    {
        public NETCARD_USUARIOPOSRepository(ContextPagNet context)
            : base(context)
        { }
    }
    public class NETCARD_USUARIOPRERepository : RepositoryBase<NETCARD_USUARIOPRE>, INETCARD_USUARIOPRERepository
    {
        public NETCARD_USUARIOPRERepository(ContextPagNet context)
            : base(context)
        { }
    }
}
