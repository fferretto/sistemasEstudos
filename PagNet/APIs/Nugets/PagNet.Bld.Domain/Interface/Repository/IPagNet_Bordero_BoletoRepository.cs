using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_BORDERO_BOLETORepository : IRepositoryBase<PAGNET_BORDERO_BOLETO>
    {
        int BuscaProximoID();
    }
}
