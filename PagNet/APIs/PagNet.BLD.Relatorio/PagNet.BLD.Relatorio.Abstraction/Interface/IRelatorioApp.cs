using PagNet.BLD.Relatorio.Abstraction.Interface.Model;
using PagNet.BLD.Relatorio.Abstraction.Model;

namespace PagNet.BLD.Relatorio.Abstraction.Interface
{
    public interface IRelatorioApp
    {
        RelatorioModel BuscaParametrosRelatorio(int codRel);
        ModRelPDFVm RelatorioPDF(IRelatorioModel model);
        RetornoModel ExportaExcel(IRelatorioModel model);
        bool VerificaTerminoGeracaoRelatorio(int codigoRelatorio);
        RetornoModel GeraRelatorioViaJob(IRelatorioModel model);
        ModRelPDFVm RetornoRelatornoViaJob(IRelatorioModel model);
    }
}
