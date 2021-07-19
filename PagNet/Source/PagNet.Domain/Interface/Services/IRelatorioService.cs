using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_RelatorioService : IServiceBase<PAGNET_RELATORIO>
    {
        List<PAGNET_RELATORIO> GetAllRelatorios();
        PAGNET_RELATORIO GetRelatorioByID(int codRel);
        List<dynamic> GetDadosRelatorio(string _query, Dictionary<string, object> Parameters);
        DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters);
    }
}
