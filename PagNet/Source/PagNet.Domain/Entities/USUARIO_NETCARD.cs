using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public partial class USUARIO_NETCARD
    {
        public USUARIO_NETCARD()
        {
            this.PAGNET_EMISSAOFATURAMENTO_LOG = new List<PAGNET_EMISSAOFATURAMENTO_LOG>();
            this.PAGNET_BORDERO_BOLETO = new List<PAGNET_BORDERO_BOLETO>();
            this.PAGNET_BORDERO_PAGAMENTO = new List<PAGNET_BORDERO_PAGAMENTO>();
            this.PAGNET_LOGEMAILENVIADO = new List<PAGNET_LOGEMAILENVIADO>();
            this.PAGNET_EMISSAO_TITULOS_LOG = new List<PAGNET_EMISSAO_TITULOS_LOG>();
            this.PAGNET_TAXAS_TITULOS = new List<PAGNET_TAXAS_TITULOS>();
        }

        public int CODUSUARIO { get; set; }
        public string NMUSUARIO { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string CPF { get; set; }
        public string EMAIL { get; set; }
        public string ADMINISTRADOR { get; set; }
        public string VISIVEL { get; set; }
        public string ATIVO { get; set; }
        public int? CODEMPRESA { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS_LOG> PAGNET_EMISSAO_TITULOS_LOG { get; set; }
        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO_LOG> PAGNET_EMISSAOFATURAMENTO_LOG { get; set; }
        public virtual ICollection<PAGNET_BORDERO_BOLETO> PAGNET_BORDERO_BOLETO { get; set; }
        public virtual ICollection<PAGNET_BORDERO_PAGAMENTO> PAGNET_BORDERO_PAGAMENTO { get; set; }
        public virtual ICollection<PAGNET_LOGEMAILENVIADO> PAGNET_LOGEMAILENVIADO { get; set; }
        public virtual ICollection<PAGNET_TAXAS_TITULOS> PAGNET_TAXAS_TITULOS { get; set; }        

    }
}
