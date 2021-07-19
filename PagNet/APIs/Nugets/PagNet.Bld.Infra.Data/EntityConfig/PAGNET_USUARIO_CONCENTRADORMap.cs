using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_USUARIO_CONCENTRADORMap : IEntityTypeConfiguration<PAGNET_USUARIO_CONCENTRADOR>
    {
        public void Configure(EntityTypeBuilder<PAGNET_USUARIO_CONCENTRADOR> entity)
        {

            // Primary Key
            entity.HasKey(t => t.CODUSUARIO);

            // Properties
            entity.Property(t => t.NMUSUARIO)
                .HasMaxLength(100);

            entity.Property(t => t.LOGIN)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.SENHA)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(t => t.CPF)
                .HasMaxLength(14);

            entity.Property(t => t.EMAIL)
                .IsFixedLength()
                .HasMaxLength(200);

            entity.Property(t => t.ADMINISTRADOR)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            entity.Property(t => t.ATIVO)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            entity.ToTable("PAGNET_USUARIO");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.NMUSUARIO).HasColumnName("NMUSUARIO");
            entity.Property(t => t.LOGIN).HasColumnName("LOGIN");
            entity.Property(t => t.SENHA).HasColumnName("SENHA");
            entity.Property(t => t.CPF).HasColumnName("CPF");
            entity.Property(t => t.EMAIL).HasColumnName("EMAIL");
            entity.Property(t => t.CODOPE).HasColumnName("CODOPE");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.ADMINISTRADOR).HasColumnName("ADMINISTRADOR");
            entity.Property(t => t.VISIVEL).HasColumnName("VISIVEL");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");

            // Relationships
            entity.HasOne(t => t.OPERADORAS)
                .WithMany(t => t.PAGNET_USUARIO_CONCENTRADOR)
                .HasForeignKey(d => d.CODOPE);

        }
    }
}
