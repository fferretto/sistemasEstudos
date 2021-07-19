using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetCard.Common.Models
{
    public class ModRel
    {
        public string LINHAIMP { get; set; }
        public int TIP { get; set; }

        public List<ModRel> ListaDadosRel(ObjConn objConn, DadosAcesso dadosAcesso, List<ParRel> parametros, out string erro)
        {
            var numCaracter = parametros[0].DISPOSICAO == "P" ? 172 : 118;
            var listaFech = new List<ModRel>();
            erro = string.Empty;
            if (parametros != null)
            {

                Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
                var sql = parametros[0].NOMPROC;
                var cmd = db.GetStoredProcCommand(sql.ToString());
                cmd.CommandTimeout = 60;
                var list = (from parametro in parametros
                            let tipoPar = ValidaParametro(parametro.TIPO)
                            select new Parametro(parametro.NOMPAR, tipoPar, parametro.VALOR)).ToList();

                foreach (var parametro in list)
                    db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

                IDataReader dr = null;
                
                try
                {
                    dr = db.ExecuteReader(cmd);
                    while (dr.Read())
                    {
                        if (dr.FieldCount > 1)
                        {
                            var modRel = new ModRel();
                            var linhaImp = Convert.ToString(dr["LINHAIMP"]).ToUpper();
                            modRel.LINHAIMP = linhaImp.Length > numCaracter ? linhaImp.Substring(0, numCaracter) : linhaImp;
                            modRel.TIP = Convert.ToInt16(dr["TIP"]);
                            listaFech.Add(modRel);
                        }
                        else
                            erro = Convert.ToString(dr["ERRO"]).ToUpper();
                    }
                    dr.Close();
                }
                catch
                {
                    if (dr != null)
                        dr.Close();
                }
            }
            return listaFech;
        }

        private DbType ValidaParametro(string tipoPar)
        {
            var retorno = new DbType();
            switch (tipoPar)
            {
                case "String":
                    retorno = DbType.String;
                    break;
                case "Byte":
                    retorno = DbType.Byte;
                    break;
            }
            return retorno;
        }

    }
}
