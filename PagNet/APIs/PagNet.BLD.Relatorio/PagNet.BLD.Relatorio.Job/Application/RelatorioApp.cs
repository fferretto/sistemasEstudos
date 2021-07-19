using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.BLD.Relatorio.Abstraction.Interface;
using PagNet.BLD.Relatorio.Abstraction.Interface.Model;
using PagNet.BLD.Relatorio.Abstraction.Model;
using PagNet.BLD.Relatorio.Job.ModelAuxiliar;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Telenet.BusinessLogicModel;
using Telenet.Data;
using Telenet.Data.SqlClient;

namespace PagNet.BLD.Relatorio.Job.Application
{

    public class RelatorioApp : Service<IContextoApp>, IRelatorioApp
    {
        private readonly IDbSqlClient<DbNetCardContext> _job;
        private readonly IParametrosApp _user;
        private readonly IPAGNET_RELATORIOService _relatorio;
        private readonly IPAGNET_PARAMETRO_RELService _parametro;

        public RelatorioApp(IContextoApp contexto,
                            IParametrosApp user,
                            IPAGNET_RELATORIOService relatorio,
                            IDbSqlClient<DbNetCardContext> job,
                            IPAGNET_PARAMETRO_RELService parametro)
            : base(contexto)
        {
            _relatorio = relatorio;
            _parametro = parametro;
            _user = user;
            _job = job;
        }

