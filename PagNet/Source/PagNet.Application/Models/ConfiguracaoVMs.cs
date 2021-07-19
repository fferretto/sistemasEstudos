using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;


namespace PagNet.Application.Models
{
    public class PaginaInicialVm
    {
        public bool acessoAdmin { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        public string TotalTitulosVencido { get; set; }
        public string TotalTitulosPendBaixa { get; set; }

        public string TotalFaturamentoVencido { get; set; }
        public string TotalFaturamentoPendRegistro { get; set; }
        public string TotalFaturamentoPendBaixa { get; set; }
    }
    public class AlertaNotificacaoPagRecebVM
    {
        public AlertaNotificacaoPagRecebVM()
        {
            listaAlertas = new List<ListaIAlertasVM>();
        }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        public int qtTotalAlerta { get; set; }

        public IList<ListaIAlertasVM> listaAlertas { get; set; }

    }
    public class ListaIAlertasVM
    {
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public string linkAcesso { get; set; }
    }

    public class RegraNegocioBoletoVm
    {
        public int CODREGRA { get; set; }
        public bool acessoAdmin { get; set; }
        public int CodUsuario { get; set; }
        public bool AgruparCobranca { get; set; }

        [Display(Name = "Será Cobrado Juros?")]
        public bool COBRAJUROS { get; set; }

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

        [Display(Name = "Será Cobrado Multa?")]
        public bool COBRAMULTA { get; set; }

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

        [Display(Name = "Primeira Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string CODPRIMEIRAINSTCOBRA { get; set; }
        public string NMPRIMEIRAINSTCOBRA { get; set; }

        [Display(Name = "Segunda Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string CODSEGUNDAINSTCOBRA { get; set; }
        public string NMSEGUNDAINSTCOBRA { get; set; }


        [Display(Name = "Valor da Taxa para Emissão de Boleto")]
        [Ajuda("Este valor será acrescentado no valor do boleto.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(13)]
        public string TAXAEMISSAOBOLETO { get; set; }


        [Display(Name = "Empresa")]
        public string CODEMPRESA { get; set; }
        public string NMEMPRESA { get; set; }

        public string ATIVO { get; set; }

        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }


        internal static RegraNegocioBoletoVm ToView(PAGNET_CONFIG_REGRA_BOL x)
        {
            return new RegraNegocioBoletoVm
            {
                CODREGRA = x.CODREGRA,
                CODEMPRESA = x.CODEMPRESA.ToString(),
                COBRAJUROS = (x.COBRAJUROS == "S"),
                VLJUROSDIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VLJUROSDIAATRASO ?? 0).Replace("R$", ""),
                PERCJUROS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.PERCJUROS ?? 0).Replace("R$", ""),
                COBRAMULTA = (x.COBRAMULTA == "S"),
                VLMULTADIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VLMULTADIAATRASO ?? 0).Replace("R$", ""),
                PERCMULTA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.PERCMULTA ?? 0).Replace("R$", ""),
                CODPRIMEIRAINSTCOBRA = Convert.ToString(x.CODPRIMEIRAINSTCOBRA),
                CODSEGUNDAINSTCOBRA = Convert.ToString(x.CODSEGUNDAINSTCOBRA),
                TAXAEMISSAOBOLETO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.TAXAEMISSAOBOLETO).Replace("R$", ""),
                AgruparCobranca = (x.AGRUPARFATURAMENTOSDIA == "S"),
                ATIVO = x.ATIVO,
            };
        }
    }
    public class RegraNegocioPagamentoVm
    {
        public int CODREGRA { get; set; }
        public bool acessoAdmin { get; set; }
        public int CodUsuario { get; set; }

        [Display(Name = "Será cobrada taxa de Antecipação de pagamento?")]
        public bool COBRATAXAANTECIPACAO { get; set; }

