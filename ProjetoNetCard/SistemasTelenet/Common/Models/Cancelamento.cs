using Clientes;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TELENET.SIL.PO;

namespace NetCard.Common.Models
{
    public class Cancelamento
    {
        public string Acao { get; set; }
        public string Status { get; set; }
        public string Validacao { get; set; }

        public RelCanc EfetuaCancelamento(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, out string retorno)
        {
            retorno = string.Empty;
            var relCanc = new RelCanc();
            relCanc.Log =  new LOG();
            relCanc.RelSint = new List<string>();
            try
            {
                var libDelphi = new TClientes();
                libDelphi.StrConnNetCard = objConn.LibDelphiNetcard;
                libDelphi.StrConnAutoriz = objConn.LibDelphiAutorizador;
                libDelphi.LoginWeb = objConn.LoginWeb;
                libDelphi.StatCli = objConn.StaCli;

                var ds = new DataSet();
                var ds2 = new DataSet();
                var ds3 = new DataSet();
                var cod = libDelphi.CancCartao(Convert.ToString(dadosAcesso.Codigo), cpf, out ds);
                if (cod == 0)
                {
                    var dr = ds.Tables[0].Rows[0];
                    if (dr != null) relCanc.Nome = Convert.ToString(dr["NOME"]);
                    relCanc.Log.AddLog(new LOG(Convert.ToString(dadosAcesso.Codigo).PadLeft(6, ' ') + " - " + cpf, "OK",
                                        //Geração de Log de sucesso
                                        "CPF CANCELADO - PROCESSO REALIZADO COM SUCESSO"));
                }
                else
                {
                    relCanc.Log.AddLog(new LOG(Convert.ToString(dadosAcesso.Codigo).PadLeft(6, ' ') + " - " + cpf, "ERRO CANCELAMENTO",
                                        //Geração de Log de FALHA
                                        "CPF JÁ FOI CANCELADO ANTERIORMENTE"));
                }
                //Grava Log Relatório Arquivo
                cod = libDelphi.RelTransAb(0, Convert.ToString(dadosAcesso.Codigo), cpf, out ds2);
                if (cod == 0)
                {
                    relCanc.RelAnalit = new List<string>();
                    relCanc.RelAnalit.AddRange(from DataRow drRel in ds2.Tables[0].Rows select drRel["LINHAIMP"].ToString());
                }
                relCanc.RelAnalit.AddRange(new[] { "", "", "" });
                cod = libDelphi.ListaTotTransAb(0, Convert.ToString(dadosAcesso.Codigo), cpf, out ds3);
                if (cod == 0)
                {
                    relCanc.RelSint.AddRange(from DataRow drRel in ds3.Tables[0].Rows select drRel["LINHAIMP"].ToString());
                }
                retorno = Constantes.ok;
            }

            catch (Exception)
            {
                retorno = "Falha ao cancelar! Verifique o cpf: " + cpf;
            }
            return relCanc;
        }

        public string CancelaCartao(ObjConn objConn, DadosAcesso dadosAcesso, string codCrt, int codJustCanc)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_CANC_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@CARTAO", DbType.String, codCrt);
            db.AddInParameter(cmd, "@ID_JUSTIFICATIVA", DbType.Int32, codJustCanc);
            db.AddInParameter(cmd, "@IDFUNC", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                dr.Close();
                retorno = retorno != "OK"
                              ? "Falha no cancelamento! Verifique o cpf informado."
                              : "Cartão cancelado com sucesso.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return retorno;
        }

        public string SuspenderCartao(ObjConn objConn, DadosAcesso dadosAcesso, string codCrt)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_SUSP_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@CARTAO", DbType.String, codCrt);
            db.AddInParameter(cmd, "@IDFUNC", DbType.Int32, dadosAcesso.Id);
            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read()) retorno = (string)dr["RETORNO"];
                dr.Close();
                retorno = retorno != "OK"
                              ? "Falha ao suspender! Verifique o cpf informado."
                              : "Cartão suspenso com sucesso.";
            }
            catch
            {
                retorno = "Ocorreu um erro durante a operação";
                if (dr != null) dr.Close();
            }
            return retorno;
        }
    }
}
