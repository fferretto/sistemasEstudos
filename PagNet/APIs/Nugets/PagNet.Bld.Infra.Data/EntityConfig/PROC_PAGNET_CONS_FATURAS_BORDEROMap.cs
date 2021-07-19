using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_CONS_FATURAS_BORDEROMap : IEntityTypeConfiguration<PROC_PAGNET_CONS_FATURAS_BORDERO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_CONS_FATURAS_BORDERO> entity)
        {
            entity.HasKey(pc => new { pc.CODCLIENTE, pc.DATVENCIMENTO });

            entity.Property(t => t.CODFATURA).HasColumnName("CODFATURA");
            entity.Property(t => t.CODCLIENTE).HasColumnName("CODCLIENTE");
            entity.Property(t => t.NMCLIENTE).HasColumnName("NMCLIENTE");
            entity.Property(t => t.CPFCNPJ).HasColumnName("CPFCNPJ");
            entity.Property(t => t.DATVENCIMENTO).HasColumnName("DATVENCIMENTO");
            entity.Property(t => t.QTFATURAMENTO).HasColumnName("QTFATURAMENTO");
            entity.Property(t => t.VALOR).HasColumnName("VALOR");

        }
    }
}