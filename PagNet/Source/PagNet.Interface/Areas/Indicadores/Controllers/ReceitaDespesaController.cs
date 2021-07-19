using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System;
using System.Threading.Tasks;

namespace PagNet.Interface.Areas.Indicadores.Controllers
{
    [Area("Indicadores")]
    [Authorize]
    public class ReceitaDespesaController : Controller
    {
        private readonly IIndicadoresApp _indicadoresApp;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;

        public ReceitaDespesaController(IIndicadoresApp indicadoresApp,
                                        IPagNetUser user,
                                        ICadastrosApp cadastro,
                                        IDiversosApp diversos)
        {
            _indicadoresApp = indicadoresApp;
            _user = user;
            _diversos = diversos;
            _cadastro = cadastro;
        }


        public async Task<IActionResult> Index(string id)
        {
            IndicaroresVM model = new IndicaroresVM();

            model.dtInicio = DateTime.Now.AddDays(-30).ToShortDateString();
            model.dtFim = DateTime.Now.ToShortDateString();

            var empresa = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
            model.codEmpresa = empresa.CODEMPRESA.ToString();
            model.nmEmpresa = empresa.NMFANTASIA.ToString();

            model.acessoAdmin = _user.isAdministrator;

            return View(model);
        }

        public async Task<IActionResult> GraficoReceitaDespesa()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                TempData["Avis.Aviso"] = ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> GraficoPagamentoPeriodo()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                TempData["Avis.Aviso"] = ex.Message;
                return View("Index");
            }
        }

        public async Task<IActionResult> GraficoReceitaPeriodo()
        {
             return PartialView();
        }
        public JsonResult IndicadorPagamentoPeriodo(IndicaroresVM model)
        {
            if (Convert.ToInt32(model.codEmpresa) < 0)
                model.codEmpresa = _user.cod_empresa.ToString();

            var chartReceitaDespesa = _indicadoresApp.GetChartPagPrevistosPorPeriodos(model);

            return new JsonResult(chartReceitaDespesa);
        }
        public JsonResult IndicadorReceitaPeriodo(IndicaroresVM model)
        {
            if (Convert.ToInt32(model.codEmpresa) < 0)
                model.codEmpresa = _user.cod_empresa.ToString();

            var chartReceitaDespesa = _indicadoresApp.GetChartRecebPrevistosPorPeriodos(model);

            return new JsonResult(chartReceitaDespesa);
        }
        public JsonResult IndicadorReceitaDespesa(IndicaroresVM model)
        {
            if (Convert.ToInt32(model.codEmpresa) < 0)
                model.codEmpresa = _user.cod_empresa.ToString();
            
            var chartReceitaDespesa = _indicadoresApp.GetChartRecDespPrevistosAno(model);

            return new JsonResult(chartReceitaDespesa);
        }
    }
}