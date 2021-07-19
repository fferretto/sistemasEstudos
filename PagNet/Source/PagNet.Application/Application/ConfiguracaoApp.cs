using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using PagNet.Domain.Interface.Services.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class ConfiguracaoApp : IConfiguracaoApp
    {
        private readonly IPagNet_Config_RegraService _configRegra;
        private readonly IProceduresService _proc;
        private readonly IPagNet_InstrucaoCobrancaService _InstrucaoCobranca;
        private readonly IPagNet_CadEmpresaService _empresa;
        private readonly IPagNet_CadPlanoContasService _PlanoContas;
        private readonly IPagNet_Emissao_TitulosService _emissaoTitulos;
        private readonly IPagNet_EmissaoBoletoService _faturamento;

        public ConfiguracaoApp(IPagNet_Config_RegraService configRegra,
                               IProceduresService proc,
                               IPagNet_CadEmpresaService empresa,
                               IPagNet_CadPlanoContasService PlanoContas,
                               IPagNet_Emissao_TitulosService emissaoTitulos,
                               IPagNet_EmissaoBoletoService faturamento,
                               IPagNet_InstrucaoCobrancaService InstrucaoCobranca)
        {
            _configRegra = configRegra;
            _proc = proc;
            _InstrucaoCobranca = InstrucaoCobranca;
            _empresa = empresa;
            _PlanoContas = PlanoContas;
            _emissaoTitulos = emissaoTitulos;
            _faturamento = faturamento;
        }

        public async Task<RegraNegocioBoletoVm> BuscaRegraAtiva(int codEmpresa)
        {
            try
            {
                RegraNegocioBoletoVm regra = new RegraNegocioBoletoVm();
                var Dadosregra = await _configRegra.BuscaRegraAtivaBol(codEmpresa);

                if (Dadosregra != null)
                {
                    regra = RegraNegocioBoletoVm.ToView(Dadosregra);

                    regra.NMPRIMEIRAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Dadosregra.CODPRIMEIRAINSTCOBRA);
                    regra.NMSEGUNDAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Dadosregra.CODSEGUNDAINSTCOBRA);

                }
                var dadosEmpresa = await _empresa.ConsultaEmpresaById(codEmpresa);
                regra.CODEMPRESA = dadosEmpresa.CODEMPRESA.ToString();
                regra.NMEMPRESA = dadosEmpresa.NMFANTASIA;

                return regra;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<RegraNegocioPagamentoVm> BuscaRegraAtivaPagamento()
        {
            try
            {
                RegraNegocioPagamentoVm regra = new RegraNegocioPagamentoVm();
                var Dadosregra = await _configRegra.BuscaRegraAtivaPag();

                if (Dadosregra != null)
                {
                    regra = RegraNegocioPagamentoVm.ToView(Dadosregra);
                }

                return regra;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<RegraNegocioBoletoVm> BuscaRegraByID(int codRegra)
        {
            try
            {
                RegraNegocioBoletoVm regra = new RegraNegocioBoletoVm();
                var Dadosregra = await _configRegra.BuscaRegraBolByID(codRegra);

                regra = RegraNegocioBoletoVm.ToView(Dadosregra);

                if (Dadosregra.CODPRIMEIRAINSTCOBRA > 0)
                {
                    regra.NMPRIMEIRAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Dadosregra.CODPRIMEIRAINSTCOBRA);
                }
                if (Dadosregra.CODSEGUNDAINSTCOBRA > 0)
                {
                    regra.NMSEGUNDAINSTCOBRA = _InstrucaoCobranca.GetInstrucaoCobrancaById(Dadosregra.CODSEGUNDAINSTCOBRA);
                }

                return regra;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RegraNegocioPagamentoVm> BuscaRegraPagamentoByID(int codRegra)
        {
            try
            {
                RegraNegocioPagamentoVm regra = new RegraNegocioPagamentoVm();
                var Dadosregra = await _configRegra.BuscaRegraPagByID(codRegra);

                regra = RegraNegocioPagamentoVm.ToView(Dadosregra);

                return regra;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<bool, string>> DesativaRegraBol(int codRegra, int codUsuario, string Justificativa)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                var RegraBol = await _configRegra.BuscaRegraBolByID(codRegra);
                RegraBol.ATIVO = "N";


                _configRegra.AtualizaRegraBol(RegraBol);
                _configRegra.IncluiLogRegraBol(RegraBol, codUsuario, "Regra de emissão de boleto removida com a justificativa: " + Justificativa);
                resultado.Add(true, "Regra de Emissao de Boleto Desativada!");


            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }

        public async Task<IDictionary<bool, string>> DesativaRegraPagamento(int codRegra, int codUsuario, string Justificativa)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                var RegraBol = await _configRegra.BuscaRegraPagByID(codRegra);
                RegraBol.ATIVO = "N";


                _configRegra.AtualizaRegraPag(RegraBol);
                _configRegra.IncluiLogRegraPag(RegraBol, codUsuario, "Regra de pagamento removida com a justificativa: " + Justificativa);
                resultado.Add(true, "Regra de pagamento Desativada!");


            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }

        public async Task<IDictionary<bool, string>> SalvaRegraBoleto(RegraNegocioBoletoVm model)
        {
            var resultado = new Dictionary<bool, string>();

            if (Convert.ToInt32(model.CODPRIMEIRAINSTCOBRA) < 0)
                model.CODPRIMEIRAINSTCOBRA = "0";

            if (Convert.ToInt32(model.CODSEGUNDAINSTCOBRA) < 0)
                model.CODSEGUNDAINSTCOBRA = "0";

            PAGNET_CONFIG_REGRA_BOL RegraBol;


            if (model.CODREGRA == 0)
            {
                RegraBol = new PAGNET_CONFIG_REGRA_BOL();
            }
            else
            {
                RegraBol = await _configRegra.BuscaRegraBolByID(model.CODREGRA);
            }

            RegraBol.CODEMPRESA = Convert.ToInt32(model.CODEMPRESA);
            RegraBol.COBRAJUROS = (model.COBRAJUROS) ? "S" : "N";
            RegraBol.VLJUROSDIAATRASO = Geral.TrataDecimal(model.VLJUROSDIAATRASO);
            RegraBol.PERCJUROS = Geral.TrataDecimal(model.PERCJUROS);
            RegraBol.COBRAMULTA = (model.COBRAMULTA) ? "S" : "N";
            RegraBol.VLMULTADIAATRASO = Geral.TrataDecimal(model.VLMULTADIAATRASO);
            RegraBol.PERCMULTA = Geral.TrataDecimal(model.PERCMULTA);
            RegraBol.CODPRIMEIRAINSTCOBRA = Geral.TrataInteiro(model.CODPRIMEIRAINSTCOBRA);
            RegraBol.CODSEGUNDAINSTCOBRA = Geral.TrataInteiro(model.CODSEGUNDAINSTCOBRA);
            RegraBol.TAXAEMISSAOBOLETO = Geral.TrataDecimal(model.TAXAEMISSAOBOLETO);
            RegraBol.AGRUPARFATURAMENTOSDIA = (model.AgruparCobranca) ? "S" : "N";
            RegraBol.ATIVO = "S";


            try
            {
                if (model.CODREGRA == 0)
                {
                    _configRegra.IncluiRegraBol(RegraBol);
                    _configRegra.IncluiLogRegraBol(RegraBol, model.CodUsuario, "Nova Regra de emissão de boleto incluida.");
                    resultado.Add(true, "Regra de Emissao de Boleto cadastrada com sucesso");
                }
                else
                {
                    _configRegra.AtualizaRegraBol(RegraBol);
                    _configRegra.IncluiLogRegraBol(RegraBol, model.CodUsuario, "Regra de emissão de boleto Atualizada.");
                    resultado.Add(true, "Regra de Emissao de Boleto atualizada com sucesso");
                }

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;

        }

        public async Task<IDictionary<bool, string>> SalvaRegraPagamento(RegraNegocioPagamentoVm model)
        {
            var resultado = new Dictionary<bool, string>();


            PAGNET_CONFIG_REGRA_PAG RegraPag;


            if (model.CODREGRA == 0)
            {
                RegraPag = new PAGNET_CONFIG_REGRA_PAG();
            }
            else
            {
                RegraPag = await _configRegra.BuscaRegraPagByID(model.CODREGRA);
            }

            RegraPag.COBRATAXAANTECIPACAO = (model.COBRATAXAANTECIPACAO) ? "S" : "N";
            RegraPag.VLTAXAANTECIPACAO = Geral.TrataDecimal(model.VLTAXAANTECIPACAO);
            RegraPag.PERCTAXAANTECIPACAO = Geral.TrataDecimal(model.PORCENTAGEMTAXAANTECIPACAO);
            RegraPag.FORMACOMPENSACAO = model.TIPOFORMACOMPENSACAO;
            RegraPag.ATIVO = "S";


            try
            {
                if (model.CODREGRA == 0)
                {
                    _configRegra.IncluiRegraPag(RegraPag);
                    _configRegra.IncluiLogRegraPag(RegraPag, model.CodUsuario, "Nova Regra de pagamento incluida.");
                    resultado.Add(true, "Regra de pagamento cadastrada com sucesso");
                }
                else
                {
                    _configRegra.AtualizaRegraPag(RegraPag);
                    _configRegra.IncluiLogRegraPag(RegraPag, model.CodUsuario, "Regra de pagamento Atualizada.");
                    resultado.Add(true, "Regra de pagamento atualizada com sucesso");
                }

            }
            catch (ArgumentException ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;

        }
        public async Task<FiltroPlanoContasVm> CarregaPlanosContas(int codEmpresa)
        {
            try
            {
                FiltroPlanoContasVm PlanoContas = new FiltroPlanoContasVm();

                var DadosEmpresa = await _empresa.ConsultaEmpresaById(codEmpresa);
                PlanoContas.codigoEmpresa = codEmpresa.ToString();
                PlanoContas.nomeEmpresa = DadosEmpresa.NMFANTASIA;

                return PlanoContas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<VisualizaListaPlanoContasVM> BuscaListaPlanosContas(int codEmpresa)
        {
            try
            {
                VisualizaListaPlanoContasVM ListaPlanoContas = new VisualizaListaPlanoContasVM();
                var listaPlanoContas = await _PlanoContas.BuscaTodosPlanosContas(codEmpresa);
                var ListaRaiz = listaPlanoContas.Where(x => x.CODPLANOCONTAS_PAI == null).ToList();
                ListaPlanoContas.listaPlanoContas = MontaListaPlanoContas(ListaRaiz, listaPlanoContas);

                return ListaPlanoContas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static List<ListaPlanoContasVm> MontaListaPlanoContas(List<PAGNET_CADPLANOCONTAS> ListaRaiz, List<PAGNET_CADPLANOCONTAS> lista)
        {
            try
            {
                var novaLista = new List<ListaPlanoContasVm>();

                ListaPlanoContasVm PlanoContas = new ListaPlanoContasVm();
                foreach (var item in ListaRaiz)
                {
                    PlanoContas = new ListaPlanoContasVm();
                    PlanoContas.CODPLANOCONTAS = item.CODPLANOCONTAS;
                    PlanoContas.CODPLANOCONTAS_PAI = item.CODPLANOCONTAS_PAI ?? 0;
                    PlanoContas.CODEMPRESA = (int)item.CODEMPRESA;
                    PlanoContas.CLASSIFICACAO = item.CLASSIFICACAO;
                    PlanoContas.TIPO = item.TIPO;
                    PlanoContas.NATUREZA = item.NATUREZA;
                    PlanoContas.DESCRICAO = item.DESCRICAO;
                    PlanoContas.DESPESA = (item.DESPESA == "S") ? "Sim" : "Não";
                    PlanoContas.UTILIZADOPAGAMENTO = (item.DEFAULTPAGAMENTO == "S") ? "Sim" : "Não";
                    PlanoContas.UTILIZADORECEBIMENTO = (item.DEFAULTRECEBIMENTO == "S") ? "Sim" : "Não";
                    PlanoContas.ListaFilho = MontaListaPlanoContas(lista.Where(x => x.CODPLANOCONTAS_PAI == item.CODPLANOCONTAS).ToList(), lista);
                    novaLista.Add(PlanoContas);
                }
                return novaLista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IDictionary<bool, string>> RemovePlanoContas(int codigoPlanoContas, int codigoEmpresa)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {

                var listaPlanoContas = await _PlanoContas.BuscaTodosPlanosContas(codigoEmpresa);
                var ListaRaiz = listaPlanoContas.Where(x => x.CODPLANOCONTAS == codigoPlanoContas).ToList();
                RemovePlanosContas(ListaRaiz, listaPlanoContas);
                resultado.Add(true, "Plano de contas removidas com sucesso!");

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        private void RemovePlanosContas(List<PAGNET_CADPLANOCONTAS> ListaRaiz, List<PAGNET_CADPLANOCONTAS> lista)
        {
            try
            {
                foreach (var item in ListaRaiz)
                {
                    if (lista.Where(x => x.CODPLANOCONTAS_PAI == item.CODPLANOCONTAS).Count() > 0)
                    {
                        RemovePlanosContas(lista.Where(x => x.CODPLANOCONTAS_PAI == item.CODPLANOCONTAS).ToList(), lista);
                    }
                    item.ATIVO = "N";
                    _PlanoContas.AtualizaPlanoContas(item);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<bool, string>> BuscaNovaClassificacaoPlanoContas(int CodigoPlanoContas, int codigoEmpresa)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                var listaPlanoContas = await _PlanoContas.BuscaTodosPlanosContas(codigoEmpresa);
                string novaClassificacao = "";
                if (CodigoPlanoContas > 0)
                {
                    var quantidadeFilho = listaPlanoContas.Where(x => x.CODPLANOCONTAS_PAI == CodigoPlanoContas).Count();
                    var Classificacao = listaPlanoContas.Where(x => x.CODPLANOCONTAS == CodigoPlanoContas).Select(t => t.CLASSIFICACAO).FirstOrDefault();
                    string ultcaractere = Classificacao.Substring(Classificacao.Length - 1, 1);
                    if (ultcaractere == ".")
                    {
                        Classificacao = Classificacao.Substring(0, Classificacao.Length - 1);
                    }
                    novaClassificacao = Classificacao + "." + (quantidadeFilho + 1);
                }
                else
                {
                    var quantidadeFilho = listaPlanoContas.Where(x => x.CODPLANOCONTAS_PAI == null).Count();
                    novaClassificacao = (quantidadeFilho + 1).ToString();
                }
                resultado.Add(true, novaClassificacao);

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }

        public async Task<IDictionary<bool, string>> SalvarNovoPlanoContas(PlanoContasVm model)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                
                if (model.PagamentoCentralizadora)
                {
                    var regra = await _PlanoContas.BuscaDefaultPlanosContasPagamento(model.codigoEmpresaPlanoContas);
                    if (regra != null)
                    {
                        regra.DEFAULTPAGAMENTO = "N";
                        _PlanoContas.AtualizaPlanoContas(regra);
                    }
                }
                if (model.RecebimentoClienteNetCard)
                {
                    var regra = await _PlanoContas.BuscaDefaultPlanosContasRecebimento(model.codigoEmpresaPlanoContas);
                    if (regra != null)
                    {
                        regra.DEFAULTRECEBIMENTO = "N";
                        _PlanoContas.AtualizaPlanoContas(regra);
                    }
                }

                PAGNET_CADPLANOCONTAS PC = new PAGNET_CADPLANOCONTAS();
                if (model.CODPLANOCONTAS > 0)
                {
                    PC = await _PlanoContas.BuscaTodosPlanosContasByID(model.CODPLANOCONTAS);
                }
                else
                {
                    if (model.CODPLANOCONTAS_PAI > 0)
                    {
                        PC.CODPLANOCONTAS_PAI = model.CODPLANOCONTAS_PAI;
                    }
                }
                               
                PC.CODEMPRESA = model.codigoEmpresaPlanoContas;
                PC.CLASSIFICACAO = model.Classificacao;
                PC.TIPO = model.CodigoTipoConta;
                PC.NATUREZA = model.CodigoNatureza;
                PC.DESCRICAO = model.Descricao;
                PC.DESPESA = (model.TipoDespesa) ? "S": "N";
                PC.PLANOCONTASDEFAULT = "N";
                PC.DEFAULTPAGAMENTO = (model.PagamentoCentralizadora) ? "S" : "N";
                PC.DEFAULTRECEBIMENTO = (model.RecebimentoClienteNetCard) ? "S" : "N";
                PC.ATIVO = "S";

                _PlanoContas.IncluiPlanoContas(PC);
                
                if (model.CODPLANOCONTAS > 0)
                {
                    resultado.Add(true, "Plano de contas alterado com sucesso.");
                }
                else
                {
                    resultado.Add(true, "Plano de contas criado com sucesso.");
                }

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<PlanoContasVm> CarregaPlanoContas(int codPlanoContas)
        {
            try
            {
                PlanoContasVm Plano = new PlanoContasVm();
                string CodigoRaiz = "0";
                string nmRaiz = "";
                var DadosPlanoConta = await _PlanoContas.BuscaTodosPlanosContasByID(codPlanoContas);
                if (DadosPlanoConta.CODPLANOCONTAS_PAI != null)
                {
                    var DadosPai = await _PlanoContas.BuscaTodosPlanosContasByID((int)DadosPlanoConta.CODPLANOCONTAS_PAI);
                    CodigoRaiz = DadosPai.CODPLANOCONTAS.ToString();
                    nmRaiz = DadosPai.DESCRICAO;
                }
                Plano = PlanoContasVm.ToView(DadosPlanoConta, CodigoRaiz, nmRaiz);

                return Plano;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
