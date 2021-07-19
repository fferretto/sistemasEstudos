using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class InstrucaoEmailController : ClientSessionControllerBase
    {
        private readonly IInstrucaoEmailApp _email;
        private readonly IPagNetUser _user;

        public InstrucaoEmailController(IInstrucaoEmailApp email,
                                     IPagNetUser user)
        {
            _email = email;
            _user = user;
        }

        public IActionResult Index(int? id)
        {
            var vm = _email.GetInstrucaoEmailById(id, _user.cod_empresa);
            vm.acessoAdmin = _user.isAdministrator;
                        
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Salvar(InstrucaoEmailVm model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Convert.ToInt32(model.codEmpresa) <= 0)
                        model.codEmpresa = _user.cod_empresa.ToString();

                    var result = await _email.SalvaInstrucao(model);

                    foreach (var item in result)
                    {
                        if (item.Key == "Sucesso")
                        {
                            TempData["Avis.Sucesso"] = item.Value.ToString();
                        }
                        else
                            TempData["Avis.Aviso"] = item.Value.ToString();
                    }

                    return RedirectToAction("Index", new { id = model.CODINSTRUCAOEMAIL });

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
