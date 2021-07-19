using System.Collections.Generic;

namespace PagNet.Domain.Entities
{
    public class PAGNET_FORMA_VERIFICACAO_DF
    {
        public PAGNET_FORMA_VERIFICACAO_DF()
        {
            this.PAGNET_ARQUIVO_DESCONTOFOLHA = new List<PAGNET_ARQUIVO_DESCONTOFOLHA>();
        }
        public int CODFORMAVERIFICACAO { get; set; }
        public string DESCRICAO { get; set; }

        public virtual ICollection<PAGNET_ARQUIVO_DESCONTOFOLHA> PAGNET_ARQUIVO_DESCONTOFOLHA { get; set; }

    }
}
