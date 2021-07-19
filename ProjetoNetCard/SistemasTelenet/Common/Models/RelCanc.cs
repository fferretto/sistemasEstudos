using System.Collections.Generic;
using TELENET.SIL.PO;

namespace NetCard.Common.Models
{
    public class RelCanc
    {
        public string Nome { get; set; }
        public List<string> RelSint { get; set; }
        public List<string> RelAnalit { get; set; }
        public LOG Log { get; set; }
    }
}
