using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_Emissao_TitulosRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS>, IPagNet_Emissao_TitulosRepository
    {
        public PagNet_Emissao_TitulosRepository(ContextNetCard context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAO_TITULOS.Select(p => p.CODTITULO).DefaultIfEmpty(0).Max();
        }
    }
}

