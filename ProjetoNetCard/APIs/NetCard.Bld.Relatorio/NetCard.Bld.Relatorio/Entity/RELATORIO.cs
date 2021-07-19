using System;
using System.Collections.Generic;
using System.Text;

namespace NetCard.Bld.Relatorio.Entity
{
    public class RELATORIO
    {
        public int ID_REL { get; set; }
        public string DESCRICAO { get; set; }
        public string NOMREL { get; set; }
        public string TIPREL { get; set; }
        public string NOMPROC { get; set; }
        public string EXECUTARVIAJOB { get; set; }
    }
}
