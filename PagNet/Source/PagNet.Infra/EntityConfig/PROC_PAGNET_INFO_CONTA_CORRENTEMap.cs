using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;


namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INFO_CONTA_CORRENTEMap : IEntityTypeConfiguration<PROC_PAGNET_INFO_CONTA_CORRENTE>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INFO_CONTA_CORRENTE> entity)
        {
            entity.HasKey(pc => new { pc.SALDOCONTA });
        }
    }
}