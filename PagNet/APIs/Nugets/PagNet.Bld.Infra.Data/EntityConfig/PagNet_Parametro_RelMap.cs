using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PagNet_Parametro_RelMap : IEntityTypeConfiguration<PAGNET_PARAMETRO_REL>
    {
        public void Configure(EntityTypeBuilder<PAGNET_PARAMETRO_REL> entity)
        {
            // Primary Key
            entity.HasKey(t => t.ID_PAR);
            
            // Table & Column Mappings
            entity.ToTable("PAGNET_PARAMETRO_REL");
            entity.Property(t => t.ID_PAR).HasColumnName("ID_PAR");
            entity.Property(t => t.ID_REL).HasColumnName("ID_REL");
            entity.Property(t => t.DESPAR).HasColumnName("DESPAR");
            entity.Property(t => t.NOMPAR).HasColumnName("NOMPAR");
            entity.Property(t => t.LABEL).HasColumnName("LABEL");
            entity.Property(t => t.TIPO).HasColumnName("TIPO");
            entity.Property(t => t.TAMANHO).HasColumnName("TAMANHO");
            entity.Property(t => t._DEFAULT).HasColumnName("_DEFAULT");
            entity.Property(t => t.REQUERIDO).HasColumnName("REQUERIDO");
            entity.Property(t => t.ORDEM_TELA).HasColumnName("ORDEM_TELA");
            entity.Property(t => t.ORDEM_PROC).HasColumnName("ORDEM_PROC");
            entity.Property(t => t.NOM_FUNCTION).HasColumnName("NOM_FUNCTION");
            entity.Property(t => t.MASCARA).HasColumnName("MASCARA");
            entity.Property(t => t.TEXTOAJUDA).HasColumnName("TEXTOAJUDA");
            

        entity.HasOne(t => t.PAGNET_RELATORIO)
                .WithMany(t => t.PAGNET_PARAMETRO_REL)
                .HasForeignKey(d => d.ID_REL);

        }
    }
}
