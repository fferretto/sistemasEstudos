using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_OCORRENCIARETPAGRepository : RepositoryBase<PAGNET_OCORRENCIARETPAG>, IPAGNET_OCORRENCIARETPAGRepository
    {
        public PAGNET_OCORRENCIARETPAGRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

