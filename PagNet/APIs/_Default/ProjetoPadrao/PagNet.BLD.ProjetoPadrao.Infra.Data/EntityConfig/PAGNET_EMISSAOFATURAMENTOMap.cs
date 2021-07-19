using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PAGNET_EMISSAOFATURAMENTOMap : IEntityTypeConfiguration<PAGNET_EMISSAOFATURAMENTO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_EMISSAOFATURAMENTO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODEMISSAOFATURAMENTO);

            // Table & Column Mappings
            entity.ToTable("PAGNET_EMISSAOFATURAMENTO");           


            // Relationships
            entity.HasOne(t => t.PAGNET_BORDERO_BOLETO)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO)
                .HasForeignKey(d => d.CODBORDERO);

            entity.HasOne(t => t.PAGNET_CADCLIENTE)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO)
                .HasForeignKey(p => p.CODCLIENTE);

            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO)
                .HasForeignKey(d => d.CODEMPRESA);

            entity.HasOne(t => t.PAGNET_FORMAS_FATURAMENTO)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO)
                .HasForeignKey(d => d.CODFORMAFATURAMENTO);

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO)
                .HasForeignKey(d => d.CODCONTACORRENTE);

            entity.HasOne(t => t.PAGNET_CADPLANOCONTAS)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO)
                .HasForeignKey(d => d.CODPLANOCONTAS);




        }
    }
    public class PAGNET_EMISSAOFATURAMENTO_LOGMap : IEntityTypeConfiguration<PAGNET_EMISSAOFATURAMENTO_LOG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_EMISSAOFATURAMENTO_LOG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODEMISSAOFATURAMENTO_LOG);

            // Table & Column Mappings
            entity.ToTable("PAGNET_EMISSAOFATURAMENTO_LOG");


            // Relationships
            entity.HasOne(t => t.USUARIO_NETCARD)
                .WithMany(t => t.PAGNET_EMISSAOFATURAMENTO_LOG)
                .HasForeignKey(d => d.CODUSUARIO);
            
        }
    }

}