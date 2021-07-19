using PagNet.BLD.Relatorio.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.BLD.Relatorio.Abstraction.Interface.Model
{
    public interface IRelatorioModel
    {
        List<ParametrosRelatorioVm> listaCampos { get; set; }

        int codRel { get; set; }
        int codigoRelatorioSendoGerado { get; set; }
        string nmProc { get; set; }
        string nmTela { get; set; }
        int TipoRelatorio { get; set; }
        string urlRelatorio { get; set; }
        string ExecutaViaJob { get; set; }
        string pathArquivo { get; set; }
        string statusGeracao { get; set; }
        bool PossuiOutroRelatorioSendoGerado { get; set; }
    }

}
