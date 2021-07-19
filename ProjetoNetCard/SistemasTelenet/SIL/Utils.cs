using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SIL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TELENET.SIL.PO;

namespace TELENET.SIL
{
    public static class UtilSIL
    {
        public static string RetirarCaracteres(string Caracteres, string Str)
        {
            var ResultStr = Str;
            for (var i = 0; i < Caracteres.Length; i++)
            {
                ResultStr = ResultStr.Replace(Caracteres[i].ToString(CultureInfo.InvariantCulture), string.Empty);
            }
            return ResultStr;
        }

        public static void GravarLog(string path, string log)
        {
            for (var i = 0; i < 4; i++)
            {
                bool acessar;
                using (new Mutex(true, path.Substring(path.Length - 12, 12), out acessar))
                {
                    if (!acessar)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        var s = File.AppendText(path);
                        s.WriteLine(log);
                        s.Close();
                        return;
                    }
                }
            }
        }

        public static string MascaraCartao(string codcrt, int tamanho)
        {
            if (codcrt == null) return string.Empty;
            var tam = codcrt.Length;
            tamanho = tamanho <= 10 ? 15 : tamanho;
            var codCrtMask = codcrt.Replace(codcrt.Substring(6, tam < 13 ? tam - 6 : 7), new string('*', tamanho - 10));
            return codCrtMask;
        }

        public static bool ValidarCnpjCpf(string cnpjcpf)
        {
            bool ok;
            switch (cnpjcpf.Length)
            {
                case 14:
                    ok = ValidarCnpj(cnpjcpf);
                    break;
                case 11:
                    ok = ValidarCpf(cnpjcpf);
                    break;
                default:
                    ok = false;
                    break;
            }
            return ok;
        }

