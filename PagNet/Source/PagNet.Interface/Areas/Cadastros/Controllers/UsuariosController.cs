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
    public class UsuariosController : ClientSessionControllerBase
    {
        private readonly IUsuarioApp _usuario;
        private readonly ICadastrosApp _cadastro;
        private readonly IPagNetUser _user;

        public UsuariosController(IUsuarioApp usu,
                                  ICadastrosApp cadastro,
                                  IPagNetUser user)
        {
            _usuario = usu;
            _user = user;
            _cadastro = cadastro;
        }
        public async Task<ActionResult> Index(int? id)
        {

            if (id == null)
            {
                id = _user.cod_usu;
            }

            var usuario = _usuario.GetUsuario(id);
            if (usuario.CodUsuario == 0)
            {
                usuario.CODEMPRESA = _user.cod_empresa.ToString();
            }
            else
            {
                var usuariosplit = usuario.Login.Split('@');
                usuario.Login = usuariosplit[0];
            }
            var empresa = _cadastro.RetornaDadosEmpresaByID(Convert.ToInt32(usuario.CODEMPRESA));

            usuario.idOperadora = _user.cod_ope;
            usuario.PerfilOperadora = "@" + empresa.NMLOGIN;
            usuario.acessoAdmin = _user.isAdministrator;
            usuario.CODEMPRESA = empresa.CODEMPRESA.ToString();
            usuario.NMEMPRESA = empresa.NMFANTASIA;


            return View(usuario);
        }
        public async Task<ActionResult> AlteraSenha()
        {
            var usuario = _usuario.GetUsuario(_user.cod_usu);

            return View(usuario);
        }
        public async Task<ActionResult> ConsultaUsuarios(int? codEmpresa)
        {
            if (Convert.ToInt32(codEmpresa) <= 0)
            {
                codEmpresa = _user.cod_empresa;
            }
            int codOpe = _user.cod_ope;

            var dados = await _usuario.GetAllUsuarioByCodEmpresa((int)codEmpresa, codOpe);

            return PartialView(dados);
        }
        [HttpPost]
        public async Task<JsonResult> SalvarAlteraSenha(UsuariosVm model)
        {
            try
            {

                if (Convert.ToInt32(model.CODEMPRESA) <= 0)
                    model.CODEMPRESA = _user.cod_empresa.ToString();

                if (!string.IsNullOrWhiteSpace(model.CODEMPRESA))
                {
                    var empresa = _cadastro.RetornaDadosEmpresaByID(Convert.ToInt32(model.CODEMPRESA));
                    model.PerfilOperadora = "@" + empresa.NMLOGIN;
                }

                model.CodOpe = _user.cod_ope.ToString();
                model.CodUsuario = _user.cod_usu;

                var result = await _usuario.Salvar(model);

                return Json(new { success = true, responseText = "Senha atualizada com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Ocorreu uma falha inesperada, favor contactar o suporte." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Salvar(UsuariosVm model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (Convert.ToInt32(model.CODEMPRESA) <= 0)
                        model.CODEMPRESA = _user.cod_empresa.ToString();

                    if (!string.IsNullOrWhiteSpace(model.CODEMPRESA))
                    {
                        var empresa = _cadastro.RetornaDadosEmpresaByID(Convert.ToInt32(model.CODEMPRESA));
                        model.PerfilOperadora = "@" + empresa.NMLOGIN;
                    }
                    model.Administrador = false;
                    model.CodOpe = model.idOperadora.ToString();
                    var result = await _usuario.Salvar(model);

                    if (!result.Any())
                    {
                        TempData["Avis.Sucesso"] = "Usuario salvo com sucesso!";
                        return RedirectToAction("Index", new { id = model.CodUsuario });
                    }
                    foreach (var item in result)
                    {
                        //Where do I get the key from the view model?
                        ModelState.AddModelError(item.Key, item.Value.ToString());
                        TempData["Avis.Aviso"] = item.Value.ToString();
                        return View("Index", model);
                    }
                    return View("Index", model);

                }

                return View("Index", model);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View("Index", model);
            }
        }

        public JsonResult BuscaFlagLogin(string codEmpresa)
        {
            string retorno = "";
            if (!string.IsNullOrWhiteSpace(codEmpresa))
            {
                var empresa = _cadastro.RetornaDadosEmpresaByID(Convert.ToInt32(codEmpresa));
                retorno = "@" + empresa.NMLOGIN;
            }
                       
            return Json(retorno);
        }
        [HttpPost]
        public async Task<ActionResult> Desativar(UsuariosVm model)
        {
            try
            {
                var result = await _usuario.Desativar(model.CodUsuario);

                if (!result.Any())
                {
                    TempData["Avis.Sucesso"] = "Usuário desativado com sucesso!";
                    return RedirectToAction("Index", new { id = "" });
                }
                foreach (var item in result)
                {
                    if (!ModelState.IsValid)
                    {
                        //Where do I get the key from the view model?
                        ModelState.AddModelError(item.Key, item.Value.ToString());
                        TempData["Avis.Aviso"] = item.Value.ToString();
                        return View("Index", model);
                    }
                }
                return View("Index", model);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                TempData["Avis.Erro"] = "Ocorreu um erro inesperado no sistema. Favor contactar o suporte.";
                return View("Index", model);
            }

        }
    }
}