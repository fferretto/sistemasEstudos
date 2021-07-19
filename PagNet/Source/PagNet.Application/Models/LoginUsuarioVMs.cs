using System;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Application.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Obrigatório informar o login.")]
        [Display(Name = "Login")]
        [StringLength(100)]
        public string Login { get; set; }


        //[MinLength(6)]
        //[MaxLength(16, ErrorMessage = "teste")]

        [Required(ErrorMessage = "Obrigatório informar a senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        [StringLength(16, ErrorMessage = "A {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Esqueceu sua Senha?")]
        public bool RememberMe { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} deve conter no mínimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "As senhas não se coincidem.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class AlterarSenhaViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Antiga")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Nova")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "As senhas não se coincidem.")]
        public string ConfirmPassword { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
    public class AutenticacaoMV
    {
        public string nmUsuario { get; set; }
        public int codUsuario { get; set; }
        public int codSubRede { get; set; }
        public string Login { get; set; }
        public string BD_Autorizador { get; set; }
        public string BD_NetCard { get; set; }

        public string nmOperadora { get; set; }
        public int codOpe { get; set; }

        public bool bBoleto { get; set; }
        public bool bPagamento { get; set; }
        public bool bAdministrador { get; set; }
        

        public DateTime Validade { get; set; }


    }
}


