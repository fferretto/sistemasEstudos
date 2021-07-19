using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_CodigoOcorrenciaService : IServiceBase<PAGNET_CODIGOOCORRENCIA>
    {
        object[][] GetTiposOcorrencias();
    }
}
