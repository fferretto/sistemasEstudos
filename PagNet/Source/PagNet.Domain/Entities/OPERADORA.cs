using System;
using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public partial class OPERADORA
    {
        public OPERADORA()
        {
            PAGNET_USUARIOS = new List<PAGNET_USUARIO>();
        }

        public short CODOPE { get; set; }
        public string NOME { get; set; }
        public string BD_AUT { get; set; }
        public string BD_NC { get; set; }
        public string FLAG_VA { get; set; }
        public string FLAG_TESTE { get; set; }
        public string SERVIDOR { get; set; }
        public string DESC_PREFEITURA { get; set; }
        public DateTime? REAJUSTE { get; set; }
        public short? DIA_VENCIMENTO { get; set; }
        public decimal? MINIMO { get; set; }
        public string TIPO_CALC { get; set; }
        public decimal? VALOR_UNITARIO { get; set; }
        public decimal? VALOR_CNP { get; set; }
        public decimal? DESCNPB { get; set; }
        public string DESC_CANC { get; set; }
        public decimal? LICENCA { get; set; }
        public string SERVIDOR_AUT { get; set; }
        public string EXIBE_GRAFICO { get; set; }
        public string OPERADORA1 { get; set; }
        public string SERVIDOR_IIS { get; set; }
        public string NOMOPERAFIL { get; set; }
        public string CAMINHO_ARQ_FECH_CLI { get; set; }
        public string CAMINHO_ARQ_FECH_CRE { get; set; }
        public string POSSUI_PAGNET { get; set; }
        //public string POSSUI_NETCARD { get; set; }
        public string CAMINHOARQUIVO { get; set; }
        public virtual ICollection<PAGNET_USUARIO> PAGNET_USUARIOS { get; set; }
    }
}


