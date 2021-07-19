using NetCard.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Yogesh.ExcelXml;

namespace NetCard.Common.Util
{
    public class Utils
    {
        public const string UsuarioBanco = "TLNUSUMW";

        public static string GetConnectionStringConcentrador(IObjetoConexao objConn)
        {
            var bdServidor = objConn.ServConcentrador;
            var banco = objConn.BancoConcentrador;
            const string senha = "TLN22BH22";
            return string.Format("Data Source={0};Initial Catalog={1};Timeout=60;Persist Security Info=True;User ID={2};Password={3}", bdServidor, banco, UsuarioBanco, senha);
        }

        public static string GetConnectionStringNerCard(IObjetoConexao objConn)
        {            
            var bdServidor = objConn.ServNertCard;
            var banco = objConn.BancoNetcard;
            const string senha = "TLN22BH22";
            return string.Format("Data Source={0};Initial Catalog={1};Timeout=60;Persist Security Info=True;User ID={2};Password={3}", bdServidor, banco, UsuarioBanco, senha);
        }

        public static string GetConnectionStringAutorizador(IObjetoConexao objConn)
        {
            var bdServidor = objConn.ServAutorizador; ;
            var banco = objConn.BancoAutorizador;
            const string senha = "TLN22BH22";
            return string.Format("Data Source={0};Initial Catalog={1};Timeout=60;Persist Security Info=True;User ID={2};Password={3}", bdServidor, banco, UsuarioBanco, senha);
        }

        public static string RemoverAcentos(string str)
        {
            /** Troca os caracteres acentuados por não acentuados **/
            var acentos = new[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            var semAcento = new[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };
            for (var i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }
            /** Troca os caracteres especiais da string por "" **/
            string[] caracteresEspeciais = { "\\.", ",", "-", ":", "\\(", "\\)", "ª", "\\|", "\\\\", "°" };
            str = caracteresEspeciais.Aggregate(str, (current, t) => current.Replace(t, ""));
            /** Troca os espaços no início por "" **/
            str = str.Replace("^\\s+", "");
            /** Troca os espaços no início por "" **/
            str = str.Replace("\\s+$", "");
            /** Troca os espaços duplicados, tabulações e etc por  " " **/
            str = str.Replace("\\s+", " ");
            return str;
        }

        public static string RetirarCaracteres(string Caracteres, string Str)
        {
            return Str != null ? Caracteres.Aggregate(Str, (current, t) => current.Replace(Convert.ToString(t), string.Empty)) : string.Empty;
        }

        public static string MascaraCartao(string codcrt, int tamanho)
        {
            var tam = codcrt.Length;
            tamanho = tamanho <= 10 ? 15 : tamanho;
            var codCrtMask = codcrt.Replace(codcrt.Substring(6, tam < 13 ? tam - 6 : 7), new string('*', tamanho - 10));
            return codCrtMask;
        }

        public static string GetStatus(string status)
        {
            switch (status)
                {
                    case "00":
                        return "ATIVO";
                    case "01":
                        return "BLOQUEADO";
                    case "02":
                        return "CANCELADO";
                    case "06":
                        return "SUSPENSO";
                    case "07":
                        return "TRANSFERIDO";
                    case "09":
                        return "EM RESCISÃO";
                    default:
                        return "STATUS N ENCONTRADO";
                }
        }
        public static bool ValidaData(string Data)
        {
            DateTime resultado = DateTime.MinValue; //Retorna se a data é válida 
            if (DateTime.TryParse(Data, out resultado)) //Testa a data em questão 
                return true; //Caso seja válida 

            return false;
        }

        public static bool ValidarCpf(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            var cpfsInvalidos = new string[10] {"00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666",
                                                     "77777777777", "88888888888", "99999999999"};

            if (cpfsInvalidos.Contains<string>(cpf))
                return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            var digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto;

            return cpf.EndsWith(digito);
        }

        public static DbType ValidaParametro(string tipoPar)
        {
            var retorno = new DbType();
            switch (tipoPar)
            {
                case "String":
                    retorno = DbType.String;
                    break;
                case "Byte":
                    retorno = DbType.Byte;
                    break;
            }
            return retorno;
        }

        public static List<ItensGenerico> MontaColunasDataTable(DataSet ds)
        {
            if (ds.Tables.Count <= 0) return null;
            var colunas = new List<ItensGenerico>();
            var dt = ds.Tables[0];
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                if (!colunas.Contains(new ItensGenerico(dt.Columns[i].ToString(), dt.Columns[i].ToString(), 100)))
                    colunas.Add(new ItensGenerico(dt.Columns[i].ToString(), dt.Columns[i].ToString(), 100));
            }

            return colunas;
        }

