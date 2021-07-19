using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Data;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_RELATORIOService : IServiceBase<PAGNET_RELATORIO>
    {
        List<PAGNET_RELATORIO> GetAllRelatorios();
        PAGNET_RELATORIO GetRelatorioByID(int codRel);
        List<dynamic> GetDadosRelatorio(string _query, Dictionary<string, object> Parameters);
        DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters);
        PAGNET_RELATORIO_STATUS BusctaStatusRelatorio(int codigoRelatorio, int codigoUsuario);
        bool PossuiOutroRelatorioEmGeracao(int codigoRelatorio, int codigoUsuario);
        void IncluiParametroUtilizado(PAGNET_RELATORIO_PARAM_UTILIZADO rel);
        void IncluiStatusRel(PAGNET_RELATORIO_STATUS status);
        void AtualizaStatusRel(PAGNET_RELATORIO_STATUS status);
        PAGNET_RELATORIO_STATUS BuscaRelatorioPendenteDownload(int codigoUsuario);


        void RemoveRelatorioResult(string codStatusRel);
        void RemoveParametrosUsadosRel(string codStatusRel);
        void RemoveRelatorioStatus(string codStatusRel);
    }
}
