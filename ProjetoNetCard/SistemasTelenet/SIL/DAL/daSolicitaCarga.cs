using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using TELENET.SIL;
using TELENET.SIL.BL;
using TELENET.SIL.DA;
using TELENET.SIL.PO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SIL.BLL;
using System.Threading;
using System.Net.Mail;
using System.Net;

namespace SIL.DAL
{
    public class daSolicitaCarga
    {
        public OPERADORA daOperadora = new OPERADORA();
        private string BDTELENET = string.Empty;
        private string BDAUTORIZADOR = string.Empty;

        //private Thread threadCargaValidaLayout;
        //private Thread threadCargaVerificaDadosParaSolicitar;
        //private Thread threadCargaSolicita;
        //private Thread threadConsultaSemaforo;
        //private Thread threadPrincipalCarga;

        //private DataTable retornoCargaValidaLayout;
        //private DataTable retornoCargaVerificaDadosParaSolicitar;
        //private DataTable retornoCargaSolicita;
        //private DataTable retornoConsultaSemaforo;
        private DataTable retornoDataTable = new DataTable();


        //private SqlConnection conexaoPrincipal;

        //private SqlConnection conexaoCargaValidaLayout;
        //private SqlConnection conexaoCargaVerificaDadosParaSolicitar;
        //private SqlConnection conexaoCargaSolicita;
        //private SqlConnection conexaoConsultaSemaforo;

        //private SqlCommand comandoCargaValidaLayout;
        //private SqlCommand comandoCargaVerificaDadosParaSolicitar;
        //private SqlCommand comandoCargaSolicita;
        //private SqlCommand comandoConsultaSemaforo;

        //private string caminhoArquivoCarga;
        //private string nomeArquivoCarga;
        //private string nomeOriginalArquivoCarga;
        //private string idSessaoCarga;

        //private string erroe = "";
        //private string textoe = "";
        //private string numCarga = "";
        //private string valorCarga = "";
        //private string dataProg = "";
        //private string cnpj = "";
        //private string nomeTablela = "";
        //private string codigoCliente = "";

        //private bool continuarProcessamento;
        //private bool continuarProcessamentoPrincipal;
        //private bool processamentoValidaCargaValido = false;

