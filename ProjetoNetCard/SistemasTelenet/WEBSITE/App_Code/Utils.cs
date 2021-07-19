using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using System.Configuration;
using SIL;
using System.Collections.Generic;

/// <summary>
/// Metodos Comuns
/// </summary>
public static class Util
{
    private static IDictionary<string, Exception> _unexpectedExceptions = new Dictionary<string, Exception>();
    private static object _synchronizer = new object();

    public const string ASPNETSessionId = "ASP.NET_SessionId";

    public static void AddUnexpectedException(string sessionId, Exception exception)
    {
        //lock (_synchronizer)
        //{
        //    if (_unexpectedExceptions.ContainsKey(sessionId))
        //    {
        //        _unexpectedExceptions[sessionId] = exception;
        //    }
        //    else
        //    {
        //        _unexpectedExceptions.Add(sessionId, exception);
        //    }
        //}
    }

    public static void RemoveUnexpectedException(string sessionId)
    {
        lock (_synchronizer)
        {
            if (_unexpectedExceptions.ContainsKey(sessionId))
            {
                _unexpectedExceptions.Remove(sessionId);
            }
        }
    }

    public static Exception GetUnexpectedException(string sessionId)
    {
        if (sessionId != null)
        {
            if (_unexpectedExceptions.ContainsKey(sessionId))
            {
                return _unexpectedExceptions[sessionId];
            }
        }

        return null;
    }

    public static string IncluirFuncaoJS(string sentenca, string funcao)
    {
        var sentencaFinal = sentenca;

        // Verifica se Funcao Pertence a Sentenca
        if (sentencaFinal.IndexOf(funcao, StringComparison.Ordinal) == -1)
        {
            if (sentencaFinal.IndexOf("function(s, e) {", StringComparison.Ordinal) == -1)
                sentencaFinal = "function(s, e) {" + sentencaFinal;

            var sb = new StringBuilder(sentencaFinal);
            if (sentenca.Length > 0)
                sb.Remove(sentencaFinal.Length - 1, 1);
            //SentencaFinal = SentencaFinal.Replace("}");

            sentencaFinal = sb + funcao + ';';
            sentencaFinal += "}";
        }
        return sentencaFinal;
    }

    public static Boolean compactaArquivo(string[] arquivos, string arquivoZip, Boolean apagarArquivos)
    {
        var zipOutPut = new ZipOutputStream(File.Create(arquivoZip));
        //Compactacao level 9
        zipOutPut.SetLevel(9);
        zipOutPut.Finish();
        zipOutPut.Close();

        var zip = new ZipFile(arquivoZip);
        //Inicia a criacao do ZIP
        zip.BeginUpdate();

        foreach (var arquivo in arquivos)
        {
            //Adicionando arquivos previamente criados ao zipFile
            var nomeZip = arquivo;
            zip.NameTransform = new ZipNameTransform(nomeZip.Substring(0, nomeZip.LastIndexOf(@"\", StringComparison.Ordinal)));
            zip.Add(nomeZip);
        }

        zip.CommitUpdate();
        zip.Close();

        //Apaga os arquivos que foram zipados.
        if (apagarArquivos)
            foreach (var arquivo in arquivos)
                File.Delete(arquivo);

        return true;
    }

    /// <summary>
    /// Retorna a chave Publica para criptografia dos dados
    /// </summary>
    /// <returns></returns>
    public static string getRijndaelKey()
    {
        return ConfigurationManager.AppSettings["PublicKey"];
    }
    public static string filtraBanco(string valor)
    {
        if (string.IsNullOrEmpty(valor)) return "0";

        var Banco = valor.Substring(0, 4);

        return Banco;
    }
    public static string filtraAgencia(string valor)
    {

        if (string.IsNullOrEmpty(valor)) return "0";

        if (valor.Length <= 7)
        {
            var ag = valor.Substring(0, 5);
            ag = ag.Trim().Replace("-", "");

            return ag;
        }
        else
        {
            var ag = valor.Substring(5, 5);
            ag = ag.Trim().Replace("-", "");

            return ag;
        }
    }
    public static string filtraDigAgencia(string valor)
    {
        string dg = "0";

        if (valor.Length <= 7 && valor.Trim().Length > 0)
        {
            if (valor.Contains("-"))
            {
                var novaAg = valor.Split('-');
                dg = novaAg[1].Trim();
            }
            else
            {
                dg = valor.Substring(5, 1);
            }
        }
        else
        {
            var ag = valor.Substring(5, 6);

            if (dg.Trim().Length > 0)
            {
                if (ag.Contains("-"))
                {
                    var novaAg = ag.Split('-');
                    dg = novaAg[1].Trim();
                }
                else
                {
                    dg = valor.Substring(10, 1);
                }
            }
        }


        if (dg.Trim() == "") dg = "0";

        return dg;
    }
    public static string filtraConta(string valor)
    {
        string conta = "0";

        if (valor.Length <= 13 && valor.Trim().Length > 0)
        {
            if (valor.Contains("-"))
            {
                var novaconta = valor.Split('-');
                conta = novaconta[0].Trim();
            }
            else
            {
                conta = valor.Substring(0, conta.Length - 1);
            }
        }
        else
        {
            conta = valor.Substring(12, valor.Length - 12);

            if (conta.Trim().Length > 0)
            {
                if (conta.Contains("-"))
                {
                    var novaconta = conta.Split('-');
                    conta = novaconta[0].Trim();
                }
                else
                {
                    conta = conta.Substring(0, conta.Length - 1);
                }
            }
        }


        return conta;
    }
    public static string filtraDigConta(string valor)
    {
        string dg = "0";

        if (valor.Trim().Length > 0)
        {
            dg = valor.Substring(valor.Length - 1, 1);
        }

        return dg;
    }
    public static string FormataCTABCO(string Banco, string Agencia, string dgAgencia, string Conta, string dgConta)
    {
        Banco = FormataInteiro(Banco, 4);
        Agencia = FormataInteiro(Agencia, 5);
        dgAgencia = FormataInteiro(dgAgencia, 1);
        Conta = FormataInteiro(Conta, 9);
        dgConta = FormataInteiro(dgConta, 1);

        string RetCTABCO = Banco + "-" + Agencia + dgAgencia + "-" + Conta + dgConta;

        return RetCTABCO;
    }
    public static string FormataInteiro(string valor, int qtCasas)
    {
        try
        {
            string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");

            if (retorno.Length > qtCasas)
            {
                retorno = retorno.Substring(0, qtCasas);
            }

            for (int i = retorno.Length; i < qtCasas; i++)
            {
                retorno = "0" + retorno;
            }
            return retorno;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}