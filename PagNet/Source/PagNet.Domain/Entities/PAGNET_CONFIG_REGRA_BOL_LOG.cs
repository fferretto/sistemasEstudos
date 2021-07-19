using System;

namespace PagNet.Domain.Entities
{
    public partial class PAGNET_CONFIG_REGRA_BOL_LOG
    {
        public int CODREGRA_LOG { get; set; }
        public int CODREGRA { get; set; }
        public int CODEMPRESA { get; set; }
        public string COBRAJUROS { get; set; }
        public decimal? VLJUROSDIAATRASO { get; set; }
        public decimal? PERCJUROS { get; set; }
        public string COBRAMULTA { get; set; }
        public decimal? VLMULTADIAATRASO { get; set; }
        public decimal? PERCMULTA { get; set; }
        public int CODPRIMEIRAINSTCOBRA { get; set; }
        public int CODSEGUNDAINSTCOBRA { get; set; }
        public decimal TAXAEMISSAOBOLETO { get; set; }
        public string AGRUPARFATURAMENTOSDIA { get; set; }        
        public string ATIVO { get; set; }
        public int CODUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }
    }
}
