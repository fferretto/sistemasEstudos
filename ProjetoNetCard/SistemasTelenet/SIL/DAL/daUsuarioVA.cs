using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using TELENET.SIL.PO;
using System.Collections;
using SIL;
using System.Configuration;

namespace TELENET.SIL.DA
{
    class daUsuarioVA
    {
        readonly string BDTELENET = string.Empty;
        readonly string BDAUTORIZADOR = string.Empty;
        readonly string BDCONCENTRADOR = string.Empty;
        readonly OPERADORA FOperador;

        public daUsuarioVA(OPERADORA Operador)
        {
            FOperador = Operador;
            var ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
            var BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, Operador.SERVIDORCON ?? ServidorConcentrador, Operador.BANCOCON ?? BancoConcentrador, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region  GET Usuario VA

        public CLIENTE GetClienteCodCli(int codCli)
        {
            var cliente = new CLIENTE();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCLI, NOMCLI ");
            sql.AppendLine("FROM CLIENTEVA ");
            sql.AppendLine("WHERE CODCLI = @CODCLI ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                cliente.CODCLI = codCli;
                cliente.NOMCLI = idr["NOMCLI"].ToString();
            }
            idr.Close();
            return cliente;
        }

        public USUARIO_VA GetUsuarioVA(int id)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            
            #region Query

            sql.AppendLine("SELECT");
            sql.AppendLine("  U.GERCRT,");
            sql.AppendLine("  U.ID,");
            sql.AppendLine("  U.CPF,");
            sql.AppendLine("  U.NOMUSU,");
            sql.AppendLine("  U.CODCRT,");
            sql.AppendLine("  U.NUMDEP,");
            sql.AppendLine("  U.CODPAR,");
            sql.AppendLine("  U.DATINC,");
            sql.AppendLine("  U.DATATV,");
            sql.AppendLine("  U.DATSTA,");
            sql.AppendLine("  S.DESTA,");
            sql.AppendLine("  S.STA,");
            sql.AppendLine("  C.NOMCLI,");
            sql.AppendLine("  U.CODFIL,");
            sql.AppendLine("  U.TRILHA2,");
            sql.AppendLine("  U.MAT,");
            sql.AppendLine("  U.NUMPAC,");
            sql.AppendLine("  U.CODSET,");
            sql.AppendLine("  U.CARGPADVA,");
            sql.AppendLine("  U.VALADES,");
            sql.AppendLine("  U.DATGERCRT,");
            sql.AppendLine("  U.DTVALCART,");
            sql.AppendLine("  U.NUMULTPAC,");
            sql.AppendLine("  U.CELULAR,");
            sql.AppendLine("  U.DATADES,");
            sql.AppendLine("  U.DTEXPSENHA,");
            sql.AppendLine("  U.NOMCRT,");
            sql.AppendLine("  C.STA AS STACLI,");
            sql.AppendLine("  C.COB2AV,");
            sql.AppendLine("  C.VAL2AV,");
            sql.AppendLine("  C.PRZVALCART,");
            sql.AppendLine("  C.CODCLI, ");
            sql.AppendLine("  P.ROTULO, ");
            sql.AppendLine("  CU.DATNAS,");
            sql.AppendLine("  CU.PAI,");
            sql.AppendLine("  CU.MAE,");
            sql.AppendLine("  CU.CEL,");
            sql.AppendLine("  CU.TEL,");
            sql.AppendLine("  CU.EMA,");
            sql.AppendLine("  CU.SEXO,");
            sql.AppendLine("  CU.RG,");
            sql.AppendLine("  CU.ORGEXPRG,");
            sql.AppendLine("  CU.NATURALIDADE,");
            sql.AppendLine("  CU.NACIONALIDADE,");
            sql.AppendLine("  CU.ENDUSU,");
            sql.AppendLine("  CU.ENDNUMUSU,");
            sql.AppendLine("  CU.ENDCPL,");
            sql.AppendLine("  CU.BAIRRO,");
            sql.AppendLine("  CU.LOCALIDADE,");
            sql.AppendLine("  CU.UF,");
            sql.AppendLine("  CU.CEP,");
            sql.AppendLine("  CU.ENDUSUCOM,");
            sql.AppendLine("  CU.ENDNUMCOM,");
            sql.AppendLine("  CU.ENDCPLCOM,");
            sql.AppendLine("  CU.BAIRROCOM,");
            sql.AppendLine("  CU.LOCALIDADECOM,");
            sql.AppendLine("  CU.UFCOM,");
            sql.AppendLine("  CU.CEPCOM,");
            sql.AppendLine("  CU.TELCOM,");
            sql.AppendLine("  ISNULL(CU.CPF, 0) AS TEMCADUSU ");
            sql.AppendLine("FROM USUARIOVA U");
            sql.AppendLine("JOIN CLIENTEVA C ON C.CODCLI = U.CODCLI");
            sql.AppendLine("LEFT JOIN CADUSUARIO CU ON U.CPF = CU.CPF");
            sql.AppendLine("JOIN PRODUTO P ON C.CODPROD = P.CODPROD");
            sql.AppendLine("JOIN STATUS S ON S.STA = U.STA");
            sql.AppendLine("WHERE U.ID = @ID ");
            sql.AppendLine("ORDER BY U.NOMUSU");

            #endregion

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@ID", DbType.Int32, id);
            var idr = db.ExecuteReader(cmd);
            var Usuario = new USUARIO_VA();
            while (idr.Read())
            {
                Usuario.TEMCAD = Convert.ToString(idr["TEMCADUSU"]) != "0";
                Usuario.CPF = Convert.ToString(idr["CPF"]);
                Usuario.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                Usuario.CODCRT = Convert.ToString(idr["CODCRT"]);
                Usuario.NUMDEP = Convert.ToString(idr["NUMDEP"]);
                Usuario.STA = Convert.ToString(idr["STA"]);
                Usuario.DESTA = Convert.ToString(idr["DESTA"]);
                Usuario.ROTULO = Convert.ToString(idr["ROTULO"]);
                Usuario.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                Usuario.ID = Convert.ToInt32(idr["ID"]);
                Usuario.CODPAR = Convert.ToString(idr["CODPAR"]);
                Usuario.DATINC = Convert.ToDateTime(idr["DATINC"]);
                Usuario.CODCLI = Convert.ToString(idr["CODCLI"]);
                Usuario.MAT = idr["MAT"].ToString();
                Usuario.CODFIL = idr["CODFIL"].ToString();
                Usuario.CODSET = idr["CODSET"].ToString();
                Usuario.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                Usuario.STACLI = idr["STACLI"].ToString();
                Usuario.DATGERCRT = idr["DATGERCRT"].ToString();
                Usuario.VALADES = idr["VALADES"].ToString();
                Usuario.NUMPAC = idr["NUMPAC"].ToString();
                Usuario.NUMULTPAC = idr["NUMULTPAC"].ToString();
                Usuario.DATADES = idr["DATADES"].ToString();
                Usuario.DATSTASTR = idr["DATSTA"].ToString();
                Usuario.DATATV = idr["DATATV"].ToString();
                if (!string.IsNullOrEmpty(idr["DATSTA"].ToString()))
                    Usuario.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                Usuario.COBRASEGVIA = Convert.ToBoolean(idr["COB2AV"]);
                if (!string.IsNullOrEmpty(idr["VAL2AV"].ToString()))
                    Usuario.VALORSEGVIA = Convert.ToDecimal(idr["VAL2AV"]);
                Usuario.PRZVALCART = idr["PRZVALCART"].ToString();
                Usuario.TRILHA2 = idr["TRILHA2"].ToString();
                Usuario.DTVALCART = idr["DTVALCART"].ToString();
                Usuario.GERCRT = idr["GERCRT"].ToString();
                Usuario.NOMCRT = idr["NOMCRT"].ToString();
                if (!string.IsNullOrEmpty(idr["DATNAS"].ToString()))
                    Usuario.DATNAS = Convert.ToDateTime(idr["DATNAS"]);
                Usuario.PAI = idr["PAI"].ToString();
                Usuario.MAE = idr["MAE"].ToString();
                Usuario.CEL = idr["CEL"].ToString();
                Usuario.TEL = idr["TEL"].ToString();
                Usuario.SEXO = idr["SEXO"].ToString();
                Usuario.EMA = idr["EMA"].ToString();
                Usuario.RG = idr["RG"].ToString();
                Usuario.ORGEXPRG = idr["ORGEXPRG"].ToString();
                Usuario.NATURALIDADE = idr["NATURALIDADE"].ToString();
                Usuario.NACIONALIDADE = idr["NACIONALIDADE"].ToString();
                Usuario.ENDUSU = idr["ENDUSU"].ToString();
                Usuario.ENDNUMUSU = idr["ENDNUMUSU"].ToString();
                Usuario.ENDCPL = idr["ENDCPL"].ToString();
                Usuario.BAIRRO = idr["BAIRRO"].ToString();
                Usuario.LOCALIDADE = idr["LOCALIDADE"].ToString();
                Usuario.UF = idr["UF"].ToString();
                Usuario.CEP = idr["CEP"].ToString();
                Usuario.ENDUSUCOM = idr["ENDUSUCOM"].ToString();
                Usuario.ENDNUMCOM = idr["ENDNUMCOM"].ToString();
                Usuario.ENDCPLCOM = idr["ENDCPLCOM"].ToString();
                Usuario.BAIRROCOM = idr["BAIRROCOM"].ToString();
                Usuario.LOCALIDADECOM = idr["LOCALIDADECOM"].ToString();
                Usuario.UFCOM = idr["UFCOM"].ToString();
                Usuario.CEPCOM = idr["CEPCOM"].ToString();
                Usuario.TELCOM = idr["TELCOM"].ToString();
                Usuario.DTEXPSENHA = idr["DTEXPSENHA"].ToString();
            }
            idr.Close();
            return Usuario;
        }

        public List<USUARIO_VA> UsuarioVA(short Classificacao, int CodCliInicial, int CodCliFinal,
            DateTime DataInicial, DateTime DataFinal, string ParametroIncial, string ParametroFinal)
        {
            List<USUARIO_VA> ColecaoUsuarioVA;
            ColecaoUsuarioVA = new List<USUARIO_VA>();

            Database db = new SqlDatabase(BDTELENET);
            string sql = "PROC_REL_USUVA";
            DbCommand cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "CLASSIF", DbType.Byte, Classificacao);
            db.AddInParameter(cmd, "CODCLI_INI", DbType.Int32, CodCliInicial);
            db.AddInParameter(cmd, "CODCLI_FIM", DbType.Int32, CodCliFinal);
            db.AddInParameter(cmd, "DATA_INI", DbType.DateTime, DataInicial);
            db.AddInParameter(cmd, "DATA_FIM", DbType.DateTime, DataFinal);
            db.AddInParameter(cmd, "PARAM_INI", DbType.String, ParametroIncial);
            db.AddInParameter(cmd, "PARAM_FIM", DbType.String, ParametroFinal);

            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                USUARIO_VA Usuario;
                Usuario = new USUARIO_VA();

                Usuario.CODCLI = Convert.ToString(idr["CODCLI"]);
                Usuario.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                Usuario.CODFILNUT = Convert.ToString(idr["CODFILNUT"]);
                Usuario.CODSET = Convert.ToString(idr["CODSET"]);
                Usuario.MAT = Convert.ToString(idr["MAT"]);
                Usuario.CPF = Convert.ToString(idr["CPF"]);
                Usuario.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                Usuario.NUMDEP = Convert.ToString(idr["NUMDEP"]);
                Usuario.CODPAR = Convert.ToString(idr["CODPAR"]);
                if (idr["DATINC"] != DBNull.Value)
                    Usuario.DATINC = Convert.ToDateTime(idr["DATINC"]);

                ColecaoUsuarioVA.Add(Usuario);
            }
            idr.Close();