        public daSolicitaCarga(OPERADORA operador)
        {
            daOperadora = operador;

            BDTELENET = string.Format(ConstantesSIL.BDTELENET, operador.SERVIDORNC, operador.BANCONC,
                                      ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, operador.SERVIDORAUT, operador.BANCOAUT,
                                          ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public int GetUltimaCarga(int codCli)
        {
            IDataReader dr = null;
            var aux = 0;
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd = new SqlCommand("PROC_NUM_PROX_CARGA", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(codCli);
            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    var proxCod = dr["MAXNUMCARGA"];
                    int.TryParse(proxCod.ToString(), out aux);
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
            return aux > 999 ? 1 : aux;
        }

        private string CripSenha(string senha)
        {
            //dllcrip.ENCRIPT_C dllEncrip = new ENCRIPT_C();
            //ulong a;
            string novaSenha = string.Empty;
            string senhaNormalizada = senha.PadRight(8, '0');
            novaSenha = BlCriptografia.Encrypt(senhaNormalizada);
            //a = dllEncrip.CalcDes(Convert.ToInt32(senha));
            //novaSenha = Int642Char(a);
            return novaSenha;
        }

        public bool ExisteCargaAndamento(int codCli, int numCarga)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd =
                new SqlCommand("SELECT LIBCARGA FROM CARGAC WITH (NOLOCK) WHERE CODCLI = @CODCLI AND NUMCARG_VA = @NUMCARGA AND DTCARGA IS NULL ",
                    conn) { CommandType = CommandType.Text };
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
            cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = numCarga;
            IDataReader dr = null;

            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                var cargaAndamento = false;
                if (dr.Read())
                {
                    cargaAndamento = dr["LIBCARGA"] != DBNull.Value && (dr["LIBCARGA"].ToString() == "S");
                }
                dr.Close();
                return cargaAndamento;
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        private static void GeraTransacao(USUARIO_VA usu, SqlConnection conn, SqlTransaction trans, int tiptra, int idOper, int numDep = 0)
        {
            var cmd = new SqlCommand("PROC_GRAVAR_TRANSACAO", conn, trans) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
            cmd.Parameters.Add("@TIPTRA", SqlDbType.Int).Value = tiptra;
            cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usu.CODCRT;//.Substring(0, 17);
            cmd.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = 0;
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
            cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = numDep;
            cmd.Parameters.Add("@NUMCARGAVA", SqlDbType.Int).Value = usu.NUMCARGA;
            cmd.Parameters.Add("@NUMFECCRE", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@DAD", SqlDbType.VarChar).Value = string.Empty;
            cmd.Parameters.Add("@ATV", SqlDbType.VarChar).Value = "N";
            cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = idOper;
            cmd.ExecuteNonQuery();
        }

        public List<string> VerificaCargaCliente(int codCli, int numCargaInt, decimal valorTotal, string dtProg,
                                                 string cnpj, int idOperWeb, int codOpeSis, int idArq)
        {
            var res = new List<string>();
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd = new SqlCommand("SP_VERIFICA_CLIENTE_CARGA", conn) { CommandType = CommandType.StoredProcedure, Connection = conn };

            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
            cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = numCargaInt;
            cmd.Parameters.Add("@VALORTOTAL", SqlDbType.Decimal).Value = valorTotal;
            if (!string.IsNullOrEmpty(dtProg))
                cmd.Parameters.Add("@DTPROG", SqlDbType.DateTime).Value = dtProg;
            if (!string.IsNullOrEmpty(cnpj))
                cmd.Parameters.Add("@CGC", SqlDbType.VarChar).Value = cnpj;
            cmd.Parameters.Add("@IDOPER", SqlDbType.Int).Value = idOperWeb == 0 ? (object)DBNull.Value : idOperWeb;
            cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = codOpeSis == 0 ? (object)DBNull.Value : codOpeSis;
            cmd.Parameters.Add("@ARQ", SqlDbType.Int).Value = idArq;
            IDataReader dr = null;

            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    res.Add(dr["RETORNO"].ToString());
                    if (res[0] == "OK")
                    {
                        res.Add(dr["NUMPAC"].ToString());
                        res.Add(dr["TAXADESTIT"].ToString());
                        res.Add(dr["TAXADESDEP"].ToString());
                        res.Add(dr["PRZVALCART"].ToString());
                        res.Add(dr["CODCLI"].ToString());
                        res.Add(dr["CARGAPADVACLIENTE"].ToString());
                    }
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
            return res;
        }

        public bool VerificaUsuario(USUARIO_VA usuario, out string cargPadVa, bool atualizaCargaPadrao)
        {
            var sta = string.Empty;
            var cpf = string.Empty;
            var mat = string.Empty;
            var set = string.Empty;
            cargPadVa = string.Empty;

            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd =
                new SqlCommand(
                    "SELECT S.STA, U.CPF, U.MAT, U.CODSET, U.CARGPADVA FROM STATUS S WITH (NOLOCK) INNER JOIN USUARIOVA U WITH (NOLOCK) ON U.STA = S.STA " +
                    "WHERE U.CODCLI = @CODCLI AND U.CPF = @CPF AND U.NUMDEP = 0",
                    conn) { CommandType = CommandType.Text };
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
            IDataReader dr = null;

            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    sta = dr["STA"].ToString();
                    cpf = dr["CPF"].ToString();
                    mat = dr["MAT"].ToString();
                    set = dr["CODSET"].ToString();
                    cargPadVa = dr["CARGPADVA"].ToString();
                }

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                //Usuario nao existe
                var daUsu = new daUsuarioVA(daOperadora);
                var reincCRT = daUsu.ReincluiCrtParamVa();
                if (cpf == string.Empty || sta == "02")
                    if (reincCRT || cpf == string.Empty)
                        return false;

                //Mantenho o status do usuario vindo do banco
                usuario.STA = sta;

                //Ser a matricula do arquivo for vazia, mantenho a do banco
                if (string.IsNullOrEmpty(usuario.MAT))
                    usuario.MAT = mat;

                //Ser o setor do arquivo for vazio, mantenho a do banco
                if (string.IsNullOrEmpty(usuario.CODSET))
                    usuario.CODSET = set;

                //Se o valor da carga padrao for 0, altero para o valor da carga atual.
                if (Convert.ToDecimal(cargPadVa) == 0 && atualizaCargaPadrao)
                {
                    AtualizaCargaPadrao(usuario);
                    cargPadVa = usuario.CARGPADVACLIENTE.ToString(CultureInfo.InvariantCulture);
                }
                dr.Close();
                return true;
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        public bool VerificaUsuarioLayout7(USUARIO_VA usuario, out string cargPadVa, bool atualizaCargaPadrao)
        {
            var sta = string.Empty;
            var cpf = string.Empty;
            var mat = string.Empty;
            var set = string.Empty;
            cargPadVa = string.Empty;

            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd =
                new SqlCommand(
                    "SELECT S.STA, U.CPF, U.MAT, U.CODSET, U.CARGPADVA FROM STATUS S WITH (NOLOCK) INNER JOIN USUARIOVA U WITH (NOLOCK) ON U.STA = S.STA " +
                    "WHERE U.CODCLI = @CODCLI AND U.CPF = @CPF AND U.CODSET = @CODSET",
                    conn) { CommandType = CommandType.Text };
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
            cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = usuario.CODSET;
            IDataReader dr = null;

            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    sta = dr["STA"].ToString();
                    cpf = dr["CPF"].ToString();
                    mat = dr["MAT"].ToString();
                    set = dr["CODSET"].ToString();
                    cargPadVa = dr["CARGPADVA"].ToString();
                }
                dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                //Usuario nao existe
                var daUsu = new daUsuarioVA(daOperadora);
                var reincCRT = daUsu.ReincluiCrtParamVa();
                if (cpf == string.Empty || sta == "02")
                    if (reincCRT || cpf == string.Empty)
                        return false;

                //Mantenho o status do usuario vindo do banco
                usuario.STA = sta;

                //Ser a matricula do arquivo for vazia, mantenho a do banco
                if (string.IsNullOrEmpty(usuario.MAT))
                    usuario.MAT = mat;

                //Ser o setor do arquivo for vazio, mantenho a do banco
                if (string.IsNullOrEmpty(usuario.CODSET))
                    usuario.CODSET = set;

                //Se o valor da carga padrao for 0, altero para o valor da carga atual.
                if (Convert.ToDecimal(cargPadVa) == 0 && atualizaCargaPadrao)
                {
                    AtualizaCargaPadrao(usuario);
                    cargPadVa = usuario.CARGPADVACLIENTE.ToString(CultureInfo.InvariantCulture);
                }

                return true;
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        private void AtualizaCargaPadrao(USUARIO_VA usuario)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            try
            {
                const string sql =
                    "UPDATE USUARIOVA SET CARGPADVA = CARGPADVA + @CARGPADVA WHERE CODCLI = @CODCLI AND CPF = @CPF AND NUMDEP = 0";
                var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text };

                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usuario.TOTALCARGAUSUARIO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usuario.CODCLI);
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
                conn.Open();
                cmd.ExecuteNonQuery();

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                throw;
            }
        }

        private void InsereCartaoNetcardAutorizador(USUARIO_VA usu, int idOper)
        {
            //Transacoes para o NetCard
            var conn = new SqlConnection();
            SqlTransaction trans = null;
            //Transacoes para o Autorizador
            var connAut = new SqlConnection();
            SqlTransaction transAut = null;

            try
            {
                //Faco um update na tabela ParamVa para atualizar para o proximo cartao                            
                //UpdateParam();
                //usu.CODCRT = MontaTrilha(1, 1); //Monto o codigo do cartao 
                if (string.IsNullOrEmpty(usu.CODCRT)) return;

                //Insiro o Usuario no NetCard                                           
                conn.ConnectionString = BDTELENET;
                conn.Open();
                trans = conn.BeginTransaction();
                InsereUsuarioCarga(usu, conn, trans);

                //Insere no Autorizador
                connAut.ConnectionString = BDAUTORIZADOR;
                connAut.Open();
                transAut = connAut.BeginTransaction();
                InsereUsuarioCargaAutorizador(usu, connAut, transAut);

                //GeraTransacao(usu, conn, trans, 999901);
                GeraTransacao(usu, conn, trans, 999001, idOper);

                //Faco o commit para os novos usuarios                             
                transAut.Commit();
                trans.Commit();

                //Fecho a conexao
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                if (connAut.State == ConnectionState.Open)
                    connAut.Close();
            }
            catch
            {
                //Erro - Faco o rollback em tudo
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                if (transAut != null && transAut.Connection != null)
                    transAut.Rollback();

                //Fecho a conexao
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                if (connAut.State == ConnectionState.Open)
                    connAut.Close();

                throw;
            }
        }

        //private bool InsereCartaoNetcardAutorizadorLayout7(USUARIO_VA usu, int idOper)
        //{
        //    //Transacoes para o NetCard
        //    var conn = new SqlConnection();
        //    SqlTransaction trans = null;
        //    //Transacoes para o Autorizador
        //    var connAut = new SqlConnection();
        //    SqlTransaction transAut = null;

        //    var continuarOperacao = false;

        //    try
        //    {
        //        //Faco um update na tabela ParamVa para atualizar para o proximo cartao                            
        //        //UpdateParam();
        //        //usu.CODCRT = MontaTrilha(1, 1); //Monto o codigo do cartao 
        //        if (string.IsNullOrEmpty(usu.CODCRT)) return false;

        //        //Insiro o Usuario no NetCard                                           
        //        conn.ConnectionString = BDTELENET;
        //        conn.Open();
        //        trans = conn.BeginTransaction();

        //        int numeroDependente = 0;
        //        string codigoCartaoTitular = "";
        //        continuarOperacao = InsereUsuarioCargaLayout7(usu, conn, trans, ref numeroDependente);

        //        if (continuarOperacao)
        //        {
        //            if (numeroDependente > 0)
        //            {
        //                var cmd = new SqlCommand("SELECT CODCRT FROM USUARIOVA WHERE CPF = @CPF AND CODCLI = @CODCLI AND NUMDEP = 0", conn, trans);

        //                cmd.Parameters.Clear();
        //                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
        //                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);

        //                DataTable dt = new DataTable();
        //                dt.Load(cmd.ExecuteReader());

        //                codigoCartaoTitular = dt.Rows[0]["CODCRT"].ToString();
        //            }

        //            connAut.ConnectionString = BDAUTORIZADOR;
        //            connAut.Open();
        //            transAut = connAut.BeginTransaction();
        //            InsereUsuarioCargaAutorizador(usu, connAut, transAut, numeroDependente, codigoCartaoTitular);

        //            //GeraTransacao(usu, conn, trans, 999901);
        //            GeraTransacao(usu, conn, trans, 999001, idOper, numeroDependente);

        //            //Faco o commit para os novos usuarios                             
        //            transAut.Commit();
        //            trans.Commit();

        //            //Fecho a conexao
        //            if (conn.State == ConnectionState.Open)
        //                conn.Close();
        //            if (connAut.State == ConnectionState.Open)
        //                connAut.Close();

        //            return true;
        //        }
        //        else
        //        {
        //            //Faco o commit para os novos usuarios                             
        //            transAut.Rollback();
        //            trans.Rollback();

        //            //Fecho a conexao
        //            if (conn.State == ConnectionState.Open)
        //                conn.Close();
        //            if (connAut.State == ConnectionState.Open)
        //                connAut.Close();

        //            return false;
        //        }

        //    }
        //    catch
        //    {
        //        //Erro - Faco o rollback em tudo
        //        if (trans != null && trans.Connection != null)
        //            trans.Rollback();
        //        if (transAut != null && transAut.Connection != null)
        //            transAut.Rollback();

        //        //Fecho a conexao
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();
        //        if (connAut.State == ConnectionState.Open)
        //            connAut.Close();

        //        throw;
        //    }
        //}

        private void InsereUsuarioCarga(USUARIO_VA usu, SqlConnection conn, SqlTransaction trans)
        {

            var cmd = new SqlCommand("INSERT into USUARIOVA (CODCLI, CPF, NUMDEP, CODFIL, NOMUSU, " +
                                     "DATINC, CODSET, MAT, GERCRT, CODCRT, STA, DATSTA, " +
                                     "CARGPADVA, ULTCARGVA, VCARGAUTO, SENHA, NUMPAC, NUMULTPAC, VALADES, " +
                                     "DTVALCART, TRILHA2, CODPAR, NOMCRT, SENUSU, DTEXPSENHA) " +
                                     "VALUES (@CODCLI, @CPF, @NUMDEP, @CODFIL, @NOMUSU, " +
                                     "@DATINC, @CODSET, @MAT, @GERCRT, @CODCRT, @STA, @DATSTA, " +
                                     "@CARGPADVA, @ULTCARGVA, @VCARGAUTO, @SENHA, @NUMPAC, @NUMULTPAC, @VALADES, " +
                                     "@DTVALCART, @TRILHA2, @CODPAR, @NOMCRT, @SENUSU, @DTEXPSENHA)",
                                     conn, trans) { CommandType = CommandType.Text };

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
            cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = Convert.ToInt16(usu.NUMDEP);
            cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = !string.IsNullOrEmpty(usu.CODFIL.Trim())
                                                                     ? (object)Convert.ToInt32(usu.CODFIL)
                                                                     : DBNull.Value;
            cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usu.NOMUSU;
            cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(usu.CODSET.Trim())
                                                                     ? (object)usu.CODSET
                                                                     : DBNull.Value;
            cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usu.MAT.Trim();
            cmd.Parameters.Add("@GERCRT", SqlDbType.VarChar).Value = "X";
            cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usu.CODCRT;//.Substring(0, 17);
            cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = "00";
            cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = DateTime.Now.Date;

            if (usu.TOTALCARGAUSUARIO <= usu.CARGPADVACLIENTE)
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usu.TOTALCARGAUSUARIO;
            else
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usu.CARGPADVACLIENTE;

            cmd.Parameters.Add("@ULTCARGVA", SqlDbType.Int).Value = usu.ULTCARGVA;
            cmd.Parameters.Add("@VCARGAUTO", SqlDbType.Decimal).Value = 0;
            cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = CripSenha(usu.CPF.Substring(0, 4));
            cmd.Parameters.Add("@SENUSU", SqlDbType.VarChar).Value = CripSenha(usu.CPF.Substring(0, 8));
            cmd.Parameters.Add("@DTEXPSENHA", SqlDbType.VarChar).Value = DateTime.Now.AddDays(DiasParaRenovarSenha()).ToString("yyyyMMdd");
            cmd.Parameters.Add("@NUMPAC", SqlDbType.Int).Value = !string.IsNullOrEmpty(usu.NUMPAC.Trim())
                                                                     ? (object)Convert.ToInt32(usu.NUMPAC)
                                                                     : DBNull.Value;
            cmd.Parameters.Add("@NUMULTPAC", SqlDbType.Int).Value = 0;
            cmd.Parameters.Add("@VALADES", SqlDbType.Decimal).Value = Convert.ToDecimal(usu.VALADES);
            cmd.Parameters.Add("@DTVALCART", SqlDbType.VarChar).Value = usu.DTVALCART;
            cmd.Parameters.Add("@TRILHA2", SqlDbType.VarChar).Value = usu.TRILHA2.Trim();//usu.CODCRT.Trim();
            cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = DBNull.Value;
            cmd.Parameters.Add("@NOMCRT", SqlDbType.VarChar).Value = usu.NOMCRT;
            cmd.ExecuteNonQuery();
            //conn.Close();
        }

        public int DiasParaRenovarSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

//        private bool InsereUsuarioCargaLayout7(USUARIO_VA usu, SqlConnection conn, SqlTransaction trans, ref int numeroDependente)
//        {
//            SqlConnection connConsulta = new SqlConnection(conn.ConnectionString);
//            connConsulta.Open();


//            string sqlQueryConsulta = @"SELECT TOP 1 NUMDEP, CODSET FROM USUARIOVA WHERE CPF = @CPF AND @CODCLI = CODCLI ORDER BY NUMDEP DESC";

//            string sqlQuery = @"INSERT into USUARIOVA (CODCLI, CPF, NUMDEP, CODFIL, NOMUSU, 
//                                     DATINC, CODSET, MAT, GERCRT, CODCRT, STA, DATSTA, 
//                                     CARGPADVA, ULTCARGVA, VCARGAUTO, SENHA, NUMPAC, NUMULTPAC, VALADES, 
//                                     DTVALCART, TRILHA2, CODPAR, NOMCRT, GENERICO ) 
//                                     VALUES (@CODCLI, @CPF, @NUMDEP, @CODFIL, @NOMUSU, 
//                                     @DATINC, @CODSET, @MAT, @GERCRT, @CODCRT, @STA, @DATSTA, 
//                                     @CARGPADVA, @ULTCARGVA, @VCARGAUTO, @SENHA, @NUMPAC, @NUMULTPAC, @VALADES, 
//                                     @DTVALCART, @TRILHA2, @CODPAR, @NOMCRT, @GENERICO)";

//            var cmdConsulta = new SqlCommand(sqlQueryConsulta, conn, trans) { CommandType = CommandType.Text };

//            cmdConsulta.Parameters.Clear();
//            cmdConsulta.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
//            cmdConsulta.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;

//            var numDepa = string.Empty;

//            DataTable dt = new DataTable();
//            dt.Load(cmdConsulta.ExecuteReader());

//            if (dt.Rows.Count > 0)
//            {
//                if (dt.Rows[0]["CODSET"].ToString() == usu.CODSET)
//                {
//                    numeroDependente = 0;
//                    return false;
//                }
//                else
//                {
//                    int numDep = Convert.ToInt32(dt.Rows[0]["NUMDEP"].ToString()) + 1;
//                    numeroDependente = numDep;

//                    var cmd = new SqlCommand(sqlQuery, conn, trans) { CommandType = CommandType.Text };

//                    cmd.Parameters.Clear();
//                    cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
//                    cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
//                    cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = numDep;
//                    cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = !string.IsNullOrEmpty(usu.CODFIL.Trim())
//                                                                             ? (object)Convert.ToInt32(usu.CODFIL)
//                                                                             : DBNull.Value;
//                    cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usu.NOMUSU;
//                    cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = DateTime.Now.Date;
//                    cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(usu.CODSET.Trim())
//                                                                             ? (object)usu.CODSET
//                                                                             : DBNull.Value;
//                    cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usu.MAT.Trim();
//                    cmd.Parameters.Add("@GERCRT", SqlDbType.VarChar).Value = "X";
//                    cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usu.CODCRT;//.Substring(0, 17);
//                    cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = "00";
//                    cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = DateTime.Now.Date;
//                    cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usu.CARGPADVACLIENTE;
//                    cmd.Parameters.Add("@ULTCARGVA", SqlDbType.Int).Value = usu.ULTCARGVA;
//                    cmd.Parameters.Add("@VCARGAUTO", SqlDbType.Decimal).Value = 0;
//                    cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = CripSenha(usu.CPF.Substring(0, 4));
//                    cmd.Parameters.Add("@NUMPAC", SqlDbType.Int).Value = !string.IsNullOrEmpty(usu.NUMPAC.Trim())
//                                                                             ? (object)Convert.ToInt32(usu.NUMPAC)
//                                                                             : DBNull.Value;
//                    cmd.Parameters.Add("@NUMULTPAC", SqlDbType.Int).Value = 0;
//                    cmd.Parameters.Add("@VALADES", SqlDbType.Decimal).Value = Convert.ToDecimal(usu.VALADES);
//                    cmd.Parameters.Add("@DTVALCART", SqlDbType.VarChar).Value = usu.DTVALCART;
//                    cmd.Parameters.Add("@TRILHA2", SqlDbType.VarChar).Value = usu.TRILHA2.Trim();//usu.CODCRT.Trim();
//                    cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = DBNull.Value;
//                    cmd.Parameters.Add("@NOMCRT", SqlDbType.VarChar).Value = usu.NOMCRT;
//                    cmd.Parameters.Add("@GENERICO", SqlDbType.VarChar).Value = usu.GENERICO;

//                    cmd.ExecuteNonQuery();

//                    return true;
//                }
//            }
//            else
//            {
//                numeroDependente = Convert.ToInt32(usu.NUMDEP);
//                var cmd = new SqlCommand(sqlQuery, conn, trans) { CommandType = CommandType.Text };

//                cmd.Parameters.Clear();
//                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
//                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
//                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = Convert.ToInt32(usu.NUMDEP);
//                cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = !string.IsNullOrEmpty(usu.CODFIL.Trim())
//                                                                         ? (object)Convert.ToInt32(usu.CODFIL)
//                                                                         : DBNull.Value;
//                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usu.NOMUSU;
//                cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = DateTime.Now.Date;
//                cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(usu.CODSET.Trim())
//                                                                         ? (object)usu.CODSET
//                                                                         : DBNull.Value;
//                cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usu.MAT.Trim();
//                cmd.Parameters.Add("@GERCRT", SqlDbType.VarChar).Value = "X";
//                cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usu.CODCRT;//.Substring(0, 17);
//                cmd.Parameters.Add("@STA", SqlDbType.VarChar).Value = "00";
//                cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = DateTime.Now.Date;
//                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usu.CARGPADVACLIENTE;
//                cmd.Parameters.Add("@ULTCARGVA", SqlDbType.Int).Value = usu.ULTCARGVA;
//                cmd.Parameters.Add("@VCARGAUTO", SqlDbType.Decimal).Value = 0;
//                cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = CripSenha(usu.CPF.Substring(0, 4));
//                cmd.Parameters.Add("@NUMPAC", SqlDbType.Int).Value = !string.IsNullOrEmpty(usu.NUMPAC.Trim())
//                                                                         ? (object)Convert.ToInt32(usu.NUMPAC)
//                                                                         : DBNull.Value;
//                cmd.Parameters.Add("@NUMULTPAC", SqlDbType.Int).Value = 0;
//                cmd.Parameters.Add("@VALADES", SqlDbType.Decimal).Value = Convert.ToDecimal(usu.VALADES);
//                cmd.Parameters.Add("@DTVALCART", SqlDbType.VarChar).Value = usu.DTVALCART;
//                cmd.Parameters.Add("@TRILHA2", SqlDbType.VarChar).Value = usu.TRILHA2.Trim();//usu.CODCRT.Trim();
//                cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = DBNull.Value;
//                cmd.Parameters.Add("@NOMCRT", SqlDbType.VarChar).Value = usu.NOMCRT;
//                cmd.Parameters.Add("@GENERICO", SqlDbType.VarChar).Value = usu.GENERICO;

//                cmd.ExecuteNonQuery();

//                return true;
//            }


//        }

        private void InsereUsuarioCargaAutorizador(USUARIO_VA usu, SqlConnection connAut, SqlTransaction transAut, int numeroDependente = 0, string codigoCartaoTitular = "")
        {
            string sqlQuery = @"INSERT into CTCARTVA (CODEMPRESA, CODCARTAO, STATUSU, DTSTATUSU, NOMEUSU, NUMDEPEND, CODCARTIT, CPFTIT, DTVALCART, SENHA, SALDOVA, DTVAULT)
                                        VALUES (@CODEMPRESA, @CODCARTAO, @STATUSU, @DTSTATUSU, @NOMEUSU, @NUMDEPEND, @CODCARTIT,  @CPFTIT, @DTVALCART, @SENHA, @SALDOVA, null) ";

            if (string.IsNullOrEmpty(codigoCartaoTitular))
                sqlQuery = sqlQuery.Replace("@CODCARTIT", "NULL");

            var cmd = new SqlCommand(sqlQuery, connAut, transAut) { CommandType = CommandType.Text };

            cmd.Parameters.Add("@CODEMPRESA", SqlDbType.VarChar).Value = usu.CODCLI.PadLeft(5, '0');
            cmd.Parameters.Add("@CODCARTAO", SqlDbType.VarChar).Value = usu.CODCRT;//.Substring(0, 17);
            cmd.Parameters.Add("@STATUSU", SqlDbType.VarChar).Value = "00";
            cmd.Parameters.Add("@DTSTATUSU", SqlDbType.DateTime).Value = DateTime.Now.Date;
            cmd.Parameters.Add("@NOMEUSU", SqlDbType.VarChar).Value = usu.NOMCRT;

            if (!string.IsNullOrEmpty(codigoCartaoTitular))
                cmd.Parameters.Add("@CODCARTIT", SqlDbType.VarChar).Value = codigoCartaoTitular;

            cmd.Parameters.Add("@CPFTIT", SqlDbType.VarChar).Value = usu.CPF;
            cmd.Parameters.Add("@NUMDEPEND", SqlDbType.VarChar).Value = numeroDependente.ToString().PadLeft(2, '0');
            cmd.Parameters.Add("@DTVALCART", SqlDbType.VarChar).Value = usu.DTVALCART;
            cmd.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = CripSenha(usu.CPF.Substring(0, 4));
            cmd.Parameters.Add("@SALDOVA", SqlDbType.Decimal).Value = 0.00;
            cmd.ExecuteNonQuery();
        }

        private static void InsereCentroCusto(USUARIO_VA usu, SqlConnection conn, SqlTransaction trans)
        {
            var cmd = new SqlCommand("INSERT INTO CARGACCUSTO ( CODCLI, CPF, NUMCARG_VA, CENTRO_CUSTO, VALOR, DTAUTORIZ) " +
                                     "VALUES ( @CODCLI , @CPF, @NUMCARG_VA, @CENTRO_CUSTO, @VALOR, GETDATE())", conn, trans) { CommandType = CommandType.Text };
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
            cmd.Parameters.Add("@NUMCARG_VA", SqlDbType.Int).Value = usu.NUMCARGA;
            cmd.Parameters.Add("@CENTRO_CUSTO", SqlDbType.VarChar).Value = usu.CENTROCUSTO;
            cmd.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = usu.CARGPADVA;
            cmd.ExecuteNonQuery();
        }

        private void SolicitaCarga(USUARIO_VA usu, SqlConnection conn, SqlTransaction trans, bool atualizaValorCarga, bool atualizaSetor = true)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE USUARIOVA SET ULTCARGVA = @NUMCARGA, MAT = @MAT");
            if (atualizaValorCarga)
                sql.AppendLine(", CARGPADVA = @CARGPADVA");

            if (atualizaSetor)
                sql.AppendLine(", CODSET = @CODSET ");

            sql.AppendLine("WHERE CODCLI = @CODCLI AND CPF = @CPF AND NUMDEP = 0");
            var cmd = new SqlCommand(sql.ToString(), conn, trans) { CommandType = CommandType.Text };

            cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = usu.NUMCARGA;
            cmd.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = usu.CARGPADVA;
            cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usu.MAT;
            cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = !string.IsNullOrEmpty(usu.CODSET)
                                                                     ? (object)usu.CODSET
                                                                     : DBNull.Value;
            if (atualizaValorCarga)
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usu.TOTALCARGAUSUARIO;

            //Where
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(usu.CODCLI);
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
            cmd.ExecuteNonQuery();
        }

