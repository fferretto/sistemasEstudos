using System.ComponentModel.DataAnnotations;

namespace PagNet.Application.Models
{
    public class AparenciaSistemaVm
    {
        public int CodOpe { get; set; }
        public int codEmpresa { get; set; }
        public string CaminhoDefaultPagNet { get; set; }

        [Display(Name = "Barra Superior")]
        public string Cabecaho { get; set; }

        [Display(Name = "Linha Cabeçalho")]
        public string LinhaCabecalho { get; set; }

        [Display(Name = "Título")]
        public string corTexto1 { get; set; }

        [Display(Name = "Dados do Usuários")]
        public string corTexto2 { get; set; }

        [Display(Name = "Alterar Senha")]
        public string corTexto3 { get; set; }

        [Display(Name = "Menu do Sistema")]
        public string corTexto4 { get; set; }

        [Display(Name = "Painel")]
        public string PainelSuperior { get; set; }

        [Display(Name = "Linha Painel")]
        public string LinhaPainel { get; set; }


        [Display(Name = "Botão 1")]
        public string btnSucesso { get; set; }

        [Display(Name = "Botão 2")]
        public string btnDanger { get; set; }

        [Display(Name = "Botão 3")]
        public string btnDefault { get; set; }
    }
    public class DadosLayoutVm
    {
        public string nmClasseCss { get; set; }
    }
}
