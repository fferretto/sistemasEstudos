using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public partial class PAGNET_RELATORIO
    {
        public PAGNET_RELATORIO()
        {
            PAGNET_PARAMETRO_REL = new List<PAGNET_PARAMETRO_REL>();
        }
        public int ID_REL { get; set; }
        public string DESCRICAO { get; set; }
        public string NOMREL { get; set; }
        public string TIPREL { get; set; }
        public string NOMPROC { get; set; }

        public virtual ICollection<PAGNET_PARAMETRO_REL> PAGNET_PARAMETRO_REL { get; set; }
    }
}