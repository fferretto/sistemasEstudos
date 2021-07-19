using System.Web.Mvc;

namespace NetCardConsulta.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new Auditing.AuditingErrorAttribute("~/Views/Shared/_Layout.cshtml", "~/Views/Shared/Error.cshtml"));
            filters.Add(new Class.SessionTimeoutCountdownAttribute());
            
        }
    }
}