using System.Collections.Generic;
using TELENET.SIL.PO;

namespace NetCard.Common.Models
{
    public class MemCarga
    {
        public string ValidacaoCarga { get; set; }
        public string MenssageCarga { get; set; }
        public List<string> ListaCargaArquivo { get; set; }
        public string DataProg { get; set; }
        public List<Cartao> ListaRecarga { get; set; }
        public LOG LogCarga { get; set; }
        public string AcaoController { get; set; }
        public int OpcaoPesq { get; set; }

    }
}
