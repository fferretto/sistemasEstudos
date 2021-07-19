using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PAGNET_BANCOMap : IEntityTypeConfiguration<PAGNET_BANCO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_BANCO> entity)
        {
            entity.ToTable("PAGNET_BANCO", "dbo");
            // Primary Key
            entity.HasKey(t => t.CODBANCO);

            // Properties
            entity.Property(t => t.NMBANCO)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.POSSUIVAN)
                .IsRequired()
                .HasMaxLength(1);

            entity.Property(t => t.ATIVO)
                .IsRequired()
                .HasMaxLength(1);
        }
    }
}
