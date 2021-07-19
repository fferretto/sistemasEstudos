using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using System.Linq;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
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
