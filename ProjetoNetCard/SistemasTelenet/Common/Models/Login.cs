using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NetCard.Common.Util;
using SIL.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace NetCard.Common.Models
{
    public class Login
    {
        public string Acesso { get; set; }
        public int Tipo { get; set; }
        public bool AlterarSenha { get; set; }
        public bool RecuperarSenha { get; set; }
        public TipoAcesso TipoDeAcesso { get; set; }
        public string Cpf { get; set; }
        public bool UrlCompleta { get; set; }
        public string Operadora { get; set; }

        [Display(Name = "Código Cliente")]
        public int Codigo { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string LogIn { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha de acesso web")]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha (Mínimo de 6 caracteres)")]
        public string NovaSenha { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma Nova Senha")]
        public string ConfNovaSenha { get; set; }

        public int tamanhoSenha { get; set; }

        public bool EPrimeiroAcesso(ObjConn objConexao, string numeroCartao, out string mensagem)
        {
            mensagem = string.Empty;
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = @"
                SELECT CASE WHEN (DTSENHA IS NULL) THEN 'CPF' ELSE 'SENHA' END AS TIPOACESSO, 
                CASE WHEN (ALTERASENHA IS NULL) THEN 'NAOALTERASENHA' ELSE 'ALTERASENHA' END AS ALTERASENHA, 
                STA AS STATUS, CODCLI
                FROM (
                SELECT CODCRT, DTSENHA, STA, (SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'MW_FORC_ALT_SEN') AS ALTERASENHA, CODCLI FROM USUARIOVA WITH (NOLOCK) 
                UNION
                SELECT CODCRT, DTSENHA, STA, (SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'MW_FORC_ALT_SEN') AS ALTERASENHA, CODCLI FROM USUARIO WITH (NOLOCK)) USUARIOS 
                WHERE CODCRT = @CODCRT";

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCRT", DbType.String, numeroCartao);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (dr.Read())
                {
                    var tipoAcesso = Convert.ToString(dr["TIPOACESSO"]);
                    var alteraSenha = Convert.ToString(dr["ALTERASENHA"]);
                    var status = Convert.ToInt32(dr["STATUS"]);
                    var codCli = Convert.ToInt32(dr["CODCLI"]);
                    if (status == 1 || status == 2)
                    {
                        mensagem = status == 1 ? "CARTAO BLOQUEADO" : "CARTAO CANCELADO";
                        return false;
                    }
                    mensagem = tipoAcesso == "SENHA" ? "Senha de Acesso WEB" : alteraSenha != "ALTERASENHA" ? tipoAcesso : "ALTERASENHA";

                    if (codCli >= 2737 && codCli <= 2747)
                    {
                        mensagem = "Cliente_Rio";
                    }
                }
                else
                {
                    dr.Close();
                    return ConsultaCartaoAutorizador(objConexao, numeroCartao, out mensagem);
                }
                return true;
            }
            catch
            {
                mensagem = "Ocorreu uma falha na consulta";
                return false;
            }
            finally
            {
                if (dr != null) dr.Close();
            }
        }

        public bool ConsultaCartaoAutorizador(ObjConn objConexao, string numeroCartao, out string mensagem)
        {
            mensagem = string.Empty;

            Database db = new SqlDatabase(Utils.GetConnectionStringAutorizador(objConexao));
            var sql = @"SELECT DTVALCART 
                FROM CTCARTAO WITH (NOLOCK) WHERE
                CODCARTAO = @PARCARTAO and DTVALCART >= " + DateTime.Now.ToShortDateString();

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@PARCARTAO", DbType.String, numeroCartao);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                if (!dr.Read())
                {
                    mensagem = "CARTAO NAO LOCALIZADO";
                    dr.Close();
                    return false;
                }
                else
                {
                    dr.Close();
                    return true;
                }
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                mensagem = "Ocorreu uma falha na consulta";
                return false;
            }
        }

        public bool Valido(ObjConn objConexao, out string retorno, out string acao, out IList<DadosAcesso> loginsAcesso)
        {
            var result = LoginValidateFactory.CreateValidator(objConexao, Acesso).ValidateLogin(this);
            retorno = result.Mensagem;
            acao = result.Acao;
            loginsAcesso = result.DadosAcesso;
            return result.Resultado;
        }

        public string AlterarSenhaUsu(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            Senha = Senha.Length > 8 ? Senha.Substring(0, 8) : Senha;
            NovaSenha = NovaSenha.Length > 8 ? NovaSenha.Substring(0, 8) : NovaSenha;

            string retorno;

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "MW_ALTERASENHA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.String, Convert.ToString(dadosAcesso.Sistema.cartaoPJVA)),
                               new Parametro("@ACESSO", DbType.String, dadosAcesso.Acesso),
                               new Parametro("@CNPJ", DbType.String, dadosAcesso.Cnpj),
                               new Parametro("@TIPOACESSO", DbType.String, dadosAcesso.TipoAcesso),
                               new Parametro("@CODPARCERIA", DbType.Int32, dadosAcesso.CodParceria),
                               new Parametro("@LOGIN", DbType.String, LogIn),
                               new Parametro("@SENHAANT", DbType.String, Senha),
                               new Parametro("@NOVASENHA", DbType.String, NovaSenha)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                retorno = dr.Read() ? dr["RETORNO"].ToString() : "Não foi possível realizar a operação, entre em contato com a operadora.";
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                retorno = "Falha no processo de alterar senha.";
            }
            return retorno;
        }

        public string AlterarSenhaCli(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            Senha = Senha.Length > 8 ? Senha.Substring(0, 8) : Senha;
            NovaSenha = NovaSenha.Length > 8 ? NovaSenha.Substring(0, 8) : NovaSenha;

            string retorno;

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "MW_ALTERASENHA";
            var cmd = db.GetStoredProcCommand(sql.ToString());
            var list = new List<Parametro>
                           {
                               new Parametro("@SISTEMA", DbType.String, Convert.ToString(dadosAcesso.Sistema.cartaoPJVA)),
                               new Parametro("@ACESSO", DbType.String, dadosAcesso.Acesso),
                               new Parametro("@CNPJ", DbType.String, dadosAcesso.Cnpj),
                               new Parametro("@TIPOACESSO", DbType.String, dadosAcesso.TipoAcesso),
                               new Parametro("@CODPARCERIA", DbType.Int32, dadosAcesso.CodParceria),
                               new Parametro("@LOGIN", DbType.String, LogIn),
                               new Parametro("@SENHAANT", DbType.String, Senha),
                               new Parametro("@NOVASENHA", DbType.String, NovaSenha)
                           };
            foreach (var parametro in list)
                db.AddInParameter(cmd, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

            IDataReader dr = null;
            try
            {
                dr = db.ExecuteReader(cmd);
                retorno = dr.Read() ? dr["RETORNO"].ToString() : "Não foi possível realizar a operação, entre em contato com a operadora.";
                dr.Close();
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                retorno = "Falha no processo de alterar senha.";
            }
            return retorno;
        }

        public string AlterarSenhaUsuCartao(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            Senha = Senha.Length > 8 ? Senha.Substring(0, 8) : Senha.PadRight(8, '0');
            Senha = BlCriptografia.Encrypt(Senha.Substring(0, 8));
            NovaSenha = NovaSenha.Length > 8 ? NovaSenha.Substring(0, 8) : NovaSenha.PadRight(8, '0');
            NovaSenha = BlCriptografia.Encrypt(NovaSenha.Substring(0, 8));
            string retorno;

            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "MW_USU_ALTERASENHA_CARTAO";
            var cmd = db.GetStoredProcCommand(sql.ToString());

            db.AddInParameter(cmd, "@SISTEMA", DbType.Int16, dadosAcesso.Sistema.cartaoPJVA);
            db.AddInParameter(cmd, "@LOGIN", DbType.String, dadosAcesso.CodCrt);
            db.AddInParameter(cmd, "@SENHAANT", DbType.String, Senha);
            db.AddInParameter(cmd, "@NOVASENHA", DbType.String, NovaSenha);

            IDataReader dr = null;

            try
            {
                dr = db.ExecuteReader(cmd);
                retorno = dr.Read() ? dr["RETORNO"].ToString() : "A operação de troca de senha não pode ser realizada. Entre em contato com a operadora do cartão.";
                dr.Close();

                //if (retorno.ToUpper() == Constantes.ok)
                //{
                //    #region *** Atualiza Senha Cielo ***

                //    Database dbCielo = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
                //    const string sqlCielo = "PROC_ATUALIZA_PWCIELO";

                //    var cmdCielo = dbCielo.GetStoredProcCommand(sqlCielo.ToString());

                //    var listCielo = new List<Parametro>
                //                    {
                //                        new Parametro("@SISTEMA", DbType.Int32, dadosAcesso.Sistema.cartaoPJVA),
                //                        new Parametro("@CODCLI", DbType.Int32, dadosAcesso.Codigo),
                //                        new Parametro("@CODCRT ", DbType.String, dadosAcesso.CodCrt)
                //                    };

                //    foreach (var parametro in listCielo)
                //        dbCielo.AddInParameter(cmdCielo, parametro.Campo, parametro.Tipo, parametro.Valor ?? DBNull.Value);

                //    try
                //    {
                //        DataTable dtCielo = new DataTable();

                //        dtCielo.Load(dbCielo.ExecuteReader(cmdCielo));
                //    }
                //    catch (Exception ex)
                //    {
                //        string sistemaUsu = dadosAcesso.Sistema.cartaoPJVA == 0 ? SIL.Email.Sistema.ConsultaWebPj : SIL.Email.Sistema.ConsultaWebVa;

                //        SIL.Email.EnviarEmailErro(
                //            operadora: dadosAcesso.RazaoOpe,
                //            sistema: sistemaUsu,
                //            arquivoErro: "Login.cs",
                //            metodoErro: "AlterarSenhaUsuCartao",
                //            descricaoErro: ex.Message,
                //            mensagem: "Erro ao atualizar senha Cielo na emissão de 2 ª via."
                //        );
                //    }

                //    #endregion
                //}
            }
            catch
            {
                if (dr != null)
                    dr.Close();
                retorno = "Falha no processo de alterar senha.";
            }
            return retorno;
        }

        public bool RenovarAcessoUsu(ObjConn objConexao)
        {
            bool retorno = true;
            string SENHA = BlCriptografia.Encrypt(Cpf.Substring(0, 8));

            string sql = @"UPDATE USUARIOVA SET DTEXPSENHA = DATEADD(day, (SELECT CONVERT(INT,VAL) FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'PRZ_SENHA_INIC'), GETDATE()), SENUSU = '" + SENHA + @"', DTSENHA = NULL, QTDEACESSOINV = 0 WHERE CPF = '{0}' 
                             UPDATE USUARIO SET DTEXPSENHA = DATEADD(day, (SELECT CONVERT(INT,VAL) FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'PRZ_SENHA_INIC'), GETDATE()), SENUSU = '" + SENHA + @"', DTSENHA = NULL, QTDEACESSOINV = 0 WHERE CPF = '{0}'";

            string query = string.Format(sql, Cpf);

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));
            SqlCommand comando = new SqlCommand(query, conexao);

            try
            {
                conexao.Open();
                int linhasAfetadas = comando.ExecuteNonQuery();
                conexao.Close();

                if (linhasAfetadas == 0)
                    retorno = false;

            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();

                return false;
                throw;
            }

            return retorno;

        }

        public string VerificaSolicitacaoCadastroUsuario(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            var retorno = string.Empty;
            string tabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "PARAM" : "PARAMVA";
            string sql = @"SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'SOL_CAD_USU_MW'";
            string query = string.Format(sql, Cpf);

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));
            SqlCommand comando = new SqlCommand(query, conexao);

            try
            {
                DataTable dt = new DataTable();
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno = Convert.ToString(dt.Rows[0]["VAL"]);
                }
                return retorno;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();
                return retorno;
            }
        }

        public string VerificaFraseSecObrigatoria(ObjConn objConexao, DadosAcesso dadosAcesso)
        {
            var retorno = string.Empty;
            string tabela = dadosAcesso.Sistema.cartaoPJVA == 0 ? "PARAM" : "PARAMVA";
            string sql = @"SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'FRASE_SEC_OBRIG'";
            string query = string.Format(sql, Cpf);

            SqlConnection conexao = new SqlConnection(Utils.GetConnectionStringNerCard(objConexao));
            SqlCommand comando = new SqlCommand(query, conexao);

            try
            {
                DataTable dt = new DataTable();
                conexao.Open();
                dt.Load(comando.ExecuteReader());
                conexao.Close();

                if (dt.Rows.Count > 0)
                {
                    retorno = Convert.ToString(dt.Rows[0]["VAL"]);
                }
                return retorno;
            }
            catch (Exception)
            {
                if (conexao.State != ConnectionState.Closed)
                    conexao.Close();
                return retorno;
            }
        }

        public bool UsaTermoConsentimento(ObjConn objConexao)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'USA_TERMOCONSEN'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public int TamanhoSenhaCartao(ObjConn objConexao, int sistema)
        {
            Database db = new SqlDatabase(Utils.GetConnectionStringNerCard(objConexao));
            var tabela = sistema == 0 ? "PARAM" : "PARAMVA";
            var sql = "SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'TAMSENHA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd)) <= 4 ? 4 : Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public enum TipoAcesso
        {
            Cliente = 1,
            Usuario = 2,
            Credenciado = 3,
            Parceria = 4
        }
    }
}
