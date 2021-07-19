using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;


namespace PagNet.Infra.Data.Repositories
{
    public class PAGNET_FORMAS_FATURAMENTORepository : RepositoryBase<PAGNET_FORMAS_FATURAMENTO>, IPAGNET_FORMAS_FATURAMENTORepository
    {
        public PAGNET_FORMAS_FATURAMENTORepository(ContextNetCard context)
            : base(context)
        { }       
    }
}

