using NetCard.Bld.Relatorio.Abstraction.Interface;
using NetCard.Bld.Relatorio.Abstraction.Interface.Model;
using NetCard.Bld.Relatorio.Abstraction.Model;
using NetCard.Bld.Relatorio.Context;
using NetCard.Bld.Relatorio.Data;
using NetCard.Bld.Relatorio.Entity;
using NetCard.Bld.Relatorio.Helper;
using NetCard.Bld.Relatorio.Interface;
using NetCard.Bld.Relatorio.ModelAuxiliar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Telenet.BusinessLogicModel;
using Telenet.Data.SqlClient;

namespace NetCard.Bld.Relatorio.Application
{
    public class RelatorioApp : Service<IContextoApp>, IRelatorioApp
    {
        private readonly IDadosApp _data;
        private readonly IDbSqlClient<DbNetCardContext> _job;
        private readonly IParametrosApp _user;

        public RelatorioApp(IContextoApp contexto,
                            IDadosApp data,
                            IDbSqlClient<DbNetCardContext> job,
                            IParametrosApp user)
            : base(contexto)
        {
            _user = user;
            _job = job;
            _data = data;
        }

        public RelatorioModel BuscaParametrosRelatorio(IFiltroRelModel filtro)
        {
            RelatorioModel model = new RelatorioModel();
            model.urlRelatorio = "";
            model.statusGeracao = "";
            model.msgRetorno = "";

            var _chaveAcesso = MontaChaveAcesso(filtro.sistema, filtro.codigoCliente);

            RELATORIO_STATUS RelEmExecucao = new RELATORIO_STATUS();
            List<RELATORIO_PARAM_UTILIZADO> ListaParamUtilizado = new List<RELATORIO_PARAM_UTILIZADO>();
            //verifica se existe algum relatório sendo gerado
            RelEmExecucao = _data.BuscaStatusRelatorio(_chaveAcesso);
            if (RelEmExecucao != null)
            {
                model.TipoRelatorio = RelEmExecucao.TIPORETORNO;
                model.statusGeracao = RelEmExecucao.STATUS;
                ListaParamUtilizado = _data.BuscaListaParametrosUtilizados(RelEmExecucao.COD_STATUS_REL);

                if(filtro.codigoRelatorio != RelEmExecucao.ID_REL)
                {
                    if(RelEmExecucao.STATUS == "FINALIZADO")
                    {
                        if(RelEmExecucao.TIPORETORNO == 0)
                            model.msgRetorno = "Necessário a visualizar este relatório.";
                        else
                            model.msgRetorno = "Necessário realizar o download deste relatório";
                    }
                    else
                    {
                        model.msgRetorno = "Aguarde a finalização deste relatório.";
                    }
                }

                filtro.codigoRelatorio = RelEmExecucao.ID_REL;
            }

            var DadosRel = _data.BuscaRelatorioByID(filtro.codigoRelatorio);

            model.idRelatorio = DadosRel.ID_REL;
            model.nmProc = DadosRel.NOMPROC;
            model.nmTela = DadosRel.DESCRICAO;
            model.ExecutaViaJob = DadosRel.EXECUTARVIAJOB ?? "N";

            ParametrosRelatorioVm param = new ParametrosRelatorioVm();

            var ListaParametrosRel = _data.BuscaListaParametrosRelatorio(filtro.codigoRelatorio);

            foreach (var x in ListaParametrosRel)
            {
                param = new ParametrosRelatorioVm();
                param.ID_PAR = x.ID_PAR;
                param.ID_REL = x.ID_REL;
                param.DESPAR = x.DESPAR.Trim();
                param.NOMPAR = x.NOMPAR.Replace("@", "");
                param.LABEL = x.LABEL;
                param.TIPO = x.TIPO;
                param.TAMANHO = x.TAMANHO;
                param._DEFAULT = x._DEFAULT;
                param.REQUERIDO = x.REQUERIDO ?? "N";
                param.ORDEM_TELA = x.ORDEM_TELA;
                param.ORDEM_PROC = x.ORDEM_PROC;
                param.NOM_FUNCTION = x.NOM_FUNCTION;
                param.MASCARA = x.MASCARA;
                param.TEXTOAJUDA = x.TEXTOAJUDA;
                if (RelEmExecucao != null)
                {
                    if (ListaParamUtilizado != null && ListaParamUtilizado.Count > 0)
                    {
                        var dadosParamUtilizado = ListaParamUtilizado.Where(y => y.NOMPAR == param.NOMPAR).FirstOrDefault();
                        if (dadosParamUtilizado != null)
                        {
                            if (dadosParamUtilizado.LABEL == "DropDownList")
                            {
                                FiltroDDLRelModel paramddl = new FiltroDDLRelModel();
                                paramddl.codigoParametro = param.ID_PAR;
                                paramddl.nomeProc = param.NOM_FUNCTION;
                                paramddl.ParametroWhere = "";
                                var dadosDDL = CarregaDDL(paramddl).ToList();
                                var DadosCampo = dadosDDL.Where(z => z.Valor.Trim() == dadosParamUtilizado.CONTEUDO.Trim()).FirstOrDefault();
                                param.VALCAMPO = DadosCampo.Descricao;
                                param._DEFAULT = dadosParamUtilizado.CONTEUDO;
                            }
                            else
                            {
                                param.VALCAMPO = dadosParamUtilizado.CONTEUDO;
                                param._DEFAULT = dadosParamUtilizado.CONTEUDO;
                            }
                        }
                    }
                }
                model.listaCampos.Add(param);
            }
            var RelatorioPendente = _data.BuscaRelatorioPendenteDownload(_chaveAcesso);
            model.PossuiOutroRelatorioSendoGerado = false;
            if (RelatorioPendente != null)
            {
                if (RelatorioPendente.ID_REL != filtro.codigoRelatorio)
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
                var _chaveAcesso = MontaChaveAcesso(model.Sistema, model.codigoCliente);
                var StatusRelatorio = _data.BuscaStatusRelatorio(_chaveAcesso);
                if (StatusRelatorio == null)
                {
                    string COD_STATUS_REL = (_user.id_login + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    IncluiStatusRel(model, COD_STATUS_REL, model.TipoRelatorio);
                    SalvaParagmetrosUsados(model, COD_STATUS_REL);

                    var query = MontaScriptJob(model, COD_STATUS_REL);

                    _job.Job("NETCARD_RELATORIOS_" + COD_STATUS_REL, DescricaoJob(COD_STATUS_REL, _user.id_login))
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
            var _chaveAcesso = MontaChaveAcesso(model.Sistema, model.codigoCliente);

            var StatusRelatorio = _data.BuscaStatusRelatorio(_chaveAcesso);
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
                _job.Job("NETCARD_RELATORIOS_" + StatusRelatorio.COD_STATUS_REL)
                .DeleteIfExists();

                var RelatorioResultado = _data.BuscaRelatorioResultado(StatusRelatorio.COD_STATUS_REL);

                if (model.TipoRelatorio == 1)
                {

                    var colunas = new List<ItensGenerico>();
                    foreach (var item in RelatorioResultado)
                    {
                        colunas.Add(new ItensGenerico(item.LINHAIMP, item.LINHAIMP, 100));
                    }
                    var nomeArquivo = "Relatorio" + DateTime.Now.Ticks + ".csv";

                    model.pathArquivo = Path.Combine(model.pathArquivo, nomeArquivo);

                    MontaCSVModulo(colunas, model.pathArquivo);

                    modelRet.caminhoArquivo = nomeArquivo; 
                }
                else
                {


                    List<ModRel> RetornoVm = new List<ModRel>();
                    foreach (var v in RelatorioResultado)
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
                _data.RemoveRelatorioStatus(StatusRelatorio.COD_STATUS_REL);

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

                var DadosRel = _data.BuscaRelatorioByID(model.idRelatorio);
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
                            param += "@" + item.NOMPAR + "=" + (item.VALCAMPO ?? "0");
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
                var MyListDinamic = _data.ListaDadosRelDataTable(_query);

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
                relatorio.msgResultado = "";
                relatorio.sucesso = true;

                var DadosRel = _data.BuscaRelatorioByID(model.idRelatorio);

                string _query = $"Exec {model.nmProc} ";

                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC);
                string param = "";
                param += "@FORMATO=0";
                foreach (var item in listaParametros)
                {
                    if (item.NOMPAR.ToUpper() == "FORMATO") continue;

                    switch (item.LABEL.ToUpper())
                    {
                        case "DROPDOWNLIST":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "0" : (item.VALCAMPO == "true") ? "1" : "0");
                            break;
                        case "CHECKBOX":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "0" : (item.VALCAMPO == "true") ? "1" : "0");
                            break;
                        case "DATEEDIT":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "''" : "'" + Convert.ToDateTime(item.VALCAMPO).ToString("yyyyMMdd") + "'");
                            break;
                        case "INT":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + (item.VALCAMPO ?? "0");
                            break;
                        default:
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + ((item.VALCAMPO == null) ? "''" : "'" + item.VALCAMPO + "'");
                            break;
                    }

                    //Parameters.Add("@" + item.NOMPAR, item.VALCAMPO);
                }
                _query += param;
                var MyListDinamic = _data.BuscaDadosRelatorio(_query).ToList();
                try
                {
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
                catch
                {
                    var resultado = MyListDinamic.FirstOrDefault();
                    relatorio.msgResultado = Convert.ToString(((IDictionary<String, Object>)resultado).Values.First());
                    relatorio.sucesso = false;
                }
                




                return relatorio;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool VerificaTerminoGeracaoRelatorio(IFiltroRelModel filtro)
        {
            var _chaveAcesso = MontaChaveAcesso(filtro.sistema, filtro.codigoCliente);
            var StatusRelatorio = _data.BuscaStatusRelatorio(_chaveAcesso);
            return (StatusRelatorio.STATUS == "FINALIZADO");
        }
        public List<APIRetornoDDLModel> CarregaDDL(IFiltroDDLRelModel filtro)
        {
            var dadosParametro = _data.BuscaDadosParametro(filtro.codigoParametro);
            List<APIRetornoDDLModel> listaRetorno = new List<APIRetornoDDLModel>();
            APIRetornoDDLModel item = new APIRetornoDDLModel();

            if (string.IsNullOrWhiteSpace(dadosParametro.NOM_FUNCTION))
            {
                var ListaDetalheParametros = _data.ListaDetalheParametro(filtro.codigoParametro);
                foreach(var detalhe in ListaDetalheParametros)
                {
                    item = new APIRetornoDDLModel();
                    item.Descricao = detalhe.TEXT;
                    item.Title = detalhe.TEXT;
                    item.Valor = detalhe.VALUE;

                    listaRetorno.Add(item);
                }
            }
            else
            {
                var listaDDD= _data.ListaDDLDINAMICO(dadosParametro.NOM_FUNCTION, filtro.ParametroWhere);
                foreach (var detalhe in listaDDD)
                {
                    item = new APIRetornoDDLModel();
                    item.Descricao = detalhe.VALOR;
                    item.Title = detalhe.VALOR;
                    item.Valor = detalhe.ID;

                    listaRetorno.Add(item);
                }
            }


            return listaRetorno;
        }

        private string MontaScriptJob(IRelatorioModel model, string COD_STATUS_REL)
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
                Query.AppendLine("       UPDATE RELATORIO_STATUS SET ERRO = @ERRO, MSG_ERRO = @MSG_ERRO, STATUS = ''FINALIZADO'' WHERE COD_STATUS_REL = ''_COD_STATUS_REL_''    ");
                Query.AppendLine();
                Query.AppendLine("   IF(@ERRO = 666666)                                                                                         ");
                Query.AppendLine("   BEGIN                                                                                                      ");
                Query.AppendLine("      RAISERROR(@MSG_ERRO, 16, 1);                                                                            ");
                Query.AppendLine("   END                                                                                                        ");

                var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC).ToList();

                string param = "";
                foreach (var item in listaParametros)
                {
                    switch (item.TIPO.ToUpper())
                    {
                        case "BYTE":
                            if (param != "") param += ", ";
                            param += "@" + item.NOMPAR + "=" + (item.VALCAMPO ?? "0");
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

                //incluir o ERRO e o msgerro no relatorio


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
            var listaParametros = model.listaCampos.OrderBy(x => x.ORDEM_PROC).ToList();
            RELATORIO_PARAM_UTILIZADO parametros = new RELATORIO_PARAM_UTILIZADO();

            foreach (var item in listaParametros)
            {
                if (item.NOMPAR.ToUpper() == "FORMATO") continue;

                parametros = new RELATORIO_PARAM_UTILIZADO();
                parametros.COD_STATUS_REL = COD_STATUS_REL;
                parametros.NOMPAR = item.NOMPAR;
                parametros.LABEL = item.LABEL;
                parametros.CONTEUDO = item.VALCAMPO ?? "";

                _data.IncluiParametroUtilizado(parametros);

            }

        }
        private void IncluiStatusRel(IRelatorioModel model, string COD_STATUS_REL, int tipoRetorno)
        {
            var _chaveAcesso = MontaChaveAcesso(model.Sistema, model.codigoCliente);

            RELATORIO_STATUS status = new RELATORIO_STATUS();
            status.COD_STATUS_REL = COD_STATUS_REL;
            status.ID_REL = model.idRelatorio;
            status.CHAVEACESSO = _chaveAcesso;
            status.STATUS = "EM_ANDAMENTO";
            status.TIPORETORNO = tipoRetorno;
            status.DATEMISSAO = DateTime.Now;
            status.ERRO = 0;
            status.MSG_ERRO = "";
            _data.IncluiStatusRel(status);
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
        private string MontaChaveAcesso(int sistema, int codigoCliente)
        {
            string chave = string.Empty;

            switch (_user.tp_usu.ToUpper())
            {
                case "CLIENTE":
                    chave = codigoCliente + _user.id_login;
                    break;
                case "CREDENCIADO":
                    chave = sistema + _user.id_login;
                    break;
                case "USUARIO":
                    chave = _user.id_login;
                    break;
                case "PARCERIA":
                    chave = codigoCliente + _user.id_login;
                    break;
                case "OPERADOR":
                    chave = _user.id_login;
                    break;
            }

            return chave;
        }
    }
}
