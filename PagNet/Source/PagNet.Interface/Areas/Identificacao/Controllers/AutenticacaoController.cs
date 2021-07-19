using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PagNet.Application.Interface;
using PagNet.Domain.Helpers;
using PagNet.Infra.Data.Context;
using PagNet.Interface.Areas.Identificacao.Models;
using System.IO;
using System.Threading.Tasks;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Identificacao.Controllers
{

    [Area("Identificacao")]
    public class AutenticacaoController : ClientAuthenticationController<LoginUsuario>
    {
        IValidaUsuarioApp _validaUsuario;
        IPagNetUser _user;
        public AutenticacaoController(IHttpContextAccessor accessor,
                                      IValidaUsuarioApp validaUsuario,
                                      IPagNetUser user)
                    : base(accessor)
        {
            _validaUsuario = validaUsuario;
            _user = user;
        }

        protected override Task<AuthenticationResult> Authenticate(string username, string password)
        {            
            password = Help.CriptografarSenha(password);
            var retorno = base.Authenticate(username, password);

            return retorno;
        }
        //public override Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPassword)
        //{
        //    if (forgotPassword.EmailLoginUsuario.IndexOf(".com") > 0)
        //    {
        //        forgotPassword.Email = forgotPassword.EmailLoginUsuario;
        //    }
        //    else
        //    {
        //        forgotPassword.Username = forgotPassword.EmailLoginUsuario;
        //    }

        //    return base.ForgotPassword(forgotPassword);
        //}
        public IActionResult GetClassCss()
        {
            var CaminhoDefault = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var CaminhoClasse = _validaUsuario.getCaminhoCss(_user.cod_ope, _user.nmPerfilOperadora, CaminhoDefault, _user.cod_empresa);
            return File(System.IO.File.ReadAllBytes(CaminhoClasse), "text/css");
        }
        public IActionResult GetImagemLogo()
        {
            var CaminhoDefault = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var CaminhoClasse = _validaUsuario.getCaminhoLogo(_user.cod_ope, _user.nmPerfilOperadora, CaminhoDefault, _user.cod_empresa);

            string extensaoArquivo = CaminhoClasse.Substring(CaminhoClasse.Length - 3, 3);
            return File(System.IO.File.ReadAllBytes(CaminhoClasse), "image/" + extensaoArquivo);
        }
        public IActionResult GetImagIco()
        {
            var CaminhoDefault = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var CaminhoClasse = _validaUsuario.getCaminhoIco(_user.cod_ope, _user.nmPerfilOperadora, CaminhoDefault, _user.cod_empresa);

            return File(System.IO.File.ReadAllBytes(CaminhoClasse), "image/ico");
        }        
    }
}