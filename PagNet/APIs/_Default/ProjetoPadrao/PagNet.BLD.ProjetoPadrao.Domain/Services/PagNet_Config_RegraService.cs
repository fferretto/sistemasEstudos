using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_Config_RegraService : ServiceBase<PAGNET_CONFIG_REGRA_BOL>, IPagNet_Config_RegraService
    {
        private readonly IPagNet_Config_Regra_BolRepository _rep;
        private readonly IPagNet_Config_Regra_Bol_LogRepository _log;
        private readonly IPagNet_Config_Regra_PagRepository _pag;
        private readonly IPagNet_Config_Regra_Pag_LogRepository _pagLog;

        public PagNet_Config_RegraService(IPagNet_Config_Regra_BolRepository rep,
                                          IPagNet_Config_Regra_PagRepository pag,
                                          IPagNet_Config_Regra_Pag_LogRepository pagLog,
                                          IPagNet_Config_Regra_Bol_LogRepository log)
            : base(rep)
        {
            _rep = rep;
            _log = log;
            _pag = pag;
            _pagLog = pagLog;
        }

        public void AtualizaRegraBol(PAGNET_CONFIG_REGRA_BOL regra)
        {
            _rep.Update(regra);
        }

        public void DesativaRegraBol(int codRegra)
        {
            var Regra = BuscaRegraBolByID(codRegra).Result;
            Regra.ATIVO = "N";

            AtualizaRegraBol(Regra);
        }

        public async Task<PAGNET_CONFIG_REGRA_BOL> BuscaRegraBolByID(int codRegra)
        {
            var regra = _rep.Get(x => x.CODREGRA == codRegra).FirstOrDefault();

            return regra;
        }

        public async Task<PAGNET_CONFIG_REGRA_BOL> BuscaRegraAtivaBol(int codEmpresa)
        {
            var regra = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.ATIVO == "S").FirstOrDefault();

            return regra;
        }
        
        public void IncluiLogRegraBol(PAGNET_CONFIG_REGRA_BOL regra, int codUsuario, string msgLog)
        {
            int id = _log.GetMaxKey();

            PAGNET_CONFIG_REGRA_BOL_LOG log = new PAGNET_CONFIG_REGRA_BOL_LOG();
            log.CODREGRA_LOG = id;
            log.CODREGRA = regra.CODREGRA;
            log.CODEMPRESA = regra.CODEMPRESA;
            log.COBRAJUROS = regra.COBRAJUROS;
            log.VLJUROSDIAATRASO = regra.VLJUROSDIAATRASO;
            log.PERCJUROS = regra.PERCJUROS;
            log.COBRAMULTA = regra.COBRAMULTA;
            log.VLMULTADIAATRASO = regra.VLMULTADIAATRASO;
            log.PERCMULTA = regra.PERCMULTA;
            log.CODPRIMEIRAINSTCOBRA = regra.CODPRIMEIRAINSTCOBRA;
            log.CODSEGUNDAINSTCOBRA = regra.CODSEGUNDAINSTCOBRA;
            log.TAXAEMISSAOBOLETO = regra.TAXAEMISSAOBOLETO;
            log.AGRUPARFATURAMENTOSDIA = regra.AGRUPARFATURAMENTOSDIA;
            log.ATIVO = regra.ATIVO;
            log.CODUSUARIO = codUsuario;
            log.DATINCLOG = DateTime.Now;
            log.DESCLOG = msgLog;

            _log.Add(log);

        }

        public void IncluiRegraBol(PAGNET_CONFIG_REGRA_BOL regra)
        {
            int id = _rep.GetMaxKey();
            regra.CODREGRA = id;

            _rep.Add(regra);
        }

        public void IncluiRegraPag(PAGNET_CONFIG_REGRA_PAG regra)
        {
            int id = _pag.GetMaxKey();
            regra.CODREGRA = id;

            _pag.Add(regra);
        }

        public void IncluiLogRegraPag(PAGNET_CONFIG_REGRA_PAG regra, int codUsuario, string msgLog)
        {
            int id = _pagLog.GetMaxKey();

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

        public async Task<PAGNET_CONFIG_REGRA_PAG> BuscaRegraAtivaPag(int codEmpresa)
        {
            var regra = _pag.Get(x => x.ATIVO == "S").FirstOrDefault();

            return regra;
        }
    }
}

