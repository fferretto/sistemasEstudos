using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Repository.Procedures;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.Bld.Infra.Data.Repositories.Procedures;
using PagNet.Bld.PGTO.Bradesco.Abstraction.Interface;
using PagNet.Bld.PGTO.Bradesco.Application;
using PagNet.Bld.PGTO.Bradesco.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.PGTO.Bradesco.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Servi?os de Transmissao PGTO Bradesco  1.0.0";
        }

        protected override string GetHostName()
        {
            return "APIBradesco";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IPagamentoApp, PagamentoApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()
                .AddScoped<IPAGNET_TITULOS_PAGOSService, PAGNET_TITULOS_PAGOSService>();


            services
                .AddScoped<IPAGNET_CADFAVORECIDORepository, PAGNET_CADFAVORECIDORepository>()
                .AddScoped<IPAGNET_CONTACORRENTERepository, PAGNET_CONTACORRENTERepository>()
                .AddScoped<IPAGNET_ARQUIVORepository, PAGNET_ARQUIVORepository>()
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTORepository, PAGNET_BORDERO_PAGAMENTORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_LOGRepository, PAGNET_CADFAVORECIDO_LOGRepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_CONFIGRepository, PAGNET_CADFAVORECIDO_CONFIGRepository>()
                .AddScoped<IPAGNET_TITULOS_PAGOSRepository, PAGNET_TITULOS_PAGOSRepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("TransmissaoBradesco", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
