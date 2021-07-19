using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_CADEMPRESA
    {
        public PAGNET_CADEMPRESA()
        {
            this.PAGNET_EMISSAO_TITULOS = new List<PAGNET_EMISSAO_TITULOS>();
            this.PAGNET_EMISSAO_TITULOS_LOG = new List<PAGNET_EMISSAO_TITULOS_LOG>();
            this.PAGNET_TITULOS_PAGOS = new List<PAGNET_TITULOS_PAGOS>();
            this.PAGNET_CONFIG_REGRA_BOL = new List<PAGNET_CONFIG_REGRA_BOL>();
            this.PAGNET_CADCLIENTE = new List<PAGNET_CADCLIENTE>();
            this.PAGNET_BORDERO_BOLETO = new List<PAGNET_BORDERO_BOLETO>();
            this.PAGNET_EMISSAOBOLETO = new List<PAGNET_EMISSAOBOLETO>();
            this.PAGNET_EMISSAOFATURAMENTO = new List<PAGNET_EMISSAOFATURAMENTO>();
            this.PAGNET_CONTACORRENTE_SALDO = new List<PAGNET_CONTACORRENTE_SALDO>();
            this.PAGNET_CADPLANOCONTAS = new List<PAGNET_CADPLANOCONTAS>();
            
        }
        

        public int CODEMPRESA { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string NMFANTASIA { get; set; }
        public string CNPJ { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string UTILIZANETCARD { get; set; }
        public int? CODSUBREDE { get; set; }
        public string NMLOGIN { get; set; }

        public virtual ICollection<PAGNET_CADPLANOCONTAS> PAGNET_CADPLANOCONTAS { get; set; }
        public virtual ICollection<PAGNET_CONTACORRENTE_SALDO> PAGNET_CONTACORRENTE_SALDO { get; set; }
        public virtual ICollection<PAGNET_CADCLIENTE> PAGNET_CADCLIENTE { get; set; }
        public virtual ICollection<PAGNET_CONFIG_REGRA_BOL> PAGNET_CONFIG_REGRA_BOL { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS_LOG> PAGNET_EMISSAO_TITULOS_LOG { get; set; }
        public virtual ICollection<PAGNET_TITULOS_PAGOS> PAGNET_TITULOS_PAGOS { get; set; }
        public virtual ICollection<PAGNET_BORDERO_BOLETO> PAGNET_BORDERO_BOLETO { get; set; }
        public virtual ICollection<PAGNET_EMISSAOBOLETO> PAGNET_EMISSAOBOLETO { get; set; }
        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }

    }
}
