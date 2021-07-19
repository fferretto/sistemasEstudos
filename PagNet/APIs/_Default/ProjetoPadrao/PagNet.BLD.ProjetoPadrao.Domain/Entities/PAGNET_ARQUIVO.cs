using System;
using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public partial class PAGNET_ARQUIVO
    {
        public PAGNET_ARQUIVO()
        {
            this.PAGNET_TITULOS_PAGOS = new List<PAGNET_TITULOS_PAGOS>();
        }

        public int CODARQUIVO { get; set; }
        public string NMARQUIVO { get; set; }
        public string CODBANCO { get; set; }
        public string TIPARQUIVO { get; set; }
        public int? NROSEQARQUIVO { get; set; }
        public DateTime DTARQUIVO { get; set; }
        public string CAMINHOARQUIVO { get; set; }
        public decimal VLTOTAL { get; set; }
        public int QTREGISTRO { get; set; }
        public string STATUS { get; set; }
        
        public virtual PAGNET_BANCO PAGNET_BANCO { get; set; }
        public virtual ICollection<PAGNET_TITULOS_PAGOS> PAGNET_TITULOS_PAGOS { get; set; }

        
    }
}
