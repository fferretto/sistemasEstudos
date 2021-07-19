using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Bld.Domain.Entities;

namespace PagNet.Bld.Infra.Data.EntityConfig
{
    public class PROC_CONFIRMAPGTOCARGAMap : IEntityTypeConfiguration<PROC_CONFIRMAPGTOCARGA>
    {
        public void Configure(EntityTypeBuilder<PROC_CONFIRMAPGTOCARGA> entity)
        {
            entity.HasKey(pc => new { pc.RETORNO });
            
        }
    }
}
