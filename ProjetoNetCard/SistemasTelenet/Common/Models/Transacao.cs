using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Linq;
using NetCard.Common.Util;
using System.Net.Mail;
using System.Net;

namespace NetCard.Common.Models
{
    public class Transacao
    {
        [Display(Name = "Listar por Centralizadora")]
        public bool CheckCen { get; set; }

        [Display(Name = "Data da movimentação")]
        public string DtInicial { get; set; }
        public string DtFinal { get; set; }

        public string Data { get; set; }
        public int Doc { get; set; }
        public string TipTra { get; set; }
        public string DesTipo { get; set; }
        public string Cartao { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorLiq { get; set; }
        public int Cliente { get; set; }
        public string TP { get; set; }
        public string DataFech { get; set; }
        public int NroFech { get; set; }
        public int CodSubRede { get; set; }
        public int CodCre { get; set; }
        public string RazSoc { get; set; }
        public string NomUsu { get; set; }
        public string Status { get; set; }
        public decimal ValTot { get; set; }
        public string InfoParc { get; set; }

        public Transacao(){}

        public Transacao(string data, int nsu, string tiptra, string cartao, decimal valor)
        {
            Data = data;
            Doc = nsu;
            TipTra = tiptra;
            Cartao = cartao;
            Valor = valor;
        }

        public Transacao(string data, int nsu, string status, string tiptra, string cartao,
             decimal valor, decimal valorliq, string dtFech, int numfec)
        {
            Data = data;
            Doc = nsu;
            Status = status;
            Cartao = cartao;
            TipTra = tiptra;
            Valor = valor;
            ValorLiq = valorliq;
            DataFech = dtFech;
            NroFech = numfec;
        }

        public Transacao(int codSubRede, int codCre, string razSoc, string data, int nsu, 
            string cartao, decimal valor, decimal valorliq, int tipTra,
            string desTipo, string datafech)
        {
            CodSubRede = codSubRede;
            CodCre = codCre;
            RazSoc = razSoc;
            Data = data;
            Doc = nsu;
            Cartao = cartao;
            Valor = valor;
            ValorLiq = valorliq;
            TipTra = tipTra.ToString();
            DesTipo = desTipo;
            DataFech = datafech;            
        }

        public Transacao(int codSubRede, int codCre, string razSoc, string data, int nsu,
            string cartao, decimal valor, decimal valorliq, int tipTra,
            string desTipo)
        {
            CodSubRede = codSubRede;
            CodCre = codCre;
            RazSoc = razSoc;
            Data = data;
            Doc = nsu;
            Cartao = cartao;
            Valor = valor;
            ValorLiq = valorliq;
            TipTra = tipTra.ToString();
            DesTipo = desTipo;
        }

        public Transacao(string numUsu, string data, int nsu, string cartao, 
                        decimal valor, string desTipo, string datafech, int nrofech)
        {
            NomUsu = numUsu;
            Data = data;
            Doc = nsu;
            Cartao = cartao;
            Valor = valor;
            DesTipo = desTipo;
            DataFech = datafech;
            NroFech = nrofech;
        }

        public Transacao(string data, int nsu, string tp, string cartao,
                  decimal valor, int cliente, int tipo, string datafech, int nrofech, string infParcela)
        {
            Data = data;
            Doc = nsu;
            switch (tp)
            {
                case "-1":
                    TP = "";
                    break;
                case "0":
                    TP = "Titular";
                    break;
                default:
                    TP = "Dependente";
                    break;
            }
            Cartao = cartao;
            Valor = valor;
            Cliente = cliente;
            TipTra = tipo.ToString();
            DataFech = datafech;
            NroFech = nrofech;
        }

        public Transacao(string data, int nsu, string cartao, decimal valor, int tipTra,
            string desTipo, string datafech, int nrofech, string infParcela)
        {
            Data = data;
            Doc = nsu;
            Cartao = cartao;
            Valor = valor;
            TipTra = tipTra.ToString();
            DesTipo = desTipo;
            DataFech = datafech;
            NroFech = nrofech;
        }

        public Transacao(string data, int nsu, string status, string cartao,
            string tiptra, decimal valor, decimal valTot, string dtFech, int numfec, string infParc)
        {
            Data = data;
            Doc = nsu;
            Status = status;
            Cartao = cartao;
            TipTra = tiptra;
            Valor = valor;
            ValTot = valTot;
            DataFech = dtFech;
            NroFech = numfec;
            InfoParc = infParc;
        }

        public List<Transacao> ConsultaPendencias(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            retorno = string.Empty;
            var lista = new List<Transacao>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_CRED_CONSPEND";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@CODCRE", DbType.Int32, objConn.CodCre),
                               //new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@TIPO", DbType.Int32, 0),
                               //new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(DtFinal).ToString("yyyyMMdd")),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);
            
            try
            {
                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lista.AddRange(
                        from DataRow dr in ds.Tables[0].Rows
                        let data = Convert.ToDateTime(dr["DATA"])
                        let nsu = Convert.ToInt32(dr["AUTORIZ"])
                        let tiptra = Convert.ToString(dr["TIPO"])
                        let cartao = Convert.ToString(dr["CARTAO"])
                        let valor = Convert.ToDecimal(dr["VALOR"])
                        select new Transacao(Convert.ToString(data), nsu, tiptra, cartao, valor));
                }
                retorno = lista.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch { retorno = "Ocorreu um erro durante a operação"; }
            return lista;
        }

