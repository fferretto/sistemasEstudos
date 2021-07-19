using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;
using SIL.BLL;

namespace TELENET.SIL.DA
{
    class daVopervaws
    {
        readonly string BDTELENET = string.Empty;
        readonly string BDCONCENTRADOR = string.Empty;
        readonly OPERADORA FOperador;

        public daVopervaws(OPERADORA Operador)
        {
            FOperador = Operador;            
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, Operador.SERVIDORCON, Operador.BANCOCON, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        public List<VOPERVAWS_CONSULTA> ColecaoVopervawsConsulta(string Filtro)
        {
            var ColecaoVopervaws = new List<VOPERVAWS_CONSULTA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine(" SELECT ID_FUNC, LOGIN, NOME, IDPERFIL, DESCRICAO, DETALHAMENTO, OPERVAWS.STA,STATUS.DESTA ");
            sql.AppendLine("   FROM OPERVAWS WITH (NOLOCK) INNER JOIN ");
            sql.AppendLine("        PERFILACESSOVA WITH (NOLOCK) ON OPERVAWS.IDPERFIL = PERFILACESSOVA.ID ");
            sql.AppendLine(" INNER JOIN STATUS WITH (NOLOCK) ON STATUS.STA = OPERVAWS.STA ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOME ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var Vopervaws = new VOPERVAWS_CONSULTA();
                Vopervaws.ID_FUNC = Convert.ToInt32(idr["ID_FUNC"]);
                Vopervaws.LOGIN = Convert.ToString(idr["LOGIN"]);
                Vopervaws.NOME = Convert.ToString(idr["NOME"]);
                Vopervaws.IDPERFIL = Convert.ToInt32(idr["IDPERFIL"]);
                Vopervaws.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                Vopervaws.DETALHAMENTO = Convert.ToString(idr["DETALHAMENTO"]);
                Vopervaws.STA = idr["STA"].ToString();
                Vopervaws.DESTA = idr["DESTA"].ToString();
                ColecaoVopervaws.Add(Vopervaws);
            }
            idr.Close();

            return ColecaoVopervaws;
        }

        public List<VOPERVAWS> ColecaoVopervaws(string Filtro)
        {
            var ColecaoVopervaws = new List<VOPERVAWS>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDCONCENTRADOR);
            sql.AppendLine(" SELECT ID_FUNC, LOGIN, SENHA, CODOPE, NOME, CLASSE, IDPERFIL, STA ");
            sql.AppendLine("   FROM LOGINS WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND {0} ", Filtro));
            sql.AppendLine(" ORDER BY NOME ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Vopervaws = new VOPERVAWS();
                Vopervaws.ID_FUNC = Convert.ToInt32(idr["ID_FUNC"]);
                Vopervaws.LOGIN = Convert.ToString(idr["LOGIN"]);
                Vopervaws.SENHA = Convert.ToString(idr["SENHA"]);
                Vopervaws.CODOPE = Convert.ToInt32(idr["CODOPE"]);
                Vopervaws.NOME = Convert.ToString(idr["NOME"]);
                Vopervaws.CLASSE = Convert.ToString(idr["CLASSE"]);
                Vopervaws.IDPERFIL = Convert.ToInt32(idr["IDPERFIL"]);
                Vopervaws.STA = idr["STA"].ToString();
                ColecaoVopervaws.Add(Vopervaws);
            }
            idr.Close();
            return ColecaoVopervaws;
        }

