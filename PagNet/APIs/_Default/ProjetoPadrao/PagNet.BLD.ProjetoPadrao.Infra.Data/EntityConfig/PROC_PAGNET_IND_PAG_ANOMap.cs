using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_IND_PAG_ANOMap : IEntityTypeConfiguration<PROC_PAGNET_IND_PAG_ANO>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_IND_PAG_ANO> entity)
        {

            entity.HasKey(pc => new { pc.MESREF });

            entity.Property(t => t.MESREF).HasColumnName("MESREF");
            entity.Property(t => t.VLPAGO).HasColumnName("VLPAGO");
            entity.Property(t => t.VLTAXA).HasColumnName("VLTAXA");
        }
    }
}