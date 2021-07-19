using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TELENET.SIL;
using NetCard.Common.Util;
using TELENET.SIL.PO;
using System.Data.SqlClient;
using SIL;


namespace NetCard.Common.Models
{
    public class Cliente
    {
        public int CodigoCliente { get; set; }
        public string RazaoSocial { get; set; }
        public bool Existe { get; set; }
        public decimal SaldoConta { get; set; }

        public TransfSaldo BuscarCliente(ObjConn objConn, string paramCodigoCliente)
        {
            StringBuilder sql = new StringBuilder();
            TransfSaldo retorno = new TransfSaldo();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" C.CODCLI, C.NOMCLI, C.CGC, S.DESTA, C.STA, C.PRZVALCART, C.SALDOCONTA, P.ROTULO ");
            sql.AppendLine(" FROM CLIENTEVA C WITH (NOLOCK) ");
            sql.AppendLine(" INNER JOIN STATUS S WITH (NOLOCK) ON C.STA = S.STA");
            sql.AppendLine(" INNER JOIN PRODUTO P WITH (NOLOCK) ON C.CODPROD = P.CODPROD ");

            IFilter filter = new Filter("C.CODCLI", SqlOperators.Equal, paramCodigoCliente);

            if (filter != null)
                sql.AppendLine(string.Format("WHERE {0} ", filter.FilterString));

            sql.AppendLine(" ORDER BY CODCLI");

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = new TransfSaldo()
                    {
                        NomeCliente = dr["NOMCLI"].ToString(),
                        CodigoCliente = dr["CODCLI"].ToString(),
                        SaldoContaCliente = dr["SALDOCONTA"].ToString(),
                        Sucesso = true
                    };
                }
                else
                {
                    retorno = new TransfSaldo()
                    {
                        Sucesso = false,
                        Mensagem = "Cliente não encontrado"
                    };
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }

