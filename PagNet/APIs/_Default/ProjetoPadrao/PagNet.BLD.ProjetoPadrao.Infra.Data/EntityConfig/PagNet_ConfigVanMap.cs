using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PagNet_ConfigVanMap : IEntityTypeConfiguration<PAGNET_CONFIGVAN>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONFIGVAN> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODCONFIGVAN);


            // Table & Column Mappings
            entity.ToTable("PAGNET_CONFIGVAN");
            entity.Property(t => t.CODCONFIGVAN).HasColumnName("CODCONFIGVAN");
            entity.Property(t => t.CODCONTACORRENTE).HasColumnName("CODCONTACORRENTE");
            entity.Property(t => t.ENVIABOLETO).HasColumnName("ENVIABOLETO");
            entity.Property(t => t.ENVIAPAGAMENTO).HasColumnName("ENVIAPAGAMENTO");
            entity.Property(t => t.USUARIOVAN).HasColumnName("USUARIOVAN");
            entity.Property(t => t.CODATIVACAO).HasColumnName("CODATIVACAO");
            entity.Property(t => t.CAIXAPOSTAL).HasColumnName("CAIXAPOSTAL");
            entity.Property(t => t.DIRETORIORAIZ).HasColumnName("DIRETORIORAIZ");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");

            entity.HasOne(t => t.PAGNET_CONTACORRENTE)
                .WithMany(t => t.PAGNET_CONFIGVAN)
                .HasForeignKey(d => d.CODCONTACORRENTE);
        }
    }
}