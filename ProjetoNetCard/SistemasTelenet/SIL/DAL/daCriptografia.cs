using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.BL;
using TELENET.SIL.DA;
using TELENET.SIL.PO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SIL.BLL;

namespace SIL.DAL
{
    public class daCriptografia
    {
        public OPERADORA daOperadora = new OPERADORA();
        private string BDTELENET = string.Empty;
        private string BDAUTORIZADOR = string.Empty;

        public daCriptografia(OPERADORA operador)
        {
            daOperadora = operador;

            BDTELENET = string.Format(ConstantesSIL.BDTELENET, operador.SERVIDORNC, operador.BANCONC,
                                      ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, operador.SERVIDORAUT, operador.BANCOAUT,
                                          ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        /// <summary>
        /// Criptografa usando a função FUNCTIONCRIP do banco de dados.
        /// </summary>
        /// <param name="paramTextoACriptografar"></param>
        /// <returns></returns>
        public string Criptografar(string paramTextoACriptografar)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd = new SqlCommand("SELECT DBO.FUNCTIONCRIP(@TEXTO) RETORNO", conn) { CommandType = CommandType.Text };

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@TEXTO", SqlDbType.VarChar).Value = paramTextoACriptografar;

            string retorno = "";

            try
            {
                conn.Open();

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                //var cargaAndamento = false;

                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["RETORNO"].ToString();
                }

                conn.Close();

                return retorno;
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                throw;
            }
        }
    }
}
