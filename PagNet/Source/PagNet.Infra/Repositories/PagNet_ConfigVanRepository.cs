using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_ConfigVanRepository : RepositoryBase<PAGNET_CONFIGVAN>, IPagNet_ConfigVanRepository
    {
        public PagNet_ConfigVanRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONFIGVAN.Select(p => p.CODCONFIGVAN).DefaultIfEmpty(0).Max();
        }
    }
}

