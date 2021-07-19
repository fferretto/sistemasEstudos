using PagNet.Domain.Entities;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPAGNET_ARQUIVO_DESCONTOFOLHAService : IServiceBase<PAGNET_ARQUIVO_DESCONTOFOLHA>
    {
        Task<PAGNET_ARQUIVO_DESCONTOFOLHA> BuscaConfiguracaoByCodCliente(int codCliente);
    }
}
