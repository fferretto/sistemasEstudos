using NetCard.Bld.Relatorio.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCard.Bld.Relatorio.Data
{
    public static class Querys
    {
        public static string BuscaTodosRelatorios()
        {
            var sql = new StringBuilder();

            //sql.AppendLine("SELECT R.ID_REL, R.NOMPROC, P.ID_PAR, P.DESPAR, R.NOMREL, ");

            return sql.ToString();
        }
        public static string QueryBuscaRelatorioByID(int codRel)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID_REL, ");
            sql.AppendLine("       DESCRICAO, ");
            sql.AppendLine("       NOMREL, ");
            sql.AppendLine("       TIPREL, ");
            sql.AppendLine("       NOMPROC, ");
            sql.AppendLine("       EXCLUSIVO, ");
            sql.AppendLine("       NAVIGATEURL, ");
            sql.AppendLine("       SISTEMA, ");
            sql.AppendLine("       REMOTO, ");
            sql.AppendLine("       TIPOACESSO, ");
            sql.AppendLine("       MODULO, ");
            sql.AppendLine("       SAIDA_ARQ_DIRETO, ");
            sql.AppendLine("       DISPOSICAO, ");
            sql.AppendLine("       EXECUTARVIAJOB ");
            sql.AppendLine("  FROM RELATORIO ");
            sql.AppendLine(" WHERE ID_REL = " + codRel);

            return sql.ToString();
        }
        public static string QueryBuscaStatusRelatorio(string CHAVEACESSO)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT COD_STATUS_REL, ");
            sql.AppendLine("ID_REL, ");
            sql.AppendLine("CHAVEACESSO, ");
            sql.AppendLine("STATUS, ");
            sql.AppendLine("TIPORETORNO, ");
            sql.AppendLine("DATEMISSAO, ");
            sql.AppendLine("ERRO, ");
            sql.AppendLine("MSG_ERRO ");
            sql.AppendLine("FROM RELATORIO_STATUS ");
            sql.AppendLine("WHERE CHAVEACESSO = " + CHAVEACESSO);
            sql.AppendLine("AND STATUS != 'BAIXADO' ");

            return sql.ToString();
        }
        public static string QueryBuscaListaParametrosUtilizados(string codigo_status)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT COD_PARAM_UTILIZADO, ");
            sql.AppendLine("COD_STATUS_REL, ");
            sql.AppendLine("NOMPAR, ");
            sql.AppendLine("LABEL, ");
            sql.AppendLine("CONTEUDO ");
            sql.AppendLine("FROM RELATORIO_PARAM_UTILIZADO ");
            sql.AppendLine("WHERE COD_STATUS_REL = '" + codigo_status + "' ");

            return sql.ToString();
        }
        public static string QueryBuscaListaParametrosRelatorio(int id_rel)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID_PAR, ");
            sql.AppendLine("ID_REL, ");
            sql.AppendLine("DESPAR, ");
            sql.AppendLine("NOMPAR, ");
            sql.AppendLine("LABEL, ");
            sql.AppendLine("TIPO, ");
            sql.AppendLine("TAMANHO, ");
            sql.AppendLine("_DEFAULT, ");
            sql.AppendLine("REQUERIDO, ");
            sql.AppendLine("HAS_DETAIL, ");
            sql.AppendLine("ORDEM_TELA, ");
            sql.AppendLine("ORDEM_PROC, ");
            sql.AppendLine("NOM_FUNCTION, ");
            sql.AppendLine("MASCARA, ");
            sql.AppendLine("TEXTOAJUDA ");
            sql.AppendLine("FROM PARAMETRO ");
            sql.AppendLine("WHERE ID_REL = " + id_rel);

            return sql.ToString();
        }
        public static string QueryBuscaRelatorioPendenteDownload(string CHAVEACESSO)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT COD_STATUS_REL, ");
            sql.AppendLine("ID_REL, ");
            sql.AppendLine("CHAVEACESSO, ");
            sql.AppendLine("STATUS, ");
            sql.AppendLine("TIPORETORNO, ");
            sql.AppendLine("DATEMISSAO, ");
            sql.AppendLine("ERRO, ");
            sql.AppendLine("MSG_ERRO ");
            sql.AppendLine("FROM RELATORIO_STATUS ");
            sql.AppendLine("WHERE CHAVEACESSO = " + CHAVEACESSO);
            sql.AppendLine("AND STATUS != 'BAIXADO' ");

            return sql.ToString();
        }
        public static string QueryBuscaRelatorioResultado(string COD_STATUS_REL)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT COD_RESULTADO, ");
            sql.AppendLine("COD_STATUS_REL, ");
            sql.AppendLine("LINHAIMP, ");
            sql.AppendLine("TIP ");
            sql.AppendLine("FROM RELATORIO_RESULTADO ");
            sql.AppendLine("WHERE COD_STATUS_REL = '"+ COD_STATUS_REL + "' ");

            return sql.ToString();
        }
        public static string QueryRemoveRelatorioStatus(string COD_STATUS_REL)
        {
            var sql = new StringBuilder();

            sql.AppendLine("DELETE FROM RELATORIO_STATUS ");
            sql.AppendLine("WHERE COD_STATUS_REL = '" + COD_STATUS_REL + "' ");

            return sql.ToString();
        }
        public static string QueryRemoveParametrosUsadosRel(string COD_STATUS_REL)
        {
            var sql = new StringBuilder();

            sql.AppendLine("DELETE FROM RELATORIO_PARAM_UTILIZADO ");
            sql.AppendLine("WHERE COD_STATUS_REL = '" + COD_STATUS_REL + "' ");

            return sql.ToString();
        }
        public static string QueryRemoveRelatorioResult(string COD_STATUS_REL)
        {
            var sql = new StringBuilder();

            sql.AppendLine("DELETE FROM RELATORIO_RESULTADO ");
            sql.AppendLine("WHERE COD_STATUS_REL = '" + COD_STATUS_REL + "' ");

            return sql.ToString();
        }
        public static string QueryBuscaProximoIdRELATORIO_PARAM_UTILIZADO()
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ISNULL(MAX(COD_PARAM_UTILIZADO),0) + 1  FROM RELATORIO_PARAM_UTILIZADO ");

            return sql.ToString();
        }
        public static string QueryIncluiRELATORIO_PARAM_UTILIZADO(RELATORIO_PARAM_UTILIZADO model)
        {
            var sql = new StringBuilder();

            sql.AppendLine("INSERT INTO RELATORIO_PARAM_UTILIZADO(COD_STATUS_REL, NOMPAR, LABEL, CONTEUDO) ");
            sql.AppendLine($"VALUES('{model.COD_STATUS_REL}', '{model.NOMPAR}', '{model.LABEL}', '{model.CONTEUDO}') ");

            return sql.ToString();
        }
        public static string QueryIncluiStatusRel(RELATORIO_STATUS model)
        {
            var sql = new StringBuilder();

            sql.AppendLine("INSERT INTO RELATORIO_STATUS(COD_STATUS_REL, ID_REL, CHAVEACESSO, STATUS, TIPORETORNO, DATEMISSAO, ERRO, MSG_ERRO) ");
            sql.AppendLine($"VALUES('{model.COD_STATUS_REL}', {model.ID_REL}, {model.CHAVEACESSO}, '{model.STATUS}', {model.TIPORETORNO}, '{model.DATEMISSAO.ToString("yyyy/MM/dd hh:mm")}', {model.ERRO}, '{model.MSG_ERRO}') ");

            return sql.ToString();
        }
        public static string QueryBuscaDadosBD(int codope)
        {
            var sql = new StringBuilder();

            sql.AppendLine($"SELECT TOP 1 BD_AUT, BD_NC FROM OPERADORA where CODOPE = {codope} ");

            return sql.ToString();
        }
        public static string QueryBuscaParametroRelatorio(int ID_PAR)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID_PAR, ");
            sql.AppendLine("ID_REL, ");
            sql.AppendLine("DESPAR, ");
            sql.AppendLine("NOMPAR, ");
            sql.AppendLine("LABEL, ");
            sql.AppendLine("TIPO, ");
            sql.AppendLine("TAMANHO, ");
            sql.AppendLine("_DEFAULT, ");
            sql.AppendLine("REQUERIDO, ");
            sql.AppendLine("HAS_DETAIL, ");
            sql.AppendLine("ORDEM_TELA, ");
            sql.AppendLine("ORDEM_PROC, ");
            sql.AppendLine("NOM_FUNCTION, ");
            sql.AppendLine("MASCARA, ");
            sql.AppendLine("TEXTOAJUDA ");
            sql.AppendLine("FROM PARAMETRO ");
            sql.AppendLine("WHERE ID_PAR = " + ID_PAR);

            return sql.ToString();
        }
        public static string QueyDetalheParametro(int codigoParametro)
        {
            var sql = new StringBuilder();

            sql.AppendLine("SELECT ID_PARAMETRO, ");
            sql.AppendLine("       TEXT, ");
            sql.AppendLine("       VALUE, ");
            sql.AppendLine("       TIPO, ");
            sql.AppendLine("       ID ");
            sql.AppendLine("  FROM DETALHES_PARAMETROS ");
            sql.AppendLine($"WHERE ID_PARAMETRO = {codigoParametro} ");

            return sql.ToString();
        }
        public static string QueryExecutaProcedureDDL(string procedure, string Parametros)
        {
            var sql = new StringBuilder();

            sql.AppendLine($"EXEC {procedure} ");
            if(!string.IsNullOrWhiteSpace(Parametros))
            {
                sql.AppendLine($" {Parametros} ");
            }

            return sql.ToString();
        }
    }
}
