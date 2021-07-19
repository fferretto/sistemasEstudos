using System;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Entities
{
    public partial class PAGNET_BORDERO_BOLETO
    {
        public PAGNET_BORDERO_BOLETO()
        {
            PAGNET_EMISSAOFATURAMENTO = new List<PAGNET_EMISSAOFATURAMENTO>();
        }
        public int CODBORDERO { get; set; }
        public string STATUS { get; set; }
        public int CODUSUARIO { get; set; }
        public int CODEMPRESA { get; set; }
        public int QUANTFATURAS { get; set; }
        public decimal VLBORDERO { get; set; }
        public DateTime? DTBORDERO { get; set; }

        public virtual PAGNET_USUARIO PAGNET_USUARIO { get; set; }
        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }

        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }
        
    }
}
