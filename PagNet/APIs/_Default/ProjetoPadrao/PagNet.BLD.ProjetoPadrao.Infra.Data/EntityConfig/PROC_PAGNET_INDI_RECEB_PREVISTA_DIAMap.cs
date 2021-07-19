using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INDI_RECEB_PREVISTA_DIAMap : IEntityTypeConfiguration<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INDI_RECEB_PREVISTA_DIA> entity)
        {

            entity.HasKey(pc => new { pc.DATPGTO });

            entity.Property(t => t.DATPGTO).HasColumnName("DATPGTO");
            entity.Property(t => t.VLRECEBER).HasColumnName("VLRECEBER");
        }
    }
}