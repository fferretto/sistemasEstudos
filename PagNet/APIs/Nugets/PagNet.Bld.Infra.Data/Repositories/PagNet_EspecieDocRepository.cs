using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_ESPECIEDOCRepository : RepositoryBase<PAGNET_ESPECIEDOC>, IPAGNET_ESPECIEDOCRepository
    {
        public PAGNET_ESPECIEDOCRepository(ContextPagNet context)
            : base(context)
        { }
    }
}

