using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;

namespace PagNet.Domain.Services
{
    public class PagNet_EmissaoBoletoService : ServiceBase<PAGNET_EMISSAOBOLETO>, IPagNet_EmissaoBoletoService
    {
        private readonly IPagNet_EmissaoBoletoRepository _rep;
        private readonly IPAGNET_EMISSAOFATURAMENTORepository _repFaturamento;
        private readonly IPAGNET_EMISSAOFATURAMENTO_LOGRepository _log;


        public PagNet_EmissaoBoletoService(IPagNet_EmissaoBoletoRepository rep,
                                           IPAGNET_EMISSAOFATURAMENTORepository repFaturamento,
                                           IPAGNET_EMISSAOFATURAMENTO_LOGRepository log)
            : base(rep)
        {
            _rep = rep;
            _log = log;
            _repFaturamento = repFaturamento;
        }

        public void AtualizaBoleto(PAGNET_EMISSAOBOLETO bol)
        {
            _rep.Update(bol);

        }

        public void AtualizaEmissaoBoletoVencido()
        {
            var BoletosVencidos = _rep.Get(x => x.dtVencimento <= DateTime.Now.AddDays(-1) &&
                                    x.Status == "PENDENTE_REGISTRO").ToList();

            foreach (var lista in BoletosVencidos)
            {
                lista.Status = "VENCIDO";
                _rep.Update(lista);
            }
        }


        public void AtualizaStatusBycodBordero(int codBordero, string NovoStatus)
        {
            var Boletos = BuscaTodosFaturamentoByCodBordero(codBordero).Result;

            foreach (var lista in Boletos)
            {
                lista.STATUS = NovoStatus;
                lista.CODBORDERO = null;
                _repFaturamento.Update(lista);
            }
        }

