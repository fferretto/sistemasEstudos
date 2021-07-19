using PagNet.Application.Cnab.Comun;
using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Application.Interface.ProcessoCnab;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using PagNet.Domain.Interface.Services.Procedures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PagNet.Application.Application
{
    public class PagamentoApp : IPagamentoApp
    {
        private readonly IPagNet_Emissao_TitulosService _emissaoTitulos;
        private readonly IProcessaDadosSantander _santander;
        private readonly IProcessaDadosBradesco _bradesco;
        private readonly IProcessaDadosItau _itau;
        private readonly IProcessaDadosBancoBrasil _bancoBrasil;
        private readonly IProcessaDadosCEF _bancoCEF;
        private readonly IProcessaDadosBrasilABC _brasilABC;
        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_OcorrenciaRetPagService _ocorrencia;
        private readonly IPagNet_Titulos_PagosService _pagamento;
        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_CadEmpresaService _cadEmpresa;
        private readonly IPagNet_CadFavorecidoService _cadFavorito;
        private readonly IPagNet_BancoService _banco;
        private readonly IOperadoraService _ope;
        private readonly IPagNet_Bordero_PagamentoService _bordero;
        private readonly IProceduresService _proc;
        private readonly IPagNet_Config_RegraService _regra;
        private readonly IPagNet_Taxas_TitulosService _taxaTitulo;

        public PagamentoApp(IPagNet_Emissao_TitulosService emissaoTitulos,
                                        IProcessaDadosSantander santander,
                                        IProcessaDadosBradesco bradesco,
                                        IPagNet_ContaCorrenteService conta,
                                        IProcessaDadosItau itau,
                                        IProcessaDadosBancoBrasil bancoBrasil,
                                        IProcessaDadosBrasilABC brasilABC,
                                        IProcessaDadosCEF bancoCEF,
                                        IPagNet_OcorrenciaRetPagService ocorrencia,
                                        IPagNet_Titulos_PagosService pagamento,
                                        IPagNet_ArquivoService arquivo,
                                        IPagNet_CadEmpresaService cadEmpresa,
                                        IPagNet_CadFavorecidoService cadFavorito,
                                        IOperadoraService ope,
                                        IPagNet_BancoService banco,
                                        IPagNet_Config_RegraService regra,
                                        IPagNet_Bordero_PagamentoService bordero,
                                        IProceduresService proc,
                                        IPagNet_Taxas_TitulosService taxaTitulo)
        {
            _emissaoTitulos = emissaoTitulos;
            _santander = santander;
            _bradesco = bradesco;
            _itau = itau;
            _bancoCEF = bancoCEF;
            _brasilABC = brasilABC;
            _bancoBrasil = bancoBrasil;
            _conta = conta;
            _ocorrencia = ocorrencia;
            _pagamento = pagamento;
            _arquivo = arquivo;
            _cadEmpresa = cadEmpresa;
            _cadFavorito = cadFavorito;
            _banco = banco;
            _ope = ope;
            _bordero = bordero;
            _proc = proc;
            _regra = regra;
            _taxaTitulo = taxaTitulo;
        }

        public FiltroTitulosPagamentoVM RetornaDadosInicio(string dados, int CodEmpresa)
        {
            FiltroTitulosPagamentoVM model = new FiltroTitulosPagamentoVM();

            if (!string.IsNullOrWhiteSpace(dados))
            {
                var param = dados.Split(';');
                model.dtInicio = param[0];
                model.dtFim = param[1];
                model.codFavorecido = Convert.ToInt32(param[3]);
                model.CaminhoArquivoDownload = param[4];
            }
            else
            {
                DateTime data = DateTime.Now;

                model.dtInicio = data.ToShortDateString();
                model.dtFim = data.ToShortDateString();
            }


            var empresa = _cadEmpresa.ConsultaEmpresaById(CodEmpresa).Result;
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public ConsultaFechamentoCredVm RetornaDadosInicioConsultaCred(string dados, int codEmpresa)
        {
            ConsultaFechamentoCredVm model = new ConsultaFechamentoCredVm();


            if (!string.IsNullOrWhiteSpace(dados))
            {
                var param = dados.Split(';');
                model.dtInicio = param[0];
                model.dtFim = param[1];
                model.codFavorecido = Convert.ToInt32(param[3]);
            }
            else
            {
                DateTime data = DateTime.Now;
                data = data.AddDays(-1);

                model.dtInicio = data.ToShortDateString();
                model.dtFim = data.ToShortDateString();
            }


            var empresa = _cadEmpresa.ConsultaEmpresaById(codEmpresa).Result;
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public FiltroDownloadArquivoVm RetornaDadosInicioDownloadCred(int codEmpresa)
        {
            FiltroDownloadArquivoVm model = new FiltroDownloadArquivoVm();


            DateTime data = DateTime.Now;

            model.dtInicio = data.ToShortDateString();
            model.dtFim = data.ToShortDateString();

            var empresa = _cadEmpresa.ConsultaEmpresaById(codEmpresa).Result;
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public IndicaroresVM RetornaDadosInicioIndicadoresCred(int codEmpresa)
        {
            IndicaroresVM model = new IndicaroresVM();

            model.dtInicio = DateTime.Now.AddDays(-30).ToShortDateString();
            model.dtFim = DateTime.Now.ToShortDateString();

            var empresa = _cadEmpresa.ConsultaEmpresaById(codEmpresa).Result;
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public async Task<List<BaixaPagamentoVM>> CarregaDadosArquivoRemessa(string CaminhoArquivo)
        {
            try
            {
                string seuNumero = "";
                bool ArqRetorno = true;
                decimal valorTotal = 0;
                int qtRegistro = 0;

                List<BaixaPagamentoVM> DadosRet = new List<BaixaPagamentoVM>();

                string[] lines = File.ReadAllLines(CaminhoArquivo);

                foreach (string line in lines)
                {
                    if (line.Substring(3, 4) == "0000" && line.Substring(142, 1) != "1")
                    {
                        ArqRetorno = false;
                        break;
                    }
                    if (line.Substring(13, 1) == "A")
                    {
                        seuNumero = line.Substring(73, 20).Trim();

                        var pagamento = _pagamento.GetTransacaoBySeuNumero(seuNumero).Result;

                        if (pagamento != null)
                        {
                            valorTotal += pagamento.VALOR;
                            qtRegistro += 1;

                            DadosRet.Add(new BaixaPagamentoVM()
                            {
                                SeuNumero = pagamento.SEUNUMERO,
                                CodRetorno = line.Substring(230, line.Length - 230).Trim(),
                                Status = pagamento.STATUS.Replace("_", " "),
                                RAZSOC = Geral.FormataTexto(pagamento.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 30),
                                CNPJ = Geral.FormataCPFCnPj(pagamento.PAGNET_CADFAVORECIDO.CPFCNPJ),
                                DATPGTO = pagamento.DTREALPAGAMENTO.ToShortDateString(),
                                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", pagamento.VALOR),
                                MsgRetorno = ""
                            });

                        }
                    }
                }
                string ResumoArquivo = "Valor Total do Arquivo: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal) + ". "
                                     + "Quantidade de registros no arquivo: " + qtRegistro + ". ";
                if (!ArqRetorno)
                {
                    DadosRet = new List<BaixaPagamentoVM>();
                    DadosRet.Add(new BaixaPagamentoVM()
                    {
                        MsgRetorno = "Tipo de arquivo inválido. O arquivo não é um arquivo de remessa."
                    });
                }

                foreach (var linha in DadosRet)
                {
                    linha.Resumo = ResumoArquivo;
                    linha.vlTotalArquivo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal);
                    linha.qtRegistroArquivo = qtRegistro;
                    break;
                }
                return DadosRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<BaixaPagamentoVM>> CarregaDadosRetorno(string CaminhoArquivo, int codUsuario)
        {
            try
            {
                List<BaixaPagamentoVM> DadosRet = new List<BaixaPagamentoVM>();

                string[] lines = File.ReadAllLines(CaminhoArquivo);

                string codBanco = lines[0].Substring(0, 3);

                if (codBanco == "033")
                {
                    DadosRet = _santander.ProcessaArquivoRetorno(CaminhoArquivo).Result;
                }
                if (codBanco == "237")
                {
                    DadosRet = _bradesco.ProcessaArquivoRetorno(CaminhoArquivo).Result;
                }
                if (codBanco == "246")
                {
                    DadosRet = _brasilABC.ProcessaArquivoRetorno(CaminhoArquivo).Result;
                }
                if (codBanco == "341")
                {
                    DadosRet = _itau.ProcessaArquivoRetorno(CaminhoArquivo).Result;
                }
                if (codBanco == "001")
                {
                    DadosRet = _bancoBrasil.ProcessaArquivoRetorno(CaminhoArquivo).Result;
                }
                if (codBanco == "104")
                {
                    DadosRet = _bancoCEF.ProcessaArquivoRetorno(CaminhoArquivo).Result;
                }



                //Realiza baixa do arquivo de retorno
                var result = ProcessaBaixaPagamento(DadosRet, codUsuario);

                return DadosRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FiltroBorderoPagVM CarregaGridTitulos(FiltroTitulosPagamentoVM model)
        {
            try
            {
                FiltroBorderoPagVM retorno;

                model.codBanco = model.codBanco ?? "";

                if (model.codBanco != "")
                {
                    model.codBanco = (Convert.ToInt32(model.codBanco)).ToString();
                    while (model.codBanco.Length < 3)
                    {
                        model.codBanco = "0" + model.codBanco;
                    }
                }

                var Cred = _proc.BuscaListaTitulosEmAberto(Convert.ToDateTime(model.dtInicio), Convert.ToDateTime(model.dtFim),
                    model.codFavorecido, Convert.ToInt32(model.codEmpresa), Convert.ToInt32(model.codContaCorrente), model.codBanco).ToList();
                var dasd = Cred.Where(x => x.CODFAVORECIDO == 430733).ToList();

                retorno = FiltroBorderoPagVM.ToView(Cred, Convert.ToDateTime(model.dtInicio), Convert.ToDateTime(model.dtFim));

                retorno.qtTitulosSelecionados = Cred.Count.ToString();
                retorno.ValorBordero = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Cred.Sum(x => x.VALORPREVISTOPAGAMENTO)).Replace("R$", "");

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ConsultaArquivosRemessaVM>> CarregaGridArquivo(FiltroDownloadArquivoVm model)
        {
            try
            {
                string nmArquivo = "";

                DateTime DataInicio = Convert.ToDateTime(model.dtInicio);
                DateTime DataFinal = Convert.ToDateTime(model.dtFim + " 23:59:59");
                int codEmpresa = Convert.ToInt32(model.codEmpresa);

                List<ConsultaArquivosRemessaVM> retorno;
                retorno = _arquivo.GetFileByDate(DataInicio, DataFinal, codEmpresa, "PAG")
                .Select(x => new ConsultaArquivosRemessaVM(x, codEmpresa)).ToList();

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FiltroTitulosPagamentoVM FindBancoByID(int id)
        {
            string CodBanco = id.ToString();

            while (CodBanco.Length < 3)
            {
                CodBanco = "0" + CodBanco;
            }

            var ret = _banco.getBancoByID(CodBanco).Result;

            if (ret == null)
            {
                FiltroTitulosPagamentoVM m = new FiltroTitulosPagamentoVM();
                m.FiltroNmBanco = "Não Encontrado";
                m.filtro = CodBanco;

                return m;
            }
            return FiltroTitulosPagamentoVM.ToViewBanco(ret, CodBanco);
        }
        public async Task<string> GeraArquivoRemessaAsync(BorderoPagVM model)
        {
            try
            {
                string Retorno = "";

                var caminhoPadrao = _ope.GetOperadoraById(model.codOpe).Result;

                var pathRemessa = Path.Combine(caminhoPadrao.CAMINHOARQUIVO, caminhoPadrao.NOMOPERAFIL, model.codEmpresa.ToString(), "ArquivoRemessaPagamento");

                if (!Directory.Exists(pathRemessa))
                    Directory.CreateDirectory(pathRemessa);

                model.CaminhoArquivo = pathRemessa;

                var contacorrete = _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente)).Result;

                if (string.IsNullOrEmpty(contacorrete.CODCONVENIOPAG))
                {
                    throw new Exception("Obrigatório informar uma conta corrente que possua um código de convêncio de pagamento cadastrado.");
                }

                //Grava dados do Arquivo de Remessa
                var codArquivo = GravaArquivoRemessa(model, contacorrete.CODBANCO);
                //Inseri os títulos para pagamento
                IncluiTituloTEDDOCPagamento(model, contacorrete.CODBANCO, codArquivo);

                //Inseri os títulos para pagamento
                IncluiTituloBoletoPagamento(model, contacorrete.CODBANCO, codArquivo);

                if (contacorrete.CODBANCO == "033")
                {
                    Retorno = await _santander.GeraArquivoSantander(model, codArquivo);
                }

                else if (contacorrete.CODBANCO == "237")
                {
                    Retorno = await _bradesco.GeraArquivoBradesco(model, codArquivo);
                }
                else if (contacorrete.CODBANCO == "246")
                {
                    Retorno = await _brasilABC.GeraArquivoBradesco(model, codArquivo);
                }
                else if (contacorrete.CODBANCO == "341")
                {
                    Retorno = await _itau.GeraArquivoItau(model, codArquivo);
                }
                else if (contacorrete.CODBANCO == "001")
                {
                    Retorno = await _bancoBrasil.GeraArquivoBancoBrasil(model, codArquivo);
                }
                else if (contacorrete.CODBANCO == "104")
                {
                    Retorno = await _bancoCEF.GeraArquivoCEF(model, codArquivo);
                }

                return Retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int GravaArquivoRemessa(BorderoPagVM model, string codBanco)
        {
            try
            {
                string nmArquivo;
                int TotalRegistros = 0;
                decimal vlTotalArquivo = 0;
                int codArquivo = 0;
                if (model.ListaBordero.Count > 0)
                {
                    foreach (var bordero in model.ListaBordero)
                    {
                        var titulos = _emissaoTitulos.BuscaTitulosByBordero(bordero.CODBORDERO).Result;
                        TotalRegistros += titulos.Count;
                        vlTotalArquivo += titulos.Sum(x => x.VALTOTAL);
                    }

                    MetodosGeraisdd _MetodosGeraisdd = new MetodosGeraisdd(_arquivo);
                    string CaminhoArquivo = model.CaminhoArquivo;

                    nmArquivo = "PG_" + codBanco + DateTime.Now.ToString("ddMMyyHHmmss") + ".rem";

                    int nroSeqArquivo = _MetodosGeraisdd.RetornaNroSeqArquivo();

                    var Status = "AGUARDANDO_ARQUIVO_RETORNO";
                    var TipArqu = "PAG";
                    codArquivo = _MetodosGeraisdd.InseriArquivo(CaminhoArquivo, nmArquivo, nroSeqArquivo, vlTotalArquivo, TotalRegistros, Status, TipArqu, codBanco);


                }
                return codArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void IncluiTituloTEDDOCPagamento(BorderoPagVM model, string codBanco, int codArquivo)
        {
            try
            {
                decimal valTaxa = 0;
                decimal taxaTED = 0;
                bool TituloHomologacao = false;
                bool taxaIncluida = false;
                bool taxaTEDExistente = false;

                PAGNET_TITULOS_PAGOS tITULOS_PAGOS = null;
                foreach (var listaBorderos in model.ListaBordero)
                {
                    taxaTEDExistente = false;
                    var ListaTitulosBordero = _emissaoTitulos.BuscaTitulosByBordero(listaBorderos.CODBORDERO).Result
                                        .Select(x => new ListaEmissaoTitulosVM(x)).ToList();

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

                            item.VALLIQ = (item.VALLIQ + valTaxa);

                            var titulo = _emissaoTitulos.BuscaTituloByID(item.CODIGOTITULO).Result;
                            titulo.VALTOTAL = item.VALLIQ;

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
                                               VALLIQ = g.Sum(s => s.VALLIQ)
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
                        var SeuNumero = Geral.GeraSeuNumero(codigoTitulo, "");

                        if (SeuNumero.Length > 20)
                        {
                            SeuNumero = Geral.FormataTexto(SeuNumero, 20);
                        }

                        var nossoNumero = Geral.GeraNossoNumero("", TitPag.CODIGOTITULO);
                        int codFormaLancamento = 0;
                        var DadosEmpresa = _cadEmpresa.ConsultaEmpresaById(TitPag.CODEMPRESA).Result;


                        if (TitPag.CODBANCO == codBanco)
                        {
                            codFormaLancamento = 1;
                            taxaTED = 0;
                        }
                        else
                        {
                            if (!TituloHomologacao)
                            {
                                taxaTED = RetornaTaxaTransferencia(TitPag.CODFAVORECIDO, Convert.ToInt32(model.codContaCorrente), titulos.VALLIQ);
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
                        tITULOS_PAGOS.CODTITULOPAGO = codigoTitulo;
                        tITULOS_PAGOS.STATUS = "AGUARDANDO_ARQUIVO_RETORNO";
                        tITULOS_PAGOS.CODUSUARIO = model.codUsuario;
                        tITULOS_PAGOS.CODCONTACORRENTE = Convert.ToInt32(model.codContaCorrente);
                        tITULOS_PAGOS.CODBORDERO = listaBorderos.CODBORDERO;
                        tITULOS_PAGOS.CODFAVORECIDO = TitPag.CODFAVORECIDO;
                        tITULOS_PAGOS.CODEMPRESA = TitPag.CODEMPRESA;
                        tITULOS_PAGOS.TIPOSERVICO = 20;
                        tITULOS_PAGOS.CODFORMALANCAMENTO = codFormaLancamento;
                        tITULOS_PAGOS.SEUNUMERO = SeuNumero;
                        tITULOS_PAGOS.NOSSONUMERO = nossoNumero;
                        tITULOS_PAGOS.DTPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTREALPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTVENCIMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.VALOR = titulos.VALLIQ - taxaTED;
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
                            tituloEmitido.CODCONTACORRENTE = Convert.ToInt32(model.codContaCorrente);
                            if (!TituloHomologacao)
                                tituloEmitido.TIPOTITULO = (codFormaLancamento == 1) ? "CC" : "TED";

                            //fiz isso para incluir apenas uma vez e para o mesmo titulo que a taxa foi aplicada
                            if (!taxaIncluida && taxaTED > 0 && !taxaTEDExistente)
                            {
                                tituloEmitido.VALTOTAL = tituloEmitido.VALTOTAL - taxaTED;
                            }

                            //Atualiza o título com o status e o seu número
                            _emissaoTitulos.AtualizaTitulo(tituloEmitido);
                            //Inseri log do título
                            _emissaoTitulos.IncluiLog(tituloEmitido, model.codUsuario, "Título incluido no arquivo de remessa para pagamento");

                            if (!taxaIncluida && taxaTED > 0 && !taxaTEDExistente)
                            {
                                PAGNET_TAXAS_TITULOS tx = new PAGNET_TAXAS_TITULOS();
                                tx.CODTITULO = tituloEmitido.CODTITULO;
                                tx.DESCRICAO = "TAXA DE TRANSFERÊNCIA";
                                tx.VALOR = -1 * taxaTED;
                                tx.ORIGEM = "PN";
                                tx.DTINCLUSAO = DateTime.Now;
                                tx.CODUSUARIO = model.codUsuario;

                                _taxaTitulo.IncluiTaxa(tx);
                                //Inseri log do título
                                _emissaoTitulos.IncluiLog(tituloEmitido, model.codUsuario, "Inclusão de taxa de transferência.");
                                taxaIncluida = true;
                            }
                        }
                    }

                    //Atualiza status do borderô
                    _bordero.AtualizaStatusBordero(listaBorderos.CODBORDERO, "AGUARDANDO_ARQUIVO_RETORNO");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void IncluiTituloBoletoPagamento(BorderoPagVM model, string codBanco, int codArquivo)
        {
            try
            {
                decimal valTaxa = 0;
                PAGNET_TITULOS_PAGOS tITULOS_PAGOS = null;
                foreach (var listaBorderos in model.ListaBordero)
                {
                    var ListaTitulosBordero = _emissaoTitulos.BuscaTitulosByBordero(listaBorderos.CODBORDERO).Result
                                       .Select(x => new ListaEmissaoTitulosVM(x)).ToList();

                    ListaTitulosBordero = ListaTitulosBordero.Where(x => x.TIPOTITULO == "BOLETO").ToList();
                    //atualiza Taxas para o pagamento
                    foreach (var item in ListaTitulosBordero)
                    {
                        valTaxa = 0;
                        var listaTaxa = _taxaTitulo.buscaTodasTaxasbyCodTitulo(item.CODIGOTITULO).Result;
                        if (listaTaxa != null)
                        {
                            valTaxa = listaTaxa.Where(x => x.ORIGEM == "PN").Sum(x => x.VALOR);

                            item.VALLIQ = (item.VALLIQ + valTaxa);

                            var titulo = _emissaoTitulos.BuscaTituloByID(item.CODIGOTITULO).Result;
                            titulo.VALTOTAL = item.VALLIQ;

                            _emissaoTitulos.AtualizaTitulo(titulo);
                        }
                    }


                    foreach (var titulos in ListaTitulosBordero)
                    {
                        tITULOS_PAGOS = new PAGNET_TITULOS_PAGOS();
                        var codigoTitulo = _pagamento.RetornaProximoNumeroTitulosPagos();
                        var SeuNumero = Geral.GeraSeuNumero(codigoTitulo, "");

                        if (SeuNumero.Length > 20)
                        {
                            SeuNumero = Geral.FormataTexto(SeuNumero, 20);
                        }

                        var nossoNumero = Geral.GeraNossoNumero("", titulos.CODIGOTITULO);
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
                        tITULOS_PAGOS.CODUSUARIO = model.codUsuario;
                        tITULOS_PAGOS.CODCONTACORRENTE = Convert.ToInt32(model.codContaCorrente);
                        tITULOS_PAGOS.CODBORDERO = listaBorderos.CODBORDERO;
                        tITULOS_PAGOS.CODFAVORECIDO = titulos.CODFAVORECIDO;
                        tITULOS_PAGOS.CODEMPRESA = titulos.CODEMPRESA;
                        tITULOS_PAGOS.TIPOSERVICO = 20;
                        tITULOS_PAGOS.CODFORMALANCAMENTO = codFormaLancamento;
                        tITULOS_PAGOS.SEUNUMERO = SeuNumero;
                        tITULOS_PAGOS.NOSSONUMERO = nossoNumero;
                        tITULOS_PAGOS.DTPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTREALPAGAMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.DTVENCIMENTO = titulos.DATPGTO;
                        tITULOS_PAGOS.VALOR = titulos.VALLIQ;
                        tITULOS_PAGOS.CODARQUIVO = codArquivo;
                        tITULOS_PAGOS.OCORRENCIARETORNO = "";
                        tITULOS_PAGOS.TIPOTITULO = "BOLETO";
                        tITULOS_PAGOS.LINHADIGITAVEL = titulos.LINHADIGITAVEL;

                        tITULOS_PAGOS = _pagamento.IncluiTransacao(tITULOS_PAGOS);

                        //Busca uma lista de registros

                        var tituloEmitido = _emissaoTitulos.BuscaTitulosByLinhaDigitavel(titulos.LINHADIGITAVEL).Result;
                        tituloEmitido.STATUS = "AGUARDANDO_ARQUIVO_RETORNO";
                        tituloEmitido.SEUNUMERO = SeuNumero;
                        tituloEmitido.CODCONTACORRENTE = Convert.ToInt32(model.codContaCorrente);
                        //Atualiza o título com o status e o seu número
                        _emissaoTitulos.AtualizaTitulo(tituloEmitido);
                        //Inseri log do título
                        _emissaoTitulos.IncluiLog(tituloEmitido, model.codUsuario, "Título incluido no arquivo de remessa para pagamento");

                    }

                    //Atualiza status do borderô
                    _bordero.AtualizaStatusBordero(listaBorderos.CODBORDERO, "AGUARDANDO_ARQUIVO_RETORNO");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private decimal RetornaTaxaTransferencia(int codFavorito, int codContaCorrente, decimal valPag)
        {
            decimal TaxaTed = 0;
            var dadosBancarios = _conta.GetContaCorrenteById(codContaCorrente).Result;

            TaxaTed = Convert.ToDecimal(dadosBancarios.VALTED);

            var DadosFavorito = _cadFavorito.BuscaFavorecidosByID(codFavorito).Result;

            return TaxaTed;
        }
        public async Task<IDictionary<string, string>> ProcessaBaixaPagamento(List<BaixaPagamentoVM> model, int codUsuario)
        {
            bool tudoCerto = true;
            int codArquivo = 0;
            int codBordero = 0;
            string StatusRetBanco = "";

            var resultado = new Dictionary<string, string>();

            try
            {
                PAGNET_TITULOS_PAGOS dadosPag;
                List<PAGNET_EMISSAO_TITULOS> ListaTitulos;

                foreach (var linha in model)
                {

                    dadosPag = new PAGNET_TITULOS_PAGOS();
                    dadosPag = _pagamento.GetTransacaoBySeuNumero(linha.SeuNumero).Result;

                    var arquivo = await _arquivo.ReturnFileById(dadosPag.CODARQUIVO);

                    //valida se o arquivo está cancelado. caso positivo não deixa subir o arquivo.
                    if (arquivo.STATUS == "CANCELADO")
                    {
                        resultado.Add("Falha", "O arquivo está cancelado no sistema PagNet");
                        return resultado;
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
                    if (linha.Status == "RECUSADO" && dadosPag.STATUS == "BAIXADO" && linha.CodRetorno != "H4")
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
                        tudoCerto = await _pagamento.AtualizaTransacao(dadosPag);

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
                        item.STATUS = StatusRetBanco;
                        item.CODCONTACORRENTE = dadosPag.CODCONTACORRENTE;

                        _emissaoTitulos.AtualizaTitulo(item);

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(item, codUsuario, linha.MsgRetBanco);

                        //ATUALIZA SALDO DA CONTACA CORRENTE
                        AtualizaSaldoContaCorrente(dadosPag.CODCONTACORRENTE, item.CODTITULO);
                    }
                    if (!tudoCerto)
                    {
                        resultado.Add("Falha", "Falha ao atualizar codigo " + linha.SeuNumero);
                        return resultado;
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
                string[] lines = Regex.Split(ex.Message, "\r\n");
                resultado.Add(ex.ParamName, lines[0]);
            }


            return resultado;
        }
        public async Task<IDictionary<string, string>> CancelaArquivoRemessaByID(int codCarquivo, int codUsuario)
        {

            var resultado = new Dictionary<string, string>();

            try
            {
                if (codCarquivo == 0)
                {
                    resultado.Add("Error", "Código do Arquivo Inválido");
                    return resultado;
                }
                var arq = _arquivo.ReturnFileById(codCarquivo).Result;


                arq.STATUS = "CANCELADO";

                _arquivo.AtualizaArquivo(arq);

                var listaPagamentos = _pagamento.GetPagamentosByCodArquivo(arq.CODARQUIVO).ToList();
                var listaBorderos = listaPagamentos.Select(x => x.CODBORDERO).Distinct();

                foreach (var pag in listaPagamentos)
                {
                    var result = await _pagamento.AtualizaStatus("CANCELADO", pag.CODTITULOPAGO);
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


                    _emissaoTitulos.IncluiLogByBordero(codBordero, "EM_BORDERO", codUsuario, "Arquivo de Remessa Cancelado");
                    //cancela taxa de TED
                    var Listatitulos = await _emissaoTitulos.BuscaTitulosByBordero(codBordero);
                    foreach (var tt in Listatitulos)
                    {
                        var listaTaxa = await _taxaTitulo.buscaTodasTaxasbyCodTitulo(tt.CODTITULO);
                        var taxaTED = listaTaxa.Where(x => x.DESCRICAO == "TAXA DE TRANSFERÊNCIA").FirstOrDefault();
                        if (taxaTED != null)
                        {
                            tt.VALTOTAL = tt.VALTOTAL + Math.Abs(taxaTED.VALOR);
                            _emissaoTitulos.AtualizaTitulo(tt);

                            _taxaTitulo.RemoveTaxa(taxaTED.CODTAXATITULO);
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
        public async Task<IDictionary<string, string>> AtualizaStatusArquivoByID(int codCarquivo, string status)
        {

            var resultado = new Dictionary<string, string>();

            try
            {
                if (codCarquivo == null || codCarquivo == 0)
                {
                    resultado.Add("Error", "Código do Arquivo Inválido");
                    return resultado;
                }
                var arq = _arquivo.ReturnFileById((int)codCarquivo).Result;


                arq.STATUS = status;
                _arquivo.AtualizaArquivo(arq);

                var listaPagamentos = _pagamento.GetPagamentosByCodArquivo(arq.CODARQUIVO).ToList();

                foreach (var pag in listaPagamentos)
                {
                    var result = await _pagamento.AtualizaStatus(status, pag.CODTITULOPAGO);
                    if (result)
                    {
                        _bordero.AtualizaStatusBordero(pag.CODBORDERO, "EM_BORDERO");
                        _emissaoTitulos.AtualizaStatusTituloBycodBordero(pag.CODBORDERO, "EM_BORDERO");
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
        public async Task<IDictionary<string, string>> AtualizaStatusArquivoByName(string nmArquivo, string status)
        {
            var resultado = new Dictionary<string, string>();

            try
            {
                var arq = _arquivo.ReturnFileByName(nmArquivo).Result;

                //Só irei atualizar o status 'Aguardando arquivo de retorno' se o status do arquivo for aguardando download.
                if (status == "AGUARDANDO_ARQUIVO_RETORNO")
                {
                    if (arq.STATUS == "AGUARDANDO_DOWNLOAD")
                    {
                        arq.STATUS = status;
                        _arquivo.AtualizaArquivo(arq);
                    }
                }
                else
                {
                    arq.STATUS = status;
                    _arquivo.AtualizaArquivo(arq);
                }

                var listaPagamentos = _pagamento.GetPagamentosByCodArquivo(arq.CODARQUIVO).ToList();

                foreach (var pag in listaPagamentos)
                {
                    var result = _pagamento.AtualizaStatus(status, pag.CODTITULOPAGO);
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
        public async Task<List<ListaTitulosPagVM>> ConsultaTransacaoPagamento(ConsultaFechamentoCredVm model)
        {
            try
            {
                List<ListaTitulosPagVM> retorno = new List<ListaTitulosPagVM>();

                DateTime dtInicio = Convert.ToDateTime(model.dtInicio);
                DateTime dtFim = Convert.ToDateTime(model.dtFim + " 23:59:59");
                int codEmpresa = Convert.ToInt32(model.codEmpresa);
                int codigoTitulo = Convert.ToInt32(model.CodigoTitulo);

                var listaCredenciado = await _proc.ConsultaTitulos(dtInicio, dtFim, model.codFavorecido, codEmpresa, model.codStatus, codigoTitulo);

                retorno = listaCredenciado
                    .Select(x => new ListaTitulosPagVM(x)).ToList();


                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> PagamentoManualAsync(FiltroBorderoPagVM model)
        {
            try
            {
                string Retorno = "Baixa Registrado com Sucesso.";

                foreach (var Lista in model.ListaFechamento)
                {
                    var ListaTitulos = await _emissaoTitulos.BuscaTitulosByFavorecidoDatPGTO(Convert.ToInt32(model.codEmpresa), Lista.CODFAVORECIDO, Convert.ToDateTime(Lista.DATPGTO));
                    foreach (var titulo in ListaTitulos)
                    {
                        var Titulo = _emissaoTitulos.GetById(titulo.CODTITULO);
                        Titulo.STATUS = "BAIXADO_MANUALMENTE";
                        Titulo.CODCONTACORRENTE = model.codigoContaCorrente;

                        _emissaoTitulos.AtualizaTitulo(Titulo);

                        if (model.codJustificativa != "OUTROS")
                        {
                            model.DescJustOutros = model.descJustificativa;
                        }

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Baixa manual com o motivo: " + model.DescJustOutros);

                        //ATUALIZA SALDO DA CONTACA CORRENTE
                        AtualizaSaldoContaCorrente(model.codigoContaCorrente, titulo.CODTITULO);
                    }
                }


                return Retorno;
            }
            catch (Exception ex)
            {
                return "Ocorreu uma falha durante o processo. favor contactar o suporte técnico";
            }
        }
        public async Task<string> BaixaManualByID(FiltroBorderoPagVM model)
        {
            try
            {
                string Retorno = "Baixa Registrado com Sucesso.";

                foreach (var tt in model.ListaFechamento)
                {
                    var Titulo = await _emissaoTitulos.BuscaTituloByID(Convert.ToInt32(tt.CODTITULO));

                    Titulo.STATUS = "BAIXADO_MANUALMENTE";
                    Titulo.CODCONTACORRENTE = model.codigoContaCorrente;

                    _emissaoTitulos.AtualizaTitulo(Titulo);

                    if (model.codJustificativa != "OUTROS")
                    {
                        model.DescJustOutros = model.descJustificativa;
                    }

                    //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                    _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Baixa manual com o motivo: " + model.DescJustOutros);

                    //ATUALIZA SALDO DA CONTACA CORRENTE
                    AtualizaSaldoContaCorrente(model.codigoContaCorrente, Convert.ToInt32(tt.CODTITULO));

                }


                return Retorno;
            }
            catch (Exception ex)
            {
                return "Ocorreu uma falha durante o processo. favor contactar o suporte técnico";
            }
        }
        public async Task<string> SalvaBordero(FiltroBorderoPagVM model)
        {
            try
            {

                string msgRetorno = "Borderô gerado com sucesso.";
                var vlBordero = model.ListaFechamento.Sum(X => Convert.ToDecimal(X.VALLIQ.Replace("R$", "").Replace(".", "")));
                int CodCamaraCentralizadora = 0;

                PAGNET_BORDERO_PAGAMENTO bordero = new PAGNET_BORDERO_PAGAMENTO();
                bordero.STATUS = "EM_BORDERO";
                bordero.CODUSUARIO = model.codUsuario;
                bordero.VLBORDERO = vlBordero;
                bordero.DTBORDERO = DateTime.Now;
                bordero.CODEMPRESA = Convert.ToInt32(model.codEmpresa);
                bordero.CODCONTACORRENTE = model.codigoContaCorrente;

                bordero = _bordero.InseriBordero(bordero);

                foreach (var lista in model.ListaFechamento)
                {
                    if (string.IsNullOrWhiteSpace(lista.LINHADIGITAVEL))
                    {
                        //titulos que serão pagos via ted ou doc
                        var ListaTitulos = await _emissaoTitulos.BuscaTitulosByFavorecidoDatPGTO(Convert.ToInt32(model.codEmpresa), lista.CODFAVORECIDO, Convert.ToDateTime(lista.DATPGTO));
                        foreach (var Titulo in ListaTitulos)
                        {
                            //var Titulo = _emissaoTitulos.GetById(titulo.CODTITULO);

                            Titulo.STATUS = "EM_BORDERO";
                            Titulo.CODBORDERO = bordero.CODBORDERO;

                            _emissaoTitulos.AtualizaTitulo(Titulo);

                            //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                            _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Inclusão de título em um borderô");
                        }
                    }
                    else
                    {
                        //titulos de boletos bancários
                        var boleto = await _emissaoTitulos.BuscaTitulosByLinhaDigitavel(lista.LINHADIGITAVEL);

                        //var Titulo = _emissaoTitulos.GetById(titulo.CODTITULO);

                        boleto.STATUS = "EM_BORDERO";
                        boleto.CODBORDERO = bordero.CODBORDERO;

                        _emissaoTitulos.AtualizaTitulo(boleto);

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(boleto, model.codUsuario, "Inclusão de título em um borderô");


                    }


                }

                return msgRetorno + " O número dele é: " + bordero.CODBORDERO;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public FiltroConsultaBorderoPagVM RetornaDadosInicioConstulaBordero(string dados, int CodEmpresa)
        {
            FiltroConsultaBorderoPagVM model = new FiltroConsultaBorderoPagVM();

            var empresa = _cadEmpresa.ConsultaEmpresaById(CodEmpresa).Result;
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;

            return model;
        }
        public BorderoPagVM ConsultaBordero(FiltroConsultaBorderoPagVM model)
        {
            try
            {
                string status = "EM_BORDERO";
                int codBordero = 0;

                if (!string.IsNullOrWhiteSpace(model.codStatus) && model.codStatus != "-1")
                {
                    status = model.codStatus;
                }

                if (!string.IsNullOrWhiteSpace(model.codBordero))
                {
                    codBordero = Convert.ToInt32(model.codBordero);
                }


                BorderoPagVM bordero;

                int codEmpresa = Convert.ToInt32(model.codEmpresa);

                List<PROC_PAGNET_CONS_BORDERO> ListaBordero = new List<PROC_PAGNET_CONS_BORDERO>();

                int codcontacorrente = Convert.ToInt32(model.codContaCorrente);

                ListaBordero = _bordero.Proc_ConsultaBorderoPagamento(codEmpresa, codBordero, codcontacorrente, status).Result;


                bordero = BorderoPagVM.ToView(ListaBordero);


                return bordero;
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public async Task<List<ConstulaPagNetFechCredVM>> GetAllPagamentosBordero(int CodBordero)
        {
            try
            {
                List<ConstulaPagNetFechCredVM> retorno = new List<ConstulaPagNetFechCredVM>();

                var titulosBordero = await _emissaoTitulos.BuscaTitulosByBordero(CodBordero);


                retorno = ConstulaPagNetFechCredVM.ToViewListaTitulosBordero(titulosBordero).ToList();

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

                var ListaTitulos = _emissaoTitulos.BuscaTitulosByBordero(codBordero).Result;
                foreach (var item in ListaTitulos)
                {
                    item.STATUS = "EM_ABERTO";
                    item.SEUNUMERO = "";
                    item.CODBORDERO = null;

                    _emissaoTitulos.AtualizaTitulo(item);
                    _emissaoTitulos.IncluiLog(item, codUsuario, "Borderô cancelado");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ConstulaPagNetFechCredVM>> GetAllPagamentosArquivo(int CodArquivo, string dtArquivo)
        {
            try
            {
                List<ConstulaPagNetFechCredVM> ListaPag = new List<ConstulaPagNetFechCredVM>();

                if (dtArquivo.Length < 8) dtArquivo = "0" + dtArquivo;
                dtArquivo = dtArquivo.Substring(0, 2) + "/" + dtArquivo.Substring(2, 2) + "/" + dtArquivo.Substring(4, 4);

                var dtEmissao = dtArquivo;

                var pag = _pagamento.GetPagamentosByCodArquivo(CodArquivo);

                ListaPag = ConstulaPagNetFechCredVM.ToViewPagNetPagamento(pag, dtEmissao).ToList();

                for (int i = 0; i < ListaPag.Count; i++)
                {
                    int codFavorecido = ListaPag[i].CODFAVORECIDO;
                    var favorecido = _cadFavorito.BuscaFavorecidosByID(codFavorecido).Result;

                    ListaPag[i].NMFAVORECIDO = Geral.FormataTexto(favorecido.NMFAVORECIDO, 35);
                    ListaPag[i].CNPJ = Geral.FormataCPFCnPj(favorecido.CPFCNPJ);
                }

                return ListaPag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DadosTituloVm> RetornaDadosTitulo(int codFechCred)
        {
            try
            {
                DadosTituloVm model = new DadosTituloVm();

                var dadosFechCred = await _emissaoTitulos.BuscaTituloByID(codFechCred);

                model = DadosTituloVm.ToView(dadosFechCred);

                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<string, string>> SalvaEdicaoTitulo(DadosTituloVm model)
        {
            var resultado = new Dictionary<string, string>();
            try
            {

                var Titulo = await _emissaoTitulos.BuscaTituloByID(model.CODIGOTITULO);

                if (model.codJustificativa != "OUTROS")
                {
                    model.DescJustOutros = model.descJustificativa;
                }

                Titulo.DATREALPGTO = Convert.ToDateTime(model.DATREALPGTO);
                Titulo.CODPLANOCONTAS = Convert.ToInt32(model.CodigoPlanoContas);

                _emissaoTitulos.AtualizaTitulo(Titulo);
                _emissaoTitulos.IncluiLog(Titulo, model.CODUSUARIO, "Edição de título com a justificativa: " + model.DescJustOutros);

                resultado.Add("sucesso", "Título alterado com sucesso!");

                return resultado;
                
            }
            catch (ArgumentException ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                resultado.Add(ex.ParamName, lines[0]);
            }
            return resultado;
        }
        private void InseriTaxaPagamento(int codTitulo, string Desc, decimal valor, bool ValPositivo, int codUsuario)
        {
            try
            {
                bool possuiTaxa = false;

                if (!ValPositivo)
                {
                    decimal valoraux = valor * 2;
                    valor = valor - valoraux;
                }

                var taxas = _taxaTitulo.buscaTodasTaxasbyCodTitulo(codTitulo).Result;

                foreach (var tx in taxas)
                {
                    if (tx.DESCRICAO == Desc)
                    {
                        possuiTaxa = true;
                        tx.VALOR = valor;
                        _taxaTitulo.AtualizaTaxa(tx);
                        break;
                    }
                }

                if (!possuiTaxa)
                {
                    PAGNET_TAXAS_TITULOS tx = new PAGNET_TAXAS_TITULOS();
                    tx.CODTITULO = codTitulo;
                    tx.DESCRICAO = Desc;
                    tx.VALOR = valor;
                    tx.ORIGEM = "PN";
                    tx.DTINCLUSAO = DateTime.Now;
                    tx.CODUSUARIO = codUsuario;

                    _taxaTitulo.IncluiTaxa(tx);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public GridTitulosVM GetAllTitulosVencidos(FiltroTitulosPagamentoVM vm)
        {
            try
            {
                GridTitulosVM Titulos = new GridTitulosVM();
                Titulos.dtTransferencia = DateTime.Now.ToShortDateString();

                int codEmpresa = Convert.ToInt32(vm.codEmpresa);
                DateTime dtInicio = Convert.ToDateTime(vm.dtInicio);
                DateTime dtFim = Convert.ToDateTime(vm.dtFim);

                var ListaTitulos = _proc.RetornaTitulosVencidos(codEmpresa, dtInicio, dtFim);

                Titulos = GridTitulosVM.ToViewTitulosVencidos(ListaTitulos);

                return Titulos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<string, string>> AlteraDataPGTOEmMassa(GridTitulosVM model)
        {

            var resultado = new Dictionary<string, string>();
            DateTime novaDataPGTO = Convert.ToDateTime(model.dtTransferencia);


            if (string.IsNullOrWhiteSpace(model.JustificativaOutros))
            {
                model.JustificativaOutros = model.Justificativa;
            }

            try
            {
                foreach (var linha in model.ListaFechamento)
                {
                    var Titulo = _emissaoTitulos.GetById(linha.CODTITULO);
                    Titulo.DATREALPGTO = novaDataPGTO;

                    _emissaoTitulos.Update(Titulo);

                    //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                    _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Alteração da data de Pagamento com a justificativa:" + model.JustificativaOutros);


                }

                resultado.Add("Sucesso", "Títulos atualizados com sucesso");

            }
            catch (ArgumentException ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                resultado.Add(ex.ParamName, lines[0]);
            }


            return resultado;
        }
        public async Task<List<LogTituloVM>> ConsultaLog(int CODTITULO)
        {
            List<LogTituloVM> Retorno = new List<LogTituloVM>();

            var dadosLog = await _emissaoTitulos.BuscaLog(CODTITULO);

            Retorno = dadosLog.Select(x => new LogTituloVM(x)).ToList();

            return Retorno;
        }
        public async Task<Dictionary<string, decimal>> ValorpagoPorCredenciado(IndicaroresVM model)
        {
            throw new NotImplementedException();
        }
        public async Task<IDictionary<bool, string>> IncluirNovoTituloPGTO(EmissaoTituloAvulsto model)
        {
            bool tudoCerto = false;

            var resultado = new Dictionary<bool, string>();

            PAGNET_EMISSAO_TITULOS Titulos;


            if (model.CODTITULO == 0)
            {
                Titulos = new PAGNET_EMISSAO_TITULOS();
            }
            else
            {
                Titulos = await _emissaoTitulos.BuscaTituloByID(model.CODTITULO);
            }

            Titulos.STATUS = "EM_ABERTO";
            Titulos.CODEMPRESA = Convert.ToInt32(model.CODEMPRESA);
            Titulos.CODFAVORECIDO = Convert.ToInt32(model.CODFORNECEDOR);
            Titulos.DATEMISSAO = DateTime.Now;

            if (model.TIPOTITULO == "BOLETO")
            {
                Titulos.DATPGTO = Convert.ToDateTime(model.DATVENCIMENTOBOLETO);
                Titulos.DATREALPGTO = Convert.ToDateTime(model.DATREALPGTOBOLETO);
                Titulos.VALBRUTO = Convert.ToDecimal(model.VALORBOLETO.Replace("R$", "").Replace(".", ""));
                Titulos.VALLIQ = Convert.ToDecimal(model.VALORBOLETO.Replace("R$", "").Replace(".", ""));
                Titulos.VALTOTAL = Convert.ToDecimal(model.VALORBOLETO.Replace("R$", "").Replace(".", ""));
            }
            else
            {
                Titulos.DATPGTO = Convert.ToDateTime(model.DATREALPGTO);
                Titulos.DATREALPGTO = Convert.ToDateTime(model.DATREALPGTO);
                Titulos.VALBRUTO = Convert.ToDecimal(model.VALOR.Replace("R$", "").Replace(".", ""));
                Titulos.VALLIQ = Convert.ToDecimal(model.VALOR.Replace("R$", "").Replace(".", ""));
                Titulos.VALTOTAL = Convert.ToDecimal(model.VALOR.Replace("R$", "").Replace(".", ""));
            }

            Titulos.TIPOTITULO = model.TIPOTITULO;
            Titulos.ORIGEM = "PN";
            Titulos.SISTEMA = 0;
            Titulos.LINHADIGITAVEL = model.LINHADIGITAVEL;
            Titulos.CODBORDERO = null;
            Titulos.SEUNUMERO = "";
            Titulos.CODPLANOCONTAS = Convert.ToInt32(model.CodigoPlanoContas);

            try
            {
                if (model.CODTITULO == 0)
                {
                    _emissaoTitulos.IncluiTitulo(Titulos);
                    _emissaoTitulos.IncluiLog(Titulos, model.codUsuario, "Título incluído manualmente via PagNet");
                    resultado.Add(true, "Título incluído com sucesso");
                }
                else
                {
                    _emissaoTitulos.AtualizaTitulo(Titulos);
                    _emissaoTitulos.IncluiLog(Titulos, model.codUsuario, "Título atualizado manualmente via PagNet");
                    resultado.Add(true, "Título atualizado com sucesso");
                }

            }
            catch (Exception ex)
            {
                resultado.Add(false, ex.Message);
            }

            return resultado;
        }
        public async Task<List<ListaTitulosPGTOVM>> ConsultaTitulosByFavorecidoDatPGTO(int codEmpresa, int codFavorecido, DateTime datPGTO)
        {
            try
            {
                List<ListaTitulosPGTOVM> ListaTitulos;

                var Dados = await _emissaoTitulos.BuscaTitulosByFavorecidoDatPGTO(codEmpresa, codFavorecido, datPGTO);
                ListaTitulos = Dados.Select(x => new ListaTitulosPGTOVM(x)).ToList();

                return ListaTitulos;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<ListaEmissaoTitulosVM>> ListaTitulosNaoLiquidado(int codEmpresa)
        {
            try
            {
                List<ListaEmissaoTitulosVM> ListaTitulos;

                var Dados = await _emissaoTitulos.ListaTitulosNaoLiquidado(codEmpresa);
                ListaTitulos = Dados.Select(x => new ListaEmissaoTitulosVM(x)).ToList();

                return ListaTitulos;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<TaxasCobradasPGTOVm>> ListarTaxasTitulo(int codTitulo)
        {
            try
            {
                var taxas = await _taxaTitulo.buscaTodasTaxasbyCodTitulo(codTitulo);

                var ListaTaxas = TaxasCobradasPGTOVm.ToListView(taxas).ToList();
                var ValorTotal = ListaTaxas.Select(x => x.ValorTotal_aux).Sum();

                for (int i = 0; i < ListaTaxas.Count; i++)
                {
                    ListaTaxas[i].ValorTotal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Abs(ValorTotal));
                }

                return ListaTaxas.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IDictionary<bool, string>> CancelarTitulo(FiltroBorderoPagVM model)
        {
            var resultado = new Dictionary<bool, string>();


            if (string.IsNullOrWhiteSpace(model.DescJustOutros))
            {
                model.DescJustOutros = model.descJustificativa;
            }

            try
            {
                foreach (var linha in model.ListaFechamento)
                {
                    var Titulo = _emissaoTitulos.GetById(linha.CODTITULO);

                    if (Titulo.SEUNUMERO != null)
                    {
                        var tt = await _pagamento.GetTransacaoBySeuNumero(Titulo.SEUNUMERO);
                        if (tt != null)
                        {
                            var result = _pagamento.AtualizaStatus("CANCELADO", tt.CODTITULOPAGO);
                        }
                    }
                    Titulo.STATUS = "CANCELADO";
                    Titulo.SEUNUMERO = null;
                    Titulo.CODBORDERO = null;

                    _emissaoTitulos.Update(Titulo);

                    //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                    _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Título cancelado com a justificativa:" + model.DescJustOutros);


                }

                resultado.Add(true, "Títulos atualizados com sucesso");

            }
            catch (ArgumentException ex)
            {
                throw ex;
            }

            return resultado;
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
        public async Task<string> DesvinculaTitulo(FiltroBorderoPagVM model)
        {
            try
            {
                string Retorno = "Título desvinculado do arquivo de remessa com sucesso.";

                foreach (var tt in model.ListaFechamento)
                {
                    var Titulo = await _emissaoTitulos.BuscaTituloByID(Convert.ToInt32(tt.CODTITULO));

                    var listaTitulos = await _emissaoTitulos.BuscaTituloBySeuNumero(Titulo.SEUNUMERO);

                    var TituloPago = await _pagamento.GetTransacaoBySeuNumero(Titulo.SEUNUMERO);
                    TituloPago.STATUS = "CANCELADO";
                    var ret = _pagamento.AtualizaTransacao(TituloPago);

                    foreach (var tit in listaTitulos)
                    {
                        tit.STATUS = "EM_ABERTO";
                        tit.SEUNUMERO = "";
                        tit.CODBORDERO = null;
                        tit.TIPOTITULO = "TEDDOC";

                        //REMOVE A TAXA DE TRANSFERÊNCIA
                        var listaTaxa = await _taxaTitulo.buscaTodasTaxasbyCodTitulo(tit.CODTITULO);
                        var taxaTED = listaTaxa.Where(x => x.DESCRICAO == "TAXA DE TRANSFERÊNCIA").FirstOrDefault();
                        if (taxaTED != null)
                        {
                            tit.VALTOTAL = tit.VALTOTAL + Math.Abs(taxaTED.VALOR);
                            _taxaTitulo.RemoveTaxa(taxaTED.CODTAXATITULO);
                        }

                        _emissaoTitulos.AtualizaTitulo(tit);

                        if (model.codJustificativa != "OUTROS")
                        {
                            model.DescJustOutros = model.descJustificativa;
                        }

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(tit, model.codUsuario, "Título desvinculado do arquivo de remessa com a justificativa: " + model.DescJustOutros);

                    }


                }

                return Retorno;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Ocorreu uma falha durante o processo.favor contactar o suporte técnico", "error");
            }
        }
        public async Task<List<ListaTitulosInclusaoMassa>> CarregaArquivoTituloMassaNC(string CaminhoArquivo, int codEmpresa, int CodUsuario)
        {
            try
            {
                List<ListaTitulosInclusaoMassa> ListaUsuarios = new List<ListaTitulosInclusaoMassa>();
                ListaTitulosInclusaoMassa usuario;
                string[] lines = File.ReadAllLines(CaminhoArquivo);

                foreach (string line in lines)
                {
                    var Colunas = line.Split(';');

                    if (Colunas[0].IndexOf("USUARIO DE CARTAO") > -1)
                    {
                        continue;
                    }

                    var Sistema = (Colunas[0] == "S") ? 0 : 1;
                    var CPF = Geral.RemoveCaracteres(Colunas[1]);

                    _proc.IncluiUsuarioNC(CPF, Sistema, codEmpresa);
                    var DadosFavorecido = await _cadFavorito.BuscaFavorecidosByCNPJ(CPF);

                    usuario = new ListaTitulosInclusaoMassa();
                    usuario.codFavorecidoNC = DadosFavorecido.CODFAVORECIDO.ToString();
                    usuario.CPFUsuarioNC = Geral.FormataCPFCnPj(DadosFavorecido.CPFCNPJ);
                    usuario.NomeUsuarioNC = DadosFavorecido.NMFAVORECIDO;
                    usuario.DataPGTO = Colunas[2].ToString();
                    usuario.vlLiquido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal((string.IsNullOrWhiteSpace(Colunas[3]) ? "0" : Colunas[3])));
                    usuario.StatusProcessamento = "";
                    usuario.LogProcessamento = "";

                    ListaUsuarios.Add(usuario);
                }

                return ListaUsuarios;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ListaTitulosInclusaoMassa>> CarregaArquivoTituloMassa(string CaminhoArquivo, int codEmpresa, int CodUsuario)
        {
            try
            {
                List<ListaTitulosInclusaoMassa> ListaUsuarios = new List<ListaTitulosInclusaoMassa>();
                ListaTitulosInclusaoMassa usuario;
                string codbanco = "";
                string[] lines = File.ReadAllLines(CaminhoArquivo);

                foreach (string line in lines)
                {
                    var Colunas = line.Split(';');

                    if (Colunas[0].IndexOf("CPF") > -1)
                    {
                        continue;
                    }

                    var CPF = Geral.RemoveCaracteres(Colunas[0]);

                    var DadosFavorecido = await _cadFavorito.BuscaFavorecidosByCNPJ(CPF);
                    if (DadosFavorecido == null)
                    {
                        codbanco = "";
                        codbanco = Colunas[11];
                        while (codbanco.Length < 3)
                        {
                            codbanco = "0" + codbanco;
                        }
                        DadosFavorecido = new PAGNET_CADFAVORECIDO();
                        DadosFavorecido.NMFAVORECIDO = Colunas[1];
                        DadosFavorecido.CPFCNPJ = Geral.RemoveCaracteres(Colunas[0]);
                        DadosFavorecido.CODCEN = null;
                        DadosFavorecido.BANCO = codbanco;
                        DadosFavorecido.AGENCIA = Colunas[12];
                        DadosFavorecido.DVAGENCIA = Colunas[13];
                        DadosFavorecido.OPE = Colunas[14];
                        DadosFavorecido.CONTACORRENTE = Colunas[15];
                        DadosFavorecido.DVCONTACORRENTE = Colunas[16];
                        DadosFavorecido.CEP = Colunas[4];
                        DadosFavorecido.LOGRADOURO = Colunas[5];
                        DadosFavorecido.NROLOGRADOURO = Colunas[6];
                        DadosFavorecido.COMPLEMENTO = Colunas[7];
                        DadosFavorecido.BAIRRO = Colunas[8];
                        DadosFavorecido.CIDADE = Colunas[9];
                        DadosFavorecido.UF = Colunas[10];
                        DadosFavorecido.ATIVO = "S";
                        DadosFavorecido.CODEMPRESA = codEmpresa;

                        _cadFavorito.IncluiFavorito(DadosFavorecido);
                        _cadFavorito.InsertLog(DadosFavorecido, CodUsuario, "Favorecido incluido via arquivo de inclusão em massa");
                    }

                    usuario = new ListaTitulosInclusaoMassa();
                    usuario.codFavorecidoNC = DadosFavorecido.CODFAVORECIDO.ToString();
                    usuario.CPFUsuarioNC = Geral.FormataCPFCnPj(DadosFavorecido.CPFCNPJ);
                    usuario.NomeUsuarioNC = DadosFavorecido.NMFAVORECIDO;
                    usuario.DataPGTO = Colunas[2].ToString();
                    usuario.vlLiquido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal((string.IsNullOrWhiteSpace(Colunas[3]) ? "0" : Colunas[3])));
                    usuario.StatusProcessamento = "";
                    usuario.LogProcessamento = "";

                    ListaUsuarios.Add(usuario);
                }

                return ListaUsuarios;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object[][] DDLListaTitulosPendentes(int codFavorecido)
        {
            var lista = _emissaoTitulos.DDLTitulosAbertosByCodFavorecido(codFavorecido).ToList();
            lista.Insert(0, new object[] { "0", " " });

            return lista.ToArray();
        }

        public async Task<IDictionary<bool, string>> SalvarAjusteValorTitulo(AjustarValorTitulo model)
        {
            var resultado = new Dictionary<bool, string>();
            try
            {

                var Titulo = await _emissaoTitulos.BuscaTituloByID(model.codigoTituloAjusteValor);
                var valorInformado = Convert.ToDecimal(model.valorConcedido);
                var justificativa = "";

                if (model.Desconto)
                {
                    justificativa = "Desconto de R$ " + valorInformado + " via sistema PagNet com a justificativa: " + model.Descricao;
                    valorInformado = -1 * valorInformado;
                }
                else
                {
                    justificativa = "Acréscimo de R$" + valorInformado + " via sistema PagNet com a justificativa: " + model.Descricao;
                }

                Titulo.VALTOTAL = Titulo.VALTOTAL + valorInformado;

                _emissaoTitulos.AtualizaTitulo(Titulo);
                _emissaoTitulos.IncluiLog(Titulo, model.codigoUsuarioAjusteValor, justificativa);

                PAGNET_TAXAS_TITULOS tx = new PAGNET_TAXAS_TITULOS();
                tx.CODTITULO = model.codigoTituloAjusteValor;
                tx.DESCRICAO = model.Descricao;
                tx.VALOR = valorInformado;
                tx.ORIGEM = "PN";
                tx.DTINCLUSAO = DateTime.Now;
                tx.CODUSUARIO = model.codigoUsuarioAjusteValor;

                _taxaTitulo.IncluiTaxa(tx);


                resultado.Add(true, "Título alterado com sucesso!");

                return resultado;

            }
            catch (ArgumentException ex)
            {
                string[] lines = Regex.Split(ex.Message, "\r\n");
                resultado.Add(false, lines[0]);
            }
            return resultado;
        }
    }
}