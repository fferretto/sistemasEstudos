using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace SIL.DAL
{
    public class daCONSULTAVA
    {
        readonly string BDTELENET = string.Empty;
        readonly OPERADORA FOperador;

        private bool ConfiguraJuncao(CONSULTA_VA consultaVA, Database database, DbCommand command, StringBuilder campos, StringBuilder parametros, out string sql)
        {
            campos.Append(", NUM_FECH_CLI_INI, NUM_FECH_CLI_FIM, DATA_FECH_CLI_INI, DATA_FECH_CLI_FIM, SISTEMA ");
            parametros.Append(", @NUM_FECH_CLI_INI, @NUM_FECH_CLI_FIM, @DATA_FECH_CLI_INI, @DATA_FECH_CLI_FIM, @SISTEMA ");

            database.AddInParameter(command, "NUM_FECH_CLI_INI", DbType.Int32, consultaVA.NUM_FECH_CLI_INI);
            database.AddInParameter(command, "NUM_FECH_CLI_FIM", DbType.Int32, consultaVA.NUM_FECH_CLI_FIM);

            if (consultaVA.DATA_FECH_CLI_INI != DateTime.MinValue)
            {
                database.AddInParameter(command, "DATA_FECH_CLI_INI", DbType.DateTime, consultaVA.DATA_FECH_CLI_INI);
            }
            else
            {
                database.AddInParameter(command, "DATA_FECH_CLI_INI", DbType.DateTime, null);
            }

            if (consultaVA.DATA_FECH_CLI_FIM != DateTime.MinValue)
            {
                database.AddInParameter(command, "DATA_FECH_CLI_FIM", DbType.DateTime, consultaVA.DATA_FECH_CLI_FIM);
            }
            else
            {
                database.AddInParameter(command, "DATA_FECH_CLI_FIM", DbType.DateTime, null);
            }

            database.AddInParameter(command, "SISTEMA", DbType.Int32, consultaVA.SISTEMA);

            sql = string.Format("INSERT INTO CONSULTA ({0}) VALUES ({1})", campos, parametros);
            return true;
        }

        public daCONSULTAVA(OPERADORA Operador)
        {
            FOperador = Operador;
            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public string ConsultaJustific(string datTra, string nsuHos, string nsuAut, string tipTra)
        {
            var retorno = string.Empty;
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT JUSTIFIC ");
            sql.AppendLine("FROM JUSTIFICVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE ");
            if (tipTra == "251007" || tipTra == "999030")
                sql.AppendLine("DATTRA >= @DATTRA AND NSUHOS = @NSUHOS AND NSUAUT = @NSUAUT");
            else
                sql.AppendLine("DATTRAORIG = @DATTRAORIG AND NSUHOSORIG = @NSUHOSORIG AND NSUAUTORIG = @NSUAUTORIG");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            if (tipTra == "251007" || tipTra == "999030")
            {
                db.AddInParameter(cmd, "DATTRA", DbType.String, Convert.ToDateTime(datTra).ToString("yyyyMMdd HH:mm:ss"));
                db.AddInParameter(cmd, "NSUHOS", DbType.Int32, nsuHos);
                db.AddInParameter(cmd, "NSUAUT", DbType.Int32, nsuAut);
            }
            else
            {
                db.AddInParameter(cmd, "DATTRAORIG", DbType.String, Convert.ToDateTime(datTra).ToString("yyyyMMdd HH:mm:ss"));
                db.AddInParameter(cmd, "NSUHOSORIG", DbType.Int32, nsuHos);
                db.AddInParameter(cmd, "NSUAUTORIG", DbType.Int32, nsuAut);
            }

            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
                retorno = Convert.ToString(idr["JUSTIFIC"]);
            idr.Close();
            return retorno;
        }
        public string ConsultaJustificSegViaCard(string DAD)
        {
            if (DAD.Length < 23)
            {
                return "Não Informado";
            }

            var retorno = string.Empty;
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT nomJustificativa ");
            sql.AppendLine("FROM JUS_SEG_VIA_CARD WITH (NOLOCK) ");
            sql.AppendLine("WHERE 1 = 1 ");

            sql.AppendLine("and codJus_Seg_Via_Card = " + DAD.Substring(20, 3));
            

            var cmd = db.GetSqlStringCommand(sql.ToString());
            
            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
                retorno = Convert.ToString(idr["nomJustificativa"]);
            idr.Close();
            return retorno;
        }
        public List<CONSULTA_VA> ColecaoConsultas(string Filtro)
        {
            var ColecaoConsultaVA = new List<CONSULTA_VA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT");
            sql.AppendLine("  C.CODCONS CODIGO,");
            sql.AppendLine("  C.CODTIPOCONS TIPO,");
            sql.AppendLine("  C.NOME_CONSULTA NOME,");
            sql.AppendLine("  C.OPERADOR OPERADOR,");
            sql.AppendLine("  T.DESCRICAO DESCRICAOTIPO");
            sql.Append(",  C.SISTEMA ");
            sql.AppendLine("FROM CONSULTA C WITH (NOLOCK) INNER JOIN TIPOCONSULTA T WITH (NOLOCK) ");
            sql.AppendLine("ON C.CODTIPOCONS = T.CODTIPOCONS");
            sql.AppendLine(Filtro);
            sql.AppendLine("  ORDER BY C.CODTIPOCONS");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var consulta = new CONSULTA_VA();
                consulta.CODCONS = Convert.ToInt32(idr["CODIGO"]);
                consulta.CODTIPOCONSINT = Convert.ToInt32(idr["TIPO"]);
                consulta.NOME_CONSULTA = Convert.ToString(idr["NOME"]);
                consulta.DESCRICAO = Convert.ToString(idr["DESCRICAOTIPO"]);
                ColecaoConsultaVA.Add(consulta);
            }
            idr.Close();

            return ColecaoConsultaVA;
        }

        public bool Inserir(CONSULTA_VA consultaVA)
        {
            var sbCamposCliente = new StringBuilder();
            var sbParametrosCliente = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sbCamposCliente.Append("CODTIPOCONS, NOME_CONSULTA, PERIODO_INI, PERIODO_FIM, HORA_PERIODO_INI, HORA_PERIODO_FIM, NUM_AUT_INI, NUM_AUT_FIM, NUM_HOST_INI, NUM_HOST_FIM,");
            sbCamposCliente.Append("NOME_USUARIO, MAT_USUARIO, CPF_USUARIO, TIPO_RESPOSTA, TIPO_TRANSACAO, NUM_CARTAO, LISTA_CRED, LISTA_CLI,");
            sbCamposCliente.Append("NUM_FECH_CRED_INI, NUM_FECH_CRED_FIM, DATA_FECH_CRED_INI, DATA_FECH_CRED_FIM, INTERVALO_CRED_INI, INTERVALO_CRED_FIM,INTERVALO_CLI_INI, INTERVALO_CLI_FIM, LISTA_COL,OPERADOR, CODCEN");

            sbParametrosCliente.Append("@CODTIPOCONS, @NOME_CONSULTA, @PERIODO_INI, @PERIODO_FIM, @HORA_PERIODO_INI, @HORA_PERIODO_FIM, @NUM_AUT_INI, @NUM_AUT_INI, @NUM_HOST_INI, @NUM_HOST_FIM,");
            sbParametrosCliente.Append("@NOME_USUARIO, @MAT_USUARIO, @CPF_USUARIO, @TIPO_RESPOSTA, @TIPO_TRANSACAO, @NUM_CARTAO, @LISTA_CRED, @LISTA_CLI,");
            sbParametrosCliente.Append("@NUM_FECH_CRED_INI, @NUM_FECH_CRED_FIM, @DATA_FECH_CRED_INI, @DATA_FECH_CRED_FIM, @INTERVALO_CRED_INI, @INTERVALO_CRED_FIM, @INTERVALO_CLI_INI, @INTERVALO_CLI_FIM, @LISTA_COL, @OPERADOR, @CODCEN");

            var sql = string.Format("INSERT INTO CONSULTAVA ({0}) VALUES ({1})", sbCamposCliente, sbParametrosCliente);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODTIPOCONS", DbType.Int32, consultaVA.CODTIPOCONS);
            db.AddInParameter(cmd, "NOME_CONSULTA", DbType.String, consultaVA.NOME_CONSULTA);

            if (consultaVA.PERIODO_INI != DateTime.MinValue) db.AddInParameter(cmd, "PERIODO_INI", DbType.DateTime, consultaVA.PERIODO_INI);
            else db.AddInParameter(cmd, "PERIODO_INI", DbType.DateTime, null);

            if (consultaVA.PERIODO_FIM != DateTime.MaxValue) db.AddInParameter(cmd, "PERIODO_FIM", DbType.DateTime, consultaVA.PERIODO_FIM);
            else db.AddInParameter(cmd, "PERIODO_FIM", DbType.DateTime, null);

            db.AddInParameter(cmd, "HORA_PERIODO_INI", DbType.String, consultaVA.HORA_PERIODO_INI);
            db.AddInParameter(cmd, "HORA_PERIODO_FIM", DbType.String, consultaVA.HORA_PERIODO_FIM);
            db.AddInParameter(cmd, "NUM_AUT_INI", DbType.Int32, consultaVA.NUM_AUT_INI);
            db.AddInParameter(cmd, "NUM_AUT_FIM", DbType.Int32, consultaVA.NUM_AUT_FIM);
            db.AddInParameter(cmd, "NUM_HOST_INI", DbType.Int32, consultaVA.NUM_HOST_INI);
            db.AddInParameter(cmd, "NUM_HOST_FIM", DbType.Int32, consultaVA.NUM_HOST_FIM);
            db.AddInParameter(cmd, "NOME_USUARIO", DbType.String, consultaVA.NOME_USUARIO);
            db.AddInParameter(cmd, "MAT_USUARIO", DbType.String, consultaVA.MAT_USUARIO);
            db.AddInParameter(cmd, "CPF_USUARIO", DbType.String, consultaVA.CPF_USUARIO);
            db.AddInParameter(cmd, "TIPO_RESPOSTA", DbType.String, consultaVA.TIPO_RESPOSTA);
            db.AddInParameter(cmd, "TIPO_TRANSACAO", DbType.String, consultaVA.TIPO_TRANSACAO);
            db.AddInParameter(cmd, "NUM_CARTAO", DbType.String, consultaVA.NUM_CARTAO);
            db.AddInParameter(cmd, "LISTA_CRED", DbType.String, consultaVA.LISTA_CRED);
            db.AddInParameter(cmd, "LISTA_CLI", DbType.String, consultaVA.LISTA_CLI);
            db.AddInParameter(cmd, "NUM_FECH_CRED_INI", DbType.Int32, consultaVA.NUM_FECH_CRED_INI);
            db.AddInParameter(cmd, "NUM_FECH_CRED_FIM", DbType.Int32, consultaVA.NUM_FECH_CRED_FIM);

            if (consultaVA.DATA_FECH_CRED_INI != DateTime.MinValue) db.AddInParameter(cmd, "DATA_FECH_CRED_INI", DbType.DateTime, consultaVA.DATA_FECH_CRED_INI);
            else db.AddInParameter(cmd, "DATA_FECH_CRED_INI", DbType.DateTime, null);

            if (consultaVA.DATA_FECH_CRED_FIM != DateTime.MinValue) db.AddInParameter(cmd, "DATA_FECH_CRED_FIM", DbType.DateTime, consultaVA.DATA_FECH_CRED_FIM);
            else db.AddInParameter(cmd, "DATA_FECH_CRED_FIM", DbType.DateTime, null);

            db.AddInParameter(cmd, "INTERVALO_CRED_INI", DbType.String, consultaVA.INTERVALO_CRED_INI);
            db.AddInParameter(cmd, "INTERVALO_CRED_FIM", DbType.String, consultaVA.INTERVALO_CRED_FIM);
            db.AddInParameter(cmd, "INTERVALO_CLI_INI", DbType.String, consultaVA.INTERVALO_CLI_INI);
            db.AddInParameter(cmd, "INTERVALO_CLI_FIM", DbType.String, consultaVA.INTERVALO_CLI_FIM);
            db.AddInParameter(cmd, "LISTA_COL", DbType.String, consultaVA.LISTA_COL);
            db.AddInParameter(cmd, "OPERADOR", DbType.Int32, consultaVA.OPERADOR);
            db.AddInParameter(cmd, "CODCEN", DbType.Int32, consultaVA.CODCEN);

            var sqlJuncao = string.Empty;
            if (ConfiguraJuncao(consultaVA, db, cmd, sbCamposCliente, sbParametrosCliente, out sqlJuncao))
            {
                cmd.CommandText = sqlJuncao;
            }

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT CONSULTA", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally { dbc.Close(); }
            return true;
        }

        public bool Excluir(CONSULTA_VA consultaVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = "DELETE CONSULTA WHERE CODCONS = @CODCONS AND CODTIPOCONS = @CODTIPOCONS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCONS", DbType.Int32, consultaVA.CODCONS);
            db.AddInParameter(cmd, "CODTIPOCONS", DbType.Int32, consultaVA.CODTIPOCONS);

            var dbc = db.CreateConnection();
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, sql, FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally { dbc.Close(); }
            return true;
        }

        public bool Excluir(int codigoConsulta, int codigoTipoCons)
        {
            Database db = new SqlDatabase(BDTELENET); ;
            var sql = "DELETE CONSULTA WHERE CODCONS = @CODCONS AND CODTIPOCONS = @CODTIPOCONS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCONS", DbType.Int32, codigoConsulta);
            db.AddInParameter(cmd, "CODTIPOCONS", DbType.Int32, codigoTipoCons);
            var dbc = db.CreateConnection();

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, sql, FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally { dbc.Close(); }
            return true;
        }

        public bool Alterar(CONSULTA_VA consultaVA)
        {
            var sql = new StringBuilder();
            sql.Append("UPDATE CONSULTA");
            sql.Append(string.Format(" SET NOME_CONSULTA = '{0}'", consultaVA.NOME_CONSULTA));
            sql.Append(consultaVA.PERIODO_INI != DateTime.MinValue
                           ? string.Format(" ,PERIODO_INI = '{0}'", consultaVA.PERIODO_INI.ToString("yyyyMMdd HH:mm:ss"))
                           : string.Format(" ,PERIODO_INI = {0}", "NULL"));
            sql.Append(consultaVA.PERIODO_FIM != DateTime.MaxValue
                           ? string.Format(" ,PERIODO_FIM = '{0}'", consultaVA.PERIODO_FIM.ToString("yyyyMMdd HH:mm:ss"))
                           : string.Format(" ,PERIODO_FIM = {0}", "NULL"));
            sql.Append(string.Format(" ,HORA_PERIODO_INI = '{0}'", consultaVA.HORA_PERIODO_INI));
            sql.Append(string.Format(" ,HORA_PERIODO_FIM = '{0}'", consultaVA.HORA_PERIODO_FIM));
            sql.Append(string.Format(" ,NUM_HOST_INI = {0}", consultaVA.NUM_HOST_INI));
            sql.Append(string.Format(" ,NUM_HOST_FIM = {0}", consultaVA.NUM_HOST_FIM));
            sql.Append(string.Format(" ,NUM_AUT_INI = {0}", consultaVA.NUM_AUT_INI));
            sql.Append(string.Format(" ,NUM_AUT_FIM = {0}", consultaVA.NUM_AUT_FIM));
            sql.Append(string.Format(" ,NOME_USUARIO = '{0}'", consultaVA.NOME_USUARIO));
            sql.Append(string.Format(" ,MAT_USUARIO = '{0}'", consultaVA.MAT_USUARIO));
            sql.Append(string.Format(" ,CPF_USUARIO = '{0}'", consultaVA.CPF_USUARIO));
            var tipoResposta = new StringBuilder();
            if (consultaVA.TIPO_RESPOSTA != null)
            {
                var respostas = consultaVA.TIPO_RESPOSTA.Split(',');
                foreach (var tipo in respostas.Where(tipo => !string.IsNullOrEmpty(tipo.ToString(CultureInfo.InvariantCulture))))
                {
                    tipoResposta.Append(string.Format("'{0}',", tipo));
                }
            }
            sql.Append(string.Format(" ,TIPO_RESPOSTA = '{0}'", tipoResposta));
            sql.Append(string.Format(" ,TIPO_TRANSACAO = '{0}'", consultaVA.TIPO_TRANSACAO));
            sql.Append(string.Format(" ,NUM_CARTAO = '{0}'", consultaVA.NUM_CARTAO));

            sql.Append(consultaVA.LISTA_CRED != null
                           ? string.Format(" ,LISTA_CRED = '{0}'", consultaVA.LISTA_CRED)
                           : string.Format(" ,LISTA_CRED = {0}", "NULL"));

            sql.Append(consultaVA.LISTA_CLI != null
                           ? string.Format(" ,LISTA_CLI = '{0}'", consultaVA.LISTA_CLI)
                           : string.Format(" ,LISTA_CLI = {0}", "NULL"));

            sql.Append(string.Format(" ,NUM_FECH_CRED_INI = {0}", consultaVA.NUM_FECH_CRED_INI));
            sql.Append(string.Format(" ,NUM_FECH_CRED_FIM = {0}", consultaVA.NUM_FECH_CRED_FIM));

            sql.Append(consultaVA.DATA_FECH_CRED_INI != DateTime.MinValue
                           ? string.Format(" ,DATA_FECH_CRED_INI = '{0}'", consultaVA.DATA_FECH_CRED_INI.ToString("yyyyMMdd HH:mm:ss"))
                           : string.Format(" ,DATA_FECH_CRED_INI = {0}", "NULL"));

            sql.Append(consultaVA.DATA_FECH_CRED_FIM != DateTime.MinValue
                           ? string.Format(" ,DATA_FECH_CRED_FIM = '{0}'", consultaVA.DATA_FECH_CRED_FIM.ToString("yyyyMMdd HH:mm:ss"))
                           : string.Format(" ,DATA_FECH_CRED_FIM = {0}", "NULL"));

            sql.Append(string.Format(" ,OPERADOR = {0}", consultaVA.OPERADOR));
            sql.Append(string.Format(" ,INTERVALO_CRED_INI = '{0}'", consultaVA.INTERVALO_CRED_INI));
            sql.Append(string.Format(" ,INTERVALO_CRED_FIM = '{0}'", consultaVA.INTERVALO_CRED_FIM));
            sql.Append(string.Format(" ,INTERVALO_CLI_INI = '{0}'", consultaVA.INTERVALO_CLI_INI));
            sql.Append(string.Format(" ,INTERVALO_CLI_FIM = '{0}'", consultaVA.INTERVALO_CLI_FIM));

            sql.Append(!string.IsNullOrEmpty(consultaVA.LISTA_COL)
                           ? string.Format(" ,LISTA_COL = '{0}'", consultaVA.LISTA_COL)
                           : string.Format(" ,LISTA_COL = {0}", "NULL"));

            sql.Append(string.Format(" ,NUM_FECH_CLI_INI = '{0}'", consultaVA.NUM_FECH_CLI_INI));
            sql.Append(string.Format(" ,NUM_FECH_CLI_FIM = '{0}'", consultaVA.NUM_FECH_CLI_FIM));

            sql.Append(consultaVA.DATA_FECH_CLI_INI != DateTime.MinValue
                ? string.Format(" ,DATA_FECH_CLI_INI = '{0}'", consultaVA.DATA_FECH_CLI_INI.ToString("yyyyMMdd HH:mm:ss"))
                : string.Format(" ,DATA_FECH_CLI_INI = {0}", "NULL"));

            sql.Append(consultaVA.DATA_FECH_CLI_FIM != DateTime.MinValue
                ? string.Format(" ,DATA_FECH_CLI_FIM = '{0}'", consultaVA.DATA_FECH_CLI_FIM.ToString("yyyyMMdd HH:mm:ss"))
                : string.Format(" ,DATA_FECH_CLI_FIM = {0}", "NULL"));
                
            sql.Append(string.Format(" ,SISTEMA = '{0}'", consultaVA.SISTEMA));
            sql.Append(string.Format(" WHERE CODCONS = {0}", consultaVA.CODCONS));

            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, sql.ToString(), FOperador, cmd);
                dbt.Commit();//Commitando as mudancas
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally { dbc.Close(); }
            return true;
        }

        public DataTable GeraListagem(string query)
        {
            var dtCustomers = new DataTable();
            try
            {
                var cn = new SqlConnection(BDTELENET);
                var cmd = new SqlCommand(query, cn);
                var adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dtCustomers);
            }
            catch { throw new Exception("Sintaxe incorreta"); }
            return dtCustomers;
        }

        public CONSULTA_VA RecuperaConsultaByCodigo(int CodigoConsulta)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT ");
            sql.AppendLine(" CODCONS, CODTIPOCONS, NOME_CONSULTA, PERIODO_INI, PERIODO_FIM, NUM_HOST_INI, NUM_HOST_FIM, NUM_AUT_INI, NUM_AUT_FIM,  ");
            sql.AppendLine(" NOME_USUARIO, MAT_USUARIO, CPF_USUARIO, TIPO_RESPOSTA, TIPO_TRANSACAO, NUM_CARTAO, LISTA_CRED, LISTA_CLI, NUM_FECH_CRED_INI, ");
            sql.AppendLine(" NUM_FECH_CRED_FIM, DATA_FECH_CRED_INI, DATA_FECH_CRED_FIM, OPERADOR, CODTIPOCONSINT, DESCRICAO, HORA_PERIODO_INI, ");
            sql.AppendLine(" HORA_PERIODO_FIM , INTERVALO_CRED_INI, INTERVALO_CRED_FIM, INTERVALO_CLI_INI, INTERVALO_CLI_FIM, LISTA_COL ");
            sql.AppendLine(", NUM_FECH_CLI_INI , NUM_FECH_CLI_FIM, DATA_FECH_CLI_INI, DATA_FECH_CLI_FIM, SISTEMA ");
            sql.AppendLine(" FROM CONSULTA WITH (NOLOCK) WHERE CODCONS = " + CodigoConsulta);

            IDataReader idr = null;
            CONSULTA_VA cons = null;

            try
            {
                var cmd = db.GetSqlStringCommand(sql.ToString());
                idr = db.ExecuteReader(cmd);
                if (idr.Read())
                {
                    cons = new CONSULTA_VA();
                    cons.CODCONS = Convert.ToInt32(idr["CODCONS"]);
                    cons.CODTIPOCONS = Convert.ToInt32(idr["CODTIPOCONS"]);
                    cons.NOME_CONSULTA = idr["NOME_CONSULTA"].ToString();
                    if (idr["PERIODO_INI"].ToString().Trim() != string.Empty) cons.PERIODO_INI = Convert.ToDateTime(idr["PERIODO_INI"]);
                    if (idr["PERIODO_FIM"].ToString().Trim() != string.Empty) cons.PERIODO_FIM = Convert.ToDateTime(idr["PERIODO_FIM"]);
                    cons.NUM_HOST_INI = Convert.ToInt32(idr["NUM_HOST_INI"]);
                    cons.NUM_HOST_FIM = Convert.ToInt32(idr["NUM_HOST_FIM"]);
                    cons.NUM_AUT_INI = Convert.ToInt32(idr["NUM_AUT_INI"]);
                    cons.NUM_AUT_FIM = Convert.ToInt32(idr["NUM_AUT_FIM"]);
                    cons.NOME_USUARIO = idr["NOME_USUARIO"].ToString();
                    cons.MAT_USUARIO = idr["MAT_USUARIO"].ToString();
                    cons.CPF_USUARIO = idr["CPF_USUARIO"].ToString();
                    cons.TIPO_RESPOSTA = idr["TIPO_RESPOSTA"].ToString();
                    cons.TIPO_TRANSACAO = idr["TIPO_TRANSACAO"].ToString();
                    cons.NUM_CARTAO = idr["NUM_CARTAO"].ToString();
                    cons.LISTA_CRED = idr["LISTA_CRED"].ToString();
                    cons.LISTA_CLI = idr["LISTA_CLI"].ToString();
                    cons.NUM_FECH_CRED_INI = Convert.ToInt32(idr["NUM_FECH_CRED_INI"]);
                    cons.NUM_FECH_CRED_FIM = Convert.ToInt32(idr["NUM_FECH_CRED_FIM"]);
                    if (idr["DATA_FECH_CRED_INI"].ToString().Trim() != string.Empty) cons.DATA_FECH_CRED_INI = Convert.ToDateTime(idr["DATA_FECH_CRED_INI"]);
                    if (idr["DATA_FECH_CRED_FIM"].ToString().Trim() != string.Empty) cons.DATA_FECH_CRED_FIM = Convert.ToDateTime(idr["DATA_FECH_CRED_FIM"]);
                    cons.OPERADOR = Convert.ToInt32(idr["OPERADOR"]);
                    if (idr["CODTIPOCONSINT"].ToString().Trim() != string.Empty) cons.CODTIPOCONSINT = Convert.ToInt32(idr["CODTIPOCONSINT"]);
                    cons.DESCRICAO = idr["DESCRICAO"].ToString();
                    cons.HORA_PERIODO_INI = idr["HORA_PERIODO_INI"].ToString();
                    cons.HORA_PERIODO_FIM = idr["HORA_PERIODO_FIM"].ToString();
                    cons.INTERVALO_CRED_INI = idr["INTERVALO_CRED_INI"].ToString();
                    cons.INTERVALO_CRED_FIM = idr["INTERVALO_CRED_FIM"].ToString();
                    cons.INTERVALO_CLI_INI = idr["INTERVALO_CLI_INI"].ToString();
                    cons.INTERVALO_CLI_FIM = idr["INTERVALO_CLI_FIM"].ToString();
                    cons.LISTA_COL = idr["LISTA_COL"].ToString();
                    cons.NUM_FECH_CLI_INI = Convert.ToInt32(idr["NUM_FECH_CLI_INI"]);
                    cons.NUM_FECH_CLI_FIM = Convert.ToInt32(idr["NUM_FECH_CLI_FIM"]);
                    if (idr["DATA_FECH_CLI_INI"].ToString().Trim() != string.Empty) cons.DATA_FECH_CLI_INI = Convert.ToDateTime(idr["DATA_FECH_CLI_INI"]);
                    if (idr["DATA_FECH_CLI_FIM"].ToString().Trim() != string.Empty) cons.DATA_FECH_CLI_FIM = Convert.ToDateTime(idr["DATA_FECH_CLI_FIM"]);
                    cons.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                }
                idr.Close();
            }
            catch { if (idr != null) idr.Close(); }
            return cons;
        }

        public ACOESTRANS ConsultaAcoesTrans(int tipTra, string codrta, string datFecCli, string datFecCre, string flagAut, int sistema, int codCli, int numDep) 
        {
            var acoesTrans = new ACOESTRANS();

            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_CONSULTA_ACOES_TRANS", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TIPTRA", SqlDbType.Int).Value = tipTra;
                cmd.Parameters.Add("@CODRTA", SqlDbType.VarChar).Value = codrta;
                if (!string.IsNullOrEmpty(datFecCli)) cmd.Parameters.Add("@DATFECCLI", SqlDbType.DateTime).Value = Convert.ToDateTime(datFecCli);
                if (!string.IsNullOrEmpty(datFecCre)) cmd.Parameters.Add("@DATFECCRE", SqlDbType.DateTime).Value = Convert.ToDateTime(datFecCre);                
                cmd.Parameters.Add("@FLAG_AUT", SqlDbType.VarChar).Value = flagAut;
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = sistema;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = numDep;
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    acoesTrans.CONFIRMAR = Convert.ToString(reader["CONFIRMAR"]) == "S";
                    acoesTrans.ALTERAR = Convert.ToString(reader["ALTERAR"]) == "S";
                    acoesTrans.CANCELAR = Convert.ToString(reader["CANCELAR"]) == "S";
                    acoesTrans.ALTVALOR = Convert.ToString(reader["ALTVALOR"]) == "S";
                    acoesTrans.VALOR = Convert.ToDecimal(reader["VALOR"]);
                }
                return acoesTrans;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
    }
}
