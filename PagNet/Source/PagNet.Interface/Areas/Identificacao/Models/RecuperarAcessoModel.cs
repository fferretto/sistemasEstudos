using PagNet.Api.Service.Interface.Model;
using PagNet.Application.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Interface.Areas.Identificacao.Models
{
    public class FiltroRecuperaSenhaModel
    {
        public FiltroRecuperaSenhaModel()
        {
            MensagemRetorno = "";
            EmailLoginUsuario = "";
        }
        [Display(Name = "Email / Login")]
        [Ajuda("Informe o email cadastrado ou login de acesso ao sistema PagNet.")]
        [StringLength(100)]
        public string EmailLoginUsuario { get; set; }
        public bool Sucesso { get; set; }
        public string MensagemRetorno { get; set; }
    }
    public class RecuperaSenhaModel : IFiltroModel
    {
        public int codigoUsuario { get; set; }
        public int codigoOperadora { get; set; }
        public int codigoEmpresa { get; set; }
        public string EmailUsuario { get; set; }
        public string Login { get; set; }
    }
    public class NovaSenhaModel
    {
        public int codigoUsuario { get; set; }
        public int codigoOperadora { get; set; }
        public string EmailUsuario { get; set; }
        public string Login { get; set; }
        public string codigoEnviado { get; set; }

        [DataType(DataType.Password)]
        [StringLength(16)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        //[Required(ErrorMessage = "Obrigatório confirmar a senha")]
        [DataType(DataType.Password)]
        [StringLength(16)]
        public string ConfirmPassword { get; set; }

        [StringLength(6)]
        public string PIN { get; set; }
    }
}
