using System;
using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public class PAGNET_CONTAEMAIL
    {
        public PAGNET_CONTAEMAIL()
        {
            PAGNET_LOGEMAILENVIADO = new List<PAGNET_LOGEMAILENVIADO>();
        }
        public int CODCONTAEMAIL { get; set; }
        public int CODEMPRESA { get; set; }
        public string NMCONTAEMAIL { get; set; }

        public string EMAIL { get; set; }
        public string SENHA { get; set; }
        public string SERVIDOR { get; set; }
        public string ENDERECOSMTP { get; set; }
        public string PORTA { get; set; }
        public string CRIPTOGRAFIA { get; set; }
        public string EMAILPRINCIPAL { get; set; }
        public string ATIVO { get; set; }
        

        public virtual ICollection<PAGNET_LOGEMAILENVIADO> PAGNET_LOGEMAILENVIADO { get; set; }

    }
}
