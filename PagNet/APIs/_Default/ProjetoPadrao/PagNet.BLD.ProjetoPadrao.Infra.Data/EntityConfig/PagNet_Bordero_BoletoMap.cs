using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_Bordero_BoletoMap : IEntityTypeConfiguration<PAGNET_BORDERO_BOLETO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_BORDERO_BOLETO> entity)
        {
            // Primary Key
            entity.HasKey(t => new { t.CODBORDERO });

            // Table & Column Mappings
            entity.ToTable("PAGNET_BORDERO_BOLETO");
            entity.Property(t => t.CODBORDERO).HasColumnName("CODBORDERO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.VLBORDERO).HasColumnName("VLBORDERO");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.DTBORDERO).HasColumnName("DTBORDERO");


            // Relationships
            entity.HasOne(t => t.USUARIO_NETCARD)
                .WithMany(t => t.PAGNET_BORDERO_BOLETO)
                .HasForeignKey(d => d.CODUSUARIO);

            entity.HasOne(t => t.PAGNET_CADEMPRESA)
                .WithMany(t => t.PAGNET_BORDERO_BOLETO)
                .HasForeignKey(d => d.CODEMPRESA);


        }
    }
}