        private void SolicitaCarga(USUARIO_VA usu, bool atualizaValorCarga)
        {
            var conn = new SqlConnection();
            SqlTransaction trans = null;
            try
            {
                conn.ConnectionString = BDTELENET;
                conn.Open();
                trans = conn.BeginTransaction();

                //Solicito a Carga
                SolicitaCarga(usu, conn, trans, atualizaValorCarga);
                //Caso exista centro de custo atualizo a tabela CargaCCusto
                //if (usu.TIPO_LAYOUT == "05" || usu.TIPO_LAYOUT == "06")
                InsereCentroCusto(usu, conn, trans);

                //Faco o commit                             
                trans.Commit();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch
            {
                //Faco o rollback em tudo
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                throw new Exception("Erro ao Solicitar a carga");
            }
        }

        public void BloqueiaSaldoContacliente(int CODCLI, int NUMCARG_VA)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };

            string sql = @"
                            DECLARE @SALDOCONTA NUMERIC(15,2)
                            DECLARE @VALORCARGA NUMERIC(15,2)

                            SELECT @SALDOCONTA = ISNULL(SALDOCONTA, 0) FROM CLIENTEVA WITH (NOLOCK) WHERE CODCLI = #CODCLI#

                            IF(@SALDOCONTA > 0)
                            BEGIN
                                SELECT @VALORCARGA = SUM(VCARGAUTO) FROM USUARIOVA WITH (NOLOCK) WHERE CODCLI = #CODCLI# AND ULTCARGVA = #NUMCARG_VA# AND VCARGAUTO > 0
                                --SELECT @VALORCARGA = VALOR FROM CARGAC WHERE NUMCARG_VA = #NUMCARG_VA# AND CODCLI = #CODCLI#