            return ColecaoUsuarioVA;
        }

        public USUARIO_VA GetDadosCompl(string cpf)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "MW_GET_DADOSCARTAO";
            DbCommand cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "TIPO_ACESSO", DbType.Int16, 0);
            IDataReader dr = db.ExecuteReader(cmd);
            var Usuario = new USUARIO_VA();
            Usuario.TEMCAD = false;
            while (dr.Read())
            {
                Usuario.TEMCAD = true;
                if (!string.IsNullOrEmpty(dr["DATNAS"].ToString()))
                    Usuario.DATNAS = Convert.ToDateTime(dr["DATNAS"]);
                Usuario.PAI = Convert.ToString(dr["PAI"]);
                Usuario.MAE = Convert.ToString(dr["MAE"]);
                Usuario.RG = Convert.ToString(dr["RG"]);
                Usuario.CEL = Convert.ToString(dr["CEL"]);
                Usuario.TEL = Convert.ToString(dr["TEL"]);
                Usuario.SEXO = Convert.ToString(dr["SEXO"]);
                Usuario.EMA = Convert.ToString(dr["EMA"]);
                Usuario.ORGEXPRG = Convert.ToString(dr["ORGEXPRG"]);
                Usuario.NATURALIDADE = Convert.ToString(dr["NATURALIDADE"]);
                Usuario.NACIONALIDADE = Convert.ToString(dr["NACIONALIDADE"]);
                Usuario.ENDUSU = Convert.ToString(dr["ENDUSU"]);
                Usuario.ENDNUMUSU = Convert.ToString(dr["ENDNUMUSU"]);
                Usuario.ENDCPL = Convert.ToString(dr["ENDCPL"]);
                Usuario.BAIRRO = Convert.ToString(dr["BAIRRO"]);
                Usuario.LOCALIDADE = Convert.ToString(dr["LOCALIDADE"]);
                Usuario.UF = Convert.ToString(dr["UF"]);
                Usuario.CEP = Convert.ToString(dr["CEP"]);
                Usuario.ENDUSUCOM = Convert.ToString(dr["ENDUSUCOM"]);
                Usuario.ENDNUMCOM = Convert.ToString(dr["ENDNUMCOM"]);
                Usuario.ENDCPLCOM = Convert.ToString(dr["ENDCPLCOM"]);
                Usuario.BAIRROCOM = Convert.ToString(dr["BAIRROCOM"]);
                Usuario.LOCALIDADECOM = Convert.ToString(dr["LOCALIDADECOM"]);
                Usuario.UFCOM = Convert.ToString(dr["UFCOM"]);
                Usuario.CEPCOM = Convert.ToString(dr["CEPCOM"]);
                Usuario.TELCOM = Convert.ToString(dr["TELCOM"]);
            }
            dr.Close();
            return Usuario;
        }

        public List<USUARIO_VA> ColecaoUsuarioVA(string Filtro)
        {
            List<USUARIO_VA> ColecaoUsuario;
            ColecaoUsuario = new List<USUARIO_VA>();

            Database db;
            StringBuilder sql = new StringBuilder();

            db = new SqlDatabase(BDTELENET);

            #region Query

            sql.AppendLine("SELECT");
            sql.AppendLine("  U.GERCRT,");
            sql.AppendLine("  U.ID,");
            sql.AppendLine("  U.CPF,");
            sql.AppendLine("  U.NOMUSU,");
            sql.AppendLine("  U.CODCRT,");
            sql.AppendLine("  U.NUMDEP,");
            sql.AppendLine("  U.CODPAR,");
            sql.AppendLine("  U.DATINC,");
            sql.AppendLine("  U.DATATV,");
            sql.AppendLine("  U.DATSTA,");
            sql.AppendLine("  S.DESTA,");
            sql.AppendLine("  S.STA,");
            sql.AppendLine("  C.NOMCLI,");
            sql.AppendLine("  U.CODFIL,");
            sql.AppendLine("  U.TRILHA2,");
            sql.AppendLine("  U.MAT,");
            sql.AppendLine("  U.NUMPAC,");
            sql.AppendLine("  U.CODSET,");
            sql.AppendLine("  U.CARGPADVA,");
            sql.AppendLine("  U.VALADES,");
            sql.AppendLine("  U.DATGERCRT,");
            sql.AppendLine("  U.DTVALCART,");
            sql.AppendLine("  U.NUMULTPAC,");
            sql.AppendLine("  U.CELULAR,");
            sql.AppendLine("  U.DATADES,");
            sql.AppendLine("  U.DTEXPSENHA,");
            sql.AppendLine("  U.NOMCRT,");
            sql.AppendLine("  C.STA AS STACLI,");
            sql.AppendLine("  C.COB2AV,");
            sql.AppendLine("  C.VAL2AV,");
            sql.AppendLine("  C.PRZVALCART,");
            sql.AppendLine("  C.CODCLI, ");
            sql.AppendLine("  P.ROTULO, ");
            sql.AppendLine("  CU.DATNAS,");
            sql.AppendLine("  CU.PAI,");
            sql.AppendLine("  CU.MAE,");
            sql.AppendLine("  CU.CEL,");
            sql.AppendLine("  CU.TEL,");
            sql.AppendLine("  CU.EMA,");
            sql.AppendLine("  CU.SEXO,");
            sql.AppendLine("  CU.RG,");
            sql.AppendLine("  CU.ORGEXPRG,");
            sql.AppendLine("  CU.NATURALIDADE,");
            sql.AppendLine("  CU.NACIONALIDADE,");
            sql.AppendLine("  CU.ENDUSU,");
            sql.AppendLine("  CU.ENDNUMUSU,");
            sql.AppendLine("  CU.ENDCPL,");
            sql.AppendLine("  CU.BAIRRO,");
            sql.AppendLine("  CU.LOCALIDADE,");
            sql.AppendLine("  CU.UF,");
            sql.AppendLine("  CU.CEP,");
            sql.AppendLine("  CU.ENDUSUCOM,");
            sql.AppendLine("  CU.ENDNUMCOM,");
            sql.AppendLine("  CU.ENDCPLCOM,");
            sql.AppendLine("  CU.BAIRROCOM,");
            sql.AppendLine("  CU.LOCALIDADECOM,");
            sql.AppendLine("  CU.UFCOM,");
            sql.AppendLine("  CU.CEPCOM,");
            sql.AppendLine("  CU.TELCOM,");
            sql.AppendLine("  ISNULL(CU.CPF, 0) AS TEMCADUSU ");
            sql.AppendLine("FROM USUARIOVA U");
            sql.AppendLine("JOIN CLIENTEVA C ON C.CODCLI = U.CODCLI");
            sql.AppendLine("LEFT JOIN CADUSUARIO CU ON U.CPF = CU.CPF");
            sql.AppendLine("JOIN PRODUTO P ON C.CODPROD = P.CODPROD");
            sql.AppendLine("JOIN STATUS S ON S.STA = U.STA");

            if (Filtro != string.Empty)
                sql.AppendLine(string.Format("WHERE {0} ", Filtro));

            sql.AppendLine("ORDER BY U.NOMUSU");

            #endregion

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                USUARIO_VA Usuario = new USUARIO_VA();
                Usuario.TEMCAD = Convert.ToString(idr["TEMCADUSU"]) != "0";
                Usuario.CPF = Convert.ToString(idr["CPF"]);
                Usuario.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                Usuario.CODCRT = Convert.ToString(idr["CODCRT"]);
                Usuario.NUMDEP = Convert.ToString(idr["NUMDEP"]);
                Usuario.STA = Convert.ToString(idr["STA"]);
                Usuario.DESTA = Convert.ToString(idr["DESTA"]);
                Usuario.ROTULO = Convert.ToString(idr["ROTULO"]);
                Usuario.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                Usuario.ID = Convert.ToInt32(idr["ID"]);
                Usuario.CODPAR = Convert.ToString(idr["CODPAR"]);
                Usuario.DATINC = Convert.ToDateTime(idr["DATINC"]);
                Usuario.CODCLI = Convert.ToString(idr["CODCLI"]);
                Usuario.MAT = idr["MAT"].ToString();
                Usuario.CODFIL = idr["CODFIL"].ToString();
                Usuario.CODSET = idr["CODSET"].ToString();
                Usuario.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                Usuario.STACLI = idr["STACLI"].ToString();
                Usuario.DATGERCRT = idr["DATGERCRT"].ToString();
                Usuario.VALADES = idr["VALADES"].ToString();
                Usuario.NUMPAC = idr["NUMPAC"].ToString();
                Usuario.NUMULTPAC = idr["NUMULTPAC"].ToString();
                Usuario.DATADES = idr["DATADES"].ToString();
                Usuario.DATSTASTR = idr["DATSTA"].ToString();
                Usuario.DATATV = idr["DATATV"].ToString();
                if (!string.IsNullOrEmpty(idr["DATSTA"].ToString()))
                    Usuario.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                Usuario.COBRASEGVIA = Convert.ToBoolean(idr["COB2AV"]);
                if (!string.IsNullOrEmpty(idr["VAL2AV"].ToString()))
                    Usuario.VALORSEGVIA = Convert.ToDecimal(idr["VAL2AV"]);
                Usuario.PRZVALCART = idr["PRZVALCART"].ToString();
                Usuario.TRILHA2 = idr["TRILHA2"].ToString();
                Usuario.DTVALCART = idr["DTVALCART"].ToString();
                Usuario.GERCRT = idr["GERCRT"].ToString();
                Usuario.NOMCRT = idr["NOMCRT"].ToString();
                if (!string.IsNullOrEmpty(idr["DATNAS"].ToString()))
                    Usuario.DATNAS = Convert.ToDateTime(idr["DATNAS"]);
                Usuario.PAI = idr["PAI"].ToString();
                Usuario.MAE = idr["MAE"].ToString();
                Usuario.CEL = idr["CEL"].ToString();
                Usuario.TEL = idr["TEL"].ToString();
                Usuario.SEXO = idr["SEXO"].ToString();
                Usuario.EMA = idr["EMA"].ToString();
                Usuario.RG = idr["RG"].ToString();
                Usuario.ORGEXPRG = idr["ORGEXPRG"].ToString();
                Usuario.NATURALIDADE = idr["NATURALIDADE"].ToString();
                Usuario.NACIONALIDADE = idr["NACIONALIDADE"].ToString();
                Usuario.ENDUSU = idr["ENDUSU"].ToString();
                Usuario.ENDNUMUSU = idr["ENDNUMUSU"].ToString();
                Usuario.ENDCPL = idr["ENDCPL"].ToString();
                Usuario.BAIRRO = idr["BAIRRO"].ToString();
                Usuario.LOCALIDADE = idr["LOCALIDADE"].ToString();
                Usuario.UF = idr["UF"].ToString();
                Usuario.CEP = idr["CEP"].ToString();
                Usuario.ENDUSUCOM = idr["ENDUSUCOM"].ToString();
                Usuario.ENDNUMCOM = idr["ENDNUMCOM"].ToString();
                Usuario.ENDCPLCOM = idr["ENDCPLCOM"].ToString();
                Usuario.BAIRROCOM = idr["BAIRROCOM"].ToString();
                Usuario.LOCALIDADECOM = idr["LOCALIDADECOM"].ToString();
                Usuario.UFCOM = idr["UFCOM"].ToString();
                Usuario.CEPCOM = idr["CEPCOM"].ToString();
                Usuario.TELCOM = idr["TELCOM"].ToString();
                Usuario.DTEXPSENHA = idr["DTEXPSENHA"].ToString();
                ColecaoUsuario.Add(Usuario);
            }

            idr.Close();

            return ColecaoUsuario;
        }

        public List<USUARIO_VA> ColecaoUsuarioVAFilter(string Filtro)
        {
            List<USUARIO_VA> ColecaoUsuario;
            ColecaoUsuario = new List<USUARIO_VA>();

            Database db;
            StringBuilder sql = new StringBuilder();

            db = new SqlDatabase(BDTELENET);

            #region Query

            sql.AppendLine("SELECT");
            sql.AppendLine("  U.ID,");
            sql.AppendLine("  U.CPF,");
            sql.AppendLine("  U.NOMUSU,");
            sql.AppendLine("  U.CODCRT,");
            sql.AppendLine("  U.NUMDEP,");
            sql.AppendLine("  U.CODPAR,");
            sql.AppendLine("  U.DATINC,");
            sql.AppendLine("  U.DATATV,");
            sql.AppendLine("  U.DATSTA,");
            sql.AppendLine("  S.DESTA,");
            sql.AppendLine("  S.STA,");
            sql.AppendLine("  C.NOMCLI,");
            sql.AppendLine("  U.CODFIL,");
            sql.AppendLine("  U.TRILHA2,");
            sql.AppendLine("  U.MAT,");
            sql.AppendLine("  U.NUMPAC,");
            sql.AppendLine("  U.CODSET,");
            sql.AppendLine("  U.CARGPADVA,");
            sql.AppendLine("  U.VALADES,");
            sql.AppendLine("  U.DATGERCRT,");
            sql.AppendLine("  U.DTVALCART,");
            sql.AppendLine("  U.NUMULTPAC,");
            sql.AppendLine("  U.DATADES,");
            sql.AppendLine("  U.NOMCRT,");
            sql.AppendLine("  C.STA AS STACLI,");
            sql.AppendLine("  C.COB2AV,");
            sql.AppendLine("  C.VAL2AV,");
            sql.AppendLine("  C.PRZVALCART,");
            sql.AppendLine("  C.CODCLI ");
            sql.AppendLine("FROM USUARIOVA U");
            sql.AppendLine("JOIN CLIENTEVA C");
            sql.AppendLine("  ON C.CODCLI = U.CODCLI");
            sql.AppendLine("JOIN STATUS S");
            sql.AppendLine("  ON S.STA = U.STA");

            if (!string.IsNullOrEmpty(Filtro))
                sql.AppendLine(string.Format("WHERE {0} ", Filtro));

            sql.AppendLine("ORDER BY U.NOMUSU");

            #endregion

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                USUARIO_VA Usuario = new USUARIO_VA();
                Usuario.CPF = Convert.ToString(idr["CPF"]);
                Usuario.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                Usuario.CODCRT = Convert.ToString(idr["CODCRT"]);
                Usuario.NUMDEP = Convert.ToString(idr["NUMDEP"]);
                Usuario.STA = Convert.ToString(idr["STA"]);
                Usuario.DESTA = Convert.ToString(idr["DESTA"]);
                Usuario.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                Usuario.ID = Convert.ToInt32(idr["ID"]);
                Usuario.CODPAR = Convert.ToString(idr["CODPAR"]);
                Usuario.DATINC = Convert.ToDateTime(idr["DATINC"]);
                Usuario.CODCLI = Convert.ToString(idr["CODCLI"]);
                Usuario.MAT = idr["MAT"].ToString();
                Usuario.CODFIL = idr["CODFIL"].ToString();
                Usuario.CODSET = idr["CODSET"].ToString();
                Usuario.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                Usuario.STACLI = idr["STACLI"].ToString();
                Usuario.DATGERCRT = idr["DATGERCRT"].ToString();
                Usuario.VALADES = idr["VALADES"].ToString();
                Usuario.NUMPAC = idr["NUMPAC"].ToString();
                Usuario.NUMULTPAC = idr["NUMULTPAC"].ToString();
                Usuario.DATADES = idr["DATADES"].ToString();
                Usuario.DATSTASTR = idr["DATSTA"].ToString();
                Usuario.DATATV = idr["DATATV"].ToString();
                if (!string.IsNullOrEmpty(idr["DATSTA"].ToString()))
                    Usuario.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                Usuario.COBRASEGVIA = Convert.ToBoolean(idr["COB2AV"]);
                if (!string.IsNullOrEmpty(idr["VAL2AV"].ToString()))
                    Usuario.VALORSEGVIA = Convert.ToDecimal(idr["VAL2AV"]);
                Usuario.PRZVALCART = idr["PRZVALCART"].ToString();
                Usuario.TRILHA2 = idr["TRILHA2"].ToString();
                Usuario.DTVALCART = idr["DTVALCART"].ToString();
                Usuario.NOMCRT = idr["NOMCRT"].ToString();
                ColecaoUsuario.Add(Usuario);
            }

            idr.Close();

            return ColecaoUsuario;
        }

        public List<USUARIO_VA> ColecaoUsuarioVA(int codIni, int CodFim)
        {
            List<USUARIO_VA> ColecaoUsuario;
            ColecaoUsuario = new List<USUARIO_VA>();

            Database db = new SqlDatabase(BDTELENET);
            string sql = "PROC_REL_CARTOES_NAOATIVOS";

            DbCommand cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "@CODCLIINI", DbType.Int32, codIni);
            db.AddInParameter(cmd, "@CODCLIFIM", DbType.Int32, CodFim);

            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                USUARIO_VA Usuario = new USUARIO_VA();
                Usuario.NOMCLI = idr["NOMCLI"].ToString();
                Usuario.CODCLI = idr["CODCLI"].ToString();
                Usuario.NOMUSU = idr["NOMUSU"].ToString();
                Usuario.STA = idr["STA"].ToString();
                Usuario.CPF = idr["CPF"].ToString();
                Usuario.MAT = idr["MAT"].ToString();
                Usuario.DATATV = string.IsNullOrEmpty(idr["DATATV"].ToString()) ? "" : Convert.ToDateTime(idr["DATATV"]).ToShortDateString();
                if (idr["DATINC"] != DBNull.Value)
                    Usuario.DATINC = Convert.ToDateTime(idr["DATINC"]);
                Usuario.CODCRT = idr["CODCRT"].ToString();
                Usuario.NUMDEP = idr["NUMDEP"].ToString();

                ColecaoUsuario.Add(Usuario);
            }
            idr.Close();

            return ColecaoUsuario;
        }

        #endregion

        #region Get Transacoes Autorizador

        public List<CTTRANSVA> TransacoesAutorizador(USUARIO_VA Usuario, string dataIni, string dataFim)
        {
            List<CTTRANSVA> ColecaoTransacao;
            ColecaoTransacao = new List<CTTRANSVA>();

            Database db = new SqlDatabase(BDAUTORIZADOR);
            string sql = "SELECT CT.NOMEPS,T.NSUHOS, T.NSUAUT, T.TIPTRA, T.CODRTA, T.CODPS, T.CODCRT, T.VALTRA, CASE T.CODRTA WHEN 'I' THEN T.DAD ELSE '' END AS DAD, T.CODCLI, T.CPF, T.NUMDEP, T.CONTSONDA, T.DATTRA, T.PROCESSADA  " +
                "FROM CTTRANSVA T INNER JOIN CTPREST CT ON T.CODPS = CT.CODPS " +
                "WHERE T.CODCLI = @CODCLI AND T.CPF = @CPF AND T.PROCESSADA = 'N' AND T.CODRTA <> 'R' AND DATTRA >= " + "'" + dataIni + "' AND T.DATTRA <= " + "'" + dataFim + "'";

            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Usuario.CODCLI);
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                CTTRANSVA Transacao = new CTTRANSVA();

                Transacao.CODCLI = Convert.ToString(idr["CODCLI"]);
                Transacao.CODCRT = idr["CODCRT"].ToString().Trim();
                Transacao.CODPS = Convert.ToString(idr["CODPS"]);
                Transacao.NOMEPS = Convert.ToString(idr["NOMEPS"]);
                Transacao.CODRTA = Convert.ToString(idr["CODRTA"]);
                Transacao.CONTSONDA = Convert.ToString(idr["CONTSONDA"]);
                Transacao.CPF = Convert.ToString(idr["CPF"]);
                Transacao.DAD = Convert.ToString(idr["DAD"]);
                Transacao.DATTRA = Convert.ToString(idr["DATTRA"]);
                Transacao.NSUAUT = Convert.ToString(idr["NSUAUT"]);
                Transacao.NSUHOS = Convert.ToString(idr["NSUHOS"]);
                Transacao.NUMDEP = Convert.ToString(idr["NUMDEP"]);
                Transacao.PROCESSADA = Convert.ToString(idr["PROCESSADA"]);
                Transacao.TIPTRA = Convert.ToString(idr["TIPTRA"]);
                Transacao.VALTRA = Convert.ToString(idr["VALTRA"]);

                ColecaoTransacao.Add(Transacao);
            }

            idr.Close();

            return ColecaoTransacao;
        }

        public List<CTTRANSVA> TransacoesAutorizador(USUARIO_VA Usuario)
        {
            List<CTTRANSVA> ColecaoTransacao;
            ColecaoTransacao = new List<CTTRANSVA>();

            Database db = new SqlDatabase(BDAUTORIZADOR);
            string sql = "SELECT CT.NOMEPS,T.NSUHOS, T.NSUAUT, T.TIPTRA, T.CODRTA, T.CODPS, T.CODCRT, T.VALTRA, T.DAD, T.CODCLI, T.CPF, T.NUMDEP, T.CONTSONDA, T.DATTRA, T.PROCESSADA  " +
                "FROM CTTRANSVA T INNER JOIN CTPREST CT ON T.CODPS = CT.CODPS " +
                "WHERE T.CODCLI = @CODCLI AND T.CPF = @CPF AND T.PROCESSADA = 'N' AND T.CODRTA <> 'R'";

            DbCommand cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Usuario.CODCLI);
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

            IDataReader idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                CTTRANSVA Transacao = new CTTRANSVA();

                Transacao.CODCLI = Convert.ToString(idr["CODCLI"]);
                Transacao.CODCRT = Convert.ToString(idr["CODCRT"]);
                Transacao.CODPS = Convert.ToString(idr["CODPS"]);
                Transacao.NOMEPS = Convert.ToString(idr["NOMEPS"]);
                Transacao.CODRTA = Convert.ToString(idr["CODRTA"]);
                Transacao.CONTSONDA = Convert.ToString(idr["CONTSONDA"]);
                Transacao.CPF = Convert.ToString(idr["CPF"]);
                Transacao.DAD = Convert.ToString(idr["DAD"]);
                Transacao.DATTRA = Convert.ToString(idr["DATTRA"]);
                Transacao.NSUAUT = Convert.ToString(idr["NSUAUT"]);
                Transacao.NSUHOS = Convert.ToString(idr["NSUHOS"]);
                Transacao.NUMDEP = Convert.ToString(idr["NUMDEP"]);
                Transacao.PROCESSADA = Convert.ToString(idr["PROCESSADA"]);
                Transacao.TIPTRA = Convert.ToString(idr["TIPTRA"]);
                Transacao.VALTRA = Convert.ToString(idr["VALTRA"]);

                ColecaoTransacao.Add(Transacao);
            }

            idr.Close();

            return ColecaoTransacao;
        }

        public void CancelarTransacao(TRANSACVA trans)
        {
            try
            {
                CancelarTransacoesAutorizador(trans);
                CancelarTransacoesNetcard(trans);
            }
            catch
            {

                throw new Exception("ERRO AO CANCELAR AS TRANSACOES");
            }

        }

        public void CancelarTransacao(CTTRANSVA trans, string justific)
        {
            try
            {
                CancelarTransacoesAutorizador(trans, justific);
                //CancelarTransacoesNetcard(trans);
            }
            catch
            {

                throw new Exception("ERRO AO CANCELAR AS TRANSACOES");
            }

        }

        private void CancelarTransacoesAutorizador(TRANSACVA trans)
        {
            DbCommand cmd;
            Database db = new SqlDatabase(BDAUTORIZADOR);
            DbTransaction dbt = null;
            DbConnection dbc = null;
            try
            {
                dbc = db.CreateConnection();
                dbc.Open();
                dbt = dbc.BeginTransaction();


                string sql = string.Format("update CTTRANSVA set CODRTA = 'C' where DATTRA = '{0}' AND NSUHOS = {1} AND NSUAUT = {2} ",
                                                       trans.DATTRA.ToString("yyyy-MM-dd HH:mm:ss"), trans.NSUHOS, trans.NSUAUT);
                cmd = db.GetSqlStringCommand(sql.ToString());
                db.ExecuteNonQuery(cmd, dbt);


                string sql2 = "update CTCARTVA set SALDOVA = SALDOVA + @SALDOVA where CODCARTAO = @CODCARTAO ";
                cmd = db.GetSqlStringCommand(sql2.ToString());
                db.AddInParameter(cmd, "@SALDOVA", DbType.Decimal, trans.VALTRA);
                db.AddInParameter(cmd, "@CODCARTAO", DbType.String, trans.CODCRT);
                db.ExecuteNonQuery(cmd, dbt);


                dbt.Commit();

                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
            }
            catch
            {
                dbt.Rollback();

                if (dbc.State == ConnectionState.Open)
                    dbc.Close();

                throw;
            }


        }

        private void CancelarTransacoesNetcard(TRANSACVA trans)
        {
            Database db = new SqlDatabase(BDTELENET);
            DbCommand cmd;
            DbTransaction dbt = null;
            DbConnection dbc = null;
            try
            {
                dbc = db.CreateConnection();
                dbc.Open();
                dbt = dbc.BeginTransaction();


                string sql = string.Format("update TRANSACVA set CODRTA = 'C' where DATTRA = '{0}' AND NSUHOS = {1} AND NSUAUT = {2} ",
                                                                    trans.DATTRA.ToString("yyyy-MM-dd HH:mm:ss"), trans.NSUHOS, trans.NSUAUT);
                cmd = db.GetSqlStringCommand(sql.ToString());
                db.ExecuteNonQuery(cmd, dbt);


                string sql2 = "update TRANSACVA set VALTRA = VALTRA + @VALTRA where CODCLI = @CODCLI AND CPF = @CPF AND TIPTRA = @TIPTRA AND DATTRA > @DATTRA";
                cmd = db.GetSqlStringCommand(sql2.ToString());
                db.AddInParameter(cmd, "@VALTRA", DbType.Decimal, trans.VALTRA);
                db.AddInParameter(cmd, "@CODCLI", DbType.Int32, trans.CODCLI);
                db.AddInParameter(cmd, "@CPF", DbType.String, trans.CPF);
                db.AddInParameter(cmd, "@TIPTRA", DbType.Int32, 999012);
                db.AddInParameter(cmd, "@DATTRA", DbType.String, trans.DATTRA.ToString("yyyy-MM-dd HH:mm:ss"));

                db.ExecuteNonQuery(cmd, dbt);

                dbt.Commit();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
            }
            catch
            {
                dbt.Rollback();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();

                throw;
            }
        }

        private void CancelarTransacoesNetcard(CTTRANSVA trans)
        {
            Database db = new SqlDatabase(BDTELENET);
            DbCommand cmd;
            DbTransaction dbt = null;
            DbConnection dbc = null;

            try
            {
                dbc = db.CreateConnection();
                dbc.Open();
                dbt = dbc.BeginTransaction();


                string sql = string.Format("update TRANSACVA set CODRTA = 'C' where DATTRA = '{0}' AND NSUHOS = {1} AND NSUAUT = {2} ",
                                                                    trans.DATTRA, trans.NSUHOS, trans.NSUAUT);
                cmd = db.GetSqlStringCommand(sql.ToString());
                db.ExecuteNonQuery(cmd, dbt);


                string sql2 = "update TRANSACVA set VALTRA = VALTRA + @VALTRA where CODCLI = @CODCLI AND CPF = @CPF AND TIPTRA = @TIPTRA AND DATTRA > @DATTRA";
                cmd = db.GetSqlStringCommand(sql2.ToString());
                db.AddInParameter(cmd, "@VALTRA", DbType.Decimal, trans.VALTRA);
                db.AddInParameter(cmd, "@CODCLI", DbType.Int32, trans.CODCLI);
                db.AddInParameter(cmd, "@CPF", DbType.String, trans.CPF);
                db.AddInParameter(cmd, "@TIPTRA", DbType.Int32, 999012);
                db.AddInParameter(cmd, "@DATTRA", DbType.String, trans.DATTRA);

                db.ExecuteNonQuery(cmd, dbt);

                dbt.Commit();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
            }
            catch
            {
                dbt.Rollback();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
                throw;
            }
        }

        private void CancelarTransacoesAutorizador(CTTRANSVA trans, string justific)
        {
            DbCommand cmd;
            Database db = new SqlDatabase(BDAUTORIZADOR);
            DbTransaction dbt = null;
            string data = null;
            DbConnection dbc = null;
            try
            {
                dbc = db.CreateConnection();
                dbc.Open();
                dbt = dbc.BeginTransaction();

                data = Convert.ToDateTime(trans.DATTRA).ToString("yyyy-MM-dd HH:mm:ss");

                string sql = string.Format("update CTTRANSVA set CODRTA = 'C' where DATTRA = '{0}' AND NSUHOS = {1} AND NSUAUT = {2} ",
                                                       data, trans.NSUHOS, trans.NSUAUT);
                cmd = db.GetSqlStringCommand(sql);
                db.ExecuteNonQuery(cmd, dbt);


                string sql2 = "update CTCARTVA set SALDOVA = SALDOVA + @SALDOVA where CODEMPRESA = @CODCLI AND CPFTIT = @CPF AND NUMDEPEND = 0 ";
                cmd = db.GetSqlStringCommand(sql2);
                db.AddInParameter(cmd, "@SALDOVA", DbType.Decimal, trans.VALTRA);
                db.AddInParameter(cmd, "@CODCLI", DbType.Int32, trans.CODCLI);
                db.AddInParameter(cmd, "@CPF", DbType.String, trans.CPF);
                db.ExecuteNonQuery(cmd, dbt);

                dbt.Commit();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
            }
            catch
            {
                dbt.Rollback();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();

                throw;
            }
        }


        #endregion

        #region Get Usuario Ja Cadastrado (CPF Existente)

        public string CPFExistente(string CPF, int codCli)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT STA");
            sql.AppendLine("FROM USUARIOVA");
            sql.AppendLine("WHERE (CPF = @CPF AND NUMDEP = 0  AND CODCLI = @CODCLI)");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CPF", DbType.String, CPF);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            return Convert.ToString(db.ExecuteScalar(cmd));
        }

        #endregion

        #region Get Gastos

        public Double GastoHoje(USUARIOVA Usuario)
        {
            Double Gasto = 0;

            if (Usuario.CODCLI != null)
            {
                Database db = new SqlDatabase(BDAUTORIZADOR);
                string sql = "PROC_GASTO_HOJE";

                DbCommand cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "CODCLI", DbType.String, Usuario.CODCLI.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

                IDataReader idr = db.ExecuteReader(cmd);

                if (idr.Read())
                    Gasto = Convert.ToDouble(idr["GASTO_HOJE"]);

                idr.Close();

            }
            return Gasto;
        }

        public Double GastoHoje(USUARIO_VA Usuario)
        {
            Double Gasto = 0;

            if (Usuario.CODCLI != null)
            {
                Database db = new SqlDatabase(BDAUTORIZADOR);
                string sql = "PROC_GASTO_HOJE";

                DbCommand cmd = db.GetStoredProcCommand(sql);
                db.AddInParameter(cmd, "CODCLI", DbType.String, Usuario.CODCLI.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

                IDataReader idr = db.ExecuteReader(cmd);

                if (idr.Read())
                    Gasto = Convert.ToDouble(idr["GASTO_HOJE"]);

                idr.Close();
            }
            return Gasto;
        }

        public Double GastoProcessado(USUARIO_VA Usuario, DateTime DataInicio, DateTime DataFim)
        {
            Double Gasto = 0;

            if (Usuario.CODCLI != null)
            {
                Database db = new SqlDatabase(BDTELENET);
                string sql = "PROC_GASTO_PROC";

                DbCommand cmd = db.GetStoredProcCommand(sql);

                db.AddInParameter(cmd, "CODCLI", DbType.String, Usuario.CODCLI.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);
                db.AddInParameter(cmd, "DATAINI", DbType.DateTime, DataInicio);
                db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, DataFim);

                IDataReader idr = db.ExecuteReader(cmd);

                if (idr.Read())
                    Gasto = string.IsNullOrEmpty(idr["GASTO_PROC"].ToString()) ? 0 : Convert.ToDouble(idr["GASTO_PROC"]);

                idr.Close();
            }
            return Gasto;
        }

        public Double GastoProcessado(USUARIOVA Usuario, DateTime DataInicio, DateTime DataFim)
        {
            Double Gasto = 0;

            if (Usuario.CODCLI != null)
            {
                Database db = new SqlDatabase(BDTELENET);
                string sql = "PROC_GASTO_PROC";

                DbCommand cmd = db.GetStoredProcCommand(sql);

                db.AddInParameter(cmd, "CODCLI", DbType.String, Usuario.CODCLI.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);
                db.AddInParameter(cmd, "DATAINI", DbType.DateTime, DataInicio);
                db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, DataFim);

                IDataReader idr = db.ExecuteReader(cmd);

                if (idr.Read())
                    Gasto = string.IsNullOrEmpty(idr["GASTO_PROC"].ToString()) ? 0 : Convert.ToDouble(idr["GASTO_PROC"]);

                idr.Close();
            }
            return Gasto;
        }



        #endregion

        #region GET Observacoes

        public List<USUARIO_OBS> Observacoes(int codCli, string cpf)
        {
            var ColecaoObservacoes = new List<USUARIO_OBS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine(" ID, CODCLI, CPF, DATA, OBS ");
            sql.AppendLine("FROM OBSUSUVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI AND CPF = @CPF ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Observacao = new USUARIO_OBS();
                Observacao.CODCLI = codCli;
                Observacao.CPF = Convert.ToString(idr["CPF"]);
                Observacao.DATA = Convert.ToDateTime(idr["DATA"]);
                Observacao.OBS = Convert.ToString(idr["OBS"]);
                Observacao.ID = Convert.ToInt32(idr["ID"]);

                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();
            return ColecaoObservacoes;
        }

        #endregion

        #region Cartao

        public CTCARTVA CartaoVA(string CodCartao)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            string sql =
                "SELECT CODEMPRESA, CODCARTAO, STATUSU, DTSTATUSU,NOMEUSU," +
                "NUMDEPEND, CODCARTIT, CPFTIT, DTVALCART, SENHA, SALDOVA, DTVAULT, DTULTCONS " +
                "FROM CTCARTVA WHERE CODCARTAO = @CODCARTAO";

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCARTAO", DbType.String, CodCartao);
            idr = db.ExecuteReader(cmd);

            CTCARTVA Cartao;
            Cartao = new CTCARTVA();

            if (idr.Read())
            {
                Cartao.CODCARTAO = Convert.ToString(idr["CODCARTAO"]);
                Cartao.CODCARTIT = Convert.ToString(idr["CODCARTIT"]);
                Cartao.CODEMPRESA = Convert.ToString(idr["CODEMPRESA"]);
                Cartao.CPFTIT = Convert.ToString(idr["CPFTIT"]);
                Cartao.DTSTATUSU = Convert.ToString(idr["DTSTATUSU"]);
                Cartao.DTULTCONS = Convert.ToString(idr["DTULTCONS"]);
                Cartao.DTVALCART = Convert.ToString(idr["DTVALCART"]);
                Cartao.DTVAULT = Convert.ToString(idr["DTVAULT"]);
                Cartao.NOMEUSU = Convert.ToString(idr["NOMEUSU"]);
                Cartao.NUMDEPEND = Convert.ToString(idr["NUMDEPEND"]);
                Cartao.SALDOVA = Convert.ToString(idr["SALDOVA"]);
                Cartao.SENHA = Convert.ToString(idr["SENHA"]);
                Cartao.STATUSU = Convert.ToString(idr["STATUSU"]);

            }
            idr.Close();

            return Cartao;
        }

        public CTCARTVA CartaoVA(int codCli, string cpf)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            string sql =
                "SELECT CODEMPRESA, CODCARTAO, STATUSU, DTSTATUSU,NOMEUSU," +
                "NUMDEPEND, CODCARTIT, CPFTIT, DTVALCART, SENHA, SALDOVA, DTVAULT, DTULTCONS " +
                "FROM CTCARTVA WHERE CPFTIT = @CPFTIT AND CODEMPRESA = @CODEMPRESA";

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CPFTIT", DbType.String, cpf);
            db.AddInParameter(cmd, "CODEMPRESA", DbType.Int32, codCli);
            idr = db.ExecuteReader(cmd);

            CTCARTVA Cartao;
            Cartao = new CTCARTVA();

            if (idr.Read())
            {
                Cartao.CODCARTAO = Convert.ToString(idr["CODCARTAO"]);
                Cartao.CODCARTIT = Convert.ToString(idr["CODCARTIT"]);
                Cartao.CODEMPRESA = Convert.ToString(idr["CODEMPRESA"]);
                Cartao.CPFTIT = Convert.ToString(idr["CPFTIT"]);
                Cartao.DTSTATUSU = Convert.ToString(idr["DTSTATUSU"]);
                Cartao.DTULTCONS = Convert.ToString(idr["DTULTCONS"]);
                Cartao.DTVALCART = Convert.ToString(idr["DTVALCART"]);
                Cartao.DTVAULT = Convert.ToString(idr["DTVAULT"]);
                Cartao.NOMEUSU = Convert.ToString(idr["NOMEUSU"]);
                Cartao.NUMDEPEND = Convert.ToString(idr["NUMDEPEND"]);
                Cartao.SALDOVA = Convert.ToString(idr["SALDOVA"]);
                Cartao.SENHA = Convert.ToString(idr["SENHA"]);
                Cartao.STATUSU = Convert.ToString(idr["STATUSU"]);

            }
            idr.Close();

            return Cartao;
        }

        public bool COBRARSEGVIA { get; set; }

        public void Gerar2Via(USUARIO_VA usu, bool cobrarSegVia)
        {
            Database db = new SqlDatabase(BDTELENET);
            DbConnection dbc = db.CreateConnection();
            DbTransaction dbt = null;



            try
            {
                COBRARSEGVIA = cobrarSegVia;

                //Update no Autorizador
                if (AlterarCartaoVA(usu))
                {
                    dbc.Open();
                    dbt = dbc.BeginTransaction();

                    //Atualiza novo cartao
                    AtualizaCartaoUsuario(usu, dbt, db);

                    //Gerar Log 2 via Cartao    
                    GeraLogTrans(dbt, db, usu, 999007);

                    dbt.Commit();

                    //Atualiza senha da Cielo
                    //AlterarSenhaCartaoCielo(usu);
                }

            }
            catch (Exception err)
            {
                dbt.Rollback();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
                throw new Exception("Erro Camada DAL [2 Via CartaoVA] " + err);
            }
            finally
            {
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
            }
        }

        public string Cancelar2Via(string codCrt, out string codRevert)
        {
            codRevert = "";
            IDataReader idr;
            Database db = new SqlDatabase(BDTELENET);
            DbCommand cmd = db.GetStoredProcCommand("PROC_REVERTE_2AVIA");
            var msg = string.Empty;

            db.AddInParameter(cmd, "SISTEMA", DbType.Int16, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "CODCRT", DbType.String, codCrt);            

            idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                msg = idr["RETORNO"].ToString();
                if (msg == "OK")
                    codRevert = UtilSIL.MascaraCartao(idr["CARTAO"].ToString(), 17);
            }

            idr.Close();
            return msg;
        }

        private static void AtualizaCartaoUsuario(USUARIO_VA usu, DbTransaction dbt, Database db)
        {
            DbCommand cmd = db.GetSqlStringCommand("UPDATE USUARIOVA SET TRILHA2 = @TRILHA2, CODCRT = @CODCRT, DTVALCART = @DTVALCART, GERCRT = @GERCRT, DATGERCRT = @DATGERCRT WHERE ID = @ID ");
            db.AddInParameter(cmd, "TRILHA2", DbType.String, usu.TRILHA2);
            db.AddInParameter(cmd, "CODCRT", DbType.String, usu.CODCRT);
            db.AddInParameter(cmd, "DTVALCART", DbType.String, usu.DTVALCART);
            db.AddInParameter(cmd, "GERCRT", DbType.String, usu.GERCRT);
            db.AddInParameter(cmd, "ID", DbType.String, usu.ID);

            if (!string.IsNullOrEmpty(usu.DATGERCRT))
                db.AddInParameter(cmd, "DATGERCRT", DbType.DateTime, usu.DATGERCRT);
            else
                db.AddInParameter(cmd, "DATGERCRT", DbType.DateTime, DBNull.Value);

            db.ExecuteNonQuery(cmd, dbt);

                        
        }

        public void UpdatePARAM()
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            string sql;
            DbCommand cmd;

            db = db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                sql = "UPDATE PARAMVA SET VAL = (VAL + 1) WHERE ID0 = 'NUMCRT'";
                cmd = db.GetSqlStringCommand(sql);
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch (Exception)
            {
                dbt.Rollback();
                throw;
            }
            finally
            {
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
            }
        }

        public Hashtable RetornaParamVA()
        {
            UpdatePARAM();

            Database db;
            DbCommand cmd;

            db = db = new SqlDatabase(BDTELENET);
            cmd = db.GetSqlStringCommand("SELECT ID0 as id, VAL as valor FROM PARAMVA WHERE ID0 in ( 'BIN', 'NUMCRT','CODOPENET','TRILHA_REDECARD')");
            var idr = db.ExecuteReader(cmd);

            Hashtable paramVA = new Hashtable();

            while (idr.Read())
            {
                if (idr["id"].ToString() == "BIN")
                    paramVA.Add("BIN", idr["valor"].ToString());
                else
                    if (idr["id"].ToString() == "NUMCRT")
                        paramVA.Add("NUMCRT", idr["valor"].ToString());
                    else
                        if (idr["id"].ToString() == "CODOPENET")
                            paramVA.Add("CODOPENET", idr["valor"].ToString());
                        else
                            if (idr["id"].ToString() == "TRILHA_REDECARD")
                                paramVA.Add("TRILHA_REDECARD", idr["valor"].ToString());
            }

            if (!idr.IsClosed)
                idr.Close();

            return paramVA;
        }

        public string CancelarCartoes(string codCrt)
        {
            IDataReader idr;
            Database db = new SqlDatabase(BDTELENET);
            DbCommand cmd = db.GetStoredProcCommand("PROC_CANC_CARTAO");
            var msg = string.Empty;

            db.AddInParameter(cmd, "SISTEMA", DbType.Int16, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "CARTAO", DbType.String, codCrt);
            db.AddInParameter(cmd, "IDFUNC", DbType.Int16, FOperador.ID_FUNC);

            idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                msg = idr["RETORNO"].ToString();
            }

            idr.Close();
            return msg;
        }

        public bool ReincluiCrtParamVa()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'REINCLUICRT'";
            var cmd = db.GetSqlStringCommand(sql);
            var ret = (Convert.ToString(db.ExecuteScalar(cmd)) == "S" || db.ExecuteScalar(cmd) == null);
            if (cmd.Connection.State == ConnectionState.Open)
                cmd.Connection.Close();
            return ret;
        }

        public int TamanhoSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'TAMSENHA'";
            var cmd = db.GetSqlStringCommand(sql);
            var tamSenha = Convert.ToInt16(db.ExecuteScalar(cmd));
            return tamSenha == 0 ? 1 : tamSenha;
        }

        public void TrocarSenhaCartao(USUARIOVA usu)
        {
            try
            {
                Database db = new SqlDatabase(BDAUTORIZADOR);
                var sql = string.Format("update CTCARTVA set SENHA = '{0}' WHERE CODCARTAO = '{1}'  ", usu.SENHA, usu.CODCRT);

                var cmd = db.GetSqlStringCommand(sql);
                db.ExecuteNonQuery(cmd);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog("UPDATE CTCARTVA (TrocarSenhaCartao)", FOperador, cmd);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string ValidadeSenha(string codcrt, out string dtSenha)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DTSENHA, DTEXPSENHA FROM USUARIOVA WHERE CODCRT = '" + codcrt + "'";
            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);
            var retorno = string.Empty;
            dtSenha = string.Empty;
            while (idr.Read())
            {
                if (idr["DTEXPSENHA"] != DBNull.Value)
                    retorno = Convert.ToDateTime(idr["DTEXPSENHA"]).ToShortDateString();
                if (idr["DTSENHA"] != DBNull.Value)
                    dtSenha = Convert.ToDateTime(idr["DTSENHA"]).ToShortDateString(); 
            }            
            return retorno;
        }

        public int DiasParaRenovarSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public void RenovarAcesso(USUARIO_VA usu)
        {
            var dataRenavacao = DateTime.Now.AddDays(DiasParaRenovarSenha()).ToString("yyyyMMdd");
            try
            {
                RenovarAcessoVa(usu, dataRenavacao);
                RenovarAcessoPj(usu, dataRenavacao);                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RenovarAcessoVa(USUARIO_VA usu, string dataRenavacao)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = string.Format("UPDATE USUARIOVA SET DTEXPSENHA = '{0}', SENUSU = '{1}', DTSENHA = NULL, QTDEACESSOINV = 0 WHERE CPF = '{2}'  ", dataRenavacao, usu.SENHA, usu.CPF);
            DbCommand cmd;
            try
            {
                cmd = db.GetSqlStringCommand(sql.ToString());
                db.ExecuteNonQuery(cmd);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog("UPDATE USUARIOVA (Renovar Acesso Usuario)", FOperador, cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RenovarAcessoPj(USUARIO_VA usu, string dataRenavacao)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = string.Format("UPDATE USUARIO SET DTEXPSENHA = '{0}', SENUSU = '{1}', DTSENHA = NULL, QTDEACESSOINV = 0 WHERE CPF = '{2}'  ", dataRenavacao, usu.SENHA, usu.CPF);
            DbCommand cmd;
            try
            {
                cmd = db.GetSqlStringCommand(sql.ToString());
                db.ExecuteNonQuery(cmd);
                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog("UPDATE USUARIO (Renovar Acesso Usuario)", FOperador, cmd);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ResetSenhaCartao(USUARIO_VA usu)
        {
            try
            {
                AlterarSenhaAutorizador(usu);
                AlterarSenhaNetcard(usu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AlterarSenhaNetcard(USUARIO_VA usu)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = string.Format("update USUARIOVA set SENHA = '{0}' WHERE ID = '{1}'  ", usu.SENHA, usu.ID);

            DbCommand cmd;

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);

        }

        private void AlterarSenhaAutorizador(USUARIO_VA usu)
        {

            Database db = new SqlDatabase(BDAUTORIZADOR);
            var sql = "update CTCARTVA set SENHA = @SENHA WHERE CODCARTAO = @CODCARTAO ";

            DbCommand cmd;

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCARTAO", DbType.String, usu.CODCRT);
            db.AddInParameter(cmd, "SENHA", DbType.String, usu.SENHA);


            db.ExecuteNonQuery(cmd);

            //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
            UtilSIL.GravarLog("UPDATE CTCARTVA (ResetSenhaCartao)", FOperador, cmd);

        }

        #endregion

        #region CRUD Usuario

        public bool ExisteTransacao(USUARIOVA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);

            StringBuilder sql;
            sql = new StringBuilder();
            sql.AppendLine("SELECT COUNT(CODCLI)");
            sql.AppendLine("FROM TRANSACVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI");
            sql.AppendLine("  AND CPF = @CPF");
            sql.AppendLine("  AND TIPTRA IN (51007, 51006, 51009, 51032, 51220, 51221, 51225, 51231, 51233, 51061, 51062)");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Usuario.CODCLI);
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

            return (Convert.ToInt32(db.ExecuteScalar(cmd)) > 0);
        }

        public bool ExisteTransacao(USUARIO_VA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);

            StringBuilder sql;
            sql = new StringBuilder();
            sql.AppendLine(" SELECT TOP 10(DATTRA)");
            sql.AppendLine(" FROM TRANSACVA");
            sql.AppendLine(" WHERE CODCLI = @CODCLI");
            sql.AppendLine(" AND CPF = @CPF");
            sql.AppendLine(" AND TIPTRA IN (51007, 51006, 51009, 51032, 51220, 51221, 51225, 51231, 51233, 51061, 51062)");

            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Usuario.CODCLI);
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

            IDataReader idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {
                idr.Close();
                return true;
            }
            else
            {
                idr.Close();
                return false;
            }            
        }

        // Status diferente Ativo mais de sete dias
        public bool StatusInativo(USUARIOVA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);

            StringBuilder sql;
            sql = new StringBuilder();
            sql.AppendLine("SELECT ID");
            sql.AppendLine("FROM UsuarioVA");
            sql.AppendLine("WHERE ID = @ID");
            sql.AppendLine("  AND (STA <> @STA)");
            sql.AppendLine("  AND (CONVERT(CHAR(10), DATSTA, 102) <= CONVERT(CHAR(10), GETDATE() - 7, 102))");

            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            int IDUsuario;

            // Usuario
            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID", DbType.Int32, Usuario.ID);
            db.AddInParameter(cmd, "STA", DbType.String, ConstantesSIL.StatusAtivo);
            IDUsuario = Convert.ToInt32(db.ExecuteScalar(cmd));

            return (IDUsuario != 0);
        }

        // Status diferente Ativo mais de sete dias
        public bool StatusInativo(USUARIO_VA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);

            StringBuilder sql;
            sql = new StringBuilder();
            sql.AppendLine("SELECT ID");
            sql.AppendLine("FROM UsuarioVA");
            sql.AppendLine("WHERE ID = @ID");
            sql.AppendLine("  AND (STA <> @STA)");
            sql.AppendLine("  AND (CONVERT(CHAR(10), DATSTA, 102) <= CONVERT(CHAR(10), GETDATE() - 7, 102))");

            DbCommand cmd = db.GetSqlStringCommand(sql.ToString());
            int IDUsuario;

            // Usuario
            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID", DbType.Int32, Usuario.ID);
            db.AddInParameter(cmd, "STA", DbType.String, ConstantesSIL.StatusAtivo);
            IDUsuario = Convert.ToInt32(db.ExecuteScalar(cmd));

            return (IDUsuario != 0);
        }

        public bool Excluir(USUARIOVA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();

            var sql = string.Format("DELETE USUARIOVA WHERE CODCLI = @CODCLI AND CPF = @CPF");
            var cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Usuario.CODCLI);
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

            // Controle Transacao
            dbc.Open();
            var dbt = dbc.BeginTransaction();

            int LinhasAfetadas;
            try
            {
                /* Aplicar Regras Exclusao */
                LinhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                //Gera log exclusao cartao titular
                GeraLogTrans(dbt, db, Usuario, 999009);

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

            /* Sucesso :: Regras Autorizador */

            // Excluir Cartao AUTORIZADOR
            if (LinhasAfetadas > 0)
                ExcluirCartoesVATitular(Usuario.CODCLI.CODCLI, Usuario.CPF);

            return true;
        }

        public bool Excluir(USUARIO_VA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);
            DbConnection dbc = db.CreateConnection();
            DbCommand cmd;

            string sql = string.Format("PROC_EXCLUIR_CARTAO ");
            cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, 1);
            db.AddInParameter(cmd, "@CARTAO", DbType.String, Usuario.CODCRT);

            // Controle Transacao
            dbc.Open();
            DbTransaction dbt = dbc.BeginTransaction();

            try
            {
                string retorno = "";
                string mensagem = "";
                
                DataTable dt = new DataTable();

                dt.Load(db.ExecuteReader(cmd, dbt));

                retorno = dt.Rows[0]["RETORNO"].ToString();
                mensagem = dt.Rows[0]["MENSAGEM"].ToString();

                if(retorno.ToLower() != "ok")
                    throw new Exception(mensagem);
                
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
            
            return true;
        }

        public bool ExcluirUsuCarga(USUARIO_VA Usuario)
        {
            Database db = new SqlDatabase(BDTELENET);
            DbConnection dbc = db.CreateConnection();
            DbCommand cmd;

            string sql = string.Format("DELETE USUARIOVA WHERE CODCLI = @CODCLI AND CPF = @CPF");
            cmd = db.GetSqlStringCommand(sql);

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Usuario.CODCLI);
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);

            // Controle Transacao
            dbc.Open();
            DbTransaction dbt = dbc.BeginTransaction();

            int LinhasAfetadas = 0;
            try
            {
                /* Aplicar Regras Exclusao */
                LinhasAfetadas = db.ExecuteNonQuery(cmd, dbt);

                //Gera log exclusao cartao titular
                //GeraLogTrans(dbt, db, Usuario, 999009);

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

            /* Sucesso :: Regras Autorizador */
            // Excluir Cartao AUTORIZADOR
            if (LinhasAfetadas > 0)
                ExcluirCartoesVATitular(Convert.ToInt32(Usuario.CODCLI), Usuario.CPF);

            return true;
        }

        public bool InserirUsuario(USUARIO_VA Usuario)
        {
            Database db;

            StringBuilder sbCampos = new StringBuilder();
            StringBuilder sbParametros = new StringBuilder();
            db = new SqlDatabase(BDTELENET);            
            
            DbCommand cmd = db.GetStoredProcCommand("PROC_INSERE_CARTAO");
            DbConnection dbc = db.CreateConnection();
            DbTransaction dbt;

            #region Add Parameters

            // Usuario VA
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, Convert.ToInt32(Usuario.CODCLI));
            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);
            db.AddInParameter(cmd, "NUMDEP", DbType.String, Usuario.NUMDEP);

            if (!string.IsNullOrEmpty(Usuario.NUMDEP) && Usuario.NUMDEP != "0")
                db.AddInParameter(cmd, "CODPAR", DbType.Int16, Convert.ToInt16(Usuario.CODPAR));
            else
                db.AddInParameter(cmd, "CODPAR", DbType.Int32, DBNull.Value);            

            if (Usuario.CODFIL != string.Empty)
                db.AddInParameter(cmd, "CODFIL", DbType.Int16, Usuario.CODFIL);
            else
                db.AddInParameter(cmd, "CODFIL", DbType.Int16, System.DBNull.Value);
            db.AddInParameter(cmd, "NOMUSU", DbType.String, UtilSIL.RemoverAcentos(Usuario.NOMUSU));
            db.AddInParameter(cmd, "DATINC", DbType.DateTime, DBNull.Value);
            if (Usuario.CODSET != string.Empty)
                db.AddInParameter(cmd, "CODSET", DbType.String, Usuario.CODSET);
            else
                db.AddInParameter(cmd, "CODSET", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "MAT", DbType.String, Usuario.MAT);

            if (Usuario.DATNAS == DateTime.MinValue)
                db.AddInParameter(cmd, "DATNAS", DbType.DateTime, null);
            else
                db.AddInParameter(cmd, "DATNAS", DbType.DateTime, Usuario.DATNAS);
            db.AddInParameter(cmd, "PAI", DbType.String, Usuario.PAI);
            db.AddInParameter(cmd, "MAE", DbType.String, Usuario.MAE);
            db.AddInParameter(cmd, "CEL", DbType.String, Usuario.CEL);
            db.AddInParameter(cmd, "TEL", DbType.String, Usuario.TEL);
            db.AddInParameter(cmd, "SEXO", DbType.String, Usuario.SEXO);
            db.AddInParameter(cmd, "EMA", DbType.String, Usuario.EMA);
            db.AddInParameter(cmd, "RG", DbType.String, Usuario.RG);
            db.AddInParameter(cmd, "ORGEXPRG", DbType.String, Usuario.ORGEXPRG);
            db.AddInParameter(cmd, "NATURALIDADE", DbType.String, Usuario.NATURALIDADE);
            db.AddInParameter(cmd, "NACIONALIDADE", DbType.String, Usuario.NACIONALIDADE);
            db.AddInParameter(cmd, "ENDUSU", DbType.String, Usuario.ENDUSU);
            db.AddInParameter(cmd, "ENDNUMUSU", DbType.String, Usuario.ENDNUMUSU);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, Usuario.ENDCPL);
            db.AddInParameter(cmd, "BAIRRO", DbType.String, Usuario.BAIRRO);
            db.AddInParameter(cmd, "LOCALIDADE", DbType.String, Usuario.LOCALIDADE);
            db.AddInParameter(cmd, "UF", DbType.String, Usuario.UF);
            db.AddInParameter(cmd, "CEP", DbType.String, Usuario.CEP);
            db.AddInParameter(cmd, "ENDUSUCOM", DbType.String, Usuario.ENDUSUCOM);
            db.AddInParameter(cmd, "ENDNUMCOM", DbType.String, Usuario.ENDNUMUSUCOM);
            db.AddInParameter(cmd, "ENDCPLCOM", DbType.String, Usuario.ENDCPLCOM);
            db.AddInParameter(cmd, "BAIRROCOM", DbType.String, Usuario.BAIRROCOM);
            db.AddInParameter(cmd, "LOCALIDADECOM", DbType.String, Usuario.LOCALIDADECOM);
            db.AddInParameter(cmd, "UFCOM", DbType.String, Usuario.UFCOM);
            db.AddInParameter(cmd, "CEPCOM", DbType.String, Usuario.CEPCOM);
            db.AddInParameter(cmd, "TELCOM", DbType.String, Usuario.TELCOM);
            db.AddInParameter(cmd, "CARGPADVA", DbType.Decimal, Usuario.CARGPADVA);
            db.AddInParameter(cmd, "CODOPE", DbType.Int32, FOperador.ID_FUNC);

            #endregion

            // Controle Transacao
            dbc.Open();            
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                               
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
                return true;
            }
            catch
            {
                dbt.Rollback();
                if (dbc.State == ConnectionState.Open)
                    dbc.Close();
                return false;                
                throw new Exception("Erro ao inserir o novo Usuario");
            }
            finally
            {
                dbc.Close();
            }            
        }

        //public bool InserirUsuario(USUARIO_VA Usuario)
        //{
        //    Database db;

        //    StringBuilder sbCampos = new StringBuilder();
        //    StringBuilder sbParametros = new StringBuilder();
        //    db = new SqlDatabase(BDTELENET);           

        //    sbCampos.Append("CODCLI, CPF, NUMDEP, CODCRT, CODFIL, NOMUSU, DATINC, CODSET, MAT, NOMCRT, VCARGAUTO, ");
        //    sbCampos.Append("STA, DATSTA, CARGPADVA, ULTCARGVA, SENHA, DTVALCART, TRILHA2, GERCRT, NUMULTPAC, NUMPAC, SENUSU, DTEXPSENHA");
            
        //    #region Parâmetros
        //    sbParametros.Append("@CODCLI, @CPF, @NUMDEP, @CODCRT, @CODFIL, @NOMUSU, @DATINC, @CODSET, @MAT, @NOMCRT, @VCARGAUTO, ");
        //    sbParametros.Append("@STA, @DATSTA, @CARGPADVA, @ULTCARGVA, @SENHA, @DTVALCART, @TRILHA2, @GERCRT, @NUMULTPAC, @NUMPAC, @SENUSU, @DTEXPSENHA");            
            
        //    #endregion

        //    #region NETCARD

        //    string sql = string.Format("INSERT INTO USUARIOVA ({0}) VALUES ({1})", sbCampos.ToString(), sbParametros.ToString());
        //    DbCommand cmd = db.GetSqlStringCommand(sql);            
        //    DbConnection dbc = db.CreateConnection();
        //    DbTransaction dbt;

        //    #region Add Parameters

        //    // Usuario VA
        //    db.AddInParameter(cmd, "CODCLI", DbType.Int32, Convert.ToInt32(Usuario.CODCLI));
        //    db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);
        //    db.AddInParameter(cmd, "NUMDEP", DbType.String, Usuario.NUMDEP);
        //    if (Usuario.CODFIL != string.Empty)
        //        db.AddInParameter(cmd, "CODFIL", DbType.Int16, Usuario.CODFIL);
        //    else
        //        db.AddInParameter(cmd, "CODFIL", DbType.Int16, System.DBNull.Value);
        //    db.AddInParameter(cmd, "NOMUSU", DbType.String, UtilSIL.RemoverAcentos(Usuario.NOMUSU));
        //    db.AddInParameter(cmd, "DATINC", DbType.DateTime, Usuario.DATINC);
        //    if (Usuario.CODSET != string.Empty)
        //        db.AddInParameter(cmd, "CODSET", DbType.String, Usuario.CODSET);
        //    else
        //        db.AddInParameter(cmd, "CODSET", DbType.String, DBNull.Value);
        //    db.AddInParameter(cmd, "MAT", DbType.String, Usuario.MAT);
        //    db.AddInParameter(cmd, "NOMCRT", DbType.String, Usuario.NOMCRT);
        //    db.AddInParameter(cmd, "VCARGAUTO", DbType.Decimal, 0);
        //    db.AddInParameter(cmd, "STA", DbType.String, Usuario.STA);
        //    db.AddInParameter(cmd, "DATSTA", DbType.DateTime, Usuario.DATSTA);
        //    db.AddInParameter(cmd, "CARGPADVA", DbType.Decimal, Usuario.CARGPADVA);
        //    db.AddInParameter(cmd, "ULTCARGVA", DbType.Int32, Usuario.ULTCARGVA);
        //    db.AddInParameter(cmd, "CODCRT", DbType.String, Usuario.CODCRT);
        //    db.AddInParameter(cmd, "DTVALCART", DbType.String, Usuario.DTVALCART);
        //    db.AddInParameter(cmd, "SENHA", DbType.String, Usuario.SENHA);
        //    db.AddInParameter(cmd, "SENUSU", DbType.String, Usuario.SENUSU);
        //    db.AddInParameter(cmd, "DTEXPSENHA", DbType.String, DateTime.Now.AddDays(DiasParaRenovarSenha()).ToString("yyyyMMdd"));
        //    db.AddInParameter(cmd, "TRILHA2", DbType.String, Usuario.TRILHA2);
        //    db.AddInParameter(cmd, "GERCRT", DbType.String, Usuario.GERCRT);
        //    db.AddInParameter(cmd, "NUMULTPAC", DbType.Int16, 0);
        //    db.AddInParameter(cmd, "NUMPAC", DbType.Int16, 1);            

        //    #endregion

        //    // Controle Transacao
        //    dbc.Open();            
        //    dbt = dbc.BeginTransaction();

        //    try
        //    {   // Linha Afetada                                               
        //        db.ExecuteNonQuery(cmd, dbt);
        //        // Cartao AUTORIZADOR


        //        if (Usuario.TEMCAD)
        //            AlterarDadosComplUsu(Usuario);
        //        else
        //            InserirDadosUsu(Usuario);
        //        InserirCartaoVA(Usuario);
        //        //Gerar Log Transacao de inclusao de cartao 
        //        GeraLogTrans(dbt, db, Usuario, 999001);
        //        GeraLogTrans(dbt, db, Usuario, 999012);
        //        dbt.Commit();

        //        AlterarSenhaCartaoCielo(Usuario);
        //    }
        //    catch
        //    {
        //        dbt.Rollback();

        //        if (dbc.State == ConnectionState.Open)
        //            dbc.Close();
                
        //        throw new Exception("Erro ao inserir o novo Usuario");
        //    }
        //    finally
        //    {
        //        dbc.Close();
        //    }

        //    #endregion

        //    return true;
        //}

        public bool InserirDadosUsu(USUARIO_VA Usuario)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);
            sql = "INSERT INTO CADUSUARIO " +
                  "(CPF, DATNAS, PAI, MAE, CEL, TEL, SEXO, EMA, RG, ORGEXPRG, NATURALIDADE, NACIONALIDADE, ENDUSU, ENDNUMUSU, ENDCPL, " +
                  "BAIRRO, LOCALIDADE, UF, CEP, ENDUSUCOM, ENDNUMCOM, ENDCPLCOM, BAIRROCOM, LOCALIDADECOM, UFCOM, CEPCOM, TELCOM ) " +

                  "VALUES (@CPF, @DATNAS, @PAI, @MAE, @CEL, @TEL, @SEXO, @EMA, @RG, @ORGEXPRG, @NATURALIDADE, @NACIONALIDADE, @ENDUSU, @ENDNUMUSU, @ENDCPL, " +
                  "@BAIRRO, @LOCALIDADE, @UF, @CEP, @ENDUSUCOM, @ENDNUMCOM, @ENDCPLCOM, @BAIRROCOM, @LOCALIDADECOM, @UFCOM, @CEPCOM, @TELCOM )";

            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);
            if (Usuario.DATNAS == DateTime.MinValue)
                db.AddInParameter(cmd, "DATNAS", DbType.DateTime, null);
            else
                db.AddInParameter(cmd, "DATNAS", DbType.DateTime, Usuario.DATNAS);
            db.AddInParameter(cmd, "PAI", DbType.String, Usuario.PAI);
            db.AddInParameter(cmd, "MAE", DbType.String, Usuario.MAE);
            db.AddInParameter(cmd, "CEL", DbType.String, Usuario.CEL);
            db.AddInParameter(cmd, "TEL", DbType.String, Usuario.TEL);
            db.AddInParameter(cmd, "SEXO", DbType.String, Usuario.SEXO);
            db.AddInParameter(cmd, "EMA", DbType.String, Usuario.EMA);
            db.AddInParameter(cmd, "RG", DbType.String, Usuario.RG);
            db.AddInParameter(cmd, "ORGEXPRG", DbType.String, Usuario.ORGEXPRG);
            db.AddInParameter(cmd, "NATURALIDADE", DbType.String, Usuario.NATURALIDADE);
            db.AddInParameter(cmd, "NACIONALIDADE", DbType.String, Usuario.NACIONALIDADE);
            db.AddInParameter(cmd, "ENDUSU", DbType.String, Usuario.ENDUSU);
            db.AddInParameter(cmd, "ENDNUMUSU", DbType.String, Usuario.ENDNUMUSU);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, Usuario.ENDCPL);
            db.AddInParameter(cmd, "BAIRRO", DbType.String, Usuario.BAIRRO);
            db.AddInParameter(cmd, "LOCALIDADE", DbType.String, Usuario.LOCALIDADE);
            db.AddInParameter(cmd, "UF", DbType.String, Usuario.UF);
            db.AddInParameter(cmd, "CEP", DbType.String, Usuario.CEP);
            db.AddInParameter(cmd, "ENDUSUCOM", DbType.String, Usuario.ENDUSUCOM);
            db.AddInParameter(cmd, "ENDNUMCOM", DbType.String, Usuario.ENDNUMUSUCOM);
            db.AddInParameter(cmd, "ENDCPLCOM", DbType.String, Usuario.ENDCPLCOM);
            db.AddInParameter(cmd, "BAIRROCOM", DbType.String, Usuario.BAIRROCOM);
            db.AddInParameter(cmd, "LOCALIDADECOM", DbType.String, Usuario.LOCALIDADECOM);
            db.AddInParameter(cmd, "UFCOM", DbType.String, Usuario.UFCOM);
            db.AddInParameter(cmd, "CEPCOM", DbType.String, Usuario.CEPCOM);
            db.AddInParameter(cmd, "TELCOM", DbType.String, Usuario.TELCOM);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch
            {
                dbt.Rollback();
                throw new Exception("Erro ao inserir o dados do usuario.");
            }
            finally
            {
                dbc.Close();
            }
        }

        //public bool InserirCartaoVA(USUARIO_VA Titular)
        //{
        //    Database db;
        //    string sql;
        //    DbCommand cmd;
        //    DbConnection dbc;
        //    DbTransaction dbt;

        //    db = new SqlDatabase(BDAUTORIZADOR);
        //    sql = "INSERT INTO CTCARTVA (CODEMPRESA, CODCARTAO, STATUSU, DTSTATUSU, NOMEUSU, NUMDEPEND, CPFTIT," +
        //          "DTVALCART, SENHA ,SALDOVA) " +

        //          "VALUES (@CODEMPRESA, @CODCARTAO, @STATUSU, @DTSTATUSU, @NOMEUSU, @NUMDEPEND, @CPFTIT," +
        //          "@DTVALCART, @SENHA, @SALDOVA)";

        //    cmd = db.GetSqlStringCommand(sql);
        //    dbc = db.CreateConnection();
        //    db.AddInParameter(cmd, "CODEMPRESA", DbType.String, Titular.CODCLI.ToString().PadLeft(5, '0'));
        //    db.AddInParameter(cmd, "CODCARTAO", DbType.String, Titular.CODCRT);
        //    db.AddInParameter(cmd, "STATUSU", DbType.String, Titular.STA);
        //    db.AddInParameter(cmd, "DTSTATUSU", DbType.DateTime, Titular.DATSTA);
        //    db.AddInParameter(cmd, "NOMEUSU", DbType.String, Titular.NOMCRT);
        //    db.AddInParameter(cmd, "NUMDEPEND", DbType.String, "00");
        //    db.AddInParameter(cmd, "CPFTIT", DbType.String, Titular.CPF);
        //    db.AddInParameter(cmd, "DTVALCART", DbType.String, Titular.DTVALCART);
        //    db.AddInParameter(cmd, "SENHA", DbType.String, Titular.SENHA);
        //    db.AddInParameter(cmd, "SALDOVA", DbType.Double, 0);
        //    // Controle Transacao
        //    dbc.Open();
        //    dbt = dbc.BeginTransaction();

        //    try
        //    {   // Linha Afetada                                
        //        int LinhaAfetada;
        //        LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
        //        dbt.Commit();
        //        return (LinhaAfetada == 1);
        //    }
        //    catch
        //    {
        //        dbt.Rollback();
        //        throw new Exception("Erro ao inserir o novo Usuario no Autorizador");
        //    }
        //    finally
        //    {
        //        dbc.Close();
        //    }
        //}

        //public string MontaTrilha(string dtValCart, int classe, int NumVia)
        //{
        //    Database db = new SqlDatabase(BDTELENET);
        //    var cmd = db.GetSqlStringCommand("SELECT dbo.MontaTrilha2 (@SISTEMA, @DATA_VALIDADE, @CLASSE, @NUMVIA, @SENHA, @CHIP)");
        //    db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
        //    db.AddInParameter(cmd, "DATA_VALIDADE", DbType.String, dtValCart);
        //    db.AddInParameter(cmd, "CLASSE", DbType.Int32, classe);
        //    db.AddInParameter(cmd, "NUMVIA", DbType.Int32, NumVia);
        //    db.AddInParameter(cmd, "SENHA", DbType.String, ConstantesSIL.FlgSim);
        //    db.AddInParameter(cmd, "CHIP", DbType.String, ConstantesSIL.FlgNao);
        //    return Convert.ToString(db.ExecuteScalar(cmd));
        //}

        public bool VerificaSolicitacao2ViaNoDia(string codCrt, int codCli)
        {
            string query = @"SELECT * FROM TRANSACVA WHERE TIPTRA = 999007 AND CODCRT = '" + codCrt + "' AND CODCLI = " + codCli + " AND CONVERT(VARCHAR, DATTRA, 112)  =  CONVERT(VARCHAR, getdate(), 112)";

            SqlConnection conn = new SqlConnection(BDTELENET);
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                DataTable dt = new DataTable();
                
                conn.Open();
                dt.Load(cmd.ExecuteReader());
                conn.Close();

                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();

                return true;
            }
        }

        public string MontaTrilha(int sistema, int codCli, string cpf, int numDep)
        {
            SqlConnection conn = null;
            SqlDataReader dr = null;
            var retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_TRILHA_SEGVIA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = sistema;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpf;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = numDep;

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    retorno = Convert.ToString(dr["RETORNO"]);
                    if (retorno == "OK")
                    {
                        retorno = Convert.ToString(dr["TRILHA2"]);
                    }
                    else
                    {
                        retorno = Convert.ToString(dr["MENSAGEM"]);
                        throw new Exception(retorno);
                    }
                }
                return retorno;
            }
            finally
            {
                if (dr != null) { dr.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }


        public void Atualizar(USUARIO_VA Usuario, bool flagSTA)
        {
            Database db;
            Database dbBairro;
            Database dbLocalidde;
            StringBuilder sbCampos;
            DbCommand cmd;
            DbConnection dbc;
            

            db = new SqlDatabase(BDTELENET); 
            dbBairro = new SqlDatabase(BDTELENET);
            dbLocalidde = new SqlDatabase(BDTELENET);
            sbCampos = new StringBuilder();

            #region Campos

            // Usuario
            sbCampos.Append("NOMUSU = @NOMUSU,");
            sbCampos.Append("NOMCRT = @NOMCRT,");
            sbCampos.Append("CODPAR = @CODPAR,");
            sbCampos.Append("MAT = @MAT,");
            sbCampos.Append("CODFIL = @CODFIL,");
            sbCampos.Append("CODSET = @CODSET,");
            sbCampos.Append("CARGPADVA = @CARGPADVA,");
            sbCampos.Append("STA = @STA,");
            sbCampos.Append("DATSTA = @DATSTA");

            #endregion

            #region SQL

            string sql = string.Format("UPDATE USUARIOVA SET {0} WHERE ID = @ID", sbCampos.ToString());
            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection(); 
            DbConnection dbcBairro = dbBairro.CreateConnection();
            DbConnection dbcLocalidade = dbLocalidde.CreateConnection();

            // Usuario
            db.AddInParameter(cmd, "NOMUSU", DbType.String, UtilSIL.RemoverAcentos(Usuario.NOMUSU));
            db.AddInParameter(cmd, "NOMCRT", DbType.String, Usuario.NOMCRT);
            db.AddInParameter(cmd, "MAT", DbType.String, Usuario.MAT);

            if (!string.IsNullOrEmpty(Usuario.CODFIL))
                db.AddInParameter(cmd, "CODFIL", DbType.Int32, Usuario.CODFIL);
            else
                db.AddInParameter(cmd, "CODFIL", DbType.Int32, DBNull.Value);

            if (!string.IsNullOrEmpty(Usuario.CODSET))
                db.AddInParameter(cmd, "CODSET", DbType.String, Usuario.CODSET);
            else
                db.AddInParameter(cmd, "CODSET", DbType.String, DBNull.Value);

            db.AddInParameter(cmd, "CARGPADVA", DbType.Decimal, Usuario.CARGPADVA);
            db.AddInParameter(cmd, "STA", DbType.String, Usuario.STA);

            if (flagSTA)
                db.AddInParameter(cmd, "DATSTA", DbType.DateTime, DateTime.Now);
            else
                db.AddInParameter(cmd, "DATSTA", DbType.DateTime, Usuario.DATSTA);

            if (!string.IsNullOrEmpty(Usuario.NUMDEP) && Usuario.NUMDEP != "0")
                db.AddInParameter(cmd, "CODPAR", DbType.Int16, Convert.ToInt16(Usuario.CODPAR));
            else
                db.AddInParameter(cmd, "CODPAR", DbType.Int32, DBNull.Value);
            
            db.AddInParameter(cmd, "ID", DbType.Int32, Usuario.ID);

            #endregion

            // Controle Transacao
            dbc.Open();
            DbTransaction dbt = dbc.BeginTransaction();

            try
            {
                // Alteracao Status => Alterar Data Ultima Alteracao Status
                if (flagSTA)
                {
                    Usuario.DATSTA = DateTime.Now;
                    if (Usuario.STA == "01" || Usuario.STA == "06" || Usuario.STA == "04")
                        GeraLogTrans(dbt, db, Usuario, 999004);
                    else
                        if (Usuario.STA == "00")
                            GeraLogTrans(dbt, db, Usuario, 999003);
                        else
                            if (Usuario.STA == "02")
                                GeraLogTrans(dbt, db, Usuario, 999005);
                }
                else
                    //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                    UtilSIL.GravarLog(db, dbt, "UPDATE USUARIOVA", FOperador, cmd);

                // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                

                //Alterar dados complementares do usuário.
                if (Usuario.TEMCAD)
                    AlterarDadosComplUsu(Usuario);
                else
                    InserirDadosUsu(Usuario);

                #region AUTORIZADOR
                Usuario.CODCRTANT = Usuario.CODCRT;

                // AUTORIZADOR
                AlterarCartaoVA(Usuario);

                #endregion

                if (LinhaAfetada == 1)
                    dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool AlterarDadosComplUsu(USUARIO_VA Usuario)
        {
            Database db;
            StringBuilder sql = new StringBuilder();
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDTELENET);

            sql.Append("UPDATE CADUSUARIO ");
            sql.Append("SET DATNAS = @DATNAS, TEL = @TEL, CEL = @CEL,");
		    sql.Append("SEXO = @SEXO, EMA = @EMA, PAI = @PAI, MAE = @MAE, RG = @RG,");
		    sql.Append("ORGEXPRG = @ORGEXPRG, NATURALIDADE = @NATURALIDADE, NACIONALIDADE = @NACIONALIDADE, ENDUSU = @ENDUSU,");
		    sql.Append("ENDNUMUSU = @ENDNUMUSU, ENDCPL = @ENDCPL, BAIRRO = @BAIRRO, LOCALIDADE = @LOCALIDADE, UF = @UF, CEP = @CEP,");
		    sql.Append("ENDUSUCOM = @ENDUSUCOM, ENDNUMCOM = @ENDNUMCOM, ENDCPLCOM = @ENDCPLCOM, BAIRROCOM = @BAIRROCOM,");
		    sql.Append("LOCALIDADECOM = @LOCALIDADECOM, UFCOM = @UFCOM, CEPCOM = @CEPCOM, TELCOM = @TELCOM, DATATUALIZ = GETDATE()");
            sql.Append("WHERE CPF = @CPF");

            cmd = db.GetSqlStringCommand(sql.ToString());
            dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CPF", DbType.String, Usuario.CPF);
            if (Usuario.DATNAS == DateTime.MinValue)
                db.AddInParameter(cmd, "DATNAS", DbType.DateTime, null);
            else
                db.AddInParameter(cmd, "DATNAS", DbType.DateTime, Usuario.DATNAS);
            db.AddInParameter(cmd, "PAI", DbType.String, Usuario.PAI);
            db.AddInParameter(cmd, "MAE", DbType.String, Usuario.MAE);
            db.AddInParameter(cmd, "CEL", DbType.String, Usuario.CEL);
            db.AddInParameter(cmd, "TEL", DbType.String, Usuario.TEL);
            db.AddInParameter(cmd, "SEXO", DbType.String, Usuario.SEXO);
            db.AddInParameter(cmd, "EMA", DbType.String, Usuario.EMA);
            db.AddInParameter(cmd, "RG", DbType.String, Usuario.RG);
            db.AddInParameter(cmd, "ORGEXPRG", DbType.String, Usuario.ORGEXPRG);
            db.AddInParameter(cmd, "NATURALIDADE", DbType.String, Usuario.NATURALIDADE);
            db.AddInParameter(cmd, "NACIONALIDADE", DbType.String, Usuario.NACIONALIDADE);
            db.AddInParameter(cmd, "ENDUSU", DbType.String, Usuario.ENDUSU);
            db.AddInParameter(cmd, "ENDNUMUSU", DbType.String, Usuario.ENDNUMUSU);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, Usuario.ENDCPL);
            db.AddInParameter(cmd, "BAIRRO", DbType.String, Usuario.BAIRRO);
            db.AddInParameter(cmd, "LOCALIDADE", DbType.String, Usuario.LOCALIDADE);
            db.AddInParameter(cmd, "UF", DbType.String, Usuario.UF);
            db.AddInParameter(cmd, "CEP", DbType.String, Usuario.CEP);
            db.AddInParameter(cmd, "ENDUSUCOM", DbType.String, Usuario.ENDUSUCOM);
            db.AddInParameter(cmd, "ENDNUMCOM", DbType.String, Usuario.ENDNUMUSUCOM);
            db.AddInParameter(cmd, "ENDCPLCOM", DbType.String, Usuario.ENDCPLCOM);
            db.AddInParameter(cmd, "BAIRROCOM", DbType.String, Usuario.BAIRROCOM);
            db.AddInParameter(cmd, "LOCALIDADECOM", DbType.String, Usuario.LOCALIDADECOM);
            db.AddInParameter(cmd, "UFCOM", DbType.String, Usuario.UFCOM);
            db.AddInParameter(cmd, "CEPCOM", DbType.String, Usuario.CEPCOM);
            db.AddInParameter(cmd, "TELCOM", DbType.String, Usuario.TELCOM);            

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro ao atualizar dados complementares do usuário. " + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ValidarCliente(int codCli)
        {
            StringBuilder sql;
            sql = new StringBuilder();
            string sta = string.Empty;

            Database db;
            db = new SqlDatabase(BDTELENET);

            sql = new StringBuilder();

            sql.AppendLine("SELECT STA");
            sql.AppendLine("FROM CLIENTEVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI ");

            DbCommand cmd;
            IDataReader idr = null;

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);

            try
            {
                idr = db.ExecuteReader(cmd);
                if (idr.Read())
                    sta = idr["STA"].ToString();
                if (!idr.IsClosed)
                    idr.Close();
            }
            catch
            {
                if (idr != null && !idr.IsClosed)
                    idr.Close();
                return false;
            }
            // 29/10/2013 a pedido da personal card
            // cliente bloqueado, podera ter cartões incluidos
            return ((sta == "00") || (sta == "01")); 
            
        }

        #endregion

        #region  CRUD Dependentes

        public int NumDep(int codCli, string cpf)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MAX(NUMDEP), 1) MaxDep");
            sql.AppendLine("FROM USUARIOVA");
            sql.AppendLine("WHERE CODCLI = @CODCLI");
            sql.AppendLine("  AND CPF = @CPF");
            sql.AppendLine("  AND NUMDEP > 0");
            var dbc = db.CreateConnection();
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "@CPF", DbType.String, cpf);
            var idr = db.ExecuteReader(cmd);
            int MaxDep;
            try
            {
                if (idr.Read()) MaxDep = Convert.ToInt32(idr["MaxDep"]);
                else            MaxDep = 1;
            }
            finally
            {
                idr.Close();
            }
            return MaxDep;
        }

        //public bool InserirDependente(USUARIOVA titular, string nomeDep, string nomeAbrev, short tipoParentesco, string trilha)
        //{
        //    Database db = new SqlDatabase(BDTELENET);
        //    var sql = new StringBuilder();
        //    sql.AppendLine("SELECT ISNULL(MAX(NUMDEP), 0) MaxDep");
        //    sql.AppendLine("FROM USUARIOVA");
        //    sql.AppendLine("WHERE CODCLI = @CODCLI");
        //    sql.AppendLine("  AND CPF = @CPF");
        //    sql.AppendLine("  AND NUMDEP > 0");
        //    var dbc = db.CreateConnection();
        //    var cmd = db.GetSqlStringCommand(sql.ToString());
        //    db.AddInParameter(cmd, "@CODCLI", DbType.Int32, titular.CODCLI.CODCLI);
        //    db.AddInParameter(cmd, "@CPF", DbType.String, titular.CPF);
        //    var idr = db.ExecuteReader(cmd);
        //    int MaxDep;
        //    try
        //    {
        //        MaxDep = idr.Read() ? Convert.ToInt32(idr["MaxDep"]) : 1;
        //    }
        //    finally
        //    {
        //        idr.Close();
        //    }
        //    dbc.Open();
        //    var dbt = dbc.BeginTransaction();
        //    try
        //    {
        //        const string sqlInsert = "INSERT INTO USUARIOVA(CODCLI, CPF, NUMDEP, NOMUSU, CODPAR, DATINC, MAT, CODFIL, CODSET, CODCRT, STA, DATSTA, CARGPADVA, ULTCARGVA, DTVALCART, TRILHA2, SENHA, NUMULTPAC, NUMPAC, NOMCRT ) " +
        //                                 "VALUES (@CODCLI, @CPF, @NUMDEP, @NOMUSU, @CODPAR, @DATINC, @MAT, @CODFIL, @CODSET,  @CODCRT, @STA, @DATSTA, @CARGPADVA, @ULTCARGVA, @DTVALCART, @TRILHA2, @SENHA, @NUMULTPAC, @NUMPAC, @NOMCRT)";
        //        cmd = db.GetSqlStringCommand(sqlInsert);
        //        MaxDep++;
        //        var Cartao = trilha.Substring(0, 17);
        //        db.AddInParameter(cmd, "CODCLI", DbType.Int32, titular.CODCLI.CODCLI);
        //        db.AddInParameter(cmd, "CPF", DbType.String, titular.CPF);
        //        db.AddInParameter(cmd, "NUMDEP", DbType.Int16, MaxDep);
        //        db.AddInParameter(cmd, "NOMUSU", DbType.String, nomeDep);
        //        if (tipoParentesco != 0)
        //            db.AddInParameter(cmd, "CODPAR", DbType.Int16, tipoParentesco);
        //        db.AddInParameter(cmd, "DATINC", DbType.DateTime, DateTime.Now);
        //        db.AddInParameter(cmd, "MAT", DbType.String, titular.MAT);
        //        db.AddInParameter(cmd, "CODFIL", DbType.Int16, titular.CODFIL);
        //        db.AddInParameter(cmd, "CODSET", DbType.String, titular.CODSET);
        //        db.AddInParameter(cmd, "STA", DbType.String, titular.STA);
        //        db.AddInParameter(cmd, "CARGPADVA", DbType.Decimal, titular.CARGPADVA);
        //        db.AddInParameter(cmd, "ULTCARGVA", DbType.Int32, titular.ULTCARGVA);
        //        db.AddInParameter(cmd, "DTVALCART", DbType.String, titular.DTVALCART);
        //        db.AddInParameter(cmd, "SENHA", DbType.String, titular.SENHA);
        //        db.AddInParameter(cmd, "TRILHA2", DbType.String, trilha);
        //        db.AddInParameter(cmd, "CODCRT", DbType.String, Cartao);
        //        db.AddInParameter(cmd, "DATSTA", DbType.DateTime, DateTime.Now);
        //        db.AddInParameter(cmd, "NUMULTPAC", DbType.Int16, 0);
        //        db.AddInParameter(cmd, "NUMPAC", DbType.Int16, 1);
        //        db.AddInParameter(cmd, "NOMCRT", DbType.String, nomeAbrev);
        //        db.ExecuteNonQuery(cmd, dbt);
        //        InserirCartaoVADependente(titular, Cartao, nomeAbrev, MaxDep);  // TODO 
        //        GeraLogTransDependente(dbt, db, titular, 999001, Cartao);
        //        dbt.Commit();
        //    }
        //    catch (Exception err)
        //    {
        //        dbt.Rollback();
        //        throw new Exception("Erro Camada DAL [Inserir Dependente] " + err);
        //    }
        //    finally
        //    {
        //        dbc.Close();
        //    }
        //    return true;
        //}

        public bool ExcluirDependente(int id)
        {
            var Dependente = GetUsuarioVA(id);
            var Cartao = Dependente.CODCRT;
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("DELETE USUARIOVA WHERE ID = @ID");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "ID", DbType.Int32, id);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            int LinhaAfetada;
            try
            {
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                GeraLogTransDependente(dbt, db, Dependente, 999009, Cartao);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Dependente] " + err);
            }
            finally
            {
                dbc.Close();
            }
            if (LinhaAfetada != 0)
                ExcluirCartaoVA(Cartao);
            return true;
        }

        public bool AlterarDependente(int id, string nomeDep, string nomeAbrev, short tipoParentesco)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "UPDATE USUARIOVA SET NOMUSU = @NOMUSU, NOMCRT = @NOMCRT, CODPAR = @CODPAR " +
                               "WHERE ID = @ID";
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "NOMUSU", DbType.String, nomeDep);
            db.AddInParameter(cmd, "NOMCRT", DbType.String, nomeAbrev);
            if (tipoParentesco != 0)
                db.AddInParameter(cmd, "CODPAR", DbType.Int16, tipoParentesco);
            db.AddInParameter(cmd, "ID", DbType.String, id);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {                              
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE USUARIOVA (Alterar Dependente)", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar Dependente] " + err);
            }
            finally
            {
                dbc.Close();
            }
            var Dependente = GetUsuarioVA(id);
            var usuDep = new USUARIO_VA();
            usuDep.CODCRT = Dependente.CODCRT;
            usuDep.CODCRTANT = Dependente.CODCRT;
            usuDep.NOMUSU = nomeAbrev;
            usuDep.STA = Dependente.STA;
            usuDep.DATSTA = Dependente.DATSTA;
            usuDep.DTVALCART = Dependente.DTVALCART;
            AlterarCartaoVA(usuDep);
            return true;
        }

        #endregion Dependente

        #region CRUD Observacoes

        public bool InserirObs(int codCli, string cpf, DateTime data, string obs)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sbCamposCliente = new StringBuilder();
            var sbParametrosCliente = new StringBuilder();
            sbCamposCliente.Append("CODCLI, CPF, DATA, OBS ");
            sbParametrosCliente.Append("@CODCLI, @CPF, @DATA, @OBS ");
            var sql = string.Format("INSERT INTO OBSUSUVA ({0}) VALUES ({1})", sbCamposCliente, sbParametrosCliente);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, data.ToString("G"));
            db.AddInParameter(cmd, "OBS", DbType.String, obs);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "INSERT OBSUSUVA", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Obs]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool AlterarObs(int codCli, string cpf, DateTime data, string obs)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "UPDATE OBSUSUVA SET OBS = @OBS WHERE CODCLI = @CODCLI AND CPF = @CPF ";
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "OBS", DbType.String, obs);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE OBSUSUVA", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar Obs]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirObs(int id)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("DELETE OBSUSUVA WHERE ID = @ID ");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();            
            db.AddInParameter(cmd, "ID", DbType.Int32, id);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE OBSUSUVA", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Obs]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirObs(Database db, DbTransaction dbt, int codCli, string cpf)
        {
            const string sql = "DELETE FROM OBSUSUVA WHERE CODCLI = @CODCLI AND CPF = @CPF ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.Int32, cpf);
            db.ExecuteNonQuery(cmd, dbt);
            UtilSIL.GravarLog(db, dbt, "DELETE OBSUSUVA (Exclusao da lista observacoes do usuario excluido)", FOperador, cmd);
            return true;
        }

        #endregion

        #region CRUD CartaoVA Autorizador

        //public bool InserirCartaoVA(USUARIOVA Titular)
        //{
        //    Database db;
        //    string sql;
        //    DbCommand cmd;
        //    DbConnection dbc;
        //    DbTransaction dbt;

        //    db = new SqlDatabase(BDAUTORIZADOR);

        //    sql = "INSERT INTO CTCARTVA (CODEMPRESA, CODCARTAO, STATUSU, DTSTATUSU, NOMEUSU, NUMDEPEND, CPFTIT," +
        //          "DTVALCART, SENHA ,SALDOVA) " +

        //          "VALUES (@CODEMPRESA, @CODCARTAO, @STATUSU, @DTSTATUSU, @NOMEUSU, @NUMDEPEND, @CPFTIT," +
        //          "@DTVALCART, @SENHA, @SALDOVA)";

        //    cmd = db.GetSqlStringCommand(sql);
        //    dbc = db.CreateConnection();
        //    db.AddInParameter(cmd, "CODEMPRESA", DbType.String, Titular.CODCLI.ToString().PadLeft(5, '0'));
        //    db.AddInParameter(cmd, "CODCARTAO", DbType.String, Titular.CODCRT);
        //    db.AddInParameter(cmd, "STATUSU", DbType.String, Titular.STA);
        //    db.AddInParameter(cmd, "DTSTATUSU", DbType.DateTime, Titular.DATSTA);
        //    db.AddInParameter(cmd, "NOMEUSU", DbType.String, Titular.NOMUSU);
        //    db.AddInParameter(cmd, "NUMDEPEND", DbType.String, "00");
        //    db.AddInParameter(cmd, "CPFTIT", DbType.String, Titular.CPF);
        //    db.AddInParameter(cmd, "DTVALCART", DbType.String, Titular.DTVALCART);
        //    db.AddInParameter(cmd, "SENHA", DbType.String, Titular.SENHA);
        //    db.AddInParameter(cmd, "SALDOVA", DbType.Double, 0);

        //    // Controle Transacao
        //    dbc.Open();
        //    dbt = dbc.BeginTransaction();

        //    try
        //    {   // Linha Afetada                                
        //        int LinhaAfetada;
        //        LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
        //        dbt.Commit();
        //        return (LinhaAfetada == 1);

        //    }
        //    catch (Exception err)
        //    {
        //        dbt.Rollback();
        //        throw new Exception("Erro Camada DAL [Inserir CartaoVA] " + err);
        //    }
        //    finally
        //    {
        //        dbc.Close();
        //    }
        //}

        //public bool InserirCartaoVADependente(USUARIOVA Titular, string Cartao, string NomeDep, int NumDep)
        //{
        //    Database db = new SqlDatabase(BDAUTORIZADOR);
        //    const string sql = "INSERT INTO CTCARTVA (CODEMPRESA, CODCARTAO, STATUSU, DTSTATUSU, NOMEUSU, NUMDEPEND, CODCARTIT, CPFTIT," +
        //                       "DTVALCART, SENHA ,SALDOVA) " +
        //                       "VALUES (@CODEMPRESA, @CODCARTAO, @STATUSU, @DTSTATUSU, @NOMEUSU, @NUMDEPEND, @CODCARTIT, @CPFTIT," +
        //                       "@DTVALCART, @SENHA, @SALDOVA)";
        //    var cmd = db.GetSqlStringCommand(sql);
        //    var dbc = db.CreateConnection();
        //    db.AddInParameter(cmd, "CODEMPRESA", DbType.String, Titular.CODCLI.CODCLI.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0'));
        //    db.AddInParameter(cmd, "CODCARTAO", DbType.String, Cartao);
        //    db.AddInParameter(cmd, "STATUSU", DbType.String, Titular.STA);
        //    db.AddInParameter(cmd, "DTSTATUSU", DbType.DateTime, Titular.DATSTA);
        //    db.AddInParameter(cmd, "NOMEUSU", DbType.String, NomeDep.Length > 30 ? NomeDep.Substring(0, 30) : NomeDep);
        //    db.AddInParameter(cmd, "NUMDEPEND", DbType.String, NumDep.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'));
        //    db.AddInParameter(cmd, "CODCARTIT", DbType.String, Titular.CODCRT);
        //    db.AddInParameter(cmd, "CPFTIT", DbType.String, Titular.CPF);
        //    db.AddInParameter(cmd, "DTVALCART", DbType.String, Titular.DTVALCART);
        //    db.AddInParameter(cmd, "SENHA", DbType.String, Titular.SENHA);
        //    db.AddInParameter(cmd, "SALDOVA", DbType.Double, 0);
        //    dbc.Open();
        //    var dbt = dbc.BeginTransaction();
        //    try
        //    {                               
        //        var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
        //        dbt.Commit();
        //        return (LinhaAfetada == 1);
        //    }
        //    catch (Exception err)
        //    {
        //        dbt.Rollback();
        //        throw new Exception("Erro Camada DAL [Inserir CartaoVA] " + err);
        //    }
        //    finally
        //    {
        //        dbc.Close();
        //    }
        //}

        public bool AlterarCartaoVA(USUARIO_VA usu)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDAUTORIZADOR);

            sql = "UPDATE CTCARTVA SET CODCARTAO = @CODCARTAONOVO, NOMEUSU = @NOMEUSU, STATUSU = @STATUSU, DTSTATUSU = @DTSTATUSU, DTVALCART = @DTVALCART " +
                  "WHERE CODCARTAO = @CODCARTAOVELHO";

            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            //WHERE
            db.AddInParameter(cmd, "CODCARTAOVELHO", DbType.String, usu.CODCRTANT);
            db.AddInParameter(cmd, "CODCARTAONOVO", DbType.String, usu.CODCRT);
            db.AddInParameter(cmd, "NOMEUSU", DbType.String, usu.NOMUSU.Length > 30 ? usu.NOMUSU.Substring(0, 30) : usu.NOMUSU);
            db.AddInParameter(cmd, "STATUSU", DbType.String, usu.STA);
            db.AddInParameter(cmd, "DTSTATUSU", DbType.DateTime, usu.DATSTA);
            db.AddInParameter(cmd, "DTVALCART", DbType.String, usu.DTVALCART);

            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar CartaoVA] " + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirCartaoVA(string CodCartao)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDAUTORIZADOR);

            sql = "DELETE CTCARTVA WHERE CODCARTAO = @CODCARTAO";

            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CODCARTAO", DbType.String, CodCartao);
            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar o valor dos parametros se houver)
                UtilSIL.GravarLog("DELETE CTCARTVA", FOperador, cmd);

                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir CartaoVA] " + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool ExcluirCartoesVATitular(int Cliente, string CPF)
        {
            Database db;
            string sql;
            DbCommand cmd;
            DbConnection dbc;
            DbTransaction dbt;

            db = new SqlDatabase(BDAUTORIZADOR);

            sql = "DELETE CTCARTVA WHERE CODEMPRESA = @CODCLI AND CPFTIT = @CPF";

            cmd = db.GetSqlStringCommand(sql);
            dbc = db.CreateConnection();

            db.AddInParameter(cmd, "CODCLI", DbType.String, Cliente.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "CPF", DbType.String, CPF);
            // Controle Transacao
            dbc.Open();
            dbt = dbc.BeginTransaction();

            try
            {   // Linha Afetada                                
                int LinhaAfetada;
                LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
                return (LinhaAfetada == 1);

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir CartaoVA] " + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion

        #region Log

        private void GeraLogTrans(DbTransaction dbt, Database db, USUARIOVA usu, int tiptra)
        {
            DbCommand cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, usu.CODCLI);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, DBNull.Value);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, tiptra);
            db.AddInParameter(cmd, "CODCRT", DbType.String, usu.CODCRT);

            //Tratamento para o tipo de transacao de Geracao de 2 via
            if (tiptra == 999007 && COBRARSEGVIA)
                db.AddInParameter(cmd, "VALOR", DbType.Decimal, usu.CODCLI.VAL2AV);
            else
                db.AddInParameter(cmd, "VALOR", DbType.Decimal, 0m);

            db.AddInParameter(cmd, "CPF", DbType.String, usu.CPF);
            db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
            db.AddInParameter(cmd, "NUMCARGAVA", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "NUMFECCRE", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "DAD", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "ATV", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, FOperador.ID_FUNC);

            db.ExecuteNonQuery(cmd, dbt);

        }

        private void GeraLogTransDependente(DbTransaction dbt, Database db, USUARIOVA usu, int tiptra, string codCrt)
        {
            DbCommand cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, usu.CODCLI.CODCLI);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, DBNull.Value);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, tiptra);
            db.AddInParameter(cmd, "CODCRT", DbType.String, codCrt);
            db.AddInParameter(cmd, "VALOR", DbType.Decimal, 0m);
            db.AddInParameter(cmd, "CPF", DbType.String, usu.CPF);
            db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
            db.AddInParameter(cmd, "NUMCARGAVA", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "NUMFECCRE", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "DAD", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "ATV", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, FOperador.ID_FUNC);

            db.ExecuteNonQuery(cmd, dbt);

        }

        private void GeraLogTrans(DbTransaction dbt, Database db, USUARIO_VA usu, int tiptra)
        {
            DbCommand cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, usu.CODCLI);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, System.DBNull.Value);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, tiptra);
            db.AddInParameter(cmd, "CODCRT", DbType.String, usu.CODCRT);

            //Tratamento para o tipo de transacao de Geracao de 2 via
            if (tiptra == 999007 && COBRARSEGVIA)
                db.AddInParameter(cmd, "VALOR", DbType.Decimal, usu.VALORSEGVIA);
            else
                db.AddInParameter(cmd, "VALOR", DbType.Decimal, 0m);

            db.AddInParameter(cmd, "CPF", DbType.String, usu.CPF);
            db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
            db.AddInParameter(cmd, "NUMCARGAVA", DbType.Int16, System.DBNull.Value);
            db.AddInParameter(cmd, "NUMFECCRE", DbType.Int16, System.DBNull.Value);
            db.AddInParameter(cmd, "DAD", DbType.String, usu.CODCRTANT);
            db.AddInParameter(cmd, "ATV", DbType.String, System.DBNull.Value);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, FOperador.ID_FUNC);

            db.ExecuteNonQuery(cmd, dbt);

        }

        private void GeraLogTransDependente(DbTransaction dbt, Database db, USUARIO_VA usu, int tiptra, string codCrt)
        {
            DbCommand cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");

            db.AddInParameter(cmd, "CODCLI", DbType.Int32, usu.CODCLI);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, System.DBNull.Value);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, tiptra);
            db.AddInParameter(cmd, "CODCRT", DbType.String, codCrt);
            db.AddInParameter(cmd, "VALOR", DbType.Decimal, 0m);
            db.AddInParameter(cmd, "CPF", DbType.String, usu.CPF);
            db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
            db.AddInParameter(cmd, "NUMCARGAVA", DbType.Int16, System.DBNull.Value);
            db.AddInParameter(cmd, "NUMFECCRE", DbType.Int16, System.DBNull.Value);
            db.AddInParameter(cmd, "DAD", DbType.Int16, System.DBNull.Value);
            db.AddInParameter(cmd, "ATV", DbType.String, System.DBNull.Value);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, FOperador.ID_FUNC);

            db.ExecuteNonQuery(cmd, dbt);

        }

        #endregion

        public List<USUARIO_VA> ListaCartoesGeral(Hashtable filtros)
        {
            IDataReader idr;
            Database db = new SqlDatabase(BDTELENET);
            string sql = "PROC_REL_CARTOES_GERAL";
            DbCommand cmd = db.GetStoredProcCommand(sql);
            string validade = string.Empty;

            if (!string.IsNullOrEmpty(filtros["codIni"].ToString()))
                db.AddInParameter(cmd, "CODCLIINI", DbType.Int32, Convert.ToInt32(filtros["codIni"]));

            if (!string.IsNullOrEmpty(filtros["codFim"].ToString()))
                db.AddInParameter(cmd, "CODCLIFIM", DbType.Int32, Convert.ToInt32(filtros["codFim"]));

            if (!string.IsNullOrEmpty(filtros["validadeIni"].ToString()))
                db.AddInParameter(cmd, "VALIDADEINI", DbType.Int32, Convert.ToInt32(filtros["validadeIni"]));
            else
                db.AddInParameter(cmd, "VALIDADEINI", DbType.Int32, 0);

            if (!string.IsNullOrEmpty(filtros["validadeFim"].ToString()))
                db.AddInParameter(cmd, "VALIDADEFIM", DbType.Int32, Convert.ToInt32(filtros["validadeFim"]));
            else
                db.AddInParameter(cmd, "VALIDADEFIM", DbType.Int32, 9999);

            if (!string.IsNullOrEmpty(filtros["status"].ToString()))
                db.AddInParameter(cmd, "STATUS", DbType.String, filtros["status"].ToString());
            else
                db.AddInParameter(cmd, "STATUS", DbType.String, "00,01,02,06"); //TODOS

            idr = db.ExecuteReader(cmd);

            List<USUARIO_VA> lista = new List<USUARIO_VA>();

            while (idr.Read())
            {
                USUARIO_VA usu = new USUARIO_VA();

                usu.CODCLI = idr["codcli"].ToString();
                usu.NOMCLI = idr["nomcli"].ToString();
                usu.CODCRT = idr["cartao"].ToString();
                usu.NOMUSU = idr["usuario"].ToString();
                usu.STA = idr["status"].ToString();
                usu.NUMDEP = idr["dependente"].ToString();

                if (!string.IsNullOrEmpty(idr["saldo"].ToString()))
                    usu.SALDO = Convert.ToDecimal(idr["saldo"]);

                if (!string.IsNullOrEmpty(idr["ult_movimentacao"].ToString()))
                    usu.ULTMOV = Convert.ToDateTime(idr["ult_movimentacao"]).ToString("dd/MM/yyyy");

                if (!string.IsNullOrEmpty(idr["validade"].ToString()))
                {
                    validade = idr["validade"].ToString().Substring(2, 2) + "/" + idr["validade"].ToString().Substring(0, 2);
                    usu.VALIDADE = validade;
                }

                lista.Add(usu);
            }

            idr.Close();

            return lista;
        }

        #region Get Clientes

        public List<CLIENTEVA_PREPAGO> ListaClientes()
        {
            Database db;
            StringBuilder sql;

            db = new SqlDatabase(BDTELENET);
            sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  CODCLI, NOMCLI, STA, PRZVALCART");
            sql.AppendLine("  FROM CLIENTEVA");
            sql.AppendLine("  ORDER BY CODCLI");

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            idr = db.ExecuteReader(cmd);

            List<CLIENTEVA_PREPAGO> colecaoClientesVA = new List<CLIENTEVA_PREPAGO>();

            while (idr.Read())
            {
                CLIENTEVA_PREPAGO clienteVA;
                clienteVA = new CLIENTEVA_PREPAGO();

                clienteVA.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                clienteVA.NOMCLI = idr["NOMCLI"].ToString();
                clienteVA.DESTA = GetStatus(Convert.ToString(idr["STA"])).DESTA;
                clienteVA.STA = idr["STA"].ToString();
                clienteVA.PRZVALCART = idr["PRZVALCART"].ToString();

                colecaoClientesVA.Add(clienteVA);
            }

            idr.Close();

            return colecaoClientesVA;
        }

        public STATUS GetStatus(string codSta)
        {
            StringBuilder sql;
            sql = new StringBuilder();

            Database db;
            db = new SqlDatabase(BDTELENET);


            sql.AppendLine("SELECT");
            sql.AppendLine(" DESTA");
            sql.AppendLine(" FROM STATUS");
            sql.AppendLine(" WHERE STA = " + codSta);
            sql.AppendLine(" ORDER BY STA");

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            idr = db.ExecuteReader(cmd);

            STATUS status = null;

            if (idr.Read())
            {
                status = new STATUS();
                status.DESTA = Convert.ToString(idr["DESTA"]);
            }

            idr.Close();

            return status;
        }

        #endregion

        public long TotalRegistrosUsuarios()
        {
            Database db;
            StringBuilder sql;
            long total = 0;

            db = new SqlDatabase(BDTELENET);
            sql = new StringBuilder();

            sql.AppendLine(" SELECT COUNT(codcrt) as total ");
            sql.AppendLine(" FROM USUARIOVA ");

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            idr = db.ExecuteReader(cmd);

            if (idr.Read())
                total = Convert.ToInt64(idr["total"]);

            idr.Close();

            return total;
        }

        public bool PermicaoCartaoMask(int idPerfil)
        {
            Database db;
            StringBuilder sql;

            db = new SqlDatabase(BDTELENET);
            sql = new StringBuilder();

            sql.AppendLine(" SELECT IDCOMP");
            sql.AppendLine(" FROM CONTROLEACESSOVA WHERE IDCOMP = 168 AND IDPERFIL = @IDPERFIL ");

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "IDPERFIL", DbType.Int32, idPerfil);
            idr = db.ExecuteReader(cmd);
            var ret = idr.Read();
            idr.Close();
            return ret;
        }

        public List<USUARIO_VA> ColecaoUsuarioPaging(int codCli, int ini, int fim)
        {
            Database db;
            StringBuilder sql;
            db = new SqlDatabase(BDTELENET);
            sql = new StringBuilder();

            sql.AppendLine(" SELECT CODCRT, NOMUSU, CODCLI ");
            sql.AppendLine(" FROM ");
            sql.AppendLine(" (SELECT CODCRT, NOMUSU, CODCLI, ");
            sql.AppendLine(" ROW_NUMBER() OVER(ORDER BY NOMUSU) as RowNum ");
            sql.AppendLine("  FROM USUARIOVA WHERE CODCLI = " + codCli + " AND STA = '00') as DerivedTableName ");
            sql.AppendLine(" WHERE RowNum BETWEEN " + (ini + 1) + " AND( " + fim + " )");

            DbCommand cmd;
            IDataReader idr;

            cmd = db.GetSqlStringCommand(sql.ToString());
            idr = db.ExecuteReader(cmd);

            List<USUARIO_VA> colecaoVA = new List<USUARIO_VA>();

            while (idr.Read())
            {
                USUARIO_VA usu;
                usu = new USUARIO_VA();

                usu.CODCRT = idr["CODCRT"].ToString();
                usu.NOMUSU = idr["NOMUSU"].ToString();
                usu.CODCLI = idr["CODCLI"].ToString();

                colecaoVA.Add(usu);
            }

            idr.Close();

            return colecaoVA; ;
        }

        public string GetNomeAbreviado(string nome)
        {
            const string procedureName = "PROC_ABREV_NOME";
            Database db = new SqlDatabase(BDTELENET);
            try
            {
                string nomeAbreviado;
                string nomeAbreviadoFinal = string.Empty;
                using (var cmd = db.GetStoredProcCommand(procedureName))
                {
                    db.AddInParameter(cmd, "NOME", DbType.String, nome);
                    using (var idr = db.ExecuteReader(cmd))
                    {
                        nomeAbreviado = null;
                        if (idr.Read())
                            nomeAbreviado = idr["RETORNO"].ToString();
                    }
                }

                return nomeAbreviado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ENDCEP GetCep(string cep)
        {
            const string procedureName = "MW_CONSCEP";
            Database db = new SqlDatabase(BDCONCENTRADOR);
            try
            {
                var endereco = new ENDCEP();                
                using (var cmd = db.GetStoredProcCommand(procedureName))
                {
                    db.AddInParameter(cmd, "CEP", DbType.String, cep);
                    using (var idr = db.ExecuteReader(cmd))
                    {
                        if (idr.Read())
                        {
                            endereco.RETORNO = idr["RETORNO"].ToString();
                            if (endereco.RETORNO == "OK")
                            {
                                endereco.LOGRA = Convert.ToString(idr["LOGRADOURO"]);
                                endereco.BAIRRO = Convert.ToString(idr["BAIRRO"]);
                                endereco.LOCALI = Convert.ToString(idr["LOCALIDADE"]);
                                endereco.UF = Convert.ToString(idr["UF"]); 
                            }                            
                        }                            
                    }
                }
                return endereco;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
