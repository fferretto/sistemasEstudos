using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_TRANSMISSAOARQUIVOMap : IEntityTypeConfiguration<PAGNET_TRANSMISSAOARQUIVO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_TRANSMISSAOARQUIVO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODTRANSMISSAOARQUIVO);


            // Table & Column Mappings
            entity.ToTable("PAGNET_TRANSMISSAOARQUIVO");
            entity.Property(t => t.CODTRANSMISSAOARQUIVO).HasColumnName("CODTRANSMISSAOARQUIVO");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");
            entity.Property(t => t.TIPOARQUIVO).HasColumnName("TIPOARQUIVO");
            entity.Property(t => t.FORMATRANSMISSAO).HasColumnName("FORMATRANSMISSAO");
            entity.Property(t => t.LOGINTRANSMISSAO).HasColumnName("LOGINTRANSMISSAO");
            entity.Property(t => t.SENHATRANSMISSAO).HasColumnName("SENHATRANSMISSAO");
            entity.Property(t => t.CAMINHOREM).HasColumnName("CAMINHOREM");
            entity.Property(t => t.CAMINHORET).HasColumnName("CAMINHORET");
            entity.Property(t => t.CAMINHOAUX).HasColumnName("CAMINHOAUX");

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                    .WithMany(t => t.PAGNET_CONFIGVAN)
                    .HasForeignKey(d => d.CODCONTACORRENTE);
        }
    }
}