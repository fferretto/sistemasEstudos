using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using TELENET.SIL.PO;


namespace TELENET.SIL.DA
{
    public class daOperador
    {
        public OPERADORA Autenticar(string Login, string Senha, out string MsgErro)
        {
            MsgErro = string.Empty;
            var Operador = new OPERADORA();
            var ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
            var BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];

            var BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, ServidorConcentrador, BancoConcentrador, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT O.CODOPE, L.SENHA, L.NOME, O.SERVIDOR_AUT, O.BD_AUT, O.SERVIDOR SERVIDOR_NC, O.BD_NC, L.IDPERFIL,  L.ID_FUNC, O.NOME AS NOMEOPERADORA, L.STA, O.SERVIDOR_IIS ");
            sql.AppendLine("FROM OPERADORA O WITH (NOLOCK) ");
            sql.AppendLine("JOIN LOGINS L WITH (NOLOCK) ");
            sql.AppendLine("  ON L.CODOPE = O.CODOPE ");
            sql.AppendLine("WHERE LOWER(L.LOGIN) = LOWER(@LOGIN )");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "LOGIN", DbType.String, Login);
            var idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                Operador.LOGIN = Login;
                Operador.NOMCLI = Convert.ToString(idr["NOME"]);
                Operador.SENHA = Convert.ToString(idr["SENHA"]);

                Operador.SERVIDORCON = ServidorConcentrador;
                Operador.BANCOCON = BancoConcentrador;
                Operador.SERVIDORAUT = Convert.ToString(idr["SERVIDOR_AUT"]);
                Operador.BANCOAUT = Convert.ToString(idr["BD_AUT"]);
                Operador.SERVIDORNC = Convert.ToString(idr["SERVIDOR_NC"]);
                Operador.BANCONC = Convert.ToString(idr["BD_NC"]);
                Operador.SERVIDORIIS = Convert.ToString(idr["SERVIDOR_IIS"]);
                Operador.IDPERFIL = Convert.ToInt16(idr["IDPERFIL"]);
                Operador.AUTENTICADO = false;
                Operador.ID_FUNC = Convert.ToInt16(idr["ID_FUNC"]);
                Operador.CODOPE = Convert.ToInt16(idr["CODOPE"]);
                Operador.NOMEOPERADORA = idr["NOMEOPERADORA"].ToString();
                Operador.STA = idr["STA"].ToString();

