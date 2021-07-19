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
    public class Cidade
    {
        [Required]
        [Display(Name = "Cidade")]
        public int id { get; set; }
        public string NomCid { get; set; }
        public string CodLoc { get; set; }

        public List<Cidade> ListaCidade(ObjConn objConn, DadosAcesso dadosAcesso, string codSeg, string nomEsp, string sigUf0)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSULTA_CIDADES";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> {
                new Parametro("@SISTEMA", DbType.String, dadosAcesso.Sistema.cartaoPJVA),
                new Parametro("@CODCLI", DbType.String, dadosAcesso.Codigo),
                new Parametro("@SEGMENTO ", DbType.String, codSeg),
                new Parametro("@ESPECIALIDADE ", DbType.String, nomEsp),
                new Parametro("@UF", DbType.String, sigUf0),
            };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            
            var listaCidade = new List<Cidade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                var i = 0;
                while (dr.Read())
                {
                    var cidade = new Cidade();
                    {
                        cidade.id = i;
                        cidade.CodLoc = dr["CODLOC"].ToString();
                        cidade.NomCid = (dr["CIDADE"]).ToString();
                    }
                    listaCidade.Add(cidade);
                    i++;
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaCidade;
        }

        public List<Cidade> ListaCidade(ObjConn objConn)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_LISTCIDADES";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            IDataReader dr = null;
            var listaCidade = new List<Cidade>();
            try
            {
                dr = db.ExecuteReader(cmd);
                var i = 0;
                while (dr.Read())
                {
                    var cidade = new Cidade();
                    {
                        cidade.id = i;
                        cidade.CodLoc = dr["CODLOC"].ToString();
                        cidade.NomCid = (dr["CIDADE"]).ToString();
                    }
                    listaCidade.Add(cidade);
                    i++;
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaCidade;
        }
    }
}
