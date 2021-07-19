using System;
using System.Collections.Generic;
using System.Text;

namespace NetCard.Bld.Relatorio.Entity
{
    public class RELATORIO_STATUS
    {

        public string COD_STATUS_REL { get; set; }
        public int ID_REL { get; set; }
        public string CHAVEACESSO { get; set; }
        public string STATUS { get; set; }
        public int TIPORETORNO { get; set; }
        public DateTime DATEMISSAO { get; set; }
        public int ERRO { get; set; }
        public string MSG_ERRO { get; set; }
    }
}
