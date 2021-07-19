using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_USUARIO_CONCENTRADORRepository : RepositoryBase<PAGNET_USUARIO_CONCENTRADOR>, IPAGNET_USUARIO_CONCENTRADORRepository
    {
        public PAGNET_USUARIO_CONCENTRADORRepository(ContextConcentrador context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbConcentrador.PAGNET_USUARIO_CONCENTRADOR.Select(p => p.CODUSUARIO).DefaultIfEmpty(0).Max();
        }
    }
}
