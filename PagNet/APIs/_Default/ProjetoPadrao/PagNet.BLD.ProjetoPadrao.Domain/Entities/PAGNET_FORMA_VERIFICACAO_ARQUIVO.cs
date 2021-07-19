using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_FORMA_VERIFICACAO_ARQUIVO
    {
        public PAGNET_FORMA_VERIFICACAO_ARQUIVO()
        {
            this.PAGNET_ARQUIVO_CONCILIACAO = new List<PAGNET_ARQUIVO_CONCILIACAO>();
        }
        public int CODFORMAVERIFICACAO { get; set; }
        public string DESCRICAO { get; set; }

        public virtual ICollection<PAGNET_ARQUIVO_CONCILIACAO> PAGNET_ARQUIVO_CONCILIACAO { get; set; }

    }
}