            return retorno;
        }

        public TransfSaldo ProcessarTransferenciaDeSaldo(ObjConn objConn, int paramTipoTransferencia, int paramCodigoCliente, string paramCpfOrigem, string paramCpfDestino,
            string paramCartaoOrigem, string paramCartaoDestino, decimal paramValor, int paramIdfunc, string paramBancoAutorizador, string paramLogin, string paramCodigoOperadora)
        {
            try
            {
                Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
                string sql = "PROC_TRANSF_SALDO";
                var cmd = db.GetStoredProcCommand(sql.ToString());

                db.AddInParameter(cmd, "@TIPO", DbType.Int32, paramTipoTransferencia);
                db.AddInParameter(cmd, "@CODCLI", DbType.Int32, paramCodigoCliente);
                db.AddInParameter(cmd, "@CPF_ORIG", DbType.String, paramCpfOrigem);
                db.AddInParameter(cmd, "@CPF_DEST", DbType.String, paramCpfDestino);
                db.AddInParameter(cmd, "@CRT_ORIG", DbType.String, paramCartaoOrigem);
                db.AddInParameter(cmd, "@CRT_DEST", DbType.String, paramCartaoDestino);
                db.AddInParameter(cmd, "@VALOR", DbType.Decimal, paramValor);
                db.AddInParameter(cmd, "@BANCO", DbType.String, paramBancoAutorizador);
                db.AddInParameter(cmd, "@ID_FUNC", DbType.Int32, paramIdfunc);
                
                var dr = db.ExecuteReader(cmd);
                dr.Close();

                sql = "PROC_INSERE_LOGVAWEB";
                cmd = db.GetStoredProcCommand(sql.ToString());                

                db.AddInParameter(cmd, "@DATA", DbType.DateTime, DateTime.Now);
                db.AddInParameter(cmd, "@CODCLI", DbType.Int32, paramCodigoCliente);
                db.AddInParameter(cmd, "@LOGIN", DbType.String, paramLogin);
                db.AddInParameter(cmd, "@CODOPE", DbType.Int32, paramCodigoOperadora);
                db.AddInParameter(cmd, "@OPERACAO", DbType.String, "Transferência de Saldo");
                db.AddInParameter(cmd, "@CPF", DbType.String, paramCpfOrigem);
                db.AddInParameter(cmd, "@CODCRT", DbType.String, paramCartaoOrigem);

                dr = db.ExecuteReader(cmd);
                dr.Close();

                return new TransfSaldo()
                {
                    Mensagem = "Transferência realizada com sucesso.",
                    Sucesso = true
                };
            }
            catch
            {
                return new TransfSaldo()
                {
                    Mensagem = "Ocorreu um erro ao realizar a transferência.",
                    Sucesso = false
                };
                throw;
            }
        }

        public List<BENEFICIO_CLIENTE> GetBeneficiosCliente(ObjConn objConexao)
        {
            List<BENEFICIO_CLIENTE> retorno = new List<BENEFICIO_CLIENTE>();

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = @" SELECT A.CODCLI, A.CODBENEF, B.NOMBENEF, A.COMPULSORIO, A.VALTIT, CONVERT(VARCHAR, A.DTASSOC, 103) DTASSOC
                            FROM BENEFCLI A WITH (NOLOCK) 
                            INNER JOIN BENEFICIO B WITH (NOLOCK) ON A.CODBENEF = B.CODBENEF
                            WHERE A.CODCLI = " + CodigoCliente.ToString();

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
                        BENEFICIO_CLIENTE benef = new BENEFICIO_CLIENTE();

                        benef.CODBENEF = Convert.ToInt32(item["CODBENEF"].ToString());
                        benef.CODCLI = Convert.ToInt32(item["CODCLI"].ToString());
                        benef.NOMBENEF = item["NOMBENEF"].ToString();
                        benef.COMPULSORIO = item["COMPULSORIO"].ToString();
                        benef.VALTIT = !string.IsNullOrEmpty(item["VALTIT"].ToString()) ? Convert.ToDecimal(item["VALTIT"].ToString()) : 0;
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

        public bool VerificaExistenciaBeneficio(ObjConn objConexao, string codigoCliente)
        {
            List<BENEFICIO_CLIENTE> retorno = new List<BENEFICIO_CLIENTE>();

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = "SELECT COUNT(*) CONT FROM BENEFCLI WITH (NOLOCK) WHERE CODCLI = " + codigoCliente;

            SqlCommand comando = new SqlCommand(sql, conexao);

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                int numeroBeneficios = Convert.ToInt32(dt.Rows[0]["CONT"].ToString());

                return numeroBeneficios > 0;

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

        public bool VerificarBeneficioCompulsório(ObjConn objConexao, string codigoCliente, string codigoBeneficio)
        {
            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));

            string sql = "SELECT COMPULSORIO FROM BENEFCLI WITH (NOLOCK) WHERE CODCLI = " + codigoCliente + " AND CODBENEF = " + codigoBeneficio;

            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.Text };

            try
            {
                conexao.Open();

                DataTable dt = new DataTable();

                dt.Load(comando.ExecuteReader());

                conexao.Close();

                return dt.Rows[0]["COMPULSORIO"].ToString().Equals("S");
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
        public Cliente BuscaDadosCli(ObjConn objConn, string codigoCliente, int sistema)
        {
            StringBuilder sql = new StringBuilder();
            Cliente retorno = new Cliente();


            sql.AppendLine(" SELECT");
            sql.AppendLine(" C.CODCLI, C.NOMCLI ");

            if (sistema == 1)
                sql.AppendLine(" FROM CLIENTEVA C WITH (NOLOCK) ");
            else
                sql.AppendLine(" FROM CLIENTE C WITH (NOLOCK) ");

            sql.AppendLine($" WHERE C.CODCLI = {codigoCliente} ");

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConn));
            var cmd = db.GetSqlStringCommand(sql.ToString());

            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    retorno = new Cliente()
                    {
                        CodigoCliente = Convert.ToInt32(dr["CODCLI"].ToString()),
                        RazaoSocial = dr["NOMCLI"].ToString()
                    };
                }
                else
                {
                    retorno = new Cliente()
                    {
                        RazaoSocial = "Credenciado não encontrado"
                    };
                }
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
            }

            return retorno;
        }

    }
}
