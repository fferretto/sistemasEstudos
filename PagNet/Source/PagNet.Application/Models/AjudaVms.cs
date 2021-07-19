using PagNet.Application.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PagNet.Application.Models
{
    public class ContatoViaEmailVM
    {
        public bool acessoAdmin { get; set; }
        public string CaminhoArquivoPadrao { get; set; }

        [Display(Name = "Arquivos Anexados")]
        public IList<ListaAnexosVm> Anexo { get; set; }

        [Display(Name = "Seu Nome")]
        [Ajuda("Informe o seu nome completo para que possamos te identificar")]
        [Required(ErrorMessage = "Obrigatório informar o seu nome")]
        [StringLength(60)]
        public string nmSolicitante { get; set; }

        [Display(Name = "Nome da Sua Empresa")]
        [Ajuda("Informe o nome da sua empresa")]
        [Required(ErrorMessage = "Obrigatório informar o nome da sua empresa")]
        [StringLength(60)]
        public string nmEmpresaSolicitante { get; set; }

        [Display(Name = "Seu E-mail")]
        [Ajuda("Informe um email para contato")]
        [Required(ErrorMessage = "Obrigatório informar um Email para contato")]
        [StringLength(60)]
        public string EmailSolicitente { get; set; }

        [Display(Name = "Telefone para contato")]
        [Ajuda("Informe um telefone para contato")]
        [InputMask("(99)99999-9999")]
        [StringLength(15)]
        public string TelefoneSolicitante { get; set; }

        [Display(Name = "Assunto")]
        [Ajuda("Informe o assunto desta mensagem. ")]
        [StringLength(100)]
        public string Assunto { get; set; }

        [Display(Name = "Mensagem")]
        [Required(ErrorMessage = "Nenhuma mensagem informada")]
        [StringLength(800)]
        public string Mensagem { get; set; }
    }
    public class ListaAnexosVm
    {
        public string NovoNomeArquivo { get; set; }
        public string nomeArquivo { get; set; }
        public int Status { get; set; }
    }
    public class TreinamentoVm
    {
        public string TipoTreinamento { get; set; }
        public string Titulo { get; set; }
        public string TextoCabecalho { get; set; }
        public string NomedoVideo { get; set; }
    }
}