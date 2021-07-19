using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace PagNet.Application.Application
{
    public class RelatorioApp : IRelatorioApp
    {
        private readonly IPagNet_RelatorioService _relatorio;
        private readonly IPagNet_Parametro_RelService _parametro;

        public RelatorioApp(IPagNet_RelatorioService relatorio,
                            IPagNet_Parametro_RelService parametro)
        {
            _relatorio = relatorio;
            _parametro = parametro;
        }

        public RelatorioVms GetParametrosByRelatorio(int codRel)
        {
            RelatorioVms model = new RelatorioVms();

            var Parametros = _relatorio.GetRelatorioByID(codRel);
            model = RelatorioVms.ToViewRelatorio(Parametros);

            return model;

        }
        public ModRelPDFVm RelatorioPDF(RelatorioVms model)
        {
            try
            {
                string _query = $"Exec {model.nmProc} ";

                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC);
                string param = "";
                param += "@FORMATO=0";
                foreach (var item in listaParametros)
                {
                    if (item.NOMPAR.ToUpper() == "FORMATO") continue;

                    switch (item.TIPO.ToUpper())
                    {                       
                        case "BYTE":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "0" : (item.VALCAMPO == "true") ? "1" : "0");
                            break;
                        case "STRING":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "''" : "'" + item.VALCAMPO + "'");
                            break;
                        case "INT":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + (item.VALCAMPO ?? "0");
                            break;
                    }

                    //Parameters.Add("@" + item.NOMPAR, item.VALCAMPO);
                }
                _query += param;
                var MyListDinamic = _relatorio.GetDadosRelatorio(_query, Parameters).ToList();

                var relatorio = new ModRelPDFVm();
            
                List<ModRel> RetornoVm = new List<ModRel>();
                foreach (var v in MyListDinamic.ToList())
                {
                    ModRel vm = new ModRel();

                    vm.LINHAIMP = ((IDictionary<String, Object>)v)["LINHAIMP"].ToString().Replace("-", "<span>-</span>").ToString().Replace(" ", "&nbsp");
                    vm.TIP = Convert.ToInt32(((IDictionary<String, Object>)v)["TIP"]);
                    RetornoVm.Add(vm);
                }

                relatorio.Cabecalho = RetornoVm.Where(x => x.TIP == 0).ToList();
                relatorio.Conteudo = RetornoVm.Where(x => x.TIP > 0).ToList();
                


                return relatorio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string ExportaExcel(RelatorioVms model, string pathArquivo)
        {
            try
            {
                string _query = $"Exec {model.nmProc} ";

                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC);
                string param = "";
                param += "@FORMATO=1";
                foreach (var item in listaParametros)
                {
                    if (item.NOMPAR.ToUpper() == "FORMATO") continue;

                    switch (item.TIPO.ToUpper())
                    {
                        case "BYTE":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "0" : (item.VALCAMPO == "true") ? "1" : "0");
                            break;
                        case "STRING":
                            if (param != "") param += ", ";
                            if (item.LABEL.ToUpper() == "DATEEDIT")
                            {
                                var newData = item.VALCAMPO.Substring(6, 4) + item.VALCAMPO.Substring(3, 2) + item.VALCAMPO.Substring(0, 2);
                                param += "@" + item.NOMPAR + "='" + newData + "'";
                            }
                            else
                            {
                                param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "''" : "'" + item.VALCAMPO + "'");
                            }
                            break;
                        case "INT":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + (item.VALCAMPO ?? "0");
                            break;
                    }

                }
                _query += param;
                var MyListDinamic = _relatorio.ListaDadosRelDataTable(_query, Parameters);

                //var dt = Geral.ConvertToDataTable(MyListDinamic);

                var cols = MontaColunasDataTable(MyListDinamic);

                pathArquivo = Path.Combine(pathArquivo, "Relatorio" + DateTime.Now.Ticks + ".csv");
                                
                return MontaCSVModulo(cols, pathArquivo, MyListDinamic);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string MontaCSVModulo(List<ItensGenerico> colunasVisualizadas, string pathArquivo, DataTable dt)
        {
            if (colunasVisualizadas == null)
            {
                return string.Empty;
            }

            if (dt.Rows.Count <= 0)
            {
                return string.Empty;
            }


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

        private static List<ItensGenerico> MontaColunasDataTable(DataTable DadosRel)
        {
            if (DadosRel.Rows.Count <= 0)
            {
                return null;
            }

            var colunas = new List<ItensGenerico>();

            for (var i = 0; i < DadosRel.Columns.Count; i++)
            {
                if (!colunas.Contains(new ItensGenerico(DadosRel.Columns[i].ToString(), DadosRel.Columns[i].ToString(), 100)))
                {
                    colunas.Add(new ItensGenerico(DadosRel.Columns[i].ToString(), DadosRel.Columns[i].ToString(), 100));
                }
            }

            return colunas;
        }
    }
}