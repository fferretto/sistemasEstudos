using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace SIL.DAL
{
    public class daCargaNovo
    {
        string BDTELENET = string.Empty;
        string BDAUTORIZADOR = string.Empty;

        SqlConnection conexao;
        SqlCommand comando;
        string query = string.Empty;

        OPERADORA FOperador;
        public daCargaNovo(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);

            comando = new SqlCommand();
            conexao = new SqlConnection();
        }
             
        /// <summary>
        /// Executa a query passada retornando um datatable
        /// </summary>
        /// <param name="bancoAutorizador"></param>
        /// <returns></returns>
        protected DataTable ExecuteSqlQuery(bool bancoAutorizador = false)
        {
            if (bancoAutorizador)
                conexao.ConnectionString = BDAUTORIZADOR;
            else
                conexao.ConnectionString = BDTELENET;
            
            comando.Connection = conexao;

            try
            {
                DataTable dt = new DataTable();
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                return dt;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool VerificaExistenciaProcessamentoCarga(string login, string codOpe, string origem)
        {
            comando.Parameters.Clear();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "CARGA_BUSCA_EM_PROCESSAMENTO";

            comando.Parameters.Add(new SqlParameter() { ParameterName = "@LOGINOPE", Value = login, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODOPE", Value = codOpe, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@ORIGEM", Value = origem, DbType = DbType.String });

            DataTable dt = ExecuteSqlQuery();

            return dt.Rows.Count > 0;
        }

        public void ConsultaSemaforo(string codOpe, string login, string codigoCliente, out string nivel, out string estado)
        {
            comando.Parameters.Clear();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "CARGA_CONSULTA_SEMAFORO";

            comando.Parameters.Add("@LOGINOPE", SqlDbType.VarChar).Value = login;
            comando.Parameters.Add("@CODOPE", SqlDbType.Int).Value = codOpe;

            comando.Parameters.Add(new SqlParameter("@ESTADO", SqlDbType.Int));
            comando.Parameters["@ESTADO"].Direction = ParameterDirection.Output;

            comando.Parameters.Add(new SqlParameter("@NIVEL", SqlDbType.Int));
            comando.Parameters["@NIVEL"].Direction = ParameterDirection.Output;

            DataTable dt = ExecuteSqlQuery();

            estado = comando.Parameters["@ESTADO"].Value.ToString();
            nivel = comando.Parameters["@NIVEL"].Value.ToString();
        }

        public DataTable ObterInformacaoCarga(string login, string codOpe, string origem)
        {
            comando.Parameters.Clear();
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "CARGA_BUSCA_EM_PROCESSAMENTO";

            comando.Parameters.Add(new SqlParameter() { ParameterName = "@LOGINOPE", Value = login, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODOPE", Value = codOpe, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@ORIGEM", Value = origem, DbType = DbType.String });

            DataTable dt = ExecuteSqlQuery();

            return dt;
        }
    }
}
