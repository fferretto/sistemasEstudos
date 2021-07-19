using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public partial class PAGNET_BANCO
    {
        public PAGNET_BANCO()
        {
            this.PAGNET_ARQUIVO = new List<PAGNET_ARQUIVO>();
        }
        public string CODBANCO { get; set; }
        public string NMBANCO { get; set; }
        public string ATIVO { get; set; }
        public string POSSUIVAN { get; set; }

        public virtual ICollection<PAGNET_ARQUIVO> PAGNET_ARQUIVO { get; set; }
    }
}