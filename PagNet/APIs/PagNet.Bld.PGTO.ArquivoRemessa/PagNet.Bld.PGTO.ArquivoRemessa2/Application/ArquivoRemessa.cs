using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface;
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface.Model;
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Model;
using PagNet.Bld.PGTO.ArquivoRemessa2.ModelAuxiliar;
using PagNet.Bld.PGTO.ArquivoRemessa2.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Telenet.BusinessLogicModel;
using static PagNet.Bld.PGTO.ArquivoRemessa2.Constants;

namespace PagNet.Bld.PGTO.ArquivoRemessa2.Application
{
    public class ArquivoRemessaApp : Service<IContextoApp>, IArquivoRemessa
    {
        protected IHttpContextAccessor _ContextAccessor { get; }
        private readonly IOPERADORAService _ope;
        private readonly IPAGNET_CONTACORRENTEService _conta;
        private readonly IPAGNET_TRANSMISSAOARQUIVOService _transmissaoArquivo;
        private readonly IParametrosApp _user;
        private readonly IPAGNET_CADFAVORECIDOService _favorecido;
        private readonly IPAGNET_EMISSAO_TITULOSService _emissaoTitulos;
        private readonly IPAGNET_TITULOS_PAGOSService _pagamento;
        private readonly IPAGNET_ARQUIVOService _arquivo;
        private readonly IPAGNET_CADEMPRESAService _cadEmpresa;
        private readonly IPAGNET_BORDERO_PAGAMENTOService _bordero;
        private readonly IPAGNET_TAXAS_TITULOSService _taxaTitulo;
        private readonly IPAGNET_OCORRENCIARETPAGService _ocorrencia;
        private readonly IPAGNET_API_PGTOService _caminhoAPI;

        public ArquivoRemessaApp(IContextoApp contexto,
                                IHttpContextAccessor contextAccessor,
                                IParametrosApp user,
                                IOPERADORAService ope,
                                IPAGNET_CONTACORRENTEService conta,
                                IPAGNET_TRANSMISSAOARQUIVOService transmissaoArquivo,
                                IPAGNET_CADFAVORECIDOService favorecido,
                                IPAGNET_EMISSAO_TITULOSService emissaoTitulos,
                                IPAGNET_TITULOS_PAGOSService pagamento,
                                IPAGNET_ARQUIVOService arquivo,
                                IPAGNET_CADEMPRESAService cadEmpresa,
                                IPAGNET_BORDERO_PAGAMENTOService bordero,
                                IPAGNET_TAXAS_TITULOSService taxaTitulo,
                                IPAGNET_API_PGTOService caminhoAPI,
                                IPAGNET_OCORRENCIARETPAGService ocorrencia)
            : base(contexto)
        {
            _user = user;
            _ope = ope;
            _conta = conta;
            _transmissaoArquivo = transmissaoArquivo;
            _favorecido = favorecido;
            _emissaoTitulos = emissaoTitulos;
            _arquivo = arquivo;
            _cadEmpresa = cadEmpresa;
            _bordero = bordero;
            _taxaTitulo = taxaTitulo;
            _pagamento = pagamento;
            _ocorrencia = ocorrencia;
            _caminhoAPI = caminhoAPI;
            _ContextAccessor = contextAccessor;
        }

