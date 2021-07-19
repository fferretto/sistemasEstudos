using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace NetCard.Common.Models
{
    public class ExtratoCB
    {
        public ExtratoCB() { }
        public ExtratoCB(DateTime dataCompra, int codCre, string estabelecimento, decimal valor, decimal cashback)
        {
            DataCompra = dataCompra;
            CodCre = codCre;
            Estabelecimento = estabelecimento;
            Valor = valor;
            Cashback = cashback;
        }

        public DateTime DataCompra { get; set; }
        public int CodCre { get; set; }
        public string Estabelecimento { get; set; }
        public decimal Valor { get; set; }
        public decimal Cashback { get; set; }

        public List<ExtratoCB> ConsultaExtratoCashBack(ObjConn objConn, DadosAcesso dadosAcesso, DateTime dataIni, DateTime dataFim)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "CB_CONSULTA_LOG";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            //db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "NUMTIT ", DbType.Int32, dadosAcesso.NumTit);
            db.AddInParameter(cmd, "DATINI", DbType.String, dataIni.ToString("yyyyMMdd"));
            db.AddInParameter(cmd, "DATFIM", DbType.String, dataFim.ToString("yyyyMMdd"));

            var extrato = new List<ExtratoCB>();
            var dr = db.ExecuteReader(cmd);

            while (dr.Read())
            {
                var trans = new ExtratoCB(
                    Convert.ToDateTime(dr["DIA"]),
                    Convert.ToInt32(dr["CODCRE"]),
                    Convert.ToString(dr["FANTASIA"]),
                    Convert.ToDecimal(dr["VTOTAL_TRANSACAO"]),
                    Convert.ToDecimal(dr["VCASH_BACK"]));

                extrato.Add(trans);
            }

            return extrato;
        }

        public decimal ConsultaSaldoCashBack(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "CB_CONSULTA_SALDO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            //db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "NUMTIT ", DbType.Int32, dadosAcesso.NumTit);

            var extrato = new List<ExtratoCB>();
            var dr = db.ExecuteReader(cmd);

            if (dr.Read())
            {
                return Convert.ToDecimal(dr["CASHBACK"]);
            }

            return 0m;
        }

        public bool RegatarCashBack(ObjConn objConn, DadosAcesso dadosAcesso, decimal valorResgate, out string msgRetorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "CB_RESGATE";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            //db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "NUMTIT ", DbType.Int32, dadosAcesso.NumTit);
            db.AddInParameter(cmd, "VALOR_RESGATE ", DbType.Decimal, valorResgate);
            db.AddInParameter(cmd, "RESULTSET ", DbType.String, "N");

            db.AddOutParameter(cmd, "RETOUT", DbType.String, 128);
            db.AddOutParameter(cmd, "MENSOUT", DbType.String, 128);
            db.AddOutParameter(cmd, "CODRETOUT", DbType.Int16, 128);

            var sucesso = false;

            try
            {
                db.ExecuteNonQuery(cmd);
                var mensagem = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MENSOUT"));
                var codRet = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CODRETOUT"));
                if (codRet == "0") { sucesso = true; }
                msgRetorno = mensagem;
            }
            catch
            {
                msgRetorno = "Ocorreu um erro durante a operação";
                sucesso = false;
            }
            return sucesso;
        }

        public bool RealizaAdesaoCashBack(ObjConn objConn, DadosAcesso dadosAcesso, out string msgRetorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "CB_ADESAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            //db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "NUMTIT ", DbType.Int32, dadosAcesso.NumTit);
            db.AddInParameter(cmd, "RESULTSET ", DbType.String, "N");

            db.AddOutParameter(cmd, "RETOUT", DbType.String, 128);
            db.AddOutParameter(cmd, "MENSOUT", DbType.String, 128);
            db.AddOutParameter(cmd, "CODRETOUT", DbType.Int16, 128);

            var sucesso = false;

            try
            {
                db.ExecuteNonQuery(cmd);
                var mensagem = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MENSOUT"));
                var codRet = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CODRETOUT"));
                if (codRet == "0")
                {
                    msgRetorno = "";
                    sucesso = true;
                }
                else
                {
                    msgRetorno = mensagem;
                }
            }
            catch
            {
                msgRetorno = "Ocorreu um erro durante a operação";
                sucesso = false;
            }
            return sucesso;
        }

        public bool ConsultaAdesao(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "SELECT ADERIU FROM CB_USUARIO WITH (NOLOCK) WHERE ID_USUARIO = @ID_USUARIO AND CODCLI = @CODCLI";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            //db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            //db.AddInParameter(cmd, "NUMTIT ", DbType.Int32, dadosAcesso.NumTit);

         
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";

        }
    }
}
