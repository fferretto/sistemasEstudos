using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasReceber.Controllers
{
    [Area("ContasReceber")]
    [Authorize]
    public class ConsultarArquivoRemessaController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagamentoApp _pag;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;

        public ConsultarArquivoRemessaController(IRecebimentoApp recebimento,
                                                 IPagamentoApp pag,
                                                 IPagNetUser user,
                                                 ICadastrosApp cadastro,
                                                 IDiversosApp diversos)
        {
            _recebimento = recebimento;
            _pag = pag;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
        }
        public async Task<IActionResult> Index(int? id)
        {
            FiltroDownloadArquivoVm model = new FiltroDownloadArquivoVm();

            DateTime data = DateTime.Now;

            model.dtInicio = data.ToShortDateString();
            model.dtFim = data.ToShortDateString();


            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA.ToString();

            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }

        public async Task<ActionResult> CarregaGridArquivosRemessa(FiltroDownloadArquivoVm model)
        {
            try
            {

                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var Dados = await _recebimento.CarregaGridArquivosRemessa(model);

                for(int i=0; i< Dados.Count; i++)
                {
                    Dados[i].codigoEmpresaArquivo = Convert.ToInt32(model.codEmpresa);
                }

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
            }
        }

        public async Task<ActionResult> VisualizarBoletos(int codArquivo)
        {
            try
            {
                var dados = await _recebimento.GetAllBoletosByCodArquivo(codArquivo, _user.cod_ope);

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        
        public async Task<FileResult> DownloadArquivoRemessa(string id, int codempresa)
        {
            if (codempresa <= 0)
            {
                codempresa = _user.cod_empresa;
            }
            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, codempresa);

            var url = Path.Combine(CaminhoPadrao, "ArquivoRemessaBoleto", id);

            var path = Path.GetFullPath(url);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        [HttpPost]
        public async Task<IActionResult> CancelaArquivoRemessa(int id)
        {
            try
            {
                var retorno = _recebimento.CancelaArquivoRemessaByID(id, _user.cod_usu);

                return Json(new { success = true, responseText = "Arquivo de remessa cancelado com sucesso!" });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(new { success = false, responseText = "Ocorreu uma falha inesperada. Favor contactar o suporte!" });
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
    }
}