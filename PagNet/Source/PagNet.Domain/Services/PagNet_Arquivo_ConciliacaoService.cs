using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Domain.Services
{
    public class PAGNET_ARQUIVO_DESCONTOFOLHAService : ServiceBase<PAGNET_ARQUIVO_DESCONTOFOLHA>, IPAGNET_ARQUIVO_DESCONTOFOLHAService
    {
        private readonly IPAGNET_ARQUIVO_DESCONTOFOLHARepository _rep;

        public PAGNET_ARQUIVO_DESCONTOFOLHAService(IPAGNET_ARQUIVO_DESCONTOFOLHARepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public async Task<PAGNET_ARQUIVO_DESCONTOFOLHA> BuscaConfiguracaoByCodCliente(int codCliente)
        {
            var DadosConfig = _rep.Get(x => x.CODCLIENTE == codCliente, "PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA,PAGNET_FORMA_VERIFICACAO_DF").FirstOrDefault();

            return DadosConfig;
        }
    }
}
