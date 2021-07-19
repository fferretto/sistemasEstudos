using NetCard.Common.Util;
using NetCardConsulta.Class;
using NetCardConsulta.Configs.Carga;
using NetCardConsulta.Configs.Shared;
using System.Configuration;
using Telenet.Carga;
using Telenet.Carga.Abstractions;
using Telenet.Core.Authorization;
using Telenet.Core.Data;
using Telenet.Core.DependencyInjection;
using Telenet.Core.Web;

namespace NetCardConsulta.App_Start
{
    /// <summary>
    /// Configura o serviço de DI da aplicação.
    /// </summary>
    public static class ServiceConfig
    {
        /// <summary>
        /// Efetua a configuração dos serviços.
        /// </summary>
        public static void ConfigureServices(MvcApplication app)
        {
            ServiceConfiguration
                .Provider(new MvcServiceProvider())

                // Infra
                .AddScoped<ISessionAccessor, SessionAccessor>()
                .AddScoped<IRequestSession, RequestSession>()
                .AddScoped<IAcessoDados, AcessoDados>()

                // Autorização
                .AddAuthorizarion(options =>
                {
                    options
                        .UseServerAddress(ConfigurationManager.AppSettings["OAuthAuthenticationServerAddress"])
                        .AddUserAuthorization<ConsultaWebAuthorizationContext>();
                })

                // Serviços
                .AddScoped<IContextoCarga, ContextoCarga>()
                .AddScoped<IContextoCargaManual, ContextoCargaManual>()
                .AddScoped<IContextoConsultaCarga, ContextoConsultaCarga>()

                .AddScoped<ISolicitaCargaConfig, SolicitaCargaConfig>()

                .AddScoped<IConsultasCarga, ConsultasCarga>()
                .AddScoped<ISolicitacaoCargaArquivo, SolicitacaoCargaArquivo>()
                .AddScoped<ISolicitacaoCargaManual, SolicitacaoCargaManual>();
        }
    }
}