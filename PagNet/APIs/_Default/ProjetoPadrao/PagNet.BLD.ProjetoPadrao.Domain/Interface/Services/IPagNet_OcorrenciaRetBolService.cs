using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_OcorrenciaRetBolService : IServiceBase<PAGNET_OCORRENCIARETBOL>
    {
        string ReturnOcorrencia(string id);
    }
}
