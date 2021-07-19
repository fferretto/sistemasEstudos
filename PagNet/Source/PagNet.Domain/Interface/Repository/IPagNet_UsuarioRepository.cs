using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface  IPagNet_UsuarioRepository : IRepositoryBase<PAGNET_USUARIO>
    {
        int GetMaxKey();
    }
}
