using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_BORDERO_PAGAMENTOMap : IEntityTypeConfiguration<PAGNET_BORDERO_PAGAMENTO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_BORDERO_PAGAMENTO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODBORDERO);


        // Table & Column Mappings
        entity.ToTable("PAGNET_BORDERO_PAGAMENTO");
            entity.Property(t => t.CODBORDERO).HasColumnName("CODBORDERO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.VLBORDERO).HasColumnName("VLBORDERO");
            entity.Property(t => t.DTBORDERO).HasColumnName("DTBORDERO");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");

            entity.HasOne(t => t.PAGNET_USUARIO)
                .WithMany(t => t.PAGNET_BORDERO_PAGAMENTO)
                .HasForeignKey(d => d.CODUSUARIO);

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                .WithMany(t => t.PAGNET_BORDERO_PAGAMENTO)
                .HasForeignKey(d => d.CODCONTACORRENTE);



        }
    }
}