        public VOPERVAWS GetVopervaws(int idFunc)
        {
            var sql = new StringBuilder();
            var Vopervaws = new VOPERVAWS();
            Database db = new SqlDatabase(BDCONCENTRADOR);
            sql.AppendLine(" SELECT ID_FUNC, LOGIN, SENHA, CODOPE, NOME, CLASSE, IDPERFIL, STA, DTEXPSENHA ");
            sql.AppendLine("   FROM LOGINS WITH (NOLOCK) ");
            sql.AppendLine(string.Format(" WHERE 1=1 AND ID_FUNC = @ID_FUNC "));
            sql.AppendLine(" ORDER BY NOME ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, idFunc);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                Vopervaws.ID_FUNC = Convert.ToInt32(idr["ID_FUNC"]);
                Vopervaws.LOGIN = Convert.ToString(idr["LOGIN"]);
                Vopervaws.SENHA = Convert.ToString(idr["SENHA"]);
                Vopervaws.CODOPE = Convert.ToInt32(idr["CODOPE"]);
                Vopervaws.NOME = Convert.ToString(idr["NOME"]);
                Vopervaws.CLASSE = Convert.ToString(idr["CLASSE"]);
                Vopervaws.IDPERFIL = Convert.ToInt32(idr["IDPERFIL"]);
                if (idr["DTEXPSENHA"] != DBNull.Value)
                    Vopervaws.DTEXPSENHA = Convert.ToDateTime(idr["DTEXPSENHA"]);
                Vopervaws.STA = idr["STA"].ToString();
            }
            idr.Close();
            return Vopervaws;
        }

        public void Inserir(VOPERVAWS Vopervaws)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var dbc = db.CreateConnection();
            dbc.Open();            
            var dbt = dbc.BeginTransaction();            

