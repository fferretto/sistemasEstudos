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
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.ContasPagar.Controllers
{
    [Area("ContasPagar")]
    [Authorize]
    public class ConsultarArquivoRemessaController : ClientSessionControllerBase
    {
        private readonly IRecebimentoApp _recebimento;
        private readonly IPagamentoApp _pag;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;
        private readonly IAPITrasmissaoPagamento _ApiEmissaoArquivoPgto;

        public ConsultarArquivoRemessaController(IRecebimentoApp recebimento,
                                                 IPagamentoApp pag,
                                                 IPagNetUser user,
                                                 ICadastrosApp cadastro,
                                                 IAPITrasmissaoPagamento ApiEmissaoArquivoPgto,
                                                 IDiversosApp diversos)
        {
            _recebimento = recebimento;
            _pag = pag;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
            _ApiEmissaoArquivoPgto = ApiEmissaoArquivoPgto;
        }
        public IActionResult Index(int? id)
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


        public async Task<ActionResult> CarregaGrig(FiltroDownloadArquivoVm model)
        {
            try
            {
                
                if (Convert.ToInt32(model.codEmpresa) < 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var Dados = await _pag.CarregaGridArquivo(model);
                
                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ConsultaPagamentosArquivo(int codArquivo, string dtArquivo)
        {
            try
            {
                var Dados = await _pag.GetAllPagamentosArquivo(codArquivo, dtArquivo);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
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
        [HttpPost]
        public ActionResult UploadArquivoValidacaoRemessa()
        {
            try
            {
                string novoCaminho = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {

                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                        var path = Path.Combine(CaminhoPadrao, "ArquivosVisualizados");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);


                        string NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff") + ".txt";

                        novoCaminho = Path.Combine(path, NewFileName);

                        formFile.SaveTo(novoCaminho);

                    }
                }

                return Json(new { success = true, responseText = novoCaminho });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu uma falha ao processar o arquivo. Favor contactar o suporte" });
            }

        }
        public async Task<IActionResult> ValidaArquivo(string caminho)
        {
            try
            {
                var Dados = await _pag.CarregaDadosArquivoRemessa(caminho);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                List<BaixaPagamentoVM> DadosRet = new List<BaixaPagamentoVM>();
                DadosRet.Add(new BaixaPagamentoVM()
                {
                    MsgRetorno = "Falha ao ler o arquivo. Favor Tentar novamente, mas se o problema persistir, favor contactar o suporte: "
                });
                return PartialView(DadosRet);            
            }

        }
        public JsonResult UpdateStatusArquivo(string caminhoArquivo)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(caminhoArquivo);

                string fileName = fileInfo.Name;

                var retorno = _pag.AtualizaStatusArquivoByName(fileName, "AGUARDANDO_ARQUIVO_RETORNO");
                
                return Json(retorno.Result.ToString());
            }
            catch (Exception)
            {
                return Json("Upload failed");
            }

        }

        [HttpPost]
        public IActionResult CancelaArquivoRemessa(int id)
        {
            try
            {
                var retorno = _ApiEmissaoArquivoPgto.CancelaArquivoRemessa(id);

                return Json(new { success = retorno.Sucesso, responseText = retorno.msgResultado });
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