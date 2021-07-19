using PagNet.Application.Models;

namespace PagNet.Application.Interface
{
    public interface IRelatorioApp
    {
        RelatorioVms GetParametrosByRelatorio(int codRel);
        ModRelPDFVm RelatorioPDF(RelatorioVms model);
        string ExportaExcel(RelatorioVms model, string pathArquivo);
    }
}
