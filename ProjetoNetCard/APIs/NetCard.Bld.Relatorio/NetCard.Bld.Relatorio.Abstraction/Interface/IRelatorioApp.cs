using NetCard.Bld.Relatorio.Abstraction.Interface.Model;
using NetCard.Bld.Relatorio.Abstraction.Model;
using System.Collections.Generic;

namespace NetCard.Bld.Relatorio.Abstraction.Interface
{
    public interface IRelatorioApp
    {
        RelatorioModel BuscaParametrosRelatorio(IFiltroRelModel filtro);
        ModRelPDFVm RelatorioPDF(IRelatorioModel model);
        RetornoModel ExportaExcel(IRelatorioModel model);
        bool VerificaTerminoGeracaoRelatorio(IFiltroRelModel filtro);
        RetornoModel GeraRelatorioViaJob(IRelatorioModel model);
        ModRelPDFVm RetornoRelatornoViaJob(IRelatorioModel model);
        List<APIRetornoDDLModel> CarregaDDL(IFiltroDDLRelModel filtro);
    }
}
