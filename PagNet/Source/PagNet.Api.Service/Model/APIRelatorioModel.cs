using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Model
{
    public class APIRelatorioModel : IAPIRelatorioModel
    {
        public List<APIParametrosRelatorioModel> listaCampos { get; set; }
        public int codRel { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }
        public string pathArquivo { get; set; }
        public string ExecutaViaJob { get; set; }
        public string statusGeracao { get; set; }
        public bool PossuiOutroRelatorioSendoGerado { get; set; }
        public int codigoRelatorioSendoGerado { get; set; }
        public int TipoRelatorio { get; set; }
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

    public class APIModRelPDFModel
    {
        public bool EmGeracao { get; set; }
        public List<APIModRelModel> Cabecalho { get; set; }
        public List<APIModRelModel> Conteudo { get; set; }
        public string nmRelatorio { get; set; }
        public int TipoRel { get; set; }
        public string caminhoArquivo { get; set; }
    }
    public class APIModRelModel
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }
    }
}
