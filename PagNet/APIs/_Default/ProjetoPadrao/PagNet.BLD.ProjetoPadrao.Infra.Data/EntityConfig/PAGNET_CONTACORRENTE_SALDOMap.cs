using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PAGNET_CONTACORRENTE_SALDOMap : IEntityTypeConfiguration<PAGNET_CONTACORRENTE_SALDO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONTACORRENTE_SALDO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODSALDO);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CONTACORRENTE_SALDO");
            entity.Property(t => t.CODSALDO).HasColumnName("CODSALDO");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.DATLANCAMENTO).HasColumnName("DATLANCAMENTO");
            entity.Property(t => t.SALDO).HasColumnName("SALDO");

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                    .WithMany(t => t.PAGNET_CONTACORRENTE_SALDO)
                    .HasForeignKey(d => d.CODCONTACORRENTE);

            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                    .WithMany(t => t.PAGNET_CONTACORRENTE_SALDO)
                    .HasForeignKey(d => d.CODEMPRESA);
        }
    }
}
