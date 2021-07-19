using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_Arquivo_ConciliacaoService : IServiceBase<PAGNET_ARQUIVO_CONCILIACAO>
    {
        Task<PAGNET_ARQUIVO_CONCILIACAO> BuscaConfiguracaoByCodCliente(int codCliente);
    }
}
