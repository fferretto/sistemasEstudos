using SIL.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using TELENET.SIL;
using TELENET.SIL.PO;
using Yogesh.ExcelXml;

namespace SIL.BLL
{
    public class blModRel
    {
        public blModRel(OPERADORA operadora)
        {
            _operadora = operadora;
            _daModRel = new daModRel(_operadora);
        }

        private OPERADORA _operadora;
        private daModRel _daModRel;

        private void AdicionaParametroSistemaSeNecessario(List<PARAMETRO> parametros)
        {
            if (!parametros.Any(p => p.NOMPAR.Equals("@SISTEMA", StringComparison.InvariantCultureIgnoreCase)))
            {
                var sistemaRelatorio = _daModRel.ObterRelatorio(parametros.First().IDREL).SISTEMA.Trim();

                parametros.Add(new PARAMETRO
                {
                    NOMPAR = "@SISTEMA",
                    VALOR = sistemaRelatorio == "PJ"
                            ? (object)ConstantesSIL.SistemaPOS
                            : sistemaRelatorio == "VA"
                                ? (object)ConstantesSIL.SistemaPRE
                                : 0
                });
            }
        }

        public List<RELATORIO> ListaRelatorio()
        {
            return _daModRel.ListaRelatorio();
        }

        public List<RELATORIO> ListaRelatorio(int codAg)
        {
            return _daModRel.ListaRelatorio(codAg);
        }

        public List<PARAMETRO> ListaParametros(int idRel)
        {
            var parametros = _daModRel.ListaParametros(idRel);
            return parametros;
        }

        public List<PARAM> FuncParametros(string nomFunc, List<PARAM> parametros)
        {
            return _daModRel.FuncParametros(nomFunc, parametros);
        }

        public List<DETPAR> DetalheParametro(int idPar)
        {
            return _daModRel.DetalheParametro(idPar);
        }

        public List<MODREL> ListaDadosRel(List<PARAMETRO> parametros, out string erro)
        {
            erro = string.Empty;
            try
            {
                AdicionaParametroSistemaSeNecessario(parametros);
                return _daModRel.ListaDadosRel(parametros, Convert.ToString(_operadora.ID_FUNC), out erro);
            }
            catch
            {
                return null;
            }
        }

