using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Application.Models
{
    public class ContaEmailVM
    {
        public int CODCONTAEMAIL { get; set; }
        public int CODUSUARIO { get; set; }
        public int CODEMISSAOBOLETO { get; set; }
        public string ATIVO { get; set; }
        public string EMAILPARA { get; set; }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Nome")]
        [Ajuda("Informe um nome que será utilizado para identificar o  E-mail")]
        [Required(ErrorMessage = "Obrigatório informar um nome para este  E-mail")]
        public string NMCONTAEMAIL { get; set; }

        [Display(Name = "Endereço de E-mail")]
        [Required(ErrorMessage = "Obrigatório informar um Email")]
        public string EMAIL { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string SENHA { get; set; }

        [Display(Name = "Confirma Senha")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("SENHA", ErrorMessage = "As senhas não se coincidem.")]
        public string CONFIRMASENHA { get; set; }

        [Display(Name = "Servidor de envio (SMTP)")]
        [Required(ErrorMessage = "Obrigatório informar o Servidor de envio (SMTP)")]
        public string SERVIDOR { get; set; }

        [Display(Name = "Porta")]
        [Required(ErrorMessage = "Obrigatório informar a Porta")]
        public string PORTA { get; set; }

        [Display(Name = "Criptografia")]
        public string CRIPTOGRAFIA { get; set; }
        public string NMCRIPTOGRAFIA { get; set; }

        [Display(Name = "E-mail Principal?")]
        public bool EMAILPRINCIPAL { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }


        internal static ContaEmailVM ToView(PAGNET_CONTAEMAIL _email)
        {
            return new ContaEmailVM
            {
                CODCONTAEMAIL = _email.CODCONTAEMAIL,
                ATIVO = _email.ATIVO,
                NMCONTAEMAIL = _email.NMCONTAEMAIL,
                EMAIL = _email.EMAIL,
                SERVIDOR = _email.SERVIDOR,
                PORTA = _email.PORTA,
                CRIPTOGRAFIA = _email.CRIPTOGRAFIA,
                NMCRIPTOGRAFIA = _email.CRIPTOGRAFIA,
                EMAILPRINCIPAL = (_email.EMAILPRINCIPAL == "S"),
                codEmpresa = _email.CODEMPRESA.ToString()
            };
        }

        internal static PAGNET_CONTAEMAIL ToEntity(ContaEmailVM _email)
        {
            return new PAGNET_CONTAEMAIL
            {
                NMCONTAEMAIL = _email.NMCONTAEMAIL.Trim(),
                EMAIL = _email.EMAIL.Trim(),
                SENHA = _email.SENHA.Trim(),
                SERVIDOR = _email.SERVIDOR.Trim(),
                ENDERECOSMTP = _email.SERVIDOR.Trim(),
                PORTA = _email.PORTA.Trim(),
                CRIPTOGRAFIA = _email.CRIPTOGRAFIA,
                EMAILPRINCIPAL = (_email.EMAILPRINCIPAL) ? "S" : "N",
                ATIVO = "S",
                CODEMPRESA = Convert.ToInt32(_email.codEmpresa)

            };
        }

    }
    public class ConsultaContaEmailVM
    {
        public ConsultaContaEmailVM(PAGNET_CONTAEMAIL _email)
        {
            CODCONTAEMAIL = _email.CODCONTAEMAIL;
            NMCONTAEMAIL = _email.NMCONTAEMAIL;
            EMAIL = _email.EMAIL;
            SERVIDOR = _email.SERVIDOR;
            PORTA = _email.PORTA;
            CRIPTOGRAFIA = _email.CRIPTOGRAFIA;
            EMAILPRINCIPAL = (_email.EMAILPRINCIPAL == "S");
        }

        public int CODCONTAEMAIL { get; set; }

        [Display(Name = "Nome")]
        public string NMCONTAEMAIL { get; set; }

        [Display(Name = "Endereço de E-mail")]
        public string EMAIL { get; set; }


        [Display(Name = "Servidor de envio (SMTP)")]
        public string SERVIDOR { get; set; }

        [Display(Name = "Porta")]
        public string PORTA { get; set; }

        [Display(Name = "Criptografia")]
        public string CRIPTOGRAFIA { get; set; }

        [Display(Name = "E-mail Principal?")]
        public bool EMAILPRINCIPAL { get; set; }
    }
    public class DadosEnvioEmailVm
    {
        public string ServidorSMTP { get; set; }
        public int Porta { get; set; }
        public string Criptografia { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }

        public string Para { get; set; }
        public string CC { get; set; }
        public string CorpoEmail { get; set; }
        public string TituloEmail { get; set; }
        public string CaminhoArquivoAnexo { get; set; }

        public int CODUSUARIO { get; set; }
        public int CODCONTAEMAIL { get; set; }
        public int CODEMISSAOBOLETO { get; set; }


        internal static DadosEnvioEmailVm ToEmailTeste(ContaEmailVM _email)
        {
            return new DadosEnvioEmailVm
            {
                ServidorSMTP = _email.SERVIDOR,
                Porta = Convert.ToInt32(_email.PORTA),
                Criptografia = _email.CRIPTOGRAFIA,
                Usuario = _email.EMAIL,
                Senha = _email.SENHA,
                Para = _email.EMAILPARA,
                CorpoEmail = "Este é um email enviado automaticamente pelo PagNet durante o teste das configurações da sua conta. ",
                TituloEmail = "Email de Teste PagNet"
            };
        }
        internal static PAGNET_LOGEMAILENVIADO ToLogEmail(DadosEnvioEmailVm email)
        {
            return new PAGNET_LOGEMAILENVIADO
            {
                CODUSUARIO = email.CODUSUARIO,
                CODCONTAEMAIL = email.CODCONTAEMAIL,
                CODEMISSAOBOLETO = email.CODEMISSAOBOLETO,
                EMAILDESTINO = email.Para,
                DTENVIO = DateTime.Now,
                STATUS = "ENVIADO",
                MENSAGEM = email.CorpoEmail
            };
        }

    }
}
