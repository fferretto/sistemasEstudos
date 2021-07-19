using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_CADEMPRESARepository : IRepositoryBase<PAGNET_CADEMPRESA>
    {
        int BuscaProximoID();
    }
}
