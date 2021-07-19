using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_TITULOS_PAGOSRepository : IRepositoryBase<PAGNET_TITULOS_PAGOS>
    {
        int BuscaProximoID();
    }
}
