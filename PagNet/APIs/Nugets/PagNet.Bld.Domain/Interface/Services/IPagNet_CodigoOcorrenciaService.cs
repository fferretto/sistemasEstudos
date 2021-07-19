using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_CODIGOOCORRENCIAService : IServiceBase<PAGNET_CODIGOOCORRENCIA>
    {
        object[][] GetTiposOcorrencias();
    }
}
