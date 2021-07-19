using System;

namespace PagNet.Domain.Entities
{
    public class PAGNET_CONTACORRENTE_SALDO
    {
        public int CODSALDO { get; set; }
        public int CODEMPRESA { get; set; }
        public int CODCONTACORRENTE { get; set; }
        public DateTime DATLANCAMENTO { get; set; }
        public decimal SALDO { get; set; }

        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }
    }
}
