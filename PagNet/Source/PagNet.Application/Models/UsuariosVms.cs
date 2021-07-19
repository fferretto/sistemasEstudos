using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using PagNet.Domain.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Application.Models
{
    public class UsuariosVm
    {
        public UsuariosVm()
        {
           
        }
        public string Btn { get; set; }
        public int CodUsuario { get; set; }
        public int idOperadora { get; set; }
        public bool acessoAdmin { get; set; }
        public string PerfilOperadora { get; set; }

        [Display(Name = "Nome do Usuário")]
        [Required(ErrorMessage = "Informe o Nome do Usuário")]
        [Ajuda("Nome utilizado para identificar a conta corrente no sistema")]
        [StringLength(50)]
        public string nmUsuario { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login é Obrigatório")]
        [Ajuda("Nome utilizado para realizar o acesso ao sistema. Máximo 15 caracteres")]
        [StringLength(15)]
        [InputAttrAux(Final = "@")]
        public string Login { get; set; }        

        [Display(Name = "Usuário Administrador?")]
        [Ajuda("Usuário administrador terá permissão total no sistema.")]
        public bool Administrador { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF é Obrigatório")]
        [StringLength(14)]
        [InputMask("999.999.999-99")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [Display(Name = "E-mail", Description = "exemplo@suaempresa.com.br")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [StringLength(50)]
        public string Email { get; set; }
                
        [Display(Name = "Operadora")]
        public string CodOpe { get; set; }
        public string nomeOperadora { get; set; }
        
        [Display(Name = "Empresa")]
        public string CODEMPRESA { get; set; }
        public string NMEMPRESA { get; set; }

        

        [StringLength(16, ErrorMessage = "A senha deverá conter entre 6 a 16 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Senha *")]
        //[Required(ErrorMessage = "Obrigatório informar a senha")]
        [Ajuda("A senha deverá conter entre 6 a 16 caracteres. Este campo é obrigatório apenas para novos cadastros")]
        public string Password { get; set; }

        [Display(Name = "Confirmar Senha *")]
        [Ajuda("A senha deverá conter entre 6 a 16 caracteres. Este campo é obrigatório apenas para novos cadastros")]
        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        //[Required(ErrorMessage = "Obrigatório confirmar a senha")]
        [DataType(DataType.Password)]
        [StringLength(16)]
        public string ConfirmPassword { get; set; }

        internal static UsuariosVm ToView(USUARIO_NETCARD _usu, string NMEMPRESA)
        {
            var auxLogin = _usu.LOGIN.Split('@');
            return new UsuariosVm()
            {
                CodUsuario = _usu.CODUSUARIO,
                nmUsuario = _usu.NMUSUARIO,
                Login = _usu.LOGIN,
                Cpf = Geral.FormataCPFCnPj(_usu.CPF),
                Email = _usu.EMAIL.Trim(),
                Administrador = (_usu.ADMINISTRADOR == "S") ? true : false,
                CODEMPRESA = Convert.ToString(_usu.CODEMPRESA),
                Password = "",
                ConfirmPassword = ""
            };
        }
        internal static PAGNET_USUARIO ToEntity_PAGNET(UsuariosVm dados)
        {
            return new PAGNET_USUARIO
            {
                CODUSUARIO = dados.CodUsuario,
                NMUSUARIO = dados.nmUsuario,
                LOGIN = dados.Login + dados.PerfilOperadora,
                SENHA = Help.CriptografarSenha(dados.Password),
                CPF = dados.Cpf,
                CODEMPRESA = Convert.ToInt32(dados.CODEMPRESA),
                EMAIL = dados.Email,
                CODOPE = Convert.ToInt16(dados.CodOpe),
                ADMINISTRADOR = (dados.Administrador) ? "S" : "N",
                VISIVEL = "S",
                ATIVO = "S"
            };
        }
        internal static USUARIO_NETCARD ToEntity_NetCard(PAGNET_USUARIO dados)
        {
            return new USUARIO_NETCARD
            {
                CODUSUARIO = dados.CODUSUARIO,
                NMUSUARIO = dados.NMUSUARIO,
                LOGIN = dados.LOGIN,
                SENHA = dados.SENHA,
                CPF = dados.CPF,
                CODEMPRESA = Convert.ToInt32(dados.CODEMPRESA),
                EMAIL = dados.EMAIL,
                ADMINISTRADOR = dados.ADMINISTRADOR,
                VISIVEL = "S",
                ATIVO = "S"
            };
        }

    }
    public class ConsultaUsuario
    {
        public ConsultaUsuario(PAGNET_USUARIO x)
        {
            CodUsuario = x.CODUSUARIO;
            nmusuario = x.NMUSUARIO.Trim();
            login = x.LOGIN.Trim();
            Email = x.EMAIL.Trim();
        }

        public int CodUsuario { get; set; }

        [Display(Name = "Nome do Usuário")]
        public string nmusuario { get; set; }

        [Display(Name = "Login")]
        public string login { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

    }


}
