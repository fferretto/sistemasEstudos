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
    public class Segmento
    {
        [Required]
        [Display(Name = "Segmento")]
        public int CodSeg { get; set; }
        public string NomSeg { get; set; }
        public int Tipo { get; set; }

        public List<Segmento> ListaSegmento(ObjConn objConn, DadosAcesso dadosAcesso)
        {

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSULTA_SEGMENTOS";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> {
                new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo)
            };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            
            var listaSegmento = new List<Segmento>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var segmento = new Segmento();
                    {
                        segmento.CodSeg = Convert.ToInt32((dr["CODSEG"]));
                        segmento.NomSeg = (dr["NOMSEG"]).ToString();
                        segmento.Tipo = Convert.ToInt16(dr["TIPO"]);
                    }
                    listaSegmento.Add(segmento);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listaSegmento;
        }
    }
}