            try
            {
                var senCrip = Vopervaws.LOGIN.Length < 8 ? BlCriptografia.Encrypt(Vopervaws.LOGIN.PadRight(8, '0')) : BlCriptografia.Encrypt(Vopervaws.LOGIN.Substring(0,8));
                var sql = "INSERT INTO LOGINS (LOGIN, SENHA, SENCRIP, CODOPE, NOME, CLASSE, IDPERFIL, STA) VALUES (@LOGIN, @SENCRIP, @SENCRIP, @CODOPE, @NOME, @CLASSE, @IDPERFIL, @STA) SET @ID = SCOPE_IDENTITY();";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "LOGIN", DbType.String, Vopervaws.LOGIN);
                db.AddInParameter(cmd, "SENCRIP", DbType.String, senCrip);
                db.AddInParameter(cmd, "CODOPE", DbType.Int16, FOperador.CODOPE);
                db.AddInParameter(cmd, "NOME", DbType.String, Vopervaws.NOME);
                db.AddInParameter(cmd, "CLASSE", DbType.String, 1);
                db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, Vopervaws.IDPERFIL);
                db.AddInParameter(cmd, "STA", DbType.String, Vopervaws.STA);
                db.AddOutParameter(cmd, "ID", DbType.Int16, 4);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);
                Vopervaws.ID_FUNC = Convert.ToInt16(db.GetParameterValue(cmd, "@ID"));

                //Sucesso, inserir na tabela local de Opervaws
                if (linhasAfetadas > 0)
                    InseirOpervaws(Vopervaws, senCrip);
                else
                    throw new Exception("Erro ao inserir o operador. Entre em contato com o suporte.");

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }            
        }

        public void InseirOpervaws(VOPERVAWS Vopervaws, string senCrip)
        {            
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            try
            {
                var sql = "INSERT INTO OPERVAWS (LOGIN, SENHA, SENCRIP, NOME, CLASSE, IDPERFIL, ID_FUNC, STA) VALUES (@LOGIN, @SENCRIP, @SENCRIP, @NOME, @CLASSE, @IDPERFIL, @ID_FUNC, @STA) ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "LOGIN", DbType.String, Vopervaws.LOGIN);                
                db.AddInParameter(cmd, "SENCRIP", DbType.String, senCrip);
                db.AddInParameter(cmd, "CODOPE", DbType.Int16, FOperador.CODOPE);
                db.AddInParameter(cmd, "NOME", DbType.String, Vopervaws.NOME);
                db.AddInParameter(cmd, "CLASSE", DbType.String, 1);
                db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, Vopervaws.IDPERFIL);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, Vopervaws.ID_FUNC);
                db.AddInParameter(cmd, "STA", DbType.String, Vopervaws.STA);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                if (linhasAfetadas > 0)
                {
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "INSERT OPERVAWS (Operador VA)", FOperador, cmd);
                    dbt.Commit();
                }
                else
                    throw new Exception("Erro ao inserir o operador. Entre em contato com o suporte.");
                
            }
            catch (Exception err)
            {
                dbt.Rollback();                
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            } 
        }
                
        public void Alterar(VOPERVAWS Vopervaws)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE LOGINS SET LOGIN = @LOGIN, CODOPE = @CODOPE, NOME = @NOME, CLASSE = @CLASSE, IDPERFIL = @IDPERFIL, STA = @STA WHERE LOGIN = @LOGIN";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, Vopervaws.ID_FUNC);
                db.AddInParameter(cmd, "LOGIN", DbType.String, Vopervaws.LOGIN);                
                db.AddInParameter(cmd, "CODOPE", DbType.Int32, Vopervaws.CODOPE);
                db.AddInParameter(cmd, "NOME", DbType.String, Vopervaws.NOME);
                db.AddInParameter(cmd, "CLASSE", DbType.String, Vopervaws.CLASSE);
                db.AddInParameter(cmd, "IDPERFIL", DbType.Int32, Vopervaws.IDPERFIL);
                db.AddInParameter(cmd, "STA", DbType.String, Vopervaws.STA);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);
                
                //Sucesso, ATUALIZAR na tabela local de Opervaws
                if (linhasAfetadas > 0)
                    AlterarOpervaws(Vopervaws);
                else
                    throw new Exception("Erro ao atualizar o operador. Entre em contato com o suporte.");

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public void AlterarOpervaws(VOPERVAWS Vopervaws)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "UPDATE OPERVAWS SET LOGIN = @LOGIN, NOME = @NOME, CLASSE = @CLASSE, IDPERFIL = @IDPERFIL, STA = @STA WHERE LOGIN = @LOGIN";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, Vopervaws.ID_FUNC);
                db.AddInParameter(cmd, "LOGIN", DbType.String, Vopervaws.LOGIN);
                db.AddInParameter(cmd, "NOME", DbType.String, Vopervaws.NOME);
                db.AddInParameter(cmd, "CLASSE", DbType.String, Vopervaws.CLASSE);
                db.AddInParameter(cmd, "IDPERFIL", DbType.Int32, Vopervaws.IDPERFIL);
                db.AddInParameter(cmd, "STA", DbType.String, Vopervaws.STA);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                if (linhasAfetadas > 0)
                {
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "UPDATE VOPERVAWS (Operador VA)", FOperador, cmd);
                    dbt.Commit();
                }
                else
                    throw new Exception("Erro ao atualizar o operador. Entre em contato com o suporte.");
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public void Excluir(VOPERVAWS Vopervaws)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE LOGINS WHERE ID_FUNC = @ID_FUNC";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, Vopervaws.ID_FUNC);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                //Sucesso, ATUALIZAR na tabela local de Opervaws
                if (linhasAfetadas > 0)                    
                    ExcluirOpervaws(Vopervaws);
                else
                    throw new Exception("Erro ao excluir o operador. Entre em contato com o suporte.");
                
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public void ExcluirOpervaws(VOPERVAWS Vopervaws)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE OPERVAWS WHERE ID_FUNC = @ID_FUNC";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "ID_FUNC", DbType.Int32, Vopervaws.ID_FUNC);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                if (linhasAfetadas > 0)
                {
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "DELETE OPERVAWS (Operador VA)", FOperador, cmd);
                    dbt.Commit();
                }
                else                    
                    throw new Exception("Erro ao excluir o operador. Entre em contato com o suporte.");
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool VerificaLogin(string Login)
        {
            var LoginIgual = false;
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT LOGIN ");
            sql.AppendLine("   FROM OPERVAWS WITH (NOLOCK) ");
            sql.AppendLine("  WHERE LOWER(LOGIN) = LOWER(@LOGIN) ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "LOGIN", DbType.String, Login);
            var LoginBanco = Convert.ToString(db.ExecuteScalar(cmd));
            if (LoginBanco.ToLower() == Login.ToLower())
                LoginIgual = true;
            return LoginIgual;
        }

        public Boolean VerificaPerfil(Int32 IdPerfil)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT IDPERFIL ");
            sql.AppendLine("   FROM OPERVAWS WITH (NOLOCK) ");
            sql.AppendLine("  WHERE IDPERFIL = @IDPERFIL ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int32, IdPerfil);
            IdPerfil = Convert.ToInt32(db.ExecuteScalar(cmd));
            return IdPerfil != 0;
        }

        public string AlterarSenha(string NovaSenha)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                var senCrip = NovaSenha.Length < 8 ? BlCriptografia.Encrypt(NovaSenha.PadRight(8, '0')) : BlCriptografia.Encrypt(NovaSenha.Substring(0,8));
                const string sql = "UPDATE LOGINS SET SENCRIP = @SENCRIP WHERE LOWER(LOGIN) = LOWER(@LOGIN) ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "LOGIN", DbType.String, FOperador.LOGIN);
                db.AddInParameter(cmd, "SENCRIP", DbType.String, senCrip);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                //Sucesso, ATUALIZAR na tabela local de Opervaws
                if (linhasAfetadas > 0)
                {
                    AlterarSenhaOpervaws(senCrip);
                    retorno = "Operação realizada com sucesso!";
                }
                else
                    retorno = "Erro ao atualizar a senha do operador.";
                
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }

            return retorno;
        }

        public string AlterarSenhaOpervaws(string NovaSenha)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                //var senCrip = FOperador.LOGIN.Length > 8 ? BlCriptografia.Encrypt(FOperador.LOGIN.PadRight(8, '0')) : FOperador.LOGIN;
                const string sql = "UPDATE OPERVAWS SET SENCRIP = @SENCRIP WHERE LOWER(LOGIN) = LOWER(@LOGIN) ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "LOGIN", DbType.String, FOperador.LOGIN);
                db.AddInParameter(cmd, "SENCRIP", DbType.String, NovaSenha);
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                if (linhasAfetadas > 0)
                {
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "UPDATE OPERVAWS (Operador VA)", FOperador, cmd);
                    retorno = "Operação realizada com sucesso!";
                }
                else
                    retorno = "Erro ao atualizar senha do operador.";

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
            return retorno;
        }

        public string AlterarSenha(VOPERVAWS operador)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                var senCrip = operador.LOGIN.Length < 8 ? 
                    BlCriptografia.Encrypt(operador.LOGIN.PadRight(8, '0')) : 
                    BlCriptografia.Encrypt(operador.LOGIN.Substring(0, 8));

                const string sql = "UPDATE LOGINS SET SENCRIP = @SENCRIP, DTSENHA = NULL, QTDEACESSOINV = NULL, DTEXPSENHA = @DTEXPSENHA WHERE LOWER(LOGIN) = LOWER(@LOGIN) ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "LOGIN", DbType.String, operador.LOGIN);
                db.AddInParameter(cmd, "SENCRIP", DbType.String, senCrip);
                db.AddInParameter(cmd, "DTEXPSENHA", DbType.DateTime, DateTime.Now.AddDays(15));
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                //Sucesso, ATUALIZAR na tabela local de Opervaws
                if (linhasAfetadas > 0)
                {
                    AlterarSenhaOpervaws(operador);
                    retorno = "Operação realizada com sucesso!";
                }
                else
                    retorno = "Erro ao atualizar a senha do operador.";

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }

            return retorno;
        }

        public string AlterarSenhaOpervaws(VOPERVAWS operador)
        {
            var retorno = string.Empty;
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                var senCrip = operador.LOGIN.Length < 8 ?
                    BlCriptografia.Encrypt(operador.LOGIN.PadRight(8, '0')) :
                    BlCriptografia.Encrypt(operador.LOGIN.Substring(0, 8));

                const string sql = "UPDATE OPERVAWS SET SENCRIP = @SENCRIP, DTSENHA = NULL, DTEXPSENHA = @DTEXPSENHA WHERE LOWER(LOGIN) = LOWER(@LOGIN) ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "LOGIN", DbType.String, operador.LOGIN);
                db.AddInParameter(cmd, "SENCRIP", DbType.String, senCrip);
                db.AddInParameter(cmd, "DTEXPSENHA", DbType.DateTime, DateTime.Now.AddDays(15));
                var linhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                if (linhasAfetadas > 0)
                {
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "UPDATE OPERVAWS (Operador VA)", FOperador, cmd);
                    retorno = "Operação realizada com sucesso!";
                }
                else
                    retorno = "Erro ao atualizar senha do operador.";

                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
            return retorno;
        }
    }
}
