using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_ESPECIEDOCMap : IEntityTypeConfiguration<PAGNET_ESPECIEDOC>
    {
        public void Configure(EntityTypeBuilder<PAGNET_ESPECIEDOC> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codEspecieDoc);

            // Properties
            entity.Property(t => t.nmEspecieDoc)
                .IsFixedLength()
                .HasMaxLength(100);

            // Table & Column Mappings
            entity.ToTable("PAGNET_ESPECIEDOC");
            entity.Property(t => t.codEspecieDoc).HasColumnName("codEspecieDoc");
            entity.Property(t => t.nmEspecieDoc).HasColumnName("nmEspecieDoc");
        }
    }
}
