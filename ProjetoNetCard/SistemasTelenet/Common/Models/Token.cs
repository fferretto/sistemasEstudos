using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace NetCard.Common.Models
{
    public class Token
    {
        public string GerarToken(ObjConn objConn, DadosAcesso dadosAcesso, string coment)
        {
            objConn.Acesso = Constantes.concentrador;
            string retorno;
            Database db = new SqlDatabase(Utils.GetConnectionStringConcentrador(objConn));
            const string sql = "MBL_GERAR_TOKEN";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@CPF", DbType.String, Convert.ToString(dadosAcesso.Cpf)),
                               new Parametro("@CODOPE", DbType.Int32, objConn.CodOpe),
                               new Parametro("@COMENTARIO", DbType.String, dadosAcesso.Nome)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                retorno = dr.Read() ? dr["TOKEN"].ToString() : "O Token não pôde ser gerado. Favor entrar em contato com o RH da empresa.";
                dr.Close();
                objConn.Acesso = null;
            }
            catch
            {
                objConn.Acesso = null;
                if (dr != null)
                    dr.Close();
                retorno = "Falha na geração do token.";
            }
            return retorno;
        }
    }
}
