using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Configuracao.Controllers
{
    [Area("Configuracao")]
    [Authorize]
    public class ConfigRegraBoletoController : ClientSessionControllerBase
    {
        private readonly IConfiguracaoApp _config;
        private readonly IPagNetUser _user;

        public ConfigRegraBoletoController(IConfiguracaoApp config,
                                        IPagNetUser user)
        {
            _config = config;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            RegraNegocioBoletoVm model = new RegraNegocioBoletoVm();

            model = await _config.BuscaRegraAtiva(_user.cod_empresa);

            model.acessoAdmin = _user.isAdministrator;
            model.CodUsuario = _user.cod_usu;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SalvaRegra(RegraNegocioBoletoVm model)
        {
            try
            {

                model.CodUsuario = _user.cod_usu;

                if (Convert.ToInt32(model.CODEMPRESA) <= 0)
                    model.CODEMPRESA = _user.cod_empresa.ToString();

                var result = await _config.SalvaRegraBoleto(model);
                if (result.FirstOrDefault().Key)
                {
                    TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                }
                else
                {
                    TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                }
                return View("Index");


            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        [HttpPost]
        public async Task<IActionResult> DesativarRegra(RegraNegocioBoletoVm model)
        {
            try
            {
                if (model.codJustificativa != "OUTROS")
                {
                    model.DescJustOutros = model.descJustificativa;
                }

                var result = await _config.DesativaRegraBol(model.CODREGRA, _user.cod_usu, model.DescJustOutros);

                return Json(new { success = true, responseText = result.FirstOrDefault().Value });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }

        }
        public JsonResult GetJustificativaDesativacao()
        {
            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "REGRANAOVIGENTE", "NÃO UTILIZARA MAIS REGRAS"},
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public ActionResult Justificativa()
        {
            JustificativaVm vm = new JustificativaVm();

            return PartialView(vm);

        }
    }
}