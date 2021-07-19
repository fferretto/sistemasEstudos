using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_BORDERO_PAGAMENTORepository : IRepositoryBase<PAGNET_BORDERO_PAGAMENTO>
    {
        int BuscaProximoID();
    }
}
