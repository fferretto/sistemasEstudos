
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface;
using PagNet.Bld.AntecipPGTO.Application;
using PagNet.Bld.AntecipPGTO.Web.Setup.ContextNegocio;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.AntecipPGTO.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de Antecipacao PGTO";
        }

        protected override string GetHostName()
        {
            return "APIAntecipacaoPGTO";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IAntecipacaoApp, AntecipacaoApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoAntecipacaoApp, ContextoAntecipacaoApp>()
                .AddScoped<IPAGNET_CONFIG_REGRAService, PAGNET_CONFIG_REGRAService>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSService, PAGNET_EMISSAO_TITULOSService>()
                .AddScoped<IPAGNET_CADEMPRESAService, PAGNET_CADEMPRESAService>()
                .AddScoped<IPAGNET_TAXAS_TITULOSService, PAGNET_TAXAS_TITULOSService>();


            services
                .AddScoped<IPAGNET_CADFAVORECIDORepository, PAGNET_CADFAVORECIDORepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOS_LOGRepository, PAGNET_EMISSAO_TITULOS_LOGRepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSRepository, PAGNET_EMISSAO_TITULOSRepository>()
                .AddScoped<IPAGNET_TAXAS_TITULOSRepository, PAGNET_TAXAS_TITULOSRepository>()
                .AddScoped<IPAGNET_CONTACORRENTERepository, PAGNET_CONTACORRENTERepository>()
                .AddScoped<IPAGNET_ARQUIVORepository, PAGNET_ARQUIVORepository>()
                .AddScoped<IPAGNET_OCORRENCIARETPAGRepository, PAGNET_OCORRENCIARETPAGRepository>()
                .AddScoped<IOPERADORARepository, OPERADORARepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_PAGRepository, PAGNET_CONFIG_REGRA_PAGRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_PAG_LOGRepository, PAGNET_CONFIG_REGRA_PAG_LOGRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_BOLRepository, PAGNET_CONFIG_REGRA_BOLRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_BOL_LOGRepository, PAGNET_CONFIG_REGRA_BOL_LOGRepository>()
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTORepository, PAGNET_BORDERO_PAGAMENTORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_LOGRepository, PAGNET_CADFAVORECIDO_LOGRepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_CONFIGRepository, PAGNET_CADFAVORECIDO_CONFIGRepository>()
                .AddScoped<IPAGNET_TITULOS_PAGOSRepository, PAGNET_TITULOS_PAGOSRepository>()
                .AddScoped<IPAGNET_CONTACORRENTE_SALDORepository, PAGNET_CONTACORRENTE_SALDORepository>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVORepository, PAGNET_TRANSMISSAOARQUIVORepository>();

            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("AntecipacaoPGTO", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
