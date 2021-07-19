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
    public class FornecedorController : ClientSessionControllerBase
    {
        private readonly IAPIFavorecido _apiFavorecido;
        private readonly ICadastrosApp _cadastro;
        private readonly IPagNetUser _user;
        private readonly IDiversosApp _diversos;

        public FornecedorController(IAPIFavorecido apiFavorecido,
                                    ICadastrosApp cadastro,
                                    IDiversosApp diversos,
                                    IPagNetUser user)
        {
            _apiFavorecido = apiFavorecido;
            _cadastro = cadastro;
            _user = user;
            _diversos = diversos;
        }
        public async Task<ActionResult> Index(int? id, int? codigoEmpresa)
        {
            APIFavorecidoVM Favorecido = new APIFavorecidoVM();
            try
            {
                if (id == null || id == 0)
                {
                    Favorecido = new APIFavorecidoVM();
                    Favorecido.codigoEmpresa = _user.cod_empresa.ToString();
                    Favorecido.nomeEmpresa = _diversos.GetnmEmpresaByID(_user.cod_empresa);
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
        public async Task<ActionResult> ConsultaFavorecidos(int? codigoEmpresa)
        {
            try
            {
                if (Convert.ToInt32(codigoEmpresa) <= 0)
                {
                    codigoEmpresa = _user.cod_empresa;
                }

                var dados = _apiFavorecido.ConsultaTodosFavorecidosFornecedores((int)codigoEmpresa);

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
        public async Task<ActionResult> localizaLog(int codigoFavorecido)
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
        public async Task<ActionResult> Salvar(APIFavorecidoVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Convert.ToInt32(model.codigoEmpresa) <= 0)
                        model.codigoEmpresa = _user.cod_empresa.ToString();
                    
                    var result = _apiFavorecido.SalvarFavorecido(model);

                    if (result.FirstOrDefault().Key)
                    {
                        TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                        return RedirectToAction("Index", new { id = model.codigoFavorecido, codigoEmpresa = model.codigoEmpresa });
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
                TempData["Avis.Erro"] = ex.Message;
                return View("Index", model);
            }
        }

        public async Task<ActionResult> DesativaFornecedor(int codFornecedor)
        {
            try
            {
                var result = _apiFavorecido.DesativaFavorecido(codFornecedor);

                if (result.FirstOrDefault().Key)
                {
                    TempData["Avis.Sucesso"] = result.FirstOrDefault().Value;
                    return RedirectToAction("Index", new { id = codFornecedor });
                }
                else
                {
                    TempData["Avis.Erro"] = result.FirstOrDefault().Value;
                    return RedirectToAction("Index", new { id = 0 });

                }

            }
            catch (Exception ex)
            {
                TempData["Avis.Erro"] = ex.Message;
                return View("Index");
            }
        }
    }
}