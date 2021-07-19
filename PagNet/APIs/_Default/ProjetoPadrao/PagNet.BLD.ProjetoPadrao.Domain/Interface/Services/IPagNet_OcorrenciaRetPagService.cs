using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_OcorrenciaRetPagService : IServiceBase<PAGNET_OCORRENCIARETPAG>
    {
        string ReturnOcorrencia(string id);
    }
}
