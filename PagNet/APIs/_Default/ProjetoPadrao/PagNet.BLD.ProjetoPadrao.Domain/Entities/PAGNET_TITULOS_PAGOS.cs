using System;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public partial class PAGNET_TITULOS_PAGOS
    {
        public int CODTITULOPAGO { get; set; }
        public string STATUS { get; set; }
        public int CODUSUARIO { get; set; }
        public int CODCONTACORRENTE { get; set; }
        public int CODBORDERO { get; set; }
        public int? CODFAVORECIDO { get; set; }
        public int CODEMPRESA { get; set; }
        public int TIPOSERVICO { get; set; }
        public int CODFORMALANCAMENTO { get; set; }
        public string SEUNUMERO { get; set; }
        public string NOSSONUMERO { get; set; }
        public DateTime DTPAGAMENTO { get; set; }
        public DateTime DTREALPAGAMENTO { get; set; }
        public DateTime DTVENCIMENTO { get; set; }
        public decimal VALOR { get; set; }
        public int CODARQUIVO { get; set; }
        public string OCORRENCIARETORNO { get; set; }
        public string TIPOTITULO { get; set; }
        public string LINHADIGITAVEL { get; set; }
               
        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual PAGNET_CADFAVORECIDO PAGNET_CADFAVORECIDO { get; set; }
        public virtual PAGNET_BORDERO_PAGAMENTO PAGNET_BORDERO_PAGAMENTO { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }
        public virtual PAGNET_ARQUIVO PAGNET_ARQUIVO { get; set; }
    }
}