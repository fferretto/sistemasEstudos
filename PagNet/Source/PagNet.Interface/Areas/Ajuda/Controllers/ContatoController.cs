using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Ajuda.Controllers
{
    [Area("Ajuda")]
    [Authorize]
    public class ContatoController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IAjudaApp _ajuda;
        private readonly IDiversosApp _diversos;
        

        public ContatoController(IAjudaApp ajuda,
                                 IPagNetUser user,
                                 IDiversosApp diversos)
        {
            _ajuda = ajuda;
            _user = user;
            _diversos = diversos;
        }
        public IActionResult Index()
        {
            List<ListaAnexosVm> lista = new List<ListaAnexosVm>();
            ContatoViaEmailVM dadosParaContato = new ContatoViaEmailVM();
            dadosParaContato.acessoAdmin = _user.isAdministrator;
            dadosParaContato.Anexo = lista;

            return View(dadosParaContato);
        }
        [HttpPost]
        public ActionResult UploadHomeReport()
        {
            try
            {
                string novoCaminho = "";
                string nomeArquivoUpload = "";
                string NewFileName = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                        nomeArquivoUpload = formFile.FileName;

                        var path = Path.Combine(CaminhoPadrao, "ArquivoAnexoContato");

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        NewFileName = DateTime.Now.ToString("ddMMyyHHmmssffff");

                        if (nomeArquivoUpload.Contains(".jpg"))
                            NewFileName += ".jpg";
                        else if (nomeArquivoUpload.Contains(".gif"))
                            NewFileName += ".gif";
                        else if (nomeArquivoUpload.Contains(".png"))
                            NewFileName += ".png";
                        else if (nomeArquivoUpload.Contains(".pdf"))
                            NewFileName += ".pdf";
                        else if (nomeArquivoUpload.Contains(".txt"))
                            NewFileName += ".txt";
                        else if (nomeArquivoUpload.Contains(".doc"))
                            NewFileName += ".doc";
                        else if (nomeArquivoUpload.Contains(".docx"))
                            NewFileName += ".docx";
                        else if (nomeArquivoUpload.Contains(".xlsx"))
                            NewFileName += ".xlsx";
                        else if (nomeArquivoUpload.Contains(".xls"))
                            NewFileName += ".xls";
                        else
                            NewFileName += ".tmp";

                        novoCaminho = Path.Combine(path, NewFileName);

                        formFile.SaveTo(novoCaminho);
                    }
                }
                return Json(new { success = true, novoNomeArquivo = NewFileName, nomeArquivo = nomeArquivoUpload });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult EnviarEmail(ContatoViaEmailVM model)
        {

            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
            model.CaminhoArquivoPadrao = Path.Combine(CaminhoPadrao, "ArquivoAnexoContato");
            var retorno = _ajuda.EnviarEmail(model).Result;

            return Json(new { success = retorno.FirstOrDefault().Key, mensagem = retorno.FirstOrDefault().Value });
          
        }
    }
}