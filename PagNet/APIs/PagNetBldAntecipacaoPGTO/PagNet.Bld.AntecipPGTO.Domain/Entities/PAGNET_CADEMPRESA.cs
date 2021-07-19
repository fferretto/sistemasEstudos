using System.Collections.Generic;

namespace PagNet.Bld.AntecipPGTO.Domain.Entities
{
    public class PAGNET_CADEMPRESA
    {
        public PAGNET_CADEMPRESA()
        {
            this.PAGNET_EMISSAO_TITULOS = new List<PAGNET_EMISSAO_TITULOS>();
        }

        public int CODEMPRESA { get; set; }
        public string RAZAOSOCIAL { get; set; }
        public string NMFANTASIA { get; set; }
        public string CNPJ { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string UTILIZANETCARD { get; set; }
        public int? CODSUBREDE { get; set; }
        public string NMLOGIN { get; set; }

        public virtual ICollection<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }

    }
}
