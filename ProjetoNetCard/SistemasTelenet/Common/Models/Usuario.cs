using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.PO;
using System.Linq;

namespace NetCard.Common.Models
{
    public class Usuario
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string CodigoCartao { get; set; }
        public string CodigoCartaoMascarado { get; set; }
        public string NumeroDependentes { get; set; }
        public string CodigoParentesco { get; set; }
        public string DataInclusao { get; set; }
        public string DataAtivacao { get; set; }
        public string DataStatus { get; set; }
        public string DescricaoStatus { get; set; }
        public string CodigoStatus { get; set; }
        public string NomeCliente { get; set; }
        public string CodigoAfiliacao { get; set; }
        public string Trilha2 { get; set; }
        public string MatriculaUsuario { get; set; }
        public string NumeroParcelas { get; set; }
        public string CodigoSetor { get; set; }
        public string CargaPadrao { get; set; }
        public string Valades { get; set; }
        public string DataGeracaoCartao { get; set; }
        public string DataValidadeCartao { get; set; }
        public string Celular { get; set; }
        public string DataDes { get; set; }
        public string NomeCartao { get; set; }


        public TransfSaldoUsuario BuscarUsuario(ObjConn objConn, string cpf, string codigoCliente)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT ");
            sql.AppendLine("U.ID, ");
            sql.AppendLine("U.CPF, ");
            sql.AppendLine("U.NOMUSU, ");
            sql.AppendLine("U.CODCRT, ");
            sql.AppendLine("S.DESTA, ");
            sql.AppendLine("A.SALDOVA ");
            sql.AppendLine("FROM USUARIOVA U WITH (NOLOCK) ");
            sql.AppendLine("JOIN CLIENTEVA C WITH (NOLOCK) ON C.CODCLI = U.CODCLI ");
            sql.AppendLine("JOIN STATUS S WITH (NOLOCK) ON S.STA = U.STA ");
            sql.AppendLine("JOIN " + objConn.BancoAutorizador + "..ctcartva A WITH (NOLOCK) ON U.CODCRT = A.CODCARTAO");

            IFilter filter = new Filter("U.CPF", SqlOperators.Equal, cpf);
            var and = new ANDFilter(filter, new Filter("u.codcli", SqlOperators.Equal, codigoCliente));

            string _filtro = string.Empty;
            if (and != null)
                _filtro = and.FilterString;

            if (!string.IsNullOrEmpty(_filtro))
                sql.AppendLine(string.Format("WHERE {0} ", _filtro));
            sql.AppendLine("ORDER BY U.NOMUSU");

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            dr = db.ExecuteReader(cmd);

