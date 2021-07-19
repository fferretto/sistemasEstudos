using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace PagNet.Application.Models
{
    public class SubRedeVM
    {
        public int CODSUBREDE { get; set; }
        public string NOMSUBREDE { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string CNPJ { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string UF { get; set; }
        public string CIDADE { get; set; }

        internal static SubRedeVM ToView(SUBREDE _sub)
        {
            return new SubRedeVM
            {
                CODSUBREDE = _sub.CODSUBREDE,
                NOMSUBREDE = _sub.NOMSUBREDE ?? "",
                RAZAOSOCIAL = _sub.RAZAOSOCIAL ?? "",
                CNPJ = Geral.FormataCPFCnPj(_sub.CNPJ),
                CEP = Geral.FormataCEP(_sub.CEP),
                LOGRADOURO = _sub.LOGRADOURO ?? "",
                NROLOGRADOURO = _sub.NROLOGRADOURO ?? "",
                COMPLEMENTO = _sub.COMPLEMENTO ?? "",
                BAIRRO = _sub.BAIRRO ?? "",
                CIDADE = _sub.CIDADE ?? "",
                UF = _sub.UF ?? ""
            };
        }
    }
    public class CadEmpresaVm
    {
        public int CODEMPRESA { get; set; }
        public bool acessoAdmin { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a Razão Social")]
        [Display(Name = "Razão Social")]
        [Ajuda("Razão social da empresa")]
        [StringLength(100)]
        public string RAZAOSOCIAL { get; set; }
        
        [Required(ErrorMessage = "Obrigatório informar o Nome Fantasia")]
        [Display(Name = "Nome Fantasia")]
        [Ajuda("Nome Fantasia da empresa")]
        [StringLength(100)]
        public string NMFANTASIA { get; set; }

        [Display(Name = "CNPJ")]
        [Ajuda("CNPJ da empresa")]
        [Required(ErrorMessage = "Informe o CNPJ da Empresa")]
        [StringLength(18)]
        [InputMask("99.999.999/9999-99")]
        public string CNPJ { get; set; }

        [Display(Name = "CEP")]
        [Ajuda("CEP da empresa")]
        [Required(ErrorMessage = "Informe o CEP da Empresa")]
        [StringLength(9)]
        [InputMask("99999-999")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Logradouro")]
        [Display(Name = "Logradouro")]
        [Ajuda("Informe o Logradouro sem o número")]
        [StringLength(100)]
        public string LOGRADOURO { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Número do logradouro")]
        [Display(Name = "Nº")]
        [Ajuda("Número do local onde a empresa está localizada")]
        [StringLength(20)]
        public string NROLOGRADOURO { get; set; }

        [Display(Name = "Complemento")]
        [Ajuda("Informe o complemento do local, como APTO, Loja, etc...")]
        [StringLength(60)]
        public string COMPLEMENTO { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o Bairro")]
        [Display(Name = "Bairro")]
        [Ajuda("Bairro da Empresa")]
        [StringLength(100)]
        public string BAIRRO { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a Cidade")]
        [Display(Name = "Cidade")]
        [Ajuda("Cidade onde está localizada a empresa")]
        [StringLength(100)]
        public string CIDADE { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o UF")]
        [Display(Name = "UF")]
        [Ajuda("Localização da empresa")]
        [StringLength(2)]
        public string UF { get; set; }
        
        [Required(ErrorMessage = "Obrigatório informar o Flag Login")]
        [Display(Name = "Flag Login")]
        [Ajuda("Flag utilizado para configurar o login do usuário.")]
        [StringLength(20)]
        public string NMLOGIN { get; set; }        

        [Display(Name = "Utiliza NetCard?")]
        [Ajuda("Informe caso esta empresa é uma operadora ou subrede no sistema NetCard")]
        public bool UTILIZANETCARD { get; set; }
        public string EmpresaNetCard { get; set; }

        [Display(Name = "Informe a SubRede")]
        [Ajuda("SubRede Cadastrada no sistema NetCard")]
        public string CODSUBREDE { get; set; }
        public string NMSUBREDE { get; set; }

        internal static CadEmpresaVm ToView(PAGNET_CADEMPRESA _cad)
        {
            return new CadEmpresaVm
            {
                CODEMPRESA = _cad.CODEMPRESA,
                RAZAOSOCIAL = _cad.RAZAOSOCIAL,
                NMFANTASIA = _cad.NMFANTASIA,
                CNPJ = Geral.FormataCPFCnPj(_cad.CNPJ),
                CEP = Geral.FormataCEP(_cad.CEP),
                LOGRADOURO = _cad.LOGRADOURO,
                NROLOGRADOURO = _cad.NROLOGRADOURO,
                COMPLEMENTO = _cad.COMPLEMENTO,
                BAIRRO = _cad.BAIRRO,
                CIDADE = _cad.CIDADE,
                NMLOGIN = _cad.NMLOGIN,
                UF = _cad.UF,
                UTILIZANETCARD = _cad.UTILIZANETCARD == "S",
                CODSUBREDE = (_cad.UTILIZANETCARD == "S") ? Convert.ToString(_cad.CODSUBREDE) : ""
            };
        }

        internal static PAGNET_CADEMPRESA ToEntity(CadEmpresaVm _cad)
        {
            return new PAGNET_CADEMPRESA
            {
                RAZAOSOCIAL = _cad.RAZAOSOCIAL,
                NMFANTASIA = _cad.NMFANTASIA,
                CNPJ = _cad.CNPJ,
                CEP = Geral.RemoveCaracteres(_cad.CEP),
                LOGRADOURO = _cad.LOGRADOURO ?? "",
                NROLOGRADOURO = _cad.NROLOGRADOURO ?? "",
                COMPLEMENTO = _cad.COMPLEMENTO ?? "",
                BAIRRO = _cad.BAIRRO ?? "",
                CIDADE = _cad.CIDADE ?? "",
                NMLOGIN = _cad.NMLOGIN ?? "",
                UF = _cad.UF ?? "",
                UTILIZANETCARD = (_cad.UTILIZANETCARD)? "S": "N",
                CODSUBREDE = Convert.ToInt32(_cad.CODSUBREDE),
            };
        }
        internal static IList<CadEmpresaVm> ToListView<T>(ICollection<T> collection) where T : PAGNET_CADEMPRESA
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static CadEmpresaVm ToListView(PAGNET_CADEMPRESA x)
        {
            return new CadEmpresaVm
            {
                CODEMPRESA = x.CODEMPRESA,
                RAZAOSOCIAL = x.RAZAOSOCIAL,
                NMFANTASIA = x.NMFANTASIA,
                CNPJ = Geral.FormataCPFCnPj(x.CNPJ),
                CEP = Geral.FormataCEP(x.CEP),
                LOGRADOURO = x.LOGRADOURO,
                NROLOGRADOURO = x.NROLOGRADOURO,
                COMPLEMENTO = x.COMPLEMENTO,
                BAIRRO = x.BAIRRO,
                CIDADE = x.CIDADE,
                NMLOGIN = x.NMLOGIN,
                UF = x.UF,
                UTILIZANETCARD = (x.UTILIZANETCARD == "S"),
                EmpresaNetCard = (x.UTILIZANETCARD == "S") ? "Sim" : "Não",
                CODSUBREDE = x.CODSUBREDE.ToString(),
            };
        }

    }
    public class CadFavorecidosVm
    {
        public int CODFAVORECIDO { get; set; }
        public int codUsuario { get; set; }
        public bool acessoAdmin { get; set; }

        public bool REGRADIFERENCIADA { get; set; }
        public bool CONTAPAGAMENTOPADRAO { get; set; }
        
        [Display(Name = "Empresa")]
        public string CODEMPRESA { get; set; }
        public string NMEMPRESA { get; set; }

        [Display(Name = "Possui Regra Diferenciada?")]
        public string strCobrancaDiferenciada { get; set; }

        [Required(ErrorMessage = "Obrigatório informar nome do favorito")]
        [Display(Name = "Nome")]
        [StringLength(100)]
        public string NMFAVORECIDO { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Código da centralizadora cadastrada no sistema NetCard")]
        [StringLength(18)]
        public string CODCEN { get; set; }

        [Display(Name = "CPF/CNPJ")]
        [Ajuda("CPF ou CNPJ")]
        [Required(ErrorMessage = "Informe o CPF ou CNPJ")]
        [StringLength(18)]
        public string CPFCNPJ { get; set; }

        [Display(Name = "Banco")]
        [StringLength(4)]
        public string BANCO { get; set; }

        [Display(Name = "Agência")]
        [StringLength(5)]
        public string AGENCIA { get; set; }

        [Display(Name = "DV Agência")]
        [Ajuda("Preencher apenas se possuir DV")]
        [StringLength(1)]
        public string DVAGENCIA { get; set; }

        [Display(Name = "OPE")]
        [Ajuda("Informar a operação apenas se o banco for da caixa econômica federal")]
        [StringLength(3)]
        public string OPERACAO { get; set; }

        [Display(Name = "Conta Corrente")]
        [StringLength(11)]
        public string CONTACORRENTE { get; set; }

        [Display(Name = "DV Conta")]
        [StringLength(1)]
        public string DVCONTACORRENTE { get; set; }

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
        
        [Display(Name = "Taxa TED")]
        [Ajuda("Taxa cobrada para transações de pagamento via TED")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string VALTED { get; set; }

        [Display(Name = "Valor Mínimo para Pagamento via TED")]
        [Ajuda("O sistema irá verificar todos os pagamentos do mesmo dia, e irá efetuar o pagamento apenas quando a soma dos valores for superior ao informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string VALMINIMOTED { get; set; }

        [Display(Name = "Valor Mínimo para Credito em Conta")]
        [Ajuda("O sistema irá verificar todos os pagamentos do mesmo dia, e irá efetuar o pagamento apenas quando a soma dos valores for superior ao informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string VALMINIMOCC { get; set; }
                   
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }


        internal static CadFavorecidosVm ToView(PAGNET_CADFAVORECIDO _cad)
        {
            return new CadFavorecidosVm
            {
                CODFAVORECIDO = _cad.CODFAVORECIDO,
                CODCEN = Convert.ToString(_cad.CODCEN),
                NMFAVORECIDO = _cad.NMFAVORECIDO,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.CPFCNPJ),
                BANCO = _cad.BANCO,
                AGENCIA = _cad.AGENCIA,
                DVAGENCIA = _cad.DVAGENCIA,
                OPERACAO = _cad.OPE,
                CONTACORRENTE = _cad.CONTACORRENTE,
                DVCONTACORRENTE = _cad.DVCONTACORRENTE,
                CEP = Geral.FormataCEP(_cad.CEP),
                LOGRADOURO = _cad.LOGRADOURO,
                NROLOGRADOURO = _cad.NROLOGRADOURO,
                COMPLEMENTO = _cad.COMPLEMENTO,
                BAIRRO = _cad.BAIRRO,
                CIDADE = _cad.CIDADE,
                UF = _cad.UF,
                CODEMPRESA = Convert.ToString(_cad.CODEMPRESA)
            };
        }

        internal static IList<CadFavorecidosVm> ToListView<T>(ICollection<T> collection) where T : PAGNET_CADFAVORECIDO
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static CadFavorecidosVm ToListView(PAGNET_CADFAVORECIDO _cad)
        {
            return new CadFavorecidosVm
            {
                CODFAVORECIDO = _cad.CODFAVORECIDO,
                NMFAVORECIDO = _cad.CODFAVORECIDO + " - " +_cad.NMFAVORECIDO,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.CPFCNPJ),
                BANCO = _cad.BANCO,
                AGENCIA = _cad.AGENCIA,
                DVAGENCIA = _cad.DVAGENCIA,
                OPERACAO = _cad.OPE,
                CONTACORRENTE = _cad.CONTACORRENTE,
                DVCONTACORRENTE = _cad.DVCONTACORRENTE,
                CEP = Geral.FormataCEP(_cad.CEP),
                LOGRADOURO = _cad.LOGRADOURO,
                NROLOGRADOURO = _cad.NROLOGRADOURO,
                COMPLEMENTO = _cad.COMPLEMENTO,
                BAIRRO = _cad.BAIRRO,
                CIDADE = _cad.CIDADE,
                UF = _cad.UF

            };
        }
    }
    public class CadClienteVm
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


        internal static CadClienteVm ToView(PAGNET_CADCLIENTE _cad)
        {
            return new CadClienteVm
            {
                CODCLIENTE = _cad.CODCLIENTE,
                NMCLIENTE = _cad.NMCLIENTE,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.CPFCNPJ),
                EMAIL = _cad.EMAIL,
                CODEMPRESA = _cad.CODEMPRESA.ToString(),
                CEP = Geral.FormataCEP(_cad.CEP),
                LOGRADOURO = _cad.LOGRADOURO,
                NROLOGRADOURO = _cad.NROLOGRADOURO,
                COMPLEMENTO = _cad.COMPLEMENTO,
                BAIRRO = _cad.BAIRRO,
                CIDADE = _cad.CIDADE,
                UF = _cad.UF,
                codigoFormaFaturamento = _cad.CODFORMAFATURAMENTO.ToString(),
                AgruparFaturamentos = (_cad.AGRUPARFATURAMENTOSDIA == "S"),
                REGRAPROPRIA = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO",
                COBRANCADIFERENCIADA = (_cad.COBRANCADIFERENCIADA == "S"),
                COBRAJUROS = (_cad.COBRAJUROS == "S"),
                VLJUROSDIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", ""),
                PERCJUROS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", ""),
                COBRAMULTA = (_cad.COBRAMULTA == "S"),
                VLMULTADIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", ""),
                PERCMULTA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", ""),
                CODPRIMEIRAINSTCOBRA = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA),
                CODSEGUNDAINSTCOBRA = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA),
                TAXAEMISSAOBOLETO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "")
            };
        }

        internal static IList<CadClienteVm> ToListView<T>(ICollection<T> collection) where T : PAGNET_CADCLIENTE
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static CadClienteVm ToListView(PAGNET_CADCLIENTE _cad)
        {
            return new CadClienteVm
            {
                CODCLIENTE = _cad.CODCLIENTE,
                NMCLIENTE = _cad.NMCLIENTE,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.CPFCNPJ),
                EMAIL = _cad.EMAIL,
                CODEMPRESA = _cad.CODEMPRESA.ToString(),
                CEP = _cad.CEP,
                LOGRADOURO = _cad.LOGRADOURO,
                NROLOGRADOURO = _cad.NROLOGRADOURO,
                COMPLEMENTO = _cad.COMPLEMENTO,
                BAIRRO = _cad.BAIRRO,
                CIDADE = _cad.CIDADE,
                UF = _cad.UF,
                REGRAPROPRIA = (_cad.COBRANCADIFERENCIADA == "S") ? "SIM" : "NÃO",
                COBRANCADIFERENCIADA = (_cad.COBRANCADIFERENCIADA == "S"),
                COBRAJUROS = (_cad.COBRAJUROS == "S"),
                VLJUROSDIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLJUROSDIAATRASO ?? 0).Replace("R$", ""),
                PERCJUROS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCJUROS ?? 0).Replace("R$", ""),
                COBRAMULTA = (_cad.COBRAMULTA == "S"),
                VLMULTADIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.VLMULTADIAATRASO ?? 0).Replace("R$", ""),
                PERCMULTA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.PERCMULTA ?? 0).Replace("R$", ""),
                CODPRIMEIRAINSTCOBRA = Convert.ToString(_cad.CODPRIMEIRAINSTCOBRA),
                CODSEGUNDAINSTCOBRA = Convert.ToString(_cad.CODSEGUNDAINSTCOBRA),
                TAXAEMISSAOBOLETO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _cad.TAXAEMISSAOBOLETO ?? 0).Replace("R$", "")

            };
        }
    }
}
