using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daOPERADORVA
    {
        readonly string BDTELENET = string.Empty;
        readonly OPERADORA FOperador;

        public daOPERADORVA(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region GET

        public OPERADORVA GetOperadorVA(int ID)
        {
            var OperadorVA = new OPERADORVA();
            var db = DatabaseFactory.CreateDatabase("TELENET");

            const string sql = "SELECT CODOPE, NOMOPE, LOGOPE, SEN, CLAOPE FROM OPERADORVA WHERE CODOPE = @CODOPE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODOPE", DbType.Int32, ID);

            var idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                OperadorVA.CLAOPE = Convert.ToChar(idr["CLAOPE"]);
                OperadorVA.CODOPE = Convert.ToInt16(idr["CODOPE"]);
                OperadorVA.LOGOPE = idr["LOGOPE"].ToString();
                OperadorVA.NOMOPE = idr["NOMOPE"].ToString();
            }
            idr.Close();

            return OperadorVA;
        }

        public List<OPERADORVA> GetOperadorVA()
        {
            var ColecaoOperadorVA = new List<OPERADORVA>();
            var db = DatabaseFactory.CreateDatabase("TELENET");

            const string sql = "SELECT CODOPE, NOMOPE, LOGOPE, SEN, CLAOPE FROM OPERADORVA ORDER BY NOMOPE";

            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Operador = new OPERADORVA();

                Operador.CLAOPE = Convert.ToChar(idr["CLAOPE"]);
                Operador.CODOPE = Convert.ToInt16(idr["CODOPE"]);
                Operador.LOGOPE = Convert.ToString(idr["LOGOPE"]);
                Operador.NOMOPE = Convert.ToString(idr["NOMOPE"]);
                Operador.SEN = Convert.ToString(idr["SEN"]);

                ColecaoOperadorVA.Add(Operador);
            }
            idr.Close();

            return ColecaoOperadorVA;
        }

        public List<OPERADOR_VA> GetColecaoOperadorVA(string Filtro)
        {
            var ColecaoOperadorVA = new List<OPERADOR_VA>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT CODOPE, NOMOPE, LOGOPE, DESCRICAO ");
            sql.AppendLine("   FROM OPERADORVA ");
            sql.AppendLine("        INNER JOIN PERFILACESSOVA ON OPERADORVA.IDPERFIL = PERFILACESSOVA.ID");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOMOPE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Operador = new OPERADOR_VA();
                Operador.CODOPE = Convert.ToInt16(idr["CODOPE"]);
                Operador.LOGOPE = Convert.ToString(idr["LOGOPE"]);
                Operador.NOMOPE = Convert.ToString(idr["NOMOPE"]);
                Operador.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);

                ColecaoOperadorVA.Add(Operador);
            }
            idr.Close();

            return ColecaoOperadorVA;
        }

        #endregion

        #region SET

        public bool Inserir(OPERADORVA Operador)
        {
            var sbCampos = new StringBuilder();
            var sbParametros = new StringBuilder();

            Database db = new SqlDatabase(BDTELENET);

            sbCampos.Append(" NOMOPE, LOGOPE, IDPERFIL ");

            #region Parâmetros
            sbParametros.Append("@NOMOPE, @LOGOPE, @IDPERFIL");
            #endregion

            #region NETCARD
            var sql = string.Format("INSERT INTO OPERADORVA ({0}) VALUES ({1})", sbCampos, sbParametros);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();

            // Usuario VA
            db.AddInParameter(cmd, "NOMOPE", DbType.String, Operador.NOMOPE);
            db.AddInParameter(cmd, "LOGOPE", DbType.String, Operador.LOGOPE);
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, Operador.IDPERFIL);

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT OPERADORVA", FOperador, cmd);

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally
            {
                dbc.Close();
            }
            #endregion


            return true;
        }

        public bool Alterar(OPERADORVA Operador)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sbCampos = new StringBuilder();

            #region NETCARD

            #region Campos
            // Usuario
            sbCampos.Append("NOMOPE = @NOMOPE,");
            sbCampos.Append("LOGOPE = @LOGOPE,");
            sbCampos.Append("IDPERFIL = @IDPERFIL,");
            #endregion

            var sql = string.Format("UPDATE OPERADORVA SET {0} WHERE ID = @ID", sbCampos);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();

            // USsuario
            db.AddInParameter(cmd, "NOMOPE", DbType.String, Operador.NOMOPE);
            db.AddInParameter(cmd, "LOGOPE", DbType.String, Operador.LOGOPE);
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, Operador.IDPERFIL);

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {
                // Linha Afetada                                
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                if (LinhaAfetada == 1)
                {
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "UPDATE OPERADORVA", FOperador, cmd);

                    dbt.Commit();
                }
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar]" + err);
            }
            finally
            {
                dbc.Close();

            }
            #endregion

            return true;
        }

        public bool Excluir(OPERADORVA OperadorVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();

            var sql = string.Format(" DELETE OPERADORVA WHERE CODOPE = @CODOPE ");
            var cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODOPE", DbType.Int32, OperadorVA.CODOPE);

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {
                /* Aplicar Regras Exclusao */
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE OPERADORVA", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir]" + err);
            }
            finally
            {
                dbc.Close();
            }

            return true;
        }
        #endregion

    }
}