using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;


namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PAGNET_FORMAS_FATURAMENTORepository : RepositoryBase<PAGNET_FORMAS_FATURAMENTO>, IPAGNET_FORMAS_FATURAMENTORepository
    {
        public PAGNET_FORMAS_FATURAMENTORepository(ContextPagNet context)
            : base(context)
        { }       
    }
}

