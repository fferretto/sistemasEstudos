using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PAGNET_MENUMap : IEntityTypeConfiguration<PAGNET_MENU>
    {
        public void Configure(EntityTypeBuilder<PAGNET_MENU> entity)
        {
            // Primary Key
            entity.HasKey(t => t.codMenu);

            // Table & Column Mappings
            entity.ToTable("PAGNET_MENU");
            entity.Property(t => t.codMenu).HasColumnName("codMenu");
            entity.Property(t => t.Nome).HasColumnName("Nome");
            entity.Property(t => t.Descricao).HasColumnName("Descricao");
            entity.Property(t => t.codMenuPai).HasColumnName("codMenuPai");
            entity.Property(t => t.Area).HasColumnName("Area");
            entity.Property(t => t.Controller).HasColumnName("Controller");
            entity.Property(t => t.Action).HasColumnName("Action");
            entity.Property(t => t.Nivel).HasColumnName("Nivel");
            entity.Property(t => t.Ordem).HasColumnName("Ordem");
            entity.Property(t => t.Ativo).HasColumnName("Ativo");
            entity.Property(t => t.favIcon).HasColumnName("favIcon");

            
    }

    }
}
