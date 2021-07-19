using System;

namespace PagNet.Bld.Domain.Entities
{
    public class PROC_PAGNET_CONSULTABOLETO
    {
        public int CODEMISSAOBOLETO { get; set; }
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
        public string NUMCONTROLETAB { get; set; }
        public DateTime DATVENCIMENTO { get; set; }
        public string NOSSONUMERO { get; set; }
        public string CODOCORRENCIA { get; set; }
        public string SEUNUMERO { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DATSOLICITACAO { get; set; }
        public DateTime DATREFERENCIA { get; set; }
        public DateTime? DATSEGUNDODESCONTO { get; set; }
        public decimal? VLDESCONTO { get; set; }
        public decimal? VLSEGUNDODESCONTO { get; set; }
        public string MENSAGEMARQUIVOREMESSA { get; set; }
        public string MENSAGEMINSTRUCOESCAIXA { get; set; }
        public string NUMCONTROLE { get; set; }
        public string OCORRENCIARETBOL { get; set; }
        public string NMBOLETOGERADO { get; set; }
        public string DESCRICAOOCORRENCIARETBOL { get; set; }
        public string BOLETORECUSADO { get; set; }
        public string MSGRECUSA { get; set; }
    }
}