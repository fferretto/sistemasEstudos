using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Application.Cnab.Comun;
using PagNet.Application.Helpers;
using PagNet.Application.Interface;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using PagNet.Interface.Areas.Relatorios.Models;
using PagNet.Interface.Helpers;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Relatorios.Controllers
{
    [Area("Relatorios")]
    [Authorize]
    public class RelatoriosController : ClientSessionControllerBase
    {
        private readonly IAPIRelatorioApp _relatorio;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly IAPIFavorecido _apiFavorecido;
        private readonly MetodosGerais _metodosGerais;

        public RelatoriosController(IAPIRelatorioApp relatorio,
                                    IPagNetUser user,
                                    IAPIFavorecido apiFavorecido,
                                    IDiversosApp diversos,
                                    MetodosGerais metodosGerais
                                    )
        {
            _relatorio = relatorio;
            _user = user;
            _diversos = diversos;
            _apiFavorecido = apiFavorecido;
            _metodosGerais = metodosGerais;
        }

        public IActionResult Index(int id)
        {
            var model = RelatoriosModel.ToView(_relatorio.BuscaParametrosRelatorio(id));

            return View(model);
        }
        [HttpPost]
        public IActionResult VisualizaRel(RelatoriosModel model)
        {
                var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                var url = Path.Combine(CaminhoPadrao, "RelatoriosCSV");

                var path = Path.GetFullPath(url);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                model.pathArquivo = path;
                var ParametrosRel = _relatorio.BuscaParametrosRelatorio(model.codRel);

                for (int i = 0; i < model.listaCampos.Count; i++)
                {
                    int codPar = model.listaCampos[i].ID_PAR;
                    var dadosPar = ParametrosRel.listaCampos.Where(x => x.ID_PAR == codPar).FirstOrDefault();

                    model.listaCampos[i].NOMPAR = dadosPar.NOMPAR;
                    model.listaCampos[i].TIPO = dadosPar.TIPO;
                    model.listaCampos[i].ORDEM_PROC = dadosPar.ORDEM_PROC;
                    model.listaCampos[i].LABEL = dadosPar.LABEL;
                    model.listaCampos[i].DESPAR = dadosPar.DESPAR;
                    if (dadosPar.NOMPAR == "CODEMPRESA")
                    {
                        if (Convert.ToInt32(model.listaCampos[i].VALCAMPO) <= 0)
                        {
                            model.listaCampos[i].VALCAMPO = _user.cod_empresa.ToString();
                        }
                    }
                }

                var retornoRel = _relatorio.RelatorioPDF(model);
                retornoRel.nmRelatorio = model.nmTela;

                var pdf = new ViewAsPdf("VisualizaRel", retornoRel)
                {
                    Model = retornoRel,
                    PageSize = Size.A4,
                    IsLowQuality = true,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(2, 6, 6, 5)
                };

                return pdf;

           
        }
        [HttpPost]
        public IActionResult ExportaExcel(RelatoriosModel model)
        {
            try
            {
                var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                var url = Path.Combine(CaminhoPadrao, "RelatoriosCSV" );

                var path = Path.GetFullPath(url);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                model.pathArquivo = path;
                var ParametrosRel = _relatorio.BuscaParametrosRelatorio(model.codRel);

                for (int i = 0; i < model.listaCampos.Count; i++)
                {
                    int codPar = model.listaCampos[i].ID_PAR;
                    var dadosPar = ParametrosRel.listaCampos.Where(x => x.ID_PAR == codPar).FirstOrDefault();

                    model.listaCampos[i].NOMPAR = dadosPar.NOMPAR;
                    model.listaCampos[i].TIPO = dadosPar.TIPO;
                    model.listaCampos[i].ORDEM_PROC = dadosPar.ORDEM_PROC;
                    model.listaCampos[i].LABEL = dadosPar.LABEL;
                    model.listaCampos[i].DESPAR = dadosPar.DESPAR;
                    if (dadosPar.NOMPAR == "CODEMPRESA")
                    {
                        if (Convert.ToInt32(model.listaCampos[i].VALCAMPO) <= 0)
                        {
                            model.listaCampos[i].VALCAMPO = _user.cod_empresa.ToString();
                        }
                    }
                }
                var retornoRel = _relatorio.ExportaExcel(model);

                return Json(new { ExecutadoViaJob = retornoRel.FirstOrDefault().Key, caminhoArquivo = retornoRel.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                return Json("erro");
            }
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
        public JsonResult BuscaFavorecido(string filtro, int? codigoEmpresa)
        {

            var DadosFavorecido = _metodosGerais.RetornaFavorecido(filtro, codigoEmpresa).FirstOrDefault();
           
            var retorno = DadosFavorecido.Key + "/" + DadosFavorecido.Value ;

            return Json(retorno);
        }
        public ActionResult ConsultaFavorecidos(int? codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                    codEmpresa = _user.cod_empresa;


                var dados = _apiFavorecido.ConsultaTodosFavorecidosPAG(Convert.ToInt32(codEmpresa));
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
        public ActionResult VerificaConclusaoRelatorio(int codigoRelatorio)
        {
            try
            {
                var relatorioConcluido = _relatorio.VerificaTerminoGeracaoRelatorio(codigoRelatorio);

                return Json(new { concluido = relatorioConcluido });
            }
            catch (ApplicationException ex)
            {
                Response.StatusCode = 409;
                return Content(ex.Message);
            }
        }
        public ActionResult GeraRelatorioViaJob(RelatoriosModel model)
        {
            try
            {
                var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                var url = Path.Combine(CaminhoPadrao, "RelatoriosCSV");

                var path = Path.GetFullPath(url);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var ParametrosRel = _relatorio.BuscaParametrosRelatorio(model.codRel);

                for (int i = 0; i < model.listaCampos.Count; i++)
                {
                    int codPar = model.listaCampos[i].ID_PAR;
                    var dadosPar = ParametrosRel.listaCampos.Where(x => x.ID_PAR == codPar).FirstOrDefault();

                    model.listaCampos[i].NOMPAR = dadosPar.NOMPAR;
                    model.listaCampos[i].TIPO = dadosPar.TIPO;
                    model.listaCampos[i].ORDEM_PROC = dadosPar.ORDEM_PROC;
                    model.listaCampos[i].LABEL = dadosPar.LABEL;
                    model.listaCampos[i].DESPAR = dadosPar.DESPAR;
                    if (dadosPar.NOMPAR == "CODEMPRESA")
                    {
                        if (Convert.ToInt32(model.listaCampos[i].VALCAMPO) <= 0)
                        {
                            model.listaCampos[i].VALCAMPO = _user.cod_empresa.ToString();
                        }
                    }
                }

                var retornoGeracaoRelatorio = _relatorio.GeraRelatorioViaJob(model);

                return Json(new { sucesso = retornoGeracaoRelatorio.Sucesso, msgResultado = retornoGeracaoRelatorio.msgResultado });
            }
            catch (ApplicationException ex)
            {
                Response.StatusCode = 409;
                return Content(ex.Message);
            }
        }
        public ActionResult RetornoRelatornoViaJob(RelatoriosModel model)
        {
            var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

            var url = Path.Combine(CaminhoPadrao, "RelatoriosCSV");

            var path = Path.GetFullPath(url);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            model.pathArquivo = path;
            try
            {
                var dadosRel = _relatorio.RetornoRelatornoViaJob(model);
                //relatório PDF
                if (dadosRel.TipoRel == 0)
                {
                    dadosRel.nmRelatorio = model.nmTela;

                    var pdf = new ViewAsPdf("RetornoRelatornoViaJob", dadosRel)
                    {
                        Model = dadosRel,
                        PageSize = Size.A4,
                        IsLowQuality = true,
                        PageOrientation = Orientation.Portrait,
                        PageMargins = new Margins(2, 6, 6, 5)
                    };
                    return pdf;
                }
                else
                {
                    return Json(new { TipoRelatorio = dadosRel.TipoRel, CaminhoArquivoRet = dadosRel.caminhoArquivo });
                }
            }
            catch (ApplicationException ex)
            {
                Response.StatusCode = 409;
                return Content(ex.Message);
            }
            
        }


    }
}