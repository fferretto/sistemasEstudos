using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IUsuario_NetCardRepository : IRepositoryBase<USUARIO_NETCARD>
    {
        int GetMaxKey();
    }
}
