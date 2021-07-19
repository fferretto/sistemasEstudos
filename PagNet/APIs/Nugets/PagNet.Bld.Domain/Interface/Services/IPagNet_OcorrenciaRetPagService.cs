using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_OCORRENCIARETPAGService : IServiceBase<PAGNET_OCORRENCIARETPAG>
    {
        string ReturnOcorrencia(string id);
    }
}
