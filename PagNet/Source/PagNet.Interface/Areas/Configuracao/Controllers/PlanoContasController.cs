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
    public class PlanoContasController : ClientSessionControllerBase
    {
        private readonly IConfiguracaoApp _config;
        private readonly IPagNetUser _user;

        public PlanoContasController(IConfiguracaoApp config,
                                      IPagNetUser user)
        {
            _config = config;
            _user = user;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _config.CarregaPlanosContas(_user.cod_empresa);
            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }
        public async Task<ActionResult> AlteraPlanoContas(int codigoPlanoContas)
        {
            var model = await _config.CarregaPlanoContas(codigoPlanoContas);

            return PartialView(model);
        }
        public async Task<ActionResult> NovoPlanoContas(int codigoEmpresa)
        {
            if (Convert.ToInt32(codigoEmpresa) < 0)
                codigoEmpresa = _user.cod_empresa;

            var retorno = _config.BuscaNovaClassificacaoPlanoContas(0, codigoEmpresa).Result;
            
            PlanoContasVm PC = new PlanoContasVm();
            PC.codigoEmpresaPlanoContas = codigoEmpresa;
            PC.Classificacao = retorno.FirstOrDefault().Value;

            return PartialView(PC);
        }
        public async Task<ActionResult> CarregaListaPlanoContas(int codigoEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codigoEmpresa) < 0)
                    codigoEmpresa = _user.cod_empresa;

                var ListaPlanoContas = await _config.BuscaListaPlanosContas(codigoEmpresa);

                return PartialView(ListaPlanoContas);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao carregar a lista de planos de contas. Favor contactar o suporte técnico.";
                return PartialView(null);
            }
        }
        public async Task<ActionResult> RemovePlanoContas(int codigoPlanoContas, int codigoEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codigoEmpresa) < 0)
                    codigoEmpresa = _user.cod_empresa;

                var result = await _config.RemovePlanoContas(codigoPlanoContas, codigoEmpresa);

                return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return Json(ex.Message);
            }
        }
        public JsonResult CarregaListaTipoConta()
        {
            var lista = new object[][]
             {
                new object[] { "Sintetica", "Sintetica"},
                new object[] { "Analítica", "Analítica" }
             };

            return Json(lista.ToList());
        }
        public JsonResult CarregaListaNatureza()
        {
            var lista = new object[][]
             {
                new object[] { "Receita", "Receita"},
                new object[] { "Despesa", "Despesa" },
                new object[] { "Ativo", "Ativo" },
                new object[] { "Passivo", "Passivo" }
             };

            return Json(lista.ToList());
        }
        public JsonResult BuscaNovaClassificacao(int codigoRaiz, int codigoEmpresa)
        {
            var retorno = _config.BuscaNovaClassificacaoPlanoContas(codigoRaiz, codigoEmpresa).Result;

            return Json(new { success = retorno.FirstOrDefault().Key, responseText = retorno.FirstOrDefault().Value });
        }

        [HttpPost]
        public async Task<ActionResult> SalvarNovoPlanoContas(PlanoContasVm model)
        {
            try
            {
                model.CodUsuarioPlanoContas = _user.cod_usu;
                 var Resultado = await _config.SalvarNovoPlanoContas(model);

                return Json(new { success = Resultado.FirstOrDefault().Key, responseText = Resultado.FirstOrDefault().Value });
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(new { success = false, responseText = "Ocorreu uma falha inesperada. Favor contactar o suporte!" });
            }

        }
    }
}