        [Display(Name = "Percentual da taxa")]
        [InputMask("##0,00", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PORCENTAGEMTAXAANTECIPACAO { get; set; }

        [Display(Name = "Valor taxa")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string VLTAXAANTECIPACAO { get; set; }

        [Display(Name = "Forma de Compensação")]
        public string TIPOFORMACOMPENSACAO { get; set; }


        public string ATIVO { get; set; }

        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }


        internal static RegraNegocioPagamentoVm ToView(PAGNET_CONFIG_REGRA_PAG x)
        {
            return new RegraNegocioPagamentoVm
            {
                CODREGRA = x.CODREGRA,
                COBRATAXAANTECIPACAO = (x.COBRATAXAANTECIPACAO == "S"),
                VLTAXAANTECIPACAO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VLTAXAANTECIPACAO ?? 0).Replace("R$", ""),
                PORCENTAGEMTAXAANTECIPACAO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.PERCTAXAANTECIPACAO ?? 0).Replace("R$", ""),
                ATIVO = x.ATIVO,
                TIPOFORMACOMPENSACAO = x.FORMACOMPENSACAO
            };
        }
    }
    public class PerfilAcessoVM
    {
        public PerfilAcessoVM()
        {
            ListaMenu = new List<ListaMenusVM>();
        }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }


        public IList<ListaMenusVM> ListaMenu { get; set; }

    }
    public class ListaMenusVM
    {
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public string linkAcesso { get; set; }
    }
    public class FiltroPlanoContasVm
    {
        public bool acessoAdmin { get; set; }
        public int CodUsuario { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
    }
    public class VisualizaListaPlanoContasVM
    {
        public VisualizaListaPlanoContasVM()
        {
            listaPlanoContas = new List<ListaPlanoContasVm>();
        }
        public IList<ListaPlanoContasVm> listaPlanoContas { get; set; }
    }
    public class ListaPlanoContasVm
    {
        public ListaPlanoContasVm()
        {
            ListaFilho = new List<ListaPlanoContasVm>();
        }
        public IList<ListaPlanoContasVm> ListaFilho { get; set; }

        public int CODPLANOCONTAS { get; set; }
        public int CODPLANOCONTAS_PAI { get; set; }
        public int CODEMPRESA { get; set; }
        public string CLASSIFICACAO { get; set; }
        public string TIPO { get; set; }
        public string NATUREZA { get; set; }
        public string DESCRICAO { get; set; }
        public string DESPESA { get; set; }
        public string UTILIZADOPAGAMENTO { get; set; }
        public string UTILIZADORECEBIMENTO { get; set; }

    }
    public class PlanoContasVm
    {
        public int CodUsuarioPlanoContas { get; set; }
        public int codigoEmpresaPlanoContas { get; set; }
        public bool TipoDespesa { get; set; }
        public bool PagamentoCentralizadora { get; set; }
        public bool RecebimentoClienteNetCard { get; set; }

        public int CODPLANOCONTAS { get; set; }
        public int CODPLANOCONTAS_PAI { get; set; }

        [Display(Name = "Código Hierarquico")]
        [StringLength(10)]
        public string Classificacao { get; set; }

        [Display(Name = "Código Hierarquico")]
        public string CodigoTipoConta { get; set; }
        public string TipoConta { get; set; }

        [Display(Name = "Natureza")]
        public string CodigoNatureza { get; set; }
        public string Natureza { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(50)]
        public string Descricao { get; set; }

        [Display(Name = "Raiz")]
        public string codigoRaiz { get; set; }
        public string nomeRaiz { get; set; }


        internal static PlanoContasVm ToView(PAGNET_CADPLANOCONTAS x, string CodRaiz, string nmRaiz)
        {
            return new PlanoContasVm
            {
                CODPLANOCONTAS = x.CODPLANOCONTAS,
                CODPLANOCONTAS_PAI = Convert.ToInt32(x.CODPLANOCONTAS_PAI),
                Classificacao = x.CLASSIFICACAO,
                CodigoTipoConta = x.TIPO,
                TipoConta = x.TIPO,
                CodigoNatureza = x.NATUREZA,
                Natureza = x.NATUREZA,
                Descricao = x.DESCRICAO,
                codigoRaiz = CodRaiz,
                nomeRaiz = nmRaiz,
                codigoEmpresaPlanoContas = (int)x.CODEMPRESA,
                TipoDespesa = (x.DESPESA == "S"),
                PagamentoCentralizadora = (x.DEFAULTPAGAMENTO == "S"),
                RecebimentoClienteNetCard = (x.DEFAULTRECEBIMENTO == "S")
            };
        }
    }
    public class ConfigTaxaVm
    {
        public int codigoTaxa { get; set; }
        public string nomeTaxa { get; set; }
    }
}
