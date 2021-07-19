using System;
using System.Collections.Generic;
using System.Text;

namespace NetCard.Bld.Relatorio.Entity
{
    public class RELATORIO_RESULTADO
    {

        public int COD_RESULTADO { get; set; }
        public string COD_STATUS_REL { get; set; }
        public string LINHAIMP { get; set; }
        public string TIP { get; set; }
    }
    public class DADOSBANCOMODEL
    {
        public string BD_AUT { get; set; }
        public string BD_NC { get; set; }
    }
}