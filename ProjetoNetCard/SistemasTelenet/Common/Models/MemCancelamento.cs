using System.Collections.Generic;
using TELENET.SIL.PO;

namespace NetCard.Common.Models
{
    public class MemCancelamento
    {
        public string ValidacaoCanc { get; set; }
        public string MenssageCanc { get; set; }
        public List<string> ListaCancArquivo { get; set; }
        public List<Cartao> ListaCanc { get; set; }
        public LOG LogCanc { get; set; }
        public string AcaoController { get; set; }
        public List<string> RelCancSint { get; set; }
        public List<string> RelCancAnalit { get; set; }
    }
}
