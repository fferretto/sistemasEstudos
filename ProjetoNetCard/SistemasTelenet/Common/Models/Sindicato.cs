using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class Sindicato
    {
        [Required]
        [Display(Name = "Sindicato")]
        public string CodSind { get; set; }
        public string NomSind { get; set; }

        public List<Sindicato> ListaSindicato(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_LISTSINDICATOS";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            IDataReader dr = null;
            var listaSind = new List<Sindicato>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var sind = new Sindicato();
                    {
                        sind.CodSind = (dr["CODSIND"]).ToString();
                        sind.NomSind = (dr["ABREV"]).ToString();
                    }
                    listaSind.Add(sind);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaSind;
        }
    }
}
