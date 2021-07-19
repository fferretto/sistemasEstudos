using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class DetalheLote
    {
        public string RazSoc { get; set; }
        public int CodCre { get; set; }
        public int NumFech { get; set; }
        public List<Lote> Lote { get; set; }
        public List<LoteSubRede> ListSubRede { get; set; }
        public int TotalQtdeTrans { get; set; }
        public decimal TotalValor { get; set; }
        public decimal TotalValLiq { get; set; }
        public List<LoteTaxas> ListTaxas { get; set; }
        public decimal TotalValTaxas { get; set; }
    }
}
