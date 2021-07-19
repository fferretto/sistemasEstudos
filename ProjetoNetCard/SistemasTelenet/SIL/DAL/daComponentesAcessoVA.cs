using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daComponentesAcessoVA
    {
        readonly string BDTELENET = string.Empty;

        public daComponentesAcessoVA(OPERADORA Operador)
        {
            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public List<COMPONENTESACESSOVA> ColecaoFormularios(Int32 IdPerfil)
        {
            var ColecaoComponentesAcessoVA = new List<COMPONENTESACESSOVA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine(" SELECT ID, ");
            sql.AppendLine("        FORM, ");
            sql.AppendLine("        FLGFORM, ");
            sql.AppendLine("        DESCRICAO ");
            sql.AppendLine("   FROM COMPONENTESACESSOVA ");
            sql.AppendLine("  WHERE FLGFORM = 'S' ");
            sql.AppendLine("    AND ID NOT IN (SELECT IDCOMP ");
            sql.AppendLine("                     FROM CONTROLEACESSOVA ");
            sql.AppendLine(string.Format("      WHERE IDPERFIL = {0}) ", IdPerfil.ToString()));
            sql.AppendLine(" ORDER BY FORM ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var ComponentesAcessoVA = new COMPONENTESACESSOVA();
                ComponentesAcessoVA.ID = Convert.ToInt32(idr["ID"]);
                ComponentesAcessoVA.FORM = Convert.ToString(idr["FORM"]);
                ComponentesAcessoVA.FLGFORM = Convert.ToString(idr["FLGFORM"]);
                ComponentesAcessoVA.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                ColecaoComponentesAcessoVA.Add(ComponentesAcessoVA);
            }
            idr.Close();
            return ColecaoComponentesAcessoVA;
        }

        public List<COMPONENTESACESSOVA> ColecaoComponentes(string Form, Int32 IdPerfil)
        {
            var ColecaoComponentesAcessoVA = new List<COMPONENTESACESSOVA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine(" SELECT ID, ");
            sql.AppendLine("        COMP, ");
            sql.AppendLine("        FLGFORM, ");
            sql.AppendLine("        DESCRICAO ");
            sql.AppendLine("   FROM COMPONENTESACESSOVA ");
            sql.AppendLine("  WHERE FLGFORM = 'N' ");
            sql.AppendLine(String.Format("  AND FORM LIKE '%{0}' ", Form));
            sql.AppendLine("    AND ID NOT IN (SELECT IDCOMP ");
            sql.AppendLine("                     FROM CONTROLEACESSOVA ");
            sql.AppendLine(string.Format("      WHERE IDPERFIL = {0}) ", IdPerfil.ToString()));
            sql.AppendLine(" ORDER BY COMP ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var ComponentesAcessoVA = new COMPONENTESACESSOVA();
                ComponentesAcessoVA.ID = Convert.ToInt32(idr["ID"]);
                ComponentesAcessoVA.COMP = Convert.ToString(idr["COMP"]);
                ComponentesAcessoVA.FLGFORM = Convert.ToString(idr["FLGFORM"]);
                ComponentesAcessoVA.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                ColecaoComponentesAcessoVA.Add(ComponentesAcessoVA);
            }
            idr.Close();
            return ColecaoComponentesAcessoVA;
        }

    }
}
