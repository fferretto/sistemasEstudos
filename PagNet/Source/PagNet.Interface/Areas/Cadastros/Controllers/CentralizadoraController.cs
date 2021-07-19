using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Model;
using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Cadastros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    [Authorize]
    public class CentralizadoraController : ClientSessionControllerBase
    {
        private readonly IAPIFavorecido _apiFavorecido;
        private readonly ICadastrosApp _cadastro;
        private readonly IPagNetUser _user;

        public CentralizadoraController(IAPIFavorecido apiFavorecido,
                                        ICadastrosApp cadastro,
                                        IPagNetUser user)
        {
            _apiFavorecido = apiFavorecido;
            _cadastro = cadastro;
            _user = user;
        }
        public ActionResult Index(int? id, int? codigoEmpresa)
        {
            APIFavorecidoVM Favorecido = new APIFavorecidoVM();
            try
            {
                if (id == null || id == 0)
                {
                    Favorecido = new APIFavorecidoVM();

                    var emp = _cadastro.RetornaDadosEmpresaByID(_user.cod_empresa);
                    Favorecido.codigoEmpresa = emp.CODEMPRESA.ToString();
                    Favorecido.nomeEmpresa = emp.NMFANTASIA;
                }
                else
                {
                    Favorecido = APIFavorecidoVM.ToView(_apiFavorecido.RetornaDadosFavorecidoByID((int)id, (int)codigoEmpresa));
                }
                Favorecido.acessoAdmin = _user.isAdministrator;

                return View(Favorecido);
            }
            catch (Exception ex)
            {
                Favorecido.acessoAdmin = _user.isAdministrator;
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View(Favorecido);
            }
        }
        public ActionResult ConsultaCentralizadora()
        {
            try
            {
                var dados = _apiFavorecido.ConsultaTodosFavorecidosCentralizadora(_user.cod_empresa);
                var ListaFavorecido = APIFavorecidoVM.ToListView(dados);


                return PartialView(ListaFavorecido);
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
        [HttpPost]
        public ActionResult Salvar(APIFavorecidoVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.codigoEmpresa = "0";
                    var result = _apiFavorecido.SalvarFavorecido(model);

                    if (result.FirstOrDefault().Key)
                            TempData["Avis.Sucesso"] = result.FirstOrDefault().Value.ToString();
                        else
                            TempData["Avis.Aviso"] = result.FirstOrDefault().Value.ToString();
     
                    return RedirectToAction("Index", new { id = model.codigoFavorecido, codigoEmpresa = model.codigoEmpresa });

                    //return Json(new { success = result.FirstOrDefault().Key, responseText = result.FirstOrDefault().Value });
                }
                return View("Index", model);


            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte." });
            }
        }

        public async Task<ActionResult> LocalizaLog(int codigoFavorecido)
        {
            try
            {
                var dados = _apiFavorecido.ConsultaLog(codigoFavorecido);
                if (dados == null)
                    dados = new List<APIDadosLogModal>();

                var dadosLog = DadosLogModal.ToListView(dados);

                return PartialView(dadosLog);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return PartialView(null);
            }

        }


        public ActionResult DesativaCentralizadora(int codigofavorecido)
        {
            try
            {
                var result = _apiFavorecido.DesativaFavorecido(codigofavorecido);

                if (result.FirstOrDefault().Key)
                {
                    TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                    return RedirectToAction("Index", new { id = codigofavorecido });
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
    }
}