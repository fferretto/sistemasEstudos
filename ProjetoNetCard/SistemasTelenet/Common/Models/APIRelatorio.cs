using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class FiltroRelModel
    {
        public int codigoRelatorio { get; set; }
        public int sistema { get; set; }
        public int codigoCliente { get; set; }
    }
    public class RelatorioModel
    {
        public RelatorioModel()
        {
            listaCampos = new List<ParametrosRelatorioModel>();
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
        public List<ParametrosRelatorioModel> listaCampos { get; set; }


    }
    public class ModRelPDF
    {
        public bool sucesso { get; set; }
        public string msgResultado { get; set; }
        public bool EmGeracao { get; set; }
        public List<ModRel> Cabecalho { get; set; }
        public List<ModRel> Conteudo { get; set; }
        public string nmRelatorio { get; set; }
        public int TipoRel { get; set; }
        public string caminhoArquivo { get; set; }
    }
    public class ModRel
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }
    }
    public class ParametrosRelatorioModel
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
}