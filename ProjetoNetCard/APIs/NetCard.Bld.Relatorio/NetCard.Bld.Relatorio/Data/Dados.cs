using NetCard.Bld.Relatorio.Abstraction.Interface;
using NetCard.Bld.Relatorio.Context;
using NetCard.Bld.Relatorio.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.Data.SqlClient;
using System.Linq;
using System.Data;
using Telenet.BusinessLogicModel;
using NetCard.Bld.Relatorio.Interface;
using Telenet.Data.Common;
using Telenet.Data;

namespace NetCard.Bld.Relatorio.Data
{
    public class DadosApp : Service<IContextoApp>, IDadosApp
    {
        private readonly IDbSqlClient<DbNetCardContext> _dbClient;
        private readonly IDbSqlClient<DbConcentradorContext> _dbConcentrador;
        private readonly IParametrosApp _user;

        public DadosApp(IContextoApp contexto,
                     IParametrosApp user,
                    IDbSqlClient<DbConcentradorContext> dbConcentrador,
                    IDbSqlClient<DbNetCardContext> dbClient)
            : base(contexto)
        {
            _dbConcentrador = dbConcentrador;
            _dbClient = dbClient;
            _user = user;
        }

        public List<RELATORIO> BuscaTodosRelatorios()
        {
            string query = "escrever a query aqui";
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<RELATORIO>()
                                .ToList();

            return restultadoQuery;
        }
        public RELATORIO BuscaRelatorioByID(int codRel)
        {
            string query = Querys.QueryBuscaRelatorioByID(codRel);
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<RELATORIO>()
                                .FirstOrDefault();

            return restultadoQuery;

        }
        public RELATORIO_STATUS BuscaStatusRelatorio(string ChaveAcesso)
        {
            string query = Querys.QueryBuscaStatusRelatorio(ChaveAcesso);
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<RELATORIO_STATUS>()
                                .FirstOrDefault();

            return restultadoQuery;
        }
        public List<RELATORIO_PARAM_UTILIZADO> BuscaListaParametrosUtilizados(string codigo_status)
        {
            string query = Querys.QueryBuscaListaParametrosUtilizados(codigo_status);
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<RELATORIO_PARAM_UTILIZADO>()
                                .ToList();

            return restultadoQuery;
        }
        public List<PARAMETRO_REL> BuscaListaParametrosRelatorio(int id_rel)
        {
            string query = Querys.QueryBuscaListaParametrosRelatorio(id_rel);
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<PARAMETRO_REL>()
                                .ToList();

            return restultadoQuery;
        }
        public RELATORIO_STATUS BuscaRelatorioPendenteDownload(string ChaveAcesso)
        {
            string query = Querys.QueryBuscaRelatorioPendenteDownload(ChaveAcesso);
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<RELATORIO_STATUS>()
                                .FirstOrDefault();

            return restultadoQuery;
        }
        public List<RELATORIO_RESULTADO> BuscaRelatorioResultado(string COD_STATUS_REL)
        {
            string query = Querys.QueryBuscaRelatorioResultado(COD_STATUS_REL);
            var restultadoQuery = _dbClient
                                .Query(query)
                                .GetData<RELATORIO_RESULTADO>()
                                .ToList();

            return restultadoQuery;
        }
        public void RemoveRelatorioStatus(string COD_STATUS_REL)
        {
            string queryParam = Querys.QueryRemoveParametrosUsadosRel(COD_STATUS_REL);
            string queryResult = Querys.QueryRemoveRelatorioResult(COD_STATUS_REL);            
            string queryStatus = Querys.QueryRemoveRelatorioStatus(COD_STATUS_REL);

            _dbClient.Command(queryResult).Execute();
            _dbClient.Command(queryParam).Execute();
            _dbClient.Command(queryStatus).Execute();

        }
        public DataTable ListaDadosRelDataTable(string _query)
        {
            DataTable Dados = new DataTable();

            using (new EnsureOpen(_dbClient.Connection))
            {
                var restultadoQuery = _dbClient
                                        .CustomCommand(_query)
                                        .ExecuteReader();

                Dados.Load(restultadoQuery);
            }
            return Dados;


        }
        public List<dynamic> BuscaDadosRelatorio(string _query)
        {
            using (new EnsureOpen(_dbClient.Connection))
            {
                var restultadoQuery = _dbClient
                                        .CustomCommand(_query)
                                        .ExecuteReader()
                                        .ToEnumerable()
                                        .ToList();

                return restultadoQuery;
            }
          
        }
        public void IncluiStatusRel(RELATORIO_STATUS dados)
        {
            var _query = Querys.QueryIncluiStatusRel(dados);

            _dbClient.Command(_query).Execute();
        }
        public void IncluiParametroUtilizado(RELATORIO_PARAM_UTILIZADO dados)
        {

            var _query = Querys.QueryIncluiRELATORIO_PARAM_UTILIZADO(dados);

            _dbClient.Command(_query).Execute();
           
        }
        public DADOSBANCOMODEL BuscaBDDadosAcesso()
        {
            string _query = Querys.QueryBuscaDadosBD(_user.cod_ope);

            var restultadoQuery = _dbConcentrador
                                   .Query(_query)
                                   .GetData<DADOSBANCOMODEL>()
                                   .FirstOrDefault();

            return restultadoQuery;
        }
        public PARAMETRO_REL BuscaDadosParametro(int codigoParametro)
        {
            string _query = Querys.QueryBuscaParametroRelatorio(codigoParametro);

            var restultadoQuery = _dbClient
                                   .Query(_query)
                                   .GetData<PARAMETRO_REL>()
                                   .FirstOrDefault();

            return restultadoQuery;
        }
        public List<DETALHES_PARAMETROS> ListaDetalheParametro(int codigoParametro)
        {            
            string _query = Querys.QueyDetalheParametro(codigoParametro);

            var restultadoQuery = _dbClient
                                   .Query(_query)
                                   .GetData<DETALHES_PARAMETROS>()
                                   .ToList();
            return restultadoQuery;
        }
        public List<DDLDINAMICO> ListaDDLDINAMICO(string procedure, string Parametros)
        {
            string _query = Querys.QueryExecutaProcedureDDL(procedure, Parametros);

            var restultadoQuery = _dbClient
                                   .Query(_query)
                                   .GetData<DDLDINAMICO>()
                                   .ToList();
            return restultadoQuery;
        }
    }
}
