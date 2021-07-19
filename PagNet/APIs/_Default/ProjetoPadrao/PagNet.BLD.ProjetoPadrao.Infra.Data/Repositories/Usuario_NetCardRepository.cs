using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Common;
using System.Linq;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories
{
    public class Usuario_NetCardRepository : RepositoryBase<USUARIO_NETCARD>, IUsuario_NetCardRepository
    {
        public Usuario_NetCardRepository(ContextPagNet context)
            : base(context)
        { }
        public override int GetMaxKey()
        {
            var codigoRetorno = DbNetCard.Usuario_NetCard.Where(x => x.CODUSUARIO != 999)
                                                         .Select(p => p.CODUSUARIO)
                                                         .DefaultIfEmpty(0)
                                                         .Max();

            codigoRetorno += 1;

            if (codigoRetorno == 999) codigoRetorno += 1;


            return codigoRetorno;
        }
    }
}

