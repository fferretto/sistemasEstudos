using System.Collections.Generic;

namespace PagNet.Bld.Domain.Entities
{
    public partial class PAGNET_CONTACORRENTE
    {
        public PAGNET_CONTACORRENTE()
        {
            PAGNET_CONFIGVAN = new List<PAGNET_TRANSMISSAOARQUIVO>();
            PAGNET_BORDERO_PAGAMENTO = new List<PAGNET_BORDERO_PAGAMENTO>();
            PAGNET_TITULOS_PAGOS = new List<PAGNET_TITULOS_PAGOS>();
            PAGNET_EMISSAOBOLETO = new List<PAGNET_EMISSAOBOLETO>();
            PAGNET_EMISSAO_TITULOS = new List<PAGNET_EMISSAO_TITULOS>();
            PAGNET_EMISSAOFATURAMENTO = new List<PAGNET_EMISSAOFATURAMENTO>();
            PAGNET_CONTACORRENTE_SALDO = new List<PAGNET_CONTACORRENTE_SALDO>();
        }
        public int CODCONTACORRENTE { get; set; }
        public string NMCONTACORRENTE { get; set; }
        public string NMEMPRESA { get; set; }
        public string CPFCNPJ { get; set; }
        public string CODBANCO { get; set; }
        public int CODEMPRESA { get; set; }
        public string NROCONTACORRENTE { get; set; }
        public string DIGITOCC { get; set; }
        public string CONTAMOVIEMNTO { get; set; }
        public string AGENCIA { get; set; }
        public string DIGITOAGENCIA { get; set; }
        public string CODCONVENIOPAG { get; set; }
        public string CODTRANSMISSAO { get; set; }
        public string CARTEIRAREMESSA { get; set; }
        public string ATIVO { get; set; }
        public string CODOPERACAOCC { get; set; }
        public decimal? VALTED { get; set; }
        public decimal? VALMINIMOCC { get; set; }
        public decimal? VALMINIMOTED { get; set; }
        public string COBRAJUROS { get; set; }
        public decimal? VLJUROSDIAATRASO { get; set; }
        public decimal? PERCJUROS { get; set; }
        public string COBRAMULTA { get; set; }
        public decimal? VLMULTADIAATRASO { get; set; }
        public decimal? PERCMULTA { get; set; }
        public int? CODPRIMEIRAINSTCOBRA { get; set; }
        public int? CODSEGUNDAINSTCOBRA { get; set; }
        public decimal? TAXAEMISSAOBOLETO { get; set; }
        public string AGRUPARFATURAMENTOSDIA { get; set; }
        public int? QTPOSICAOARQPGTO { get; set; }
        public int? QTPOSICAOARQBOL { get; set; }
        public string CODCEDENTE { get; set; }
        public string DVCODCEDENTE { get; set; }
        public string VARIACAOCARTEIRA { get; set; }


        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual ICollection<PAGNET_CONTACORRENTE_SALDO> PAGNET_CONTACORRENTE_SALDO { get; set; }
        public virtual ICollection<PAGNET_TRANSMISSAOARQUIVO> PAGNET_CONFIGVAN { get; set; }
        public virtual ICollection<PAGNET_BORDERO_PAGAMENTO> PAGNET_BORDERO_PAGAMENTO { get; set; }
        public virtual ICollection<PAGNET_TITULOS_PAGOS> PAGNET_TITULOS_PAGOS { get; set; }
        public virtual ICollection<PAGNET_EMISSAOBOLETO> PAGNET_EMISSAOBOLETO { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }

    }
}


