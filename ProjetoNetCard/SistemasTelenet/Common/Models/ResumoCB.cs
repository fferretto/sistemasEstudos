using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class ResumoCB
    {
        public List<ExtratoCB> UltCompras { get; set; }
        public decimal TotalPeriodo { get; set; }
        public decimal SaldoCashback { get; set; }
    }
}
