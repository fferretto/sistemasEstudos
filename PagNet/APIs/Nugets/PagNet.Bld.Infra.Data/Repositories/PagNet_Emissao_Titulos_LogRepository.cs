using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_EMISSAO_TITULOS_LOGRepository : RepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>, IPAGNET_EMISSAO_TITULOS_LOGRepository
    {
        public PAGNET_EMISSAO_TITULOS_LOGRepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_EMISSAO_TITULOS_LOG.Select(p => p.CODTITULO_LOG).DefaultIfEmpty(0).Max();
        }
    }
}


