using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PAGNET_LOGEMAILENVIADOMap : IEntityTypeConfiguration<PAGNET_LOGEMAILENVIADO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_LOGEMAILENVIADO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODLOGEMAILENVIADO);


            // Table & Column Mappings
            entity.ToTable("PAGNET_LOGEMAILENVIADO");
            entity.Property(t => t.CODLOGEMAILENVIADO).HasColumnName("CODLOGEMAILENVIADO");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.CODCONTAEMAIL).HasColumnName("CODCONTAEMAIL");
            entity.Property(t => t.CODEMISSAOBOLETO).HasColumnName("CODEMISSAOBOLETO");
            entity.Property(t => t.EMAILDESTINO).HasColumnName("EMAILDESTINO");
            entity.Property(t => t.DTENVIO).HasColumnName("DTENVIO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");
            entity.Property(t => t.MENSAGEM).HasColumnName("MENSAGEM");


            entity.HasOne(t => t.PAGNET_CONTAEMAIL)
                .WithMany(t => t.PAGNET_LOGEMAILENVIADO)
                .HasForeignKey(d => d.CODCONTAEMAIL);

            entity.HasOne(t => t.USUARIO_NETCARD)
                .WithMany(t => t.PAGNET_LOGEMAILENVIADO)
                .HasForeignKey(d => d.CODUSUARIO);

            entity.HasOne(t => t.PAGNET_EMISSAOBOLETO)
                .WithMany(t => t.PAGNET_LOGEMAILENVIADO)
                .HasForeignKey(d => d.CODEMISSAOBOLETO);


        }
    }
}