using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Repository.Procedures;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Domain.Services.Procedures;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.Bld.Infra.Data.Repositories.Procedures;
using PagNet.BLD.ContaCorrente.Abstraction.Interface;
using PagNet.BLD.ContaCorrente.Web.Setup.ContextNegocio;
using PagNet.BLD.ContaCorrente2;
using PagNet.BLD.ContaCorrente2.Application;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.BLD.ContaCorrente.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "PagNet Services - Serviços de cadastro e consulta de conta corrente  1.0.0";
        }

        protected override string GetHostName()
        {
            return "APIContaCorrente";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<IContaCorrenteApp, ContaCorrenteApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()

                .AddScoped<IPAGNET_EMISSAOBOLETOService, PAGNET_EMISSAOBOLETOService>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSService, PAGNET_EMISSAO_TITULOSService>()
                .AddScoped<IPAGNET_CADCLIENTEService, PAGNET_CADCLIENTEService>()
                .AddScoped<IPAGNET_USUARIOService, PAGNET_USUARIOService>()
                .AddScoped<IPAGNET_CONTACORRENTEService, PAGNET_CONTACORRENTEService>()
                .AddScoped<IOPERADORAService, OPERADORAService>()
                .AddScoped<IPAGNET_BANCOService, PAGNET_BANCOService>()
                .AddScoped<IPAGNET_ARQUIVOService, PAGNET_ARQUIVOService>()
                .AddScoped<IPAGNET_CADEMPRESAService, PAGNET_CADEMPRESAService>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTOService, PAGNET_BORDERO_PAGAMENTOService>()
                .AddScoped<IPAGNET_BORDERO_BOLETOService, PAGNET_BORDERO_BOLETOService>()
                .AddScoped<IPAGNET_TITULOS_PAGOSService, PAGNET_TITULOS_PAGOSService>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVOService, PAGNET_TRANSMISSAOARQUIVOService>()
                .AddScoped<IPAGNET_CADPLANOCONTASService, PAGNET_CADPLANOCONTASService>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCAService, PAGNET_INSTRUCAOCOBRANCAService>();

           


        services
                .AddScoped<IPAGNET_CADCLIENTERepository, PAGNET_CADCLIENTERepository>()
                .AddScoped<IPAGNET_CADCLIENTE_LOGRepository, PAGNET_CADCLIENTE_LOGRepository>()
                .AddScoped<IPAGNET_CADFAVORECIDORepository, PAGNET_CADFAVORECIDORepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOS_LOGRepository, PAGNET_EMISSAO_TITULOS_LOGRepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSRepository, PAGNET_EMISSAO_TITULOSRepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTORepository, PAGNET_EMISSAOFATURAMENTORepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTO_LOGRepository, PAGNET_EMISSAOFATURAMENTO_LOGRepository>()
                .AddScoped<IPAGNET_TAXAS_TITULOSRepository, PAGNET_TAXAS_TITULOSRepository>()
                .AddScoped<IPAGNET_USUARIORepository, PAGNET_USUARIORepository>()
                .AddScoped<IPAGNET_CONTACORRENTERepository, PAGNET_CONTACORRENTERepository>()
                .AddScoped<IPAGNET_CADPLANOCONTASRepository, PAGNET_CADPLANOCONTASRepository>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVORepository, PAGNET_TRANSMISSAOARQUIVORepository>()
                .AddScoped<IPAGNET_ARQUIVORepository, PAGNET_ARQUIVORepository>()
                .AddScoped<IPAGNET_BANCORepository, PAGNET_BANCORepository>()
                .AddScoped<IOPERADORARepository, OPERADORARepository>()
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IPAGNET_EMISSAOBOLETORepository, PAGNET_EMISSAOBOLETORepository>()
                .AddScoped<IPAGNET_FORMAS_FATURAMENTORepository, PAGNET_FORMAS_FATURAMENTORepository>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTORepository, PAGNET_BORDERO_PAGAMENTORepository>()
                .AddScoped<IPROC_PAGNET_CONS_BORDERORepository, PROC_PAGNET_CONS_BORDERORepository>()
                .AddScoped<IPAGNET_BORDERO_BOLETORepository, PAGNET_BORDERO_BOLETORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_LOGRepository, PAGNET_CADFAVORECIDO_LOGRepository>()
                .AddScoped<IPAGNET_TITULOS_PAGOSRepository, PAGNET_TITULOS_PAGOSRepository>()
                .AddScoped<IPROC_PAGNET_INFO_CONTA_CORRENTERepository, PROC_PAGNET_INFO_CONTA_CORRENTERepository>()
                .AddScoped<IPAGNET_CONTACORRENTE_SALDORepository, PAGNET_CONTACORRENTE_SALDORepository>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCARepository, PAGNET_INSTRUCAOCOBRANCARepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("ContaCorrente", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
