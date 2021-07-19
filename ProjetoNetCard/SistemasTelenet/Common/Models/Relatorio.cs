using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Linq;
using NetCard.Common.Util;

namespace NetCard.Common.Models
{
    public class Relatorio
    {
        public int IDREL { get; set; }
        public string NOMREL { get; set; }
        public string DESCRICAO { get; set; }
        public string PROCREL { get; set; }
        public string TIPREL { get; set; }
        public string NAVIGATEURL { get; set; }

        public List<Relatorio> ListaRelatorio(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_RETORNARELATORIOS";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = new List<Parametro>
                {
                    new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                    new Parametro("@MODULO", DbType.String, Constantes.moduloWeb),
                    new Parametro("@TIPOACESSO", DbType.String, dadosAcesso.Acesso.ToUpper())
                };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;

            var listRel = new List<Relatorio>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var relatorio = new Relatorio();
                    {
                        relatorio.IDREL = Convert.ToInt32(dr["ID_REL"]);
                        relatorio.NOMREL = Convert.ToString(dr["NOMREL"]);
                        relatorio.DESCRICAO = Convert.ToString(dr["DESCRICAO"]);
                        relatorio.PROCREL = Convert.ToString(dr["NOMPROC"]);
                        relatorio.TIPREL = dr["TIPREL"].ToString();
                        relatorio.NAVIGATEURL = dr["NAVIGATEURL"].ToString();
                    }
                    listRel.Add(relatorio);
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return listRel;
        }

        public string GeraExcelModuloRelatorios(ObjConn objConn, List<ParRel> parametros, string pathArquivo, int sistema)
        {
            try
            {
                var ds = ListaDadosRelDataSet(objConn, parametros);
                var cols = Utils.MontaColunasDataTable(ds);
                return Utils.MontaExcelModulo(cols, pathArquivo, ds);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet ListaDadosRelDataSet(ObjConn objConn, List<ParRel> parametros)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var sql = parametros[0].NOMPROC;
            var cmd = db.GetStoredProcCommand(sql.ToString());

            var list = (from parametro in parametros
                        let tipoPar = Utils.ValidaParametro(parametro.TIPO)
                        select new Parametro(parametro.NOMPAR, tipoPar, parametro.VALOR)).ToList();
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            var ds = new DataSet();            
            try { ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd)); }
            catch { }
            return ds;
        }

        public void GravaLogAcessoRel(ObjConn objConn, string nomProc, string parametros)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "SP_GRAVA_LOG_MODREL";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, 1),
                               new Parametro("@CODOPE", DbType.Int32, objConn.CodOpe),
                               new Parametro("@NOMPROC", DbType.String, nomProc),
                               new Parametro("@PARAMETROS", DbType.String, parametros)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                dr.Close();
            }
            catch { if (dr != null) dr.Close(); }
        }
    }
}
