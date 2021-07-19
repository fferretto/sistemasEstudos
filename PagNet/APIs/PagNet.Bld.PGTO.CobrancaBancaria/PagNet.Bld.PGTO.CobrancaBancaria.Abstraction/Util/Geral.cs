using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Util
{
    public static class Geral
    {
        public static string FormataCPFCnPj(string val)
        {
            if (val == null)
                return "";

            val = val ?? "";
            if (val.Length == 13) val = "0" + val;
            string valor = val.Replace("-", "").Replace("/", "").Replace(" ", "").Replace("\\", "").Replace(".", "");

            if (valor.Length == 11)
                valor = Convert.ToUInt64(valor).ToString(@"000\.000\.000\-00");
            else if (valor.Length == 14)
                valor = Convert.ToUInt64(valor).ToString(@"00\.000\.000\/0000\-00");

            return valor;

        }
        public static string FormataCEP(string val)
        {
            val = val ?? "";
            string valor = val.Replace("-", "").Replace("/", "").Replace(" ", "").Replace("\\", "").Replace(".", "");

            if (valor.Length == 8)
                valor = Convert.ToUInt64(valor).ToString(@"00000\-000");

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
                if (valor == "")
                    valor = "0";

                string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");

                retorno = retorno.Trim();
                retorno = (Convert.ToInt64(retorno)).ToString();

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
            Valor = Valor ?? "";
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
                valor = valor.Replace("R$", "");

                valor = valor ?? "";
                string retorno = valor.Trim();

                if (retorno.Trim() != "")
                {
                    var auxasd = Convert.ToDecimal(retorno);
                    //var asd = retorno("C", CultureInfo.CurrentCulture);
                    return Convert.ToDecimal(retorno);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string TrataValorMonetario(decimal valor)
        {
            string novoValor = valor.ToString();
            novoValor = novoValor.Replace(",", ".");
            if (novoValor.LastIndexOf(".") < 0)
            {
                novoValor = novoValor + ".00";
            }
            return novoValor;
        }
        public static string TrataValorMonetario(string valor)
        {
            string valorFinal = "0,00";
            valor = valor ?? "";

            valor = valor.Replace("R$", "");
            string retorno = valor.Trim();

            if (valor.LastIndexOf(".") > 0)
            {
                var tratamento = valor.Split('.');
                if (tratamento.Length > 0)
                {
                    var centavos = tratamento[(tratamento.Length - 1)];
                    if (centavos.Length == 1)
                    {
                        var valorReal = valor.Substring(0, valor.Length - centavos.Length);
                        valor = valorReal + centavos + "0";
                    }
                }
                valor = valor.Replace(",", "");
                valor = valor.Replace(".", "");
                var val1 = valor.Substring(0, valor.Length - 2);
                var val2 = valor.Substring(valor.Length - 2, 2);
                valorFinal = val1 + "," + val2;
            }
            else
            {
                if (valor.LastIndexOf(",") < 0)
                {
                    valorFinal = valor + ",00";
                }
                else
                {
                    valorFinal = valor;
                }
            }
            return valorFinal;
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
            valor = valor ?? "";
            string retorno = valor.Trim().Replace(".", "").Replace("-", "").Replace(",", "");
            retorno = retorno.Trim();
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

        public static string GeraNumeroControle(int CodEmissaoBoleto, string NumControle)
        {
            string Retorno = "";
            if (NumControle == "" || NumControle == "0" || NumControle == null)
            {
                Retorno = CodEmissaoBoleto.ToString() + DateTime.Now.ToString("ddMMhhss");
            }
            else
                Retorno = NumControle;

            return Retorno;
        }
        public static string GeraSeuNumero(int CodEmissaoBoleto, string SeuNumero)
        {
            string Retorno = "";
            if (SeuNumero == "" || SeuNumero == "0" || SeuNumero == null)
            {
                Retorno = CodEmissaoBoleto.ToString() + DateTime.Now.ToString("ddMMhhss");
            }
            else
                Retorno = SeuNumero;

            return Retorno;
        }

        public static string GeraNossoNumero(string NossoNumero, int codTitulo)
        {
            try
            {
                NossoNumero = (string.IsNullOrWhiteSpace(NossoNumero)) ? "" : NossoNumero;
                NossoNumero = NossoNumero.Trim();


                if (NossoNumero == "0" || NossoNumero == "")
                {
                    NossoNumero = codTitulo.ToString() + DateTime.Now.ToString("mmhhsss");


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
                }

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
        public static string RemoveCaracterEspecial(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
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
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable dt = new DataTable();
            dt.Clear();


            dt.Columns.Add("Name");
            dt.Columns.Add("Marks");


            DataRow _ravi = dt.NewRow();
            _ravi["Name"] = "ravi";
            _ravi["Marks"] = "500";
            dt.Rows.Add(_ravi);

            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        public static string OcorrenciaBoletoCnab240(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "REGISTRADO"; // "Solicitação de Impressão de Títulos Confirmada";
                case "02":
                    return "REGISTRADO"; //"Entrada Confirmada";
                case "03":
                    return "RECUSADO"; // "Entrada Rejeitada";
                case "04":
                    return "LIQUIDADO"; // "Transferência de Carteira/Entrada";
                case "05":
                    return "LIQUIDADO"; // "Transferência de Carteira/Baixa";
                case "06":
                    return "LIQUIDADO"; // "Liquidação";
                case "07":
                    return "REGISTRADO"; //"Confirmação do Recebimento da Instrução de Desconto";
                case "08":
                    return "REGISTRADO"; //"Confirmação do Recebimento do Cancelamento do Desconto";
                case "09":
                    return "BAIXADO"; // "Baixa";
                case "12":
                    return "Confirmação Recebimento Instrução de Abatimento";
                case "13":
                    return "Confirmação Recebimento Instrução de Cancelamento Abatimento";
                case "14":
                    return "Confirmação Recebimento Instrução Alteração de Vencimento";
                case "17":
                    return "Liquidação Após Baixa ou Liquidação Título Não Registrado";
                case "19":
                    return "Confirmação Recebimento Instrução de Protesto";
                case "20":
                    return "Confirmação Recebimento Instrução Sustação de Protesto";
                case "23":
                    return "Remessa a Cartório";
                case "24":
                    return "Retirada de Cartório";
                case "25":
                    return "Protestado e Baixado (Baixa por Ter Sido Protestado)";
                case "26":
                    return "Instrução Rejeitada";
                case "27":
                    return "Confrmação do Pedido de Alteração de Outros Dados";
                case "28":
                    return "Débito de Tarifas/Custas";
                case "30":
                    return "Alteração de Dados Rejeitada";
                case "33":
                    return "Confirmação da Alteração dos Dados do Rateio de Crédito";
                case "34":
                    return "Confirmação do Cancelamento dos Dados do Rateio de Crédito";
                case "35":
                    return "Confirmação de Inclusão Banco de Pagador";
                case "36":
                    return "Confirmação de Alteração Banco de Pagador";
                case "37":
                    return "Confirmação de Exclusão Banco de Pagador";
                case "38":
                    return "Emissão de Boletos de Banco de Pagador";
                case "39":
                    return "Manutenção de Pagador Rejeitada";
                case "40":
                    return "Entrada de Título via Banco de Pagador Rejeitada";
                case "41":
                    return "Manutenção de Banco de Pagador Rejeitada";
                case "44":
                    return "Estorno de Baixa / Liquidação";
                case "45":
                    return "Alteração de Dados";
                default:
                    return "";
            }
        }
        public static string OcorrenciaRetLiquidacaoBoleto(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "Por saldo";
                case "02":
                    return "Por conta";
                case "03":
                    return "No próprio banco";
                case "04":
                    return "Compensação eletrônica";
                case "05":
                    return "Compensação convencional";
                case "06":
                    return "Arquivo magnético ";
                case "07":
                    return "Após feriado local ";
                case "08":
                    return "Em cartório";
                case "09":
                    return "Pagamento Parcial";
                default:
                    return "";
            }
        }
        public static string OcorrenciaRetBaixaBoleto(string codigo)
        {
            switch (codigo)
            {
                case "09":
                    return "Comandada banco";
                case "10":
                    return "Comandada cliente arquivo";
                case "11":
                    return "Comandada cliente on-line";
                case "12":
                    return "Decurso prazo–cliente";
                case "13":
                    return "Decurso prazo–banco";
                default:
                    return "";
            }
        }
        public static string ValidaTituloPendenteVencido(DateTime dtTitulo, string Status)
        {
            string TituloVencido = "N";

            if (dtTitulo < DateTime.Today)
            {
                if (Status != "BAIXADO" && Status != "BAIXADO_MANUALMENTE" && Status != "AGUARDANDO_ARQUIVO_RETORNO")
                    TituloVencido = "S";
            }

            return TituloVencido;
        }


        public static System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
        public static string RetornaDadosContaFavorecido(string conta, string digito)
        {
            string contacorrente = "";
            int contaAux = 0;

            if (conta.Length > 12)
            {
                contaAux = Convert.ToInt32(conta.Substring(6, 12));
                contacorrente = contaAux.ToString() + conta.Substring(19, 1);
            }
            else
            {
                contaAux = Convert.ToInt32(conta);
                contacorrente = contaAux.ToString() + digito;
            }

            return contacorrente;
        }
        public static string TipoSegmentoBoletoArrecadacao(string codigoSegmento)
        {
            string TipoSegumento = "";
            switch (codigoSegmento)
            {
                case "1":
                    TipoSegumento = "Prefeituras";
                    break;
                case "2":
                    TipoSegumento = "Saneamento";
                    break;
                case "3":
                    TipoSegumento = "Energia Elétrica e Gás";
                    break;
                case "4":
                    TipoSegumento = "Telecomunicações";
                    break;
                case "5":
                    TipoSegumento = "Órgãos Governamentais";
                    break;
                case "6":
                    TipoSegumento = "Carnes e Assemelhados ou demais Empresas";
                    break;
                case "7":
                    TipoSegumento = "Multas de trânsito";
                    break;
                default:
                    TipoSegumento = "";
                    break;
            }

            return TipoSegumento;

        }
    }
}