        public ResultadoGeracaoArquivo TransmiteArquivoBanco(FiltroTransmissaoBancoVM filtro)
        {
            ResultadoGeracaoArquivo DadosRetorno = new ResultadoGeracaoArquivo();
            DadosGeracaoArquivoRemessa DadosEmissaoRemessa = new DadosGeracaoArquivoRemessa();
            bool ExisteTituloParaGerarArquivo;
            bool ExisteBoletoParaGerarArquivo;
            try
            {

                var caminhoPadrao = _ope.GetOperadoraById(_user.cod_ope).Result;

                var pathRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, filtro.codigoEmpresa.ToString(), "ArquivoRemessaPagamento");

                if (!Directory.Exists(pathRemessa))
                    Directory.CreateDirectory(pathRemessa);

                string CaminhoArquivo = pathRemessa;

                var contaCorrente = _conta.GetContaCorrenteById(Convert.ToInt32(filtro.codigoContaCorrente)).Result;

                DadosEmissaoRemessa.cedente = mdCedente.ToView(contaCorrente);

                //AssertionValidator
                //    .AssertNow(!string.IsNullOrWhiteSpace(contaCorrente.CODCONVENIOPAG), CodigosErro.CodigoConvenioNaoInformado)
                //    .Validate();

                string nmArquivo = "PG_" + contaCorrente.CODBANCO + DateTime.Now.ToString("ddMMyyHHmmss") + ".rem";

                //Grava dados do Arquivo de Remessa
                var codArquivo = GravaArquivoRemessa(CaminhoArquivo, filtro, contaCorrente.CODBANCO, nmArquivo);
                //Inseri os títulos para pagamento
                ExisteTituloParaGerarArquivo = IncluiTituloTEDDOCPagamento(filtro, contaCorrente.CODBANCO, codArquivo);
                //Inseri os títulos para pagamento
                ExisteBoletoParaGerarArquivo = IncluiTituloBoletoPagamento(filtro, contaCorrente.CODBANCO, codArquivo);

                //Caso não exista títulos para incluir no arquivo de remessa, será retor
                if (!ExisteTituloParaGerarArquivo && !ExisteBoletoParaGerarArquivo)
                {
                    DadosRetorno = new ResultadoGeracaoArquivo();
                    DadosRetorno.Resultado = false;
                    DadosRetorno.msgResultado = "Não é possível incluir títulos vencidos no arquivo de remessa.";
                    return DadosRetorno;
                }

                DadosEmissaoRemessa.codigoContaCorrente = contaCorrente.CODCONTACORRENTE;
                DadosEmissaoRemessa.codigoEmpresa = filtro.codigoEmpresa;
                DadosEmissaoRemessa.ListaBorderosPGTO.AddRange(filtro.ListaBorderosPGTO);
                DadosEmissaoRemessa.codigoArquivo = codArquivo;
                DadosEmissaoRemessa.CaminhoArquivo = CaminhoArquivo + "\\" + nmArquivo;
                DadosEmissaoRemessa.codigoBanco = contaCorrente.CODBANCO;

                //Busca a forma que será transmitido os títulos para o banco
                var FormaTransmissao = _transmissaoArquivo.BuscaFormaTransmissao(contaCorrente.CODCONTACORRENTE, "PAGAMENTO");
                if (FormaTransmissao != null)
                {
                    if (FormaTransmissao.FORMATRANSMISSAO == "API")
                    {
                        //tem que ser desenvolvido;
                    }
                    else //se não for API sempre terá que gerar um arquivo de remessa.
                    {
                        DadosRetorno = GeraArquivoRemessa(DadosEmissaoRemessa);

                        if (FormaTransmissao.FORMATRANSMISSAO == "VAN")
                        {
                            var DadosArquivo = _arquivo.ReturnFileById(codArquivo).Result;

                            string sourceFile = Path.Combine(DadosArquivo.CAMINHOARQUIVO, DadosArquivo.NMARQUIVO);
                            string destFile = Path.Combine(FormaTransmissao.CAMINHOREM, DadosArquivo.NMARQUIVO);
                            File.Copy(sourceFile, destFile, true);
                        }

                    }
                    DadosRetorno.FormaTransmissao = FormaTransmissao.FORMATRANSMISSAO;
                }
                else
                {
                    DadosRetorno = GeraArquivoRemessa(DadosEmissaoRemessa);
                }
                DadosRetorno.CaminhoCompletoArquivo = DadosEmissaoRemessa.CaminhoArquivo;
                DadosRetorno.nomeArquivo = nmArquivo;

                //Caso por algum motivo não foi possivel criar o arquivo mas não acusou o erro, o sistema irá fazer o rollback
                if (!File.Exists(DadosRetorno.CaminhoCompletoArquivo))
                {
                    throw new Exception("Arquivo de remessa não foi criado corretamente");
                }
            }
            catch (Exception ex)
            {
                RollbackGeracaoArquivo(filtro);
                DadosRetorno.Resultado = false;
                DadosRetorno.msgResultado = "Falha na criação do arquivo de remessa, favor contactar o suporte.";
            }
            return DadosRetorno;
        }
        private bool IncluiTituloTEDDOCPagamento(IFiltroTransmissaoBancoVM model, string codBanco, int codArquivo)
        {
            try
            {
                decimal ValorTotalPGTO = 0;
                bool ExisteTituloParaGerarArquivo = false;
                decimal valTaxa = 0;
                decimal taxaTED = 0;
                bool TituloHomologacao = false;
                bool taxaIncluida = false;
                bool taxaTEDExistente = false;
                DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                PAGNET_TITULOS_PAGOS tITULOS_PAGOS = null;
                foreach (var listaBorderos in model.ListaBorderosPGTO)
                {
                    taxaTEDExistente = false;
                    var ListaTitulosBordero = _emissaoTitulos.BuscaTitulosByBordero(listaBorderos.codigoBordero).Result
                                        .Select(x => new ListaEmissaoTitulosVM(x)).ToList();

                    var ListaTitulosVencidos = ListaTitulosBordero.Where(x => x.DATPGTO < dataAtual).ToList();

                    //Remove os Titulos vencidos da lista que deverá ser paga
                    foreach (var tt in ListaTitulosVencidos)
                    {
                        var titulo = _emissaoTitulos.BuscaTituloByID(tt.CODIGOTITULO).Result;
                        titulo.STATUS = "EM_ABERTO";
                        titulo.CODBORDERO = null;

                        _emissaoTitulos.IncluiLog(titulo, _user.cod_usu, "Titulo não incluido no arquivo de remessa pois está com a data real para pagamento vencido.");
                        _emissaoTitulos.AtualizaTitulo(titulo);

                        //remove o titulo da lista
                        ListaTitulosBordero.RemoveAll(x => x.CODIGOTITULO == tt.CODIGOTITULO);
                    }


                    //atualiza Taxas para o pagamento
                    foreach (var item in ListaTitulosBordero)
                    {
                        valTaxa = 0;
                        TituloHomologacao = item.TIPOTITULO == "HOM";
                        var listaTaxa = _taxaTitulo.buscaTodasTaxasbyCodTitulo(item.CODIGOTITULO).Result;
                        if (listaTaxa.Count > 0)
                        {
                            valTaxa = listaTaxa.Where(x => x.ORIGEM == "PN").Sum(x => x.VALOR);

                            var taxaTedTemp = listaTaxa.Where(x => x.DESCRICAO == "TAXA DE TRANSFERÊNCIA").FirstOrDefault();

                            if (taxaTedTemp != null)
                            {
                                taxaTEDExistente = true;
                            }

                            //INCLUSÃO DAS TAXAS GERADAS NO SISTEMA PAGNET
                            item.VALFINALPGTO = (item.VALLIQ + valTaxa);

                            var titulo = _emissaoTitulos.BuscaTituloByID(item.CODIGOTITULO).Result;
                            titulo.VALTOTAL = item.VALFINALPGTO;

                            _emissaoTitulos.AtualizaTitulo(titulo);
                        }
                    }


                    var NewListaTitulos = (from reg in ListaTitulosBordero
                                           where (reg.TIPOTITULO == "TEDDOC" || reg.TIPOTITULO == "TED" || reg.TIPOTITULO == "CC" || reg.TIPOTITULO == "HOM")
                                           group reg by new
                                           {
                                               reg.CODBANCO,
                                               reg.AGENCIA,
                                               reg.CONTACORRENTE,
                                               reg.DATPGTO,
                                           } into g
                                           select new
                                           {
                                               g.Key.CODBANCO,
                                               g.Key.AGENCIA,
                                               g.Key.CONTACORRENTE,
                                               g.Key.DATPGTO,
                                               VALFINALPGTO = g.Sum(s => s.VALFINALPGTO)
                                           }).ToList();

                    foreach (var titulos in NewListaTitulos)
                    {
                        taxaIncluida = false;
                        //Busca apenas um registros para validar dados
                        var TitPag = ListaTitulosBordero.Where(x => x.CODBANCO == titulos.CODBANCO &&
                                                                         x.AGENCIA == titulos.AGENCIA &&
                                                                         x.CONTACORRENTE == titulos.CONTACORRENTE &&
                                                                         x.DATPGTO == titulos.DATPGTO).FirstOrDefault();


                        tITULOS_PAGOS = new PAGNET_TITULOS_PAGOS();
                        var codigoTitulo = _pagamento.RetornaProximoNumeroTitulosPagos();
                        var SeuNumero = Helper.GeraSeuNumero(codigoTitulo, "");

                        if (SeuNumero.Length > 20)
                        {
                            SeuNumero = Helper.FormataTexto(SeuNumero, 20);
                        }

                        var nossoNumero = Helper.GeraNossoNumero("", TitPag.CODIGOTITULO);
                        int codFormaLancamento = 0;
                        var DadosEmpresa = _cadEmpresa.ConsultaEmpresaById(TitPag.CODEMPRESA).Result;


                        if (TitPag.CODBANCO == codBanco)
                        {
                            codFormaLancamento = 1;
                            taxaTED = 0;
                        }
                        else
                        {
                            if (!TituloHomologacao && !taxaTEDExistente)
                            {
                                taxaTED = RetornaTaxaTransferencia(TitPag.CODFAVORECIDO, model, titulos.VALFINALPGTO);
                            }
                            if (DadosEmpresa.CNPJ == TitPag.CNPJ)
                            {
                                codFormaLancamento = 43;
                            }
                            else
                            {
                                if (codBanco == "237")
                                    codFormaLancamento = 03;
                                else
                                    codFormaLancamento = 41;
                            }
                        }
                        ValorTotalPGTO += (titulos.VALFINALPGTO - taxaTED);
                        tITULOS_PAGOS.CODTITULOPAGO = codigoTitulo;
                        tITULOS_PAGOS.STATUS = "AGUARDANDO_ARQUIVO_RETORNO";
                        tITULOS_PAGOS.CODUSUARIO = _user.cod_usu;
                        tITULOS_PAGOS.CODCONTACORRENTE = model.codigoContaCorrente;
                        tITULOS_PAGOS.CODBORDERO = listaBorderos.codigoBordero;
                        tITULOS_PAGOS.CODFAVORECIDO = TitPag.CODFAVORECIDO;
                        tITULOS_PAGOS.CODEMPRESA = TitPag.CODEMPRESA;
                        tITULOS_PAGOS.TIPOSERVICO = 20;
                        tITULOS_PAGOS.CODFORMALANCAMENTO = codFormaLancamento;
                        tITULOS_PAGOS.SEUNUMERO = SeuNumero;
                        tITULOS_PAGOS.NOSSONUMERO = nossoNumero;
                        tITULOS_PAGOS.DTPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTREALPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTVENCIMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.VALOR = titulos.VALFINALPGTO - taxaTED;
                        tITULOS_PAGOS.CODARQUIVO = codArquivo;
                        tITULOS_PAGOS.OCORRENCIARETORNO = "";
                        tITULOS_PAGOS.TIPOTITULO = "TEDDOC";


                        tITULOS_PAGOS = _pagamento.IncluiTransacao(tITULOS_PAGOS);

                        //Busca uma lista de registros
                        var ListaTiti = ListaTitulosBordero.Where(x => x.CODBANCO == titulos.CODBANCO &&
                                                                         x.AGENCIA == titulos.AGENCIA &&
                                                                         x.CONTACORRENTE == titulos.CONTACORRENTE &&
                                                                         x.DATPGTO == titulos.DATPGTO).ToList();

                        foreach (var t in ListaTiti)
                        {
                            var tituloEmitido = _emissaoTitulos.BuscaTituloByID(t.CODIGOTITULO).Result;
                            tituloEmitido.STATUS = "AGUARDANDO_ARQUIVO_RETORNO";
                            tituloEmitido.SEUNUMERO = SeuNumero;
                            tituloEmitido.CODCONTACORRENTE = model.codigoContaCorrente;

                            //fiz isso para incluir apenas uma vez e para o mesmo titulo que a taxa foi aplicada
                            if (!taxaIncluida && taxaTED > 0 && !taxaTEDExistente)
                            {
                                tituloEmitido.VALTOTAL = tituloEmitido.VALTOTAL - taxaTED;
                            }

                            if (!taxaIncluida && taxaTED > 0 && !taxaTEDExistente)
                            {
                                PAGNET_TAXAS_TITULOS tx = new PAGNET_TAXAS_TITULOS();
                                tx.CODTITULO = tituloEmitido.CODTITULO;
                                tx.DESCRICAO = "TAXA DE TRANSFERÊNCIA";
                                tx.VALOR = -1 * taxaTED;
                                tx.ORIGEM = "PN";
                                tx.DTINCLUSAO = DateTime.Now;
                                tx.CODUSUARIO = _user.cod_usu;

                                _taxaTitulo.IncluiTaxa(tx);
                                //Inseri log do título
                                _emissaoTitulos.IncluiLog(tituloEmitido, _user.cod_usu, "Inclusão de taxa de transferência.");
                                taxaIncluida = true;
                            }
                            //Atualiza o título com o status e o seu número
                            _emissaoTitulos.AtualizaTitulo(tituloEmitido);

                            //Inseri log do título
                            _emissaoTitulos.IncluiLog(tituloEmitido, _user.cod_usu, "Título incluido no arquivo de remessa para pagamento");
                        }
                    }
                    //Atualiza status do borderô
                    if (ListaTitulosBordero.Count() == 0)
                    {
                        _bordero.AtualizaStatusBordero(listaBorderos.codigoBordero, "CANCELADO");
                    }
                    else
                    {
                        _bordero.AtualizaStatusBordero(listaBorderos.codigoBordero, "AGUARDANDO_ARQUIVO_RETORNO");
                        ExisteTituloParaGerarArquivo = true;
                    }

                }
                return ExisteTituloParaGerarArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private decimal RetornaTaxaTransferencia(int codFavorito, IFiltroTransmissaoBancoVM filtro, decimal valPag)
        {
            decimal TaxaTed = 0;
            var dadosBancarios = _conta.GetContaCorrenteById(filtro.codigoContaCorrente).Result;

            TaxaTed = Convert.ToDecimal(dadosBancarios.VALTED);

            var DadosFavorito = _favorecido.BuscaFavorecidosByID(codFavorito);
            var configFavorecido = DadosFavorito.PAGNET_CADFAVORECIDO_CONFIG.Where(x => x.CODEMPRESA == filtro.codigoEmpresa).FirstOrDefault();

            if (configFavorecido != null)
            {
                if (configFavorecido.REGRADIFERENCIADA == "S")
                {
                    TaxaTed = Convert.ToDecimal(configFavorecido.VALTED);
                }
            }

            return TaxaTed;
        }
        private int GravaArquivoRemessa(string CaminhoArquivo, IFiltroTransmissaoBancoVM model, string codBanco, string nomeArquivo)
        {
            try
            {
                string nmArquivo;
                int TotalRegistros = 0;
                decimal vlTotalArquivo = 0;
                int codArquivo = 0;
                if (model.ListaBorderosPGTO.Count > 0)
                {
                    foreach (var bordero in model.ListaBorderosPGTO)
                    {
                        var titulos = _emissaoTitulos.BuscaTitulosByBordero(bordero.codigoBordero).Result;
                        TotalRegistros += titulos.Count;
                        vlTotalArquivo += titulos.Sum(x => x.VALTOTAL);
                    }

                    MetodosGerais _MetodosGerais = new MetodosGerais(_arquivo);


                    int nroSeqArquivo = _MetodosGerais.RetornaNroSeqArquivo();

                    var Status = "AGUARDANDO_ARQUIVO_RETORNO";
                    var TipArqu = "PAG";
                    codArquivo = _MetodosGerais.InseriArquivo(CaminhoArquivo, nomeArquivo, nroSeqArquivo, vlTotalArquivo, TotalRegistros, Status, TipArqu, codBanco);


                }
                return codArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool IncluiTituloBoletoPagamento(IFiltroTransmissaoBancoVM model, string codBanco, int codArquivo)
        {
            try
            {
                decimal valTaxa = 0;
                bool ExisteTituloParaGerarArquivo = false;
                PAGNET_TITULOS_PAGOS tITULOS_PAGOS = null;
                DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                foreach (var listaBorderos in model.ListaBorderosPGTO)
                {
                    var ListaTitulosBordero = _emissaoTitulos.BuscaTitulosByBordero(listaBorderos.codigoBordero).Result
                                       .Select(x => new ListaEmissaoTitulosVM(x)).ToList();


                    var ListaTitulosVencidos = ListaTitulosBordero.Where(x => x.DATPGTO < dataAtual).ToList();

                    //Remove os Titulos vencidos da lista que deverá ser paga
                    foreach (var tt in ListaTitulosVencidos)
                    {
                        var titulo = _emissaoTitulos.BuscaTituloByID(tt.CODIGOTITULO).Result;
                        titulo.STATUS = "EM_ABERTO";
                        titulo.CODBORDERO = null;

                        _emissaoTitulos.IncluiLog(titulo, _user.cod_usu, "Titulo não incluido no arquivo de remessa pois está com a data real para pagamento vencido.");
                        _emissaoTitulos.AtualizaTitulo(titulo);

                        //remove o titulo da lista
                        ListaTitulosBordero.RemoveAll(x => x.CODIGOTITULO == tt.CODIGOTITULO);
                    }


                    ListaTitulosBordero = ListaTitulosBordero.Where(x => x.TIPOTITULO == "BOLETO").ToList();
                    //atualiza Taxas para o pagamento
                    foreach (var item in ListaTitulosBordero)
                    {
                        valTaxa = 0;
                        var listaTaxa = _taxaTitulo.buscaTodasTaxasbyCodTitulo(item.CODIGOTITULO).Result;
                        if (listaTaxa != null)
                        {
                            valTaxa = listaTaxa.Where(x => x.ORIGEM == "PN").Sum(x => x.VALOR);

                            item.VALFINALPGTO = (item.VALLIQ + valTaxa);

                            var titulo = _emissaoTitulos.BuscaTituloByID(item.CODIGOTITULO).Result;
                            titulo.VALTOTAL = item.VALFINALPGTO;

                            _emissaoTitulos.AtualizaTitulo(titulo);
                        }
                    }


                    foreach (var titulos in ListaTitulosBordero)
                    {
                        tITULOS_PAGOS = new PAGNET_TITULOS_PAGOS();
                        var codigoTitulo = _pagamento.RetornaProximoNumeroTitulosPagos();
                        var SeuNumero = Helper.GeraSeuNumero(codigoTitulo, "");

                        if (SeuNumero.Length > 20)
                        {
                            SeuNumero = Helper.FormataTexto(SeuNumero, 20);
                        }

                        var nossoNumero = Helper.GeraNossoNumero("", titulos.CODIGOTITULO);
                        int codFormaLancamento = 0;
                        var DadosEmpresa = _cadEmpresa.ConsultaEmpresaById(titulos.CODEMPRESA).Result;

                        string tipoBoleto = titulos.LINHADIGITAVEL.Substring(0, 1);
                        string codbanco = titulos.LINHADIGITAVEL.Substring(0, 3);

                        if (tipoBoleto != "8")
                        {//boleto de cobrança normal
                            if (codbanco == codBanco)
                            {
                                codFormaLancamento = 30;
                            }
                            else
                            {
                                codFormaLancamento = 31;
                            }
                        }
                        else
                        {//boleto de consumo (agua, luz, telefone)
                            codFormaLancamento = 11;
                        }
                        tITULOS_PAGOS.CODTITULOPAGO = codigoTitulo;
                        tITULOS_PAGOS.STATUS = "AGUARDANDO_ARQUIVO_RETORNO";
                        tITULOS_PAGOS.CODUSUARIO = _user.cod_usu;
                        tITULOS_PAGOS.CODCONTACORRENTE = model.codigoContaCorrente;
                        tITULOS_PAGOS.CODBORDERO = listaBorderos.codigoBordero;
                        tITULOS_PAGOS.CODFAVORECIDO = titulos.CODFAVORECIDO;
                        tITULOS_PAGOS.CODEMPRESA = titulos.CODEMPRESA;
                        tITULOS_PAGOS.TIPOSERVICO = 20;
                        tITULOS_PAGOS.CODFORMALANCAMENTO = codFormaLancamento;
                        tITULOS_PAGOS.SEUNUMERO = SeuNumero;
                        tITULOS_PAGOS.NOSSONUMERO = nossoNumero;
                        tITULOS_PAGOS.DTPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTREALPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTVENCIMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.VALOR = titulos.VALFINALPGTO;
                        tITULOS_PAGOS.CODARQUIVO = codArquivo;
                        tITULOS_PAGOS.OCORRENCIARETORNO = "";
                        tITULOS_PAGOS.TIPOTITULO = "BOLETO";
                        tITULOS_PAGOS.LINHADIGITAVEL = titulos.LINHADIGITAVEL;

                        tITULOS_PAGOS = _pagamento.IncluiTransacao(tITULOS_PAGOS);

                        //Busca uma lista de registros

                        var tituloEmitido = _emissaoTitulos.BuscaTitulosByLinhaDigitavel(titulos.LINHADIGITAVEL).Result;
                        tituloEmitido.STATUS = "AGUARDANDO_ARQUIVO_RETORNO";
                        tituloEmitido.SEUNUMERO = SeuNumero;
                        tituloEmitido.CODCONTACORRENTE = model.codigoContaCorrente;
                        //Atualiza o título com o status e o seu número
                        _emissaoTitulos.AtualizaTitulo(tituloEmitido);
                        //Inseri log do título
                        _emissaoTitulos.IncluiLog(tituloEmitido, _user.cod_usu, "Título incluido no arquivo de remessa para pagamento");

                    }

                    //Atualiza status do borderô
                    if (ListaTitulosBordero.Count() == 0)
                    {
                        _bordero.AtualizaStatusBordero(listaBorderos.codigoBordero, "CANCELADO");
                    }
                    else
                    {
                        _bordero.AtualizaStatusBordero(listaBorderos.codigoBordero, "AGUARDANDO_ARQUIVO_RETORNO");
                        ExisteTituloParaGerarArquivo = true;
                    }
                }

                return ExisteTituloParaGerarArquivo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ResultadoGeracaoArquivo GeraArquivoRemessa(DadosGeracaoArquivoRemessa dadosGeracaoArquivo)
        {
            ResultadoGeracaoArquivo dadosret = new ResultadoGeracaoArquivo();
            var CaminhoAPI = _caminhoAPI.RetornaCaminhoAPI(dadosGeracaoArquivo.codigoBanco);
            using (var client = new HttpClient())
            {
                string relativePath = "GeraArquivoRemessa";

                string BaseAddress = @"https://www1.tln.com.br/";
                if (_user.ServidorDadosNetCard.ToUpper() == "NETUNO")
                {
                    BaseAddress = @"https://www3.tln.com.br/";
                }

                BaseAddress += CaminhoAPI;
                //BaseAddress = @"https://localhost:44387/";

                if (client.BaseAddress == null)
                {
                    //var token = (_TokensRepository.Get(_ContextAccessor.HttpContext.Request.GetSid()) as IAccessToken).Token;
                    var token = _ContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

                    client.BaseAddress = new Uri(BaseAddress);
                    client.Timeout = TimeSpan.FromMinutes(5);
                    client.DefaultRequestHeaders.Add("Authorization", $"{token}");
                }

                var content = new StringContent(JsonConvert.SerializeObject(dadosGeracaoArquivo),
                                                Encoding.UTF8,
                                                "application/json");

                var response = new ApiResponse<ResultadoGeracaoArquivo>(client.PostAsync(relativePath, content).Result);

                if (!response.Success)
                    throw new Exception(string.Join(Environment.NewLine, response.Errors));

                dadosret = response.Result;
            }


            return dadosret;
        }
        public List<RetornoLeituraArquivoModel> ProcessaArquivoRetorno(string caminhoArquivo)
        {
            List<RetornoLeituraArquivoModel> dadosret = new List<RetornoLeituraArquivoModel>();

            string[] LinhaArquivo = System.IO.File.ReadAllLines(caminhoArquivo);
            string codigoBanco = LinhaArquivo[0].Substring(0, 3);
            var CaminhoAPI = _caminhoAPI.RetornaCaminhoAPI(codigoBanco);
            bool duzentoQuarentaPosicoes = (LinhaArquivo[0].Length == 240);
            //Realizar a leitura do arquivo aqui mesmo, apenas criar dois metodos, uma para tipo de arquivo de 240 posicoes e outro para 400
            //não precisa utilizar uma api para realizar a leitura do arquivo de retorno.
            //if(duzentoQuarentaPosicoes)
            //{
            dadosret.AddRange(LeArquivo240Posicoes(caminhoArquivo));
            //}

            return dadosret;
        }
        private List<RetornoLeituraArquivoModel> LeArquivo240Posicoes(string caminhoArquivo)
        {
            try
            {
                string seuNumero = "";
                string msgRet = "Mensagem no arquivo de retorno do banco: ";
                bool ArqRetorno = true;
                decimal valorTotal = 0;
                int qtRegistro = 0;
                int qtRegistroOK = 0;
                int qtRegistroFalha = 0;
                string codigoRetornoTitulo = "";


                List<RetornoLeituraArquivoModel> DadosRet = new List<RetornoLeituraArquivoModel>();
                MetodosGerais _MetodosGerais = new MetodosGerais(_arquivo);

                string[] lines = File.ReadAllLines(caminhoArquivo);

                foreach (string line in lines)
                {
                    if (line.Substring(3, 4) == "0000" && line.Substring(142, 1) != "2")
                    {
                        ArqRetorno = false;
                        break;
                    }
                    else if (line.Substring(13, 1) == "A")
                    {
                        seuNumero = line.Substring(73, 20).Trim();

                        var pagamento = _pagamento.GetTransacaoBySeuNumero(seuNumero).Result;

                        if (pagamento != null)
                        {
                            qtRegistro += 1;
                            valorTotal += pagamento.VALOR;
                            if (line.Length > 231)
                            {
                                if (line.Substring(230, 2).Trim() != "")
                                {
                                    msgRet = _ocorrencia.ReturnOcorrencia(line.Substring(230, 2).Trim()).Trim() + "; ";
                                }
                                if (line.Length > 233)
                                {
                                    if (line.Substring(232, 2).Trim() != "")
                                    {
                                        msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(232, 2).Trim()).Trim() + "; ";
                                    }
                                    if (line.Length > 235)
                                    {
                                        if (line.Substring(234, 2).Trim() != "")
                                        {
                                            msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(234, 2).Trim()).Trim() + "; ";
                                        }
                                        if (line.Length > 237)
                                        {
                                            if (line.Substring(236, 2).Trim() != "")
                                            {
                                                msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(236, 2).Trim()).Trim() + "; ";
                                            }
                                            if (line.Length > 239)
                                            {
                                                if (line.Substring(238, 2).Trim() != "")
                                                {
                                                    msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(238, 2).Trim()).Trim() + "; ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (line.Substring(230, 2).Trim() == "00")
                                qtRegistroOK += 1;
                            else
                                qtRegistroFalha += 1;

                            codigoRetornoTitulo = line.Substring(230, line.Length - 230).Trim();
                            DadosRet.Add(new RetornoLeituraArquivoModel()
                            {
                                SeuNumero = pagamento.SEUNUMERO,
                                codigoRetorno = codigoRetornoTitulo,
                                TituloRecusado = (codigoRetornoTitulo != "00" && codigoRetornoTitulo.ToUpper() != "BD"),
                                Status = (string.IsNullOrWhiteSpace(msgRet)) ? "EM BORDERO" : _MetodosGerais.validaMsgRetorno(line.Substring(230, line.Length - 230).Trim(), (int)pagamento.CODARQUIVO),
                                RAZSOC = Helper.FormataTexto(pagamento.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 30),
                                CNPJ = Helper.FormataCPFCnPj(pagamento.PAGNET_CADFAVORECIDO.CPFCNPJ),
                                dataPGTO = pagamento.DTREALPAGAMENTO.ToShortDateString(),
                                ValorLiquido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", pagamento.VALOR),
                                MsgRetBanco = msgRet.Trim(),
                                mensagemRetorno = ""
                            });

                        }
                    }
                }
                string ResumoArquivo = "Valor Total do Arquivo: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal) + ". "
                                     + "Quantidade de registros no arquivo: " + qtRegistro + ". "
                                     + "Total liquidados: " + qtRegistroOK + ". "
                                     + "Total Recusados: " + qtRegistroFalha + ".";
                if (!ArqRetorno)
                {
                    DadosRet = new List<RetornoLeituraArquivoModel>();
                    DadosRet.Add(new RetornoLeituraArquivoModel()
                    {
                        mensagemRetorno = "Tipo de arquivo inválido. O arquivo não é um arquivo de retorno."
                    });
                }
                else if (DadosRet.Count == 0)
                {
                    DadosRet = new List<RetornoLeituraArquivoModel>();
                    DadosRet.Add(new RetornoLeituraArquivoModel()
                    {
                        mensagemRetorno = "Arquivo inválido. O arquivo não possui títulos origidados do sistema PagNet."
                    });
                }

                foreach (var linha in DadosRet)
                {
                    linha.Resumo = ResumoArquivo;
                    linha.vlTotalArquivo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal);
                    linha.qtRegistroArquivo = qtRegistro;
                    linha.qtRegistroOK = qtRegistroOK;
                    linha.qtRegistroFalha = qtRegistroFalha;
                    break;
                }
                ProcessaBaixaPagamento(DadosRet);
                return DadosRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void ProcessaBaixaPagamento(List<RetornoLeituraArquivoModel> model)
        {
            bool tudoCerto = true;
            int codArquivo = 0;
            int codBordero = 0;
            string StatusRetBanco = "";
            decimal ValorTaxaTed = 0;

            var resultado = new Dictionary<string, string>();

            try
            {
                PAGNET_TITULOS_PAGOS dadosPag;
                List<PAGNET_EMISSAO_TITULOS> ListaTitulos;

                foreach (var linha in model)
                {

                    dadosPag = new PAGNET_TITULOS_PAGOS();
                    dadosPag = _pagamento.GetTransacaoBySeuNumero(linha.SeuNumero).Result;

                    var arquivo = _arquivo.ReturnFileById(dadosPag.CODARQUIVO).Result;

                    //valida se o arquivo está cancelado. caso positivo não deixa subir o arquivo.
                    if (arquivo.STATUS == "CANCELADO")
                    {
                        return;
                    }
                    //valida se o título já foi baixado, isso acontece quando sobe os arquivos de retorno fora de ordem.
                    if (linha.Status == "AGUARDANDO_ARQUIVO_RETORNO" && dadosPag.STATUS == "BAIXADO")
                    {
                        continue;
                    }

                    //Verifica se o título já foi Recusado pelo banco.
                    if (dadosPag.STATUS == "RECUSADO")
                    {
                        continue;
                    }
                    // só permite processar um arquivo de retorno de um titulo já pago, apenas se o código do retorno for de estorno de pagamento;
                    if (linha.TituloRecusado && dadosPag.STATUS == "BAIXADO" && linha.codigoRetorno != "H4")
                    {
                        continue;
                    }

                    if (linha.MsgRetBanco.Length > 7999)
                        dadosPag.OCORRENCIARETORNO = linha.MsgRetBanco.Substring(0, 7999);
                    else
                        dadosPag.OCORRENCIARETORNO = linha.MsgRetBanco;


                    if (dadosPag.STATUS != linha.Status)
                    {
                        dadosPag.STATUS = linha.Status;
                        StatusRetBanco = linha.Status;

                        codBordero = dadosPag.CODBORDERO;
                        tudoCerto = _pagamento.AtualizaTransacao(dadosPag).Result;

                    }

                    codArquivo = dadosPag.CODARQUIVO;
                    StatusRetBanco = dadosPag.STATUS;

                    //ATUALIZA STATUS DO BORDERO
                    if (codBordero != dadosPag.CODBORDERO)
                    {
                        //FIZ ISSO PARA ATUALIZAR APENAS UMA VEZ O BORDERÔ
                        codBordero = dadosPag.CODBORDERO;
                        _bordero.AtualizaStatusBordero(dadosPag.CODBORDERO, StatusRetBanco);
                    }

                    ListaTitulos = new List<PAGNET_EMISSAO_TITULOS>();

                    ListaTitulos = _emissaoTitulos.BuscaTituloBySeuNumero(dadosPag.SEUNUMERO).Result;

                    foreach (var item in ListaTitulos)
                    {
                        //Remove a taxa de transferencia caso o tpitulo tenha sido recusado
                        if (linha.TituloRecusado)
                        {
                            ValorTaxaTed = RemoveTaxaTransferencia(item);
                            item.VALTOTAL += ValorTaxaTed;
                        }

                        item.STATUS = StatusRetBanco;
                        item.CODCONTACORRENTE = dadosPag.CODCONTACORRENTE;

                        _emissaoTitulos.AtualizaTitulo(item);

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(item, _user.cod_usu, linha.MsgRetBanco);

                        //ATUALIZA SALDO DA CONTACA CORRENTE
                        //AtualizaSaldoContaCorrente(dadosPag.CODCONTACORRENTE, item.CODTITULO);
                    }
                    if (!tudoCerto)
                    {
                        return;
                    }
                }


                if (codArquivo > 0)
                {
                    var arquivo = _arquivo.ReturnFile(codArquivo).Result;
                    arquivo.STATUS = StatusRetBanco;
                    _arquivo.AtualizaArquivo(arquivo);

                    _bordero.AtualizaStatusBordero(codBordero, StatusRetBanco);
                }

            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }
        private decimal RemoveTaxaTransferencia(PAGNET_EMISSAO_TITULOS Titulo)
        {
            decimal valorTaxa = 0;
            var listaTaxa = _taxaTitulo.buscaTodasTaxasbyCodTitulo(Titulo.CODTITULO).Result;
            var taxaTed = listaTaxa.Where(x => x.DESCRICAO == "TAXA DE TRANSFERÊNCIA").FirstOrDefault();

            if (taxaTed != null)
            {
                valorTaxa = Math.Abs(taxaTed.VALOR);
                _taxaTitulo.RemoveTaxa(taxaTed.CODTAXATITULO);
                //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                _emissaoTitulos.IncluiLog(Titulo, _user.cod_usu, "Remoção de taxa de transferência.");
            }
            return valorTaxa;
        }
        public void AtualizaSaldoContaCorrente(int codcontacorrente, int codTitulo)
        {
            try
            {
                var dadosContaCorrente = _conta.GetContaCorrenteById(codcontacorrente).Result;
                var dadosTitulo = _emissaoTitulos.BuscaTituloByID(codTitulo).Result;
                var dadosTaxas = _taxaTitulo.buscaTodasTaxasbyCodTitulo(codTitulo).Result;

                decimal valorPago = 0;
                decimal valorTaxas = 0;
                decimal saldoContaCorrente = 0;

                valorPago = dadosTitulo.VALTOTAL;
                saldoContaCorrente = _conta.RetornaSaldoAtual(codcontacorrente);

                if (dadosTaxas != null)
                {
                    valorTaxas = dadosTaxas.Select(x => x.VALOR).Sum();
                }

                decimal CalculoSaldoAtual = saldoContaCorrente - (valorPago + valorTaxas);

                _conta.InseriNovoSaldo(codcontacorrente, dadosContaCorrente.CODEMPRESA, CalculoSaldoAtual);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RetornoModel CancelaArquivoRemessaByID(int codigoArquivo)
        {

            var resultado = new RetornoModel();

            try
            {
                if (codigoArquivo == 0)
                {
                    resultado.Sucesso = false;
                    resultado.msgResultado = "Código do Arquivo Inválido";
                    return resultado;
                }
                var arq = _arquivo.ReturnFileById(codigoArquivo).Result;


                arq.STATUS = "CANCELADO";

                _arquivo.AtualizaArquivo(arq);

                var listaPagamentos = _pagamento.GetPagamentosByCodArquivo(arq.CODARQUIVO).ToList();
                var listaBorderos = listaPagamentos.Select(x => x.CODBORDERO).Distinct();

                foreach (var pag in listaPagamentos)
                {
                    var result = _pagamento.AtualizaStatus("CANCELADO", pag.CODTITULOPAGO).Result;
                }

                //Atualiza todos os borderos e títulos
                foreach (var codBordero in listaBorderos)
                {
                    _bordero.AtualizaStatusBordero(codBordero, "EM_BORDERO");
                    var titulos = _emissaoTitulos.BuscaTitulosByBordero(codBordero).Result;
                    foreach (var t in titulos)
                    {
                        t.STATUS = "EM_BORDERO";
                        t.SEUNUMERO = "";
                        if (t.TIPOTITULO == "TED") t.TIPOTITULO = "TEDDOC";
                        if (t.TIPOTITULO == "CC") t.TIPOTITULO = "TEDDOC";

                        _emissaoTitulos.AtualizaTitulo(t);
                    }


                    _emissaoTitulos.IncluiLogByBordero(codBordero, "EM_BORDERO", _user.cod_usu, "Arquivo de Remessa Cancelado");
                    //cancela taxa de TED
                    var Listatitulos = _emissaoTitulos.BuscaTitulosByBordero(codBordero).Result;
                    foreach (var tt in Listatitulos)
                    {
                        var listaTaxa = _taxaTitulo.buscaTodasTaxasbyCodTitulo(tt.CODTITULO).Result;
                        var taxaTED = listaTaxa.Where(x => x.DESCRICAO == "TAXA DE TRANSFERÊNCIA").FirstOrDefault();
                        if (taxaTED != null)
                        {
                            tt.VALTOTAL = tt.VALTOTAL + Math.Abs(taxaTED.VALOR);
                            _emissaoTitulos.AtualizaTitulo(tt);

                            _taxaTitulo.RemoveTaxa(taxaTED.CODTAXATITULO);
                        }
                    }

                }


                resultado.Sucesso = true;
                resultado.msgResultado = "Arquivo Cancelado com sucesso";

            }
            catch (ArgumentException ex)
            {
                resultado.Sucesso = false;
                resultado.msgResultado = "Falha ao cancelar o arquivo. Favor contactar o suporte.";
            }
            return resultado;
        }
        private void RollbackGeracaoArquivo(IFiltroTransmissaoBancoVM model)
        {
            decimal ValorTaxaTed = 0;
            foreach (var listaBorderos in model.ListaBorderosPGTO)
            {
                var ListaTitulosBordero = _emissaoTitulos.BuscaTitulosByBordero(listaBorderos.codigoBordero).Result
                                    .Select(x => new ListaEmissaoTitulosVM(x)).ToList();

                //VOLTA O STATUS DO BORDERO
                _bordero.AtualizaStatusBordero(listaBorderos.codigoBordero, "EM_BORDERO");

                //Realiza o roolback nos títulos
                foreach (var tt in ListaTitulosBordero)
                {
                    var titulo = _emissaoTitulos.BuscaTituloByID(tt.CODIGOTITULO).Result;
                    //Se o título possuir o seu numero quer dizer que foi criado um registro na tabela de títulos pagos
                    if (titulo.STATUS == "AGUARDANDO_ARQUIVO_RETORNO")
                    {
                        var tituloPago = _pagamento.GetTransacaoBySeuNumero(titulo.SEUNUMERO).Result;
                        if (tituloPago != null && tituloPago.STATUS != "CANCELADO")
                        {
                            _pagamento.AtualizaStatus("CANCELADO", tituloPago.CODTITULOPAGO);
                        }

                        titulo.SEUNUMERO = null;
                        ValorTaxaTed = RemoveTaxaTransferencia(titulo);
                        titulo.VALTOTAL += ValorTaxaTed;
                        titulo.STATUS = "EM_BORDERO";


                        //atualiando dados dos título
                        _emissaoTitulos.AtualizaTitulo(titulo);

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(titulo, _user.cod_usu, "Falha ao gerar o arquivo de remessa.");
                    }

                }

            }
        }
    }
}
