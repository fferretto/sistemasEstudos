using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_OcorrenciaRetPagService : IServiceBase<PAGNET_OCORRENCIARETPAG>
    {
        string ReturnOcorrencia(string id);
    }
}
