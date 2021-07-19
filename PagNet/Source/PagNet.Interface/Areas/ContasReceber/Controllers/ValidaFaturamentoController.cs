using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Model;
using PagNet.Application.Application.Common;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Interface.Areas.ContasReceber.Models;
using PagNet.Interface.Helpers;
using PagNet.Interface.Helpers.HelperModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class ValidaFaturamentoController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly IConfiguracaoApp _cofig;
        private readonly IDiversosApp _diversos;
        private readonly IAPIGestaoDescontoFolhaAppClient _apiDescontoFolha;
        private readonly IAPIClienteAPP _APICliente;

        public ValidaFaturamentoController(IRecebimentoApp recebimento,
                                           IConfiguracaoApp cofig,
                                           IDiversosApp diversos,
                                           IAPIGestaoDescontoFolhaAppClient apiDescontoFolha,
                                           IAPIClienteAPP APICliente,
                                           IPagNetUser user)
        {
            _recebimento = recebimento;
            _user = user;
            _cofig = cofig;
            _diversos = diversos;
            _apiDescontoFolha = apiDescontoFolha;
            _APICliente = APICliente;
        }
        public async Task<IActionResult> Index(string id)
        {
            var model = _recebimento.RetornaDadosInicio(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;
            model.UtilizaNetCard = _user.PossuiNetCard;

            return View(model);
        }

        public async Task<ActionResult> CarregaPedidosFaturamento(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                model.codStatus = "A_FATURAR";
                model.dtFim = DateTime.MaxValue.ToShortDateString();

                var Dados = await _recebimento.CarregaListaBoletos(model);

                DadosBoletoVM Pedidos = new DadosBoletoVM();
                foreach (var item in Dados)
                {
                    Pedidos.ListaBoletos.Add(item);
                }

                return PartialView(Pedidos);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ConsultaTodosClientes(string codempresa)
        {
            try
            {
                if (Convert.ToInt32(codempresa) <= 0)
                    codempresa = _user.cod_empresa.ToString();

                var dados = await _APICliente.ConsultaTodosCliente(Convert.ToInt32(codempresa), "");
                var ListaCliente = APIClienteVm.ToListView(dados).ToList();

                return PartialView(ListaCliente);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        public JsonResult BuscarCliente(string filtro, string codempresa)
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
        public async Task<ActionResult> VisualizarPedidoFaturamento(string codFaturamento, string codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                    codEmpresa = _user.cod_empresa.ToString();

                var dados = await _recebimento.DadosInicioEmissaoBoletoAvulso(Convert.ToInt32(codFaturamento), Convert.ToInt32(codEmpresa));

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        [HttpPost]
        public async Task<JsonResult> ValidarPedidosFaturamento(DadosBoletoVM vm)
        {
            try
            {
                var retorno = await _recebimento.ValidapedidosFaturamento(vm.ListaBoletos.ToList(), _user.cod_usu);

                return Json(new { success = true, responseText = retorno.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(new { success = false, responseText = "Ocorreu uma falha durante o processo. Favor contactar o suporte." });
            }

        }
        public JsonResult CarregaListaFaturas(int? id)
        {
            int codigoCliente = 0;

            if (Convert.ToInt32(id) > 0)
            {
                codigoCliente = (int)id;
            }
            var dados = _recebimento.CarregaListaFaturas(codigoCliente);

            return Json(dados);
        }
        [HttpPost]
        public async Task<ActionResult> UploadArquivoValidacaoRetorno(IFormFile file)
        {
            try
            {
                string filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                filename = this.EnsureCorrectFilename(filename);
                var extensao = Path.GetExtension(filename);

                //if (extensao.ToUpper() != ".CSV")
                //{
                //    return Json(new { success = false, responseText = "Extensão do arquivo inválido. Permitido apenas arquivos com extensão '.CSV'" });
                //}

                var NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff") + extensao;

                FileStream output;
                using (output = System.IO.File.Create(this.GetPathAndFilename(NewFileName)))
                    await file.CopyToAsync(output);

                string CaminhoArquivo = output.Name;

                return Json(new { success = true, responseText = CaminhoArquivo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
            var path = Path.Combine(CaminhoPadrao, "ArquivosVisualizados");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, filename);

            return path;
        }
        public IActionResult ConsolidaArquivoRetornoCliente(FiltroDescontoFolhaModel model)
        {
            try
            {
                var vm = RetornoValidaDescontoFolhaVM.ToView(_apiDescontoFolha.ConsolidaArquivoRetornoCliente(model));

                return PartialView(vm);
            }
            catch (PagNetException ex1)
            {
                TempData["Avis.Erro"] = ex1.Message;
                return PartialView(null);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
            }

        }
        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> ValidaFaturamentoViaArquivo(RetornoValidaDescontoFolhaVM model)
        {
            try
            {
                APIDFUsuarioNaoDescontadoVM mdApi = new APIDFUsuarioNaoDescontadoVM();
                mdApi.codigoFatura = model.codigoFatura;
                mdApi.CodigoCliente = model.codigoCliente;

                Api.Service.Interface.Model.APIListaUsuariosArquivoRetornoVm mdTemp;
                foreach (var item in model.ListaUsuarios)
                {
                    mdTemp = new Api.Service.Interface.Model.APIListaUsuariosArquivoRetornoVm();
                    mdTemp.Acao = item.Acao;
                    mdTemp.CPF = Geral.RemoveCaracteres(item.CPF);

                    mdApi.ListaUsuarios.Add(mdTemp);
                }

                var result = _apiDescontoFolha.ExecutaProcessoDescontoFolha(mdApi);

                return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.InnerException.Message });
            }

        }
        

    }
}