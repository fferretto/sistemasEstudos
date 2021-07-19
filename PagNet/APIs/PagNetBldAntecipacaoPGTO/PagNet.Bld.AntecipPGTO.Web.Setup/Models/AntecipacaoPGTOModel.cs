using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.AntecipPGTO.Web.Setup.Models
{
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
