using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.DA;
using TELENET.SIL.PO;

namespace SIL.DAL
{
    public class daCarga
    {
        string BDTELENET = string.Empty;
        string BDAUTORIZADOR = string.Empty;

        OPERADORA FOperador;
        public daCarga(OPERADORA Operador)
        {
            FOperador = Operador;

            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public CARGAAUTO CargaAutoHabilitada()
        {
            var cargaAuto = new CARGAAUTO();
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'HABCARGAUTO'";
            var cmd = db.GetSqlStringCommand(sql);
            var result = db.ExecuteScalar(cmd);
            cargaAuto.EXIBECARGAAUTO = (result != null);
            cargaAuto.HABCARGAUTO = Convert.ToString(result) == "S";
            if (cargaAuto.HABCARGAUTO)
            {
                cargaAuto.MAXVALCARGAUTO = ValCargAuto();
                cargaAuto.TEMPCARGAUTO = TempCargAuto();
            }
            return cargaAuto;
        }

        public void LigaDesligaCargaAuto(bool acao)
        {            
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            { 
                const string sql = "UPDATE PARAMVA SET VAL = @VAL WHERE ID0 = 'HABCARGAUTO'";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "VAL", DbType.String, acao ? "S" : "N");
                db.ExecuteScalar(cmd);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "UPDATE PARAMVA SET VAL = @VAL WHERE ID0 = 'HABCARGAUTO'", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception)
            {
                dbt.Rollback();                
            }
            finally
            {
                dbc.Close();
            }
            return;
        }

        public decimal ValCargAuto()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'MAXVALCARGAUTO'";
            var cmd = db.GetSqlStringCommand(sql);
            decimal valCargAuto = Convert.ToDecimal(db.ExecuteScalar(cmd));
            valCargAuto = valCargAuto > 0 ? valCargAuto : 50000;
            return valCargAuto;
        }

