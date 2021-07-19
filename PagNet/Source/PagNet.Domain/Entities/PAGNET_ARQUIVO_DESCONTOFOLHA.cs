using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public class PAGNET_ARQUIVO_DESCONTOFOLHA
    {
            public PAGNET_ARQUIVO_DESCONTOFOLHA()
            {
                this.PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA = new List<PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA>();
            }
            public int CODARQUIVO_CONCILIACAO { get; set; }
            public int CODCLIENTE { get; set; }
            public int CODFORMAVERIFICACAO { get; set; }
            public string EXTENSAOARQUI_RET { get; set; }
            public string ATIVO { get; set; }

            public virtual ICollection<PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA> PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA { get; set; }

            public virtual PAGNET_FORMA_VERIFICACAO_DF PAGNET_FORMA_VERIFICACAO_DF { get; set; }
            public virtual PAGNET_CADCLIENTE PAGNET_CADCLIENTE { get; set; }            
    }
}