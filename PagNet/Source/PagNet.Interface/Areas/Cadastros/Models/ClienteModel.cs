using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using PagNet.Application.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Interface.Areas.Cadastros.Models
{
    public class APIClienteVm
    {
        [Display(Name = "Código")]
        public int CODCLIENTE { get; set; }
        public bool acessoAdmin { get; set; }
        public int codUsuario { get; set; }

        public bool COBRANCADIFERENCIADA { get; set; }
        public bool COBRAJUROS { get; set; }
        public bool COBRAMULTA { get; set; }


        [Display(Name = "Utiliza Regra Própria")]
        public string REGRAPROPRIA { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o nome do cliente")]
        [StringLength(100)]
        public string NMCLIENTE { get; set; }

        [Display(Name = "Email")]
        [StringLength(100)]
        public string EMAIL { get; set; }

        [Display(Name = "CPF/CNPJ")]
        [Ajuda("CPF ou CNPJ")]
        [Required(ErrorMessage = "Informe o CPF ou CNPJ")]
        [StringLength(18)]
        public string CPFCNPJ { get; set; }

        [Display(Name = "Empresa")]
        public string CODEMPRESA { get; set; }
        public string NMEMPRESA { get; set; }

        [Display(Name = "CEP")]
        [StringLength(9)]
        [InputMask("99999-999")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Logradouro")]
        [Display(Name = "Logradouro")]
        [StringLength(100)]
        public string LOGRADOURO { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Número do logradouro")]
        [Display(Name = "Nº")]
        [StringLength(20)]
        public string NROLOGRADOURO { get; set; }

        [Display(Name = "Complemento")]
        [Ajuda("Informe o complemento do local, como APTO, Loja, etc...")]
        [StringLength(60)]
        public string COMPLEMENTO { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Bairro")]
        [Display(Name = "Bairro")]
        [StringLength(100)]
        public string BAIRRO { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a Cidade")]
        [Display(Name = "Cidade")]
        [StringLength(100)]
        public string CIDADE { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o UF")]
        [Display(Name = "UF")]
        [StringLength(2)]
        public string UF { get; set; }

        [Display(Name = "Valor Multa")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string VLMULTADIAATRASO { get; set; }

        [Display(Name = "Percentual da Multa")]
        [Ajuda("Percentual da multa a ser cobrado")]
        [InputMask("##0,00", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PERCMULTA { get; set; }

        [Display(Name = "Percentual do Juros")]
        [Ajuda("Percentual do juros a ser cobrado")]
        [InputMask("##0,00", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PERCJUROS { get; set; }

        [Display(Name = "Valor Juros")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string VLJUROSDIAATRASO { get; set; }

        [Display(Name = "Primeira Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string CODPRIMEIRAINSTCOBRA { get; set; }
        public string NMPRIMEIRAINSTCOBRA { get; set; }

        [Display(Name = "Segunda Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string CODSEGUNDAINSTCOBRA { get; set; }
        public string NMSEGUNDAINSTCOBRA { get; set; }

        [Display(Name = "Forma de Faturamento *")]
        [Ajuda("Forma de Faturamento padrão para os pedidos de faturamento recebidos via NetCard. Esta opção poderá ser alterada posteriormente.")]
        public string codigoFormaFaturamento { get; set; }
        public string nomeFormaFaturamento { get; set; }

        [Display(Name = "Tipo de Pessoa")]
        public string CodigoTipoPessoa { get; set; }
        public string NomeTipoPessoa { get; set; }

        [Display(Name = "Valor da Taxa para Emissão de Boleto")]
        [Ajuda("Este valor será acrescentado no valor do boleto.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(13)]
        public string TAXAEMISSAOBOLETO { get; set; }

        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }
        public bool AgruparFaturamentos { get; set; }


        internal static APIClienteVm ToView(IAPIClienteModel _cad)
        {
            return new APIClienteVm
            {
                CODCLIENTE = _cad.codigoCliente,
                NMCLIENTE = _cad.nomeCliente,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.cpfCnpj),
                EMAIL = _cad.email,
                CODEMPRESA = _cad.codigoEmpresa,
                CEP = Geral.FormataCEP(_cad.cep),
                LOGRADOURO = _cad.logradouro,
                NROLOGRADOURO = _cad.numeroLogradouro,
                COMPLEMENTO = _cad.complemento,
                BAIRRO = _cad.bairro,
                CIDADE = _cad.cidade,
                UF = _cad.UF,
                codigoFormaFaturamento = _cad.codigoFormaFaturamento,
                nomeFormaFaturamento = _cad.nomeFormaFaturamento,
                AgruparFaturamentos = (_cad.AgruparFaturamentos),
                REGRAPROPRIA = _cad.regraPropria,
                COBRANCADIFERENCIADA = _cad.cobrancaDiferenciada,
                COBRAJUROS = (_cad.cobraJuros),
                VLJUROSDIAATRASO = _cad.valorJurosDiaAtraso ,
                PERCJUROS =  _cad.percentualJuros,
                COBRAMULTA = (_cad.cobraMulta),
                VLMULTADIAATRASO =  _cad.valorMultaDiaAtraso,
                PERCMULTA =  _cad.percentualMulta ,
                CODPRIMEIRAINSTCOBRA = _cad.codigoPrimeiraIntrucaoCobranca,
                NMPRIMEIRAINSTCOBRA = _cad.nomePrimeiraIntrucaoCobranca,
                CODSEGUNDAINSTCOBRA = _cad.codigoSegundaIntrucaoCobranca,
                NMSEGUNDAINSTCOBRA = _cad.nomeSegundaIntrucaoCobranca,
                NMEMPRESA = _cad.nomeEmpresa,
                TAXAEMISSAOBOLETO =  _cad.taxaEmissaoBoleto
            };
        }

        internal static APIClienteModel ToReturn(APIClienteVm _cad)
        {
            return new APIClienteModel
            {
                codigoCliente = _cad.CODCLIENTE,
                nomeCliente = _cad.NMCLIENTE,
                cpfCnpj = _cad.CPFCNPJ,
                email = _cad.EMAIL,
                codigoEmpresa = _cad.CODEMPRESA,
                cep = _cad.CEP,
                logradouro = _cad.LOGRADOURO,
                numeroLogradouro = _cad.NROLOGRADOURO,
                complemento = _cad.COMPLEMENTO,
                bairro = _cad.BAIRRO,
                cidade = _cad.CIDADE,
                UF = _cad.UF,
                codigoFormaFaturamento = _cad.codigoFormaFaturamento,
                AgruparFaturamentos = (_cad.AgruparFaturamentos),
                regraPropria = _cad.REGRAPROPRIA,
                cobrancaDiferenciada = _cad.COBRANCADIFERENCIADA,
                cobraJuros = (_cad.COBRAJUROS),
                valorJurosDiaAtraso = _cad.VLJUROSDIAATRASO,
                percentualJuros = _cad.PERCJUROS,
                cobraMulta = (_cad.COBRAMULTA),
                valorMultaDiaAtraso = _cad.VLMULTADIAATRASO,
                percentualMulta = _cad.PERCMULTA,
                codigoPrimeiraIntrucaoCobranca = _cad.CODPRIMEIRAINSTCOBRA,
                nomePrimeiraIntrucaoCobranca = _cad.NMPRIMEIRAINSTCOBRA,
                codigoSegundaIntrucaoCobranca = _cad.CODSEGUNDAINSTCOBRA,
                nomeSegundaIntrucaoCobranca = _cad.NMSEGUNDAINSTCOBRA,
                nomeEmpresa = _cad.NMEMPRESA,
                taxaEmissaoBoleto = _cad.TAXAEMISSAOBOLETO
            };
        }

        internal static IList<APIClienteVm> ToListView<T>(ICollection<T> collection) where T : IAPIClienteModel
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static APIClienteVm ToListView(IAPIClienteModel _cad)
        {
            return new APIClienteVm
            {
                CODCLIENTE = _cad.codigoCliente,
                NMCLIENTE = _cad.nomeCliente,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.cpfCnpj),
                EMAIL = _cad.email,
                CODEMPRESA = _cad.codigoEmpresa,
                CEP = Geral.FormataCEP(_cad.cep),
                LOGRADOURO = _cad.logradouro,
                NROLOGRADOURO = _cad.numeroLogradouro,
                COMPLEMENTO = _cad.complemento,
                BAIRRO = _cad.bairro,
                CIDADE = _cad.cidade,
                UF = _cad.UF,
                codigoFormaFaturamento = _cad.codigoFormaFaturamento,
                AgruparFaturamentos = (_cad.AgruparFaturamentos),
                REGRAPROPRIA = _cad.regraPropria,
                COBRANCADIFERENCIADA = _cad.cobrancaDiferenciada,
                COBRAJUROS = (_cad.cobraJuros),
                VLJUROSDIAATRASO = _cad.valorJurosDiaAtraso,
                PERCJUROS = _cad.percentualJuros,
                COBRAMULTA = (_cad.cobraMulta),
                VLMULTADIAATRASO = _cad.valorMultaDiaAtraso,
                PERCMULTA = _cad.percentualMulta,
                CODPRIMEIRAINSTCOBRA = _cad.codigoPrimeiraIntrucaoCobranca,
                NMPRIMEIRAINSTCOBRA = _cad.nomePrimeiraIntrucaoCobranca,
                CODSEGUNDAINSTCOBRA = _cad.codigoSegundaIntrucaoCobranca,
                NMSEGUNDAINSTCOBRA = _cad.nomeSegundaIntrucaoCobranca,
                NMEMPRESA = _cad.nomeEmpresa,
                TAXAEMISSAOBOLETO = _cad.taxaEmissaoBoleto
            };
        }
    }

    public class JustificativaClienteVm
    {
        [Display(Name = "Justificativa")]
        [Ajuda("Informe o motivo")]
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }

        [Display(Name = "Especifique")]
        [Ajuda("Especifique o motivo da ação.")]
        [StringLength(200)]
        public string DescJustOutros { get; set; }

    }
}
