using FluentDateTime;
using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using PagNet.Domain.Interface.Services.Procedures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class TesourariaApp : ITesourariaApp
    {
        private readonly IPagNet_CadClienteService _cliente;
        private readonly IPagNet_InstrucaoCobrancaService _instrucaoCobranca;
        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_EmissaoBoletoService _emissaoFaturamento;
        private readonly IPagNet_Emissao_TitulosService _emissaoTitulos;
        private readonly IPagNet_Titulos_PagosService _titulosPagos;
        private readonly IPagNet_CadEmpresaService _empresa;
        private readonly IOperadoraService _ope;
        private readonly IPagNet_Bordero_BoletoService _bordero;
        private readonly IPagNet_OcorrenciaRetBolService _ocorrenciaBol;
        private readonly IProceduresService _proc;
        private readonly IPagNet_Config_RegraService _regraBol;
        private readonly IPagNet_Formas_FaturamentoService _formaFaturamento;


        public TesourariaApp(IPagNet_InstrucaoCobrancaService instrucaoCobranca,
                             IPagNet_Emissao_TitulosService emissaoTitulos,
                             IPagNet_CadClienteService cliente,
                             IPagNet_ContaCorrenteService conta,
                             IPagNet_ArquivoService arquivo,
                             IOperadoraService ope,
                             IPagNet_EmissaoBoletoService emissaoBoleto,
                             IPagNet_Titulos_PagosService titulosPagos,
                             IPagNet_CadEmpresaService empresa,
                             IPagNet_OcorrenciaRetBolService ocorrenciaBol,
                             IProceduresService proc,
                             IPagNet_Config_RegraService regraBol,
                             IPagNet_Formas_FaturamentoService formaFaturamento,
                             IPagNet_Bordero_BoletoService bordero)
        {
            _cliente = cliente;
            _instrucaoCobranca = instrucaoCobranca;
            _conta = conta;
            _arquivo = arquivo;
            _emissaoFaturamento = emissaoBoleto;
            _emissaoTitulos = emissaoTitulos;
            _empresa = empresa;
            _bordero = bordero;
            _ope = ope;
            _ocorrenciaBol = ocorrenciaBol;
            _proc = proc;
            _regraBol = regraBol;
            _formaFaturamento = formaFaturamento;
            _titulosPagos = titulosPagos;
        }
        /// <summary>
        /// ~Inclusão de transações de Recebimetnos que não deverão ser incluídos no arquivo de remessa.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<IDictionary<bool, string>> IncluirNovaTransacaoENTRADA(IncluiTransacaoVM vm)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                bool UltimoDiaMes = false;
                int codigoPai = 0;
                DateTime dtParcela = Convert.ToDateTime(vm.dtIncTransacao);
                //DateTime com o último dia do mês
                DateTime ultimoDiaDoMes = dtParcela.LastDayOfMonth();

                if (ultimoDiaDoMes.Day == dtParcela.Day)
                {
                    UltimoDiaMes = true;
                }
                int TotalParcelas = (vm.ParcelaTerminoIncTransacao - vm.ParcelaInicioIncTransacao) + 1;
                for (int i = 1; i <= TotalParcelas; i++)
                {
                    //CRIA UM NOVO REGISTRO NA TABELA DE EMISSÃO DE TÍTULOS
                    var NovoCodEmissaoFaturamento = _emissaoFaturamento.BuscaNovoIDEmissaoFaturamento();

                    if (codigoPai == 0)
                    {
                        codigoPai = NovoCodEmissaoFaturamento;
                    }

                    PAGNET_EMISSAOFATURAMENTO EF = new PAGNET_EMISSAOFATURAMENTO();

                    EF.CODEMISSAOFATURAMENTO = NovoCodEmissaoFaturamento;
                    EF.STATUS = "PENDENTE_COSOLIDACAO";
                    EF.CODCLIENTE = null;
                    EF.CODBORDERO = null;
                    EF.CODEMPRESA = Convert.ToInt32(vm.codigoEmpresaIncTransacao);
                    EF.CODFORMAFATURAMENTO = 2;
                    EF.ORIGEM = "PN";
                    EF.TIPOFATURAMENTO = vm.DescricaoIncTransacao;
                    EF.NROREF_NETCARD = null;
                    EF.SEUNUMERO = null;
                    EF.VALOR = Convert.ToDecimal(vm.ValorIncTransacao) * TotalParcelas;
                    EF.DATSOLICITACAO = DateTime.Now;
                    EF.DATSEGUNDODESCONTO = null;
                    EF.VLDESCONTO = null;
                    EF.VLSEGUNDODESCONTO = null;
                    EF.MENSAGEMARQUIVOREMESSA = null;
                    EF.MENSAGEMINSTRUCOESCAIXA = null;
                    EF.NRODOCUMENTO = null;
                    EF.DATPGTO = null;
                    EF.VLPGTO = null;
                    EF.VLDESCONTOCONCEDIDO = null;
                    EF.JUROSCOBRADO = null;
                    EF.MULTACOBRADA = null;
                    EF.CODEMISSAOFATURAMENTOPAI = codigoPai;
                    EF.PARCELA = i;
                    EF.TOTALPARCELA = TotalParcelas;
                    EF.VALORPARCELA = Convert.ToDecimal(vm.ValorIncTransacao);
                    EF.CODCONTACORRENTE = Convert.ToInt32(vm.codContaCorrenteIncTransacao);

                    if (UltimoDiaMes)
                    {
                        EF.DATVENCIMENTO = dtParcela.LastDayOfMonth();
                    }
                    else
                    {
                        EF.DATVENCIMENTO = dtParcela;
                    }

                    _emissaoFaturamento.IncluiFaturamento(EF);
                    //INSERI LOG DO REGISTRO
                    _emissaoFaturamento.IncluiLog(EF, vm.codigoUsuarioIncTransacao, "Inclusão de Transação via PagNet");

                    dtParcela = dtParcela.AddMonths(1);
                }

                resultado.Add(true, "Transação incluída com sucesso");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
                throw ex;
            }
            return resultado;
        }
        public async Task<IDictionary<bool, string>> IncluirNovaTransacaoJaConciliadaENTRADA(IncluiTransacaoVM vm)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                bool UltimoDiaMes = false;
                int codigoPai = 0;
                DateTime dtParcela = Convert.ToDateTime(vm.dtIncTransacao);
                //DateTime com o último dia do mês
                DateTime ultimoDiaDoMes = dtParcela.LastDayOfMonth();

                if (ultimoDiaDoMes.Day == dtParcela.Day)
                {
                    UltimoDiaMes = true;
                }
                int TotalParcelas = (vm.ParcelaTerminoIncTransacao - vm.ParcelaInicioIncTransacao) + 1;
                for (int i = 1; i <= TotalParcelas; i++)
                {
                    //CRIA UM NOVO REGISTRO NA TABELA DE EMISSÃO DE TÍTULOS
                    var NovoCodEmissaoFaturamento = _emissaoFaturamento.BuscaNovoIDEmissaoFaturamento();

                    if (codigoPai == 0)
                    {
                        codigoPai = NovoCodEmissaoFaturamento;
                    }

                    PAGNET_EMISSAOFATURAMENTO EF = new PAGNET_EMISSAOFATURAMENTO();

                    EF.CODEMISSAOFATURAMENTO = NovoCodEmissaoFaturamento;
                    EF.STATUS = "CONCILIADA";
                    EF.CODCLIENTE = null;
                    EF.CODBORDERO = null;
                    EF.CODEMPRESA = Convert.ToInt32(vm.codigoEmpresaIncTransacao);
                    EF.CODFORMAFATURAMENTO = 2;
                    EF.ORIGEM = "PN";
                    EF.TIPOFATURAMENTO = vm.DescricaoIncTransacao;
                    EF.NROREF_NETCARD = null;
                    EF.SEUNUMERO = null;
                    EF.VALOR = Convert.ToDecimal(vm.ValorIncTransacao) * TotalParcelas;
                    EF.DATSOLICITACAO = DateTime.Now;
                    EF.DATSEGUNDODESCONTO = null;
                    EF.VLDESCONTO = 0;
                    EF.VLSEGUNDODESCONTO = 0;
                    EF.MENSAGEMARQUIVOREMESSA = "";
                    EF.MENSAGEMINSTRUCOESCAIXA = "";
                    EF.NRODOCUMENTO = null;
                    EF.VLPGTO = Convert.ToDecimal(vm.ValorIncTransacao) * TotalParcelas;
                    EF.VLDESCONTOCONCEDIDO = 0;
                    EF.JUROSCOBRADO = 0;
                    EF.MULTACOBRADA = 0;
                    EF.CODEMISSAOFATURAMENTOPAI = codigoPai;
                    EF.PARCELA = i;
                    EF.TOTALPARCELA = TotalParcelas;
                    EF.VALORPARCELA = Convert.ToDecimal(vm.ValorIncTransacao);
                    EF.CODCONTACORRENTE = Convert.ToInt32(vm.codContaCorrenteIncTransacao);

                    if (UltimoDiaMes)
                    {
                        EF.DATVENCIMENTO = dtParcela.LastDayOfMonth();
                        EF.DATPGTO = dtParcela.LastDayOfMonth();
                    }
                    else
                    {
                        EF.DATVENCIMENTO = dtParcela;
                        EF.DATPGTO = dtParcela;
                    }

                    _emissaoFaturamento.IncluiFaturamento(EF);
                    //INSERI LOG DO REGISTRO
                    _emissaoFaturamento.IncluiLog(EF, vm.codigoUsuarioIncTransacao, "Inclusão de Transação via conciliação bancária PagNet");

                    dtParcela = dtParcela.AddMonths(1);
                }

                resultado.Add(true, "Transação incluída com sucesso");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
                throw ex;
            }
            return resultado;
        }
        /// <summary>
        /// ~Inclusão de transações de pagamentos que não deverão ser incluídos no arquivo de remessa.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<IDictionary<bool, string>> IncluirNovaTransacaoSAIDA(IncluiTransacaoVM vm)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                bool UltimoDiaMes = false;
                int codigoPai = 0;
                DateTime dtParcela = Convert.ToDateTime(vm.dtIncTransacao);
                //DateTime com o último dia do mês
                DateTime ultimoDiaDoMes = dtParcela.LastDayOfMonth();

                if (ultimoDiaDoMes.Day == dtParcela.Day)
                {
                    UltimoDiaMes = true;
                }
                int TotalParcelas = (vm.ParcelaTerminoIncTransacao - vm.ParcelaInicioIncTransacao) + 1;
                for (int i = 0; i < TotalParcelas; i++)
                {
                    //CRIA UM NOVO REGISTRO NA TABELA DE EMISSÃO DE TÍTULOS
                    var NovoCodTitulo = await _emissaoTitulos.BuscaProximoCodTitulo();
                    if (codigoPai == 0)
                    {
                        codigoPai = NovoCodTitulo;
                    }

                    PAGNET_EMISSAO_TITULOS et = new PAGNET_EMISSAO_TITULOS();
                    et.CODTITULO = NovoCodTitulo;
                    et.CODTITULOPAI = codigoPai;
                    et.STATUS = "PENDENTE_COSOLIDACAO";
                    et.CODEMPRESA = Convert.ToInt32(vm.codigoEmpresaIncTransacao);
                    et.CODFAVORECIDO = null;
                    et.DATEMISSAO = DateTime.Now;
                    et.VALBRUTO = Convert.ToDecimal(vm.ValorIncTransacao);
                    et.VALLIQ = Convert.ToDecimal(vm.ValorIncTransacao);
                    et.TIPOTITULO = vm.DescricaoIncTransacao;
                    et.ORIGEM = "PN";
                    et.SISTEMA = 0;
                    et.LINHADIGITAVEL = "";
                    et.CODBORDERO = null;
                    et.VALTOTAL = Convert.ToDecimal(vm.ValorIncTransacao);
                    et.SEUNUMERO = "";
                    et.CODCONTACORRENTE = Convert.ToInt32(vm.codContaCorrenteIncTransacao);

                    if (UltimoDiaMes)
                    {
                        et.DATPGTO = dtParcela.LastDayOfMonth();
                        et.DATREALPGTO = dtParcela.LastDayOfMonth();
                    }
                    else
                    {
                        et.DATPGTO = dtParcela;
                        et.DATREALPGTO = dtParcela;
                    }

                    _emissaoTitulos.IncluiTitulo(et);
                    //INSERI LOG DO REGISTRO
                    _emissaoTitulos.IncluiLog(et, vm.codigoUsuarioIncTransacao, "Inclusão de Transação via PagNet");

                    dtParcela = dtParcela.AddMonths(1);
                }

                resultado.Add(true, "Transação incluída com sucesso");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
                throw ex;
            }
            return resultado;
        }
        public async Task<IDictionary<bool, string>> IncluirNovaTransacaoJaConciliadaSAIDA(IncluiTransacaoVM vm)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                bool UltimoDiaMes = false;
                int codigoPai = 0;
                DateTime dtParcela = Convert.ToDateTime(vm.dtIncTransacao);
                //DateTime com o último dia do mês
                DateTime ultimoDiaDoMes = dtParcela.LastDayOfMonth();

                if (ultimoDiaDoMes.Day == dtParcela.Day)
                {
                    UltimoDiaMes = true;
                }
                int TotalParcelas = (vm.ParcelaTerminoIncTransacao - vm.ParcelaInicioIncTransacao) + 1;
                for (int i = 0; i < TotalParcelas; i++)
                {
                    //CRIA UM NOVO REGISTRO NA TABELA DE EMISSÃO DE TÍTULOS
                    var NovoCodTitulo = await _emissaoTitulos.BuscaProximoCodTitulo();
                    if (codigoPai == 0)
                    {
                        codigoPai = NovoCodTitulo;
                    }

                    PAGNET_EMISSAO_TITULOS et = new PAGNET_EMISSAO_TITULOS();
                    et.CODTITULO = NovoCodTitulo;
                    et.CODTITULOPAI = codigoPai;
                    et.STATUS = "CONCILIADA";
                    et.CODEMPRESA = Convert.ToInt32(vm.codigoEmpresaIncTransacao);
                    et.CODFAVORECIDO = null;
                    et.DATEMISSAO = DateTime.Now;
                    et.VALBRUTO = Convert.ToDecimal(vm.ValorIncTransacao);
                    et.VALLIQ = Convert.ToDecimal(vm.ValorIncTransacao);
                    et.TIPOTITULO = vm.DescricaoIncTransacao;
                    et.ORIGEM = "PN";
                    et.SISTEMA = 0;
                    et.LINHADIGITAVEL = "";
                    et.CODBORDERO = null;
                    et.VALTOTAL = Convert.ToDecimal(vm.ValorIncTransacao);
                    et.SEUNUMERO = "";
                    et.CODCONTACORRENTE = Convert.ToInt32(vm.codContaCorrenteIncTransacao);

                    if (UltimoDiaMes)
                    {
                        et.DATPGTO = dtParcela.LastDayOfMonth();
                        et.DATREALPGTO = dtParcela.LastDayOfMonth();
                    }
                    else
                    {
                        et.DATPGTO = dtParcela;
                        et.DATREALPGTO = dtParcela;
                    }

                    _emissaoTitulos.IncluiTitulo(et);
                    //INSERI LOG DO REGISTRO
                    _emissaoTitulos.IncluiLog(et, vm.codigoUsuarioIncTransacao, "Inclusão de Transação via conciliação bancária PagNet");

                    dtParcela = dtParcela.AddMonths(1);
                }

                resultado.Add(true, "Transação incluída com sucesso");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
                throw ex;
            }
            return resultado;
        }
        public async Task<List<ListaTransacoesAConsolidarVm>> ListaTransacoesAConsolidar(FiltroTransacaoVM vm)
        {
            try
            {
                int Mes = Convert.ToInt32(vm.MesRef.Substring(0, 2));
                int Ano = Convert.ToInt32(vm.MesRef.Substring(vm.MesRef.Length - 4, 4));
                List<ListaTransacoesAConsolidarVm> listaRetorno = new List<ListaTransacoesAConsolidarVm>();

                var ListaTransacaoDespesa = await _emissaoTitulos.BuscaTransacaoAConsolidar(Convert.ToInt32(vm.codigoEmpresa), Convert.ToInt32(vm.codContaCorrente), Mes, Ano);
                var ListaTransacaoReceita = await _emissaoFaturamento.BuscaTransacaoAConsolidar(Convert.ToInt32(vm.codigoEmpresa), Convert.ToInt32(vm.codContaCorrente), Mes, Ano);
                ListaTransacoesAConsolidarVm tran;

                foreach (var Desp in ListaTransacaoDespesa)
                {
                    tran = new ListaTransacoesAConsolidarVm();
                    tran.itemChecado = false;
                    tran.codigo = Desp.CODTITULO;
                    tran.Descricao = Desp.TIPOTITULO;
                    tran.dataSolicitacao = Desp.DATEMISSAO.ToShortDateString();
                    tran.dataPGTO = Desp.DATREALPGTO.ToShortDateString();
                    tran.ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Desp.VALTOTAL);
                    tran.TipoTransacao = "SAIDA";
                    listaRetorno.Add(tran);
                }

                foreach (var Rec in ListaTransacaoReceita)
                {
                    tran = new ListaTransacoesAConsolidarVm();
                    tran.itemChecado = false;
                    tran.codigo = Rec.CODEMISSAOFATURAMENTO;
                    tran.Descricao = Rec.TIPOFATURAMENTO;
                    tran.dataSolicitacao = Rec.DATSOLICITACAO.ToShortDateString();
                    tran.dataPGTO = Rec.DATVENCIMENTO.ToShortDateString();
                    tran.ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Rec.VALORPARCELA);
                    tran.TipoTransacao = "ENTRADA";
                    listaRetorno.Add(tran);
                }


                return listaRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> ConsolidaTransacao(EditarTransacaoVM item)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                int codcontacorrente = 0;

                if (item.TipoTransacao == "ENTRADA")
                {
                    var ENTRADA = await _emissaoFaturamento.BuscaFaturamentoByID(Convert.ToInt32(item.codigoTransacao));
                    codcontacorrente = (int)ENTRADA.CODCONTACORRENTE;
                    ENTRADA.STATUS = "CONSOLIDADO";
                    ENTRADA.DATPGTO = DateTime.Now;
                    ENTRADA.VLPGTO = ENTRADA.VALOR;
                    _emissaoFaturamento.AtualizaFaturamento(ENTRADA);
                    _emissaoFaturamento.IncluiLog(ENTRADA, item.codigoUsuario, "Consolidação de transação");
                }
                else
                {
                    var despesa = await _emissaoTitulos.BuscaTituloByID(Convert.ToInt32(item.codigoTransacao));
                    codcontacorrente = (int)despesa.CODCONTACORRENTE;
                    despesa.STATUS = "CONSOLIDADO";
                    _emissaoTitulos.AtualizaTitulo(despesa);
                    _emissaoTitulos.IncluiLog(despesa, item.codigoUsuario, "Consolidação de transação");
                }
                //ATUALIZA SALDO DA CONTA CORRENTE
                AtualizaSaldoContaCorrente(codcontacorrente, Convert.ToInt32(item.codigoTransacao), item.TipoTransacao);


                resultado.Add(true, "Transações consolidadas com sucesso!");
            }
            catch (Exception ex)
            {
                resultado.Add(false, "Ocorreu uma falha durante o processo de consolidação. Favor contactar o suporte!");
                throw ex;
            }

            return resultado;
        }
        public async Task<IDictionary<bool, string>> CancelaTransacao(EditarTransacaoVM item)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {

                if (item.TipoTransacao == "ENTRADA")
                {
                    if (Convert.ToInt32(item.ParcelaTermino) > 1)
                    {
                        var ListaReceita = await _emissaoFaturamento.BuscaFaturamentoByIDPai(0, item.codigoTransacao);
                        ListaReceita = ListaReceita.Where(x => x.CODEMISSAOFATURAMENTO >= item.codigoTransacao).ToList();
                        foreach (var receita in ListaReceita)
                        {
                            receita.STATUS = "CANCELADO";
                            _emissaoFaturamento.AtualizaFaturamento(receita);
                            _emissaoFaturamento.IncluiLog(receita, item.codigoUsuario, "Cancelamento de Transação");
                        }
                    }
                    else
                    {
                        var receita = await _emissaoFaturamento.BuscaFaturamentoByID(Convert.ToInt32(item.codigoTransacao));
                        receita.STATUS = "CANCELADO";
                        _emissaoFaturamento.AtualizaFaturamento(receita);
                        _emissaoFaturamento.IncluiLog(receita, item.codigoUsuario, "Cancelamento de Transação");
                    }
                }
                else// DESPESAS
                {
                    if (Convert.ToInt32(item.ParcelaTermino) > 1)
                    {
                        var ListaDespesa = await _emissaoTitulos.BuscaTituloByIDPai(0, item.codigoTransacao);
                        ListaDespesa = ListaDespesa.Where(x => x.CODTITULO >= item.codigoTransacao).ToList();
                        foreach (var despesa in ListaDespesa)
                        {
                            despesa.STATUS = "CANCELADO";
                            _emissaoTitulos.AtualizaTitulo(despesa);
                            _emissaoTitulos.IncluiLog(despesa, item.codigoUsuario, "Cancelamento de Transação");
                        }
                    }
                    else
                    {
                        var despesa = await _emissaoTitulos.BuscaTituloByID(Convert.ToInt32(item.codigoTransacao));
                        despesa.STATUS = "CANCELADO";
                        _emissaoTitulos.AtualizaTitulo(despesa);
                        _emissaoTitulos.IncluiLog(despesa, item.codigoUsuario, "Cancelamento de Transação");
                    }
                }

                resultado.Add(true, "Transações cancelada com sucesso!");
            }
            catch (Exception ex)
            {
                resultado.Add(false, "Ocorreu uma falha durante o processo de cancelamento. Favor contactar o suporte!");
                throw ex;
            }

            return resultado;
        }
        public async Task<EditarTransacaoVM> ConsultaTransacao(int CodTransacao, string TipoTransacao)
        {
            try
            {
                EditarTransacaoVM transacao = new EditarTransacaoVM();

                if (TipoTransacao == "ENTRADA")
                {
                    var receita = await _emissaoFaturamento.BuscaFaturamentoByID(CodTransacao);
                    transacao = EditarTransacaoVM.ToViewReceita(receita);
                }
                else
                {
                    var despesa = await _emissaoTitulos.BuscaTituloByID(CodTransacao);
                    transacao = EditarTransacaoVM.ToViewDespesa(despesa);
                }
                if (Convert.ToInt32(transacao.codContaCorrenteTransacao) > 0)
                {
                    var cc = await _conta.GetContaCorrenteById(Convert.ToInt32(transacao.codContaCorrenteTransacao));
                    transacao.nmContaCorrenteTransacao = "Banco:" + cc.CODBANCO + " Agencia:" + cc.AGENCIA + " Conta:" + cc.NROCONTACORRENTE + "-" + cc.DIGITOCC;

                }
                return transacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> AtualizaTransacaoENTRADA(EditarTransacaoVM vm)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                DateTime dtParcela = Convert.ToDateTime(vm.dtTransacao);

                var EF = await _emissaoFaturamento.BuscaFaturamentoByID(vm.codigoTransacao);

                EF.STATUS = "PENDENTE_COSOLIDACAO";
                EF.CODEMPRESA = Convert.ToInt32(vm.codigoEmpresaTransacao);
                EF.TIPOFATURAMENTO = vm.DescricaoTransacao;
                EF.VALOR = Convert.ToDecimal(vm.ValorTransacao);
                EF.DATSOLICITACAO = DateTime.Now;
                EF.VALORPARCELA = Convert.ToDecimal(vm.ValorTransacao);
                EF.CODCONTACORRENTE = Convert.ToInt32(vm.codContaCorrenteTransacao);
                EF.DATVENCIMENTO = dtParcela;

                _emissaoFaturamento.AtualizaFaturamento(EF);
                //INSERI LOG DO REGISTRO
                _emissaoFaturamento.IncluiLog(EF, vm.codigoUsuario, "Transação atualizada");

                resultado.Add(true, "Transação atualizada com sucesso");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
                throw ex;
            }
            return resultado;
        }
        public async Task<IDictionary<bool, string>> AtualizaTransacaoSAIDA(EditarTransacaoVM vm)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {
                DateTime dtParcela = Convert.ToDateTime(vm.dtTransacao);

                var et = await _emissaoTitulos.BuscaTituloByID(vm.codigoTransacao);
                et.STATUS = "PENDENTE_COSOLIDACAO";
                et.CODEMPRESA = Convert.ToInt32(vm.codigoEmpresaTransacao);
                et.VALBRUTO = Convert.ToDecimal(vm.ValorTransacao);
                et.VALLIQ = Convert.ToDecimal(vm.ValorTransacao);
                et.TIPOTITULO = vm.DescricaoTransacao;
                et.VALTOTAL = Convert.ToDecimal(vm.ValorTransacao);
                et.CODCONTACORRENTE = Convert.ToInt32(vm.codContaCorrenteTransacao);
                et.DATPGTO = Convert.ToDateTime(vm.dtTransacao);
                et.DATREALPGTO = Convert.ToDateTime(vm.dtTransacao);

                _emissaoTitulos.AtualizaTitulo(et);
                //INSERI LOG DO REGISTRO
                _emissaoTitulos.IncluiLog(et, vm.codigoUsuario, "Transação atualizada");


                resultado.Add(true, "Transação atualizada com sucesso");
            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
                throw ex;
            }
            return resultado;
        }
        public async Task<bool> VerificaParcelasFuturas(int CodTransacao, string TipoTransacao)
        {
            try
            {
                bool qtTransacao = false;
                if (TipoTransacao == "RECEITA")
                {
                    qtTransacao = await _emissaoFaturamento.RetornaquantidadeParcelasFuturas(CodTransacao) > 0;
                }
                else
                {
                    qtTransacao = await _emissaoTitulos.RetornaquantidadeParcelasFuturas(CodTransacao) > 0;
                }
                return qtTransacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void AtualizaSaldoContaCorrente(int codcontacorrente, int codigo, string TipoTransacao)
        {
            try
            {
                decimal saldoContaCorrente = 0;
                decimal CalculoSaldoAtual = 0;

                var dadosContaCorrente = _conta.GetContaCorrenteById(codcontacorrente).Result;
                saldoContaCorrente = _conta.RetornaSaldoAtual(codcontacorrente);

                if (TipoTransacao == "RECEITA")
                {
                    var dadosFaturamento = _emissaoFaturamento.BuscaFaturamentoByID(codigo).Result;
                    var valorRecebido = (decimal)dadosFaturamento.VLPGTO;
                    CalculoSaldoAtual = saldoContaCorrente + valorRecebido;
                }
                else
                {
                    var dadosTitulo = _emissaoTitulos.BuscaTituloByID(codigo).Result;
                    var valorPago = dadosTitulo.VALTOTAL;
                    CalculoSaldoAtual = saldoContaCorrente - valorPago;
                }

                _conta.InseriNovoSaldo(codcontacorrente, dadosContaCorrente.CODEMPRESA, CalculoSaldoAtual);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TesourariaExtratoBancarioVM>> ListaExtratoBancario(FiltroExtratoBancarioVm vm)
        {
            try
            {
                List<TesourariaExtratoBancarioVM> lista = new List<TesourariaExtratoBancarioVM>();
                int codEmpresa = Convert.ToInt32(vm.codigoEmpresa);
                int codContaCorrente = Convert.ToInt32(vm.codContaCorrente);
                DateTime dataInicio = Convert.ToDateTime(vm.dtInicio);
                DateTime dataFim = Convert.ToDateTime(vm.dtFim);


                var RetornoDados = await _proc.Listar_EXTRATO_BANCARIO(codEmpresa, codContaCorrente, dataInicio, dataFim);

                lista = RetornoDados.Select(x => new TesourariaExtratoBancarioVM(x)).ToList();

                decimal SaldoAnteior = 0;
                decimal SaldoTransacao = 0;

                for (int i = 0; i < lista.Count; i++)
                {
                    if (SaldoAnteior == 0) SaldoAnteior = lista[i].SaldoAnterior;

                    SaldoTransacao = lista[i].ValorTransacao + SaldoAnteior;
                    lista[i].Saldo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoTransacao).Replace("R$ ", "");

                    SaldoAnteior = SaldoTransacao;
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TesourariaMaioresDespesasVM>> ListaMaioresSAIDAs(FiltroExtratoBancarioVm vm)
        {
            try
            {
                List<TesourariaMaioresDespesasVM> lista = new List<TesourariaMaioresDespesasVM>();
                int codEmpresa = Convert.ToInt32(vm.codigoEmpresa);
                int codContaCorrente = Convert.ToInt32(vm.codContaCorrente);
                DateTime dataInicio = Convert.ToDateTime(vm.dtInicio);
                DateTime dataFim = Convert.ToDateTime(vm.dtFim);

                var RetornoDados = await _proc.Listar_MAIORES_DESPESAS(codEmpresa, codContaCorrente, dataInicio, dataFim);

                lista = RetornoDados.Select(x => new TesourariaMaioresDespesasVM(x)).ToList();

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<TesourariaMaioresReceitasVM>> ListaMaioresENTRADAs(FiltroExtratoBancarioVm vm)
        {
            try
            {
                List<TesourariaMaioresReceitasVM> lista = new List<TesourariaMaioresReceitasVM>();
                int codEmpresa = Convert.ToInt32(vm.codigoEmpresa);
                int codContaCorrente = Convert.ToInt32(vm.codContaCorrente);
                DateTime dataInicio = Convert.ToDateTime(vm.dtInicio);
                DateTime dataFim = Convert.ToDateTime(vm.dtFim);

                var RetornoDados = await _proc.Listar_MAIORES_RECEITAS(codEmpresa, codContaCorrente, dataInicio, dataFim);

                lista = RetornoDados.Select(x => new TesourariaMaioresReceitasVM(x)).ToList();

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<TesourariaInformacaoCCVM> BuscaSaldoContaCorrente(FiltroExtratoBancarioVm vm)
        {
            try
            {
                TesourariaInformacaoCCVM Retorno = new TesourariaInformacaoCCVM();
                int codEmpresa = Convert.ToInt32(vm.codigoEmpresa);
                int codContaCorrente = Convert.ToInt32(vm.codContaCorrente);
                DateTime dataInicio = Convert.ToDateTime(vm.dtInicio);
                DateTime dataFim = Convert.ToDateTime(vm.dtFim);

                var RetornoDados = await _proc.Consultar_INFO_CONTA_CORRENTE(codEmpresa, codContaCorrente, dataInicio, dataFim);

                Retorno = TesourariaInformacaoCCVM.ToView(RetornoDados);

                return Retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<FiltroTransacaoVM> CarregaDadosInicio(int codEmpresa)
        {
            try
            {
                var dadosEmpresa = await _empresa.ConsultaEmpresaById(codEmpresa);
                var cc = _conta.GetAllContaCorrente(codEmpresa).FirstOrDefault();

                FiltroTransacaoVM model = new FiltroTransacaoVM();
                if (cc != null)
                {
                    model.codigoEmpresa = codEmpresa.ToString();
                    model.nomeEmpresa = dadosEmpresa.NMFANTASIA;
                    model.MesRef = DateTime.Now.ToString("MM/yyyy");
                    model.codContaCorrente = cc.CODCONTACORRENTE.ToString();
                    model.nmContaCorrente = "Banco:" + cc.CODBANCO + " Agencia:" + cc.AGENCIA + " Conta:" + cc.NROCONTACORRENTE + "-" + cc.DIGITOCC;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ListaConciliacaoVM>> ProcessaArquivoConciliacaoBancaria(FiltroConciliacaoBancariaVm model)
        {
            try
            {
                int TotalRegistroConciliados = 0;
                int TotalRegistrosPendentes = 0;
                int TotalRegistrosNaoEncontrados = 0;

                decimal TotalCredito = 0;
                decimal TotalDebito = 0;
                decimal SaldoAnterior = _conta.RetornaSaldoAtual(Convert.ToInt32(model.codContaCorrente));
                decimal SaldoAtual = 0;
                string contacorrente = "";
                string CodigoBanco = "";
                //decimal SaldoFinal = 0;

                var dados = ImportOfx.toXElement(model.caminhoArquivo);

                string[] lines = File.ReadAllLines(model.caminhoArquivo);

                int counter = 0;
                string line;

                StreamReader file = new StreamReader(model.caminhoArquivo);

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("BALAMT"))
                    {
                        SaldoAtual = TrataValor(ImportOfx.getTagValue(line));                        
                    }
                    if (line.Contains("ACCTID"))
                    {
                        contacorrente = ImportOfx.getTagValue(line);
                    }
                    if (line.Contains("BANKID"))
                    {
                        CodigoBanco = ImportOfx.getTagValue(line);
                    }
                    counter++;
                }
                file.Close();

                var dadosConta = await _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente));
                if (Convert.ToInt32(dadosConta.CODBANCO) != Convert.ToInt32(CodigoBanco))
                {
                    throw new Exception("Banco do arquivo não coincide com o banco da conta corrente selecionada.");
                }
                //if (contacorrente.IndexOf('-') > -1)
                //{
                //    contacorrente = contacorrente.Substring(0, contacorrente.IndexOf('-'));
                //}
                //if(contacorrente != dadosConta.NROCONTACORRENTE)
                //{
                //    throw new Exception("Conta Corrente do arquivo não coincide com a conta corrente selecionada.");
                //}

                var DadosArquivo = (from c in dados.Descendants("STMTTRN")
                                    select new DadosArquivoOFXVM
                                    {
                                        ValorOFX = TrataValor(c.Element("TRNAMT").Value.Replace("-", "").ToString()),
                                        DataOFX = DateTime.ParseExact(c.Element("DTPOSTED").Value.Substring(0,8),
                                                                   "yyyyMMdd", null),
                                        DescricaoOFX = c.Element("MEMO").Value,
                                        CodigoTransacaoOFX = c.Element("CHECKNUM").Value,
                                        TipoOFX = c.Element("TRNTYPE").Value
                                    }).ToList();


                TotalDebito = DadosArquivo.Where(w => w.TipoOFX == "DEBIT").Select(x => x.ValorOFX).Sum();
                TotalCredito = DadosArquivo.Where(w => w.TipoOFX == "CREDIT").Select(x => x.ValorOFX).Sum();
                //SaldoFinal = (SaldoAnterior + TotalCredito) - TotalDebito;

                var dataInicial = DadosArquivo.Select(x => x.DataOFX).Min();
                var dataFinal = DadosArquivo.Select(x => x.DataOFX).Max();

                var AgruparData =
                        from d in DadosArquivo
                        group d by d.DataOFX into newGroup
                        orderby newGroup.Key
                        select newGroup;

                List<ListaConciliacaoVM> listaConciliacao = new List<ListaConciliacaoVM>();
                ListaConciliacaoVM transacao = new ListaConciliacaoVM();

                foreach (var dataBase in AgruparData)
                {
                    var ListaTransacoesByData = DadosArquivo.Where(x => x.DataOFX == dataBase.Key).ToList();
                    var ListaTitulos = await _emissaoTitulos.BuscaTitulosByData(dataBase.Key, Convert.ToInt32(model.codContaCorrente));
                    var ListaTitulosPagos = await _titulosPagos.BuscaTitulosByData(dataBase.Key, Convert.ToInt32(model.codContaCorrente));
                    var ListaFaturamentos = await _emissaoFaturamento.BuscaFaturamentosByData(dataBase.Key, Convert.ToInt32(model.codContaCorrente));
                                       
                    foreach (var item in ListaTransacoesByData)
                    {

                        var ValorTransacao = item.ValorOFX;

                        transacao = new ListaConciliacaoVM();
                        transacao.DataConciliacao = item.DataOFX.ToShortDateString();
                        transacao.DescricaoConciliacao = item.DescricaoOFX;

                        if (item.DescricaoOFX.Length > 40)
                            transacao.DescricaoAbreviadaConciliacao = item.DescricaoOFX.Substring(0, 40);
                        else
                            transacao.DescricaoAbreviadaConciliacao = item.DescricaoOFX;

                        transacao.TipoConciliacao = (item.TipoOFX == "CREDIT") ? "Entrada" : "Saída";
                        transacao.ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", ValorTransacao);
                        transacao.TotalCredito = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalCredito);
                        transacao.TotalDebito = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalDebito);
                        transacao.SaldoAnterior = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAnterior);
                        transacao.SaldoFinal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAtual);
                        transacao.TransacaoEncontrada = "N";
                        transacao.StatusPN = "TRANSAÇÃO NÃO ENCONTRADA";

                        if (item.TipoOFX == "CREDIT")
                        {
                            if (ListaFaturamentos.Count > 0)
                            {
                                var Fatura = ListaFaturamentos.Where(x => x.VALOR == ValorTransacao).FirstOrDefault();

                                if (Fatura != null)
                                {
                                    transacao.CodigoTransacaoPN = Fatura.CODEMISSAOFATURAMENTO.ToString();
                                    transacao.ValorPN = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Fatura.VLPGTO);
                                    transacao.TransacaoEncontrada = "S";
                                    if (Fatura.STATUS != "CONCILIADA")
                                    {
                                        ConciliarTransacaoEntrada(Fatura, model.codigoUsuario);
                                    }
                                    transacao.StatusPN = "CONCILIADA";
                                    TotalRegistroConciliados += 1;                         
                                }
                            }
                        }
                        else
                        {
                            //verifica primeiro na tabela de títulos pagos, pois no retorno do banco eu vou ter o valor que está nesta tabela. casos 
                            //em que possui mais de um título em apenas uma transação bancária.
                            if (ListaTitulosPagos.Count > 0)
                            {
                                var ttPago = ListaTitulosPagos.Where(x => x.VALOR == ValorTransacao).FirstOrDefault();

                                if (ttPago != null)
                                {
                                    transacao.CodigoTransacaoPN = ttPago.CODTITULOPAGO.ToString();
                                    transacao.ValorPN = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", ttPago.VALOR);
                                    transacao.TransacaoEncontrada = "S";
                                    if (ttPago.STATUS != "CONCILIADA")
                                    {
                                        ConciliarTransacaoSaida(ttPago, model.codigoUsuario);
                                    }

                                    transacao.StatusPN = "CONCILIADA";
                                    TotalRegistroConciliados += 1;
                                }
                                else
                                {
                                    //irá entrar neste caso quando for transação que não geraram arquivo de remessa.
                                    var Titulo = ListaTitulos.Where(x => x.VALTOTAL == ValorTransacao).FirstOrDefault();

                                    if (Titulo != null)
                                    {
                                        transacao.CodigoTransacaoPN = Titulo.CODTITULO.ToString();
                                        transacao.ValorPN = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Titulo.VALTOTAL);
                                        transacao.TransacaoEncontrada = "S";
                                        if (Titulo.STATUS != "CONCILIADA")
                                        {
                                            ConciliarTransacaoSaida(Titulo, model.codigoUsuario);
                                        }

                                        transacao.StatusPN = "CONCILIADA";
                                        TotalRegistroConciliados += 1;
                                    }
                                }

                            }
                            else if (ListaTitulos.Count > 0)
                            {
                                /*Irá entrar aqui apenas se a transação não foi gerada via arquivo de remessa.*/
                                var Titulo = ListaTitulos.Where(x => x.VALTOTAL == ValorTransacao).FirstOrDefault();
                                if (Titulo != null)
                                {
                                    transacao.CodigoTransacaoPN = Titulo.CODTITULO.ToString();
                                    transacao.ValorPN = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Titulo.VALTOTAL);
                                    transacao.TransacaoEncontrada = "S";
                                    if (Titulo.STATUS != "CONCILIADA")
                                    {
                                        ConciliarTransacaoSaida(Titulo, model.codigoUsuario);
                                    }

                                    transacao.StatusPN = "CONCILIADA";
                                    TotalRegistroConciliados += 1;                                 
                                }
                            }
                        }
                        if (transacao.TransacaoEncontrada == "N")
                        {
                            TotalRegistrosNaoEncontrados += 1;
                        }
                        listaConciliacao.Add(transacao);    
                    }
                }

                /*Realiza a busca no sistema para verificar se existe alguma transação que não consta no arquivo de conciliação*/
                var listaTitulosNaoConciliados = await _emissaoTitulos.BuscaTitulosNaoConciliados(Convert.ToInt32(model.codigoEmpresa), 
                                                                Convert.ToInt32(model.codContaCorrente), dataInicial, dataFinal);
                var listaFaturamentosNaoConciliados = await _emissaoFaturamento.BuscaFaturamentosNaoConciliados(Convert.ToInt32(model.codigoEmpresa),
                                                                Convert.ToInt32(model.codContaCorrente), dataInicial, dataFinal);
                //Lista de Titulos não encontrados no arquivo de conciliação
                if (listaTitulosNaoConciliados != null)
                {
                    foreach(var tit in listaTitulosNaoConciliados)
                    {
                        var ValorTransacao = tit.VALTOTAL;

                        transacao = new ListaConciliacaoVM();
                        transacao.DataConciliacao = tit.DATREALPGTO.ToShortDateString();
                        transacao.DescricaoConciliacao = (tit.PAGNET_CADFAVORECIDO == null) ? tit.TIPOTITULO : tit.PAGNET_CADFAVORECIDO.NMFAVORECIDO;

                        if (tit.TIPOTITULO.Length > 40)
                        {
                            if (tit.PAGNET_CADFAVORECIDO == null)
                                transacao.DescricaoAbreviadaConciliacao = tit.TIPOTITULO.Substring(0, 40);
                            else
                                transacao.DescricaoAbreviadaConciliacao = tit.PAGNET_CADFAVORECIDO.NMFAVORECIDO.Substring(0, 40);
                        }
                        else
                            transacao.DescricaoAbreviadaConciliacao = (tit.PAGNET_CADFAVORECIDO == null) ? tit.TIPOTITULO : tit.PAGNET_CADFAVORECIDO.NMFAVORECIDO;

                        transacao.TipoConciliacao = "Saída";
                        transacao.ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", ValorTransacao);
                        transacao.TotalCredito = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalCredito);
                        transacao.TotalDebito = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalDebito);
                        transacao.SaldoAnterior = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAnterior);
                        transacao.SaldoFinal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAtual);
                        transacao.TransacaoEncontrada = "T";
                        transacao.StatusPN = "TÍTULO NÃO ENCONTRADA NO ARQUIVO DE CONCILIAÇÃO";
                        TotalRegistrosPendentes += 1;

                        listaConciliacao.Add(transacao);
                    }
                }
                //Lista de pedidos de faturamentos não encontrados no arquivo de conciliação
                if (listaFaturamentosNaoConciliados != null)
                {
                    foreach (var tit in listaFaturamentosNaoConciliados)
                    {
                        var ValorTransacao = tit.VALORPARCELA;

                        transacao = new ListaConciliacaoVM();
                        transacao.DataConciliacao = tit.DATVENCIMENTO.ToShortDateString();
                        transacao.DescricaoConciliacao = (tit.PAGNET_CADCLIENTE == null) ? tit.TIPOFATURAMENTO : tit.PAGNET_CADCLIENTE.NMCLIENTE;

                        if (tit.TIPOFATURAMENTO.Length > 40)
                        {
                            if (tit.PAGNET_CADCLIENTE == null)
                                transacao.DescricaoAbreviadaConciliacao = tit.TIPOFATURAMENTO.Substring(0, 40);
                            else
                                transacao.DescricaoAbreviadaConciliacao = tit.PAGNET_CADCLIENTE.NMCLIENTE.Substring(0, 40);
                        }
                        else
                            transacao.DescricaoAbreviadaConciliacao = (tit.PAGNET_CADCLIENTE == null) ? tit.TIPOFATURAMENTO : tit.PAGNET_CADCLIENTE.NMCLIENTE;

                        transacao.TipoConciliacao = "Entrada";
                        transacao.ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", ValorTransacao);
                        transacao.TotalCredito = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalCredito);
                        transacao.TotalDebito = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", TotalDebito);
                        transacao.SaldoAnterior = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAnterior);
                        transacao.SaldoFinal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAtual);
                        transacao.TransacaoEncontrada = "T";
                        transacao.StatusPN = "FATURA NÃO ENCONTRADA NO ARQUIVO DE CONCILIAÇÃO";
                        TotalRegistrosPendentes += 1;

                        listaConciliacao.Add(transacao);
                    }
                }

                for (int i = 0; i < listaConciliacao.Count; i++)
                {
                    listaConciliacao[i].TotalRegistroConciliados = TotalRegistroConciliados;
                    listaConciliacao[i].TotalRegistrosNaoEncontrados = TotalRegistrosNaoEncontrados;
                    listaConciliacao[i].TotalRegistrosPendentes = TotalRegistrosPendentes;
                    listaConciliacao[i].TotalRegistros = TotalRegistroConciliados + TotalRegistrosNaoEncontrados + TotalRegistrosPendentes;
                }


                return listaConciliacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static decimal TrataValor(string valor)
        {
            valor = Geral.RemoveCaracteres(valor);
            string ValFinal = "";
            if (valor.Length >= 3)
            {
                string Val1 = valor.Substring(0, valor.Length - 2);
                string Val2 = valor.Substring(valor.Length - 2, 2);
                ValFinal = Val1 + "," + Val2;
            }
            else if(valor.Length == 2)
            {
                string Val2 = valor.Substring(0, 2);
                ValFinal = "0," + Val2;
            }
            else
            {
                ValFinal = "0,0" + valor;
            }

            return Convert.ToDecimal(ValFinal);
        }
        private void ConciliarTransacaoSaida(PAGNET_EMISSAO_TITULOS titulo, int codusu)
        {
            try
            {
                titulo.STATUS = "CONCILIADA";

                _emissaoTitulos.AtualizaTitulo(titulo);
                _emissaoTitulos.IncluiLog(titulo, codusu, "Título conciliado através de arquivo via PagNet");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void ConciliarTransacaoSaida(PAGNET_TITULOS_PAGOS ttPago, int codusu)
        {
            try
            {
                ttPago.STATUS = "CONCILIADA";

                _titulosPagos.AtualizaTransacao(ttPago);
                var ListaTitulos = _emissaoTitulos.BuscaTituloBySeuNumero(ttPago.SEUNUMERO).Result;

                foreach (var titulo in ListaTitulos)
                {
                    titulo.STATUS = "CONCILIADA";

                    _emissaoTitulos.AtualizaTitulo(titulo);
                    _emissaoTitulos.IncluiLog(titulo, codusu, "Título conciliado através de arquivo via PagNet");
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void ConciliarTransacaoEntrada(PAGNET_EMISSAOFATURAMENTO fatura, int codusu)
        {
            try
            {
                if (fatura.STATUS == "BAIXADO" || fatura.STATUS == "BAIXADO_MANUALMENTE")
                    fatura.STATUS = "CONCILIADA";
                else
                {
                    fatura.STATUS = "CONCILIADA";
                    fatura.VLPGTO = fatura.VALORPARCELA;
                    fatura.DATPGTO = fatura.DATVENCIMENTO;
                }

                _emissaoFaturamento.AtualizaFaturamento(fatura);
                _emissaoFaturamento.IncluiLog(fatura, codusu, "Título conciliado através de arquivo via PagNet");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
