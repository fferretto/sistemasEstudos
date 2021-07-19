using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.ContasReceber.Models;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class GerarArquivoRemessaController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly IAPITransmissaoCobrancaBancaria _ApiCobrancaBancaria;

        public GerarArquivoRemessaController(IRecebimentoApp recebimento,
                                               IPagNetUser user,
                                               IDiversosApp diversos,
                                               IAPITransmissaoCobrancaBancaria ApiCobrancaBancaria
                                               )
        {
            _recebimento = recebimento;
            _user = user;
            _diversos = diversos;
            _ApiCobrancaBancaria = ApiCobrancaBancaria;
        }
        public async Task<IActionResult> Index(string id)
        {
            var model = _recebimento.RetornaDadosInicioConstulaBordero(id, _user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        public async Task<ActionResult> ConsultaBordero(FiltroConsultaFaturamentoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                model.codStatus = "EM_BORDERO";

                var Dados = _recebimento.ConsultaBordero(model);

                Dados.codigoEmpresa = model.codigoEmpresa;
                Dados.dtVencimento = DateTime.Now.ToShortDateString();

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                BorderoBolVM Dados = new BorderoBolVM();
                return PartialView(Dados);
            }

        }
        public async Task<ActionResult> ConsultaBoletosBordero(int codBordero)
        {
            try
            {
                var dados = await _recebimento.GetAllBoletosByBordero(codBordero);

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
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public ActionResult GeraArquivo(FiltroTransmissaoCB model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa;

                var caminhoArquvio = _ApiCobrancaBancaria.GeraArquivoRemessa(model);

                return Json(new { success = true, responseText = caminhoArquvio });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.InnerException.Message });
            }

        }
        public async Task<FileResult> DownloadArquivoRemessa(string url)
        {

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