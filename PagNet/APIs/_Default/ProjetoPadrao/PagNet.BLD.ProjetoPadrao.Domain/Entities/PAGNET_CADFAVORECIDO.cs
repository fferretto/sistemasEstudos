using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public partial class PAGNET_CADFAVORECIDO
    {
        public PAGNET_CADFAVORECIDO()
        {
            this.PAGNET_EMISSAO_TITULOS = new List<PAGNET_EMISSAO_TITULOS>();
            this.PAGNET_TITULOS_PAGOS = new List<PAGNET_TITULOS_PAGOS>();
            this.PAGNET_EMISSAO_TITULOS_LOG = new List<PAGNET_EMISSAO_TITULOS_LOG>();
        }

        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string CPFCNPJ { get; set; }
        public int? CODCEN { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string DVAGENCIA { get; set; }
        public string OPE { get; set; }
        public string CONTACORRENTE { get; set; }
        public string DVCONTACORRENTE { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string REGRADIFERENCIADA { get; set; }
        public decimal? VALTED { get; set; }
        public decimal? VALMINIMOTED { get; set; }
        public decimal? VALMINIMOCC { get; set; }
        public string ATIVO { get; set; }
        public int? CODCONTACORRENTE { get; set; }
        public int? CODEMPRESA { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }

        public virtual ICollection<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS_LOG> PAGNET_EMISSAO_TITULOS_LOG { get; set; }
        public virtual ICollection<PAGNET_TITULOS_PAGOS> PAGNET_TITULOS_PAGOS { get; set; }
    }
}
