using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Infra.Data.EntityConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using Telenet.BusinessLogicModel.Abstractions;
using Telenet.Data;

namespace PagNet.Bld.Infra.Data.ContextDados
{
    //--------------CONTEXTO BASE para usar JOB DbClientContextBase--------------------------

    /// <summary>
    /// Classe de contexto de conexão para um objeto DbClient. Já cria a string de conexão com o de->para de nome e ip de servidor.
    /// </summary>

    public class DbNetCardContext : IDbClientContext
    {
        public DbNetCardContext(IServiceProvider user)
        {
            _user = user;
        }
        protected readonly IServiceProvider _user;
        public string ConnectionString
        {
            get
            {
                var service = _user.CreateScope().ServiceProvider.GetService(typeof(IParametrosApp)) as IParametrosApp;
                return service.GetConnectionStringPagNet();
            }
        }
        public string Name => "NetCard";
    }
    //--------------CONTEXTO BASE DE DADOS DO CONCENTRADOR--------------------------
    public class ContextConcentrador : DbContext
    {
        public ContextConcentrador(IParametrosApp user, IMessageTable messages)
        {
            _connectionString = user.GetConnectionStringConcentrador();
        }

        private readonly string _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                var connString = _connectionString;
                optionsBuilder
                    .EnableSensitiveDataLogging(false)
                    .UseSqlServer(connString, options => options.MaxBatchSize(150));
            }

            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<OPERADORA> Operadora { get; set; }
        public DbSet<PAGNET_USUARIO_CONCENTRADOR> PAGNET_USUARIO_CONCENTRADOR { get; set; }
        public DbSet<MW_CONSCEP> MW_CONSCEP { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OPERADORAMap());
            modelBuilder.ApplyConfiguration(new PAGNET_USUARIO_CONCENTRADORMap());
            modelBuilder.ApplyConfiguration(new MW_CONSCEPMap());

