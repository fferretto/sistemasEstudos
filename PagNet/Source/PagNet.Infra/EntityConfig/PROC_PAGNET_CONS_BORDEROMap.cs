using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_CONS_BORDEROMap : IEntityTypeConfiguration<PROC_PAGNET_CONS_BORDERO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_CONS_BORDERO> entity)
        {
            entity.HasKey(pc => new { pc.CODBORDERO });

            entity.Property(t => t.CODBORDERO).HasColumnName("CODBORDERO");
            entity.Property(t => t.VLBORDERO).HasColumnName("VLBORDERO");
            entity.Property(t => t.DTBORDERO).HasColumnName("DTBORDERO");
            entity.Property(t => t.CODEMPRESA).HasColumnName("CODEMPRESA");
            entity.Property(t => t.CODUSUARIO).HasColumnName("CODUSUARIO");
            entity.Property(t => t.STATUS).HasColumnName("STATUS");

        }
    }
}
