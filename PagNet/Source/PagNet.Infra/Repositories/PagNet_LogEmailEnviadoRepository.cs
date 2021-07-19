using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_LogEmailEnviadoRepository : RepositoryBase<PAGNET_LOGEMAILENVIADO>, IPagNet_LogEmailEnviadoRepository
    {
        public PagNet_LogEmailEnviadoRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_LOGEMAILENVIADO.Select(p => p.CODLOGEMAILENVIADO).DefaultIfEmpty(0).Max();
        }
    }
}

