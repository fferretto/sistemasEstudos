using PagNet.Bld.AntecipPGTO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.EntityConfig
{
    public class PAGNET_USUARIOMap : IEntityTypeConfiguration<PAGNET_USUARIO>
    {
        public void Configure(EntityTypeBuilder<PAGNET_USUARIO> entity)
        {
            // Primary Key
            entity.HasKey(t => t.CODUSUARIO);


            // Table & Column Mappings
            entity.ToTable("PAGNET_USUARIO");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.NMUSUARIO).HasColumnName("NMUSUARIO");
            entity.Property(t => t.LOGIN).HasColumnName("LOGIN");
            entity.Property(t => t.SENHA).HasColumnName("SENHA");
            entity.Property(t => t.CPF).HasColumnName("CPF");
            entity.Property(t => t.EMAIL).HasColumnName("EMAIL");
            entity.Property(t => t.ADMINISTRADOR).HasColumnName("ADMINISTRADOR");
            entity.Property(t => t.VISIVEL).HasColumnName("VISIVEL");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");

        }
    }
}
