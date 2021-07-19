namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public partial class PAGNET_PARAMETRO_REL
    {
        public int ID_PAR { get; set; }
        public int ID_REL { get; set; }
        public string DESPAR { get; set; }
        public string NOMPAR { get; set; }
        public string LABEL { get; set; }
        public string TIPO { get; set; }
        public int TAMANHO { get; set; }
        public string _DEFAULT { get; set; }
        public string REQUERIDO { get; set; }
        public short ORDEM_TELA { get; set; }
        public short ORDEM_PROC { get; set; }
        public string NOM_FUNCTION { get; set; }
        public string MASCARA { get; set; }
        public string TEXTOAJUDA { get; set; }

        public virtual PAGNET_RELATORIO PAGNET_RELATORIO { get; set; }
    }
}