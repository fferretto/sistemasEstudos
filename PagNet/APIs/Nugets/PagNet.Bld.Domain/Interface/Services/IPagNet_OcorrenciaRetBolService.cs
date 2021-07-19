using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_OCORRENCIARETBOLService : IServiceBase<PAGNET_OCORRENCIARETBOL>
    {
        string ReturnOcorrencia(string id);
    }
}
