using PagNet.BLD.Relatorio.Abstraction.Interface.Model;
using PagNet.BLD.Relatorio.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.BLD.Relatorio.Web.Setup.Models
{
    public class ParametrosRel : IRelatorioModel
    {
        public List<ParametrosRelatorioVm> listaCampos { get; set; }
        public int codRel { get; set; }
        public string nmProc { get; set; }
        public string nmTela { get; set; }
        public string urlRelatorio { get; set; }
        public string pathArquivo { get; set; }
        public string statusGeracao { get; set; }
        public bool PossuiOutroRelatorioSendoGerado { get; set; }
        public string ExecutaViaJob { get; set; }
        public int TipoRelatorio { get; set; }
        public int codigoRelatorioSendoGerado { get; set; }
    }
}
