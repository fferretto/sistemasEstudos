using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;
using System.Data;

namespace TELENET.SIL.DA
{
    class daComponentesAcesso
    {
        readonly string BDTELENET = string.Empty;

        public daComponentesAcesso(OPERADORA Operador)
        {
            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public List<COMPONENTESACESSO> ColecaoFormularios(Int32 IdPerfil)
        {
            var ColecaoComponentesAcesso = new List<COMPONENTESACESSO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("SELECT ID, FORM, FLGFORM, DESCRICAO, ");
            sql.AppendLine("CASE ");
	        sql.AppendLine("WHEN IDCOMP IS NULL THEN 'Bloqueado' ");
            sql.AppendLine("ELSE 'Liberado' ");
            sql.AppendLine("END AS ACESSO ");
            sql.AppendLine("FROM COMPONENTESACESSO COMP WITH (NOLOCK) ");
            sql.AppendLine("LEFT JOIN CONTROLEACESSO CTRL WITH (NOLOCK) ON COMP.ID = CTRL.IDCOMP AND IDPERFIL = @IDPERFIL");
            sql.AppendLine("WHERE FLGFORM = 'S' ");
            sql.AppendLine("ORDER BY FORM ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, IdPerfil);


            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var ComponentesAcesso = new COMPONENTESACESSO();
                ComponentesAcesso.ID = Convert.ToInt32(idr["ID"]);
                ComponentesAcesso.FORM = Convert.ToString(idr["FORM"]);
                ComponentesAcesso.FLGFORM = Convert.ToString(idr["FLGFORM"]);
                ComponentesAcesso.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                ComponentesAcesso.ACESSO = Convert.ToString(idr["ACESSO"]);
                ColecaoComponentesAcesso.Add(ComponentesAcesso);
            }
            idr.Close();
            return ColecaoComponentesAcesso;
        }

        public List<COMPONENTESACESSO> ColecaoComponentes(string Form, Int32 IdPerfil)
        {
            var ColecaoComponentesAcesso = new List<COMPONENTESACESSO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine(" SELECT DISTINCT DESCRICAO, ");
            sql.AppendLine("        FLGFORM ");
            sql.AppendLine("   FROM COMPONENTESACESSO WITH (NOLOCK) ");
            sql.AppendLine("  WHERE FLGFORM = 'N' ");
            sql.AppendLine(String.Format("  AND FORM LIKE '%{0}' ", Form));
            sql.AppendLine("    AND ID NOT IN (SELECT IDCOMP ");
            sql.AppendLine("                     FROM CONTROLEACESSO WITH (NOLOCK)");
            sql.AppendLine(string.Format("      WHERE IDPERFIL = {0}) ", IdPerfil.ToString()));
            sql.AppendLine(" ORDER BY DESCRICAO ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var ComponentesAcesso = new COMPONENTESACESSO();             
                ComponentesAcesso.FLGFORM = Convert.ToString(idr["FLGFORM"]);
                ComponentesAcesso.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                ColecaoComponentesAcesso.Add(ComponentesAcesso);
            }
            idr.Close();
            return ColecaoComponentesAcesso;
        }

    }
}
