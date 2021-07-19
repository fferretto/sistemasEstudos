using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_TAXAS_TITULOSRepository : IRepositoryBase<PAGNET_TAXAS_TITULOS>
    {
        int BuscaProximoID();
    }
}
