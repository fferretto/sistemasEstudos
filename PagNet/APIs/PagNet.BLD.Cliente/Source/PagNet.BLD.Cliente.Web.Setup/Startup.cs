using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.BLD.Cliente.Abstraction.Interface;
using PagNet.BLD.Cliente.Application;
using PagNet.BLD.Cliente.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.BLD.Cliente.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de CRUD Cliente  1.0.0";
        }

        protected override string GetHostName()
        {
            return "APICrudCliente";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IClienteApp, ClienteApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()
                .AddScoped<IPAGNET_CADCLIENTEService, PAGNET_CADCLIENTEService>()
                .AddScoped<IPAGNET_CADEMPRESAService, PAGNET_CADEMPRESAService>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCAService, PAGNET_INSTRUCAOCOBRANCAService>()
                .AddScoped<IPAGNET_FORMAS_FATURAMENTOService, PAGNET_FORMAS_FATURAMENTOService>();


            services
                .AddScoped<IPAGNET_CADCLIENTERepository, PAGNET_CADCLIENTERepository>()
                .AddScoped<IPAGNET_CADCLIENTE_LOGRepository, PAGNET_CADCLIENTE_LOGRepository>()
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IPAGNET_FORMAS_FATURAMENTORepository, PAGNET_FORMAS_FATURAMENTORepository>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCARepository, PAGNET_INSTRUCAOCOBRANCARepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("CRUDCliente", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
