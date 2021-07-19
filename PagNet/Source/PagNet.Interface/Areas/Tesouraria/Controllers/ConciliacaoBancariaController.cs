using System;
using System.Collections.Generic;
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

namespace PagNet.Interface.Areas.Tesouraria.Controllers
{
    [Area("Tesouraria")]
    [Authorize]
    public class ConciliacaoBancariaController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ITesourariaApp _tesouraria;

        public ConciliacaoBancariaController(ITesourariaApp tesouraria,
                                         IDiversosApp diversos,
                                         IPagNetUser user)
        {
            _tesouraria = tesouraria;
            _user = user;
            _diversos = diversos;
        }
        public IActionResult Index()
        {
            FiltroConciliacaoBancariaVm md = new FiltroConciliacaoBancariaVm();
            md.codigoEmpresa = _user.cod_empresa.ToString();
            md.nomeEmpresa = _diversos.GetnmEmpresaByID(_user.cod_empresa);
            md.acessoAdmin = _user.isAdministrator;

            return View(md);
        }
        [HttpPost]
        public ActionResult UploadHomeReport()
        {
            try
            {
                string novoCaminho = "";
                foreach (var formFile in Request.Form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var CaminhoPadrao = _diversos.GetCaminhoArquivoPadrao(_user.cod_ope, _user.cod_empresa);

                        var path = Path.Combine(CaminhoPadrao, _user.nmPerfilOperadora, _user.cod_empresa.ToString(), "ConciliacaoBancaria");

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
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        public async Task<IActionResult> ListarTransacoes(FiltroConciliacaoBancariaVm model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresa) < 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                model.codigoUsuario = _user.cod_usu;
                //lê o arquivo de retorno e realiza a baixa nos pagamentos
                var Dados = await _tesouraria.ProcessaArquivoConciliacaoBancaria(model);


                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return PartialView(null);
            }

        }
        [HttpPost]
        public IActionResult IncluirTransacao(IncluiTransacaoVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codigoEmpresaIncTransacao) <= 0)
                    model.codigoEmpresaIncTransacao = _user.cod_empresa.ToString();

                model.codigoUsuarioIncTransacao = _user.cod_usu;
                model.ValorIncTransacao = model.ValorIncTransacao.Replace("R$", "");
                model.ValorTotalIncTransacao = model.ValorTotalIncTransacao.Replace("R$", "");

                IDictionary<bool, string> resultado;
                if (model.TipoIncTransacao == "Entrada")
                {
                    resultado = _tesouraria.IncluirNovaTransacaoJaConciliadaENTRADA(model).Result;
                }
                else
                {
                    resultado = _tesouraria.IncluirNovaTransacaoJaConciliadaSAIDA(model).Result;
                }

                return Json(new { success = true, responseText = resultado.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        
        public JsonResult CarregaListaAcoesConciliacao()
        {

            var lista = new object[][]
             {
                //new object[] { "RealizarAcao", "VINCULARTRANSACAO", "Vincular a uma Transação"},
                new object[] { "RealizarAcao", "INCLUIRTRANSACAO", "Incluir Transação" }
             };

            return Json(lista.ToList());
        }
    }
}