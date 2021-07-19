using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_API_PGTORepository : IRepositoryBase<PAGNET_API_PGTO>
    {
        int BuscaProximoID();
    }
}
