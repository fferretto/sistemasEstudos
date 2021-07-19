using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace NetCard.Common.Models
{
    public class ObjConn : IObjetoConexao
    {
        public string BancoConcentrador { get; set; }
        public string BancoNetcard { get; set; }
        public string BancoAutorizador { get; set; }
        public string LibDelphiAutorizador { get; set; }
        public string LibDelphiNetcard { get; set; }
        public string ServNertCard { get; set; }
        public string ServAutorizador { get; set; }
        public string ServConcentrador { get; set; }
        public string Acesso { get; set; }
        public string LoginWeb { get; set; }
        public string CodCli { get; set; }
        public string StaCli { get; set; }
        public string CodCrt { get; set; }
        public string CodCre { get; set; }
        public string Cpf { get; set; }
        public int CodOpe { get; set; }
        public string NomOperadora { get; set; }

        public ObjConn getNomeBancos(string cliente)
        {
            ServConcentrador = ConfigurationManager.AppSettings["bdServidor"];
            BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];

            Database db = new SqlDatabase(Utils.GetConnectionStringConcentrador(this));
            const string sql = "MW_CONEXAO_BD";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro> { new Parametro("@OPERADORA", DbType.String, cliente.ToLower()) };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            this.Acesso = Constantes.concentrador;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    Acesso = null;
                    CodOpe = Convert.ToInt32(dr["CODOPE"]);
                    NomOperadora = Convert.ToString(dr["NOME"]);
                    BancoAutorizador = Convert.ToString(dr["BD_AUT"]);
                    BancoNetcard = Convert.ToString(dr["BD_NC"]);
                    ServAutorizador = Convert.ToString(dr["SERVIDOR_AUT"]);
                    ServNertCard = Convert.ToString(dr["SERVIDOR"]);
                    LibDelphiAutorizador = "Provider=SQLOLEDB.1;Password=TLN22BH22;Persist Security Info=True;" +
                                           "User ID=TLNUSUNETCARD;Initial Catalog=" + BancoAutorizador + ";" +
                                           "Data Source=" + ServAutorizador + ";Application Name=NetCardWeb;";
                    
                    LibDelphiNetcard = "Provider=SQLOLEDB.1;Password=TLN22BH22;Persist Security Info=True;" +
                                       "User ID=TLNUSUNETCARD;Initial Catalog=" + BancoNetcard + ";" +
                                       "Data Source=" + ServNertCard + ";Application Name=NetCardWeb;";



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
