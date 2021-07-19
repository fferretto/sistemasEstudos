using System;

namespace PagNet.Domain.Entities
{
    public class PAGNET_EMISSAOFATURAMENTO
    {
        public int CODEMISSAOFATURAMENTO { get; set; }
        public string STATUS { get; set; }
        public int? CODCLIENTE { get; set; }
        public int? CODBORDERO { get; set; }
        public int CODEMPRESA { get; set; }
        public int CODFORMAFATURAMENTO { get; set; }
        public DateTime DATVENCIMENTO { get; set; }
        public string ORIGEM { get; set; }
        public string TIPOFATURAMENTO { get; set; }
        public string NROREF_NETCARD { get; set; }
        public string SEUNUMERO { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DATSOLICITACAO { get; set; }
        public DateTime? DATSEGUNDODESCONTO { get; set; }
        public decimal? VLDESCONTO { get; set; }
        public decimal? VLSEGUNDODESCONTO { get; set; }
        public string MENSAGEMARQUIVOREMESSA { get; set; }
        public string MENSAGEMINSTRUCOESCAIXA { get; set; }
        public string NRODOCUMENTO { get; set; }
        public int? CODPLANOCONTAS { get; set; }

        public DateTime? DATPGTO { get; set; }
        public decimal? VLPGTO { get; set; }
        public decimal? VLDESCONTOCONCEDIDO { get; set; }
        public decimal? JUROSCOBRADO { get; set; }
        public decimal? MULTACOBRADA { get; set; }

        public int CODEMISSAOFATURAMENTOPAI { get; set; }
        public int PARCELA { get; set; }
        public int TOTALPARCELA { get; set; }
        public decimal VALORPARCELA { get; set; }
        public int? CODCONTACORRENTE { get; set; }


        public virtual PAGNET_BORDERO_BOLETO PAGNET_BORDERO_BOLETO { get; set; }
        public virtual PAGNET_CADCLIENTE PAGNET_CADCLIENTE { get; set; }
        public virtual PAGNET_FORMAS_FATURAMENTO PAGNET_FORMAS_FATURAMENTO { get; set; }
        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }
        public virtual PAGNET_CADPLANOCONTAS PAGNET_CADPLANOCONTAS { get; set; }
    }
    public class PAGNET_EMISSAOFATURAMENTO_LOG
    {
        public int CODEMISSAOFATURAMENTO_LOG { get; set; }
        public int CODEMISSAOFATURAMENTO { get; set; }
        public string STATUS { get; set; }
        public int CODCLIENTE { get; set; }
        public int? CODBORDERO { get; set; }
        public int CODEMPRESA { get; set; }
        public int CODFORMAFATURAMENTO { get; set; }
        public DateTime DATVENCIMENTO { get; set; }
        public string ORIGEM { get; set; }
        public string TIPOFATURAMENTO { get; set; }
        public string NROREF_NETCARD { get; set; }
        public string SEUNUMERO { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DATSOLICITACAO { get; set; }
        public DateTime? DATSEGUNDODESCONTO { get; set; }
        public decimal? VLDESCONTO { get; set; }
        public decimal? VLSEGUNDODESCONTO { get; set; }
        public string MENSAGEMARQUIVOREMESSA { get; set; }
        public string MENSAGEMINSTRUCOESCAIXA { get; set; }
        public int CODUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }
        public string NRODOCUMENTO { get; set; }

        public DateTime? DATPGTO { get; set; }
        public decimal? VLPGTO { get; set; }
        public decimal? VLDESCONTOCONCEDIDO { get; set; }
        public decimal? JUROSCOBRADO { get; set; }
        public decimal? MULTACOBRADA { get; set; }
        public int? CODPLANOCONTAS { get; set; }

        public int CODEMISSAOFATURAMENTOPAI { get; set; }
        public int PARCELA { get; set; }
        public int TOTALPARCELA { get; set; }
        public decimal VALORPARCELA { get; set; }
        public int? CODCONTACORRENTE { get; set; }


        public virtual USUARIO_NETCARD USUARIO_NETCARD { get; set; }

    }
}