using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class MemBloqueioEmMassa
    {
        public List<string> Linhas { get; set; }

        public MemBloqueioEmMassa()
        {
            Linhas = new List<string>();
        }
    }
}
