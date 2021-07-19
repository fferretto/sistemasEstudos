using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;
namespace PagNet.Infra.Data.EntityConfig
{
    public class SUBREDEMap : IEntityTypeConfiguration<SUBREDE>
    {
        public void Configure(EntityTypeBuilder<SUBREDE> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODSUBREDE);


            // Table & Column Mappings
            entity.ToTable("SUBREDE");
            entity.Property(t => t.CODSUBREDE).HasColumnName("CODSUBREDE");
            entity.Property(t => t.NOMSUBREDE).HasColumnName("NOMSUBREDE");

        }
    }
}