        public DetalheTotalTransacao MovimentacaoDiaria(ObjConn objConn, DadosAcesso dadosAcesso, out string retorno)
        {
            decimal subTotalValor = 0m, subTotalValorLiq = 0m;
            decimal totalValor = 0m, totalValorLiq = 0m;
            var numfech = 0;
            var dtfech = string.Empty;
            var totais = new DetalheTotalTransacao();
            var pCodCen = (dadosAcesso.Codigo == dadosAcesso.CodCen && CheckCen);

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_REL_LISTTRANSDIA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                               new Parametro("@DATA_INI", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@DATA_FIM", DbType.String, Convert.ToDateTime(DtInicial).ToString("yyyyMMdd")),
                               new Parametro("@PARAM_INI", DbType.Int32, objConn.CodCre),
                               new Parametro("@PARAM_FIM", DbType.Int32, objConn.CodCre),
                               new Parametro("@CENTRALIZ", DbType.Int32, pCodCen? 1 : 0),
                               new Parametro("@FORMATO", DbType.Int32, 2)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            try
            {

                var ds = Utils.ConvertDataReaderToDataSet(db.ExecuteReader(cmd));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var drAux = ds.Tables[0].Rows[0];
                    var razao = (string)drAux["RAZSOC"];
                    var codcre = (int)drAux["CODCRE"];
                    var codAux = codcre;
                    var cnpj = drAux["CNPJ"].ToString();
                    var parcial = new DetalheParcialTransacao(razao, codcre, cnpj);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        codcre = (int)dr["CODCRE"];
                        if (!codAux.Equals(codcre))
                        {
                            parcial.SubTotalValor = subTotalValor;
                            parcial.SubTotalValorLiq = subTotalValorLiq;
                            totais.add(parcial);
                            razao = (string)dr["RAZSOC"];
                            codcre = (int)dr["CODCRE"];
                            cnpj = dr["CNPJ"].ToString();
                            parcial = new DetalheParcialTransacao(razao, codcre, cnpj);
                            subTotalValor = 0;
                            subTotalValorLiq = 0;
                            codAux = codcre;
                        }
                        
                        var data = Convert.ToDateTime(dr["DATA"]);
                        var nsu = Convert.ToInt32(dr["AUTORIZ"]);
                        var tipTra = dr["TRANSACAO"].ToString();
                        var status = dr["STATUS"].ToString();
                        var cartao = dr["CARTAO"].ToString();
                        var valor = (decimal)dr["VALOR"];
                        var valorliq = (decimal)dr["VAL_LIQ"];
                        if (!string.IsNullOrEmpty(dr["DTFECH"].ToString())) dtfech = Convert.ToString(dr["DTFECH"]);
                        if (!string.IsNullOrEmpty(dr["NUMFEC"].ToString())) numfech = Convert.ToInt32(dr["NUMFEC"]);
                        if (status == "VALIDA")
                        {
                            totalValor += valor;
                            totalValorLiq += valorliq;
                            subTotalValor += valor;
                            subTotalValorLiq += valorliq; 
                        }
                        parcial.add(new Transacao(Convert.ToString(data), nsu, status, tipTra, cartao, valor, valorliq, dtfech, numfech));
                    }
                    parcial.SubTotalValor = subTotalValor;
                    parcial.SubTotalValorLiq = subTotalValorLiq;
                    totais.add(parcial);
                    totais.TotalValor = totalValor;
                    totais.TotalValorLiq = totalValorLiq;
                }
                retorno = totais.Parciais.Count > 0 ? Constantes.ok : "Nenhuma informação encontrada.";
            }
            catch 
            {
                retorno = "Ocorreu um erro durante a operação";
            }

            return totais;
        }
    }
}