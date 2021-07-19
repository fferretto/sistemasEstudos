using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    class PROC_PAGNET_IND_PAG_REALIZADO_DIAMap : IEntityTypeConfiguration<PROC_PAGNET_IND_PAG_REALIZADO_DIA>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_IND_PAG_REALIZADO_DIA> entity)
        {

            entity.HasKey(pc => new { pc.DATPGTO });

            entity.Property(t => t.DATPGTO).HasColumnName("DATPGTO");
            entity.Property(t => t.VLPAGAR).HasColumnName("VLPAGAR");
            entity.Property(t => t.VALTAXA).HasColumnName("VALTAXA");
        }
    }
}