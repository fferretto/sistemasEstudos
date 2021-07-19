using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_ARQUIVO_CONCILIACAO
    {
            public PAGNET_ARQUIVO_CONCILIACAO()
            {
                this.PAGNET_PARAM_ARQUIVO_CONCILIACAO = new List<PAGNET_PARAM_ARQUIVO_CONCILIACAO>();
            }
            public int CODARQUIVO_CONCILIACAO { get; set; }
            public int CODCLIENTE { get; set; }
            public int CODFORMAVERIFICACAO { get; set; }
            public string EXTENSAOARQUI_RET { get; set; }
            public string ATIVO { get; set; }

            public virtual ICollection<PAGNET_PARAM_ARQUIVO_CONCILIACAO> PAGNET_PARAM_ARQUIVO_CONCILIACAO { get; set; }

            public virtual PAGNET_FORMA_VERIFICACAO_ARQUIVO PAGNET_FORMA_VERIFICACAO_ARQUIVO { get; set; }
            public virtual PAGNET_CADCLIENTE PAGNET_CADCLIENTE { get; set; }            
    }
}