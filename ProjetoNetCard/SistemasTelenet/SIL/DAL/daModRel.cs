using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace SIL.DAL
{
    public class daModRel
    {
        public daModRel(OPERADORA operador)
        {
            if (operador == null)
            {
                return;
            }
            _bdtelenet = string.Format(ConstantesSIL.BDTELENET, operador.SERVIDORNC, operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        private readonly string _bdtelenet = string.Empty;

        public RELATORIO ObterRelatorio(int idRel)
        {
            using (var connection = new SqlConnection(_bdtelenet))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID_REL, DESCRICAO, NOMREL, NOMPROC, TIPREL, NAVIGATEURL, SISTEMA FROM RELATORIO WITH (NOLOCK) WHERE ID_REL = @ID_REL";
                command.Parameters.AddWithValue("@ID_REL", idRel);
                connection.Open();

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    reader.Read();

                    return new RELATORIO
                    {
                        IDREL = Convert.ToInt32(reader["ID_REL"]),
                        NOMREL = Convert.ToString(reader["NOMREL"]),
                        DESCRICAO = Convert.ToString(reader["DESCRICAO"]),
                        PROCREL = Convert.ToString(reader["NOMPROC"]),
                        TIPREL = Convert.ToString(reader["TIPREL"]),
                        NAVIGATEURL = Convert.ToString(reader["NAVIGATEURL"]),
                        SISTEMA = Convert.ToString(reader["SISTEMA"])
                    };
                }
            }
        }

        public List<RELATORIO> ListaRelatorio()
        {
            Database db = new SqlDatabase(_bdtelenet);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT R.ID_REL, R.DESCRICAO, R.NOMREL, R.NOMPROC, R.TIPREL, R.NAVIGATEURL, R.SISTEMA " +
                           "FROM RELATORIO R WITH (NOLOCK) " +
                           "WHERE R.SISTEMA IN('VA', 'PJ', '*') AND R.MODULO IN('NC', '*') AND R.NOMPROC IS NOT NULL " +
                           "AND NOT EXISTS(SELECT A.ID_REL FROM AGRUP_RELATORIO A WITH (NOLOCK) WHERE A.ID_REL = R.ID_REL) " +
                           "ORDER BY R.TIPREL, R.NOMREL, R.SISTEMA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var listRel = new List<RELATORIO>();
            while (idr.Read())
            {
                var relatorio = new RELATORIO
                {
                    IDREL = Convert.ToInt32(idr["ID_REL"]),
                    NOMREL = Convert.ToString(idr["NOMREL"]),
                    DESCRICAO = Convert.ToString(idr["DESCRICAO"]),
                    PROCREL = Convert.ToString(idr["NOMPROC"]),
                    TIPREL = Convert.ToString(idr["TIPREL"]),
                    NAVIGATEURL = Convert.ToString(idr["NAVIGATEURL"]),
                    SISTEMA = Convert.ToString(idr["SISTEMA"])
                };
                listRel.Add(relatorio);
            }
            idr.Close();
            return listRel;
        }

        public List<RELATORIO> ListaRelatorio(int codAg)
        {
            Database db = new SqlDatabase(_bdtelenet);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT R.ID_REL, R.DESCRICAO, R.NOMREL, R.NOMPROC, R.TIPREL, R.NAVIGATEURL, SISTEMA ");
            sql.AppendLine("FROM RELATORIO R WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN AGRUP_RELATORIO G ON G.ID_REL = R.ID_REL");
            sql.AppendLine("WHERE G.CODAG = @CODAG AND R.SISTEMA IN('VA', 'PJ', '*') AND R.MODULO IN('NC', '*') AND R.NOMPROC IS NOT NULL ORDER BY R.TIPREL, R.NOMREL, R.SISTEMA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODAG", DbType.String, codAg);
            var idr = db.ExecuteReader(cmd);
            var listRel = new List<RELATORIO>();
            while (idr.Read())
            {
                var relatorio = new RELATORIO
                {
                    IDREL = Convert.ToInt32(idr["ID_REL"]),
                    NOMREL = Convert.ToString(idr["NOMREL"]),
                    DESCRICAO = Convert.ToString(idr["DESCRICAO"]),
                    PROCREL = Convert.ToString(idr["NOMPROC"]),
                    TIPREL = Convert.ToString(idr["TIPREL"]),
                    NAVIGATEURL = Convert.ToString(idr["NAVIGATEURL"]),
                    SISTEMA = Convert.ToString(idr["SISTEMA"])
                };
                listRel.Add(relatorio);
            }
            idr.Close();
            //DefineTipoCartaoNomeRelatorio(listRel);
            return listRel;
        }

        public List<PARAMETRO> ListaParametros(int idRel)
        {
            Database db = new SqlDatabase(_bdtelenet);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT R.ID_REL, R.NOMPROC, P.ID_PAR, P.DESPAR, R.NOMREL, ");
            sql.AppendLine("       P.NOMPAR, P.LABEL, P.TIPO, P.TAMANHO, _DEFAULT, P.REQUERIDO, P.HAS_DETAIL, R.SAIDA_ARQ_DIRETO, ");
            sql.AppendLine("       P.ORDEM_TELA, P.NOM_FUNCTION, R.DISPOSICAO ");
            sql.AppendLine("FROM RELATORIO R WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN PARAMETRO P WITH (NOLOCK) ON R.ID_REL = P.ID_REL");
            sql.AppendLine("WHERE R.ID_REL = " + idRel);
            sql.AppendLine(" ORDER BY P.ORDEM_TELA ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var listPar = new List<PARAMETRO>();
            while (idr.Read())
            {
                var parametro = new PARAMETRO
                {
                    IDREL = Convert.ToInt32(idr["ID_REL"]),
                    NOMPROC = Convert.ToString(idr["NOMPROC"]),
                    DISPOSICAO = Convert.ToString(idr["DISPOSICAO"]),
                    IDPAR = Convert.ToInt32(idr["ID_PAR"]),
                    DESPAR = Convert.ToString(idr["DESPAR"]),
                    NOMPAR = Convert.ToString(idr["NOMPAR"]),
                    LABEL = Convert.ToString(idr["LABEL"]),
                    TIPO = Convert.ToString(idr["TIPO"]),
                    TAMANHO = Convert.ToInt16(idr["TAMANHO"]),
                    HASDETAIL = Convert.ToString(idr["HAS_DETAIL"]),
                    NOMREL = Convert.ToString(idr["NOMREL"]),
                    DEFAULT = Convert.ToString(idr["_DEFAULT"]),
                    SAIDA_ARQ_DIRETO = Convert.ToString(idr["SAIDA_ARQ_DIRETO"]),
                    ORDEM_TELA = Convert.ToInt16(idr["ORDEM_TELA"]),
                    PARAMETRODINAMICO = false
                };

                if (idr["NOM_FUNCTION"] != DBNull.Value)
                {

                    var nomFunction = Convert.ToString(idr["NOM_FUNCTION"]);

                    if (nomFunction.Contains("@"))
                    {
                        parametro.NOM_FUNCTION = nomFunction.Substring(0, nomFunction.IndexOf('@') - 1);
                        parametro.PARAM_FUNCTION = nomFunction.Substring(nomFunction.IndexOf('@')).Replace(" ", "");
                    }
                    else
                    {
                        parametro.NOM_FUNCTION = nomFunction;
                        parametro.PARAM_FUNCTION = "";
                    }



                }

                listPar.Add(parametro);
            }

            idr.Close();
            return listPar;
        }

        public List<DETPAR> DetalheParametro(int idPar)
        {
            Database db = new SqlDatabase(_bdtelenet);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT TEXT, VALUE, TIPO ");
            sql.AppendLine("FROM DETALHES_PARAMETROS WITH (NOLOCK) ");
            sql.AppendLine("WHERE ID_PARAMETRO = " + idPar);

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var listDet = new List<DETPAR>();
            
            while (idr.Read())
            {
                var detalhe = new DETPAR();
                detalhe.TEXT = Convert.ToString(idr["TEXT"]);
                detalhe.VALUE = Convert.ToString(idr["VALUE"]);
                detalhe.TIPO = Convert.ToString(idr["TIPO"]);
                listDet.Add(detalhe);
            }

            idr.Close();
            return listDet;
        }

        public DataSet ListaDadosRelDataSet(List<PARAMETRO> parametros)
        {
            Database db = new SqlDatabase(_bdtelenet);
            var cmd = db.GetStoredProcCommand(parametros[0].NOMPROC);
            cmd.CommandTimeout = 300;

            foreach (var list in parametros)
            {
                var tipoPar = list.LABEL == "DateEdit" ? DbType.String : ValidaParametro(list.TIPO);
                var valFiltro = list.LABEL == "DateEdit" ? Convert.ToDateTime(list.VALOR).ToString("yyyyMMdd") : list.VALOR;
                if (string.IsNullOrEmpty(list.VALOR.ToString())) continue;
                db.AddInParameter(cmd, list.NOMPAR, tipoPar, valFiltro);                
            }

            var results = db.ExecuteDataSet(cmd);
            return results;
        }

        public List<MODREL> ListaDadosRel(List<PARAMETRO> parametros, string codope, out string erro)
        {
            var numCaracter = parametros[0].DISPOSICAO == "P" ? 172 : 118;
            erro = string.Empty;
            var listaFech = new List<MODREL>();
            Database db = new SqlDatabase(_bdtelenet);
            var cmd = db.GetStoredProcCommand(parametros[0].NOMPROC);
            cmd.CommandTimeout = 300;
            var nomProc = parametros[0].NOMPROC;
            var logParametros = string.Empty;

            foreach (var list in parametros)
            {
                var tipoPar = list.LABEL == "DateEdit" ? DbType.String : ValidaParametro(list.TIPO);
                var valFiltro = list.LABEL == "DateEdit" ? Convert.ToDateTime(list.VALOR).ToString("yyyyMMdd") : list.VALOR;
                if (string.IsNullOrEmpty(list.VALOR.ToString())) continue;
                db.AddInParameter(cmd, list.NOMPAR, tipoPar, valFiltro);
                logParametros += list.NOMPAR + " = " + valFiltro + ", ";
            }

            try
            {
                var idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    if (idr.FieldCount > 1)
                    {
                        var fech = new MODREL();
                        var linhaImp = Convert.ToString(idr["LINHAIMP"]).ToUpper();                        
                        fech.LINHAIMP = linhaImp;
                        fech.TIP = Convert.ToInt16(idr["TIP"]);
                        listaFech.Add(fech);
                    }
                    else
                        erro = Convert.ToString(idr["ERRO"]).ToUpper();
                }
                idr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GravarLogModRel(codope, nomProc, logParametros);
            }
            return listaFech;
        }

        private DbType ValidaParametro(string tipoPar)
        {
            var retorno = new DbType();
            switch (tipoPar)
            {
                case "String":
                    retorno = DbType.String;
                    break;
                case "Byte":
                    retorno = DbType.Byte;
                    break;
            }
            return retorno;
        }

        public void GravarLogModRel(string idOper, string nomProc, string logParametros)
        {
            Database db = new SqlDatabase(_bdtelenet);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "SP_GRAVA_LOG_MODREL";
                var cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "DATA", DbType.DateTime, DateTime.Now);
                db.AddInParameter(cmd, "IDOPER", DbType.String, idOper);
                db.AddInParameter(cmd, "NOMPROC", DbType.String, nomProc);
                db.AddInParameter(cmd, "PARAMETROS", DbType.String, logParametros);                
                var sistema = logParametros.Split(',').Where(x => x.Contains("@SISTEMA")).FirstOrDefault().Split('=')[1].Trim();
                if(!string.IsNullOrEmpty(sistema))
                    db.AddInParameter(cmd, "SISTEMA", DbType.Int32, Convert.ToInt32(sistema));
                
                db.ExecuteReader(cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public List<PARAM> FuncParametros(string nomFunc, List<PARAM> parametros)
        {
            var lista = new List<PARAM>();
            Database db = new SqlDatabase(_bdtelenet);
            var cmd = db.GetStoredProcCommand(nomFunc);
            cmd.CommandTimeout = 300;
            var nomProc = nomFunc;
            var logParametros = string.Empty;

            foreach (var list in parametros)
            {
                var tipoPar = ValidaParametro("String");
                if (string.IsNullOrEmpty(list.VAL.ToString())) continue;
                db.AddInParameter(cmd, list.ID0, tipoPar, list.VAL);
                logParametros += list.ID0 + ":" + list.VAL + ";";
            }

            try
            {
                var idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var val = new PARAM();
                    val.ID0 = Convert.ToString(idr["ID"]).ToUpper();
                    val.VAL = Convert.ToString(idr["VALOR"]);
                    lista.Add(val);
                }
                idr.Close();
            }
            catch (Exception ex) { return lista; }
            return lista;
        }

        public List<PARAM> ListaOpcoesDropDown(string nomProc)
        {
            var lista = new List<PARAM>();
            Database db = new SqlDatabase(_bdtelenet);
            var cmd = db.GetStoredProcCommand(nomProc);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            cmd.CommandTimeout = 300;
            var logParametros = string.Empty;

            try
            {
                DataTable dt = new DataTable();
                dt.Load(db.ExecuteReader(cmd));

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        PARAM itemDrop = new PARAM();

                        itemDrop.ID0 = Convert.ToString(item["ID"]).ToUpper();
                        itemDrop.VAL = Convert.ToString(item["VALOR"]);
                        lista.Add(itemDrop);
                    }
                }

                return lista;
            }
            catch 
            { 
                return lista; 
            }
        }
    }
}