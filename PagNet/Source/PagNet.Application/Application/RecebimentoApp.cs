using Boleto2Net;
using Newtonsoft.Json;
using PagNet.Application.Application.Common;
using PagNet.Application.Cnab.Comun;
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
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class RecebimentoApp : IRecebimentoApp
    {
        private IBanco _banco;

        private readonly IPagNet_CadClienteService _cliente;
        private readonly IPagNet_InstrucaoCobrancaService _instrucaoCobranca;
        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_EmissaoBoletoService _emissaoBoleto;
        private readonly IPagNet_CadEmpresaService _empresa;
        private readonly IOperadoraService _ope;
        private readonly IPagNet_Bordero_BoletoService _bordero;
        private readonly IPagNet_OcorrenciaRetBolService _ocorrenciaBol;
        private readonly IProceduresService _proc;
        private readonly IPagNet_Config_RegraService _regraBol;
        private readonly IPagNet_Formas_FaturamentoService _formaFaturamento;
        private readonly IPAGNET_ARQUIVO_DESCONTOFOLHAService _arquivoConciliacao;



        public RecebimentoApp(IPagNet_InstrucaoCobrancaService instrucaoCobranca,
                                IPagNet_CadClienteService cliente,
                                IPagNet_ContaCorrenteService conta,
                                IPagNet_ArquivoService arquivo,
                                IOperadoraService ope,
                                IPagNet_EmissaoBoletoService emissaoBoleto,
                                IPagNet_CadEmpresaService empresa,
                                IPagNet_OcorrenciaRetBolService ocorrenciaBol,
                                IProceduresService proc,
                                IPagNet_Config_RegraService regraBol,
                                IPagNet_Formas_FaturamentoService formaFaturamento,
                                IPagNet_Bordero_BoletoService bordero,
                                IPAGNET_ARQUIVO_DESCONTOFOLHAService arquivoConciliacao)
        {
            _cliente = cliente;
            _instrucaoCobranca = instrucaoCobranca;
            _conta = conta;
            _arquivo = arquivo;
            _emissaoBoleto = emissaoBoleto;
            _empresa = empresa;
            _bordero = bordero;
            _ope = ope;
            _ocorrenciaBol = ocorrenciaBol;
            _proc = proc;
            _regraBol = regraBol;
            _formaFaturamento = formaFaturamento;
            _arquivoConciliacao = arquivoConciliacao;
        }


        public async Task<List<ConsultaClienteVM>> ConsultaClientes()
        {
            try
            {
                List<ConsultaClienteVM> retorno;

                var DadosCli = await _cliente.BuscaAllCliente();

                retorno = DadosCli
                .Select(x => new ConsultaClienteVM(x)).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int InsereNovoArquivo(BorderoBolVM model, decimal vlArquivo, int TotalRegistros)
        {
            try
            {

                MetodosGeraisdd _MetodosGeraisdd = new MetodosGeraisdd(_arquivo);

                string nmArquivoRemessa = "BOL_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".rem";

                var caminhoPadrao = _ope.GetOperadoraById(model.codOpe).Result;

                var CaminhoRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, model.codigoEmpresa, "ArquivoRemessaBoleto");


                if (!Directory.Exists(CaminhoRemessa))
                    Directory.CreateDirectory(CaminhoRemessa);


                int nroSeqArquivo = RetornaNroSeqArquivo();

                var contacorrete = _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente)).Result;

                //INSERI DADOS DO ARQUIVO PARA ESTE BOLETO
                var Status = "PENDENTE_REGISTRO";
                var TipArqu = "BOL";

                int codArquivo = _MetodosGeraisdd.InseriArquivo(CaminhoRemessa, nmArquivoRemessa, nroSeqArquivo,
                                            vlArquivo, TotalRegistros, Status, TipArqu, contacorrete.CODBANCO);

                return codArquivo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Processo para alterar o código do arquivo para ser incluído em um novo arquivo de remessa.
        /// </summary>
        private void IncluiBoletosEditadosNoArquivoRemessa(List<PAGNET_EMISSAOFATURAMENTO> listaBoletos, int codArquivo, int codUsuario)
        {
            int codbordero = 0;
            foreach (var itemBol in listaBoletos)
            {
                var Boleto = _emissaoBoleto.BuscaBoletoBySeuNumero(itemBol.SEUNUMERO.Trim()).Result;
                if (Boleto != null)
                {
                    Boleto.Status = "PENDENTE_REGISTRO";
                    Boleto.CODARQUIVO = codArquivo;
                    Boleto.BOLETOENVIADO = "N";

                    _emissaoBoleto.AtualizaBoleto(Boleto);
                    var Faturamento = _emissaoBoleto.BuscaFaturamentoBySeuNumero(itemBol.SEUNUMERO).Result;
                    foreach (var fatura in Faturamento)
                    {
                        fatura.STATUS = Boleto.Status;

                        _emissaoBoleto.AtualizaFaturamento(fatura);
                        _emissaoBoleto.IncluiLog(fatura, codUsuario, "Faturamento incluíno no Arquivo de Remessa.");

                        if (codbordero != fatura.CODBORDERO)
                        {
                            codbordero = (int)fatura.CODBORDERO;
                            _bordero.AtualizaStatusBordero(codbordero, Boleto.Status);
                        }
                    }
                }
            }

        }
        private int IncluiDadosEmissaoBoleto(BorderoBolVM model)
        {
            try
            {
                bool AgruparFaturamentosDia = false;
                int codbordero = 0;
                decimal ValorBoleto = 0;
                List<PAGNET_EMISSAOFATURAMENTO> ListaFaturamentos = new List<PAGNET_EMISSAOFATURAMENTO>();
                //AGRUPA TODOS OS FATURAMENTOS DOS BORDEROS PARA REALIZAR OS TRATAMENTOS
                foreach (var bordero in model.ListaBordero)
                {
                    ListaFaturamentos.AddRange(_emissaoBoleto.BuscaTodosFaturamentoByCodBordero(bordero.CODBORDERO).Result);
                }

                //separa a lista de faturamento dos itens que já foram incluido no arquivo de remessa anteriormente.
                var listaBoletos = ListaFaturamentos.Where(x => x.SEUNUMERO != null).ToList();

                //Remove os itens que serão incluidos em um arquivo de remessa pela primeira vez.
                ListaFaturamentos.RemoveAll(x => x.SEUNUMERO != null);

                //Pega o valor total que será incluído no arquivo
                var ValorTotalArquivo = ListaFaturamentos.Sum(x => x.VALOR);
                ValorTotalArquivo += listaBoletos.Sum(x => x.VALOR);

                int codArquivo = InsereNovoArquivo(model, ValorTotalArquivo, ListaFaturamentos.Count);

                IncluiBoletosEditadosNoArquivoRemessa(listaBoletos, codArquivo, model.codUsuario);

                //SEPARA POR CLIENTES PARA VERIFICAR AS REGRAS E INSERIR NO BD;
                var listaClientes = ListaFaturamentos.Select(x => x.CODCLIENTE).Distinct();
                foreach (var codCliente in listaClientes)
                {
                    var dadosCli = _cliente.BuscaClienteByID(Convert.ToInt32(codCliente)).Result;

                    //Se for para agrupar todos os faturamentos do cliente com vencimento para o mesmo dia
                    if (dadosCli.COBRANCADIFERENCIADA == "N")
                    {
                        var RegraDefault = _regraBol.BuscaRegraAtivaBol(Convert.ToInt32(model.codigoEmpresa)).Result;
                        if (RegraDefault != null)
                        {
                            AgruparFaturamentosDia = (RegraDefault.AGRUPARFATURAMENTOSDIA == "S");
                            ValorBoleto = RegraDefault.TAXAEMISSAOBOLETO;
                        }
                    }
                    else
                    {
                        AgruparFaturamentosDia = (dadosCli.AGRUPARFATURAMENTOSDIA == "S");
                        ValorBoleto = Convert.ToDecimal(dadosCli.TAXAEMISSAOBOLETO);
                    }

                    if (AgruparFaturamentosDia)
                    {
                        var NewlistaFaturamento = (from reg in ListaFaturamentos.Where(x => x.CODCLIENTE == codCliente)
                                                   group reg by new
                                                   {
                                                       reg.DATVENCIMENTO
                                                   } into g
                                                   select new
                                                   {
                                                       g.Key.DATVENCIMENTO,
                                                       ValorBol = g.Sum(s => s.VALOR),
                                                       VLDESCONTOCONCEDIDO = g.Sum(s => s.VLDESCONTOCONCEDIDO),
                                                       JUROSCOBRADO = g.Sum(s => s.JUROSCOBRADO),
                                                       MULTACOBRADA = g.Sum(s => s.MULTACOBRADA)
                                                   }).ToList();

                        foreach (var Faturamento in NewlistaFaturamento)
                        {
                            PAGNET_EMISSAOBOLETO bol = new PAGNET_EMISSAOBOLETO();
                            var codEmissaoBoleto = _emissaoBoleto.BuscaNovoIDEmissaoBoleto();

                            var ValorTotalBoleto = (Faturamento.ValorBol + ValorBoleto + Convert.ToDecimal(Faturamento.JUROSCOBRADO) + Convert.ToDecimal(Faturamento.MULTACOBRADA)) - Convert.ToDecimal(Faturamento.VLDESCONTOCONCEDIDO);

                            bol.codEmissaoBoleto = codEmissaoBoleto;
                            bol.Status = "PENDENTE_REGISTRO";
                            bol.CodCliente = Convert.ToInt32(codCliente);
                            bol.codEmpresa = Convert.ToInt32(model.codigoEmpresa);
                            bol.dtVencimento = Faturamento.DATVENCIMENTO;
                            bol.codContaCorrente = Convert.ToInt32(model.codContaCorrente);
                            bol.DATPGTO = null;
                            bol.VLPGTO = null;
                            bol.NossoNumero = Geral.GeraNossoNumero("", codEmissaoBoleto);
                            bol.codOcorrencia = 1;
                            bol.SeuNumero = Geral.GeraNumeroControle(codEmissaoBoleto, "");
                            bol.Valor = ValorTotalBoleto;
                            bol.dtSolicitacao = DateTime.Now;
                            bol.dtReferencia = DateTime.Now;
                            bol.dtSegundoDesconto = null;
                            bol.vlDesconto = null;
                            bol.vlSegundoDesconto = null;
                            bol.MensagemArquivoRemessa = "";
                            bol.MensagemInstrucoesCaixa = "";
                            bol.numControle = Geral.GeraNumeroControle(codEmissaoBoleto, "");
                            bol.OcorrenciaRetBol = "";
                            bol.DescricaoOcorrenciaRetBol = "";
                            bol.nmBoletoGerado = "Boleto_Cliente_" + codEmissaoBoleto;
                            bol.CODARQUIVO = codArquivo;
                            bol.BOLETOENVIADO = "N";

                            _emissaoBoleto.IncluiBoleto(bol);

                            //Atualiza os Faturamentos
                            var Listafaturas = ListaFaturamentos.Where(x => x.CODCLIENTE == codCliente && x.DATVENCIMENTO == bol.dtVencimento).ToList();
                            foreach (var fatura in Listafaturas)
                            {
                                fatura.SEUNUMERO = bol.SeuNumero;
                                fatura.STATUS = bol.Status;

                                _emissaoBoleto.AtualizaFaturamento(fatura);
                                _emissaoBoleto.IncluiLog(fatura, model.codUsuario, "Faturamento incluíno no Arquivo de Remessa.");

                                if (codbordero != fatura.CODBORDERO)
                                {
                                    codbordero = (int)fatura.CODBORDERO;
                                    _bordero.AtualizaStatusBordero(codbordero, bol.Status);
                                }
                            }

                        }

                    }
                    else
                    {

                        foreach (var Faturamento in ListaFaturamentos.Where(x => x.CODCLIENTE == codCliente))
                        {
                            PAGNET_EMISSAOBOLETO bol = new PAGNET_EMISSAOBOLETO();
                            var codEmissaoBoleto = _emissaoBoleto.BuscaNovoIDEmissaoBoleto();

                            var ValorTotalBoleto = (Faturamento.VALOR + ValorBoleto + Convert.ToDecimal(Faturamento.JUROSCOBRADO) + Convert.ToDecimal(Faturamento.MULTACOBRADA)) - Convert.ToDecimal(Faturamento.VLDESCONTOCONCEDIDO);

                            bol.codEmissaoBoleto = codEmissaoBoleto;
                            bol.Status = "PENDENTE_REGISTRO";
                            bol.CodCliente = Convert.ToInt32(codCliente);
                            bol.codEmpresa = Convert.ToInt32(model.codigoEmpresa);
                            bol.dtVencimento = Faturamento.DATVENCIMENTO;
                            bol.codContaCorrente = Convert.ToInt32(model.codContaCorrente);
                            bol.DATPGTO = null;
                            bol.VLPGTO = null;
                            bol.NossoNumero = Geral.GeraNossoNumero("", codEmissaoBoleto);
                            bol.codOcorrencia = 1;
                            bol.SeuNumero = Geral.GeraNumeroControle(codEmissaoBoleto, Faturamento.SEUNUMERO);
                            bol.Valor = ValorTotalBoleto;
                            bol.dtSolicitacao = DateTime.Now;
                            bol.dtReferencia = DateTime.Now;
                            bol.dtSegundoDesconto = null;
                            bol.vlDesconto = null;
                            bol.vlSegundoDesconto = null;
                            bol.MensagemArquivoRemessa = "";
                            bol.MensagemInstrucoesCaixa = Faturamento.MENSAGEMINSTRUCOESCAIXA;
                            bol.numControle = Geral.GeraNumeroControle(codEmissaoBoleto, "");
                            bol.OcorrenciaRetBol = "";
                            bol.DescricaoOcorrenciaRetBol = "";
                            bol.nmBoletoGerado = "Boleto_Cliente_" + codEmissaoBoleto;
                            bol.CODARQUIVO = codArquivo;
                            bol.BOLETOENVIADO = "N";

                            _emissaoBoleto.IncluiBoleto(bol);

                            //Atualiza os Faturamentos
                            Faturamento.SEUNUMERO = bol.SeuNumero;
                            Faturamento.STATUS = bol.Status;

                            _emissaoBoleto.AtualizaFaturamento(Faturamento);
                            _emissaoBoleto.IncluiLog(Faturamento, model.codUsuario, "Faturamento incluíno no Arquivo de Remessa.");

                            if (codbordero != Faturamento.CODBORDERO)
                            {
                                codbordero = (int)Faturamento.CODBORDERO;
                                _bordero.AtualizaStatusBordero(codbordero, bol.Status);
                            }

                        }
                    }
                }
                return codArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GeraArquivoRemessaAsync(BorderoBolVM model)
        {
            try
            {
                string nmArquivoRemessa = "";
                var codArquivo = IncluiDadosEmissaoBoleto(model);
                int codigoPriemriaInstrucaoCobranca = 0;
                int codigoSegundaInstrucaoCobranca = 0;
                decimal PercMulta = 0;
                decimal VLMultaDiaAtraso = 0;
                decimal PercJuros = 0;
                decimal VLJurosDiaAtraso = 0;

                var caminhoPadrao = _ope.GetOperadoraById(model.codOpe).Result;
                var pathBoleto = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, model.codigoEmpresa, "PDFBoleto");
                var CaminhoRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, model.codigoEmpresa, "ArquivoRemessaBoleto");


                if (!Directory.Exists(pathBoleto))
                    Directory.CreateDirectory(pathBoleto);


                string Retorno = "";

                var DadosBoleto = await _emissaoBoleto.BuscaTodosBoletoByCodArquivo(codArquivo);


                var contacorrete = await _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente));
                var dadosEmpresa = await _empresa.ConsultaEmpresaById(contacorrete.CODEMPRESA);

                if (DadosBoleto.Count > 0)
                {
                    var contaBancaria = new ContaBancaria
                    {
                        Agencia = contacorrete.AGENCIA,
                        DigitoAgencia = contacorrete.DIGITOAGENCIA,
                        Conta = contacorrete.NROCONTACORRENTE,
                        DigitoConta = contacorrete.DIGITOCC,
                        TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                        TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                        TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
                    };
                    if (contacorrete.CODBANCO == "001")
                    {
                        contaBancaria.VariacaoCarteiraPadrao = "019";
                    }
                    _banco = Banco.Instancia(Convert.ToInt32(contacorrete.CODBANCO));
                    _banco.Cedente = GerarCedente(contacorrete, dadosEmpresa, contaBancaria);
                    List<ListaEmissaoBoletoVM> listaEmissaoBoleto = new List<ListaEmissaoBoletoVM>();

                    MetodosGeraisdd _MetodosGeraisdd = new MetodosGeraisdd(_arquivo);

                    foreach (var bol in DadosBoleto)
                    {
                        ListaEmissaoBoletoVM dados = new ListaEmissaoBoletoVM();


                        var cli = await _cliente.BuscaClienteByID(bol.CodCliente);
                        if (cli.COBRANCADIFERENCIADA == "N")
                        {
                            var regraGeracaoBoleto = _regraBol.BuscaRegraAtivaBol(cli.CODEMPRESA).Result;
                            if (regraGeracaoBoleto != null)
                            {
                                codigoPriemriaInstrucaoCobranca = regraGeracaoBoleto.CODPRIMEIRAINSTCOBRA;
                                codigoSegundaInstrucaoCobranca = regraGeracaoBoleto.CODSEGUNDAINSTCOBRA;
                                PercMulta = Convert.ToDecimal(regraGeracaoBoleto.PERCMULTA);
                                VLMultaDiaAtraso = Convert.ToDecimal(regraGeracaoBoleto.VLMULTADIAATRASO);
                                PercJuros = Convert.ToDecimal(regraGeracaoBoleto.PERCJUROS);
                                VLJurosDiaAtraso = Convert.ToDecimal(regraGeracaoBoleto.VLJUROSDIAATRASO);
                            }

                        }
                        else
                        {
                            codigoPriemriaInstrucaoCobranca = (int)cli.CODPRIMEIRAINSTCOBRA;
                            codigoSegundaInstrucaoCobranca = (int)cli.CODSEGUNDAINSTCOBRA;
                            PercMulta = Convert.ToDecimal(cli.PERCMULTA);
                            VLMultaDiaAtraso = Convert.ToDecimal(cli.VLMULTADIAATRASO);
                            PercJuros = Convert.ToDecimal(cli.PERCJUROS);
                            VLJurosDiaAtraso = Convert.ToDecimal(cli.VLJUROSDIAATRASO);
                        }


                        dados.CodigoOcorrencia = Geral.FormataInteiro(bol.codOcorrencia.ToString(), 2);
                        dados.DescricaoOcorrencia = "Remessa Registrar";

                        dados.DataEmissao = DateTime.Now;
                        dados.DataProcessamento = DateTime.Now;
                        dados.DataVencimento = bol.dtVencimento;
                        dados.ValorTitulo = bol.Valor;
                        dados.NossoNumero = bol.NossoNumero;
                        dados.NumeroDocumento = bol.SeuNumero;
                        dados.CodigoInstrucao1 = Convert.ToString(codigoPriemriaInstrucaoCobranca);
                        dados.ComplementoInstrucao1 = _instrucaoCobranca.GetInstrucaoCobrancaById(codigoPriemriaInstrucaoCobranca).Trim();
                        dados.CodigoInstrucao2 = Convert.ToString(codigoSegundaInstrucaoCobranca);
                        dados.ComplementoInstrucao2 = _instrucaoCobranca.GetInstrucaoCobrancaById(codigoSegundaInstrucaoCobranca).Trim();
                        dados.DataMulta = Convert.ToDateTime(bol.dtVencimento);
                        dados.PercentualMulta = PercMulta;
                        dados.ValorMulta = VLMultaDiaAtraso;
                        dados.DataJuros = Convert.ToDateTime(bol.dtVencimento);
                        dados.PercentualJurosDia = PercJuros;
                        dados.ValorJurosDia = VLJurosDiaAtraso;
                        dados.MensagemArquivoRemessa = bol.MensagemArquivoRemessa;
                        dados.MensagemInstrucoesCaixa = bol.MensagemInstrucoesCaixa;
                        dados.NumeroControleParticipante = bol.numControle.ToString();
                        dados.nmBoleto = bol.nmBoletoGerado;

                        dados.Sacado = APIBoletoSacadoVM.ToViewEmissaoBoleto(bol);

                        listaEmissaoBoleto.Add(dados);

                    }

                    var DadosArquivo = await _arquivo.ReturnFileById(codArquivo);
                    nmArquivoRemessa = DadosArquivo.NMARQUIVO;
                    var pathRemessa = Path.Combine(CaminhoRemessa, nmArquivoRemessa);

                    //inicia o processo de geração do arquivo remessa
                    var vm = APIBoletoVM.ToView(_banco, pathRemessa, pathBoleto, listaEmissaoBoleto, contacorrete.CODBANCO);
                    vm.CodigoSequencialArquivo = codArquivo;
                    vm.GeraArquivoRemessa = true;
                    vm.GeraBoleto = true;
                    
                    using (var client = new HttpClient())
                    {
                        string caminho = "API/APIRemessaBoleto/CriaArquivoRemessa";

                        if (client.BaseAddress == null)
                        {
                            //client.BaseAddress = new Uri("http://localhost:52327/");
                            client.BaseAddress = new Uri("https://www3.tln.com.br/apistelenet/apiboleto/");
                        }

                        var content = new StringContent(JsonConvert.SerializeObject(vm),
                                                        Encoding.UTF8,
                                                        "application/json");

                        HttpResponseMessage response = await client.PostAsync(caminho, content);
                        response.EnsureSuccessStatusCode();
                        var msgretorno = await response.Content.ReadAsStringAsync();

                        if (msgretorno != "Sucesso")
                        {
                            //processo para realizar o rollback
                            RollbackEmissaoBoleto(DadosBoleto, codArquivo, model.codUsuario);

                            throw new Exception(msgretorno);
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(Retorno)) Retorno = nmArquivoRemessa;

                return Retorno;
            }
            catch (ArgumentException ex1)
            {
                throw ex1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void RollbackEmissaoBoleto(List<PAGNET_EMISSAOBOLETO> DadosBoleto, int codArquivo, int codUsuario)
        {
            try
            {
                //cancela o arquivo de remessa
                var arquivoRemessa = _arquivo.GetById(codArquivo);
                arquivoRemessa.STATUS = "CANCELADO";

                _arquivo.AtualizaArquivo(arquivoRemessa);

                foreach (var boleto in DadosBoleto)
                {
                    boleto.Status = "CANCELADO";
                    //cancela a soliciação de emissao do boleto
                    _emissaoBoleto.AtualizaBoleto(boleto);

                    var ListaFaturas = _emissaoBoleto.BuscaFaturamentoBySeuNumero(boleto.SeuNumero).Result;

                    foreach (var fatura in ListaFaturas)
                    {
                        //volta o status dos faturamentos
                        fatura.STATUS = "EM_BORDERO";
                        fatura.SEUNUMERO = null;
                        _emissaoBoleto.AtualizaFaturamento(fatura);
                        _emissaoBoleto.IncluiLog(fatura, codUsuario, "ROLLBACK - Ocorreu uma falha durante o processo de emissão de arquivo de remessa.");


                        //volta o status do bordero
                        var bord = _bordero.LocalizaBordero((int)fatura.CODBORDERO);
                        bord.STATUS = "EM_BORDERO";

                        _bordero.AtualizaBordero(bord);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object[][] GetInstrucaoCobranca()
        {
            var lista = _instrucaoCobranca.GetHashInstrucaoCobranca().ToList();

            return lista.ToArray();
        }
        internal Cedente GerarCedente(PAGNET_CONTACORRENTE dados, PAGNET_CADEMPRESA empresa, ContaBancaria contaBancaria)
        {
            try
            {
                Cedente ced = new Cedente();

                ced.CPFCNPJ = dados.CPFCNPJ;
                ced.Nome = dados.NMEMPRESA;
                ced.CodigoDV = "";
                ced.MostrarCNPJnoBoleto = true;
                ced.Endereco = new Endereco
                {
                    LogradouroEndereco = empresa.LOGRADOURO,
                    LogradouroNumero = empresa.NROLOGRADOURO,
                    LogradouroComplemento = empresa.COMPLEMENTO,
                    Bairro = empresa.BAIRRO,
                    Cidade = empresa.CIDADE,
                    UF = empresa.UF,
                    CEP = empresa.CEP.Replace("-", "").Trim()
                };
                ced.ContaBancaria = contaBancaria;

                return ced;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RetornaNroSeqArquivo()
        {
            try
            {
                int ultimoArquivo = _arquivo.GetMaxNroSequencial();

                ultimoArquivo += 1;

                if (ultimoArquivo < 11)
                {
                    ultimoArquivo = 11;
                }

                return ultimoArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FiltroConsultaFaturamentoVM RetornaDadosInicio(string dados, int codEmpresa)
        {

            FiltroConsultaFaturamentoVM model = new FiltroConsultaFaturamentoVM();

            if (!string.IsNullOrWhiteSpace(dados))
            {
                var param = dados.Split(';');
                model.dtInicio = param[0];
                model.dtFim = param[1];
                model.CodigoCliente = Convert.ToInt32(param[3]);
                //model.CaminhoArquivoDownload = param[4];
            }
            else
            {
                DateTime data = DateTime.Now;

                model.dtInicio = data.ToShortDateString();
                model.dtFim = data.ToShortDateString();
            }


            var empresa = _empresa.ConsultaEmpresaById(codEmpresa).Result;
            model.codigoEmpresa = empresa.CODEMPRESA.ToString();
            model.nomeEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public async Task<string> SalvaBordero(DadosBoletoVM model)
        {
            try
            {
                string msgRetorno = "Borderô gerado com sucesso.";

                var vlBordero = model.ListaBoletos.Sum(X => Convert.ToDecimal(X.Valor.Replace("R$", "").Replace(".", "")));
                var quantidadeFaturas = model.ListaBoletos.Count();

                PAGNET_BORDERO_BOLETO bordero = new PAGNET_BORDERO_BOLETO();

                bordero.STATUS = "EM_BORDERO";
                bordero.CODUSUARIO = model.codUsuario;
                bordero.QUANTFATURAS = quantidadeFaturas;
                bordero.VLBORDERO = vlBordero;
                bordero.CODEMPRESA = model.codEmpresa;
                bordero.DTBORDERO = DateTime.Now;

                bordero = _bordero.InseriBordero(bordero);

                foreach (var item in model.ListaBoletos)
                {
                    var DadosEmissaoBoleto = await _emissaoBoleto.BuscaFaturamentoByID(item.codEmissaoBoleto);

                    DadosEmissaoBoleto.STATUS = bordero.STATUS;
                    DadosEmissaoBoleto.CODBORDERO = bordero.CODBORDERO;

                    _emissaoBoleto.AtualizaFaturamento(DadosEmissaoBoleto);
                    _emissaoBoleto.IncluiLog(DadosEmissaoBoleto, model.codUsuario, "Boleto incluído em um borderô");

                }

                return msgRetorno + " O número dele é: " + bordero.CODBORDERO;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public FiltroConsultaFaturamentoVM RetornaDadosInicioConstulaBordero(string dados, int codEmpresa)
        {
            FiltroConsultaFaturamentoVM model = new FiltroConsultaFaturamentoVM();

            var empresa = _empresa.ConsultaEmpresaById(codEmpresa).Result;
            model.codigoEmpresa = empresa.CODEMPRESA.ToString();
            model.nomeEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public BorderoBolVM ConsultaBordero(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                string status = model.codStatus;
                int codBordero = 0;

                if (!string.IsNullOrWhiteSpace(model.codBordero))
                {
                    codBordero = Convert.ToInt32(model.codBordero);
                }

                BorderoBolVM bordero;

                int codEmpresa = Convert.ToInt32(model.codigoEmpresa);

                List<PAGNET_BORDERO_BOLETO> ListaBordero = new List<PAGNET_BORDERO_BOLETO>();

                ListaBordero = _bordero.BuscaBordero(status, codEmpresa, codBordero);

                bordero = BorderoBolVM.ToView(ListaBordero);

                DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                foreach (var item in bordero.ListaBordero)
                {
                    var boletos = _emissaoBoleto.BuscaTodosFaturamentoByCodBordero(item.CODBORDERO).Result;
                    bool PossuiBoletoVencido = (boletos.Where(x => x.DATVENCIMENTO < dataAtual).ToList().Count > 0);
                    if (PossuiBoletoVencido)
                    {
                        item.POSSUIBOLETOVENCIDO = "S";
                        //_bordero.AtualizaBorderoVencido();
                    }
                    else
                    {
                        item.POSSUIBOLETOVENCIDO = "N";
                    }
                }

                return bordero;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ListaBoletosVM>> ConsultaBoletosGerados(FiltroConsultaFaturamentoVM model)
        {
            try
            {

                var DadosBoleto = _emissaoBoleto.GetAllBoletosNaoPagos(Convert.ToInt32(model.codigoEmpresa), model.CodigoCliente,
                                            Convert.ToDateTime(model.dtInicio), Convert.ToDateTime(model.dtFim));

                if (DadosBoleto != null)
                {
                    if (model.CodApenasBoletosEntreges != null && model.CodApenasBoletosEntreges != "T")
                    {
                        DadosBoleto = DadosBoleto.Where(x => x.BOLETOENVIADO == model.CodApenasBoletosEntreges).ToList();
                    }
                }

                var ListaBoletos = ListaBoletosVM.ToView(DadosBoleto);

                return ListaBoletos.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ConstulaEmissao_BoletoVM>> GetAllBoletosByBordero(int CodBordero)
        {
            try
            {
                List<ConstulaEmissao_BoletoVM> retorno = new List<ConstulaEmissao_BoletoVM>();

                var dadosEmissaoBoleto = await _emissaoBoleto.BuscaTodosFaturamentoByCodBordero(CodBordero);

                retorno = ConstulaEmissao_BoletoVM.ToViewFaturamento(dadosEmissaoBoleto).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CancelaBordero(int codBordero, int codUsuario)
        {
            try
            {
                _bordero.AtualizaStatusBordero(codBordero, "CANCELADO");
                _emissaoBoleto.IncluiLogByBordero(codBordero, "EM_ABERTO", codUsuario, "Borderô cancelado");
                _emissaoBoleto.AtualizaStatusBycodBordero(codBordero, "EM_ABERTO");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ConsultaArquivosRemessaVM>> CarregaGridArquivosRemessa(FiltroDownloadArquivoVm model)
        {
            try
            {
                string nmArquivo = "";

                DateTime DataInicio = Convert.ToDateTime(model.dtInicio);
                DateTime DataFinal = Convert.ToDateTime(model.dtFim);
                int codigoEmpresa = Convert.ToInt32(model.codEmpresa);

                var validaArquivo = _arquivo.GetFileByDate(DataInicio, DataFinal, codigoEmpresa, "BOL").ToList();

                List<ConsultaArquivosRemessaVM> retorno;
                retorno = _arquivo.GetFileByDate(DataInicio, DataFinal, codigoEmpresa, "BOL")
                .Select(x => new ConsultaArquivosRemessaVM(x, codigoEmpresa)).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SolicitacaoBoletoVM>> CarregaDadosArquivoRetorno(string CaminhoArquivo, int CodOpe, int CodUsuario)
        {
            try
            {
                APIDadosRetArquivo vm = new APIDadosRetArquivo();
                vm.caminhoArquivo = CaminhoArquivo;

                List<SolicitacaoBoletoVM> RetronoBoletos = new List<SolicitacaoBoletoVM>();

                using (var client = new HttpClient())
                {
                    string caminho = "API/APIRemessaBoleto/RetornaDadosArquivo";

                    if (client.BaseAddress == null)
                        //client.BaseAddress = new Uri("http://localhost:52327/");
                        client.BaseAddress = new Uri("https://www3.tln.com.br/apistelenet/apiboleto/");


                    var content = new StringContent(JsonConvert.SerializeObject(vm),
                                                    Encoding.UTF8,
                                                    "application/json");

                    HttpResponseMessage response = await client.PostAsync(caminho, content);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();


                    APIBoletoVM dadosRetBol = JsonConvert.DeserializeObject<APIBoletoVM>(json);

                    if (dadosRetBol.ListaBoletos.Count > 0)
                    {
                        foreach (var bol in dadosRetBol.ListaBoletos)
                        {
                            var BoletoRegistrado = await _emissaoBoleto.BuscaBoletoBySeuNumero(bol.NumeroControleParticipante.Trim());
                            if (BoletoRegistrado != null)
                            {
                                if (BoletoRegistrado.Status == "CANCELADO")
                                {
                                    BoletoRegistrado.DescricaoOcorrenciaRetBol = "Boleto cancelado.";

                                }
                                else
                                {
                                    BoletoRegistrado.Status = Geral.OcorrenciaBoletoCnab240(bol.CodigoOcorrencia);
                                    BoletoRegistrado.DescricaoOcorrenciaRetBol = ReturnDescricaoRetBoleto(bol.CodOcorrenciaAuxiliar, bol.CodigoOcorrencia);
                                    _emissaoBoleto.AtualizaBoleto(BoletoRegistrado);
                                    var pedidosFaturamento = await _emissaoBoleto.BuscaFaturamentoBySeuNumero(BoletoRegistrado.SeuNumero);
                                    foreach(var fatura in pedidosFaturamento)
                                    {
                                        fatura.STATUS = BoletoRegistrado.Status;
                                        fatura.CODCONTACORRENTE = BoletoRegistrado.codContaCorrente;

                                        if (BoletoRegistrado.Status == "LIQUIDADO" || BoletoRegistrado.Status == "BAIXADO")
                                        {
                                            fatura.DATPGTO = bol.DataVencimento;
                                            fatura.VLPGTO = bol.ValorTitulo;
                                        }
                                        _emissaoBoleto.AtualizaFaturamento(fatura);

                                        if (fatura.TIPOFATURAMENTO == "CARGA")
                                        {
                                            _proc.RegistraPagamentoCargaNetCard(Convert.ToInt32(fatura.CODCLIENTE), Convert.ToInt32(fatura.NROREF_NETCARD));
                                        }
                                    }

                                }


                                //REALIZA A BAIXA NO FATURAMENTO CASO O MESMO TENHA SITO PAGO.
                                if (BoletoRegistrado.Status == "LIQUIDADO" || BoletoRegistrado.Status == "BAIXADO")
                                {
                                    var ListaFaturamento = await _emissaoBoleto.BuscaFaturamentoBySeuNumero(BoletoRegistrado.SeuNumero);
                                    foreach (var fatura in ListaFaturamento)
                                    {

                                        AtualizaSaldoContaCorrente((int)fatura.CODCONTACORRENTE, fatura.CODEMISSAOFATURAMENTO);

                                    }
                                }


                                var boVm = SolicitacaoBoletoVM.ToView(BoletoRegistrado);

                                RetronoBoletos.Add(boVm);
                            }
                        }
                        //AtualizaStatusArquivosRemessa(RetronoBoletos);
                    }
                    else
                    {
                        SolicitacaoBoletoVM aux = new SolicitacaoBoletoVM();
                        aux.MsgRetorno = dadosRetBol.MsgRetorno;
                        RetronoBoletos.Add(aux);
                    }

                }


                return RetronoBoletos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<SolicitacaoBoletoVM>> CarregaListaBoletos(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                List<SolicitacaoBoletoVM> listBol = new List<SolicitacaoBoletoVM>();
                model.dtFim = model.dtFim + " 23:59:59";

                DateTime dtInicio = Convert.ToDateTime(model.dtInicio);
                DateTime dtFim = Convert.ToDateTime(model.dtFim);


                var boletos = await _emissaoBoleto.GetAllFaturas(model.codStatus, Convert.ToInt32(model.codigoEmpresa), model.CodigoCliente, dtInicio, dtFim);
                listBol = SolicitacaoBoletoVM.ToViewFatura(boletos).ToList();

                return listBol;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<SolicitacaoBoletoVM>> BoletosRegistradosNaoLiquidados(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                List<SolicitacaoBoletoVM> listBol = new List<SolicitacaoBoletoVM>();

                DateTime dtInicio = Convert.ToDateTime("01/01/1900");
                DateTime dtFim = Convert.ToDateTime("01/01/1900");

                if (model.dtInicio != null)
                {
                    dtInicio = Convert.ToDateTime(model.dtInicio);
                }
                if (model.dtFim != null)
                {
                    dtFim = Convert.ToDateTime(model.dtFim);
                }

                var boletos = await _emissaoBoleto.GetBoletosRegistradosNaoLiquidados(Convert.ToInt32(model.codigoEmpresa), Convert.ToInt32(model.codigoContaCorrente), model.CodigoCliente, dtInicio, dtFim);
                listBol = SolicitacaoBoletoVM.ToView(boletos).ToList();

                return listBol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string ReturnDescricaoRetBoleto(string codOcorrenciaRetorno, string CodigoOcorrencia)
        {
            string descricaoRet = "";

            for (int i = 0; i < codOcorrenciaRetorno.Length; i += 2)
            {
                string codigo = codOcorrenciaRetorno.Substring(i, 2);
                if (codigo != "00")
                {
                    if (CodigoOcorrencia == "03")
                    {
                        descricaoRet = _ocorrenciaBol.ReturnOcorrencia(codigo);
                    }
                    else if (CodigoOcorrencia == "06")
                    {
                        descricaoRet = Geral.OcorrenciaRetLiquidacaoBoleto(codigo);
                    }
                    else if (CodigoOcorrencia == "09")
                    {
                        descricaoRet = Geral.OcorrenciaRetBaixaBoleto(codigo);
                    }
                }

            }

            return descricaoRet;
        }
        public async Task<List<ConstulaEmissao_BoletoVM>> GetAllBoletosByCodArquivo(int CodArquivo, int codOpe)
        {
            try
            {
                List<ConstulaEmissao_BoletoVM> retorno = new List<ConstulaEmissao_BoletoVM>();

                var dadosEmissaoBoleto = await _emissaoBoleto.BuscaTodosBoletoByCodArquivo(CodArquivo);

                retorno = ConstulaEmissao_BoletoVM.ToViewBoleto(dadosEmissaoBoleto).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ConstulaEmissao_BoletoVM>> BustaTodosBoletosParaEdicao(int codigoEmpresa)
        {
            try
            {
                List<ConstulaEmissao_BoletoVM> retorno = new List<ConstulaEmissao_BoletoVM>();

                var dadosEmissaoBoleto = await _emissaoBoleto.BustaTodosFaturamentosParaEdicao(codigoEmpresa);

                retorno = ConstulaEmissao_BoletoVM.ToViewFaturamento(dadosEmissaoBoleto).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<EmissaoBoletoVM> DadosInicioEmissaoBoletoAvulso(int codEmissaoBoleto, int codEmpresa)
        {
            try
            {
                EmissaoBoletoVM boleto = new EmissaoBoletoVM();

                if (codEmissaoBoleto > 0)
                {
                    var dadosboleto = await _emissaoBoleto.BuscaFaturamentoByID(codEmissaoBoleto);
                    boleto = EmissaoBoletoVM.ToView(dadosboleto);
                    if (boleto.CobrancaDiferenciada == "N")
                    {
                        var dadosDefault = await _regraBol.BuscaRegraAtivaBol(dadosboleto.CODEMPRESA);
                        if (dadosDefault != null)
                        {
                            boleto.TaxaEmissaoBoleto = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", dadosDefault.TAXAEMISSAOBOLETO));
                            boleto.PrimeiraInstrucaoCobranca = dadosDefault.CODPRIMEIRAINSTCOBRA.ToString();
                            boleto.SegundaInstrucaoCobranca = dadosDefault.CODSEGUNDAINSTCOBRA.ToString();
                            boleto.CobraMulta = dadosDefault.COBRAMULTA;
                            boleto.ValorMulta = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(dadosDefault.VLMULTADIAATRASO)));
                            boleto.PercentualMulta = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(dadosDefault.PERCMULTA)));
                            boleto.CobraJuros = dadosDefault.COBRAJUROS;
                            boleto.ValorJuros = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(dadosDefault.VLJUROSDIAATRASO)));
                            boleto.PercentualJuros = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(dadosDefault.PERCJUROS)));
                        }
                    }

                    boleto.PrimeiraInstrucaoCobranca = _instrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(boleto.PrimeiraInstrucaoCobranca));
                    boleto.SegundaInstrucaoCobranca = _instrucaoCobranca.GetInstrucaoCobrancaById(Convert.ToInt32(boleto.SegundaInstrucaoCobranca));
                    boleto.nomeFormaFaturamento = _formaFaturamento.GetFormaFaturamentoById(Convert.ToInt32(boleto.codigoFormaFaturamento));

                    decimal valorPrevisto = Convert.ToDecimal(boleto.Valor);
                    decimal valorJuros = Convert.ToDecimal(boleto.ValorJuros.Replace("R$", ""));
                    decimal PercentualJuros = Convert.ToDecimal(boleto.PercentualJuros.Replace("R$", ""));
                    decimal valorMulta = Convert.ToDecimal(boleto.ValorMulta.Replace("R$", ""));
                    decimal PercentualMulta = Convert.ToDecimal(boleto.PercentualMulta.Replace("R$", ""));


                    decimal MultaCobrada = Convert.ToDecimal(boleto.MultaCobrada.Replace("R$", ""));
                    decimal JurosCobrado = Convert.ToDecimal(boleto.JurosCobrado.Replace("R$", ""));
                    decimal ValorDescontoConcedido = Convert.ToDecimal(boleto.ValorDescontoConcedido.Replace("R$", ""));

                    if (PercentualJuros > 0)
                        valorJuros = ((valorPrevisto / 100) * PercentualJuros);

                    if (PercentualMulta > 0)
                        valorMulta = ((valorPrevisto / 100) * PercentualMulta);

                    decimal ValorRecebido = valorPrevisto + valorJuros + valorMulta;
                    ValorRecebido = (ValorRecebido + MultaCobrada + JurosCobrado) - ValorDescontoConcedido;
                    boleto.ValorRecebido = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", ValorRecebido)).Replace("R$", "");
                    boleto.valorPrevistoRecebimento = boleto.ValorRecebido;
                }
                else
                {
                    var dadosEmpresa = await _empresa.ConsultaEmpresaById(codEmpresa);

                    boleto.codigoEmpresa = dadosEmpresa.CODEMPRESA.ToString();
                    boleto.nomeEmpresa = dadosEmpresa.NMFANTASIA;
                }
                return boleto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> IncluiNovoPedidoFaturamento(EmissaoBoletoVM model)
        {
            var resultado = new Dictionary<bool, string>();

            PAGNET_EMISSAOFATURAMENTO faturamento;

            if (model.codigoEmissaoBoleto == 0)
            {
                //Valida se existe um boleto criado com o seu número informado pelo usuário
                if (!string.IsNullOrWhiteSpace(model.nroDocumento))
                {
                    var dadosboleto = await _emissaoBoleto.BuscaFaturamentoByNroDocumento(model.nroDocumento);
                    if (dadosboleto != null)
                    {
                        resultado.Add(false, "Já existe um pedido de faturamento criado com este código de rastreio, favor informar um novo número");
                        return resultado;
                    }
                }

                faturamento = new PAGNET_EMISSAOFATURAMENTO();
                int codFaturamento = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
                faturamento.CODEMISSAOFATURAMENTO = codFaturamento;
                faturamento.ORIGEM = "PAGNET";
                faturamento.TIPOFATURAMENTO = "PAGNET";
                faturamento.DATSOLICITACAO = DateTime.Now;
                faturamento.CODCLIENTE = Convert.ToInt32(model.CodigoCliente);
                faturamento.CODBORDERO = null;
                faturamento.CODEMPRESA = Convert.ToInt32(model.codigoEmpresa);
                faturamento.STATUS = "A_FATURAR";
                faturamento.VALOR = Convert.ToDecimal(model.Valor);
                faturamento.CODEMISSAOFATURAMENTOPAI = codFaturamento;
                faturamento.PARCELA = 1;
                faturamento.TOTALPARCELA = 1;
                faturamento.VALORPARCELA = Convert.ToDecimal(model.Valor);
            }
            else
            {
                faturamento = await _emissaoBoleto.BuscaFaturamentoByID(model.codigoEmissaoBoleto);
                if (faturamento.STATUS == "PENDENTE_REGISTRO" || faturamento.STATUS == "REGISTRADO")
                {
                    var boleto = await _emissaoBoleto.BuscaBoletoBySeuNumero(faturamento.SEUNUMERO);
                    boleto.codOcorrencia = Convert.ToInt32(model.codigoOcorrencia);
                    boleto.dtVencimento = Convert.ToDateTime(model.dataVencimento);
                    boleto.Valor = (faturamento.VALOR + Convert.ToDecimal(model.JurosCobrado) + Convert.ToDecimal(model.MultaCobrada)) - Convert.ToDecimal(model.ValorDescontoConcedido);
                    _emissaoBoleto.AtualizaBoleto(boleto);
                    faturamento.STATUS = "EM_ABERTO";
                }
                else if (faturamento.STATUS != "A_FATURAR")
                {
                    faturamento.STATUS = "A_FATURAR";
                }
            }


            faturamento.CODPLANOCONTAS = Convert.ToInt32(model.CodigoPlanoContas);
            faturamento.DATVENCIMENTO = Convert.ToDateTime(model.dataVencimento);
            faturamento.NRODOCUMENTO = model.nroDocumento;

            faturamento.JUROSCOBRADO = Convert.ToDecimal(model.JurosCobrado);
            faturamento.MULTACOBRADA = Convert.ToDecimal(model.MultaCobrada);
            faturamento.VLDESCONTOCONCEDIDO = Convert.ToDecimal(model.ValorDescontoConcedido);

            //faturamento.MENSAGEMARQUIVOREMESSA = model.MensagemArquivoRemessa;
            faturamento.MENSAGEMINSTRUCOESCAIXA = model.MensagemInstrucoesCaixa;
            faturamento.CODFORMAFATURAMENTO = Convert.ToInt32(model.codigoFormaFaturamento);

            try
            {
                if (model.codigoEmissaoBoleto == 0)
                {
                    _emissaoBoleto.IncluiFaturamento(faturamento);
                    _emissaoBoleto.IncluiLog(faturamento, model.codigoUsuario, "Pedido de faturamento incluída manualmente via PagNet");
                    resultado.Add(true, "Pedido de faturamento incluído com sucesso");
                }
                else
                {

                    if (model.codJustificativa != "OUTROS")
                    {
                        model.DescJustOutros = model.descJustificativa;
                    }
                    if (string.IsNullOrWhiteSpace(model.DescJustOutros))
                    {
                        model.DescJustOutros = "Alterado o pedido de faturamento via PagNet";
                    }

                    _emissaoBoleto.AtualizaFaturamento(faturamento);
                    _emissaoBoleto.IncluiLog(faturamento, model.codigoUsuario, model.DescJustOutros);
                    resultado.Add(true, "Pedido de faturamento atualizado com sucesso");
                }

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<DadosBoletoVM> ConsultaSolicitacoesBoletos(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                DadosBoletoVM dadosBol = new DadosBoletoVM();

                DateTime dtInicio = Convert.ToDateTime(model.dtInicio);
                DateTime dtFim = Convert.ToDateTime(model.dtFim);
                int codCliente = Convert.ToInt32(model.CodigoCliente);
                int codEmpresa = Convert.ToInt32(model.codigoEmpresa);

                var dadosBoletos = await _proc.ConsultaEmissaoBoleto(dtInicio, dtFim, codCliente, codEmpresa);

                dadosBol = DadosBoletoVM.ToView(dadosBoletos.ToList());

                return dadosBol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GeraBoletoPDF(string caminhoPadrao, int codigoFatura, int CodigoUsuario)
        {

            try
            {
                int codigoPriemriaInstrucaoCobranca = 0;
                int codigoSegundaInstrucaoCobranca = 0;
                decimal PercMulta = 0;
                decimal VLMultaDiaAtraso = 0;
                decimal PercJuros = 0;
                decimal VLJurosDiaAtraso = 0;

                var dadosFatura = _emissaoBoleto.BuscaFaturamentoByID(codigoFatura).Result;
                var DadosBoleto = _emissaoBoleto.BuscaBoletoBySeuNumero(dadosFatura.SEUNUMERO).Result;

                //int nroSeqArquivo = RetornaNroSeqArquivo();
                var contacorrete = _conta.GetContaCorrenteById(DadosBoleto.codContaCorrente).Result;
                var dadosEmpresa = _empresa.ConsultaEmpresaById(contacorrete.CODEMPRESA).Result;

                var contaBancaria = new ContaBancaria
                {
                    Agencia = contacorrete.AGENCIA,
                    DigitoAgencia = contacorrete.DIGITOAGENCIA,
                    Conta = contacorrete.NROCONTACORRENTE,
                    DigitoConta = contacorrete.DIGITOCC,
                    TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
                };
                if (contacorrete.CODBANCO == "001")
                {
                    contaBancaria.VariacaoCarteiraPadrao = "019";
                }
                _banco = Banco.Instancia(Convert.ToInt32(contacorrete.CODBANCO));
                _banco.Cedente = GerarCedente(contacorrete, dadosEmpresa, contaBancaria);
                List<ListaEmissaoBoletoVM> listaEmissaoBoleto = new List<ListaEmissaoBoletoVM>();

                ListaEmissaoBoletoVM dados = new ListaEmissaoBoletoVM();

                var cli = DadosBoleto.PAGNET_CADCLIENTE;
                if (cli.COBRANCADIFERENCIADA == "N")
                {
                    var regraGeracaoBoleto = _regraBol.BuscaRegraAtivaBol(cli.CODEMPRESA).Result;
                    if (regraGeracaoBoleto != null)
                    {
                        codigoPriemriaInstrucaoCobranca = regraGeracaoBoleto.CODPRIMEIRAINSTCOBRA;
                        codigoSegundaInstrucaoCobranca = regraGeracaoBoleto.CODSEGUNDAINSTCOBRA;
                        PercMulta = Convert.ToDecimal(regraGeracaoBoleto.PERCMULTA);
                        VLMultaDiaAtraso = Convert.ToDecimal(regraGeracaoBoleto.VLMULTADIAATRASO);
                        PercJuros = Convert.ToDecimal(regraGeracaoBoleto.PERCJUROS);
                        VLJurosDiaAtraso = Convert.ToDecimal(regraGeracaoBoleto.VLJUROSDIAATRASO);
                    }

                }
                else
                {
                    codigoPriemriaInstrucaoCobranca = (int)cli.CODPRIMEIRAINSTCOBRA;
                    codigoSegundaInstrucaoCobranca = (int)cli.CODSEGUNDAINSTCOBRA;
                    PercMulta = Convert.ToDecimal(cli.PERCMULTA);
                    VLMultaDiaAtraso = Convert.ToDecimal(cli.VLMULTADIAATRASO);
                    PercJuros = Convert.ToDecimal(cli.PERCJUROS);
                    VLJurosDiaAtraso = Convert.ToDecimal(cli.VLJUROSDIAATRASO);
                }

                dados.CodigoOcorrencia = Geral.FormataInteiro(DadosBoleto.codOcorrencia.ToString(), 2);
                dados.DescricaoOcorrencia = "Remessa Registrar";

                dados.DataEmissao = DateTime.Now;
                dados.DataProcessamento = DateTime.Now;
                dados.DataVencimento = Convert.ToDateTime(DadosBoleto.dtVencimento);
                dados.ValorTitulo = DadosBoleto.Valor;
                dados.NossoNumero = Geral.GeraNossoNumero("0", DadosBoleto.codEmissaoBoleto);
                dados.NumeroDocumento = DadosBoleto.codEmissaoBoleto.ToString();
                dados.CodigoInstrucao1 = Convert.ToString(codigoPriemriaInstrucaoCobranca);
                dados.ComplementoInstrucao1 = _instrucaoCobranca.GetInstrucaoCobrancaById(codigoPriemriaInstrucaoCobranca).Trim();
                dados.CodigoInstrucao2 = Convert.ToString(codigoSegundaInstrucaoCobranca);
                dados.ComplementoInstrucao2 = _instrucaoCobranca.GetInstrucaoCobrancaById(codigoSegundaInstrucaoCobranca).Trim();
                dados.DataMulta = Convert.ToDateTime(DadosBoleto.dtVencimento);
                dados.PercentualMulta = PercMulta;
                dados.ValorMulta = VLMultaDiaAtraso;
                dados.DataJuros = Convert.ToDateTime(DadosBoleto.dtVencimento);
                dados.PercentualJurosDia = PercJuros;
                dados.ValorJurosDia = VLJurosDiaAtraso;
                dados.MensagemArquivoRemessa = DadosBoleto.MensagemArquivoRemessa;
                dados.MensagemInstrucoesCaixa = DadosBoleto.MensagemInstrucoesCaixa;
                dados.NumeroControleParticipante = DadosBoleto.codEmissaoBoleto.ToString();
                dados.nmBoleto = DadosBoleto.nmBoletoGerado;

                dados.Sacado = APIBoletoSacadoVM.ToViewEmissaoBoleto(DadosBoleto);

                listaEmissaoBoleto.Add(dados);

                _emissaoBoleto.IncluiLogBySeuNumero(DadosBoleto.SeuNumero, DadosBoleto.Status, CodigoUsuario, "Gerado PDF do Boleto");


                //inicia o processo de geração do arquivo remessa
                var vm = APIBoletoVM.ToView(_banco, caminhoPadrao, caminhoPadrao, listaEmissaoBoleto, contacorrete.CODBANCO);
                vm.GeraArquivoRemessa = false;
                vm.GeraBoleto = true;

                using (var client = new HttpClient())
                {
                    string caminho = "API/APIRemessaBoleto/CriaArquivoRemessa";

                    if (client.BaseAddress == null)
                    {
                        //client.BaseAddress = new Uri("http://localhost:52327/");
                        client.BaseAddress = new Uri("https://www3.tln.com.br/apistelenet/apiboleto/");
                    }

                    var content = new StringContent(JsonConvert.SerializeObject(vm),
                                                    Encoding.UTF8,
                                                    "application/json");

                    HttpResponseMessage response = client.PostAsync(caminho, content).Result;
                    response.EnsureSuccessStatusCode();
                    var msgretorno = response.ReasonPhrase;

                    if (msgretorno != "Sucesso" && msgretorno != "OK")
                    {
                        throw new Exception(msgretorno);
                    }
                }

            }
            catch (ArgumentException ex1)
            {
                throw ex1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<string, string>> CancelaArquivoRemessaByID(int codCarquivo, int codUsuario)
        {

            var resultado = new Dictionary<string, string>();
            int codBordero = 0;
            try
            {
                if (codCarquivo == 0)
                {
                    resultado.Add("Error", "Código do Arquivo Inválido");
                    return resultado;
                }

                var DadosBoleto = await _emissaoBoleto.BuscaTodosBoletoByCodArquivo(codCarquivo);

                //cancela o arquivo de remessa
                var arquivoRemessa = _arquivo.GetById(codCarquivo);
                arquivoRemessa.STATUS = "CANCELADO";

                _arquivo.AtualizaArquivo(arquivoRemessa);

                foreach (var boleto in DadosBoleto)
                {
                    boleto.Status = "CANCELADO";

                    //cancela a soliciação de emissao do boleto
                    _emissaoBoleto.AtualizaBoleto(boleto);

                    var ListaFaturas = _emissaoBoleto.BuscaFaturamentoBySeuNumero(boleto.SeuNumero).Result;

                    foreach (var fatura in ListaFaturas)
                    {
                        //volta o status dos faturamentos
                        fatura.STATUS = "EM_BORDERO";
                        fatura.SEUNUMERO = null;
                        _emissaoBoleto.AtualizaFaturamento(fatura);
                        _emissaoBoleto.IncluiLog(fatura, codUsuario, "Arquivo de remessa cancelado pelo usuario");

                        if (codBordero != fatura.CODBORDERO)
                        {
                            codBordero = (int)fatura.CODBORDERO;

                            //volta o status do bordero
                            var bord = _bordero.LocalizaBordero(codBordero);
                            bord.STATUS = "EM_BORDERO";

                            _bordero.AtualizaBordero(bord);
                        }
                    }

                }

                resultado.Add("OK", "sucesso");

                return resultado;

            }
            catch (ArgumentException ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                resultado.Add(ex.ParamName, lines[0]);
            }
            return resultado;
        }
        public void AtualizaSolicitacaoCarga()
        {
            try
            {
                DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                _proc.AtualizaSolicitacaoCarga(dataAtual);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> CancelarFatura(EmissaoBoletoVM model)
        {
            var resultado = new Dictionary<bool, string>();


            var faturamento = await _emissaoBoleto.BuscaFaturamentoByID(model.codigoEmissaoBoleto);

            faturamento.STATUS = "CANCELADO";

            try
            {

                if (model.codJustificativa != "OUTROS")
                {
                    model.DescJustOutros = model.descJustificativa;
                }

                _emissaoBoleto.AtualizaFaturamento(faturamento);
                _emissaoBoleto.IncluiLog(faturamento, model.codigoUsuario, "Pedido de faturamento cancelada pelo usuário com a justificativa: " + model.DescJustOutros);
                resultado.Add(true, "Pedido de faturamento cancelada com sucesso");

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<IDictionary<bool, string>> LiquidacaoManual(EmissaoBoletoVM model)
        {
            var resultado = new Dictionary<bool, string>();

            var fatura = await _emissaoBoleto.BuscaFaturamentoByID(model.codigoEmissaoBoleto);

            fatura.STATUS = "LIQUIDADO_MANUALMENTE";
            fatura.DATPGTO = DateTime.Now;
            fatura.VLPGTO = Convert.ToDecimal(model.ValorRecebido);
            fatura.VLDESCONTOCONCEDIDO = Convert.ToDecimal(model.ValorDescontoConcedido);
            fatura.JUROSCOBRADO = Convert.ToDecimal(model.JurosCobrado);
            fatura.MULTACOBRADA = Convert.ToDecimal(model.MultaCobrada);
            fatura.CODCONTACORRENTE = Convert.ToInt32(model.codigoContaCorrente);

            try
            {
                if (model.codJustificativa != "OUTROS")
                {
                    model.DescJustOutros = model.descJustificativa;
                }

                _emissaoBoleto.AtualizaFaturamento(fatura);
                _emissaoBoleto.IncluiLog(fatura, model.codigoUsuario, "Faturamento liquidada manualmente pelo usuário com a justificativa: " + model.DescJustOutros);

                //ATUALIZA SALDO DA CONTA CORRENTE
                AtualizaSaldoContaCorrente((int)fatura.CODCONTACORRENTE, fatura.CODEMISSAOFATURAMENTO);

                if (fatura.TIPOFATURAMENTO == "CARGA")
                {
                    _proc.RegistraPagamentoCargaNetCard(Convert.ToInt32(fatura.CODCLIENTE), Convert.ToInt32(fatura.NROREF_NETCARD));
                }

                resultado.Add(true, "Faturamento Liquidada Com Sucesso");

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<List<ConstulaLogFaturaVM>> VisualizarLog(int CodFatura)
        {
            try
            {
                List<ConstulaLogFaturaVM> retorno = new List<ConstulaLogFaturaVM>();

                var dadosEmissaoBoleto = await _emissaoBoleto.BuscaLog(CodFatura);

                retorno = dadosEmissaoBoleto
                    .Select(x => new ConstulaLogFaturaVM(x)).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> RetornaNomeBoletoByID(int codFaturamento)
        {
            try
            {
                string nomeBoleto = "";
                var dadosFaturamento = await _emissaoBoleto.BuscaFaturamentoByID(codFaturamento);

                if (!string.IsNullOrWhiteSpace(dadosFaturamento.SEUNUMERO))
                {
                    var dadosBoleto = await _emissaoBoleto.BuscaBoletoBySeuNumero(dadosFaturamento.SEUNUMERO);
                    nomeBoleto = dadosBoleto.nmBoletoGerado;
                }
                return nomeBoleto;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> ValidapedidosFaturamento(List<SolicitacaoBoletoVM> listaPedidos, int CodUsuario)
        {
            try
            {
                var resultado = new Dictionary<bool, string>();

                foreach (var pedido in listaPedidos)
                {
                    var faturamento = await _emissaoBoleto.BuscaFaturamentoByID(pedido.codEmissaoBoleto);
                    faturamento.STATUS = "EM_ABERTO";

                    _emissaoBoleto.AtualizaFaturamento(faturamento);
                    _emissaoBoleto.IncluiLog(faturamento, CodUsuario, "Pedidto autorizado para ser faturado.");
                }

                resultado.Add(true, "Pedidos Validados com Sucesso!");
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ConstulaEmissao_BoletoVM>> ListaFaturamentoNaoLiquidado(int codEmpresa)
        {
            try
            {
                List<ConstulaEmissao_BoletoVM> ListaTitulos;

                var Dados = await _emissaoBoleto.ListaFaturamentoNaoLiquidado(codEmpresa);
                ListaTitulos = ConstulaEmissao_BoletoVM.ToViewFaturamento(Dados).ToList();

                return ListaTitulos;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<DetalhamentoFaturaReembolsoVm> RetornaDadosDetalhamentoCobranca(int codEmissaoFaturamento)
        {
            try
            {
                var dadosFaturamento = await _emissaoBoleto.BuscaFaturamentoByID(codEmissaoFaturamento);

                DetalhamentoFaturaReembolsoVm model = new DetalhamentoFaturaReembolsoVm();

                model.nroDocumento = Geral.FormataInteiro(dadosFaturamento.NRODOCUMENTO, 11);
                model.datEmissao = dadosFaturamento.DATSOLICITACAO.ToShortDateString();
                model.datVencimento = dadosFaturamento.DATVENCIMENTO.ToShortDateString();
                model.vlTotal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", dadosFaturamento.VALOR);

                var DadosEmpresa = await _empresa.ConsultaEmpresaById(dadosFaturamento.CODEMPRESA);
                var DadosCliente = await _cliente.BuscaClienteByID(Convert.ToInt32(dadosFaturamento.CODCLIENTE));

                //Dados Credor
                model.Credor = DadosEmpresa.RAZAOSOCIAL;
                model.CNPJCredor = Geral.FormataCPFCnPj(DadosEmpresa.CNPJ);
                model.CEPCredor = Geral.FormataCEP(DadosEmpresa.CEP);
                model.EnderecoCredor = DadosEmpresa.LOGRADOURO;
                model.nroCredor = DadosEmpresa.NROLOGRADOURO;
                model.ComplementoCredor = DadosEmpresa.COMPLEMENTO;
                model.BairroCredor = DadosEmpresa.BAIRRO;
                model.CidadeCredor = DadosEmpresa.CIDADE;
                model.EstadoCredor = DadosEmpresa.UF;

                //Dados Devedor
                model.Devedor = DadosCliente.NMCLIENTE;
                model.CNPJDevedor = Geral.FormataCPFCnPj(DadosCliente.CPFCNPJ);
                model.CEPDevedor = DadosCliente.CEP;
                model.EnderecoDevedor = DadosCliente.LOGRADOURO;
                model.nroDevedor = DadosCliente.NROLOGRADOURO;
                model.ComplementoDevedor = DadosCliente.COMPLEMENTO;
                model.BairroDevedor = DadosCliente.BAIRRO;
                model.CidadeDevedor = DadosCliente.CIDADE;
                model.EstadoDevedor = DadosCliente.UF;

                var detalhamentoCobranca = await _proc.DetalhamentoCobranca(codEmissaoFaturamento);

                DetalhamentoValoresCobradosFaturaVm detalhe;
                foreach (var linha in detalhamentoCobranca)
                {
                    detalhe = new DetalhamentoValoresCobradosFaturaVm();
                    detalhe.Descricao = linha.DESCRICAO;
                    detalhe.Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", linha.VALOR);
                    model.Detalhamento.Add(detalhe);
                }

                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDictionary<bool, string>> SalvarParcelamentoFatura(ParcelamentoFaturaVm model)
        {
            var resultado = new Dictionary<bool, string>();

            var DadosFatura = await _emissaoBoleto.BuscaFaturamentoByID(model.Parcela_codFaturamento);

            int qtParcela = model.Parcela_qtParcelas;

            PAGNET_EMISSAOFATURAMENTO fatura = new PAGNET_EMISSAOFATURAMENTO();
            try
            {
                foreach (var parcela in model.ListaParcelas)
                {
                    fatura = new PAGNET_EMISSAOFATURAMENTO();
                    fatura = DadosFatura;

                    fatura.CODEMISSAOFATURAMENTOPAI = DadosFatura.CODEMISSAOFATURAMENTO;
                    fatura.VALORPARCELA = Convert.ToDecimal(parcela.ValorParcela.Replace("R$", ""));
                    fatura.PARCELA = Convert.ToInt32(parcela.NroParcela);
                    fatura.TOTALPARCELA = qtParcela;
                    fatura.DATVENCIMENTO = Convert.ToDateTime(parcela.VencimentoParcela);

                    if (parcela.NroParcela == "1")
                    {
                        //ATUALIZA FATURAMENTO ORIGINAL
                        _emissaoBoleto.AtualizaFaturamento(fatura);
                    }
                    else
                    {
                        //INCLUI UMA NOVA FATURA
                        fatura.CODEMISSAOFATURAMENTO = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
                        _emissaoBoleto.IncluiFaturamento(fatura);
                    }

                    //INCLUSÃO DE LOG
                    _emissaoBoleto.IncluiLog(fatura, model.Parcela_codUsuario, $"Fatura parcelada em {qtParcela}x, com taxa de {model.Parcela_TaxaMensal} ao mês.");
                }

                resultado.Add(true, "Fatura parcelada com sucesso.");


            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }

        public void AtualizaSaldoContaCorrente(int codcontacorrente, int codigo)
        {
            try
            {
                decimal saldoContaCorrente = 0;
                decimal CalculoSaldoAtual = 0;

                var dadosContaCorrente = _conta.GetContaCorrenteById(codcontacorrente).Result;
                saldoContaCorrente = _conta.RetornaSaldoAtual(codcontacorrente);

                var dadosFaturamento = _emissaoBoleto.BuscaFaturamentoByID(codigo).Result;
                var valorRecebido = (decimal)dadosFaturamento.VLPGTO;
                CalculoSaldoAtual = saldoContaCorrente + valorRecebido;

                _conta.InseriNovoSaldo(codcontacorrente, dadosContaCorrente.CODEMPRESA, CalculoSaldoAtual);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object[][] CarregaListaFaturas(int codigoCliente)
        {
            var lista = _emissaoBoleto.CarregaListaFaturas(codigoCliente).ToList();

            return lista.ToArray();
        }

        public async Task<List<ListaClienteUsuarioVm>> ConsolidaArquivoRetornoCliente(string CaminhoArquivo, string nmOperadora, int codFatura)
        {
            try
            {
                List<ListaClienteUsuarioVm> ListaRemessa = new List<ListaClienteUsuarioVm>();
                List<ListaClienteUsuarioVm> ListaRetorno = new List<ListaClienteUsuarioVm>();
                List<ListaClienteUsuarioVm> ListaUsuarios = new List<ListaClienteUsuarioVm>();
                //BUSCA INFORMAÇÕES PARA REALIZAR A CONCILIAÇÃO
                CaminhoArquivo = @"C:\Users\felipe\Desktop\Teste Arquivo Prefeitura\Arquivo da prefeitura para teste.csv";
                var DadosFatura = await _emissaoBoleto.BuscaFaturamentoByID(codFatura);
                var dadosConfig = await _arquivoConciliacao.BuscaConfiguracaoByCodCliente(Convert.ToInt32(DadosFatura.CODCLIENTE));
                string codCliente = Geral.FormataInteiro(DadosFatura.CODCLIENTE.ToString(), 5);

                //CARREGA INFORMAÇÕES PARA LER O ARQUIVO DE REMESSA
                int PosicaoValorRemessa = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "VALOR").Select(t => t.POSICAO_CSV - 1).FirstOrDefault();
                int PosicaoInicialValorRem = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "VALOR").Select(t => t.POSICAOINI_TXT - 1).FirstOrDefault();
                int PosicaoFinalValorRem = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "VALOR").Select(t => t.POSICAOFIM_TXT - 1).FirstOrDefault();

                int PosicaoMatriculaRemessa = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "MATRICULA").Select(t => t.POSICAO_CSV - 1).FirstOrDefault();
                int PosicaoInicialMatriculaRem = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "MATRICULA").Select(t => t.POSICAOINI_TXT - 1).FirstOrDefault();
                int PosicaoFinalMatriculaRem = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "MATRICULA").Select(t => t.POSICAOFIM_TXT - 1).FirstOrDefault();
                int LinhaInicioRemessa = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "REM" && x.CAMPO == "MATRICULA").Select(t => t.LINHAINICIO - 1).FirstOrDefault();

                //CARREGA INFORMAÇÕES PARA LER O ARQUIVO DE RETORNO DO CLIENTE
                int PosicaoValorRetorno = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "VALOR").Select(t => t.POSICAO_CSV - 1).FirstOrDefault();
                int PosicaoInicialValorRet = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "VALOR").Select(t => t.POSICAOINI_TXT - 1).FirstOrDefault();
                int PosicaoFinalValorRet = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "VALOR").Select(t => t.POSICAOFIM_TXT - 1).FirstOrDefault();

                int PosicaoMatriculaRetorno = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "MATRICULA").Select(t => t.POSICAO_CSV - 1).FirstOrDefault();
                int PosicaoInicialMatriculaRet = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "MATRICULA").Select(t => t.POSICAOINI_TXT - 1).FirstOrDefault();
                int PosicaoFinalMatriculaRet = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "MATRICULA").Select(t => t.POSICAOFIM_TXT - 1).FirstOrDefault();
                int LinhaInicioRetorno = dadosConfig.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA.Where(x => x.TIPOARQUIVO == "RET" && x.CAMPO == "MATRICULA").Select(t => t.LINHAINICIO - 1).FirstOrDefault();


                //LOCALIZA O ARQUIVO DE REMESSA
                var pathArquivoConciliacao = Path.Combine(@"\\zeus\ARQ_CONCILIA", nmOperadora, "CLIENTES", codCliente);

                DirectoryInfo info = new DirectoryInfo(pathArquivoConciliacao);
                var files = info.GetFiles().ToArray();
                var fileRemessa = files.Where(x => x.LastWriteTime.ToShortDateString() == DadosFatura.DATSOLICITACAO.ToShortDateString()).FirstOrDefault();
                var FullpathArquivoRemessa = fileRemessa.FullName;
                var extensaoArquivoRemessa = fileRemessa.Extension;
                //LE O ARQUIVO DE REMESSA E CARREGA A LISTA DOS USUÁRIOS E VALORES PARA COMPARAÇÃO
                string[] linesRemessa = File.ReadAllLines(FullpathArquivoRemessa);

                ListaClienteUsuarioVm itemRem = new ListaClienteUsuarioVm();
                for (int i = LinhaInicioRemessa; i < linesRemessa.Length; i++)
                {
                    itemRem = new ListaClienteUsuarioVm();
                    if (extensaoArquivoRemessa == ".CSV")
                    {
                        var item = linesRemessa[i].Split(';');

                        itemRem.Matricula = item[PosicaoMatriculaRemessa].Trim();
                        itemRem.CodigoCliente = Convert.ToString(DadosFatura.CODCLIENTE);
                        itemRem.codigoFatura = codFatura;
                        itemRem.Valor = Convert.ToString(Math.Abs(Convert.ToDecimal(item[PosicaoValorRemessa].Replace("R$", ""))));
                        ListaRemessa.Add(itemRem);
                    }
                    else
                    {
                        itemRem.Matricula = linesRemessa[i].Substring(PosicaoInicialMatriculaRem, PosicaoFinalMatriculaRem);
                        itemRem.CodigoCliente = Convert.ToString(DadosFatura.CODCLIENTE);
                        itemRem.codigoFatura = codFatura;
                        itemRem.Valor = linesRemessa[i].Substring(PosicaoInicialValorRem, PosicaoFinalValorRem);
                        ListaRemessa.Add(itemRem);
                    }
                }

                //LE O ARQUIVO DE RETORNO DO CLIENTE E CARREGA A LISTA DOS USUÁRIOS E VALORES PARA COMPARAÇÃO
                string[] linesRetorno = File.ReadAllLines(CaminhoArquivo);

                ListaClienteUsuarioVm itemRet = new ListaClienteUsuarioVm();
                for (int t = LinhaInicioRetorno; t < linesRetorno.Length; t++)
                {
                    itemRet = new ListaClienteUsuarioVm();
                    if (extensaoArquivoRemessa == ".CSV")
                    {
                        var item = linesRetorno[t].Split(';');

                        itemRet.Matricula = item[PosicaoMatriculaRetorno].Trim();
                        itemRem.CodigoCliente = Convert.ToString(DadosFatura.CODCLIENTE);
                        itemRem.codigoFatura = codFatura;
                        itemRet.Valor = Convert.ToString(Math.Abs(Convert.ToDecimal(item[PosicaoValorRetorno].Replace("R$", ""))));
                        ListaRetorno.Add(itemRet);
                    }
                    else
                    {
                        itemRet.Matricula = linesRetorno[t].Substring(PosicaoInicialMatriculaRet, PosicaoFinalMatriculaRet);
                        itemRem.CodigoCliente = Convert.ToString(DadosFatura.CODCLIENTE);
                        itemRem.codigoFatura = codFatura;
                        itemRet.Valor = linesRetorno[t].Substring(PosicaoInicialValorRet, PosicaoFinalValorRet);
                        ListaRetorno.Add(itemRet);
                    }
                }
                string msgRetornoValidacao = "";
                var validado = ValidaArquivoConciliacaoPrefeitura(ListaRetorno, ListaRemessa, codFatura, out msgRetornoValidacao);

                if (!validado)
                {
                    throw new PagNetException(msgRetornoValidacao);
                }
                else
                {
                    var ListaUsuariosNaoDescontados =
                        (from m in ListaRemessa
                         where !(from t in ListaRetorno
                                 select new { t.Matricula, t.Valor })
                                .Contains(new { m.Matricula, m.Valor })
                         select m).ToList();

                    var valorTotal = ListaUsuariosNaoDescontados.Sum(x => Convert.ToDecimal(x.Valor));

                    ListaClienteUsuarioVm ItemUsu = new ListaClienteUsuarioVm();
                    foreach (var item in ListaUsuariosNaoDescontados)
                    {
                        ItemUsu = new ListaClienteUsuarioVm();

                        var usuarioNC = await _proc.BuscaDadosUsuarioNC(item.Matricula, 0, Convert.ToInt32(item.CodigoCliente));
                        if (usuarioNC != null)
                        {
                            ItemUsu.Matricula = item.Matricula;
                            ItemUsu.CodigoCliente = item.CodigoCliente;
                            ItemUsu.CPF = Geral.FormataCPFCnPj(usuarioNC.CPF);
                            ItemUsu.NomeClienteUsuario = usuarioNC.NOMEUSUARIO.Trim();
                            ItemUsu.Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(item.Valor));
                            ItemUsu.codigoFatura = codFatura;

                            ItemUsu.qtNaoDefinidos = ListaUsuariosNaoDescontados.Count;
                            ItemUsu.qtGeraFatura = 0;
                            ItemUsu.qtBaixaAutomatica = 0;
                            ItemUsu.vlNaoDefinido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal);
                            ItemUsu.vlGeraFatura = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                            ItemUsu.vlBaixaAutomatica = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                        }
                        else
                        {
                            ItemUsu.Matricula = item.Matricula;
                            ItemUsu.CodigoCliente = item.CodigoCliente;
                            ItemUsu.CPF = "-";
                            ItemUsu.NomeClienteUsuario = "Usuário não encontrado";
                            ItemUsu.Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(item.Valor));
                            ItemUsu.codigoFatura = codFatura;
                            ItemUsu.qtNaoDefinidos = ListaUsuariosNaoDescontados.Count;
                            ItemUsu.qtGeraFatura = 0;
                            ItemUsu.qtBaixaAutomatica = 0;
                            ItemUsu.vlNaoDefinido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal);
                            ItemUsu.vlGeraFatura = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                            ItemUsu.vlBaixaAutomatica = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                        }

                        ListaUsuarios.Add(ItemUsu);
                    }
                }

                return ListaUsuarios;

            }
            catch (PagNetException ex1)
            {
                throw ex1;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu uma falha ao tentar ler o arquivo. Favor contactar o suporte.");
            }
        }

        public async Task<IDictionary<bool, string>> ValidaFaturamentoViaArquivo(List<ListaClienteUsuarioVm> model, int cod_usu, int cod_empresa)
        {

            var resultado = new Dictionary<bool, string>();
            try
            {
                decimal ValorTotal = 0;
                decimal ValorProximaFatura = 0;
                int codFatura = model.FirstOrDefault().codigoFatura;
                var DadosFatura = await _emissaoBoleto.BuscaFaturamentoByID(codFatura);
                int CODEMISSAOFATURAMENTO = DadosFatura.CODEMISSAOFATURAMENTO;

                PAGNET_EMISSAOFATURAMENTO fatura = new PAGNET_EMISSAOFATURAMENTO();
                foreach (var item in model)
                {
                    ValorTotal += Convert.ToDecimal(item.Valor.Replace("R$", ""));
                    if (item.chkGeraFaturaUsuario)
                    {
                        fatura = new PAGNET_EMISSAOFATURAMENTO();
                        fatura = DadosFatura;
                        var codCliente = (await _proc.IncluiClienteUsuarioNC(Geral.RemoveCaracteres(item.CPF), 0, cod_empresa, cod_usu)).CODCLIENTE;

                        if (codCliente == 0)
                        {
                            throw new Exception("Ocorreu uma falha ao tentar identificar o usuário no NetCard. Favor contactar o suporte.");
                        }

                        fatura.CODEMISSAOFATURAMENTOPAI = CODEMISSAOFATURAMENTO;
                        fatura.VALORPARCELA = Convert.ToDecimal(item.Valor.Replace("R$", ""));
                        fatura.VALOR = Convert.ToDecimal(item.Valor.Replace("R$", ""));
                        fatura.PARCELA = 1;
                        fatura.STATUS = "EM_ABERTO";
                        fatura.TOTALPARCELA = 1;
                        fatura.CODCLIENTE = codCliente;

                        //INCLUI UMA NOVA FATURA
                        fatura.CODEMISSAOFATURAMENTO = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
                        _emissaoBoleto.IncluiFaturamento(fatura);

                        //INCLUSÃO DE LOG
                        _emissaoBoleto.IncluiLog(fatura, cod_usu, $"Criação de uma nova fatura a partir do arquivo de retorno de validação de faturamento.");

                    }
                    else if (item.chkBaixarFatura)
                    {
                        fatura = new PAGNET_EMISSAOFATURAMENTO();
                        fatura = DadosFatura;
                        var codCliente = (await _proc.IncluiClienteUsuarioNC(Geral.RemoveCaracteres(item.CPF), 0, cod_empresa, cod_usu)).CODCLIENTE;

                        if (codCliente == 0)
                        {
                            throw new Exception("Ocorreu uma falha ao tentar identificar o usuário no NetCard. Favor contactar o suporte.");
                        }

                        fatura.CODEMISSAOFATURAMENTOPAI = CODEMISSAOFATURAMENTO;
                        fatura.VALORPARCELA = Convert.ToDecimal(item.Valor.Replace("R$", ""));
                        fatura.VALOR = Convert.ToDecimal(item.Valor.Replace("R$", ""));
                        fatura.PARCELA = 1;
                        fatura.STATUS = "CANCELADA";
                        fatura.TOTALPARCELA = 1;
                        fatura.CODCLIENTE = codCliente;

                        //INCLUI UMA NOVA FATURA
                        fatura.CODEMISSAOFATURAMENTO = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
                        _emissaoBoleto.IncluiFaturamento(fatura);

                        //INCLUSÃO DE LOG
                        _emissaoBoleto.IncluiLog(fatura, cod_usu, $"Criação de uma nova fatura a partir do arquivo de retorno de validação de faturamento.");

                    }
                    else if (item.chkProximaFatura)
                    {
                        ValorProximaFatura += Convert.ToDecimal(item.Valor.Replace("R$", ""));
                    }
                }
                if (ValorTotal > 0)
                {
                    fatura = new PAGNET_EMISSAOFATURAMENTO();
                    fatura = await _emissaoBoleto.BuscaFaturamentoByID(codFatura);

                    fatura.VALORPARCELA = fatura.VALORPARCELA - ValorTotal;
                    fatura.STATUS = "EM_ABERTO";

                    _emissaoBoleto.AtualizaFaturamento(fatura);

                    //INCLUSÃO DE LOG
                    _emissaoBoleto.IncluiLog(fatura, cod_usu, $"Alteração do valor a pagar devido o arquivo de retorno que a prefeitura enviou constando quem ela conseguiu ou não descontar.");
                }
                resultado.Add(true, "Processamento realizado com sucesso.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }
        private bool ValidaArquivoConciliacaoPrefeitura(List<ListaClienteUsuarioVm> ListaPrefeitura, List<ListaClienteUsuarioVm> ListaRemessa, int codFatura, out string msgRetorno)
        {
            try
            {
                bool ArquivoValidado = true;
                msgRetorno = "";

                var ListaUsuario =
                        (from m in ListaRemessa
                         where (from t in ListaPrefeitura
                                select new { t.Matricula, t.Valor })
                                .Contains(new { m.Matricula, m.Valor })
                         select m).ToList();

                if (ListaUsuario.Count < ListaPrefeitura.Count)
                {
                    msgRetorno = "Arquivo de retorno não coincide com o arquivo de conciliação enviado ao cliente.";
                    ArquivoValidado = false;
                }

                var DadosFatura = _emissaoBoleto.BuscaFaturamentoByIDPai(codFatura, 0).Result;
                if (DadosFatura.Count > 1)
                {
                    msgRetorno = "Arquivo de retorno já validado.";
                    ArquivoValidado = false;
                }

                return ArquivoValidado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
