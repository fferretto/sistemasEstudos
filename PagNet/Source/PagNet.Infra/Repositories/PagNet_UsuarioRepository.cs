using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_UsuarioRepository : RepositoryBase<PAGNET_USUARIO>, IPagNet_UsuarioRepository
    {
        public PagNet_UsuarioRepository(ContextConcentrador context)
            : base(context)
        { }
        protected override int GetInitialSeed()
        {
            return DbConcentrador.PAGNET_USUARIO.Select(p => p.CODUSUARIO).DefaultIfEmpty(0).Max();
        }
    }
}
