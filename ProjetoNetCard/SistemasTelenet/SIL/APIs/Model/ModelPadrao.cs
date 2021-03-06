using System.Collections.Generic;

namespace SIL.APIs.Model
{
    public class APIFiltroRelModel
    {
        public int codigoRelatorio { get; set; }
        public int sistema { get; set; }
        public int codigoCliente { get; set; }
    }
    public class APIRelatorioModel
    {
        public APIRelatorioModel()
        {
            listaCampos = new List<APIParametrosRelatorioModel>();
        }
        //public IList<ParametrosRelatorioModel> listaCampos { get; set; }
        public int idRelatorio { get; set; }
        public int codigoCliente { get; set; }
        public int Sistema { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }
        public string pathArquivo { get; set; }
        public string ExecutaViaJob { get; set; }
        public string statusGeracao { get; set; }
        public int TipoRelatorio { get; set; }
        public bool PossuiOutroRelatorioSendoGerado { get; set; }
        public int codigoRelatorioSendoGerado { get; set; }
        public string msgRetorno { get; set; }
        public List<APIParametrosRelatorioModel> listaCampos { get; set; }


    }
    public class APIModRelPDF
    {
        public bool sucesso { get; set; }
        public string msgResultado { get; set; }
        public bool EmGeracao { get; set; }
        public List<APIModRel> Cabecalho { get; set; }
        public List<APIModRel> Conteudo { get; set; }
        public string nmRelatorio { get; set; }
        public int TipoRel { get; set; }
        public string caminhoArquivo { get; set; }
    }
    public class APIModRel
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }
    }
    public class APIParametrosRelatorioModel
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
        public int ORDEM_TELA { get; set; }
        public int ORDEM_PROC { get; set; }
        public string NOM_FUNCTION { get; set; }
        public string VALCAMPO { get; set; }
        public string MASCARA { get; set; }
        public string TEXTOAJUDA { get; set; }
    }
    public class FiltroDDLRelModel
    {
        public int codigoParametro { get; set; }
        public string nomeProc { get; set; }
        public string ParametroWhere { get; set; }
    }

    public class APIRetornoDDLModel
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Title { get; set; }
    }
    public class APIRetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }
}