using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.ContasPagar.Models;
using PagNet.Interface.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class GerarArquivoRemessaController : ClientSessionControllerBase
    {
        private readonly IPagamentoApp _fecheCre;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPITrasmissaoPagamento _ApiEmissaoArquivoPgto;

        public GerarArquivoRemessaController(IPagamentoApp fecheCre,
                                                IPagNetUser user,
                                             ICadastrosApp cadastro,
                                                IAPITrasmissaoPagamento ApiEmissaoArquivoPgto,
                                                IDiversosApp diversos)
        {
            _fecheCre = fecheCre;
            _user = user;
            _diversos = diversos;
            _ApiEmissaoArquivoPgto = ApiEmissaoArquivoPgto;
            _cadastro = cadastro;
        }

        public ActionResult Index(string id)
        {

            FiltroConsultaBorderoPagVM model = new FiltroConsultaBorderoPagVM();

            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA;
            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        public ActionResult ConsultaBordero(FiltroConsultaBorderoPagVM model)
        {
            try
            {

                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var Dados = _fecheCre.ConsultaBordero(model);

                Dados.codigoEmpresa = model.codEmpresa;
                Dados.codigoFormaPGTO = model.CodFormaPagamento;
                Dados.codigoBanco = model.codBanco;

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                GridTitulosVM Dados = new GridTitulosVM();
                return PartialView(Dados);
            }

        }
        public async Task<ActionResult> ConsultaTitulosBordero(int codBordero)
        {
            try
            {
                var dados = await _fecheCre.GetAllPagamentosBordero(codBordero);

                return PartialView(dados);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public JsonResult BuscaBanco(string filtro)
        {
            FiltroTitulosPagamentoVM model;
            var retorno = "";

            if (IsNumeric(filtro))
            {
                model = _fecheCre.FindBancoByID(Convert.ToInt32(filtro));

                retorno = model.filtroCodBanco + "/" + model.FiltroNmBanco + "/" + model.codBanco;
            }
            else
            {
                retorno = "0/Código Inválido/0";
            }

            return Json(retorno);
        }

        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> GeraArquivo(TransmissaoArquivoModal model)
        {
            try
            {
                var ResultadoTransmissao = new Dictionary<string, string>();

                ResultadoTransmissao = _ApiEmissaoArquivoPgto.TransmiteArquivoBanco(model);

                bool bSucesso = (ResultadoTransmissao.FirstOrDefault().Key != "");
                string msgRetorno = (bSucesso) ? ResultadoTransmissao.FirstOrDefault().Key : ResultadoTransmissao.FirstOrDefault().Value;

                return Json(new { success = bSucesso, responseText = msgRetorno });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu uma falha no processo. Favor contactar o suporte." });
            }

        }

        public async Task<FileResult> DownloadArquivoRemessa(string id, int codigoEmpresa)
        {


            if (codigoEmpresa <= 0)
                codigoEmpresa = _user.cod_empresa;

            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, codigoEmpresa);

            var url = Path.Combine(CaminhoPadrao, "ArquivoRemessaPagamento", id);

            var path = Path.GetFullPath(url);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        public async Task<IActionResult> DownloadArquivo(string url)
        {
            try
            {
                if (url == null)
                    return Content("Arquivo não Encontrado");

                var path = Path.GetFullPath(url);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json(ex.Message);
            }

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
        /// <summary>
        /// Se todos os caracters forem digitos então é numerico
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsNumeric(string data)
        {
            bool isnumeric = false;
            char[] datachars = data.ToCharArray();

            foreach (var datachar in datachars)
                isnumeric = isnumeric ? char.IsDigit(datachar) : isnumeric;


            return isnumeric;
        }
    }
}
