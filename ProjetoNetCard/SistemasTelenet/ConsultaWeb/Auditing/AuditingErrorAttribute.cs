using NetCard.Common.Models;
using SIL;
using System.Configuration;
using System.Data;
using System.Web.Mvc;

namespace NetCardConsulta.Auditing
{
    public class AuditingErrorAttribute : ActionFilterAttribute, IExceptionFilter
    {
         

        public AuditingErrorAttribute(string masterView, string errorView)
        {
            _masterView = masterView;
            _errorView = errorView;
        }

        private string _masterView;
        private string _errorView;
        protected DadosAcesso dadosAcesso;
        protected ObjConn objConexao;

        public void OnException(ExceptionContext filterContext)
        {
            AuditingService.CreateAuditingConsultLog(2, filterContext.Exception, CarregaDados());

            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];

            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            filterContext.Result = new ViewResult
            {
                ViewName = _errorView,
                MasterName = _masterView,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
        private DataTable CarregaDados()
        {
            bool.TryParse(ConfigurationManager.AppSettings["enableAuditing"], out bool enableAuditing);

            if (!enableAuditing)
                return null;

            //Criando a tabela
            DataTable dt = new DataTable();

            //Estrutura do DataTable
            dt.Columns.Add("CODOPE");
            dt.Columns.Add("LOGIN");
            dt.Columns.Add("CODCLI");
            dt.Columns.Add("NOMCLI");
            dt.Columns.Add("SERVIDORAUT");
            dt.Columns.Add("SERVIDORCON");
            dt.Columns.Add("SERVIDORNC");
            dt.Columns.Add("BANCOCON");
            dt.Columns.Add("BANCOAUT");
            dt.Columns.Add("BANCONC");
            dt.Columns.Add("ACESSO");
            dt.Columns.Add("STA");

            if (System.Web.HttpContext.Current.Session != null)
            {
                dadosAcesso = (DadosAcesso)System.Web.HttpContext.Current.Session["DadosAcesso"];
                objConexao = (ObjConn)System.Web.HttpContext.Current.Session["ObjConexao"];

                if (objConexao != null && dadosAcesso != null)
                {
                    //Criando Registros 
                    DataRow row = dt.NewRow();

                    row["CODOPE"] = objConexao.CodOpe;
                    row["LOGIN"] = objConexao.LoginWeb ?? "";
                    row["CODCLI"] = objConexao.CodCli ?? "";
                    row["NOMCLI"] = dadosAcesso.Nome ?? "";
                    row["SERVIDORAUT"] = objConexao.ServAutorizador ?? "";
                    row["SERVIDORCON"] = objConexao.ServConcentrador ?? "";
                    row["SERVIDORNC"] = objConexao.ServNertCard ?? "";
                    row["BANCOCON"] = objConexao.BancoAutorizador ?? "";
                    row["BANCOAUT"] = objConexao.BancoAutorizador ?? "";
                    row["BANCONC"] = objConexao.BancoNetcard ?? "";
                    row["ACESSO"] = dadosAcesso.Acesso ?? "";
                    row["STA"] = dadosAcesso.Status ?? "";

                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }
}
