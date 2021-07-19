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
    public class Saldo
    {
        [Display(Name = "Informe o CPF ou o N° do Cartão")]
        public string Filtro { get; set; }

        public string Nome { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }
        public decimal Limite { get; set; }
        public decimal Premio { get; set; }
        public decimal Bonus { get; set; }
        public DateTime DtRenov { get; set; }
        public decimal SaldoUsu { get; set; }
        public string Habfin { get; set; }
        public decimal LimCred { get; set; }
        public decimal SaldoCred { get; set; }
        public decimal ValorCred { get; set; }
        public List<Cartao> Titular { get; set; }
        public List<Cartao> Dependente { get; set; }
        public List<Cartao> Exclusivo { get; set; }
        public string MaxParc { get; set; }
        public decimal TotalCompra { get; set; }
        public decimal ValorParc { get; set; }

        public List<Cartao> ConsultaSaldo(ObjConn objConn, DadosAcesso dadosAcesso, string filtro, out string retorno)
        {
            var listaResult = new List<Cartao>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSSALDO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                {
                    new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                    new Parametro("@FILTRO", DbType.String, filtro),
                    new Parametro("@CODEMPRESA", DbType.Int32, dadosAcesso.Codigo)
                };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {
                    var cartao = new Cartao();
                    cartao.Nome = (dr["NOME"]).ToString();
                    cartao.Cpf = (dr["CPFTIT"]).ToString();
                    cartao.CodCrt = (dr["CARTAO"]).ToString();
                    cartao.Status = Utils.GetStatus((dr["STATUSU"]).ToString());
                    cartao.Td = (dr["NUMDEPEND"]).ToString() == "00" ? "TITULAR" : "DEPENDENTE";
                    cartao.Saldo = Convert.ToDecimal(dr["SALDO"]);
                    if (dr["DTRENOV"] != DBNull.Value ) cartao.DtRenov = Convert.ToDateTime(dr["DTRENOV"]);
                    if (dadosAcesso.Sistema.cartaoPJVA == 0)
                    {
                        cartao.Limite = Convert.ToDecimal(dr["LIMITE"]);
                        cartao.Premio = Convert.ToDecimal(dr["PREMIO"]);
                        cartao.Bonus = Convert.ToDecimal(dr["BONUS"]);
                    }
                    listaResult.Add(cartao);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhum resultado encontrado.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public List<Cartao> DetalhaSaldo(ObjConn objConn, DadosAcesso dadosAcesso, string filtro, out string retorno)
        {
            var listaResult = new List<Cartao>();
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_DETALHA_SALDO ";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                {
                    new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                    new Parametro("@CPF", DbType.String, filtro)
                };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read())
                {                    
                    var cartao = new Cartao();
                    cartao.Cpf = (dr["TIPO"]).ToString();
                    cartao.Nome = (dr["DESCRICAO"]).ToString();
                    cartao.Saldo = Convert.ToDecimal(dr["SALDO"]);                    
                    cartao.Limite = Convert.ToDecimal(dr["LIMITE"]);
                    //cartao.DtRenov = Convert.ToDateTime(dr["DTPROXRENOV"]);                    
                    listaResult.Add(cartao);
                }
                dr.Close();
                retorno = listaResult.Count > 0 ? Constantes.ok : "Nenhum resultado encontrado.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null)
                    dr.Close();
            }
            return listaResult;
        }

        public decimal ConsultaSaldoContaCli(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            var saldo = 0m;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CONSSALDOCLI";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                {
                    new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                    new Parametro("@CODCLI", DbType.String, Convert.ToString(dadosAcesso.Codigo))
                };

            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;            
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) { saldo = Convert.ToDecimal(dr["SALDOCONTA"]); }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return saldo;
        }

        public string DetalheBeneficio(ObjConn objConn, string cpf)
        {
            var detalhe = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_DETALHA_BONUS";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCLI", DbType.Int32 , objConn.CodCli);
            db.AddInParameter(cmd, "@CPF", DbType.String, cpf);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                while (dr.Read()) 
                {
                    detalhe = "<p style='font-size:10px'>" + Convert.ToString(dr["BONUS"]) + ": " + Convert.ToString(dr["VALOR"]) + "<//p>";
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }
            return detalhe; 
        }

        public bool ExibeCalc(ObjConn objConexao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABCALCSALDOUSU'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }
    }
}
