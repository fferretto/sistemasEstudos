using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_CODIGOOCORRENCIAMap : IEntityTypeConfiguration<PAGNET_CODIGOOCORRENCIA>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CODIGOOCORRENCIA> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codOcorrencia);

            // Properties
            entity.Property(t => t.nmOcorrencia)
                .IsFixedLength()
                .HasMaxLength(100);

            // Table & Column Mappings
            entity.ToTable("PAGNET_CODIGOOCORRENCIA");
            entity.Property(t => t.codOcorrencia).HasColumnName("codOcorrencia");
            entity.Property(t => t.nmOcorrencia).HasColumnName("nmOcorrencia");
        }
    }
}
