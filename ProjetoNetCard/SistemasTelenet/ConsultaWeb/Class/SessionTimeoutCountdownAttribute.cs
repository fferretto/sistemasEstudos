using System;
using System.Web.Mvc;

namespace NetCardConsulta.Class
{
    public class SessionTimeoutCountdownAttribute : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            try
            {
                var timeout = TimeSpan.FromMinutes(filterContext.HttpContext.Session.Timeout);
                filterContext.Controller.ViewBag.SessionHours = timeout.Hours;
                filterContext.Controller.ViewBag.SessionMinutes = timeout.Minutes;
                filterContext.Controller.ViewBag.SessionSeconds = timeout.Seconds;
            }
            catch { }
        }
    }
}
