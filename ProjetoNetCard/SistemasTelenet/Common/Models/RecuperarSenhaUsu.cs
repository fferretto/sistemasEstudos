using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using SIL.BLL;
using System.Net.Mail;
using System.Net;

namespace NetCard.Common.Models
{
    public class RecuperarSenhaUsu
    {

        private ObjConn ObjetoConexao { get; set; }

        public bool ObrigaCadFrase { get; set; }
        public string CodCrt { get; set; }
        public List<Pergunta> Perguntas { get; set; }
        public RespostaUsuario Resposta { get; set; }
        public bool SucessoCadastro { get; set; }

        public RecuperarSenhaUsu(ObjConn objConexao)
        {
            ObjetoConexao = objConexao;
            Perguntas = new List<Pergunta>();
            Resposta = new RespostaUsuario();
        }


        #region *** Métodos ***
        public bool SalvarRespostaSecreta()
        {
//            string tabelausuario = Resposta.TipoSistema == 0 ? "USUARIO" : "USUARIOVA";

//            string sql = @"DELETE FROM RESPSECUSU WHERE CODCLI = {0} AND CPF = '{1}' AND NUMDEP = {2};
//                           INSERT INTO RESPSECUSU VALUES ({0}, '{1}', '" + CodCrt + @"', {2}, {3}, {4}, GETDATE(), '{5}', 0);
//                           UPDATE USUARIO SET PGTASECCAD = 'S' WHERE CODCLI = {0} AND CPF = '{1}' AND NUMDEP = {2};
//                           UPDATE USUARIOVA SET PGTASECCAD = 'S' WHERE CODCLI = {0} AND CPF = '{1}' AND NUMDEP = {2};";


//            string query = string.Format(sql,
//                Resposta.CodCli.ToString(),
//                Resposta.Cpf,
//                Resposta.NumDep,
//                Resposta.TipoSistema.ToString(),
//                Resposta.IdPergunta.ToString(),
//                Resposta.Resposta
//                );

            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(ObjetoConexao));
            string procedure = "PROC_INSERE_RESP_SECRETA";

            SqlCommand cmd = new SqlCommand(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CPF", Value = Resposta.Cpf, DbType = DbType.String });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@NUMDEP", Value = Resposta.NumDep, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODPERGSECUSU", Value = Resposta.IdPergunta, DbType = DbType.Int32 });
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@RESPOSTA", Value = Resposta.Resposta, DbType = DbType.String });

            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                int numeroCadastros = cmd.ExecuteNonQuery();
                connection.Close();

                return numeroCadastros > 0;

            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
            }
        }

        public void BuscarPerguntasDisponíveis()
        {
            string sql = "SELECT ID, PERGUNTA FROM PERGSECUSU WITH (NOLOCK) ";
            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(ObjetoConexao));
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;


            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                foreach (DataRow row in dt.Rows)
                {
                    Pergunta perg = new Pergunta();

                    perg.Id = Convert.ToInt32(row["ID"].ToString());
                    perg.Descricao = row["PERGUNTA"].ToString();

                    Perguntas.Add(perg);
                }

            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        public void AdicionarTentativaDeAcesso(string codCrt, string cpf)
        {
            string procedure = "PROC_ADICIONA_TENTATIVA_RESP_SECRETA";
            

            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(ObjetoConexao));
            SqlCommand cmd = new SqlCommand(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = codCrt, DbType = DbType.String });

            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        public bool VerificaCadastroRespostaSecreta(string codCli, string cpf, string numDep, int tipoSistema)
        {
            bool retorno = false;

            string tabelausuario = tipoSistema == 0 ? "USUARIO" : "USUARIOVA";

            string sql = @"SELECT TOP 1 PGTASECCAD FROM USUARIO WITH (NOLOCK) WHERE CODCLI = {0} AND CPF = '{1}' AND NUMDEP = {2} 
                           UNION 
                           SELECT TOP 1 PGTASECCAD FROM USUARIOVA WITH (NOLOCK) WHERE CODCLI = {0} AND CPF = '{1}' AND NUMDEP = {2} ";

            sql = string.Format(sql, codCli, cpf, numDep); 
            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(ObjetoConexao));
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;

            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    // Se a pergunta secreta está cadastrada pra o usuário
                    if (dt.Rows[0]["PGTASECCAD"].ToString() == "S")
                        retorno = true;
                }

                return retorno;

            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
            }
        }

        public bool BuscarPerguntaSecretaUsuario(string codCli, string cpf, string cartao, int tipoSistema, out string perguntaSecreta, out string respostaSecreta, out string numeroTentativas)
        {
            bool retorno = false;

            string procedure = "MW_BUSCA_PERG_SECRETA";

            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(ObjetoConexao));
            SqlCommand cmd = new SqlCommand(procedure, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = cartao, DbType = DbType.String });

            DataTable dt = new DataTable();

            try
            {
                string pergunta = "";
                string resposta = "";
                string tentativas = "0";

                connection.Open();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    pergunta = dt.Rows[0]["PERGUNTA"].ToString();
                    resposta = dt.Rows[0]["RESPOSTA"].ToString();
                    tentativas = dt.Rows[0]["NUMTENTATIVAS"].ToString();
                }

                perguntaSecreta = pergunta;
                respostaSecreta = resposta;
                numeroTentativas = tentativas;

                return retorno;

            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                perguntaSecreta = "";
                respostaSecreta = "";
                numeroTentativas = "";
                return false;
            }
        }

        public bool BuscarExistenciaPerguntaSecreta(string codCrt)
        {
            bool retorno = false;
            
            SqlConnection connection = new SqlConnection(Utils.GetConnectionStringNerCard(ObjetoConexao));
            SqlCommand cmd = new SqlCommand("MW_VERIF_EXISTE_RESPOSTA_SECRETA", connection);
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter() { Value = codCrt, ParameterName = "@CPF", DbType = System.Data.DbType.String });

            DataTable dt = new DataTable();

            try
            {
                

                connection.Open();
                dt.Load(cmd.ExecuteReader());
                connection.Close();

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["REGISTROS"].ToString()) > 0)
                        retorno = true;
                }               

                return retorno;

            }
            catch (Exception)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
            }
        } 



        #endregion

        #region *** Classes ***
        public class Pergunta
        {
            public int Id { get; set; }
            public string Descricao { get; set; }

        }

        public class RespostaUsuario
        {
            public int Id { get; set; }
            public int CodCli { get; set; }
            public int NumDep { get; set; }
            public string Cpf { get; set; }
            public int IdPergunta { get; set; }
            public string Resposta { get; set; }
            public int TipoSistema { get; set; }

        } 
        #endregion
    }
}
