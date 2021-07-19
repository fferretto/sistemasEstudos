using Boleto2Net;
using Newtonsoft.Json;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Model;
using PagNet.Bld.PGTO.CobrancaBancaria.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Telenet.BusinessLogicModel;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Application
{
    public class CobrancaBancariaApp : Service<IContextoApp>, ICobrancaBancariaApp
    {
        private IBanco _banco;
        private readonly IParametrosApp _user;
        private readonly IPAGNET_CONTACORRENTEService _conta;
        private readonly IOPERADORAService _ope;
        private readonly IPAGNET_BORDERO_BOLETOService _bordero;
        private readonly IPAGNET_EMISSAOBOLETOService _emissaoBoleto;
        private readonly IPAGNET_CADCLIENTEService _cliente;
        private readonly IPAGNET_ARQUIVOService _arquivo;
        private readonly IPAGNET_CADEMPRESAService _empresa;
        private readonly IPAGNET_INSTRUCAOCOBRANCAService _instrucaoCobranca;
        private readonly IPAGNET_OCORRENCIARETBOLService _ocorrenciaBol;
        private readonly IProceduresService _proc;

        public CobrancaBancariaApp(IContextoApp contexto,
                                IParametrosApp user,
                                IOPERADORAService ope,
                                IPAGNET_CONTACORRENTEService conta,
                                IPAGNET_BORDERO_BOLETOService bordero,
                                IPAGNET_EMISSAOBOLETOService emissaoBoleto,
                                IPAGNET_CADCLIENTEService cliente,
                                IPAGNET_ARQUIVOService arquivo,
                                IPAGNET_CADEMPRESAService empresa,
                                IPAGNET_INSTRUCAOCOBRANCAService instrucaoCobranca,
                                IPAGNET_OCORRENCIARETBOLService ocorrenciaBol,
                                IProceduresService proc
                                )
            : base(contexto)
        {
            _user = user;
            _ope = ope;
            _conta = conta;
            _bordero = bordero;
            _emissaoBoleto = emissaoBoleto;
            _cliente = cliente;
            _arquivo = arquivo;
            _empresa = empresa;
            _instrucaoCobranca = instrucaoCobranca;
            _ocorrenciaBol = ocorrenciaBol;
            _proc = proc;
        }

        public ResultadoTransmissaoArquivo GeraArquivoRemessa(IBorderoBolVM filtro)
        {
            ResultadoTransmissaoArquivo dadosRetorno = new ResultadoTransmissaoArquivo();
            string nmArquivoRemessa = "";
            var codArquivo = IncluiDadosEmissaoBoleto(filtro);
            int codigoPriemriaInstrucaoCobranca = 0;
            int codigoSegundaInstrucaoCobranca = 0;
            decimal PercMulta = 0;
            decimal VLMultaDiaAtraso = 0;
            decimal PercJuros = 0;
            decimal VLJurosDiaAtraso = 0;

            var caminhoPadrao = _ope.GetOperadoraById(_user.cod_ope).Result;
            var pathBoleto = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, filtro.codigoEmpresa.ToString(), "PDFBoleto");
            var CaminhoRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, filtro.codigoEmpresa.ToString(), "ArquivoRemessaBoleto");
            if (!string.IsNullOrWhiteSpace(filtro.CaminhoArquivo))
            {
                CaminhoRemessa = filtro.CaminhoArquivo;
            }


            if (!Directory.Exists(pathBoleto))
                Directory.CreateDirectory(pathBoleto);
            
            var DadosBoleto = _emissaoBoleto.BuscaTodosBoletoByCodArquivo(codArquivo);

            var contacorrete = _conta.GetContaCorrenteById(Convert.ToInt32(filtro.codContaCorrente)).Result;
            var dadosEmpresa = _empresa.ConsultaEmpresaById(filtro.codigoEmpresa).Result;

            if (DadosBoleto.Count > 0)
            {
                var contaBancaria = new ContaBancaria
                {
                    Agencia = contacorrete.AGENCIA,
                    DigitoAgencia = contacorrete.DIGITOAGENCIA,
                    Conta = contacorrete.NROCONTACORRENTE,
                    DigitoConta = contacorrete.DIGITOCC,
                    CodigoBancoCorrespondente = 0,
                    OperacaoConta =  contacorrete.CODOPERACAOCC,
                    CarteiraPadrao = (contacorrete.CODBANCO == "237" && contacorrete.CARTEIRAREMESSA.ToString() == "9") ? "0" + contacorrete.CARTEIRAREMESSA.ToString() : contacorrete.CARTEIRAREMESSA.ToString(),
                    VariacaoCarteiraPadrao = contacorrete.VARIACAOCARTEIRA,
                    TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
                };
                _banco = Banco.Instancia(Convert.ToInt32(contacorrete.CODBANCO));
                _banco.Cedente = GerarCedente(contacorrete, dadosEmpresa, contaBancaria);
                List<ListaEmissaoBoletoVM> listaEmissaoBoleto = new List<ListaEmissaoBoletoVM>();

                MetodosGerais _MetodosGerais = new MetodosGerais(_arquivo);

                foreach (var bol in DadosBoleto)
                {
                    ListaEmissaoBoletoVM dados = new ListaEmissaoBoletoVM();


                    var cli = _cliente.BuscaClienteByID(bol.CodCliente).Result;
                    if (cli.COBRANCADIFERENCIADA == "N")
                    {
                            codigoPriemriaInstrucaoCobranca = Convert.ToInt32(contacorrete.CODPRIMEIRAINSTCOBRA);
                            codigoSegundaInstrucaoCobranca = Convert.ToInt32(contacorrete.CODSEGUNDAINSTCOBRA);
                            PercMulta = Convert.ToDecimal(contacorrete.PERCMULTA);
                            VLMultaDiaAtraso = Convert.ToDecimal(contacorrete.VLMULTADIAATRASO);
                            PercJuros = Convert.ToDecimal(contacorrete.PERCJUROS);
                            VLJurosDiaAtraso = Convert.ToDecimal(contacorrete.VLJUROSDIAATRASO);

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
                    
                    dados.CodigoOcorrencia = Helper.FormataInteiro(bol.codOcorrencia.ToString(), 2);
                    dados.DescricaoOcorrencia = "Remessa Registrar";

                    dados.DataEmissao = DateTime.Now;
                    dados.DataProcessamento = DateTime.Now;
                    dados.DataVencimento = bol.dtVencimento;
                    dados.ValorTitulo = bol.Valor;
                    dados.NossoNumero = (contacorrete.CODBANCO == "756") ? Helper.RemoveCaracterDireita(bol.NossoNumero, 7): bol.NossoNumero;
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
                    dados.nmBoleto = Helper.RemoveCaracterEspecial(bol.nmBoletoGerado);


                    dados.Sacado = APIBoletoSacadoVM.ToViewEmissaoBoleto(bol);

                    listaEmissaoBoleto.Add(dados);

                }

                var DadosArquivo = _arquivo.ReturnFileById(codArquivo).Result;
                nmArquivoRemessa = DadosArquivo.NMARQUIVO;
                var pathRemessa = Path.Combine(CaminhoRemessa, nmArquivoRemessa);

                //inicia o processo de geração do arquivo remessa
                var vm = APIBoletoVM.ToView(_banco, pathRemessa, pathBoleto, listaEmissaoBoleto, contacorrete.CODBANCO);
                vm.CodigoSequencialArquivo = codArquivo;
                vm.GeraArquivoRemessa = true;
                vm.GeraBoleto = true;

                vm.QuantidadePosicao = Convert.ToInt32(contacorrete.QTPOSICAOARQBOL);

                using (var client = new HttpClient())
                {
                    string caminho = "CriaArquivoRemessa";

                    string BaseAddress = @"https://www1.tln.com.br/API/PagNet/EmissaoBoleto/";
                    if (_user.ServidorDadosNetCard.ToUpper() == "NETUNO")
                    {
                        BaseAddress = @"https://www3.tln.com.br/API/PagNet/EmissaoBoleto/";
                    }

                    //BaseAddress = @"http://localhost:52327/";
                    if (client.BaseAddress == null)
                    {
                        client.BaseAddress = new Uri(BaseAddress);
                        client.Timeout = TimeSpan.FromMinutes(10);
                    }

                    var content = new StringContent(JsonConvert.SerializeObject(vm),
                                                    Encoding.UTF8,
                                                    "application/json");

                    HttpResponseMessage response = client.PostAsync(caminho, content).Result;
                    response.EnsureSuccessStatusCode();
                    var msgretorno = response.Content.ReadAsStringAsync().Result;

                    if (msgretorno != "Sucesso")
                    {
                        //processo para realizar o rollback
                        RollbackEmissaoBoleto(DadosBoleto, codArquivo, _user.cod_usu);
                        dadosRetorno.Resultado = false;
                        dadosRetorno.msgResultado = msgretorno;
                        
                    }
                    dadosRetorno.Resultado = true;
                    dadosRetorno.msgResultado = msgretorno;
                    dadosRetorno.nomeArquivo = nmArquivoRemessa;
                    dadosRetorno.CaminhoCompletoArquivo = pathRemessa;
                }
            }


            return dadosRetorno;
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
                    if (boleto.codOcorrencia != 1)
                    {
                        boleto.Status = "CANCELADO";
                        //cancela a soliciação de emissao do boleto
                        _emissaoBoleto.AtualizaBoleto(boleto);
                    }
                    var ListaFaturas = _emissaoBoleto.BuscaFaturamentoBySeuNumero(boleto.SeuNumero);

                    foreach (var fatura in ListaFaturas)
                    {
                        if (boleto.codOcorrencia != 1)
                        {
                            fatura.SEUNUMERO = null;
                        }
                        //volta o status dos faturamentos
                        fatura.STATUS = "EM_BORDERO";
                        
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

        private int IncluiDadosEmissaoBoleto(IBorderoBolVM model)
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
                    ListaFaturamentos.AddRange(_emissaoBoleto.BuscaTodosFaturamentoByCodBordero(bordero.codigoBordero));
                }

                //separa a lista de faturamento dos itens que já foram incluido no arquivo de remessa anteriormente.
                var listaBoletos = ListaFaturamentos.Where(x => x.SEUNUMERO != null).ToList();

                //Remove os itens que serão incluidos em um arquivo de remessa pela primeira vez.
                ListaFaturamentos.RemoveAll(x => x.SEUNUMERO != null);
                //if(ListaFaturamentos.Count == 0)
                //{
                //    return 0;
                //}
                //Pega o valor total que será incluído no arquivo
                var ValorTotalArquivo = ListaFaturamentos.Sum(x => x.VALOR);
                ValorTotalArquivo += listaBoletos.Sum(x => x.VALOR);

                int codArquivo = InsereNovoArquivo(model, ValorTotalArquivo, ListaFaturamentos.Count);

                var contacorrete = _conta.GetContaCorrenteById(model.codContaCorrente).Result;

                IncluiBoletosEditadosNoArquivoRemessa(listaBoletos, codArquivo, _user.cod_usu);

                //SEPARA POR CLIENTES PARA VERIFICAR AS REGRAS E INSERIR NO BD;
                var listaClientes = ListaFaturamentos.Select(x => x.CODCLIENTE).Distinct().ToList();
                foreach (var codCliente in listaClientes)
                {
                    var dadosCli = _cliente.BuscaClienteByID(Convert.ToInt32(codCliente)).Result;

                    //Se for para agrupar todos os faturamentos do cliente com vencimento para o mesmo dia
                    if (dadosCli.COBRANCADIFERENCIADA == "N")
                    {
                            AgruparFaturamentosDia = (contacorrete.AGRUPARFATURAMENTOSDIA == "S");
                            ValorBoleto = Convert.ToDecimal(contacorrete.TAXAEMISSAOBOLETO);
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
                                                       reg.DATVENCIMENTO,
                                                       reg.SEUNUMERO

                                                   } into g
                                                   select new
                                                   {
                                                       g.Key.DATVENCIMENTO,
                                                       g.Key.SEUNUMERO,
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
                                bol.NossoNumero = Helper.GeraNossoNumero(Faturamento.SEUNUMERO, codEmissaoBoleto, contacorrete.CODBANCO);
                                bol.codOcorrencia = 1;
                                bol.SeuNumero = Helper.GeraNumeroControle(codEmissaoBoleto, "");
                                bol.Valor = ValorTotalBoleto;
                                bol.dtSolicitacao = DateTime.Now;
                                bol.dtReferencia = DateTime.Now;
                                bol.dtSegundoDesconto = null;
                                bol.vlDesconto = null;
                                bol.vlSegundoDesconto = null;
                                bol.MensagemArquivoRemessa = "";
                                bol.MensagemInstrucoesCaixa = "";
                                bol.numControle = Helper.GeraNumeroControle(codEmissaoBoleto, "");
                                bol.OcorrenciaRetBol = "";
                                bol.DescricaoOcorrenciaRetBol = "";
                                bol.nmBoletoGerado = $"{Convert.ToInt32(codCliente)}-{dadosCli.NMCLIENTE} Venc_{Faturamento.DATVENCIMENTO.ToString("ddMMyyyy")} chave_{codEmissaoBoleto}";
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
                                _emissaoBoleto.IncluiLog(fatura, _user.cod_usu, "Faturamento incluíno no Arquivo de Remessa.");

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
                                bol.NossoNumero = Helper.GeraNossoNumero("", codEmissaoBoleto, contacorrete.CODBANCO);
                                bol.codOcorrencia = 1;
                                bol.SeuNumero = Helper.GeraNumeroControle(codEmissaoBoleto, Faturamento.SEUNUMERO);
                                bol.Valor = ValorTotalBoleto;
                                bol.dtSolicitacao = DateTime.Now;
                                bol.dtReferencia = DateTime.Now;
                                bol.dtSegundoDesconto = null;
                                bol.vlDesconto = null;
                                bol.vlSegundoDesconto = null;
                                bol.MensagemArquivoRemessa = "";
                                bol.MensagemInstrucoesCaixa = Faturamento.MENSAGEMINSTRUCOESCAIXA;
                                bol.numControle = Helper.GeraNumeroControle(codEmissaoBoleto, "");
                                bol.OcorrenciaRetBol = "";
                                bol.DescricaoOcorrenciaRetBol = "";
                                bol.nmBoletoGerado = $"{Convert.ToInt32(codCliente)}-{dadosCli.NMCLIENTE} Venc_{Faturamento.DATVENCIMENTO.ToString("ddMMyyyy")} chave_{bol.codEmissaoBoleto}";
                                bol.CODARQUIVO = codArquivo;
                                bol.BOLETOENVIADO = "N";

                                _emissaoBoleto.IncluiBoleto(bol);

                                //Atualiza os Faturamentos
                                Faturamento.SEUNUMERO = bol.SeuNumero;
                           
                            Faturamento.STATUS = bol.Status;

                            _emissaoBoleto.AtualizaFaturamento(Faturamento);
                            _emissaoBoleto.IncluiLog(Faturamento, _user.cod_usu, "Faturamento incluíno no Arquivo de Remessa.");

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
        private int InsereNovoArquivo(IBorderoBolVM model, decimal vlArquivo, int TotalRegistros)
        {
            try
            {

                MetodosGerais _MetodosGerais = new MetodosGerais(_arquivo);

                string nmArquivoRemessa = "BOL_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".rem";

                var caminhoPadrao = _ope.GetOperadoraById(_user.cod_ope).Result;

                var CaminhoRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, model.codigoEmpresa.ToString(), "ArquivoRemessaBoleto");


                if (!Directory.Exists(CaminhoRemessa))
                    Directory.CreateDirectory(CaminhoRemessa);


                int nroSeqArquivo = _MetodosGerais.RetornaNroSeqArquivo();

                var contacorrete = _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente)).Result;

                //INSERI DADOS DO ARQUIVO PARA ESTE BOLETO
                var Status = "PENDENTE_REGISTRO";
                var TipArqu = "BOL";

                int codArquivo = _MetodosGerais.InseriArquivo(CaminhoRemessa, nmArquivoRemessa, nroSeqArquivo,
                                            vlArquivo, TotalRegistros, Status, TipArqu, contacorrete.CODBANCO);

                return codArquivo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal Cedente GerarCedente(PAGNET_CONTACORRENTE dados, PAGNET_CADEMPRESA empresa, ContaBancaria contaBancaria)
        {
            try
            {
                Cedente ced = new Cedente();
                
                ced.CPFCNPJ = dados.CPFCNPJ;
                ced.Nome = dados.NMEMPRESA;
                ced.Codigo = Convert.ToString(dados.CODCEDENTE);
                ced.CodigoDV = dados.DVCODCEDENTE;
                    
                ced.CodigoTransmissao = Convert.ToString(dados.CODTRANSMISSAO);
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

        /// <summary>
        /// Processo para alterar o código do arquivo para ser incluído em um novo arquivo de remessa.
        /// </summary>
        private void IncluiBoletosEditadosNoArquivoRemessa(List<PAGNET_EMISSAOFATURAMENTO> listaBoletos, int codArquivo, int codUsuario)
        {
            int codbordero = 0;
            foreach (var itemBol in listaBoletos)
            {
                var Boleto = _emissaoBoleto.BuscaBoletoBySeuNumero(itemBol.SEUNUMERO.Trim());
                var dadosCli = _cliente.BuscaClienteByID(Boleto.CodCliente).Result;
                if (Boleto != null)
                {
                    Boleto.Status = "PENDENTE_REGISTRO";
                    Boleto.CODARQUIVO = codArquivo;
                    Boleto.BOLETOENVIADO = "N";
                    Boleto.nmBoletoGerado = $"{Boleto.CodCliente}-{dadosCli.NMCLIENTE} Venc_{Boleto.dtVencimento.ToString("ddMMyyyy")} chave_{Boleto.codEmissaoBoleto}";

                    _emissaoBoleto.AtualizaBoleto(Boleto);
                    var Faturamento = _emissaoBoleto.BuscaFaturamentoBySeuNumero(itemBol.SEUNUMERO);
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


        public void GeraBoletoPDF(IFiltroCobranca filtro)
        {
            try
            {
                int codigoPriemriaInstrucaoCobranca = 0;
                int codigoSegundaInstrucaoCobranca = 0;
                decimal PercMulta = 0;
                decimal VLMultaDiaAtraso = 0;
                decimal PercJuros = 0;
                decimal VLJurosDiaAtraso = 0;
                PAGNET_EMISSAOBOLETO DadosBoleto = new PAGNET_EMISSAOBOLETO();

                if(filtro.codigoEmissaoBoleto != 0)
                {
                    DadosBoleto = _emissaoBoleto.BuscaBoletoByID(filtro.codigoEmissaoBoleto);
                }
                else
                {
                    var dadosFatura = _emissaoBoleto.BuscaFaturamentoByID(filtro.codigoFatura);
                    DadosBoleto = _emissaoBoleto.BuscaBoletoBySeuNumero(dadosFatura.SEUNUMERO);
                }

                //int nroSeqArquivo = RetornaNroSeqArquivo();
                var contacorrete = _conta.GetContaCorrenteById(DadosBoleto.codContaCorrente).Result;
                var dadosEmpresa = _empresa.ConsultaEmpresaById(contacorrete.CODEMPRESA).Result;

                var contaBancaria = new ContaBancaria
                {
                    Agencia = contacorrete.AGENCIA,
                    DigitoAgencia = contacorrete.DIGITOAGENCIA,
                    Conta = contacorrete.NROCONTACORRENTE,
                    DigitoConta = contacorrete.DIGITOCC,
                    CarteiraPadrao = (contacorrete.CODBANCO == "237") ? "0" + contacorrete.CARTEIRAREMESSA.ToString() : contacorrete.CARTEIRAREMESSA.ToString(),
                    VariacaoCarteiraPadrao = contacorrete.VARIACAOCARTEIRA,
                    TipoCarteiraPadrao = TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = TipoImpressaoBoleto.Empresa
                };
                _banco = Banco.Instancia(Convert.ToInt32(contacorrete.CODBANCO));
                _banco.Cedente = GerarCedente(contacorrete, dadosEmpresa, contaBancaria);
                List<ListaEmissaoBoletoVM> listaEmissaoBoleto = new List<ListaEmissaoBoletoVM>();

                ListaEmissaoBoletoVM dados = new ListaEmissaoBoletoVM();

                var cli = DadosBoleto.PAGNET_CADCLIENTE;
                if (cli.COBRANCADIFERENCIADA == "N")
                {
                        codigoPriemriaInstrucaoCobranca = Convert.ToInt32(contacorrete.CODPRIMEIRAINSTCOBRA);
                        codigoSegundaInstrucaoCobranca = Convert.ToInt32(contacorrete.CODSEGUNDAINSTCOBRA);
                        PercMulta = Convert.ToDecimal(contacorrete.PERCMULTA);
                        VLMultaDiaAtraso = Convert.ToDecimal(contacorrete.VLMULTADIAATRASO);
                        PercJuros = Convert.ToDecimal(contacorrete.PERCJUROS);
                        VLJurosDiaAtraso = Convert.ToDecimal(contacorrete.VLJUROSDIAATRASO);

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

                dados.CodigoOcorrencia = Helper.FormataInteiro(DadosBoleto.codOcorrencia.ToString(), 2);
                dados.DescricaoOcorrencia = "Remessa Registrar";

                dados.DataEmissao = DateTime.Now;
                dados.DataProcessamento = DateTime.Now;
                dados.DataVencimento = Convert.ToDateTime(DadosBoleto.dtVencimento);
                dados.ValorTitulo = DadosBoleto.Valor;
                dados.NossoNumero = Helper.GeraNossoNumero("0", DadosBoleto.codEmissaoBoleto, contacorrete.CODBANCO);
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

                _emissaoBoleto.IncluiLogBySeuNumero(DadosBoleto.SeuNumero, DadosBoleto.Status, _user.cod_usu, "Gerado PDF do Boleto");


                //inicia o processo de geração do arquivo remessa
                var vm = APIBoletoVM.ToView(_banco, filtro.caminhoArquivo, filtro.caminhoArquivo, listaEmissaoBoleto, contacorrete.CODBANCO);               
                vm.GeraArquivoRemessa = false;
                vm.GeraBoleto = true;

                using (var client = new HttpClient())
                {
                    string caminho = "CriaArquivoRemessa";

                    if (client.BaseAddress == null)
                    {
                        //client.BaseAddress = new Uri("http://localhost:52327/");
                        client.BaseAddress = new Uri("https://www3.tln.com.br/services/APIPagNetGeracaoBoleto/");
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
        public List<SolicitacaoBoletoVM> CarregaDadosArquivoRetorno(IFiltroCobranca filtro)
        {
            try
            {
                APIDadosRetArquivo vm = new APIDadosRetArquivo();
                vm.caminhoArquivo = filtro.caminhoArquivo;

                List<SolicitacaoBoletoVM> RetronoBoletos = new List<SolicitacaoBoletoVM>();

                using (var client = new HttpClient())
                {
                    string caminho = "RetornaDadosArquivo";

                    string BaseAddress = @"https://www1.tln.com.br/API/PagNet/EmissaoBoleto/";
                    if (_user.ServidorDadosNetCard.ToUpper() == "NETUNO")
                    {
                        BaseAddress = @"https://www3.tln.com.br/API/PagNet/EmissaoBoleto/";
                    }

                    //BaseAddress = @"http://localhost:52327/";
                    if (client.BaseAddress == null)
                    {
                        client.BaseAddress = new Uri(BaseAddress);
                        client.Timeout = TimeSpan.FromMinutes(10);
                    }

                    var content = new StringContent(JsonConvert.SerializeObject(vm),
                                                    Encoding.UTF8,
                                                    "application/json");

                    HttpResponseMessage response = client.PostAsync(caminho, content).Result;
                    response.EnsureSuccessStatusCode();

                    var json = response.Content.ReadAsStringAsync().Result;


                    APIBoletoVM dadosRetBol = JsonConvert.DeserializeObject<APIBoletoVM>(json);

                    if (dadosRetBol.ListaBoletos.Count > 0)
                    {
                        foreach (var bol in dadosRetBol.ListaBoletos)
                        {
                            var BoletoRegistrado = _emissaoBoleto.BuscaBoletoBySeuNumero(bol.NumeroControleParticipante.Trim());
                            if (BoletoRegistrado != null)
                            {
                                if (BoletoRegistrado.Status == "CANCELADO")
                                {
                                    BoletoRegistrado.DescricaoOcorrenciaRetBol = "Boleto cancelado.";

                                }
                                else
                                {
                                    BoletoRegistrado.Status = Helper.OcorrenciaBoletoCnab240(bol.CodigoOcorrencia);
                                    BoletoRegistrado.DescricaoOcorrenciaRetBol = ReturnDescricaoRetBoleto(bol.CodOcorrenciaAuxiliar, bol.CodigoOcorrencia);
                                    _emissaoBoleto.AtualizaBoleto(BoletoRegistrado);
                                    var pedidosFaturamento = _emissaoBoleto.BuscaFaturamentoBySeuNumero(BoletoRegistrado.SeuNumero);
                                    foreach (var fatura in pedidosFaturamento)
                                    {
                                        fatura.STATUS = BoletoRegistrado.Status;
                                        fatura.CODCONTACORRENTE = BoletoRegistrado.codContaCorrente;

                                        if (BoletoRegistrado.Status == "LIQUIDADO" || BoletoRegistrado.Status == "BAIXADO")
                                        {
                                            fatura.DATPGTO = bol.DataVencimento;
                                            fatura.VLPGTO = bol.ValorTitulo;

                                            if (fatura.TIPOFATURAMENTO == "CARGA")
                                            {
                                                _proc.RegistraPagamentoCargaNetCard(Convert.ToInt32(fatura.CODCLIENTE), Convert.ToInt32(fatura.NROREF_NETCARD));
                                            }
                                        }
                                        _emissaoBoleto.AtualizaFaturamento(fatura);
                                        _emissaoBoleto.IncluiLog(fatura, _user.cod_usu, "Leitura do arquivo de retorno");

                                    }

                                }

                                SolicitacaoBoletoVM boVm = new SolicitacaoBoletoVM();
                                boVm.chkBoleto = true;
                                boVm.codEmissaoBoleto = BoletoRegistrado.codEmissaoBoleto;
                                boVm.nomeBoleto = BoletoRegistrado.nmBoletoGerado;
                                //codBordero = Convert.ToString(x.CODBORDERO),
                                boVm.Status = BoletoRegistrado.Status.Replace("_", " ");
                                boVm.codigoCliente = BoletoRegistrado.CodCliente.ToString();
                                boVm.nomeCliente = BoletoRegistrado.CodCliente + " - " + Helper.FormataTexto(BoletoRegistrado.PAGNET_CADCLIENTE.NMCLIENTE, 39).Trim();
                                boVm.cnpj = Helper.FormataCPFCnPj(BoletoRegistrado.PAGNET_CADCLIENTE.CPFCNPJ);
                                boVm.Email = Convert.ToString(BoletoRegistrado.PAGNET_CADCLIENTE.EMAIL);
                                boVm.dtVencimento = BoletoRegistrado.dtVencimento.ToShortDateString();
                                boVm.dtEmissao = Convert.ToDateTime(BoletoRegistrado.dtSolicitacao).ToShortDateString();
                                boVm.Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", BoletoRegistrado.Valor);
                                boVm.SeuNumero = BoletoRegistrado.SeuNumero;
                                boVm.DescricaoOcorrenciaRet = BoletoRegistrado.DescricaoOcorrenciaRetBol;
                                boVm.MsgRetorno = BoletoRegistrado.DescricaoOcorrenciaRetBol;

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
                        descricaoRet = Helper.OcorrenciaRetLiquidacaoBoleto(codigo);
                    }
                    else if (CodigoOcorrencia == "09")
                    {
                        descricaoRet = Helper.OcorrenciaRetBaixaBoleto(codigo);
                    }
                }

            }

            return descricaoRet;
        }
        public void AtualizaSaldoContaCorrente(int codcontacorrente, int codigo)
        {
            try
            {
                decimal saldoContaCorrente = 0;
                decimal CalculoSaldoAtual = 0;

                var dadosContaCorrente = _conta.GetContaCorrenteById(codcontacorrente).Result;
                saldoContaCorrente = _conta.RetornaSaldoAtual(codcontacorrente);

                var dadosFaturamento = _emissaoBoleto.BuscaFaturamentoByID(codigo);
                var valorRecebido = (decimal)dadosFaturamento.VLPGTO;
                CalculoSaldoAtual = saldoContaCorrente + valorRecebido;

                _conta.InseriNovoSaldo(codcontacorrente, dadosContaCorrente.CODEMPRESA, CalculoSaldoAtual);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
