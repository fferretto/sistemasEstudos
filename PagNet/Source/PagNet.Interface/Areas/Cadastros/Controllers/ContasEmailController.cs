using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Helpers;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class ContasEmailController : ClientSessionControllerBase
    {
        private readonly IContaEmailApp _email;
        private readonly IPagNetUser _user;

        public ContasEmailController(IContaEmailApp email,
                                     IPagNetUser user)
        {
            _email = email;
            _user = user;
        }

        public IActionResult Index(int? id)
        {
            var vm = _email.GetContaEmailById(id, _user.cod_empresa);
            vm.acessoAdmin = _user.isAdministrator;

            return View(vm);
        }

        public async Task<ActionResult> ConsultaEmailsCadastrados(int? codEmpresa)
        {
            if (Convert.ToInt32(codEmpresa) <= 0)
                codEmpresa = _user.cod_empresa;

            var dados = await _email.GetAllContasEmail((int)codEmpresa);

            return PartialView(dados);

        }

        public async Task<JsonResult> GetTiposCriptografia()
        {

            var lista = new object[][]
             {
                new object[] { "Nenhuma", "Nenhuma"},
                new object[] { "SSL", "SSL"}
             };

            return Json(lista.ToList());
        }
        [HttpPost]
        [RequestFormSizeLimit(valueCountLimit: 9000)]
        public async Task<IActionResult> TestarConfiguracoes(ContaEmailVM model)
        {
            try
            {
                bool EmailValidado = _email.ValidaContaEmail(model);
                string msgRetorno = "";

                if (EmailValidado) msgRetorno = "Email enviado com sucesso!";
                else msgRetorno = "Falha no Envio.";

                return Json(new { success = EmailValidado, responseText = msgRetorno });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Salvar(ContaEmailVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (Convert.ToInt32(model.codEmpresa) <= 0)
                        model.codEmpresa = _user.cod_empresa.ToString();

                    model.CRIPTOGRAFIA = (model.CRIPTOGRAFIA == "-1") ? "Nenhuma" : model.CRIPTOGRAFIA;

                    var result = await _email.SalvaConta(model);

                    foreach (var item in result)
                    {
                        if (item.Key == "Sucesso")
                        {
                            TempData["Avis.Sucesso"] = item.Value.ToString();
                        }
                        else
                            TempData["Avis.Aviso"] = item.Value.ToString();
                    }

                    return RedirectToAction("Index", new { id = model.CODCONTAEMAIL });


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
        public async Task<ActionResult> Desativar(ContaEmailVM model)
        {
            try
            {
                var result = await _email.RemoveConta(model.CODCONTAEMAIL);

                foreach (var item in result)
                {
                    if (item.Key == "Sucesso")
                    {
                        TempData["Avis.Sucesso"] = item.Value.ToString();
                    }
                    else
                        TempData["Avis.Aviso"] = item.Value.ToString();
                }

                return RedirectToAction("Index", new { id = "" });
         
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return View("Index", model);
            }
        }
    }
}