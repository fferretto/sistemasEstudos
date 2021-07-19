using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_CADFAVORECIDORepository : IRepositoryBase<PAGNET_CADFAVORECIDO>
    {
        int BuscaProximoID();
    }

    public interface IPAGNET_CADFAVORECIDO_LOGRepository : IRepositoryBase<PAGNET_CADFAVORECIDO_LOG>
    {
        int BuscaProximoID();
    }
}
