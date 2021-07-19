using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Models;
using PagNet.Infra.Data.Context;
using System.Threading.Tasks;

namespace PagNet.Interface.Helpers
{
    [ViewComponent(Name = "DadosUser")]
    public class DadosUserViewComponent : ViewComponent
    {
        IPagNetUser _user;

        public DadosUserViewComponent(IPagNetUser user)
        {
            _user = user;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            DadosUserVm user = new DadosUserVm();

            user.Login = _user.login_usu;
            user.nmUsuario = _user.nmUsuario;
            if (_user.nmUsuario.Length > 20)
            {
                user.nmUsuario = _user.nmUsuario.Substring(0,20);
            }
            user.Perfil = (_user.isAdministrator) ? "Administrador" : "Usuário";
                        
            return View("~/Views/Shared/DadosUser.cshtml", user);
        }
    }
}
