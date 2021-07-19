using System.Web.Mvc;
using System.Web.Routing;

namespace NetCardConsulta.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Cliente",
                url: "{controller}/{action}/{operadora}",
                defaults: new { controller = "Home", action = "Cliente", operadora = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Autenticar",
                url: "{controller}/{action}/{acao}",
                defaults: new { controller = "Autenticar", action = "LogarUsuario", acao = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ConsultaRedeImprimir",
                url: "{controller}/{action}/{segmento}/{estado}/{cidade}/{bairro}",
                defaults: new { controller = "ConsultaRede", action = "Imprimir", segmento = UrlParameter.Optional, estado = UrlParameter.Optional, cidade = UrlParameter.Optional, bairro = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PrimeiroAcesso",
                url: "{controller}/{action}/{cartao}",
                defaults: new { controller = "Autenticar", action = "EPrimeiroAcesso" }
            );


            routes.MapPageRoute("WebFormThing", "Relatórios", "~/SelRelParametros.aspx");
        }
    }
}