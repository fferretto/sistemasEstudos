using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_API_PGTOMap : IEntityTypeConfiguration<PAGNET_API_PGTO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_API_PGTO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODIGOAPI);


            // Table & Column Mappings
            entity.ToTable("PAGNET_API_PGTO");
            entity.Property(t => t.CODIGOAPI).HasColumnName("CODIGOAPI");
            entity.Property(t => t.CAMINHO).HasColumnName("CAMINHO");
            entity.Property(t => t.CODIGOBANCO).HasColumnName("CODIGOBANCO");

        }
    }
}
