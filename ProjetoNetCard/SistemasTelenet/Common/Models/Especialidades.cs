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
    public class Especialidades
    {
        [Required]
        [Display(Name = "Especialidades")]
        public int CodEsp { get; set; }
        public string NomEsp { get; set; }

        public List<Especialidades> ListaEspecialidades(ObjConn objConn, DadosAcesso dadosAcesso, string nomSeg)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSULTA_ESPECIALIDADES";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> {
                new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                new Parametro("@SEGMENTO", DbType.String, nomSeg)
            };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            IDataReader dr = null;
            var listaEspecialidades = new List<Especialidades>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var especialidade = new Especialidades();
                    {
                        especialidade.CodEsp = Convert.ToInt32((dr["CODESP"]));
                        especialidade.NomEsp = (dr["ESPECIALIDADE"]).ToString();
                    }
                    listaEspecialidades.Add(especialidade);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaEspecialidades;
        }
    }
}
