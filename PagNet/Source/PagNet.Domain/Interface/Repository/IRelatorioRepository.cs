using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Data;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_RelatorioRepository : IRepositoryBase<PAGNET_RELATORIO>
    {
        List<dynamic> ExecQueryDinamica(string _query, Dictionary<string, object> Parameters);
        DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters);
    }
}
