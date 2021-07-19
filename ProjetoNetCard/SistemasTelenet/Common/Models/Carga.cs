using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace NetCard.Common.Models
{
    public class Carga
    {
        public int OpcaoPesq { get; set; }
        public string Filtro { get; set; }
        public decimal Valor { get; set; }

        [Display(Name = "Centro de Custo")]
        public string CentroCusto { get; set; }

        public string ResultadoPesquisa { get; set; }
        public string Cpf { get; set; }
        public string Matricula { get; set; }
        public string Cartao { get; set; }

        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public int NumCarga { get; set; }
        public int NumCartoes { get; set; }
        public DateTime? DataAut { get; set; }
        public DateTime? DataCarga { get; set; }
        public string DataProg { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoContaCli { get; set; }
        public string StatusCarga { get; set; }
        public string Situacao { get; set; }
        public string Acao { get; set; }
        public string Validacao { get; set; }
        public List<Cartao> ListaCartoes { get; set; }

        public Carga CargaProgramada(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSCARGA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>();
            list.Add(new Parametro("@SISTEMA", DbType.Int16, 1));
            list.Add(new Parametro("@CODCLI", DbType.Int32, Convert.ToInt32(dadosAcesso.Codigo)));
            list.Add(new Parametro("@CODCRT", DbType.String, dadosAcesso.CodCrt));

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    if (Convert.ToString(dr["RETORNO"]) == Constantes.ok)
                    {
                        DataProg = Convert.ToDateTime(dr["DTPROG"]).ToShortDateString();
                        Total = Convert.ToDecimal(dr["VCARGAUTO"]);
                        retorno = Constantes.ok;
                    }
                    else
                    {
                        retorno = Convert.ToString(dr["RETORNO"]);
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
