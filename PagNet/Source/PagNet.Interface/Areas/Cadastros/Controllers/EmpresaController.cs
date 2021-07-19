using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class EmpresaController : ClientSessionControllerBase
    {
        private readonly ICadastrosApp _cadastro;
        private readonly IPagNetUser _user;

        public EmpresaController(ICadastrosApp cadastro,
                                  IPagNetUser user)
        {
            _cadastro = cadastro;
            _user = user;
        }
        public async Task<ActionResult> Index(int? id)
        {
            CadEmpresaVm emp = new CadEmpresaVm();
            try
            {
                if (id == null || id == 0)
                {
                    emp = new CadEmpresaVm();
                }
                else
                {
                    emp = _cadastro.RetornaDadosEmpresaByID((int)id);
                }
                emp.acessoAdmin = _user.isAdministrator;

                return View(emp);
            }
            catch (Exception ex)
            {
                emp.acessoAdmin = _user.isAdministrator;
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View(emp);
            }
        }
        public async Task<ActionResult> ConsultaEmpresas()
        {
            try
            {
                var dados = await _cadastro.ConsultaTodasEmpresas();

                return PartialView(dados);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }
        
        }
        public async Task<JsonResult> BuscaEndereco(string cpf)
        {
            EnderecoVM endereco = new EnderecoVM(); 

            if (!string.IsNullOrWhiteSpace(cpf))
            {
                endereco = await _cadastro.RetornaEndereco(cpf);
            }
            
            return Json(endereco);
        }
        public JsonResult BuscaDadosSubRede(string id)
        {
            SubRedeVM subRede = new SubRedeVM();
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    subRede = _cadastro.RetornaDadosSubRede(Convert.ToInt32(id));
                }

                return Json(subRede);

            }
            catch (Exception ex)
            {
                return Json(subRede);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Salvar(CadEmpresaVm model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _cadastro.SalvarEmpresa(model);

                    if (result.FirstOrDefault().Key)
                    { 
                        TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                        return RedirectToAction("Index", new { id = model.CODEMPRESA });
                    }
                    else
                    {
                        TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                        return RedirectToAction("Index", new { id = 0});

                    }

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