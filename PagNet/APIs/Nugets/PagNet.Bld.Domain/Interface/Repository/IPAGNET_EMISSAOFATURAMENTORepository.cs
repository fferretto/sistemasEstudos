using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_EMISSAOFATURAMENTORepository : IRepositoryBase<PAGNET_EMISSAOFATURAMENTO>
    {
        int BuscaProximoID();
    }
    public interface IPAGNET_EMISSAOFATURAMENTO_LOGRepository : IRepositoryBase<PAGNET_EMISSAOFATURAMENTO_LOG>
    {
        int BuscaProximoID();
    }
}
