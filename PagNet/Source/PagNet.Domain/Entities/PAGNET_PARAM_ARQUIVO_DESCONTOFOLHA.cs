namespace PagNet.Domain.Entities
{
    public class PAGNET_PARAM_ARQUIVO_DESCONTOFOLHA
    {
        public int CODPARAM { get; set; }
        public int CODARQUIVO_CONCILIACAO { get; set; }
        public string TIPOARQUIVO { get; set; }
        public string CAMPO { get; set; }
        public int LINHAINICIO { get; set; }
        public int POSICAO_CSV { get; set; }
        public int POSICAOINI_TXT { get; set; }
        public int POSICAOFIM_TXT { get; set; }

        public virtual PAGNET_ARQUIVO_DESCONTOFOLHA PAGNET_ARQUIVO_DESCONTOFOLHA { get; set; }
    }
}