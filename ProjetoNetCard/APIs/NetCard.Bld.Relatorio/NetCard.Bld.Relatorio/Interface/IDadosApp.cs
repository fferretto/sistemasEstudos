using NetCard.Bld.Relatorio.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NetCard.Bld.Relatorio.Interface
{
    public interface IDadosApp
    {
        List<RELATORIO> BuscaTodosRelatorios();
        RELATORIO BuscaRelatorioByID(int codRel);
        RELATORIO_STATUS BuscaStatusRelatorio(string ChaveAcesso);
        List<RELATORIO_PARAM_UTILIZADO> BuscaListaParametrosUtilizados(string codigo_status);
        List<PARAMETRO_REL> BuscaListaParametrosRelatorio(int id_rel);
        PARAMETRO_REL BuscaDadosParametro(int codigoParametro);
        List<DETALHES_PARAMETROS> ListaDetalheParametro(int codigoParametro);
        List<DDLDINAMICO> ListaDDLDINAMICO(string procedure, string Parametros);
        RELATORIO_STATUS BuscaRelatorioPendenteDownload(string ChaveAcesso);
        List<RELATORIO_RESULTADO> BuscaRelatorioResultado(string COD_STATUS_REL);
        void RemoveRelatorioStatus(string COD_STATUS_REL);
        DataTable ListaDadosRelDataTable(string _query);
        List<dynamic> BuscaDadosRelatorio(string _query);
        void IncluiStatusRel(RELATORIO_STATUS dados);
        void IncluiParametroUtilizado(RELATORIO_PARAM_UTILIZADO dados);
        DADOSBANCOMODEL BuscaBDDadosAcesso();
    }
}
