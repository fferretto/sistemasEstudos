using PagNet.Application.Helpers;
using System.ComponentModel.DataAnnotations;
using Telenet.AspNetCore.Mvc.Authentication;

namespace PagNet.Interface.Areas.Identificacao.Models
{
    //
    // Summary:
    //     Classe de encapsulamento dos dados de login de um usuário.
    public class LoginUsuario : IUserLogin
    {
        //
        // Summary:
        //     Obtém ou define o nome do usuário.
        [Required(ErrorMessage = "Obrigatório informar o Login")]
        public string Username { get; set; }

        //
        // Summary:
        //     Obtém ou define a senha do usuário.
        [Required(ErrorMessage = "Obrigatório informar a senha")]
        public string Password { get; set; }

        //
        // Summary:
        //     Obtém ou define o path para redirecionamento após login.
        public string ReturnUrl { get; set; }

        bool IUserLogin.KeepConnected => true;
    }
    public class ForgotPasswordModel //: IForgotPassword
    {
        public string Email { get; set; }
        public string Username { get; set; }

        [Display(Name = "Email / Login")]
        [Ajuda("Informe o email cadastrado ou login de acesso ao sistema PagNet.")]
        [StringLength(100)]
        public string EmailLoginUsuario { get; set; }

        //short IForgotPassword.CodigoOperadora => 0;
        //string IForgotPassword.ChangePasswordUrl => "https://localhost:44365/Identificacao/Autenticacao/ChangePassword/";
    }
    public class ChangePassword //: IChangePassword
    {
        //short IChangePassword.CodigoOperadora => 0;

        [DataType(DataType.Password)]
        [StringLength(16)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        //[Required(ErrorMessage = "Obrigatório confirmar a senha")]
        [DataType(DataType.Password)]
        [StringLength(16)]
        public string ConfirmPassword { get; set; }

               
        public string NewPassword { get; set; }

        [StringLength(6)]
        public int Pin { get; set; }

        public string Token { get; set; }
    }

}
