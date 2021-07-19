using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;


namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_PAGNET_INFO_CONTA_CORRENTEMap : IEntityTypeConfiguration<PROC_PAGNET_INFO_CONTA_CORRENTE>
    {
        public void Configure(EntityTypeBuilder<PROC_PAGNET_INFO_CONTA_CORRENTE> entity)
        {
            entity.HasKey(pc => new { pc.SALDOCONTA });
        }
    }
}