using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_FORMAS_FATURAMENTORepository : RepositoryBase<PAGNET_FORMAS_FATURAMENTO>, IPAGNET_FORMAS_FATURAMENTORepository
    {
        public PAGNET_FORMAS_FATURAMENTORepository(ContextPagNet context)
            : base(context)
        { }       
    }
}

