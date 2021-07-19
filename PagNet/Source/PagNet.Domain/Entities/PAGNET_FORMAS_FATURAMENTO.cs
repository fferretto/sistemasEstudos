using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public class PAGNET_FORMAS_FATURAMENTO
    {
        public PAGNET_FORMAS_FATURAMENTO()
        {
            PAGNET_CADCLIENTE = new List<PAGNET_CADCLIENTE>();
            PAGNET_EMISSAOFATURAMENTO = new List<PAGNET_EMISSAOFATURAMENTO>();
        }
        public int CODFORMAFATURAMENTO { get; set; }
        public string NMFORMAFATURAMENTO { get; set; }
        public string ATIVO { get; set; }

        public virtual ICollection<PAGNET_CADCLIENTE> PAGNET_CADCLIENTE { get; set; }
        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }
    }
}