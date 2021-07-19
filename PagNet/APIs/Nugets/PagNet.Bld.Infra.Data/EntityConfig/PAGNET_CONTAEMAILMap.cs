using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_CONTAEMAILMap : IEntityTypeConfiguration<PAGNET_CONTAEMAIL>
    {
        public void Configure(EntityTypeBuilder<PAGNET_CONTAEMAIL> entity)
        {
            // Primary Key

            entity.HasKey(t => t.CODCONTAEMAIL);

            // Hack pois o método ValueGeneratedOnAdd do EF Core ainda não aceita propriedades decimal
            entity.Property(i => i.CODCONTAEMAIL).HasDefaultValueSql("SELECT MAX(CODCONTAEMAIL) + 1  FROM PAGNET_CONTAEMAIL").HasColumnName("CODCONTAEMAIL");

            entity.Property(f => f.CODCONTAEMAIL).ValueGeneratedOnAdd();


            // Table & Column Mappings
            entity.ToTable("PAGNET_CONTAEMAIL");
            entity.Property(t => t.CODCONTAEMAIL).HasColumnName("CODCONTAEMAIL");
            entity.Property(t => t.NMCONTAEMAIL).HasColumnName("NMCONTAEMAIL");
            entity.Property(t => t.EMAIL).HasColumnName("EMAIL");
            entity.Property(t => t.SENHA).HasColumnName("SENHA");
            entity.Property(t => t.SERVIDOR).HasColumnName("SERVIDOR");
            entity.Property(t => t.ENDERECOSMTP).HasColumnName("ENDERECOSMTP");
            entity.Property(t => t.PORTA).HasColumnName("PORTA");
            entity.Property(t => t.CRIPTOGRAFIA).HasColumnName("CRIPTOGRAFIA");
            entity.Property(t => t.EMAILPRINCIPAL).HasColumnName("EMAILPRINCIPAL");
            entity.Property(t => t.ATIVO).HasColumnName("ATIVO");




    }
    }
}
