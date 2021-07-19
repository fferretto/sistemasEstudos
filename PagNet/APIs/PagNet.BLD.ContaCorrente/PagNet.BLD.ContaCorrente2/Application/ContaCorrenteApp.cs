
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.BLD.ContaCorrente.Abstraction.Interface;
using PagNet.BLD.ContaCorrente.Abstraction.Interface.Model;
using PagNet.BLD.ContaCorrente.Abstraction.Model;
using PagNet.BLD.ContaCorrente2.ModelAuxiliar;
using PagNet.BLD.ContaCorrente2.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Telenet.BusinessLogicModel;

namespace PagNet.BLD.ContaCorrente2.Application
{
    public class ContaCorrenteApp : Service<IContextoApp>, IContaCorrenteApp
    {
        protected IHttpContextAccessor _ContextAccessor { get; }
        private readonly IParametrosApp _user;
        private readonly IPAGNET_CONTACORRENTEService _conta;
        private readonly IPAGNET_TRANSMISSAOARQUIVOService _transmissaoArquivo;
        private readonly IPAGNET_CADCLIENTEService _cliente;
        private readonly IPAGNET_BANCOService _banco;
        private readonly IPAGNET_CADEMPRESAService _empresa;
        private readonly IPAGNET_EMISSAO_TITULOSService _emissaoTitulos;
        private readonly IPAGNET_EMISSAOBOLETOService _emissaoBoleto;
        private readonly IPAGNET_CADPLANOCONTASService _PlanoContas;
        private readonly IPAGNET_BORDERO_PAGAMENTOService _borderoPGTO;
        private readonly IPAGNET_BORDERO_BOLETOService _borderoBoleto;
        private readonly IPAGNET_TITULOS_PAGOSService _pagamento;
        private readonly IPAGNET_ARQUIVOService _arquivo;
        private readonly IOPERADORAService _ope;
        private readonly IPAGNET_USUARIOService _usuPagNet;
        //private readonly IPAGNET_USUARIO_CONCENTRADORService _usuPagNet;
        private readonly IPAGNET_INSTRUCAOCOBRANCAService _InstrucaoCobranca;

        public ContaCorrenteApp(IContextoApp contexto,
                                IHttpContextAccessor contextAccessor,
                                IParametrosApp user, 
                                IPAGNET_CONTACORRENTEService conta,
                                IPAGNET_TRANSMISSAOARQUIVOService transmissaoArquivo,
                                IPAGNET_CADCLIENTEService cliente,
                                IPAGNET_EMISSAOBOLETOService emissaoBoleto,
                                IPAGNET_CADPLANOCONTASService PlanoContas,
                                IPAGNET_BANCOService banco,
                                IPAGNET_ARQUIVOService arquivo,
                                IPAGNET_BORDERO_PAGAMENTOService borderoPGTO,
                                IPAGNET_TITULOS_PAGOSService pagamento,
                                IPAGNET_EMISSAO_TITULOSService emissaoTitulos,
                                IPAGNET_BORDERO_BOLETOService borderoBoleto,
                                IOPERADORAService ope,
                                IPAGNET_CADEMPRESAService empresa,
                                IPAGNET_USUARIOService usuPagNet,
                                IPAGNET_INSTRUCAOCOBRANCAService InstrucaoCobranca
                                )
            : base(contexto)
        {
            _user = user;
            _conta = conta;
            _transmissaoArquivo = transmissaoArquivo;
            _banco = banco;
            _empresa = empresa;
            _emissaoTitulos = emissaoTitulos;
            _emissaoBoleto = emissaoBoleto;
            _PlanoContas = PlanoContas;
            _borderoPGTO = borderoPGTO;
            _arquivo = arquivo;
            _borderoBoleto = borderoBoleto;
            _pagamento = pagamento;
            _cliente = cliente;
            _ope = ope;
            _usuPagNet = usuPagNet;
            _ContextAccessor = contextAccessor;
            _InstrucaoCobranca = InstrucaoCobranca;
            //_recebimentoApp = recebimentoApp;
            //_cadastro = cadastro;
            //_pagApp = pagApp;
        }