        public RelatorioModel BuscaParametrosRelatorio(int codigoRelatorio)
        {
            RelatorioModel model = new RelatorioModel();

            var DadosRel = _relatorio.GetRelatorioByID(codigoRelatorio);

            model.codRel = DadosRel.ID_REL;
            model.nmProc = DadosRel.NOMPROC;
            model.nmTela = DadosRel.DESCRICAO;
            model.ExecutaViaJob = DadosRel.EXECUTARVIAJOB;
            model.urlRelatorio = "";
            model.statusGeracao = "";

            PAGNET_RELATORIO_STATUS RelEmExecucao = new PAGNET_RELATORIO_STATUS();
            //verifica se existe algum relatório sendo gerado
            RelEmExecucao = _relatorio.BusctaStatusRelatorio(codigoRelatorio, _user.cod_usu);
            if(RelEmExecucao != null)
            {
                model.TipoRelatorio = RelEmExecucao.TIPORETORNO;
                model.statusGeracao = RelEmExecucao.STATUS;
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
                if (RelEmExecucao != null)
                {
                    if (RelEmExecucao.PAGNET_RELATORIO_PARAM_UTILIZADO != null)
                    {
                        param.VALCAMPO = RelEmExecucao.PAGNET_RELATORIO_PARAM_UTILIZADO.Where(y => y.NOMPAR == param.NOMPAR).Select(t => t.CONTEUDO).FirstOrDefault();
                        param._DEFAULT = RelEmExecucao.PAGNET_RELATORIO_PARAM_UTILIZADO.Where(y => y.NOMPAR == param.NOMPAR).Select(t => t.CONTEUDO).FirstOrDefault();
                    }
                }
                model.listaCampos.Add(param);
            }
            var RelatorioPendente = _relatorio.BuscaRelatorioPendenteDownload(_user.cod_usu);
            model.PossuiOutroRelatorioSendoGerado = false;
            if (RelatorioPendente != null)
            {
                if(RelatorioPendente.ID_REL != codigoRelatorio)
                    model.PossuiOutroRelatorioSendoGerado = true;

                model.codigoRelatorioSendoGerado = RelatorioPendente.ID_REL;
            }

            return model;
        }
        public RetornoModel GeraRelatorioViaJob(IRelatorioModel model)
        {
            RetornoModel retorno = new RetornoModel();
            try
            {
                var StatusRelatorio = _relatorio.BusctaStatusRelatorio(model.codRel, _user.cod_usu);
                if (StatusRelatorio == null)
                {
                    string COD_STATUS_REL = (_user.cod_usu.ToString() + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    IncluiStatusRel(model, COD_STATUS_REL, model.TipoRelatorio);
                    SalvaParagmetrosUsados(model, COD_STATUS_REL);

                    var query = MontaScriptJob(model, model.TipoRelatorio, COD_STATUS_REL);

                    _job.Job("PAGNET_RELATORIOS_" + COD_STATUS_REL, DescricaoJob(COD_STATUS_REL, _user.cod_usu.ToString()))
                        .Create(query)
                        .Start();


                    retorno.Sucesso = true;
                    retorno.msgResultado = "";
                }
            }
            catch (Exception ex)
            {
                retorno.Sucesso = false;
                retorno.msgResultado = ex.Message;
            }
            return retorno;
        }
        public ModRelPDFVm RetornoRelatornoViaJob(IRelatorioModel model)
        {

            var StatusRelatorio = _relatorio.BusctaStatusRelatorio(model.codRel, _user.cod_usu);
            //realiza a validação para prosseguir com a geração do relatório
            AssertionValidator
                .AssertNow(StatusRelatorio.STATUS == "FINALIZADO", Constants.CodigosErro.RelatorioNaoConcluido)
                .Validate();

            try
            {
                ModRelPDFVm modelRet = new ModRelPDFVm();
                modelRet.TipoRel = model.TipoRelatorio;


                if (StatusRelatorio.ERRO != 0)
                {
                    throw new Exception(StatusRelatorio.MSG_ERRO);
                }

                //remover o job que foi utilizado para gerar este relatório
                _job.Job("PAGNET_RELATORIOS_" + StatusRelatorio.COD_STATUS_REL)
                .DeleteIfExists();
              

                if (model.TipoRelatorio == 1)
                {

                    var colunas = new List<ItensGenerico>();
                    foreach (var item in StatusRelatorio.PAGNET_RELATORIO_RESULTADO)
                    {
                        colunas.Add(new ItensGenerico(item.LINHAIMP, item.LINHAIMP, 100));
                    }

                    model.pathArquivo = Path.Combine(model.pathArquivo, "Relatorio" + DateTime.Now.Ticks + ".csv");

                    modelRet.caminhoArquivo = MontaCSVModulo(colunas, model.pathArquivo);
                }
                else
                {


                    List<ModRel> RetornoVm = new List<ModRel>();
                    foreach (var v in StatusRelatorio.PAGNET_RELATORIO_RESULTADO)
                    {
                        ModRel vm = new ModRel();

                        vm.LINHAIMP = v.LINHAIMP.Replace("-", "<span>-</span>").ToString().Replace(" ", "&nbsp");
                        vm.TIP = Convert.ToInt32(v.TIP);
                        RetornoVm.Add(vm);
                    }

                    modelRet.Cabecalho = RetornoVm.Where(x => x.TIP == 0).ToList();
                    modelRet.Conteudo = RetornoVm.Where(x => x.TIP > 0).ToList();

                }
                //limpa a tabela utilizada para gerar o relatório.
                //_relatorio.RemoveParametrosUsadosRel(StatusRelatorio.COD_STATUS_REL);
                //_relatorio.RemoveRelatorioResult(StatusRelatorio.COD_STATUS_REL);
                _relatorio.RemoveRelatorioStatus(StatusRelatorio.COD_STATUS_REL);

                return modelRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RetornoModel ExportaExcel(IRelatorioModel model)
        {
            try
            {
                RetornoModel retorno = new RetornoModel();

                var DadosRel = _relatorio.GetRelatorioByID(model.codRel);
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

                    model.pathArquivo = Path.Combine(model.pathArquivo, "Relatorio" + DateTime.Now.Ticks + ".csv");

                    retorno.Sucesso = false;
                    retorno.msgResultado = MontaCSVModulo(cols, model.pathArquivo, MyListDinamic);
                
                return retorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ModRelPDFVm RelatorioPDF(IRelatorioModel model)
        {
            try
            {
                var relatorio = new ModRelPDFVm();

                var DadosRel = _relatorio.GetRelatorioByID(model.codRel);
                
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
        private string MontaScriptJob(IRelatorioModel model, int TipoRelatorio, string COD_STATUS_REL)
        {
            try
            {
                StringBuilder Query = new StringBuilder();
                Query.AppendLine("   USE _NOME_BANCO_                                                                                           ");
                Query.AppendLine();
                Query.AppendLine("   DECLARE @ERRO int                                                                                          ");
                Query.AppendLine("   DECLARE @MSG_ERRO varchar(512)                                                                             ");
                Query.AppendLine();
                Query.AppendLine("   EXEC [dbo].[_NOMEPROCEDURE_]                                                                               ");
                Query.AppendLine("              _PARAMETROS_,                                                                                   ");
                Query.AppendLine("              @COD_STATUS_REL = ''_COD_STATUS_REL_'',                                                          ");
                Query.AppendLine("              @ERRO = @ERRO OUTPUT,                                                                           ");
                Query.AppendLine("              @MSG_ERRO = @MSG_ERRO OUTPUT                                                                    ");
                Query.AppendLine();
                Query.AppendLine("       UPDATE PAGNET_RELATORIO_STATUS SET ERRO = @ERRO, MSG_ERRO = @MSG_ERRO, STATUS = ''FINALIZADO'' WHERE COD_STATUS_REL = ''_COD_STATUS_REL_''    ");
                Query.AppendLine();
                Query.AppendLine("   IF(@ERRO = 666666)                                                                                         ");
                Query.AppendLine("   BEGIN                                                                                                      ");
                Query.AppendLine("      RAISERROR(@MSG_ERRO, 16, 1);                                                                            ");
                Query.AppendLine("   END                                                                                                        ");

                var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC);

                string param = "";
                param += "@FORMATO=" + TipoRelatorio.ToString();
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
                                param += "@" + item.NOMPAR + "=''" + newData + "''";
                            }
                            else
                            {
                                param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "''''" : "''" + item.VALCAMPO + "''");
                            }
                            break;
                        case "INT":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + (item.VALCAMPO ?? "0");
                            break;
                    }

                }

                string QueryRetorno = Query.ToString()
                        .Replace("_NOMEPROCEDURE_", model.nmProc)
                        .Replace("_NOME_BANCO_", _user.BdNetCard)
                        .Replace("_COD_STATUS_REL_", COD_STATUS_REL)
                        .Replace("_PARAMETROS_", param);



                return QueryRetorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private static string DescricaoJob(string COD_STATUS_REL, string codigoUsuario)
        {
            return string.Format(@"Job de geração de relatórios. 
            Processo: {0}
            usuario: {1}", COD_STATUS_REL, codigoUsuario);
        }
        private void SalvaParagmetrosUsados(IRelatorioModel model, string COD_STATUS_REL)
        {
            var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC);
            PAGNET_RELATORIO_PARAM_UTILIZADO parametros = new PAGNET_RELATORIO_PARAM_UTILIZADO();

            foreach (var item in listaParametros)
            {
                if (item.NOMPAR.ToUpper() == "FORMATO") continue;

                parametros = new PAGNET_RELATORIO_PARAM_UTILIZADO();
                parametros.COD_STATUS_REL = COD_STATUS_REL;
                parametros.NOMPAR = item.NOMPAR;
                parametros.CONTEUDO = item.VALCAMPO ?? "";

                _relatorio.IncluiParametroUtilizado(parametros);

            }

        }
        private void IncluiStatusRel(IRelatorioModel model, string COD_STATUS_REL, int tipoRetorno)
        {

            PAGNET_RELATORIO_STATUS status = new PAGNET_RELATORIO_STATUS();
            status.COD_STATUS_REL = COD_STATUS_REL;
            status.ID_REL = model.codRel;
            status.CODUSUARIO = _user.cod_usu;
            status.STATUS = "EM_ANDAMENTO";
            status.TIPORETORNO = tipoRetorno;
            status.DATEMISSAO = DateTime.Now;
            status.ERRO = 0;
            status.MSG_ERRO = "";
            _relatorio.IncluiStatusRel(status);
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

    }
}
