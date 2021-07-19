using System.Collections.Generic;

namespace PagNet.Bld.AntecipPGTO.Domain.Entities
{
    public class PAGNET_USUARIO
    {
        public PAGNET_USUARIO()
        {
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
        public virtual ICollection<PAGNET_TAXAS_TITULOS> PAGNET_TAXAS_TITULOS { get; set; }

    }
}
