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
    public class ListCarga : Carga
    {
        [Display(Name = "Data Inicial")]
        public string DtInicial { get; set; }

        [Display(Name = "Data Final")]
        public string DtFinal { get; set; }

        public List<ListCarga> ListaCargas { get; set; }

        public List<DetalheCarga> DetalheCargas {get; set;}

        public List<Carga> ListagemCarga(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LISTCARGAANA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@TIPO", DbType.String, 0),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            var colecaoListaCarga = new List<Carga>();
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var carga = new Carga();
                    carga.NumCarga = Convert.ToInt32(dr["NUMCARGA"]);
                    carga.DataAut = Convert.ToDateTime(dr["DTAUTORIZ"]);
                    carga.DataCarga = Convert.ToDateTime(dr["DTCARGA"]);
                    carga.Valor = Convert.ToDecimal(dr["VALTOTAL"]);
                    carga.NumCartoes = Convert.ToInt32(dr["QUANT_CARTOES"]);
                    colecaoListaCarga.Add(carga);
                }
                dr.Close();
                retorno = colecaoListaCarga.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return colecaoListaCarga;
        }

        public ListCarga ListagemDetalhadaCarga(ObjConn objConn, DadosAcesso dadosAcesso, int numCarga, string centrocusto, out string retorno)
        {
            var listaCarga = new ListCarga();
            var colecaoListaCarga = new List<ListCarga>();
            var carga = new ListCarga();
            carga.DetalheCargas = new List<DetalheCarga>();
            var detalheCarga = new DetalheCarga();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LISTCARGAANA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@CODCLI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo)),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),                               
                               new Parametro("@NUMCARGA", DbType.Int32, numCarga),
                               new Parametro("@CCUSTO", DbType.String, centrocusto),
                               new Parametro("@TIPO", DbType.String, 1),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);           
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                retorno = "Nenhuma informação encontrada.";
                listaCarga.ListaCargas = colecaoListaCarga;
                if (ds.Tables[0].Rows.Count == 0) return listaCarga;
                var drAux = ds.Tables[0].Rows[0];
                carga.Nome = dadosAcesso.Nome;
                carga.Cnpj = dadosAcesso.Cnpj;
                carga.NumCarga = (int) drAux["NUMCARGA"];
                var centroCusto = drAux["CENTROCUSTO"].ToString();
                carga.DataAut = Convert.ToDateTime(drAux["DTAUTORIZ"]);
                carga.DataCarga = Convert.ToDateTime(drAux["DTCARGA"]);
                carga.Total = (decimal) drAux["VALTOTAL"];
                var numcargaaux = carga.NumCarga;
                var ccustoaux = centroCusto;
                decimal totalCentrocusto = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    detalheCarga.NumCarga = (int) dr["NUMCARGA"];
                    if (detalheCarga.NumCarga != numcargaaux)
                    {
                        var totalCCusto = new DetalheCarga();
                        totalCCusto.Valor = totalCentrocusto;
                        totalCCusto.CentroCusto = "TOTAL C.CUSTO";
                        carga.DetalheCargas.Add(totalCCusto);
                        totalCentrocusto = 0;
                        colecaoListaCarga.Add(carga);
                        carga = new ListCarga();
                        carga.DetalheCargas = new List<DetalheCarga>();
                        
                        ccustoaux = dr["CENTROCUSTO"].ToString();
                        carga.Nome = dadosAcesso.Nome;
                        carga.Cnpj = dadosAcesso.Cnpj;
                        carga.NumCarga = (int)dr["NUMCARGA"];
                        carga.DataAut = Convert.ToDateTime(dr["DTAUTORIZ"]);
                        carga.DataCarga = Convert.ToDateTime(dr["DTCARGA"]);
                        carga.Total = (decimal)dr["VALTOTAL"];
                        numcargaaux = carga.NumCarga;
                    }

                    detalheCarga = new DetalheCarga();
                    detalheCarga.TitNome = dr["NOMUSU"].ToString();
                    detalheCarga.Matricula = dr["MATRICULA"].ToString();
                    detalheCarga.Cartao = (string)dr["CARTAO"];
                    detalheCarga.Valor = (decimal) dr["VALCARGA"];
                    detalheCarga.CentroCusto = dr["CENTROCUSTO"].ToString();

                    if (detalheCarga.CentroCusto != ccustoaux)
                    {
                        var totalCCusto = new DetalheCarga();
                        totalCCusto.Valor = totalCentrocusto;
                        totalCentrocusto = 0;
                        totalCCusto.CentroCusto = "TOTAL C.CUSTO";
                        ccustoaux = detalheCarga.CentroCusto;
                        carga.DetalheCargas.Add(totalCCusto);
                    }
                    totalCentrocusto += detalheCarga.Valor;
                    carga.DetalheCargas.Add(detalheCarga);
                }
                var totalCCustoFinal = new DetalheCarga();
                totalCCustoFinal.Valor = totalCentrocusto;
                totalCCustoFinal.CentroCusto = "TOTAL C.CUSTO";
                carga.DetalheCargas.Add(totalCCustoFinal);
                colecaoListaCarga.Add(carga);
                listaCarga.ListaCargas = colecaoListaCarga;
                retorno = colecaoListaCarga.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
            }
            return listaCarga;
        }

        public List<DetalheCarga> ListagemDetalhadaCargaSolicitada(IObjetoConexao objConn, IDadosAcesso dadosAcesso, int numCarga, out string retorno)
        {   
            var listCarga = new List<DetalheCarga>();            
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "REL_CARGA_AUT";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@PARAM_INI", DbType.Int32, Convert.ToString(dadosAcesso.Codigo));
            db.AddInParameter(cmd, "@PARAM_FIM", DbType.Int32, Convert.ToString(dadosAcesso.Codigo));
            db.AddInParameter(cmd, "@NUMCARGA", DbType.Int32, numCarga);
            db.AddInParameter(cmd, "@TIPO", DbType.Int32, 1);
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, 1);
            db.AddInParameter(cmd, "@FORMATO", DbType.Int32, 2);
            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var carga = new DetalheCarga();
                    carga = new DetalheCarga();
                    carga.NumCarga = numCarga;
                    carga.TitNome = dr["NOMUSU"].ToString();
                    carga.Cpf = dr["CPF"].ToString();
                    carga.Cartao = (string)dr["CODCRT"];
                    carga.Valor = (decimal)dr["VCARGAUTO"];
                    listCarga.Add(carga); 
                }
                dr.Close();
                retorno = listCarga.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
            }
            return listCarga;
        } 

        public string GetPastaArquivosCarga(ObjConn objConn)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "SELECT VAL FROM CONFIG_JOBS WITH (NOLOCK) WHERE ID0 = 'DIR_ARQ_CARGA'";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var retorno = string.Empty;
            retorno = Convert.ToString(db.ExecuteScalar(cmd));                
            return retorno;
        }
    }
}
