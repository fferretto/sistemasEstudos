using System;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Entities
{
    public partial class PAGNET_RELATORIO
    {
        public PAGNET_RELATORIO()
        {
            PAGNET_PARAMETRO_REL = new List<PAGNET_PARAMETRO_REL>();
            PAGNET_RELATORIO_STATUS = new List<PAGNET_RELATORIO_STATUS>();
        }
        public int ID_REL { get; set; }
        public string DESCRICAO { get; set; }
        public string NOMREL { get; set; }
        public string TIPREL { get; set; }
        public string NOMPROC { get; set; }
        public string EXECUTARVIAJOB { get; set; }

        public virtual ICollection<PAGNET_PARAMETRO_REL> PAGNET_PARAMETRO_REL { get; set; }
        public virtual ICollection<PAGNET_RELATORIO_STATUS> PAGNET_RELATORIO_STATUS { get; set; }
    }
    public partial class PAGNET_RELATORIO_STATUS
    {
        public PAGNET_RELATORIO_STATUS()
        {
            PAGNET_RELATORIO_RESULTADO = new List<PAGNET_RELATORIO_RESULTADO>();
            PAGNET_RELATORIO_PARAM_UTILIZADO = new List<PAGNET_RELATORIO_PARAM_UTILIZADO>();
        }
        public string COD_STATUS_REL { get; set; }
        public int ID_REL { get; set; }
        public int CODUSUARIO { get; set; }
        public string STATUS { get; set; }
        public int TIPORETORNO { get; set; }
        public DateTime DATEMISSAO { get; set; }
        public int ERRO { get; set; }
        public string MSG_ERRO { get; set; }

        public virtual PAGNET_RELATORIO PAGNET_RELATORIO { get; set; }
        public virtual ICollection<PAGNET_RELATORIO_RESULTADO> PAGNET_RELATORIO_RESULTADO { get; set; }
        public virtual ICollection<PAGNET_RELATORIO_PARAM_UTILIZADO> PAGNET_RELATORIO_PARAM_UTILIZADO { get; set; }
    }
    public partial class PAGNET_RELATORIO_RESULTADO
    {
        public int COD_RESULTADO { get; set; }
        public string COD_STATUS_REL { get; set; }
        public string LINHAIMP { get; set; }
        public string TIP { get; set; }

        public virtual PAGNET_RELATORIO_STATUS PAGNET_RELATORIO_STATUS { get; set; }
    }
    public partial class PAGNET_RELATORIO_PARAM_UTILIZADO
    {
        public int COD_PARAM_UTILIZADO { get; set; }
        public string COD_STATUS_REL { get; set; }
        public string NOMPAR { get; set; }
        public string CONTEUDO { get; set; }

        public virtual PAGNET_RELATORIO_STATUS PAGNET_RELATORIO_STATUS { get; set; }
    }
}