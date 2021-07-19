using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_OcorrenciaRetPagMap : IEntityTypeConfiguration<PAGNET_OCORRENCIARETPAG>
    {
        public void Configure(EntityTypeBuilder<PAGNET_OCORRENCIARETPAG> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codOcorrenciaRetPag);

            // Properties
            entity.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(t => t.codOcorrenciaRetPag)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            entity.ToTable("PAGNET_OCORRENCIARETPAG");
            entity.Property(t => t.codOcorrenciaRetPag).HasColumnName("codOcorrenciaRetPag");
            entity.Property(t => t.Descricao).HasColumnName("Descricao");
        }

    }
}
