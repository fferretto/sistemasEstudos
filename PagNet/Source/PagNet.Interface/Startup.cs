using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Api.Service.Interface;
using PagNet.Api.Service.Interface.Common;
using PagNet.Api.Service.Service;
using PagNet.Application.Application;
using PagNet.Application.Cnab.BancoABCBrasil;
using PagNet.Application.Cnab.BancoBradesco;
using PagNet.Application.Cnab.BancoBrasil;
using PagNet.Application.Cnab.BancoCEF;
using PagNet.Application.Cnab.BancoItau;
using PagNet.Application.Cnab.BancoSantander;
using PagNet.Application.Cnab.Boleto;
using PagNet.Application.Interface;
using PagNet.Application.Interface.ProcessoCnab;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Repository.Procedures;
using PagNet.Domain.Interface.Services;
using PagNet.Domain.Interface.Services.Procedures;
using PagNet.Domain.Services;
using PagNet.Domain.Services.Procedures;
using PagNet.Infra.Data.Context;
using PagNet.Infra.Data.Repositories;
using PagNet.Infra.Data.Repositories.Procedures;
using PagNet.Interface.Helpers;
using Rotativa.AspNetCore;
using Telenet.AspNetCore.Mvc.Authentication;
using Telenet.AspNetCore.Mvc.Core;
using Telenet.Extensions.Logging;

