using System;
using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public class PAGNET_BORDERO_PAGAMENTO
    {
        public PAGNET_BORDERO_PAGAMENTO()
        {
            PAGNET_TITULOS_PAGOS = new List<PAGNET_TITULOS_PAGOS>();
            PAGNET_EMISSAO_TITULOS = new List<PAGNET_EMISSAO_TITULOS>();
        }
        public int CODBORDERO { get; set; }
        public string STATUS { get; set; }
        public int CODUSUARIO { get; set; }
        public int CODCONTACORRENTE { get; set; }
        public decimal VLBORDERO { get; set; }
        public DateTime DTBORDERO { get; set; }
        public int CODEMPRESA { get; set; }
        public virtual USUARIO_NETCARD USUARIO_NETCARD { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }

        public virtual ICollection<PAGNET_TITULOS_PAGOS> PAGNET_TITULOS_PAGOS { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
    }
}
