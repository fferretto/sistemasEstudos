using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_EMISSAO_TITULOSRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS>, IPAGNET_EMISSAO_TITULOSRepository
    {
        public PAGNET_EMISSAO_TITULOSRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAO_TITULOS.Select(p => p.CODTITULO).DefaultIfEmpty(0).Max();
        }
    }
}

