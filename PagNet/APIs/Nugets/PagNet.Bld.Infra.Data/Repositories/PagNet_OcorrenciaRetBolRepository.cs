using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_OCORRENCIARETBOLRepository : RepositoryBase<PAGNET_OCORRENCIARETBOL>, IPAGNET_OCORRENCIARETBOLRepository
    {
        public PAGNET_OCORRENCIARETBOLRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

