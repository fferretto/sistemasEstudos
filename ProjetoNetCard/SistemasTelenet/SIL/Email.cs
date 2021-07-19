using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.PO;

namespace SIL
{
    public static class Email
    {

        public static OPERADORA daOperadora = new OPERADORA();
        private static string BDTELENET = string.Empty;
        private static string BDCONCENTRADOR = string.Empty;

        

        public struct Sistema
        {
            public const string ConsultaWebPj = "Consulta Web PJ";
            public const string ConsultaWebVa = "Consulta Web VA";
            public const string NetCardVa = "Netcard VA";
        }

        public static void EnviarEmailErro(string operadora = "", string sistema = "", string operadorWeb = "", string cartao = "", string cliente = "", string credenciado = "", string arquivoErro = "", string metodoErro = "", string descricaoErro = "", string mensagem = "")
        {
            SqlConnection conexao = new SqlConnection(string.Format(ConstantesSIL.BDCONCENTRADOR, "netcard", "netcard2222"));
            SqlCommand comando = new SqlCommand("PROC_ENVIA_EMAIL_ERRO", conexao);
            comando.CommandType = CommandType.StoredProcedure;

            try
            {
                if (!string.IsNullOrEmpty(operadora))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@OPERADORA", Value = operadora, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(sistema))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@SISTEMA", Value = sistema, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(operadorWeb))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@OPERADORWEB", Value = operadorWeb, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(cartao))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@CARTAO", Value = cartao, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(cliente))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@CLIENTE", Value = cliente, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(credenciado))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@CREDENCIADO", Value = credenciado, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(arquivoErro))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@ARQUIVOERRO", Value = arquivoErro, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(metodoErro))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@METODOERRO", Value = metodoErro, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(descricaoErro))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@DESCRICAOERRO", Value = descricaoErro, SqlDbType = SqlDbType.VarChar });

                if (!string.IsNullOrEmpty(mensagem))
                    comando.Parameters.Add(new SqlParameter() { ParameterName = "@MENSAGEM", Value = mensagem, SqlDbType = SqlDbType.VarChar });
                    
                //comando.Parameters.Add("@OPERADORA", SqlDbType.VarChar).Value = operadora;
                //comando.Parameters.Add("@SISTEMA", SqlDbType.VarChar).Value = sistema;
                //comando.Parameters.Add("@OPERADORWEB", SqlDbType.VarChar).Value = operadorWeb;
                //comando.Parameters.Add("@CARTAO", SqlDbType.VarChar).Value = cartao;
                //comando.Parameters.Add("@CLIENTE", SqlDbType.VarChar).Value = cliente;
                //comando.Parameters.Add("@CREDENCIADO", SqlDbType.VarChar).Value = credenciado;
                //comando.Parameters.Add("@ARQUIVOERRO", SqlDbType.VarChar).Value = arquivoErro;
                //comando.Parameters.Add("@METODOERRO", SqlDbType.VarChar).Value = metodoErro;
                //comando.Parameters.Add("@DESCRICAOERRO", SqlDbType.VarChar).Value = descricaoErro;

                conexao.Open();

                comando.ExecuteNonQuery();

                conexao.Close();
            }
            catch(Exception)
            {

            }
        }
    }
}
