using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface  IPagNet_UsuarioRepository : IRepositoryBase<PAGNET_USUARIO>
    {
        int GetMaxKey();
    }
}
