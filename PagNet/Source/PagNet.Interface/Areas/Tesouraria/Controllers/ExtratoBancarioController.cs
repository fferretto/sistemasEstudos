using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Tesouraria.Controllers
{
    [Area("Tesouraria")]
    [Authorize]
    public class ExtratoBancarioController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;
        private readonly ITesourariaApp _secretaria;

        public ExtratoBancarioController(ITesourariaApp secretaria,
                                         IDiversosApp diversos,
                                         IPagNetUser user)
        {
            _secretaria = secretaria;
            _user = user;
            _diversos = diversos;
        }
        public IActionResult Index()
        {
            FiltroExtratoBancarioVm vm = new FiltroExtratoBancarioVm();
            vm.acessoAdmin = _user.isAdministrator;
            vm.codigoEmpresa = _user.cod_empresa.ToString();
            vm.nomeEmpresa = _diversos.GetnmEmpresaByID(_user.cod_empresa);
            vm.dtInicio = DateTime.Now.AddDays(-15).ToShortDateString();
            vm.dtFim = DateTime.Now.ToShortDateString();

            return View(vm);
        }

        public async Task<ActionResult> BuscaSaldoContaCorrente(FiltroExtratoBancarioVm model)
        {
            try
            {

                if (Convert.ToInt32(model.codigoEmpresa) < 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _secretaria.BuscaSaldoContaCorrente(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ListaExtratoBancario(FiltroExtratoBancarioVm model)
        {
            try
            {

                if (Convert.ToInt32(model.codigoEmpresa) < 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _secretaria.ListaExtratoBancario(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ListaMaioresReceitas(FiltroExtratoBancarioVm model)
        {
            try
            {

                if (Convert.ToInt32(model.codigoEmpresa) < 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _secretaria.ListaMaioresENTRADAs(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
            }

        }
        public async Task<ActionResult> ListaMaioresDespesas(FiltroExtratoBancarioVm model)
        {
            try
            {

                if (Convert.ToInt32(model.codigoEmpresa) < 0)
                    model.codigoEmpresa = _user.cod_empresa.ToString();

                var Dados = await _secretaria.ListaMaioresSAIDAs(model);

                return PartialView(Dados);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu uma falha ao realizar a pesquisa. Favor contactar o suporte técnico.";
                return PartialView(null);
            }

        }
    }
}