                if (!string.IsNullOrEmpty(Operador.STA) && Operador.STA != ConstantesSIL.StatusAtivo)
                    MsgErro = "Operador nao esta ativo no sistema.";
                else
                    if (Operador.SENHA.ToLower() == Senha.ToLower())
                        Operador.AUTENTICADO = true;
                    else
                        MsgErro = "Senha incorreta. Favor verificar.";
            }
            else
                MsgErro = string.Format("Login nao cadastrado. Favor verificar.");

            idr.Close();

            return Operador;
        }

        public OPERADORA Autenticar2(string Login, string Senha, out string MsgErro, out string Acao)
        {
            MsgErro = string.Empty;
            var Operador = new OPERADORA();
            var ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
            var BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];

            var BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, ServidorConcentrador, BancoConcentrador, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            Database db = new SqlDatabase(BDCONCENTRADOR);
            const string sql = "NC_LOGIN";
            var cmd = db.GetStoredProcCommand(sql);
            
            db.AddInParameter(cmd, "LOGIN", DbType.String, Login);
            db.AddInParameter(cmd, "SENHA", DbType.String, Senha);

            var idr = db.ExecuteReader(cmd);
            Acao = string.Empty;

            if (idr.Read())
            {
                MsgErro = Convert.ToString(idr["RETORNO"]);
                if (MsgErro == "OK")
                {
                    Acao = Convert.ToString(idr["ACAO"]);
                    Operador.LOGIN = Login;
                    Operador.NOMCLI = Convert.ToString(idr["NOME"]);
                    Operador.SENHA = Convert.ToString(idr["SENHA"]);

                    Operador.SERVIDORCON = ServidorConcentrador;
                    Operador.BANCOCON = BancoConcentrador;
                    Operador.SERVIDORAUT = Convert.ToString(idr["SERVIDOR_AUT"]);
                    Operador.BANCOAUT = Convert.ToString(idr["BD_AUT"]);
                    Operador.SERVIDORNC = Convert.ToString(idr["SERVIDOR_NC"]);
                    Operador.BANCONC = Convert.ToString(idr["BD_NC"]);
                    Operador.SERVIDORIIS = Convert.ToString(idr["SERVIDOR_IIS"]);
                    Operador.IDPERFIL = Convert.ToInt16(idr["IDPERFIL"]);
                    Operador.ID_FUNC = Convert.ToInt16(idr["ID_FUNC"]);
                    Operador.CODOPE = Convert.ToInt16(idr["CODOPE"]);
                    Operador.NOMEOPERADORA = idr["NOMEOPERADORA"].ToString();
                    Operador.POSSUI_PAGNET = Convert.ToString(idr["POSSUI_PAGNET"]).ToUpper() == "S";
                    Operador.STA = idr["STA"].ToString();
                    Operador.AUTENTICADO = true;
                }                
            }
            idr.Close();
            return Operador;
        }

        public OPERADORA AlterarSenha(string Login, string Senha, string NovaSenha, out string MsgErro)
        {
            MsgErro = string.Empty;
            var Operador = new OPERADORA();
            var ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
            var BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];

            var BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, ServidorConcentrador, BancoConcentrador, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            Database db = new SqlDatabase(BDCONCENTRADOR);
            const string sql = "NC_ALTERASENHA";
            var cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "LOGIN", DbType.String, Login);
            db.AddInParameter(cmd, "@SENHAANT", DbType.String, Senha);
            db.AddInParameter(cmd, "@NOVASENHA", DbType.String, NovaSenha);

            var idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                MsgErro = Convert.ToString(idr["RETORNO"]);
                if (MsgErro == "OK")
                {                    
                    Operador.LOGIN = Login;
                    Operador.NOMCLI = Convert.ToString(idr["NOME"]);
                    Operador.SENHA = Convert.ToString(idr["SENHA"]);

                    Operador.SERVIDORCON = ServidorConcentrador;
                    Operador.BANCOCON = BancoConcentrador;
                    Operador.SERVIDORAUT = Convert.ToString(idr["SERVIDOR_AUT"]);
                    Operador.BANCOAUT = Convert.ToString(idr["BD_AUT"]);
                    Operador.SERVIDORNC = Convert.ToString(idr["SERVIDOR_NC"]);
                    Operador.BANCONC = Convert.ToString(idr["BD_NC"]);
                    Operador.SERVIDORIIS = Convert.ToString(idr["SERVIDOR_IIS"]);
                    Operador.IDPERFIL = Convert.ToInt16(idr["IDPERFIL"]);
                    Operador.ID_FUNC = Convert.ToInt16(idr["ID_FUNC"]);
                    Operador.CODOPE = Convert.ToInt16(idr["CODOPE"]);
                    Operador.NOMEOPERADORA = idr["NOMEOPERADORA"].ToString();
                    Operador.STA = idr["STA"].ToString();
                    Operador.AUTENTICADO = true;
                }
            }
            idr.Close();
            return Operador;
        }

        public void AutenticarUsuario(string login, string senha)
        {
            var ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
            var BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];
            var BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, ServidorConcentrador, BancoConcentrador, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);

            var sql = new StringBuilder();

            sql.AppendLine("SELECT TOP 1 O.CODOPE, L.SENHA, L.NOME, O.SERVIDOR_AUT, O.BD_AUT, O.SERVIDOR SERVIDOR_NC, O.BD_NC, L.IDPERFIL, L.ID_FUNC, O.NOME AS NOMEOPERADORA, L.STA ");
            sql.AppendLine("FROM OPERADORA O WITH (NOLOCK) ");
            sql.AppendLine("JOIN LOGINS L ");
            sql.AppendLine("  ON L.CODOPE = O.CODOPE ");

            //SQLTelenet.Select<OPERADORA>.Executar(BDCONCENTRADOR, sql.ToString(), true);
        }
    }
}