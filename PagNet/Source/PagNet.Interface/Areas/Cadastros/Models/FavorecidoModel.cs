using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using PagNet.Application.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PagNet.Interface.Areas.Cadastros.Models
{
    public class APIFavorecidoVM : IAPIFavorecidoModel
    {
        public bool acessoAdmin { get; set; }
        public int codigoFavorecido { get; set; }
        public bool regraDiferenciada { get; set; }
        public bool contaPagamentoPadrao { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Possui Regra Diferenciada?")]
        public string cobrancaDiferenciada { get; set; }

        [Required(ErrorMessage = "Obrigatório informar nome do favorito")]
        [Display(Name = "Nome")]
        [StringLength(100)]
        public string nomeFavorecido { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Código da centralizadora cadastrada no sistema NetCard")]
        [StringLength(18)]
        public string codigoCentralizadora { get; set; }

        [Display(Name = "CPF/CNPJ")]
        [Ajuda("CPF ou CNPJ")]
        [Required(ErrorMessage = "Informe o CPF ou CNPJ")]
        [StringLength(18)]
        public string CPFCNPJ { get; set; }

        [Display(Name = "Banco")]
        [StringLength(4)]
        public string Banco { get; set; }

        [Display(Name = "Agência")]
        [StringLength(5)]
        public string Agencia { get; set; }

        [Display(Name = "DV Agência")]
        [Ajuda("Preencher apenas se possuir DV")]
        [StringLength(1)]
        public string DvAgencia { get; set; }

        [Display(Name = "OPE")]
        [Ajuda("Informar a operação apenas se o banco for da caixa econômica federal")]
        [StringLength(3)]
        public string Operacao { get; set; }

        [Display(Name = "Conta Corrente")]
        [StringLength(11)]
        public string contaCorrente { get; set; }

        [Display(Name = "DV Conta")]
        [StringLength(1)]
        public string DvContaCorrente { get; set; }

        [Display(Name = "CEP")]
        [StringLength(9)]
        [InputMask("99999-999")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Logradouro")]
        [Display(Name = "Logradouro")]
        [StringLength(100)]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Número do logradouro")]
        [Display(Name = "Nº")]
        [StringLength(20)]
        public string nroLogradouro { get; set; }

        [Display(Name = "Complemento")]
        [Ajuda("Informe o complemento do local, como APTO, Loja, etc...")]
        [StringLength(60)]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Bairro")]
        [Display(Name = "Bairro")]
        [StringLength(100)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a Cidade")]
        [Display(Name = "Cidade")]
        [StringLength(100)]
        public string cidade { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o UF")]
        [Display(Name = "UF")]
        [StringLength(2)]
        public string UF { get; set; }

        [Display(Name = "Taxa TED")]
        [Ajuda("Taxa cobrada para transações de pagamento via TED")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string ValorTED { get; set; }

        [Display(Name = "Valor Mínimo para Pagamento via TED")]
        [Ajuda("O sistema irá verificar todos os pagamentos do mesmo dia, e irá efetuar o pagamento apenas quando a soma dos valores for superior ao informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string ValorMinimoTed { get; set; }

        [Display(Name = "Valor Mínimo para Credito em Conta")]
        [Ajuda("O sistema irá verificar todos os pagamentos do mesmo dia, e irá efetuar o pagamento apenas quando a soma dos valores for superior ao informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string ValorMinimoCC { get; set; }

        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codigoContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        internal static APIFavorecidoVM ToView(IAPIFavorecidoModel x)
        {
            return new APIFavorecidoVM
            {

                codigoFavorecido = x.codigoFavorecido,
                regraDiferenciada = x.regraDiferenciada,
                contaPagamentoPadrao = x.contaPagamentoPadrao,
                codigoEmpresa = x.codigoEmpresa,
                nomeEmpresa = x.nomeEmpresa,
                cobrancaDiferenciada = x.cobrancaDiferenciada,
                nomeFavorecido = x.nomeFavorecido,
                codigoCentralizadora = x.codigoCentralizadora,
                CPFCNPJ = x.CPFCNPJ,
                Banco = x.Banco,
                Agencia = x.Agencia,
                DvAgencia = x.DvAgencia,
                Operacao = x.Operacao,
                contaCorrente = x.contaCorrente,
                DvContaCorrente = x.DvContaCorrente,
                CEP = x.CEP,
                Logradouro = x.Logradouro,
                nroLogradouro = x.nroLogradouro,
                Complemento = x.Complemento,
                Bairro = x.Bairro,
                cidade = x.cidade,
                UF = x.UF,
                ValorTED = x.ValorTED,
                ValorMinimoTed = x.ValorMinimoTed,
                ValorMinimoCC = x.ValorMinimoCC,
                codigoContaCorrente = x.codigoContaCorrente,
                nmContaCorrente = x.nmContaCorrente
            };
        }
        internal static IList<APIFavorecidoVM> ToListView<T>(ICollection<T> collection) where T : APIFavorecidoModel
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static APIFavorecidoVM ToListView(APIFavorecidoModel x)
        {
            return new APIFavorecidoVM
            {
                codigoFavorecido = x.codigoFavorecido,
                regraDiferenciada = x.regraDiferenciada,
                contaPagamentoPadrao = x.contaPagamentoPadrao,
                codigoEmpresa = x.codigoEmpresa,
                nomeEmpresa = x.nomeEmpresa,
                cobrancaDiferenciada = x.cobrancaDiferenciada,
                nomeFavorecido = x.codigoFavorecido + " - " + x.nomeFavorecido,
                codigoCentralizadora = x.codigoCentralizadora,
                CPFCNPJ = x.CPFCNPJ,
                Banco = x.Banco,
                Agencia = x.Agencia,
                DvAgencia = x.DvAgencia,
                Operacao = x.Operacao,
                contaCorrente = x.contaCorrente,
                DvContaCorrente = x.DvContaCorrente,
                CEP = x.CEP,
                Logradouro = x.Logradouro,
                nroLogradouro = x.nroLogradouro,
                Complemento = x.Complemento,
                Bairro = x.Bairro,
                cidade = x.cidade,
                UF = x.UF,
                ValorTED = x.ValorTED,
                ValorMinimoTed = x.ValorMinimoTed,
                ValorMinimoCC = x.ValorMinimoCC,
                codigoContaCorrente = x.codigoContaCorrente,
                nmContaCorrente = x.nmContaCorrente

            };
        }
    }
    public class DadosLogModal
    {
        public string dataLog { get; set; }
        public string descLog { get; set; }
        public string nomeUsu { get; set; }

        internal static IList<DadosLogModal> ToListView<T>(ICollection<T> collection) where T : APIDadosLogModal
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static DadosLogModal ToListView(APIDadosLogModal x)
        {
            return new DadosLogModal
            {
                dataLog = x.dataLog,
                descLog = x.descLog,
                nomeUsu = x.nomeUsu

            };
        }

    }
}
