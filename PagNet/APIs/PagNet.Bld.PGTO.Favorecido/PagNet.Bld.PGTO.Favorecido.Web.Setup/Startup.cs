using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.Bld.PGTO.Favorecido.Abstraction.Interface;
using PagNet.Bld.PGTO.Favorecido.Application;
using PagNet.Bld.PGTO.Favorecido.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.PGTO.Favorecido.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de consulta e cadastro de favorecido  1.0.0";
        }

        protected override string GetHostName()
        {
            return "APIFavorecido";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor()
                .AddScoped<ICadastroApp, CadastroApp>()
                .AddScoped<IParametrosApp>(s =>
                {
                    var contexto = s.GetRequiredService<IHttpContextAccessor>();
                    return new ParametrosInfraDataApp(contexto.HttpContext.User);
                });

            services
                .AddScoped<IContextoApp, ContextApp>()

                .AddScoped<IPAGNET_USUARIOService, PAGNET_USUARIOService>()
                .AddScoped<IPAGNET_CADFAVORECIDOService, PAGNET_CADFAVORECIDOService>()
                .AddScoped<IPAGNET_EMISSAOBOLETOService, PAGNET_EMISSAOBOLETOService>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSService, PAGNET_EMISSAO_TITULOSService>()
                .AddScoped<IPAGNET_TAXAS_TITULOSService, PAGNET_TAXAS_TITULOSService>()
                .AddScoped<IPAGNET_CADCLIENTEService, PAGNET_CADCLIENTEService>()
                .AddScoped<INETCARD_USUARIOPOSService, NETCARD_USUARIOPOSService>()
                .AddScoped<INETCARD_USUARIOPREService, NETCARD_USUARIOPREService>()
                .AddScoped<IPAGNET_USUARIO_CONCENTRADORService, PAGNET_USUARIO_CONCENTRADORService>()
                .AddScoped<IPAGNET_CONTACORRENTEService, PAGNET_CONTACORRENTEService>()
                .AddScoped<IPAGNET_OCORRENCIARETPAGService, PAGNET_OCORRENCIARETPAGService>()
                .AddScoped<IPAGNET_OCORRENCIARETBOLService, PAGNET_OCORRENCIARETBOLService>()
                .AddScoped<IOPERADORAService, OPERADORAService>()
                .AddScoped<IPAGNET_BANCOService, PAGNET_BANCOService>()
                .AddScoped<IPAGNET_ARQUIVOService, PAGNET_ARQUIVOService>()
                .AddScoped<IPAGNET_PARAMETRO_RELService, PAGNET_PARAMETRO_RELService>()
                .AddScoped<IPAGNET_CADEMPRESAService, PAGNET_CADEMPRESAService>()
                .AddScoped<IPAGNET_RELATORIOService, PAGNET_RELATORIOService>()
                .AddScoped<IPAGNET_CODIGOOCORRENCIAService, PAGNET_CODIGOOCORRENCIAService>()
                .AddScoped<IPAGNET_ESPECIEDOCService, PAGNET_ESPECIEDOCService>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCAService, PAGNET_INSTRUCAOCOBRANCAService>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTOService, PAGNET_BORDERO_PAGAMENTOService>()
                .AddScoped<IPAGNET_MENUService, PAGNET_MENUService>()
                .AddScoped<IPAGNET_FORMAS_FATURAMENTOService, PAGNET_FORMAS_FATURAMENTOService>()
                .AddScoped<ISUBREDEService, SUBREDEService>()
                .AddScoped<IPAGNET_BORDERO_BOLETOService, PAGNET_BORDERO_BOLETOService>()
                .AddScoped<IPAGNET_CONTAEMAILService, PAGNET_CONTAEMAILService>()
                .AddScoped<IPAGNET_CONFIG_REGRAService, PAGNET_CONFIG_REGRAService>()
                .AddScoped<IPAGNET_TITULOS_PAGOSService, PAGNET_TITULOS_PAGOSService>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVOService, PAGNET_TRANSMISSAOARQUIVOService>()
                .AddScoped<IPAGNET_CADPLANOCONTASService, PAGNET_CADPLANOCONTASService>();


            services
                .AddScoped<INETCARD_USUARIOPOSRepository, NETCARD_USUARIOPOSRepository>()
                .AddScoped<INETCARD_USUARIOPRERepository, NETCARD_USUARIOPRERepository>()
                .AddScoped<IPAGNET_CADCLIENTERepository, PAGNET_CADCLIENTERepository>()
                .AddScoped<IPAGNET_CADCLIENTE_LOGRepository, PAGNET_CADCLIENTE_LOGRepository>()
                .AddScoped<IPAGNET_CADFAVORECIDORepository, PAGNET_CADFAVORECIDORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_CONFIGRepository, PAGNET_CADFAVORECIDO_CONFIGRepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOS_LOGRepository, PAGNET_EMISSAO_TITULOS_LOGRepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSRepository, PAGNET_EMISSAO_TITULOSRepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTORepository, PAGNET_EMISSAOFATURAMENTORepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTO_LOGRepository, PAGNET_EMISSAOFATURAMENTO_LOGRepository>()
                .AddScoped<IPAGNET_TAXAS_TITULOSRepository, PAGNET_TAXAS_TITULOSRepository>()
                .AddScoped<IPAGNET_USUARIO_CONCENTRADORRepository, PAGNET_USUARIO_CONCENTRADORRepository>()
                .AddScoped<IPAGNET_CONTACORRENTERepository, PAGNET_CONTACORRENTERepository>()
                .AddScoped<IPAGNET_CADPLANOCONTASRepository, PAGNET_CADPLANOCONTASRepository>()
                .AddScoped<IPAGNET_TRANSMISSAOARQUIVORepository, PAGNET_TRANSMISSAOARQUIVORepository>()
                .AddScoped<IPAGNET_ARQUIVORepository, PAGNET_ARQUIVORepository>()
                .AddScoped<IPAGNET_BANCORepository, PAGNET_BANCORepository>()
                .AddScoped<IPAGNET_OCORRENCIARETPAGRepository, PAGNET_OCORRENCIARETPAGRepository>()
                .AddScoped<IPAGNET_OCORRENCIARETBOLRepository, PAGNET_OCORRENCIARETBOLRepository>()
                .AddScoped<IPAGNET_USUARIORepository, PAGNET_USUARIORepository>()
                .AddScoped<IOPERADORARepository, OPERADORARepository>()
                .AddScoped<ISUBREDERepository, SUBREDERepository>()
                .AddScoped<IPAGNET_RELATORIORepository, PAGNET_RELATORIORepository>()
                .AddScoped<IPAGNET_PARAMETRO_RELRepository, PAGNET_PARAMETRO_RELRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_PAGRepository, PAGNET_CONFIG_REGRA_PAGRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_PAG_LOGRepository, PAGNET_CONFIG_REGRA_PAG_LOGRepository>()
                .AddScoped<IPAGNET_CADEMPRESARepository, PAGNET_CADEMPRESARepository>()
                .AddScoped<IPAGNET_CODIGOOCORRENCIARepository, PAGNET_CODIGOOCORRENCIARepository>()
                .AddScoped<IPAGNET_EMISSAOBOLETORepository, PAGNET_EMISSAOBOLETORepository>()
                .AddScoped<IPAGNET_FORMAS_FATURAMENTORepository, PAGNET_FORMAS_FATURAMENTORepository>()
                .AddScoped<IPAGNET_ESPECIEDOCRepository, PAGNET_ESPECIEDOCRepository>()
                .AddScoped<IPAGNET_MENURepository, PAGNET_MENURepository>()
                .AddScoped<IPAGNET_LOGEMAILENVIADORepository, PAGNET_LOGEMAILENVIADORepository>()
                .AddScoped<IPAGNET_CONTAEMAILRepository, PAGNET_CONTAEMAILRepository>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCARepository, PAGNET_INSTRUCAOCOBRANCARepository>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTORepository, PAGNET_BORDERO_PAGAMENTORepository>()
                .AddScoped<IPAGNET_BORDERO_BOLETORepository, PAGNET_BORDERO_BOLETORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_LOGRepository, PAGNET_CADFAVORECIDO_LOGRepository>()
                .AddScoped<IPAGNET_TITULOS_PAGOSRepository, PAGNET_TITULOS_PAGOSRepository>()
                .AddScoped<IPAGNET_CONTACORRENTE_SALDORepository, PAGNET_CONTACORRENTE_SALDORepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("PagNetAPIFavorecido", "MessageTable.xml"))
                .AddTransient<ContextConcentrador>()
                .AddTransient<ContextPagNet>();
        }
    }
}
