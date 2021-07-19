using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;
using System.Data.SqlClient;

namespace TELENET.SIL.DA
{
    class daBeneficios
    {
        readonly string BDTELENET = string.Empty;

        readonly OPERADORA FOperador;

        public daBeneficios(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public int BuscarProximoCodigo()
        {
            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand("select ISNULL(MAX(CODBENEF), 0) CODBENEF from BENEFICIO WITH (NOLOCK) ", conexao);

            DataTable dt = new DataTable();

            try
            {
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                int codigo = Convert.ToInt32(dt.Rows[0]["CODBENEF"].ToString());

                codigo++;

                return codigo;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                throw new Exception("Erro ao buscar benefícios");
            }
        }

        public List<BENEFICIOS_CONSULTA> ColecaoBeneficiosConsulta(string Filtro)
        {
            var listBeneficios = new List<BENEFICIOS_CONSULTA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODBENEF, ABREVBENEF, NOMBENEF, TRENOVA, BENEFICIO.STATUS, STATUS.DESTA, TIPTRA ");
            sql.AppendLine("   FROM BENEFICIO WITH (NOLOCK) INNER JOIN ");
            sql.AppendLine("        STATUS ON STATUS.STA = BENEFICIO.STATUS ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOMBENEF ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var beneficio = new BENEFICIOS_CONSULTA();
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.ABREVBENEF = Convert.ToString(idr["ABREVBENEF"]);
                beneficio.NOMBENEF = Convert.ToString(idr["NOMBENEF"]);
                beneficio.STATUS = idr["STATUS"].ToString();
                beneficio.DESTA = idr["DESTA"].ToString();
                beneficio.TRENOVA = idr["TRENOVA"].ToString();
                beneficio.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                listBeneficios.Add(beneficio);
            }
            idr.Close();
            return listBeneficios;
        }        

        public BENEFICIO GetBenefico(int idBenef)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODBENEF, ABREVBENEF, NOMBENEF, TRENOVA, STATUS, TIPTRA ");
            sql.AppendLine("   FROM BENEFICIO WITH (NOLOCK) ");
            sql.AppendLine(" WHERE CODBENEF = " + idBenef);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            var beneficio = new BENEFICIO();
            if (idr.Read())
            {
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.ABREVBENEF = Convert.ToString(idr["ABREVBENEF"]);
                beneficio.NOMBENEF = Convert.ToString(idr["NOMBENEF"]);
                beneficio.STATUS = idr["STATUS"].ToString();
                beneficio.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                beneficio.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
            }
            idr.Close();
            return beneficio;
        }

        public List<BENEFICIO> ColecaoBeneficios(string Filtro)
        {
            var listBeneficios = new List<BENEFICIO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODBENEF, ABREVBENEF, NOMBENEF, TRENOVA, STATUS ");
            sql.AppendLine("   FROM BENEFICIO WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOMBENEF ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var beneficio = new BENEFICIO();
                beneficio.CODBENEF = Convert.ToInt32(idr["CODBENEF"]);
                beneficio.ABREVBENEF = Convert.ToString(idr["ABREVBENEF"]);
                beneficio.NOMBENEF = Convert.ToString(idr["NOMBENEF"]);
                beneficio.STATUS = idr["STATUS"].ToString();
                beneficio.TRENOVA = Convert.ToString(idr["TRENOVA"]);
                beneficio.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                listBeneficios.Add(beneficio);
            }
            idr.Close();
            return listBeneficios;
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

        public void Inserir(BENEFICIO beneficio)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "PROC_INSERE_BENEFICIO";
                var cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPOS);
                db.AddInParameter(cmd, "NOMBENEF", DbType.String, beneficio.NOMBENEF);
                db.AddInParameter(cmd, "ABREVBENEF", DbType.String, beneficio.ABREVBENEF);
                db.AddInParameter(cmd, "STATUS", DbType.String, beneficio.STATUS);
                db.ExecuteReader(cmd);
                UtilSIL.GravarLog(db, dbt, "INSERT BENEFICIO (Operador)", FOperador, cmd);
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

