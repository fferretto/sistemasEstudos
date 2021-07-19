using NetCardConsulta.Configs.Shared;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Telenet.Core.DependencyInjection;

namespace NetCardConsulta.Configs.Carga
{
    /// <summary>
    /// Atributo de autorização de acesso a processos envolvendo carga.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutorizaSolicitaCargaAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Processa a requisição.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Este filtro deveria ser herdado de AuthorizeAttribute. Entretanto, pela forma como tratamos o redirecionamento de requisições não 
            // autorizadas hoje, utilizamos um ActionFilterAttribute e não um AuthorizeAttribute para evitar um comportamento diferente do 
            // atualmente utilizado na ConsultaWeb.

            var requestSession = ServiceConfiguration.ServiceProvider.GetService<IRequestSession>();
            var action = string.Empty;
            var controller = string.Empty;
            var message = string.Empty;

            if (requestSession.DadosAcesso == null || requestSession.ObjetoConexao == null)
            {
                action = "Autenticar";
                controller = "Logar";
            }

            if (requestSession.Permissao == null || requestSession.Permissao.FCARGA == "N")
            {
                action = "Index";
                controller = "Home";
                message = "Operador não tem permissão para realizar carga";
            }

            if (!string.IsNullOrWhiteSpace(controller))
            {
                var redirectTargetDictionary = new RouteValueDictionary();
                redirectTargetDictionary.Add("area", "");
                redirectTargetDictionary.Add("action", action);
                redirectTargetDictionary.Add("controller", controller);

                if (!string.IsNullOrWhiteSpace(message))
                {
                    redirectTargetDictionary.Add("msg", message);
                }

                filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
            }
        }
    }
}