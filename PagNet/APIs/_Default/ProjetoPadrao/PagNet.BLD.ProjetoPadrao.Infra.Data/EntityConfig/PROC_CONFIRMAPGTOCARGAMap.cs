using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PagNet.BLD.ProjetoPadrao.Domain.Entities;

namespace PagNet.BLD.ProjetoPadrao.Infra.Data.EntityConfig
{
    public class PROC_CONFIRMAPGTOCARGAMap : IEntityTypeConfiguration<PROC_CONFIRMAPGTOCARGA>
    {
        public void Configure(EntityTypeBuilder<PROC_CONFIRMAPGTOCARGA> entity)
        {
            entity.HasKey(pc => new { pc.RETORNO });
            
        }
    }
}
