using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_CADFAVORECIDO_CONFIGRepository : IRepositoryBase<PAGNET_CADFAVORECIDO_CONFIG>
    {
        int BuscaProximoID();
    }

}
