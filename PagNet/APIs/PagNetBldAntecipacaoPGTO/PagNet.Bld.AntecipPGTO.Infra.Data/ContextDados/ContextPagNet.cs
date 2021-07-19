using Microsoft.EntityFrameworkCore;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface;
using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Infra.Data.EntityConfig;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Telenet.BusinessLogicModel.Abstractions;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.ContextDados
{
    public class ContextPagNet : DbContext
    {
        public ContextPagNet(IParametrosApp user, IMessageTable messages)
        {
            _connectionString = user.GetConnectionStringPagNet();
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

        public DbSet<PAGNET_USUARIO> PAGNET_USUARIO { get; set; }
        public DbSet<PAGNET_CADEMPRESA> PAGNET_CADEMPRESA { get; set; }
        public DbSet<PAGNET_CADFAVORECIDO> PAGNET_CADFAVORECIDO { get; set; }
        public DbSet<PAGNET_EMISSAO_TITULOS> PAGNET_EMISSAO_TITULOS { get; set; }
        public DbSet<PAGNET_EMISSAO_TITULOS_LOG> PAGNET_EMISSAO_TITULOS_LOG { get; set; }
        public DbSet<PAGNET_TAXAS_TITULOS> PAGNET_TAXAS_TITULOS { get; set; }
        public DbSet<PAGNET_CONFIG_REGRA_PAG> PAGNET_CONFIG_REGRA_PAG { get; set; }
        public DbSet<PAGNET_CONFIG_REGRA_PAG_LOG> PAGNET_CONFIG_REGRA_PAG_LOG { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {     

            modelBuilder.ApplyConfiguration(new PAGNET_USUARIOMap());
            modelBuilder.ApplyConfiguration(new PAGNET_CADEMPRESAMAP());
            modelBuilder.ApplyConfiguration(new PAGNET_CADFAVORECIDOSMAP());
            modelBuilder.ApplyConfiguration(new PAGNET_EMISSAO_TITULOSMAP());
            modelBuilder.ApplyConfiguration(new PAGNET_EMISSAO_TITULOS_LOGMAP());
            modelBuilder.ApplyConfiguration(new PAGNET_TAXAS_TITULOSMAP());
            modelBuilder.ApplyConfiguration(new PAGNET_CONFIG_REGRA_PAGMAP());
            modelBuilder.ApplyConfiguration(new PAGNET_CONFIG_REGRA_PAG_LOGMAP());

            base.OnModelCreating(modelBuilder);
        }

        public string ConnectionString => _connectionString;

        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
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