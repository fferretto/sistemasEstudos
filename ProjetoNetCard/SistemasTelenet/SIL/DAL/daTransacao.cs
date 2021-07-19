using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace SIL.DAL
{
    public class daTransacao
    {
        public daTransacao(OPERADORA Operador)
        {
            _operadora = Operador;
            _bdTelenet = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        private class SqlTransacao
        {
            public SqlTransacao(OPERADORA operadora)
            {
                _operadora = operadora;
                _whereAutorizador = new StringBuilder();
                _whereNetCard = new StringBuilder();
            }

            #region Templates SQL

            private const string SqlColunas = @"    R.NOMSUBREDE  AS 'SUBREDE', 
    T.DATTRA      AS 'DATA', 
    T.NSUHOS      AS 'Nº HOST', 
    T.NSUAUT      AS 'Nº AUT', 
    U.NOMUSU      AS 'NOME USUARIO', 
    T.CPF         AS 'CPF', 
    T.CODCLI      AS 'COD. CLIENTE', 
    T.NUMDEP      AS 'Nº SEQ. DEPT', 
    U.MAT         AS 'MATRICULA_USU', 
    U.CODFIL      AS 'FILIAL_USU', 
    U.CODSET      AS 'SETOR_USU', 
    T.CODRTA      AS 'TIPO RESP', 
    DBO.F_DAD(T.CODRTA, T.DAD) AS 'DAD',    
    T.DAD         AS 'DAD_JUST',
    {0}           AS 'ATV', 
    T.TIPTRA      AS 'TIPO TRANS', 
    {1}           AS 'CODOPE', 
    T.CODCRT      AS 'NUM CARTAO', 
    T.VALTRA      AS 'VALOR TRANS', 
    {2}           AS 'DATFECCRE', 
    {3}           AS 'NUMFECCRE', 
    {4}           AS 'DATPGTOCRE', 
    {5}           AS 'NUM_CARGA', 
    {6}           AS 'CREDENCIADO', 
    A.DESTIPTRA   AS 'DESTIPTRA', 
    C.NOMCLI      AS 'NOME CLIENTE', 
    CR.CGC        AS 'CGC', 
    CR.NOMFAN     AS 'NOMFAN', 
    CR.RAZSOC     AS 'RAZAO SOCIAL_CRED', 
    S.NOMSEG      AS 'NOMSEG', 
    {7}           AS 'ORIGEM OP', 
    {8}           AS 'NOME OPERADOR',
    W.NOME        AS 'REDE', 
    {9}           AS 'FLAG_AUT', 
    {10}           AS 'TVALOR', 
    {11}          AS 'PARCELA', 
    {12}          AS 'TPARCELA', 
    {13}          AS 'DATFECCLI', 
    {14}          AS 'NUMFECCLI',
    {15}          AS 'DATPGTOCLI'
    {16} ";

            private const string SqlBase = @"SELECT 
{0}
LEFT JOIN TIPTRANS A WITH (NOLOCK) ON A.TIPTRA = T.TIPTRA  
LEFT JOIN CREDENCIADO CR WITH (NOLOCK) ON CR.CODCRE = {1} 
LEFT JOIN SEGMENTO S WITH (NOLOCK) ON CR.CODSEG = S.CODSEG 
LEFT JOIN TABREDE W WITH (NOLOCK) ON T.REDE = W.REDE";

            #endregion

            private OPERADORA _operadora;
            private StringBuilder _whereAutorizador;
            private StringBuilder _whereNetCard;

            private static string GetWhere(StringBuilder builder)
            {
                var where = builder.ToString();

                if (!string.IsNullOrEmpty(where))
                {
                    where = string.Concat(" WHERE ", where);
                }

                if (where.EndsWith("AND"))
                {
                    where = where.Remove(where.Length - 3, 3);
                }

                return where;
            }

            private static string CriaSqlNetCardPrePago(bool count)
            {
                const string fromValue = @"FROM TRANSACVA T WITH (NOLOCK) 
    
    LEFT JOIN CLIENTEVA C WITH (NOLOCK) ON C.CODCLI = T.CODCLI   
    LEFT JOIN SUBREDE R WITH (NOLOCK) ON R.CODSUBREDE = T.CODSUBREDE 
    LEFT JOIN USUARIOVA U WITH (NOLOCK) ON U.CPF = T.CPF AND T.CODCLI = U.CODCLI AND T.NUMDEP = U.NUMDEP     
    LEFT JOIN FECHCREDVA FC WITH (NOLOCK) ON T.CODCRE = FC.CODCRE AND T.NUMFECCRE = FC.NUMFECCRE
    LEFT JOIN OPERVAWS O WITH (NOLOCK) ON O.ID_FUNC = T.ID_OPERADOR 
    LEFT JOIN OPERADORMW OW WITH (NOLOCK) ON OW.ID = T.ID_OPERADOR 
    LEFT JOIN OPERVAWS O2 WITH (NOLOCK) ON O2.ID_FUNC = T.CODOPE ";

                if (count)
                {
                    return string.Format(SqlBase, "COUNT(1) TOTAL " + fromValue, "T.CODCRE");
                }
                else
                {
                    var sql = string.Format(SqlBase, SqlColunas, "{6}");
                    return string.Format(sql,
                        "T.ATV",
                        "T.CODOPE",
                        "T.DATFECCRE",
                        "T.NUMFECCRE",
                        "ISNULL(FC.DATPGTO, DBO.F_DATPGTO(FC.DATFIN, CR.PRAPAG))",
                        "T.NUMCARG_VA",
                        "T.CODCRE",
                        "T.ORIGEM_OPERADOR",
                        @"CASE T.ORIGEM_OPERADOR 
        WHEN 'NC' THEN O.NOME 
        WHEN 'MW' THEN OW.NOME 
        ELSE CASE T.CODOPE 
            WHEN 0 THEN DBO.NOME_OPERADOR(0, T.DATTRA, T.TIPTRA, T.CODCLI, T.CPF, T.CODOPE) 
            ELSE O2.NOME 
        END 
    END",
                        "0",
                        "T.VALTRA",
                        "1",
                        "1",
                        "NULL",
                        "0",
                        "NULL",
                        fromValue);
                }
            }

            private static string CriaSqlAutorizadosPrePago(bool count, string servidor)
            {
                var fromValue = string.Format(@"FROM {0}..CTTRANSVA T WITH (NOLOCK) 
    
    LEFT JOIN CLIENTEVA C WITH (NOLOCK) ON C.CODCLI = T.CODCLI 
    LEFT JOIN SUBREDE R WITH (NOLOCK) ON R.CODSUBREDE = C.CODSUBREDE 
    LEFT JOIN USUARIOVA U WITH (NOLOCK) ON U.CPF = T.CPF AND T.CODCLI = U.CODCLI AND T.NUMDEP = U.NUMDEP ", servidor);

                if (count)
                {
                    return string.Format(SqlBase, "COUNT(1) TOTAL " + fromValue, "T.CODPS");
                }
                else
                {
                    var sql = string.Format(SqlBase, SqlColunas, "{6}");
                    return string.Format(sql,
                        "NULL",
                        "NULL",
                        "NULL",
                        "NULL",
                        "NULL",
                        "NULL",
                        "T.CODPS",
                        "NULL",
                        "NULL",
                        "1",
                        "T.VALTRA",
                        "1",
                        "1",
                        "NULL",
                        "0",
                        "NULL",
                        fromValue);
                }
            }

            private static string CriaSqlNetCardPosPago(bool count)
            {
                const string fromValue = @"FROM TRANSACAO T WITH (NOLOCK) 
    
    LEFT JOIN CLIENTE C WITH (NOLOCK) ON C.CODCLI = T.CODCLI
    LEFT JOIN SUBREDE R WITH (NOLOCK) ON R.CODSUBREDE = T.CODSUBREDE 
    LEFT JOIN FECHCLIENTE FCLI WITH (NOLOCK) ON C.CODCLI = FCLI.CODCLI AND C.NUMFEC = FCLI.NUMFECCLI
    LEFT JOIN FECHCRED FC WITH (NOLOCK) ON T.CODCRE = FC.CODCRE AND T.NUMFECCRE = FC.NUMFECCRE
    LEFT JOIN USUARIO U WITH (NOLOCK) ON U.CPF = T.CPF AND T.CODCLI = U.CODCLI AND T.NUMDEP = U.NUMDEP 
    LEFT JOIN OPERVAWS O WITH (NOLOCK) ON O.ID_FUNC = T.ID_OPERADOR 
    LEFT JOIN OPERADORMW OW WITH (NOLOCK) ON OW.ID = T.ID_OPERADOR 
    LEFT JOIN OPERVAWS O2 WITH (NOLOCK) ON O2.ID_FUNC = T.CODOPE ";

                if (count)
                {
                    return string.Format(SqlBase, "COUNT(1) TOTAL " + fromValue, "T.CODCRE");
                }
                else
                {
                    var sql = string.Format(SqlBase, SqlColunas, "{6}");
                    return string.Format(sql,
                        "T.ATV",
                        "T.CODOPE",
                        "T.DATFECCRE",
                        "T.NUMFECCRE",
                        "ISNULL(FC.DATPGTO, DBO.F_DATPGTO(FC.DATFIN, CR.PRAPAG))",
                        "NULL",
                        "T.CODCRE",
                        "T.ORIGEM_OPERADOR",
                        @"CASE T.ORIGEM_OPERADOR 
        WHEN 'NC' THEN O.NOME 
        WHEN 'MW' THEN OW.NOME 
        ELSE CASE T.CODOPE 
            WHEN 0 THEN DBO.NOME_OPERADOR(0, T.DATTRA, T.TIPTRA, T.CODCLI, T.CPF, T.CODOPE) 
            ELSE O2.NOME 
        END 
    END",
                        "0",
                        "CASE WHEN (A.PARCELADA = 'S' AND T.CODRTA <> 'I') THEN CAST(SUBSTRING(T.DAD,20,9) AS NUMERIC(15,2)) ELSE T.VALTRA END",
                        "CASE WHEN (A.PARCELADA = 'S' AND T.CODRTA <> 'I') THEN  ASCII(SUBSTRING(T.DAD, 30, 1)) - 32 ELSE 1 END",
                        "CASE WHEN (A.PARCELADA = 'S' AND T.CODRTA <> 'I') THEN  ASCII(SUBSTRING(T.DAD, 31, 1)) - 32 ELSE 1 END",
                        "T.DATFECCLI",
                        "T.NUMFECCLI",
                        "ISNULL(FCLI.DATPGTO, DBO.F_DATPGTO_CLI(FCLI.DATFIN, C.PRAPAG, C.TIPPAG))",
                        fromValue);
                }
            }

            private static string CriaSqlAutorizadosPosPago(bool count, string servidor)
            {
                var fromValue = string.Format(@"FROM {0}..CTTRANS T WITH (NOLOCK) 

LEFT JOIN CLIENTE C WITH (NOLOCK) ON C.CODCLI = T.CODCLI 
LEFT JOIN SUBREDE R WITH (NOLOCK) ON R.CODSUBREDE = C.CODSUBREDE 
LEFT JOIN USUARIO U WITH (NOLOCK) ON U.CPF = T.CPF AND T.CODCLI = U.CODCLI AND T.NUMDEP = U.NUMDEP ", servidor);

                if (count)
                {
                    return string.Format(SqlBase, "COUNT(1) TOTAL " + fromValue, "T.CODPS");
                }
                else
                {
                    var sql = string.Format(SqlBase, SqlColunas, "{6}");
                    return string.Format(sql,
                        "NULL",
                    "NULL",
                    "NULL",
                    "NULL",
                    "NULL",
                    "NULL",
                    "T.CODPS",
                    "NULL",
                    "NULL",
                    "1",
                    "CASE WHEN (A.PARCELADA = 'S' AND T.CODRTA <> 'I') THEN CAST(SUBSTRING(T.DAD,20,9) AS NUMERIC(15,2)) ELSE T.VALTRA END",
                    "CASE WHEN (A.PARCELADA = 'S' AND T.CODRTA <> 'I') THEN  ASCII(SUBSTRING(T.DAD, 30, 1)) - 32 ELSE 1 END",
                    "CASE WHEN (A.PARCELADA = 'S' AND T.CODRTA <> 'I') THEN  ASCII(SUBSTRING(T.DAD, 31, 1)) - 32 ELSE 1 END",
                    "NULL",
                    "0",
                    "NULL",
                    fromValue);
                }
            }

            private void AdicionarAutorizadorSe(bool condition, string filtro)
            {
                if (condition)
                {
                    _whereAutorizador.Append(filtro);
                }
            }

            private void AdicionarNetCardVASe(bool condition, string filtro)
            {
                if (condition)
                {
                    _whereNetCard.Append(filtro);
                }
            }

            private void AdicionarParaTodosSe(bool condicao, string filtro)
            {
                if (condicao)
                {
                    AdicionarParaTodos(filtro);
                }
            }

            private void AdicionarAutorizador(string filtro)
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    return;
                }

                _whereAutorizador.Append(filtro);
            }

            private void AdicionarNetCardVA(string filtro)
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    return;
                }

                _whereNetCard.Append(filtro);
            }

            private void AdicionarParaTodos(string filtro)
            {
                AdicionarAutorizador(filtro);
                AdicionarNetCardVA(filtro);
            }

            private string CriaWhereAutorizador()
            {
                return GetWhere(_whereAutorizador);
            }

            private string CriaWhereNetCard()
            {
                return GetWhere(_whereNetCard);
            }

            private void CriaWhere(CONSULTA_VA filtros)
            {
                AdicionarParaTodosSe(filtros.AGRUPAMENTO > 0, string.Concat(" C.CODAG = '", filtros.AGRUPAMENTO, "' AND"));

                if (filtros.PERIODO_INI != DateTime.MinValue && !string.IsNullOrEmpty(filtros.HORA_PERIODO_INI))
                {
                    AdicionarParaTodos(string.Concat(" T.DATTRA >= ", "'", filtros.PERIODO_INI.ToString("yyyy-MM-dd"), " ", filtros.HORA_PERIODO_INI, "' AND"));
                }
                else if (filtros.PERIODO_INI != DateTime.MinValue)
                {
                    AdicionarParaTodos(string.Concat(" T.DATTRA >= ", "'", filtros.PERIODO_INI.ToString("yyyy-MM-dd"), "' AND"));
                }

                if (filtros.PERIODO_FIM != DateTime.MaxValue && !string.IsNullOrEmpty(filtros.HORA_PERIODO_FIM))
                {
                    AdicionarParaTodos(string.Concat(" T.DATTRA <= ", "'", filtros.PERIODO_FIM.ToString("yyyy-MM-dd") + " ", filtros.HORA_PERIODO_FIM, "' AND"));
                }
                else if (filtros.PERIODO_FIM != DateTime.MaxValue)
                {
                    AdicionarParaTodos(string.Concat(" T.DATTRA <= ", "'", filtros.PERIODO_FIM.AddDays(1).ToString("yyyy-MM-dd"), "' AND"));
                }

                AdicionarParaTodosSe(filtros.NUM_HOST_INI > 0, string.Concat(" T.NSUHOS >= ", filtros.NUM_HOST_INI + " AND"));
                AdicionarParaTodosSe(filtros.NUM_HOST_FIM > 0, string.Concat(" T.NSUHOS <= ", filtros.NUM_HOST_FIM + " AND"));

                AdicionarParaTodosSe(filtros.NUM_AUT_INI > 0, string.Concat(" T.NSUAUT >= ", filtros.NUM_AUT_INI, " AND"));
                AdicionarParaTodosSe(filtros.NUM_AUT_FIM > 0, string.Concat(" T.NSUAUT <= ", filtros.NUM_AUT_FIM, " AND"));

                AdicionarParaTodosSe(!string.IsNullOrEmpty(filtros.NOME_USUARIO), string.Concat(" U.NOMUSU LIKE '", filtros.NOME_USUARIO, "%' AND"));

                AdicionarParaTodosSe(!string.IsNullOrEmpty(filtros.CPF_USUARIO), string.Concat(" T.CPF = '", filtros.CPF_USUARIO, "' AND"));
                AdicionarParaTodosSe(!string.IsNullOrEmpty(filtros.MAT_USUARIO), string.Concat(" U.MAT LIKE '" + filtros.MAT_USUARIO + "' AND"));

                if (!string.IsNullOrEmpty(filtros.SUBREDE))
                {
                    AdicionarParaTodos(string.Concat(" C.CODSUBREDE IN ( ", filtros.SUBREDE.Remove(filtros.SUBREDE.Length - 1, 1), " ) AND"));
                }

                if (!string.IsNullOrEmpty(filtros.TIPO_TRANSACAO))
                {
                    AdicionarParaTodos(string.Concat(" T.TIPTRA IN ( ", filtros.TIPO_TRANSACAO.Remove(filtros.TIPO_TRANSACAO.Length - 1, 1), " ) AND"));
                }

                if (!string.IsNullOrEmpty(filtros.REDE))
                {
                    string codigosRede = string.Empty;

                    foreach (var item in filtros.REDE.Remove(filtros.REDE.Length - 1, 1).Split(','))
                    {
                        codigosRede += "'" + item + "'" + ",";
                    }

                    AdicionarParaTodos(string.Concat(" T.REDE in ( ", codigosRede.Remove(codigosRede.Length - 1, 1), " ) AND"));
                }

                if (!string.IsNullOrEmpty(filtros.TIPO_RESPOSTA))
                {
                    AdicionarParaTodos(string.Concat(" T.CODRTA in ( ", filtros.TIPO_RESPOSTA.Remove(filtros.TIPO_RESPOSTA.Length - 1, 1), " ) AND"));
                }

                AdicionarParaTodosSe(!string.IsNullOrEmpty(filtros.NUM_CARTAO), string.Concat(" T.CODCRT = '", filtros.NUM_CARTAO, "' AND"));

                if (!string.IsNullOrEmpty(filtros.LISTA_CLI))
                {
                    AdicionarParaTodos(filtros.LISTA_CLI.Substring(filtros.LISTA_CLI.Length - 1, 1) == ","
                    ? string.Concat(" T.CODCLI in ( ", filtros.LISTA_CLI.Remove(filtros.LISTA_CLI.Length - 1, 1), " ) AND")
                    : string.Concat(" T.CODCLI in ( ", filtros.LISTA_CLI, " ) AND"));
                }
                else
                {
                    AdicionarParaTodosSe(!string.IsNullOrEmpty(filtros.INTERVALO_CLI_INI) && !string.IsNullOrEmpty(filtros.INTERVALO_CLI_FIM),
                    string.Concat(" T.CODCLI >= ", filtros.INTERVALO_CLI_INI, " AND T.CODCLI <= ", filtros.INTERVALO_CLI_FIM, "  AND"));
                }

                if (!string.IsNullOrEmpty(filtros.LISTA_CRED))
                {
                    var condicao = string.Concat(" T.{0} IN ( ", filtros.LISTA_CRED.Remove(filtros.LISTA_CRED.Length - 1, 1), " ) AND");
                    AdicionarAutorizador(string.Format(condicao, "CODPS"));
                    AdicionarNetCardVA(string.Format(condicao, "CODCRE"));
                }
                else if (!string.IsNullOrEmpty(filtros.INTERVALO_CRED_INI) && !string.IsNullOrEmpty(filtros.INTERVALO_CRED_FIM))
                {
                    var condicao = string.Concat(" T.{0} >= ", filtros.INTERVALO_CRED_INI, " AND T.{0} <= ", filtros.INTERVALO_CRED_FIM, "  AND");
                    AdicionarAutorizador(string.Format(condicao, "CODPS"));
                    AdicionarNetCardVA(string.Format(condicao, "CODCRE"));
                }

                AdicionarAutorizador(" T.PROCESSADA = 'N' AND ");
                AdicionarAutorizadorSe(filtros.CODCEN > 0, string.Concat(" CR.CODCEN = ", filtros.CODCEN, " AND"));

                AdicionarAutorizadorSe(filtros.SISTEMA == 0, " NOT EXISTS (SELECT T2.DATTRA FROM TRANSACAO T2 WITH (NOLOCK) WHERE T2.DATTRA = T.DATTRA AND T2.NSUHOS = T.NSUHOS AND T2.NSUAUT = T.NSUAUT) ");
                AdicionarAutorizadorSe(filtros.SISTEMA == 1, " NOT EXISTS (SELECT T2.DATTRA FROM TRANSACVA T2 WITH (NOLOCK) WHERE T2.DATTRA = T.DATTRA AND T2.NSUHOS = T.NSUHOS AND T2.NSUAUT = T.NSUAUT) ");

                AdicionarNetCardVASe(filtros.DATA_FECH_CRED_INI != DateTime.MinValue, string.Concat(" T.DATFECCRE >= '", filtros.DATA_FECH_CRED_INI.ToString("yyyy-MM-dd"), "' AND"));
                AdicionarNetCardVASe(filtros.DATA_FECH_CRED_FIM != DateTime.MinValue, string.Concat(" T.DATFECCRE <= '", filtros.DATA_FECH_CRED_FIM.ToString("yyyy-MM-dd"), "' AND"));
                AdicionarNetCardVASe(filtros.NUM_FECH_CRED_INI > 0, string.Concat(" T.NUMFECCRE >= ", filtros.NUM_FECH_CRED_INI, " AND"));
                AdicionarNetCardVASe(filtros.NUM_FECH_CRED_FIM > 0, string.Concat(" T.NUMFECCRE <= ", filtros.NUM_FECH_CRED_FIM, " AND"));

                if (filtros.SISTEMA == 0) {
                    AdicionarNetCardVASe(filtros.DATA_FECH_CLI_INI != DateTime.MinValue, string.Concat(" T.DATFECCLI >= '", filtros.DATA_FECH_CLI_INI.ToString("yyyy-MM-dd"), "' AND"));
                    AdicionarNetCardVASe(filtros.DATA_FECH_CLI_FIM != DateTime.MinValue, string.Concat(" T.DATFECCLI <= '", filtros.DATA_FECH_CLI_FIM.ToString("yyyy-MM-dd"), "' AND"));
                    AdicionarNetCardVASe(filtros.NUM_FECH_CLI_INI > 0, string.Concat(" T.NUMFECCLI >= ", filtros.NUM_FECH_CLI_INI, " AND"));
                    AdicionarNetCardVASe(filtros.NUM_FECH_CLI_FIM > 0, string.Concat(" T.NUMFECCLI <= ", filtros.NUM_FECH_CLI_FIM, " AND"));
                }

                AdicionarNetCardVASe(filtros.CODCEN > 0, string.Concat(" CR.CODCEN = ", filtros.CODCEN, " AND"));
            }

            private string ConfiguraConsulta(CONSULTA_VA consultaVA, bool count)
            {
                CriaWhere(consultaVA);

                var sql = new StringBuilder();

                sql.AppendLine(consultaVA.SISTEMA == 0 ? CriaSqlNetCardPosPago(count) : CriaSqlNetCardPrePago(count));
                sql.AppendLine(GetWhere(_whereNetCard).ToString());
                sql.AppendLine("UNION");
                sql.AppendLine(consultaVA.SISTEMA == 0 ? CriaSqlAutorizadosPosPago(count, _operadora.BANCOAUT) : CriaSqlAutorizadosPrePago(count, _operadora.BANCOAUT));
                sql.AppendLine(GetWhere(_whereAutorizador).ToString());

                if (!count)
                {
                    sql.AppendLine("ORDER BY T.DATTRA");
                }

                return sql.ToString();
            }

            public string CriaSqlContadorTransacoes(CONSULTA_VA consultaVA)
            {
                return string.Format("SELECT SUM(TOTAL) AS QUANTIDADE FROM ({0}) RESULT", ConfiguraConsulta(consultaVA, true));
            }

            public string CriaSqlConsulta(CONSULTA_VA consultaVA)
            {
                return ConfiguraConsulta(consultaVA, false);
            }
        }

        private readonly string _bdTelenet = string.Empty;
        private readonly OPERADORA _operadora;

        private string AtualizaTrans(int sistema, string dattra, string numHost, string nsuAut, string justificativa)
        {
            Database database = new SqlDatabase(_bdTelenet);
            using (var comando = database.GetStoredProcCommand("PROC_CONFIRMA_TRANS"))
            {
                comando.CommandType = CommandType.StoredProcedure;
                database.AddInParameter(comando, "SISTEMA", DbType.Int32, sistema);
                database.AddInParameter(comando, "DATTRA", DbType.DateTime, Convert.ToDateTime(dattra));
                database.AddInParameter(comando, "NSUHOS", DbType.Int32, Convert.ToInt32(numHost));
                database.AddInParameter(comando, "NSUAUT", DbType.Int32, Convert.ToInt32(nsuAut));

                if (!string.IsNullOrEmpty(justificativa))
                {
                    database.AddInParameter(comando, "JUSTIFIC", DbType.String, justificativa);
                }

                database.AddInParameter(comando, "ID_FUNC", DbType.Int32, _operadora.ID_FUNC);


                try
                {
                    using (var reader = database.ExecuteReader(comando))
                    {
                        return reader.Read() ? Convert.ToString(reader["RETORNO"]) : string.Empty;
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }

        public long ObterNumeroTransacoes(CONSULTA_VA filtros)
        {
            var sql = new SqlTransacao(_operadora).CriaSqlContadorTransacoes(filtros);
            var database = new SqlDatabase(_bdTelenet);

            using (var comando = database.GetSqlStringCommand(sql))
            {
                var quantidade = database.ExecuteScalar(comando);
                return quantidade == DBNull.Value ? 0 : Convert.ToInt64(quantidade);
            }
        }

        public List<CTTRANSVA> GeraConsultaTransacao(CONSULTA_VA filtros)
        {
            if (ObterNumeroTransacoes(filtros) > _operadora.NUMCONSREGTRANS)
            {
                return null;
            }

            var sql = new SqlTransacao(_operadora).CriaSqlConsulta(filtros);
            var colecaoTransacao = new List<CTTRANSVA>();
            var database = new SqlDatabase(_bdTelenet);

            using (var comando = database.GetSqlStringCommand(sql))
            using (var reader = database.ExecuteReader(comando))
            {
                while (reader.Read())
                {
                    var transacao = new CTTRANSVA
                    {
                        SUBREDE = Convert.ToString(reader["SUBREDE"]),
                        DATTRA = reader["DATA"] != DBNull.Value ? Convert.ToDateTime(reader["DATA"]).ToString("dd/MM/yyyy HH:mm:ss") : "",
                        NSUHOS = Convert.ToString(reader["Nº HOST"]),
                        CODCLI = Convert.ToString(reader["COD. CLIENTE"]),
                        NOMCLI = Convert.ToString(reader["NOME CLIENTE"]),
                        CODCRE = Convert.ToString(reader["CREDENCIADO"]),
                        RAZSOC = Convert.ToString(reader["RAZAO SOCIAL_CRED"]),
                        TIPTRA = Convert.ToString(reader["TIPO TRANS"]),
                        CODCRT = Convert.ToString(reader["NUM CARTAO"]),
                        MAT = reader["MATRICULA_USU"].ToString(),
                        CODFIL = reader["FILIAL_USU"].ToString(),
                        CODSET = reader["SETOR_USU"].ToString(),
                        CODRTA = Convert.ToString(reader["TIPO RESP"]),
                        VALTRA = Convert.ToString(reader["VALOR TRANS"]),
                        NSUAUT = Convert.ToString(reader["Nº AUT"]),
                        CPF = Convert.ToString(reader["CPF"]),
                        NUMDEP = Convert.ToString(reader["Nº SEQ. DEPT"]),
                        NOMUSU = Convert.ToString(reader["NOME USUARIO"]),
                        DATFECCRE = reader["DATFECCRE"] != DBNull.Value ? Convert.ToDateTime(reader["DATFECCRE"]).ToString("dd/MM/yyyy") : "",
                        NUMFECCRE = Convert.ToString(reader["NUMFECCRE"]),
                        DATPGTOCRE = reader["DATPGTOCRE"] != DBNull.Value ? Convert.ToDateTime(reader["DATPGTOCRE"]).ToString("dd/MM/yyyy") : "",
                        NUMCARG_VA = Convert.ToString(reader["NUM_CARGA"]),
                        NOMFAN = Convert.ToString(reader["NOMFAN"]),
                        DESTIPTRA = Convert.ToString(reader["DESTIPTRA"]),
                        CGC = reader["CGC"].ToString(),
                        DAD = reader["DAD"].ToString(),
                        NOMSEG = Convert.ToString(reader["NOMSEG"]),
                        REDE = Convert.ToString(reader["REDE"]),
                        FLAG_AUT = Convert.ToString(reader["FLAG_AUT"]),
                        DAD_JUST = Convert.ToString(reader["DAD_JUST"]),

                        TVALOR = Convert.ToString(reader["TVALOR"]),
                        PARCELA = Convert.ToString(reader["PARCELA"]),
                        TPARCELA = Convert.ToString(reader["TPARCELA"])
                    };

                    if (filtros.SISTEMA == 0)
                    {
                        transacao.DATFECCLI = reader["DATFECCLI"] != DBNull.Value ? Convert.ToDateTime(reader["DATFECCLI"]).ToString("dd/MM/yyyy") : "";
                        transacao.NUMFECCLI = Convert.ToString(reader["NUMFECCLI"]);
                        transacao.DATPGTOCLI = reader["DATPGTOCLI"] != DBNull.Value ? Convert.ToDateTime(reader["DATPGTOCLI"]).ToString("dd/MM/yyyy") : "";
                    }

                    transacao.ORIGEMOP = reader["ORIGEM OP"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ORIGEM OP"]);
                    transacao.NOMOPERADOR = reader["NOME OPERADOR"] == DBNull.Value ? string.Empty : Convert.ToString(reader["NOME OPERADOR"]);

                    colecaoTransacao.Add(transacao);
                }
            }

            return colecaoTransacao;
        }

        //public List<CTTRANSVA> ListaTransacoesNetcardAutorizador(CONSULTA filtros)
        //{
        //    var retorno = new List<CTTRANSVA>();

        //    using (var conexao = new SqlConnection(_bdTelenet))
        //    using (var comando = conexao.CreateCommand())
        //    {
        //        comando.CommandType = CommandType.StoredProcedure;
        //        comando.CommandText = "PROC_CONS_TRANS_USUARIO";
        //        comando.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = filtros.SISTEMA;
        //        comando.Parameters.Add("@CODCLI", SqlDbType.Int).Value = filtros.CODCLI;
        //        comando.Parameters.Add("@CODCRE", SqlDbType.Int).Value = filtros.CODCRE;
        //        comando.Parameters.Add("@CPF", SqlDbType.VarChar).Value = filtros.CPF_USUARIO;
        //        comando.Parameters.Add("@LOTE", SqlDbType.Int).Value = filtros.LOTE;
        //        if (filtros.PERIODO_INI != DateTime.MinValue) comando.Parameters.Add("@DATAINI", SqlDbType.DateTime).Value = filtros.PERIODO_INI;
        //        if (filtros.PERIODO_FIM != DateTime.MinValue) comando.Parameters.Add("@DATAFIM", SqlDbType.DateTime).Value = filtros.PERIODO_FIM;
        //        comando.Connection = conexao;

        //        conexao.Open();

        //        using (var reader = comando.ExecuteReader())
        //        {
        //            var dataTable = new DataTable();
        //            dataTable.Load(reader);

        //            conexao.Close();


        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                var transacao = new CTTRANSVA
        //                {
        //                    SUBREDE = row["SUBREDE"].ToString(),
        //                    DATTRA = Convert.ToDateTime(row["DATTRA"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
        //                    NSUHOS = row["NSUHOS"].ToString(),
        //                    CODCLI = row["CODCLI"].ToString(),
        //                    NOMCLI = row["CLIENTE"].ToString(),
        //                    CODCRE = row["CODCRE"].ToString(),
        //                    RAZSOC = row["RAZSOC"].ToString(),
        //                    TIPTRA = row["TIPTRA"].ToString(),
        //                    CODCRT = row["CODCRT"].ToString(),
        //                    MAT = row["MATRICULA"].ToString(),
        //                    CODFIL = row["FILIAL"].ToString(),
        //                    CODSET = row["SETOR"].ToString(),
        //                    CODRTA = row["CODRTA"].ToString(),
        //                    VALTRA = row["VALTRA"].ToString(),
        //                    TVALOR = row["TVALOR"].ToString(),
        //                    PARCELA = row["PARCELA"].ToString(),
        //                    TPARCELA = row["TPARCELA"].ToString(),
        //                    NSUAUT = row["NSUAUT"].ToString(),
        //                    CPF = row["CPF"].ToString(),
        //                    NUMDEP = row["NUMDEP"].ToString(),
        //                    NOMUSU = row["NOMUSU"].ToString(),
        //                    DATFECCRE = !string.IsNullOrEmpty(row["DATFECCRE"].ToString()) ? Convert.ToDateTime(row["DATFECCRE"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
        //                    NUMFECCRE = row["NUMFECCRE"].ToString(),
        //                    //DATPGTOCRE = !string.IsNullOrEmpty(row["DATPGTO"].ToString()) ? Convert.ToDateTime(row["DATPGTO"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
        //                    NUMCARG_VA = row["NUM_CARGA"].ToString(),
        //                    NOMFAN = row["NOMFAN"].ToString(),
        //                    DESTIPTRA = row["DESCRICAO"].ToString(),
        //                    CGC = row["CNPJ"].ToString(),
        //                    DAD = row["DAD"].ToString(),
        //                    NOMSEG = row["SEGMENTO"].ToString(),
        //                    REDE = row["REDE"].ToString(),
        //                    ORIGEMOP = row["ORIGEM"] == DBNull.Value ? string.Empty : row["ORIGEM"].ToString(),
        //                    NOMOPERADOR = row["OPERADOR"] == DBNull.Value ? string.Empty : row["OPERADOR"].ToString(),
        //                    TIPTRANS = row["TIPTRANS"].ToString()
        //                };



        //                retorno.Add(transacao);
        //            }

        //            return retorno;
        //        }
        //    }
        //}

        public List<CTTRANSVA> ListaTransacoesNetcardAutorizador(CONSULTA filtros, out string valTot)
        {
            valTot = "0";
            var retorno = new List<CTTRANSVA>();            
            Database db = new SqlDatabase(_bdTelenet);
            DbCommand cmd = db.GetStoredProcCommand("PROC_CONS_TRANS_USUARIO");            

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, filtros.SISTEMA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, filtros.CODCLI);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, filtros.CODCRE);
            db.AddInParameter(cmd, "CPF", DbType.String, filtros.CPF_USUARIO);
            db.AddInParameter(cmd, "LOTE", DbType.Int32, filtros.LOTE);
            

            if (filtros.PERIODO_INI != DateTime.MinValue)
                db.AddInParameter(cmd, "DATAINI", DbType.DateTime, filtros.PERIODO_INI);

            if (filtros.PERIODO_FIM != DateTime.MinValue)
                db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, filtros.PERIODO_FIM);


            db.AddOutParameter(cmd, "VALTOTAL", DbType.String, 20);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {                
                var transacao = new CTTRANSVA
                {
                    SUBREDE = idr["SUBREDE"].ToString(),
                    DATTRA = Convert.ToDateTime(idr["DATTRA"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    NSUHOS = idr["NSUHOS"].ToString(),
                    CODCLI = idr["CODCLI"].ToString(),
                    NOMCLI = idr["CLIENTE"].ToString(),
                    CODCRE = idr["CODCRE"].ToString(),
                    RAZSOC = idr["RAZSOC"].ToString(),
                    TIPTRA = idr["TIPTRA"].ToString(),
                    CODCRT = idr["CODCRT"].ToString(),
                    MAT = idr["MATRICULA"].ToString(),
                    CODFIL = idr["FILIAL"].ToString(),
                    CODSET = idr["SETOR"].ToString(),
                    CODRTA = idr["CODRTA"].ToString(),
                    VALTRA = idr["VALTRA"].ToString(),
                    TVALOR = idr["TVALOR"].ToString(),
                    PARCELA = idr["PARCELA"].ToString(),
                    TPARCELA = idr["TPARCELA"].ToString(),
                    NSUAUT = idr["NSUAUT"].ToString(),
                    CPF = idr["CPF"].ToString(),
                    NUMDEP = idr["NUMDEP"].ToString(),
                    NOMUSU = idr["NOMUSU"].ToString(),
                    DATFECCRE = !string.IsNullOrEmpty(idr["DATFECCRE"].ToString()) ? Convert.ToDateTime(idr["DATFECCRE"].ToString()).ToString("dd/MM/yyyy") : string.Empty,
                    NUMFECCRE = idr["NUMFECCRE"].ToString(),                    
                    NUMCARG_VA = idr["NUM_CARGA"].ToString(),
                    NOMFAN = idr["NOMFAN"].ToString(),
                    DESTIPTRA = idr["DESCRICAO"].ToString(),
                    CGC = idr["CNPJ"].ToString(),
                    DAD = idr["DAD"].ToString(),
                    NOMSEG = idr["SEGMENTO"].ToString(),
                    REDE = idr["REDE"].ToString(),
                    ORIGEMOP = idr["ORIGEM"] == DBNull.Value ? string.Empty : idr["ORIGEM"].ToString(),
                    NOMOPERADOR = idr["OPERADOR"] == DBNull.Value ? string.Empty : idr["OPERADOR"].ToString(),
                    TIPTRANS = idr["TIPTRANS"].ToString()
                };
                retorno.Add(transacao);
                valTot = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@VALTOTAL"));
            }
            return retorno;
        }

        public List<TIPTRANS> ListaTipoTrans()
        {
            var database = new SqlDatabase(_bdTelenet);
            using (var cmd = database.GetSqlStringCommand("SELECT TIPTRA, DESTIPTRA FROM TIPTRANS WITH (NOLOCK) ORDER BY TIPTRA"))
            using (var reader = database.ExecuteReader(cmd))
            {
                var colecaoTipoTrans = new List<TIPTRANS>();
                try
                {
                    while (reader.Read())
                    {
                        colecaoTipoTrans.Add(new TIPTRANS
                        {
                            TIPTRA = Convert.ToInt32(reader["TIPTRA"]),
                            DESTIPTRA = (reader["DESTIPTRA"].ToString())
                        });
                    }
                    return colecaoTipoTrans;
                }
                catch
                {
                    throw new Exception("Erro ao criar a lista de Transacoes");
                }
            }
        }

        public List<TRANSACAO_VA> ListaTotalTransAnalitico(Hashtable filtros)
        {
            IDataReader idr = null;
            var Selecao = filtros["Selecao"].ToString();
            var Formato = filtros["Formato"].ToString();
            var Condicao = filtros["Condicao"].ToString();

            Database db = new SqlDatabase(_bdTelenet);
            const string sql = "PROC_REL_TOTALTRANSACOES";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "@SELECAO", DbType.Byte, Selecao);
            db.AddInParameter(cmd, "@FORMATO", DbType.Byte, Formato);
            db.AddInParameter(cmd, "@COND", DbType.Byte, Condicao);

            if (!string.IsNullOrEmpty(filtros["ParamIni"].ToString())) db.AddInParameter(cmd, "PARAM_INI", DbType.String, filtros["ParamIni"].ToString());
            else db.AddInParameter(cmd, "PARAM_INI", DbType.String, DBNull.Value);

            if (!string.IsNullOrEmpty(filtros["ParamFim"].ToString())) db.AddInParameter(cmd, "PARAM_FIM", DbType.String, filtros["ParamFim"].ToString());
            else db.AddInParameter(cmd, "PARAM_FIM", DbType.String, DBNull.Value);

            if (Convert.ToDateTime(filtros["DataIni"]) != DateTime.MinValue) db.AddInParameter(cmd, "@DATA_INI", DbType.String, Convert.ToDateTime(filtros["DataIni"]).ToString("yyyyMMdd"));
            else db.AddInParameter(cmd, "@DATA_INI", DbType.String, DBNull.Value);

            if (Convert.ToDateTime(filtros["DataFim"]) != DateTime.MaxValue) db.AddInParameter(cmd, "@DATA_FIM", DbType.String, Convert.ToDateTime(filtros["DataFim"]).ToString("yyyyMMdd"));
            else db.AddInParameter(cmd, "@DATA_FIM", DbType.String, DBNull.Value);

            db.AddInParameter(cmd, "@ORDEM", DbType.String, DBNull.Value);

            var lista = new List<TRANSACAO_VA>();

            try
            {
                idr = db.ExecuteReader(cmd);
                var countTrans = 0; //quantAtiv = 0, quantPend = 0, quantRec = 0 ;
                while (idr.Read())
                {
                    var trans = new TRANSACAO_VA();
                    if (Selecao == "0")// Cliente
                    {
                        trans.CODIGO = Convert.ToInt32(idr["CODCLI"]);
                        trans.CODAUX = Convert.ToInt32(idr["CODCRE"]);
                        trans.DESCRICAO = idr["NOMCLI"].ToString();
                        countTrans++;
                    }
                    else// Credenciado
                    {
                        trans.CODIGO = Convert.ToInt32(idr["CODCRE"]);
                        trans.CODAUX = Convert.ToInt32(idr["CODCLI"]);
                        trans.DESCRICAO = idr["NOMCRE"].ToString();
                        countTrans++;
                    }

                    trans.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                    trans.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                    trans.DATTRA = Convert.ToDateTime(idr["DATA"]).ToString("dd/MM/yyyy hh:mm:ss");//Data
                    trans.NUMHOST = idr["NUMHOST"].ToString();
                    trans.NUMAUT = idr["NUMAUT"].ToString();
                    trans.TIPTRA = Convert.ToString(idr["TIPTRA"]);//Codigo da transacao
                    trans.CODCRT = Convert.ToString(idr["CODCRT"]); // Codigo Cartao                
                    trans.STATUS = Convert.ToChar(idr["STATUS"]);
                    trans.VALOR = Convert.ToDecimal(idr["VALOR"]);
                    trans.CPF = Convert.ToString(idr["CPF"]);
                    trans.NUMDEP = Convert.ToInt32(idr["NUMDEP"]);
                    trans.DATFECCRE = !string.IsNullOrEmpty(idr["DATFECCRE"].ToString()) ? Convert.ToDateTime(idr["DATFECCRE"]).ToString("dd/MM/yyyy") : "";
                    trans.NUMFECRE = Convert.ToString(idr["NUMFECCRE"]);
                    trans.DATPGTO = !string.IsNullOrEmpty(idr["DATPGTO"].ToString()) ? Convert.ToDateTime(idr["DATPGTO"]).ToString("dd/MM/yyyy") : "";
                    trans.ATV = idr["ATV"].ToString();
                    trans.TOTALGERAL = idr["TOTALGERAL"].ToString();
                    trans.MEDIAPC = idr["VALMEDIOPC"].ToString();
                    trans.PP = Convert.ToDecimal(idr["PP"]);
                    lista.Add(trans);
                }
                if (lista.Count > 0)
                    lista[0].TOTALTRANS = countTrans;
                idr.Close();
            }
            catch
            {
                if (idr != null)
                    idr.Close();
                throw new Exception("Erro ao criar o relatorio");
            }
            return lista;
        }

        public List<TRANSACAO_VA> ListaTotalTransSintetico(Hashtable filtros)
        {
            var Selecao = filtros["Selecao"].ToString();
            var Formato = filtros["Formato"].ToString();
            var Condicao = filtros["Condicao"].ToString();
            var Ordem = filtros["Ordem"].ToString();

            Database db = new SqlDatabase(_bdTelenet);
            const string sql = "PROC_REL_TOTALTRANSACOES";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "@SELECAO", DbType.Byte, Selecao);
            db.AddInParameter(cmd, "@FORMATO", DbType.Byte, Formato);
            db.AddInParameter(cmd, "@COND", DbType.Byte, Condicao);

            if (!string.IsNullOrEmpty(filtros["ParamIni"].ToString())) db.AddInParameter(cmd, "PARAM_INI", DbType.String, filtros["ParamIni"].ToString());
            else db.AddInParameter(cmd, "PARAM_INI", DbType.String, DBNull.Value);

            if (!string.IsNullOrEmpty(filtros["ParamFim"].ToString())) db.AddInParameter(cmd, "PARAM_FIM", DbType.String, filtros["ParamFim"].ToString());
            else db.AddInParameter(cmd, "PARAM_FIM", DbType.String, DBNull.Value);

            if (Convert.ToDateTime(filtros["DataIni"]) != DateTime.MinValue) db.AddInParameter(cmd, "@DATA_INI", DbType.String, Convert.ToDateTime(filtros["DataIni"]).ToString("yyyyMMdd"));
            else db.AddInParameter(cmd, "@DATA_INI", DbType.String, DBNull.Value);

            if (Convert.ToDateTime(filtros["DataFim"]) != DateTime.MaxValue) db.AddInParameter(cmd, "@DATA_FIM", DbType.String, Convert.ToDateTime(filtros["DataFim"]).ToString("yyyyMMdd"));
            else db.AddInParameter(cmd, "@DATA_FIM", DbType.String, DBNull.Value);

            if (!string.IsNullOrEmpty(Ordem)) db.AddInParameter(cmd, "@ORDEM", DbType.Byte, Ordem);

            var lista = new List<TRANSACAO_VA>();
            var totalTransGeral = 0;
            decimal valotTotalGeral = 0m, mediaGeral = 0m;
            IDataReader idr = null;
            try
            {
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var trans = new TRANSACAO_VA();
                    trans.DESCRICAO = idr["DESCRICAO"].ToString();
                    trans.CODIGO = Convert.ToInt32(idr["CODIGO"]);
                    trans.TOTALTRANS = Convert.ToInt32(idr["TOTALTRANS"]);
                    trans.VALOR = Convert.ToDecimal(idr["VALORTOTAL"]);
                    trans.MEDIA = Convert.ToDecimal(idr["VALORMEDIO"]);
                    trans.TOTALGERAL = idr["TOTALGERAL"].ToString();
                    trans.MEDIAPC = idr["VALMEDIOPC"].ToString();
                    trans.PP = Convert.ToDecimal(idr["PP"]);
                    totalTransGeral += trans.TOTALTRANS;
                    valotTotalGeral += trans.VALOR;
                    mediaGeral = (valotTotalGeral / totalTransGeral);

                    lista.Add(trans);
                }
                if (lista.Count > 0) { foreach (var trn in lista) trn.MEDIAGERALSINTETICO = mediaGeral.ToString("n2"); }
                idr.Close();
            }
            catch
            {
                if (idr != null)
                    idr.Close();
                throw new Exception("Erro ao criar o relatorio");
            }
            return lista;
        }

        public string AlterarValTrans(int sistema, string dattra, string numHost, string nsuAut, decimal valor)
        {
            Database database = new SqlDatabase(_bdTelenet);
            using (var comando = database.GetStoredProcCommand("PROC_ALTERA_VALOR_TRANS"))
            {
                comando.CommandType = CommandType.StoredProcedure;
                database.AddInParameter(comando, "SISTEMA", DbType.Int32, sistema);
                database.AddInParameter(comando, "DATTRA", DbType.DateTime, Convert.ToDateTime(dattra));
                database.AddInParameter(comando, "NSUHOS", DbType.Int32, Convert.ToInt32(numHost));
                database.AddInParameter(comando, "NSUAUT", DbType.Int32, Convert.ToInt32(nsuAut));
                database.AddInParameter(comando, "VALOR", DbType.Int32, Convert.ToInt32(valor));
                database.AddInParameter(comando, "ID_FUNC", DbType.Int32, _operadora.ID_FUNC);

                try
                {
                    using (var reader = database.ExecuteReader(comando))
                    {
                        return reader.Read() ? Convert.ToString(reader["RETORNO"]) : string.Empty;
                    }
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
        }
        public string CancelarTrans(int sistema, string dattra, string numHost, string nsuAut, string justificativa)
        {
            Database database = new SqlDatabase(_bdTelenet);
            using (var comando = database.GetStoredProcCommand("PROC_CANCELA_TRANS"))
            {
                comando.CommandType = CommandType.StoredProcedure;
                database.AddInParameter(comando, "SISTEMA", DbType.Int32, sistema);
                database.AddInParameter(comando, "DATTRA", DbType.DateTime, Convert.ToDateTime(dattra));
                database.AddInParameter(comando, "NSUHOS", DbType.Int32, Convert.ToInt32(numHost));
                database.AddInParameter(comando, "NSUAUT", DbType.Int32, Convert.ToInt32(nsuAut));
                database.AddInParameter(comando, "JUSTIFIC", DbType.String, justificativa);
                database.AddInParameter(comando, "ID_FUNC", DbType.Int32, _operadora.ID_FUNC);

                try
                {
                    using (var reader = database.ExecuteReader(comando))
                    {
                        return reader.Read() ? Convert.ToString(reader["RETORNO"]) : string.Empty;
                    }
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
        }

        public string ConfirmaTrans(int sistema, string dattra, string numHost, string nsuAut, string justificativa)
        {
            return AtualizaTrans(sistema, dattra, numHost, nsuAut, justificativa);
        }

        public string AlteraTrans(int sistema, string dattra, string numHost, string nsuAut)
        {
            return AtualizaTrans(sistema, dattra, numHost, nsuAut, null);
        }
    }
}
