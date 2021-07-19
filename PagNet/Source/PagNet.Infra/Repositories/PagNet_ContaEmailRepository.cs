using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_ContaEmailRepository : RepositoryBase<PAGNET_CONTAEMAIL>, IPagNet_ContaEmailRepository
    {
        public PagNet_ContaEmailRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONTAEMAIL.Select(p => p.CODCONTAEMAIL).DefaultIfEmpty(0).Max();
        }
    }
}

