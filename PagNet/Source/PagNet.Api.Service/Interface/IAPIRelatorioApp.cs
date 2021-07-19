using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface
{
    public interface IAPIRelatorioApp
    {
        APIRelatorioModel BuscaParametrosRelatorio(int codRel);
        APIModRelPDFModel RelatorioPDF(IAPIRelatorioModel model);
        IDictionary<bool, string> ExportaExcel(IAPIRelatorioModel model);
        bool VerificaTerminoGeracaoRelatorio(int codigoRelatorio);
        RetornoModel GeraRelatorioViaJob(IAPIRelatorioModel model);
        APIModRelPDFModel RetornoRelatornoViaJob(IAPIRelatorioModel model);
    }
}
