using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_CadEmpresaRepository : RepositoryBase<PAGNET_CADEMPRESA>, IPagNet_CadEmpresaRepository
    {
        public PagNet_CadEmpresaRepository(ContextNetCard context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_CADEMPRESA.Select(p => p.CODEMPRESA).DefaultIfEmpty(0).Max();
        }
    }
}

