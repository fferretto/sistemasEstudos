using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PagNet_Taxas_TitulosMap : IEntityTypeConfiguration<PAGNET_TAXAS_TITULOS>
    {
        public void Configure(EntityTypeBuilder<PAGNET_TAXAS_TITULOS> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODTAXATITULO);

            // Table & Column Mappings
            entity.ToTable("PAGNET_TAXAS_TITULOS");
            entity.Property(t => t.CODTAXATITULO).HasColumnName("CODTAXATITULO");
            entity.Property(t => t.CODTITULO).HasColumnName("CODTITULO");
            entity.Property(t => t.DESCRICAO).HasColumnName("DESCRICAO");
            entity.Property(t => t.VALOR).HasColumnName("VALOR");
            entity.Property(t => t.DTINCLUSAO).HasColumnName("DTINCLUSAO");
            entity.Property(t => t.ORIGEM).HasColumnName("ORIGEM");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");

            entity.HasOne(t => t.PAGNET_EMISSAO_TITULOS)
                        .WithMany(t => t.PAGNET_TAXAS_TITULOS)
                        .HasForeignKey(d => d.CODTITULO);

            entity.HasOne(t => t.USUARIO_NETCARD)
                    .WithMany(t => t.PAGNET_TAXAS_TITULOS)
                    .HasForeignKey(d => d.CODUSUARIO);

        }
    }
}
