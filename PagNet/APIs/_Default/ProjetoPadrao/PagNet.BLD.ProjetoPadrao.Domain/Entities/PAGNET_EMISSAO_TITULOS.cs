using System;
using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public partial class PAGNET_EMISSAO_TITULOS
    {
        public PAGNET_EMISSAO_TITULOS()
        {
            this.PAGNET_TAXAS_TITULOS = new List<PAGNET_TAXAS_TITULOS>();
        }
        public int CODTITULO { get; set; }
        public int? CODTITULOPAI { get; set; }
        public string STATUS { get; set; }
        public int CODEMPRESA { get; set; }
        public int? CODFAVORECIDO { get; set; }
        public DateTime DATEMISSAO { get; set; }
        public DateTime DATPGTO { get; set; }
        public DateTime DATREALPGTO { get; set; }
        public decimal VALBRUTO { get; set; }
        public decimal VALLIQ { get; set; }
        public string TIPOTITULO { get; set; }
        public string ORIGEM { get; set; }
        public int SISTEMA { get; set; }
        public string LINHADIGITAVEL { get; set; }
        public int? CODBORDERO { get; set; }
        public decimal VALTOTAL { get; set; }
        public string SEUNUMERO { get; set; }
        public int? CODCONTACORRENTE { get; set; }
        public int? CODPLANOCONTAS { get; set; }


        public virtual PAGNET_BORDERO_PAGAMENTO PAGNET_BORDERO_PAGAMENTO { get; set; }
        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual PAGNET_CADFAVORECIDO PAGNET_CADFAVORECIDO { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }
        public virtual PAGNET_CADPLANOCONTAS PAGNET_CADPLANOCONTAS { get; set; }

        public virtual ICollection<PAGNET_TAXAS_TITULOS> PAGNET_TAXAS_TITULOS { get; set; }
    }

    public partial class PAGNET_EMISSAO_TITULOS_LOG
    {
        public int CODTITULO_LOG { get; set; }
        public int CODTITULO { get; set; }
        public int? CODTITULOPAI { get; set; }
        public string STATUS { get; set; }
        public int CODEMPRESA { get; set; }
        public int? CODFAVORECIDO { get; set; }
        public DateTime DATEMISSAO { get; set; }
        public DateTime DATPGTO { get; set; }
        public DateTime DATREALPGTO { get; set; }
        public decimal? VALBRUTO { get; set; }
        public decimal? VALLIQ { get; set; }
        public string TIPOTITULO { get; set; }
        public string ORIGEM { get; set; }
        public int SISTEMA { get; set; }
        public string LINHADIGITAVEL { get; set; }
        public int? CODBORDERO { get; set; }
        public decimal? VALTOTAL { get; set; }
        public string SEUNUMERO { get; set; }
        public int CODUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }
        public int? CODCONTACORRENTE { get; set; }
        public int? CODPLANOCONTAS { get; set; }


        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual USUARIO_NETCARD USUARIO_NETCARD { get; set; }
        public virtual PAGNET_CADFAVORECIDO PAGNET_CADFAVORECIDO { get; set; }
    }
}