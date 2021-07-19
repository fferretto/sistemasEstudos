using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;


namespace PagNet.Infra.Data.EntityConfig
{
    public class PagNet_OcorrenciaRetBolMap : IEntityTypeConfiguration<PAGNET_OCORRENCIARETBOL>
    {
        public void Configure(EntityTypeBuilder<PAGNET_OCORRENCIARETBOL> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codOcorrenciaRetBol);

            // Properties
            entity.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(t => t.codOcorrenciaRetBol)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            entity.ToTable("PAGNET_OCORRENCIARETBOL");
            entity.Property(t => t.codOcorrenciaRetBol).HasColumnName("codOcorrenciaRetBol");
            entity.Property(t => t.Descricao).HasColumnName("Descricao");
        }

    }
}
