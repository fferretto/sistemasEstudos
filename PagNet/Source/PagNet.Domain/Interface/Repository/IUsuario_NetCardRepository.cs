using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IUsuario_NetCardRepository : IRepositoryBase<USUARIO_NETCARD>
    {
        int GetMaxKey();
    }
}
