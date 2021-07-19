using PagNet.Api.Service.Helpers;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace PagNet.Api.Service.Model
{
    public class APIFiltroAntecipacaoVM : IFiltroAntecipacaoPGTOModel
    {
        public int? codigoEmpresa { get; set; }
        public int? codigoFavorecido { get; set; }
        public DateTime DatRealPGTO { get; set; }
        public DateTime NovaDataPGTO { get; set; }
        public int codigoTitulo { get; set; }
    }
    public class APIAntecipacaoPGTOResult
    {
        [Display(Name = "Código")]
        public int codigoTitulo { get; set; }

        [Display(Name = "Código")]
        public int codigoFavorecido { get; set; }

        [Display(Name = "Favorecido")]
        public string nomeFavorecido { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "Data Emissao")]
        public string strDataEmissao { get; set; }
        public DateTime dataEmissao { get; set; }

        [Display(Name = "Data Real PGTO")]
        public string strDataRealPGTO { get; set; }
        public DateTime dataRealPGTO { get; set; }

        [Display(Name = "Banco")]
        public string codigoBanco { get; set; }

        [Display(Name = "Agência")]
        public string agencia { get; set; }

        [Display(Name = "Conta")]
        public string contaCorrente { get; set; }

        [Display(Name = "Valor")]
        public string strValorAtual { get; set; }
        public decimal valorAtual { get; set; }

        [Display(Name = "Taxa Cobrada")]
        public string valorTaxa { get; set; }

        [Display(Name = "Valor Taxa")]
        public string strValorTaxaProRata { get; set; }
        public decimal valorTaxaProRata { get; set; }

        [Display(Name = "Valor Total")]
        public string strValorTotalComTaxa { get; set; }
        public decimal valorTotalComTaxa { get; set; }

        [Display(Name = "Tipo de Cartão")]
        public string tipoCartao { get; set; }

        [Display(Name = "Codigo da Empresa")]
        public int codigoEmpresa { get; set; }

        internal static APIAntecipacaoPGTOResult ToView(AntecipacaoPGTOModel x)
        {
            return new APIAntecipacaoPGTOResult
            {
                strDataEmissao = x.DataEmissao.ToShortDateString(),
                strDataRealPGTO = x.DataRealPGTO.ToShortDateString(),
                strValorAtual = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.ValorAtual),
                strValorTaxaProRata = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.ValorTaxaProRata),
                strValorTotalComTaxa = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.ValorTotalComTaxa),
                codigoTitulo = x.CodigoTitulo,
                codigoFavorecido = x.CodigoFavorecido,
                nomeFavorecido = x.NomeFavorecido,
                cnpj = Geral.FormataCPFCnPj(x.CNPJ),
                dataEmissao = x.DataEmissao,
                dataRealPGTO = x.DataRealPGTO,
                codigoBanco = x.CodigoBanco,
                agencia = x.Agencia,
                contaCorrente = x.ContaCorrente,
                valorAtual = x.ValorAtual,
                valorTaxa = x.ValorTaxa,
                valorTaxaProRata = x.ValorTaxaProRata,
                valorTotalComTaxa = x.ValorTotalComTaxa,
                tipoCartao = x.TipoCartao,
                codigoEmpresa = x.CodigoEmpresa

            };
        }
        internal static IList<APIAntecipacaoPGTOResult> ToListView<T>(ICollection<T> collection) where T : AntecipacaoPGTOModel
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static APIAntecipacaoPGTOResult ToListView(AntecipacaoPGTOModel x)
        {
            return new APIAntecipacaoPGTOResult
            {
                strDataEmissao = x.DataEmissao.ToShortDateString(),
                strDataRealPGTO = x.DataRealPGTO.ToShortDateString(),
                strValorAtual = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.ValorAtual),
                strValorTaxaProRata = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.ValorTaxaProRata),
                strValorTotalComTaxa = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.ValorTotalComTaxa),
                codigoTitulo = x.CodigoTitulo,
                codigoFavorecido = x.CodigoFavorecido,
                nomeFavorecido = x.NomeFavorecido,
                cnpj = Geral.FormataCPFCnPj(x.CNPJ),
                dataEmissao = x.DataEmissao,
                dataRealPGTO = x.DataRealPGTO,
                codigoBanco = x.CodigoBanco,
                agencia = x.Agencia,
                contaCorrente = x.ContaCorrente,
                valorAtual = x.ValorAtual,
                valorTaxa = x.ValorTaxa,
                valorTaxaProRata = x.ValorTaxaProRata,
                valorTotalComTaxa = x.ValorTotalComTaxa,
                tipoCartao = x.TipoCartao,
                codigoEmpresa = x.CodigoEmpresa
            };
        }
    }
    public class APIRegraNegocioPGTOResult
    {
        public int CODREGRA { get; set; }
        public bool acessoAdmin { get; set; }
        public int CodUsuario { get; set; }
        public bool COBRATAXAANTECIPACAO { get; set; }
        public string PORCENTAGEMTAXAANTECIPACAO { get; set; }
        public string VLTAXAANTECIPACAO { get; set; }
        public string TIPOFORMACOMPENSACAO { get; set; }
        public string ATIVO { get; set; }
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }
    }
}