        public async Task<List<PAGNET_EMISSAOBOLETO>> BuscaBoletoByCodCliente(int CodCliente)
        {
            var boleto = _rep.Get(x => x.CodCliente == CodCliente, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();

            return boleto;
        }

        public async Task<PAGNET_EMISSAOBOLETO> BuscaBoletoByID(int id)
        {
            try
            {
                var boleto = _rep.Get(x => x.codEmissaoBoleto == id, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").FirstOrDefault();

                return boleto;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PAGNET_EMISSAOBOLETO> BuscaBoletoByNossoNumero(string NossoNumero)
        {
            var boleto = _rep.Get(x => x.NossoNumero == NossoNumero, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").FirstOrDefault();

            return boleto;
        }

        public async Task<PAGNET_EMISSAOBOLETO> BuscaBoletoBySeuNumero(string seuNumero)
        {
            try
            {
                var boleto = _rep.Get(x => x.SeuNumero == seuNumero, "PAGNET_CADCLIENTE").FirstOrDefault();

                return boleto;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO_LOG>> BuscaLog(int codEmissaoFaturamento)
        {
            var boleto = _log.Get(x => x.CODEMISSAOFATURAMENTO == codEmissaoFaturamento, "USUARIO_NETCARD").ToList();

            return boleto;
        }

        public int BuscaNovoIDEmissaoBoleto()
        {
            int numBol = _rep.GetMaxKey();

            return numBol;
        }

        public int BuscaNovoIDEmissaoFaturamento()
        {
            int numBol = _repFaturamento.GetMaxKey();

            return numBol;
        }
        public async Task<List<PAGNET_EMISSAOBOLETO>> BuscaTodosBoletoByCodArquivo(int CodArquivo)
        {
            var boleto = _rep.Get(x => x.CODARQUIVO == CodArquivo, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();
            return boleto;
        }

        public async Task<List<PAGNET_EMISSAOBOLETO>> BustaTodosBoletosParaEdicao(int codigoEmpresa)
        {
            var boleto = _rep.Get(x => (x.Status.Contains("REGISTRADO") || x.Status.Contains("EM_ABERTO") || x.Status.Contains("PENDENTE_REGISTRO")) && 
                                            x.codEmpresa == codigoEmpresa, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();
            return boleto;
        }

        public async Task<List<PAGNET_EMISSAOBOLETO>> GetAllBoletos(string status, int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim)
        {
            status = (status == "TODOS") ? "" : status;

            return _rep.Get(x => x.codEmpresa == codEmpresa &&
                                 ((status == "") || x.Status == status) &&                                 
                                 ((codCli == 0) || x.CodCliente == codCli) &&
                                 x.dtVencimento >= dtInicio &&
                                 x.dtVencimento <= dtFim, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA"
                                 ).ToList();
        }

        public async Task<List<PAGNET_EMISSAOBOLETO>> GetAllBoletosNaoVencidos(int codEmpresa, int codCli)
        {
            DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            return _rep.Get(x => (x.Status == "REGISTRADO" || x.Status == "PENDENTE_REGISTRO") &&
                                 x.codEmpresa == codEmpresa &&
                                 x.dtVencimento >= dataAtual &&
                                 ((codCli == 0) || x.CodCliente == codCli), "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();
        }

        public async Task<List<PAGNET_EMISSAOBOLETO>> GetBoletosRegistradosNaoLiquidados(int codEmpresa, int CodContaCorrente, int codCli, DateTime dtInicio, DateTime dtFim)
        {
            return _rep.Get(x => new[] { "REGISTRADO", "VENCIDO" }.Contains(x.Status) &&
                                 x.codEmpresa == codEmpresa &&
                                 ((codCli == 0) || x.CodCliente == codCli) &&
                                 ((dtInicio == Convert.ToDateTime("01/01/1900")) || x.dtVencimento >= dtInicio) &&
                                 ((dtFim == Convert.ToDateTime("01/01/1900")) || x.dtVencimento <= dtFim), "PAGNET_CADCLIENTE, PAGNET_CADEMPRESA"
                                 ).ToList();
        }

        public void IncluiBoleto(PAGNET_EMISSAOBOLETO bol)
        {
            try
            {
                _rep.Add(bol);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluiLog(PAGNET_EMISSAOFATURAMENTO faturamento, int codUsuario, string Justificativa)
        {
            try
            {

                int numBol_log = _log.GetMaxKey();

                PAGNET_EMISSAOFATURAMENTO_LOG LOG = new PAGNET_EMISSAOFATURAMENTO_LOG();

                LOG.CODEMISSAOFATURAMENTO_LOG = numBol_log;
                LOG.CODEMISSAOFATURAMENTO = faturamento.CODEMISSAOFATURAMENTO;
                LOG.STATUS = faturamento.STATUS;
                LOG.CODCLIENTE = Convert.ToInt32(faturamento.CODCLIENTE);
                LOG.CODBORDERO = faturamento.CODBORDERO;
                LOG.DATPGTO = faturamento.DATPGTO;
                LOG.VLPGTO = faturamento.VLPGTO;
                LOG.CODFORMAFATURAMENTO = faturamento.CODFORMAFATURAMENTO;
                LOG.TIPOFATURAMENTO = faturamento.TIPOFATURAMENTO;
                LOG.NROREF_NETCARD = faturamento.NROREF_NETCARD;
                LOG.ORIGEM = faturamento.ORIGEM;
                LOG.CODEMPRESA = faturamento.CODEMPRESA;
                LOG.DATVENCIMENTO = faturamento.DATVENCIMENTO;
                LOG.SEUNUMERO = faturamento.SEUNUMERO;
                LOG.VALOR = faturamento.VALOR;
                LOG.DATSOLICITACAO = faturamento.DATSOLICITACAO;
                LOG.DATSEGUNDODESCONTO = faturamento.DATSEGUNDODESCONTO;
                LOG.VLDESCONTO = faturamento.VLDESCONTO;
                LOG.VLSEGUNDODESCONTO = faturamento.VLSEGUNDODESCONTO;
                LOG.MENSAGEMARQUIVOREMESSA = faturamento.MENSAGEMARQUIVOREMESSA;
                LOG.MENSAGEMINSTRUCOESCAIXA = faturamento.MENSAGEMINSTRUCOESCAIXA;
                LOG.CODUSUARIO = codUsuario;
                LOG.DATINCLOG = DateTime.Now;
                LOG.DESCLOG = Justificativa;
                LOG.NRODOCUMENTO = faturamento.NRODOCUMENTO;
                LOG.VLDESCONTOCONCEDIDO = faturamento.VLDESCONTOCONCEDIDO;
                LOG.JUROSCOBRADO = faturamento.JUROSCOBRADO;
                LOG.MULTACOBRADA = faturamento.MULTACOBRADA;
                LOG.CODEMISSAOFATURAMENTOPAI = faturamento.CODEMISSAOFATURAMENTOPAI;
                LOG.PARCELA = faturamento.PARCELA;
                LOG.TOTALPARCELA = faturamento.TOTALPARCELA;
                LOG.VALORPARCELA = faturamento.VALORPARCELA;
                LOG.CODCONTACORRENTE = faturamento.CODCONTACORRENTE;


                _log.Add(LOG);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void IncluiLogByBordero(int codBordero, string status, int codUsuario, string Justificativa)
        {
            var ListaTitulos = BuscaTodosFaturamentoByCodBordero(codBordero).Result;
            PAGNET_EMISSAOFATURAMENTO_LOG LOG = new PAGNET_EMISSAOFATURAMENTO_LOG();
            LOG = null;

            foreach (var faturamento in ListaTitulos)
            {
                int id = _log.GetMaxKey();
                LOG = new PAGNET_EMISSAOFATURAMENTO_LOG();

                LOG.CODEMISSAOFATURAMENTO_LOG = id;
                LOG.CODEMISSAOFATURAMENTO = faturamento.CODEMISSAOFATURAMENTO;
                LOG.STATUS = faturamento.STATUS;
                LOG.CODCLIENTE = Convert.ToInt32(faturamento.CODCLIENTE);
                LOG.CODBORDERO = faturamento.CODBORDERO;
                LOG.DATPGTO = faturamento.DATPGTO;
                LOG.VLPGTO = faturamento.VLPGTO;
                LOG.CODFORMAFATURAMENTO = faturamento.CODFORMAFATURAMENTO;
                LOG.TIPOFATURAMENTO = faturamento.TIPOFATURAMENTO;
                LOG.NROREF_NETCARD = faturamento.NROREF_NETCARD;
                LOG.ORIGEM = faturamento.ORIGEM;
                LOG.CODEMPRESA = faturamento.CODEMPRESA;
                LOG.DATVENCIMENTO = faturamento.DATVENCIMENTO;
                LOG.SEUNUMERO = faturamento.SEUNUMERO;
                LOG.VALOR = faturamento.VALOR;
                LOG.DATSOLICITACAO = faturamento.DATSOLICITACAO;
                LOG.DATSEGUNDODESCONTO = faturamento.DATSEGUNDODESCONTO;
                LOG.VLDESCONTO = faturamento.VLDESCONTO;
                LOG.VLSEGUNDODESCONTO = faturamento.VLSEGUNDODESCONTO;
                LOG.MENSAGEMARQUIVOREMESSA = faturamento.MENSAGEMARQUIVOREMESSA;
                LOG.MENSAGEMINSTRUCOESCAIXA = faturamento.MENSAGEMINSTRUCOESCAIXA;
                LOG.CODUSUARIO = codUsuario;
                LOG.DATINCLOG = DateTime.Now;
                LOG.DESCLOG = Justificativa;
                LOG.NRODOCUMENTO = faturamento.NRODOCUMENTO;
                LOG.VLDESCONTOCONCEDIDO = faturamento.VLDESCONTOCONCEDIDO;
                LOG.JUROSCOBRADO = faturamento.JUROSCOBRADO;
                LOG.MULTACOBRADA = faturamento.MULTACOBRADA;
                LOG.CODEMISSAOFATURAMENTOPAI = faturamento.CODEMISSAOFATURAMENTOPAI;
                LOG.PARCELA = faturamento.PARCELA;
                LOG.TOTALPARCELA = faturamento.TOTALPARCELA;
                LOG.VALORPARCELA = faturamento.VALORPARCELA;
                LOG.CODCONTACORRENTE = faturamento.CODCONTACORRENTE;

                _log.Add(LOG);
            }
        }

        public async Task<List<PAGNET_EMISSAOBOLETO>> BustaTodosBoletosVencidos()
        {
            DateTime datLimite = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            return _rep.Get(x => x.dtVencimento < datLimite && x.Status == "EM_BORDERO").ToList();
        }

        public async Task<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoByID(int id)
        {
            try
            {
                var faturamento = _repFaturamento.Get(x => x.CODEMISSAOFATURAMENTO == id, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").FirstOrDefault();

                return faturamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaTodosFaturamentoByCodBordero(int CodBordero)
        {
            var faturamento = _repFaturamento.Get(x => x.CODBORDERO == CodBordero, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();
            return faturamento;
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BustaTodosFaturamentosParaEdicao(int codigoEmpresa)
        {
            var boleto = _repFaturamento.Get(x => (x.STATUS == "REGISTRADO" || x.STATUS == "EM_ABERTO" || x.STATUS == "A_FATURAR" || x.STATUS =="PENDENTE_REGISTRO") &&
                                            x.CODEMPRESA == codigoEmpresa, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();
            return boleto;
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentoBySeuNumero(string seuNumero)
        {
            try
            {
                var boleto = _repFaturamento.Get(x => x.SEUNUMERO == seuNumero && x.STATUS != "CANCELADO").ToList();

                return boleto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentoByCodCliente(int CodCliente)
        {
            try
            {
                var faturamento = _repFaturamento.Get(x => x.CODCLIENTE == CodCliente && x.STATUS != "CANCELADO", "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();

                return faturamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluiFaturamento(PAGNET_EMISSAOFATURAMENTO bol)
        {
            _repFaturamento.Add(bol);
        }

        public void AtualizaFaturamento(PAGNET_EMISSAOFATURAMENTO bol)
        {
            _repFaturamento.Update(bol);
        }

        public void IncluiLogBySeuNumero(string seuNumero, string status, int codUsuario, string Justificativa)
        {
            var ListaTitulos = BuscaFaturamentoBySeuNumero(seuNumero).Result;
            PAGNET_EMISSAOFATURAMENTO_LOG LOG = new PAGNET_EMISSAOFATURAMENTO_LOG();
            LOG = null;

            foreach (var faturamento in ListaTitulos)
            {
                int id = _log.GetMaxKey();
                LOG = new PAGNET_EMISSAOFATURAMENTO_LOG();

                LOG.CODEMISSAOFATURAMENTO_LOG = id;
                LOG.CODEMISSAOFATURAMENTO = faturamento.CODEMISSAOFATURAMENTO;
                LOG.STATUS = faturamento.STATUS;
                LOG.CODCLIENTE = Convert.ToInt32(faturamento.CODCLIENTE);
                LOG.CODBORDERO = faturamento.CODBORDERO;
                LOG.DATPGTO = faturamento.DATPGTO;
                LOG.VLPGTO = faturamento.VLPGTO;
                LOG.CODFORMAFATURAMENTO = faturamento.CODFORMAFATURAMENTO;
                LOG.TIPOFATURAMENTO = faturamento.TIPOFATURAMENTO;
                LOG.NROREF_NETCARD = faturamento.NROREF_NETCARD;
                LOG.ORIGEM = faturamento.ORIGEM;
                LOG.CODEMPRESA = faturamento.CODEMPRESA;
                LOG.DATVENCIMENTO = faturamento.DATVENCIMENTO;
                LOG.SEUNUMERO = faturamento.SEUNUMERO;
                LOG.VALOR = faturamento.VALOR;
                LOG.DATSOLICITACAO = faturamento.DATSOLICITACAO;
                LOG.DATSEGUNDODESCONTO = faturamento.DATSEGUNDODESCONTO;
                LOG.VLDESCONTO = faturamento.VLDESCONTO;
                LOG.VLSEGUNDODESCONTO = faturamento.VLSEGUNDODESCONTO;
                LOG.MENSAGEMARQUIVOREMESSA = faturamento.MENSAGEMARQUIVOREMESSA;
                LOG.MENSAGEMINSTRUCOESCAIXA = faturamento.MENSAGEMINSTRUCOESCAIXA;
                LOG.CODUSUARIO = codUsuario;
                LOG.DATINCLOG = DateTime.Now;
                LOG.DESCLOG = Justificativa;
                LOG.NRODOCUMENTO = faturamento.NRODOCUMENTO;
                LOG.VLDESCONTOCONCEDIDO = faturamento.VLDESCONTOCONCEDIDO;
                LOG.JUROSCOBRADO = faturamento.JUROSCOBRADO;
                LOG.MULTACOBRADA = faturamento.MULTACOBRADA;
                LOG.CODEMISSAOFATURAMENTOPAI = faturamento.CODEMISSAOFATURAMENTOPAI;
                LOG.PARCELA = faturamento.PARCELA;
                LOG.TOTALPARCELA = faturamento.TOTALPARCELA;
                LOG.VALORPARCELA = faturamento.VALORPARCELA;
                LOG.CODCONTACORRENTE = faturamento.CODCONTACORRENTE;

                _log.Add(LOG);
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> GetAllFaturas(string status, int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim)
        {
            status = (status == "TODOS") ? "" : status;

            return _repFaturamento.Get(x => x.CODEMPRESA == codEmpresa &&
                                 ((status == "") || x.STATUS == status) &&
                                 x.STATUS != "CANCELADO" &&
                                 x.STATUS != "CONSOLIDADO" &&
                                 x.STATUS != "PENDENTE_COSOLIDACAO" &&
                                 ((codCli == 0) || x.CODCLIENTE == codCli) &&
                                 x.DATVENCIMENTO >= dtInicio &&
                                 x.DATVENCIMENTO <= dtFim, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA"
                                 ).ToList();
        }

        public async Task<PAGNET_EMISSAOFATURAMENTO> BuscaFaturamentoByNroDocumento(string nroDocumento)
        {
            try
            {
                var boleto = _repFaturamento.Get(x => x.NRODOCUMENTO == nroDocumento && x.STATUS != "CANCELADO").FirstOrDefault();

                return boleto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> ListaFaturamentoNaoLiquidado(int codEmpresa)
        {
            try
            {
                DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                var dados = _repFaturamento.Get(x => x.CODEMPRESA == codEmpresa 
                                                && x.STATUS != "CANCELADO"
                                                && x.STATUS != "LIQUIDADO"
                                                && x.STATUS != "LIQUIDADO_MANUALMENTE"
                                                && x.STATUS != "CONSOLIDADO"
                                                && x.STATUS != "PENDENTE_COSOLIDACAO" 
                                                && x.DATVENCIMENTO < dataAtual, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA").ToList();
                return dados.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaTransacaoAConsolidar(int codigoEmpresa, int codContaCorrente, int mes, int ano)
        {
            var listaTransacao = _repFaturamento.Get(x => x.CODEMPRESA == codigoEmpresa &&
                                   x.CODCONTACORRENTE == codContaCorrente &&
                                   x.STATUS == "PENDENTE_COSOLIDACAO" &&
                                   x.DATVENCIMENTO.Month == mes &&
                                   x.DATVENCIMENTO.Year == ano
                                   ).ToList();
            return listaTransacao;
        }

        public async Task<int> RetornaquantidadeParcelasFuturas(int codigo)
        {
            try
            {
                var codigoPai = _repFaturamento.Get(x => x.CODEMISSAOFATURAMENTO == codigo).FirstOrDefault().CODEMISSAOFATURAMENTOPAI;
                var qtTransacoes = _repFaturamento.Get(x => x.CODEMISSAOFATURAMENTOPAI == codigoPai && x.CODEMISSAOFATURAMENTO > codigo && x.STATUS == "PENDENTE_COSOLIDACAO").ToList().Count();

                return qtTransacoes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentoByIDPai(int codigoPai, int Codigo)
        {
            if (codigoPai > 0)
            {
                return _repFaturamento.Get(x => x.CODEMISSAOFATURAMENTOPAI == codigoPai, "PAGNET_CADCLIENTE").ToList();
            }
            else
            {
                codigoPai = _repFaturamento.Get(x => x.CODEMISSAOFATURAMENTO == Codigo).FirstOrDefault().CODEMISSAOFATURAMENTOPAI;
                return _repFaturamento.Get(x => x.CODEMISSAOFATURAMENTOPAI == codigoPai).ToList();
            }
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentosByData(DateTime dtRealPGTO, int codContaCorrente)
        {
            var ListaFaturamentos = _repFaturamento.Get(x => x.DATVENCIMENTO == dtRealPGTO &&
                                                        ((codContaCorrente == 0) || (x.CODCONTACORRENTE == codContaCorrente))).ToList();

            return ListaFaturamentos;
        }

        public async Task<List<PAGNET_EMISSAOFATURAMENTO>> BuscaFaturamentosNaoConciliados(int codigoEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var listaTransacao = _repFaturamento.Get(x => x.CODEMPRESA == codigoEmpresa &&
                                    x.CODCONTACORRENTE == codContaCorrente &&
                                    x.DATVENCIMENTO >= dataInicio &&
                                    x.DATVENCIMENTO <= dataFim &&
                                    (x.STATUS == "BAIXADO" || x.STATUS == "BAIXADO_MANUALMENTE"), "PAGNET_CADCLIENTE");

            return listaTransacao.ToList();
        }

        public object[][] CarregaListaFaturas(int codigoCliente)
        {
            return _repFaturamento.Get(x => x.STATUS == "A_FATURAR" &&
                                 x.DATVENCIMENTO <= DateTime.Now.AddMonths(1) &&
                                 x.CODCLIENTE == codigoCliente)
                .Select(x => new object[] { x.CODEMISSAOFATURAMENTO, "Código Referência: " + x.NROREF_NETCARD + 
                                     " Valor: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}",  x.VALOR)}).ToArray();
        }

        public void AtualizaPlanoContasFaturamentosNaoLiquidados(int codigoEmpresa, int codigoPlanoContas, int codigousuario)
        {
            var listaFaturamento = _repFaturamento.Get(x => x.STATUS == "A_FATURAR" && x.CODEMPRESA == codigoEmpresa).ToList();
            foreach(var fatura in listaFaturamento)
            {
                fatura.CODPLANOCONTAS = codigoPlanoContas;

                AtualizaFaturamento(fatura);
                IncluiLog(fatura, codigousuario, "Alteração do plano de contas padrão.");
            }
        }
        public void CancelaFaturaHomologacao(int codigoContaCorrente, int codigoUsuario, string Justificativa)
        {
            var fatura = _repFaturamento.Get(x => x.CODCONTACORRENTE == codigoContaCorrente &&
                                               x.STATUS != "CANCELADO" &&
                                               x.STATUS != "LIQUIDADO" &&
                                               x.TIPOFATURAMENTO == "HOM").FirstOrDefault();

            fatura.STATUS = "CANCELADO";
            _repFaturamento.Update(fatura);

            IncluiLog(fatura, codigoUsuario, Justificativa);
        }
        public PAGNET_EMISSAOFATURAMENTO RetornaFaturaHomologacao(int codigoContaCorrente)
        {
            var fatura = _repFaturamento.Get(x => x.CODCONTACORRENTE == codigoContaCorrente &&
                                               x.STATUS != "CANCELADO" &&
                                               x.STATUS != "LIQUIDADO" &&
                                               x.TIPOFATURAMENTO == "HOM").FirstOrDefault();

            return fatura;
        }

        public List<PAGNET_EMISSAOBOLETO> GetAllBoletosNaoPagos(int codEmpresa, int codCli, DateTime dtInicio, DateTime dtFim)
        {

            return _rep.Get(x => x.codEmpresa == codEmpresa &&
                                  x.Status != "BAIXADO" &&
                                  x.Status != "LIQUIDADO" &&
                                  x.Status != "CANCELADO" &&
                                  x.Status != "BAIXADO_MANUALMENTE" &&
                                 ((codCli == 0) || x.CodCliente == codCli) &&
                                 x.dtVencimento >= dtInicio &&
                                 x.dtVencimento <= dtFim, "PAGNET_CADCLIENTE,PAGNET_CADEMPRESA"
                                 ).ToList();
        }
    }
}
