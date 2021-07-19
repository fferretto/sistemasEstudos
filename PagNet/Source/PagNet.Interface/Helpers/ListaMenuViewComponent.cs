using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Infra.Data.Context;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PagNet.Interface.Helpers
{
    [ViewComponent(Name = "ListaMenu")]
    public class ListaMenuViewComponent : ViewComponent
    {
        IValidaUsuarioApp _validaUsuario;
        IPagNetUser _user;

        public ListaMenuViewComponent(IValidaUsuarioApp validaUsuario,
                                      IPagNetUser user)
        {
            _validaUsuario = validaUsuario;
            _user = user;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
            var menu = _validaUsuario.CarregaMenus(_user.cod_ope, _user.nmPerfilOperadora, Caminho, _user.PossuiNetCard, _user.login_usu);

            menu[0].Login = RemoveCaracterDireita(_user.login_usu, 21);
            menu[0].nmUsuario = RemoveCaracterDireita(_user.nmUsuario, 21);
            menu[0].Perfil = (_user.isAdministrator) ? "Administrador" : "Usuário";



            return View("~/Views/Shared/Master.cshtml", menu);
        }

        public static string RemoveCaracterDireita(string valor, int qt)
        {
            try
            {
                if (valor == null) return "";

                string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");

                if (retorno.Length > qt)
                {
                    retorno = retorno.Substring(0, qt);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
