using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCard.Bld.Relatorio.Abstraction.Interface;
using NetCard.Bld.Relatorio.Application;
using NetCard.Bld.Relatorio.Context;
using NetCard.Bld.Relatorio.Data;
using NetCard.Bld.Relatorio.Interface;
using NetCard.Bld.Relatorio.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;
using Telenet.Extensions.Data;
using Telenet.Extensions.Data.SqlClient;

namespace NetCard.Bld.Relatorio.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de Geração de Relatório";
        }

        protected override string GetHostName()
        {
            return "APIRelatorio";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IRelatorioApp, RelatorioApp>()
                .AddScoped<IDadosApp, DadosApp>()

                
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>();

            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("Relatorio", "MessageTable.xml"));


            services
                .AddSqlClient<DbConcentradorContext>()
                .AddSqlClient<DbNetCardContext>();
        }
    }
}