        public List<ConsultaContaCorrenteVM> GetAllContaCorrente(int CodEmpresa)
        {
            try
            {
                List<ConsultaContaCorrenteVM> retorno = new List<ConsultaContaCorrenteVM>();

                var dadosContaCorente = _conta.GetAllContaCorrente(CodEmpresa).ToList();
                ConsultaContaCorrenteVM item = new ConsultaContaCorrenteVM();
                foreach (var x in dadosContaCorente)
                {
                    item = new ConsultaContaCorrenteVM();
                    item.codContaCorrente = x.CODCONTACORRENTE;
                    item.nmContaCorrente = x.NMCONTACORRENTE.Trim();
                    item.nroContaCorrente = x.NROCONTACORRENTE.Trim() + "-" + x.DIGITOCC.Trim();
                    item.Agencia = x.AGENCIA.Trim() + "-" + x.DIGITOAGENCIA.Trim();
                    retorno.Add(item);
                }
                
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ContaCorrenteVm GetContaCorrenteById(int id, int codEmpresa)
        {
            try
            {
                ContaCorrenteVm retorno = new ContaCorrenteVm();

                if (id > 0)
                { 
                    var conta = _conta.GetContaCorrenteById((int)id).Result;
                    codEmpresa = conta.CODEMPRESA;

                    var banco = _banco.getBancoByID(conta.CODBANCO).Result;
                    var dadosTransmissao = _transmissaoArquivo.BuscaTodasFormaTransmissao(conta.CODCONTACORRENTE);

                    retorno.codContaCorrente = conta.CODCONTACORRENTE;
                    retorno.nmContaCorrente = conta.NMCONTACORRENTE;
                    retorno.CodBanco = conta.CODBANCO;
                    retorno.nmBanco = banco.NMBANCO;
                    retorno.nmEmpresa = conta.NMEMPRESA;
                    retorno.CpfCnpj = Helper.FormataCPFCnPj(conta.CPFCNPJ);
                    retorno.nroContaCorrente = conta.NROCONTACORRENTE;
                    retorno.DigitoCC = conta.DIGITOCC;
                    retorno.Agencia = conta.AGENCIA;
                    retorno.DigitoAgencia = conta.DIGITOAGENCIA;
                    retorno.codOperacaoCC = Convert.ToString(conta.CODOPERACAOCC);
                    retorno.CodTransmissao = conta.CODTRANSMISSAO;
                    retorno.CarteiraRemessa = Convert.ToString(conta.CARTEIRAREMESSA);
                    retorno.VariacaoCarteira = Convert.ToString(conta.VARIACAOCARTEIRA);
                    retorno.CodConvenioPag = conta.CODCONVENIOPAG;
                    retorno.valTED = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VALTED).Replace("R$", "");
                    retorno.ValMinPGTO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VALMINIMOCC).Replace("R$", "");
                    retorno.ValMinTED = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VALMINIMOTED).Replace("R$", "");

                    retorno.teraJuros = (conta.COBRAJUROS == "S");
                    retorno.ValorJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VLJUROSDIAATRASO).Replace("R$", ""); 
                    retorno.PercJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.PERCJUROS).Replace("R$", ""); 
                    retorno.teraMulta = (conta.COBRAJUROS == "S");
                    retorno.valorMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VLMULTADIAATRASO).Replace("R$", "");
                    retorno.PercMulta = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.PERCMULTA).Replace("R$", "");
                    retorno.codigoPrimeiraInscricaoCobraca = Convert.ToString(conta.CODPRIMEIRAINSTCOBRA);
                    retorno.NomePrimeiraInscricaoCobraca = _InstrucaoCobranca.GetInstrucaoCobrancaById((int)conta.CODPRIMEIRAINSTCOBRA);
                    retorno.codigoSegundaInscricaoCobraca = Convert.ToString(conta.CODSEGUNDAINSTCOBRA);
                    retorno.NomeSegundaInscricaoCobraca = _InstrucaoCobranca.GetInstrucaoCobrancaById((int)conta.CODSEGUNDAINSTCOBRA);
                    retorno.TaxaEmissaoBoleto = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.TAXAEMISSAOBOLETO).Replace("R$", ""); 
                    retorno.AgruparFaturamentosDia = conta.AGRUPARFATURAMENTOSDIA == "S";
                    retorno.qtPosicaoArqPGTO = (conta.QTPOSICAOARQPGTO == null) ? "0" : conta.QTPOSICAOARQPGTO.ToString();
                    retorno.qtPosicaoArqBoleto = (conta.QTPOSICAOARQBOL == null) ? "0" : conta.QTPOSICAOARQBOL.ToString();
                    retorno.codigoCedente = Convert.ToString(conta.CODCEDENTE);
                    retorno.digitoCodigoCedente = Convert.ToString(conta.DVCODCEDENTE);


                    if (dadosTransmissao.Count > 0)
                    {
                        var TransmissaoPG = dadosTransmissao.Where(x => x.TIPOARQUIVO.ToUpper() == "PAGAMENTO").FirstOrDefault();
                        var TransmissaoBol = dadosTransmissao.Where(x => x.TIPOARQUIVO.ToUpper() == "RECEBIMENTO").FirstOrDefault();

                        if (TransmissaoPG != null)
                        {
                            retorno.bPagamento = true;
                            retorno.tipoArquivoPG = TransmissaoPG.TIPOARQUIVO;
                            retorno.formaTransmissaoPG = TransmissaoPG.FORMATRANSMISSAO;
                            retorno.loginTransmissaoPG = TransmissaoPG.LOGINTRANSMISSAO;
                            retorno.senhaTransmissaoPG = TransmissaoPG.SENHATRANSMISSAO;
                            retorno.caminhoRemessaPG = TransmissaoPG.CAMINHOREM;
                            retorno.caminhoRetornoPG = TransmissaoPG.CAMINHORET;
                            retorno.caminhoAuxiliarPG = TransmissaoPG.CAMINHOAUX;
                        }
                        if(TransmissaoBol != null)
                        {
                            retorno.bBoleto = true;
                            retorno.tipoArquivoBol = TransmissaoBol.TIPOARQUIVO;
                            retorno.formaTransmissaoBol = TransmissaoBol.FORMATRANSMISSAO;
                            retorno.loginTransmissaoBol = TransmissaoBol.LOGINTRANSMISSAO;
                            retorno.senhaTransmissaoBol = TransmissaoBol.SENHATRANSMISSAO;
                            retorno.caminhoRemessaBol = TransmissaoBol.CAMINHOREM;
                            retorno.caminhoRetornoBol = TransmissaoBol.CAMINHORET;
                            retorno.caminhoAuxiliarBol = TransmissaoBol.CAMINHOAUX;
                        }
                    }

                }

                var subrede = _empresa.ConsultaEmpresaById(codEmpresa).Result;
                retorno.codEmpresa = subrede.CODEMPRESA.ToString();
                retorno.nmEmpresaPagNet = subrede.NMFANTASIA.ToString();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Salvar(IContaCorrenteVm model)
        {
            bool tudoCerto;
            PAGNET_CONTACORRENTE contaCorrente;
            model.nroContaCorrente = Helper.RemoveCaracteres(model.nroContaCorrente);

            if (model.codContaCorrente == 0)
            {
                contaCorrente = new PAGNET_CONTACORRENTE();
                var ContaJaCadastrado = _conta.bValidaContaCadastrada(model.CodBanco, model.Agencia, model.DigitoAgencia, model.nroContaCorrente, model.DigitoCC, Convert.ToInt32(model.codEmpresa));
                if (ContaJaCadastrado)
                {
                    return "Já existe esta conta corrente cadastrada.";
                }
            }
            else
            {
                contaCorrente = _conta.GetContaCorrenteById(model.codContaCorrente).Result;
            }

            try
            {

                contaCorrente.NMCONTACORRENTE = model.nmContaCorrente.Trim();
                contaCorrente.NMEMPRESA = model.nmEmpresa.Trim();
                contaCorrente.CPFCNPJ = Helper.RemoveCaracteres(model.CpfCnpj);
                contaCorrente.CODBANCO = model.CodBanco.ToString();
                contaCorrente.NROCONTACORRENTE = model.nroContaCorrente.Trim();
                contaCorrente.DIGITOCC = model.DigitoCC.Trim();
                contaCorrente.CODOPERACAOCC = Convert.ToString(model.codOperacaoCC);
                contaCorrente.CODEMPRESA = Convert.ToInt32(model.codEmpresa);
                contaCorrente.CONTAMOVIEMNTO = "";
                contaCorrente.AGENCIA = model.Agencia;
                contaCorrente.DIGITOAGENCIA = model.DigitoAgencia.Trim();
                contaCorrente.CODCONVENIOPAG = Convert.ToString(model.CodConvenioPag).Trim();
                contaCorrente.CODTRANSMISSAO = Convert.ToString(model.CodTransmissao);
                contaCorrente.CARTEIRAREMESSA = Convert.ToString(model.CarteiraRemessa);
                contaCorrente.VARIACAOCARTEIRA = Convert.ToString(model.VariacaoCarteira);
                contaCorrente.ATIVO = "S";
                contaCorrente.VALTED = Helper.TrataDecimal(model.valTED);
                contaCorrente.VALMINIMOCC = Helper.TrataDecimal(model.ValMinPGTO);
                contaCorrente.VALMINIMOTED = Helper.TrataDecimal(model.ValMinTED);


                contaCorrente.COBRAJUROS = (model.teraJuros)?"S":"N";
                contaCorrente.VLJUROSDIAATRASO = Helper.TrataDecimal(model.ValorJuros); 
                contaCorrente.PERCJUROS = Helper.TrataDecimal(model.PercJuros); 
                contaCorrente.COBRAJUROS = (model.teraMulta) ? "S" : "N"; 
                contaCorrente.VLMULTADIAATRASO = Helper.TrataDecimal(model.valorMulta); 
                contaCorrente.PERCMULTA = Helper.TrataDecimal(model.PercMulta); 
                contaCorrente.CODPRIMEIRAINSTCOBRA = Convert.ToInt32(model.codigoPrimeiraInscricaoCobraca);
                contaCorrente.CODSEGUNDAINSTCOBRA = Convert.ToInt32(model.codigoSegundaInscricaoCobraca);
                contaCorrente.TAXAEMISSAOBOLETO = Helper.TrataDecimal(model.TaxaEmissaoBoleto);
                contaCorrente.AGRUPARFATURAMENTOSDIA = (model.AgruparFaturamentosDia) ? "S" : "N";
                contaCorrente.QTPOSICAOARQPGTO = Convert.ToInt32(model.qtPosicaoArqPGTO);
                contaCorrente.QTPOSICAOARQBOL = Convert.ToInt32(model.qtPosicaoArqBoleto);

                contaCorrente.CODCEDENTE = Convert.ToString(model.codigoCedente);
                contaCorrente.DVCODCEDENTE = Convert.ToString(model.digitoCodigoCedente);


                if (model.codContaCorrente == 0)
                    tudoCerto = _conta.IncluiContaCorrente(contaCorrente).Result;
                else
                    tudoCerto = _conta.AtualizaContaCorrente(contaCorrente).Result;

                //decimal saldoTela = 0;
                //model.SaldoConta = model.SaldoConta ?? "";
                //model.SaldoConta = (string.IsNullOrWhiteSpace(model.SaldoConta)) ? "0" : model.SaldoConta;

                //if (model.SaldoConta.IndexOf("-") >= 0)
                //{
                //    var saldo = model.SaldoConta.Replace("-", "");
                //    saldo = saldo.Trim();
                //    var novosaldo = Convert.ToDecimal(saldo);
                //    saldoTela = -1 * novosaldo;
                //}
                //else
                //{
                //    saldoTela = Convert.ToDecimal(model.SaldoConta);
                //}

                //if (saldoTela > 0)
                //{
                //    _conta.InseriNovoSaldo(contaCorrente.CODCONTACORRENTE, contaCorrente.CODEMPRESA, Convert.ToDecimal(model.SaldoConta));
                //}

                //Grava forma de transmissao
                _transmissaoArquivo.RemoveTodasFormasTransmissao(contaCorrente.CODCONTACORRENTE);

                //Forma transmissão de pagamento
                if (model.bPagamento)
                {
                    PAGNET_TRANSMISSAOARQUIVO TA = new PAGNET_TRANSMISSAOARQUIVO();
                    TA.CODCONTACORRENTE = contaCorrente.CODCONTACORRENTE;
                    TA.TIPOARQUIVO = "PAGAMENTO";
                    TA.FORMATRANSMISSAO = "MANUAL";
                    TA.LOGINTRANSMISSAO = model.loginTransmissaoPG;
                    TA.SENHATRANSMISSAO = model.senhaTransmissaoPG;
                    TA.CAMINHOREM = model.caminhoRemessaPG;
                    TA.CAMINHORET = model.caminhoRetornoPG;
                    TA.CAMINHOAUX = model.caminhoAuxiliarPG;
                    _transmissaoArquivo.IncluiFormaTransmissao(TA);
                }
                //Forma transmissão de Boleto
                if (model.bBoleto)
                {
                    PAGNET_TRANSMISSAOARQUIVO TA = new PAGNET_TRANSMISSAOARQUIVO();
                    TA.CODCONTACORRENTE = contaCorrente.CODCONTACORRENTE;
                    TA.TIPOARQUIVO = "RECEBIMENTO";
                    TA.FORMATRANSMISSAO = "MANUAL";
                    TA.LOGINTRANSMISSAO = model.loginTransmissaoBol;
                    TA.SENHATRANSMISSAO = model.senhaTransmissaoBol;
                    TA.CAMINHOREM = model.caminhoRemessaBol;
                    TA.CAMINHORET = model.caminhoRetornoBol;
                    TA.CAMINHOAUX = model.caminhoAuxiliarBol;
                    _transmissaoArquivo.IncluiFormaTransmissao(TA);
                }


                return "Conta corrente salva com sucesso.";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void Desativar(int CodContaCorrente)
        {
            var resultado = new Dictionary<string, string>();

            bool tudoCerto = false;
            try
            {
                tudoCerto = _conta.Desativa(CodContaCorrente).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RetornoDDLVM> GetContaCorrente(int codEmpresa)
        {
            var lista = _conta.GetHashContaCorrente(codEmpresa).ToList();

            RetornoDDLVM ddl = new RetornoDDLVM();
            List<RetornoDDLVM> listaDdl= new List<RetornoDDLVM>();
            foreach (var item in lista)
            {
                ddl = new RetornoDDLVM();
                ddl.Valor = item[0].ToString();
                ddl.Descricao = item[1].ToString();
                listaDdl.Add(ddl);
            }

            return listaDdl;
        }
        public List<RetornoDDLVM> GetBanco()
        {
            var lista = _banco.GetBancoAtivos().ToList();
            lista.Insert(0, new object[] { "0", " " });

            RetornoDDLVM ddl = new RetornoDDLVM();
            List<RetornoDDLVM> listaDdl = new List<RetornoDDLVM>();
            foreach (var item in lista)
            {
                ddl = new RetornoDDLVM();
                ddl.Valor = item[0].ToString();
                ddl.Descricao = item[1].ToString();
                listaDdl.Add(ddl);
            }

            return listaDdl;
        }
        public List<RetornoDDLVM> GetContaCorrentePagamento(int codEmpresa)
        {
            var lista = _conta.GetContaCorrentePagamento(codEmpresa).ToList();
            //lista.Insert(0, new object[] { "0", " " });

            RetornoDDLVM ddl = new RetornoDDLVM();
            List<RetornoDDLVM> listaDdl = new List<RetornoDDLVM>();
            foreach (var item in lista)
            {
                ddl = new RetornoDDLVM();
                ddl.Valor = item[0].ToString();
                ddl.Descricao = item[1].ToString();
                listaDdl.Add(ddl);
            }

            return listaDdl;
        }
        public List<RetornoDDLVM> GetContaCorrenteBoleto(int codEmpresa)
        {
            var lista = _conta.GetContaCorrenteBoleto(codEmpresa).ToList();
            //lista.Insert(0, new object[] { "0", " " });

            RetornoDDLVM ddl = new RetornoDDLVM();
            List<RetornoDDLVM> listaDdl = new List<RetornoDDLVM>();
            foreach (var item in lista)
            {
                ddl = new RetornoDDLVM();
                ddl.Valor = item[0].ToString();
                ddl.Descricao = item[1].ToString();
                listaDdl.Add(ddl);
            }

            return listaDdl;
        }
        public BancoVM GetBancoByCodContaCorrente(int codContaCorrente)
        {
            BancoVM banco = new BancoVM();

            var contacorrente = _conta.GetContaCorrenteById(codContaCorrente).Result;

            var dadosBanco = _banco.getBancoByID(contacorrente.CODBANCO).Result;
            
            banco.CODBANCO = dadosBanco.CODBANCO;
            banco.NMBANCO = dadosBanco.NMBANCO;

            return banco;
        }        
        public string BuscaBancoByID(string codBanco)
        {
            var dadosBanco = _banco.getBancoByID(codBanco).Result;

            if (dadosBanco == null)
            {
                return "Banco não Cadastrado";
            }
            return dadosBanco.NMBANCO;
        }
        public ResultadoTransmissaoArquivo GeraArquivoRemessaHomologacao(IDadosHomologarContaCorrenteVm model)
        {
            ResultadoTransmissaoArquivo resultado = new ResultadoTransmissaoArquivo();
            try
            {

                if (model.TipoArquivo == "PAG")
                {
                    //Verifica se existe algum título não baixado e cancela ele. Só pode existir 1 título em aberto para homologação.
                    CancelaTituloHomologacao(model.CodigoContaCorrente, _user.cod_usu);

                    var codigoTitulo = InclusaoTitulo(model.codigoFavorecido, model.codigoEmpresa, _user.cod_usu, model.CodigoContaCorrente);
                    var codigoBorderoPGTO = IncluiTituloBordero(codigoTitulo, model.codigoEmpresa, _user.cod_usu, model.CodigoContaCorrente);

                    resultado = GeraArquivoRemessaPagamento(model, codigoBorderoPGTO);
                }
                else
                {
                    //Verifica se existe alguma fatura não baixada e cancela ela. Só pode existir 1 fatura em aberto para homologação.
                    CancelamentoFaturaHomologacao(model.CodigoContaCorrente, _user.cod_usu);

                    var codigoFatura = InclusaoFaturamento(model.codigoEmpresa, _user.cod_usu, model.CodigoContaCorrente);
                    var codigoBorderoFatura = InclusaoFaturaBordero(codigoFatura, model.codigoEmpresa, _user.cod_usu, model.CodigoContaCorrente);

                    resultado = GeraArquivoRemessaBoleto(model, codigoBorderoFatura);
                }

            }
            catch (Exception e)
            {
                resultado.Resultado = false;
                resultado.msgResultado = e.Message;
            }
            return resultado;
        }
        private void CancelaTituloHomologacao(int codigoContaCorrente, int codigoUsuario)
        {
            var Titulo = _emissaoTitulos.RetornaTituloHomologacao(codigoContaCorrente);
            if (Titulo != null)
            {
                _emissaoTitulos.CancelaTituloHomologacao(codigoContaCorrente, codigoUsuario, "Título cancelado para a inclusão de um outro título para homologacao");
                if (Convert.ToInt32(Titulo.CODBORDERO) > 0)
                {
                    _borderoPGTO.AtualizaStatusBordero((int)Titulo.CODBORDERO, "CANCELADO");
                }
                if (!string.IsNullOrWhiteSpace(Titulo.SEUNUMERO))
                {
                    var tituloPago = _pagamento.GetTransacaoBySeuNumero(Titulo.SEUNUMERO).Result;
                    tituloPago.STATUS = "CANCELADO";
                    _pagamento.AtualizaTransacao(tituloPago);

                    var arquivo = _arquivo.ReturnFileById(tituloPago.CODARQUIVO).Result;
                    arquivo.STATUS = "CANCELADO";
                    _arquivo.AtualizaArquivo(arquivo);
                }
            }
        }
        private void CancelamentoFaturaHomologacao(int codigoContaCorrente, int codigoUsuario)
        {
            var Fatura = _emissaoBoleto.RetornaFaturaHomologacao(codigoContaCorrente);
            if (Fatura != null)
            {
                _emissaoBoleto.CancelaFaturaHomologacao(codigoContaCorrente, codigoUsuario, "Fatura cancelado para a inclusão de uma outra fatura para homologacao");
                if (Convert.ToInt32(Fatura.CODBORDERO) > 0)
                {
                    _borderoBoleto.AtualizaStatusBordero((int)Fatura.CODBORDERO, "CANCELADO");
                }
                if (!string.IsNullOrWhiteSpace(Fatura.SEUNUMERO))
                {
                    var Boleto = _emissaoBoleto.BuscaBoletoBySeuNumero(Fatura.SEUNUMERO);
                    Boleto.Status = "CANCELADO";
                    _emissaoBoleto.AtualizaBoleto(Boleto);

                    var arquivo = _arquivo.ReturnFileById(Boleto.CODARQUIVO).Result;
                    arquivo.STATUS = "CANCELADO";
                    _arquivo.AtualizaArquivo(arquivo);
                }
            }
        }
        private int InclusaoTitulo(int codigoFavorecido, int codigoEmpresa, int codigoUsuario, int codigoContaCorrente)
        {
            var PlanoContas = _PlanoContas.BuscaDefaultPlanosContasPagamento(codigoEmpresa).Result;
            PAGNET_EMISSAO_TITULOS Titulos = new PAGNET_EMISSAO_TITULOS();
            Titulos.STATUS = "EM_ABERTO";
            Titulos.CODEMPRESA = codigoEmpresa;
            Titulos.CODFAVORECIDO = codigoFavorecido;
            Titulos.DATEMISSAO = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Titulos.DATPGTO = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Titulos.DATREALPGTO = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Titulos.VALBRUTO = 1;
            Titulos.VALLIQ = 1;
            Titulos.VALTOTAL = 1;

            Titulos.TIPOTITULO = "HOM";
            Titulos.ORIGEM = "PN";
            Titulos.SISTEMA = 0;
            Titulos.CODBORDERO = null;
            Titulos.SEUNUMERO = "";
            Titulos.CODPLANOCONTAS = PlanoContas.CODPLANOCONTAS;
            Titulos.CODCONTACORRENTE = codigoContaCorrente;

            _emissaoTitulos.IncluiTitulo(Titulos);
            _emissaoTitulos.IncluiLog(Titulos, codigoUsuario, "Título incluido para homologação da conta corrente.");

            return Titulos.CODTITULO;
        }
        private int InclusaoFaturamento(int codigoEmpresa, int codigoUsuario, int codigoContaCorrente)
        {
            try
            {
                PAGNET_EMISSAOFATURAMENTO fatura = new PAGNET_EMISSAOFATURAMENTO();
                int codFaturamento = _emissaoBoleto.BuscaNovoIDEmissaoFaturamento();
                var PlanoContas = _PlanoContas.BuscaDefaultPlanosContasRecebimento(codigoEmpresa).Result;
                var codigoCliente = CriaClienteUsuarioByEmpresa(codigoEmpresa);

                fatura.CODEMISSAOFATURAMENTO = codFaturamento;
                fatura.ORIGEM = "PAGNET";
                fatura.CODCLIENTE = codigoCliente;
                fatura.TIPOFATURAMENTO = "HOM";
                fatura.DATSOLICITACAO = DateTime.Now;
                fatura.CODBORDERO = null;
                fatura.CODEMPRESA = Convert.ToInt32(codigoEmpresa);
                fatura.STATUS = "EM_ABERTO";
                fatura.VALOR = 1;
                fatura.CODEMISSAOFATURAMENTOPAI = codFaturamento;
                fatura.PARCELA = 1;
                fatura.TOTALPARCELA = 1;
                fatura.VALORPARCELA = 2;

                fatura.CODPLANOCONTAS = PlanoContas.CODPLANOCONTAS;
                fatura.CODCONTACORRENTE = codigoContaCorrente;
                fatura.DATVENCIMENTO = (Convert.ToDateTime(DateTime.Now.ToShortDateString())).AddDays(6);
                fatura.NRODOCUMENTO = "HOM_" + codFaturamento.ToString();

                fatura.JUROSCOBRADO = 0;
                fatura.MULTACOBRADA = 0;
                fatura.VLDESCONTOCONCEDIDO = 0;

                //faturamento.MENSAGEMARQUIVOREMESSA = model.MensagemArquivoRemessa;
                fatura.MENSAGEMINSTRUCOESCAIXA = "";
                fatura.CODFORMAFATURAMENTO = 1; //boleto

                _emissaoBoleto.IncluiFaturamento(fatura);
                _emissaoBoleto.IncluiLog(fatura, codigoUsuario, "Fatura incluída para homologar a conta corrente.");


                return codFaturamento;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int IncluiTituloBordero(int CodigoTitulo, int codigoEmpresa, int codigoUsuario, int codigoContaCorrente)
        {
            PAGNET_BORDERO_PAGAMENTO bordero = new PAGNET_BORDERO_PAGAMENTO();
            bordero.STATUS = "EM_BORDERO";
            bordero.CODUSUARIO = codigoUsuario;
            bordero.VLBORDERO = 1;
            bordero.DTBORDERO = DateTime.Now;
            bordero.CODEMPRESA = codigoEmpresa;
            bordero.CODCONTACORRENTE = codigoContaCorrente;

            bordero = _borderoPGTO.InseriBordero(bordero);

            var Titulo = _emissaoTitulos.BuscaTituloByID(CodigoTitulo).Result;
            Titulo.STATUS = "EM_BORDERO";
            Titulo.CODBORDERO = bordero.CODBORDERO;

            _emissaoTitulos.AtualizaTitulo(Titulo);

            //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
            _emissaoTitulos.IncluiLog(Titulo, codigoUsuario, "Inclusão de título em um borderô");

            return bordero.CODBORDERO;
        }
        private int InclusaoFaturaBordero(int codigoFatura, int codigoEmpresa, int codigoUsuario, int codigoContaCorrente)
        {
            PAGNET_BORDERO_BOLETO bordero = new PAGNET_BORDERO_BOLETO();

            bordero.STATUS = "EM_BORDERO";
            bordero.CODUSUARIO = codigoUsuario;
            bordero.QUANTFATURAS = 1;
            bordero.VLBORDERO = 1;
            bordero.CODEMPRESA = codigoEmpresa;
            bordero.DTBORDERO = DateTime.Now;

            bordero = _borderoBoleto.InseriBordero(bordero);

            var DadosEmissaoBoleto = _emissaoBoleto.BuscaFaturamentoByID(codigoFatura);

            DadosEmissaoBoleto.STATUS = bordero.STATUS;
            DadosEmissaoBoleto.CODBORDERO = bordero.CODBORDERO;

            _emissaoBoleto.AtualizaFaturamento(DadosEmissaoBoleto);
            _emissaoBoleto.IncluiLog(DadosEmissaoBoleto, codigoUsuario, "Fatura incluído em um borderô");

            return bordero.CODBORDERO;

        }
        private ResultadoTransmissaoArquivo GeraArquivoRemessaPagamento(IDadosHomologarContaCorrenteVm model, int codigoBordero)
        {
            ResultadoTransmissaoArquivo RetornoAPI = new ResultadoTransmissaoArquivo();
            try
            {
                FiltroTransmissaoBancoVM filtro = new FiltroTransmissaoBancoVM();
                APIBorderosPGTOVM bordero = new APIBorderosPGTOVM();
                bordero.codigoBordero = codigoBordero;

                filtro.codigoContaCorrente = model.CodigoContaCorrente;
                filtro.codigoEmpresa = model.codigoEmpresa;
                filtro.ListaBorderosPGTO.Add(bordero);
                var dadosContaCorrente = _conta.GetContaCorrenteById(model.CodigoContaCorrente).Result;
                               
                using (var client = new HttpClient())
                {

                    string relativePath = "TransmiteArquivoBanco";

                    
                    string BaseAddress = @"https://www1.tln.com.br/API/PagNet/MontaArquivoRemessaPGTO/";
                    if (_user.ServidorDadosNetCard.ToUpper() == "NETUNO")
                    {
                        BaseAddress = @"https://www3.tln.com.br/API/PagNet/MontaArquivoRemessaPGTO/";
                    }

                    //BaseAddress = @"https://localhost:44368/";
                    if (client.BaseAddress == null)
                    {
                        var token = _ContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                        client.BaseAddress = new Uri(BaseAddress);
                        client.Timeout = TimeSpan.FromMinutes(5);
                        client.DefaultRequestHeaders.Add("Authorization", $"{token}");

                    }

                    var content = new StringContent(JsonConvert.SerializeObject(filtro),
                                                    Encoding.UTF8,
                                                    "application/json");

                    var response = new ApiResponse<ResultadoTransmissaoArquivo>(client.PostAsync(relativePath, content).Result);

                    if (!response.Success)
                        throw new Exception(string.Join(Environment.NewLine, response.Errors));

                    RetornoAPI = response.Result;
                }

            }
            catch (Exception ex)
            {
                RetornoAPI.Resultado = false;
                RetornoAPI.msgResultado = ex.Message;
            }
            return RetornoAPI;
        }
        private ResultadoTransmissaoArquivo GeraArquivoRemessaBoleto(IDadosHomologarContaCorrenteVm model, int codigoBordero)
        {
            try
            {
                var caminhoPadrao = _ope.GetOperadoraById(model.codOPE).Result;
                var CaminhoRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, model.codigoEmpresa.ToString(), "ArquivoRemessaBoleto");
                ResultadoTransmissaoArquivo dadosRetorno = new ResultadoTransmissaoArquivo();

                APIBorderosPGTOVM listBordero = new APIBorderosPGTOVM();
                listBordero.codigoBordero = codigoBordero;

                FiltroTransmissaoBoletoModel filtroEnvio = new FiltroTransmissaoBoletoModel();
                filtroEnvio.codigoEmpresa = model.codigoEmpresa;
                filtroEnvio.codContaCorrente = model.CodigoContaCorrente;
                filtroEnvio.CaminhoArquivo = CaminhoRemessa;
                filtroEnvio.ListaBordero.Add(listBordero);

                using (var client = new HttpClient())
                {

                    string relativePath = "TransmissaoArquivoRemessa";


                    string BaseAddress = @"https://www1.tln.com.br/API/PagNet/CobrancaBancaria/";
                    if (_user.ServidorDadosNetCard.ToUpper() == "NETUNO")
                    {
                        BaseAddress = @"https://www3.tln.com.br/API/PagNet/CobrancaBancaria/";
                    }

                    //BaseAddress = @"https://localhost:44314/";
                    if (client.BaseAddress == null)
                    {
                        var token = _ContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                        client.BaseAddress = new Uri(BaseAddress);
                        client.Timeout = TimeSpan.FromMinutes(5);
                        client.DefaultRequestHeaders.Add("Authorization", $"{token}");

                    }

                    var content = new StringContent(JsonConvert.SerializeObject(filtroEnvio),
                                                    Encoding.UTF8,
                                                    "application/json");

                    var response = new ApiResponse<ResultadoTransmissaoArquivo>(client.PostAsync(relativePath, content).Result);

                    if (!response.Success)
                        throw new Exception(string.Join(Environment.NewLine, response.Errors));

                    dadosRetorno = response.Result;
                }

                if(dadosRetorno.msgResultado != "Sucesso")
                {
                    dadosRetorno.Resultado = false;
                }
                
                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private int IncluiEmpresaCliente(int codigoEmpresa, int codigoUsuario)
        {
            try
            {
                int codigoCliente = 0;

                var dadosEmpresa = _empresa.ConsultaEmpresaById(codigoEmpresa).Result;
                var dadosCliente = _cliente.BuscaClienteByCNPJ(dadosEmpresa.CNPJ).Result;
                if (dadosCliente == null)
                {

                    PAGNET_CADCLIENTE cliente = new PAGNET_CADCLIENTE();
                    cliente.NMCLIENTE = dadosEmpresa.RAZAOSOCIAL;
                    cliente.CPFCNPJ = Helper.RemoveCaracteres(dadosEmpresa.CNPJ);
                    cliente.CODEMPRESA = codigoEmpresa;
                    cliente.CEP = Helper.RemoveCaracteres(dadosEmpresa.CEP);
                    cliente.EMAIL = "";
                    cliente.CODFORMAFATURAMENTO = 1;
                    cliente.LOGRADOURO = dadosEmpresa.LOGRADOURO;
                    cliente.NROLOGRADOURO = dadosEmpresa.NROLOGRADOURO;
                    cliente.COMPLEMENTO = dadosEmpresa.COMPLEMENTO;
                    cliente.BAIRRO = dadosEmpresa.BAIRRO;
                    cliente.CIDADE = dadosEmpresa.CIDADE;
                    cliente.UF = dadosEmpresa.UF;
                    cliente.COBRANCADIFERENCIADA = "N";
                    cliente.AGRUPARFATURAMENTOSDIA = "N";
                    cliente.COBRAJUROS = "N";
                    cliente.VLJUROSDIAATRASO = 0;
                    cliente.PERCJUROS = 0;
                    cliente.COBRAMULTA = "N";
                    cliente.VLMULTADIAATRASO = 0;
                    cliente.PERCMULTA = 0;
                    cliente.CODPRIMEIRAINSTCOBRA = 0;
                    cliente.CODSEGUNDAINSTCOBRA = 0;
                    cliente.TAXAEMISSAOBOLETO = 0;
                    cliente.TIPOCLIENTE = "j";

                    cliente.ATIVO = "S";

                    _cliente.IncluiCliente(cliente);
                    _cliente.InsertLog(cliente, codigoUsuario, "Empresa incluida como cliente para realizar a homologação de boletos.");

                    codigoCliente = cliente.CODCLIENTE;
                }
                else
                {
                    codigoCliente = dadosCliente.CODCLIENTE;
                }

                return codigoCliente;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ResultadoTransmissaoArquivo GeraBoletoPDFHomologacao(IDadosHomologarContaCorrenteVm model)
        {
            ResultadoTransmissaoArquivo retorno = new ResultadoTransmissaoArquivo();
            var fatura = _emissaoBoleto.RetornaFaturaHomologacao(model.CodigoContaCorrente);
            var DadosBoleto = _emissaoBoleto.BuscaBoletoBySeuNumero(fatura.SEUNUMERO);

            var url = Path.Combine(model.CaminhoArquivo, "PDFBoleto", DadosBoleto.nmBoletoGerado + ".pdf");

            //if (!File.Exists(url))
            //{
            //     _recebimentoApp.GeraBoletoPDF(Path.Combine(model.CaminhoArquivo, "PDFBoleto"), fatura.CODEMISSAOFATURAMENTO, model.codigoUsuario);
            //}
            var DataAtual = DateTime.Now;
            var DataLimite = DateTime.Now.AddMinutes(2);
            int i = 0;

            while (!File.Exists(url))
            {
                i++;
                if (DataAtual > DataLimite)
                    break;
            }
            retorno.CaminhoCompletoArquivo = url;
            retorno.nomeArquivo = DadosBoleto.nmBoletoGerado + ".pdf";
            retorno.Resultado = true;

            return retorno;

        }
        public bool ExisteArquivoRemessaBoletoCriado(int codigoContaCorrente)
        {
            bool ExisteArqRemessaBol = false;

            var Titulo = _emissaoBoleto.RetornaFaturaHomologacao(codigoContaCorrente);
            ExisteArqRemessaBol = (Titulo != null);

            return ExisteArqRemessaBol;
        }
        public int CriaClienteUsuarioByEmpresa(int codigoEmpresa)
        {
            int CodigoCliente = 0;
            var DadosUsuario = _usuPagNet.BuscaUsuarioAleatorioByEmpresa(codigoEmpresa);
            var DadosCliente = _cliente.BuscaClienteByCNPJ(Helper.RemoveCaracteres(DadosUsuario.CPF)).Result;
            var DadosEmpresa = _empresa.ConsultaEmpresaById(codigoEmpresa).Result;

            if (DadosCliente != null)
            {
                CodigoCliente = DadosCliente.CODCLIENTE;
            }
            else
            {
                PAGNET_CADCLIENTE cli = new PAGNET_CADCLIENTE();

                cli.NMCLIENTE = DadosUsuario.NMUSUARIO;
                cli.CPFCNPJ = Helper.RemoveCaracteres(DadosUsuario.CPF);
                cli.CODEMPRESA = codigoEmpresa;
                cli.CEP = Helper.RemoveCaracteres(DadosEmpresa.CEP);
                cli.EMAIL = DadosUsuario.EMAIL;
                cli.CODFORMAFATURAMENTO = 1;
                cli.LOGRADOURO = DadosEmpresa.LOGRADOURO;
                cli.NROLOGRADOURO = DadosEmpresa.NROLOGRADOURO;
                cli.COMPLEMENTO = DadosEmpresa.COMPLEMENTO;
                cli.BAIRRO = DadosEmpresa.BAIRRO;
                cli.CIDADE = DadosEmpresa.CIDADE;
                cli.UF = DadosEmpresa.UF;
                cli.COBRANCADIFERENCIADA = "N";
                cli.AGRUPARFATURAMENTOSDIA = "N";
                cli.COBRAJUROS = "N";
                cli.VLJUROSDIAATRASO = 0;
                cli.PERCJUROS = 0;
                cli.COBRAMULTA = "N";
                cli.VLMULTADIAATRASO = 0;
                cli.PERCMULTA = 0;
                cli.CODPRIMEIRAINSTCOBRA = 0;
                cli.CODSEGUNDAINSTCOBRA = 0;
                cli.TAXAEMISSAOBOLETO = 0;
                cli.TIPOCLIENTE = "F";
                cli.ATIVO = "N";

                _cliente.IncluiCliente(cli);
                _cliente.InsertLog(cli, 9999, "Cliente incluído para ser utilizado na homologação de cobrança.");

                CodigoCliente = cli.CODCLIENTE;
            }

            return CodigoCliente;

        }

    }
}
