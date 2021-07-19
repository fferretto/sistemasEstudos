using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELENET.SIL;
using NetCard.Common.Util;
using TELENET.SIL.PO;
using System.Data.SqlClient;
using SIL;


namespace NetCard.Common.Models
{
    public class Credenciado
    { 
        public int codCre { get; set; }
        public int codCen { get; set; }
        public string nomeFantasia { get; set; }
        public string nomeExibição { get; set; }
        public bool Sucesso { get; set; }

        public Credenciado BuscarCredenciadoByCodCre(ObjConn objConn, string paramCodigoCredenciado)
        {
            StringBuilder sql = new StringBuilder();
            Credenciado retorno = new Credenciado();
            
            sql.AppendLine(" SELECT");
            sql.AppendLine(" C.CODCRE, CODCEN, NOMFAN, NOMEXIBICAO ");
            sql.AppendLine(" FROM VCREDENCIADO C WITH (NOLOCK) ");
            sql.AppendLine("WHERE C.CODCRE = " + paramCodigoCredenciado);

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = new Credenciado()
                    {
                        codCre = Convert.ToInt32(dr["CODCRE"].ToString()),
                        codCen = Convert.ToInt32(dr["CODCEN"].ToString()),
                        nomeFantasia = dr["NOMFAN"].ToString(),
                        nomeExibição = dr["NOMEXIBICAO"].ToString(),
                        Sucesso = true
                    };
                }
                else
                {
                    retorno = new Credenciado()
                    {
                        Sucesso = false,
                        nomeFantasia = "Credenciado não encontrado",
                        nomeExibição = "Credenciado não encontrado"
                    };
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }

            return retorno;
        }

    }
}
