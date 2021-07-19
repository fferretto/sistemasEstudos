using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.Domain.Entities;

namespace PagNet.Infra.Data.EntityConfig
{
    public class PROC_CONFIRMAPGTOCARGAMap : IEntityTypeConfiguration<PROC_CONFIRMAPGTOCARGA>
    {
        public void Configure(EntityTypeBuilder<PROC_CONFIRMAPGTOCARGA> entity)
        {
            entity.HasKey(pc => new { pc.RETORNO });
            
        }
    }
}
