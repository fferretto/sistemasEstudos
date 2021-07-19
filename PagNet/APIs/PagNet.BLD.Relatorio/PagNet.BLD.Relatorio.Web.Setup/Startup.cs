using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.BLD.Relatorio.Abstraction.Interface;
using PagNet.BLD.Relatorio.Job;
using PagNet.BLD.Relatorio.Job.Application;
using PagNet.BLD.Relatorio.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;
using Telenet.Extensions.Data;
using Telenet.Extensions.Data.SqlClient;

namespace PagNet.BLD.Relatorio.Web.Setup
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
        internal class DbConcentradorContext : DbClientContextBase
        {
            public DbConcentradorContext(IConfiguration configuration)
                : base("Concentrador", configuration.GetConnectionString("Concentrador"))
            { }
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IRelatorioApp, RelatorioApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()
                .AddScoped<IPAGNET_PARAMETRO_RELService, PAGNET_PARAMETRO_RELService>()
                .AddScoped<IPAGNET_RELATORIOService, PAGNET_RELATORIOService>();               


            services
                .AddScoped<IPAGNET_RELATORIORepository, PAGNET_RELATORIORepository>()
                .AddScoped<IPAGNET_PARAMETRO_RELRepository, PAGNET_PARAMETRO_RELRepository>()
                .AddScoped<IPAGNET_RELATORIO_PARAM_UTILIZADORepository, PAGNET_RELATORIO_PARAM_UTILIZADORepository>()
                .AddScoped<IPAGNET_RELATORIO_RESULTADORepository, PAGNET_RELATORIO_RESULTADORepository>()
                .AddScoped<IPAGNET_RELATORIO_STATUSRepository, PAGNET_RELATORIO_STATUSRepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("Relatorio", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();

            //services.AddSqlClient<DbNetCardContext>();
            services.AddSqlClient<DbNetCardContext>();
        }
    }
}
