using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class Atividade
    {
        [Required]
        [Display(Name = "Atividade")]
        public int CodAti { get; set; }
        public string NomAti { get; set; }

        public List<Atividade> ListaAtividade(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_LISTATIVIDADES";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> { 
                new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA)};
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaAtividade = new List<Atividade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var atividade = new Atividade();
                    {
                        atividade.CodAti = Convert.ToInt32((dr["CODATI"]));
                        atividade.NomAti = (dr["NOMATI"]).ToString();
                    }
                    listaAtividade.Add(atividade);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaAtividade;
        }
    }
}
