using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Application.Models
{
    public class InstrucaoEmailVm
    {
        public int CODINSTRUCAOEMAIL { get; set; }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Assunto")]
        [Ajuda("Informe o assunto que será utilizado para identificar o E-mail")]
        [Required(ErrorMessage = "Obrigatório informar um assunto para este E-mail")]
        public string ASSUNTO { get; set; }

        [Display(Name = "Mensagem")]
        [Ajuda("Informe a mensagem que será utilizado como texto do E-mail")]
        [Required(ErrorMessage = "Obrigatório informar um texto para este E-mail")]
        public string MENSAGEM { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }

        internal static InstrucaoEmailVm ToView(PAGNET_INSTRUCAOEMAIL _email)
        {
            return new InstrucaoEmailVm
            {
                CODINSTRUCAOEMAIL = _email.CODINSTRUCAOEMAIL,
                ASSUNTO = _email.ASSUNTO,
                MENSAGEM = _email.MENSAGEM,
                codEmpresa = _email.CODEMPRESA.ToString()
            };
        }

        internal static PAGNET_INSTRUCAOEMAIL ToEntity(InstrucaoEmailVm _email)
        {
            return new PAGNET_INSTRUCAOEMAIL
            {
                ASSUNTO = _email.ASSUNTO.Trim(),
                MENSAGEM = _email.MENSAGEM.Trim(),
                CODEMPRESA = Convert.ToInt32(_email.codEmpresa)
            };
        }
    }
}
