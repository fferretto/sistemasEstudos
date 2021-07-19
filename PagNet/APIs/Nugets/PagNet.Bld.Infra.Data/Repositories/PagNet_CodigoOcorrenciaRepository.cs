using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_CODIGOOCORRENCIARepository : RepositoryBase<PAGNET_CODIGOOCORRENCIA>, IPAGNET_CODIGOOCORRENCIARepository
    {
        public PAGNET_CODIGOOCORRENCIARepository(ContextPagNet context)
            : base(context)
        { }
    }
}

