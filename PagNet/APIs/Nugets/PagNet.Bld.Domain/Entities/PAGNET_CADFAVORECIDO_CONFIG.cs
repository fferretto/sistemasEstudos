using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Domain.Entities
{
    public class PAGNET_CADFAVORECIDO_CONFIG
    {
        public int CODFAVORECIDOCONFIG { get; set; }
        public int CODFAVORECIDO { get; set; }
        public int CODEMPRESA { get; set; }
        public string REGRADIFERENCIADA { get; set; }
        public decimal VALTED { get; set; }
        public decimal VALMINIMOTED { get; set; }
        public decimal VALMINIMOCC { get; set; }
        public int? CODCONTACORRENTE { get; set; }

        public virtual PAGNET_CADFAVORECIDO PAGNET_CADFAVORECIDO { get; set; }
    }
}