        public string GeraExcel(Hashtable consulta, List<FECHCRED_VA> result, string pathArquivo)
        {
            if (result == null)
            {
                return string.Empty;
            }

            var colunasVisualizadas = new List<ItensGenerico> 
            {
                new ItensGenerico(UtilSIL.BANCO, "Banco", 40, 1),
                new ItensGenerico(UtilSIL.AGENCIA, "Agencia", 40, 2),
                new ItensGenerico(UtilSIL.CONTA, "Conta", 40, 3),
                new ItensGenerico(UtilSIL.CODCRE, "Cod. Cre.", 50, 4),
                new ItensGenerico(UtilSIL.RAZSOC, "Razao Social", 200, 5),
                new ItensGenerico(UtilSIL.VALAFE, "Val.Afe", 50, 6),
                new ItensGenerico(UtilSIL.QTEAFE, "Qt.Afe", 50, 7),
                new ItensGenerico(UtilSIL.VALTAXA, "Valor", 50, 8),
                new ItensGenerico(UtilSIL.VALLIQ, "Val.Liq", 50, 9),
                new ItensGenerico(UtilSIL.DATFECLOT, "DtFechto", 60, 10),
                new ItensGenerico(UtilSIL.PRAZO, "Prz", 30, 11),
                new ItensGenerico(UtilSIL.DATPGTOCRE, "Dt.Pgto", 60, 12)
            };

            var excel = new ExcelXmlWorkbook { Properties = { Author = _operadora.LOGIN } };
            var planilha = excel[0];
            planilha.Name = "Credenciados_Fechados";

            var valafe = 0m;
            var qteafe = 0;
            var valliq = 0m;
            var dataIni = Convert.ToDateTime(consulta["DataIni"]).ToShortDateString();
            var dataFim = Convert.ToDateTime(consulta["DataFim"]).ToShortDateString();

            for (int i = 0; i < colunasVisualizadas.Count(); i++)
            {
                //Criei o cabecalho da coluna
                planilha[i, 0].Value = colunasVisualizadas[i].Text;
                
                var style = new XmlStyle();
                var styleData = new XmlStyle();

                style.Interior.Color = Color.LightGray;
                style.Font.Bold = true;
                style.Font.Name = "Calibri";
                style.Font.Size = 11;
                styleData.DisplayFormat = DisplayFormatType.ShortDate;
                planilha[i, 0].Style = style;
                planilha[i, result.Count + 1].Style = style;
                planilha.Columns(i).Width = colunasVisualizadas[i].TamanhoColuna;

                for (int j = 0; j < result.Count; j++)
                {
                    if (colunasVisualizadas[i].Value.Substring(0, 2) == "DT" ||
                        colunasVisualizadas[i].Value.Substring(0, 2) == "DA")
                    {
                        planilha[i, j + 1].Value =
                            Convert.ToDateTime(result[j].GetCollumnValue(colunasVisualizadas[i].Value)).
                                ToShortDateString();
                        planilha[i, j + 1].Style = styleData;
                    }
                    else
                    {
                        planilha[i, j + 1].Value = result[j].GetCollumnValue(colunasVisualizadas[i].Value) ?? "";
                    }

                    if (colunasVisualizadas[i].Value == "VALAFE")
                    {
                        valafe += Convert.ToDecimal(planilha[i, j + 1].Value);
                    }
                    if (colunasVisualizadas[i].Value == "QTEAFE")
                    {
                        qteafe += Convert.ToInt16(planilha[i, j + 1].Value);
                    }
                    if (colunasVisualizadas[i].Value == "VALLIQ")
                    {
                        valliq += Convert.ToDecimal(planilha[i, j + 1].Value);
                    }
                }
            }

            planilha[0, result.Count + 1].Value = "Totais:";
            planilha[5, result.Count + 1].Value = valafe;
            planilha[6, result.Count + 1].Value = qteafe;
            planilha[8, result.Count + 1].Value = valliq;

            planilha.InsertRowBefore(0);
            planilha[0, 0].Value = "Movimentacao Diaria dos Credenciados";
            planilha.InsertRowBefore(1);
            planilha.InsertRowBefore(2);
            planilha[0, 2].Value = "Data de Pagamento de " + dataIni + " a " + dataFim;
            planilha.InsertRowBefore(3);

            var fileName = "Listagem" + DateTime.Now.Ticks + ".xml";
            var outputFile = pathArquivo + fileName;
            
            excel.Export(outputFile);
            
            return fileName;
        }