        public static bool ValidarCnpj(string cnpj)
        {
            cnpj = cnpj.Replace("/", "");
            cnpj = cnpj.Replace(".", "");
            cnpj = cnpj.Replace("-", "");
            if (cnpj == "00000000000000") { return false; }
            const string ftmt = "6543298765432";
            var digitos = new Int32[14];
            var soma = new Int32[2];
            soma[0] = 0;
            soma[1] = 0;
            var resultado = new Int32[2];
            resultado[0] = 0;
            resultado[1] = 0;
            var cnpjOk = new Boolean[2];
            cnpjOk[0] = false;
            cnpjOk[1] = false;
            try
            {
                Int32 nrDig;
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));
                    if (nrDig <= 11) soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
                    if (nrDig <= 12) soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
                }
                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        cnpjOk[nrDig] = (digitos[12 + nrDig] == 0);
                    else
                        cnpjOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
                }
                return (cnpjOk[0] && cnpjOk[1]);
            }
            catch { return false; }
        }

        public static bool ValidarCpf(string cpf)
        {
            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11) return false;
            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;
            for (var i = 0; i < 9; i++) soma += int.Parse(tempCpf[i].ToString(CultureInfo.InvariantCulture)) * multiplicador1[i];
            var resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;
            var digito = resto.ToString(CultureInfo.InvariantCulture);
            tempCpf = tempCpf + digito;
            soma = 0;
            for (var i = 0; i < 10; i++) soma += int.Parse(tempCpf[i].ToString(CultureInfo.InvariantCulture)) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;
            digito = digito + resto;
            return cpf.EndsWith(digito);
        }

        public static String zeroEsquerda(String value, Int32 tamString)
        {
            for (var i = value.Length; i < tamString; i++) value = "0" + value;
            return value;
        }

        public static string Cvv(char[] cocCrt)
        {
            char[] _cvv = new char[3];
            int[] s = new int[3] { 0, 0, 0 };

            for (int i = 1; i <= 16; ++i)
            {
                int ci = cocCrt[16 - i] - '0';
                for (int j = 0; j < 3; ++j)
                    s[j] += ci * (i + (j << 4));
            }

            for (int j = 0; j < 3; ++j)
            {
                int m = (j + 1) << 4;
                _cvv[2 - j] = Convert.ToChar((((s[j] % m) * 10) / m) + '0');
            }

            //_cvv[3] = Convert.ToChar(0);

            return new string(_cvv);
        }
        
        public static void GravarLog(Database db, DbTransaction dbt, string comando, OPERADORA operador, DbCommand cmdParameters)
        {
            var parametros = new StringBuilder();
            parametros.Append(comando);
            foreach (DbParameter param in cmdParameters.Parameters)
                parametros.Append("  --  " + param.ParameterName + " = " + param.Value);
            const string sql = "INSERT INTO TABLOGVA (DATA,CODOPE,NOMOPE,COMANDO) VALUES (@DATA,@CODOPE,@NOMOPE,@COMANDO)";
            var cmd = db.GetSqlStringCommand(sql);
            db.AddInParameter(cmd, "DATA", DbType.DateTime, DateTime.Now);
            db.AddInParameter(cmd, "CODOPE", DbType.Int16, operador.CODOPE);
            db.AddInParameter(cmd, "NOMOPE", DbType.String, operador.LOGIN);
            db.AddInParameter(cmd, "COMANDO", DbType.String, parametros.ToString());
            db.ExecuteNonQuery(cmd, dbt);
        }

        public static void GravarLog(string comando, OPERADORA operador, DbCommand cmdParameters)
        {
            var BDTELENET = string.Format(ConstantesSIL.BDTELENET, operador.SERVIDORNC, operador.BANCONC,
                ConstantesSIL.UsuarioBanco, ConstantesSIL.SenhaBanco);
            var db = new SqlDatabase(BDTELENET);
            var dbc = db.CreateConnection();
            dbc.Open();
            var dbt = dbc.BeginTransaction();
            try
            {
                GravarLog(db, dbt, comando, operador, cmdParameters);
                dbt.Commit();
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

        public static bool GerarArquivoLog(string pathArquivo, LOG dadosLog)
        {
            if (File.Exists(pathArquivo)) ExcluirArquivo(pathArquivo);
            StreamWriter arquivoLog = null;
            try
            {
                arquivoLog = new StreamWriter(pathArquivo);
                arquivoLog.WriteLine("---------------------------  ARQUIVO DE LOG - DATA:" + DateTime.Now.ToShortDateString() + "  --------------------------");
                arquivoLog.WriteLine();
                arquivoLog.WriteLine(" O arquivo foi processado e apresentou os seguintes resultados:");
                arquivoLog.WriteLine();

                foreach (var log in dadosLog.getArquivoLog())
                    arquivoLog.WriteLine("{0} {1} - {2}", log.ID, log.TIPO, log.DESCRICAOLOG.PadRight(120, ' '));

                return true;
            }
            finally { arquivoLog.Close(); }
        }

        public static bool GerarArquivoLogCartaoTemp(string pathArquivo, LOG dadosLog)
        {
            if (File.Exists(pathArquivo)) ExcluirArquivo(pathArquivo);
            StreamWriter arquivoLog = null;
            try
            {
                arquivoLog = new StreamWriter(pathArquivo);
                arquivoLog.WriteLine("---------- ARQUIVO DE LOG INCLUSAO DE CARTOES TEMPORARIOS - DATA: " + DateTime.Now.ToShortDateString() + " ----------");
                arquivoLog.WriteLine();

                foreach (var log in dadosLog.getArquivoLog())
                    arquivoLog.WriteLine("{0} {1} - {2}", log.ID, log.TIPO, log.DESCRICAOLOG.PadRight(120, ' '));

                return true;
            }
            finally { arquivoLog.Close(); }
        }

        public static bool GerarArquivoLogConfTrans(string rotulo, string pathArquivo, LOG dadosLog)
        {
            if (File.Exists(pathArquivo)) ExcluirArquivo(pathArquivo);
            StreamWriter arquivoLog = null;
            try
            {
                arquivoLog = new StreamWriter(pathArquivo);
                arquivoLog.WriteLine("---------- ARQUIVO DE LOG CONFIRMAÇÃO DE TRANSAÇÕES " + rotulo + " - DATA: " + DateTime.Now.ToShortDateString() + " ----------");
                arquivoLog.WriteLine();

                foreach (var log in dadosLog.getArquivoLog())
                    arquivoLog.WriteLine("{0} {1} - {2}", log.ID, log.TIPO, log.DESCRICAOLOG.PadRight(120, ' '));

                return true;
            }
            finally { arquivoLog.Close(); }
        }

        public static bool GerarArquivoLogInvTrans(string rotulo, string pathArquivo, LOG dadosLog)
        {
            if (File.Exists(pathArquivo)) ExcluirArquivo(pathArquivo);
            StreamWriter arquivoLog = null;
            try
            {
                arquivoLog = new StreamWriter(pathArquivo);
                arquivoLog.WriteLine("---------- ARQUIVO DE LOG INVALIDAÇÃO DE TRANSAÇÕES " + rotulo + " - DATA: " + DateTime.Now.ToShortDateString() + " ----------");
                arquivoLog.WriteLine();

                foreach (var log in dadosLog.getArquivoLog())
                    arquivoLog.WriteLine("{0} {1} - {2}", log.ID, log.TIPO, log.DESCRICAOLOG.PadRight(120, ' '));

                return true;
            }
            finally { arquivoLog.Close(); }
        }

        public static void ExcluirArquivo(string path)
        {
            if (File.Exists(path)) File.Delete(path);
        }

        public static string RemoverAcentos(string str)
        {
            /* Troca os caracteres acentuados por não acentuados */
            var acentos = new[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            var semAcento = new[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (var i = 0; i < acentos.Length; i++) { str = str.Replace(acentos[i], semAcento[i]); }
            /* Troca os caracteres especiais da string por "" */
            string[] caracteresEspeciais = { "'", "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", "#" };
            str = caracteresEspeciais.Aggregate(str, (current, t) => current.Replace(t, ""));
            /* Troca os espaços no início por "" */
            str = str.Replace("^\\s+", "");
            /* Troca os espaços no início por "" */
            str = str.Replace("\\s+$", "");
            /* Troca os espaços duplicados, tabulações e etc por  " " */
            str = str.Replace("\\s+", " ");
            return str;
        }

        public static string RemoverAcentosMenosHifem(string str)
        {
            /* Troca os caracteres acentuados por não acentuados */
            var acentos = new[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            var semAcento = new[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (var i = 0; i < acentos.Length; i++) { str = str.Replace(acentos[i], semAcento[i]); }
            /* Troca os caracteres especiais da string por "" */
            string[] caracteresEspeciais = { "'", "\\.", ",", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°", "#" };
            str = caracteresEspeciais.Aggregate(str, (current, t) => current.Replace(t, ""));
            /* Troca os espaços no início por "" */
            str = str.Replace("^\\s+", "");
            /* Troca os espaços no início por "" */
            str = str.Replace("\\s+$", "");
            /* Troca os espaços duplicados, tabulações e etc por  " " */
            str = str.Replace("\\s+", " ");
            return str;
        }

        public static string OperacaoCarga = "CARGA";

        public static void ValidaUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return;

            var engines = ConfigurationManager.AppSettings["enabledEngines"];

            if (string.IsNullOrEmpty(engines))
                return;

            if (!engines.Split(';').Any(e => userAgent.Contains(e)))
            {
                throw new UnauthorizedUserAgentException("Unauthorized user agent engine: " + userAgent);
            }        
        }        

        //Sessions Globais
        public const string RESULTADOCONSULTA = "RESULTADOCONSULTA";
        public const string DADOSPARACONSULTA = "DADOSPARACONSULTA";
        public const string ISRETORNORESULTCONSULTA = "ISRETORNORESULTCONSULTA";

        //Nome das Colunas Listagem de Transacoes
        public const string SUBREDE = "SUBREDE";
        public const string DATTRA = "DATTRA";
        public const string NSUHOS = "NSUHOS";
        public const string NSUAUT = "NSUAUT";
        public const string TIPTRA = "TIPTRA";
        public const string ORIGEMOP = "ORIGEMOP";
        public const string NOMOPERADOR = "NOMOPERADOR";
        public const string DESTIPTRA = "DESTIPTRA";
        public const string VALTRA = "VALTRA";
        public const string TVALOR = "TVALOR"; /* PJ */
        public const string PARCELA = "PARCELA"; /* PJ */
        public const string TPARCELA = "TPARCELA"; /* PJ */
        public const string NOMUSU = "NOMUSU";
        public const string CODCRT = "CODCRTMASK";
        public const string CPF = "CPF";
        public const string MAT = "MAT";
        public const string CODFIL = "CODFIL";
        public const string CODSET = "CODSET";
        public const string CODCLI = "CODCLI";
        public const string NOMCLI = "NOMCLI";
        public const string DATPGTOCLI = "DATPGTOCLI";
        public const string CODCRE = "CODCRE";
        public const string RAZSOC = "RAZSOC";
        public const string NOMFAN = "NOMFAN";
        public const string CGC = "CGC";
        public const string DATFECCLI = "DATFECCLI"; /* PJ */
        public const string NUMFECCLI = "NUMFECCLI"; /* PJ */
        public const string DATFECCRE = "DATFECCRE";
        public const string NUMFECCRE = "NUMFECCRE";
        public const string DATPGTOCRE = "DATPGTOCRE";
        public const string CODRTA = "CODRTA";
        public const string NUMCARG_VA = "NUMCARG_VA";
        public const string NUMDEP = "NUMDEP";
        public const string NOMSEG = "NOMSEG";
        public const string REDE = "REDE";
        public const string FLAGAUT = "FLAG_AUT";
        public const string DAD_JUST = "DAD_JUST";
        

        //Nome das Colunas Listagem de Transacoes

        public const string VALINF = "VALINF";
        public const string QTEINF = "QTEINF";
        public const string VALAFE = "VALAFE";
        public const string NUMBOR = "NUMBOR";
        public const string QTEAFE = "QTEAFE";
        public const string TAXADM = "TAXADM";
        public const string VALTAXA = "VALTAXA";
        public const string ANUIDADE = "ANUIDADE";
        public const string VALLIQ = "VALLIQ";
        public const string DTINIFECH = "DTINIFECH";
        public const string DTFIMFECH = "DTFIMFECH";
        public const string NUMFECH = "NUMFECH";
        public const string DATFECLOT = "DATFECLOT";
        public const string PRAZO = "PRAZO";
        public const string DATBOR = "DATBOR";
        public const string CONTA = "CONTA";
        public const string AGENCIA = "AGENCIA";
        public const string BANCO = "BANCO";
    }

    public class Sort<T> : IComparer<T>
    {
        public Sort(string campoOrdenacao)
        {
            //We must have a property name for this comparer to work
            CAMPO_ORDENACAO = campoOrdenacao;
        }

        #region PROPRIEDADE

        public string CAMPO_ORDENACAO { get; set; }

        #endregion

        #region IComparer<T> Members

        public int Compare(T x, T y)
        {
            var t = x.GetType();
            var val = t.GetProperty(CAMPO_ORDENACAO);
            if (val != null)
            {
                return Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
            }
            throw new Exception(CAMPO_ORDENACAO + " is not a valid property to sort on.  It doesn't exist in the Class.");
        }

        #endregion IComparer<T> Members

    }

    public enum SqlOperators
    {
        Greater,
        Less,
        Equal,
        StartsLike,
        EndsLike,
        Like,
        NotLike,
        LessOrEqual,
        GreaterOrEqual,
        NotEqual,
        In,
        NotIn
    }

    public interface IFilter { string FilterString { get; } }

    public class Filter : IFilter
    {
        private readonly string m_strFilterName;
        private readonly SqlOperators m_sqlOperator;
        private string m_strFilterValue;

        public Filter(string strFilterName, SqlOperators sqlOperator, string strFilterValue)
        {
            m_strFilterName = strFilterName;
            m_sqlOperator = sqlOperator;
            m_strFilterValue = strFilterValue;
        }
        public string FilterString
        {
            get
            {
                string strFilter;
                m_strFilterValue = m_strFilterValue.Replace("'", "''");
                switch (m_sqlOperator)
                {
                    case SqlOperators.Greater: strFilter = m_strFilterName + " > '" + m_strFilterValue + "'"; break;
                    case SqlOperators.Less: strFilter = m_strFilterName + " < '" + m_strFilterValue + "'"; break;
                    case SqlOperators.Equal: strFilter = m_strFilterName + " = '" + m_strFilterValue + "'"; break;
                    case SqlOperators.LessOrEqual: strFilter = m_strFilterName + " <= '" + m_strFilterValue + "'"; break;
                    case SqlOperators.GreaterOrEqual: strFilter = m_strFilterName + " >= '" + m_strFilterValue + "'"; break;
                    case SqlOperators.NotEqual: strFilter = m_strFilterName + " <> '" + m_strFilterValue + "'"; break;
                    case SqlOperators.StartsLike: strFilter = m_strFilterName + " LIKE '" + m_strFilterValue + "%'"; break;
                    case SqlOperators.EndsLike: strFilter = m_strFilterName + " LIKE '%" + m_strFilterValue + "'"; break;
                    case SqlOperators.Like: strFilter = m_strFilterName + " LIKE '%" + m_strFilterValue + "%'"; break;
                    case SqlOperators.NotLike: strFilter = m_strFilterName + " NOT LIKE '" + m_strFilterValue + "'"; break;
                    case SqlOperators.In: strFilter = m_strFilterName + " IN '" + m_strFilterValue + "'"; break;
                    case SqlOperators.NotIn: strFilter = m_strFilterName + " NOT IN '" + m_strFilterValue + "'"; break;
                    default: throw new Exception("This operator type is not supported");
                }
                return strFilter;
            }
        }

    }

    public class INFilter : IFilter
    {
        private readonly string m_strFilterName;
        private readonly StringCollection m_strColFilterValues;
        public INFilter(string strFilterName, StringCollection strColValues)
        {
            m_strFilterName = strFilterName;
            m_strColFilterValues = strColValues;
        }

        public string FilterString
        {
            get
            {
                var strFilter = "";
                if (m_strColFilterValues.Count > 0)
                {
                    for (var i = 0; i < m_strColFilterValues.Count - 1; i++)
                    {
                        strFilter += "'" + m_strColFilterValues[i] + "'" + ",";
                    }
                    strFilter += "'" + m_strColFilterValues[m_strColFilterValues.Count - 1] + "'";
                    strFilter = m_strFilterName + " IN(" + strFilter + ")";
                }
                return strFilter;
            }
        }
    }

    public class ANDFilter : IFilter
    {
        private readonly FilterExpressionList m_filterExpressionList = new FilterExpressionList();
        public ANDFilter(IFilter filterExpressionLeft, IFilter filterExpressionRight)
        {
            m_filterExpressionList.Add(filterExpressionLeft);
            m_filterExpressionList.Add(filterExpressionRight);
        }

        public ANDFilter(FilterExpressionList filterExpressionList)
        {
            m_filterExpressionList = filterExpressionList;
        }

        public string FilterString
        {
            get
            {
                var strFilter = "";
                if (m_filterExpressionList.Count > 0)
                {
                    for (var i = 0; i < m_filterExpressionList.Count - 1; i++)
                    {
                        if (m_filterExpressionList[i] != null)
                        {
                            strFilter += m_filterExpressionList[i].FilterString + " AND ";
                        }
                    }
                    strFilter += m_filterExpressionList[m_filterExpressionList.Count - 1].FilterString;
                    strFilter = "(" + strFilter + ")";
                }
                return strFilter;
            }
        }
    }

    public class ORFilter : IFilter
    {
        private readonly FilterExpressionList m_filterExpressionList = new FilterExpressionList();
        public ORFilter(IFilter filterExpressionLeft, IFilter filterExpressionRight)
        {
            m_filterExpressionList.Add(filterExpressionLeft);
            m_filterExpressionList.Add(filterExpressionRight);
        }

        public ORFilter(FilterExpressionList filterExpressionList)
        {
            m_filterExpressionList = filterExpressionList;
        }

        public string FilterString
        {
            get
            {
                var strFilter = "";
                if (m_filterExpressionList.Count > 0)
                {
                    for (var i = 0; i < m_filterExpressionList.Count - 1; i++)
                    {
                        strFilter += m_filterExpressionList[i].FilterString + " OR ";
                    }
                    strFilter += m_filterExpressionList[m_filterExpressionList.Count - 1].FilterString;
                    strFilter = "(" + strFilter + ")";
                }
                return strFilter;
            }
        }
    }

    public class FilterExpressionList : IEnumerable, IEnumerator
    {
        readonly ArrayList alItems;
        readonly IEnumerator ienum;
        public FilterExpressionList() { alItems = new ArrayList(); ienum = alItems.GetEnumerator(); }
        public IEnumerator GetEnumerator() { return this; }
        public IFilter Current { get { return (IFilter)ienum.Current; } }
        object IEnumerator.Current { get { return ienum.Current; } }
        public void Reset() { ienum.Reset(); }
        public bool MoveNext() { return ienum.MoveNext(); }
        public void Add(IFilter filterExpresion) { alItems.Add(filterExpresion); }
        public IFilter this[int index] { get { return (IFilter)alItems[index]; } set { alItems[index] = value; } }
        public int Count { get { return alItems.Count; } }
    }
}


