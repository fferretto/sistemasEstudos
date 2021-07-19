using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPAGNET_EMISSAOFATURAMENTORepository : IRepositoryBase<PAGNET_EMISSAOFATURAMENTO>
    {
        int GetMaxKey();
    }
    public interface IPAGNET_EMISSAOFATURAMENTO_LOGRepository : IRepositoryBase<PAGNET_EMISSAOFATURAMENTO_LOG>
    {
        int GetMaxKey();
    }
}
