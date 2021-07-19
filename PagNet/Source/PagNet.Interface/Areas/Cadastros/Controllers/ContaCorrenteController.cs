using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Model;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class ContaCorrenteController : ClientSessionControllerBase
    {
        private readonly IAPIContaCorrente _APIContaCorrente;
        private readonly IAPIFavorecido _apiFavorecido;
        private readonly IPagNetUser _user;
        private readonly ICadastrosApp _cadastro;
        private readonly IDiversosApp _diversos;

        public ContaCorrenteController(IAPIContaCorrente APIContaCorrente,
                                       IAPIFavorecido apiFavorecido,
                                       ICadastrosApp cadastro,
                                       IPagNetUser user,
                                       IDiversosApp diversos)
        {
            _APIContaCorrente = APIContaCorrente;
            _apiFavorecido = apiFavorecido;
            _cadastro = cadastro;
            _user = user;
            _diversos = diversos;
        }

        // GET: Configuracao/ContaCorrente
        public ActionResult Index(int? id)
        {
            ContaCorrenteModel dados = new ContaCorrenteModel();
            dados = ContaCorrenteModel.ToView(_APIContaCorrente.GetContaCorrenteById(Convert.ToInt32(id), _user.cod_empresa));
            
            dados.acessoAdmin = _user.isAdministrator;
            dados.UsuarioTelenet = _user.isTelenet;

            return View(dados);
        }
        public ActionResult ConsultaContaCorrente(int? codEmpresa)
        {
            if (Convert.ToInt32(codEmpresa) <= 0)
                codEmpresa = _user.cod_empresa;

            var ListaRetorno = new List<ConsultaContaCorrenteModel>();

            var dados = _APIContaCorrente.GetAllContaCorrente((int)codEmpresa);
            if (dados != null)
            {
                ListaRetorno = ConsultaContaCorrenteModel.ToListView(dados).ToList();
            }
            return PartialView(ListaRetorno);

        }
        public ActionResult ProcessoHomologacaoConta(int codigoContaCorrente)
        {
            DadosHomologarContaCorrenteModel dados = new DadosHomologarContaCorrenteModel();
            dados.ExisteArqRemessaBol = _APIContaCorrente.ExisteArquivoRemessaBoletoCriado(codigoContaCorrente);
            return PartialView(dados);
        }
        public async Task<ActionResult> ConsultaFavorecido(int? codigoEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codigoEmpresa) <= 0)
                {
                    codigoEmpresa = _user.cod_empresa;
                }
                var dados = _apiFavorecido.ConsultaTodosFavorecidosPAG(Convert.ToInt32(codigoEmpresa));
                var ListaFavorecido = APIFavorecidoVM.ToListView(dados);

                return PartialView(ListaFavorecido);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }
        }
        public JsonResult BuscaFavorecido(string filtroFavorecido, int? codigoEmpresa)
        {
            APIFavorecidoVM model = new APIFavorecidoVM();

            if (Convert.ToInt32(codigoEmpresa) <= 0)
            {
                codigoEmpresa = _user.cod_empresa;
            }

            if (filtroFavorecido.Length <= 10)
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID(Convert.ToInt32(filtroFavorecido), Convert.ToInt32(codigoEmpresa)));
            }
            else
            {
                model = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByCPFCNPJ(filtroFavorecido, Convert.ToInt32(codigoEmpresa)));
            }

            var retorno = model.codigoFavorecido + "/" + model.nomeFavorecido;

            if (model.codigoEmpresa != "" && model.codigoEmpresa != codigoEmpresa.ToString())
            {
                retorno = filtroFavorecido + "/Favorecido não encontrado";
            }

            return Json(retorno);
        }
        [HttpPost]
        public ActionResult Salvar(ContaCorrenteModel model)
        {
            try
            {

                if (Convert.ToInt32(model.codEmpresa) <= 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                if (string.IsNullOrWhiteSpace(model.CodConvenioPag))
                {
                    model.CodConvenioPag = "";
                }
               

                if (ModelState.IsValid)
                {

                    var result = _APIContaCorrente.Salvar(model);
                    if (result.FirstOrDefault().Key == "SUCESSO")
                    {
                        TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                        return RedirectToAction("Index", new { id = model.codContaCorrente });
                    }
                    else
                    {
                        ModelState.AddModelError(result.FirstOrDefault().Key, result.FirstOrDefault().Value.ToString());
                        TempData["Avis.Aviso"] = result.FirstOrDefault().Value.ToString();
                        return View("Index", model);
                    }

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu uma falha ao salvar a conta corrente. Favor contactar o suporte.";
                return View("Index", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Desativar(ContaCorrenteModel model)
        {
            try
            {
                _APIContaCorrente.Desativar(model.codContaCorrente);

                TempData["Avis.Sucesso"] = "Conta corrente removida com sucesso!";
                return RedirectToAction("Index", new { id = "" });
                
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return View("Index", model.codContaCorrente);
            }
        }
        [HttpPost]
        public JsonResult GeraArquivo(DadosHomologarContaCorrenteModel model)
        {
            try
            {
                model.codOPE = _user.cod_ope;

                if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                    model.codigoEmpresa = _user.cod_empresa;

                var InfoArquivoGerado = _APIContaCorrente.GeraArquivoRemessaHomologacao(model);
                if(InfoArquivoGerado.Resultado)
                {
                    return Json(new { success = InfoArquivoGerado.Resultado, responseText = InfoArquivoGerado.CaminhoCompletoArquivo });
                }
                else
                {
                    return Json(new { success = InfoArquivoGerado.Resultado, responseText = InfoArquivoGerado.msgResultado });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async Task<FileResult> DownloadArquivoRemessa(string id, int codigoEmpresa, string TipoArquivo)
        {

            var path = Path.GetFullPath(id);

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
        public async Task<FileResult> DownloadBoleto(DadosHomologarContaCorrenteModel model)
        {

            if (model.codigoEmpresa <= 0)
            {
                model.codigoEmpresa = _user.cod_empresa;
            }

            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, model.codigoEmpresa);
            model.codOPE = _user.cod_ope;
            model.codigoUsuario = _user.cod_usu;
            model.CaminhoArquivo = CaminhoPadrao;

            var InfoBoleto = _APIContaCorrente.GeraBoletoPDFHomologacao(model);


            var memory = new MemoryStream();
            using (var stream = new FileStream(InfoBoleto.CaminhoCompletoArquivo, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(InfoBoleto.CaminhoCompletoArquivo), Path.GetFileName(InfoBoleto.CaminhoCompletoArquivo));
        }

    }
}