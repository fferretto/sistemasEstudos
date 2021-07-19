using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_CADCLIENTERepository : IRepositoryBase<PAGNET_CADCLIENTE>
    {
        int BuscaProximoID();
    }

    public interface IPAGNET_CADCLIENTE_LOGRepository : IRepositoryBase<PAGNET_CADCLIENTE_LOG>
    {
        int BuscaProximoID();
    }
}