        public static string MontaExcelModulo(IList<ItensGenerico> colunasVisualizadas, string pathArquivo, DataSet ds)
        {
            if (colunasVisualizadas == null)
                return string.Empty;

            if (ds.Tables.Count <= 0) return string.Empty;
            var dt = ds.Tables[0];

            var excel = new ExcelXmlWorkbook { Properties = { Author = "Telenet NetCard" } };
            var planilha = excel[0];
            planilha.Name = "Resultado";

            if (colunasVisualizadas.Any())
            {
                for (var i = 0; i < colunasVisualizadas.Count(); i++)
                {
                    //Criei o cabecalho da coluna
                    planilha[i, 0].Value = colunasVisualizadas[i].Text;
                    var style = new XmlStyle
                    {
                        Interior = { Color = Color.LightGray },
                        Font = { Bold = true, Name = "Calibri", Size = 11 }
                    };
                    planilha[i, 0].Style = style;
                    planilha.Columns(i).Width = colunasVisualizadas[i].TamanhoColuna;

                    for (var j = 0; j < dt.Rows.Count; j++)
                    {
                        decimal valorDecimal;
                        if (decimal.TryParse(dt.Rows[j][colunasVisualizadas[i].Text].ToString(), out valorDecimal) && dt.Rows[j][colunasVisualizadas[i].Text].ToString().Length <= 10)
                        {
                            //if (dt.Rows[j][colunasVisualizadas[i].Text].ToString().Length > 3 &&
                            //    dt.Rows[j][colunasVisualizadas[i].Text].ToString().Substring(dt.Rows[j][colunasVisualizadas[i].Text].ToString().Length - 3, 1) == ",")
                            //{                                
                            //    float a;
                            //    float.TryParse(dt.Rows[j][colunasVisualizadas[i].Text].ToString(), out a);
                            //    planilha[i, j + 1].Value = a;                                                                                                      
                            //}

                            //else
                            planilha[i, j + 1].Value = valorDecimal;
                        }
                        else
                            planilha[i, j + 1].Value = dt.Rows[j][colunasVisualizadas[i].Text].ToString();
                    }
                }
            }

            var fileName = "Relatorio" + DateTime.Now.Ticks + ".xml";
            var outputFile = pathArquivo + fileName;
            excel.Export(outputFile);
            return fileName;
        }

        public static DataSet ConvertDataReaderToDataSet(IDataReader reader)
        {
            var ds = new DataSet();
            var dataTable = new DataTable();
            var schemaTable = reader.GetSchemaTable();
            var count = schemaTable.Rows.Count;
            for (var i = 0; i < count; i++)
            {
                var row = schemaTable.Rows[i];
                var columnName = (string)row["ColumnName"];
                var column = new DataColumn(columnName, (Type)row["DataType"]);
                dataTable.Columns.Add(column);
            }
            ds.Tables.Add(dataTable);
            var values = new object[count];
            try
            {
                dataTable.BeginLoadData();
                while (reader.Read())
                {
                    reader.GetValues(values);
                    dataTable.LoadDataRow(values, true);                    
                }
                reader.Close();
            }
            finally
            {
                dataTable.EndLoadData();
                reader.Close();
            }
            return ds;
        }

        public static string TratarTamanhoTexto(string texto, int tamanhoTextoFinal, bool retornarEspaçoHtml = false)
        {
            string retorno = "";
            int tamanhoTexto = texto.Length;
            if (tamanhoTexto == 0) return "";

            if (tamanhoTexto > tamanhoTextoFinal)
            {
                for (int i = 0; i < tamanhoTexto - 3; i++)
                {
                    retorno += texto[i];
                }
                retorno += "...";
            }
            else if (tamanhoTexto <  tamanhoTextoFinal)
            {
                int diferenca = tamanhoTextoFinal - tamanhoTexto;

                retorno = texto;
                
                for (int i = 0; i < diferenca; i++)
                {
                    retorno += " ";
                }
            }

            if (retornarEspaçoHtml)
                retorno = retorno.Replace(" ", "&nbsp;");

            return retorno;
        }
    }
}
