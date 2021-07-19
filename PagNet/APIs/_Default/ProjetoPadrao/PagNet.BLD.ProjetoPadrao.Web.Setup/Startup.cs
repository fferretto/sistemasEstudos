using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Procedures;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Procedures;
using PagNet.BLD.ProjetoPadrao.Domain.Services;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Procedure;
using PagNet.BLD.ProjetoPadrao.Infra.Data.ContextDados;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories;
using PagNet.BLD.ProjetoPadrao.Infra.Data.Repositories.Procedures;
using PagNet.BLD.ProjetoPadrao.Web.Setup.ContextNegocio;
using Telenet.Bld.ControleAcesso.Web.Setup;
using Telenet.BusinessLogicModel;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.BLD.ProjetoPadrao.Web.Setup
{
    public class Startup : ControleAcessoClientStartupBase
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        { }

        protected override string GetHostDescription()
        {
            return "Telenet Services - Serviços de Antecipacao PGTO 1.0.0";
        }

        protected override string GetHostName()
        {
            return "Antecipação";
        }

        protected override void OnConfigureServices(IServiceCollection services)
        {
            base.OnConfigureServices(services);

            services
                .AddHttpContextAccessor();

            //services
            //    .AddScoped<IAntecipacaoApp, AntecipacaoApp>()
            //    .AddScoped<IParametrosApp>(s =>
            //    {
            //        var contexto = s.GetRequiredService<IHttpContextAccessor>();
            //        return new ParametrosApp(contexto.HttpContext.User); //Quando o OAuth for implementado
            //    });

            services
                .AddScoped<IContextoAntecipacaoApp, ContextApp>()

                .AddScoped<IPagNet_UsuarioService, PagNet_UsuarioService>()
                .AddScoped<IUsuario_NetCardService, Usuario_NetCardService>()
                .AddScoped<IPagNet_ContaCorrenteService, PagNet_ContaCorrenteService>()
                .AddScoped<IPagNet_OcorrenciaRetPagService, PagNet_OcorrenciaRetPagService>()
                .AddScoped<IPagNet_OcorrenciaRetBolService, PagNet_OcorrenciaRetBolService>()
                .AddScoped<IPagNet_BancoService, PagNet_BancoService>()
                .AddScoped<IPagNet_ArquivoService, PagNet_ArquivoService>()
                .AddScoped<IPagNet_Parametro_RelService, PagNet_Parametro_RelService>()
                .AddScoped<IPagNet_CadEmpresaService, PagNet_CadEmpresaService>()
                .AddScoped<IPagNet_CadFavorecidoService, PagNet_CadFavorecidoService>()
                .AddScoped<IPagNet_RelatorioService, PagNet_RelatorioService>()
                .AddScoped<IPagNet_CodigoOcorrenciaService, PagNet_CodigoOcorrenciaService>()
                .AddScoped<IPagNet_EmissaoBoletoService, PagNet_EmissaoBoletoService>()
                .AddScoped<IPagNet_EspecieDocService, PagNet_EspecieDocService>()
                .AddScoped<IPagNet_InstrucaoCobrancaService, PagNet_InstrucaoCobrancaService>()
                .AddScoped<IPagNet_Bordero_PagamentoService, PagNet_Bordero_PagamentoService>()
                .AddScoped<IPagNet_MenuService, PagNet_MenuService>()
                .AddScoped<IPagNet_Formas_FaturamentoService, PagNet_Formas_FaturamentoService>()
                .AddScoped<IPagNet_Bordero_BoletoService, PagNet_Bordero_BoletoService>()
                .AddScoped<IPagNet_ContaEmailService, PagNet_ContaEmailService>()
                .AddScoped<IPagNet_Emissao_TitulosService, PagNet_Emissao_TitulosService>()
                .AddScoped<IPagNet_Config_RegraService, PagNet_Config_RegraService>()
                .AddScoped<IPagNet_Titulos_PagosService, PagNet_Titulos_PagosService>()
                .AddScoped<IPagNet_Taxas_TitulosService, PagNet_Taxas_TitulosService>()
                .AddScoped<IPagNet_CadClienteService, PagNet_CadClienteService>()
                .AddScoped<IProceduresService, ProceduresService>()
                .AddScoped<IPagNet_CadPlanoContasService, PagNet_CadPlanoContasService>()
                .AddScoped<IPagNet_Arquivo_ConciliacaoService, PagNet_Arquivo_ConciliacaoService>();

            services
                .AddScoped<IPagNet_UsuarioRepository, PagNet_UsuarioRepository>()
                .AddScoped<IPagNet_ContaCorrenteRepository, PagNet_ContaCorrenteRepository>()
                .AddScoped<IPagNet_CadPlanoContasRepository, PagNet_CadPlanoContasRepository>()
                .AddScoped<IPagNet_ConfigVanRepository, PagNet_ConfigVanRepository>()
                .AddScoped<IPagNet_ArquivoRepository, PagNet_ArquivoRepository>()
                .AddScoped<IPagNet_BancoRepository, PagNet_BancoRepository>()
                .AddScoped<IPagNet_OcorrenciaRetPagRepository, PagNet_OcorrenciaRetPagRepository>()
                .AddScoped<IPagNet_OcorrenciaRetBolRepository, PagNet_OcorrenciaRetBolRepository>()
                .AddScoped<IUsuario_NetCardRepository, Usuario_NetCardRepository>()
                .AddScoped<IPagNet_RelatorioRepository, PagNet_RelatorioRepository>()
                .AddScoped<IPagNet_Parametro_RelRepository, PagNet_Parametro_RelRepository>()
                .AddScoped<IPagNet_Config_Regra_PagRepository, PagNet_Config_Regra_PagRepository>()
                .AddScoped<IPagNet_Config_Regra_Pag_LogRepository, PagNet_Config_Regra_Pag_LogRepository>()
                .AddScoped<IPagNet_CadEmpresaRepository, PagNet_CadEmpresaRepository>()
                .AddScoped<IPagNet_CadFavorecidoRepository, PagNet_CadFavorecidoRepository>()
                .AddScoped<IPagNet_CodigoOcorrenciaRepository, PagNet_CodigoOcorrenciaRepository>()
                .AddScoped<IPagNet_EmissaoBoletoRepository, PagNet_EmissaoBoletoRepository>()
                .AddScoped<IPAGNET_FORMAS_FATURAMENTORepository, PAGNET_FORMAS_FATURAMENTORepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTORepository, PAGNET_EMISSAOFATURAMENTORepository>()
                .AddScoped<IPAGNET_EMISSAOFATURAMENTO_LOGRepository, PAGNET_EMISSAOFATURAMENTO_LOGRepository>()
                .AddScoped<IPROC_CONFIRMAPGTOCARGARepository, PROC_CONFIRMAPGTOCARGARepository>()
                .AddScoped<IPROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository, PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCRepository>()
                .AddScoped<IPagNet_EspecieDocRepository, PagNet_EspecieDocRepository>()
                .AddScoped<IPagNet_MenuRepository, PagNet_MenuRepository>()
                .AddScoped<IPagNet_LogEmailEnviadoRepository, PagNet_LogEmailEnviadoRepository>()
                .AddScoped<IPagNet_ContaEmailRepository, PagNet_ContaEmailRepository>()
                .AddScoped<IPagNet_InstrucaoCobrancaRepository, PagNet_InstrucaoCobrancaRepository>()
                .AddScoped<IProc_PagNet_Cons_Tran_CreRepository, Proc_PagNet_Cons_Tran_CreRepository>()
                .AddScoped<IPROC_PAGNET_CONS_TITULOS_VENCIDOSRepository, PROC_PAGNET_CONS_TITULOS_VENCIDOSRepository>()
                .AddScoped<IProc_PagNet_Indicador_Pagamento_DiaRepository, Proc_PagNet_Indicador_Pagamento_DiaRepository>()
                .AddScoped<IPROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository, PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANORepository>()
                .AddScoped<IPROC_PAGNET_IND_PAG_REALIZADO_DIARepository, PROC_PAGNET_IND_PAG_REALIZADO_DIARepository>()
                .AddScoped<IPROC_PAGNET_SOLICITACAOBOLETORepository, PROC_PAGNET_SOLICITACAOBOLETORepository>()
                .AddScoped<IPROC_PAGNET_CONSULTABOLETORepository, PROC_PAGNET_CONSULTABOLETORepository>()
                .AddScoped<IPROC_PAGNET_DETALHAMENTO_COBRANCARepository, PROC_PAGNET_DETALHAMENTO_COBRANCARepository>()
                .AddScoped<IPROC_PAGNET_REG_CARGA_CLI_NETCARDRepository, PROC_PAGNET_REG_CARGA_CLI_NETCARDRepository>()
                .AddScoped<IPROC_PAGNET_IND_PAG_ANORepository, PROC_PAGNET_IND_PAG_ANORepository>()
                .AddScoped<IPROC_PAGNET_CONSULTA_TITULOSRepository, PROC_PAGNET_CONSULTA_TITULOSRepository>()
                .AddScoped<IPagNet_Bordero_PagamentoRepository, PagNet_Bordero_PagamentoRepository>()
                .AddScoped<IPROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository, PROC_PAGNET_INDI_RECEB_PREVISTA_DIARepository>()
                .AddScoped<IPROC_PAGNET_CONSCARGA_PGTORepository, PROC_PAGNET_CONSCARGA_PGTORepository>()
                .AddScoped<IPROC_PAGNET_CONS_BORDERORepository, PROC_PAGNET_CONS_BORDERORepository>()
                .AddScoped<IPagNet_Bordero_BoletoRepository, PagNet_Bordero_BoletoRepository>()
                .AddScoped<IPagNet_CadFavorecido_LogRepository, PagNet_CadFavorecido_LogRepository>()
                .AddScoped<IPagNet_Emissao_TitulosRepository, PagNet_Emissao_TitulosRepository>()
                .AddScoped<IPagNet_Emissao_Titulos_LogRepository, PagNet_Emissao_Titulos_LogRepository>()
                .AddScoped<IPagNet_CadClienteRepository, PagNet_CadClienteRepository>()
                .AddScoped<IPagNet_CadCliente_LogRepository, PagNet_CadCliente_LogRepository>()
                .AddScoped<IPagNet_Titulos_PagosRepository, PagNet_Titulos_PagosRepository>()
                .AddScoped<IPagNet_Taxas_TitulosRepository, PagNet_Taxas_TitulosRepository>()
                .AddScoped<IPagNet_Config_Regra_Bol_LogRepository, PagNet_Config_Regra_Bol_LogRepository>()
                .AddScoped<IPagNet_Config_Regra_BolRepository, PagNet_Config_Regra_BolRepository>()
                .AddScoped<IPROC_PAGNET_MAIORES_RECEITASRepository, PROC_PAGNET_MAIORES_RECEITASRepository>()
                .AddScoped<IPROC_PAGNET_MAIORES_DESPESASRepository, PROC_PAGNET_MAIORES_DESPESASRepository>()
                .AddScoped<IPROC_PAGNET_INFO_CONTA_CORRENTERepository, PROC_PAGNET_INFO_CONTA_CORRENTERepository>()
                .AddScoped<IPagNet_ContaCorrente_SaldoRepository, PagNet_ContaCorrente_SaldoRepository>()
                .AddScoped<IPROC_PAGNET_EXTRATO_BANCARIORepository, PROC_PAGNET_EXTRATO_BANCARIORepository>()
                .AddScoped<IPagNet_Param_Arquivo_ConciliacaoRepository, PagNet_Param_Arquivo_ConciliacaoRepository>()
                .AddScoped<IPagNet_Arquivo_ConciliacaoRepository, PagNet_Arquivo_ConciliacaoRepository>()
                .AddScoped<IPagNet_Forma_Verificacao_ArquivoRepository, PagNet_Forma_Verificacao_ArquivoRepository>()
                .AddScoped<IPROC_PAGNET_BUSCA_USUARIO_NCRepository, PROC_PAGNET_BUSCA_USUARIO_NCRepository>()
                .AddScoped<IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository, PROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository>();

            services
                .AddSingleton<IMessageTable>(s => new XmlFileDomainMessageTable("AntecipacaoPGTO", "MessageTable.xml"))
                .AddTransient<ContextPagNet>();
        }
    }
}
