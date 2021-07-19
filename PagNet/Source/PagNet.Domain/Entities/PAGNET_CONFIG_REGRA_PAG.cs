using System;

namespace PagNet.Domain.Entities
{
    public partial class PAGNET_CONFIG_REGRA_PAG
    {
        public int CODREGRA { get; set; }
        public string COBRATAXAANTECIPACAO { get; set; }
        public decimal? PERCTAXAANTECIPACAO { get; set; }
        public decimal? VLTAXAANTECIPACAO { get; set; }
        public string FORMACOMPENSACAO { get; set; }
        public string ATIVO { get; set; }        
    }
    public partial class PAGNET_CONFIG_REGRA_PAG_LOG
    {
        public int CODREGRA_LOG { get; set; }
        public int CODREGRA { get; set; }
        public string COBRATAXAANTECIPACAO { get; set; }
        public decimal? PERCTAXAANTECIPACAO { get; set; }
        public decimal? VLTAXAANTECIPACAO { get; set; }
        public string FORMACOMPENSACAO { get; set; }
        public string ATIVO { get; set; }
        public int CODUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }

    }
}