using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Text;
using TELENET.SIL.PO;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace TELENET.SIL.DA
{
    internal class daCredenciadoVA
    {
        private readonly string BDTELENET = string.Empty;
        private readonly string BDCONCENTRADOR = string.Empty;
        private readonly string BDAUTORIZADOR = string.Empty;
        private readonly OPERADORA FOperador;

        public daCredenciadoVA(OPERADORA Operador)
        {
            if (Operador == null) return;
            FOperador = Operador;

            // Monta String Conexao
            BDCONCENTRADOR = string.Format(ConstantesSIL.BDCONCENTRADOR, Operador.SERVIDORCON, Operador.BANCOCON, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            BDTELENET = string.Format(ConstantesSIL.BDTELENET, Operador.SERVIDORNC, Operador.BANCONC, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);            
            BDAUTORIZADOR = string.Format(ConstantesSIL.BDAUTORIZADOR, Operador.SERVIDORAUT, Operador.BANCOAUT, ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
        }

        #region GET Credenciados

        public CREDENCIADO GetCredenciadoCodNome(int codCre)
        {
            var credenciado = new CREDENCIADO();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT CODCRE, RAZSOC ");
            sql.AppendLine("FROM CREDENCIADO ");
            sql.AppendLine("WHERE CODCRE = @CODCRE ");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                credenciado.CODCRE = codCre;
                credenciado.RAZSOC = idr["RAZSOC"].ToString();
            }
            idr.Close();
            return credenciado;
        }

        public CREDENCIADO GetCredenciado(int codCre)
        {
            var credenciado = new CREDENCIADO();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT * ");
            sql.AppendLine("FROM CREDENCIADO ");
            sql.AppendLine("WHERE CODCRE = @CODCRE ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                credenciado.CODCRE = codCre;
                credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                credenciado.CODLOC = Convert.ToInt32(idr["CODLOC"]);
                credenciado.ENDCRE = idr["ENDCRE"].ToString();
                credenciado.OBS = idr["OBS"].ToString();
                credenciado.CEP = idr["CEP"].ToString();
                credenciado.INSEST = idr["INSEST"].ToString();
                credenciado.CGC = idr["CGC"].ToString();
                credenciado.CODFILNUT = Convert.ToInt32(idr["CODFILNUT"]);
                credenciado.SENCRE = idr["SENCRE"].ToString();
                credenciado.PRAPAG = Convert.ToInt16(idr["PRAPAG"]);
                credenciado.ENDCPL = idr["ENDCPL"].ToString();
                credenciado.TIPEST = Convert.ToInt16(idr["TIPEST"]);
                credenciado.CODMAT = new CREDENCIADO();
                credenciado.CODMAT = GetCredenciadoCodNome(Convert.ToInt32(idr["CODMAT"]));
                credenciado.QTEFIL = Convert.ToInt16(idr["QTEFIL"]);
                credenciado.QTEMAQ = Convert.ToInt16(idr["QTEMAQ"]);
                credenciado.LOCPAG = Convert.ToInt16(idr["LOCPAG"]);
                credenciado.CODCEN = new CREDENCIADO();
                credenciado.CODCEN = GetCredenciadoCodNome(Convert.ToInt32(idr["CODCEN"]));
                if (idr["DATGERCRT"] != DBNull.Value)
                    credenciado.DATGERCRT = Convert.ToDateTime(idr["DATGERCRT"]);
                credenciado.GERCRT = Convert.ToChar(idr["GERCRT"]);
                credenciado.ENDCOR = idr["ENDCOR"].ToString();
                credenciado.ENDCPLCOR = idr["ENDCPLCOR"].ToString();
                credenciado.CODBAICOR = Convert.ToInt32(idr["CODBAICOR"]);
                credenciado.CODLOCCOR = Convert.ToInt32(idr["CODLOCCOR"]);
                credenciado.SIGUF0COR = idr["SIGUF0COR"].ToString();
                credenciado.CEPCOR = idr["CEPCOR"].ToString();
                credenciado.FORPAG = Convert.ToInt16(idr["FORPAG"]);
                credenciado.CODCAN = Convert.ToInt32(idr["CODCAN"]);
                credenciado.CODREG = Convert.ToInt32(idr["CODREG"]);
                credenciado.CODREO = Convert.ToInt16(idr["CODREO"]);
                credenciado.CODEPS = Convert.ToInt16(idr["CODEPS"]);
                credenciado.TRANSHAB = idr["TRANSHAB"].ToString();
                if (idr["FLAG_PJ"] != DBNull.Value)
                    credenciado.FLAG_PJ = Convert.ToChar(idr["FLAG_PJ"]);
                credenciado.MASCONTA = idr["MASCONTA"].ToString();
                credenciado.FLAG_VA = Convert.ToChar(idr["FLAG_VA"]);
                if (idr["INTVENDINI"] != DBNull.Value)
                    credenciado.INTVENDINI = Convert.ToInt16(idr["INTVENDINI"]);
                if (idr["INTVENDFIN"] != DBNull.Value)
                    credenciado.INTVENDFIN = Convert.ToInt16(idr["INTVENDFIN"]);
                if (idr["CTRLFUNC"] != DBNull.Value)
                    credenciado.CTRLFUNC = Convert.ToDateTime(idr["CTRLFUNC"]);
                credenciado.PROPRIETARIO = idr["PROPRIETARIO"].ToString();
                credenciado.IDENTIDADE = idr["IDENTIDADE"].ToString();
                if (idr["CATEGORIA"] != DBNull.Value)
                    credenciado.CATEGORIA = Convert.ToInt16(idr["CATEGORIA"]);
                credenciado.DIAFEC_VA = Convert.ToInt32(idr["DIAFEC_VA"]);
                credenciado.PRAENT = Convert.ToInt16(idr["PRAENT"]);
                credenciado.PRAREE = Convert.ToInt16(idr["PRAREE"]);
                credenciado.TAXADM = Convert.ToDecimal(idr["TAXADM"]);
                if (idr["DATTAXADM"] != DBNull.Value)
                    credenciado.DATTAXADM = Convert.ToDateTime(idr["DATTAXADM"]);
                credenciado.LIMREENT = Convert.ToDecimal(idr["LIMREENT"]);
                credenciado.RESP = idr["RESP"].ToString();
                credenciado.CTABCO_VA = idr["CTABCO_VA"].ToString();
                if (idr["NUMFEC_VA"] != DBNull.Value)
                    credenciado.NUMFEC_VA = Convert.ToInt32(idr["NUMFEC_VA"]);
                if (idr["DATULTFEC_VA"] != DBNull.Value)
                    credenciado.DATULTFEC_VA = Convert.ToDateTime(idr["DATULTFEC_VA"]);
                credenciado.TIPFEC_VA = Convert.ToInt32(idr["TIPFEC_VA"]);
                if (idr["DATCTT_VA"] != DBNull.Value)
                    credenciado.DATCTT_VA = Convert.ToDateTime(idr["DATCTT_VA"]);
                if (idr["LAYADIVA"] != DBNull.Value)
                    credenciado.LAYADIVA = Convert.ToInt32(idr["LAYADIVA"]);
                if (idr["LAYADIPJ"] != DBNull.Value)
                    credenciado.LAYADIPJ = Convert.ToInt32(idr["LAYADIPJ"]);
                if (idr["NUMPACVA"] != DBNull.Value)
                    credenciado.NUMPACVA = Convert.ToInt16(idr["NUMPACVA"]);
                if (idr["NUMULTPACVA"] != DBNull.Value)
                    credenciado.NUMULTPACVA = Convert.ToInt16(idr["NUMULTPACVA"]);
                if (idr["VALANUVA"] != DBNull.Value)
                    credenciado.VALANUVA = Convert.ToDecimal(idr["VALANUVA"]);
                if (idr["DTANUVA"] != DBNull.Value)
                    credenciado.DTANUVA = Convert.ToDateTime(idr["DTANUVA"]);
                if (idr["DTRENANU"] != DBNull.Value)
                    credenciado.DTRENANU = Convert.ToDateTime(idr["DTRENANU"]);
                if (idr["AUTARQSF"] != DBNull.Value) 
                    credenciado.AUTARQSF = Convert.ToChar(idr["AUTARQSF"]);
                credenciado.CODBAI = Convert.ToInt32(idr["CODBAI"]);
                credenciado.CODATI = Convert.ToInt32(idr["CODATI"]);
                credenciado.CODATIAUX = credenciado.CODATI;
                credenciado.CODSEG = Convert.ToInt32(idr["CODSEG"]);
                credenciado.SIGUF0 = idr["SIGUF0"].ToString();
                credenciado.STA = idr["STA"].ToString();
                credenciado.RAZSOC = idr["RAZSOC"].ToString();
                credenciado.NOMFAN = idr["NOMFAN"].ToString();
                credenciado.TEL = idr["TEL"].ToString();
                credenciado.COA = idr["COA"].ToString();
                if (idr["DATINC"] != DBNull.Value)
                credenciado.DATINC = Convert.ToDateTime(idr["DATINC"]);
                if (idr["DATCTT"] != DBNull.Value)
                credenciado.DATCTT = Convert.ToDateTime(idr["DATCTT"]);
                if (idr["DATSTA"] != DBNull.Value)
                credenciado.DATSTA = Convert.ToDateTime(idr["DATSTA"]);
                credenciado.CTABCO = idr["CTABCO"].ToString();
                credenciado.TAXSER = Convert.ToDecimal(idr["TAXSER"]);
                if (idr["DATTAXSER"] != DBNull.Value)
                credenciado.DATTAXSER = Convert.ToDateTime(idr["DATTAXSER"]);
                credenciado.MSGATVCRT = Convert.ToChar(idr["MSGATVCRT"]);
                credenciado.QTDPOS = Convert.ToInt16(idr["QTDPOS"]);
                credenciado.EMA = idr["EMA"].ToString();
                credenciado.FAX = idr["FAX"].ToString();
                credenciado.TIPFEC = Convert.ToInt32(idr["TIPFEC"]);
                credenciado.DIAFEC = Convert.ToInt32(idr["DIAFEC"]);
                credenciado.NUMFEC = Convert.ToInt32(idr["NUMFEC"]);
                if (idr["DATULTFEC"] != DBNull.Value)
                credenciado.DATULTFEC = Convert.ToDateTime(idr["DATULTFEC"]);
                if (idr["DATPRCULTFEC"] != DBNull.Value)
                credenciado.DATPRCULTFEC = Convert.ToDateTime(idr["DATPRCULTFEC"]);
                credenciado.CODPRI = new CREDENCIADO();
                credenciado.CODPRI = GetCredenciadoCodNome(Convert.ToInt32(idr["CODPRI"]));
            }
            idr.Close();
            return credenciado;
        }

        public List<CREDENCIADO> ColecaoCredenciados()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string query = "SELECT CODCRE, RAZSOC FROM CREDENCIADO WHERE FLAG_VA = 'S' ORDER BY CODCRE";
            var cmd = db.GetSqlStringCommand(query);
            var idr = db.ExecuteReader(cmd);
            var colecaoCred = new List<CREDENCIADO>();
            while (idr.Read())
            {
                var Credenciado = new CREDENCIADO();
                Credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                Credenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                colecaoCred.Add(Credenciado);
            }
            idr.Close();
            return colecaoCred;
        }

        public List<CREDENCIADO> GetCredenciadoCNPJPrincipal(int codCre)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string query = "SELECT CODCRE, RAZSOC FROM CREDENCIADO WHERE FLAG_VA = 'S' AND CODCRE = @CODCRE AND CODCRE = CODPRI ORDER BY CODCRE";
            var cmd = db.GetSqlStringCommand(query);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);
            var colecaoCred = new List<CREDENCIADO>();            
            while (idr.Read())
            {
                var credenciado = new CREDENCIADO();
                credenciado.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                credenciado.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                colecaoCred.Add(credenciado);
            }
            idr.Close();
            return colecaoCred;
        }

        public List<CREDENCIADO_VA> ColecaoCredenciados(string Filtro)
        {
            var ColecaoCredenciadoVA = new List<CREDENCIADO_VA>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine("SELECT");
            sql.AppendLine("  C.CODCRE CODIGO,");
            sql.AppendLine("  C.RAZSOC RAZAO,");
            sql.AppendLine("  C.NOMFAN NOME,");
            sql.AppendLine("  C.CGC CNPJ,");
            sql.AppendLine("  L.NOMLOC LOCALIDADE,");
            sql.AppendLine("  B.NOMBAI BAIRRO,");
            sql.AppendLine("  S.DESTA STATUS,");
            sql.AppendLine("  S.STA CODSTA");
            sql.AppendLine("FROM CREDENCIADO C");
            sql.AppendLine("LEFT JOIN BAIRRO B");
            sql.AppendLine("ON B.CODBAI = C.CODBAI");
            sql.AppendLine("LEFT JOIN LOCALIDADE L");
            sql.AppendLine("ON L.CODLOC = C.CODLOC");
            sql.AppendLine("LEFT JOIN STATUS S");
            sql.AppendLine("ON S.STA = C.STA");
            sql.AppendLine(string.Format("WHERE FLAG_VA = 'S' AND {0} ", Filtro));
            sql.AppendLine("ORDER BY C.RAZSOC");

            IDataReader idr = null;
            try
            {
                var cmd = db.GetSqlStringCommand(sql.ToString());
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var Credenciado = new CREDENCIADO_VA();
                    Credenciado.CODCRE = Convert.ToString(idr["CODIGO"]);
                    Credenciado.RAZSOC = Convert.ToString(idr["RAZAO"]);
                    Credenciado.NOMFAN = Convert.ToString(idr["NOME"]);
                    Credenciado.CNPJ = Convert.ToString(idr["CNPJ"]);
                    Credenciado.LOCALIDADE = Convert.ToString(idr["LOCALIDADE"]);
                    Credenciado.BAIRRO = Convert.ToString(idr["BAIRRO"]);
                    Credenciado.CODSTA = Convert.ToString(idr["CODSTA"]);
                    Credenciado.STATUS = Convert.ToString(idr["STATUS"]);                 
                    ColecaoCredenciadoVA.Add(Credenciado);
                }
                idr.Close();
            }
            catch
            {
                if (idr != null)
                    idr.Close();
            }
            return ColecaoCredenciadoVA;
        }

        #endregion

        #region Get Codigo Afiliacao

        public string CodigoAfiliacao(string cnpj, int codCre)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODAFIL + REDE AS CODAFIL FROM TABRAFIL2 WHERE ");
            sql.AppendLine("CODOPE = " + FOperador.CODOPE + " AND CNPJ = '" + cnpj + "'");
            sql.AppendLine("AND SUBSTRING(COD_DESTINO, 1, 6) = " + Convert.ToString(codCre).PadLeft(6, '0'));

            var cmd = db.GetSqlStringCommand(sql.ToString());
            var codAfil = Convert.ToString(db.ExecuteScalar(cmd)) != string.Empty
                              ? Convert.ToString(db.ExecuteScalar(cmd))
                              : "";
            return codAfil;
        }

        public string ObtemRede(string codAfil)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT REDE FROM TABRAFIL2 WHERE ");
            sql.AppendLine("CODAFIL = '" + codAfil + "'");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var rede = Convert.ToString(db.ExecuteScalar(cmd)) != string.Empty
                           ? Convert.ToString(db.ExecuteScalar(cmd))
                           : "01";
            return rede;
        }

        public AFILIACAO RetornaDadosCred(string cnpj)
        {
            var dadosCred = new AFILIACAO();
            const string sql = "CONSULTA_DADOSCRED";
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                //dadosCred.CODAFIL = Convert.ToString(idr["CODAFIL"]);
                dadosCred.SIGUF0 = Convert.ToString(idr["SIGUF0"]);
                dadosCred.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                dadosCred.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                dadosCred.TEL = Convert.ToString(idr["TEL"]);
                dadosCred.FAX = Convert.ToString(idr["FAX"]);
                dadosCred.EMA = Convert.ToString(idr["EMA"]);
                dadosCred.ENDCRE = Convert.ToString(idr["ENDCRE"]);
                dadosCred.CEP = Convert.ToString(idr["CEP"]);
                dadosCred.INSEST = Convert.ToString(idr["INSEST"]);
                dadosCred.CGC = Convert.ToString(idr["CGC"]);
            }
            idr.Close();
            return dadosCred;
        }

        #endregion

        #region Get Proximo Codigo

        public int ProximoCodigoLivre()
        {
            var proxCod = 0;
            const string sql = "PROC_PROX_CODCRE";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);            
            proxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return proxCod;
        }

        public int ProximoCodigoLivreGrupoCrendenciados()
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT ISNULL(MIN(CODGRUPO)+1, 1) AS ProxCod");
            sql.AppendLine("FROM GRUPO");
            sql.AppendLine("WHERE (CODGRUPO+1) NOT IN (SELECT CODGRUPO FROM GRUPO)");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var ProxCod = Convert.ToInt32(db.ExecuteScalar(cmd));
            return ProxCod;
        }

        #endregion

        #region Get Observacoes

        public List<CREDENCIADO_OBS> Observacoes(Int32 Credenciado)
        {
            var ColecaoObservacoes = new List<CREDENCIADO_OBS>();
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE, DATA, OBS, ID");
            sql.AppendLine("FROM OBSCRED");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, Credenciado);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var Observacao = new CREDENCIADO_OBS();
                Observacao.CODCRE = Credenciado;
                Observacao.ID = Convert.ToInt32(idr["ID"]);
                Observacao.DATA = Convert.ToDateTime(idr["DATA"]);
                Observacao.OBS = Convert.ToString(idr["OBS"]);
                ColecaoObservacoes.Add(Observacao);
            }
            idr.Close();
            return ColecaoObservacoes;
        }

        #endregion

        #region Get Redes

        public List<REDES> ConsultaRedes(int codCre)
        {
            var redes = new List<REDES>();
            const string sql = "PROC_REDES_HABILITADAS";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            IDataReader idr = null;
            try
            {
                idr = db.ExecuteReader(cmd);
                while (idr.Read())
                {
                    var rede = new REDES();
                    rede.REDE = Convert.ToString(idr["REDE"]);
                    if (idr["CODCRE"] != DBNull.Value) rede.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                    rede.SELECIONADA = Convert.ToString(idr["SELECIONADA"]);
                    rede.NOME = Convert.ToString(idr["NOME"]);
                    rede.TEMCODAFIL = Convert.ToString(idr["TEMCODAFIL"]);
                    rede.HABREDE = Convert.ToString(idr["HABREDE"]) == "S"? "Sim" : "Não";
                    rede.CODAFILREDE = Convert.ToString(idr["CODAFILREDE"]);
                    if (idr["DATATUALIZ"] != DBNull.Value) rede.DATATUALIZ = Convert.ToDateTime(idr["DATATUALIZ"]);
                    rede.STATUSREDE = Convert.ToString(idr["STATUSREDE"]);
                    if (idr["DATAREDE"] != DBNull.Value) rede.DATAREDE = Convert.ToDateTime(idr["DATAREDE"]);                    
                    redes.Add(rede);
                }
                idr.Close();
            }
            catch 
            {
                if (idr != null)
                    idr.Close(); 
            }
            return redes;
        }

        #endregion

        #region Get Taxas Credenciado

        public List<MODTAXA> ConsultaTaxaCre(int codCre)
        {
            var taxaCre = new List<MODTAXA>();
            const string sql = "PROC_LER_TAXACRE";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, codCre);
            db.AddInParameter(cmd, "@SISTEMA", DbType.String, ConstantesSIL.SistemaPRE);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var taxa = new MODTAXA();
                taxa.COD = Convert.ToInt32(idr["CODCRE"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.NOMTAXA = Convert.ToString(idr["NOMTAXA"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALOR"]);
                taxa.NUMPARC = Convert.ToInt32(idr["NUMPAC"]);
                taxa.DTINC = Convert.ToDateTime(idr["DTINC"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["DIASPINICIO"]);
                taxa.COBSEMCRED = Convert.ToString(idr["COBSEMCRED"]);
                taxa.PRIORIDADE = Convert.ToInt32(idr["PRIORIDADE"]);
                taxa.DESTA = Convert.ToString(idr["DESTA"]);
                taxa.TAXAHAB = Convert.ToString(idr["TAXAHAB"]);
                taxaCre.Add(taxa);
            }
            idr.Close();
            return taxaCre;
        }

        public MODTAXA ConsultaCodTaxaCre(int codCre, int codTaxa)
        {
            var taxa = new MODTAXA();
            var sql = new StringBuilder();
            sql.AppendLine("SELECT C.CODCRE, C.CODTAXA, T.TIPTRA, C.VALOR, C.NUMPAC, C.DTINC, ");
            sql.AppendLine("       C.DTINICIO, C.DIASPINICIO, C.COBSEMCRED, C.PRIORIDADE, C.TAXAHAB, C.VALMINCRED, C.DIACOB ");
            sql.AppendLine("FROM TAXACREVA C ");
            sql.AppendLine("	INNER JOIN TAXAVA T ON T.CODTAXA = C.CODTAXA AND T.TIPO = 2");
            sql.AppendLine("WHERE C.CODCRE = @CODCRE AND C.CODTAXA = @CODTAXA ");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "@CODCRE", DbType.Int32, codCre);
            db.AddInParameter(cmd, "@CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                taxa.COD = Convert.ToInt32(idr["CODCRE"]);
                taxa.CODTAXA = Convert.ToInt32(idr["CODTAXA"]);
                taxa.TIPTRA = Convert.ToInt32(idr["TIPTRA"]);
                taxa.VALOR = Convert.ToDecimal(idr["VALOR"]);
                taxa.NUMPARC = Convert.ToInt32(idr["NUMPAC"]);
                taxa.DTINC = Convert.ToDateTime(idr["DTINC"]);
                taxa.DTINICIO = Convert.ToDateTime(idr["DTINICIO"]);
                if (idr["DIACOB"] != DBNull.Value) taxa.DIACOB = Convert.ToInt32(idr["DIACOB"]);
                taxa.DIASPINICIO = Convert.ToInt32(idr["DIASPINICIO"]);
                taxa.COBSEMCRED = Convert.ToString(idr["COBSEMCRED"]);
                taxa.PRIORIDADE = Convert.ToInt32(idr["PRIORIDADE"]);
                taxa.TAXAHAB = Convert.ToString(idr["TAXAHAB"]);
                taxa.VALMINCRED = Convert.ToDecimal(idr["VALMINCRED"]);
            }
            idr.Close();
            return taxa;
        }

        #endregion

        #region Get Equipamentos

        public List<CTPOS> ColecaoEquipamentos(string codAfil, int codOpe)
        {
            var colecaoEquip = new List<CTPOS>();
            const string sql = "LISTA_TERMINAIS";
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODAFIL", DbType.String, codAfil);
            db.AddInParameter(cmd, "@CODOPE", DbType.Int32, codOpe);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var equipamento = new CTPOS
                                      {
                                          CODPOS = Convert.ToString(idr["CODPOS"]),
                                          TIPOEQ = Convert.ToString(idr["TIPOEQ"]),
                                          VERAPL = Convert.ToString(idr["VERAPL"]),
                                          TIPOCON = Convert.ToString(idr["TIPOCON"]),
                                          ULTINIC = idr["ULTINIC"] == DBNull.Value? DateTime.MinValue: Convert.ToDateTime(idr["ULTINIC"]),
                                          INICIALIZADO = Convert.ToString(idr["INICIALIZADO"]),
                                          OPERADORA = Convert.ToString(idr["OPERADORA"]),
                                          EQUIPID = Convert.ToString(idr["EQUIPID"])
                                      };
                colecaoEquip.Add(equipamento);
            }
            idr.Close();
            return colecaoEquip;
        }

        #endregion

        #region TaxaCre

        public bool ExibeModuloTaxaCre()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'TAXACRE'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";
        }

        #endregion

        #region Equipamentos Parametrizacao

        public string EquipamentoArqParametrizacao(int Credenciado, string CodEquipamento)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "GERAPESTAB";
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODPS", DbType.Int32, Credenciado);
            db.AddInParameter(cmd, "@CODPOS", DbType.String, CodEquipamento);
            var idr = db.ExecuteReader(cmd);
            var LinhaCartao = string.Empty;
            if (idr.Read())
            {
                LinhaCartao += Convert.ToString(idr["OPERADORA"]);
                LinhaCartao += string.Empty.PadRight(80);
                LinhaCartao += "60";
                LinhaCartao += string.Empty.PadRight(02);
                LinhaCartao += Convert.ToString(idr["RAZSOC"]);
                LinhaCartao += string.Empty.PadRight(26);
                LinhaCartao += Convert.ToString(idr["CGC"]);
                LinhaCartao += string.Empty.PadRight(14);
                LinhaCartao += Convert.ToString(idr["ENDCRE"]);
                LinhaCartao += string.Empty.PadRight(30);
                LinhaCartao += Convert.ToString(idr["CGC"]);
                LinhaCartao += string.Empty.PadRight(26);
                LinhaCartao += Convert.ToString(CodEquipamento);
            }
            idr.Close();
            return LinhaCartao;
        }

        #endregion

        #region CRUD Credenciados

        public bool Inserir(string codAfil, CREDENCIADO Credenciado)
        {
            var sbCampos = new StringBuilder();
            var sbParametros = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);

            #region Campos
            sbCampos.Append("CODCRE, RAZSOC, NOMFAN, CGC, INSEST, CODSEG, CODATI, FLAG_VA,");
            sbCampos.Append("TIPEST, CODMAT, QTEFIL, CODREG, CODCEN, PROPRIETARIO, IDENTIDADE, CODPRI,");
            sbCampos.Append("ENDCRE, ENDCPL, CODBAI, CODLOC, SIGUF0, CEP, TEL, FAX, COA, EMA, ENDCOR, ENDCPLCOR, CODBAICOR, CODLOCCOR, SIGUF0COR, CEPCOR,");
            sbCampos.Append("TIPFEC_VA, DIAFEC_VA, NUMFEC_VA, DATULTFEC_VA, VALANUVA, NUMPACVA, NUMULTPACVA, DTANUVA, PRAENT,");
            sbCampos.Append("PRAREE, LIMREENT,");
            sbCampos.Append("QTDPOS, QTEMAQ, MSGATVCRT,");
            sbCampos.Append("DATGERCRT, GERCRT,");
            sbCampos.Append("DATCTT_VA, DATINC, STA, DATSTA, CTABCO_VA, TAXADM, DATTAXADM, LOCPAG, FORPAG, CODFILNUT, SENCRE, SENWEB, ");
            sbCampos.Append("TAXSER, DATTAXSER, NUMFEC, DIAFEC, PRAPAG, CODCAN, CODREO, CODEPS, DATCTT, CTABCO, TIPFEC, AUTARQSF, TRANSHAB");
            #endregion

            #region Parâmetros
            sbParametros.Append("@CODCRE, @RAZSOC, @NOMFAN, @CGC, @INSEST, @CODSEG, @CODATI, @FLAG_VA,");
            sbParametros.Append("@TIPEST, @CODMAT, @QTEFIL, @CODREG, @CODCEN, @PROPRIETARIO, @IDENTIDADE, @CODPRI,");
            sbParametros.Append("@ENDCRE, @ENDCPL, @CODBAI, @CODLOC, @SIGUF0, @CEP, @TEL, @FAX, @COA, @EMA, @ENDCOR, @ENDCPLCOR, @CODBAICOR, @CODLOCCOR, @SIGUF0COR, @CEPCOR,");
            sbParametros.Append("@TIPFEC_VA, @DIAFEC_VA, @NUMFEC_VA, @DATULTFEC_VA, @VALANUVA, @NUMPACVA, @NUMULTPACVA, @DTANUVA, @PRAENT,");
            sbParametros.Append("@PRAREE, @LIMREENT,");
            sbParametros.Append("@QTDPOS, @QTEMAQ, @MSGATVCRT,");
            sbParametros.Append("@DATGERCRT, @GERCRT,");
            sbParametros.Append("@DATCTT_VA, @DATINC, @STA, @DATSTA, @CTABCO_VA, @TAXADM, @DATTAXADM, @LOCPAG, @FORPAG, @CODFILNUT, @SENCRE, @SENWEB, ");
            sbParametros.Append("@TAXSER, @DATTAXSER, @NUMFEC, @DIAFEC, @PRAPAG, @CODCAN, @CODREO, @CODEPS, @DATCTT, @CTABCO, @TIPFEC, @AUTARQSF, @TRANSHAB");
            #endregion

            #region NETCARD
            var sql = string.Format("INSERT INTO CREDENCIADO ({0}) VALUES ({1})", sbCampos,sbParametros);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "RAZSOC", DbType.String, UtilSIL.RemoverAcentos(Credenciado.RAZSOC));
            db.AddInParameter(cmd, "NOMFAN", DbType.String, UtilSIL.RemoverAcentos(Credenciado.NOMFAN));
            db.AddInParameter(cmd, "CGC", DbType.String, Credenciado.CGC);
            db.AddInParameter(cmd, "INSEST", DbType.String, Credenciado.INSEST);
            db.AddInParameter(cmd, "CODSEG", DbType.Int32, Credenciado.CODSEG);
            db.AddInParameter(cmd, "CODATI", DbType.Int32, Credenciado.CODATI);
            db.AddInParameter(cmd, "TIPEST", DbType.Int16, Credenciado.TIPEST);
            db.AddInParameter(cmd, "CODMAT", DbType.Int32, Credenciado.CODMAT.CODCRE);
            db.AddInParameter(cmd, "QTEFIL", DbType.Int16, Credenciado.QTEFIL);
            db.AddInParameter(cmd, "CODREG", DbType.Int32, Credenciado.CODREG);
            db.AddInParameter(cmd, "CODCEN", DbType.Int32, Credenciado.CODCEN.CODCRE);
            db.AddInParameter(cmd, "PROPRIETARIO", DbType.String, Credenciado.PROPRIETARIO);
            db.AddInParameter(cmd, "IDENTIDADE", DbType.String, Credenciado.IDENTIDADE);
            db.AddInParameter(cmd, "CODPRI", DbType.String, Credenciado.CODPRI.CODCRE);
            db.AddInParameter(cmd, "FLAG_VA", DbType.String, "S");
            db.AddInParameter(cmd, "ENDCRE", DbType.String, Credenciado.ENDCRE);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, Credenciado.ENDCPL);
            db.AddInParameter(cmd, "CODBAI", DbType.Int32, Credenciado.CODBAI);
            db.AddInParameter(cmd, "CODLOC", DbType.Int32, Credenciado.CODLOC);
            if (Credenciado.SIGUF0 != null)
                db.AddInParameter(cmd, "SIGUF0", DbType.String, Credenciado.SIGUF0);
            db.AddInParameter(cmd, "CEP", DbType.String, Credenciado.CEP);
            db.AddInParameter(cmd, "TEL", DbType.String, Credenciado.TEL);
            db.AddInParameter(cmd, "FAX", DbType.String, Credenciado.FAX);
            db.AddInParameter(cmd, "COA", DbType.String, Credenciado.COA);
            db.AddInParameter(cmd, "EMA", DbType.String, Credenciado.EMA);
            db.AddInParameter(cmd, "ENDCOR", DbType.String, Credenciado.ENDCOR);
            db.AddInParameter(cmd, "ENDCPLCOR", DbType.String, Credenciado.ENDCPLCOR);
            db.AddInParameter(cmd, "CODBAICOR", DbType.Int32, Credenciado.CODBAICOR);
            db.AddInParameter(cmd, "CODLOCCOR", DbType.Int32, Credenciado.CODLOCCOR);
            if (Credenciado.SIGUF0COR != null)
                db.AddInParameter(cmd, "SIGUF0COR", DbType.String, Credenciado.SIGUF0COR);
            db.AddInParameter(cmd, "CEPCOR", DbType.String, Credenciado.CEPCOR);
            db.AddInParameter(cmd, "TIPFEC_VA", DbType.Int32, Credenciado.TIPFEC_VA);
            db.AddInParameter(cmd, "DIAFEC_VA", DbType.Int16, Credenciado.DIAFEC_VA);
            db.AddInParameter(cmd, "NUMFEC_VA", DbType.Int32, Credenciado.NUMFEC_VA);
            db.AddInParameter(cmd, "DATULTFEC_VA", DbType.DateTime, Credenciado.DATULTFEC_VA);
            db.AddInParameter(cmd, "VALANUVA", DbType.Decimal, Credenciado.VALANUVA);
            db.AddInParameter(cmd, "NUMPACVA", DbType.Int16, Credenciado.NUMPACVA);
            if (Credenciado.NUMULTPACVA == 0)
                db.AddInParameter(cmd, "NUMULTPACVA", DbType.Int16, null);
            else
                db.AddInParameter(cmd, "NUMULTPACVA", DbType.Int16, Credenciado.NUMULTPACVA);
            if (Credenciado.DTANUVA == DateTime.MinValue)
                db.AddInParameter(cmd, "DTANUVA", DbType.DateTime, null);
            else
                db.AddInParameter(cmd, "DTANUVA", DbType.DateTime, Credenciado.DTANUVA);
            db.AddInParameter(cmd, "PRAENT", DbType.Int16, Credenciado.PRAENT);
            db.AddInParameter(cmd, "PRAREE", DbType.Int16, Credenciado.PRAREE);
            db.AddInParameter(cmd, "LIMREENT", DbType.Decimal, Credenciado.LIMREENT);
            db.AddInParameter(cmd, "QTDPOS", DbType.Int16, Credenciado.QTDPOS);
            db.AddInParameter(cmd, "QTEMAQ", DbType.Int16, Credenciado.QTEMAQ);
            db.AddInParameter(cmd, "MSGATVCRT", DbType.String, Credenciado.MSGATVCRT);
            db.AddInParameter(cmd, "DATGERCRT", DbType.DateTime, Credenciado.DATGERCRT);
            db.AddInParameter(cmd, "GERCRT", DbType.String, Credenciado.GERCRT);
            db.AddInParameter(cmd, "DATCTT_VA", DbType.DateTime, Credenciado.DATCTT_VA);
            db.AddInParameter(cmd, "DATINC", DbType.DateTime, Credenciado.DATINC);
            if (Credenciado.STA != null)
                db.AddInParameter(cmd, "STA", DbType.String, Credenciado.STA);
            db.AddInParameter(cmd, "DATSTA", DbType.DateTime, Credenciado.DATSTA);
            db.AddInParameter(cmd, "CTABCO_VA", DbType.String, Credenciado.CTABCO_VA);
            db.AddInParameter(cmd, "TAXADM", DbType.Decimal, Credenciado.TAXADM);
            db.AddInParameter(cmd, "DATTAXADM", DbType.DateTime, Credenciado.DATTAXADM);
            db.AddInParameter(cmd, "LOCPAG", DbType.Int16, Credenciado.LOCPAG);
            db.AddInParameter(cmd, "FORPAG", DbType.Int16, Credenciado.FORPAG);
            if (Credenciado.CODFILNUT != -1)
                db.AddInParameter(cmd, "CODFILNUT", DbType.Int32, Credenciado.CODFILNUT);
            else
                db.AddInParameter(cmd, "CODFILNUT", DbType.Int32, DBNull.Value);
            db.AddInParameter(cmd, "SENCRE", DbType.String, Credenciado.SENCRE);
            db.AddInParameter(cmd, "SENWEB", DbType.String, Credenciado.SENWEB);
            db.AddInParameter(cmd, "TAXSER", DbType.Int16, 0);
            db.AddInParameter(cmd, "DATTAXSER", DbType.Date, DateTime.Today);
            db.AddInParameter(cmd, "NUMFEC", DbType.Int16, 0);
            db.AddInParameter(cmd, "DIAFEC", DbType.Int16, 1);
            db.AddInParameter(cmd, "PRAPAG", DbType.Int16, 0);
            db.AddInParameter(cmd, "CODCAN", DbType.Int16, 1);
            db.AddInParameter(cmd, "CODREO", DbType.Int32, Credenciado.CODREO);
            db.AddInParameter(cmd, "CODEPS", DbType.Int32, Credenciado.CODEPS);
            db.AddInParameter(cmd, "DATCTT", DbType.DateTime, DateTime.Now);
            db.AddInParameter(cmd, "CTABCO", DbType.String, string.Empty);
            db.AddInParameter(cmd, "TIPFEC", DbType.String, string.Empty);
            db.AddInParameter(cmd, "AUTARQSF", DbType.String, Credenciado.AUTARQSF);
            db.AddInParameter(cmd, "TRANSHAB", DbType.String, Credenciado.TRANSHAB);
            #endregion

            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "INSERT CREDENCIADO", FOperador, cmd);
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

            #region AUTORIZADOR

            InserirAutorizador(Credenciado);

            #endregion

            #region Ramo Atividade

            if (Credenciado.CODATIAUX != Credenciado.CODATI)
                AtualizaRamoAtividade(Credenciado);

            #endregion

            #region TABRAFIL

            var codDestino = Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0') + "0000000";
            codDestino += ObtemRede(codAfil);
            InserirTabrafil(FOperador.CODOPE, Credenciado.CGC, codDestino);

            #endregion

            return true;
        }

        public bool Alterar(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sbCampos = new StringBuilder();

            #region NETCARD

            #region Campos
            sbCampos.Append("RAZSOC = @RAZSOC,");
            sbCampos.Append("NOMFAN = @NOMFAN,");
            sbCampos.Append("CGC = @CGC,");
            sbCampos.Append("INSEST = @INSEST,");
            sbCampos.Append("CODSEG = @CODSEG,");
            sbCampos.Append("CODATI = @CODATI,");
            sbCampos.Append("TIPEST = @TIPEST,");
            sbCampos.Append("CODMAT = @CODMAT,");
            sbCampos.Append("QTEFIL = @QTEFIL,");
            sbCampos.Append("CODREG = @CODREG,");
            sbCampos.Append("CODREO = @CODREO,");
            sbCampos.Append("CODEPS = @CODEPS,");
            sbCampos.Append("CODCEN = @CODCEN,");
            sbCampos.Append("PROPRIETARIO = @PROPRIETARIO,");
            sbCampos.Append("IDENTIDADE = @IDENTIDADE,");
            sbCampos.Append("CODPRI = @CODPRI,");
            sbCampos.Append("ENDCRE = @ENDCRE,");
            sbCampos.Append("ENDCPL = @ENDCPL,");
            sbCampos.Append("CODBAI = @CODBAI,");
            sbCampos.Append("CODLOC = @CODLOC,");
            sbCampos.Append("SIGUF0 = @SIGUF0,");
            sbCampos.Append("CEP = @CEP,");
            sbCampos.Append("TEL = @TEL,");
            sbCampos.Append("FAX = @FAX,");
            sbCampos.Append("COA = @COA,");
            sbCampos.Append("EMA = @EMA,");
            sbCampos.Append("ENDCOR = @ENDCOR,");
            sbCampos.Append("ENDCPLCOR = @ENDCPLCOR,");
            sbCampos.Append("CODBAICOR = @CODBAICOR,");
            sbCampos.Append("CODLOCCOR = @CODLOCCOR,");
            sbCampos.Append("SIGUF0COR = @SIGUF0COR,");
            sbCampos.Append("CEPCOR = @CEPCOR,");
            sbCampos.Append("TIPFEC_VA = @TIPFEC_VA,");
            sbCampos.Append("DIAFEC_VA = @DIAFEC_VA,");
            sbCampos.Append("VALANUVA = @VALANUVA,");
            sbCampos.Append("NUMPACVA = @NUMPACVA,");
            sbCampos.Append("PRAENT = @PRAENT,");
            sbCampos.Append("PRAREE = @PRAREE,");
            sbCampos.Append("LIMREENT = @LIMREENT,");
            sbCampos.Append("TRANSHAB = @TRANSHAB,");
            sbCampos.Append("QTDPOS = @QTDPOS,");
            sbCampos.Append("QTEMAQ = @QTEMAQ,");
            sbCampos.Append("MSGATVCRT = @MSGATVCRT,");
            sbCampos.Append("GERCRT = @GERCRT,");
            sbCampos.Append("DATCTT_VA = @DATCTT_VA,");
            sbCampos.Append("DATINC = @DATINC,");
            sbCampos.Append("STA = @STA,");
            sbCampos.Append("DATSTA = @DATSTA,");
            sbCampos.Append("CTABCO_VA = @CTABCO_VA,");
            sbCampos.Append("TAXADM = @TAXADM,");
            sbCampos.Append("LOCPAG = @LOCPAG,");
            sbCampos.Append("FORPAG = @FORPAG,");
            sbCampos.Append("CODFILNUT = @CODFILNUT,");
            sbCampos.Append("SENCRE = @SENCRE");
            #endregion

            #region Add Paramentros
            var sql = string.Format("UPDATE Credenciado SET {0} WHERE CODCRE = @CODCRE", sbCampos);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "RAZSOC", DbType.String, UtilSIL.RemoverAcentos(Credenciado.RAZSOC));
            db.AddInParameter(cmd, "NOMFAN", DbType.String, UtilSIL.RemoverAcentos(Credenciado.NOMFAN));
            db.AddInParameter(cmd, "CGC", DbType.String, Credenciado.CGC);
            db.AddInParameter(cmd, "INSEST", DbType.String, Credenciado.INSEST);
            db.AddInParameter(cmd, "CODSEG", DbType.Int32, Credenciado.CODSEG);
            db.AddInParameter(cmd, "CODATI", DbType.Int32, Credenciado.CODATI);
            db.AddInParameter(cmd, "TIPEST", DbType.Int16, Credenciado.TIPEST);
            db.AddInParameter(cmd, "CODMAT", DbType.Int32, Credenciado.CODMAT == null ? Credenciado.CODCRE : Credenciado.CODMAT.CODCRE);
            db.AddInParameter(cmd, "QTEFIL", DbType.Int16, Credenciado.QTEFIL);
            db.AddInParameter(cmd, "CODREG", DbType.Int32, Credenciado.CODREG);
            db.AddInParameter(cmd, "CODREO", DbType.Int32, Credenciado.CODREO);
            db.AddInParameter(cmd, "CODEPS", DbType.Int32, Credenciado.CODEPS);
            db.AddInParameter(cmd, "CODCEN", DbType.Int32, Credenciado.CODCEN == null ? Credenciado.CODCRE : Credenciado.CODCEN.CODCRE);
            db.AddInParameter(cmd, "PROPRIETARIO", DbType.String, Credenciado.PROPRIETARIO);
            db.AddInParameter(cmd, "IDENTIDADE", DbType.String, Credenciado.IDENTIDADE);
            db.AddInParameter(cmd, "CODPRI", DbType.Int32, Credenciado.CODPRI == null ? Credenciado.CODCRE : Credenciado.CODPRI.CODCRE);
            db.AddInParameter(cmd, "ENDCRE", DbType.String, Credenciado.ENDCRE);
            db.AddInParameter(cmd, "ENDCPL", DbType.String, Credenciado.ENDCPL);
            db.AddInParameter(cmd, "CODBAI", DbType.Int32, Credenciado.CODBAI);
            db.AddInParameter(cmd, "CODLOC", DbType.Int32, Credenciado.CODLOC);
            if (Credenciado.SIGUF0 != null)
                db.AddInParameter(cmd, "SIGUF0", DbType.String, Credenciado.SIGUF0);
            db.AddInParameter(cmd, "CEP", DbType.String, Credenciado.CEP);
            db.AddInParameter(cmd, "TEL", DbType.String, Credenciado.TEL);
            db.AddInParameter(cmd, "FAX", DbType.String, Credenciado.FAX);
            db.AddInParameter(cmd, "COA", DbType.String, Credenciado.COA);
            db.AddInParameter(cmd, "EMA", DbType.String, Credenciado.EMA);
            db.AddInParameter(cmd, "ENDCOR", DbType.String, Credenciado.ENDCOR);
            db.AddInParameter(cmd, "ENDCPLCOR", DbType.String, Credenciado.ENDCPLCOR);
            db.AddInParameter(cmd, "CODBAICOR", DbType.String, Credenciado.CODBAICOR);
            db.AddInParameter(cmd, "CODLOCCOR", DbType.Int32, Credenciado.CODLOCCOR);
            if (Credenciado.SIGUF0COR != null)
                db.AddInParameter(cmd, "SIGUF0COR", DbType.String, Credenciado.SIGUF0COR);
            db.AddInParameter(cmd, "CEPCOR", DbType.String, Credenciado.CEPCOR);
            db.AddInParameter(cmd, "TIPFEC_VA", DbType.Int32, Credenciado.TIPFEC_VA);
            db.AddInParameter(cmd, "DIAFEC_VA", DbType.Int32, Credenciado.DIAFEC_VA);
            db.AddInParameter(cmd, "VALANUVA", DbType.Decimal, Credenciado.VALANUVA);
            db.AddInParameter(cmd, "NUMPACVA", DbType.Int16, Credenciado.NUMPACVA);
            db.AddInParameter(cmd, "PRAENT", DbType.Int16, Credenciado.PRAENT);
            db.AddInParameter(cmd, "PRAREE", DbType.Int16, Credenciado.PRAREE);
            db.AddInParameter(cmd, "LIMREENT", DbType.Decimal, Credenciado.LIMREENT);
            db.AddInParameter(cmd, "TRANSHAB", DbType.String, Credenciado.TRANSHAB);
            db.AddInParameter(cmd, "QTDPOS", DbType.Int16, Credenciado.QTDPOS);
            db.AddInParameter(cmd, "QTEMAQ", DbType.Int16, Credenciado.QTEMAQ);
            db.AddInParameter(cmd, "MSGATVCRT", DbType.String, Credenciado.MSGATVCRT);
            db.AddInParameter(cmd, "GERCRT", DbType.String, Credenciado.GERCRT);
            db.AddInParameter(cmd, "DATCTT_VA", DbType.DateTime, Credenciado.DATCTT_VA);
            db.AddInParameter(cmd, "DATINC", DbType.DateTime, Credenciado.DATINC);
            db.AddInParameter(cmd, "STA", DbType.String, Credenciado.STA);
            db.AddInParameter(cmd, "DATSTA", DbType.DateTime, Credenciado.DATSTA);
            db.AddInParameter(cmd, "CTABCO_VA", DbType.String, Credenciado.CTABCO_VA);
            db.AddInParameter(cmd, "TAXADM", DbType.Decimal, Credenciado.TAXADM);
            db.AddInParameter(cmd, "LOCPAG", DbType.Int16, Credenciado.LOCPAG);
            db.AddInParameter(cmd, "FORPAG", DbType.Int16, Credenciado.FORPAG);
            db.AddInParameter(cmd, "CODFILNUT", DbType.Int32, Credenciado.CODFILNUT);
            db.AddInParameter(cmd, "SENCRE", DbType.String, Credenciado.SENCRE);
            #endregion

            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var creAux = GetCredenciado(Credenciado.CODCRE);
                if ((creAux != null) && (creAux.STA != Credenciado.STA))
                {
                    if (Credenciado.STA == "01" && creAux.STA != "01")
                    {
                        GeraLogTrans(dbt, db, Credenciado, 999004);
                    }
                    else
                        if (Credenciado.STA == "00" && creAux.STA != "00")
                        {
                            GeraLogTrans(dbt, db, Credenciado, 999003);
                        }
                        else
                            if (Credenciado.STA == "02" && creAux.STA != "02")
                            {
                                GeraLogTrans(dbt, db, Credenciado, 999005);
                            }
                }
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE CREDENCIADO", FOperador, cmd);
                if (LinhaAfetada > 0)
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
            #endregion

            #region AUTORIZADOR
            AlterarAutorizador(Credenciado);
            #endregion

            #region Ramo Atividade

            if (Credenciado.CODATIAUX != Credenciado.CODATI)
                AtualizaRamoAtividade(Credenciado);

            #endregion

            return true;
        }

        public bool ValidarExclusao(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            sql.AppendLine("  AND (STA = @STA)");
            sql.AppendLine("  AND (CONVERT(CHAR(10), DATSTA, 102) <= CONVERT(CHAR(10), GETDATE() - 7, 102))");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "STA", DbType.String, ConstantesSIL.StatusCancelado);
            var CodCre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCre != 0);
        }

        public string CentralizadoraDependente(int IdCredenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TOP 1 RAZSOC");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CODCEN = @CODCEN");
            sql.AppendLine("  AND CODCRE <> @CODCEN");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCEN", DbType.Int32, IdCredenciado);
            return (Convert.ToString(db.ExecuteScalar(cmd)));
        }

        public string MatrizDependente(int IdCredenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TOP 1 RAZSOC");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CODMAT = @CODMAT");
            sql.AppendLine("  AND CODCRE <> @CODMAT");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODMAT", DbType.Int32, IdCredenciado);
            return (Convert.ToString(db.ExecuteScalar(cmd)));
        }

        public bool Excluir(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            DbCommand cmd;
            if (Credenciado.FLAG_PJ == ConstantesSIL.FlgSim)
            {
                var sql = string.Format("UPDATE CREDENCIADO SET FLAG_VA = @FLG WHERE CODCRE = @CODCRE");
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
                db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgNao);
            }
            else
            {
                var sql = string.Format("DELETE CREDENCIADO WHERE CODCRE = @CODCRE");
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            }
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ExcluirObs(db, dbt, Credenciado.CODCRE);
                ExcluirDependencias(db, dbt, Credenciado.CODCRE);
                db.ExecuteNonQuery(cmd, dbt);

                UtilSIL.GravarLog(db, dbt, Credenciado.FLAG_PJ == ConstantesSIL.FlgSim
                                      ? "UPDATE CREDENCIADO (Modificou o Flag para PJ)"
                                      : "DELETE CREDENCIADO", FOperador, cmd);
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
            ExcluirAutorizador(Credenciado);
            return true;
        }

        public bool ImportarNetcardPj(int Credenciado)
        {
            var sql = new StringBuilder();
            sql.AppendLine("UPDATE CREDENCIADO");
            sql.AppendLine("    SET FLAG_VA = @FLG, ");
            sql.AppendLine("        DIAFEC_VA = DIAFEC, ");
            sql.AppendLine("        CTABCO_VA = CTABCO, ");
            sql.AppendLine("        NUMFEC_VA = NUMFEC, ");
            sql.AppendLine("        DATULTFEC_VA = DATULTFEC, ");
            sql.AppendLine("        TIPFEC_VA = TIPFEC, ");
            sql.AppendLine("        DATCTT_VA = '" + DateTime.Now.ToString("yyyyMMdd") + "', ");
            sql.AppendLine("        PRAREE = PRAPAG, ");
            sql.AppendLine("        TAXADM = TAXSER, ");
            sql.AppendLine("        PRAENT = 5, ");
            sql.AppendLine("        LIMREENT = 600000 ");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgSim);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE CREDENCIADO (ImportarNETCARDPJ)", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Importacao]" + err);
            }
            finally
            {
                dbc.Close();
            }

            #region AUTORIZADOR
            ImportarAutorizador(Credenciado);
            #endregion

            return true;
        }

        #endregion

        #region Modulo Web

        public bool RenovarAcesso(CREDENCIADO Credenciado)
        {
            var dataRenavacao = DateTime.Now.AddDays(DiasParaRenovarSenha()).ToString("yyyyMMdd");
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("UPDATE VCREDENCIADO " +
                                    "   SET SENWEB = @SENWEB, SENCRE = @SENCRE, DTEXPSENHA = @DTEXPSENHA, DTSENHA = @DTSENHA, " +
                                    "   QTDEACESSOINV = @QTDEACESSOINV " +
                                    "WHERE CODCRE = @CODCRE");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "SENWEB", DbType.String, Credenciado.SENWEB);
            db.AddInParameter(cmd, "SENCRE", DbType.String, 123456);
            db.AddInParameter(cmd, "DTEXPSENHA", DbType.String, dataRenavacao);
            db.AddInParameter(cmd, "DTSENHA", DbType.DateTime, null);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "QTDEACESSOINV", DbType.Int32, 0);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);

                UtilSIL.GravarLog("UPDATE CREDENCIADO", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Renovar Acesso]" + err);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }

        public int DiasParaRenovarSenha()
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'PRZ_SENHA_INIC'";
            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public string ValidadeSenha(int CODCRE)
        {
            Database db = new SqlDatabase(BDTELENET);
            string sql = "SELECT DTEXPSENHA FROM CREDENCIADO WHERE CODCRE = " + CODCRE;
            var cmd = db.GetSqlStringCommand(sql);
            var validade = db.ExecuteScalar(cmd);
            var retorno = string.Empty;
            if (validade != DBNull.Value)
                retorno = Convert.ToDateTime(validade).ToShortDateString();
            return retorno;
        }

        #endregion

        #region CRUD Afiliacao

        public void IncluirDadosCred(AFILIACAO dadosCred)
        {            
            const string sql = "INSERE_DADOSCRED";
            Database db = new SqlDatabase(BDCONCENTRADOR);

            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@SIGUF0", DbType.String, dadosCred.SIGUF0);
            db.AddInParameter(cmd, "@RAZSOC", DbType.String, UtilSIL.RemoverAcentos(dadosCred.RAZSOC));
            db.AddInParameter(cmd, "@NOMFAN", DbType.String, UtilSIL.RemoverAcentos(dadosCred.NOMFAN));
            db.AddInParameter(cmd, "@TEL", DbType.String, dadosCred.TEL);
            db.AddInParameter(cmd, "@EMA", DbType.String, dadosCred.EMA);
            db.AddInParameter(cmd, "@FAX", DbType.String, dadosCred.FAX);
            db.AddInParameter(cmd, "@ENDCRE", DbType.String, dadosCred.ENDCRE);
            db.AddInParameter(cmd, "@CEP", DbType.String, dadosCred.CEP);
            db.AddInParameter(cmd, "@INSEST", DbType.String, dadosCred.INSEST);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, dadosCred.CGC);
            var idr = db.ExecuteReader(cmd);
        }

        #endregion

        #region CRUD Observacoes

        public bool InserirObs(int CODCRE, DateTime DATA, string OBS, Int32 ID)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sbCamposCredenciado = new StringBuilder();
            var sbParametrosCamposCredenciado = new StringBuilder();
            sbCamposCredenciado.Append("CODCRE, DATA, OBS");
            sbParametrosCamposCredenciado.Append("@CODCRE, @DATA, @OBS");
            var sql = string.Format("INSERT INTO OBSCRED ({0}) VALUES ({1})", sbCamposCredenciado, sbParametrosCamposCredenciado);
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, CODCRE);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, DATA.ToString("G"));
            db.AddInParameter(cmd, "OBS", DbType.String, OBS);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {                                
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "INSERT OBSCRED", FOperador, cmd);
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

        public bool AlterarObs(Int32 ID, string OBS)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "UPDATE OBSCRED SET OBS = @OBS WHERE ID = @ID";
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "ID", DbType.Int32, ID);
            db.AddInParameter(cmd, "OBS", DbType.String, OBS);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "UPDATE OBSCRED", FOperador, cmd);
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

        public bool ExcluirObs(Int32 ID)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = string.Format("DELETE OBSCRED WHERE ID = @ID");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "ID", DbType.Int32, ID);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE OBSCRED", FOperador, cmd);
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

        public bool ExcluirObs(Database db, DbTransaction dbt, int Credenciado)
        {
            const string sql = "DELETE FROM OBSCRED WHERE CODCRE = @CODCRE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.ExecuteNonQuery(cmd, dbt);
            return true;
        }

        public bool ExcluirDependencias(Database db, DbTransaction dbt, int Credenciado)
        {
            var sql = "DELETE FROM FECHCREDVA WHERE CODCRE = @CODCRE";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.ExecuteNonQuery(cmd, dbt);
            sql = "DELETE FROM FECHCRED WHERE CODCRE = @CODCRE";
            cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado);
            db.ExecuteNonQuery(cmd, dbt);
            return true;
        }

        #endregion

        #region Credenciado Ja Cadastrado

        public bool CodigoExistente(int codigo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            sql.AppendLine("  AND FLAG_VA = @FLAG_VA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codigo);
            db.AddInParameter(cmd, "FLAG_VA", DbType.String, ConstantesSIL.FlgSim);
            var CodCre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCre != 0);
        }

        public bool CredenciadoExistenteNetcardPj(int codigo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CODCRE = @CODCRE");
            sql.AppendLine("  AND ((FLAG_VA IS NULL) OR (FLAG_VA = @FLAG))");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codigo);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgNao);
            var CodCre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (CodCre != 0);
        }

        public bool CredenciadoExistenteNetcardPj(string cnpj, out int codcre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CGC = @CGC");
            sql.AppendLine("  AND ((FLAG_VA IS NULL) OR (FLAG_VA = @FLAG))");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CGC", DbType.String, cnpj);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgNao);
            codcre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (codcre != 0);
        }

        public bool CredenciadoExistenteDadosCred(string cnpj)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CGC");
            sql.AppendLine("FROM DADOSCRED");
            sql.AppendLine("WHERE CGC = @CNPJ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, cnpj);
            var cgc = Convert.ToString(db.ExecuteScalar(cmd));
            return (cgc!= string.Empty);
        }

        public bool CnpjExistentePrincipal(string cnpj, out int codcre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CGC = @CNPJ");
            sql.AppendLine("  AND FLAG_VA = @FLAG");
            sql.AppendLine("  AND CODCRE = CODPRI");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgSim);
            codcre = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (codcre != 0);
        }

        public bool ValidarSegmentoPrncipal(string cnpj)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT CODCRE");
            sql.AppendLine("FROM CREDENCIADO");
            sql.AppendLine("WHERE CGC = @CNPJ");
            sql.AppendLine("  AND FLAG_VA = @FLAG");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgSim);
            var Codigo = Convert.ToInt32(db.ExecuteScalar(cmd));
            return (Codigo != 0);
        }

        #endregion

        #region CRUD Equipamentos

        private void AtualizarQtdeEquipamento(int qtde, int codCre)
        {
            var db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var cmd = db.GetSqlStringCommand("UPDATE  CREDENCIADO SET QTDPOS = " + qtde + " WHERE CODCRE = " + codCre);
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch
            {
                dbt.Rollback();
                throw new Exception("Erro ao atualizar Qtde Equipamentos");
            }
            finally
            {
                dbc.Close();
            }
        }

        public bool InserirEquipamento(int codOpe, string codAfil, CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            const string sql = "CAD_TERMINAL";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            var codCre = Convert.ToString(Credenciado.CODCRE).PadLeft(6, '0');
            db.AddInParameter(cmd, "@CODAFIL", DbType.String, codAfil);
            db.AddInParameter(cmd, "@CODOPE", DbType.Int32, codOpe);
            db.AddInParameter(cmd, "@CODPS", DbType.String, codCre);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var linhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                if (linhaAfetada == 1)
                {
                    var quantEquip = Convert.ToInt16(Credenciado.QTDPOS - 1);
                    Credenciado.QTDPOS = quantEquip;
                    AtualizarQtdeEquipamento(quantEquip, Credenciado.CODCRE);
                }
                UtilSIL.GravarLog("INSERT CTPOS", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Equipamento]" + err);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }

        public bool ExcluirEquipamento(string ID, CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDCONCENTRADOR);
            var sql = string.Format("DELETE CTPOS WHERE CODPOS = @CODPOS");
            var cmd = db.GetSqlStringCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "CODPOS", DbType.String, ID);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                if (LinhaAfetada == 1)
                {
                    var quantEquip = Convert.ToInt16(Credenciado.QTDPOS - 1);
                    Credenciado.QTDPOS = quantEquip;
                    AtualizarQtdeEquipamento(quantEquip, Credenciado.CODCRE);
                }
                UtilSIL.GravarLog("DELETE CTPOS", FOperador, cmd);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Equipamento]" + err);
            }
            finally
            {
                dbc.Close();
            }
            return true;
        }        

        #endregion

        #region CRUD AUTORIZADOR

        private void InserirAutorizador(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                InserirAutorizadorCredenciado(Credenciado, db, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Autorizador]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        private void AtualizaRamoAtividade(CREDENCIADO Credenciado)
        {
            const string sql = "PROC_ATUALIZA_ATIV_CLI";
            Database db = new SqlDatabase(BDTELENET);

            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, Credenciado.CODCRE);
            db.AddInParameter(cmd, "CODSEG", DbType.Int32, Credenciado.CODSEG);
            db.AddInParameter(cmd, "CODATI", DbType.Int32, Credenciado.CODATI);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var teste = Convert.ToString(idr["RETORNO"]);
            }
            idr.Close();
        }

        private void AlterarAutorizador(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                if (!ExisteCredenciadoAutorizador(Credenciado, db, dbt))
                    InserirAutorizadorCredenciado(Credenciado, db, dbt);
                else
                    AlterarAutorizadorCredenciado(Credenciado, db, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Alterar Autorizador]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        private void ImportarAutorizador(int Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ImportarAutorizadorCredenciado(Credenciado, db, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Autorizador]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        private void ExcluirAutorizador(CREDENCIADO Credenciado)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ExcluirAutorizadorCredenciado(Credenciado, db, dbt);
                dbt.Commit();
            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Autorizador]" + err);
            }
            finally
            {
                dbc.Close();
            }
        }

        private static bool ExisteCredenciadoAutorizador(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "SELECT CODPS FROM CTPREST WHERE CODPS = @CODPS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            var Codigo = Convert.ToString(db.ExecuteScalar(cmd, dbt));
            return (Codigo != string.Empty);
        }

        private static void InserirAutorizadorCredenciado(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "INSERT INTO CTPREST " +
                               "(CODPS, NOMEPS, SEGPS, CODATI, STATPS, DTSTATPS, MSGATIVA, SENHA, UF, FLG_VA)" +
                               "VALUES " +
                               "(@CODPS, @NOMEPS, @SEGPS, @CODATI, @STATPS, @DTSTATPS, @MSGATIVA, @SENHA, @UF, @FLG_VA)";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            db.AddInParameter(cmd, "NOMEPS", DbType.String, Credenciado.RAZSOC.Length > 30 ? Credenciado.RAZSOC.Substring(0, 30) : Credenciado.RAZSOC);
            db.AddInParameter(cmd, "SEGPS", DbType.String, Credenciado.CODSEG.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0'));
            db.AddInParameter(cmd, "CODATI", DbType.Int32, Credenciado.CODATI);
            db.AddInParameter(cmd, "STATPS", DbType.String, Credenciado.STA);
            db.AddInParameter(cmd, "DTSTATPS", DbType.DateTime, Credenciado.DATSTA);
            db.AddInParameter(cmd, "MSGATIVA", DbType.String, Credenciado.MSGATVCRT);
            db.AddInParameter(cmd, "SENHA", DbType.String, Credenciado.SENCRE);
            db.AddInParameter(cmd, "UF", DbType.String, Credenciado.SIGUF0);
            db.AddInParameter(cmd, "FLG_VA", DbType.String, Credenciado.FLAG_VA);
            db.ExecuteNonQuery(cmd, dbt);
        }

        private static void AlterarAutorizadorCredenciado(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "UPDATE CTPREST SET NOMEPS = @NOMEPS, SEGPS = @SEGPS, CODATI = @CODATI, STATPS = @STATPS, DTSTATPS = @DTSTATPS, " +
                               "MSGATIVA = @MSGATIVA, SENHA = @SENHA, UF = @UF, FLG_VA = @FLG " +
                               "WHERE CODPS = @CODPS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            var NomeCredenciado = Credenciado.RAZSOC;
            if (NomeCredenciado.Length > 30)
                NomeCredenciado = Credenciado.RAZSOC.Substring(0, 30);
            db.AddInParameter(cmd, "NOMEPS", DbType.String, NomeCredenciado);
            db.AddInParameter(cmd, "SEGPS", DbType.String, Credenciado.CODSEG.ToString(CultureInfo.InvariantCulture).PadLeft(5, '0'));
            db.AddInParameter(cmd, "CODATI", DbType.Int32, Credenciado.CODATI);
            db.AddInParameter(cmd, "STATPS", DbType.String, Credenciado.STA);
            db.AddInParameter(cmd, "DTSTATPS", DbType.DateTime, Credenciado.DATSTA);
            db.AddInParameter(cmd, "MSGATIVA", DbType.String, Credenciado.MSGATVCRT);
            db.AddInParameter(cmd, "SENHA", DbType.String, Credenciado.SENCRE);
            db.AddInParameter(cmd, "UF", DbType.String, Credenciado.SIGUF0);
            db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgSim);
            db.ExecuteNonQuery(cmd, dbt);
        }

        private static void ImportarAutorizadorCredenciado(int Credenciado, Database db, DbTransaction dbt)
        {
            const string sql = "UPDATE CTPREST SET FLG_VA = @FLG WHERE CODPS = @CODPS";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
            db.AddInParameter(cmd, "FLG", DbType.String, ConstantesSIL.FlgSim);
            db.ExecuteNonQuery(cmd, dbt);
        }

        private static void ExcluirAutorizadorCredenciado(CREDENCIADO Credenciado, Database db, DbTransaction dbt)
        {
            string sql;
            DbCommand cmd;
            if (Credenciado.FLAG_PJ == ConstantesSIL.FlgSim)
            {
                sql = "UPDATE CTPREST SET FLG_VA = @FLAG WHERE CODPS = @CODPS";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
                db.AddInParameter(cmd, "FLAG", DbType.String, ConstantesSIL.FlgNao);
                db.ExecuteNonQuery(cmd, dbt);
            }
            else
            {
                sql = "DELETE CTPREST WHERE CODPS = @CODPS";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODPS", DbType.String, Credenciado.CODCRE.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'));
                db.ExecuteNonQuery(cmd, dbt);
            }
        }

        #endregion

        #region CRUD TABRAFIL

        public void InserirTabrafil(int codOpe, string cnpj, string codDestino)
        {            
            const string sql = "INSERE_TABRAFIL";
            Database db = new SqlDatabase(BDCONCENTRADOR);

            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@CODOPE", DbType.Int16, codOpe);            
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@DESTINO", DbType.String, codDestino);
            db.ExecuteReader(cmd);
        }

        #endregion

        #region CRUD TAXASCRE

        public bool SalvarTaxaCre(MODTAXA taxacre)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_ASSOCIA_TAXACRE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@SISTEMA", DbType.Int32, ConstantesSIL.SistemaPRE);
            db.AddInParameter(cmd, "@CODCRE", DbType.String, taxacre.COD);
            db.AddInParameter(cmd, "@CODTAXA", DbType.String, taxacre.CODTAXA);
            db.AddInParameter(cmd, "@VALOR", DbType.Decimal, taxacre.VALOR);
            db.AddInParameter(cmd, "@NUMPAC", DbType.Int16, taxacre.NUMPARC);
            db.AddInParameter(cmd, "@DTINICIO", DbType.DateTime, taxacre.DTINICIO);
            db.AddInParameter(cmd, "@DIACOB", DbType.Int32, taxacre.DIACOB);
            db.AddInParameter(cmd, "@DIASPINICIO", DbType.Int16, taxacre.DIASPINICIO);
            db.AddInParameter(cmd, "@COBSEMCRED", DbType.String, taxacre.COBSEMCRED);
            db.AddInParameter(cmd, "@PRIORIDADE", DbType.Int16, taxacre.PRIORIDADE);
            db.AddInParameter(cmd, "@TAXAHAB", DbType.String, taxacre.TAXAHAB);
            db.AddInParameter(cmd, "@VALMINCRED", DbType.Decimal, taxacre.VALMINCRED);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {                               
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_ASSOCIA_TAXACRE", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
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

        public bool ImportarTaxasPj(int codCre)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_IMPORTA_TAXAPJ";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            db.AddInParameter(cmd, "@CODCRE", DbType.Int32, codCre);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var LinhaAfetada = db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "PROC_IMPORTA_TAXAPJ", FOperador, cmd);
                dbt.Commit();
                return (LinhaAfetada == 1);
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

        public bool ExcluirTaxaCre(MODTAXA taxacre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            var sql = string.Format("DELETE TAXACREVA WHERE CODCRE = @CODCRE AND CODTAXA = @CODTAXA");
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, taxacre.COD);
            db.AddInParameter(cmd, "CODTAXA", DbType.Int32, taxacre.CODTAXA);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                db.ExecuteNonQuery(cmd, dbt);
                UtilSIL.GravarLog(db, dbt, "DELETE TAXACREVA", FOperador, cmd);
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

        public int ValidarExclusaoTaxaCre(MODTAXA taxacre)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT COUNT(DATTRA) AS NUMTRANS FROM TRANSACVA ");
            sql.AppendLine("WHERE TIPTRA = @TIPTRA ");
            sql.AppendLine("AND CODCRE = @CODCRE");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, taxacre.COD);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, taxacre.TIPTRA);
            return (int) (db.ExecuteScalar(cmd));
        }

        public string ValidarNumPac(Int32 codTaxa)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();
            sql.AppendLine("SELECT TRENOVA FROM TAXAVA ");
            sql.AppendLine("WHERE CODTAXA = @CODTAXA");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODTAXA", DbType.Int32, codTaxa);
            var idr = db.ExecuteReader(cmd);
            var renovacao = string.Empty;
            while (idr.Read())
            {
                renovacao = Convert.ToString(idr["TRENOVA"]);
            }
            idr.Close();
            return renovacao;
        }

        #endregion

        #region CRUD HABREDES

        public string SalvarHabRedes(REDES rede, string cnpj, bool excluir)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_HABILITA_REDE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            var retorno = string.Empty;

            db.AddInParameter(cmd, "@CODCRE", DbType.String, rede.CODCRE);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@REDE", DbType.String, rede.REDE);
            db.AddInParameter(cmd, "@HABREDE", DbType.String, excluir ? "E" : rede.HABREDE);
            db.AddInParameter(cmd, "@HABPOS", DbType.String, "S");
            db.AddInParameter(cmd, "@HABTEF", DbType.String, "N");
            db.AddInParameter(cmd, "@CODAFILREDE", DbType.String, rede.CODAFILREDE);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                idr = db.ExecuteReader(cmd, dbt);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    idr.Close();
                    if (retorno == "OK")
                        UtilSIL.GravarLog(db, dbt, "PROC_HABILITA_REDE ", FOperador, cmd);                    
                }
                if (idr != null) idr.Close();
                dbt.Commit();
                return retorno;
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

        public string EqualizarHabRedes(REDES rede, string cnpj, bool excluir)
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "PROC_EQUALIZA_HAB_REDE";
            var cmd = db.GetStoredProcCommand(sql);
            var dbc = db.CreateConnection();
            IDataReader idr = null;
            var retorno = string.Empty;

            db.AddInParameter(cmd, "@CODCRE", DbType.String, rede.CODCRE);
            db.AddInParameter(cmd, "@CNPJ", DbType.String, cnpj);
            db.AddInParameter(cmd, "@REDE", DbType.String, rede.REDE);
            db.AddInParameter(cmd, "@HABREDE", DbType.String, excluir ? "E" : rede.HABREDE);
            db.AddInParameter(cmd, "@HABPOS", DbType.String, "S");
            db.AddInParameter(cmd, "@HABTEF", DbType.String, "N");
            db.AddInParameter(cmd, "@CODAFILREDE", DbType.String, rede.CODAFILREDE);
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                idr = db.ExecuteReader(cmd, dbt);
                if (idr.Read())
                {
                    retorno = Convert.ToString(idr["RETORNO"]);
                    idr.Close();                    
                    if (retorno == "OK")
                        UtilSIL.GravarLog(db, dbt, "PROC_EQUALIZA_HAB_REDE ", FOperador, cmd);
                }
                dbt.Commit();
                if (idr != null) idr.Close();
                return retorno;
            }
            catch (Exception err)
            {
                if (idr != null)
                    idr.Close();
                dbt.Rollback();
                throw new Exception(err.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        #endregion

        #region CRUD Especialidades Ativas

        public bool InserirEspAtiva(int codCre, int codEsp)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            string sql;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                sql = "INSERT INTO ESPECRED (CODCRE, CODESP) " +
                      "SELECT @CODCRE, @CODESP " +
                      " WHERE NOT EXISTS " +
                      "(SELECT CODESP FROM ESPECRED WHERE CODCRE = @CODCRE AND CODESP = @CODESP)";

                cmd = db.GetSqlStringCommand(sql);

                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
                db.AddInParameter(cmd, "CODESP", DbType.Int32, codEsp);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT ESPECRED", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Especialidades]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // Sucesso
            return true;

        }

        // Inserir Todos Especialidades Disponiveis
        public bool InserirEspAtiva(int codCre)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            StringBuilder sql;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();
            sql = new StringBuilder();
            try
            {
                sql.AppendLine("INSERT INTO ESPECRED (CODCRE, CODESP) ");
                sql.AppendLine("SELECT @CODCRE, E.CODESP");
                sql.AppendLine("FROM ESPECIALIDADE E");
                sql.AppendLine("WHERE NOT EXISTS");
                sql.AppendLine("(SELECT EA.CODESP");
                sql.AppendLine("  FROM ESPECRED EA");
                sql.AppendLine(" WHERE EA.CODESP = E.CODESP");
                sql.AppendLine("   AND EA.CODCRE = @CODCRE)");

                cmd = db.GetSqlStringCommand(sql.ToString());
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "INSERT ESPECRED", FOperador, cmd);
                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Inserir Especialidades]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // Sucesso
            return true;

        }

        public bool ExcluirEspAtiva(int codCre, int codEsp)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            string sql;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                sql = "DELETE FROM ESPECRED WHERE CODCRE = @CODCRE AND CODESP = @CODESP";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);
                db.AddInParameter(cmd, "CODESP", DbType.Int32, codEsp);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE ESPECRED", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Especialidade]" + err);
            }
            finally
            {
                dbc.Close();
            }

            // Sucesso
            return true;
        }

        // Excluir Todos Segmentos Disponiveis
        public bool ExcluirEspAtiva(int codCre)
        {
            Database db;
            DbConnection dbc;
            DbTransaction dbt;
            string sql;
            DbCommand cmd;

            db = new SqlDatabase(BDTELENET);
            dbc = db.CreateConnection();
            dbc.Open();

            // Controle Transacao
            dbt = dbc.BeginTransaction();

            try
            {
                sql = "DELETE FROM ESPECRED WHERE CODCRE = @CODCRE";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "CODCRE", DbType.Int32, codCre);

                db.ExecuteNonQuery(cmd, dbt);

                //LOG GERAL PARA QUALQUER MODIFICACAO NOS DADOS (O cmd e pra listar os parametros)
                UtilSIL.GravarLog(db, dbt, "DELETE ESPECRED", FOperador, cmd);

                dbt.Commit();

            }
            catch (Exception err)
            {
                dbt.Rollback();
                throw new Exception("Erro Camada DAL [Excluir Colecao Especialidades]" + err);
            }
            finally
            {
                dbc.Close();

            }

            // Sucesso
            return true;
        }

        #endregion

        #region GET Especialidades Ativadas

        public List<ESPATIVADA_CREDENCIADO> EspAtivas(int codCre)
        {
            var ColecaoEspecAtivadas = new List<ESPATIVADA_CREDENCIADO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  E.CODESP, E.NOMESP");
            sql.AppendLine("FROM ESPECIALIDADE E");
            sql.AppendLine("JOIN ESPECRED EA");
            sql.AppendLine("  ON EA.CODESP = E.CODESP");
            sql.AppendLine(" AND EA.CODCRE = @CODCRE");
            sql.AppendLine("ORDER BY E.NOMESP");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var EspecAtivada = new ESPATIVADA_CREDENCIADO();
                EspecAtivada.CODCRE = codCre;
                EspecAtivada.CODESP = Convert.ToInt32(idr["CODESP"]);
                EspecAtivada.NOMESP = Convert.ToString(idr["NOMESP"]).Trim();
                ColecaoEspecAtivadas.Add(EspecAtivada);
            }
            idr.Close();
            return ColecaoEspecAtivadas;
        }

        public List<ESPATIVADA_CREDENCIADO> EspDisponiveis(int codCre)
        {
            var ColecaoEspAtivas = new List<ESPATIVADA_CREDENCIADO>();

            Database db = new SqlDatabase(BDTELENET);
            var sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("  E.CODESP, E.NOMESP");
            sql.AppendLine("FROM ESPECIALIDADE E");
            sql.AppendLine("WHERE NOT EXISTS");
            sql.AppendLine("(SELECT EA.CODESP");
            sql.AppendLine("  FROM ESPECRED EA");
            sql.AppendLine(" WHERE EA.CODESP = E.CODESP");
            sql.AppendLine("   AND EA.CODCRE = @CODCRE)");
            sql.AppendLine("ORDER BY E.NOMESP");

            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODCRE", DbType.String, codCre);
            var idr = db.ExecuteReader(cmd);

            while (idr.Read())
            {
                var espAtivas = new ESPATIVADA_CREDENCIADO();
                espAtivas.CODCRE = codCre;
                espAtivas.CODESP = Convert.ToInt32(idr["CODESP"]);
                espAtivas.NOMESP = Convert.ToString(idr["NOMESP"]).Trim();

                ColecaoEspAtivas.Add(espAtivas);
            }
            idr.Close();

            return ColecaoEspAtivas;
        }

        #endregion

        #region  LOG

        private void GeraLogTrans(DbTransaction dbt, Database db, CREDENCIADO cre, int tiptra)
        {
            var cmd = db.GetStoredProcCommand("PROC_GRAVAR_TRANSACAO");
            db.AddInParameter(cmd, "CODCLI", DbType.Int32, DBNull.Value);
            db.AddInParameter(cmd, "CODCRE", DbType.Int32, cre.CODCRE);
            db.AddInParameter(cmd, "TIPTRA", DbType.Int32, tiptra);
            db.AddInParameter(cmd, "CODCRT", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "VALOR", DbType.Decimal, 0m);
            db.AddInParameter(cmd, "CPF", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "NUMDEP", DbType.Int16, 0);
            db.AddInParameter(cmd, "NUMCARGAVA", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "NUMFECCRE", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "DAD", DbType.Int16, DBNull.Value);
            db.AddInParameter(cmd, "ATV", DbType.String, DBNull.Value);
            db.AddInParameter(cmd, "ID_FUNC", DbType.Int16, FOperador.ID_FUNC);
            db.ExecuteNonQuery(cmd, dbt);
        }

        #endregion        

        #region Grupos Credenciados

        internal List<GRUPO> ColecaoGrupos(string filtro)
        {
            var colecao = new List<GRUPO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODGRUPO, NOMGRUPO, SISTEMA ");
            sql.AppendLine(" FROM GRUPO ");
            if (!string.IsNullOrEmpty(filtro))
                sql.AppendLine(string.Format("WHERE {0} ", filtro));
            sql.AppendLine(" ORDER BY NOMGRUPO ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var grupo = new GRUPO();
                grupo.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                grupo.NOMGRUPO = Convert.ToString(idr["NOMGRUPO"]);
                grupo.SISTEMA = Convert.ToString(idr["SISTEMA"]);
                colecao.Add(grupo);
            }
            idr.Close();
            return colecao;
        }

        internal List<GRUPOCREDENCIADO> ColecaoCredenciadosPorGrupo(int idGrupo)
        {
            var colecao = new List<GRUPOCREDENCIADO>();
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT G.CODGRUPO, G.CODCRE, C.NOMFAN, C.RAZSOC ");
            sql.AppendLine(" FROM GRUPOCRED G INNER JOIN CREDENCIADO C ");
            sql.AppendLine(" ON G.CODCRE = C.CODCRE  ");
            sql.AppendLine(string.Format(" WHERE G.CODGRUPO = {0} ", idGrupo));
            sql.AppendLine(" ORDER BY C.NOMFAN ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var grupoCred = new GRUPOCREDENCIADO();
                grupoCred.CODGRUPO = Convert.ToInt32(idr["CODGRUPO"]);
                grupoCred.CODCRE = Convert.ToInt32(idr["CODCRE"]);
                grupoCred.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                grupoCred.RAZSOC = Convert.ToString(idr["RAZSOC"]);

                colecao.Add(grupoCred);
            }
            idr.Close();
            return colecao;
        }

        internal void ExcluirCredenciadosGrupo(int idGrupo, List<object> listaCredenciados)
        {
            try
            {
                ExcluirCredenciadosGrupoNetcard(idGrupo, listaCredenciados);
                ExcluirCredenciadosAutorizador(idGrupo, listaCredenciados);
            }
            catch
            {
                throw new Exception("Erro ao excluir os credenciados do grupo. Repita a operacao");
            }           
        }

        internal void ExcluirCredenciadosAutorizador(int idGrupo, List<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var lista = listaCredenciados.Aggregate(string.Empty,(current, credenciado) => current + (credenciado + ","));
            lista = lista.Remove(lista.Length - 1, 1);
            sql.AppendLine(" DELETE CTGRUPOCRED ");
            sql.AppendLine(string.Format(" WHERE CODGRUPO = {0} ", idGrupo));
            sql.AppendLine(string.Format(" AND CODPS IN ( {0} )", lista));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        internal void ExcluirCredenciadosGrupoNetcard(int idGrupo, List<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            var lista = listaCredenciados.Aggregate(string.Empty,(current, credenciado) => current + (credenciado + ","));
            lista = lista.Remove(lista.Length - 1, 1);
            sql.AppendLine(" DELETE GRUPOCRED ");
            sql.AppendLine(string.Format(" WHERE CODGRUPO = {0} ", idGrupo));
            sql.AppendLine(string.Format(" AND CODCRE IN ( {0} )", lista));
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        internal bool VerificaCodigoExistente(int codGrupo)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" SELECT CODGRUPO ");
            sql.AppendLine(" FROM GRUPO ");
            sql.AppendLine(string.Format(" WHERE CODGRUPO = {0} ", codGrupo));
            var codigo = 0;
            var cmd = db.GetSqlStringCommand(sql.ToString());
            try
            {
                var idr = db.ExecuteReader(cmd);
                while (idr.Read())
                    codigo = Convert.ToInt32(idr["CODGRUPO"]);
                idr.Close();
            }
            catch
            {
                throw new Exception("Erro ao verificar a existencia do codigo do grupo inserido. Repita a operacao");
            }
            return codigo > 0;
        }

        internal bool SalvarGrupo(int codGrupo, string nome)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" INSERT GRUPO (CODGRUPO, NOMGRUPO, SISTEMA ) ");
            sql.AppendLine(" VALUES ( @CODGRUPO, @NOMGRUPO,  @SISTEMA) ");
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.AddInParameter(cmd, "CODGRUPO", DbType.Int32, codGrupo);
            db.AddInParameter(cmd, "NOMGRUPO", DbType.String, nome);
            db.AddInParameter(cmd, "SISTEMA", DbType.String, "VA");
            try
            {
                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                return false;
            }
            return true;
        }

        internal void ExcluirGrupo(int codGrupo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                ExcluirCredenciadosdoGrupo(db, dbt, codGrupo);
                dbt.Commit();
                ExcluirFinalGrupo(codGrupo);
                ExcluirCredenciadosEGrupoAutorizador(codGrupo);
            }
            catch (Exception ex)
            {
                if (dbt.Connection != null)
                    dbt.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                dbc.Close();
            }
        }

        private static void ExcluirCredenciadosdoGrupo(Database db, DbTransaction dbt, int codGrupo)
        {
            try
            {
                var sql = "DELETE FROM SEGAUTORIZVA WHERE codgrupo = @codgrupo";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
                sql = "DELETE FROM grupocred WHERE codgrupo = @codgrupo";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao excluir os credenciados do grupo.");
            }
        }

        private void ExcluirFinalGrupo(int codGrupo)
        {
            Database db = new SqlDatabase(BDTELENET);
            var sql = "DELETE FROM GRUPO WHERE codgrupo = " + codGrupo;
            var cmd = db.GetSqlStringCommand(sql);
            try
            {
                db.ExecuteNonQuery(cmd);
            }
            catch
            {
                throw new Exception("Somente os credenciados do grupo foram excluidos. Este Grupo nao sera excluido pois o mesmo pode estar sendo usado no sistema PJ.");
            }
        }

        private void ExcluirCredenciadosEGrupoAutorizador(int codGrupo)
        {
            Database db = new SqlDatabase(BDAUTORIZADOR);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                var sql = "DELETE FROM CTGRUPOCRED WHERE codgrupo = @codgrupo";
                var cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
                sql = "DELETE FROM TabSegVA WHERE codgrupo = @codgrupo";
                cmd = db.GetSqlStringCommand(sql);
                db.AddInParameter(cmd, "codgrupo", DbType.Int32, codGrupo);
                db.ExecuteNonQuery(cmd, dbt);
                dbt.Commit();
            }
            catch
            {
                dbt.Rollback();
                throw new Exception("Erro ao excluir o grupo no Autorizador.");
            }
            finally
            {
                dbc.Close();
            }
        }

        internal List<CREDENCIADO_VA> ColecaoCredenciadosForaDoGrupo(string sql)
        {
            var colecao = new List<CREDENCIADO_VA>();
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetSqlStringCommand(sql);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var cred = new CREDENCIADO_VA();
                cred.CODCRE = Convert.ToString(idr["CODCRE"]);
                cred.NOMFAN = Convert.ToString(idr["NOMFAN"]);
                cred.RAZSOC = Convert.ToString(idr["RAZSOC"]);
                colecao.Add(cred);
            }
            idr.Close();
            return colecao;
        }

        internal void IncluirCredenciadosGrupo(int idGrupo, List<object> listaCredenciados)
        {
            try
            {
                IncluirCredenciadosGrupoNetcard(idGrupo, listaCredenciados);
                IncluirCredenciadosGrupoAutorizador(idGrupo, listaCredenciados);
            }
            catch (Exception)
            {
                throw new Exception("Erro ao adicionar os credenciados ao grupo. Repita a operacao");
            }
        }

        private void IncluirCredenciadosGrupoNetcard(int idGrupo, IEnumerable<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDTELENET);
            sql.AppendLine(" INSERT INTO GRUPOCRED (CODGRUPO, CODCRE )");
            foreach (var credenciado in listaCredenciados)
                sql.AppendLine("SELECT " + idGrupo + " , " + credenciado + " UNION ALL ");
            sql.Remove(sql.Length - 10, 10);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        private void IncluirCredenciadosGrupoAutorizador(int idGrupo, IEnumerable<object> listaCredenciados)
        {
            var sql = new StringBuilder();
            Database db = new SqlDatabase(BDAUTORIZADOR);
            sql.AppendLine(" INSERT INTO CTGRUPOCRED (CODGRUPO, CODPS )");
            foreach (var credenciado in listaCredenciados)
                sql.AppendLine("SELECT " + idGrupo + " , '" + credenciado.ToString().PadLeft(6,'0') + "' UNION ALL ");
            sql.Remove(sql.Length - 10, 10);
            var cmd = db.GetSqlStringCommand(sql.ToString());
            db.ExecuteNonQuery(cmd);
        }

        internal string RecuperaGrupobyNome(string nomeGrupo)
        {
            var sql = "SELECT NOMGRUPO FROM GRUPO WHERE NOMGRUPO = '" + nomeGrupo + "'";
            Database db = new SqlDatabase(BDTELENET);
            var nomeGrupoRecuperado = string.Empty;
            try
            {
                var cmd = db.GetSqlStringCommand(sql);
                var idr = db.ExecuteReader(cmd);
                while (idr.Read())
                    nomeGrupoRecuperado = Convert.ToString(idr["NOMGRUPO"]);
                idr.Close();
            }
            catch
            {
                return string.Empty;
            }
            return nomeGrupoRecuperado;
        }

        #endregion

        #region 4DatasCred

        public List<_4DATAS> Proc_Ler_4datas_Fechcred(int DIAFEC_VA)
        {
            var colecao4Datas = new List<_4DATAS>();
            const string sql = "PROC_LER_4DATAS_FECHCRED";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@DIAFEC", DbType.Int32, DIAFEC_VA);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                var _4Datas = new _4DATAS
                {
                    DIA1 = Convert.ToString(idr["DIA1"]),
                    DIA2 = Convert.ToString(idr["DIA2"]),
                    DIA3 = Convert.ToString(idr["DIA3"]),
                    DIA4 = Convert.ToString(idr["DIA4"])
                };
                colecao4Datas.Add(_4Datas);
            }
            idr.Close();
            return colecao4Datas;
        }

        public int Proc_Gera_4dadas_Fechcred(int DIA1, int DIA2,int DIA3, int DIA4)
        {
           
            const string sql = "PROC_GERA_4DATAS_FECHCRED";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@DIA1", DbType.Int32, DIA1);
            db.AddInParameter(cmd, "@DIA2", DbType.Int32, DIA2);
            db.AddInParameter(cmd, "@DIA3", DbType.Int32, DIA3);
            db.AddInParameter(cmd, "@DIA4", DbType.Int32, DIA4);
         //   var idr = db.ExecuteReader(cmd);
            var DiaFech = Convert.ToInt32(db.ExecuteScalar(cmd));
            return DiaFech;
        }

        ///
        public bool BuscaParamFechCred4d() 
        {
            Database db = new SqlDatabase(BDTELENET);
            const string sql = "SELECT VAL FROM PARAMVA WHERE ID0 = 'FECHCRED4D' ";

            var cmd = db.GetSqlStringCommand(sql);
            return Convert.ToString(db.ExecuteScalar(cmd)) == "S";           

        }

        #endregion

        #region TransHab

        public TRANSHABCKB GetTransHab(string transHab)
        {
            var transHabCkb = new TRANSHABCKB();
            const string sql = "PROC_TRANSHAB_LER";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@TRANSHAB", DbType.String, transHab);
            var idr = db.ExecuteReader(cmd);
            while (idr.Read())
            {
                transHabCkb.PDV = Convert.ToString(idr["PDV"]);
                transHabCkb.POSMIC = Convert.ToString(idr["POSMIC"]);
                transHabCkb.URA = Convert.ToString(idr["URA"]);
                transHabCkb.CENTRAL = Convert.ToString(idr["CENTRAL"]);
                transHabCkb.COMPRA = Convert.ToString(idr["COMPRA"]);
                transHabCkb.COMPRAMED = Convert.ToString(idr["COMPRAMED"]);
                transHabCkb.COMPRAPARC = Convert.ToString(idr["COMPRAPARC"]);
                transHabCkb.COMPRAMEDPARC = Convert.ToString(idr["COMPRAMEDPARC"]);
                transHabCkb.COMPRACRTDIG = Convert.ToString(idr["COMPRACRTDIG"]);
            }
            idr.Close();
            return transHabCkb;
        }

        public string SetTransHab(TRANSHABCKB transHab)
        {
            var transHabVal = string.Empty;
            const string sql = "PROC_TRANSHAB_GRAVAR";
            Database db = new SqlDatabase(BDTELENET);
            var cmd = db.GetStoredProcCommand(sql);
            db.AddInParameter(cmd, "@PDV", DbType.String, transHab.PDV);
            db.AddInParameter(cmd, "@POSMIC", DbType.String, transHab.POSMIC);
            db.AddInParameter(cmd, "@URA", DbType.String, transHab.URA);
            db.AddInParameter(cmd, "@CENTRAL", DbType.String, transHab.CENTRAL);
            db.AddInParameter(cmd, "@COMPRA", DbType.String, transHab.COMPRA);
            db.AddInParameter(cmd, "@COMPRAMED", DbType.String, transHab.COMPRAMED);
            db.AddInParameter(cmd, "@COMPRAPARC", DbType.String, transHab.COMPRAPARC);
            db.AddInParameter(cmd, "@COMPRAMEDPARC", DbType.String, transHab.COMPRAMEDPARC);
            db.AddInParameter(cmd, "@COMPRACRTDIG", DbType.String, transHab.COMPRACRTDIG);

            var idr = db.ExecuteReader(cmd);
            if (idr.Read())
            {
                transHabVal = Convert.ToString(idr["TRANSHAB"]);
            }
            idr.Close();
            return transHabVal;
        }

        #endregion
    }
}