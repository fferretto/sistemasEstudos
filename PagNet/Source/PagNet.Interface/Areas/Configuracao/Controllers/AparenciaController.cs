using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Configuracao.Controllers
{
    [Area("Configuracao")]
    [Authorize]
    public class AparenciaController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly IValidaUsuarioApp _validaUsuario;
        private readonly IAparenciaSistemaApp _Aparencia;

        public AparenciaController(IPagNetUser user,
                                   IValidaUsuarioApp validaUsuario,
                                   IAparenciaSistemaApp Aparencia,
                                   IDiversosApp diversos)
        {
            _validaUsuario = validaUsuario;
            _user = user;
            _diversos = diversos;
            _Aparencia = Aparencia;
        }

        // GET: Configuracao/ Aparencia
        public async Task<ActionResult> Index()
        {
            var model = _Aparencia.CarregaLayoutAtual(_user.cod_ope, _user.cod_empresa);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UploadImgLogo()
        {
            try
            {
                string novoCaminho = "";                

                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                        string CaminhoArquivo = Path.Combine(CaminhoPadrao, "Imagens", "Logo");

                        DirectoryInfo dir = new DirectoryInfo(CaminhoArquivo);
                        var Arquivos = dir.GetFiles("*.*").ToList();

                        //verifica primeiro se existe a ultima logo que foi alterada
                        foreach (var file in Arquivos)
                        {
                            if (file.Name.Contains("New"))
                            {
                                System.IO.File.Delete(file.FullName);
                            }
                        }
                        Arquivos = dir.GetFiles("*.*").ToList();

                        string NewFileName = "V" + (Arquivos.Count + 1) + "_ImagemLogoNew." + formFile.FileName.Substring(formFile.FileName.Length - 3, 3);

                        var path = Path.Combine(CaminhoPadrao, "Imagens", "Logo", NewFileName);
                        novoCaminho = path;

                        formFile.SaveTo(novoCaminho);
                    }
                }

                return Json(novoCaminho);
            }
            catch (Exception)
            {
                return Json("");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadImgIco()
        {
            try
            {
                string novoCaminho = "";

                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);
                        string CaminhoArquivo = Path.Combine(CaminhoPadrao, "Imagens", "Icone");

                        DirectoryInfo dir = new DirectoryInfo(CaminhoArquivo);
                        var Arquivos = dir.GetFiles("*.*").ToList();

                        //verifica primeiro se existe a ultima logo que foi alterada
                        foreach (var file in Arquivos)
                        {
                            if (file.Name.Contains("New"))
                            {
                                System.IO.File.Delete(file.FullName);
                            }
                        }
                        Arquivos = dir.GetFiles("*.*").ToList();

                        string NewFileName = "V" + (Arquivos.Count + 1) + "_ImagemIconeNew." + formFile.FileName.Substring(formFile.FileName.Length - 3, 3);

                        var path = Path.Combine(CaminhoArquivo, NewFileName);
                        novoCaminho = path;

                        formFile.SaveTo(novoCaminho);
                    }
                }

                return Json(novoCaminho);
            }
            catch (Exception)
            {
                return Json("");
            }

        }

        [HttpGet]
        public IActionResult GetImagemLogo(string timestamp)
        {
            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);


            string CaminhoArquivo = Path.Combine(CaminhoPadrao, "Imagens", "Logo");

            DirectoryInfo dir = new DirectoryInfo(CaminhoArquivo);
            var Arquivos = dir.GetFiles("*.*").ToList();
            string caminhoArquivoRetorno = "";

            //verifica primeiro se existe a ultima logo que foi alterada
            foreach (var file in Arquivos)
            {
                if (file.Name.Contains("New"))
                {
                    caminhoArquivoRetorno = file.FullName;
                }
            }

            //caso não encontre, o sistema busca a logo default
            if (caminhoArquivoRetorno == "")
            {
                var _arq = Arquivos.Where(y => !y.Name.Contains("Thumbs")).OrderByDescending(x => x.LastAccessTime).FirstOrDefault();
                caminhoArquivoRetorno = _arq.FullName;
            }

            string extensaoArquivo = caminhoArquivoRetorno.Substring(caminhoArquivoRetorno.Length - 3, 3);
            return File(System.IO.File.ReadAllBytes(caminhoArquivoRetorno), "image/" + extensaoArquivo);
        }
        [HttpGet]
        public IActionResult GetImagemIco(string timestamp)
        {
            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);


            string CaminhoArquivo = Path.Combine(CaminhoPadrao, "Imagens", "Icone");


            DirectoryInfo dir = new DirectoryInfo(CaminhoArquivo);

            if (!dir.Exists)
            {
                dir.Create();
                var CaminhoImagem = Path.Combine(CaminhoArquivo, "V1_ImagemIconeNew.ico");
                var pathIco = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var ImagDefault = Path.Combine(pathIco, "icoPagNet.ico");
                System.IO.File.Copy(ImagDefault, CaminhoImagem);
            }

            var Arquivos = dir.GetFiles("*.*").ToList();
            if (Arquivos.Count == 0)
            {
                var CaminhoImagem = Path.Combine(CaminhoArquivo, "V1_ImagemIconeNew.ico");
                var pathIco = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var ImagDefault = Path.Combine(pathIco, "icoPagNet.ico");
                System.IO.File.Copy(ImagDefault, CaminhoImagem);
            }
            string caminhoArquivoRetorno = "";

            //verifica primeiro se existe a ultima logo que foi alterada
            foreach (var file in Arquivos)
            {
                if (file.Name.Contains("New"))
                {
                    caminhoArquivoRetorno = file.FullName;
                }
            }

            //caso não encontre, o sistema busca a logo default
            if (caminhoArquivoRetorno == "")
            {
                var _arq = Arquivos.Where(y => !y.Name.Contains("Thumbs")).OrderByDescending(x => x.LastAccessTime).FirstOrDefault();
                caminhoArquivoRetorno = _arq.FullName;
            }

            return File(System.IO.File.ReadAllBytes(caminhoArquivoRetorno), "image/ico");
        }

        [HttpPost]
        public async Task<ActionResult> Salvar(AparenciaSistemaVm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.CodOpe = _user.cod_ope;
                    model.codEmpresa = _user.cod_empresa;
                    var result = await _Aparencia.SalvarLayout(model);

                    if (!result.Any())
                    {
                        TempData["Avis.Sucesso"] = "Aparencia salva com sucesso!";
                        return RedirectToAction("Index");
                    }
                    foreach (var item in result)
                    {
                        //Where do I get the key from the view model?
                        ModelState.AddModelError(item.Key, item.Value.ToString());
                        TempData["Avis.Sucesso"] = item.Value.ToString();
                        return View("Index", model);
                    }
                    return View("Index", model);

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return View("Index", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> ResetarAparencia(AparenciaSistemaVm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\css");

                    model.CodOpe = _user.cod_ope;
                    model.codEmpresa = _user.cod_empresa;
                    model.CaminhoDefaultPagNet = Caminho;

                    var result = await _Aparencia.LayoutDefaultPagNet(model);

                    if (!result.Any())
                    {
                        TempData["Avis.Sucesso"] = "Layout resetado com sucesso!";
                        return RedirectToAction("Index");
                    }
                    foreach (var item in result)
                    {
                        //Where do I get the key from the view model?
                        ModelState.AddModelError(item.Key, item.Value.ToString());
                        TempData["Avis.Sucesso"] = item.Value.ToString();
                        return View("Index", model);
                    }
                    return View("Index", model);

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return View("Index", model);
            }
        }

    }
}