using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_CADPLANOCONTAS
    {
        public PAGNET_CADPLANOCONTAS()
        {
            this.PAGNET_EMISSAOFATURAMENTO = new List<PAGNET_EMISSAOFATURAMENTO>();
            this.PAGNET_EMISSAO_TITULOS = new List<PAGNET_EMISSAO_TITULOS>();
        }
        public int CODPLANOCONTAS { get; set; }
        public int? CODPLANOCONTAS_PAI { get; set; }
        public int? CODEMPRESA { get; set; }
        public string CLASSIFICACAO { get; set; }
        public string TIPO { get; set; }
        public string NATUREZA { get; set; }
        public string DESCRICAO { get; set; }
        public string DESPESA { get; set; }
        public string DEFAULTPAGAMENTO { get; set; }
        public string DEFAULTRECEBIMENTO { get; set; }
        public string PLANOCONTASDEFAULT { get; set; }
        public string ATIVO { get; set; }

        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }

        public virtual ICollection<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }
        public virtual ICollection<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
    }
}
