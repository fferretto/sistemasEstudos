namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PAGNET_CONFIGVAN
    {
        public int CODCONFIGVAN { get; set; }
        public int CODCONTACORRENTE { get; set; }
        public string ENVIABOLETO { get; set; }
        public string ENVIAPAGAMENTO { get; set; }
        public string USUARIOVAN { get; set; }
        public string CODATIVACAO { get; set; }
        public string CAIXAPOSTAL { get; set; }
        public string DIRETORIORAIZ { get; set; }
        public string ATIVO { get; set; }

        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }
    }
}