

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
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface;
using PagNet.Bld.PGTO.ArquivoRemessa.Web.Setup.ContextNegocio;
using PagNet.Bld.PGTO.ArquivoRemessa2;
using PagNet.Bld.PGTO.ArquivoRemessa2.Application;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.PGTO.ArquivoRemessa.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de Emissao e Transmissao de Arquivo de Remessa de Pagamento";
        }

        protected override string GetHostName()
        {
            return "GeracaoArquivogto";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IArquivoRemessa, ArquivoRemessaApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()

                .AddScoped<IPAGNET_CADFAVORECIDOService, PAGNET_CADFAVORECIDOService>()
                .AddScoped<IOPERADORAService, OPERADORAService>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSService, PAGNET_EMISSAO_TITULOSService>()
                .AddScoped<IPAGNET_TAXAS_TITULOSService, PAGNET_TAXAS_TITULOSService>()
                .AddScoped<IPAGNET_CONTACORRENTEService, PAGNET_CONTACORRENTEService>()
                .AddScoped<IPAGNET_OCORRENCIARETPAGService, PAGNET_OCORRENCIARETPAGService>()
                .AddScoped<IPAGNET_ARQUIVOService, PAGNET_ARQUIVOService>()
                .AddScoped<IPAGNET_CADEMPRESAService, PAGNET_CADEMPRESAService>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTOService, PAGNET_BORDERO_PAGAMENTOService>()
                .AddScoped<IPAGNET_TITULOS_PAGOSService, PAGNET_TITULOS_PAGOSService>()
                .AddScoped<IPAGNET_API_PGTOService, PAGNET_API_PGTOService>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVOService, PAGNET_TRANSMISSAOARQUIVOService>();


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
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTORepository, PAGNET_BORDERO_PAGAMENTORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_LOGRepository, PAGNET_CADFAVORECIDO_LOGRepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_CONFIGRepository, PAGNET_CADFAVORECIDO_CONFIGRepository>()
                .AddScoped<IPAGNET_TITULOS_PAGOSRepository, PAGNET_TITULOS_PAGOSRepository>()
                .AddScoped<IPAGNET_CONTACORRENTE_SALDORepository, PAGNET_CONTACORRENTE_SALDORepository>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVORepository, PAGNET_TRANSMISSAOARQUIVORepository>()
                .AddScoped<IPAGNET_API_PGTORepository, PAGNET_API_PGTORepository>()
                .AddScoped<IPROC_PAGNET_CONS_BORDERORepository, PROC_PAGNET_CONS_BORDERORepository>();

            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("GeracaoArquivoRemesa", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
