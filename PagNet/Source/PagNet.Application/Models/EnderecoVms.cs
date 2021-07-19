using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PagNet.Application.Models
{
    public class EnderecoVM
    {
        public EnderecoVM()
        {

        }
        [Key]
        public int EnderecoVMId { get; set; }

        [Required(ErrorMessage = "Digite o número")]
        [DisplayName("Número")]
        [StringLength(10)]
        public string Numero { get; set; }

        [DisplayName("Complemento")]
        [StringLength(40)]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "Digite o nome do Logradouro")]
        [DisplayName("Endereço")]
        [StringLength(100)]
        public string Endereco { get; set; }

        [DisplayName("CEP")]
        [InputMask("99999-999")]
        [RegularExpression(@"^(\d{5}-\d{3})?$", ErrorMessage = "CEP inválido")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Estado")]
        [Display(Name = "UF")]
        [StringLength(6)]
        public string LocalidadeUfDescricao { get; set; }

        [Required(ErrorMessage = "Bairro")]
        [DisplayName("Bairro")]
        public string LocalidadeBairroDescricao { get; set; }

        [Required(ErrorMessage = "Município")]
        [Display(Name = "Município")]
        [StringLength(50)]
        public string LocalidadeMunicipioDescricao { get; set; }

        internal static EnderecoVM ToView(MW_CONSCEP endereco, string CEP)
        {
            return new EnderecoVM()
            {
                Cep = Geral.FormataCEP(CEP),

                LocalidadeUfDescricao = Convert.ToString(endereco.UF),
                LocalidadeMunicipioDescricao = Convert.ToString(endereco.LOCALIDADE),
                Endereco = Convert.ToString(endereco.LOGRADOURO),
                LocalidadeBairroDescricao = Convert.ToString(endereco.BAIRRO),

            };
        }

        internal static EnderecoVM ToViewAPI(APIClienteVM Api)
        {
            return new EnderecoVM()
            {
                Cep = Api.CEP,

                LocalidadeUfDescricao = Api.UF.Trim(),
                LocalidadeMunicipioDescricao = Api.Cidade.Trim(),
                Endereco = Api.LogradouroEndereco.Trim(),
                LocalidadeBairroDescricao = Api.Bairro.Trim(),
                Numero = Api.LogradouroNumero.ToString().Trim(),
                Complemento = Api.LogradouroComplemento.Trim()

            };
        }
    }
}