namespace PagNet.Interface
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProcessLogger(Configuration);
            services.AddRequestLog();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpContextAccessor();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddScoped<IAPIAntecipacaoPGTO, AntecipacaoAppClient>()
                .AddScoped<IAPIGestaoDescontoFolhaAppClient, APIGestaoDescontoFolhaAppClient>()
                .AddScoped<IAPIClienteAPP, APIClienteAPP>()                
                .AddScoped<IAPITrasmissaoPagamento, APITrasmissaoPagamento>()
                .AddScoped<IAPIContaCorrente, APIContaCorrente>()                
                .AddScoped<IAPIFavorecido, APIFavorecido>()
                .AddScoped<IAPIRelatorioApp, APIRelatorioApp>()
                .AddScoped<IAPITransmissaoCobrancaBancaria, APITransmissaoCobrancaBancaria>()
                .AddScoped<IAPICobrancaBordero, APICobrancaBordero>()
                .AddScoped<IAPIUsuario, APIUsuario>();
                       

            services
                .AddScoped<IDiversosApp, DiversosApp>()
                .AddScoped<ICadastrosApp, CadastrosApp>()          
                .AddScoped<IValidaUsuarioApp, ValidaUsuarioApp>()
                .AddScoped<IUsuarioApp, UsuarioApp>()
                .AddScoped<IRelatorioApp, RelatorioApp>()
                .AddScoped<IIndicadoresApp, IndicadoresApp>()
                .AddScoped<IPagamentoApp, PagamentoApp>()
                .AddScoped<IContaEmailApp, ContaEmailApp>()
                .AddScoped<IInstrucaoEmailApp, InstrucaoEmailApp>()
                .AddScoped<IRecebimentoApp, RecebimentoApp>()
                .AddScoped<IAparenciaSistemaApp, AparenciaSistemaApp>()                
                .AddScoped<IProcessaDadosSantander, ProcessaDadosSantander>()
                .AddScoped<IProcessaDadosBancoBrasil, ProcessaDadosBancoBrasil>()
                .AddScoped<IProcessaDadosBradesco, ProcessaDadosBradesco>()
                .AddScoped<IProcessaDadosCEF, ProcessaDadosCEF>()
                .AddScoped<IProcessaDadosItau, ProcessaDadosItau>()
                .AddScoped<IProcessaDadosBrasilABC, ProcessaDadosBrasilABC>()
                .AddScoped<IRegraNegocioApp, RegraNegocioApp>()
                .AddScoped<IConfiguracaoApp, ConfiguracaoApp>()
                .AddScoped<ITesourariaApp, TesourariaApp>()
                .AddScoped<IAjudaApp, AjudaApp>()                
                .AddScoped<IArquivoRemessaBoleto, ArquivoRemessaBoleto>();



            services
                .AddScoped<IPagNet_UsuarioService, PagNet_UsuarioService>()
                .AddScoped<IUsuario_NetCardService, Usuario_NetCardService>()
                .AddScoped<IPagNet_ContaCorrenteService, PagNet_ContaCorrenteService>()
                .AddScoped<IPagNet_OcorrenciaRetPagService, PagNet_OcorrenciaRetPagService>()
                .AddScoped<IPagNet_OcorrenciaRetBolService, PagNet_OcorrenciaRetBolService>()
                .AddScoped<IOperadoraService, OperadoraService>()
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
                .AddScoped<ISubRedeService, SubRedeService>()
                .AddScoped<IPagNet_Bordero_BoletoService, PagNet_Bordero_BoletoService>()
                .AddScoped<IPagNet_ContaEmailService, PagNet_ContaEmailService>()
                .AddScoped<IPagNet_InstrucaoEmailService, PagNet_InstrucaoEmailService>()
                .AddScoped<IPagNet_Emissao_TitulosService, PagNet_Emissao_TitulosService>()
                .AddScoped<IPagNet_Config_RegraService, PagNet_Config_RegraService>()
                .AddScoped<IPagNet_Titulos_PagosService, PagNet_Titulos_PagosService>()
                .AddScoped<IPagNet_Taxas_TitulosService, PagNet_Taxas_TitulosService>()                
                .AddScoped<IPagNet_CadClienteService, PagNet_CadClienteService>()
                .AddScoped<IProceduresService, ProceduresService>()
                .AddScoped<IPagNet_CadPlanoContasService, PagNet_CadPlanoContasService>()
                .AddScoped<IPAGNET_ARQUIVO_DESCONTOFOLHAService, PAGNET_ARQUIVO_DESCONTOFOLHAService>();
            
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
                .AddScoped<IOperadoraRepository, OperadoraRepository>()
                .AddScoped<ISubRedeRepository, SubRedeRepository>()
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
                .AddScoped<IPagNet_InstrucaoEmailRepository, PagNet_InstrucaoEmailRepository>()
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
                .AddScoped<IPAGNET_PARAM_ARQUIVO_DESCONTOFOLHARepository, PAGNET_PARAM_ARQUIVO_DESCONTOFOLHARepository>()
                .AddScoped<IPAGNET_ARQUIVO_DESCONTOFOLHARepository, PAGNET_ARQUIVO_DESCONTOFOLHARepository>()
                .AddScoped<IPAGNET_FORMA_VERIFICACAO_DFRepository, PAGNET_FORMA_VERIFICACAO_DFRepository>()
                .AddScoped<IPROC_PAGNET_BUSCA_USUARIO_NCRepository, PROC_PAGNET_BUSCA_USUARIO_NCRepository>()
                .AddScoped<IPROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository, PROC_PAGNET_INC_CLIENTE_USUARIO_NCRepository>()
                .AddScoped<IMW_CONSCEPRepository, MW_CONSCEPRepository>();


            services
                .AddTransient<IPagNetUser, PagNetUser>()
                //.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
                .AddTransient<ContextConcentrador, ContextConcentrador>(s => new ContextConcentrador(Configuration.GetConnectionString("ConnectionStringConcentrador")))
                .AddTransient<ContextAutorizador>()
                .AddTransient<ContextNetCard>();

            services
                .AddScoped<MetodosGerais>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var authOptions = new ClientAuthenticationOptions(Configuration);
            authOptions.ApplicationName = "PagNet";
            authOptions.LoginPath = new PathString("/identificacao/autenticacao/login");

            //Ao realizar a publicação, será necessário realizar a alteração deste campo.
            //#if RELEASE
            //authOptions.ApplicationPath = new PathString("/PagNet");
            //authOptions.ApplicationPath = new PathString("/PagNetTeste");
            //#endif

            services.AddClientAuthentication(Configuration, authOptions);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLog("PagNet");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            RotativaConfiguration.Setup(env, "Rotativa");

            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            Helpers.UrlHelperExtensions.Configure(httpContextAccessor);

            app.UseHttpsRedirection();
            app.UseClientAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "areaRoute",
                     template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(
                //     name: "areaRoute",
                //     template: "{area=Identificacao}/{controller=Autenticacao}/{action=Logar}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Configuração do rotativa
            // Isso serve para que o rotativa utilize os arquivos presentes na pasta wwwroot/Rotativa
            RotativaConfiguration.Setup(env);
        }
    }
}
