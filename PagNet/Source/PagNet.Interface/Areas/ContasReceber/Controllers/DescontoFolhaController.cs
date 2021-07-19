using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Model;
using PagNet.Application.Application.Common;
using PagNet.Application.Interface;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Interface.Areas.Configuracao.Models;
using PagNet.Interface.Areas.ContasReceber.Models;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class DescontoFolhaController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IAPIGestaoDescontoFolhaAppClient _apiDescontoFolha;
        private readonly IAPIClienteAPP _APICliente;
        private readonly ICadastrosApp _cadastro;
        private readonly IDiversosApp _diversos;

        public DescontoFolhaController(IAPIGestaoDescontoFolhaAppClient apiDescontoFolha,
                                        IAPIClienteAPP APICliente,
                                        ICadastrosApp cadastro,
                                           IDiversosApp diversos,
                                        IPagNetUser user)
        {
            _user = user;
            _apiDescontoFolha = apiDescontoFolha;
            _APICliente = APICliente;
            _cadastro = cadastro;
            _diversos = diversos;
        }

        public IActionResult Index()
        {
            ModelDescontoFolhaVM model = new ModelDescontoFolhaVM();
            model.acessoAdmin = _user.isAdministrator;

            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codigoEmpresa = empresa.CODEMPRESA.ToString();
            model.nomeEmpresa = empresa.NMFANTASIA.ToString();

            return View(model);
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
        public JsonResult CarregaListaFaturas(int? id)
        {

            if (Convert.ToInt32(id) == 0) return Json("");

            int codigoCliente = 0;

            codigoCliente = (int)id;
            var dados = _apiDescontoFolha.CarregaListaFaturasAbertas(codigoCliente);

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
            RetornoValidaDescontoFolhaVM vm = new RetornoValidaDescontoFolhaVM();
            try
            {
                if(model.ValidaArquivo)
                    vm = RetornoValidaDescontoFolhaVM.ToView(_apiDescontoFolha.ConsolidaArquivoRetornoCliente(model));
                else
                    vm = RetornoValidaDescontoFolhaVM.ToView(_apiDescontoFolha.BuscaFechamentosNaoDescontados(model));

                return PartialView(vm);
            }
            catch (Exception ex)
            {
                vm.msgRetornoValidacao = "Ocorreu uma falha durante o processo de leitura do arquivo. Favor contactar o suporte.";
                TempData["Avis.Erro"] = "Ocorreu uma falha durante o processo de leitura do arquivo. Favor contactar o suporte.";
                return PartialView(vm);
            }

        }

        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> ProcessaDescontoFolha(RetornoValidaDescontoFolhaVM model)
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