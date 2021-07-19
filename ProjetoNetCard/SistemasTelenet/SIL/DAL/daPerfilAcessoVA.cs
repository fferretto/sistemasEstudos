using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daPerfilAcessoVA
    {
        readonly string BDTELENET = string.Empty;

        readonly OPERADORA FOperador;

        public daPerfilAcessoVA(OPERADORA Operador)
        {
            FOperador = Operador;
            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public PERFILACESSOVA GetPerfilAcessoVA(int id)
        {
            var sql = new StringBuilder();
            var PerfilAcessoVA = new PERFILACESSOVA();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  ID,");
            sql.AppendLine("  DESCRICAO,");
            sql.AppendLine("  DETALHAMENTO ");
            sql.AppendLine(" FROM PERFILACESSOVA WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE ID = @ID "));
            sql.AppendLine(" ORDER BY DESCRICAO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID", DbType.Int32, id);
            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {
                PerfilAcessoVA.ID = Convert.ToInt32(idr["ID"]);
                PerfilAcessoVA.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                PerfilAcessoVA.DETALHAMENTO = Convert.ToString(idr["DETALHAMENTO"]);
            }
            idr.Close();
            return PerfilAcessoVA;
        }

        public List<PERFILACESSOVA> ColecaoPerfilAcessoVA(string Filtro)
        {
            var ColecaoPerfilAcessoVA = new List<PERFILACESSOVA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine(" SELECT ");
            sql.AppendLine("  ID,");
            sql.AppendLine("  DESCRICAO,");
            sql.AppendLine("  DETALHAMENTO ");
            sql.AppendLine("  ,CODAG ");
            sql.AppendLine(" FROM PERFILACESSOVA WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE DESCRICAO LIKE '{0}' ", Filtro));
            sql.AppendLine(" ORDER BY DESCRICAO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var PerfilAcessoVA = new PERFILACESSOVA();
                PerfilAcessoVA.ID = Convert.ToInt32(idr["ID"]);
                PerfilAcessoVA.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                PerfilAcessoVA.DETALHAMENTO = Convert.ToString(idr["DETALHAMENTO"]);
                PerfilAcessoVA.CODAG = idr["CODAG"] == DBNull.Value ? 0 : Convert.ToInt32(idr["CODAG"]);
                ColecaoPerfilAcessoVA.Add(PerfilAcessoVA);
            }
            idr.Close();
            return ColecaoPerfilAcessoVA;
        }

        public List<PERFILACESSOVA> ColecaoPerfilAcessoVA()
        {
            var ColecaoPerfilAcessoVA = new List<PERFILACESSOVA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine(" SELECT ");
            sql.AppendLine("  ID,");
            sql.AppendLine("  DESCRICAO,");
            sql.AppendLine("  DETALHAMENTO ");
            sql.AppendLine(" FROM PERFILACESSOVA WITH (NOLOCK) ");
            sql.AppendLine(" ORDER BY DESCRICAO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var PerfilAcessoVA = new PERFILACESSOVA();
                PerfilAcessoVA.ID = Convert.ToInt32(idr["ID"]);
                PerfilAcessoVA.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                PerfilAcessoVA.DETALHAMENTO = Convert.ToString(idr["DETALHAMENTO"]);
                ColecaoPerfilAcessoVA.Add(PerfilAcessoVA);
            }
            idr.Close();
            return ColecaoPerfilAcessoVA;
        }

        public void Inserir(PERFILACESSOVA PerfilAcessoVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "INSERT INTO PERFILACESSOVA (DESCRICAO, DETALHAMENTO) VALUES (@DESCRICAO, @DETALHAMENTO)";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "DESCRICAO", DbType.String, PerfilAcessoVA.DESCRICAO);
                db.AddInParameter(cmd, "DETALHAMENTO", DbType.String, PerfilAcessoVA.DETALHAMENTO);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT PERFILACESSOVA", FOperador, cmd);
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

        public void Alterar(PERFILACESSOVA PerfilAcessoVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE PERFILACESSOVA SET DESCRICAO = @DESCRICAO, DETALHAMENTO = @DETALHAMENTO WHERE ID = @ID";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID", DbType.Int32, PerfilAcessoVA.ID);
                db.AddInParameter(cmd, "DESCRICAO", DbType.String, PerfilAcessoVA.DESCRICAO);
                db.AddInParameter(cmd, "DETALHAMENTO", DbType.String, PerfilAcessoVA.DETALHAMENTO);
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE PERFILACESSOVA", FOperador, cmd);
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

        public bool ValidaNomePerfil(PERFILACESSOVA PerfilAcessoVA)
        {
            var existePerfil = false;

            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd =
                new SqlCommand(
                    "SELECT DESCRICAO FROM PERFILACESSOVA WITH (NOLOCK) " +
                    "WHERE DESCRICAO = @DESCRICAO AND ID <> @ID",
                    conn) { CommandType = CommandType.Text };
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = PerfilAcessoVA.DESCRICAO;
            cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = PerfilAcessoVA.ID;

            try
            {
                conn.Open();
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                    existePerfil = true;
                dr.Close();

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                return existePerfil;
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        public void Excluir(PERFILACESSOVA PerfilAcessoVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE PERFILACESSOVA WHERE ID = @ID";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID", DbType.Int32, PerfilAcessoVA.ID);
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE PERFILACESSOVA", FOperador, cmd);
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

        public bool OperadorTelenet(int idPerfil)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DESCRICAO FROM PERFILACESSOVA WITH (NOLOCK) WHERE ID = " + idPerfil;
            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);
            bool retorno = true;

            while (idr.Read())
            {
                if (idr["DESCRICAO"].ToString().Trim() != "TELENET")
                    retorno = false;
            }
            return retorno;
        }
    }
}
