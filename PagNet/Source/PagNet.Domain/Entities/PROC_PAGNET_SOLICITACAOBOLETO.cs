using System;

namespace PagNet.Domain.Entities
{
    public class PROC_PAGNET_SOLICITACAOBOLETO
    {
        public int CODEMISSAOFATURAMENTO { get; set; }
        public string STATUS { get; set; }
        public int CODCLIENTE { get; set; }
        public string NMCLIENTE { get; set; }
        public string CPFCNPJ { get; set; }
        public string EMAIL { get; set; }
        public string COBRANCADIFERENCIADA { get; set; }
        public string COBRAJUROS { get; set; }
        public decimal? VLJUROSDIAATRASO { get; set; }
        public decimal? PERCJUROS { get; set; }
        public string COBRAMULTA { get; set; }
        public decimal? VLMULTADIAATRASO { get; set; }
        public decimal? PERCMULTA { get; set; }
        public string NOMEPRIMEIRAINSTCOBRA { get; set; }
        public string NOMESEGUNDAINSTCOBRA { get; set; }
        public decimal? TAXAEMISSAOBOLETO { get; set; }
        public string ORIGEM { get; set; }
        public string TIPOFATURAMENTO { get; set; }
        public DateTime DATVENCIMENTO { get; set; }
        public DateTime? DATPGTO { get; set; }
        public decimal? VLPGTO { get; set; }
        public string SEUNUMERO { get; set; }
        public string NRODOCUMENTO { get; set; }        
        public decimal VALOR { get; set; }
        public DateTime DATSOLICITACAO { get; set; }
        public DateTime? DATSEGUNDODESCONTO { get; set; }
        public decimal? VLDESCONTO { get; set; }
        public decimal? VLSEGUNDODESCONTO { get; set; }
        public string MENSAGEMARQUIVOREMESSA { get; set; }
        public string MENSAGEMINSTRUCOESCAIXA { get; set; }

        public int CODEMISSAOFATURAMENTOPAI { get; set; }
        public int PARCELA { get; set; }
        public int TOTALPARCELA { get; set; }
        public decimal VALORPARCELA { get; set; }
    }
}       
