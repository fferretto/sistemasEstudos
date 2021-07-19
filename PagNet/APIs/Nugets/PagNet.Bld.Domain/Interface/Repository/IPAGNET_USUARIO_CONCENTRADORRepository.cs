using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_USUARIO_CONCENTRADORRepository : IRepositoryBase<PAGNET_USUARIO_CONCENTRADOR>
    {
        int BuscaProximoID();
    }
}
