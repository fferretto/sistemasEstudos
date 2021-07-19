using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_CONFIG_REGRAService : ServiceBase<PAGNET_CONFIG_REGRA_PAG>, IPAGNET_CONFIG_REGRAService
    {
        private readonly IPAGNET_CONFIG_REGRA_PAGRepository _pag;
        private readonly IPAGNET_CONFIG_REGRA_PAG_LOGRepository _pagLog;

        public PAGNET_CONFIG_REGRAService(IPAGNET_CONFIG_REGRA_PAGRepository pag,
                                          IPAGNET_CONFIG_REGRA_PAG_LOGRepository pagLog)
            : base(pag)
        {
            _pag = pag;
            _pagLog = pagLog;
        }
        
        public void IncluiRegraPag(PAGNET_CONFIG_REGRA_PAG regra)
        {
            int id = _pag.BuscaProximoID();
            regra.CODREGRA = id;

            _pag.Add(regra);
        }

        public void IncluiLogRegraPag(PAGNET_CONFIG_REGRA_PAG regra, int codUsuario, string msgLog)
        {
            int id = _pagLog.BuscaProximoID();

            PAGNET_CONFIG_REGRA_PAG_LOG log = new PAGNET_CONFIG_REGRA_PAG_LOG();
            log.CODREGRA_LOG = id;
            log.CODREGRA = regra.CODREGRA;
            log.COBRATAXAANTECIPACAO = regra.COBRATAXAANTECIPACAO;
            log.PERCTAXAANTECIPACAO = regra.PERCTAXAANTECIPACAO;
            log.VLTAXAANTECIPACAO = regra.VLTAXAANTECIPACAO;
            log.FORMACOMPENSACAO = regra.FORMACOMPENSACAO;
            log.ATIVO = regra.ATIVO;
            log.CODUSUARIO = codUsuario;
            log.DATINCLOG = DateTime.Now;
            log.DESCLOG = msgLog;

            _pagLog.Add(log);
        }

        public void AtualizaRegraPag(PAGNET_CONFIG_REGRA_PAG regra)
        {
            _pag.Update(regra);
        }

        public void DesativaRegrPag(int codRegra)
        {
            var Regra = BuscaRegraPagByID(codRegra).Result;
            Regra.ATIVO = "N";

            AtualizaRegraPag(Regra);
        }

        public async Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraPagByID(int codRegra)
        {
            var regra = _pag.Get(x => x.CODREGRA == codRegra).FirstOrDefault();

            return regra;
        }

        public async Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraAtivaPag()
        {
            var regra = _pag.Get(x => x.ATIVO == "S").FirstOrDefault();

            return regra;
        }
    }
}

