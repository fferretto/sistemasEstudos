using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace APIGeracaoBoleto.Processamento
{
    public class Util
    {
        public static string filtraBanco(string valor)
        {
            var Banco = valor.Substring(0, 4);

            return Banco;
        }
        public static string filtraAgencia(string valor)
        {
            var ag = valor.Substring(5, 5);


            return ag;
        }
        public static string filtraDigAgencia(string valor)
        {
            string dg = "0";

            var ag = valor.Substring(5, 6);

            if (ag.Contains("-"))
            {
                var novaAg = ag.Split('-');
                dg = novaAg[1].Trim();
            }
            else
            {
                dg = valor.Substring(10, 1);
            }

            return dg;
        }
        public static string filtraConta(string valor)
        {
            string conta = "0";
            //valor = RemoveCaracteres(valor);

            conta = valor.Substring(12, valor.Length - 12);


            if (conta.Contains("-"))
            {
                var novaconta = conta.Split('-');
                conta = novaconta[0].Trim();
            }
            else
            {
                conta = conta.Substring(0, conta.Length - 1);
            }


            return conta;
        }
        public static string filtraDigConta(string valor)
        {
            string dg = "0";
            var conta = valor.Substring(12, valor.Length - 12);

            if (conta.Contains("-"))
            {
                var novaconta = conta.Split('-');

                dg = TratadigitoConta(novaconta[1].Trim());

            }
            else
            {
                dg = conta.Substring(conta.Length - 1, 1);
            }


            return dg;
        }
        public static string FormataCPFCnPj(string val)
        {
            string valor = val.Replace("-", "").Replace("/", "").Replace(" ", "").Replace("\\", "").Replace(".", "");

            if (valor.Length == 11)
                valor = Convert.ToUInt64(valor).ToString(@"000\.000\.000\-00");
            else if (valor.Length == 14)
                valor = Convert.ToUInt64(valor).ToString(@"00\.000\.000\/0000\-00");

            return valor;

        }
        public static string RemoveCaracteres(string val)
        {
            string valor = val.Replace("-", "").Replace("/", "").Replace(" ", "").Replace("\\", "").Replace(".", "").Replace(",", "");
            return valor;
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
        public static string RemoveEspaco(string Valor)
        {
            string Ret = Valor;

            while (Ret.IndexOf(" ") >= 0)
                Ret = Ret.Replace(" ", "");

            return Ret;
        }
        public static DateTime TrataData(string valor)
        {
            try
            {
                string retorno = valor.ToString();

                if (retorno.Trim() != "")
                {
                    return System.DateTime.Parse(retorno);
                }
                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static decimal TrataDecimal(string valor)
        {
            try
            {
                string retorno = valor.Trim();

                if (retorno.Trim() != "")
                {
                    return Convert.ToDecimal(retorno);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int TrataInteiro(string valor)
        {
            try
            {
                string retorno = valor.Trim();

                if (retorno == null)
                    retorno = "0";

                if (retorno.Trim() != "")
                {
                    return Convert.ToInt32(retorno);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string TratadigitoConta(string valor)
        {
            try
            {
                string retorno = valor.Trim();

                if (retorno == null)
                    retorno = "0";

                if (retorno.Trim() != "")
                {
                    return (Convert.ToInt32(retorno)).ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                return valor;
            }
        }
        public static string TrataTexto(string valor)
        {
            try
            {
                string retorno = valor.Trim();

                if (retorno == null)
                    retorno = "";

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string RemoveCaracterDireita(string valor, int qt)
        {
            try
            {
                if (valor == null) return "";

                string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");

                if (retorno.Length > qt)
                {
                    retorno = retorno.Substring(0, qt);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string RemoveCaracterEsquerda(string valor, int qt)
        {
            try
            {
                string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");

                if (retorno.Length > qt)
                {
                    int posicao1 = retorno.Length - qt;
                    int PosicaoFim = retorno.Length;

                    retorno = retorno.Substring(posicao1, qt);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string FormataTexto(string valor, int qtCasas)
        {
            string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");

            if (retorno.Length > qtCasas)
            {
                retorno = retorno.Substring(0, qtCasas);
            }

            for (int i = retorno.Length; i < qtCasas; i++)
            {
                retorno = retorno + " ";
            }
            return retorno;
        }

        public static string GeraNumeroControle(int CodOpe, string codTransacaoPagamento, string NumControle)
        {
            string Retorno = "";
            if (NumControle == "")
            {
                Retorno = CodOpe.ToString() + codTransacaoPagamento;
            }
            else
                Retorno = NumControle;

            return Retorno;
        }

        public static string GeraNossoNumero(string NossoNumero, int codTransacao)
        {
            try
            {
                NossoNumero = NossoNumero.Trim();


                if (NossoNumero == "0" || NossoNumero == "")
                {
                    NossoNumero = codTransacao.ToString() + DateTime.Now.ToString("hhmmss");
                }

                //Calculo utilizado para localizar o dígito do nosso número
                int multiplicador = 1;
                string CalcNossoNumero = RemoveCaracterDireita(NossoNumero, 7);
                int valor1 = 0;
                int valor2 = 0;

                int posicaoIni = 0;

                for (int i = 0; i < CalcNossoNumero.Length; i++)
                {
                    posicaoIni = CalcNossoNumero.Length - 1;

                    valor1 = Convert.ToInt32(CalcNossoNumero.Substring(posicaoIni, 1));
                    CalcNossoNumero = CalcNossoNumero.Remove(CalcNossoNumero.Length - 1);

                    if (multiplicador > 9)
                        multiplicador = 2;
                    else
                        multiplicador += 1;

                    valor2 += valor1 * multiplicador;
                }
                int RestoDivisao = valor2 % 11;
                NossoNumero += "-" + (11 - RestoDivisao);


                return NossoNumero;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string ValidaDiretorioRemessa(string CaminhoOrigem)
        {
            string CaminhoDestino = "";

            if (CaminhoOrigem.EndsWith("\\"))
                CaminhoDestino = CaminhoOrigem + "RemessaPagamento\\";
            else
                CaminhoDestino = CaminhoOrigem + "\\RemessaPagamento\\";

            if (!Directory.Exists(CaminhoDestino))
            {
                //Criamos um com o nome folder
                Directory.CreateDirectory(CaminhoDestino);
            }

            return CaminhoDestino;


        }
        public static string RemoveCaracterEspecial(string info)
        {
            string pattern = @"(?i)[^0-9a-záéíóúàèìòùâêîôûãõç\s]";
            string replacement = "";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(info, replacement);

            return result;
        }
        public static string FormataCodigoBarraPagamento(string Codigo)
        {
            string str = "";
            Codigo = Codigo.Replace("-", "").Replace(".", "");
            while (Codigo.IndexOf(" ") >= 0)
                Codigo = Codigo.Replace(" ", "").Replace(".", "");

            string A = Codigo.Substring(0, 3);
            string B = Codigo.Substring(3, 1);
            string C_1 = Codigo.Substring(4, 5);
            string C_2 = Codigo.Substring(10, 10);
            string C_3 = Codigo.Substring(21, 10);
            string D = Codigo.Substring(32, 1);
            string E = Codigo.Substring(33, 14);

            str = A + B + D + E + C_1 + C_2 + C_3;

            return str;
        }
        public static string FormataCodigoBarraPagConcessionario(string Codigo)
        {
            string str = "";

            while (Codigo.IndexOf(" ") >= 0)
                Codigo = Codigo.Replace(" ", "").Replace("-", "").Replace(".", "");

            string A = Codigo.Substring(0, 11);
            string B = Codigo.Substring(12, 11);
            string C = Codigo.Substring(24, 11);
            string D = Codigo.Substring(36, 11);

            str = A + B + C + D;

            return str;
        }
        /// <summary>
        /// Se todos os caracters forem digitos então é numerico
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsNumeric(string data)
        {
            bool isnumeric = false;
            char[] datachars = data.ToCharArray();

            foreach (var datachar in datachars)
                isnumeric = isnumeric ? char.IsDigit(datachar) : isnumeric;


            return isnumeric;
        }

    }
}
