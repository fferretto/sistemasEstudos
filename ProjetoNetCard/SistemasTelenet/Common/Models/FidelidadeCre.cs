using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class FidelidadeCre
    {
        [Display(Name = "Data Inicial")]
        public string Inicio { get; set; }
        [Display(Name = "Data Final")]
        public string Fim { get; set; }

        [Display(Name = "Pontos Gerados")]
        public int Pontos { get { return PontosManual + PontosAutomatic; } }
        [Display(Name = "Pontuação Manual")]
        public int PontosManual { get; set; }
        [Display(Name = "Pontuação Automática")]
        public int PontosAutomatic { get; set; }
        [Display(Name = "Última Atualização")]
        public DateTime UltAtualizacao { get; set; }

        public FidelidadeCre ConsultaResumoCre(ObjConn objConn, string cnpj, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "FIDELIDADE_RESUMO_PONTOS_CRED";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@DATAINI", DbType.String, Inicio);
            db.AddInParameter(cmd, "@DATAFIM", DbType.String, Fim);
            retorno = string.Empty;

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    if (Convert.ToInt16(dr["RETORNO"]) > 0)
                        retorno = Convert.ToString(dr["MENSAGEM"]);
                    else
                    {
                        this.PontosAutomatic = Convert.ToInt32(dr["PONTOS_AUTOMATICOS"]);
                        this.PontosManual = Convert.ToInt32(dr["PONTOS_MANUAIS"]);
                        this.Inicio = Inicio;
                        this.Fim = Fim;
                    }
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return this;
        }        
    }
}