            base.OnModelCreating(modelBuilder);
        }

        public string ConnectionString => _connectionString;
    }
    //--------------------------------------CONTEXTO BASE DE DADOS DO PAGNET-------------------
    public class ContextPagNet : DbContext
    {
        public ContextPagNet(IParametrosApp user, IMessageTable messages)
        {
            _connectionString = user.GetConnectionStringPagNet();
            var connectionStringBuilder = new SqlConnectionStringBuilder(_connectionString);
            GetNameBD = connectionStringBuilder.InitialCatalog;

        }

        private readonly string _connectionString;
        public string GetNameBD
        {
            get;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                var connString = _connectionString;
                optionsBuilder
                    .EnableSensitiveDataLogging(false)
                    .UseSqlServer(connString, options => options.MaxBatchSize(150));
            }

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<PAGNET_TRANSMISSAOARQUIVO> PAGNET_TRANSMISSAOARQUIVO { get; set; }
        public DbSet<PAGNET_USUARIO> PAGNET_USUARIO { get; set; }
        public DbSet<PAGNET_CONTACORRENTE> PagNet_ContaCorrente { get; set; }
        public DbSet<PAGNET_CONTACORRENTE_SALDO> PagNet_ContaCorrente_Saldo { get; set; }
        public DbSet<PAGNET_BANCO> PAGNET_BANCO { get; set; }
        public DbSet<PAGNET_ARQUIVO> PAGNET_ARQUIVO { get; set; }
        public DbSet<PAGNET_RELATORIO> PAGNET_RELATORIO { get; set; }
        public DbSet<PAGNET_RELATORIO_STATUS> PAGNET_RELATORIO_STATUS { get; set; }
        public DbSet<PAGNET_RELATORIO_RESULTADO> PAGNET_RELATORIO_RESULTADO { get; set; }
        public DbSet<PAGNET_RELATORIO_PARAM_UTILIZADO> PAGNET_RELATORIO_PARAM_UTILIZADO { get; set; }
        public DbSet<PAGNET_PARAMETRO_REL> PAGNET_PARAMETRO_REL { get; set; }
        public DbSet<PAGNET_OCORRENCIARETPAG> PAGNET_OCORRENCIARETPAG { get; set; }
        public DbSet<PAGNET_OCORRENCIARETBOL> PAGNET_OCORRENCIARETBOL { get; set; }
        public DbSet<PAGNET_CODIGOOCORRENCIA> PAGNET_CODIGOOCORRENCIA { get; set; }
        public DbSet<PAGNET_EMISSAOBOLETO> PAGNET_EMISSAOBOLETO { get; set; }
        public DbSet<PAGNET_EMISSAOFATURAMENTO> PAGNET_EMISSAOFATURAMENTO { get; set; }
        public DbSet<PAGNET_EMISSAOFATURAMENTO_LOG> PAGNET_EMISSAOFATURAMENTO_LOG { get; set; }
        public DbSet<PAGNET_FORMAS_FATURAMENTO> PAGNET_FORMAS_FATURAMENTO { get; set; }
        public DbSet<PAGNET_CADPLANOCONTAS> PAGNET_CADPLANOCONTAS { get; set; }

        public DbSet<PAGNET_ESPECIEDOC> PAGNET_ESPECIEDOC { get; set; }
        public DbSet<PAGNET_INSTRUCAOCOBRANCA> PAGNET_INSTRUCAOCOBRANCA { get; set; }
        public DbSet<PAGNET_MENU> PAGNET_MENU { get; set; }
        public DbSet<PAGNET_BORDERO_PAGAMENTO> PAGNET_BORDERO_PAGAMENTO { get; set; }
        public DbSet<PAGNET_CONTAEMAIL> PAGNET_CONTAEMAIL { get; set; }
        public DbSet<PAGNET_LOGEMAILENVIADO> PAGNET_LOGEMAILENVIADO { get; set; }
        public DbSet<PAGNET_CADEMPRESA> PAGNET_CADEMPRESA { get; set; }
        public DbSet<PAGNET_CADCLIENTE> PAGNET_CADCLIENTE { get; set; }
        public DbSet<PAGNET_CADCLIENTE_LOG> PAGNET_CADCLIENTE_LOG { get; set; }
        public DbSet<PAGNET_CADFAVORECIDO> PAGNET_CADFAVORECIDO { get; set; }
        public DbSet<PAGNET_CADFAVORECIDO_LOG> PAGNET_CADFAVORECIDO_LOG { get; set; }
        public DbSet<PAGNET_CADFAVORECIDO_CONFIG> PAGNET_CADFAVORECIDO_CONFIG { get; set; }
        public DbSet<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
        public DbSet<PAGNET_EMISSAO_TITULOS_LOG> PAGNET_EMISSAO_TITULOS_LOG { get; set; }
        public DbSet<PAGNET_TITULOS_PAGOS> PAGNET_TITULOS_PAGOS { get; set; }
        public DbSet<PAGNET_TAXAS_TITULOS> PAGNET_TAXAS_TITULOS { get; set; }

        public DbSet<PAGNET_BORDERO_BOLETO> PAGNET_BORDERO_BOLETO { get; set; }
        public DbSet<PAGNET_CONFIG_REGRA_PAG> PAGNET_CONFIG_REGRA_PAG { get; set; }
        public DbSet<PAGNET_CONFIG_REGRA_PAG_LOG> PAGNET_CONFIG_REGRA_PAG_LOG { get; set; }

        public DbSet<SUBREDE> SUBREDE { get; set; }
               
        public DbSet<PROC_PAGNET_CONS_TITULOS_BORDERO> PROC_PAGNET_CONS_TITULOS_BORDERO { get; set; }
        public DbSet<PROC_PAGNET_BUSCA_USUARIO_NC> PROC_PAGNET_BUSCA_USUARIO_NC { get; set; }
        public DbSet<PROC_PAGNET_INC_CLIENTE_USUARIO_NC> PROC_PAGNET_INC_CLIENTE_USUARIO_NC { get; set; }
        
        public DbSet<PROC_PAGNET_CONS_TITULOS_VENCIDOS> PROC_PAGNET_CONS_TITULOS_VENCIDOS { get; set; }
        public DbSet<PROC_PAGNET_CONS_BORDERO> PROC_PAGNET_CONS_BORDERO { get; set; }
        public DbSet<PROC_PAGNET_SOLICITACAOBOLETO> PROC_PAGNET_SOLICITACAOBOLETO { get; set; }
        public DbSet<PROC_PAGNET_CONSULTABOLETO> PROC_PAGNET_CONSULTABOLETO { get; set; }
        public DbSet<PROC_PAGNET_DETALHAMENTO_COBRANCA> PROC_PAGNET_DETALHAMENTO_COBRANCA { get; set; }
        public DbSet<PROC_PAGNET_REG_CARGA_CLI_NETCARD> PROC_PAGNET_REG_CARGA_CLI_NETCARD { get; set; }
        public DbSet<PROC_PAGNET_CONS_FATURAS_BORDERO> PROC_PAGNET_CONS_FATURAS_BORDERO { get; set; }
        public DbSet<PROC_PAGNET_CONSCARGA_PGTO> PROC_PAGNET_CONSCARGA_PGTO { get; set; }

        public DbSet<PROC_PAGNET_MAIORES_RECEITAS> PROC_PAGNET_MAIORES_RECEITAS { get; set; }
        public DbSet<PROC_PAGNET_MAIORES_DESPESAS> PROC_PAGNET_MAIORES_DESPESAS { get; set; }
        public DbSet<PROC_PAGNET_INFO_CONTA_CORRENTE> PROC_PAGNET_INFO_CONTA_CORRENTE { get; set; }
        public DbSet<PROC_PAGNET_EXTRATO_BANCARIO> PROC_PAGNET_EXTRATO_BANCARIO { get; set; }
        public DbSet<PROC_PAGNET_CONSULTA_TITULOS> PROC_PAGNET_CONSULTA_TITULOS { get; set; }

        public DbSet<PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO> PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO { get; set; }
        public DbSet<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA> PROC_PAGNET_INDI_RECEB_PREVISTA_DIA { get; set; }
        public DbSet<PROC_PAGNET_IND_PAG_PREVISTO_DIA> PROC_PAGNET_IND_PAG_PREVISTO_DIA { get; set; }
        public DbSet<PROC_PAGNET_IND_PAG_REALIZADO_DIA> PROC_PAGNET_IND_PAG_REALIZADO_DIA { get; set; }
        public DbSet<PROC_PAGNET_IND_PAG_ANO> PROC_PAGNET_IND_PAG_ANO { get; set; }
        public DbSet<PROC_CONFIRMAPGTOCARGA> PROC_CONFIRMAPGTOCARGA { get; set; }
        public DbSet<PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC> PROC_PAGNET_INC_FAVORECIDO_USUARIO_NC { get; set; }
        public DbSet<PROC_PAGNET_LIBERA_DESCONTOFOLHA> PROC_PAGNET_LIBERA_DESCONTOFOLHA { get; set; }
        public DbSet<PAGNET_API_PGTO> PAGNET_API_PGTO { get; set; }

        public DbSet<NETCARD_USUARIOPRE> NetCard_UsuarioPre { get; set; }
        public DbSet<NETCARD_USUARIOPOS> NetCard_UsuarioPos { get; set; }
        public DbSet<PROC_BAIXA_LOTE_CPF> PROC_BAIXA_LOTE_CPF { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new NetCard_UsuarioPreMap());
            modelBuilder.ApplyConfiguration(new NetCard_UsuarioPosMap());
            modelBuilder.ApplyConfiguration(new PROC_BAIXA_LOTE_CPFMap());


            modelBuilder.ApplyConfiguration(new PAGNET_USUARIOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_CONTACORRENTEMap());
            modelBuilder.ApplyConfiguration(new PAGNET_CONTACORRENTE_SALDOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_TRANSMISSAOARQUIVOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_BANCOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_ARQUIVOMap());
            modelBuilder.ApplyConfiguration(new PagNet_OcorrenciaRetPagMap());
            modelBuilder.ApplyConfiguration(new PagNet_OcorrenciaRetBolMap());
            modelBuilder.ApplyConfiguration(new PagNet_Config_Regra_PagMAP());
            modelBuilder.ApplyConfiguration(new PagNet_Config_Regra_Pag_LogMAP());

            modelBuilder.ApplyConfiguration(new PAGNET_CODIGOOCORRENCIAMap());
            modelBuilder.ApplyConfiguration(new PAGNET_EMISSAOBOLETOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_EMISSAOFATURAMENTOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_EMISSAOFATURAMENTO_LOGMap());
            modelBuilder.ApplyConfiguration(new PAGNET_FORMAS_FATURAMENTOMap());
            modelBuilder.ApplyConfiguration(new PROC_CONFIRMAPGTOCARGAMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_INC_FAVORECIDO_USUARIO_NCMap());
            modelBuilder.ApplyConfiguration(new PAGNET_ESPECIEDOCMap());
            modelBuilder.ApplyConfiguration(new PAGNET_INSTRUCAOCOBRANCAMap());
            modelBuilder.ApplyConfiguration(new PAGNET_MENUMap());
            modelBuilder.ApplyConfiguration(new PAGNET_BORDERO_PAGAMENTOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_CONTAEMAILMap());
            modelBuilder.ApplyConfiguration(new PAGNET_LOGEMAILENVIADOMap());
            modelBuilder.ApplyConfiguration(new PagNet_CadEmpresaMap());
            modelBuilder.ApplyConfiguration(new PagNet_CadFavorecidosMap());
            modelBuilder.ApplyConfiguration(new PagNet_CadClienteMap());
            modelBuilder.ApplyConfiguration(new PagNet_CadCliente_LogMap());
            modelBuilder.ApplyConfiguration(new PagNet_CadPlanoContasMap());
            
            modelBuilder.ApplyConfiguration(new PAGNET_CADFAVORECIDO_CONFIGMAP());
            modelBuilder.ApplyConfiguration(new PagNet_CadFavorecidos_LogMap());
            modelBuilder.ApplyConfiguration(new PagNet_Emissao_TitulosMap());
            modelBuilder.ApplyConfiguration(new PagNet_Taxas_TitulosMap());
            modelBuilder.ApplyConfiguration(new PagNet_Emissao_Titulos_LogMap());
            modelBuilder.ApplyConfiguration(new PagNet_Titulos_PagosMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_REG_CARGA_CLI_NETCARDMap());
            
            modelBuilder.ApplyConfiguration(new SUBREDEMap());
            modelBuilder.ApplyConfiguration(new PagNet_RelatorioMap());
            modelBuilder.ApplyConfiguration(new PagNet_Parametro_RelMap());

            modelBuilder.ApplyConfiguration(new PAGNET_RELATORIO_STATUSMap());
            modelBuilder.ApplyConfiguration(new PAGNET_RELATORIO_RESULTADOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_RELATORIO_PARAM_UTILIZADOMap());
            
            modelBuilder.ApplyConfiguration(new PagNet_Bordero_BoletoMap());

            modelBuilder.ApplyConfiguration(new PAGNET_API_PGTOMap());
            


            //Procedures
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONS_TITULOS_BORDEROMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONS_TITULOS_VENCIDOSMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONS_BORDEROMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONSCARGA_PGTOMap());

            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONSULTA_TITULOSMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANOMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_IND_PAG_PREVISTO_DIAMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_INDI_RECEB_PREVISTA_DIAMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_IND_PAG_ANOMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_IND_PAG_REALIZADO_DIAMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_SOLICITACAOBOLETOMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONSULTABOLETOMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_DETALHAMENTO_COBRANCAMap());

            modelBuilder.ApplyConfiguration(new PROC_PAGNET_MAIORES_RECEITASMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_MAIORES_DESPESASMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_INFO_CONTA_CORRENTEMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_EXTRATO_BANCARIOMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_BUSCA_USUARIO_NCMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_INC_CLIENTE_USUARIO_NCMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_LIBERA_DESCONTOFOLHAMap());
            modelBuilder.ApplyConfiguration(new PROC_PAGNET_CONS_FATURAS_BORDEROMap());

            

            base.OnModelCreating(modelBuilder);
        }

        public string ConnectionString => _connectionString;

        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                               || e.State == EntityState.Deleted
                           select e.Entity;
            var validationResults = new List<ValidationResult>();
            foreach (var entity in entities)
            {
                if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                {
                    // throw new ValidationException() or do whatever you want
                }
            }

            return base.SaveChanges();
        }
    }

}