        public string GeraExcelModuloRelatorios(List<PARAMETRO> parametros, string pathArquivo)
        {
            try
            {
                AdicionaParametroSistemaSeNecessario(parametros);

                var ds = _daModRel.ListaDadosRelDataSet(parametros);
                var cols = MontaColunasDataTable(ds);

                return MontaExcelModulo(cols, pathArquivo, ds);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GeraCSVModuloRelatorios(List<PARAMETRO> parametros, string pathArquivo)
        {
            try
            {
                AdicionaParametroSistemaSeNecessario(parametros);
                
                var ds = _daModRel.ListaDadosRelDataSet(parametros);
                var cols = MontaColunasDataTable(ds);
                
                pathArquivo += "Relatorio" + DateTime.Now.Ticks + ".csv";
                
                return MontaCSVModulo(cols, pathArquivo, ds);
            }
            catch
            {
                return null;
            }
        }

        public string GeraTXTModuloRelatorios(List<PARAMETRO> parametros, string pathArquivo)
        {
            try
            {
                var ds = _daModRel.ListaDadosRelDataSet(parametros);
                var cols = MontaColunasDataTable(ds);

                pathArquivo += "Relatorio" + DateTime.Now.Ticks + ".txt";
                
                return MontaTXTModulo(pathArquivo, ds);
            }
            catch 
            {
                return null;
            }
        }

        private static List<ItensGenerico> MontaColunasDataTable(DataSet ds)
        {
            if (ds.Tables.Count <= 0)
            {
                return null;
            }

            var colunas = new List<ItensGenerico>();
            var dt = ds.Tables[0];
            
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                if (!colunas.Contains(new ItensGenerico(dt.Columns[i].ToString(), dt.Columns[i].ToString(), 100)))
                {
                    colunas.Add(new ItensGenerico(dt.Columns[i].ToString(), dt.Columns[i].ToString(), 100));
                }
            }

            return colunas;
        }

        private string MontaExcelModulo(List<ItensGenerico> colunasVisualizadas, string pathArquivo, DataSet ds)
        {
            if (colunasVisualizadas == null)
            {
                return string.Empty;
            }

            if (ds.Tables.Count <= 0)
            {
                return string.Empty;
            }
            
            var dt = ds.Tables[0];
            var excel = new ExcelXmlWorkbook { Properties = { Author = _operadora.LOGIN } };
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
                        Font = { Bold = true, Name = "Calibri", Size = 11 },
                        DisplayFormat = 0
                    };

                    planilha[i, 0].Style = style;
                    planilha.Columns(i).Width = colunasVisualizadas[i].TamanhoColuna;

                    for (var j = 0; j < dt.Rows.Count; j++)
                    {
                        decimal valorDecimal;

                        if (dt.Rows[j][colunasVisualizadas[i].Text].ToString().Contains(',') && dt.Rows[j][colunasVisualizadas[i].Text].ToString().Length <= 10)
                        {
                            decimal.TryParse(dt.Rows[j][colunasVisualizadas[i].Text].ToString(), out valorDecimal);
                            planilha[i, j + 1].Value = valorDecimal;
                        }
                        else
                        {
                            planilha[i, j + 1].Value = dt.Rows[j][colunasVisualizadas[i].Text].ToString();
                        }
                    }
                }
            }

            var fileName = "Relatorio" + DateTime.Now.Ticks + ".xml";
            var outputFile = pathArquivo + fileName;
            
            excel.Export(outputFile);
            
            return fileName;
        }

        private string MontaCSVModulo(List<ItensGenerico> colunasVisualizadas, string pathArquivo, DataSet ds)
        {
            if (colunasVisualizadas == null)
            {
                return string.Empty;
            }

            if (ds.Tables.Count <= 0)
            {
                return string.Empty;
            }
            
            var dt = ds.Tables[0];

            if (colunasVisualizadas.Any())
            {
                using (var outfile = new StreamWriter(pathArquivo))
                {
                    var cabec = string.Empty;

                    foreach (var cab in colunasVisualizadas)
                    {
                        //Criei o cabecalho da coluna
                        cabec += cab.Text + ";";
                    }

                    outfile.WriteLine(cabec);
                    var linha = string.Empty;
                    
                    foreach (DataRow lin in dt.Rows)
                    {
                        linha = string.Empty;
                    
                        for (int i = 0; i < lin.ItemArray.Count(); i++)
                        {
                            linha += lin.ItemArray[i].ToString().Replace(';', ',') + ";";
                        }
                        
                        outfile.WriteLine(linha);
                    }
                }
            }

            return pathArquivo;
        }

        private string MontaTXTModulo(string pathArquivo, DataSet ds)
        {
            if (ds.Tables.Count <= 0)
            {
                return string.Empty;
            }

            var dt = ds.Tables[0];

            using (var outfile = new StreamWriter(pathArquivo))
            {
                var linha = string.Empty;
                
                foreach (DataRow lin in dt.Rows)
                {
                    linha = string.Empty;
                    
                    for (int i = 0; i < lin.ItemArray.Count(); i++)
                    {
                        linha += lin.ItemArray[i].ToString().Replace(';', ',');
                    }
                    
                    outfile.WriteLine(linha);
                }
            }

            return pathArquivo;
        }

        public List<PARAM> ListaOpcoesDropDown(string nomProc)
        {
            return _daModRel.ListaOpcoesDropDown(nomProc);
        }
    }
}