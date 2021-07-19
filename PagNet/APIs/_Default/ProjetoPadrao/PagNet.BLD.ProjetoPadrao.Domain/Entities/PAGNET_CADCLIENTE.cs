using System;
using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_CADCLIENTE
    {
        public PAGNET_CADCLIENTE()
        {
            this.PAGNET_EMISSAOBOLETO = new List<PAGNET_EMISSAOBOLETO>();
            this.PAGNET_EMISSAOFATURAMENTO = new List<PAGNET_EMISSAOFATURAMENTO>();
            this.PAGNET_ARQUIVO_CONCILIACAO = new List<PAGNET_ARQUIVO_CONCILIACAO>();
        }

        public int CODCLIENTE { get; set; }
        public string NMCLIENTE { get; set; }
        public string CPFCNPJ { get; set; }
        public string EMAIL { get; set; }
        public int CODEMPRESA { get; set; }
        public int CODFORMAFATURAMENTO { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string COBRANCADIFERENCIADA { get; set; }
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
        public string ATIVO { get; set; }
        public string TIPOCLIENTE { get; set; }

        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual PAGNET_FORMAS_FATURAMENTO PAGNET_FORMAS_FATURAMENTO { get; set; }
        public virtual ICollection<PAGNET_EMISSAOBOLETO> PAGNET_EMISSAOBOLETO { get; set; }
        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }
        public virtual ICollection<PAGNET_ARQUIVO_CONCILIACAO> PAGNET_ARQUIVO_CONCILIACAO { get; set; }

        
    }
    public class PAGNET_CADCLIENTE_LOG
    {
        public int CODCLIENTE_LOG { get; set; }
        public int CODCLIENTE { get; set; }
        public string NMCLIENTE { get; set; }
        public string CPFCNPJ { get; set; }
        public string EMAIL { get; set; }
        public int CODEMPRESA { get; set; }
        public int CODFORMAFATURAMENTO { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string COBRANCADIFERENCIADA { get; set; }
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
        public string ATIVO { get; set; }
        public int CODUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }
        public string TIPOCLIENTE { get; set; }

    }
}
