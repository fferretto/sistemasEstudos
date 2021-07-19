using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;
using System.Collections.Generic;
using System.Data;

namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_RELATORIORepository : IRepositoryBase<PAGNET_RELATORIO>
    {
        List<dynamic> ExecQueryDinamica(string _query, Dictionary<string, object> Parameters);
        DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters);
    }
    public interface IPAGNET_RELATORIO_STATUSRepository : IRepositoryBase<PAGNET_RELATORIO_STATUS>
    {
    }
    public interface IPAGNET_RELATORIO_RESULTADORepository : IRepositoryBase<PAGNET_RELATORIO_RESULTADO>
    {
    }
    public interface IPAGNET_RELATORIO_PARAM_UTILIZADORepository : IRepositoryBase<PAGNET_RELATORIO_PARAM_UTILIZADO>
    {
        int BuscaProximoID();
    }
}