                                -- ALTERA O VALOR A SER USADO CASO O VALOR DA CARGA SEJA MENOR QUE O SALDO DA CONTA
                                IF(@VALORCARGA <= @SALDOCONTA)
                                    SELECT @SALDOCONTA = @VALORCARGA
                            
                                UPDATE CARGAC SET SALDOCONTABLOQ = @SALDOCONTA WHERE NUMCARG_VA = #NUMCARG_VA# AND CODCLI = #CODCLI#
                                UPDATE CLIENTEVA SET SALDOCONTA = SALDOCONTA - @SALDOCONTA WHERE CODCLI = #CODCLI#
                                UPDATE CLIENTE_PRE SET SALDOCONTA = SALDOCONTA - @SALDOCONTA WHERE CODCLI = #CODCLI#
                                EXEC PROC_GRAVAR_TRANSACAO #CODCLI#, NULL, 999019, NULL, @SALDOCONTA, NULL, 0, #NUMCARG_VA#, null, '', NULL, NULL
                            END";

            sql = sql.Replace("#CODCLI#", CODCLI.ToString())
                .Replace("#NUMCARG_VA#", NUMCARG_VA.ToString());

            var cmd = new SqlCommand(sql.ToString(), conn) { CommandType = CommandType.Text };

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                throw;
            }

        }

        private void SolicitaCargaLayout7(USUARIO_VA usu, bool atualizaValorCarga)
        {
            var conn = new SqlConnection();
            SqlTransaction trans = null;
            try
            {
                conn.ConnectionString = BDTELENET;
                conn.Open();
                trans = conn.BeginTransaction();

                //Solicito a Carga
                SolicitaCarga(usu, conn, trans, atualizaValorCarga, false);
                //Caso exista centro de custo atualizo a tabela CargaCCusto
                //if (usu.TIPO_LAYOUT == "05" || usu.TIPO_LAYOUT == "06")
                //InsereCentroCusto(usu, conn, trans);

                //Faco o commit                             
                trans.Commit();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch
            {
                //Faco o rollback em tudo
                if (trans != null && trans.Connection != null)
                    trans.Rollback();
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                throw new Exception("Erro ao Solicitar a carga");
            }
        }

        public void CorrigeTabelaCargaCentroCusto(int codcli, int numcargava)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            try
            {
                const string sql =
                    " DELETE C FROM CARGACCUSTO  C WHERE C.CODCLI = @CODCLI AND C.NUMCARG_VA = @NUMCARGA AND C.CENTRO_CUSTO = '' " +
                    " AND NOT EXISTS " +
                    "( SELECT C2.CPF FROM CARGACCUSTO C2 WITH (NOLOCK) " +
                    " WHERE C2.CODCLI = C.CODCLI AND C2.CPF = C.CPF AND C2.NUMCARG_VA = C.NUMCARG_VA AND C2.CENTRO_CUSTO <> '' ) ";
                var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text };
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codcli;
                cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = numcargava;
                conn.Open();
                cmd.ExecuteNonQuery();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        public bool ValidaCPFParamVa()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'VALIDACPF'";
            var cmd = db.GetSqlStringCommand(sql);
            var ret = (Convert.ToString(db.ExecuteScalar(cmd)) == "S" || db.ExecuteScalar(cmd) == null);
            if (cmd.Connection.State == ConnectionState.Open)
                cmd.Connection.Close();
            return ret;
        }

        /// <summary>
        /// Verifica se os cartões podem ser inseridos. Retorna true para sim e false para não.
        /// </summary>
        /// <param name="linhas"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool VerificaCargaArquivoLayout7(List<string> linhas, ref LOG log)
        {
            var retorno = true;
            LOG logErro = null;
            IDataReader dr = null;

            foreach (var linha in linhas)
            {
                var conn = new SqlConnection { ConnectionString = BDTELENET };

                if (linha != linhas[0] && linha != linhas[linhas.Count - 1] && logErro == null)
                {

                    try
                    {
                        conn.Open();
                        var cmd = new SqlCommand("SELECT count(*) Existe from usuariova WITH (NOLOCK) where cpf = '@CPF' and codcli = @CODCLI and codset = '@CODSET'", conn);
                        cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = linha.Substring(17, 11);
                        cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = Convert.ToInt32(linha.Substring(66, 2));
                        cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(linhas[0].Substring(134, 5));
                        cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = linha.Substring(101, 23);

                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            if (Convert.ToInt32(dr["Existe"].ToString()) > 0)
                                retorno = false;

                            if (!retorno)
                                logErro = new LOG("ERRO AO CADASTRAR", "CPF INVÁLIDO", "Já existe um CPF cadastrado com o mesmo setor.");
                        }
                        dr.Close();
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                    catch
                    {
                        retorno = false;
                        if (dr != null)
                            dr.Close();
                        if (conn.State == ConnectionState.Open)
                            conn.Close();

                        return retorno;

                        throw;
                    }
                }

                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            log = logErro;
            return retorno;
        }

        public int SolicitaCargaUsuario(USUARIO_VA usu, LOG log, ref bool erroPorCartao, bool validaCPFTela, int id, int idOper, out int inclusao, string layout = "")
        {
            string cargPadVa;
            inclusao = 0;
            var textoLog = "LINHA ";

            bool sucessoInsercaoCartao = true;

            if (layout != "07")
            {
                #region *** Layout até 6 ***

                if (!VerificaUsuario(usu, out cargPadVa, true)) //Se o usuario nao existe ou esta cancelado
                {
                    if (UtilSIL.ValidarCpf(usu.CPF) || !ValidaCPFParamVa() || !validaCPFTela)
                    {
                        var daUsu = new daUsuarioVA(daOperadora);
                        var aux = new blUsuarioVA(daOperadora);

                        daUsu.ExcluirUsuCarga(usu);
                        daUsu.InserirUsuario(usu);//Insiro na base

                        if (usu.TOTALCARGAUSUARIO > usu.CARGPADVACLIENTE && sucessoInsercaoCartao)
                        {
                            log.AddLog(new LOG(textoLog + Convert.ToString(id).PadLeft(5, '0'),
                                               "CLIENTE " + usu.CODCLI.PadLeft(5, '0') + " CPF " + usu.CPF,
                                               "CARTAO INCLUIDO. LIMITE DE CARGA ULTRAPASSADO PARA O CARTAO."));
                            erroPorCartao = true;
                        }
                        else if (usu.TOTALCARGAUSUARIO <= usu.CARGPADVACLIENTE && sucessoInsercaoCartao)
                        {
                            //Solicito a carga neste momento
                            SolicitaCarga(usu, false);

                            log.AddLog(new LOG(textoLog + Convert.ToString(id).PadLeft(5, '0'),
                                               "CLIENTE " + usu.CODCLI.PadLeft(5, '0') + " CPF " + usu.CPF,
                                               "CARTAO INCLUIDO - PROCESSO REALIZADO COM SUCESSO."));
                        }
                        inclusao = 1;
                    }
                    else
                    {
                        log.AddLog(new LOG(textoLog + Convert.ToString(id).PadLeft(5, '0'),
                                           "CLIENTE " + usu.CODCLI.PadLeft(5, '0') + " CPF " + usu.CPF,
                                           "PROCESSO NAO REALIZADO. CPF INVALIDO"));
                        erroPorCartao = true;
                    }


                }
                else
                {
                    decimal maxCardPadVa;
                    decimal.TryParse(cargPadVa, out maxCardPadVa);

                    if (usu.TOTALCARGAUSUARIO > maxCardPadVa && usu.TOTALCARGAUSUARIO > usu.CARGPADVACLIENTE)
                    {
                        log.AddLog(new LOG(textoLog + Convert.ToString(id).PadLeft(5, '0'),
                                           "CLIENTE " + usu.CODCLI.PadLeft(5, '0') + " CPF " + usu.CPF,
                                           "PROCESSO NAO REALIZADO. LIMITE DE CARGA ULTRAPASSADO PARA O CARTAO."));
                        erroPorCartao = true;
                    }
                    else
                    {
                        if (usu.STA != "00") //Usuario existe e nao esta ativo (Gero log de erro de status)
                        {
                            log.AddLog(new LOG(textoLog + Convert.ToString(id).PadLeft(5, '0'),
                                               "CLIENTE " + usu.CODCLI.PadLeft(5, '0') + " CPF " + usu.CPF,
                                               "PROCESSO NAO REALIZADO. STATUS INVALIDO. CARTAO = " + usu.STASTR));
                            erroPorCartao = true;
                        }
                        else
                        {
                            var retorno = string.Empty;
                            SolicitaCarga(usu, usu.TOTALCARGAUSUARIO > maxCardPadVa);
                            if (usu.TOTALCARGAUSUARIO > usu.CARGPADVACLIENTE)
                                retorno = " VALOR MAIOR QUE A CARGA PADRÃO DO CLIENTE.";
                            //SolicitaCarga(usu, usu.TOTALCARGAUSUARIO > maxCardPadVa);//Solicito a carga neste momento                                                    
                            log.AddLog(new LOG(textoLog + Convert.ToString(id).PadLeft(5, '0'),
                                               "CLIENTE " + usu.CODCLI.PadLeft(5, '0') + " CPF " + usu.CPF,
                                               "PROCESSO REALIZADO COM SUCESSO." + retorno));
                        }
                    }
                }
                #endregion
            }
            return id + 1;
        }

        public List<string> ValidaLayoutArquivoCarga(string nomeArquivo, string caminhoArquivo)
        {
            //Transacoes para o NetCard
            var conn = new SqlConnection();

            try
            {
                #region *** Consulta no banco ***
                string erroe = "";
                string textoe = "";

                var cmd = new SqlCommand("VALIDA_LAYOUT_ARQ_CARGA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add("@DIRETORIO", SqlDbType.VarChar).Value = caminhoArquivo;
                cmd.Parameters.Add("@ARQUIVO", SqlDbType.VarChar).Value = nomeArquivo;
                cmd.Parameters.Add("@ERRO", SqlDbType.VarChar).Value = erroe;
                cmd.Parameters.Add("@TEXTO", SqlDbType.VarChar).Value = textoe;

                conn.ConnectionString = BDTELENET;
                conn.Open();

                DataTable dtValidaLayout = new DataTable();
                dtValidaLayout.Load(cmd.ExecuteReader());
                //conn.Close(); 
                #endregion

                List<string> linhasRetorno = new List<string>();
                List<string> linhasValidaLayout = new List<string>();
                List<string> linhasValidaDados = new List<string>();

                //Passa o resultado da procedure para uma lista
                foreach (DataRow row in dtValidaLayout.Rows)
                {
                    linhasValidaLayout.Add(row[0].ToString());
                }

                //Preenche os valores de cada linha do retorno
                string rOperacao, rData, rNomeTabela, rCliente, rCnpj, rNumeroCarga, rValorCarga, rRegistrosValidos, rRegistroInvalidos, rValorCarrega;

                rOperacao = linhasValidaLayout[0].Substring(26, linhasValidaLayout[0].Length - 26);             // OPERAÇÃO                                                           
                rData = linhasValidaLayout[1].Substring(26, linhasValidaLayout[1].Length - 26);                 // DATA PROGRAMAÇÃO
                rNomeTabela = linhasValidaLayout[2].Substring(26, linhasValidaLayout[2].Length - 26);           // NOME DA TABELA
                rCliente = linhasValidaLayout[3].Substring(26, linhasValidaLayout[3].Length - 26);              // CODIGO DO CLIENTE
                rCnpj = linhasValidaLayout[4].Substring(26, linhasValidaLayout[4].Length - 26);                 // CNPJ DO CLIENTE
                rNumeroCarga = linhasValidaLayout[5].Substring(26, linhasValidaLayout[5].Length - 26);          // NÚMERO DA CARGA
                rValorCarga = linhasValidaLayout[6].Substring(26, linhasValidaLayout[6].Length - 26);           // VALOR DA CARGA DO TRAILER
                rRegistrosValidos = linhasValidaLayout[7].Substring(26, linhasValidaLayout[7].Length - 26);     // NÚMERO DE REGISTROS VÁLIDOS
                rRegistroInvalidos = linhasValidaLayout[8].Substring(26, linhasValidaLayout[8].Length - 26);    // NÚMERO DE REGISTROS INVÁLIDOS
                rValorCarrega = linhasValidaLayout[9].Substring(26, linhasValidaLayout[9].Length - 26);         // VALOR QUE SERÁ EFETUADO DE CARGA

                if (rOperacao.Equals("ARQUIVO VÁLIDO"))
                {
                    #region *** Consulta no banco ***
                    cmd = new SqlCommand("VERIFICA_DADOS_PARA_SOLICITAR_CARGA", conn) { CommandType = CommandType.StoredProcedure };
                    //conn.Open();

                    DataTable dtVerificaDados = new DataTable();
                    cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(rCliente);
                    cmd.Parameters.Add("@NUM_CARGA", SqlDbType.Int).Value = Convert.ToInt32(rNumeroCarga);
                    cmd.Parameters.Add("@VALOR_DA_CARGA", SqlDbType.Decimal).Value = Convert.ToDecimal(rValorCarga.Replace(".", ","));
                    cmd.Parameters.Add("@DT_PROG", SqlDbType.DateTime).Value = rData;
                    cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar).Value = rCnpj;
                    cmd.Parameters.Add("@NOME_TABLE", SqlDbType.VarChar).Value = rNomeTabela;

                    dtVerificaDados.Load(cmd.ExecuteReader());
                    conn.Close();
                    #endregion

                    foreach (DataRow row in dtVerificaDados.Rows)
                    {
                        linhasValidaDados.Add(row[0].ToString());
                    }

                    linhasRetorno = linhasValidaDados;

                }
                else
                {
                    linhasRetorno = linhasValidaLayout;
                }

                return linhasRetorno;
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                throw;
            }
        }

        public List<string> ConfirmaCargaArquivo(string nomeTabela, string codigoCliente, string numCarga, string valorCarga, string dataProg, string cnpj, string idOperador, string codigoOperadora)
        {
            var conn = new SqlConnection();

            List<string> linhasRetorno = new List<string>();

            #region *** Consulta no banco ***

            var cmd = new SqlCommand("SOLICITA_CARGA", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codigoCliente;
            cmd.Parameters.Add("@NUM_CARGA", SqlDbType.Int).Value = numCarga;
            cmd.Parameters.Add("@VALOR_DA_CARGA", SqlDbType.Decimal).Value = valorCarga;
            cmd.Parameters.Add("@DTPROG", SqlDbType.DateTime).Value = dataProg;
            cmd.Parameters.Add("@CNPJ", SqlDbType.VarChar).Value = cnpj;
            cmd.Parameters.Add("@IDOPER", SqlDbType.VarChar).Value = idOperador;
            cmd.Parameters.Add("@ARQ", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = codigoOperadora;
            cmd.Parameters.Add("@NOME_TABLE", SqlDbType.VarChar).Value = nomeTabela;

            conn.ConnectionString = BDTELENET;
            conn.Open();

            DataTable dtEfetuaCarga = new DataTable();
            dtEfetuaCarga.Load(cmd.ExecuteReader());
            conn.Close();

            foreach (DataRow row in dtEfetuaCarga.Rows)
            {
                linhasRetorno.Add(row[0].ToString());
            }

            #endregion

            return linhasRetorno;
        }

        public List<string> CancelaCargaArquivo(string nomeTabela)
        {
            var conn = new SqlConnection();

            List<string> linhasRetorno = new List<string>();

            #region *** Consulta no banco ***

            var cmd = new SqlCommand(string.Format("IF object_id('{0}') IS NOT NULL DROP TABLE {0}", nomeTabela), conn) { CommandType = CommandType.Text };

            conn.ConnectionString = BDTELENET;
            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();

            linhasRetorno.Add("OPERAÇÃO                : CARGA CANCELADA");
            linhasRetorno.Add("DATA                    : " + DateTime.Now.ToString());

            #endregion

            return linhasRetorno;
        }


        public bool VerificaUsuarioPJ(USUARIO_VA usuario)
        {
            var sta = string.Empty;
            var cpf = string.Empty;
            var mat = string.Empty;
            var set = string.Empty;

            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var cmd =
                new SqlCommand(
                    "SELECT S.STA, U.CPF, U.MAT, U.CODSET FROM STATUS S WITH (NOLOCK) INNER JOIN USUARIO U WITH (NOLOCK) ON U.STA = S.STA " +
                    "WHERE U.CODCLI = @CODCLI AND U.CPF = @CPF AND U.NUMDEP = 0",
                    conn) { CommandType = CommandType.Text };
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
            IDataReader dr = null;

            try
            {
                conn.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    sta = dr["STA"].ToString();
                    cpf = dr["CPF"].ToString();
                    mat = dr["MAT"].ToString();
                    set = dr["CODSET"].ToString();
                }
                dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                //Usuario nao existe
                if (cpf == string.Empty)
                    return false;

                //Mantenho o status do usuario vindo do banco
                usuario.STA = sta;

                //Ser a matricula do arquivo for vazia, mantenho a do banco
                if (string.IsNullOrEmpty(usuario.MAT))
                    usuario.MAT = mat;

                //Ser o setor do arquivo for vazio, mantenho a do banco
                if (string.IsNullOrEmpty(usuario.CODSET))
                    usuario.CODSET = set;

                return true;
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        public void TransferenciaSaldo(int tipoTransf, int codCli, string cpfOrig, string cpfDest,
            string crtOrig, string crtDest, decimal valor, int idFunc, string bancoAut)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };

            try
            {
                conn.Open();
                var cmd = new SqlCommand("PROC_TRANSF_SALDO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add("@TIPO", SqlDbType.Int).Value = Convert.ToInt32(tipoTransf);
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(codCli);
                cmd.Parameters.Add("@CPF_ORIG", SqlDbType.VarChar).Value = cpfOrig;
                cmd.Parameters.Add("@CPF_DEST", SqlDbType.VarChar).Value = cpfDest;
                cmd.Parameters.Add("@CRT_ORIG", SqlDbType.VarChar).Value = crtOrig;
                cmd.Parameters.Add("@CRT_DEST", SqlDbType.VarChar).Value = crtDest;
                cmd.Parameters.Add("@VALOR", SqlDbType.Decimal).Value = valor;
                cmd.Parameters.Add("@BANCO", SqlDbType.VarChar).Value = bancoAut;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = idFunc;
                cmd.ExecuteNonQuery();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                throw;
            }
        }

        public void TransferenciaUsuario(int codCliOrigem, int codCliDestino, string cpfOrigem, int idFunc, out string retorno, out string mensagem)
        {
            string ret = "";
            string msg = "";

            var conn = new SqlConnection { ConnectionString = BDTELENET };

            var cmd = new SqlCommand("PROC_TRANSFERE_USUARIO", conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.Add("@CODCLIANT", SqlDbType.Int).Value = Convert.ToInt32(codCliOrigem);
            cmd.Parameters.Add("@CODCLINOVO", SqlDbType.Int).Value = Convert.ToInt32(codCliDestino);
            cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpfOrigem;
            cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = idFunc;

            try
            {
                conn.Open();

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                if (dt.Rows.Count > 0)
                {
                    ret = dt.Rows[0]["RETORNO"].ToString();
                    //msg = dt.Rows[0]["MENSAGEM"].ToString();

                }

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                retorno = ret;
                mensagem = msg;
            }
            catch
            {

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                throw;
            }

        }

        public bool SolicitaCargaUsuario(int codCli, int numSeq, DateTime dtAutoriz)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };
            var retorno = false;
            var cmd = new SqlCommand("SP_AUTORIZA_CARGACCUSTO", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = Convert.ToInt32(codCli);
            cmd.Parameters.Add("@NUMCARGA", SqlDbType.Int).Value = Convert.ToInt32(numSeq);
            cmd.Parameters.Add("@DTAUTORIZ", SqlDbType.VarChar).Value = dtAutoriz.ToString("yyyyMMdd");
            IDataReader dr = null;
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


        #region *** Novo Processo Carga ***

        public void VerificaProcessamentoCargaBarraProgresso(string login, string codope, out string nivel, out string estado)
        {
            SqlConnection conexaoConsultaSemaforoBarraProgresso;
            SqlCommand comandoConsultaSemaforoBarraProgresso;

            conexaoConsultaSemaforoBarraProgresso = new SqlConnection(BDTELENET);

            conexaoConsultaSemaforoBarraProgresso.Open();
            comandoConsultaSemaforoBarraProgresso = new SqlCommand("CARGA_CONSULTA_SEMAFORO", conexaoConsultaSemaforoBarraProgresso) { CommandType = CommandType.StoredProcedure };

            comandoConsultaSemaforoBarraProgresso.Parameters.Add("@LOGINOPE", SqlDbType.VarChar).Value = login;
            comandoConsultaSemaforoBarraProgresso.Parameters.Add("@CODOPE", SqlDbType.Int).Value = codope;

            comandoConsultaSemaforoBarraProgresso.Parameters.Add(new SqlParameter("@ESTADO", SqlDbType.Int));
            comandoConsultaSemaforoBarraProgresso.Parameters["@ESTADO"].Direction = ParameterDirection.Output;

            comandoConsultaSemaforoBarraProgresso.Parameters.Add(new SqlParameter("@NIVEL", SqlDbType.Int));
            comandoConsultaSemaforoBarraProgresso.Parameters["@NIVEL"].Direction = ParameterDirection.Output;

            comandoConsultaSemaforoBarraProgresso.ExecuteNonQuery();

            estado = comandoConsultaSemaforoBarraProgresso.Parameters["@ESTADO"].Value.ToString();
            nivel = comandoConsultaSemaforoBarraProgresso.Parameters["@NIVEL"].Value.ToString();

            conexaoConsultaSemaforoBarraProgresso.Close();
        }

        public bool VerificaProcessamentoCarga(string paramCodigoOperador, string paramLoginOperador, string paramCodigoCliente)
        {
            var conn = new SqlConnection { ConnectionString = BDTELENET };

            try
            {
                string sql = string.Format("SELECT ID FROM CARGA_CTRL_TABS WITH (NOLOCK) WHERE CODOPE = {0} AND LOGINOPE = '{1}' AND CLIENTE_ORIGEM = {2} AND ORIGEM = 'MW' AND BAIXOU_LOG IS NULL", paramCodigoOperador, paramLoginOperador, paramCodigoCliente);

                var cmd = new SqlCommand(sql, conn) { CommandType = CommandType.Text };

                DataTable dt = new DataTable();

                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                return dt.Rows.Count > 0;
            }
            catch 
            {
                throw;
            }
        }

        public void VerificaProcessamentoCargaBarraProgresso(string paramCodigoOperador, string paramLoginOperador, string paramCodigoCliente, out string paramNivel, out string paramEstado)
        {
            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand("CARGA_CONSULTA_SEMAFORO", conexao) { CommandType = CommandType.StoredProcedure };

            try
            {
                comando.Parameters.Add("@LOGINOPE", SqlDbType.VarChar).Value = paramLoginOperador;
                comando.Parameters.Add("@CODOPE", SqlDbType.Int).Value = paramCodigoOperador;

                comando.Parameters.Add(new SqlParameter("@ESTADO", SqlDbType.Int));
                comando.Parameters["@ESTADO"].Direction = ParameterDirection.Output;

                comando.Parameters.Add(new SqlParameter("@NIVEL", SqlDbType.Int));
                comando.Parameters["@NIVEL"].Direction = ParameterDirection.Output;

                conexao.Open();

                comando.ExecuteNonQuery();

                conexao.Close();

                paramEstado = comando.Parameters["@ESTADO"].Value.ToString();
                paramNivel = comando.Parameters["@NIVEL"].Value.ToString();

            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                paramNivel = "";
                paramEstado = "";
            }
        }

        public CARGA_CTRL_TABS BuscarInformacoesCarga(string paramCodigoOperador, string paramLoginOperador, string paramCodigoCliente)
        {
            string sql = @"SELECT TOP 1 * FROM CARGA_CTRL_TABS WITH (NOLOCK) WHERE LOGINOPE = '" + paramLoginOperador + "' AND CODOPE = " + paramCodigoOperador + " AND CLIENTE_ORIGEM = " + paramCodigoCliente + " AND BAIXOU_LOG IS NULL AND ORIGEM = 'MW'";

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.Text };

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                CARGA_CTRL_TABS retorno = new CARGA_CTRL_TABS();

                if (dt.Rows.Count > 0)
                {
                    retorno.ID = dt.Rows[0]["ID"].ToString();
                    retorno.CODCLI = dt.Rows[0]["CODCLI"].ToString();
                    retorno.CLIENTE_ORIGEM = dt.Rows[0]["CLIENTE_ORIGEM"].ToString();
                    retorno.DT_CRIACAO = dt.Rows[0]["DT_CRIACAO"].ToString();
                    retorno.NUM_CARGA = dt.Rows[0]["NUM_CARGA"].ToString();
                    retorno.DT_PROG = dt.Rows[0]["DT_PROG"].ToString();
                    retorno.TOT_REGS = dt.Rows[0]["TOT_REGS"].ToString();
                    retorno.VALOR_CARGA = dt.Rows[0]["VALOR_CARGA"].ToString();
                    retorno.NOME_TABELA = dt.Rows[0]["NOME_TABELA"].ToString();
                    retorno.CNPJ = dt.Rows[0]["CNPJ"].ToString();
                    retorno.VALOR_A_CARREGAR = dt.Rows[0]["VALOR_A_CARREGAR"].ToString();
                    retorno.TOT_INVALIDOS = dt.Rows[0]["TOT_INVALIDOS"].ToString();
                    retorno.RESULTADO = dt.Rows[0]["RESULTADO"].ToString();
                    retorno.DT_CARGA = dt.Rows[0]["DT_CARGA"].ToString();
                    retorno.TIPO = dt.Rows[0]["TIPO"].ToString();
                    retorno.LOGINOPE = dt.Rows[0]["LOGINOPE"].ToString();
                    retorno.CODOPE = dt.Rows[0]["CODOPE"].ToString();
                    retorno.NOM_ORIGINAL_ARQUIVO = dt.Rows[0]["NOM_ORIGINAL_ARQUIVO"].ToString();
                    retorno.CAM_COMP_ARQ = dt.Rows[0]["CAM_COMP_ARQ"].ToString();
                    retorno.ERRO_PROC = dt.Rows[0]["ERRO_PROC"].ToString();
                    retorno.STATUS_PROC = dt.Rows[0]["STATUS_PROC"].ToString();
                    retorno.PROC_COMP = dt.Rows[0]["PROC_COMP"].ToString();
                    retorno.BAIXOU_LOG = dt.Rows[0]["BAIXOU_LOG"].ToString();
                    retorno.ORIGEM = dt.Rows[0]["ORIGEM"].ToString();
                }

                return retorno;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                throw;
            }
        }

        public List<CARGA_CTRL_TABS_RESUMO> BuscarInformacoesResumoCarga(string paramId)
        {
            string sql = @"SELECT * FROM CARGA_CTRL_TABS_RESUMO WITH (NOLOCK) WHERE ID = '" + paramId + "'";

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.Text };

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                List<CARGA_CTRL_TABS_RESUMO> retorno = new List<CARGA_CTRL_TABS_RESUMO>();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        retorno.Add(new CARGA_CTRL_TABS_RESUMO()
                        {
                            ID = item["CODCLI"].ToString(),
                            CODCLI = item["CODCLI"].ToString(),
                            NUM_CARGA = item["NUM_CARGA"].ToString(),
                            REGISTRO_LOG = item["REGISTRO_LOG"].ToString(),
                            TIPO_REG = item["TIPO_REG"].ToString()
                        });
                    }                    
                }

                return retorno;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                throw;
            }
        }

        public bool BuscarInformacaoSolicitacaoCarga(string paramId)
        {
            string retorno = "";
            string sql = "CARGA_VERIFICA_AUTORIZACAO";

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.StoredProcedure };

            try
            {
                DataTable dt = new DataTable();

                comando.Parameters.Add(new SqlParameter(){ Value = paramId, DbType = DbType.String, ParameterName = "@ID"});

                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();


                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["RETORNO"].ToString();
                }

                return retorno.Equals("true");
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                throw;
            }
        }

        public bool AlterarProcessamentoCarga(string paramSql)
        {
            string sql = paramSql;

            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand(sql, conexao) { CommandType = CommandType.Text };

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                int linhas = comando.ExecuteNonQuery();
                conexao.Close();

                return linhas > 0;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                throw;
            }
        }

        public bool DeletarDadosCarga(string paramLoginOpe, string paramCodope, string paramClienteOrigem)
        {
            SqlConnection conexao = new SqlConnection(BDTELENET);
            SqlCommand comando = new SqlCommand("CARGA_NEGA_SOLICITACAO", conexao) { CommandType = CommandType.StoredProcedure };

            comando.Parameters.Add(new SqlParameter() { ParameterName = "@LOGINOPE", Value = paramLoginOpe, DbType = DbType.String });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CODOPE", Value = paramCodope, DbType = DbType.Int32 });
            comando.Parameters.Add(new SqlParameter() { ParameterName = "@CLIENTE_ORIGEM", Value = paramClienteOrigem, DbType = DbType.Int32 });

            try
            {
                DataTable dt = new DataTable();

                conexao.Open();
                int linhas = comando.ExecuteNonQuery();
                conexao.Close();

                return linhas > 0;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                throw;
            }
        }

        #endregion

    }
}
