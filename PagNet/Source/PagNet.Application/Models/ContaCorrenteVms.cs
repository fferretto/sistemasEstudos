using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PagNet.Application.Models
{
    public class Country
    {
        public int Country_id { get; set; }
        public string Description { get; set; }
    }
    public class ContaCorrenteVm
    {
        public int codContaCorrente { get; set; }
        public bool bBoleto { get; set; }
        public bool bPagamento { get; set; }
        public bool bTransmitirArqAuto { get; set; }
        public bool acessoAdmin { get; set; }


        [Display(Name = "Nome da Conta Corrente")]
        [Required(ErrorMessage = "Informe o Nome da Conta Corrente")]
        [Ajuda("Nome utilizado para identificar a conta corrente no sistema.")]
        [StringLength(80)]
        public string nmContaCorrente { get; set; }

        [Display(Name = "Razão Social da Empresa")]
        [Required(ErrorMessage = "Obrigatório informar a Razão Social da Empresa.")]
        [Ajuda("Razão Social da empresa dona da conta.")]
        [StringLength(150)]
        public string nmEmpresa { get; set; }

        [Display(Name = "CNPJ")]
        [Ajuda("CNPJ da empresa")]
        [Required(ErrorMessage = "Informe o CNPJ da Empresa")]
        [StringLength(18)]
        [InputMask("99.999.999/9999-99")]
        public string CpfCnpj { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresaPagNet { get; set; }

        [Display(Name = "Banco")]
        [Required(ErrorMessage = "Obrigatório informar o banco")]
        [Ajuda("Selecione o banco desta conta corrente")]
        public string CodBanco { get; set; }
        public string nmBanco { get; set; }

        [Display(Name = "Operação")]
        [StringLength(3)]
        public string codOperacaoCC { get; set; }

        [Display(Name = "Nº Conta Corrente")]
        [Required(ErrorMessage = "Informe o Nº Conta Corrente")]
        [StringLength(10)]
        public string nroContaCorrente { get; set; }

        [Display(Name = "Dígito")]
        [Required(ErrorMessage = "Informe o Dígito da Conta Corrente")]
        [Ajuda("Dígito da conta corrente")]
        [StringLength(1)]
        public string DigitoCC { get; set; }

        [Display(Name = "Nº da Agência")]
        [Required(ErrorMessage = "Informe o Nº da Agência")]
        [StringLength(6)]
        public string Agencia { get; set; }

        [Display(Name = "Dígito")]
        [Required(ErrorMessage = "Informe o Dígito da Agência")]
        [Ajuda("Dígito da agência")]
        [StringLength(1)]
        public string DigitoAgencia { get; set; }

        [Display(Name = "Convênio do Boleto")]
        [RequiredIf("bBoleto", true, "Informe o Convênio do Boleto")]
        [Ajuda("Informação concedida pelo banco.")]
        [StringLength(20)]
        public string CodConvenioBol { get; set; }

        [Display(Name = "Código de Transmissão 240")]
        [Ajuda("Informação concedida pelo banco.")]
        [StringLength(40)]
        public string CodTrasmissao240 { get; set; }

        [Display(Name = "Código de Transmissão 400")]
        [Ajuda("Informação concedida pelo banco.")]
        [StringLength(40)]
        public string CodTransmissao400 { get; set; }

        [Display(Name = "Nº da Carteira para o Arquivo Remessa")]
        [RequiredIf("bBoleto", true, "Informe o Nº da Carteira Remessa")]
        [Ajuda("Informação concedida pelo banco.")]
        [StringLength(9)]
        public string CarteiraRemessa { get; set; }

        [Display(Name = "Nº da Carteira para ser Incluida no Boleto")]
        [RequiredIf("bBoleto", true, "Informe o Nº da Carteira Boleto")]
        [Ajuda("Informação concedida pelo banco.")]
        [StringLength(9)]
        public string CarteiraBol { get; set; }

        [Display(Name = "Convênio para Pagamento")]
        [Ajuda("Informação concedida pelo banco (Banco Itaú não possui esta informação).")]
        [StringLength(20)]
        public string CodConvenioPag { get; set; }

        [Display(Name = "Parâmetro de Transmissão")]
        [Ajuda("Informação concedida pelo banco Caixa Econômica Feredal.")]
        [StringLength(2)]
        public string ParametroTransmissaoPag { get; set; }

        [Display(Name = "Valor TED")]
        [Ajuda("Este valor será abatido no valor total da transferência caso a forma de pagamento utilizada seja TED")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string valTED { get; set; }

        [Display(Name = "Valor Mínimo para Pagamento via CC")]
        [Ajuda("Somatória de todo valor a ser pago via CRÉDITO EM CONTA para o favorecido no dia deverá ser igual ou superior ao valor informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValMinPGTO { get; set; }

        [Display(Name = "Valor Mínimo para Pagamento via TED")]
        [Ajuda("Somatória de todo valor a ser pago via TED para o favorecido no dia deverá ser igual ou superior ao valor informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValMinTED { get; set; }

        [Display(Name = "Saldo em Conta")]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string SaldoConta { get; set; }


        public static ContaCorrenteVm ToView(PAGNET_CONTACORRENTE conta, PAGNET_BANCO banco)
        {
            return new ContaCorrenteVm()
            {
                codContaCorrente = conta.CODCONTACORRENTE,
                nmContaCorrente = conta.NMCONTACORRENTE,
                CodBanco = conta.CODBANCO,
                nmBanco = banco.NMBANCO,
                nmEmpresa = conta.NMEMPRESA,
                CpfCnpj = Geral.FormataCPFCnPj(conta.CPFCNPJ),
                nroContaCorrente = conta.NROCONTACORRENTE,
                DigitoCC = conta.DIGITOCC,
                Agencia = conta.AGENCIA,
                DigitoAgencia = conta.DIGITOAGENCIA,
                codOperacaoCC = Convert.ToString(conta.CODOPERACAOCC),
                CarteiraRemessa = Convert.ToString(conta.CARTEIRAREMESSA),
                CodConvenioPag = conta.CODCONVENIOPAG,
                valTED = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VALTED).Replace("R$", ""),
                ValMinPGTO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VALMINIMOCC).Replace("R$", ""),
                ValMinTED = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", conta.VALMINIMOTED).Replace("R$", ""),
                bPagamento = (!string.IsNullOrEmpty(conta.CODCONVENIOPAG))
            };
        }

        internal static PAGNET_CONTACORRENTE ToEntity(ContaCorrenteVm dados)
        {
            return new PAGNET_CONTACORRENTE
            {
                CODCONTACORRENTE = dados.codContaCorrente,
                NMCONTACORRENTE = dados.nmContaCorrente.Trim(),
                NMEMPRESA = dados.nmEmpresa.Trim(),
                CPFCNPJ = Geral.RemoveCaracteres(dados.CpfCnpj),
                CODBANCO = dados.CodBanco.ToString(),
                NROCONTACORRENTE = dados.nroContaCorrente.Trim(),
                DIGITOCC = dados.DigitoCC.Trim(),
                CODOPERACAOCC = Convert.ToString(dados.codOperacaoCC),
                CODEMPRESA = Convert.ToInt32(dados.codEmpresa),
                CONTAMOVIEMNTO = "",
                AGENCIA = dados.Agencia.Trim(),
                DIGITOAGENCIA = dados.DigitoAgencia.Trim(),
                CODCONVENIOPAG = Convert.ToString(dados.CodConvenioPag).Trim(),
                CARTEIRAREMESSA = Convert.ToString(dados.CarteiraRemessa),
                ATIVO = "S",
                VALTED = Geral.TrataDecimal(dados.valTED),
                VALMINIMOCC = Geral.TrataDecimal(dados.ValMinPGTO),
                VALMINIMOTED = Geral.TrataDecimal(dados.ValMinTED)
            };
        }
    }
    public class DadosHomologarContaCorrenteVm
    {
        public int codigoEmpresa { get; set; }
        public int codOPE { get; set; }
        public int CodigoContaCorrente { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFavorecido { get; set; }
        public int codigoCliente { get; set; }
        public string TipoArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool ExisteArqRemessaBol { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código do Fornecedor/Centralizadora")]
        public string filtroFavorecido { get; set; }

        [Display(Name = "Favorecido")]
        public string nomeFavorecido { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código do Cliente")]
        public string filtroCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }
    }

    public class ConsultaContaCorrenteVM
    {
        public ConsultaContaCorrenteVM(PAGNET_CONTACORRENTE x)
        {
            codContaCorrente = x.CODCONTACORRENTE;
            nmContaCorrente = x.NMCONTACORRENTE.Trim();
            nroContaCorrente = x.NROCONTACORRENTE.Trim() + "-" + x.DIGITOCC.Trim();
            Agencia = x.AGENCIA.Trim() + "-" + x.DIGITOAGENCIA.Trim();
        }

        public int codContaCorrente { get; set; }

        [Display(Name = "Nome da Conta Corrente")]
        public string nmContaCorrente { get; set; }

        [Display(Name = "Conta Corrente")]
        public string nroContaCorrente { get; set; }

        [Display(Name = "Agência")]
        public string Agencia { get; set; }

    }
    public class BancoVM
    {
        public string CODBANCO { get; set; }

        [Display(Name = "Nome do Banco")]
        public string NMBANCO { get; set; }

        public static BancoVM ToView(PAGNET_BANCO banco)
        {
            return new BancoVM()
            {
                CODBANCO = banco.CODBANCO,
                NMBANCO = banco.NMBANCO.Trim()
            };
        }

    }

}
