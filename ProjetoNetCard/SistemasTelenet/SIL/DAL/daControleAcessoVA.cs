using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;

namespace TELENET.SIL.DA
{
    class daControleAcessoVA
    {
        readonly string BDTELENET = string.Empty;

        readonly OPERADORA FOperador;

        public daControleAcessoVA(OPERADORA Operador)
        {
            FOperador = Operador;
            // Monta String Conexao
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region GET

        public List<RESTRICAOACESSO> ColecaoRestricoes(Int16 Perfil, string FlgForm, string Form)
        {
            var ColecaoRestricoes = new List<RESTRICAOACESSO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT IDCOMP, ");
            sql.AppendLine("        IDPERFIL, ");

            switch (FlgForm)
            {
                case "S":
                    sql.AppendLine("        COMPONENTESACESSOVA.FORM AS DESCRICAO, COMPONENTESACESSOVA.TIPOCONTROLE ");
                    break;
                case "N":
                    sql.AppendLine("        COMPONENTESACESSOVA.COMP AS DESCRICAO, COMPONENTESACESSOVA.TIPOCONTROLE ");
                    break;
                default:
                    sql.AppendLine("        COMPONENTESACESSOVA.DESCRICAO, 'Desabilitado' AS TIPOCONTROLE ");
                    break;
            }

            sql.AppendLine("   FROM CONTROLEACESSOVA INNER JOIN ");
            sql.AppendLine("        COMPONENTESACESSOVA ON CONTROLEACESSOVA.IDCOMP = COMPONENTESACESSOVA.ID INNER JOIN ");
            sql.AppendLine("        PERFILACESSOVA ON CONTROLEACESSOVA.IDPERFIL = PERFILACESSOVA.ID ");
            sql.AppendLine(String.Format("  WHERE CONTROLEACESSOVA.IDPERFIL = {0} ", Perfil.ToString()));

            if (FlgForm != string.Empty)
                sql.AppendLine(String.Format("    AND FLGFORM = '{0}' ", FlgForm.ToUpper()));

            if (Form != string.Empty)
                sql.AppendLine(String.Format("    AND UPPER(FORM) = '{0}' ", Form.ToUpper()));

            sql.AppendLine(" ORDER BY COMPONENTESACESSOVA.DESCRICAO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var RestricaoAcesso = new RESTRICAOACESSO();
                RestricaoAcesso.IDCOMP = Convert.ToInt32(idr["IDCOMP"]);
                RestricaoAcesso.IDPERFIL = Convert.ToInt32(idr["IDPERFIL"]);
                RestricaoAcesso.DESCRICAO = Convert.ToString(idr["DESCRICAO"]);
                RestricaoAcesso.TIPOCONTROLE = Convert.ToString(idr["TIPOCONTROLE"]);
                ColecaoRestricoes.Add(RestricaoAcesso);
            }
            idr.Close();
            return ColecaoRestricoes;
        }       

        public bool DuplicidadeCodigos(int IdPerfil, int IdComp)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT IDCOMP ");
            sql.AppendLine("   FROM CONTROLEACESSOVA ");
            sql.AppendLine("  WHERE IDCOMP = @IDCOMP ");
            sql.AppendLine("    AND IDPERFIL = @IDPERFIL ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "IDCOMP", DbType.String, IdComp);
            db.AddInParameter(cmd, "IDPERFIL", DbType.String, IdPerfil);
            var Comp = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Comp != 0);
        }

        public bool DuplicidadeCodigos(int IdPerfil, List<object> IdsComp)
        {
            var idsComp = string.Empty;
            foreach (int id in IdsComp)
            {
                idsComp += id.ToString();
                idsComp += ",";
            }           

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT IDCOMP ");
            sql.AppendLine("  FROM CONTROLEACESSOVA ");
            sql.AppendLine(String.Format("  WHERE IDPERFIL = {0} ", IdPerfil.ToString()));
            sql.AppendLine(String.Format("  AND IDCOMP IN ({0}) ",  idsComp.Remove(idsComp.Length - 1)));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var Comp = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Comp != 0);
        }

        public bool HabilitaAgrupamento()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WHERE ID0 = 'AGRUPAMENTO'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #region Nova Carga Ativa

        public bool NovaCargaAtiva()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'NOVACARGAATIVA'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        public int NumRegConsultaTransacao()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT ISNULL(VAL, 50000) FROM PARAM WHERE ID0 = 'NUMCONSREGTRANS'";
            var cmd = db.GetSqlStringCommand(sql);
            var tamanho = db.ExecuteScalar(cmd);
            return tamanho == null || tamanho == DBNull.Value ? 50000 : Convert.ToInt32(tamanho);
        }

        public int ConsultaAgrupamento(int IdPerfil)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine(" SELECT CODAG ");
            sql.AppendLine("   FROM PERFILACESSOVA ");
            sql.AppendLine("  WHERE ID = @ID ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID", DbType.String, IdPerfil);
            var Comp = db.ExecuteScalar(cmd);
            return Comp == DBNull.Value ? 0 : Convert.ToInt32(Comp);
        }

        #endregion

        #region SET

        public bool Inserir(List<CONTROLEACESSOVA> ControleAcessoVAList)
        {
            var sbCampos = new StringBuilder();
            var sql = string.Empty;
            Database db = new SqlDatabase(BDTELENET);
            sbCampos.Append(" IDCOMP, IDPERFIL ");
            foreach (var controleAcesso in ControleAcessoVAList)
            {
                sql += string.Format("INSERT INTO CONTROLEACESSOVA ({0}) VALUES ({1} , {2})", sbCampos.ToString(), controleAcesso.IDCOMP,controleAcesso.IDPERFIL);
                sql += "   ";
            }                     

            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "INSERT CONTROLEACESSOVA", FOperador, cmd);
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

        public bool Inserir(CONTROLEACESSOVA ControleAcessoVA)
        {
            var sbCampos = new StringBuilder();
            var sbParametros = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sbCampos.Append(" IDCOMP, IDPERFIL ");
            sbParametros.Append(" @IDCOMP, @IDPERFIL ");
            var sql = string.Format("INSERT INTO CONTROLEACESSOVA ({0}) VALUES ({1})", sbCampos, sbParametros);
            var cmd = db.GetSqlStringCommand(sql);
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
                UtilSIL.GravarLog(db, dbt, "INSERT CONTROLEACESSOVA", FOperador, cmd);
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

        public void Excluir(CONTROLEACESSOVA ControleAcessoVA)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE CONTROLEACESSOVA WHERE IDCOMP = @IDCOMP AND IDPERFIL = @IDPERFIL ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "IDCOMP", DbType.Int32, ControleAcessoVA.IDCOMP);
                db.AddInParameter(cmd, "IDPERFIL", DbType.Int32, ControleAcessoVA.IDPERFIL);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE CONTROLEACESSOVA", FOperador, cmd);
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

        public void ExcluirPerfil(Int32 IdPerfil)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            // Controle Transacao
            var dbt = dbc.BeginTransaction();
            try
            {
                const string sql = "DELETE CONTROLEACESSOVA WHERE IDPERFIL = @IDPERFIL ";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "IDPERFIL", DbType.Int32, IdPerfil);
                db.ExecuteNonQuery(cmd, dbt);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog(db, dbt, "DELETE CONTROLEACESSOVA", FOperador, cmd);
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

        #endregion
    }
}
