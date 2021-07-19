using System;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_TAXAS_TITULOS
    {
        public int CODTAXATITULO { get; set; }
        public int CODTITULO { get; set; }
        public string DESCRICAO { get; set; }
        public decimal VALOR { get; set; }
        public DateTime DTINCLUSAO { get; set; }
        public string ORIGEM { get; set; }
        public int CODUSUARIO { get; set; }

        public virtual PAGNET_EMISSAO_TITULOS PAGNET_EMISSAO_TITULOS { get; set; }
        public virtual USUARIO_NETCARD USUARIO_NETCARD { get; set; }
    }
}
