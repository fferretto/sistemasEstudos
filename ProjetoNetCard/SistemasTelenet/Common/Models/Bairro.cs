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
    public class Bairro
    {
        [Required]
        [Display(Name = "Bairro")]
        public int CodBai { get; set; }
        public string NomBai { get; set; }
        public string CodBairro { get; set; }

        public List<Bairro> ListaBairro(ObjConn objConn, DadosAcesso dadosAcesso, string codSeg, string nomEsp, string uf, string nomLoc)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSULTA_BAIRROS";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                {
                    new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                    new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                    new Parametro("@SEGMENTO", DbType.String, codSeg),
                    new Parametro("@ESPECIALIDADE", DbType.String, nomEsp),
                    new Parametro("@UF", DbType.String, uf),
                    new Parametro("@CIDADE", DbType.String, nomLoc)
                };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var listaBairro = new List<Bairro>();
            try
            {
                var teste = DateTime.Now;
                dr = db.ExecuteReader(cmd);
                
                while (dr.Read())
                {
                    var bairro = new Bairro();
                    {
                        bairro.CodBai = Convert.ToInt32((dr["CODBAI"]));
                        bairro.NomBai = (dr["NOMBAI"]).ToString();
                    }
                    listaBairro.Add(bairro);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaBairro;
        }

        public List<Bairro> ListaBairro(ObjConn objConn)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_LISTBAIRROS";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            IDataReader dr = null;
            var listaBairro = new List<Bairro>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var bairro = new Bairro();
                    {
                        bairro.CodBai = Convert.ToInt32((dr["CODBAI"]));
                        bairro.NomBai = (dr["NOMBAI"]).ToString();
                    }
                    listaBairro.Add(bairro);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaBairro;
        }
    }
}
