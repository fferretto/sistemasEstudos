using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Repository.Procedures;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.Bld.Domain.Services;
using PagNet.Bld.Domain.Services.Procedures;
using PagNet.Bld.Infra.Data.ContextDados;
using PagNet.Bld.Infra.Data.Repositories;
using PagNet.Bld.Infra.Data.Repositories.Procedures;
using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface;
using PagNet.Bld.PGTO.ABCBrasil.Application;
using PagNet.Bld.PGTO.ABCBrasil.Web.Setup.ContextNegocio;
using PagNet.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.PGTO.ABCBrasil.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de Emissão e Transmissão de pagamento Banco ABC Basil  1.0.0";
        }

        protected override string GetHostName()
        {
            return "PGTOABCBrasil";
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

                .AddScoped<IPAGNET_USUARIOService, PAGNET_USUARIOService>()
                .AddScoped<IPAGNET_CADFAVORECIDOService, PAGNET_CADFAVORECIDOService>()
                .AddScoped<IPAGNET_EMISSAOBOLETOService, PAGNET_EMISSAOBOLETOService>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSService, PAGNET_EMISSAO_TITULOSService>()
                .AddScoped<IPAGNET_TAXAS_TITULOSService, PAGNET_TAXAS_TITULOSService>()
                .AddScoped<IPAGNET_CADCLIENTEService, PAGNET_CADCLIENTEService>()
                .AddScoped<ITAB_GESTAO_DESCONTO_FOLHAService, TAB_GESTAO_DESCONTO_FOLHAService>()
                .AddScoped<IProceduresService, ProceduresService>()
                .AddScoped<IPAGNET_ARQUIVO_CONCILIACAOService, PAGNET_ARQUIVO_CONCILIACAOService>()
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
                .AddScoped<IPROC_BAIXA_LOTE_CPFRepository, PROC_BAIXA_LOTE_CPFRepository>()
                .AddScoped<IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository, PROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository>()
                .AddScoped<INETCARD_USUARIOPOSRepository, NETCARD_USUARIOPOSRepository>()
                .AddScoped<INETCARD_USUARIOPRERepository, NETCARD_USUARIOPRERepository>()
                .AddScoped<IPAGNET_ARQUIVO_CONCILIACAORepository, PAGNET_ARQUIVO_CONCILIACAORepository>()
                .AddScoped<IPAGNET_PARAM_ARQUIVO_CONCILIACAORepository, PAGNET_PARAM_ARQUIVO_CONCILIACAORepository>()
                .AddScoped<IPAGNET_FORMA_VERIFICACAO_ARQUIVORepository, PAGNET_FORMA_VERIFICACAO_ARQUIVORepository>()
                .AddScoped<IPAGNET_CADCLIENTERepository, PAGNET_CADCLIENTERepository>()
                .AddScoped<IPAGNET_CADCLIENTE_LOGRepository, PAGNET_CADCLIENTE_LOGRepository>()
                .AddScoped<IPAGNET_CADFAVORECIDORepository, PAGNET_CADFAVORECIDORepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOS_LOGRepository, PAGNET_EMISSAO_TITULOS_LOGRepository>()
                .AddScoped<IPAGNET_EMISSAO_TITULOSRepository, PAGNET_EMISSAO_TITULOSRepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTORepository, PAGNET_EMISSAOFATURAMENTORepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTO_LOGRepository, PAGNET_EMISSAOFATURAMENTO_LOGRepository>()
                .AddScoped<IPAGNET_TAXAS_TITULOSRepository, PAGNET_TAXAS_TITULOSRepository>()
                .AddScoped<ITAB_GESTAO_DESCONTO_FOLHARepository, TAB_GESTAO_DESCONTO_FOLHARepository>()

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
                .AddScoped<IPROC_CONFIRMAPGTOCARGARepository, PROC_CONFIRMAPGTOCARGARepository>()
                .AddScoped<IPROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository, PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository>()
                .AddScoped<IPAGNET_ESPECIEDOCRepository, PAGNET_ESPECIEDOCRepository>()
                .AddScoped<IPAGNET_MENURepository, PAGNET_MENURepository>()
                .AddScoped<IPAGNET_LOGEMAILENVIADORepository, PAGNET_LOGEMAILENVIADORepository>()
                .AddScoped<IPAGNET_CONTAEMAILRepository, PAGNET_CONTAEMAILRepository>()
                .AddScoped<IPAGNET_INSTRUCAOCOBRANCARepository, PAGNET_INSTRUCAOCOBRANCARepository>()
                .AddScoped<IPROC_PAGNET_CONS_TRAN_CRERepository, PROC_PAGNET_CONS_TRAN_CRERepository>()
                .AddScoped<IPROC_PAGNET_CONS_TITULOS_VENCIDOSRepository, PROC_PAGNET_CONS_TITULOS_VENCIDOSRepository>()
                .AddScoped<IPROC_PAGNET_INDICADOR_PAGAMENTO_DIARepository, PROC_PAGNET_INDICADOR_PAGAMENTO_DIARepository>()
                .AddScoped<IPROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository, PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository>()
                .AddScoped<IPROC_PAGNET_IND_PAG_REALIZADO_DIARepository, PROC_PAGNET_IND_PAG_REALIZADO_DIARepository>()
                .AddScoped<IPROC_PAGNET_SOLICITACAOBOLETORepository, PROC_PAGNET_SOLICITACAOBOLETORepository>()
                .AddScoped<IPROC_PAGNET_CONSULTABOLETORepository, PROC_PAGNET_CONSULTABOLETORepository>()
                .AddScoped<IPROC_PAGNET_DETALHAMENTO_COBRANCARepository, PROC_PAGNET_DETALHAMENTO_COBRANCARepository>()
                .AddScoped<IPROC_PAGNET_REG_CARGA_CLI_NETCARDRepository, PROC_PAGNET_REG_CARGA_CLI_NETCARDRepository>()
                .AddScoped<IPROC_PAGNET_IND_PAG_ANORepository, PROC_PAGNET_IND_PAG_ANORepository>()
                .AddScoped<IPROC_PAGNET_CONSULTA_TITULOSRepository, PROC_PAGNET_CONSULTA_TITULOSRepository>()
                .AddScoped<IPAGNET_BORDERO_PAGAMENTORepository, PAGNET_BORDERO_PAGAMENTORepository>()
                .AddScoped<IPROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository, PROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository>()
                .AddScoped<IPROC_PAGNET_CONSCARGA_PGTORepository, PROC_PAGNET_CONSCARGA_PGTORepository>()
                .AddScoped<IPROC_PAGNET_CONS_BORDERORepository, PROC_PAGNET_CONS_BORDERORepository>()
                .AddScoped<IPAGNET_BORDERO_BOLETORepository, PAGNET_BORDERO_BOLETORepository>()
                .AddScoped<IPAGNET_CADFAVORECIDO_LOGRepository, PAGNET_CADFAVORECIDO_LOGRepository>()
                .AddScoped<IPAGNET_TITULOS_PAGOSRepository, PAGNET_TITULOS_PAGOSRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_BOL_LOGRepository, PAGNET_CONFIG_REGRA_BOL_LOGRepository>()
                .AddScoped<IPAGNET_CONFIG_REGRA_BOLRepository, PAGNET_CONFIG_REGRA_BOLRepository>()
                .AddScoped<IPROC_PAGNET_MAIORES_RECEITASRepository, PROC_PAGNET_MAIORES_RECEITASRepository>()
                .AddScoped<IPROC_PAGNET_MAIORES_DESPESASRepository, PROC_PAGNET_MAIORES_DESPESASRepository>()
                .AddScoped<IPROC_PAGNET_INFO_CONTA_CORRENTERepository, PROC_PAGNET_INFO_CONTA_CORRENTERepository>()
                .AddScoped<IPAGNET_CONTACORRENTE_SALDORepository, PAGNET_CONTACORRENTE_SALDORepository>()
                .AddScoped<IPROC_PAGNET_EXTRATO_BANCARIORepository, PROC_PAGNET_EXTRATO_BANCARIORepository>()
                .AddScoped<IPROC_PAGNET_BUSCA_USUARIO_NCRepository, PROC_PAGNET_BUSCA_USUARIO_NCRepository>()
                .AddScoped<IMW_CONSCEPRepository, MW_CONSCEPRepository>();


            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("DescontoEmFolha", "MessageTable.xml"))
                .AddTransient<ContextConcentrador, ContextConcentrador>(s => new ContextConcentrador(Configuration.GetConnectionString("ConnectionStringConcentrador")))
                .AddTransient<ContextPagNet>();
        }
    }
}
