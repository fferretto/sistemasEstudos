using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_ARQUIVOMap : IEntityTypeConfiguration<PAGNET_ARQUIVO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_ARQUIVO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODARQUIVO);

            // Properties
            entity.Property(t => t.NMARQUIVO)
                .HasMaxLength(100);

            entity.Property(t => t.TIPARQUIVO)
                .IsFixedLength()
                .HasMaxLength(3);

            entity.Property(t => t.CAMINHOARQUIVO)
                .HasMaxLength(1000);

            // Table & Column Mappings
            entity.ToTable("PAGNET_ARQUIVO");
            entity.Property(t => t.CODARQUIVO).HasColumnName("CODARQUIVO");         
            entity.Property(t => t.NMARQUIVO).HasColumnName("NMARQUIVO");
            entity.Property(t => t.CODBANCO).HasColumnName("CODBANCO");
            entity.Property(t => t.TIPARQUIVO).HasColumnName("TIPARQUIVO");
            entity.Property(t => t.NROSEQARQUIVO).HasColumnName("NROSEQARQUIVO");
            entity.Property(t => t.DTARQUIVO).HasColumnName("DTARQUIVO");
            entity.Property(t => t.CAMINHOARQUIVO).HasColumnName("CAMINHOARQUIVO");
            entity.Property(t => t.VLTOTAL).HasColumnName("VLTOTAL");
            entity.Property(t => t.QTREGISTRO).HasColumnName("QTREGISTRO");



            entity.HasOne(t => t.PAGNET_BANCO)
                .WithMany(t => t.PAGNET_ARQUIVO)
                .HasForeignKey(d => d.CODBANCO);


        }
    }
}
