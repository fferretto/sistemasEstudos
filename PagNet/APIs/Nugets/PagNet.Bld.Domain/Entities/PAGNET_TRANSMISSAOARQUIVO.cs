namespace PagNet.Bld.Domain.Entities
{
    public class PAGNET_TRANSMISSAOARQUIVO
    {
        public int CODTRANSMISSAOARQUIVO { get; set; }
        public int CODCONTACORRENTE { get; set; }
        public string TIPOARQUIVO { get; set; }
        public string FORMATRANSMISSAO { get; set; }
        public string LOGINTRANSMISSAO { get; set; }
        public string SENHATRANSMISSAO { get; set; }
        public string CAMINHOREM { get; set; }
        public string CAMINHORET { get; set; }
        public string CAMINHOAUX { get; set; }

        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }
    }
}