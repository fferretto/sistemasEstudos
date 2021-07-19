using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.Bld.Usuario.Abstraction.Interface;
using PagNet.Bld.Usuario.Application;
using PagNet.Bld.Usuario.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.Usuario.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "PagNet Services - Serviços de cadastro e consulta de Usuário";
        }

        protected override string GetHostName()
        {
            return "APIUsuario";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IUsuarioApp, UsuarioApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()

                .AddScoped<IPAGNET_USUARIOService, PAGNET_USUARIOService>()
                .AddScoped<IPAGNET_USUARIO_CONCENTRADORService, PAGNET_USUARIO_CONCENTRADORService>()
                .AddScoped<IPAGNET_CADEMPRESAService, PAGNET_CADEMPRESAService>()
                .AddScoped<IOPERADORAService, OPERADORAService>();

            services
                .AddScoped<IPAGNET_USUARIO_CONCENTRADORRepository, PAGNET_USUARIO_CONCENTRADORRepository>()
                .AddScoped<IPAGNET_USUARIORepository, PAGNET_USUARIORepository>()
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IOPERADORARepository, OPERADORARepository>();

            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("Usuario", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
