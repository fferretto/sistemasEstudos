using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Services;
using PagNet.Bld.AntecipPGTO.Domain.Services.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Bld.AntecipPGTO.Domain.Services
{
    public class ConfiguracaoRegrasService : ServiceBase<PAGNET_CONFIG_REGRA_PAG>, IConfiguracaoRegrasService
    {
        private readonly IPagNet_Config_Regra_PagRepository _pag;

        public ConfiguracaoRegrasService(IPagNet_Config_Regra_PagRepository pag)
            : base(pag)
        {
            _pag = pag;
        }

        public PAGNET_CONFIG_REGRA_PAG BuscaRegraAtivaPag()
        {
            var regra = _pag.Get(x => x.ATIVO == "S").FirstOrDefault();

            return regra;
        }

        public PAGNET_CONFIG_REGRA_PAG BuscaRegraPagByID(int codRegra)
        {
            var regra = _pag.Get(x => x.CODREGRA == codRegra).FirstOrDefault();

            return regra;
        }
    }
}
