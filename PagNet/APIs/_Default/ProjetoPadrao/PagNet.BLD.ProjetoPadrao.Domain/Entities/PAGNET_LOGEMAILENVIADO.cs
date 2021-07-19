using System;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_LOGEMAILENVIADO
    {
        public int CODLOGEMAILENVIADO { get; set; }
        public int CODUSUARIO { get; set; }
        public int CODCONTAEMAIL { get; set; }
        public int CODEMISSAOBOLETO { get; set; }
        public string EMAILDESTINO { get; set; }
        public DateTime DTENVIO { get; set; }
        public string STATUS { get; set; }
        public string MENSAGEM { get; set; }

        public virtual USUARIO_NETCARD USUARIO_NETCARD { get; set; }
        public virtual PAGNET_CONTAEMAIL PAGNET_CONTAEMAIL { get; set; }
        public virtual PAGNET_EMISSAOBOLETO PAGNET_EMISSAOBOLETO { get; set; }
    }
}