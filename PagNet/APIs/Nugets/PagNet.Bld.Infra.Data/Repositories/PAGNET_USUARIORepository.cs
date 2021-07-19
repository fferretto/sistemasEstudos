using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_USUARIORepository : RepositoryBase<PAGNET_USUARIO>, IPAGNET_USUARIORepository
    {
        public PAGNET_USUARIORepository(ContextPagNet context)
            : base(context)
        { }
        public override int BuscaProximoID()
        {
            var codigoRetorno = DbNetCard.PAGNET_USUARIO.Where(x => x.CODUSUARIO != 999)
                                                         .Select(p => p.CODUSUARIO)
                                                         .DefaultIfEmpty(0)
                                                         .Max();

            codigoRetorno += 1;

            if (codigoRetorno == 999) codigoRetorno += 1;


            return codigoRetorno;
        }
    }
}

