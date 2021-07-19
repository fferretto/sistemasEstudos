using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_CodigoOcorrenciaService : IServiceBase<PAGNET_CODIGOOCORRENCIA>
    {
        object[][] GetTiposOcorrencias();
    }
}
