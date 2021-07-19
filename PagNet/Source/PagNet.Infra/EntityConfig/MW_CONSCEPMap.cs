using PagNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PagNet.Infra.Data.EntityConfig
{
    public class MW_CONSCEPMap : IEntityTypeConfiguration<MW_CONSCEP>
    {
        public void Configure(EntityTypeBuilder<MW_CONSCEP> entity)
        {
            // Primary Key
            entity.HasKey(t => t.RETORNO);

            // Table & Column Mappings
            entity.ToTable("MW_CONSCEP");
            entity.Property(t => t.RETORNO).HasColumnName("RETORNO");
            entity.Property(t => t.UF).HasColumnName("UF");
            entity.Property(t => t.LOCALIDADE).HasColumnName("LOCALIDADE");
            entity.Property(t => t.BAIRRO).HasColumnName("BAIRRO");
            entity.Property(t => t.LOGRADOURO).HasColumnName("LOGRADOURO");

        }
    }
}