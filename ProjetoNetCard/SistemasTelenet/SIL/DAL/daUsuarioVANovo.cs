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
    class daUsuarioVANovo
    {
        readonly string BDTELENET = string.Empty;
        readonly string BDAUTORIZADOR = string.Empty;
        readonly string BDCONCENTRADOR = string.Empty;
        readonly OPERADORA FOperador;

        public daUsuarioVANovo(OPERADORA Operador)
        {
            FOperador = Operador;
            var ServidorConcentrador = ConfigurationManager.AppSettings["ServidorConcentrador"];
            var BancoConcentrador = ConfigurationManager.AppSettings["bdConcentrador"];
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, Operador.SERVIDORCON ?? ServidorConcentrador, Operador.BANCOCON ?? BancoConcentrador, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }        

        #region GET Usuário

        public CADUSUARIO GetUsuario(int idUsuario)
        {
            var cadUsuario = new CADUSUARIO();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" ID_USUARIO, DATINC, SISTEMA, PRODUTO, CODCLI, NOMCLI, CPF, NOMUSU, TIPO, NUMDEP, STA, DESTA, CODCRT, CODCRTMASC, BLOQCARTUSU, ");
            sql.AppendLine(" BAIRRO, CEP, ENDUSU, ENDNUMUSU, ENDCPL, LOCALIDADE, UF, TEL, CEL, EMA, RG, PAI, MAE, SEXO, DATATUALIZ, ");
            sql.AppendLine(" DATNAS, ORGEXPRG, NATURALIDADE, NACIONALIDADE, NIS, CEPCOM, ENDUSUCOM, ENDNUMCOM, ENDCPLCOM, BAIRROCOM, ");
            sql.AppendLine(" LOCALIDADECOM, UFCOM, TELCOM ");
            sql.AppendLine(" FROM VRESUMOUSU WITH (NOLOCK) ");
            sql.AppendLine(" WHERE ID_USUARIO = @ID_USUARIO AND NUMDEP = 0");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, idUsuario);
            var idr = db.ExecuteReader(cmd);

            if (idr.Read())
            {
                cadUsuario.ID_USUARIO = idUsuario;
                cadUsuario.CPF = idr["CPF"].ToString();
                if (idr["CODCRT"] != DBNull.Value) cadUsuario.CODCRT = Convert.ToString(idr["CODCRT"]);
                if (idr["CODCRTMASC"] != DBNull.Value) cadUsuario.CODCRTMASK = idr["CODCRTMASC"].ToString();
                if (idr["BLOQCARTUSU"] != DBNull.Value) cadUsuario.BLOQCARTUSU = idr["BLOQCARTUSU"].ToString() == "S";
                if (idr["NOMUSU"] != DBNull.Value) cadUsuario.NOMUSU = idr["NOMUSU"].ToString();
                if (idr["BAIRRO"] != DBNull.Value) cadUsuario.BAIRRO = Convert.ToString(idr["BAIRRO"]);
                if (idr["CEP"] != DBNull.Value) cadUsuario.CEP = Convert.ToString(idr["CEP"]);
                if (idr["ENDUSU"] != DBNull.Value) cadUsuario.ENDUSU = Convert.ToString(idr["ENDUSU"]);
                if (idr["ENDNUMUSU"] != DBNull.Value) cadUsuario.ENDNUMUSU = Convert.ToString(idr["ENDNUMUSU"]);
                if (idr["ENDCPL"] != DBNull.Value) cadUsuario.ENDCPL = Convert.ToString(idr["ENDCPL"]);
                if (idr["LOCALIDADE"] != DBNull.Value) cadUsuario.LOCALIDADE = Convert.ToString(idr["LOCALIDADE"]);
                if (idr["UF"] != DBNull.Value) cadUsuario.UF = Convert.ToString(idr["UF"]);
                if (idr["TEL"] != DBNull.Value) cadUsuario.TEL = Convert.ToString(idr["TEL"]);
                if (idr["CEL"] != DBNull.Value) cadUsuario.CEL = Convert.ToString(idr["CEL"]);
                if (idr["EMA"] != DBNull.Value) cadUsuario.EMA = Convert.ToString(idr["EMA"]);
                if (idr["RG"] != DBNull.Value) cadUsuario.RG = Convert.ToString(idr["RG"]);
                if (idr["PAI"] != DBNull.Value) cadUsuario.PAI = Convert.ToString(idr["PAI"]);
                if (idr["MAE"] != DBNull.Value) cadUsuario.MAE = Convert.ToString(idr["MAE"]);
                if (idr["SEXO"] != DBNull.Value && (Convert.ToString(idr["SEXO"]) == "M" || Convert.ToString(idr["SEXO"]) == "F"))
                    cadUsuario.SEXO = Convert.ToString(idr["SEXO"]);
                if (idr["DATNAS"] != DBNull.Value) cadUsuario.DATNAS = Convert.ToDateTime(idr["DATNAS"]);
                if (idr["ORGEXPRG"] != DBNull.Value) cadUsuario.ORGEXPRG = Convert.ToString(idr["ORGEXPRG"]);
                if (idr["NATURALIDADE"] != DBNull.Value) cadUsuario.NATURALIDADE = Convert.ToString(idr["NATURALIDADE"]);
                if (idr["NACIONALIDADE"] != DBNull.Value) cadUsuario.NACIONALIDADE = Convert.ToString(idr["NACIONALIDADE"]);
                if (idr["NIS"] != DBNull.Value) cadUsuario.NIS = Convert.ToString(idr["NIS"]);
                if (idr["CEPCOM"] != DBNull.Value) cadUsuario.CEPCOM = Convert.ToString(idr["CEPCOM"]);
                if (idr["ENDUSUCOM"] != DBNull.Value) cadUsuario.ENDUSUCOM = Convert.ToString(idr["ENDUSUCOM"]);
                if (idr["ENDNUMCOM"] != DBNull.Value) cadUsuario.ENDNUMCOM = Convert.ToString(idr["ENDNUMCOM"]);
                if (idr["ENDCPLCOM"] != DBNull.Value) cadUsuario.ENDCPLCOM = Convert.ToString(idr["ENDCPLCOM"]);
                if (idr["BAIRROCOM"] != DBNull.Value) cadUsuario.BAIRROCOM = Convert.ToString(idr["BAIRROCOM"]);
                if (idr["LOCALIDADECOM"] != DBNull.Value) cadUsuario.LOCALIDADECOM = Convert.ToString(idr["LOCALIDADECOM"]);
                if (idr["UFCOM"] != DBNull.Value) cadUsuario.UFCOM = Convert.ToString(idr["UFCOM"]);
                if (idr["TELCOM"] != DBNull.Value) cadUsuario.TELCOM = Convert.ToString(idr["TELCOM"]);
            }
            idr.Close();

            return cadUsuario;
        }

        public List<VPRODUTOSUSU> GetUsuarioProd(int idusuario)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" A.SISTEMA, A.CODPROD, PRODUTO, CODCLI, NOMCLI, STACLI, DESTACLI, CPF, NUMDEP, CODFIL, CODPAR, NOMUSU, DATATV, DATINC, ");
            sql.AppendLine(" CODSET, MAT, DATGERCRT, GERCRT, GENERICO, CODCRT, STA, DATSTA, BLOQCARTUSU, DATBLOQUSU, TRILHA2, DTVALCART, NOMCRT, SENHA, ");
            sql.AppendLine(" SENHACIELO, SENUSU, DTSENHA, DTEXPSENHA, ULTACESSO, QTDEACESSOINV, ID_CARTAO, DATATUALIZ, DTULTRENOV, ");
            sql.AppendLine(" NACIONALIDADE, CEPCOM, ENDUSUCOM, ENDNUMCOM, ENDCPLCOM, BAIRROCOM, LOCALIDADECOM, UFCOM, TELCOM, ");
            sql.AppendLine(" B.TIPOPROD, NUMVIA, PRZVALCART, CODAG, PMO, LIMPAD, BONUS, CTRATV, ACRESPAD, AGENCIA, CONTA, BANCO, LIMCREDITO, ");
            sql.AppendLine(" MAXPARC, MAXPARCCLI, HABTRANSDIG, LIMRISCO, LIMRISCOCLI, CODSIND, SINDICALIZADO, CARGPADVA, ULTCARGVA, VCARGAUTO, LOTEATUAL, COBRASEGVIA, VALORSEGVIA ");
            sql.AppendLine(" FROM VPRODUTOSUSU A WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN PRODUTO B WITH (NOLOCK) ON A.CODPROD = B.CODPROD");
            sql.AppendLine(" WHERE ID_USUARIO = @ID_USUARIO ");
            sql.AppendLine("ORDER BY CPF DESC, CODCLI");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "ID_USUARIO", DbType.Int32, idusuario);

            var idr = db.ExecuteReader(cmd);
            var listProdutosUsu = new List<VPRODUTOSUSU>();

            while (idr.Read())
            {
                var usuarioProd = new VPRODUTOSUSU();
                usuarioProd.ID_USUARIO = idusuario;
                usuarioProd.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                usuarioProd.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                usuarioProd.STACLI = Convert.ToString(idr["STACLI"]);
                usuarioProd.DESTACLI = Convert.ToString(idr["DESTACLI"]);
                usuarioProd.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                usuarioProd.CODPROD = Convert.ToInt32(idr["CODPROD"]);
                usuarioProd.PRODUTO = Convert.ToString(idr["PRODUTO"]);
                usuarioProd.CPF = Convert.ToString(idr["CPF"]);
                usuarioProd.STA = Convert.ToString(idr["STA"]);
                var status = string.Empty;
                switch (usuarioProd.STA)
                {
                    case "00": status = "ATIVO"; break;
                    case "01": status = "BLOQUEADO"; break;
                    case "02": status = "CANCELADO"; break;
                    case "06": status = "SUSPENSO"; break;
                    case "07": status = "TRANSFERIDO"; break;
                }                
                usuarioProd.STATUS = status;

                if (!string.IsNullOrEmpty(usuarioProd.STA))
                {
                    if (idr["CODCRT"] != DBNull.Value) usuarioProd.CODCRT = Convert.ToString(idr["CODCRT"]);
                    if (idr["NOMCRT"] != DBNull.Value) usuarioProd.NOMCRT = Convert.ToString(idr["NOMCRT"]);
                    if (idr["NUMDEP"] != DBNull.Value) usuarioProd.NUMDEP = Convert.ToInt16(idr["NUMDEP"]);
                    if (idr["CODFIL"] != DBNull.Value) usuarioProd.CODFIL = Convert.ToInt16(idr["CODFIL"]);
                    if (idr["CODPAR"] != DBNull.Value) usuarioProd.CODPAR = Convert.ToInt16(idr["CODPAR"]);
                    if (idr["DATATV"] != DBNull.Value && Convert.ToDateTime(idr["DATATV"]) != DateTime.MinValue) usuarioProd.DATATV = Convert.ToDateTime(idr["DATATV"]);
                    if (idr["DATINC"] != DBNull.Value && Convert.ToDateTime(idr["DATINC"]) != DateTime.MinValue) usuarioProd.DATINC = Convert.ToDateTime(idr["DATINC"]);
                    if (idr["CODSET"] != DBNull.Value) usuarioProd.CODSET = Convert.ToString(idr["CODSET"]);
                    if (idr["MAT"] != DBNull.Value) usuarioProd.MAT = Convert.ToString(idr["MAT"]);
                    if (idr["DATGERCRT"] != DBNull.Value && Convert.ToDateTime(idr["DATGERCRT"]) != DateTime.MinValue) usuarioProd.DATGERCRT = Convert.ToDateTime(idr["DATGERCRT"]);
                    if (idr["GERCRT"] != DBNull.Value) usuarioProd.GERCRT = Convert.ToString(idr["GERCRT"]);
                    if (idr["DATSTA"] != DBNull.Value && Convert.ToDateTime(idr["DATSTA"]) != DateTime.MinValue) usuarioProd.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                    if (idr["BLOQCARTUSU"] != DBNull.Value) usuarioProd.BLOQCARTUSU = Convert.ToString(idr["BLOQCARTUSU"]) == "S";
                    if (idr["DATBLOQUSU"] != DBNull.Value && Convert.ToDateTime(idr["DATBLOQUSU"]) != DateTime.MinValue) usuarioProd.DATBLOQUSU = Convert.ToDateTime(idr["DATBLOQUSU"]);
                    if (idr["TRILHA2"] != DBNull.Value) usuarioProd.TRILHA2 = Convert.ToString(idr["TRILHA2"]);
                    if (idr["DTVALCART"] != DBNull.Value) usuarioProd.DTVALCART = Convert.ToString(idr["DTVALCART"]);
                    if (idr["SENHA"] != DBNull.Value) usuarioProd.SENHA = Convert.ToString(idr["SENHA"]);
                    if (idr["DTSENHA"] != DBNull.Value && Convert.ToDateTime(idr["DTSENHA"]) != DateTime.MinValue) usuarioProd.DTSENHA = Convert.ToDateTime(idr["DTSENHA"]);
                    if (idr["DTEXPSENHA"] != DBNull.Value && Convert.ToDateTime(idr["DTEXPSENHA"]) != DateTime.MinValue) usuarioProd.DTEXPSENHA = Convert.ToDateTime(idr["DTEXPSENHA"]);
                    if (idr["QTDEACESSOINV"] != DBNull.Value) usuarioProd.QTDEACESSOINV = Convert.ToInt32(idr["QTDEACESSOINV"]);
                    if (usuarioProd.SISTEMA == 0 && idr["DTULTRENOV"] != DBNull.Value && Convert.ToDateTime(idr["DTULTRENOV"]) != DateTime.MinValue) usuarioProd.DTULTRENOV = Convert.ToDateTime(idr["DTULTRENOV"]);
                    if (idr["TIPOPROD"] != DBNull.Value) usuarioProd.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
                    if (idr["PRZVALCART"] != DBNull.Value) usuarioProd.PRZVALCART = Convert.ToString(idr["PRZVALCART"]);
                    if (idr["LOTEATUAL"] != DBNull.Value) usuarioProd.LOTEATUAL = Convert.ToInt32(idr["LOTEATUAL"]);
                    if (usuarioProd.SISTEMA == 0 && idr["PMO"] != DBNull.Value) usuarioProd.PMO = Convert.ToDecimal(idr["PMO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["BONUS"] != DBNull.Value) usuarioProd.BONUS = Convert.ToDecimal(idr["BONUS"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMPAD"] != DBNull.Value) usuarioProd.LIMPAD = Convert.ToDecimal(idr["LIMPAD"]);
                    if (idr["CTRATV"] != DBNull.Value) usuarioProd.CTRATV = Convert.ToString(idr["CTRATV"]);
                    if (idr["ACRESPAD"] != DBNull.Value) usuarioProd.ACRESPAD = Convert.ToDecimal(idr["ACRESPAD"]);
                    if (idr["AGENCIA"] != DBNull.Value) usuarioProd.AGENCIA = Convert.ToString(idr["AGENCIA"]);
                    if (idr["CONTA"] != DBNull.Value) usuarioProd.CONTA = Convert.ToString(idr["CONTA"]);
                    if (idr["BANCO"] != DBNull.Value) usuarioProd.BANCO = Convert.ToString(idr["BANCO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMCREDITO"] != DBNull.Value) usuarioProd.LIMCREDITO = Convert.ToDecimal(idr["LIMCREDITO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["MAXPARC"] != DBNull.Value) usuarioProd.MAXPARCCRT = Convert.ToInt16(idr["MAXPARC"]);
                    if (usuarioProd.SISTEMA == 0 && idr["MAXPARCCLI"] != DBNull.Value) usuarioProd.MAXPARCCLI = Convert.ToInt16(idr["MAXPARCCLI"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMRISCO"] != DBNull.Value) usuarioProd.LIMRISCOCRT = Convert.ToInt16(idr["LIMRISCO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMRISCOCLI"] != DBNull.Value) usuarioProd.LIMRISCOCLI = Convert.ToInt16(idr["LIMRISCOCLI"]);
                    if (usuarioProd.SISTEMA == 1 && idr["CARGPADVA"] != DBNull.Value) usuarioProd.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                    if (usuarioProd.SISTEMA == 1 && idr["ULTCARGVA"] != DBNull.Value) usuarioProd.ULTCARGVA = Convert.ToInt32(idr["ULTCARGVA"]);
                    if (usuarioProd.SISTEMA == 1 && idr["VCARGAUTO"] != DBNull.Value) usuarioProd.VCARGAUTO = Convert.ToDecimal(idr["VCARGAUTO"]);
                    if (idr["COBRASEGVIA"] != DBNull.Value) usuarioProd.COBRASEGVIA = Convert.ToString(idr["COBRASEGVIA"]) == "S";
                    if (idr["VALORSEGVIA"] != DBNull.Value) usuarioProd.VALORSEGVIA = Convert.ToDecimal(idr["VALORSEGVIA"]);
                }
                listProdutosUsu.Add(usuarioProd);
            }
            idr.Close();
            return listProdutosUsu;
        }

        public List<VUSUARIODEP> GetUsuariosDep(int codCli, string cpf)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" A.ID_USUARIO, A.SISTEMA, PRODUTO, CODCLI, NOMCLI, STACLI, DESTACLI, CPF, NUMDEP, CODFIL, A.CODPAR, DESPAR, NOMUSU, DATATV, DATINC, ");
            sql.AppendLine(" CODSET, MAT, DATGERCRT, GERCRT, GENERICO, CODCRT, A.STA, S.DESTA, DATSTA, TRILHA2, DTVALCART, NOMCRT, SENHA, ");
            sql.AppendLine(" SENHACIELO, SENUSU, DTSENHA, DTEXPSENHA, ULTACESSO, QTDEACESSOINV, ID_CARTAO, DATATUALIZ, ");
            sql.AppendLine(" NACIONALIDADE, CEPCOM, ENDUSUCOM, ENDNUMCOM, ENDCPLCOM, BAIRROCOM, LOCALIDADECOM, UFCOM, TELCOM, ");
            sql.AppendLine(" B.TIPOPROD, NUMVIA, PRZVALCART, CODAG, PMO, LIMPAD, LIMDEP, CTRATV, ACRESPAD, AGENCIA, CONTA, BANCO, LIMCREDITO, ");
            sql.AppendLine(" MAXPARC, HABTRANSDIG, LIMRISCO, CODSIND, SINDICALIZADO, BONUS, CARGPADVA, ULTCARGVA, VCARGAUTO, SEXO, DATNAS, COBRASEGVIA, VALORSEGVIA  ");
            sql.AppendLine(" FROM VPRODUTOSUSU A WITH (NOLOCK) ");
            sql.AppendLine("INNER JOIN PRODUTO B WITH (NOLOCK) ON A.CODPROD = B.CODPROD ");
            sql.AppendLine("INNER JOIN PARENTESCO P ON A.CODPAR = P.CODPAR ");
            sql.AppendLine("INNER JOIN STATUS S ON A.STA = S.STA ");
            sql.AppendLine(" WHERE CPF = @CPF AND CODCLI = @CODCLI AND NUMDEP <> 0 ");
            sql.AppendLine("ORDER BY NUMDEP ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);

            var idr = db.ExecuteReader(cmd);
            var listProdutosUsu = new List<VUSUARIODEP>();

            while (idr.Read())
            {
                var usuarioProd = new VUSUARIODEP();
                usuarioProd.ID_USUARIO = Convert.ToInt32(idr["ID_USUARIO"]);
                usuarioProd.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                usuarioProd.NOMCLI = Convert.ToString(idr["NOMCLI"]);
                usuarioProd.STACLI = Convert.ToString(idr["STACLI"]);
                usuarioProd.DESTACLI = Convert.ToString(idr["DESTACLI"]);
                usuarioProd.SISTEMA = Convert.ToInt16(idr["SISTEMA"]);
                usuarioProd.PRODUTO = Convert.ToString(idr["PRODUTO"]);
                usuarioProd.CPF = Convert.ToString(idr["CPF"]);
                usuarioProd.NOMUSU = Convert.ToString(idr["NOMUSU"]);
                usuarioProd.STA = Convert.ToString(idr["STA"]);
                usuarioProd.DESTA = Convert.ToString(idr["DESTA"]);


                if (!string.IsNullOrEmpty(usuarioProd.STA))
                {
                    if (idr["CODCRT"] != DBNull.Value) usuarioProd.CODCRT = Convert.ToString(idr["CODCRT"]);
                    if (idr["NOMCRT"] != DBNull.Value) usuarioProd.NOMCRT = Convert.ToString(idr["NOMCRT"]);
                    if (idr["NUMDEP"] != DBNull.Value) usuarioProd.NUMDEP = Convert.ToInt16(idr["NUMDEP"]);
                    if (idr["CODFIL"] != DBNull.Value) usuarioProd.CODFIL = Convert.ToInt16(idr["CODFIL"]);
                    if (idr["SEXO"] != DBNull.Value && (Convert.ToString(idr["SEXO"]) == "M" || Convert.ToString(idr["SEXO"]) == "F"))
                        usuarioProd.SEXO = Convert.ToString(idr["SEXO"]);
                    if (idr["DATNAS"] != DBNull.Value) usuarioProd.DATNAS = Convert.ToString(idr["DATNAS"]);
                    if (idr["CODPAR"] != DBNull.Value) usuarioProd.CODPAR = Convert.ToInt16(idr["CODPAR"]);
                    if (idr["DESPAR"] != DBNull.Value) usuarioProd.DESPAR = Convert.ToString(idr["DESPAR"]);
                    if (idr["DATATV"] != DBNull.Value && Convert.ToDateTime(idr["DATATV"]) != DateTime.MinValue) usuarioProd.DATATV = Convert.ToDateTime(idr["DATATV"]);
                    if (idr["DATINC"] != DBNull.Value && Convert.ToDateTime(idr["DATINC"]) != DateTime.MinValue) usuarioProd.DATINC = Convert.ToDateTime(idr["DATINC"]);
                    if (idr["CODSET"] != DBNull.Value) usuarioProd.CODSET = Convert.ToString(idr["CODSET"]);
                    if (idr["MAT"] != DBNull.Value) usuarioProd.MAT = Convert.ToString(idr["MAT"]);
                    if (idr["DATGERCRT"] != DBNull.Value && Convert.ToDateTime(idr["DATGERCRT"]) != DateTime.MinValue) usuarioProd.DATGERCRT = Convert.ToDateTime(idr["DATGERCRT"]);
                    if (idr["GERCRT"] != DBNull.Value) usuarioProd.GERCRT = Convert.ToString(idr["GERCRT"]);
                    if (idr["DATSTA"] != DBNull.Value && Convert.ToDateTime(idr["DATSTA"]) != DateTime.MinValue) usuarioProd.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                    if (idr["TRILHA2"] != DBNull.Value) usuarioProd.TRILHA2 = Convert.ToString(idr["TRILHA2"]);
                    if (idr["DTVALCART"] != DBNull.Value) usuarioProd.DTVALCART = Convert.ToString(idr["DTVALCART"]);
                    if (idr["SENHA"] != DBNull.Value) usuarioProd.SENHA = Convert.ToString(idr["SENHA"]);
                    if (idr["DTSENHA"] != DBNull.Value && Convert.ToDateTime(idr["DTSENHA"]) != DateTime.MinValue) usuarioProd.DTSENHA = Convert.ToDateTime(idr["DTSENHA"]);
                    if (idr["DTEXPSENHA"] != DBNull.Value && Convert.ToDateTime(idr["DTEXPSENHA"]) != DateTime.MinValue) usuarioProd.DTEXPSENHA = Convert.ToDateTime(idr["DTEXPSENHA"]);
                    if (idr["QTDEACESSOINV"] != DBNull.Value) usuarioProd.QTDEACESSOINV = Convert.ToInt32(idr["QTDEACESSOINV"]);
                    if (idr["TIPOPROD"] != DBNull.Value) usuarioProd.TIPOPROD = Convert.ToInt32(idr["TIPOPROD"]);
                    if (idr["PRZVALCART"] != DBNull.Value) usuarioProd.PRZVALCART = Convert.ToString(idr["PRZVALCART"]);
                    if (usuarioProd.SISTEMA == 0 && idr["PMO"] != DBNull.Value) usuarioProd.PMO = Convert.ToDecimal(idr["PMO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMPAD"] != DBNull.Value) usuarioProd.LIMPAD = Convert.ToDecimal(idr["LIMPAD"]);
                    if (usuarioProd.SISTEMA == 0 && (idr["LIMDEP"] != DBNull.Value && Convert.ToDecimal(idr["LIMDEP"]) > 0m)) usuarioProd.LIMDEP = Convert.ToString(idr["LIMDEP"]);
                    if (idr["CTRATV"] != DBNull.Value) usuarioProd.CTRATV = Convert.ToString(idr["CTRATV"]);
                    if (idr["ACRESPAD"] != DBNull.Value) usuarioProd.ACRESPAD = Convert.ToDecimal(idr["ACRESPAD"]);
                    if (idr["AGENCIA"] != DBNull.Value) usuarioProd.AGENCIA = Convert.ToString(idr["AGENCIA"]);
                    if (idr["CONTA"] != DBNull.Value) usuarioProd.CONTA = Convert.ToString(idr["CONTA"]);
                    if (idr["BANCO"] != DBNull.Value) usuarioProd.BANCO = Convert.ToString(idr["BANCO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMCREDITO"] != DBNull.Value) usuarioProd.LIMCREDITO = Convert.ToDecimal(idr["LIMCREDITO"]);
                    if (usuarioProd.SISTEMA == 0 && idr["MAXPARC"] != DBNull.Value) usuarioProd.MAXPARCCRT = Convert.ToInt16(idr["MAXPARC"]);
                    if (usuarioProd.SISTEMA == 0 && idr["LIMRISCO"] != DBNull.Value) usuarioProd.LIMRISCOCRT = Convert.ToInt16(idr["LIMRISCO"]);
                    if (usuarioProd.SISTEMA == 1 && idr["CARGPADVA"] != DBNull.Value) usuarioProd.CARGPADVA = Convert.ToDecimal(idr["CARGPADVA"]);
                    if (usuarioProd.SISTEMA == 1 && idr["ULTCARGVA"] != DBNull.Value) usuarioProd.ULTCARGVA = Convert.ToInt32(idr["ULTCARGVA"]);
                    if (usuarioProd.SISTEMA == 1 && idr["VCARGAUTO"] != DBNull.Value) usuarioProd.VCARGAUTO = Convert.ToDecimal(idr["VCARGAUTO"]);

                    if (idr["COBRASEGVIA"] != DBNull.Value) usuarioProd.COBRASEGVIA = Convert.ToString(idr["COBRASEGVIA"]) == "S";
                    if (idr["VALORSEGVIA"] != DBNull.Value) usuarioProd.VALORSEGVIA = Convert.ToDecimal(idr["VALORSEGVIA"]);
                }
                listProdutosUsu.Add(usuarioProd);
            }
            idr.Close();
            return listProdutosUsu;
        }

        public List<JustSegViaCard> ColecaoJustSegViaCard()
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();

            sql.AppendLine(" SELECT");
            sql.AppendLine(" codJus_Seg_Via_Card, nomJustificativa, Cobrar ");
            sql.AppendLine(" FROM JUS_SEG_VIA_CARD WITH (NOLOCK) ");
            sql.AppendLine(" WHERE Ativo = 'S'");
            sql.AppendLine(" ORDER BY nomJustificativa");

            var cmd = new SqlCommand(sql.ToString(), conn);
            conn.Open();
            var idr = cmd.ExecuteReader();
            var colecaoJust = new List<JustSegViaCard>();

            while (idr.Read())
            {
                var Just = new JustSegViaCard();
                Just.codJus_Seg_Via_Card = Convert.ToInt32(idr["codJus_Seg_Via_Card"]);

                Just.CobrarSegundaVia = (idr["Cobrar"].ToString() == "N") ? "Não será cobrada a segunda via." : "Será cobrada a segunda via.";

                Just.nomJustificativa = idr["nomJustificativa"].ToString();

                colecaoJust.Add(Just);
            }
            idr.Close();
            if (conn.State == ConnectionState.Open)
                conn.Close();

            return colecaoJust;
        }
        public List<VRESUMOUSU> ColecaoUsuario(string Filtro)
        {
            var conn = new SqlConnection();
            conn.ConnectionString = BDTELENET;
            var sql = new StringBuilder();


            sql.AppendLine(" SELECT");
            sql.AppendLine(" ID_USUARIO, CPF, CODCRT, CODCRTMASC, NOMUSU, TIPO, NUMDEP, CODCLI, NOMCLI, SISTEMA, PRODUTO, STA, DESTA, QTDDEP, MAT, BLOQCARTUSU ");
            sql.AppendLine(" FROM VRESUMOUSU WITH (NOLOCK) ");

            if (!string.IsNullOrEmpty(Filtro))
                sql.AppendLine(string.Format("WHERE {0} ", Filtro));
            sql.AppendLine(" ORDER BY CPF, NUMDEP, CODCRT");
            var cmd = new SqlCommand(sql.ToString(), conn);
            conn.Open();
            var idr = cmd.ExecuteReader();
            var colecaoUsuario = new List<VRESUMOUSU>();

            while (idr.Read())
            {
                var cadUsuario = new VRESUMOUSU();
                cadUsuario.ID_USUARIO = Convert.ToInt32(idr["ID_USUARIO"]);
                cadUsuario.CPF = idr["CPF"].ToString();
                if (idr["CODCRT"] != DBNull.Value) cadUsuario.CODCRT = idr["CODCRT"].ToString();
                if (idr["CODCRTMASC"] != DBNull.Value) cadUsuario.CODCRTMASC = idr["CODCRTMASC"].ToString();
                if (idr["NOMUSU"] != DBNull.Value) cadUsuario.NOMUSU = idr["NOMUSU"].ToString();
                if (idr["TIPO"] != DBNull.Value) cadUsuario.TIPO = idr["TIPO"].ToString();
                if (idr["NUMDEP"] != DBNull.Value) cadUsuario.NUMDEP = Convert.ToInt32(idr["NUMDEP"]);
                if (idr["QTDDEP"] != DBNull.Value) cadUsuario.QTDDEP = Convert.ToInt32(idr["QTDDEP"]);
                if (idr["CODCLI"] != DBNull.Value) cadUsuario.CODCLI = Convert.ToInt32(idr["CODCLI"]);
                if (idr["NOMCLI"] != DBNull.Value) cadUsuario.NOMCLI = idr["NOMCLI"].ToString();
                if (idr["STA"] != DBNull.Value) cadUsuario.STA = idr["STA"].ToString();
                if (idr["DESTA"] != DBNull.Value) cadUsuario.DESTA = idr["DESTA"].ToString();
                if (idr["SISTEMA"] == DBNull.Value) cadUsuario.SISTEMA = 2;
                else cadUsuario.SISTEMA = Convert.ToInt32(idr["SISTEMA"]);
                if (idr["PRODUTO"] != DBNull.Value) cadUsuario.PRODUTO = idr["PRODUTO"].ToString();
                if (idr["MAT"] != DBNull.Value) cadUsuario.MAT = idr["MAT"].ToString();
                if (idr["BLOQCARTUSU"] != DBNull.Value) cadUsuario.BLOQCARTUSU = idr["BLOQCARTUSU"].ToString() == "S";
                colecaoUsuario.Add(cadUsuario);
            }
            idr.Close();
            if (conn.State == ConnectionState.Open)
                conn.Close();

            return colecaoUsuario;
        }

        public SALDO GetSaldo(VPRODUTOSUSU usuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                var saldos = new SALDO();
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_CONSULTA_SALDO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    saldos.SALDOPOS = Convert.ToDecimal(reader["SALDOPOS"]);
                    saldos.SALDOPRE = Convert.ToDecimal(reader["SALDOPRE"]);
                    saldos.VALCOMP = Convert.ToDecimal(reader["VALCOMP"]);
                    saldos.GASTOATUAL = Convert.ToDecimal(reader["GASTOATUAL"]);
                }
                return saldos;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
        public CalcValorParcela GetCalcParcela(int numParcela, decimal vlLimiete, int limiteRisco, decimal saldoAtual, decimal vlComprometido)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("CALCULADORA_PARC_NETCARD", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();


                cmd.Parameters.Add("@NUMPARC", SqlDbType.Int).Value = numParcela;
                cmd.Parameters.Add("@LIMITE", SqlDbType.Decimal).Value = vlLimiete;
                cmd.Parameters.Add("@LIMRISCO", SqlDbType.Int).Value = limiteRisco;
                cmd.Parameters.Add("@SALDO", SqlDbType.Decimal).Value = saldoAtual;
                cmd.Parameters.Add("@COMPFUTURO", SqlDbType.Decimal).Value = vlComprometido;

                reader = cmd.ExecuteReader();

                CalcValorParcela calc = new CalcValorParcela();

                if (reader.Read())
                {
                    calc.CodCalcParcTotalCompra = Convert.ToString(reader["TOTALCOMPRA"]);
                    calc.CodCalcParcValorParcela = Convert.ToString(reader["VALPARC"]);
                }
                return calc;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public decimal GetSaldoComprometido(VPRODUTOSUSU usuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_CONSULTA_SALDO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return Convert.ToDecimal(reader["VALCOMP"]);
                }
                return 0m;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
        

        #endregion

        #region CRUD

        public int Inserir(CADUSUARIO cadUsuario, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;
            var idUsuario = 0;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_USUARIO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cadUsuario.CPF;
                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = cadUsuario.NOMUSU;

                if(cadUsuario.DATNAS !=  DateTime.MinValue) cmd.Parameters.Add("@DATNAS", SqlDbType.DateTime).Value = cadUsuario.DATNAS;

                cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@PAI", SqlDbType.VarChar).Value = cadUsuario.PAI;
                cmd.Parameters.Add("@MAE", SqlDbType.VarChar).Value = cadUsuario.MAE;
                cmd.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = cadUsuario.SEXO;
                cmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = cadUsuario.TEL;
                cmd.Parameters.Add("@CEL", SqlDbType.VarChar).Value = cadUsuario.CEL;
                cmd.Parameters.Add("@EMA", SqlDbType.VarChar).Value = cadUsuario.EMA;
                cmd.Parameters.Add("@RG", SqlDbType.VarChar).Value = cadUsuario.RG;
                cmd.Parameters.Add("@ORGEXPRG", SqlDbType.VarChar).Value = cadUsuario.ORGEXPRG;
                cmd.Parameters.Add("@NATURALIDADE", SqlDbType.VarChar).Value = cadUsuario.NATURALIDADE;
                cmd.Parameters.Add("@NACIONALIDADE", SqlDbType.VarChar).Value = cadUsuario.NACIONALIDADE;
                cmd.Parameters.Add("@NIS", SqlDbType.VarChar).Value = cadUsuario.NIS;

                // Endereco Residencial
                cmd.Parameters.Add("@ENDUSU", SqlDbType.VarChar).Value = cadUsuario.ENDUSU;
                cmd.Parameters.Add("@ENDNUMUSU", SqlDbType.VarChar).Value = cadUsuario.ENDNUMUSU;
                cmd.Parameters.Add("@ENDCPL", SqlDbType.VarChar).Value = cadUsuario.ENDCPL;
                cmd.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = cadUsuario.BAIRRO;
                cmd.Parameters.Add("@LOCALIDADE", SqlDbType.VarChar).Value = cadUsuario.LOCALIDADE;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cadUsuario.UF == null ? string.Empty : cadUsuario.UF;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cadUsuario.CEP;

                // Endereco Comercial
                cmd.Parameters.Add("@ENDUSUCOM", SqlDbType.VarChar).Value = cadUsuario.ENDUSUCOM;
                cmd.Parameters.Add("@ENDNUMCOM", SqlDbType.Int).Value = cadUsuario.ENDNUMCOM;
                cmd.Parameters.Add("@ENDCPLCOM", SqlDbType.VarChar).Value = cadUsuario.ENDCPLCOM;
                cmd.Parameters.Add("@BAIRROCOM", SqlDbType.VarChar).Value = cadUsuario.BAIRROCOM;
                cmd.Parameters.Add("@LOCALIDADECOM", SqlDbType.VarChar).Value = cadUsuario.LOCALIDADECOM;
                cmd.Parameters.Add("@UFCOM", SqlDbType.VarChar).Value = cadUsuario.UFCOM == null ? string.Empty : cadUsuario.UFCOM;
                cmd.Parameters.Add("@CEPCOM", SqlDbType.VarChar).Value = cadUsuario.CEPCOM;
                cmd.Parameters.Add("@TELCOM", SqlDbType.VarChar).Value = cadUsuario.TELCOM;

                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "OK")
                    {
                        mensagem = "Registro incluído com sucesso.";
                        idUsuario = Convert.ToInt32(reader["ID_USUARIO"]);
                        return idUsuario;
                    }
                    else
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return idUsuario;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool IncluirCartaoPos(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CARTAO2", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = usuarioProd.ID_USUARIO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuarioProd.CODCLI;
                cmd.Parameters.Add("@SISTEMA", SqlDbType.VarChar).Value = usuarioProd.SISTEMA;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuarioProd.CPF;
                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usuarioProd.NOMUSU;
                cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = usuarioProd.DATINC;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = usuarioProd.NUMDEP;
                //cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = usuarioProd.STATUS;
                //cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = usuarioProd.DATSTA;
                cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = usuarioProd.CODFIL;
                cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = usuarioProd.CODSET;
                cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usuarioProd.MAT;
                cmd.Parameters.Add("@LIMPAD", SqlDbType.Decimal).Value = usuarioProd.LIMPAD;
                cmd.Parameters.Add("@NUMMAXPARC", SqlDbType.Int).Value = usuarioProd.MAXPARCCRT;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = usuarioProd.CODPAR;
                cmd.Parameters.Add("@LIMRISCO", SqlDbType.Int).Value = usuarioProd.LIMRISCOCRT;
                cmd.Parameters.Add("@BANCO", SqlDbType.VarChar).Value = usuarioProd.BANCO;
                cmd.Parameters.Add("@AGENCIA", SqlDbType.VarChar).Value = usuarioProd.AGENCIA;
                cmd.Parameters.Add("@CONTA", SqlDbType.VarChar).Value = usuarioProd.CONTA;

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem += "Registro incluído com sucesso.";
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool IncluirCartaoPre(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_INSERE_CARTAO2", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = usuarioProd.ID_USUARIO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuarioProd.CODCLI;
                cmd.Parameters.Add("@SISTEMA", SqlDbType.VarChar).Value = usuarioProd.SISTEMA;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuarioProd.CPF;
                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usuarioProd.NOMUSU;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = usuarioProd.NUMDEP;
                cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = usuarioProd.DATINC;
                //cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = usuarioProd.STATUS;
                //cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = usuarioProd.DATSTA;
                cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = usuarioProd.CODFIL;
                cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = usuarioProd.CODSET;
                cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usuarioProd.MAT;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = usuarioProd.CODPAR;
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usuarioProd.CARGPADVA;
                
                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem += "Registro incluído com sucesso.";
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
        
        public bool Alterar(CADUSUARIO cadUsuario, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_USUARIO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = cadUsuario.ID_USUARIO;
                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = cadUsuario.NOMUSU;

                if (cadUsuario.DATNAS != DateTime.MinValue) cmd.Parameters.Add("@DATNAS", SqlDbType.DateTime).Value = cadUsuario.DATNAS;                
                cmd.Parameters.Add("@PAI", SqlDbType.VarChar).Value = cadUsuario.PAI;
                cmd.Parameters.Add("@MAE", SqlDbType.VarChar).Value = cadUsuario.MAE;
                cmd.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = cadUsuario.SEXO;
                cmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = cadUsuario.TEL;
                cmd.Parameters.Add("@CEL", SqlDbType.VarChar).Value = cadUsuario.CEL;
                cmd.Parameters.Add("@EMA", SqlDbType.VarChar).Value = cadUsuario.EMA;
                cmd.Parameters.Add("@RG", SqlDbType.VarChar).Value = cadUsuario.RG;
                cmd.Parameters.Add("@ORGEXPRG", SqlDbType.VarChar).Value = cadUsuario.ORGEXPRG;
                cmd.Parameters.Add("@NATURALIDADE", SqlDbType.VarChar).Value = cadUsuario.NATURALIDADE;
                cmd.Parameters.Add("@NACIONALIDADE", SqlDbType.VarChar).Value = cadUsuario.NACIONALIDADE;
                cmd.Parameters.Add("@NIS", SqlDbType.VarChar).Value = cadUsuario.NIS;

                // Endereco Residencial
                cmd.Parameters.Add("@ENDUSU", SqlDbType.VarChar).Value = cadUsuario.ENDUSU;
                cmd.Parameters.Add("@ENDNUMUSU", SqlDbType.VarChar).Value = cadUsuario.ENDNUMUSU;
                cmd.Parameters.Add("@ENDCPL", SqlDbType.VarChar).Value = cadUsuario.ENDCPL;
                cmd.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = cadUsuario.BAIRRO;
                cmd.Parameters.Add("@LOCALIDADE", SqlDbType.VarChar).Value = cadUsuario.LOCALIDADE;
                cmd.Parameters.Add("@UF", SqlDbType.VarChar).Value = cadUsuario.UF;
                cmd.Parameters.Add("@CEP", SqlDbType.VarChar).Value = cadUsuario.CEP;                

                // Endereco Comercial
                cmd.Parameters.Add("@ENDUSUCOM", SqlDbType.VarChar).Value = cadUsuario.ENDUSUCOM;
                cmd.Parameters.Add("@ENDNUMCOM", SqlDbType.VarChar).Value = cadUsuario.ENDNUMCOM;
                cmd.Parameters.Add("@ENDCPLCOM", SqlDbType.VarChar).Value = cadUsuario.ENDCPLCOM;
                cmd.Parameters.Add("@BAIRROCOM", SqlDbType.VarChar).Value = cadUsuario.BAIRROCOM;
                cmd.Parameters.Add("@LOCALIDADECOM", SqlDbType.VarChar).Value = cadUsuario.LOCALIDADECOM;
                cmd.Parameters.Add("@UFCOM", SqlDbType.VarChar).Value = cadUsuario.UFCOM;
                cmd.Parameters.Add("@CEPCOM", SqlDbType.VarChar).Value = cadUsuario.CEPCOM;
                cmd.Parameters.Add("@TELCOM", SqlDbType.VarChar).Value = cadUsuario.TELCOM;

                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "OK")
                    {
                        retorno = "Registro alterado com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }

                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AlterarCartaoPos(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CARTAO2", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                    
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = usuarioProd.ID_USUARIO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuarioProd.CODCLI;
                cmd.Parameters.Add("@SISTEMA", SqlDbType.VarChar).Value = usuarioProd.SISTEMA;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuarioProd.CPF;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = usuarioProd.NUMDEP;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = usuarioProd.STATUS;
                cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = usuarioProd.DATSTA;
                cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = usuarioProd.CODFIL;
                cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = usuarioProd.CODSET;
                cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usuarioProd.MAT;
                cmd.Parameters.Add("@ALTLIMITE", SqlDbType.VarChar).Value = usuarioProd.ALTLIMITE;
                cmd.Parameters.Add("@IMEDIATO", SqlDbType.VarChar).Value = usuarioProd.IMEDIATO;
                cmd.Parameters.Add("@LIMPAD", SqlDbType.Decimal).Value = usuarioProd.LIMPAD;
                cmd.Parameters.Add("@PREMIO", SqlDbType.Decimal).Value = usuarioProd.PMO;
                cmd.Parameters.Add("@NUMMAXPARC", SqlDbType.Int).Value = usuarioProd.MAXPARCCRT;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = usuarioProd.CODPAR;
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usuarioProd.CARGPADVA;
                cmd.Parameters.Add("@LIMRISCO", SqlDbType.Int).Value = usuarioProd.LIMRISCOCRT;
                cmd.Parameters.Add("@BANCO", SqlDbType.VarChar).Value = usuarioProd.BANCO;
                cmd.Parameters.Add("@AGENCIA", SqlDbType.VarChar).Value = usuarioProd.AGENCIA;
                cmd.Parameters.Add("@CONTA", SqlDbType.VarChar).Value = usuarioProd.CONTA;
                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usuarioProd.NOMUSU;

                
                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);

                        if (string.IsNullOrEmpty(mensagem))
                            mensagem = "Registro alterado com sucesso.";

                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AlterarCartaoPre(VPRODUTOSUSU usuarioProd, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ALTERA_CARTAO2", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = usuarioProd.ID_USUARIO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuarioProd.CODCLI;
                cmd.Parameters.Add("@SISTEMA", SqlDbType.VarChar).Value = usuarioProd.SISTEMA;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuarioProd.CPF;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = usuarioProd.NUMDEP;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = usuarioProd.STATUS;
                cmd.Parameters.Add("@DATSTA", SqlDbType.DateTime).Value = usuarioProd.DATSTA;
                cmd.Parameters.Add("@CODFIL", SqlDbType.Int).Value = usuarioProd.CODFIL;
                cmd.Parameters.Add("@CODSET", SqlDbType.VarChar).Value = usuarioProd.CODSET;
                cmd.Parameters.Add("@MAT", SqlDbType.VarChar).Value = usuarioProd.MAT;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = usuarioProd.CODPAR;
                cmd.Parameters.Add("@CARGPADVA", SqlDbType.Decimal).Value = usuarioProd.CARGPADVA;

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem = "Registro alterado com sucesso.";
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ExcluirCadUsuario(string cpf, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_USUARIO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();                
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpf;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Cadastro de usuário excluido com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }

                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ExcluirCartao(int sistema, int codCli, string cpf, int numDep, out string retorno)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            retorno = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_EXCLUI_CARTAO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = sistema;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = codCli;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpf;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = numDep;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retorno = Convert.ToString(reader["RETORNO"]);
                    if (retorno == "0")
                    {
                        retorno = "Cartão excluido com sucesso.";
                        return true;
                    }
                    else
                    {
                        retorno = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ManuDependente(VUSUARIODEP usuarioDep, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_MANU_DEPEND", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuarioDep.SISTEMA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = usuarioDep.ID_USUARIO;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuarioDep.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuarioDep.CPF;
                cmd.Parameters.Add("@NOMUSU", SqlDbType.VarChar).Value = usuarioDep.NOMUSU;
                cmd.Parameters.Add("@LIMDEP", SqlDbType.Decimal).Value = Convert.ToDecimal(usuarioDep.LIMDEP);
                if (usuarioDep.DATNAS != null)
                    cmd.Parameters.Add("@DATNAS", SqlDbType.DateTime).Value = Convert.ToDateTime(usuarioDep.DATNAS);
                cmd.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = usuarioDep.SEXO;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = usuarioDep.NUMDEP;
                cmd.Parameters.Add("@CODPAR", SqlDbType.Int).Value = usuarioDep.CODPAR;
                cmd.Parameters.Add("@DATINC", SqlDbType.DateTime).Value = usuarioDep.DATINC;
                cmd.Parameters.Add("@CODOPE", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@EXCLUIR", SqlDbType.Char).Value = usuarioDep.EXCLUIDO ? 'S' : 'N';

                // Executando o commando e obtendo o resultado
                reader = cmd.ExecuteReader();

                // Exibindo os registros
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        //mensagem = string.Format("Alteração do dependente {0} realizada com sucesso.", usuarioDep.NOMUSU);
                        return true;
                    }
                    else
                    {
                        mensagem = string.Format(string.Format("Não foi possível alterar o dependente {0}. Erro: {1}", usuarioDep.NOMUSU, Convert.ToString(reader["MENSAGEM"])));
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool AtualizaUsuarioPreJuncao(USUARIO_VA usu)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ATUAILIZA_USUARIO_PRE_JUNCAO ", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usu.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usu.CPF;
                cmd.Parameters.Add("@NUMDEP", SqlDbType.Int).Value = usu.NUMDEP;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var mensagem = Convert.ToString(reader["RETORNO"]);
                    if (mensagem == "0")
                    {
                        return true;
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region Get Gastos

        public Double GastoHoje(VPRODUTOSUSU usuario)
        {
            Double Gasto = 0;            
            Database db = new SqlDatabase(BDTELENET);
            string sql = "PROC_GASTO_HOJE";

            DbCommand cmd = db.GetStoredProcCommand(sql);

            db.AddInParameter(cmd, "SISTEMA", DbType.String, usuario.SISTEMA);
            db.AddInParameter(cmd, "CODCLI", DbType.String, usuario.CODCLI.ToString().PadLeft(5, '0'));
            db.AddInParameter(cmd, "CPF", DbType.String, usuario.CPF);

            IDataReader idr = db.ExecuteReader(cmd);

            if (idr.Read())
                Gasto = Convert.ToDouble(idr["GASTO_HOJE"]);

            idr.Close();

            return Gasto;
        }

        public Double GastoProcessado(VPRODUTOSUSU usuario, DateTime dataInicio, DateTime dataFim, int lote)
        {
            Double Gasto = 0;

            if (usuario.CODCLI != 0)
            {
                Database db = new SqlDatabase(BDTELENET);
                string sql = "PROC_GASTO_PROC";

                DbCommand cmd = db.GetStoredProcCommand(sql);

                db.AddInParameter(cmd, "SISTEMA", DbType.String, usuario.SISTEMA);
                db.AddInParameter(cmd, "CODCLI", DbType.String, usuario.CODCLI.ToString().PadLeft(5, '0'));
                db.AddInParameter(cmd, "CPF", DbType.String, usuario.CPF);

                if (dataInicio != DateTime.MaxValue && dataInicio != DateTime.MinValue)
                    db.AddInParameter(cmd, "DATAINI", DbType.DateTime, dataFim);

                if (dataFim != DateTime.MaxValue && dataFim != DateTime.MinValue)
                    db.AddInParameter(cmd, "DATAFIM", DbType.DateTime, dataFim);

                db.AddInParameter(cmd, "LOTE", DbType.Int32, lote);

                IDataReader idr = db.ExecuteReader(cmd);

                if (idr.Read())
                    Gasto = string.IsNullOrEmpty(idr["GASTO_PROC"].ToString()) ? 0 : Convert.ToDouble(idr["GASTO_PROC"]);

                idr.Close();
            }
            return Gasto;
        }

        #endregion

        #region GET Observacoes

        public List<USUARIO_OBS> ObservacoesPos(int codCli, string cpf)
        {
            var ColecaoObservacoes = new List<USUARIO_OBS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("CODCLI, CPF, DATA, OBS ");
            sql.AppendLine("FROM OBSUSU WITH (NOLOCK) ");
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

                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();
            return ColecaoObservacoes;
        }

        public List<USUARIO_OBS> ObservacoesPre(int codCli, string cpf)
        {
            var ColecaoObservacoes = new List<USUARIO_OBS>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("CODCLI, CPF, DATA, OBS ");
            sql.AppendLine("FROM OBSUSUVA WITH (NOLOCK) ");
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

                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();
            return ColecaoObservacoes;
        }

        #endregion

        #region GET Taxas e Benefícios

        public List<TAXAUSUARIO> GetTaxaUsuario(VPRODUTOSUSU usuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listTaxa = new List<TAXAUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_LISTA_TAXA_USU", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                //cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var taxaUsu = new TAXAUSUARIO();
                    taxaUsu.CODTAXA = Convert.ToInt32(reader["CODTAXA"]);
                    taxaUsu.NUMPAC = Convert.ToInt32(reader["NUMPAC"]);
                    taxaUsu.NUMULTPAC = Convert.ToInt32(reader["NUMULTPAC"]);                    
                    if (reader["DATRENOV"] != DBNull.Value) taxaUsu.DATRENOV = Convert.ToDateTime(reader["DATRENOV"]);
                    if (reader["DATTAXA"] != DBNull.Value) taxaUsu.DATTAXA = Convert.ToDateTime(reader["DATTAXA"]);                    
                    taxaUsu.VALTAXA = Convert.ToDecimal(reader["VALTAXA"]);
                    taxaUsu.NOMTAXA = Convert.ToString(reader["NOMTAXA"]);
                    taxaUsu.CTRATV = Convert.ToString(reader["CTRATV"]);
                    taxaUsu.PAGANU = Convert.ToString(reader["PAGANU"]);
                    taxaUsu.INDIVIDUAL = Convert.ToString(reader["INDIVIDUAL"]);
                    taxaUsu.TIPO = Convert.ToInt32(reader["TIPO"]);
                    taxaUsu.NUMDEP = Convert.ToInt32(reader["NUMDEP"]);
                    listTaxa.Add(taxaUsu);
                }
                return listTaxa;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public List<TAXAUSUARIO> GetTaxasAAssociar(VPRODUTOSUSU usuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listTaxa = new List<TAXAUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_LISTA_TAXA_USU_ASSOC", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                //cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var taxaUsu = new TAXAUSUARIO();                    
                    taxaUsu.CODTAXA = Convert.ToInt32(reader["CODTAXA"]);
                    taxaUsu.NUMPAC = Convert.ToInt32(reader["NUMPAC"]);                    
                    if (reader["DATTAXA"] != DBNull.Value) taxaUsu.DATTAXA = Convert.ToDateTime(reader["DATTAXA"]);
                    taxaUsu.VALTAXA = Convert.ToDecimal(reader["VALTIT"]);
                    taxaUsu.NOMTAXA = Convert.ToString(reader["NOMTAXA"]);
                    taxaUsu.ABREVCLASSE = Convert.ToString(reader["ABREVCLASSE"]);
                    taxaUsu.TRENOVA = Convert.ToString(reader["TRENOVA"]);
                    taxaUsu.VALDEP = Convert.ToDecimal(reader["VALDEP"]);
                    taxaUsu.ASSOCIADA = Convert.ToString(reader["ASSOCIADA"]);
                    listTaxa.Add(taxaUsu);
                }
                return listTaxa;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public string AssociarTaxas(VPRODUTOSUSU usuario, int codTaxa)
        {
            var mensagem = string.Empty;
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listTaxa = new List<TAXAUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ASSOCIA_TAXAUSU", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
                cmd.Parameters.Add("@CODTAXA", SqlDbType.Int).Value = codTaxa;
                cmd.Parameters.Add("@ID_OPERADOR", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@ORIGEM_OPERADOR", SqlDbType.VarChar).Value = "NC";

                reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem += "Associação realizada com sucesso.";
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return mensagem;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public string DesassociarTaxas(VPRODUTOSUSU usuario, int codTaxa)
        {
            var mensagem = string.Empty;
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listTaxa = new List<TAXAUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_DESASSOCIA_TAXAUSU", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
                cmd.Parameters.Add("@CODTAXA", SqlDbType.Int).Value = codTaxa;
                cmd.Parameters.Add("@ID_OPERADOR", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@ORIGEM_OPERADOR", SqlDbType.VarChar).Value = "NC";

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem += "Desasociação realizada com sucesso.";
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return mensagem;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public List<BENEFICIOUSUARIO> GetBeneficiosUsuario(VPRODUTOSUSU usuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listBeneficio = new List<BENEFICIOUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_LISTA_BENEF_USU", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                //cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var beneficioUsu = new BENEFICIOUSUARIO();
                    beneficioUsu.CODBENEF = Convert.ToInt32(reader["CODBENEF"]);
                    if (reader["DATRENOV"] != DBNull.Value) beneficioUsu.DATRENOV = Convert.ToDateTime(reader["DATRENOV"]);
                    if (reader["DATBENEF"] != DBNull.Value) beneficioUsu.DATBENEF = Convert.ToDateTime(reader["DATBENEF"]);
                    beneficioUsu.VALBENEF = Convert.ToDecimal(reader["VALBENEF"]);
                    beneficioUsu.NOMBENEF = Convert.ToString(reader["NOMBENEF"]);
                    beneficioUsu.COMPULSORIO = Convert.ToString(reader["COMPULSORIO"]);
                    beneficioUsu.SUBBENEF = Convert.ToString(reader["SUBBENEF"]);
                    beneficioUsu.PERSUB = Convert.ToInt32(reader["PERSUB"]);
                    if (reader["DTCARENCIA"] != DBNull.Value) beneficioUsu.DTCARENCIA = Convert.ToDateTime(reader["DTCARENCIA"]);
                    if (reader["DTVIGENCIA"] != DBNull.Value) beneficioUsu.DTVIGENCIA = Convert.ToDateTime(reader["DTVIGENCIA"]);
                    beneficioUsu.DTASSOC = Convert.ToDateTime(reader["DTASSOC"]);
                    beneficioUsu.NUMDEP = Convert.ToInt32(reader["NUMDEP"]);
                    listBeneficio.Add(beneficioUsu);
                }
                return listBeneficio;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public List<BENEFICIOUSUARIO> GetBeneficiosAAssociar(VPRODUTOSUSU usuario)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listBene = new List<BENEFICIOUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_LISTA_BENEF_USU_ASSOC", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                //cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var beneUsu = new BENEFICIOUSUARIO();
                    beneUsu.CODBENEF = Convert.ToInt32(reader["CODBENEF"]);
                    beneUsu.VALBENEF = Convert.ToDecimal(reader["VALTIT"]);
                    beneUsu.NOMBENEF = Convert.ToString(reader["NOMBENEF"]);
                    beneUsu.COMPULSORIO = Convert.ToString(reader["COMPULSORIO"]);
                    if (reader["DTASSOC"] != DBNull.Value) beneUsu.DTASSOC = Convert.ToDateTime(reader["DTASSOC"]);
                    if (reader["DATBENEF"] != DBNull.Value) beneUsu.DATBENEF = Convert.ToDateTime(reader["DATBENEF"]);
                    beneUsu.TRENOVA = Convert.ToString(reader["TRENOVA"]);
                    beneUsu.VALDEP = Convert.ToDecimal(reader["VALDEP"]);
                    beneUsu.ADERIDO = Convert.ToString(reader["ADERIDO"]);
                    beneUsu.JAASSOC = Convert.ToString(reader["JAASSOC"]);
                    listBene.Add(beneUsu);
                }
                return listBene;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public string AssociarBeneficio(VPRODUTOSUSU usuario, int codTaxa)
        {
            var mensagem = string.Empty;
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listTaxa = new List<TAXAUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_ASSOCIA_BENEFUSU", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
                cmd.Parameters.Add("@CODBENEF", SqlDbType.Int).Value = codTaxa;
                cmd.Parameters.Add("@ID_OPERADOR", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@ORIGEM_OPERADOR", SqlDbType.VarChar).Value = "NC";

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem += "Associação realizada com sucesso.";
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return mensagem;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public string DesassociarBeneficio(VPRODUTOSUSU usuario, int codTaxa)
        {
            var mensagem = string.Empty;
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            var listTaxa = new List<TAXAUSUARIO>();

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_DESASSOCIA_BENEFUSU", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCLI", SqlDbType.Int).Value = usuario.CODCLI;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
                cmd.Parameters.Add("@CODBENEF", SqlDbType.Int).Value = codTaxa;
                cmd.Parameters.Add("@ID_OPERADOR", SqlDbType.Int).Value = FOperador.ID_FUNC;
                cmd.Parameters.Add("@ORIGEM_OPERADOR", SqlDbType.VarChar).Value = "NC";

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        //mensagem = Convert.ToString(reader["MENSAGEM"]);
                        //if (!string.IsNullOrEmpty(mensagem)) mensagem += "<br />";
                        mensagem += "Associação realizada com sucesso.";
                    }
                    else
                    {
                        mensagem = Convert.ToString(reader["MENSAGEM"]);
                    }
                }
                return mensagem;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region CRUD Observacoes

        public bool InserirObsPos(int codCli, string cpf, DateTime data, string obs)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sbCamposCliente = new StringBuilder();
            var sbParametrosCliente = new StringBuilder();
            sbCamposCliente.Append("CODCLI, CPF, DATA, OBS ");
            sbParametrosCliente.Append("@CODCLI, @CPF, @DATA, @OBS ");
            var sql = string.Format("INSERT INTO OBSUSU ({0}) VALUES ({1})", sbCamposCliente, sbParametrosCliente);
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
                UtilSIL.GravarLog(db, dbt, "INSERT OBSUSU", FOperador, cmd);
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

        public bool AlterarObsPos(int codCli, string cpf, DateTime data, string obs)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "UPDATE OBSUSU SET OBS = @OBS WHERE CODCLI = @CODCLI AND CPF = @CPF ";
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
                UtilSIL.GravarLog(db, dbt, "UPDATE OBSUSU", FOperador, cmd);
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

        public bool ExcluirObsPos(int codCli, string cpf, DateTime data)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("DELETE OBSUSU WHERE CODCLI = @CODCLI AND CPF = @CPF AND DATA = @DATA ");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, data);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE OBSUSU", FOperador, cmd);
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

        public bool ExcluirObsPos(Database db, DbTransaction dbt, int codCli, string cpf)
        {
            const string sql = "DELETE FROM OBSUSU WHERE CODCLI = @CODCLI AND CPF = @CPF ";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.Int32, cpf);
            db.ExecuteNonQuery(cmd, dbt);
            UtilSIL.GravarLog(db, dbt, "DELETE OBSUSU (Exclusao da lista observacoes do usuario excluido)", FOperador, cmd);
            return true;
        }

        public bool InserirObsPre(int codCli, string cpf, DateTime data, string obs)
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

        public bool AlterarObsPre(int codCli, string cpf, DateTime data, string obs)
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

        public bool ExcluirObsPre(int codCli, string cpf, DateTime data)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("DELETE OBSUSUVA WHERE CODCLI = @CODCLI AND CPF = @CPF AND DATA = @DATA ");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, codCli);
            db.AddInParameter(cmd, "CPF", DbType.String, cpf);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, data);
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

        public bool ExcluirObsPre(Database db, DbTransaction dbt, int codCli, string cpf)
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

        #region Ações Cartões

        public bool OperadorTelenetSupervisor(int idPerfil)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DESCRICAO FROM PERFILACESSOVA WITH (NOLOCK) WHERE ID = " + idPerfil;
            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);
            bool retorno = true;

            while (idr.Read())
            {
                var descr = idr["DESCRICAO"].ToString().Trim();
                if (descr != "TELENET" && descr != "SUPERVISOR")
                    retorno = false;
            }
            return retorno;
        }

        public bool Gerar2ViaCartao(VPRODUTOSUSU usuario, bool mantemAntigo, bool cobraSegVia, int CodJustSegViaCard, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_GERA_SEGVIA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usuario.CODCRT;
                cmd.Parameters.Add("@MANTEM_ANTIGO", SqlDbType.Char).Value = mantemAntigo ? "S" : "N";
                cmd.Parameters.Add("@COBRA2AV", SqlDbType.VarChar).Value = cobraSegVia ? "S" : "N";
                cmd.Parameters.Add("@VAL2AV", SqlDbType.Decimal).Value = usuario.VALORSEGVIA;
                cmd.Parameters.Add("@ID_JUSTIFICATIVA", SqlDbType.Int).Value = CodJustSegViaCard;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;
                
                reader = cmd.ExecuteReader();                
                while (reader.Read())
                {
                    if (Convert.ToString(reader[0]) == "OK")
                    {
                        mensagem = "Segunda via gerada com sucesso!";
                        return true; 
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
        public bool ReinclusaoCartaoCancelado(VPRODUTOSUSU usuario, string CobraTaxa,  out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_REINCLUI_CARTAO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usuario.CODCRT;
                cmd.Parameters.Add("@COBRAR", SqlDbType.VarChar).Value= CobraTaxa;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.ToString(reader[0]) == "OK")
                    {
                        mensagem = "Cartão reincluido com sucesso!";
                        return true;
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
        public bool ReativacaoCartaoSuspenso(VPRODUTOSUSU usuario, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_REATIVA_CARTAO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usuario.CODCRT;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (Convert.ToString(reader[0]) == "OK")
                    {
                        mensagem = "Cartão reativado com sucesso!";
                        return true;
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }
        
        public int DiasCartoesAntigoFuncionando()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'DIASVALCART'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public bool Cancelar2ViaCartao(VPRODUTOSUSU usuario, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_REVERTE_2AVIA", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usuario.CODCRT;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        mensagem = "Segunda via revertida com sucesso!";
                        return true;
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public string ValidadeSenha(string codcrt, out string dtSenha)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DTSENHA, DTEXPSENHA FROM USUARIOVA WITH (NOLOCK) WHERE CODCRT = '" + codcrt + "'";
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

        public bool InsereCartaoBloqueado(int codcli, int Sistema)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "";
            if (Sistema == 0)
                sql = "select top 1 CRTINCBLQ from CLIENTE_POS WITH (NOLOCK) where CODCLI = " + codcli.ToString();
            else
                sql = "select top 1 CRTINCBLQ from CLIENTE_PRE WITH (NOLOCK) where CODCLI = " + codcli.ToString();


            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);
            string Valor = "";
            while (idr.Read())
            {
                Valor = idr["CRTINCBLQ"].ToString();
            }

            return (Valor == "N") ? false : true;
        }

        public int DiasParaRenovarSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WITH (NOLOCK) WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public bool ExibeCvv()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'BLOQTRANSDIG'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool TemCashback()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAM WITH (NOLOCK) WHERE ID0 = 'HABCASHBACK'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool BloqUsuModulo(int sistema)
        {
            Database db = new SqlDatabase(BDTELENET);
            var tabela = sistema == 0 ? "PARAM" : "PARAMVA";
            var sql = "SELECT VAL FROM " + tabela + " WITH (NOLOCK) WHERE ID0 = 'BLOQCARTUSU'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        public bool RenovarAcesso(VPRODUTOSUSU usuario, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_RENOVA_ACESSO_USUARIO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CPF", SqlDbType.VarChar).Value = usuario.CPF;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var codRet = Convert.ToInt16(reader["CODRET"]);
                    if (codRet == 0)
                    {
                        mensagem = "Renovação de acesso realizada com sucesso.";
                        return true;
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool ResetSenhaCartao(VPRODUTOSUSU usuario, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_RESETA_SENHA_CARTAO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CODCRT", SqlDbType.VarChar).Value = usuario.CODCRT;
                cmd.Parameters.Add("@ID_FUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        mensagem = "A senha do cartão foi resetada com sucesso.";
                        return true; 
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        public bool CancelarCartoes(VPRODUTOSUSU usuario, out string mensagem)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            var codRet = string.Empty;
            mensagem = string.Empty;

            try
            {
                conn = new SqlConnection(BDTELENET);
                conn.Open();
                SqlCommand cmd = new SqlCommand("PROC_CANC_CARTAO", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Clear();

                cmd.Parameters.Add("@SISTEMA", SqlDbType.Int).Value = usuario.SISTEMA;
                cmd.Parameters.Add("@CARTAO", SqlDbType.VarChar).Value = usuario.CODCRT;
                cmd.Parameters.Add("@IDFUNC", SqlDbType.Int).Value = FOperador.ID_FUNC;

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codRet = Convert.ToString(reader["CODRET"]);
                    if (codRet == "0")
                    {
                        mensagem = "Cartão cancelado com sucesso.";
                        return true;
                    }
                    mensagem = Convert.ToString(reader["MENSAGEM"]);
                }
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }
        }

        #endregion

        #region Get CEP

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

        #endregion

    }
}