            if (dr.Read())
            {
                var retorno = new TransfSaldoUsuario()
                {
                    CodigoUsuario = dr["ID"].ToString(),
                    NomeUsuario = dr["NOMUSU"].ToString(),
                    NumeroCartao = dr["CODCRT"].ToString(),
                    StatusCartao = dr["DESTA"].ToString().ToUpper(),
                    SaldoCartao = dr["SALDOVA"].ToString(),
                    Existe = true

                };

                dr.Close();
                return retorno;

            }
            else
            {
                dr.Close();
                return new TransfSaldoUsuario()
                {
                    CodigoUsuario = null,
                    NomeUsuario = null,
                    NumeroCartao = null,
                    StatusCartao = null,
                    SaldoCartao = null,
                    Existe = false

                };

            }

        }

        public TransfSaldoUsuario BuscarUsuarioOrigem(ObjConn objConn, DadosAcesso dadosAcesso, string cpfCartao)
        {
            StringBuilder sql = new StringBuilder();
            var tabelaUsuario = dadosAcesso.Sistema.cartaoPJVA == 0 ? "USUARIO" : "USUARIOVA";
            var tabelaCliente = dadosAcesso.Sistema.cartaoPJVA == 0 ? "VCLIENTE" : "VCLIENTEVA";
            var tabelaCTCARTAO = dadosAcesso.Sistema.cartaoPJVA == 0 ? "CTCARTAO" : "CTCARTVA";
            sql.AppendLine("SELECT ");
            if (dadosAcesso.Sistema.cartaoPJVA == 1) sql.AppendLine("U.ID, ");
            sql.AppendLine("U.CPF, ");
            sql.AppendLine("U.NOMUSU, ");
            sql.AppendLine("U.CODCRT, ");
            sql.AppendLine("S.DESTA ");
            if (dadosAcesso.Sistema.cartaoPJVA == 1) sql.AppendLine(", A.SALDOVA ");
            sql.AppendLine("FROM " + tabelaUsuario + " U WITH (NOLOCK) ");
            sql.AppendLine("JOIN " + tabelaCliente + " C WITH (NOLOCK) ON C.CODCLI = U.CODCLI ");
            sql.AppendLine("JOIN STATUS S WITH (NOLOCK) ON S.STA = U.STA ");
            sql.AppendLine("JOIN " + objConn.BancoAutorizador + ".." + tabelaCTCARTAO + "  A WITH (NOLOCK) ON U.CODCRT = A.CODCARTAO");
            IFilter filter = null;
            if (cpfCartao.Length == 11)
                filter = new Filter("U.CPF", SqlOperators.Equal, cpfCartao);
            if (cpfCartao.Length == 17)
                filter = new Filter("U.CODCRT", SqlOperators.Equal, cpfCartao);

            var and = new ANDFilter(filter, new Filter("U.CODCLI", SqlOperators.Equal, Convert.ToString(dadosAcesso.Codigo)));

            string _filtro = string.Empty;
            if (and != null)
                _filtro = and.FilterString;

            if (!string.IsNullOrEmpty(_filtro))
                sql.AppendLine(string.Format("WHERE {0} ", _filtro));
            sql.AppendLine("ORDER BY U.NOMUSU");

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            dr = db.ExecuteReader(cmd);

            if (dr.Read())
            {
                var retorno = new TransfSaldoUsuario();                
                if (dadosAcesso.Sistema.cartaoPJVA == 1) retorno.CodigoUsuario = dr["ID"].ToString();
                retorno.Cpf = dr["CPF"].ToString();
                retorno.NomeUsuario = dr["NOMUSU"].ToString();
                retorno.NumeroCartao = dr["CODCRT"].ToString();
                retorno.StatusCartao = dr["DESTA"].ToString().ToUpper();
                if (dadosAcesso.Sistema.cartaoPJVA == 1) retorno.SaldoCartao = dr["SALDOVA"].ToString();
                retorno.Existe = true;

                dr.Close();
                return retorno;

            }
            else
            {
                dr.Close();
                return new TransfSaldoUsuario()
                {
                    CodigoUsuario = null,
                    NomeUsuario = null,
                    NumeroCartao = null,
                    StatusCartao = null,
                    SaldoCartao = null,
                    Existe = false

                };

            }

        }

        public TransfSaldoUsuario BuscarUsuarioDestinoTroca(ObjConn objConn, DadosAcesso dadosAcesso, string cpfCartao)
        {
            StringBuilder sql = new StringBuilder();
            var tabelaUsuario = dadosAcesso.Sistema.cartaoPJVA == 0 ? "USUARIO" : "USUARIOVA";
            sql.AppendLine("SELECT CPF, CODCRT, NOMUSU, STA, CODFIL, CODSET, MAT ");
            sql.AppendLine("FROM " + tabelaUsuario + " U WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCLI = " + dadosAcesso.Codigo + " AND CPF = '" + cpfCartao + "'");
            
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            dr = db.ExecuteReader(cmd);

            if (dr.Read())
            {
                var retorno = new TransfSaldoUsuario();
                retorno.Cpf = Convert.ToString(dr["CPF"]);
                retorno.NumeroCartao = Convert.ToString(dr["CODCRT"]);
                retorno.NomeUsuario = Convert.ToString(dr["NOMUSU"]);
                retorno.StatusCartao = Convert.ToString(dr["STA"]);
                retorno.Filial = Convert.ToString(dr["CODFIL"]);
                retorno.Setor = Convert.ToString(dr["CODSET"]).ToUpper();
                retorno.Matricula = Convert.ToString(dr["MAT"]).ToUpper();
                retorno.Existe = true;

                dr.Close();
                return retorno;
            }
            else
            {
                dr.Close();
                return new TransfSaldoUsuario()
                {                    
                    Existe = false
                };
            }
        }

        public SubstituirCartaoUsuario BuscarCartaoDestino(ObjConn objConn, string codCrt, string codCli)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT U.ID, U.CPF, U.NOMUSU, U.CODCRT, S.DESTA, A.SALDOVA ");
            sql.AppendLine("FROM USUARIOVA U WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN CLIENTE_PRE C WITH (NOLOCK) ON C.CODCLI = U.CODCLI ");
            sql.AppendLine("INNER JOIN STATUS S WITH (NOLOCK) ON S.STA = U.STA ");
            sql.AppendLine("INNER JOIN " + objConn.BancoAutorizador + "..CTCARTVA A WITH (NOLOCK) ON U.CODCRT = A.CODCARTAO");            
            sql.AppendLine("WHERE U.CODCRT = '" + codCrt + "' AND ");
            sql.AppendLine("U.CODCLI = " + codCli);
            sql.AppendLine("ORDER BY U.NOMUSU");

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            dr = db.ExecuteReader(cmd);

            if (dr.Read())
            {
                var retorno = new SubstituirCartaoUsuario()
                {
                    CodigoUsuario = dr["ID"].ToString(),
                    NomeUsuario = dr["NOMUSU"].ToString(),
                    NumeroCartao = dr["CODCRT"].ToString(),
                    StatusCartao = dr["DESTA"].ToString().ToUpper(),
                    SaldoCartao = dr["SALDOVA"].ToString(),
                    Cpf = dr["CPF"].ToString(),
                    Existe = true
                };

                dr.Close();
                return retorno;

            }
            else
            {
                dr.Close();
                return new SubstituirCartaoUsuario()
                {
                    CodigoUsuario = null,
                    NomeUsuario = null,
                    NumeroCartao = null,
                    StatusCartao = null,
                    SaldoCartao = null,
                    Existe = false

                };
            }
        }

        public bool ExisteTransacoesCompras(ObjConn objConn, string cpf, int codCli)
        {
            StringBuilder sql = new StringBuilder();
            bool retorno;
            sql.AppendLine("SELECT COUNT(1) AS COMPRAS FROM TRANSACVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CPF = '" + cpf + "' AND CODCLI = " + codCli + " AND");
            sql.AppendLine("(TIPTRA < 80000 OR TIPTRA = 999010) AND CODRTA = 'V'");
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            retorno = Convert.ToInt32(db.ExecuteScalar(cmd)) > 0;
            return retorno;
        }

        public bool GerouCartao(ObjConn objConn, string codCrt, int codCli)
        {
            StringBuilder sql = new StringBuilder();
            bool retorno;
            sql.AppendLine("SELECT COUNT(1) AS GEROUCRT FROM USUARIOVA WITH (NOLOCK) ");
            sql.AppendLine("WHERE CODCRT = '" + codCrt + "' AND CODCLI = " + codCli + " AND");
            sql.AppendLine("DATGERCRT IS NOT NULL ");
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            retorno = Convert.ToInt32(db.ExecuteScalar(cmd)) > 0;
            return retorno;
        }

        public void CarregarUsuario(ObjConn objConn, string cpf, string codigoCliente)
        {
            List<BENEFICIO_USUARIO> retorno = new List<BENEFICIO_USUARIO>();

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            string sql = @" SELECT NOMUSU, CODCRT FROM USUARIO WITH (NOLOCK) WHERE CPF = '" + Cpf + "' AND CODCLI = " + codigoCliente;

            SqlCommand comando = new SqlCommand(sql, conexao);

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    Nome = dt.Rows[0]["NOMUSU"].ToString();
                    CodigoCartaoMascarado = Utils.MascaraCartao(dt.Rows[0]["CODCRT"].ToString(), 17);
                }

                

            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                throw;
            }
        }

        public TransfSaldo ProcessarTransferenciaDeSaldo(ObjConn objConn, int paramTipoTransferencia, int paramCodigoCliente, string paramCpfOrigem, string paramCpfDestino,
            string paramCartaoOrigem, string paramCartaoDestino, decimal paramValor, int paramIdfunc, string paramBancoAutorizador, string paramLogin, string paramCodigoOperadora)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));
            SqlConnection conexaoLog = new SqlConnection(Utils.GetConnectionStringNerCard(objConn));

            try
            {

                string nomeProcedureTransferencia = "PROC_TRANSF_SALDO";
                string nomeProcedureLog = "PROC_INSERE_LOGVAWEB";


                SqlCommand comandoTransferencia = new SqlCommand(nomeProcedureTransferencia, conexao);
                comandoTransferencia.CommandType = CommandType.StoredProcedure;

                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@TIPO", Value = paramTipoTransferencia, DbType = DbType.Int32 });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@CODCLI", Value = paramCodigoCliente, DbType = DbType.Int32 });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@CPF_ORIG", Value = paramCpfOrigem, DbType = DbType.String });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@CPF_DEST", Value = paramCpfDestino, DbType = DbType.String });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@CRT_ORIG", Value = paramCartaoOrigem, DbType = DbType.String });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@CRT_DEST", Value = paramCartaoDestino, DbType = DbType.String });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@VALOR", Value = paramValor, DbType = DbType.Decimal });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@BANCO", Value = paramBancoAutorizador, DbType = DbType.String });
                comandoTransferencia.Parameters.Add(new SqlParameter() { ParameterName = "@IDOPEMW", Value = paramIdfunc, DbType = DbType.Int32 });

                conexao.Open();
                comandoTransferencia.ExecuteNonQuery();
                conexao.Close();

                SqlCommand comandoLog = new SqlCommand(nomeProcedureLog, conexaoLog);
                comandoLog.CommandType = CommandType.StoredProcedure;


                DateTime now = DateTime.Now;
                string data = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@DATA", Value = now, DbType = DbType.DateTime });
                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@CODCLI", Value = paramCodigoCliente, DbType = DbType.Int32 });
                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@LOGIN", Value = paramLogin, DbType = DbType.String });
                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@CODOPE", Value = paramCodigoOperadora, DbType = DbType.Int32 });
                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@OPERACAO", Value = "Transferência de Saldo", DbType = DbType.String });
                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@CPF", Value = paramCpfOrigem, DbType = DbType.String });
                comandoLog.Parameters.Add(new SqlParameter() { ParameterName = "@CODCRT", Value = paramCartaoOrigem, DbType = DbType.String });

                conexaoLog.Open();
                comandoLog.ExecuteReader();
                conexaoLog.Close();

                return new TransfSaldo()
                           {
                               Mensagem = "Transferência realizada com sucesso.",
                               Sucesso = true
                           };
            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                //if(dr != null)
                //    dr.Close();

                return new TransfSaldo()
                           {
                               Mensagem = "Ocorreu um erro ao realizar a transferência.",
                               Sucesso = false
                           };
                throw;
            }
        }

        public SubstituirUsuario ProcessarSubstituicao(ObjConn objConn, DadosAcesso dadosAcesso, string codCrtOri, string codCrtDes)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "PROC_SUBSTITUI_CRT_CPF_TEMPORARIO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCRT_ANT", DbType.String, codCrtOri);
            db.AddInParameter(cmd, "CODCRT_NOVO", DbType.String, codCrtDes);
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, dadosAcesso.Id);

            db.AddOutParameter(cmd, "RETORNO", DbType.String, 128);
            db.AddOutParameter(cmd, "MENSAGEM", DbType.String, 128);
            db.AddOutParameter(cmd, "CODRET", DbType.Int16, 128);

            var retorno = new SubstituirUsuario();
            retorno.Sucesso = false;

            try
            {                
                db.ExecuteNonQuery(cmd);                
                var mensagem = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MENSAGEM"));
                var codRet = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CODRET"));
                if (codRet == "0")
                {
                    retorno.Mensagem = "Troca de Cpf realizada com sucesso.";
                    retorno.Sucesso = true;
                }
            }
            catch (Exception)
            {
                retorno.Mensagem = "Ocorreu um erro durante a operação";
            }
            return retorno;
        }

        public SubstituirUsuario ProcessarAlteracao(ObjConn objConn, DadosAcesso dadosAcesso, 
            string cpfTemp, string novoCpf, string nomeUsu, string filial, string setor, string matricula, string dataNascimento)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var retorno = new SubstituirUsuario { Sucesso = false };

            const string sql = "PROC_ALTERA_CPF_TEMPORARIO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "CPF_TEMPORARIO", DbType.String, cpfTemp);
            db.AddInParameter(cmd, "CPF", DbType.String, novoCpf);
            db.AddInParameter(cmd, "NOMUSU", DbType.String, nomeUsu);
            if (!string.IsNullOrEmpty(filial)) db.AddInParameter(cmd, "FILIAL", DbType.Int16, Convert.ToInt32(filial));
            if (!string.IsNullOrEmpty(setor)) db.AddInParameter(cmd, "SETOR", DbType.String, setor);
            if (!string.IsNullOrEmpty(matricula)) db.AddInParameter(cmd, "MATRICULA", DbType.String, matricula);
            if (!string.IsNullOrEmpty(dataNascimento)) db.AddInParameter(cmd, "DATNAS", DbType.String, Convert.ToDateTime(dataNascimento).ToString("yyyyMMdd"));
            db.AddInParameter(cmd, "IDOPEMW", DbType.Int32, dadosAcesso.Id);

            db.AddOutParameter(cmd, "ERRO", DbType.Int32, 128);
            db.AddOutParameter(cmd, "MSG_ERRO", DbType.String, 128);
            
            try
            {
                db.ExecuteNonQuery(cmd);
                var erro = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@ERRO"));
                var msg = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MSG_ERRO"));

                if (erro == "0")
                {
                    retorno.Mensagem = "Troca de Cpf realizada com sucesso.";
                    retorno.Sucesso = true;
                }
                else
                {
                    retorno.Mensagem = msg;
                }                
            }
            catch (Exception e)
            {                
                retorno.Mensagem = "Ocorreu um erro durante a operação";
            }
            return retorno;
        }

        public InclusaoTemp ProcessarInclusaoCpfTemp(ObjConn objConn, DadosAcesso dadosAcesso, string nomePadrao, decimal limitePadrao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var retorno = new InclusaoTemp() { Sucesso = false };

            const string sql = "PROC_INSERE_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "CPF", DbType.String, "FICTICIO");
            db.AddInParameter(cmd, "NOMUSU", DbType.String, nomePadrao);
            db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
            db.AddInParameter(cmd, "CODFIL", DbType.Int16, 0);
            db.AddInParameter(cmd, "DATINC", DbType.String, DateTime.Now.ToString("yyyyMMdd"));
            db.AddInParameter(cmd, "CODSET", DbType.String, "0");
            db.AddInParameter(cmd, "MAT", DbType.String, "0");
            db.AddInParameter(cmd, "DATNAS", DbType.String, "19900101");
            db.AddInParameter(cmd, "SEXO", DbType.String, "M");
            db.AddInParameter(cmd, "LIMPAD", DbType.Decimal, limitePadrao);
            db.AddInParameter(cmd, "CODOPE", DbType.Int16, 0);
            db.AddInParameter(cmd, "IDOPEWEB", DbType.Int32, dadosAcesso.Id);

            db.AddOutParameter(cmd, "CPFRET", DbType.String, 128);
            db.AddOutParameter(cmd, "NOMRET", DbType.String, 128);
            db.AddOutParameter(cmd, "CODCRTRET", DbType.String, 128);

            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    if (dr["RETORNO"].ToString() == Constantes.ok)
                    {                        
                        retorno.Mensagem = "Cartão temporário incluído";
                        retorno.Sucesso = true;
                    }
                    else
                    {
                        retorno.Mensagem = dr["MENSAGEM"].ToString();
                        retorno.Sucesso = false;
                    }                    
                }
                dr.Close();

                if (retorno.Sucesso)
                {
                    retorno.CpfTemp = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CPFRET"));
                    retorno.NomePadrao = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@NOMRET"));
                    retorno.NumCrtTemp = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CODCRTRET"));
                }
            }
            catch (Exception ex)
            {
                retorno.Mensagem = "Ocorreu um erro durante a operação";
                retorno.Sucesso = false;
                if (dr != null)
                    dr.Close();
            }
            return retorno;
        }

        public List<BENEFICIO_USUARIO> GetBeneficiosUsuario(ObjConn objConexao, string codigoCliente)
        {
            List<BENEFICIO_USUARIO> retorno = new List<BENEFICIO_USUARIO>();

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = @" 
                            SELECT A.CODCLI, A.CODBENEF, B.NOMBENEF, C.COMPULSORIO, C.VALTIT, CONVERT(VARCHAR, A.DTASSOC, 103) DTASSOC
                            FROM BENEFUSU A WITH (NOLOCK) 
                            INNER JOIN BENEFICIO B WITH (NOLOCK) ON A.CODBENEF = B.CODBENEF
                            INNER JOIN BENEFCLI C WITH (NOLOCK) ON A.CODBENEF = C.CODBENEF
                            WHERE C.CODCLI = " + codigoCliente + " AND A.CPF = '" + Cpf + "'";

            SqlCommand comando = new SqlCommand(sql, conexao);


            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        BENEFICIO_USUARIO benef = new BENEFICIO_USUARIO();

                        benef.CODBENEF = Convert.ToInt32(item["CODBENEF"].ToString());
                        benef.CODCLI = Convert.ToInt32(item["CODCLI"].ToString());
                        benef.NOMBENEF = item["NOMBENEF"].ToString();
                        benef.COMPULSORIO = item["COMPULSORIO"].ToString();
                        benef.VALTIT = !String.IsNullOrEmpty(item["VALTIT"].ToString()) ? Convert.ToDecimal(item["VALTIT"].ToString()) : 0;
                        benef.DTASSOC = item["DTASSOC"].ToString();

                        retorno.Add(benef);
                    }
                }

                return retorno;

            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                throw;
            }
        }

        public List<BENEFICIO_USUARIO_CLIENTE> GetBeneficiosUsuarioCliente(ObjConn objConexao, string codigoCliente, string cpf)
        {

            List<BENEFICIO_USUARIO_CLIENTE> retorno = new List<BENEFICIO_USUARIO_CLIENTE>();

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = @"PROC_LISTA_BENEFICIO";

            SqlCommand comando = new SqlCommand(sql, conexao);
            comando.CommandType = CommandType.StoredProcedure;

            comando.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(codigoCliente);
            comando.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpf;

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        BENEFICIO_USUARIO_CLIENTE benef = new BENEFICIO_USUARIO_CLIENTE();

                        benef.CODBENEF = Convert.ToInt32(item["CODBENEF"].ToString());
                        benef.CODCLI = Convert.ToInt32(codigoCliente);
                        benef.NOMBENEF = item["NOMBENEF"].ToString();
                        benef.COMPULSORIO = item["COMPULSORIO"].ToString();
                        benef.VALTIT = !String.IsNullOrEmpty(item["VALTIT"].ToString()) ? Convert.ToDecimal(item["VALTIT"].ToString()) : 0;
                        benef.DTASSOC = string.IsNullOrEmpty(item["DTASSOC"].ToString()) ? "" : Convert.ToDateTime(item["DTASSOC"].ToString()).ToShortDateString();
                        benef.JAASSOC = item["JAASSOC"].ToString();

                        retorno.Add(benef);
                    }
                }

                return retorno;

            }

            catch (Exception)
            {
                throw;
            }
        }

        public void AssociarBeneficioUsuario(ObjConn objConexao, string codigoCliente, string codigoBeneficio, out string retorno, out string mensagemRetorno)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = "PROC_ASSOCIA_BENEFUSU";

            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.Add((new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" }));
            comando.Parameters.Add((new SqlParameter() { Value = codigoCliente, DbType = DbType.Int32, ParameterName = "@CODCLI" }));
            comando.Parameters.Add((new SqlParameter() { Value = Cpf, DbType = DbType.String, ParameterName = "@CPF" }));
            comando.Parameters.Add((new SqlParameter() { Value = codigoBeneficio, DbType = DbType.Int32, ParameterName = "@CODBENEF" }));

            

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                retorno = dt.Rows[0]["RETORNO"].ToString();
                mensagemRetorno = dt.Rows[0]["MENSAGEM"].ToString();

            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                throw;

            }
        }

        public bool AssociarVariosBeneficioUsuario(ObjConn objConexao, string codigoCliente, string codigoBeneficio)
        {
            string[] codigosBeneficioStrings = codigoBeneficio.Split(',');

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = "PROC_ASSOCIA_BENEFUSU";

            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.Add((new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" }));
            comando.Parameters.Add((new SqlParameter() { Value = codigoCliente, DbType = DbType.Int32, ParameterName = "@CODCLI" }));
            comando.Parameters.Add((new SqlParameter() { Value = Cpf, DbType = DbType.String, ParameterName = "@CPF" }));
            comando.Parameters.Add((new SqlParameter() { Value = codigoBeneficio, DbType = DbType.Int32, ParameterName = "@CODBENEF" }));

            try
            {
                foreach (var x in codigosBeneficioStrings)
                {
                    comando = new SqlCommand(sql, conexao) { CommandType = CommandType.StoredProcedure };
                    comando.Parameters.Clear();

                    comando.Parameters.Add((new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" }));
                    comando.Parameters.Add((new SqlParameter() { Value = codigoCliente, DbType = DbType.Int32, ParameterName = "@CODCLI" }));
                    comando.Parameters.Add((new SqlParameter() { Value = Cpf, DbType = DbType.String, ParameterName = "@CPF" }));
                    comando.Parameters.Add((new SqlParameter() { Value = x, DbType = DbType.Int32, ParameterName = "@CODBENEF" }));

                    conexao.Open();

                    DataTable dt = new DataTable();

                    dt.Load(comando.ExecuteReader());

                    conexao.Close();
                }

                return true;

            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                throw;

            }
        }

        public bool DesassociarBeneficioUsuario(ObjConn objConexao, string codigoCliente, string codigoBeneficio)
        {

            string[] codigosBeneficioStrings = codigoBeneficio.Split(',');

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = "PROC_DESASSOCIA_BENEFUSU";

            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.Add((new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" }));
            comando.Parameters.Add((new SqlParameter() { Value = codigoCliente, DbType = DbType.Int32, ParameterName = "@CODCLI" }));
            comando.Parameters.Add((new SqlParameter() { Value = Cpf, DbType = DbType.String, ParameterName = "@CPF" }));
            comando.Parameters.Add((new SqlParameter() { Value = codigoBeneficio, DbType = DbType.Int32, ParameterName = "@CODBENEF" }));

            try
            {
                foreach (var x in codigosBeneficioStrings)
                {
                    comando = new SqlCommand(sql, conexao) { CommandType = CommandType.StoredProcedure };
                    comando.Parameters.Clear();

                    comando.Parameters.Add((new SqlParameter() { Value = 0, DbType = DbType.Int32, ParameterName = "@SISTEMA" }));
                    comando.Parameters.Add((new SqlParameter() { Value = codigoCliente, DbType = DbType.Int32, ParameterName = "@CODCLI" }));
                    comando.Parameters.Add((new SqlParameter() { Value = Cpf, DbType = DbType.String, ParameterName = "@CPF" }));
                    comando.Parameters.Add((new SqlParameter() { Value = x, DbType = DbType.Int32, ParameterName = "@CODBENEF" }));

                    conexao.Open();

                    DataTable dt = new DataTable();

                    dt.Load(comando.ExecuteReader());

                    conexao.Close();
                }

                return true;

            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                throw;

            }
        }

        public bool VerificarAssociacaoBeneficio(ObjConn objConexao, string codigoCliente, string codigoBeneficio)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = "SELECT COUNT(*) NUM_BENEFICIOS FROM BENEFUSU WITH (NOLOCK) WHERE CPF = '" + Cpf + "' AND CODCLI = " + codigoCliente + " AND CODBENEF = " + codigoBeneficio;

            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.Text };

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                return Convert.ToInt32(dt.Rows[0]["NUM_BENEFICIOS"].ToString()) > 0;
            }
            catch
            {
                if (conexao.State != ConnectionState.Closed)
                {
                    conexao.Close();
                }

                throw;

            }
        }

        public bool ValidarCpfTemporarioOrigem(ObjConn objConn, DadosAcesso dadosAcesso, string cpfTemp, out string msg)
        {
            msg = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "CARGA_VALIDA_CPF_TEMPORARIO_PARA_TROCA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "CPF_TEMPORARIO", DbType.String, cpfTemp);
            db.AddOutParameter(cmd, "CPF_TEMP_VALIDO_PARA_TROCA", DbType.String, 128);
            db.AddOutParameter(cmd, "ERRO", DbType.Int32, 128);
            db.AddOutParameter(cmd, "MSG_ERRO", DbType.String, 128);
            db.ExecuteNonQuery(cmd);
            var retorno = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CPF_TEMP_VALIDO_PARA_TROCA"));
            msg = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MSG_ERRO"));
            return retorno == "S";
        }

        public bool ValidarCpfTemporarioDestino(ObjConn objConn, DadosAcesso dadosAcesso, string cpf, out string msg)
        {
            msg = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "CARGA_VALIDA_CPF_PARA_TROCA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            db.AddOutParameter(cmd, "CPF_VALIDO_PARA_TROCA", DbType.String, 128);
            db.AddOutParameter(cmd, "ERRO", DbType.Int32, 128);
            db.AddOutParameter(cmd, "MSG_ERRO", DbType.String, 128);
            db.ExecuteNonQuery(cmd);
            var retorno = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@CPF_VALIDO_PARA_TROCA"));
            msg = string.Format(CultureInfo.CurrentCulture, "{0}", db.GetParameterValue(cmd, "@MSG_ERRO"));
            return retorno == "S";
        }

        public InclusaoTemp BuscarDadosInclusaoTemp(ObjConn objConn, DadosAcesso dadosAcesso, out string msg)
        {
            msg = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var dadosInclusaoTemp = new InclusaoTemp();
            const string sql = "RETORNA_PARAM_CPFTEMP";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "RESULTSET", DbType.String, Constantes.sim);

            IDataReader dr = null;
            dr = db.ExecuteReader(cmd);
            if (dr.Read())
            {
                if (Convert.ToInt32(dr["CODRETOUT"]) == 0)
                {
                    dadosInclusaoTemp.MaxCartTemp = Convert.ToInt32(dr["MAXCARTTEMP"]);
                    dadosInclusaoTemp.QtCartTtempSUtil = Convert.ToInt32(dr["QTCARTTEMP"]);
                }
                else
                {
                    msg = Convert.ToString(dr["@MENSOUT"]);
                }
            }
            return dadosInclusaoTemp;
        }

        public bool AceiteTermoConsentimento(ObjConn objConn, DadosAcesso dadosAcesso, out string msgRetorno)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "ACEITE_TERMO_CONSENTIMENTO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
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

        public bool ConsultaTermoConsentimento(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "SELECT ACEITE_TERMO FROM CADUSUARIO WITH (NOLOCK) WHERE ID_USUARIO = @ID_USUARIO";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, dadosAcesso.IdUsuario);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool ConsultaBloqueioCartao(ObjConn objConn, DadosAcesso dadosAcesso)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var tabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "USUARIO" : "USUARIOVA";
            var sql = "SELECT BLOQCARTUSU FROM " + tabela + " WITH (NOLOCK) WHERE CODCLI = @CODCLI AND CPF = @CPF AND NUMDEP = 0";
            var cmd = db.GetSqlStringCommand(sql.ToString());
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "CODCLI", DbType.String, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "CPF", DbType.String, dadosAcesso.Cpf);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool BloqueioDesbloqueioUsuario(ObjConn objConn, DadosAcesso dadosAcesso, string acao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            const string sql = "MW_BLOQUEIO_USUARIO";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            cmd.CommandTimeout = 60;

            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, dadosAcesso.Codigo);
            db.AddInParameter(cmd, "CPF", DbType.String, dadosAcesso.Cpf);
            db.AddInParameter(cmd, "ACAO", DbType.String, acao);

            var sucesso = false;

            try
            {
                IDataReader dr = null;
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    sucesso = Convert.ToString(dr["RETORNO"]) == "OK";
                }
            }
            catch
            {
                sucesso = false;
            }
            return sucesso;
        }
    }
}
