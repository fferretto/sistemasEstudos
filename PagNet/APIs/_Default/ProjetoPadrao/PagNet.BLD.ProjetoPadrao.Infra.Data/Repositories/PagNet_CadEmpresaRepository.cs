using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class PagNet_CadEmpresaRepository : RepositoryBase<PAGNET_CADEMPRESA>, IPagNet_CadEmpresaRepository
    {
        public PagNet_CadEmpresaRepository(ContextPagNet context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADEMPRESA.Select(p => p.CODEMPRESA).DefaultIfEmpty(0).Max();
        }
    }
}

