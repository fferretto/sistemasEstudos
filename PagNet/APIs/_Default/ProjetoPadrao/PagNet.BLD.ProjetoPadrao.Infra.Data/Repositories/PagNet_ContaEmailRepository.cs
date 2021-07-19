using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_ContaEmailRepository : RepositoryBase<PAGNET_CONTAEMAIL>, IPagNet_ContaEmailRepository
    {
        public PagNet_ContaEmailRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CONTAEMAIL.Select(p => p.CODCONTAEMAIL).DefaultIfEmpty(0).Max();
        }
    }
}

