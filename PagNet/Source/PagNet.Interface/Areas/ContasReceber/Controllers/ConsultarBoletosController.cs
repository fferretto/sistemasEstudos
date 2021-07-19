using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;
using FluentDateTime;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Api.Service.Interface;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class ConsultarBoletosController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly ICadastrosApp _cadastro;
        private readonly IConfiguracaoApp _cofig;
        private readonly IDiversosApp _diversos;
        private readonly IContaEmailApp _email;
        private readonly IRelatorioApp _relatorio;
        private readonly IAPIClienteAPP _APICliente;

        public ConsultarBoletosController(IRecebimentoApp recebimento,
                                           ICadastrosApp cadastro,
                                           IConfiguracaoApp cofig,
                                           IContaEmailApp email,
                                           IRelatorioApp relatorio,
                                           IDiversosApp diversos,
                                           IAPIClienteAPP APICliente,
                                           IPagNetUser user)
        {
            _recebimento = recebimento;
            _user = user;
            _cadastro = cadastro;
            _cofig = cofig;
            _diversos = diversos;
            _email = email;
            _relatorio = relatorio;
            _APICliente = APICliente;
        }
        public async Task<IActionResult> Index(string id)
        {
            var model = _recebimento.RetornaDadosInicio(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;
            model.codStatus = "TODOS";
            model.nmStatus = "TODOS";

            return View(model);
        }
        public async Task<ActionResult> CarregaListaBoletos(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _recebimento.CarregaListaBoletos(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }
        public async Task<ActionResult> VisualizarLog(int CodFatura)
        {
            try
            {
                var Dados = await _recebimento.VisualizarLog(CodFatura);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }
        }
        public async Task<ActionResult> EditarPedidoFaturamento(int CodFatura, string codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                    codEmpresa = _user.cod_empresa.ToString();

                var model = await _recebimento.DadosInicioEmissaoBoletoAvulso(CodFatura, Convert.ToInt32(codEmpresa));

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }
        public async Task<ActionResult> LiquidarManualmente(int CodFatura, string codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                    codEmpresa = _user.cod_empresa.ToString();

                var model = await _recebimento.DadosInicioEmissaoBoletoAvulso(CodFatura, Convert.ToInt32(codEmpresa));

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }
        public async Task<ActionResult> RealizaAcoes(int CodFatura, string status)
        {
            try
            {
                var model = await _recebimento.DadosInicioEmissaoBoletoAvulso(CodFatura, _user.cod_empresa);

                return PartialView(model);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }

        public JsonResult AtualizaSolicitacaoCarga()
        {
            try
            {
                _recebimento.AtualizaSolicitacaoCarga();

                return Json(new { success = true, responseText = "Processo Realizado com Sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

        public JsonResult CarregaListaAcoes()
        {

            var lista = new object[][]
             {
                new object[] { "RealizarAcao", "EDITARFATURAMENTO", "Editar"},
                new object[] { "RealizarAcao", "DETALHAMENTOCOBRANCA", "Detalhamento Cobrança" },
                new object[] { "RealizarAcao", "PARCELARFATURA", "Parcelar Fatura" },
                new object[] { "RealizarAcao", "LIQUIDAMANUALMENTE", "Liquidar Manualmente"},
                new object[] { "RealizarAcao", "EXCLUIRFATURAMENTO", "Cancelar Pedido de Faturamento" }
             };

            return Json(lista.ToList());
        }
        public ActionResult JustificarCancelamentoPedidoFaturamento()
        {
            EmissaoBoletoVM vm = new EmissaoBoletoVM();

            return PartialView(vm);

        }
        public JsonResult BuscaJustificativas()
        {

            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "FATURADO_DEPOSITO", "FATURADO VIA DEPÓSITO"},
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public async Task<JsonResult> CarregaStatus()
        {

            var lista = new object[][]
             {
                new object[] { "TODOS", "TODOS" },
                new object[] { "A_FATURAR", "A FATURAR" },
                new object[] { "EM_ABERTO", "EM ABERTO" },
                new object[] { "EM_BORDERO", "EM BORDERO" },
                new object[] { "PENDENTE_REGISTRO", "PENDENTE REGISTRO" },
                new object[] { "REGISTRADO", "REGISTRADO" },
                new object[] { "LIQUIDADO", "LIQUIDADO" },
                new object[] { "LIQUIDADO_MANUALMENTE", "LIQUIDADO MANUALMENTE" },
                new object[] { "RECUSADO", "RECUSADO" }
             };


            return Json(lista.ToList());
        }
        public async Task<ActionResult> EnvioEmailEmMassa()
        {
            return PartialView();
        }
        public JsonResult BuscaCliente(string filtro, string codempresa)
        {
            APIClienteVm model = new APIClienteVm();

            if (Convert.ToInt32(codempresa) <= 0)
                codempresa = _user.cod_empresa.ToString();

            if (filtro.Length < 11)
            {
                model = APIClienteVm.ToView(_APICliente.RetornaDadosClienteByIDeCodEmpresa(Convert.ToInt32(filtro), Convert.ToInt32(codempresa)));
            }
            else
            {
                model = APIClienteVm.ToView(_APICliente.RetornaDadosClienteByCPFCNPJeCodEmpresa(filtro, Convert.ToInt32(codempresa)));
            }

            return Json(model);
        }
        [HttpPost]
        public async Task<IActionResult> SalvarAlteracao(EmissaoBoletoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                model.codigoUsuario = _user.cod_usu;

                var result = await _recebimento.IncluiNovoPedidoFaturamento(model);

                return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao alterar o faturamento. Favor contactar o suporte." });

            }
        }
        [HttpPost]
        public async Task<IActionResult> EnviarBoletoEmail(int codEmissaoBoleto)
        {
            try
            {
                var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                CaminhoPadrao = Path.Combine(CaminhoPadrao, "PDFBoleto");

                var retorno = await _email.EnviaEmailBoleto(codEmissaoBoleto, _user.cod_ope, _user.cod_usu, _user.cod_empresa);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> EnviarBoletoOutroEmail(int codEmissaoBoleto, string email)
        {
            try
            {
                var retorno = await _email.EnviarBoletoOutroEmail(codEmissaoBoleto, email, _user.cod_ope, _user.cod_usu, _user.cod_empresa);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }

        public async Task<FileResult> DownloadBoleto(string id)
        {
            var nmBoleto = await _recebimento.RetornaNomeBoletoByID(Convert.ToInt32(id));

            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

            var url = Path.Combine(CaminhoPadrao, "PDFBoleto", nmBoleto + ".pdf");

            if (!System.IO.File.Exists(url))
            {
                _recebimento.GeraBoletoPDF(Path.Combine(CaminhoPadrao, "PDFBoleto"), Convert.ToInt32(id), _user.cod_usu);
            }

            var path = Path.GetFullPath(url);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".rem", "text/rem"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        [HttpPost]
        public async Task<ActionResult> CancelarFaturamento(EmissaoBoletoVM model)
        {
            try
            {
                model.codigoUsuario = _user.cod_usu;

                var retorno = await _recebimento.CancelarFatura(model);
                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu uma falha durante o processo de cancelamento. Favor contactar o suporte!" });
            }
        }
        [HttpPost]
        public async Task<ActionResult> LiquidacaoManual(EmissaoBoletoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                model.codigoUsuario = _user.cod_usu;

                var result = await _recebimento.LiquidacaoManual(model);

                return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao liquidar o faturamento. Favor contactar o suporte." });

            }

        }
        [HttpPost]
        public async Task<IActionResult> EnviarDetalhamentoFaturaEmail(string nomeArquivo, int codEmissaoBoleto, string email)
        {
            try
            {
                var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                var caminhoArquivo = Path.Combine(CaminhoPadrao, "PDFBoleto");
                string caminho = Path.Combine(caminhoArquivo, nomeArquivo + ".pdf");

                var retorno = await _email.EnviaEmailDetalhamentoFatura(codEmissaoBoleto, caminho, email, _user.cod_usu);

                return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu uma falha durante o processo de envio do detalhamento da fatura. Favor contactar o suporte." });
            }
        }

        public async Task<FileResult> DownloadDetalhamentoFatura(string nomeArquivo)
        {

            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
            var caminhoArquivo = Path.Combine(CaminhoPadrao, "PDFBoleto");
            string url = Path.Combine(caminhoArquivo, nomeArquivo + ".pdf");

            var path = Path.GetFullPath(url);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        public IActionResult CriaDetalhamentoFaturaReembolso(string codEmissaoBoleto, string nomeArquivo)
        {
            try
            {
                var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                var caminhoArquivo = Path.Combine(CaminhoPadrao, "PDFBoleto");

                if (!Directory.Exists(caminhoArquivo))
                {
                    //Criamos um com o nome folder
                    Directory.CreateDirectory(caminhoArquivo);
                }

                string caminho = Path.Combine(caminhoArquivo, nomeArquivo + ".pdf");


                if (System.IO.File.Exists(caminho))
                {
                    System.IO.File.Delete(caminho);
                }

                var DadosCobranca = _recebimento.RetornaDadosDetalhamentoCobranca(Convert.ToInt32(codEmissaoBoleto)).Result;

                var pdf = new ViewAsPdf("CriaDetalhamentoFaturaReembolso", DadosCobranca)
                {
                    Model = DadosCobranca,
                    PageSize = Size.A4,
                    IsLowQuality = true,

                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(2, 6, 6, 5),
                    SaveOnServerPath = caminho // preecha caminho com o diretório a ser salvo
                };

                return pdf;
                //return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ActionResult> ParcelarFatura(int CodFatura, string codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                    codEmpresa = _user.cod_empresa.ToString();

                var model = await _recebimento.DadosInicioEmissaoBoletoAvulso(CodFatura, Convert.ToInt32(codEmpresa));

                ParcelamentoFaturaVm parcela = new ParcelamentoFaturaVm();
                parcela.Parcela_codFaturamento = Convert.ToInt32(model.codigoFormaFaturamento);
                parcela.Parcela_Cliente = model.CodigoCliente + " - " + model.nomeCliente;
                parcela.Parcela_TaxaMensal = "0,00";
                parcela.Parcela_codFaturamento = model.codigoEmissaoBoleto;
                parcela.Parcela_codUsuario = _user.cod_usu;
                parcela.Parcela_dtVencimento = model.dataVencimento;
                parcela.Parcela_dataPrimeiraParcela = DateTime.Now.ToShortDateString();
                parcela.Parcela_ValorOriginal = model.Valor;


                return PartialView(parcela);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }
        public async Task<ActionResult> CalculaParcelasFaturamentos(string valor, int qtParcela, string primeiraParcela, string taxaMes)
        {
            try
            {
                // Calculo de juros compostos usando o biblioteca Math
                // Formula: PMT = [PV. (1 + i) ^ n] i / [(1 + i) ^ n - 1]

                // Valor financiado
                double PV = Convert.ToDouble(valor);
                // Parcelas
                int n = qtParcela;
                // Taxa mensal
                double i = Convert.ToDouble(taxaMes) / 100;
                // Prestação a descobrir.
                double PMT = 0;
                double SaldoDevedor = 0;
                double JurosParcela = 0;
                double SaldoAnterior = 0;
                bool UltimoDiaMes = false;

                PMT = (PV * Math.Pow((1 + i), n) * i) / (Math.Pow((1 + i), n) - 1);

                List<ListaParcelasVm> ListaParcelas = new List<ListaParcelasVm>();
                int contador = 1;
                DateTime dtPrimeiraParcela = Convert.ToDateTime(primeiraParcela.Replace("_", "/"));
                //DateTime com o último dia do mês
                DateTime ultimoDiaDoMes = dtPrimeiraParcela.LastDayOfMonth();

                ListaParcelasVm p = new ListaParcelasVm();
                if (ultimoDiaDoMes.Day == dtPrimeiraParcela.Day)
                {
                    UltimoDiaMes = true;
                }

                while (contador <= qtParcela)
                {
                    p = new ListaParcelasVm();
                    SaldoAnterior = (SaldoDevedor == 0) ? PV : (SaldoDevedor);
                    JurosParcela = Math.Round((SaldoDevedor == 0) ? (PV * i) : (SaldoDevedor * i), 2);
                    SaldoDevedor = (SaldoDevedor == 0) ? ((PV * i) + PV) : ((SaldoDevedor * i) + SaldoDevedor);
                    p.NroParcela = contador.ToString();
                    p.ValorParcela = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Round(PMT, 2));
                    p.TotalAPagar = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (Math.Round(PMT, 2) * qtParcela));
                    p.TotalJuros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (Math.Round(PMT, 2) * qtParcela) - PV);
                    p.SaldoDevedor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoDevedor - Math.Round(PMT, 2));
                    p.Juros = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", JurosParcela);
                    p.ValorPago = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (Math.Round(PMT, 2) * contador));
                    p.SaldoAnterior = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoAnterior);
                    p.Amortizacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Round(PMT, 2) - JurosParcela);

                    if (contador == qtParcela)//fiz isso por causa dos arredondamentos, sempre sobra 0,01 centavos
                        p.SaldoDevedor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0);
                    else
                        p.SaldoDevedor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", SaldoDevedor - Math.Round(PMT, 2));


                    if (UltimoDiaMes)
                    {
                        p.VencimentoParcela = dtPrimeiraParcela.LastDayOfMonth().ToShortDateString();
                    }
                    else
                    {
                        p.VencimentoParcela = dtPrimeiraParcela.ToShortDateString();
                    }
                    ListaParcelas.Add(p);
                    contador++;
                    dtPrimeiraParcela = dtPrimeiraParcela.AddMonths(1);
                    SaldoDevedor = SaldoDevedor - Math.Round(PMT, 2);

                }

                return PartialView(ListaParcelas);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }
        [HttpPost]
        public async Task<ActionResult> SalvarParcelamentoFatura(ParcelamentoFaturaVm model)
        {
            try
            {
                if (Convert.ToInt32(model.Parcela_codEmpresa) <= 0)
                    model.Parcela_codEmpresa = _user.cod_empresa;

                model.Parcela_codUsuario = _user.cod_usu;

                var result = await _recebimento.SalvarParcelamentoFatura(model);

                return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Falha ao liquidar o faturamento. Favor contactar o suporte." });

            }

        }
    }
}