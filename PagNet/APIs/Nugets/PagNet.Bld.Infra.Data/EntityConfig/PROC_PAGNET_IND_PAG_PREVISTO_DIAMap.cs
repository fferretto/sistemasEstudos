using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_IND_PAG_PREVISTO_DIAMap : IEntityTypeConfiguration<PROC_PAGNET_IND_PAG_PREVISTO_DIA>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_IND_PAG_PREVISTO_DIA> entity)
        {
            entity.HasKey(pc => new { pc.DATPGTO, pc.VLPAGAR });

            entity.Property(t => t.DATPGTO).HasColumnName("DATPGTO");
            entity.Property(t => t.VLPAGAR).HasColumnName("VLPAGAR");
            entity.Property(t => t.VALTAXA).HasColumnName("VALTAXA");

        }
        
    }
}
