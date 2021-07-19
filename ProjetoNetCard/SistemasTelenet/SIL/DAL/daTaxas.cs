using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daTaxas
    {
        readonly string BDTELENET = string.Empty;

        readonly OPERADORA FOperador;

        public daTaxas(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public List<TAXAS_CONSULTA> ColecaoTaxasConsulta(string Filtro)
        {
            var ColecaoTaxas = new List<TAXAS_CONSULTA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODTAXA, ABREVTAXA, NOMTAXA, TRENOVA, TAXAVA.STATUS, STATUS.DESTA, TIPO ");
            sql.AppendLine("   FROM TAXAVA WITH (NOLOCK) INNER JOIN ");
            sql.AppendLine("        STATUS WITH (NOLOCK) ON STATUS.STA = TAXAVA.STATUS ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOMTAXA ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Taxas = new TAXAS_CONSULTA();
                Taxas.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                Taxas.ABREVTAXA = Convert.ToString(idr["ABREVTAXA"]);
                Taxas.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                Taxas.STATUS = idr["STATUS"].ToString();
                Taxas.DESTA = idr["DESTA"].ToString();
                Taxas.TRENOVA = idr["TRENOVA"].ToString();
                Taxas.TIPO = Convert.ToInt16(idr["TIPO"]) == 2 ? "Credenciado" : 
                    (Convert.ToInt16(idr["TIPO"]) == 3 ? "Cliente" : "Usuario");
                ColecaoTaxas.Add(Taxas);
            }
            idr.Close();
            return ColecaoTaxas;
        }

        public List<TAXAS_CONSULTA> ColecaoTaxasConsultaVaPj(string Filtro)
        {
            var ColecaoTaxas = new List<TAXAS_CONSULTA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT A.CODTAXA, A.ABREVTAXA, A.NOMTAXA, A.TRENOVA, B.STA, B.DESTA, A.TIPO, A.SISTEMA, A.CENTRALIZADORA, A.TAXADEFAULT ");
            sql.AppendLine("FROM VTAXA A WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN STATUS B WITH (NOLOCK) ON A.STATUS = B.STA ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY TIPO, NOMTAXA ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Taxas = new TAXAS_CONSULTA();
                Taxas.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                Taxas.ABREVTAXA = Convert.ToString(idr["ABREVTAXA"]);
                Taxas.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                Taxas.STATUS = idr["STA"].ToString();
                Taxas.DESTA = idr["DESTA"].ToString();
                Taxas.TRENOVA = idr["TRENOVA"].ToString();
                Taxas.TIPO = idr["TIPO"].ToString();// == 2 ? "Credenciado" : (Convert.ToInt16(idr["TIPO"]) == 3 ? "Cliente" : "Usuario");
                Taxas.DESCTIPO = idr["TIPO"].ToString() == "2" ? "Credenciado" : (Convert.ToInt16(idr["TIPO"]) == 3 ? "Cliente" : "Usuario");
                Taxas.SISTEMA = idr["SISTEMA"].ToString();
                Taxas.DESCSISTEMA = idr["SISTEMA"].ToString() == "0" ? "Pós-pago" : "Pré-pago";
                Taxas.CENTRALIZADORA = Convert.ToString(idr["CENTRALIZADORA"]);
                Taxas.TAXADEFAULT = Convert.ToString(idr["TAXADEFAULT"]);
                ColecaoTaxas.Add(Taxas);
            }
            idr.Close();
            return ColecaoTaxas;
        }

        public TAXAVA GetTaxa(int idTaxa)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODTAXA, ABREVTAXA, NOMTAXA, TRENOVA, STATUS, TIPO, CENTRALIZADORA ");
            sql.AppendLine("   FROM TAXAVA WITH (NOLOCK) ");
            sql.AppendLine(" WHERE CODTAXA = " + idTaxa);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var Taxas = new TAXAVA();
            if (idr.Read())
            {
                Taxas.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                Taxas.ABREVTAXA = Convert.ToString(idr["ABREVTAXA"]);
                Taxas.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                Taxas.STATUS = idr["STATUS"].ToString();
                Taxas.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                Taxas.TIPO = Convert.ToInt16(idr["TIPO"]);
                Taxas.CENTRALIZADORA = Convert.ToString(idr["CENTRALIZADORA"]);
            }
            idr.Close();
            return Taxas;
        }

        public List<TAXAVA> ColecaoTaxas(string Filtro)
        {
            var ColecaoTaxas = new List<TAXAVA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODTAXA, ABREVTAXA, NOMTAXA, TRENOVA, STATUS ");
            sql.AppendLine("   FROM TAXAVA WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOMTAXA ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Taxas = new TAXAVA();
                Taxas.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                Taxas.ABREVTAXA = Convert.ToString(idr["LOGIN"]);
                Taxas.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                Taxas.STATUS = idr["STATUS"].ToString();
                Taxas.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                ColecaoTaxas.Add(Taxas);
            }
            idr.Close();
            return ColecaoTaxas;
        }

        public List<TAXAVAPJ> ColecaoTaxasVaPj(string filtro)
        {
            var ColecaoTaxas = new List<TAXAVAPJ>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODTAXA, ABREVTAXA, NOMTAXA, TRENOVA, STATUS, SISTEMA, TIPO, CENTRALIZADORA, TAXADEFAULT ");
            sql.AppendLine("   FROM VTAXA WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", filtro));
            sql.AppendLine(" ORDER BY NOMTAXA ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Taxas = new TAXAVAPJ();
                Taxas.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                Taxas.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                Taxas.ABREVTAXA = Convert.ToString(idr["ABREVTAXA"]);
                Taxas.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                Taxas.STATUS = idr["STATUS"].ToString();
                Taxas.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                Taxas.TIPO = Convert.ToInt16(idr["TIPO"]);                
                Taxas.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                Taxas.CENTRALIZADORA = Convert.ToString(idr["CENTRALIZADORA"]);
                Taxas.TAXADEFAULT = Convert.ToString(idr["TAXADEFAULT"]);
                ColecaoTaxas.Add(Taxas);
            }
            idr.Close();
            return ColecaoTaxas;
        }

        public List<STATUS> ColecaoStatus()
        {
            var listStatus = new List<STATUS>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT STA, DESTA ");
            sql.AppendLine("   FROM STATUS WITH (NOLOCK) ");
            sql.AppendLine(" WHERE STA IN ('00', '01', '02')");
            sql.AppendLine(" ORDER BY STA ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var status = new STATUS();
                status.STA = Convert.ToString(idr["STA"]);
                status.DESTA = Convert.ToString(idr["DESTA"]);
                listStatus.Add(status);
            }
            idr.Close();
            return listStatus;
        }

        public void Inserir(TAXAVA taxava)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "PROC_INSERE_TAXA";
                var cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
                db.AddInParameter(cmd, "ABREVTAXA", DbType.String, taxava.ABREVTAXA);
                db.AddInParameter(cmd, "NOMTAXA", DbType.String, taxava.NOMTAXA);
                db.AddInParameter(cmd, "STATUS", DbType.String, taxava.STATUS);
                db.AddInParameter(cmd, "TRENOVA", DbType.String, taxava.TRENOVA);
                db.AddInParameter(cmd, "TIPO", DbType.Int16, taxava.TIPO);
                db.ExecuteReader(cmd);
                UtilSIL.GravarLog(db, dbt, "INSERT TAXAVA (Operador VA)", FOperador, cmd);
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

        public bool Inserir(TAXAVAPJ taxa, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_TAXA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = taxa.SISTEMA;
                cmd.Parameters.Add("@ABREVTAXA", SqlDbType.VarChar).Value = taxa.ABREVTAXA;
                cmd.Parameters.Add("@NOMTAXA", SqlDbType.VarChar).Value = taxa.NOMTAXA;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = taxa.STATUS;
                cmd.Parameters.Add("@TRENOVA", SqlDbType.VarChar).Value = taxa.TRENOVA;
                cmd.Parameters.Add("@TIPO", SqlDbType.SmallInt).Value = taxa.TIPO;
                cmd.Parameters.Add("@CENTRALIZADORA", SqlDbType.VarChar).Value = taxa.CENTRALIZADORA;
                cmd.Parameters.Add("@TAXADEFAULT", SqlDbType.VarChar).Value = taxa.TAXADEFAULT;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                    if (Convert.ToString(reader[0]) == "OK")
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public void Alterar(TAXAVAPJ taxa)
        {
            
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "PROC_ALTERA_TAXA";
                var cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxa.CODTAXA);
                db.AddInParameter(cmd, "SISTEMA", DbType.Int32, taxa.SISTEMA);
                db.AddInParameter(cmd, "ABREVTAXA", DbType.String, taxa.ABREVTAXA);
                db.AddInParameter(cmd, "NOMTAXA", DbType.String, taxa.NOMTAXA);
                db.AddInParameter(cmd, "STATUS", DbType.String, taxa.STATUS);
                db.AddInParameter(cmd, "TRENOVA", DbType.String, taxa.TRENOVA);
                db.AddInParameter(cmd, "TIPO", DbType.Int16, taxa.TIPO);
                db.AddInParameter(cmd, "CENTRALIZADORA", DbType.String, taxa.CENTRALIZADORA);
                db.ExecuteReader(cmd);
                UtilSIL.GravarLog(db, dbt, "INSERT TAXAVA (Operador VA)", FOperador, cmd);
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

        public bool Excluir(TAXAVAPJ taxa, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_TAXA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CODTAXA", SqlDbType.Int).Value = taxa.CODTAXA;
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = taxa.SISTEMA;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                    if (Convert.ToString(reader[0]) == "OK")
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }

        }

        public void Alterar(TAXAVA taxava)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE TAXAVA SET ABREVTAXA = @ABREVTAXA, NOMTAXA = @NOMTAXA, TRENOVA = @TRENOVA, STATUS = @STATUS, CENTRALIZADORA = @CENTRALIZADORA  WHERE CODTAXA = @CODTAXA";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxava.CODTAXA);
                db.AddInParameter(cmd, "ABREVTAXA", DbType.String, taxava.ABREVTAXA);
                db.AddInParameter(cmd, "NOMTAXA", DbType.String, taxava.NOMTAXA);
                db.AddInParameter(cmd, "STATUS", DbType.String, taxava.STATUS);
                db.AddInParameter(cmd, "TRENOVA", DbType.String, taxava.TRENOVA);
                db.AddInParameter(cmd, "CENTRALIZADORA", DbType.String, taxava.CENTRALIZADORA);
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE TAXAVA (Operador VA)", FOperador, cmd);
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

        public void Excluir(TAXAVA taxava)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE TAXAVA WHERE CODTAXA = @CODTAXA";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxava.CODTAXA);
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE TAXAVA (Operador VA)", FOperador, cmd);
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

        //public void ExcluirTipTrans(TAXAVA taxava)
        //{
        //    Database db = new SqlDatabase(BDTELENET);
        //    var dbc = db.CreateConnection();
        //    dbc.Open();
        //    var dbt = dbc.BeginTransaction();
        //    try
        //    {
        //        const string sql = "DELETE TIPTRANS WHERE TIPTRA = (SELECT TIPTRA FROM TAXAVA WHERE CODTAXA = @CODTAXA)";
        //        var cmd = db.GetSqlStringCommand(sql);
        //        db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxava.CODTAXA);
        //        db.ExecuteNonQuery(cmd, dbt);
        //        UtilSIL.GravarLog(db, dbt, "DELETE TAXAVA (Operador VA)", FOperador, cmd);
        //        dbt.Commit();
        //    }
        //    catch (Exception err)
        //    {
        //        dbt.Rollback();
        //        throw new Exception(err.Message);
        //    }
        //    finally
        //    {
        //        dbc.Close();
        //    }
        //}

        public bool VerificaAbrev(string Abrev)
        {
            var AbrevIgual = false;
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT ABREVTAXA ");
            sql.AppendLine("   FROM TAXAVA WITH (NOLOCK) ");
            sql.AppendLine("  WHERE LOWER(ABREVTAXA) = LOWER(@ABREVTAXA) ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ABREVTAXA", DbType.String, Abrev);
            var AbrevBanco = Convert.ToString(db.ExecuteScalar(cmd));
            if (AbrevBanco.ToLower() == Abrev.ToLower())
                AbrevIgual = true;
            return AbrevIgual;
        }
    }
}
