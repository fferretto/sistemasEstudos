using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.BLD.Relatorio.Abstraction.Interface;
using PagNet.BLD.Relatorio.Abstraction.Interface.Model;
using PagNet.BLD.Relatorio.Abstraction.Model;
using PagNet.BLD.Relatorio.ModelAuxiliar;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Telenet.BusinessLogicModel;

namespace PagNet.BLD.Relatorio.Application
{
    public class PagamentoApp : Service<IContextoApp>, IPagamentoApp
    {
        private readonly IParametrosApp _user;
        private readonly IPAGNET_RELATORIOService _relatorio;
        private readonly IPAGNET_PARAMETRO_RELService _parametro;

        public PagamentoApp(IContextoApp contexto,
                            IParametrosApp user,
                            IPAGNET_RELATORIOService relatorio,
                            IPAGNET_PARAMETRO_RELService parametro)
            : base(contexto)
        {
            _relatorio = relatorio;
            _parametro = parametro;
            _user = user;
        }

        public RelatorioModel BuscaParametrosRelatorio(int codigoRelatorio)
        {
            RelatorioModel model = new RelatorioModel();

            var DadosRel = _relatorio.GetRelatorioByID(codigoRelatorio);

            model.codRel = DadosRel.ID_REL;
            model.nmProc = DadosRel.NOMPROC;
            model.nmTela = DadosRel.DESCRICAO;
            model.urlRelatorio = "";

            PAGNET_RELATORIO_STATUS RelEmExecucao = new PAGNET_RELATORIO_STATUS();
            if (DadosRel.EXECUTARVIAJOB == "S")
            {
                RelEmExecucao = _relatorio.BusctaStatusRelatorio(codigoRelatorio, _user.cod_usu);
                if (RelEmExecucao != null && RelEmExecucao.STATUS == "EM_ANDAMENTO")
                {
                    model.EmGeracao = true;
                }
            }

            ParametrosRelatorioVm param = new ParametrosRelatorioVm();

            foreach (var x in DadosRel.PAGNET_PARAMETRO_REL)
            {
                param = new ParametrosRelatorioVm();
                param.ID_PAR = x.ID_PAR;
                param.ID_REL = x.ID_REL;
                param.DESPAR = x.DESPAR;
                param.NOMPAR = x.NOMPAR.Replace("@", "");
                param.LABEL = x.LABEL;
                param.TIPO = x.TIPO;
                param.TAMANHO = x.TAMANHO;
                param._DEFAULT = x._DEFAULT;
                param.REQUERIDO = x.REQUERIDO;
                param.ORDEM_TELA = x.ORDEM_TELA;
                param.ORDEM_PROC = x.ORDEM_PROC;
                param.NOM_FUNCTION = x.NOM_FUNCTION;
                param.MASCARA = x.MASCARA;
                param.TEXTOAJUDA = x.TEXTOAJUDA;
                if (RelEmExecucao.PAGNET_RELATORIO_PARAM_UTILIZADO != null)
                {
                    param.VALCAMPO = RelEmExecucao.PAGNET_RELATORIO_PARAM_UTILIZADO.Where(y => y.NOMPAR == param.NOMPAR).Select(t => t.CONTEUDO).FirstOrDefault();
                }
                model.listaCampos.Add(param);
            }

            model.PossuiOutroRelatorioSendoGerado = _relatorio.PossuiOutroRelatorioEmGeracao(codigoRelatorio, _user.cod_usu);

            return model;
        }
        public IDictionary<bool, string> ExportaExcel(IRelatorioModel model, string pathArquivo)
        {
            try
            {
                Dictionary<bool, string> retorno = new Dictionary<bool, string>();

                var DadosRel = _relatorio.GetRelatorioByID(model.codRel);
                if (DadosRel.EXECUTARVIAJOB == "S")
                {
                    var StatusRelatorio = _relatorio.BusctaStatusRelatorio(model.codRel, _user.cod_usu);
                    if (StatusRelatorio == null)
                    {
                         
                    }
                    else
                    {
                        //realiza a validação para prosseguir com a geração do relatório
                        AssertionValidator
                            .AssertNow(StatusRelatorio.STATUS != "FINALIZADO", Constants.CodigosErro.RelatorioNaoConcluido)
                            .Validate();

                        if (StatusRelatorio.ERRO != 0)
                        {
                            throw new Exception(StatusRelatorio.MSG_ERRO);
                        }


                        var colunas = new List<ItensGenerico>();
                        foreach (var item in StatusRelatorio.PAGNET_RELATORIO_RESULTADO)
                        {
                            colunas.Add(new ItensGenerico(item.LINHAIMP, item.LINHAIMP, 100));
                        }

                        retorno.Add(true, MontaCSVModulo(colunas, pathArquivo));

                    }
                }
                else
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

                    retorno.Add(true,MontaCSVModulo(cols, pathArquivo, MyListDinamic));
                }
                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        private string MontaCSVModulo(List<ItensGenerico> colunasVisualizadas, string pathArquivo)
        {
            if (colunasVisualizadas == null)
            {
                return string.Empty;
            }

            using (var outfile = new StreamWriter(pathArquivo))
            {
                var linha = string.Empty;

                foreach (var lin in colunasVisualizadas)
                {
                    linha = lin.Value;
                    outfile.WriteLine(linha);
                }
            }

            return pathArquivo;
        }
        public ModRelPDFVm RelatorioPDF(IRelatorioModel model)
        {
            try
            {
                var relatorio = new ModRelPDFVm();
                var StatusRelatorio = _relatorio.BusctaStatusRelatorio(model.codRel, _user.cod_usu);
                if (StatusRelatorio != null)
                {
                    //realiza a validação para prosseguir com a geração do relatório
                    AssertionValidator
                        .AssertNow(StatusRelatorio.STATUS != "FINALIZADO", Constants.CodigosErro.RelatorioNaoConcluido)
                        .Validate();

                    if (StatusRelatorio.ERRO != 0)
                    {
                        throw new Exception(StatusRelatorio.MSG_ERRO);
                    }

                    List<ModRel> RetornoVm = new List<ModRel>();
                    foreach (var v in StatusRelatorio.PAGNET_RELATORIO_RESULTADO)
                    {
                        ModRel vm = new ModRel();

                        vm.LINHAIMP = ((IDictionary<String, Object>)v)["LINHAIMP"].ToString().Replace("-", "<span>-</span>").ToString().Replace(" ", "&nbsp");
                        vm.TIP = Convert.ToInt32(((IDictionary<String, Object>)v)["TIP"]);
                        RetornoVm.Add(vm);
                    }

                    relatorio.Cabecalho = RetornoVm.Where(x => x.TIP == 0).ToList();
                    relatorio.Conteudo = RetornoVm.Where(x => x.TIP > 0).ToList();


                }
                else
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

                }

                return relatorio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool VerificaTerminoGeracaoRelatorio(int codigoRelatorio)
        {
            var StatusRelatorio = _relatorio.BusctaStatusRelatorio(codigoRelatorio, _user.cod_usu);
            return (StatusRelatorio.STATUS == "FINALIZADO");
        }
    }
}
