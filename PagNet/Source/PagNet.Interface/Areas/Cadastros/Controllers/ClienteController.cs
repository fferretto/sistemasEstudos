using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class ClienteController : ClientSessionControllerBase
    {
        private readonly IPagNetUser _user;
        private readonly IAPIClienteAPP _APICliente;

        public ClienteController(IAPIClienteAPP APICliente,
                                  IPagNetUser user)
        {
            _user = user;
            _APICliente = APICliente;
        }
        public async Task<ActionResult> Index(int? id)
        {
            APIClienteVm cliente = new APIClienteVm();
            try
            {
                if (id == null || id == 0)
                {
                    cliente = new APIClienteVm();
                }
                else
                {
                    cliente = APIClienteVm.ToView(_APICliente.RetornaDadosClienteByID((int)id));
                }
                cliente.acessoAdmin = _user.isAdministrator;

                return View(cliente);
            }
            catch (Exception ex)
            {
                cliente.acessoAdmin = _user.isAdministrator;
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View(cliente);
            }
        }

        public async Task<ActionResult> ConsultaCliente(int? codEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codEmpresa) <= 0)
                {
                    codEmpresa = _user.cod_empresa;
                }

                var dados = await _APICliente.ConsultaTodosCliente((int)codEmpresa, "J");
                var ListaCliente = APIClienteVm.ToListView(dados).ToList();

                return PartialView(ListaCliente);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }
        [HttpPost]
        public async Task<ActionResult> Salvar(APIClienteVm model)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    if (Convert.ToInt32(model.codigoFormaFaturamento) <= 0)
                        model.codigoFormaFaturamento = "1";   

                    model.CodigoTipoPessoa = "J";
                    var result = await _APICliente.SalvarCliente(APIClienteVm.ToReturn(model));

                    if (result.FirstOrDefault().Key)
                    {
                        TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                        return RedirectToAction("Index", new { id = model.CODCLIENTE });
                    }
                    else
                    {
                        TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                        return RedirectToAction("Index", new { id = 0 });

                    }

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View("Index", model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> DesativaCliente(APIClienteVm model)
        {
            try
            {

                if (model.codJustificativa != "OUTROS")
                {
                    model.DescJustOutros = model.descJustificativa;
                }

                var result = await _APICliente.DesativaCliente(model.CODCLIENTE,  model.DescJustOutros);

                if (result.FirstOrDefault().Key)
                {
                    TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                    return RedirectToAction("Index", new { id = model.CODCLIENTE });
                }
                else
                {
                    TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                    return RedirectToAction("Index", new { id = 0 });

                }

            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View("Index");
            }
        }

        public JsonResult GetJustificativaDesativacao()
        {
            var lista = new object[][]
             {
                new object[] { "NAOINFORMADO", ""},
                new object[] { "CLIENTECANCELADONETCARD", "CLIENTE FOI CANCELADO NO SISTEMA NETCARD"},
                new object[] { "CLIENTECANCELADO", "CANCELAMENTO DE CONTRATO"},
                new object[] { "OUTROS", "OUTROS"}
             };

            return Json(lista.ToList());
        }
        public ActionResult Justificativa()
        {
            JustificativaClienteVm vm = new JustificativaClienteVm();

            return PartialView(vm);

        }
    }
}