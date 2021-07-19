using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PagNet_CadPlanoContasMap : IEntityTypeConfiguration<PAGNET_CADPLANOCONTAS>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CADPLANOCONTAS> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODPLANOCONTAS);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CADPLANOCONTAS");
            entity.Property(t => t.CODPLANOCONTAS).HasColumnName("CODPLANOCONTAS");
            entity.Property(t => t.CODPLANOCONTAS_PAI).HasColumnName("CODPLANOCONTAS_PAI");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.TIPO).HasColumnName("TIPO");
            entity.Property(t => t.NATUREZA).HasColumnName("NATUREZA");
            entity.Property(t => t.DESCRICAO).HasColumnName("DESCRICAO");

            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                .WithMany(t => t.PAGNET_CADPLANOCONTAS)
                .HasForeignKey(d => d.CODEMPRESA);

        }
    }
}