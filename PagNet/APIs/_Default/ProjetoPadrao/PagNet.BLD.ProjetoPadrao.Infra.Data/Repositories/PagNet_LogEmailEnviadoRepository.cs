using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_LogEmailEnviadoRepository : RepositoryBase<PAGNET_LOGEMAILENVIADO>, IPagNet_LogEmailEnviadoRepository
    {
        public PagNet_LogEmailEnviadoRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_LOGEMAILENVIADO.Select(p => p.CODLOGEMAILENVIADO).DefaultIfEmpty(0).Max();
        }
    }
}

