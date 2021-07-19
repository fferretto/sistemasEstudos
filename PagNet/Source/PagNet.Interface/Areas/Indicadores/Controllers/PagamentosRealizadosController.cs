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
    public class PagamentosRealizadosController : Controller
    {
        private readonly IIndicadoresApp _indicadoresApp;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ICadastrosApp _cadastro;

        public PagamentosRealizadosController(IIndicadoresApp indicadoresApp,
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
        public async Task<IActionResult> GraficoPagamentosPeriodo()
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

        public async Task<IActionResult> GraficoPagamentoMensal()
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
        public JsonResult IndicadorPagamentoPeriodo(IndicaroresVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) < 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var chart = _indicadoresApp.GetChartPagRealizadoDia(model);

                return new JsonResult(chart);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return new JsonResult(null);
            }
        }
        public JsonResult IndicadorPagamentoMensal(IndicaroresVM model)
        {
            try
            {
                if (Convert.ToInt32(model.codEmpresa) < 0)
                    model.codEmpresa = _user.cod_empresa.ToString();

                var chart = _indicadoresApp.GetChartPagRealizadoAno(model);

                return new JsonResult(chart);

            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return new JsonResult(null);
            }
        }

    }
}