        public int TempCargAuto()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'TEMPCARGAUTO'";
            var cmd = db.GetSqlStringCommand(sql);
            int tempCargAuto = Convert.ToInt32(db.ExecuteScalar(cmd));
            tempCargAuto = tempCargAuto > 0 ? tempCargAuto : 120;
            return tempCargAuto;
        }

        public List<CARGAC_VA> ListaCarga_VA(string Selecao, string Tipo, string ParamIni, string ParamFim)
        {
            IDataReader idr = null;
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_REL_CARGA_AUT";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "CLASSIF", DbType.Byte, Selecao);
            db.AddInParameter(cmd, "TIPO", DbType.Byte, Tipo);
            db.AddInParameter(cmd, "PARAM_INI", DbType.String, ParamIni);
            db.AddInParameter(cmd, "PARAM_FIM", DbType.String, ParamFim);

            var listaCarga = new List<CARGAC_VA>();

            try
            {
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var carga = new CARGAC_VA();
                    carga.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                    carga.NOMCLI = idr["NOMCLI"].ToString();
                    carga.NUMCARG_VA = Convert.ToInt32(idr["NUMCARG_VA"]);
                    carga.DTAUTORIZ = string.IsNullOrEmpty(idr["DTAUTORIZ"].ToString()) ? "" : Convert.ToDateTime(idr["DTAUTORIZ"]).ToShortDateString();
                    carga.DTPROG = string.IsNullOrEmpty(idr["DTPROG"].ToString()) ? "" : Convert.ToDateTime(idr["DTPROG"]).ToShortDateString();
                    carga.VCARGAUTO = Convert.ToDecimal(idr["VCARGAUTO"]);

                    //Analitico
                    if (Tipo == "1")
                    {
                        carga.CODCRT = idr["CODCRT"].ToString();
                        carga.NOMUSU = idr["NOMUSU"].ToString();
                        carga.CPF = idr["CPF"].ToString();
                        carga.VCARGAUTO = string.IsNullOrEmpty(idr["VCARGAUTO"].ToString()) ? 0m : Convert.ToDecimal(idr["VCARGAUTO"]);
                    }

                    listaCarga.Add(carga);
                }
            }
            finally
            {
                if (idr != null)
                    idr.Close();
            }
                       
            return listaCarga;
        }

        public List<CARGA_EFETUADAS> ListaCargaEfetuadas(Hashtable filtros)
        {
            var Selecao = filtros["Selecao"].ToString();
            var Tipo = filtros["TipoRelatorio"].ToString();
            var ParamIni = !string.IsNullOrEmpty(filtros["ParamIni"].ToString()) ? filtros["ParamIni"].ToString() : null;
            var ParamFim = !string.IsNullOrEmpty(filtros["ParamFim"].ToString()) ? filtros["ParamFim"].ToString() : null;
            var DataIni = Convert.ToDateTime(filtros["DataIni"]) != DateTime.MinValue ? Convert.ToDateTime(filtros["DataIni"]).ToString("yyyyMMdd") : null;
            var DataFim = string.Empty;
            if (Convert.ToDateTime(filtros["DataFim"]) != DateTime.MaxValue)
                DataFim = Convert.ToDateTime(filtros["DataFim"]).ToString("yyyyMMdd");
            IDataReader idr = null;
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_REL_CARGA_EFET";
            var cmd = db.GetStoredProcCommand(sql);
            cmd.CommandTimeout = 600;
            db.AddInParameter(cmd, "CLASSIF", DbType.Byte, Selecao);
            db.AddInParameter(cmd, "TIPO", DbType.Byte, Tipo);
            db.AddInParameter(cmd, "PARAM_INI", DbType.String, ParamIni);
            db.AddInParameter(cmd, "PARAM_FIM", DbType.String, ParamFim);
            db.AddInParameter(cmd, "DATINI", DbType.String, DataIni);
            db.AddInParameter(cmd, "DATFIM", DbType.String, DataFim);

            var listaCarga = new List<CARGA_EFETUADAS>();          
            try
            {
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var carga = new CARGA_EFETUADAS();
                    carga.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                    carga.CGC = idr["CGC"].ToString();
                    carga.NOMCLI = idr["NOMCLI"].ToString();
                    carga.NUMCARG_VA = Convert.ToInt32(idr["NUMCARG_VA"]);
                    carga.DTAUTORIZ = string.IsNullOrEmpty(idr["DTAUTORIZ"].ToString()) ? "" : Convert.ToDateTime(idr["DTAUTORIZ"]).ToShortDateString();
                    carga.VALOR = string.IsNullOrEmpty(idr["VALOR"].ToString()) ? 0 : Convert.ToDouble(idr["VALOR"]);
                    carga.TAXSER = string.IsNullOrEmpty(idr["TAXSER"].ToString()) ? 0 : Convert.ToDouble(idr["TAXSER"]);
                    carga.PRAPAG = string.IsNullOrEmpty(idr["PRAPAG"].ToString()) ? 0 : Convert.ToInt32(idr["PRAPAG"]);
                    DateTime aux;
                    DateTime.TryParse(idr["DTCARGA"].ToString(), out aux);
                    carga.DTCARGA = aux.Ticks > new DateTime(1900, 1, 1).Ticks ? aux.ToShortDateString() : "Cancelada";
                    carga.VAL2AVIA = string.IsNullOrEmpty(idr["VAL2AVIA"].ToString()) ? 0 : Convert.ToDouble(idr["VAL2AVIA"]);
                    carga.TAXADESAO = string.IsNullOrEmpty(idr["TAXADESAO"].ToString()) ? 0 : Convert.ToDouble(idr["TAXADESAO"]);
                    carga.TAXACRT = carga.TAXSER;

                    //Analitico
                    if (Tipo == "1")
                    {
                        carga.CODCRT = idr["CODCRT"].ToString();
                        carga.NOMUSU = idr["NOMUSU"].ToString();
                        carga.TAXACRT = string.IsNullOrEmpty(idr["TAXACRT"].ToString()) ? 0 : Convert.ToDouble(idr["TAXACRT"]);
                        carga.VCARGAUTO = string.IsNullOrEmpty(idr["VALCARGA"].ToString()) ? 0m : Convert.ToDecimal(idr["VALCARGA"]);
                        carga.VALSEG = string.IsNullOrEmpty(idr["VALSEG"].ToString()) ? 0 : Convert.ToDouble(idr["VALSEG"]);
                        carga.ANUIMENS = string.IsNullOrEmpty(idr["ANUIMENS"].ToString()) ? 0 : Convert.ToDouble(idr["ANUIMENS"]);
                    }

                    listaCarga.Add(carga);
                }
            }
            finally
            {
                if (idr != null)
                    idr.Close();
            }
            return listaCarga;
        }

        public int CargaDiasEfet()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'CARGA_DIAS_EFET'";
            var cmd = db.GetSqlStringCommand(sql);
            int cargaDiasEfet = Convert.ToInt16(db.ExecuteScalar(cmd));
            cargaDiasEfet = cargaDiasEfet > 0 ? cargaDiasEfet : 60;
            return cargaDiasEfet;
        }

        public List<CLIENTE_CARGA> ListaCargaNaoFinalizadas(bool listaCargasAutomaticas)
        {
            var colecao = new List<CLIENTE_CARGA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            var habilitaNovaCarga = new daControleAcessoVA(FOperador).NovaCargaAtiva();
            int cargaDiasEfet = new daCarga(FOperador).CargaDiasEfet();
            var cargAuto = CargaAutoHabilitada();

            if (habilitaNovaCarga)
            {
                sql.AppendLine("SELECT G.LIBCARGA, G.PERCENTUAL, C.CODCLI, C.PGTOANTECIPADO, G.IDOPERCLIWEB, G.IDLIBCARGOPR, ");
                sql.AppendLine("C.NOMCLI, G.NUMCARG_VA, G.DTAUTORIZ, G.DTPROG, G.DTPGTO, P.ROTULO, ");
                sql.AppendLine("G.QUANT_A_CARREGAR AS QUANTUSU, G.VALOR_A_CARREGAR AS VALOR, C.STA, G.BLOQ, G.LIBVENC, G.LIDA ");
                sql.AppendLine("FROM VPRODUTOSCLI C WITH (NOLOCK) ");
                sql.AppendLine("INNER JOIN CARGAC G WITH (NOLOCK) ON C.CODCLI = G.CODCLI ");
                sql.AppendLine("INNER JOIN PRODUTO P WITH (NOLOCK) ON (C.CODPROD = P.CODPROD)");
                sql.AppendLine("WHERE C.NSUCARGA is not null ");
                sql.AppendLine("and C.NSUCARGA <> 0 ");
                sql.AppendLine("and G.DTCARGA is Null ");                
                sql.AppendLine("order by C.CODCLI, G.DTAUTORIZ, G.NUMCARG_VA ");
            }
            else
            {
                sql.AppendLine("SELECT G.LIBCARGA, G.PERCENTUAL, C.CODCLI, C.PGTOANTECIPADO, G.IDOPERCLIWEB, G.IDLIBCARGOPR, ");
                sql.AppendLine("C.NOMCLI, G.NUMCARG_VA, G.DTAUTORIZ, G.DTPROG, G.DTPGTO, P.ROTULO, ");
                sql.AppendLine("COUNT(U.CPF) AS QUANTUSU, SUM(U.VCARGAUTO) AS VALOR, C.STA, G.BLOQ, G.LIBVENC, G.LIDA ");
                sql.AppendLine("FROM CLIENTEVA C WITH (NOLOCK) ");
                sql.AppendLine("INNER JOIN CARGAC G WITH (NOLOCK) ON C.CODCLI = G.CODCLI ");
                sql.AppendLine("INNER JOIN USUARIOVA U WITH (NOLOCK) ON U.CODCLI = G.CODCLI AND U.ULTCARGVA = G.NUMCARG_VA ");
                sql.AppendLine("INNER JOIN PRODUTO P WITH (NOLOCK) ON (C.CODPROD = P.CODPROD)");
                sql.AppendLine("WHERE C.NSUCARGA is not null ");
                sql.AppendLine("and C.NSUCARGA <> 0 ");
                sql.AppendLine("and G.DTCARGA is Null ");
                sql.AppendLine("group by G.IDOPERCLIWEB, G.IDLIBCARGOPR, C.STA,G.LIBCARGA, G.PERCENTUAL, C.CODCLI, ");
                sql.AppendLine("         C.PGTOANTECIPADO, C.NOMCLI, G.NUMCARG_VA, G.DTAUTORIZ, G.DTPROG, G.DTPGTO, P.ROTULO, G.BLOQ, G.LIBVENC ");
                sql.AppendLine("order by C.CODCLI, G.NUMCARG_VA ");
            }

            var cmd = db.GetSqlStringCommand(sql.ToString());
            if ((cargAuto.HABCARGAUTO && cargAuto.EXIBECARGAAUTO) && !listaCargasAutomaticas)
                db.AddInParameter(cmd, "VALCARGAUTO", DbType.String, cargAuto.MAXVALCARGAUTO);

            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var clienteCarga = new CLIENTE_CARGA();
                clienteCarga.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                clienteCarga.NOMCLIENTE = idr["NOMCLI"].ToString();
                clienteCarga.NUMCARGA = Convert.ToInt32(idr["NUMCARG_VA"]);
                clienteCarga.PGTOANTECIPADO = idr["PGTOANTECIPADO"].ToString() == "S";
                clienteCarga.BLOQ = idr["BLOQ"].ToString() == "S";
                clienteCarga.LIBVENC = Convert.ToString(idr["LIBVENC"]);
                clienteCarga.CARGA_DIAS_EFET = cargaDiasEfet;
                clienteCarga.ROTULO = Convert.ToString(idr["ROTULO"]);
                clienteCarga.LIDA = idr["LIDA"] == DBNull.Value ? false : idr["LIDA"].ToString() == "S";
                DateTime dataAux;
                DateTime.TryParse(idr["DTAUTORIZ"].ToString(), out dataAux);
                clienteCarga.DATAUTORDT = dataAux;
                clienteCarga.STACLI = idr["STA"].ToString();
                if (!string.IsNullOrEmpty(idr["IDOPERCLIWEB"].ToString()))
                    clienteCarga.IDOPERCLIWEB = Convert.ToInt32(idr["IDOPERCLIWEB"]);
                if (!string.IsNullOrEmpty(idr["IDLIBCARGOPR"].ToString()))
                    clienteCarga.IDLIBCARGOPR = Convert.ToInt32(idr["IDLIBCARGOPR"]);
                if (!string.IsNullOrEmpty(idr["QUANTUSU"].ToString()))
                    clienteCarga.QTDUSU = Convert.ToInt32(idr["QUANTUSU"]);
                if (!string.IsNullOrEmpty(idr["VALOR"].ToString()))
                    clienteCarga.VALOR = Convert.ToDecimal(idr["VALOR"]);
                clienteCarga.DTPGTO = idr["DTPGTO"] != DBNull.Value;
                if ((clienteCarga.IDOPERCLIWEB > 0) && (idr["IDLIBCARGOPR"] == DBNull.Value))
                {
                    clienteCarga.VIACONSULTA = true;   
                    if (!string.IsNullOrEmpty(idr["DTPROG"].ToString()))
                    {
                        DateTime.TryParse(idr["DTPROG"].ToString(), out dataAux);
                        clienteCarga.DATPROGDT = dataAux;
                    }
                }
                else
                    if (!string.IsNullOrEmpty(idr["DTPROG"].ToString()))
                    {
                        DateTime.TryParse(idr["DTPROG"].ToString(), out dataAux);
                        clienteCarga.DATPROGDT = dataAux;
                        clienteCarga.PROGRAMA = true;
                        clienteCarga.VENCIDA = clienteCarga.DATPROGDT < DateTime.Now;
                    }
                    else
                    {
                        if (idr["LIBCARGA"].ToString().ToUpper() == "S")
                        {
                            clienteCarga.EMPROCESSAMENTO = true;
                            clienteCarga.PROGRAMA = false;
                            clienteCarga.CARREGA = false;
                            int auxInt;
                            int.TryParse(idr["PERCENTUAL"].ToString(), out auxInt);
                            clienteCarga.PERCENTUAL = auxInt;
                        }
                    }                                              
                colecao.Add(clienteCarga);
            }
            idr.Close();

            if (cargAuto.HABCARGAUTO && cargAuto.EXIBECARGAAUTO && listaCargasAutomaticas)
            {
                var listaCargAut = new List<CLIENTE_CARGA>();
                var listaCargNaoAut = new List<CLIENTE_CARGA>();
                foreach (var item in colecao)
                {
                    if (item.VALOR < cargAuto.MAXVALCARGAUTO && 
                        item.DATPROGDT == DateTime.MinValue && 
                        !item.PGTOANTECIPADO && 
                        DateTime.Now.Subtract(item.DATAUTORDT).Days < item.CARGA_DIAS_EFET)
                        listaCargAut.Add(item);
                    else
                        listaCargNaoAut.Add(item);
                }                
                colecao = new List<CLIENTE_CARGA>();
                colecao.AddRange(listaCargAut);
                colecao.AddRange(listaCargNaoAut);
            }
            return colecao;
        }

        public List<LogCarga> GetLogCargas()
        {
            var colecao = new List<LogCarga>();            
            Database db = new SqlDatabase(BDTELENET);
            var sql = "SELECT DATA, CODCLI, NUMCARG_VA, DTCARGA, CODERRO, MENSAGEM, CPF, STA, VALOR FROM CARGA_EFETUADA_LOG WITH (NOLOCK) WHERE DATA >= GETDATE() - 30";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var logCarga = new LogCarga() {
                    DATA = Convert.ToDateTime(idr["DATA"]),
                    CODCLI = Convert.ToInt32(idr["CODCLI"]),
                    NUMCARG_VA = Convert.ToInt32(idr["NUMCARG_VA"]),
                    DTCARGA = Convert.ToDateTime(idr["DTCARGA"]),
                    CODERRO = Convert.ToInt32(idr["CODERRO"]),
                    MENSAGEM = Convert.ToString(idr["MENSAGEM"]),
                    CPF = Convert.ToString(idr["CPF"]),
                    STA = Convert.ToString(idr["STA"]),
                    VALOR = idr["VALOR"] != DBNull.Value ? Convert.ToDecimal(idr["VALOR"]) : 0
                };
                colecao.Add(logCarga);
            }
            idr.Close();
            return colecao;
        }
        public void ConfirmaCancelaCarga(string id_Processo)
        {   
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" EXEC CARGA_ENCERRA_STATUS '" + id_Processo + "'");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteReader(cmd);
        }
        public void ConfirmaPgtoCarga(string CODCLI, string NUMCARGA, string IDLIBPGTOOPR)
        {   
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" EXEC PROC_CONFIRMAPGTOCARGA " + CODCLI + " , " + NUMCARGA + " , " + IDLIBPGTOOPR);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteReader(cmd);
        }
        public List<CARGA_PENDENTE> PopulaCargasSolicitadas()
        {
            var colecao = new List<CARGA_PENDENTE>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" EXEC CARGA_CONSULTA_SOLICITACOES ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var CargaAguarLibPgto = new CARGA_PENDENTE();

                CargaAguarLibPgto.DATA = idr["DATA"].ToString();
                CargaAguarLibPgto.LOGIN = idr["LOGIN"].ToString();
                CargaAguarLibPgto.NOME_ARQUIVO = idr["NOME_ARQUIVO"].ToString();
                CargaAguarLibPgto.CLIENTE = idr["CLIENTE"].ToString();
                CargaAguarLibPgto.NUM_CARGA = idr["NUM_CARGA"].ToString();
                CargaAguarLibPgto.ORIGEM = idr["ORIGEM"].ToString();
                CargaAguarLibPgto.VALOR_ARQUIVO = "R$ " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", idr["VALOR_ARQUIVO"].ToString());
                CargaAguarLibPgto.ETAPA = idr["ETAPA"].ToString();
                CargaAguarLibPgto.NIVEL = idr["NIVEL"].ToString();
                CargaAguarLibPgto.ERRO = idr["ERRO"].ToString();
                CargaAguarLibPgto.MENSAGEM = idr["MENSAGEM"].ToString();
                CargaAguarLibPgto.BAIXOU_LOG = idr["BAIXOU_LOG"].ToString();
                CargaAguarLibPgto.ID_PROCESSO = idr["ID_PROCESSO"].ToString();
                
                colecao.Add(CargaAguarLibPgto);
            }
            idr.Close();
            return colecao;
        }
        public List<CARGA_AGUAR_LIB_PGTO> ListaCargaAguardandoPagamento()
        {
            var colecao = new List<CARGA_AGUAR_LIB_PGTO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" EXEC PROC_CONSCARGAPGTOANTECIPADO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var CargaAguarLibPgto = new CARGA_AGUAR_LIB_PGTO();
                CargaAguarLibPgto.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                CargaAguarLibPgto.NOMCLIENTE = idr["NOMCLI"].ToString();
                CargaAguarLibPgto.NUMCARGA = Convert.ToInt32(idr["NUMCARG_VA"]);
                DateTime dataAux;
                DateTime.TryParse(idr["DTAUTORIZ"].ToString(), out dataAux);
                CargaAguarLibPgto.DATAUTORDT = dataAux; //
               
                if (!string.IsNullOrEmpty(idr["QUANT"].ToString()))
                    CargaAguarLibPgto.QTDUSU = Convert.ToInt16(idr["QUANT"]);
                if (!string.IsNullOrEmpty(idr["VALOR"].ToString()))
                    CargaAguarLibPgto.VALOR = Convert.ToDecimal(idr["VALOR"]);

                if (!string.IsNullOrEmpty(idr["VAL2AVIA"].ToString()))
                    CargaAguarLibPgto.VAL2AVIA = Convert.ToDecimal(idr["VAL2AVIA"]);

                if (!string.IsNullOrEmpty(idr["VALTAXSER"].ToString()))
                    CargaAguarLibPgto.VALTAXSER = Convert.ToDecimal(idr["VALTAXSER"]);

                if (!string.IsNullOrEmpty(idr["TOTAL"].ToString()))
                    CargaAguarLibPgto.TOTAL = Convert.ToDecimal(idr["TOTAL"]);

                if (!string.IsNullOrEmpty(idr["SALDOCONTAUTIL"].ToString()))
                    CargaAguarLibPgto.SALDOCONTAUTIL = Convert.ToDecimal(idr["SALDOCONTAUTIL"]);
                colecao.Add(CargaAguarLibPgto);
            }
            idr.Close();
            return colecao;
        }

        public string CancelaCargaCliente(CLIENTE_CARGA clienteCarga)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var mensagem = string.Empty;
            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("SP_CANCELA_CARGA_SOLICITADA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = clienteCarga.CODCLI;
                cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = clienteCarga.NUMCARGA;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC; ;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.ToString(reader["RETORNO"]) == "OK")
                    {
                        mensagem = "Carga N. " + clienteCarga.NUMCARGA + " cancelada com sucesso.";
                    }
                    else
                        mensagem = "Erro ao cancelar a carga N. " + clienteCarga.NUMCARGA;
                }
                if (mensagem == string.Empty)
                    mensagem = "Erro ao cancelar a carga N. " + clienteCarga.NUMCARGA;

                return mensagem;                
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        private static bool CancelaCargaC(CLIENTE_CARGA Cliente_Carga, SqlConnection conn, SqlTransaction trans)
        {
            var sql = "UPDATE CARGAC set DTCARGA = 0, VALOR = 0, TAXSER = 0, VAL2AVIA = 0, PRAPAG = 0, CODOPE = 0, TAXADESAO = 0 " +
                         "WHERE CODCLI = " + Cliente_Carga.CODCLI + " AND " + "NUMCARG_VA = " + Cliente_Carga.NUMCARGA + 
                         " AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + Convert.ToDateTime(Cliente_Carga.DATAUTORDT).ToString("yyyyMMdd") + "'";
            var cmd = new SqlCommand(sql, conn, trans) { CommandType = CommandType.Text };
            var linhaAfetada = cmd.ExecuteNonQuery();
            return linhaAfetada == 1;
        }        

        public void CancelaProgramacaoCarga(CLIENTE_CARGA Cliente_Carga)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };

            DateTime dataProg;
            DateTime.TryParse(Cliente_Carga.DATPROG, out dataProg);

            var sql = new StringBuilder();
            sql.AppendLine(" UPDATE CARGAC ");
            sql.AppendLine(" SET DTPROG = NULL ");
            sql.AppendLine(" WHERE CODCLI = " + Cliente_Carga.CODCLI);
            sql.AppendLine(" AND NUMCARG_VA = " + Cliente_Carga.NUMCARGA);
            sql.AppendLine(" AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + Convert.ToDateTime(Cliente_Carga.DATAUTORDT).ToString("yyyyMMdd") + "'");
            sql.AppendLine(" AND DTPROG = '" + dataProg.ToString("yyyyMMdd") + "'");
            sql.AppendLine(" AND DTCARGA IS NULL");

            var cmd = new SqlCommand(sql.ToString(), conn) { CommandType = CommandType.Text };
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Erro ao cancelar programacao");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void GeraProgramacao(CLIENTE_CARGA Cliente_Carga)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            DateTime dataProg;
            DateTime.TryParse(Cliente_Carga.DATPROG, out dataProg);
            var data = (dataProg.Date > DateTime.Now.Date) ? "'" + dataProg.ToString("yyyyMMdd") + "'" : "NULL";
            var sql = new StringBuilder();
            sql.AppendLine(" UPDATE CARGAC ");
            sql.AppendLine(" SET DTPROG = " + data);
            sql.AppendLine(" WHERE CODCLI = " + Cliente_Carga.CODCLI);
            sql.AppendLine(" AND NUMCARG_VA = " + Cliente_Carga.NUMCARGA);
            sql.AppendLine(" AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + Convert.ToDateTime(Cliente_Carga.DATAUTORDT).ToString("yyyyMMdd") + "'");
            var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar programacao");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void EfetuaCarga(int codCli, int numCarga, LOG log, int idFunc)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var cmd = new SqlCommand("SP_LIB_EFETUA_CARGA", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 18000;
            cmd.Connection = conn;
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = numCarga;
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
            cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = idFunc;
            try
            {
                conn.Open();
                cmd.ExecuteReader();
                log.AddLog(new LOG("", "Carga gerada com sucesso", "A carga para o cliente " + codCli + " foi gerada com sucesso"));
            }
            catch
            {
                log.AddLog(new LOG("", "Carga gerada com sucesso", "A carga para o cliente " + codCli + " sera efetuada na sequencia a carga anterior. "));
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }            
        }

        public void RetornaLog(int codCli, int numCarga, LOG log)
        {
            IDataReader dr = null;
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var cmd = new SqlCommand("SELECT CPF, MENSAGEM, VALOR FROM CARGA_EFETUADA_LOG WITH (NOLOCK) WHERE CODCLI = @CODCLI AND NUMCARG_VA = @NUMCARG_VA ", conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 18000;
            cmd.Connection = conn;
            cmd.Parameters.Clear();

            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
            cmd.Parameters.Add("@NUMCARG_VA", SqlDbType.Int).Value = numCarga;            
            
            conn.Open();
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var linha = "CPF: " + Convert.ToString(dr["CPF"]) + " " +
                            "VALOR CARGA: " + Convert.ToString(dr["VALOR"]) + " " +
                            "MENSAGEM: " + Convert.ToString(dr["MENSAGEM"]);

                log.AddLog(new LOG("", linha, "CLIENTE: " + codCli + " NUMERO CARGA: " + Convert.ToString(numCarga))); 
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        public void LiberaCargaCliente(int codCLi, int numCarga, DateTime dtAutoriz, int idFunc)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();
            sql.AppendLine(" UPDATE CARGAC ");
            sql.AppendLine(" SET IDLIBCARGOPR =  " + idFunc);
            sql.AppendLine(" WHERE CODCLI = " + codCLi);
            sql.AppendLine(" AND NUMCARG_VA = " + numCarga);
            sql.AppendLine(" AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + dtAutoriz.ToString("yyyyMMdd") + "'");
            sql.AppendLine(" AND DTCARGA IS NULL");
            var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Erro ao liberar a carga");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void LiberaCargaVencida(int codCLi, int numCarga, DateTime dtAutoriz, int idFunc)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();
            sql.AppendLine(" UPDATE CARGAC SET LIBVENC = 'S' ");
            sql.AppendLine(" WHERE CODCLI = " + codCLi);
            sql.AppendLine(" AND NUMCARG_VA = " + numCarga);
            sql.AppendLine(" AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + dtAutoriz.ToString("yyyyMMdd") + "'");
            sql.AppendLine(" AND DTCARGA IS NULL");
            var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Erro ao liberar a carga vencida");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void LiberaDataProg(int codCLi, int numCarga, DateTime dtAutoriz)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();
            sql.AppendLine(" UPDATE CARGAC ");
            sql.AppendLine(" SET DTPROG = NULL ");
            sql.AppendLine(" WHERE CODCLI = " + codCLi);
            sql.AppendLine(" AND NUMCARG_VA = " + numCarga);
            sql.AppendLine(" AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + dtAutoriz.ToString("yyyyMMdd") + "'");
            sql.AppendLine(" AND DTPROG IS NOT NULL AND DTPROG = '" + DateTime.Now.ToString("yyyyMMdd") + "'");
            var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Erro ao liberar a carga");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public void BloqueiaCargaCliente(int codCLi, int numCarga, DateTime dtAutoriz)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();
            sql.AppendLine(" UPDATE CARGAC ");
            sql.AppendLine(" SET BLOQ = 'S'");
            sql.AppendLine(" WHERE CODCLI = " + codCLi);
            sql.AppendLine(" AND NUMCARG_VA = " + numCarga);
            sql.AppendLine(" AND convert(datetime,round(convert(float,DTAUTORIZ),0,1)) = '" + dtAutoriz.ToString("yyyyMMdd") + "'");
            sql.AppendLine(" AND DTCARGA IS NULL");
            var cmd = new SqlCommand(sql.ToString(), conn);
            cmd.CommandType = CommandType.Text;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("Erro ao liberar a carga");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public bool VerificaValCarga(int codCli, int numCarga)
        {
            var retorno = false;
            IDataReader dr = null;
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd = new SqlCommand("SP_VERIFICA_VAL_CARGA", conn)
                {
                CommandType = CommandType.StoredProcedure,
                Connection = conn
                };
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
            cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = numCarga;
            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    retorno = dr["RETORNO"].ToString() == "OK";
                }
                dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();

                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
            return retorno;
        }

        public string GetPastaArquivosCarga()
        {
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand("SELECT VAL FROM CONFIG_JOBS WITH (NOLOCK) WHERE ID0 = 'DIR_ARQ_CARGA'");
            return Convert.ToString(db.ExecuteScalar(cmd));
        }
    }   
}

