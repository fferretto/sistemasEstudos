using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface;
using Telenet.Bld.ControleAcesso.Web.Setup;
using PagNet.Bld.Cobranca.Bordero.Application;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Cobranca.Bordero.Web.Setup.ContextNegocio;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.Bld.Domain.Services.Procedures;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Repository.Procedures;
using PagNet.Bld.Infra.Data.Repositories.Procedures;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.Cobranca.Bordero.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "PagNet Services - Serviços de borderô de cobrança";
        }

        protected override string GetHostName()
        {
            return "APICobrancaBordero";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IApplication, BorderoApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()

                .AddScoped<IPAGNET_EMISSAOBOLETOService, PAGNET_EMISSAOBOLETOService>()
                .AddScoped<IPAGNET_CADCLIENTEService, PAGNET_CADCLIENTEService>()
                .AddScoped<IPAGNET_CONTACORRENTEService, PAGNET_CONTACORRENTEService>()
                .AddScoped<IPAGNET_BORDERO_BOLETOService, PAGNET_BORDERO_BOLETOService>()
                .AddScoped<IProceduresService, ProceduresService>();

            services
                    .AddScoped<IPAGNET_CADCLIENTERepository, PAGNET_CADCLIENTERepository>()
                    .AddScoped<IPAGNET_CADCLIENTE_LOGRepository, PAGNET_CADCLIENTE_LOGRepository>()
                    .AddScoped<IPAGNET_EMISSAOFATURAMENTORepository, PAGNET_EMISSAOFATURAMENTORepository>()
                    .AddScoped<IPAGNET_EMISSAOBOLETORepository, PAGNET_EMISSAOBOLETORepository>()
                    .AddScoped<IPAGNET_EMISSAOFATURAMENTO_LOGRepository, PAGNET_EMISSAOFATURAMENTO_LOGRepository>()
                    .AddScoped<IPAGNET_BORDERO_BOLETORepository, PAGNET_BORDERO_BOLETORepository>()
                    .AddScoped<IPAGNET_CONTACORRENTERepository, PAGNET_CONTACORRENTERepository>()
                    .AddScoped<IPROC_PAGNET_CONS_FATURAS_BORDERORepository, PROC_PAGNET_CONS_FATURAS_BORDERORepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("BorderoCobranca", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