        public void Alterar(BENEFICIO beneficio)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE BENEFICIO SET ABREVBENEF = @ABREVBENEF, NOMBENEF = @NOMBENEF, TRENOVA = @TRENOVA, STATUS = @STATUS  WHERE CODBENEF = @CODBENEF";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODBENEF", DbType.Int32, beneficio.CODBENEF);
                db.AddInParameter(cmd, "ABREVBENEF", DbType.String, beneficio.ABREVBENEF);
                db.AddInParameter(cmd, "NOMBENEF", DbType.String, beneficio.NOMBENEF);
                db.AddInParameter(cmd, "STATUS", DbType.String, beneficio.STATUS);
                db.AddInParameter(cmd, "TRENOVA", DbType.String, beneficio.TRENOVA);
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE BENEFICIO (Operador VA)", FOperador, cmd);
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

        public void Excluir(BENEFICIO beneficio)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "PROC_EXCLUI_BENEFICIO";
                var cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPOS);
                db.AddInParameter(cmd, "CODBENEF", DbType.Int32, beneficio.CODBENEF);
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "EXCLUI BENEFICIO (Operador VA)", FOperador, cmd);
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

        public bool VerificaAbrev(string Abrev)
        {
            var AbrevIgual = false;
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT ABREVBENEF ");
            sql.AppendLine("   FROM BENEFICIO WITH (NOLOCK) ");
            sql.AppendLine("  WHERE LOWER(ABREVBENEF) = LOWER(@ABREVBENEF) ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ABREVBENEF", DbType.String, Abrev);
            var AbrevBanco = Convert.ToString(db.ExecuteScalar(cmd));
            if (AbrevBanco.ToLower() == Abrev.ToLower())
                AbrevIgual = true;
            return AbrevIgual;
        }

        /**************************************************************************************/        

        public int InserirBeneficio(BENEFICIO beneficio)
        {
            int retorno = 0;


            var beneficioExiste = BuscarBeneficio(beneficio.CODBENEF);

            if (beneficioExiste.CODBENEF > 0)
            {
                SqlConnection conexao = new SqlConnection(BDTELENET);
                string sql = "UPDATE BENEFICIO SET NOMBENEF = '" + beneficio.NOMBENEF + "', " + " STATUS = '" + beneficio.STATUS + "', " + "ABREVBENEF = '" + beneficio.ABREVBENEF + "' WHERE CODBENEF = " + beneficio.CODBENEF.ToString();
                SqlCommand comando = new SqlCommand(sql, conexao);

                try
                {
                    conexao.Open();
                    retorno = comando.ExecuteNonQuery();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception)
                {
                    if (conexao.State != ConnectionState.Closed)
                        conexao.Close();

                    throw;
                }
            }
            else
            {



                SqlConnection conexao = new SqlConnection(BDTELENET);
                SqlCommand comando = new SqlCommand("PROC_INSERE_BENEFICIO", conexao);
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add(new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" });
                comando.Parameters.Add(new SqlParameter() { Value = beneficio.NOMBENEF, DbType = DbType.String, ParameterName = "@NOMBENEF" });
                comando.Parameters.Add(new SqlParameter() { Value = beneficio.ABREVBENEF, DbType = DbType.String, ParameterName = "@ABREVBENEF" });
                comando.Parameters.Add(new SqlParameter() { Value = beneficio.STATUS, DbType = DbType.String, ParameterName = "@STATUS" });


                try
                {
                    conexao.Open();
                    retorno = comando.ExecuteNonQuery();
                    conexao.Close();

                    return retorno;
                }
                catch (Exception)
                {
                    if (conexao.State != ConnectionState.Closed)
                        conexao.Close();

                    throw;
                }

            }

        }

        public BENEFICIO BuscarBeneficio(int codBenef)
        {

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand("SELECT * FROM BENEFICIO WITH (NOLOCK) WHERE CODBENEF = " + codBenef.ToString(), conexao);


            BENEFICIO retorno = new BENEFICIO();

            try
            {
                var dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno.CODBENEF = Convert.ToInt32(dt.Rows[0]["CODBENEF"].ToString());
                    retorno.NOMBENEF = dt.Rows[0]["NOMBENEF"].ToString();
                    retorno.STATUS = dt.Rows[0]["STATUS"].ToString();
                    retorno.ABREVBENEF = dt.Rows[0]["ABREVBENEF"].ToString();
                }

                return retorno;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool ExcluirBeneficio(int codBenef)
        {
            int retorno = 0;

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand("PROC_EXCLUI_BENEFICIO ", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" });
            comando.Parameters.Add(new SqlParameter() { Value = codBenef, DbType = DbType.Int32, ParameterName = "@CODBENEF" });


            try
            {
                conexao.Open();
                retorno = comando.ExecuteNonQuery();
                conexao.Close();

                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<BENEF_CLIENTE> BuscarBeneficiosClientes(int codcli)
        {
            SqlConnection conexao = new SqlConnection(BDTELENET);

            string query = @"SELECT 
                            A.CODCLI ,
                            A.CODBENEF, 
                            A.VALTIT, 
                            A.VALDEP,
                            A.SUBBENEF,
                            A.PERSUB,
                            A.COMPULSORIO, 
                            A.DTASSOC, 
                            B.NOMBENEF,
                            A.CARENCIA, 
							A.VIGENCIA,
                            A.GRUPOFAMILIAR,
                            A.RENOVAUT,
                            A.COBCANC

                            FROM BENEFCLI A WITH (NOLOCK)
                            INNER JOIN BENEFICIO B ON A.CODBENEF = B.CODBENEF
                            WHERE A.CODCLI = " + codcli;

            SqlCommand comando = new SqlCommand(query, conexao);

            DataTable dt = new DataTable();

            var retorno = new List<BENEF_CLIENTE>();

            try
            {
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        var b = new BENEF_CLIENTE();

                        b.CODBENEF = Convert.ToInt32(item["CODBENEF"].ToString());
                        b.CODCLI = Convert.ToInt32(item["CODCLI"].ToString());
                        b.VALTIT = Convert.ToDecimal(item["VALTIT"].ToString());
                        b.VALDEP = Convert.ToDecimal(item["VALDEP"].ToString());
                        b.PERSUB = Convert.ToUInt16(item["PERSUB"].ToString());
                        b.SUBBENEF = item["SUBBENEF"].ToString();
                        b.COMPULSORIO = item["COMPULSORIO"].ToString();
                        b.DTASSOC = Convert.ToDateTime(item["DTASSOC"].ToString());
                        b.NOMBENEF = item["NOMBENEF"].ToString();
                        b.CARENCIA = item["CARENCIA"].ToString();
                        b.VIGENCIA = item["VIGENCIA"].ToString();
                        b.RENOVAUT = item["RENOVAUT"].ToString();
                        b.GRUPO = item["GRUPOFAMILIAR"].ToString();
                        b.COBCANC = item["COBCANC"].ToString();
                        retorno.Add(b);
                    }

                }
                return retorno;

            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                return new List<BENEF_CLIENTE>();
            }
        }

        public bool AssociaBeneficioCliente(BENEF_CLIENTE beneficio, int idOperador, out string msgRetorno)
        {
            SqlConnection conexao = new SqlConnection(BDTELENET);

            SqlCommand comando = new SqlCommand("PROC_ASSOCIA_BENEFCLI", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODCLI", Value = beneficio.CODCLI, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODBENEF", Value = beneficio.CODBENEF, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@VALOR", Value = beneficio.VALTIT, DbType = DbType.Decimal });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@VALORDEP", Value = beneficio.VALDEP, DbType = DbType.Decimal });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CARENCIA", Value = beneficio.CARENCIA, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@COBCANC", Value = beneficio.COBCANC, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@SUBBENEF", Value = beneficio.SUBBENEF, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@PERSUB", Value = beneficio.PERSUB, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@COMPULSORIO", Value = beneficio.COMPULSORIO, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@VIGENCIA", Value = beneficio.VIGENCIA, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@RENOVAUT", Value = beneficio.RENOVAUT, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@GRUPO", Value = beneficio.GRUPO, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@ID_OPERADOR", Value = idOperador, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@ORIGEM_OPERADOR", Value = "NC", DbType = DbType.String });

            DataTable dt = new DataTable();
            var retorno = string.Empty;
            msgRetorno = string.Empty;

            try
            {
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["RETORNO"].ToString();
                    msgRetorno = dt.Rows[0]["MENSAGEM"].ToString();
                }
                return retorno.Equals("0");
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                return false;
            }
        }

        public bool DesassociaBeneCli(BENEF_CLIENTE beneCli, int idOperador)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_DESASSOCIA_BENEFCLI";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPOS);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, beneCli.CODCLI);
            db.AddInParameter(cmd, "CODBENEF", DbType.Int32, beneCli.CODBENEF);
            db.AddInParameter(cmd, "ID_OPERADOR ", DbType.Int32, idOperador);
            db.AddInParameter(cmd, "ORIGEM_OPERADOR  ", DbType.String, "NC");
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_DESASSOCIA_BENEFCLI", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
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
    }
}
