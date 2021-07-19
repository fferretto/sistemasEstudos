using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_Arquivo_ConciliacaoService : ServiceBase<PAGNET_ARQUIVO_CONCILIACAO>, IPagNet_Arquivo_ConciliacaoService
    {
        private readonly IPagNet_Arquivo_ConciliacaoRepository _rep;

        public PagNet_Arquivo_ConciliacaoService(IPagNet_Arquivo_ConciliacaoRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public async Task<PAGNET_ARQUIVO_CONCILIACAO> BuscaConfiguracaoByCodCliente(int codCliente)
        {
            var DadosConfig = _rep.Get(x => x.CODCLIENTE == codCliente, "PAGNET_PARAM_ARQUIVO_CONCILIACAO,PAGNET_FORMA_VERIFICACAO_ARQUIVO").FirstOrDefault();

            return DadosConfig;
        }
    }
}
