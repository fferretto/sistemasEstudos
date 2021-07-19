using PagNet.BLD.Relatorio.Abstraction.Interface.Model;
using System.Collections.Generic;

namespace PagNet.BLD.Relatorio.Abstraction.Model
{
    public class RelatorioModel: IRelatorioModel
    {
        public RelatorioModel()
        {
            listaCampos = new List<ParametrosRelatorioVm>();
        }
        public List<ParametrosRelatorioVm> listaCampos { get; set; }

        public int codRel { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }
        public string ExecutaViaJob { get; set; }
        public string statusGeracao { get; set; }
        public bool PossuiOutroRelatorioSendoGerado { get; set; }
        public string pathArquivo { get; set; }
        public int TipoRelatorio { get; set; }
        public int codigoRelatorioSendoGerado { get; set; }
    }

    public class ParametrosRelatorioVm
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

    public class ModRelPDFVm
    {
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
    public class RetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }
}
