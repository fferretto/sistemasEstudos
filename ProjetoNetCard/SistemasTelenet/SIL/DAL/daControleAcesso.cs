using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daControleAcesso
    {
        readonly string BDTELENET = string.Empty;

        readonly OPERADORA FOperador;

        public daControleAcesso(OPERADORA Operador)
        {
            FOperador = Operador;
            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region GET

        public List<PERMISSAOACESSO> ColecaoPermissoes(Int16 Perfil, string FlgForm, string Form)
        {
            var ColecaoPermissoes = new List<PERMISSAOACESSO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT IDCOMP, ");
            sql.AppendLine("        IDPERFIL, ");

            switch (FlgForm)
            {
                case "S":
                    sql.AppendLine("        COMPONENTESACESSO.FORM AS DESCRICAO, COMPONENTESACESSO.TIPOCONTROLE ");
                    break;
                case "N":
                    sql.AppendLine("        COMPONENTESACESSO.COMP AS DESCRICAO, COMPONENTESACESSO.TIPOCONTROLE ");
                    break;
                default:
                    sql.AppendLine("        COMPONENTESACESSO.DESCRICAO, 'Desabilitado' AS TIPOCONTROLE ");
                    break;
            }

            sql.AppendLine("   FROM CONTROLEACESSO WITH (NOLOCK) INNER JOIN ");
            sql.AppendLine("        COMPONENTESACESSO WITH (NOLOCK) ON CONTROLEACESSO.IDCOMP = COMPONENTESACESSO.ID INNER JOIN ");
            sql.AppendLine("        PERFILACESSOVA WITH (NOLOCK) ON CONTROLEACESSO.IDPERFIL = PERFILACESSOVA.ID ");
            sql.AppendLine(String.Format("  WHERE CONTROLEACESSO.IDPERFIL = {0} ", Perfil.ToString()));

            if (FlgForm != string.Empty)
                sql.AppendLine(String.Format("    AND FLGFORM = '{0}' ", FlgForm.ToUpper()));

            if (Form != string.Empty)
                sql.AppendLine(String.Format("    AND UPPER(FORM) = '{0}' ", Form.ToUpper()));

            sql.AppendLine(" ORDER BY COMPONENTESACESSO.DESCRICAO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var PermissaoAcesso = new PERMISSAOACESSO();
                PermissaoAcesso.IDCOMP = Convert.ToInt32(idr["IDCOMP"]);
                PermissaoAcesso.IDPERFIL = Convert.ToInt32(idr["IDPERFIL"]);
                PermissaoAcesso.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                PermissaoAcesso.TIPOCONTROLE = Convert.ToString(idr["TIPOCONTROLE"]);
                ColecaoPermissoes.Add(PermissaoAcesso);
            }
            idr.Close();
            return ColecaoPermissoes;
        }

        public List<PERMISSAOACESSO> ColecaoPermissoes2(Int16 Perfil, string FlgForm, string Form)
        {
            var ColecaoPermissoes = new List<PERMISSAOACESSO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT DISTINCT COMPONENTESACESSO.DESCRICAO, IDPERFIL ");
            sql.AppendLine(" FROM CONTROLEACESSO WITH (NOLOCK) INNER JOIN ");
            sql.AppendLine("        COMPONENTESACESSO WITH (NOLOCK) ON CONTROLEACESSO.IDCOMP = COMPONENTESACESSO.ID INNER JOIN ");
            sql.AppendLine("        PERFILACESSOVA WITH (NOLOCK) ON CONTROLEACESSO.IDPERFIL = PERFILACESSOVA.ID ");
            sql.AppendLine(String.Format("  WHERE CONTROLEACESSO.IDPERFIL = {0} ", Perfil.ToString()));
            sql.AppendLine(" ORDER BY COMPONENTESACESSO.DESCRICAO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var PermissaoAcesso = new PERMISSAOACESSO();                
                PermissaoAcesso.IDPERFIL = Convert.ToInt32(idr["IDPERFIL"]);
                PermissaoAcesso.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                ColecaoPermissoes.Add(PermissaoAcesso);
            }
            idr.Close();
            return ColecaoPermissoes;
        }       

        public bool DuplicidadeCodigos(int IdPerfil, int IdComp)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT IDCOMP ");
            sql.AppendLine("   FROM CONTROLEACESSO WITH (NOLOCK) ");
            sql.AppendLine("  WHERE IDCOMP = @IDCOMP ");
            sql.AppendLine("    AND IDPERFIL = @IDPERFIL ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "IDCOMP", DbType.String, IdComp);
            db.AddInParameter(cmd, "IDPERFIL", DbType.String, IdPerfil);
            var Comp = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Comp != 0);
        }

        public bool DuplicidadeCodigos(int IdPerfil, string acao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT IDCOMP FROM CONTROLEACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE IDPERFIL = @IDPERFIL AND IDCOMP IN ( ");
            sql.AppendLine("SELECT DISTINCT ID FROM COMPONENTESACESSO WITH (NOLOCK) WHERE DESCRICAO = @DESCRICAO )  ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "DESCRICAO", DbType.String, acao);
            db.AddInParameter(cmd, "IDPERFIL", DbType.String, IdPerfil);
            var Comp = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Comp != 0);
        }

        public bool FormPaiLiberado(int IdPerfil, string acao)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT IDCOMP FROM CONTROLEACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE IDPERFIL = @IDPERFIL AND IDCOMP =  ");
            sql.AppendLine("(SELECT ID FROM COMPONENTESACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE FLGFORM = 'S' AND FORM = ( SELECT DISTINCT FORM FROM COMPONENTESACESSO WITH (NOLOCK) WHERE DESCRICAO = @DESCRICAO ) ) ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "DESCRICAO", DbType.String, acao);
            db.AddInParameter(cmd, "IDPERFIL", DbType.String, IdPerfil);
            var Comp = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Comp != 0);
        }

        public bool HabilitaAgrupamento()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'AGRUPAMENTO'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public int NumRegConsultaTransacao()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT ISNULL(VAL, 50000) FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'NUMCONSREGTRANS'";
            var cmd = db.GetSqlStringCommand(sql);
            var tamanho = db.ExecuteScalar(cmd);
            return tamanho == null || tamanho == DBNull.Value ? 50000 : Convert.ToInt32(tamanho);
        }

        public int ConsultaAgrupamento(int IdPerfil)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT CODAG ");
            sql.AppendLine("   FROM PERFILACESSOVA WITH (NOLOCK) ");
            sql.AppendLine("  WHERE ID = @ID ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID", DbType.String, IdPerfil);
            var Comp = db.ExecuteScalar(cmd);
            return Comp == DBNull.Value ? 0 : Convert.ToInt32(Comp);
        }

        #endregion

        #region SET

        public bool Inserir(int idPerfil, string acao)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("INSERT INTO CONTROLEACESSO ");
            sql.AppendLine("SELECT ID AS IDCOMP, @IDPERFIL AS IDPERFIL FROM COMPONENTESACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE DESCRICAO = @DESCRICAO ");
            sql.AppendLine("ORDER BY ID");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();
            // Usuario VA
            db.AddInParameter(cmd, "DESCRICAO", DbType.String, acao);
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, idPerfil);
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT CONTROLEACESSO", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally
            {
                dbc.Close();
            }

            return true;
        }

        public bool InserirFormPai(int idPerfil, string acao)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("INSERT INTO CONTROLEACESSO ");
            sql.AppendLine("SELECT ID, @IDPERFIL as IDPERFIL FROM COMPONENTESACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE FLGFORM = 'S' AND FORM = (  ");
            sql.AppendLine("SELECT DISTINCT FORM FROM COMPONENTESACESSO WITH (NOLOCK) WHERE DESCRICAO = @DESCRICAO ) ");	

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();
            // Usuario VA
            db.AddInParameter(cmd, "DESCRICAO", DbType.String, acao);
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, idPerfil);
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT CONTROLEACESSO", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally
            {
                dbc.Close();
            }

            return true;
        }

        public bool Inserir(CONTROLEACESSO ControleAcessoVA)
        {   
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("INSERT INTO CONTROLEACESSO ");
            sql.AppendLine("SELECT ID AS IDCOMP, @IDPERFIL AS IDPERFIL FROM COMPONENTESACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE FORM = (SELECT FORM FROM COMPONENTESACESSO WITH (NOLOCK) WHERE ID = @IDCOMP AND FLGFORM = 'S') ");
            sql.AppendLine("ORDER BY ID");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();
            // Usuario VA
            db.AddInParameter(cmd, "IDCOMP", DbType.Int16, ControleAcessoVA.IDCOMP);
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, ControleAcessoVA.IDPERFIL);
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT CONTROLEACESSO", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir]" + err);
            }
            finally
            {
                dbc.Close();
            }

            return true;
        }

        public void Excluir(int idPerfil, string acao)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            sql.AppendLine("DELETE CONTROLEACESSO ");
            sql.AppendLine("WHERE IDPERFIL = @IDPERFIL AND IDCOMP IN (");
            sql.AppendLine("SELECT ID AS IDCOMP FROM COMPONENTESACESSO WITH (NOLOCK) ");
            sql.AppendLine("WHERE DESCRICAO LIKE '" + acao + "%'");
            sql.AppendLine(")");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();                        
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int16, idPerfil);
            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE CONTROLEACESSO", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion
    }
}
