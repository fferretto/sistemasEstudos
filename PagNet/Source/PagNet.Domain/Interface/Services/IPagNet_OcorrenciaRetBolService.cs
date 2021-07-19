using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_OcorrenciaRetBolService : IServiceBase<PAGNET_OCORRENCIARETBOL>
    {
        string ReturnOcorrencia(string id);
    }
}
