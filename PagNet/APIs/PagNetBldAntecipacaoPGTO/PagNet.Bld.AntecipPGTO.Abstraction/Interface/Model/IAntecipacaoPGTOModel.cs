using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model
{
    public interface IAntecipacaoPGTOModel
    {
        int CodigoTitulo { get; set; }
        int CodigoFavorecido { get; set; }
        string NomeFavorecido { get; set; }
        string CNPJ { get; set; }
        DateTime DataEmissao { get; set; }
        DateTime DataRealPGTO { get; set; }
        string CodigoBanco { get; set; }
        string Agencia { get; set; }
        string ContaCorrente { get; set; }
        decimal ValorAtual { get; set; }
        string ValorTaxa { get; set; }
        decimal ValorTaxaProRata { get; set; }
        decimal ValorTotalComTaxa { get; set; }
        string TipoCartao { get; set; }
        int CodigoEmpresa { get; set; }
    }

    public class AntecipacaoPGTOModel : IAntecipacaoPGTOModel
    {
        public int CodigoTitulo { get; set; }
        public int CodigoFavorecido { get; set; }
        public string NomeFavorecido { get; set; }
        public string CNPJ { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataRealPGTO { get; set; }
        public string CodigoBanco { get; set; }
        public string Agencia { get; set; }
        public string ContaCorrente { get; set; }
        public decimal ValorAtual { get; set; }
        public string ValorTaxa { get; set; }
        public decimal ValorTaxaProRata { get; set; }
        public decimal ValorTotalComTaxa { get; set; }
        public string TipoCartao { get; set; }
        public int CodigoEmpresa { get; set; }
    }
}
