using System.Collections.Generic;

namespace PagNet.Bld.Domain.Entities
{
    public partial class PAGNET_CODIGOOCORRENCIA
    {
        public PAGNET_CODIGOOCORRENCIA()
        {
            this.PAGNET_EMISSAOBOLETO = new List<PAGNET_EMISSAOBOLETO>();
        }

        public int codOcorrencia { get; set; }
        public string nmOcorrencia { get; set; }
        public string ATIVO { get; set; }
        public virtual ICollection<PAGNET_EMISSAOBOLETO> PAGNET_EMISSAOBOLETO { get; set; }
    }
}
